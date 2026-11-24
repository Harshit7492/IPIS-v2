
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace MemberDomain
{
    public sealed class TrainStatusProcessor
    {
        // =========================================================
        // STATUS CODES
        // =========================================================

        public const int STATUS_ARRIVAL_RIGHT_TIME = 0x01;
        public const int STATUS_WILL_ARRIVE_SHORTLY = 0x02;
        public const int STATUS_IS_ARRIVING = 0x03;
        public const int STATUS_HAS_ARRIVED = 0x04;
        public const int STATUS_RUNNING_LATE = 0x05;
        public const int STATUS_CANCELLED_ARRIVAL = 0x06;
        public const int STATUS_INDEFINITE_LATE = 0x07;
        public const int STATUS_TERMINATED_AT = 0x08;
        public const int STATUS_PLATFORM_CHANGED_ARR = 0x09;
        public const int STATUS_DEPARTURE_RIGHT_TIME = 0x0A;
        public const int STATUS_CANCELLED_DEPARTURE = 0x0B;
        public const int STATUS_READY_TO_LEAVE = 0x0C;
        public const int STATUS_ON_PLATFORM = 0x0D;
        public const int STATUS_DEPARTED = 0x0E;
        public const int STATUS_RESCHEDULED = 0x0F;
        public const int STATUS_DIVERTED = 0x10;
        public const int STATUS_DELAY_DEPARTURE = 0x11;
        public const int STATUS_PLATFORM_CHANGED_DEP = 0x12;
        public const int STATUS_CHANGE_OF_SOURCE = 0x13;

        // =========================================================
        // LANGUAGES
        // =========================================================

        public const string LANG_ENGLISH = "E";
        public const string LANG_HINDI = "H";
        public const string LANG_REGIONAL = "R";

        private static readonly HashSet<string> ValidLanguages =
            new(StringComparer.OrdinalIgnoreCase)
            {
                LANG_ENGLISH,
                LANG_HINDI,
                LANG_REGIONAL
            };

        private static readonly Dictionary<string, int> LanguageIndex =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["E"] = 0,
                ["H"] = 1,
                ["R"] = 2
            };

        // =========================================================
        // FLIP STATUSES
        // =========================================================

        private static readonly HashSet<int> FlipStatusCodes = new()
        {
            STATUS_TERMINATED_AT,
            STATUS_RESCHEDULED,
            STATUS_DIVERTED,
            STATUS_CHANGE_OF_SOURCE,
            STATUS_HAS_ARRIVED,
            STATUS_INDEFINITE_LATE
        };

        // =========================================================
        // SPECIAL TEXT STATUS
        // =========================================================

        private static readonly Dictionary<string, int> SpecialStatusText =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["arrived"] = STATUS_HAS_ARRIVED,
                ["Infinite late"] = STATUS_INDEFINITE_LATE,
                ["terminated"] = STATUS_TERMINATED_AT,
                ["terminated at"] = STATUS_TERMINATED_AT,

                ["cancelled"] = STATUS_CANCELLED_DEPARTURE,
                ["canceled"] = STATUS_CANCELLED_DEPARTURE,

                ["rescheduled"] = STATUS_RESCHEDULED,
                ["diverted"] = STATUS_DIVERTED,

                ["change of source"] = STATUS_CHANGE_OF_SOURCE,
                ["start at"] = STATUS_CHANGE_OF_SOURCE
            };

        // =========================================================
        // PUBLIC PROCESS
        // =========================================================

        public JsonObject Process(JsonObject input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            // Deep copy exactly like Python copy.deepcopy()
            JsonObject output =
                JsonNode.Parse(input.ToJsonString())!.AsObject();

            JsonArray? trains = input["TrainList"] as JsonArray;

            if (trains == null)
            {
                output["TrainList"] = new JsonArray();
                return output;
            }

            List<string> languages = GetLanguages(input);

            JsonArray processedTrains = new();

            foreach (JsonNode? node in trains)
            {
                if (node is not JsonObject train)
                    continue;

                processedTrains.Add(
                    ProcessTrain(train, languages)
                );
            }

            output["TrainList"] = processedTrains;

            return output;
        }

        // =========================================================
        // PROCESS TRAIN
        // =========================================================

        private JsonObject ProcessTrain(
            JsonObject train,
            List<string> languages)
        {
            JsonObject outputTrain =
                JsonNode.Parse(train.ToJsonString())!.AsObject();

            // Ensure CCD (Coach Sequence) is present on outputTrain
            string ccd = GetCoachDisplayFromTrain(train, languages.Count > 0 ? languages[0] : LANG_ENGLISH);
            if (!string.IsNullOrEmpty(ccd))
            {
                outputTrain["CCD"] = ccd;
            }

            string sta = GetString(train, "STA");

            string firstPart = sta
                .Split('#', 2)[0]
                .Trim()
                .ToLowerInvariant();

            // =====================================================
            // CANCELLED SPECIAL CASE
            // =====================================================

            if (firstPart == "cancelled" ||
                firstPart == "canceled")
            {
                outputTrain["SPECIAL_STATUS"] = 1;
                outputTrain["FLIP_REQ"] = 0;

                string[] parts = sta
                    .Split('#')
                    .Select(x => x.Trim())
                    .ToArray();

                JsonObject statusOutput = new();

                foreach (string language in languages)
                {
                    if (!LanguageIndex.TryGetValue(
                            language,
                            out int index))
                        continue;

                    string statusText =
                        index < parts.Length
                            ? parts[index]
                            : string.Empty;

                    statusOutput[language] =
                        new JsonArray(statusText);
                }

                outputTrain["STA"] = statusOutput;

                return outputTrain;
            }

            // =====================================================
            // STATUS CODE
            // =====================================================

            int? statusCode = GetStatusCode(sta);

            // =====================================================
            // NORMAL STATUS
            // =====================================================

            if (!statusCode.HasValue ||
                !FlipStatusCodes.Contains(statusCode.Value))
            {
                outputTrain["SPECIAL_STATUS"] = 0;
                outputTrain["FLIP_REQ"] = 0;

                // Keep original STA exactly as received.
                outputTrain["STA"] = train["STA"]?.DeepClone();

                return outputTrain;
            }

            // =====================================================
            // SPECIAL STATUS WITH FLIP
            // =====================================================

            outputTrain["SPECIAL_STATUS"] = 1;
            outputTrain["FLIP_REQ"] = 1;

            JsonObject specialSta = new();

            foreach (string language in languages)
            {
                (string text1, string text2) =
                    GetFlipTexts(
                        sta,
                        statusCode.Value,
                        language,
                        train);

                specialSta[language] =
                    new JsonArray(text1, text2);
            }

            outputTrain["STA"] = specialSta;

            return outputTrain;
        }

        // =========================================================
        // LANGUAGES
        // =========================================================

        public List<string> GetLanguages(JsonObject input)
        {
            JsonArray? lineConf =
                input["LineConf"] as JsonArray;

            if (lineConf == null ||
                lineConf.Count == 0 ||
                lineConf[0] is not JsonObject config)
            {
                return new List<string> { LANG_ENGLISH };
            }

            string lan = GetString(config, "LAN");

            if (string.IsNullOrWhiteSpace(lan))
                return new List<string> { LANG_ENGLISH };

            List<string> languages = new();

            foreach (string value in lan.ToUpperInvariant().Split('#'))
            {
                string code = value.Trim();

                if (string.IsNullOrEmpty(code))
                    continue;

                if (!ValidLanguages.Contains(code))
                    continue;

                if (!languages.Contains(code))
                    languages.Add(code);
            }

            if (languages.Count == 0)
                languages.Add(LANG_ENGLISH);

            return languages;
        }

        // =========================================================
        // STATUS CODE
        // =========================================================

        private int? GetStatusCode(string sta)
        {
            if (string.IsNullOrWhiteSpace(sta))
                return null;

            string value = sta.Trim();

            // 0x01 ... 0xFF
            if (Regex.IsMatch(
                    value,
                    @"^0[xX][0-9a-fA-F]{1,2}$"))
            {
                try
                {
                    return Convert.ToInt32(
                        value.Substring(2),
                        16);
                }
                catch
                {
                    return null;
                }
            }

            string firstPart = value
                .Split('#', 2)[0]
                .Trim()
                .ToLowerInvariant();

            return SpecialStatusText.TryGetValue(
                firstPart,
                out int code)
                ? code
                : null;
        }

        // =========================================================
        // FLIP TEXT
        // =========================================================

        private (string Text1, string Text2) GetFlipTexts(
            string sta,
            int statusCode,
            string language,
            JsonObject train)
        {
            switch (statusCode)
            {
                case STATUS_TERMINATED_AT:
                    return ParseTerminated(
                        sta,
                        language);

                case STATUS_RESCHEDULED:
                    return ParseRescheduled(
                        sta,
                        language,
                        train);

                case STATUS_DIVERTED:
                    return ParseDiverted(
                        sta,
                        language);

                case STATUS_CHANGE_OF_SOURCE:
                    return ParseChangeOfSource(
                        sta,
                        language);
                case STATUS_HAS_ARRIVED:
                    return ParseHasArrivedOn(sta, language, train);

                case STATUS_INDEFINITE_LATE:
                    return ParseIndefiniteLate(sta, language);

                default:
                    return (sta, string.Empty);
            }
        }


        private (string, string) ParseIndefiniteLate(string sta, string language)
        {
            var (statusParts, stationParts) =
                ParseMultilingualSta(sta);

            string status =
                GetLanguagePart(statusParts, language);

            string station =
                GetLanguagePart(stationParts, language);

            if (language == LANG_ENGLISH)
            {
                return (
                    "Infinite Late",
                    station
                );
            }

            if (language == LANG_HINDI)
            {
                return (
                    station,
                    status
                );
            }

            if (language == LANG_REGIONAL)
            {
                return (
                    station,
                    status
                );
            }

            return (
                status,
                station
            );
        }

        // =========================================================
        // HAS ARRIVED ON
        // =========================================================

        private (string, string) ParseHasArrivedOn(string sta, string language, JsonObject train)
        {
            var (statusParts, valueParts) =
                            ParseMultilingualSta(sta);

            string status =
                GetLanguagePart(statusParts, language);

            string value =
                GetLanguagePart(valueParts, language);

            if (language == LANG_ENGLISH)
            {
                status = "ARRIVED";
            }

            return (
                status,
                value
            );
        }


        // =========================================================
        // TERMINATED
        // =========================================================

        private (string, string) ParseTerminated(string sta, string language)
        {
            var (statusParts, stationParts) =
                ParseMultilingualSta(sta);

            string status =
                GetLanguagePart(statusParts, language);

            string station =
                GetLanguagePart(stationParts, language);

            if (language == LANG_ENGLISH)
            {
                return (
                    "TERMINATED AT",
                    station
                );
            }

            if (language == LANG_HINDI)
            {
                return (
                    station,
                    status
                );
            }

            if (language == LANG_REGIONAL)
            {
                return (
                    station,
                    status
                );
            }

            return (
                status,
                station
            );
        }

        // =========================================================
        // RESCHEDULED
        // =========================================================

        private (string, string) ParseRescheduled(string sta, string language, JsonObject train)
        {
            var (statusParts, valueParts) =
                ParseMultilingualSta(sta);

            string status =
                GetLanguagePart(statusParts, language);

            string value =
                GetLanguagePart(valueParts, language);

            if (language == LANG_ENGLISH)
            {
                status = "RESCHEDULED";
            }

            return (
                status,
                value
            );
        }

        // =========================================================
        // DIVERTED
        // =========================================================

        private (string, string) ParseDiverted(string sta, string language)
        {
            var (statusParts, routeParts) =
                ParseMultilingualSta(sta);

            string status =
                GetLanguagePart(statusParts, language);

            string route =
                GetLanguagePart(routeParts, language);

            if (language == LANG_ENGLISH &&
                string.IsNullOrEmpty(status))
            {
                status = "DIVERTED";
            }

            return (
                status,
                route
            );
        }

        // =========================================================
        // CHANGE OF SOURCE
        // =========================================================

        private (string, string) ParseChangeOfSource(string sta, string language)
        {
            var (statusParts, stationParts) =
                ParseMultilingualSta(sta);

            string status =
                GetLanguagePart(statusParts, language);

            string station =
                GetLanguagePart(stationParts, language);

            if (language == LANG_ENGLISH)
            {
                return (
                    "START AT",
                    station
                );
            }

            return (
                status,
                station
            );
        }

        // =========================================================
        // MULTILINGUAL STA
        // =========================================================

        private static (
            List<string> Left,
            List<string> Right)
            ParseMultilingualSta(string sta)
        {
            if (string.IsNullOrWhiteSpace(sta))
            {
                return (
                    new List<string>(),
                    new List<string>()
                );
            }

            if (!sta.Contains('-'))
            {
                return (
                    sta.Split('#')
                       .Select(x => x.Trim())
                       .ToList(),

                    new List<string>()
                );
            }

            string[] split = sta.Split(
                new[] { '-' },
                2);

            List<string> left =
                split[0]
                    .Split('#')
                    .Select(x => x.Trim())
                    .ToList();

            List<string> right =
                split.Length > 1
                    ? split[1]
                        .Split('#')
                        .Select(x => x.Trim())
                        .ToList()
                    : new List<string>();

            return (
                left,
                right
            );
        }

        // =========================================================
        // LANGUAGE PART
        // =========================================================

        private static string GetLanguagePart(
            List<string> values,
            string language)
        {
            if (!LanguageIndex.TryGetValue(
                    language,
                    out int index))
            {
                return string.Empty;
            }

            if (index >= values.Count)
                return string.Empty;

            return Clean(values[index]);
        }


        private static string GetString(
            JsonObject obj,
            string property)
        {
            JsonNode? node = obj[property];

            if (node == null)
                return string.Empty;

            try
            {
                return node.GetValue<string>()?.Trim()
                       ?? string.Empty;
            }
            catch
            {
                return node.ToString().Trim();
            }
        }

        private static string Clean(object? value)
        {
            return value?.ToString()?.Trim()
                   ?? string.Empty;
        }



        private string GetCoachDisplayFromTrain(JsonObject train, string language)
        {
           

            // Try to get raw coach sequence
            if (train.ContainsKey("CCD"))
            {
                return train["CCD"]?.GetValue<string>() ?? string.Empty;
            }

            if (train.ContainsKey("Coach_Sequence"))
            {
                return train["Coach_Sequence"]?.GetValue<string>() ?? string.Empty;
            }

            return string.Empty;
        }
    }

}
