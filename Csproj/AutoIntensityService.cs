using DataBaseHandler;
using DataModel;
using Global_Data;
using LogManager;
using System.Globalization;

namespace IPIS
{
    /// <summary>
    /// Day / night automatic intensity, in one place.
    ///
    /// Evaluate() is pure: given the admin setting and a clock reading it says
    /// whether auto mode is on, whether it is currently "day", and which
    /// intensity applies. Apply() evaluates against the live admin setting and,
    /// when auto mode is on, writes that intensity to every board in
    /// BOARDSETTING (only rows whose value actually differs, so it is cheap to
    /// call every minute).
    ///
    /// Dashboard calls Apply() on a timer so the switch happens at the
    /// configured times whether or not any settings form is open. The board
    /// setting forms call it on load so the combo shows the value in force.
    /// </summary>
    public static class AutoIntensityService
    {
        public sealed class Result
        {
            public bool Enabled;
            public bool IsDay;
            public string Intensity;          // "25" | "50" | "75" | "100", null if not applicable
            public TimeSpan DayStart;
            public TimeSpan NightStart;
            public int RowsUpdated;           // -1 = not attempted / failed
            public string Reason;             // why nothing was applied, for the log
        }

        private static readonly TimeSpan DefaultDayStart = new TimeSpan(6, 0, 0);
        private static readonly TimeSpan DefaultNightStart = new TimeSpan(20, 30, 0);
        private static readonly object _gate = new object();

        /// <summary>Decide day/night and the intensity for <paramref name="now"/>. No I/O.</summary>
        public static Result Evaluate(AdminSetting s, DateTime now)
        {
            var r = new Result { RowsUpdated = -1, DayStart = DefaultDayStart, NightStart = DefaultNightStart };

            if (s == null) { r.Reason = "admin setting not loaded"; return r; }
            if (s.AUTO_INTENSITY != 1) { r.Reason = "auto intensity off"; return r; }
            r.Enabled = true;

            if (TryParseClock(s.SET_Day_Time, out var day)) r.DayStart = day;
            if (TryParseClock(s.SET_Night_Time, out var night)) r.NightStart = night;

            // Operators type the night switch as "8:30" meaning 20:30 often enough
            // that a night start before noon is treated as PM.
            if (r.NightStart.Hours > 0 && r.NightStart.Hours < 12)
                r.NightStart = r.NightStart.Add(TimeSpan.FromHours(12));

            var t = now.TimeOfDay;
            r.IsDay = r.DayStart < r.NightStart
                ? (t >= r.DayStart && t < r.NightStart)
                : (t >= r.DayStart || t < r.NightStart);

            string raw = r.IsDay ? s.SET_Day_Intensity : s.SET_Night_Intensity;
            if (!TryNormalizeIntensity(raw, out r.Intensity))
            {
                r.Reason = $"{(r.IsDay ? "day" : "night")} intensity '{raw}' is not a number";
                r.Intensity = null;
            }
            return r;
        }

        /// <summary>
        /// Read the live admin setting, evaluate, and push the intensity to
        /// BOARDSETTING when auto mode is on. Safe to call from a timer.
        /// </summary>
        public static Result Apply()
        {
            lock (_gate)
            {
                try
                {
                    var setting = _dbService.GET_ADMIN_SETTING();
                    if (setting != null)
                        GlobalData._lstAdminSetting = setting;
                    else
                        setting = GlobalData._lstAdminSetting;

                    var r = Evaluate(setting, DateTime.Now);
                    if (!r.Enabled || r.Intensity == null)
                    {
                        if (r.Enabled)
                            LoggerService.Logger.Warning("Auto intensity: nothing applied ({Reason}).", r.Reason);
                        return r;
                    }

                    // Intensity is a validated integer, so inlining it is safe.
                    // The WHERE keeps the write idempotent: no row is touched
                    // when every board already carries this value.
                    string sql =
                        $"UPDATE BOARDSETTING SET Intensity = '{r.Intensity}' " +
                        $"WHERE ISNULL(CAST(Intensity AS NVARCHAR(10)), '') <> '{r.Intensity}';";

                    r.RowsUpdated = _dbService.ExecuteNonQuery(sql);

                    if (r.RowsUpdated < 0)
                        LoggerService.Logger.Warning("Auto intensity: BOARDSETTING update failed.");
                    else if (r.RowsUpdated > 0)
                        LoggerService.Logger.Information(
                            "Auto intensity: {Mode} -> {Intensity} applied to {Rows} board(s) (day {Day}, night {Night}).",
                            r.IsDay ? "DAY" : "NIGHT", r.Intensity, r.RowsUpdated,
                            r.DayStart.ToString(@"hh\:mm"), r.NightStart.ToString(@"hh\:mm"));

                    return r;
                }
                catch (Exception ex)
                {
                    LoggerService.Logger.Error(ex, "Auto intensity: failed to apply.");
                    return new Result { RowsUpdated = -1, Reason = ex.Message };
                }
            }
        }

        // ------------------------------------------------------------------

        /// <summary>"06:00", "6:00", "20:30", "8:30 PM", "0830" from the masked box.</summary>
        private static bool TryParseClock(string text, out TimeSpan value)
        {
            value = default;
            if (string.IsNullOrWhiteSpace(text)) return false;
            text = text.Trim();

            if (TimeSpan.TryParse(text, CultureInfo.InvariantCulture, out value) && value < TimeSpan.FromDays(1))
                return true;

            if (DateTime.TryParseExact(text, new[] { "h:mm tt", "hh:mm tt", "h:mmtt", "HHmm", "Hmm" },
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
            {
                value = dt.TimeOfDay;
                return true;
            }
            return false;
        }

        /// <summary>"50", " 50 ", "50%" → "50". Anything non-numeric fails.</summary>
        private static bool TryNormalizeIntensity(string raw, out string normalized)
        {
            normalized = null;
            if (string.IsNullOrWhiteSpace(raw)) return false;
            var digits = raw.Trim().TrimEnd('%').Trim();
            if (!int.TryParse(digits, NumberStyles.Integer, CultureInfo.InvariantCulture, out int n)) return false;
            if (n < 0 || n > 100) return false;
            normalized = n.ToString(CultureInfo.InvariantCulture);
            return true;
        }
    }
}
