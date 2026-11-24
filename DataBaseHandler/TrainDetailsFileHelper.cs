using DataModel;
using DataBaseHandler;
using LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Helper
{
    /// <summary>
    /// Generates Traindetails.txt as per RDSO IPIS spec (RDSO/SPN/TC/108/2019 v2.0).
    ///
    ///  Header record (24 bytes):
    ///    SOH STX | dd/MM/yyyy | ' ' | HH:mm:ss | ETX CR LF
    ///
    ///  Train record (264 bytes):
    ///    SOH STX                              2
    ///    Train number                         5   TRAIN_NUMBER
    ///    Train name (UPPERCASE)              30   TRAIN_NAME_ENGLISH
    ///    Scheduled arrival   HHmm / ----      4   SCHEDULE_ARRIVAL_TIME
    ///    Scheduled departure HHmm / ----      4   SCHEDULE_DEPARTURE_TIME
    ///    A/D                                  1   ADFlag
    ///    Late by HHmm / ----                  4   LATE
    ///    Expected arrival   HHmm / ----       4   ACTUAL_ARRIVAL_TIME (else scheduled + LATE)
    ///    Expected departure HHmm / ----       4   ACTUAL_DEPARTURE_TIME (else scheduled + LATE)
    ///    PF number (2 chars)                  2   ACTUAL_PLATFORM_NUMBER (else SCHEDULE_PLATFORM_NUMBER)
    ///    Status                               1   STA_StatusCode (byte from TRAINSTATUS)
    ///    Coach marshaling (comma, $FA, pad) 200   COACH_SEQUENCE
    ///    ETX CR LF                            3
    /// </summary>
    public static class TrainDetailsFileHelper
    {
        private static readonly object _fileLock = new object();

        public static readonly string TrainDetailsFilePath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Docs", "Backwardcompatibility.txt");

        private const string COACH_END = "$FA";
        private const string NO_TIME = "----";
        private const string STATUS_NONE = " "; // NULL STA_StatusCode → single space (0x20)

        private const int TRAIN_NUMBER_LENGTH = 5;
        private const int TRAIN_NAME_LENGTH = 30;
        private const int PLATFORM_LENGTH = 2;
        private const int COACH_FIELD_LENGTH = 200;
        private const int TRAIN_RECORD_LENGTH = 264;

        private static readonly CultureInfo Inv = CultureInfo.InvariantCulture;

       
        public static Func<string, string, byte?> ResolveStaStatusCode { get; set; }

        // Per-write cache: maps STATUS_CODE / STATUS_ENGLISH -> STA_StatusCode byte
        [ThreadStatic]
        private static Dictionary<string, byte?> _staMap;

        public static int WriteTadbTrains(IEnumerable<train_updated_data> allTrains, bool append = false)
        {
            try
            {
                var trains = (allTrains ?? Enumerable.Empty<train_updated_data>()).Where(IsValidTrainForFile).ToList();

                // Build the status cache ONCE per file write (single DB round trip).
                _staMap = BuildStaStatusCodeMap();

                lock (_fileLock)
                {
                    string directory = Path.GetDirectoryName(TrainDetailsFilePath);
                    if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
                        Directory.CreateDirectory(directory);

                    var fileContent = new StringBuilder();
                    fileContent.Append(CreateDateTimeRecord(DateTime.Now));

                    foreach (var train in trains)
                        fileContent.Append(CreateTrainRecord(train));

                    if (append)
                        File.AppendAllText(TrainDetailsFilePath, fileContent.ToString(), Encoding.ASCII);
                    else
                        File.WriteAllText(TrainDetailsFilePath, fileContent.ToString(), Encoding.ASCII);
                }

                LoggerService.Logger.Information("Traindetails.txt generated. Valid trains: {Count}", trains.Count);
                return trains.Count;
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, "Failed to generate Traindetails.txt: " + ex.Message);
                return 0;
            }
            finally
            {
                _staMap = null;
            }
        }

        private static bool IsValidTrainForFile(train_updated_data train)
        {
            return train != null && (train.TADB_SEND_DATA == 1 || train.CGDB_SEND_DATA == 1);
        }

        private static string CreateDateTimeRecord(DateTime dateTime)
        {
            var record = new StringBuilder();
            record.Append("SOH");
            record.Append("STX");
            record.Append(dateTime.ToString("dd/MM/yyyy", Inv));
            record.Append(' ');
            record.Append(dateTime.ToString("HH:mm:ss", Inv));
            record.Append("ETX");
            record.Append("CR");
            record.Append("LF");
            return record.ToString();
        }

        private static string CreateTrainRecord(train_updated_data train)
        {
            var record = new StringBuilder();
            int lateMinutes = ParseLateMinutes(train.LATE) ?? 0;

            record.Append("SOH");
            record.Append("STX");
            record.Append(FixedField(train.TRAIN_NUMBER, TRAIN_NUMBER_LENGTH, false));
            record.Append(FixedField(train.TRAIN_NAME_ENGLISH, TRAIN_NAME_LENGTH, true));
            record.Append(FormatTime(ParseTime(train.SCHEDULE_ARRIVAL_TIME)));
            record.Append(FormatTime(ParseTime(train.SCHEDULE_DEPARTURE_TIME)));
            record.Append(GetArrivalDeparture(train));
            record.Append(DelayTime(lateMinutes));
            record.Append(GetExpectedTime(train.ACTUAL_ARRIVAL_TIME, train.SCHEDULE_ARRIVAL_TIME, lateMinutes));
            record.Append(GetExpectedTime(train.ACTUAL_DEPARTURE_TIME, train.SCHEDULE_DEPARTURE_TIME, lateMinutes));
            record.Append(GetPlatformNumber(train));
            record.Append(GetStatus(train));      // status: after platform, just before coaches
            record.Append(GetCoachMarshaling(train));
           

            if (record.Length != TRAIN_RECORD_LENGTH)
            {
                LoggerService.Logger.Warning("Train record length {Len} != {Expected} for train {Train}",
                    record.Length, TRAIN_RECORD_LENGTH, train.TRAIN_NUMBER);
            }

            return record.ToString();
        }

        // ---------------------------------------------------------------- times

        private static DateTime? ParseTime(object value)
        {
            if (value == null || value == DBNull.Value)
                return null;

            if (value is DateTime dt) return dt;
            if (value is TimeSpan ts) return DateTime.Today.Add(ts);

            string text = Convert.ToString(value, Inv)?.Trim();
            if (string.IsNullOrWhiteSpace(text)) return null;

            if (DateTime.TryParse(text, Inv, DateTimeStyles.None, out DateTime parsed))
                return parsed;

            text = text.Replace(":", "").Replace(".", "");
            if (text.Length == 4 && text.All(char.IsDigit))
            {
                int h = int.Parse(text.Substring(0, 2), Inv);
                int m = int.Parse(text.Substring(2, 2), Inv);
                if (h < 24 && m < 60) return DateTime.Today.AddHours(h).AddMinutes(m);
            }

            return null;
        }

        private static string FormatTime(DateTime? time)
        {
            return time.HasValue ? time.Value.ToString("HHmm", Inv) : NO_TIME;
        }

        private static string GetExpectedTime(string actual, string scheduled, int lateMinutes)
        {
            DateTime? time = ParseTime(actual);
            if (!time.HasValue)
            {
                DateTime? sched = ParseTime(scheduled);
                if (sched.HasValue) time = sched.Value.AddMinutes(lateMinutes);
            }
            return FormatTime(time);
        }


        private static string GetArrivalDeparture(train_updated_data train)
        {
            string value = (train.ADFlag ?? "").Trim().ToUpperInvariant();
            if (value.StartsWith("A")) return "A";
            if (value.StartsWith("D")) return "D";
            return " ";
        }

        private static string GetStatus(train_updated_data train)
        {
            try
            {
                string statusCode = train.TRAIN_STATUS_CODE?.Trim();
                string statusEnglish = train.TRAIN_STATUS_ENGLISH?.Trim();

                if (string.IsNullOrEmpty(statusCode) && string.IsNullOrEmpty(statusEnglish))
                    return STATUS_NONE;

                string sql = "SELECT TOP 1 STA_StatusCode FROM TRAINSTATUS " +
                             "WHERE STATUS = 1 AND (" +
                             $"STATUS_CODE = '{statusCode}' OR STATUS_ENGLISH = '{statusEnglish}')";

                string raw = _dbService.ExecuteScalar(sql)?.Trim();

                return string.IsNullOrEmpty(raw) ||
                       raw.Equals("NULL", StringComparison.OrdinalIgnoreCase)
                    ? STATUS_NONE
                    : raw;
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, "GetStatus failed: " + ex.Message);
                return STATUS_NONE;
            }
        }

        /// <summary>
        /// Builds a dictionary of STA_StatusCode keyed by both STATUS_CODE and STATUS_ENGLISH.
        /// </summary>
        private static Dictionary<string, byte?> BuildStaStatusCodeMap()
        {
            var map = new Dictionary<string, byte?>(StringComparer.OrdinalIgnoreCase);
            try
            {
                string sql = "SELECT STATUS_CODE, STATUS_ENGLISH, STA_StatusCode FROM TRAINSTATUS WHERE STATUS = 1";
                var dt = _dbService.GetTable(sql);
                if (dt == null) return map;

                foreach (DataRow row in dt.Rows)
                {
                    string code = row["STATUS_CODE"]?.ToString()?.Trim();
                    string eng = row["STATUS_ENGLISH"]?.ToString()?.Trim();
                    byte? sta = TryParseStatusCode(row["STA_StatusCode"]);

                    if (!string.IsNullOrWhiteSpace(code) && !map.ContainsKey(code)) map[code] = sta;
                    if (!string.IsNullOrWhiteSpace(eng) && !map.ContainsKey(eng)) map[eng] = sta;
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, "Failed to build STA_StatusCode map: " + ex.Message);
            }
            return map;
        }

      
        private static byte? TryParseStatusCode(object value)
        {
            if (value == null || value == DBNull.Value) return null;

            if (value is byte[] bytes) return bytes.Length > 0 ? bytes[0] : (byte?)null;
            if (value is byte b) return b;
            if (value is sbyte sb) return unchecked((byte)sb);
            if (value is short s) return unchecked((byte)s);
            if (value is int i) return unchecked((byte)i);
            if (value is long l) return unchecked((byte)l);

            string text = value.ToString().Trim();
            if (string.IsNullOrWhiteSpace(text)) return null;

            if (text.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                if (byte.TryParse(text.Substring(2), NumberStyles.HexNumber, Inv, out byte hex))
                    return hex;
                return null;
            }

            if (byte.TryParse(text, NumberStyles.Integer, Inv, out byte dec))
                return dec;

            return null;
        }

        // ----------------------------------------------------------------- late

        private static int? ParseLateMinutes(string late)
        {
            if (string.IsNullOrWhiteSpace(late)) return null;
            string text = late.Trim();

            if (text.Contains(":"))
                return TimeSpan.TryParse(text, Inv, out TimeSpan ts) ? (int?)ts.TotalMinutes : null;

            text = text.Replace(".", "");
            if (!text.All(char.IsDigit)) return null;

            if (text.Length == 4)
                return int.Parse(text.Substring(0, 2), Inv) * 60 + int.Parse(text.Substring(2, 2), Inv);

            return int.TryParse(text, NumberStyles.None, Inv, out int minutes) ? (int?)minutes : null;
        }

        private static string DelayTime(int totalMinutes)
        {
            if (totalMinutes <= 0) return NO_TIME;
            totalMinutes = Math.Min(totalMinutes, 99 * 60 + 59);
            return (totalMinutes / 60).ToString("D2", Inv) + (totalMinutes % 60).ToString("D2", Inv);
        }


        private static string GetPlatformNumber(train_updated_data train)
        {
            string platform = train.ACTUAL_PLATFORM_NUMBER?.Trim();
            if (string.IsNullOrEmpty(platform)) platform = train.SCHEDULE_PLATFORM_NUMBER?.Trim();
            if (string.IsNullOrEmpty(platform)) return "00";
            return platform.PadLeft(2, '0');
        }

        // --------------------------------------------------------------- coaches

        private static string GetCoachMarshaling(train_updated_data train)
        {
            string coaches = Convert.ToString(train.COACH_SEQUENCE)?.Replace("\r", "").Replace("\n", "").Trim() ?? "";
            coaches = ToAscii(coaches).ToUpperInvariant();

            if (coaches.EndsWith(COACH_END, StringComparison.OrdinalIgnoreCase))
                coaches = coaches.Substring(0, coaches.Length - COACH_END.Length);

            coaches = coaches.Replace('-', ',').Replace(';', ',');

            int maxCoachChars = COACH_FIELD_LENGTH - COACH_END.Length;
            if (coaches.Length > maxCoachChars)
            {
                coaches = coaches.Substring(0, maxCoachChars);
                int lastComma = coaches.LastIndexOf(',');
                if (lastComma >= 0) coaches = coaches.Substring(0, lastComma + 1);
            }

            coaches += COACH_END;
            return coaches.PadRight(COACH_FIELD_LENGTH, ' ');
        }

        // ---------------------------------------------------------------- utils

        private static string FixedField(object value, int length, bool upper)
        {
            string text = ToAscii(Convert.ToString(value)?.Trim() ?? "");
            if (upper) text = text.ToUpperInvariant();
            if (text.Length > length) text = text.Substring(0, length);
            return text.PadRight(length, ' ');
        }

        private static string ToAscii(string value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            var result = new StringBuilder(value.Length);
            foreach (char c in value) result.Append(c >= 32 && c <= 126 ? c : ' ');
            return result.ToString();
        }
    }
}