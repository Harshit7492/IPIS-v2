using DataBaseHandler;
using DataModel;
using IPISv2.HealthStatus;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPISv2.HealthStatusReport
{

    public partial class GenericDisplayBoard : UserControl
    {
        // =========================================================
        // =========================================================
        // BOARD GEOMETRY
        // =========================================================

        private const int MatrixRows = 64;
        private const int MatrixCols = 384;
        private const int ModuleRows = 32;            // one LED matrix = 384 x 32
        private const int TrainLineMaxFontPx = 24;    // train line font in the 1st matrix (shrinks to fit)
        private const int TrainLineGapPx = 12;        // gap between train name and time / A-D / PF
        private const int MaxCoachesInTwoLines = 26;  // more coaches -> 3 coach lines in the 2nd matrix
        private const int CoachMaxFontPx = 14;
        private const int MinFontPx = 7;

        
        private const string LANG_ENGLISH = "E";
        private const string LANG_HINDI = "H";
        private const string LANG_REGIONAL = "R";

        private static readonly string[] LanguageDisplayOrder = { LANG_ENGLISH, LANG_HINDI, LANG_REGIONAL };

        private const string EnglishFontFamily = "Arial";
        private const string IndicFontFamily = "Nirmala UI"; // Arial has no Devanagari / Indic glyphs

        // =========================================================
        // STATE
        // =========================================================

        private List<AGDBDisplayBoardData> _allBoardData = new List<AGDBDisplayBoardData>();
        private List<AGDBDisplayBoardData> _rawActiveTrains = new List<AGDBDisplayBoardData>();
        private System.Windows.Forms.Timer _refreshTimer;
        private bool _isInitialized = false;
        private bool _isProcessingLiveData = false;
        private JsonObject? _lastProcessedLiveData = null;

        // Languages configured per board (key = board IP) and the languages sent in the live data LineConf
        private Dictionary<string, List<string>> _boardLanguages = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        private List<string> _lineConfLanguages = new List<string>();

        // Hindi coach names from the Coach Legend (LEGEND "B1" -> LEGEND_HINDI "बी-1")
        private Dictionary<string, string> _coachHindiNames = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        private readonly MemberDomain.TrainStatusProcessor _statusProcessor = new MemberDomain.TrainStatusProcessor();
        private readonly List<BoardRowControls> _dynamicRows = new List<BoardRowControls>();

        private class BoardRowControls
        {
            public int BoardId { get; set; }
            public string BoardIp { get; set; }
            public string SelectedPf { get; set; }
            public List<AGDBDisplayBoardData> Trains { get; set; }

            // Display step: every step shows one train in one language (train changes after all languages)
            public int CurrentTrainIndex { get; set; }

            public System.Windows.Forms.Timer TrainTimer { get; set; }
            public MaterialSkin.Controls.MaterialLabel TitleLabel { get; set; }
            public Panel ScrollPanel { get; set; }
            public AGDBBoardPreview BoardPreview { get; set; }
        }

        /// <summary>
        /// What is drawn on the board for one step.
        /// </summary>
        private sealed class AgdbFrame
        {
            public string TrainLine { get; set; } = string.Empty;    // 1st matrix, left:  train no + name
            public string DetailLine { get; set; } = string.Empty;   // 1st matrix, right: time A/D PF or status
            public List<string> Coaches { get; set; } = new List<string>(); // 2nd matrix
        }

        public GenericDisplayBoard()
        {
            InitializeComponent();
            if (IsInDesignMode()) return;
            this.Dock = DockStyle.Top;
            lblTitle.Visible = false;
            panelPreviewScroll.Visible = false;
            this.Load += GenericDisplayBoard_Load;
        }

        private void GenericDisplayBoard_Load(object sender, EventArgs e)
        {
            if (_isInitialized) return;

            LoadActiveBoardData();
            _refreshTimer = new System.Windows.Forms.Timer();
            int activeDelay = _allBoardData.FirstOrDefault()?.DelayTime ?? 10;
            if (activeDelay <= 0) activeDelay = 10;
            _refreshTimer.Interval = activeDelay * 1000;
            _refreshTimer.Tick += RefreshTimer_Tick;
            _refreshTimer.Start();

            this.Disposed += (s, ev) =>
            {
                _refreshTimer?.Stop();
                _refreshTimer?.Dispose();
                foreach (var row in _dynamicRows)
                {
                    if (row.TrainTimer != null)
                    {
                        row.TrainTimer.Stop();
                        row.TrainTimer.Dispose();
                    }
                }
            };

            _isInitialized = true;
        }

        private async void RefreshTimer_Tick(object sender, EventArgs e)
        {
            LoadActiveBoardData();
            await ProcessAndRenderLiveDataAsync();
        }

        private bool IsHostReachable(string ipAddress, int timeoutMs = 500)
        {
            if (string.IsNullOrWhiteSpace(ipAddress)) return false;

            try
            {
                using (Ping ping = new Ping())
                {
                    PingReply reply = ping.Send(ipAddress.Trim(), timeoutMs);
                    return reply != null && reply.Status == IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
        }

        private string ResolveSelectedPf(AGDBDisplayBoardData board)
        {
            if (board == null) return string.Empty;

            if (!string.IsNullOrWhiteSpace(board.SelectedPf))
                return board.SelectedPf.Trim();

            if (!string.IsNullOrWhiteSpace(board.PlatformNumber))
                return board.PlatformNumber.Trim();

            return string.Empty;
        }

        /// <summary>
        /// Checks whether a train's platform is contained within a board's configured SELECTED_PF value —
        /// handles both a plain value like "2" and a compound value like "2+2,2+3,2+4,2+5".
        /// </summary>
        private static bool PlatformMatches(string selectedPf, string trainPf)
        {
            if (string.IsNullOrWhiteSpace(selectedPf)) return false;
            if (IsAllPlatforms(selectedPf)) return true;
            if (string.IsNullOrWhiteSpace(trainPf)) return false;
            //if (string.IsNullOrWhiteSpace(selectedPf) || string.IsNullOrWhiteSpace(trainPf)) return false;

            var selectedPlatforms = selectedPf
                .Split(new[] { ',', '+' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim())
                .Where(p => p.Length > 0);

            //return selectedPlatforms.Any(p => string.Equals(p, trainPf.Trim(), StringComparison.OrdinalIgnoreCase));

            // The live feed sends "02" while the board setting has "2" -> compare without leading zeros.
            return selectedPlatforms.Any(p => string.Equals(NormalizePlatform(p), NormalizePlatform(trainPf), StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>"02" -> "2", " 2 " -> "2", "00" -> "0". Non-numeric values are only trimmed.</summary>
        private static string NormalizePlatform(string pf)
        {
            string value = (pf ?? string.Empty).Trim();
            if (value.Length == 0) return value;

            string withoutZeros = value.TrimStart('0');
            return withoutZeros.Length == 0 ? "0" : withoutZeros;
        }

        /// <summary>Platform 0 in the board setting = trains of every platform.</summary>
        private static bool IsAllPlatforms(string selectedPf)
        {
            if (string.IsNullOrWhiteSpace(selectedPf)) return false;

            return selectedPf
                .Split(new[] { ',', '+' }, StringSplitOptions.RemoveEmptyEntries)
                .Any(p => NormalizePlatform(p) == "0");
        }

        private string GetRowBoardIp(BoardRowControls row)
        {
            return row.BoardIp
                ?? (row.Trains != null && row.Trains.Count > 0 ? row.Trains[0].BoardIp : null);
        }

        // =========================================================
        // LIVE DATA
        // =========================================================

        public async Task ProcessAndRenderLiveDataAsync()
        {
            if (_isProcessingLiveData) return;
            _isProcessingLiveData = true;

            try
            {
                var devices = new List<DeviceInfo>();
                var displays = new List<DeviceInfo>();

                if (_allBoardData != null && _allBoardData.Count > 0)
                {
                    // One device entry per physical board (grouped by IP).
                    var distinctBoards = _allBoardData
                        .Where(b => b != null && !string.IsNullOrWhiteSpace(b.BoardIp))
                        .GroupBy(b => b.BoardIp.Trim())
                        .ToList();

                    foreach (var boardGroup in distinctBoards)
                    {
                        string ip = boardGroup.Key;

                        // Ping the board IP first — skip unreachable boards.
                        if (!IsHostReachable(ip))
                        {
                            LogManager.LoggerService.Logger.Debug(
                                $"AGDB board {ip} not reachable — skipping in payload.");
                            continue;
                        }

                        // Use the SELECTED_PF configured for THIS board IP.
                        string pf = ResolveSelectedPf(boardGroup.First());

                        devices.Add(new DeviceInfo
                        {
                            Vendor = "VARDAAN",
                            Device = "AGDB",
                            IP = ip,
                            PF = pf,
                            INT = "50",
                            HSR = "ALL OK"
                        });

                        displays.Add(new DeviceInfo
                        {
                            Device = "AGDB",
                            IP = ip,
                            INT = "50",
                            HSR = "ALL OK"
                        });
                    }
                }

                if (devices.Count == 0)
                {
                    LogManager.LoggerService.Logger.Warning(
                        "No reachable AGDB boards — skipping live-data request.");
                    return;
                }

                var request = new LiveDataRequest
                {
                    Device = devices,
                    DisplayList = displays
                };
                string rawJson = JsonSerializer.Serialize(request);

                var liveData = await _dbService.GetLiveDataDisplayAsyncForAGDB(request, rawJson);

                JsonObject? jsonObject = JsonSerializer.SerializeToNode(liveData) as JsonObject;
                if (jsonObject == null) return;

                _lineConfLanguages = GetLineConfLanguages(jsonObject["LineConf"] as JsonArray);

                // Make TrainStatusProcessor build the status texts for every configured language (E / H / R).
                PrepareLanguagesForProcessing(jsonObject);

                JsonObject processedOutput = _statusProcessor.Process(jsonObject);
                RenderLiveFramesToAGDB(processedOutput);
            }
            catch (Exception ex)
            {
                LogManager.LoggerService.Logger.Error(ex, "Error processing AGDB live data.");
            }
            finally
            {
                _isProcessingLiveData = false;
            }
        }

        private void RenderLiveFramesToAGDB(JsonObject data)
        {
            _lastProcessedLiveData = data;
            if (this.InvokeRequired)
            {
                try { this.BeginInvoke(new Action(() => RenderLiveFramesToAGDB(data))); } catch { }
                return;
            }

            JsonArray? trainsArray = data["TrainList"] as JsonArray;
            //if (trainsArray == null || _dynamicRows.Count == 0) return;
            if (_dynamicRows.Count == 0) return;
            if (trainsArray == null)
            {
                // No live list -> keep showing the trains added to the board (DB data).
                foreach (var dataRow in _dynamicRows) RenderBoardDataFrame(dataRow);
                return;
            }

            List<JsonObject> allLiveTrains = trainsArray
                .OfType<JsonObject>()
                .Where(t => !string.IsNullOrEmpty(DisplayHelper.GetString(t, "TNO")))
                .ToList();

            if (allLiveTrains.Count == 0)
                allLiveTrains = trainsArray.OfType<JsonObject>().ToList();

            //if (allLiveTrains.Count == 0) return;
            if (allLiveTrains.Count == 0)
            {
                foreach (var dataRow in _dynamicRows) RenderBoardDataFrame(dataRow);
                return;
            }

            foreach (var row in _dynamicRows)
            {
                if (row == null || row.BoardPreview == null || row.BoardPreview.IsDisposed) continue;

                string boardIp = GetRowBoardIp(row);

                // Only show / update the preview if the board's IP responds to a ping first.
                if (!IsHostReachable(boardIp))
                {
                    ClearBoard(row);
                    continue;
                }

                // Only the trains of this board's configured platform (SELECTED_PF)
                List<JsonObject> matchingTrains = !string.IsNullOrWhiteSpace(row.SelectedPf)
                    ? allLiveTrains.Where(t => PlatformMatches(row.SelectedPf, DisplayHelper.GetString(t, "PNO"))).ToList()
                    : allLiveTrains;

                //if (matchingTrains.Count == 0)
                //{
                //    RenderAgdbFrame(row, new AgdbFrame { TrainLine = "No Live Data" });
                //    continue;
                //}
                // No live train for this board -> show the trains added to the board (DB data) instead of
                // "No Live Data". RenderBoardDataFrame shows "No Data" only when the board really has no train.
                if (matchingTrains.Count == 0)
                {
                    RenderBoardDataFrame(row);
                    continue;
                }

                List<string> languages = GetBoardLanguages(boardIp);
                (int trainIndex, string language) = GetDisplayStep(row, matchingTrains.Count, languages);

                //RenderAgdbFrame(row, BuildLiveFrame(matchingTrains[trainIndex], language));
                int liveLanguageIndex = languages != null ? languages.IndexOf(language) : -1;
                RenderAgdbFrame(row, BuildLiveFrame(matchingTrains[trainIndex], language, liveLanguageIndex));
            }
        }

        
        /// <summary>
        /// Database ACTUAL_* values are authoritative; the live feed's Eat/Edt fall back
        /// to the scheduled time when the API omits ETA/EDT.
        /// </summary>
        private string ResolveActualTime(string trainNo, bool arrival, string feedValue)
        {
            //var row = _allBoardData?.FirstOrDefault(x =>
            //    string.Equals((x.TrainNumber ?? string.Empty).Trim(),
            //                  (trainNo ?? string.Empty).Trim(),
            //                  StringComparison.OrdinalIgnoreCase));
            //
            //string dbValue = row == null
            //    ? null
            //    : (arrival ? row.ActualArrivalTime : row.ActualDepartureTime);
            //
            //return !string.IsNullOrWhiteSpace(dbValue) ? dbValue : feedValue;

            // Every active train row from the DB first (same as SLDB / MLDB), then the board rows.
            string key = (trainNo ?? string.Empty).Trim();

            string dbValue = FindActualTime(_rawActiveTrains, key, arrival);
            if (string.IsNullOrWhiteSpace(dbValue))
                dbValue = FindActualTime(_allBoardData, key, arrival);

            return !string.IsNullOrWhiteSpace(dbValue) ? dbValue : feedValue;
        }

        private static string FindActualTime(List<AGDBDisplayBoardData> rows, string trainNo, bool arrival)
        {
            if (rows == null || string.IsNullOrEmpty(trainNo)) return null;

            return rows
                .Where(x => x != null && string.Equals((x.TrainNumber ?? string.Empty).Trim(), trainNo, StringComparison.OrdinalIgnoreCase))
                .Select(x => arrival ? x.ActualArrivalTime : x.ActualDepartureTime)
                .FirstOrDefault(v => !string.IsNullOrWhiteSpace(v));
        }

        /// <summary>Coach sequence of a train from the DB, used when the live feed sends no CCD.</summary>
        private string FindDbCoachSequence(string trainNo)
        {
            string key = (trainNo ?? string.Empty).Trim();
            if (key.Length == 0) return string.Empty;

            IEnumerable<AGDBDisplayBoardData> rows = (_rawActiveTrains ?? new List<AGDBDisplayBoardData>())
                .Concat(_allBoardData ?? new List<AGDBDisplayBoardData>());

            return rows
                .Where(x => x != null && string.Equals((x.TrainNumber ?? string.Empty).Trim(), key, StringComparison.OrdinalIgnoreCase))
                .Select(x => x.Coach_Sequence)
                .FirstOrDefault(v => !string.IsNullOrWhiteSpace(v)) ?? string.Empty;
        }

        /// <summary>Position of a language in E / H / R order (used when the board's list is not known).</summary>
        private static int GetLanguageIndex(string language)
        {
            int index = Array.IndexOf(LanguageDisplayOrder, language);
            return index < 0 ? 0 : index;
        }

        //private AgdbFrame BuildLiveFrame(JsonObject train, string language)
        private AgdbFrame BuildLiveFrame(JsonObject train, string language, int languageIndex = -1)
        {
            TrainDisplayData english = TrainDisplayHelper.BuildTrainData(train, LANG_ENGLISH, 0);

            string trainNo = SanitizeText(english.TrainNo);
            string trainName = SanitizeText(GetTrainNameForLanguage(english, language));

            if (languageIndex < 0) languageIndex = GetLanguageIndex(language);

            // Coach sequence from the live feed (CCD); from the DB when the feed sends none.
            string coachSequence = DisplayHelper.GetString(train, "CCD");
            if (string.IsNullOrWhiteSpace(coachSequence))
                coachSequence = FindDbCoachSequence(english.TrainNo);

            return new AgdbFrame
            {
                TrainLine = JoinParts(trainNo, trainName),
                //DetailLine = BuildLiveDetailLine(train, english, language),
                //Coaches = GetCoachesForLanguage(ParseCoaches(DisplayHelper.GetString(train, "CCD")), language)
                DetailLine = BuildLiveDetailLine(train, english, language, languageIndex),
                Coaches = GetCoachesForLanguage(ParseCoaches(coachSequence), language)
            };
        }

        /// <summary>
        /// Second row of the 1st matrix: "time  A/D  PF" for normal trains,
        /// or the special status (cancelled / rescheduled / terminated / diverted ...) in the current language.
        /// </summary>
        //private string BuildLiveDetailLine(JsonObject train, TrainDisplayData english, string language)
        private string BuildLiveDetailLine(JsonObject train, TrainDisplayData english, string language, int languageIndex = -1)
        {
            string adFlag = GetArrivalDepartureFlag(DisplayHelper.GetString(train, "ADF"), english);
            string time = GetDisplayTime(adFlag, english.Eat, english.Edt);
            // Database ACTUAL_* wins; english.Eat/Edt degrade to the scheduled time
            // whenever the live API omits ETA/EDT.
            string arrivalTime = DisplayTextFormatter.FormatTimeOnly(ResolveActualTime(english.TrainNo, true, english.Eat));
            string departureTime = DisplayTextFormatter.FormatTimeOnly(ResolveActualTime(english.TrainNo, false, english.Edt));
            string platform = SanitizeText(english.Platform);

            // ===== RDSO status handling (0x01 - 0x13), same rules as SLDB / MLDB =====
            // AGDB shows one detail line per train/language step (no half-slot flip), so a two-part status
            // shows "status + station" together. No A/D anywhere.
            if (languageIndex < 0) languageIndex = GetLanguageIndex(language);

            string englishStatus = DisplayTextFormatter.SelectLanguagePart(english.Status, 0);
            string englishFlipText = SanitizeText(DisplayHelper.GetTrainStatus(train, LANG_ENGLISH, 1));

            // Status text of this language: per-language STA object, else this language's part of a '#'-packed STA.
            bool staIsPerLanguage = train?["STA"] is JsonObject;
            string languageStatus0 = language != LANG_ENGLISH && staIsPerLanguage ? SanitizeText(DisplayHelper.GetTrainStatus(train, language, 0)) : string.Empty;
            string languageStatus1 = language != LANG_ENGLISH && staIsPerLanguage ? SanitizeText(DisplayHelper.GetTrainStatus(train, language, 1)) : string.Empty;

            string shown0 = DisplayTextFormatter.SelectLanguagePart(!string.IsNullOrEmpty(languageStatus0) ? languageStatus0 : english.Status, languageIndex);
            string shown1 = DisplayTextFormatter.SelectLanguagePart(!string.IsNullOrEmpty(languageStatus1) ? languageStatus1 : englishFlipText, languageIndex);

            bool rdsoCancelled = DisplayTextFormatter.IsCancelledStatus(englishStatus);
            bool rdsoTwoPart = !rdsoCancelled && DisplayTextFormatter.IsTwoPartStatus(englishStatus);
            bool rdsoRescheduled = !rdsoCancelled && !rdsoTwoPart && DisplayTextFormatter.IsRescheduledStatus(englishStatus);
            bool rdsoTextOnly = !rdsoCancelled && !rdsoTwoPart && !rdsoRescheduled && DisplayTextFormatter.IsSpecialStatusDetail(englishStatus);

            // 0x06 Cancelled: status only (translated text in Hindi / Regional when the feed has it).
            if (rdsoCancelled)
            {
                bool translated = language != LANG_ENGLISH && shown0.Length > 3 &&
                                  !string.Equals(shown0, englishStatus, StringComparison.OrdinalIgnoreCase);
                return translated ? shown0 : "CANCELLED";
            }

            // 0x08 Terminated At / 0x10 Diverted / 0x13 Change of Source - Start at: status + station, no times / PF.
            if (rdsoTwoPart)
            {
                if (DisplayTextFormatter.TrySplitTwoPartStatus(shown0, out _, out _))
                    return shown0;

                string station = DisplayTextFormatter.GetTwoPartSecondFrame(shown0, shown1);
                return JoinParts(shown0.Length > 0 ? shown0 : englishStatus, station);
            }

            // 0x04 Has Arrived / 0x07 Indefinite Late: status only, no times / PF.
            if (rdsoTextOnly)
                return shown0.Length > 0 ? shown0 : englishStatus;

            // 0x0F Rescheduled: status + actual arrival + actual departure + PF (no A/D).
            if (rdsoRescheduled)
            {
                string flipTime = DisplayTextFormatter.FormatTimeOnly(englishFlipText);
                if (!TimeSpan.TryParse(flipTime, out _)) flipTime = string.Empty;

                string reschArrival = !string.IsNullOrEmpty(arrivalTime) ? arrivalTime : flipTime;
                string reschDeparture = !string.IsNullOrEmpty(departureTime) ? departureTime : flipTime;

                return JoinParts(shown0.Length > 3 ? shown0 : "RESCHEDULED", reschArrival, reschDeparture, platform);
            }

            // Every other status (Right Time, Running Late, Is Arriving, Platform Change ...): times + PF.
            return JoinParts(arrivalTime, departureTime, platform);

            // ----- previous status handling (kept for reference) -----
//            bool isSpecial = english.SpecialStatus || english.FlipRequired;

//            if (!isSpecial)
//            {
//                // Cancelled trains sent as a plain status text
//                if (DisplayTextFormatter.IsCancelledStatus(english.Status))
//                    return "CANCELLED";

//                return JoinParts(arrivalTime, departureTime, platform);
//            }

//            // Special status texts prepared by TrainStatusProcessor (flip 0 / flip 1)
//            string englishText1 = SanitizeText(english.Status);
//            string englishText2 = english.FlipRequired ? SanitizeText(DisplayHelper.GetTrainStatus(train, LANG_ENGLISH, 1)) : string.Empty;

//            string text1 = englishText1;
//            string text2 = englishText2;

//            if (language != LANG_ENGLISH)
//            {
//                string languageText1 = SanitizeText(DisplayHelper.GetTrainStatus(train, language, 0));
//                string languageText2 = english.FlipRequired ? SanitizeText(DisplayHelper.GetTrainStatus(train, language, 1)) : string.Empty;

//                // Use the language text when available, otherwise keep the English status
//                if (!string.IsNullOrEmpty(languageText1) || !string.IsNullOrEmpty(languageText2))
//                {
//                    text1 = languageText1;
//                    text2 = languageText2;
//                }
//            }

//            // RESCHEDULED: status + new expected time + A/D + PF
//            if (DisplayTextFormatter.IsRescheduledStatus(englishText1))
//            {
//                string newTime = DisplayTextFormatter.FormatTimeOnly(englishText2);
//                return JoinParts(string.IsNullOrEmpty(text1) ? "RESCHEDULED" : text1, newTime, adFlag, platform);
//            }

//            // TERMINATED AT / DIVERTED / START AT / ARRIVED / INDEFINITE LATE / CANCELLED ...
//            return JoinParts(text1, text2);
        }

        // =========================================================
        // BOARD DATA
        // =========================================================

        public void LoadActiveBoardData()
        {
            try
            {
                var allSettings = _dbService.GET_BOARD_SETTING(0) ?? new List<BoardSettingInfo>();

                var installedSettings = allSettings.Where(b => b.IsActive == 1 && b.BOARD_NAME != null && (
                    b.BOARD_NAME.IndexOf("At-a-Glance", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    b.BOARD_NAME.IndexOf("At a Glance", StringComparison.OrdinalIgnoreCase) >= 0
                )).ToList();

                var activeTrains = _dbService.GetAllActiveBoardsForAGDB() ?? new List<AGDBDisplayBoardData>();
                _rawActiveTrains = activeTrains.Where(t => !string.IsNullOrEmpty(t.TrainNumber)).ToList();

                var tempBoardList = new List<AGDBDisplayBoardData>();
                var tempBoardLanguages = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

                if (installedSettings.Count > 0)
                {
                    foreach (var setting in installedSettings)
                    {
                        // Languages configured on this board (e.g. "Hindi+English+Marathi" -> E, H, R)
                        string settingIp = setting.BOARD_IP?.Trim() ?? string.Empty;
                        if (!tempBoardLanguages.ContainsKey(settingIp))
                            tempBoardLanguages[settingIp] = ParseLanguageCodes(setting.LANGUAGES);

                        // Configured PF for THIS board (stored so the payload can use it).
                        string configuredPf = !string.IsNullOrWhiteSpace(setting.SELECTED_PF)
                            ? setting.SELECTED_PF.Trim()
                            : (!string.IsNullOrWhiteSpace(setting.PF_NO) ? setting.PF_NO.Trim() : string.Empty);

                        var trainsToUse = _rawActiveTrains.Count > 0 ? _rawActiveTrains : activeTrains;

                        // Only trains whose platform is within this board's configured SELECTED_PF.
                        // When no trains match, other platforms' trains are not shown.
                        var platformFilteredTrains = !string.IsNullOrWhiteSpace(configuredPf)
                            ? trainsToUse.Where(t => PlatformMatches(configuredPf, t.PlatformNumber)).ToList()
                            : trainsToUse;

                        if (platformFilteredTrains.Count > 0)
                        {
                            foreach (var activeTrain in platformFilteredTrains)
                            {
                                tempBoardList.Add(new AGDBDisplayBoardData
                                {
                                    BoardId = setting.BOARD_ID,
                                    BoardName = setting.BOARD_NAME,
                                    BoardIp = setting.BOARD_IP,
                                    BoardLocation = setting.BOARD_LOCATION,
                                    BoardLine = setting.BOARD_LINE.ToString(),
                                    IsActive = setting.IsActive == 1,
                                    FontSize = setting.FONT_SIZE,
                                    DelayTime = setting.DELAYTIME > 0 ? setting.DELAYTIME : 10,
                                    TrainNumber = activeTrain.TrainNumber,
                                    TrainNameEnglish = activeTrain.TrainNameEnglish,
                                    TrainNameHindi = activeTrain.TrainNameHindi,
                                    ScheduleArrivalTime = activeTrain.ActualArrivalTime,
                                    ScheduleDepartureTime = activeTrain.ActualDepartureTime,
                                    // ACTUAL_* were never copied, so the board-data frame showed no time.
                                    ActualArrivalTime = activeTrain.ActualArrivalTime,
                                    ActualDepartureTime = activeTrain.ActualDepartureTime,
                                    ADFlag = activeTrain.ADFlag,
                                    TrainStatusEnglish = activeTrain.TrainStatusEnglish,
                                    PlatformNumber = !string.IsNullOrEmpty(activeTrain.PlatformNumber) ? activeTrain.PlatformNumber : configuredPf,
                                    SelectedPf = configuredPf,
                                    Coach_Sequence = activeTrain.Coach_Sequence
                                });
                            }
                        }
                        else
                        {
                            tempBoardList.Add(new AGDBDisplayBoardData
                            {
                                BoardId = setting.BOARD_ID,
                                BoardName = setting.BOARD_NAME,
                                BoardIp = setting.BOARD_IP,
                                BoardLocation = setting.BOARD_LOCATION,
                                BoardLine = setting.BOARD_LINE.ToString(),
                                IsActive = setting.IsActive == 1,
                                FontSize = setting.FONT_SIZE,
                                DelayTime = setting.DELAYTIME > 0 ? setting.DELAYTIME : 10,
                                TrainNumber = string.Empty,
                                PlatformNumber = configuredPf,
                                SelectedPf = configuredPf
                            });
                        }
                    }
                }
                else
                {
                    tempBoardList = activeTrains;
                }

                _allBoardData = tempBoardList;
                _boardLanguages = tempBoardLanguages;

                LoadCoachLegend();

                int activeDelay = _allBoardData.FirstOrDefault()?.DelayTime ?? 10;
                if (activeDelay <= 0) activeDelay = 10;
                if (_refreshTimer != null)
                {
                    _refreshTimer.Interval = activeDelay * 1000;
                }

                UpdateDynamicUI();
            }
            catch (Exception ex)
            {
                LogManager.LoggerService.Logger.Error(ex, "Error loading AGDB active board data.");
            }
        }

        private void UpdateDynamicUI()
        {
            var groups = _allBoardData.GroupBy(b => b.BoardId).ToList();
            bool needsRecreate = false;

            if (groups.Count == 0)
            {
                if (_dynamicRows.Count != 1 || _dynamicRows[0].BoardId != 0)
                    needsRecreate = true;
            }
            else if (_dynamicRows.Count != groups.Count)
            {
                needsRecreate = true;
            }
            else
            {
                for (int i = 0; i < groups.Count; i++)
                {
                    if (_dynamicRows[i].BoardId != groups[i].Key)
                    {
                        needsRecreate = true;
                        break;
                    }
                }
            }

            if (needsRecreate)
                RecreateBoardControls(groups);

            if (groups.Count == 0)
            {
                var row = _dynamicRows[0];
                row.TitleLabel.Text = "At-a-Glance Display Board (AGDB) No Active Boards";
                row.TrainTimer?.Stop();
                return;
            }

            for (int i = 0; i < groups.Count; i++)
            {
                var group = groups[i];
                var row = _dynamicRows[i];

                row.Trains = group.ToList();
                var firstBoard = group.First();

                // Cache IP + configured PF on the row for payload / filtering use.
                row.BoardIp = firstBoard.BoardIp;
                row.SelectedPf = ResolveSelectedPf(firstBoard);

                string platformStr = !string.IsNullOrEmpty(row.SelectedPf) ? row.SelectedPf
                    : (!string.IsNullOrEmpty(firstBoard.BoardLocation) ? firstBoard.BoardLocation : "N/A");
                string languagesStr = string.Join("/", GetBoardLanguages(row.BoardIp));
                row.TitleLabel.Text = $"At-a-Glance Display Board (AGDB) IP: {firstBoard.BoardIp} Platform: {platformStr} ";

                int delayTime = firstBoard.DelayTime ?? 10;
                if (delayTime <= 0) delayTime = 10;

                if (row.TrainTimer == null)
                {
                    row.TrainTimer = new System.Windows.Forms.Timer();
                    row.TrainTimer.Tag = row;
                    row.TrainTimer.Tick += (s, ev) =>
                    {
                        var timerRow = (BoardRowControls)((System.Windows.Forms.Timer)s).Tag;
                        TrainTimer_Tick_Row(timerRow);
                    };
                }
                row.TrainTimer.Interval = delayTime * 1000;
                row.TrainTimer.Start();

                // Before live data arrives, show the board data (only when the board pings).
                if (_lastProcessedLiveData == null)
                    RenderBoardDataFrame(row);
            }
        }

        private void TrainTimer_Tick_Row(BoardRowControls row)
        {
            if (row == null) return;

            // Next step = next language of the same train, then the next train
            row.CurrentTrainIndex = row.CurrentTrainIndex == int.MaxValue ? 0 : row.CurrentTrainIndex + 1;

            if (_lastProcessedLiveData != null)
                RenderLiveFramesToAGDB(_lastProcessedLiveData);
            else
                RenderBoardDataFrame(row);
        }

        /// <summary>
        /// Renders a row from the board data (used until the first live data is received).
        /// </summary>
        private void RenderBoardDataFrame(BoardRowControls row)
        {
            if (row == null || row.BoardPreview == null || row.BoardPreview.IsDisposed) return;

            if (!IsHostReachable(GetRowBoardIp(row)))
            {
                ClearBoard(row);
                return;
            }

            var trains = row.Trains?.Where(t => t != null && !string.IsNullOrWhiteSpace(t.TrainNumber)).ToList()
                         ?? new List<AGDBDisplayBoardData>();

            if (trains.Count == 0)
            {
                RenderAgdbFrame(row, new AgdbFrame { TrainLine = "No Data" });
                return;
            }

            List<string> languages = GetBoardLanguages(GetRowBoardIp(row));
            (int trainIndex, string language) = GetDisplayStep(row, trains.Count, languages);

            RenderAgdbFrame(row, BuildBoardDataFrame(trains[trainIndex], language));
        }

        private AgdbFrame BuildBoardDataFrame(AGDBDisplayBoardData train, string language)
        {
            string trainName = language == LANG_ENGLISH ? train.TrainNameEnglish
                : language == LANG_HINDI ? train.TrainNameHindi
                : string.Empty;

            //string statusText = SanitizeText(train.TrainStatusEnglish ?? train.ADFlag ?? string.Empty);
            //string upperStatus = statusText.ToUpperInvariant();
            // '#'-packed status: classify on the English part, show this language's part.
            string rawStatus = !string.IsNullOrWhiteSpace(train.TrainStatusEnglish) ? train.TrainStatusEnglish : (train.ADFlag ?? string.Empty);
            string statusText = SanitizeText(DisplayTextFormatter.SelectLanguagePart(rawStatus, GetLanguageIndex(language)));
            string upperStatus = SanitizeText(DisplayTextFormatter.SelectLanguagePart(rawStatus, 0)).ToUpperInvariant();

            bool isCancelled = DisplayTextFormatter.IsCancelledStatus(train.ADFlag) || DisplayTextFormatter.IsCancelledStatus(train.TrainStatusEnglish) || upperStatus.Contains("CANCEL");
            bool isRescheduled = DisplayTextFormatter.IsRescheduledStatus(train.ADFlag) || DisplayTextFormatter.IsRescheduledStatus(train.TrainStatusEnglish) || upperStatus.Contains("RESCHED");
            bool isTerminated = upperStatus.Contains("TERMINATED");
            bool isDiverted = upperStatus.Contains("DIVERTED");
            bool isSpecialDetail = DisplayTextFormatter.IsSpecialStatusDetail(train.ADFlag) || DisplayTextFormatter.IsSpecialStatusDetail(train.TrainStatusEnglish);

            string detailLine;
            if (isCancelled)
            {
                detailLine = "CANCELLED";
            }
            else if (isRescheduled)
            {
                detailLine = "RESCHEDULED";
            }
            else if (isTerminated)
            {
                detailLine = string.IsNullOrEmpty(statusText) ? "TERMINATED AT" : statusText;
            }
            else if (isDiverted)
            {
                detailLine = string.IsNullOrEmpty(statusText) ? "DIVERTED VIA" : statusText;
            }
            else if (isSpecialDetail)
            {
                detailLine = statusText;
            }
            else
            {
                string adFlag = SanitizeText(train.ADFlag);
                if (string.IsNullOrEmpty(adFlag) || adFlag == "-") adFlag = "A";
                if (adFlag.Length > 1) adFlag = adFlag.Substring(0, 1).ToUpperInvariant();

                string time = GetDisplayTime(adFlag, train.ActualArrivalTime, train.ActualArrivalTime);
                string arrivalTime = DisplayTextFormatter.FormatTimeOnly(train.ActualArrivalTime);
                string departureTime = DisplayTextFormatter.FormatTimeOnly(train.ActualDepartureTime);
                detailLine = JoinParts(arrivalTime, departureTime, SanitizeText(train.PlatformNumber));
            }

            return new AgdbFrame
            {
                TrainLine = JoinParts(SanitizeText(train.TrainNumber), SanitizeText(trainName)),
                DetailLine = detailLine,
                Coaches = GetCoachesForLanguage(ParseCoaches(train.Coach_Sequence), language)
            };
        }

        private void RecreateBoardControls(List<IGrouping<int, AGDBDisplayBoardData>> groups)
        {
            foreach (var row in _dynamicRows)
            {
                if (row.TitleLabel != null)
                {
                    this.Controls.Remove(row.TitleLabel);
                    row.TitleLabel.Dispose();
                }
                if (row.BoardPreview != null)
                    row.BoardPreview.Dispose();
                if (row.ScrollPanel != null)
                {
                    this.Controls.Remove(row.ScrollPanel);
                    row.ScrollPanel.Dispose();
                }
                if (row.TrainTimer != null)
                {
                    row.TrainTimer.Stop();
                    row.TrainTimer.Dispose();
                }
            }
            _dynamicRows.Clear();

            int count = groups.Count == 0 ? 1 : groups.Count;
            int currentY = 1;

            for (int i = 0; i < count; i++)
            {
                var group = groups.Count > 0 ? groups[i] : null;
                int boardId = group != null ? group.Key : 0;

                int availableWidth = this.Width - 50;
                int ledSize = Math.Max(4, availableWidth / MatrixCols);
                if (ledSize > 8) ledSize = 8;

                int boardWidth = ledSize * MatrixCols;
                int boardHeight = ledSize * MatrixRows;

                var titleLabel = new MaterialSkin.Controls.MaterialLabel
                {
                    AutoSize = true,
                    Depth = 0,
                    Font = new Font("Arial", 16F, FontStyle.Regular, GraphicsUnit.Pixel),
                    FontType = MaterialSkin.MaterialSkinManager.fontType.Subtitle1,
                    Location = new Point(17, currentY),
                    Margin = new Padding(0),
                    MouseState = MaterialSkin.MouseState.HOVER,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
                };

                var scrollPanel = new Panel
                {
                    AutoScroll = true,
                    AutoScrollMinSize = new Size(boardWidth, boardHeight),
                    BackColor = Color.FromArgb(20, 20, 20),
                    Margin = new Padding(0),
                    //Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                    // Width is set in SyncScrollPanelWidths() whenever this control resizes.
                    // The right anchor did not keep the panel inside the visible width, so the
                    // right part of the board was cut off and could not be scrolled to.
                    Anchor = AnchorStyles.Top | AnchorStyles.Left,
                    AutoSize = false
                };

                var boardPreview = new AGDBBoardPreview
                {
                    BackColor = Color.FromArgb(20, 20, 20),
                    Location = new Point(0, 0),
                    Margin = new Padding(0),
                    OffColor = Color.FromArgb(60, 60, 60),
                    OnColor = Color.FromArgb(76, 175, 80),
                    Size = new Size(boardWidth, boardHeight),
                    MatrixRows = MatrixRows,
                    MatrixCols = MatrixCols,
                    LedSizeMultiplier = 1.3f
                };

                scrollPanel.Controls.Add(boardPreview);

                this.Controls.Add(titleLabel);
                this.Controls.Add(scrollPanel);

                //int scrollPanelHeight = boardHeight + 2;
                int scrollPanelHeight = boardHeight + SystemInformation.HorizontalScrollBarHeight + 2;

                titleLabel.Location = new Point(17, currentY);
                currentY += 24;

                scrollPanel.Location = new Point(17, currentY);
                //scrollPanel.Size = new Size(this.Width - 34, scrollPanelHeight);
                scrollPanel.Size = new Size(GetScrollPanelWidth(), scrollPanelHeight);
                currentY += scrollPanelHeight + 20;

                var firstBoard = group?.FirstOrDefault();

                _dynamicRows.Add(new BoardRowControls
                {
                    BoardId = boardId,
                    BoardIp = firstBoard?.BoardIp,
                    SelectedPf = ResolveSelectedPf(firstBoard),
                    Trains = group?.ToList(),
                    CurrentTrainIndex = 0,
                    TitleLabel = titleLabel,
                    ScrollPanel = scrollPanel,
                    BoardPreview = boardPreview
                });
            }

            // Grow/shrink to fit every AGDB row, so the parent's scroll bar can reach the last board
            // (with a fixed height, the rows below it were clipped and could never be scrolled to)
            this.Height = currentY;
        }

        /// <summary>
        /// Visible width for a board's scroll panel: this control's width minus the 17 px margin on each side.
        /// </summary>
        private int GetScrollPanelWidth()
        {
            return Math.Max(100, this.ClientSize.Width - 34);
        }

        /// <summary>
        /// Keeps every scroll panel exactly as wide as the visible area, so a board wider than the
        /// screen gets a horizontal scroll bar that reaches its right edge. Layout only, no data change.
        /// </summary>
        private void SyncScrollPanelWidths()
        {
            int width = GetScrollPanelWidth();

            foreach (var row in _dynamicRows)
            {
                if (row?.ScrollPanel == null || row.ScrollPanel.IsDisposed) continue;

                if (row.ScrollPanel.Width != width)
                    row.ScrollPanel.Width = width;

                if (row.BoardPreview != null && !row.BoardPreview.IsDisposed)
                    row.ScrollPanel.AutoScrollMinSize = row.BoardPreview.Size;
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            SyncScrollPanelWidths();
        }

        // =========================================================
        // RENDERING
        // =========================================================

        private static void ClearBoard(BoardRowControls row)
        {
            if (row?.BoardPreview == null || row.BoardPreview.IsDisposed) return;
            row.BoardPreview.SetLEDMatrix(new bool[MatrixRows, MatrixCols]);
        }

        /// <summary>
        /// Draws one frame on the 384 x 64 LED board:
        /// 1st matrix = train line + detail line, 2nd matrix = coach positions.
        /// </summary>
        private void RenderAgdbFrame(BoardRowControls row, AgdbFrame frame)
        {
            if (row == null || row.BoardPreview == null || row.BoardPreview.IsDisposed || frame == null) return;

            bool[,] matrix = new bool[MatrixRows, MatrixCols];

            using (Bitmap bmp = new Bitmap(MatrixCols, MatrixRows))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Black);
                // One bit per LED: crisp letters on the LED matrix (anti-aliasing smears small text into blobs)
                g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                // 1st LED matrix (rows 0-31): train no + name (left) and time / A-D / PF or status (right) on one line
                DrawTrainLine(g, frame.TrainLine, frame.DetailLine, new Rectangle(2, 0, MatrixCols - 4, ModuleRows));

                // 2nd LED matrix (rows 32-63): coach positions
                DrawCoaches(g, frame.Coaches, new Rectangle(0, ModuleRows, MatrixCols, MatrixRows - ModuleRows));

                // Convert to LED matrix
                for (int y = 0; y < MatrixRows; y++)
                {
                    for (int x = 0; x < MatrixCols; x++)
                    {
                        Color p = bmp.GetPixel(x, y);
                        matrix[y, x] = p.R > 30 || p.G > 30 || p.B > 30;
                    }
                }
            }

            row.BoardPreview.SetLEDMatrix(matrix);
        }
        private static void DrawTrainLine(Graphics g, string leftText, string rightText, Rectangle bounds)
        {
            leftText = leftText ?? string.Empty;
            rightText = rightText ?? string.Empty;
            if (string.IsNullOrWhiteSpace(leftText) && string.IsNullOrWhiteSpace(rightText)) return;

            bool hasRight = !string.IsNullOrWhiteSpace(rightText);
            int gap = hasRight && !string.IsNullOrWhiteSpace(leftText) ? TrainLineGapPx : 0;

            using (StringFormat nearFormat = CreateTextFormat(StringAlignment.Near))
            using (StringFormat farFormat = CreateTextFormat(StringAlignment.Far))
            {
                int fontPx = Math.Min(TrainLineMaxFontPx, bounds.Height);
                while (fontPx > MinFontPx)
                {
                    float needed = MeasureTextWidth(g, leftText, fontPx, nearFormat) + gap + MeasureTextWidth(g, rightText, fontPx, nearFormat);
                    if (needed <= bounds.Width)
                        break;
                    fontPx--;
                }

                float rightWidth = hasRight ? Math.Min(bounds.Width, MeasureTextWidth(g, rightText, fontPx, nearFormat)) : 0f;

                // Right part: time / A-D / PF (or status), right-aligned
                if (hasRight)
                    DrawText(g, rightText, fontPx, bounds, farFormat);

                // Left part: train no + name, in the space left of the right part
                var leftBounds = new Rectangle(bounds.X, bounds.Y, Math.Max(0, bounds.Width - (int)Math.Ceiling(rightWidth) - gap), bounds.Height);
                DrawText(g, leftText, fontPx, leftBounds, nearFormat);
            }
        }

        /// <summary>
        /// Draws the coach sequence in the 2nd LED matrix, without borders:
        /// 2 lines (3 lines for more than 26 coaches), coaches evenly spaced and centered in their slot.
        /// All coaches use the same font size — the largest size at which every coach fits its slot.
        /// </summary>
        private static void DrawCoaches(Graphics g, List<string> coaches, Rectangle area)
        {
            if (coaches == null || coaches.Count == 0) return;

            int lineCount = coaches.Count == 1 ? 1 : (coaches.Count <= MaxCoachesInTwoLines ? 2 : 3);
            int coachesPerLine = (int)Math.Ceiling(coaches.Count / (double)lineCount);
            float slotWidth = area.Width / (float)coachesPerLine;
            int lineHeight = area.Height / lineCount;

            using (StringFormat centerFormat = CreateTextFormat(StringAlignment.Center))
            {
                // One font size for all coaches (largest that fits the widest coach code)
                int fontPx = Math.Min(CoachMaxFontPx, lineHeight);
                while (fontPx > MinFontPx &&
                       coaches.Any(c => MeasureTextWidth(g, c, fontPx, centerFormat) > slotWidth - 2))
                {
                    fontPx--;
                }

                for (int i = 0; i < coaches.Count; i++)
                {
                    int line = i / coachesPerLine;
                    int slot = i % coachesPerLine;

                    int left = area.X + (int)Math.Round(slot * slotWidth);
                    int right = area.X + (int)Math.Round((slot + 1) * slotWidth);
                    var slotBounds = new Rectangle(left, area.Y + line * lineHeight, right - left, lineHeight);

                    DrawText(g, coaches[i], fontPx, slotBounds, centerFormat);
                }
            }
        }

        /// <summary>
        /// Text format for LED text: one line, vertically centered, no wrapping / trimming.
        /// </summary>
        private static StringFormat CreateTextFormat(StringAlignment alignment)
        {
            var format = (StringFormat)StringFormat.GenericTypographic.Clone();
            format.Alignment = alignment;
            format.LineAlignment = StringAlignment.Center;
            format.FormatFlags |= StringFormatFlags.NoWrap;
            // GenericTypographic has LineLimit, which skips a line taller than the box —
            // Nirmala UI's line height is taller than a small LED row, so Hindi / Regional would not be drawn.
            format.FormatFlags &= ~StringFormatFlags.LineLimit;
            format.Trimming = StringTrimming.None;
            return format;
        }

        /// <summary>
        /// English uses Arial, Hindi / Regional use Nirmala UI.
        /// </summary>
        private static Font CreateLedFont(string text, int fontPx)
        {
            string fontFamily = ContainsNonAsciiText(text) ? IndicFontFamily : EnglishFontFamily;
            return new Font(fontFamily, Math.Max(MinFontPx, fontPx), FontStyle.Bold, GraphicsUnit.Pixel);
        }

        private static float MeasureTextWidth(Graphics g, string text, int fontPx, StringFormat format)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0f;

            using (Font font = CreateLedFont(text, fontPx))
                return g.MeasureString(text, font, PointF.Empty, format).Width;
        }

        private static void DrawText(Graphics g, string text, int fontPx, Rectangle bounds, StringFormat format)
        {
            if (string.IsNullOrWhiteSpace(text) || bounds.Width <= 0 || bounds.Height <= 0) return;

            using (Font font = CreateLedFont(text, fontPx))
            using (SolidBrush brush = new SolidBrush(Color.White))
            {
                g.SetClip(bounds);
                g.DrawString(text, font, brush, bounds, format);
                g.ResetClip();
            }
        }

        private static (int TrainIndex, string Language) GetDisplayStep(BoardRowControls row, int trainCount, List<string> languages)
        {
            if (languages == null || languages.Count == 0)
                languages = new List<string> { LANG_ENGLISH };

            int step = Math.Max(0, row.CurrentTrainIndex);
            int trainIndex = (step / languages.Count) % Math.Max(1, trainCount);
            string language = languages[step % languages.Count];

            return (trainIndex, language);
        }

        // =========================================================
        // LANGUAGES
        // =========================================================

        /// <summary>
        /// Converts a configured language value into language codes in display order (E, H, R).
        /// Accepts codes ("E#H#R") and board-setting names ("Hindi+English+Marathi").
        /// Any language name other than English / Hindi is the regional language (R).
        /// </summary>
        private static List<string> ParseLanguageCodes(string configuredLanguages)
        {
            var codes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            if (!string.IsNullOrWhiteSpace(configuredLanguages))
            {
                string[] tokens = configuredLanguages.Split(
                    new[] { '#', '+', ',', '|', ';' },
                    StringSplitOptions.RemoveEmptyEntries);

                foreach (string rawToken in tokens)
                {
                    string token = rawToken.Trim().ToUpperInvariant();
                    if (token.Length == 0)
                        continue;

                    switch (token)
                    {
                        case "E":
                        case "ENG":
                        case "ENGLISH":
                            codes.Add(LANG_ENGLISH);
                            break;

                        case "H":
                        case "HIN":
                        case "HINDI":
                            codes.Add(LANG_HINDI);
                            break;

                        case "NONE":
                        case "NA":
                        case "N/A":
                        case "0":
                            break;

                        default:
                            if (!token.Contains("SELECT"))
                                codes.Add(LANG_REGIONAL); // "R", "REGIONAL", "MARATHI", "GUJARATI", ...
                            break;
                    }
                }
            }

            return LanguageDisplayOrder.Where(codes.Contains).ToList();
        }

        /// <summary>
        /// Languages sent in the live data LineConf (LANG, or LAN as read by TrainStatusProcessor).
        /// </summary>
        private static List<string> GetLineConfLanguages(JsonArray? lineConf)
        {
            if (lineConf == null || lineConf.Count == 0 || lineConf[0] is not JsonObject config)
                return new List<string>();

            List<string> languages = ParseLanguageCodes(DisplayHelper.GetString(config, "LANG"));
            if (languages.Count == 0)
                languages = ParseLanguageCodes(DisplayHelper.GetString(config, "LAN"));

            return languages;
        }

        /// <summary>
        /// Languages to show on a board: the board's own configured languages,
        /// otherwise the LineConf languages, otherwise English.
        /// </summary>
        private List<string> GetBoardLanguages(string boardIp)
        {
            string ip = boardIp?.Trim() ?? string.Empty;

            if (_boardLanguages.TryGetValue(ip, out List<string> configured) && configured.Count > 0)
                return configured;

            if (_lineConfLanguages != null && _lineConfLanguages.Count > 0)
                return _lineConfLanguages;

            return new List<string> { LANG_ENGLISH };
        }

        /// <summary>
        /// The live data LineConf carries the languages in "LANG", but TrainStatusProcessor reads "LAN".
        /// Sets "LAN" (English + every language used by the boards) on the local live-data copy so the
        /// processor builds the Hindi / Regional status texts too. English is always included because the
        /// status logic works on the English text.
        /// </summary>
        private void PrepareLanguagesForProcessing(JsonObject liveData)
        {
            JsonArray? lineConf = liveData["LineConf"] as JsonArray;

            var codes = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { LANG_ENGLISH };
            foreach (string code in GetLineConfLanguages(lineConf))
                codes.Add(code);
            foreach (List<string> boardLanguages in _boardLanguages.Values)
                foreach (string code in boardLanguages)
                    codes.Add(code);

            string lan = string.Join("#", LanguageDisplayOrder.Where(codes.Contains));

            if (lineConf == null)
            {
                lineConf = new JsonArray();
                liveData["LineConf"] = lineConf;
            }

            if (lineConf.Count == 0)
                lineConf.Add(new JsonObject());

            if (lineConf[0] is JsonObject config)
                config["LAN"] = lan;
        }

        /// <summary>
        /// Train name in the requested language only (TNE / TNH / TNR).
        /// If that language's name is empty, the name stays empty (no fallback to English).
        /// </summary>
        private static string GetTrainNameForLanguage(TrainDisplayData train, string language)
        {
            if (train?.Names == null)
                return string.Empty;

            return train.Names.TryGetValue(language, out string name) && !string.IsNullOrWhiteSpace(name)
                ? name.Trim()
                : string.Empty;
        }

        // =========================================================
        // TEXT HELPERS
        // =========================================================

        /// <summary>
        /// "A" or "D" from the ADF field; when missing, "A" if there is an arrival time, otherwise "D".
        /// </summary>
        private static string GetArrivalDepartureFlag(string adf, TrainDisplayData train)
        {
            string flag = SanitizeText(adf).ToUpperInvariant();
            if (flag.StartsWith("A")) return "A";
            if (flag.StartsWith("D")) return "D";

            return !string.IsNullOrWhiteSpace(DisplayTextFormatter.FormatTimeOnly(train?.Eat)) ? "A" : "D";
        }

        /// <summary>
        /// Arrival time for arriving trains, departure time for departing trains (the other one as fallback).
        /// </summary>
        private static string GetDisplayTime(string adFlag, string arrivalTime, string departureTime)
        {
            string arrival = DisplayTextFormatter.FormatTimeOnly(arrivalTime);
            string departure = DisplayTextFormatter.FormatTimeOnly(departureTime);

            return adFlag == "A"
                ? (!string.IsNullOrEmpty(arrival) ? arrival : departure)
                : (!string.IsNullOrEmpty(departure) ? departure : arrival);
        }

        /// <summary>
        /// Splits the coach sequence (CCD) into coach codes: "ENG,GEN,S1,S2" / "ENG GEN S1" / "ENG-GEN-S1" / "ENG#GEN#S1".
        /// </summary>
        private static List<string> ParseCoaches(string rawCoachSequence)
        {
            string sanitized = SanitizeText(rawCoachSequence);
            if (string.IsNullOrWhiteSpace(sanitized))
                return new List<string>();

            return sanitized
                .Split(new[] { ',', ' ', '-', '#', '|', ';', '\t' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.Trim())
                .Where(c => c.Length > 0)
                .ToList();
        }

        /// <summary>
        /// Loads the Hindi coach names from the Coach Legend (LEGENDLIST: LEGEND = coach code, LEGEND_HINDI = Hindi).
        /// Keeps the previous names if the legend cannot be read.
        /// </summary>
        private void LoadCoachLegend()
        {
            try
            {
                var legendList = _dbService.GET_LEGEND_LIST() ?? new List<Legend>();
                var hindiNames = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                foreach (var legend in legendList)
                {
                    string code = legend?.LEGEND?.Trim();
                    string hindi = legend?.LEGEND_HINDI?.Trim();

                    if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(hindi) && !hindiNames.ContainsKey(code))
                        hindiNames[code] = hindi;
                }

                _coachHindiNames = hindiNames;
            }
            catch (Exception ex)
            {
                LogManager.LoggerService.Logger.Error(ex, "Error loading coach legend for AGDB.");
            }
        }

        private List<string> GetCoachesForLanguage(List<string> coachCodes, string language)
        {
            if (coachCodes == null || coachCodes.Count == 0 || language == LANG_ENGLISH)
                return coachCodes ?? new List<string>();

            if (language == LANG_HINDI)
            {
                var hindiCoaches = coachCodes
                    //.Select(code => _coachHindiNames.TryGetValue(code, out string hindi) ? hindi : string.Empty)
                    // A coach with no Hindi name in the legend keeps its code instead of an empty slot.
                    .Select(code => _coachHindiNames.TryGetValue(code, out string hindi) && !string.IsNullOrWhiteSpace(hindi) ? hindi : code)
                    .ToList();

                // Nothing translated -> show no coaches at all
                return hindiCoaches.Any(c => !string.IsNullOrWhiteSpace(c)) ? hindiCoaches : new List<string>();
            }

            return new List<string>();
        }

        private static string JoinParts(params string[] parts)
        {
            return string.Join("  ", parts.Where(p => !string.IsNullOrWhiteSpace(p)).Select(p => p.Trim()));
        }

        private static string SanitizeText(string text)
        {
            return DisplayTextFormatter.SanitizeForDisplay(text ?? string.Empty);
        }

        private static bool ContainsNonAsciiText(string text)
        {
            return !string.IsNullOrEmpty(text) && text.Any(c => c > 127);
        }

        private bool IsInDesignMode()
        {
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                return true;

            Control ctrl = this;
            while (ctrl != null)
            {
                if (ctrl.Site != null && ctrl.Site.DesignMode)
                    return true;
                ctrl = ctrl.Parent;
            }
            return false;
        }
    }

    public class AGDBBoardPreview : Panel
    {
        private bool[,] ledMatrix = new bool[64, 384];
        public Color OnColor { get; set; } = Color.FromArgb(76, 175, 80);
        public Color OffColor { get; set; } = Color.FromArgb(60, 60, 60);
        public Color FaultColor { get; set; } = Color.FromArgb(244, 67, 54);
        public int MatrixRows { get; set; } = 64;
        public int MatrixCols { get; set; } = 384;
        public float LedSizeMultiplier { get; set; } = 1.8f;

        public AGDBBoardPreview()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;
            ResizeRedraw = true;
        }

        public void SetLEDMatrix(bool[,] matrix)
        {
            if (matrix != null && matrix.GetLength(0) == 64 && matrix.GetLength(1) == 384)
            {
                ledMatrix = (bool[,])matrix.Clone();
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (var bgBrush = new SolidBrush(Color.FromArgb(15, 15, 20)))
                g.FillRectangle(bgBrush, ClientRectangle);

            float pixelW = (float)Width / 384;
            float pixelH = (float)Height / 64;

            float size = Math.Min(pixelW, pixelH) * 0.85f;
            if (size < 1.5f) size = 1.5f;

            for (int row = 0; row < 64; row++)
            {
                for (int col = 0; col < 384; col++)
                {
                    float x = col * pixelW + (pixelW - size) / 2f;
                    float y = row * pixelH + (pixelH - size) / 2f;

                    bool isActive = ledMatrix[row, col];

                    if (isActive)
                    {
                        using (var glowBrush = new SolidBrush(Color.FromArgb(60, OnColor)))
                        {
                            float glowSize = size * 1.8f;
                            g.FillEllipse(glowBrush, x - (glowSize - size) / 2f, y - (glowSize - size) / 2f, glowSize, glowSize);
                        }
                        using (var ledBrush = new SolidBrush(OnColor))
                        {
                            g.FillEllipse(ledBrush, x, y, size, size);
                        }
                    }
                    else
                    {
                        using (var ledBrush = new SolidBrush(OffColor))
                        {
                            g.FillEllipse(ledBrush, x, y, size, size);
                        }
                    }
                }
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e) { }
    }
}
