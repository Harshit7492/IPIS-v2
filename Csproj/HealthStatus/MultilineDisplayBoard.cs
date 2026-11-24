//using DataBaseHandler;
//using DataModel;
//using IPISv2.HealthStatus;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Drawing.Drawing2D;
//using System.Drawing.Imaging;
//using System.Drawing.Text;
//using System.Globalization;
//using System.Linq;
//using System.Net.NetworkInformation;
//using System.Text;
//using System.Text.Json;
//using System.Text.Json.Nodes;
//using System.Text.RegularExpressions;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//
//namespace IPISv2.HealthStatusReport
//{
//    public partial class MultilineDisplayBoard : UserControl
//    {
//        private List<SingleLineDisplayBoardData> _allBoardData = new List<SingleLineDisplayBoardData>();
//        private System.Windows.Forms.Timer _refreshTimer;
//        private int _calculatedHeight = 450;
//
//        // Shared column widths optimized for 672px LED matrix width (total ~46 chars)
//        private const int W_TrainNo = 6;
//        private const int W_TrainName = 14;
//        private const int W_Arrival = 5;
//        private const int W_AdFlag = 2;
//        private const int W_Platform = 4;
//        private const int W_Status = 12;
//
//        private class BoardRowControls
//        {
//            public int BoardId { get; set; }
//            public List<SingleLineDisplayBoardData> Trains { get; set; }
//            public MaterialSkin.Controls.MaterialLabel TitleLabel { get; set; }
//            public Panel ScrollPanel { get; set; }
//            public MultiLineBoardPreview BoardPreview { get; set; }
//            public int BoardLines { get; set; } = 3;
//        }
//
//        private readonly List<BoardRowControls> _dynamicRows = new List<BoardRowControls>();
//        private readonly MemberDomain.TrainStatusProcessor _statusProcessor = new MemberDomain.TrainStatusProcessor();
//        private bool _isProcessingLiveData = false;
//        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
//
//        // =========================================================
//        // LANGUAGES (E = English, H = Hindi, R = Regional)
//        // =========================================================
//
//        private const string LANG_ENGLISH = "E";
//        private const string LANG_HINDI = "H";
//        private const string LANG_REGIONAL = "R";
//
//        // Display order on the board: English -> Hindi -> Regional
//        private static readonly string[] LanguageDisplayOrder = { LANG_ENGLISH, LANG_HINDI, LANG_REGIONAL };
//
//        // Font used for Hindi / Regional text (Consolas has no Devanagari / Indic glyphs)
//        private const string IndicFontFamily = "Nirmala UI";
//
//        // Languages configured per board (key = board IP), filled in LoadActiveBoardData
//        private Dictionary<string, List<string>> _boardLanguages =
//            new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
//
//        public MultilineDisplayBoard()
//        {
//            InitializeComponent();
//            if (IsInDesignMode()) return;
//
//            // Hide default controls
//            if (lblTitle != null) lblTitle.Visible = false;
//            if (panelPreviewScroll != null) panelPreviewScroll.Visible = false;
//
//            _refreshTimer = new System.Windows.Forms.Timer();
//            // ✅ FIX #1: Use a short poll interval (2s). The render loop itself
//            // sleeps for PTO inside — the guard below prevents overlap.
//            _refreshTimer.Interval = 10000;
//            _refreshTimer.Tick += RefreshTimer_Tick;
//
//            LoadActiveBoardData();
//
//            if (!_refreshTimer.Enabled)
//                _refreshTimer.Start();
//
//            this.Disposed += (s, e) =>
//            {
//                _refreshTimer?.Stop();
//                _refreshTimer?.Dispose();
//                _cts?.Cancel();
//                _cts?.Dispose();
//            };
//        }
//
//        protected override void OnSizeChanged(EventArgs e)
//        {
//            base.OnSizeChanged(e);
//            if (this.Height != _calculatedHeight)
//            {
//                this.Height = _calculatedHeight;
//            }
//        }
//
//        private async void RefreshTimer_Tick(object sender, EventArgs e)
//        {
//            await ProcessAndRenderLiveDataAsync();
//        }
//
//        /// <summary>
//        /// Checks whether the given board IP responds to a ping.
//        /// Used to gate preview rendering: preview is only shown when the board pings successfully.
//        /// </summary>
//        private bool IsHostReachable(string ipAddress, int timeoutMs = 500)
//        {
//            if (string.IsNullOrWhiteSpace(ipAddress)) return false;
//
//            try
//            {
//                using (Ping ping = new Ping())
//                {
//                    PingReply reply = ping.Send(ipAddress.Trim(), timeoutMs);
//                    return reply != null && reply.Status == IPStatus.Success;
//                }
//            }
//            catch
//            {
//                return false;
//            }
//        }
//
//        public async Task ProcessAndRenderLiveDataAsync()
//        {
//            // ✅ Guard against overlapping ticks (render loop can take longer than the 2s interval).
//            if (_isProcessingLiveData)
//            {
//                LogManager.LoggerService.Logger.Debug("MLDB tick skipped (previous cycle still running).");
//                return;
//            }
//            _isProcessingLiveData = true;
//
//            try
//            {
//                // ✅ FIX #2: Reload board settings + active trains from DB on every tick
//                //           so that adding/removing trains or boards takes effect immediately.
//                LoadActiveBoardData();
//
//                LogManager.LoggerService.Logger.Information($"MLDB tick start. Boards in _dynamicRows={_dynamicRows.Count}");
//
//                var request = new LiveDataRequest
//                {
//                    Device = new List<DeviceInfo> { new DeviceInfo { Vendor = "VARDAAN", Device = "MLDB", IP = "10.0.1.1", PF = "1", INT = "50", HSR = "ALL OK" } },
//                    DisplayList = new List<DeviceInfo> { new DeviceInfo { Device = "MLDB", IP = "10.0.1.2", INT = "50", HSR = "ALL OK" } }
//                };
//                string rawJson = JsonSerializer.Serialize(request);
//
//                var liveData = await _dbService.GetLiveDataDisplayAsyncForMLDB(request, rawJson);
//
//                if (liveData == null || liveData.TrainList == null || liveData.TrainList.Count == 0)
//                {
//                    liveData = BuildFallbackLiveDataFromActiveBoards();
//                }
//
//                JsonObject? jsonObject = JsonSerializer.SerializeToNode(liveData) as JsonObject;
//                if (jsonObject == null) return;
//
//                // Make TrainStatusProcessor build the status texts for every configured language (E / H / R).
//                PrepareLanguagesForProcessing(jsonObject);
//
//                JsonObject processedOutput = _statusProcessor.Process(jsonObject);
//                await UpdateBoardsWithLiveDataAsync(processedOutput, _cts.Token);
//            }
//            catch (OperationCanceledException)
//            {
//                // normal on dispose
//            }
//            catch (Exception ex)
//            {
//                LogManager.LoggerService.Logger.Error(ex, "Error processing MLDB live data.");
//            }
//            finally
//            {
//                _isProcessingLiveData = false;
//            }
//        }
//
//        private async Task UpdateBoardsWithLiveDataAsync(JsonObject data, CancellationToken token)
//        {
//            JsonArray? trains = data["TrainList"] as JsonArray;
//            JsonArray? lineConf = data["LineConf"] as JsonArray;
//
//            if (trains == null || trains.Count == 0 || _dynamicRows.Count == 0)
//            {
//                UpdateBoardWithNoTrains();
//                return;
//            }
//
//            // Languages sent in LineConf — used for boards that have no language configured.
//            List<string> lineConfLanguages = GetLineConfLanguages(lineConf);
//
//            int boardDelay = _allBoardData.FirstOrDefault()?.DelayTime ?? 10;
//            double pto = DisplayHelper.GetPto(lineConf, boardDelay > 0 ? boardDelay : 10);
//            if (pto <= 0) pto = boardDelay > 0 ? boardDelay : 10.0;
//
//            for (int r = 0; r < _dynamicRows.Count; r++)
//            {
//                token.ThrowIfCancellationRequested();
//
//                var row = _dynamicRows[r];
//                if (row == null || row.BoardPreview == null || row.BoardPreview.IsDisposed) continue;
//
//                // Number of lines comes from the board setting (BOARD_LINE)
//                int lines = row.BoardLines > 0 ? row.BoardLines : 3;
//                List<List<JsonObject?>> pages = DisplayPageHelper.CreatePages(trains, lines);
//
//                if (pages.Count == 0) continue;
//
//                // Languages configured on this board, in display order: E, or E + H, or E + H + R
//                List<string> boardLanguages = GetBoardLanguages(row.Trains?.FirstOrDefault()?.BoardIp, lineConfLanguages);
//
//                for (int pageIndex = 0; pageIndex < pages.Count; pageIndex++)
//                {
//                    List<JsonObject?> page = pages[pageIndex];
//
//                    // Every page is shown once per configured language, each language for the full PTO time.
//                    foreach (string language in boardLanguages)
//                    {
//                        token.ThrowIfCancellationRequested();
//
//                        string[] frame0Lines = new string[lines];
//                        string[] frame1Lines = new string[lines];
//                        bool pageHasFlip = false;
//
//                        for (int i = 0; i < lines; i++)
//                        {
//                            JsonObject? tObj = (i < page.Count) ? page[i] : null;
//                            if (tObj != null)
//                            {
//                                // Status logic (cancelled / terminated / diverted / rescheduled / flip) always uses the English data.
//                                TrainDisplayData f0 = TrainDisplayHelper.BuildTrainData(tObj, LANG_ENGLISH, 0);
//                                string f0LanguageStatus = GetLanguageStatus(tObj, language, 0);
//                                frame0Lines[i] = FormatTrainLineWithStatus(f0, language, 0, f0.Status, f0LanguageStatus);
//
//                                if (f0.FlipRequired || f0.SpecialStatus)
//                                {
//                                    pageHasFlip = true;
//                                    TrainDisplayData f1 = TrainDisplayHelper.BuildTrainData(tObj, LANG_ENGLISH, 1);
//                                    string f1LanguageStatus = GetLanguageStatus(tObj, language, 1);
//                                    frame1Lines[i] = FormatTrainLineWithStatus(f1, language, 1, f0.Status, f1LanguageStatus);
//                                }
//                                else
//                                {
//                                    frame1Lines[i] = frame0Lines[i];
//                                }
//                            }
//                            else
//                            {
//                                frame0Lines[i] = FormatEmptyTrainLine();
//                                frame1Lines[i] = FormatEmptyTrainLine();
//                            }
//                        }
//
//                        int fontSize = row.Trains?.FirstOrDefault()?.FontSize ?? 13;
//
//                        // Only show the preview if the board's IP responds to a ping first.
//                        bool boardPingedF0 = IsHostReachable(row.Trains?.FirstOrDefault()?.BoardIp);
//
//                        // Step 1: Render Frame 0
//                        if (this.InvokeRequired)
//                        {
//                            this.Invoke(new Action(() => {
//                                if (boardPingedF0)
//                                    RenderBoardFrameLines(row.BoardPreview, frame0Lines, lines, fontSize);
//                            }));
//                        }
//                        else
//                        {
//                            if (boardPingedF0)
//                                RenderBoardFrameLines(row.BoardPreview, frame0Lines, lines, fontSize);
//                        }
//
//                        if (pageHasFlip)
//                        {
//                            // Wait first half of PTO (5 seconds for 10s PTO)
//                            await DisplayHelper.DelaySeconds(pto / 2.0, token);
//
//                            // Only show the preview if the board's IP responds to a ping first.
//                            bool boardPingedF1 = IsHostReachable(row.Trains?.FirstOrDefault()?.BoardIp);
//
//                            // Render Frame 1
//                            if (this.InvokeRequired)
//                            {
//                                this.Invoke(new Action(() => {
//                                    if (boardPingedF1)
//                                        RenderBoardFrameLines(row.BoardPreview, frame1Lines, lines, fontSize);
//                                }));
//                            }
//                            else
//                            {
//                                if (boardPingedF1)
//                                    RenderBoardFrameLines(row.BoardPreview, frame1Lines, lines, fontSize);
//                            }
//
//                            // Wait second half of PTO (5 seconds for 10s PTO)
//                            await DisplayHelper.DelaySeconds(pto / 2.0, token);
//                        }
//                        else
//                        {
//                            // Wait full PTO (10 seconds)
//                            await DisplayHelper.DelaySeconds(pto, token);
//                        }
//                    }
//                }
//            }
//        }
//
//        private LiveDataResponse BuildFallbackLiveDataFromActiveBoards()
//        {
//            var trains = new List<TrainDto>();
//            if (_allBoardData != null && _allBoardData.Count > 0)
//            {
//                foreach (var b in _allBoardData)
//                {
//                    if (string.IsNullOrEmpty(b.TrainNumber)) continue;
//                    trains.Add(new TrainDto
//                    {
//                        TNO = b.TrainNumber ?? "",
//                        TNE = b.TrainNameEnglish ?? "",
//                        TNH = b.TrainNameHindi ?? "",
//                        TNR = "", // no regional name in the board data -> regional name stays empty
//
//                        ADF = b.ADFlag ?? "",
//                        SAT = b.ScheduleArrivalTime ?? "",
//                        SDT = b.ScheduleDepartureTime ?? "",
//                        PNO = b.PlatformNumber ?? "",
//                        STA = !string.IsNullOrEmpty(b.TrainStatusEnglish) ? b.TrainStatusEnglish : "RIGHT TIME",
//                        CCD = b.Coach_Sequence ?? "",
//                        COL = "WHITE"
//                    });
//                }
//            }
//
//            return new LiveDataResponse
//            {
//                TrainList = trains,
//                LineConf = new List<LineConfDto> { new LineConfDto { INT = 5, PTO = 10, DTO = 5, CHR = 1, EFF = 1, SPD = 1, LANG = "E#H" } }
//            };
//        }
//
//        private void UpdateBoardWithNoTrains()
//        {
//            foreach (var row in _dynamicRows)
//            {
//                if (row == null || row.BoardPreview == null || row.BoardPreview.IsDisposed) continue;
//
//                int lines = row.BoardLines > 0 ? row.BoardLines : 3;
//                string[] emptyLines = new string[lines];
//                for (int i = 0; i < lines; i++) emptyLines[i] = FormatEmptyTrainLine();
//
//                // Only show the preview if the board's IP responds to a ping first.
//                bool boardPingedNoTrains = IsHostReachable(row.Trains?.FirstOrDefault()?.BoardIp);
//
//                if (boardPingedNoTrains)
//                    RenderBoardFrameLines(row.BoardPreview, emptyLines, lines, 13);
//            }
//        }
//
//        /// <summary>
//        /// Formats one board line.
//        /// <paramref name="train"/> is the English data (used for all status logic),
//        /// <paramref name="language"/> selects the train name (E / H / R — empty when that name is missing),
//        /// <paramref name="languageStatus"/> is the status text in that language (empty for English),
//        /// used instead of the English status label when available.
//        /// </summary>
//        private string FormatTrainLineWithStatus(TrainDisplayData train, string language, int flipIndex = 0, string primaryStatus = "", string languageStatus = "")
//        {
//            if (train == null || string.IsNullOrEmpty(train.TrainNo))
//            {
//                return FormatEmptyTrainLine();
//            }
//
//            string trainNo = DisplayTextFormatter.SanitizeForDisplay(train.TrainNo);
//            if (trainNo.Length > W_TrainNo) trainNo = trainNo.Substring(0, W_TrainNo);
//
//            // Train name only in the current language — empty when not available (no fallback to English).
//            string trainName = TruncateText(DisplayTextFormatter.SanitizeForDisplay(GetTrainNameForLanguage(train, language)), W_TrainName);
//
//            // Status text in the current language (Hindi / Regional), if the live data provides it.
//            string localizedStatus = TruncateText(DisplayTextFormatter.SanitizeForDisplay(languageStatus), 20);
//            bool hasLocalizedStatus = !string.IsNullOrEmpty(localizedStatus);
//
//            string statusText = DisplayTextFormatter.SanitizeForDisplay(train.Status);
//            string effectiveStatus = (!string.IsNullOrEmpty(primaryStatus) ? primaryStatus : statusText).ToUpperInvariant();
//
//            bool isCancelled = DisplayTextFormatter.IsCancelledStatus(train.Status) || effectiveStatus.Contains("CANCEL");
//            bool isRescheduled = DisplayTextFormatter.IsRescheduledStatus(train.Status) || effectiveStatus.Contains("RESCHED");
//            bool isTerminated = effectiveStatus.Contains("TERMINATED");
//            bool isDiverted = effectiveStatus.Contains("DIVERTED");
//
//            // 1. CANCELLED: Show only Train Number, Train Name, and CANCELLED
//            if (isCancelled)
//            {
//                return string.Format(
//                    "{0,-" + W_TrainNo + "} {1,-" + W_TrainName + "}  {2}",
//                    trainNo, trainName, hasLocalizedStatus ? localizedStatus : "CANCELLED");
//            }
//
//            // 2. TERMINATED AT: Show ONLY Train Number, Train Name, and Status/Station Name (NO time, NO A/D, NO Platform)
//            if (isTerminated)
//            {
//                if (flipIndex == 0)
//                {
//                    return string.Format(
//                        "{0,-" + W_TrainNo + "} {1,-" + W_TrainName + "}  {2}",
//                        trainNo, trainName, hasLocalizedStatus ? localizedStatus : "TERMINATED AT");
//                }
//                else
//                {
//                    string station = statusText;
//                    if (station.ToUpperInvariant().Contains("TERMINATED AT"))
//                    {
//                        station = station.Substring(station.ToUpperInvariant().IndexOf("TERMINATED AT") + "TERMINATED AT".Length).Trim();
//                        if (string.IsNullOrEmpty(station)) station = statusText;
//                    }
//                    if (station.Length > 20) station = station.Substring(0, 20);
//
//                    return string.Format(
//                        "{0,-" + W_TrainNo + "} {1,-" + W_TrainName + "}  {2}",
//                        trainNo, trainName, hasLocalizedStatus ? localizedStatus : station);
//                }
//            }
//
//            // 3. DIVERTED VIA: Show ONLY Train Number, Train Name, and Status/Route (NO time, NO A/D, NO Platform)
//            if (isDiverted)
//            {
//                if (flipIndex == 0)
//                {
//                    return string.Format(
//                        "{0,-" + W_TrainNo + "} {1,-" + W_TrainName + "}  {2}",
//                        trainNo, trainName, hasLocalizedStatus ? localizedStatus : "DIVERTED VIA");
//                }
//                else
//                {
//                    string route = statusText;
//                    if (route.ToUpperInvariant().Contains("DIVERTED VIA"))
//                    {
//                        route = route.Substring(route.ToUpperInvariant().IndexOf("DIVERTED VIA") + "DIVERTED VIA".Length).Trim();
//                        if (string.IsNullOrEmpty(route)) route = statusText;
//                    }
//                    if (route.Length > 20) route = route.Substring(0, 20);
//
//                    return string.Format(
//                        "{0,-" + W_TrainNo + "} {1,-" + W_TrainName + "}  {2}",
//                        trainNo, trainName, hasLocalizedStatus ? localizedStatus : route);
//                }
//            }
//
//            // 4. RESCHEDULED:
//            // Frame 0 (first half of PTO): Show Train Number, Train Name, and RESCHEDULED status
//            // Frame 1 (second half of PTO): Show Train Number, Train Name, Expected Time, A/D flag, and Platform number
//            if (isRescheduled)
//            {
//                if (flipIndex == 0)
//                {
//                    // First half of PTO: show RESCHEDULED status (in the current language when available)
//                    return string.Format(
//                        "{0,-" + W_TrainNo + "} {1,-" + W_TrainName + "}  {2}",
//                        trainNo, trainName, hasLocalizedStatus ? localizedStatus : "RESCHEDULED");
//                }
//                else
//                {
//                    // Second half of PTO: show Expected Time, A/D flag, and Platform number
//                    string expTime = DisplayTextFormatter.FormatTimeOnly(train.Eat);
//                    if (string.IsNullOrEmpty(expTime)) expTime = DisplayTextFormatter.FormatTimeOnly(train.Edt);
//                    if (expTime.Length > W_Arrival) expTime = expTime.Substring(0, W_Arrival);
//
//                    string platform = DisplayTextFormatter.SanitizeForDisplay(train.Platform);
//                    if (platform.Length > W_Platform) platform = platform.Substring(0, W_Platform);
//
//                    string adFlag = "D";
//                    if (!string.IsNullOrEmpty(train.Status))
//                    {
//                        string upperStatus = train.Status.ToUpperInvariant();
//                        if (upperStatus.StartsWith("A") || upperStatus.Contains("ARR"))
//                            adFlag = "A";
//                    }
//                    if (adFlag.Length > W_AdFlag) adFlag = adFlag.Substring(0, W_AdFlag);
//
//                    return string.Format(
//                        "{0,-" + W_TrainNo + "} {1,-" + W_TrainName + "} {2,-" + W_Arrival + "} {3,-" + W_AdFlag + "} {4,-" + W_Platform + "}",
//                        trainNo, trainName, expTime, adFlag, platform);
//                }
//            }
//
//            // 5. NORMAL TRAIN LINE: Show TrainNo, TrainName, Time, A/D, Platform
//            string arrTime = DisplayTextFormatter.FormatTimeOnly(train.Eat);
//            if (string.IsNullOrEmpty(arrTime)) arrTime = DisplayTextFormatter.FormatTimeOnly(train.Edt);
//            if (arrTime.Length > W_Arrival) arrTime = arrTime.Substring(0, W_Arrival);
//
//            string pf = DisplayTextFormatter.SanitizeForDisplay(train.Platform);
//            if (pf.Length > W_Platform) pf = pf.Substring(0, W_Platform);
//
//            string ad = "A";
//            if (!string.IsNullOrEmpty(train.Status))
//            {
//                string upperStatus = train.Status.ToUpperInvariant();
//                if (upperStatus.StartsWith("D") || upperStatus.Contains("DEP"))
//                    ad = "D";
//            }
//            if (ad.Length > W_AdFlag) ad = ad.Substring(0, W_AdFlag);
//
//            // For normal trains: NO status column, just TrainNo, TrainName, Time, A/D, Platform
//            return string.Format(
//                "{0,-" + W_TrainNo + "} {1,-" + W_TrainName + "} {2,-" + W_Arrival + "} {3,-" + W_AdFlag + "} {4,-" + W_Platform + "}",
//                trainNo, trainName, arrTime, ad, pf);
//        }
//
//        private string FormatEmptyTrainLine()
//        {
//            return string.Format(
//                "{0,-" + W_TrainNo + "} {1,-" + W_TrainName + "} {2,-" + W_Arrival + "} {3,-" + W_AdFlag + "} {4,-" + W_Platform + "} {5,-" + W_Status + "}",
//                "-", "-", "-", "-", "-", "-");
//        }
//
//        protected override void OnResize(EventArgs e)
//        {
//            base.OnResize(e);
//            if (panelPreviewScroll != null)
//            {
//                panelPreviewScroll.Left = 10;
//                panelPreviewScroll.Width = this.Width - 20;
//            }
//            if (lblTitle != null)
//            {
//                lblTitle.Left = 10;
//            }
//        }
//
//        public void LoadActiveBoardData()
//        {
//            try
//            {
//                var allSettings = _dbService.GET_BOARD_SETTING(0) ?? new List<BoardSettingInfo>();
//                var installedSettings = allSettings
//                    .Where(b => b.IsActive == 1 &&
//                                !string.IsNullOrEmpty(b.BOARD_NAME) &&
//                                (b.BOARD_NAME.IndexOf("Multiline", StringComparison.OrdinalIgnoreCase) >= 0 ||
//                                 b.BOARD_NAME.IndexOf("MLDB", StringComparison.OrdinalIgnoreCase) >= 0 ||
//                                 b.BOARD_NAME.IndexOf("Multi Line", StringComparison.OrdinalIgnoreCase) >= 0))
//                    .ToList();
//
//                var activeTrains = _dbService.GetAllActiveBoards() ?? new List<SingleLineDisplayBoardData>();
//
//                var tempBoardList = new List<SingleLineDisplayBoardData>();
//                var tempBoardLanguages = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
//
//                foreach (var setting in installedSettings)
//                {
//                    // Languages configured on this board (e.g. "Hindi+English+Marathi" -> E, H, R)
//                    string settingIp = setting.BOARD_IP?.Trim() ?? string.Empty;
//                    if (!tempBoardLanguages.ContainsKey(settingIp))
//                    {
//                        tempBoardLanguages[settingIp] = ParseLanguageCodes(setting.LANGUAGES);
//                    }
//
//                    int delayTime = setting.DELAYTIME > 0 ? setting.DELAYTIME : 10;
//                    var matches = activeTrains.Where(t =>
//                        t.BoardId == setting.BOARD_ID ||
//                        (string.Equals(t.BoardIp?.Trim(), setting.BOARD_IP?.Trim(), StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(setting.BOARD_IP)))
//                        .ToList();
//
//                    if (matches.Count > 0)
//                    {
//                        foreach (var match in matches)
//                        {
//                            match.BoardName = setting.BOARD_NAME;
//                            match.BoardLocation = setting.BOARD_LOCATION;
//                            match.BoardIp = setting.BOARD_IP;
//                            match.FontSize = setting.FONT_SIZE;
//                            match.DelayTime = delayTime;
//                            match.BoardLine = setting.BOARD_LINE.ToString();
//                            tempBoardList.Add(match);
//                        }
//                    }
//                    else
//                    {
//                        tempBoardList.Add(new SingleLineDisplayBoardData
//                        {
//                            BoardId = setting.BOARD_ID,
//                            BoardName = setting.BOARD_NAME,
//                            BoardIp = setting.BOARD_IP,
//                            BoardLocation = setting.BOARD_LOCATION,
//                            BoardLine = setting.BOARD_LINE.ToString(),
//                            IsActive = setting.IsActive == 1,
//                            FontSize = setting.FONT_SIZE,
//                            DelayTime = delayTime,
//                            TrainNumber = string.Empty,
//                            PlatformNumber = setting.PF_NO
//                        });
//                    }
//                }
//
//                _allBoardData = tempBoardList;
//                _boardLanguages = tempBoardLanguages;
//
//                // ✅ FIX #3: Removed the block that was overriding _refreshTimer.Interval
//                //           The timer is now permanently set to 2000 ms in the constructor.
//
//                UpdateDynamicUI(tempBoardList);
//            }
//            catch (Exception ex)
//            {
//                LogManager.LoggerService.Logger.Error(ex, "Error loading MLDB data.");
//                if (lblTitle != null) lblTitle.Text = "Multi Line Display Board (MLDB) - Error Loading Data";
//                UpdateDynamicUI(new List<SingleLineDisplayBoardData>());
//            }
//        }
//
//        private void UpdateDynamicUI(List<SingleLineDisplayBoardData> mldbBoards)
//        {
//            var groups = mldbBoards
//                .GroupBy(b => b.BoardId)
//                .ToList();
//
//            bool needsRecreate = false;
//
//            if (groups.Count == 0)
//            {
//                if (_dynamicRows.Count != 1 || _dynamicRows[0].BoardId != 0)
//                {
//                    needsRecreate = true;
//                }
//            }
//            else
//            {
//                if (_dynamicRows.Count != groups.Count)
//                {
//                    needsRecreate = true;
//                }
//                else
//                {
//                    for (int i = 0; i < groups.Count; i++)
//                    {
//                        if (_dynamicRows[i].BoardId != groups[i].Key)
//                        {
//                            needsRecreate = true;
//                            break;
//                        }
//
//                        // Number of lines changed in the board setting -> rebuild so the preview area fits the new line count
//                        if (GetConfiguredLineCount(groups[i].First()) != _dynamicRows[i].BoardLines)
//                        {
//                            needsRecreate = true;
//                            break;
//                        }
//                    }
//                }
//            }
//
//            if (needsRecreate)
//            {
//                RecreateBoardControls(groups);
//            }
//
//            if (groups.Count == 0)
//            {
//                var row = _dynamicRows[0];
//                if (row.TitleLabel != null) row.TitleLabel.Text = "Multi Line Display Board (MLDB) No Active Boards";
//                ClearBoardPreview(row.BoardPreview, row.BoardLines);
//            }
//            else
//            {
//                for (int i = 0; i < groups.Count; i++)
//                {
//                    var group = groups[i];
//                    var row = _dynamicRows[i];
//
//                    // ✅ Update the train list for this board every tick so add/remove
//                    //    of individual trains reflects immediately.
//                    row.Trains = group.ToList();
//
//                    var firstBoard = group.First();
//                    row.BoardLines = GetConfiguredLineCount(firstBoard);
//
//                    string platformStr = !string.IsNullOrEmpty(firstBoard.PlatformNumber) ? firstBoard.PlatformNumber :
//                                        (!string.IsNullOrEmpty(firstBoard.BoardLocation) ? firstBoard.BoardLocation : "N/A");
//                    if (row.TitleLabel != null) row.TitleLabel.Text = $"Multi Line Display Board (MLDB) IP: {firstBoard.BoardIp} Platform: {platformStr}";
//
//                    int fontSize = firstBoard.FontSize ?? 13;
//
//                    // Only show the preview if the board's IP responds to a ping first.
//                    bool boardPinged = IsHostReachable(firstBoard.BoardIp);
//
//                    if (boardPinged)
//                        RenderBoardFrame(row.BoardPreview, row.Trains, row.BoardLines, fontSize);
//                }
//            }
//        }
//
//        private void RecreateBoardControls(List<IGrouping<int, SingleLineDisplayBoardData>> groups)
//        {
//            foreach (var row in _dynamicRows)
//            {
//                if (row.TitleLabel != null)
//                {
//                    this.Controls.Remove(row.TitleLabel);
//                    row.TitleLabel.Dispose();
//                }
//                if (row.BoardPreview != null)
//                {
//                    row.BoardPreview.Dispose();
//                }
//                if (row.ScrollPanel != null)
//                {
//                    this.Controls.Remove(row.ScrollPanel);
//                    row.ScrollPanel.Dispose();
//                }
//            }
//            _dynamicRows.Clear();
//
//            int count = groups.Count == 0 ? 1 : groups.Count;
//            int currentY = 12;
//
//            for (int i = 0; i < count; i++)
//            {
//                var group = groups.Count > 0 ? groups[i] : null;
//                int boardId = group != null ? group.Key : 0;
//
//                int lines = group != null ? GetConfiguredLineCount(group.First()) : 3;
//
//                var titleLabel = new MaterialSkin.Controls.MaterialLabel
//                {
//                    AutoSize = true,
//                    Depth = 0,
//                    Font = new Font("Roboto", 16F, FontStyle.Regular, GraphicsUnit.Pixel),
//                    FontType = MaterialSkin.MaterialSkinManager.fontType.Subtitle1,
//                    Location = new Point(17, currentY),
//                    Margin = new Padding(0),
//                    MouseState = MaterialSkin.MouseState.HOVER,
//                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
//                    ForeColor = Color.FromArgb(200, 200, 200)
//                };
//
//                var scrollPanel = new Panel
//                {
//                    AutoScroll = true,
//                    BackColor = Color.FromArgb(20, 20, 20),
//                    Margin = new Padding(0),
//                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
//                };
//
//                int previewHeight = lines * 80;
//                int previewWidth = 1800;
//                var boardPreview = new MultiLineBoardPreview
//                {
//                    BackColor = Color.FromArgb(20, 20, 20),
//                    FaultColor = Color.FromArgb(244, 67, 54),
//                    Location = new Point(0, 0),
//                    Margin = new Padding(0),
//                    OffColor = Color.FromArgb(60, 60, 60),
//                    OnColor = Color.FromArgb(76, 175, 80),
//                    Size = new Size(previewWidth, previewHeight)
//                };
//
//                scrollPanel.Controls.Add(boardPreview);
//
//                this.Controls.Add(titleLabel);
//                this.Controls.Add(scrollPanel);
//
//                int scrollPanelHeight = Math.Max(260, boardPreview.Height + SystemInformation.HorizontalScrollBarHeight + 12);
//
//                titleLabel.Location = new Point(17, currentY);
//                currentY += 28;
//
//                scrollPanel.Location = new Point(17, currentY);
//                scrollPanel.Size = new Size(Math.Max(300, this.Width - 34), scrollPanelHeight);
//                currentY += scrollPanelHeight + 16;
//
//                _dynamicRows.Add(new BoardRowControls
//                {
//                    BoardId = boardId,
//                    Trains = group?.ToList(),
//                    TitleLabel = titleLabel,
//                    ScrollPanel = scrollPanel,
//                    BoardPreview = boardPreview,
//                    BoardLines = lines
//                });
//            }
//
//            _calculatedHeight = currentY + 30;
//            this.Height = _calculatedHeight;
//        }
//
//        private void ClearBoardPreview(MultiLineBoardPreview preview, int lines)
//        {
//            int width = 672;
//            int totalHeight = lines * 32;
//
//            preview.InitializeMatrix(totalHeight, width);
//            preview.Size = new Size(1800, totalHeight * 2);
//
//            bool[,] matrix = new bool[totalHeight, width];
//            preview.SetLEDMatrix(matrix);
//        }
//
//        private void RenderBoardFrame(MultiLineBoardPreview preview, List<SingleLineDisplayBoardData> trains, int lines, int fontSizeVal)
//        {
//            int singleLineHeight = 32;
//            int width = 672;
//            int totalHeight = lines * singleLineHeight;
//
//            preview.InitializeMatrix(totalHeight, width);
//            preview.Size = new Size(1800, totalHeight * 2);
//
//            var uniqueTrains = trains
//                .GroupBy(t => t.TrainNumber, StringComparer.OrdinalIgnoreCase)
//                .Select(g => g.First())
//                .OrderBy(t => t.ScheduleArrivalTime)
//                .ToList();
//
//            string[] trainLines = new string[lines];
//            for (int i = 0; i < lines; i++)
//            {
//                if (i < uniqueTrains.Count)
//                {
//                    trainLines[i] = FormatTrainLineWithStatusFromData(uniqueTrains[i]);
//                }
//                else
//                {
//                    trainLines[i] = FormatEmptyTrainLine();
//                }
//            }
//
//            RenderBoardFrameLinesInternal(preview, trainLines, lines, fontSizeVal);
//        }
//
//        private void RenderBoardFrameLines(MultiLineBoardPreview preview, string[] trainLines, int lines, int fontSizeVal)
//        {
//            int singleLineHeight = 32;
//            int width = 672;
//            int totalHeight = lines * singleLineHeight;
//
//            preview.InitializeMatrix(totalHeight, width);
//            preview.Size = new Size(1800, totalHeight * 2);
//
//            RenderBoardFrameLinesInternal(preview, trainLines, lines, fontSizeVal);
//        }
//
//        private void RenderBoardFrameLinesInternal(MultiLineBoardPreview preview, string[] trainLines, int lines, int fontSizeVal)
//        {
//            int singleLineHeight = 32;
//            int width = 672;
//            int totalHeight = lines * singleLineHeight;
//
//            bool[,] matrix = new bool[totalHeight, width];
//            const int scale = 4;
//            int scaledWidth = width * scale;
//            int scaledHeight = totalHeight * scale;
//            int scaledLineHeight = singleLineHeight * scale;
//
//            int fontSize = 15;
//
//            using (Bitmap bmp = new Bitmap(scaledWidth, scaledHeight, PixelFormat.Format32bppArgb))
//            using (Graphics g = Graphics.FromImage(bmp))
//            {
//                g.Clear(Color.Black);
//
//                g.SmoothingMode = SmoothingMode.HighQuality;
//                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
//                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
//                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
//
//                using (Font font = new Font("Consolas", fontSize * scale, FontStyle.Bold, GraphicsUnit.Pixel))
//                using (SolidBrush brush = new SolidBrush(Color.White))
//                {
//                    bool frameHasIndicText = trainLines.Take(lines).Any(ContainsNonAsciiText);
//
//                    if (!frameHasIndicText)
//                    {
//                        // English frame: drawn exactly as before (one Consolas string per line)
//                        for (int i = 0; i < lines && i < trainLines.Length; i++)
//                        {
//                            string lineText = trainLines[i] ?? string.Empty;
//
//                            SizeF textSize = g.MeasureString(lineText, font);
//                            float yOffset = i * scaledLineHeight + (scaledLineHeight - textSize.Height) / 2f;
//                            float xOffset = 6 * scale;
//
//                            g.DrawString(lineText, font, brush, xOffset, yOffset);
//                        }
//                    }
//                    else
//                    {
//                        // Hindi / Regional frame: draw column by column so Devanagari / Indic text uses an
//                        // Indic-capable font while the columns stay aligned to the Consolas grid.
//                        using (Font indicFont = new Font(IndicFontFamily, fontSize * scale, FontStyle.Bold, GraphicsUnit.Pixel))
//                        {
//                            for (int i = 0; i < lines && i < trainLines.Length; i++)
//                            {
//                                DrawLineByColumns(g, trainLines[i] ?? string.Empty, font, indicFont, brush,
//                                    6 * scale, i * scaledLineHeight, scaledLineHeight, scaledWidth);
//                            }
//                        }
//                    }
//                }
//
//                ConvertBitmapToMatrix(bmp, matrix, width, totalHeight, scale);
//            }
//
//            preview.SetLEDMatrix(matrix);
//        }
//
//        private string FormatTrainLineWithStatusFromData(SingleLineDisplayBoardData data)
//        {
//            if (data == null || string.IsNullOrEmpty(data.TrainNumber))
//            {
//                return FormatEmptyTrainLine();
//            }
//
//            string trainNo = DisplayTextFormatter.SanitizeForDisplay(data.TrainNumber);
//            if (trainNo.Length > W_TrainNo) trainNo = trainNo.Substring(0, W_TrainNo);
//
//            string trainName = DisplayTextFormatter.SanitizeForDisplay(data.TrainNameEnglish ?? "");
//            if (trainName.Length > W_TrainName) trainName = trainName.Substring(0, W_TrainName);
//
//            // Check if cancelled FIRST
//            bool isCancelled = DisplayTextFormatter.IsCancelledStatus(data.ADFlag);
//
//            // If cancelled, show ONLY Train No, Train Name, and CANCELLED
//            if (isCancelled)
//            {
//                return string.Format(
//                    "{0,-" + W_TrainNo + "} {1,-" + W_TrainName + "}  {2}",
//                    trainNo, trainName, "CANCELLED");
//            }
//
//            // Check for special status (TERMINATED AT, DIVERTED VIA, RESCHEDULED)
//            bool isSpecialStatus = DisplayTextFormatter.IsSpecialStatusDetail(data.ADFlag);
//
//            if (isSpecialStatus)
//            {
//                string status = DisplayTextFormatter.SanitizeForDisplay(data.ADFlag);
//                if (status.Length > 25) status = status.Substring(0, 25);
//
//                return string.Format(
//                    "{0,-" + W_TrainNo + "} {1,-" + W_TrainName + "}  {2}",
//                    trainNo, trainName, status);
//            }
//
//            // For normal trains: NO status, show all other details
//            string arrival = DisplayTextFormatter.FormatTimeOnly(data.ScheduleArrivalTime);
//            if (string.IsNullOrEmpty(arrival)) arrival = DisplayTextFormatter.FormatTimeOnly(data.ScheduleDepartureTime);
//            if (arrival.Length > W_Arrival) arrival = arrival.Substring(0, W_Arrival);
//
//            string platform = DisplayTextFormatter.SanitizeForDisplay(data.PlatformNumber);
//            if (platform.Length > W_Platform) platform = platform.Substring(0, W_Platform);
//
//            string adFlag = DisplayTextFormatter.SanitizeForDisplay(data.ADFlag);
//            if (string.IsNullOrEmpty(adFlag)) adFlag = "A";
//            if (adFlag.Length > W_AdFlag) adFlag = adFlag.Substring(0, W_AdFlag);
//
//            // Normal train: NO status column
//            return string.Format(
//                "{0,-" + W_TrainNo + "} {1,-" + W_TrainName + "} {2,-" + W_Arrival + "} {3,-" + W_AdFlag + "} {4,-" + W_Platform + "}",
//                trainNo, trainName, arrival, adFlag, platform);
//        }
//
//        public void UpdateHealthStatus(bool online, double temperatureCelsius, double voltage, TimeSpan uptime)
//        {
//            foreach (var row in _dynamicRows)
//            {
//                if (row != null && row.BoardPreview != null && !row.BoardPreview.IsDisposed)
//                {
//                    row.BoardPreview.Enabled = online;
//                }
//            }
//        }
//
//        private static void ConvertBitmapToMatrix(Bitmap bmp, bool[,] matrix, int width, int height, int scale)
//        {
//            BitmapData bmpData = bmp.LockBits(
//                new Rectangle(0, 0, bmp.Width, bmp.Height),
//                ImageLockMode.ReadOnly,
//                PixelFormat.Format32bppArgb);
//
//            try
//            {
//                int stride = bmpData.Stride;
//                const int bytesPerPixel = 4;
//                int byteCount = stride * bmp.Height;
//                byte[] pixels = new byte[byteCount];
//                System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, pixels, 0, byteCount);
//
//                for (int cellY = 0; cellY < height; cellY++)
//                {
//                    for (int cellX = 0; cellX < width; cellX++)
//                    {
//                        bool lit = false;
//                        for (int sy = 0; sy < scale && !lit; sy++)
//                        {
//                            int py = cellY * scale + sy;
//                            if (py >= bmp.Height) break;
//                            int rowOffset = py * stride;
//
//                            for (int sx = 0; sx < scale && !lit; sx++)
//                            {
//                                int px = cellX * scale + sx;
//                                if (px >= bmp.Width) break;
//                                int idx = rowOffset + px * bytesPerPixel;
//
//                                byte b = pixels[idx];
//                                byte gCol = pixels[idx + 1];
//                                byte r = pixels[idx + 2];
//
//                                int maxComponent = Math.Max(r, Math.Max(gCol, b));
//                                if (maxComponent / 255f > 0.45f)
//                                {
//                                    lit = true;
//                                    break;
//                                }
//                            }
//                        }
//                        matrix[cellY, cellX] = lit;
//                    }
//                }
//            }
//            finally
//            {
//                bmp.UnlockBits(bmpData);
//            }
//        }
//
//        private bool IsInDesignMode()
//        {
//            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
//                return true;
//
//            Control ctrl = this;
//            while (ctrl != null)
//            {
//                if (ctrl.Site != null && ctrl.Site.DesignMode)
//                    return true;
//                ctrl = ctrl.Parent;
//            }
//            return false;
//        }
//
//        // =========================================================
//        // LINES
//        // =========================================================
//
//        /// <summary>
//        /// Number of lines configured for the board (BOARD_LINE), 3 when not set.
//        /// </summary>
//        private static int GetConfiguredLineCount(SingleLineDisplayBoardData board)
//        {
//            return board != null && int.TryParse(board.BoardLine, out int lines) && lines > 0 ? lines : 3;
//        }
//
//        // =========================================================
//        // LANGUAGES
//        // =========================================================
//
//        /// <summary>
//        /// Converts a configured language value into language codes in display order (E, H, R).
//        /// Accepts codes ("E#H#R") and board-setting names ("Hindi+English+Marathi").
//        /// Any language name other than English / Hindi is the regional language (R).
//        /// </summary>
//        private static List<string> ParseLanguageCodes(string configuredLanguages)
//        {
//            var codes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
//
//            if (!string.IsNullOrWhiteSpace(configuredLanguages))
//            {
//                string[] tokens = configuredLanguages.Split(
//                    new[] { '#', '+', ',', '|', ';' },
//                    StringSplitOptions.RemoveEmptyEntries);
//
//                foreach (string rawToken in tokens)
//                {
//                    string token = rawToken.Trim().ToUpperInvariant();
//                    if (token.Length == 0)
//                        continue;
//
//                    switch (token)
//                    {
//                        case "E":
//                        case "ENG":
//                        case "ENGLISH":
//                            codes.Add(LANG_ENGLISH);
//                            break;
//
//                        case "H":
//                        case "HIN":
//                        case "HINDI":
//                            codes.Add(LANG_HINDI);
//                            break;
//
//                        case "NONE":
//                        case "NA":
//                        case "N/A":
//                        case "0":
//                            break;
//
//                        default:
//                            if (!token.Contains("SELECT"))
//                                codes.Add(LANG_REGIONAL); // "R", "REGIONAL", "MARATHI", "GUJARATI", ...
//                            break;
//                    }
//                }
//            }
//
//            return LanguageDisplayOrder.Where(codes.Contains).ToList();
//        }
//
//        /// <summary>
//        /// Languages sent in the live data LineConf (LANG, or LAN as read by TrainStatusProcessor).
//        /// </summary>
//        private static List<string> GetLineConfLanguages(JsonArray? lineConf)
//        {
//            if (lineConf == null || lineConf.Count == 0 || lineConf[0] is not JsonObject config)
//                return new List<string>();
//
//            List<string> languages = ParseLanguageCodes(DisplayHelper.GetString(config, "LANG"));
//            if (languages.Count == 0)
//                languages = ParseLanguageCodes(DisplayHelper.GetString(config, "LAN"));
//
//            return languages;
//        }
//
//        /// <summary>
//        /// Languages to show on a board: the board's own configured languages,
//        /// otherwise the LineConf languages, otherwise English.
//        /// </summary>
//        private List<string> GetBoardLanguages(string boardIp, List<string> lineConfLanguages)
//        {
//            string ip = boardIp?.Trim() ?? string.Empty;
//
//            if (_boardLanguages.TryGetValue(ip, out List<string> configured) && configured.Count > 0)
//                return configured;
//
//            if (lineConfLanguages != null && lineConfLanguages.Count > 0)
//                return lineConfLanguages;
//
//            return new List<string> { LANG_ENGLISH };
//        }
//
//        /// <summary>
//        /// The live data LineConf carries the languages in "LANG", but TrainStatusProcessor reads "LAN".
//        /// Sets "LAN" (English + every language used by the boards) on the local live-data copy so the
//        /// processor builds the Hindi / Regional status texts too. English is always included because the
//        /// status logic (cancelled / terminated / diverted / rescheduled) works on the English text.
//        /// </summary>
//        private void PrepareLanguagesForProcessing(JsonObject liveData)
//        {
//            JsonArray? lineConf = liveData["LineConf"] as JsonArray;
//
//            var codes = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { LANG_ENGLISH };
//            foreach (string code in GetLineConfLanguages(lineConf))
//                codes.Add(code);
//            foreach (List<string> boardLanguages in _boardLanguages.Values)
//                foreach (string code in boardLanguages)
//                    codes.Add(code);
//
//            string lan = string.Join("#", LanguageDisplayOrder.Where(codes.Contains));
//
//            if (lineConf == null)
//            {
//                lineConf = new JsonArray();
//                liveData["LineConf"] = lineConf;
//            }
//
//            if (lineConf.Count == 0)
//                lineConf.Add(new JsonObject());
//
//            if (lineConf[0] is JsonObject config)
//                config["LAN"] = lan;
//        }
//
//        /// <summary>
//        /// Train name in the requested language only (TNE / TNH / TNR).
//        /// If that language's name is empty, the name stays empty (no fallback to English).
//        /// </summary>
//        private static string GetTrainNameForLanguage(TrainDisplayData train, string language)
//        {
//            if (train?.Names == null)
//                return string.Empty;
//
//            return train.Names.TryGetValue(language, out string name) && !string.IsNullOrWhiteSpace(name)
//                ? name.Trim()
//                : string.Empty;
//        }
//
//        /// <summary>
//        /// Special-status text (flip 0 / flip 1) in the requested language, as prepared by TrainStatusProcessor.
//        /// Empty for English (the English line keeps its own labels) and for normal statuses.
//        /// </summary>
//        private static string GetLanguageStatus(JsonObject train, string language, int flipIndex)
//        {
//            if (string.Equals(language, LANG_ENGLISH, StringComparison.OrdinalIgnoreCase))
//                return string.Empty;
//
//            if (train?["STA"] is not JsonObject)
//                return string.Empty;
//
//            return DisplayHelper.GetTrainStatus(train, language, flipIndex);
//        }
//
//        /// <summary>
//        /// Cuts text to at most <paramref name="maxLength"/> characters without splitting a
//        /// Hindi / Indic letter from its vowel signs (plain English text is cut exactly like Substring).
//        /// </summary>
//        private static string TruncateText(string text, int maxLength)
//        {
//            if (string.IsNullOrEmpty(text) || text.Length <= maxLength)
//                return text ?? string.Empty;
//
//            var result = new StringBuilder();
//            TextElementEnumerator elements = StringInfo.GetTextElementEnumerator(text);
//            while (elements.MoveNext())
//            {
//                string element = elements.GetTextElement();
//                if (result.Length + element.Length > maxLength)
//                    break;
//                result.Append(element);
//            }
//            return result.ToString();
//        }
//
//        private static bool ContainsNonAsciiText(string text)
//        {
//            return !string.IsNullOrEmpty(text) && text.Any(c => c > 127);
//        }
//
//        /// <summary>
//        /// Draws one board line in three columns (train no | train name | time, A/D, PF or status),
//        /// placed on the Consolas character grid so the columns line up on every line.
//        /// Each column uses Consolas for English text and the Indic font for Hindi / Regional text.
//        /// </summary>
//        private static void DrawLineByColumns(Graphics g, string lineText, Font asciiFont, Font indicFont, Brush brush,
//            float xStart, float lineTop, float lineHeight, float maxWidth)
//        {
//            using (var format = (StringFormat)StringFormat.GenericTypographic.Clone())
//            {
//                float charWidth = g.MeasureString("0000000000", asciiFont, PointF.Empty, format).Width / 10f;
//
//                int nameColumn = W_TrainNo + 1;
//                int restColumn = nameColumn + W_TrainName;
//
//                string trainNoPart = lineText.Length > 0 ? lineText.Substring(0, Math.Min(nameColumn, lineText.Length)) : string.Empty;
//                string namePart = lineText.Length > nameColumn ? lineText.Substring(nameColumn, Math.Min(W_TrainName, lineText.Length - nameColumn)) : string.Empty;
//                string restPart = lineText.Length > restColumn ? lineText.Substring(restColumn) : string.Empty;
//
//                DrawColumn(trainNoPart, 0, 0);
//                DrawColumn(namePart, nameColumn, W_TrainName);
//                DrawColumn(restPart, restColumn, 0);
//
//                void DrawColumn(string text, int column, int clipColumns)
//                {
//                    if (string.IsNullOrWhiteSpace(text))
//                        return;
//
//                    // Leading spaces become column offset (typographic drawing does not advance for them reliably)
//                    int leadingSpaces = text.Length - text.TrimStart(' ').Length;
//                    string value = text.Trim();
//                    int startColumn = column + leadingSpaces;
//                    int clipWidthColumns = clipColumns > 0 ? clipColumns - leadingSpaces : 0;
//
//                    Font font = ContainsNonAsciiText(value) ? indicFont : asciiFont;
//                    float x = xStart + startColumn * charWidth;
//                    SizeF size = g.MeasureString(value, font, PointF.Empty, format);
//                    float y = lineTop + (lineHeight - size.Height) / 2f;
//
//                    float clipWidth = clipWidthColumns > 0 ? clipWidthColumns * charWidth : maxWidth - x;
//                    if (clipWidth <= 0)
//                        return;
//
//                    g.SetClip(new RectangleF(x, lineTop, clipWidth, lineHeight));
//                    g.DrawString(value, font, brush, x, y, format);
//                    g.ResetClip();
//                }
//            }
//        }
//    }
//
//    public class MultiLineBoardPreview : Panel
//    {
//        private bool[,] ledMatrix;
//        private int rows = 96;
//        private int cols = 672;
//
//        public Color OnColor { get; set; } = Color.FromArgb(76, 175, 80);
//        public Color OffColor { get; set; } = Color.FromArgb(60, 60, 60);
//        public Color FaultColor { get; set; } = Color.FromArgb(244, 67, 54);
//
//        public MultiLineBoardPreview()
//        {
//            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
//            DoubleBuffered = true;
//            ResizeRedraw = true;
//            InitializeMatrix(96, 672);
//        }
//
//        public void InitializeMatrix(int rows, int cols)
//        {
//            this.rows = rows;
//            this.cols = cols;
//            this.ledMatrix = new bool[rows, cols];
//            this.Invalidate();
//        }
//
//        public void SetLEDMatrix(bool[,] matrix)
//        {
//            if (matrix.GetLength(0) == rows && matrix.GetLength(1) == cols)
//            {
//                ledMatrix = (bool[,])matrix.Clone();
//                Invalidate();
//            }
//        }
//
//        protected override void OnPaint(PaintEventArgs e)
//        {
//            base.OnPaint(e);
//            Graphics g = e.Graphics;
//            g.SmoothingMode = SmoothingMode.AntiAlias;
//
//            using (var bgBrush = new SolidBrush(Color.FromArgb(15, 15, 20)))
//            {
//                g.FillRectangle(bgBrush, ClientRectangle);
//            }
//
//            float pixelW = (float)Width / cols;
//            float pixelH = (float)Height / rows;
//
//            float size = Math.Min(pixelW, pixelH) * 0.85f;
//            if (size < 1.5f) size = 1.5f;
//
//            for (int row = 0; row < rows; row++)
//            {
//                if (row > 0 && row % 32 == 0)
//                {
//                    using (Pen dividerPen = new Pen(Color.FromArgb(30, 100, 100, 120), 1.5f))
//                    {
//                        float dividerY = row * pixelH;
//                        g.DrawLine(dividerPen, 0, dividerY, Width, dividerY);
//                    }
//                }
//
//                for (int col = 0; col < cols; col++)
//                {
//                    float x = col * pixelW + (pixelW - size) / 2f;
//                    float y = row * pixelH + (pixelH - size) / 2f;
//
//                    bool isActive = ledMatrix[row, col];
//
//                    if (isActive)
//                    {
//                        using (var glowBrush = new SolidBrush(Color.FromArgb(60, OnColor)))
//                        {
//                            float glowSize = size * 1.8f;
//                            g.FillEllipse(glowBrush, x - (glowSize - size) / 2f, y - (glowSize - size) / 2f, glowSize, glowSize);
//                        }
//                        using (var ledBrush = new SolidBrush(OnColor))
//                        {
//                            g.FillEllipse(ledBrush, x, y, size, size);
//                        }
//                    }
//                    else
//                    {
//                        using (var ledBrush = new SolidBrush(OffColor))
//                        {
//                            g.FillEllipse(ledBrush, x, y, size, size);
//                        }
//                    }
//                }
//            }
//        }
//
//        protected override void OnPaintBackground(PaintEventArgs e)
//        {
//        }
//    }
//
//}

using DataBaseHandler;
using DataModel;
using IPISv2.HealthStatus;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPISv2.HealthStatusReport
{
    public partial class MultilineDisplayBoard : UserControl
    {
        private List<SingleLineDisplayBoardData> _allBoardData = new List<SingleLineDisplayBoardData>();
        // Every active train row from the DB (not only rows matched to an MLDB board) - same list SLDB
        // uses to read ACTUAL_ARRIVAL_TIME / ACTUAL_DEPARTURE_TIME for the trains in the live feed.
        private List<SingleLineDisplayBoardData> _allActiveTrains = new List<SingleLineDisplayBoardData>();
        // MLDB board IP -> BOARD_ID from BOARDSETTING. Every train attached to a board (by BOARD_ID or by IP)
        // is drawn on that one board, even when the train row carries another BoardId (e.g. platform 2 train
        // on a platform-0 board), so it no longer opens a separate board preview.
        private Dictionary<string, int> _mldbBoardIdByIp = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        // MLDB board IP -> PF_NO from BOARDSETTING (0 = all platforms). Used for the live-data request and the
        // title, instead of the first train's platform.
        private Dictionary<string, string> _mldbPlatformByIp = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private System.Windows.Forms.Timer _refreshTimer;
        private int _calculatedHeight = 450;

        // Shared column widths optimized for 672px LED matrix width (total ~46 chars)
        private const int W_TrainNo = 6;
        private const int W_TrainName = 14;
        private const int W_SchArr = 5;
        private const int W_Arrival = 5;
        private const int W_ExpTime = 5;
        private const int W_SchDep = 5;
        private const int W_Departure = 5;
        private const int W_AdFlag = 2;
        private const int W_Platform = 4;
        private const int W_Status = 12;

        private class BoardRowControls
        {
            public int BoardId { get; set; }
            public List<SingleLineDisplayBoardData> Trains { get; set; }
            public MaterialSkin.Controls.MaterialLabel TitleLabel { get; set; }
            public Panel ScrollPanel { get; set; }
            public MultiLineBoardPreview BoardPreview { get; set; }
            public int BoardLines { get; set; } = 3;
        }

        private readonly List<BoardRowControls> _dynamicRows = new List<BoardRowControls>();
        private readonly MemberDomain.TrainStatusProcessor _statusProcessor = new MemberDomain.TrainStatusProcessor();
        private bool _isProcessingLiveData = false;
        // True while the live PTO rotation is drawing the boards. The DB reload at the start of the
        // next tick (LoadActiveBoardData -> UpdateDynamicUI) then does not redraw them, so the board
        // does not flash the DB line (times / platform) for a moment between cycles.
        private bool _liveRotationOwnsBoards = false;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        // =========================================================
        // LANGUAGES (E = English, H = Hindi, R = Regional)
        // =========================================================

        private const string LANG_ENGLISH = "E";
        private const string LANG_HINDI = "H";
        private const string LANG_REGIONAL = "R";

        // Display order on the board: English -> Hindi -> Regional
        private static readonly string[] LanguageDisplayOrder = { LANG_ENGLISH, LANG_HINDI, LANG_REGIONAL };

        // Font used for Hindi / Regional text (Consolas has no Devanagari / Indic glyphs)
        private const string IndicFontFamily = "Nirmala UI";

        // Languages configured per board (key = board IP), filled in LoadActiveBoardData
        private Dictionary<string, List<string>> _boardLanguages =
            new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

        public MultilineDisplayBoard()
        {
            InitializeComponent();
            if (IsInDesignMode()) return;

            // Hide default controls
            if (lblTitle != null) lblTitle.Visible = false;
            if (panelPreviewScroll != null) panelPreviewScroll.Visible = false;

            _refreshTimer = new System.Windows.Forms.Timer();
            // ✅ FIX #1: Use a short poll interval (2s). The render loop itself
            // sleeps for PTO inside — the guard below prevents overlap.
            _refreshTimer.Interval = 10000;
            _refreshTimer.Tick += RefreshTimer_Tick;

            LoadActiveBoardData();

            if (!_refreshTimer.Enabled)
                _refreshTimer.Start();

            this.Disposed += (s, e) =>
            {
                _refreshTimer?.Stop();
                _refreshTimer?.Dispose();
                _cts?.Cancel();
                _cts?.Dispose();
            };
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (this.Height != _calculatedHeight)
            {
                this.Height = _calculatedHeight;
            }
        }

        private async void RefreshTimer_Tick(object sender, EventArgs e)
        {
            await ProcessAndRenderLiveDataAsync();
        }

        /// <summary>
        /// Checks whether the given board IP responds to a ping.
        /// Used to gate preview rendering: preview is only shown when the board pings successfully.
        /// </summary>
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

        public async Task ProcessAndRenderLiveDataAsync()
        {
            // ✅ Guard against overlapping ticks (render loop can take longer than the 2s interval).
            if (_isProcessingLiveData)
            {
                LogManager.LoggerService.Logger.Debug("MLDB tick skipped (previous cycle still running).");
                return;
            }
            _isProcessingLiveData = true;
            bool liveRenderedThisCycle = false;

            try
            {
                // ✅ FIX #2: Reload board settings + active trains from DB on every tick
                //           so that adding/removing trains or boards takes effect immediately.
                LoadActiveBoardData();

                LogManager.LoggerService.Logger.Information($"MLDB tick start. Boards in _dynamicRows={_dynamicRows.Count}");

                var distinctBoards = _allBoardData
                    .Where(b => b != null && !string.IsNullOrWhiteSpace(b.BoardIp))
                    .GroupBy(b => b.BoardIp.Trim())
                    .ToList();

                var deviceList = new List<DeviceInfo>();
                var displayList = new List<DeviceInfo>();

                foreach (var bg in distinctBoards)
                {
                    string ip = bg.Key;
                    var first = bg.First();
                    //string pf = !string.IsNullOrWhiteSpace(first.PlatformNumber) && first.PlatformNumber != "0"
                    //    ? first.PlatformNumber
                    //    : "0";
                    // Platform from BOARDSETTING (PF_NO), not the first train's platform: a board set to
                    // platform 0 must ask for every platform's trains, not only the first train's platform.
                    string firstTrainPf = !string.IsNullOrWhiteSpace(first.PlatformNumber) && first.PlatformNumber != "0"
                        ? first.PlatformNumber
                        : "0";
                    string pf = GetMldbBoardPlatform(ip, firstTrainPf);

                    deviceList.Add(new DeviceInfo
                    {
                        Vendor = "VARDAAN",
                        Device = "MLDB",
                        IP = ip,
                        PF = pf,
                        INT = "50",
                        HSR = "ALL OK"
                    });

                    displayList.Add(new DeviceInfo
                    {
                        Device = "MLDB",
                        IP = ip,
                        INT = "50",
                        HSR = "ALL OK"
                    });
                }

                if (deviceList.Count == 0)
                {
                    deviceList.Add(new DeviceInfo { Vendor = "VARDAAN", Device = "MLDB", IP = "10.0.0.54", PF = "0", INT = "50", HSR = "ALL OK" });
                    displayList.Add(new DeviceInfo { Device = "MLDB", IP = "10.0.0.54", INT = "50", HSR = "ALL OK" });
                }

                var request = new LiveDataRequest
                {
                    Device = deviceList,
                    DisplayList = displayList
                };
                string rawJson = JsonSerializer.Serialize(request);

                var liveData = await _dbService.GetLiveDataDisplayAsyncForMLDB(request, rawJson);

                if (liveData == null || liveData.TrainList == null || liveData.TrainList.Count == 0)
                {
                    liveData = BuildFallbackLiveDataFromActiveBoards();
                }

                JsonObject? jsonObject = JsonSerializer.SerializeToNode(liveData) as JsonObject;
                if (jsonObject == null) return;

                // Make TrainStatusProcessor build the status texts for every configured language (E / H / R).
                PrepareLanguagesForProcessing(jsonObject);

                JsonObject processedOutput = _statusProcessor.Process(jsonObject);
                liveRenderedThisCycle = true;
                _liveRotationOwnsBoards = true;
                await UpdateBoardsWithLiveDataAsync(processedOutput, _cts.Token);
            }
            catch (OperationCanceledException)
            {
                // normal on dispose
            }
            catch (Exception ex)
            {
                LogManager.LoggerService.Logger.Error(ex, "Error processing MLDB live data.");
            }
            finally
            {
                // Live data drew the boards this cycle -> keep the DB reload from redrawing them.
                // No live data (or failure) -> the DB render in UpdateDynamicUI is shown again.
                _liveRotationOwnsBoards = liveRenderedThisCycle;
                _isProcessingLiveData = false;
            }
        }

        private async Task UpdateBoardsWithLiveDataAsync(JsonObject data, CancellationToken token)
        {
            JsonArray? trains = data["TrainList"] as JsonArray;
            JsonArray? lineConf = data["LineConf"] as JsonArray;

            if (trains == null || trains.Count == 0 || _dynamicRows.Count == 0)
            {
                UpdateBoardWithNoTrains();
                return;
            }

            // Languages sent in LineConf — used for boards that have no language configured.
            List<string> lineConfLanguages = GetLineConfLanguages(lineConf);

            int boardDelay = _allBoardData.FirstOrDefault()?.DelayTime ?? 10;
            double pto = DisplayHelper.GetPto(lineConf, boardDelay > 0 ? boardDelay : 10);
            if (pto <= 0) pto = boardDelay > 0 ? boardDelay : 10.0;

            for (int r = 0; r < _dynamicRows.Count; r++)
            {
                token.ThrowIfCancellationRequested();

                var row = _dynamicRows[r];
                if (row == null || row.BoardPreview == null || row.BoardPreview.IsDisposed) continue;

                // Number of lines comes from the board setting (BOARD_LINE)
                int lines = row.BoardLines > 0 ? row.BoardLines : 3;
                List<List<JsonObject?>> pages = DisplayPageHelper.CreatePages(trains, lines);

                if (pages.Count == 0) continue;

                // Languages configured on this board, in display order: E, or E + H, or E + H + R
                List<string> boardLanguages = GetBoardLanguages(row.Trains?.FirstOrDefault()?.BoardIp, lineConfLanguages);

                // PTO is shared by the configured languages: 2 languages -> PTO / 2 each, 3 languages -> PTO / 3 each.
                // A flipping status (Terminated At / Diverted / Change of Source / Rescheduled) flips at half of its language slot.
                double languageSlotSeconds = pto / Math.Max(1, boardLanguages.Count);

                for (int pageIndex = 0; pageIndex < pages.Count; pageIndex++)
                {
                    List<JsonObject?> page = pages[pageIndex];

                    // Every page is shown once per configured language, each language for the full PTO time.
                    // (now: each language for its share of the PTO - see languageSlotSeconds)
                    foreach (string language in boardLanguages)
                    {
                        token.ThrowIfCancellationRequested();

                        // Position of this language on the board; picks the matching part of a '#'-packed STA.
                        int languageIndex = boardLanguages.IndexOf(language);
                        if (languageIndex < 0) languageIndex = 0;

                        string[] frame0Lines = new string[lines];
                        string[] frame1Lines = new string[lines];
                        bool pageHasFlip = false;

                        for (int i = 0; i < lines; i++)
                        {
                            JsonObject? tObj = (i < page.Count) ? page[i] : null;
                            if (tObj != null)
                            {
                                // Status logic (cancelled / terminated / diverted / rescheduled / flip) always uses the English data.
                                TrainDisplayData f0 = TrainDisplayHelper.BuildTrainData(tObj, LANG_ENGLISH, 0);
                                string f0LanguageStatus = GetLanguageStatus(tObj, language, 0);
                                //frame0Lines[i] = FormatTrainLineWithStatus(f0, language, 0, f0.Status, f0LanguageStatus);
                                frame0Lines[i] = FormatTrainLineWithStatus(f0, language, 0, f0.Status, f0LanguageStatus, languageIndex);

                                //if (f0.FlipRequired || f0.SpecialStatus)
                                //{
                                //    pageHasFlip = true;
                                //    TrainDisplayData f1 = TrainDisplayHelper.BuildTrainData(tObj, LANG_ENGLISH, 1);
                                //    string f1LanguageStatus = GetLanguageStatus(tObj, language, 1);
                                //    frame1Lines[i] = FormatTrainLineWithStatus(f1, language, 1, f0.Status, f1LanguageStatus);
                                //}
                                //else
                                //{
                                //    frame1Lines[i] = frame0Lines[i];
                                //}

                                // Classify on the English part of the status ('#'-packed STA is split first).
                                string f0EnglishStatus = DisplayTextFormatter.SelectLanguagePart(f0.Status, 0);
                                bool lineCancelled = DisplayTextFormatter.IsCancelledStatus(f0EnglishStatus);
                                bool lineTwoPart = !lineCancelled && DisplayTextFormatter.IsTwoPartStatus(f0EnglishStatus);
                                bool lineRescheduled = !lineCancelled && !lineTwoPart && DisplayTextFormatter.IsRescheduledStatus(f0EnglishStatus);
                                bool lineTextOnly = lineCancelled ||
                                    (!lineTwoPart && !lineRescheduled && DisplayTextFormatter.IsSpecialStatusDetail(f0EnglishStatus));

                                if (lineTextOnly)
                                {
                                    // Cancelled / Indefinite Late / Has Arrived: status text for the whole language slot.
                                    // Never flips, so times and platform are never shown - even if the feed sets a flip.
                                    frame1Lines[i] = frame0Lines[i];
                                }
                                else if (lineTwoPart || lineRescheduled || f0.FlipRequired || f0.SpecialStatus)
                                {
                                    // Terminated At / Diverted / Change of Source: prefix -> station (never times).
                                    // Rescheduled: status -> times + platform. Other feed flips keep the old behaviour.
                                    TrainDisplayData f1 = TrainDisplayHelper.BuildTrainData(tObj, LANG_ENGLISH, 1);
                                    string f1LanguageStatus = GetLanguageStatus(tObj, language, 1);
                                    string f0LanguageText = DisplayTextFormatter.SelectLanguagePart(
                                        !string.IsNullOrEmpty(f0LanguageStatus) ? f0LanguageStatus : f0.Status, languageIndex);
                                    frame1Lines[i] = FormatTrainLineWithStatus(f1, language, 1, f0.Status, f1LanguageStatus, languageIndex, f0LanguageText);

                                    // Only flip when the second half actually shows something different.
                                    if (!string.Equals(frame1Lines[i], frame0Lines[i], StringComparison.Ordinal))
                                        pageHasFlip = true;
                                }
                                else
                                {
                                    frame1Lines[i] = frame0Lines[i];
                                }
                            }
                            else
                            {
                                frame0Lines[i] = FormatEmptyTrainLine();
                                frame1Lines[i] = FormatEmptyTrainLine();
                            }
                        }

                        int fontSize = row.Trains?.FirstOrDefault()?.FontSize ?? 13;

                        // Only show the preview if the board's IP responds to a ping first.
                        bool boardPingedF0 = IsHostReachable(row.Trains?.FirstOrDefault()?.BoardIp);

                        // Step 1: Render Frame 0
                        if (this.InvokeRequired)
                        {
                            this.Invoke(new Action(() => {
                                if (boardPingedF0)
                                    RenderBoardFrameLines(row.BoardPreview, frame0Lines, lines, fontSize);
                            }));
                        }
                        else
                        {
                            if (boardPingedF0)
                                RenderBoardFrameLines(row.BoardPreview, frame0Lines, lines, fontSize);
                        }

                        if (pageHasFlip)
                        {
                            // Wait first half of PTO (5 seconds for 10s PTO)
                            //await DisplayHelper.DelaySeconds(pto / 2.0, token);
                            // Wait first half of this language's slot
                            await DisplayHelper.DelaySeconds(languageSlotSeconds / 2.0, token);

                            // Only show the preview if the board's IP responds to a ping first.
                            bool boardPingedF1 = IsHostReachable(row.Trains?.FirstOrDefault()?.BoardIp);

                            // Render Frame 1
                            if (this.InvokeRequired)
                            {
                                this.Invoke(new Action(() => {
                                    if (boardPingedF1)
                                        RenderBoardFrameLines(row.BoardPreview, frame1Lines, lines, fontSize);
                                }));
                            }
                            else
                            {
                                if (boardPingedF1)
                                    RenderBoardFrameLines(row.BoardPreview, frame1Lines, lines, fontSize);
                            }

                            // Wait second half of PTO (5 seconds for 10s PTO)
                            //await DisplayHelper.DelaySeconds(pto / 2.0, token);
                            // Wait second half of this language's slot
                            await DisplayHelper.DelaySeconds(languageSlotSeconds / 2.0, token);
                        }
                        else
                        {
                            // Wait full PTO (10 seconds)
                            //await DisplayHelper.DelaySeconds(pto, token);
                            // Wait this language's full slot
                            await DisplayHelper.DelaySeconds(languageSlotSeconds, token);
                        }
                    }
                }
            }
        }

        private LiveDataResponse BuildFallbackLiveDataFromActiveBoards()
        {
            var trains = new List<TrainDto>();
            if (_allBoardData != null && _allBoardData.Count > 0)
            {
                foreach (var b in _allBoardData)
                {
                    if (string.IsNullOrEmpty(b.TrainNumber)) continue;
                    trains.Add(new TrainDto
                    {
                        TNO = b.TrainNumber ?? "",
                        TNE = b.TrainNameEnglish ?? "",
                        TNH = b.TrainNameHindi ?? "",
                        TNR = "", // no regional name in the board data -> regional name stays empty

                        ADF = b.ADFlag ?? "",
                        SAT = b.ScheduleArrivalTime ?? "",
                        SDT = b.ScheduleDepartureTime ?? "",
                        ETA = b.ActualArrivalTime ?? "",
                        EDT = b.ActualDepartureTime ?? "",
                        LATE = b.Late ?? "",
                        PNO = b.PlatformNumber ?? "",
                        STA = !string.IsNullOrEmpty(b.TrainStatusEnglish) ? b.TrainStatusEnglish : "RIGHT TIME",
                        CCD = b.Coach_Sequence ?? "",
                        COL = "WHITE"
                    });
                }
            }

            return new LiveDataResponse
            {
                TrainList = trains,
                LineConf = new List<LineConfDto> { new LineConfDto { INT = 5, PTO = 10, DTO = 5, CHR = 1, EFF = 1, SPD = 1, LANG = "E#H" } }
            };
        }

        private void UpdateBoardWithNoTrains()
        {
            foreach (var row in _dynamicRows)
            {
                if (row == null || row.BoardPreview == null || row.BoardPreview.IsDisposed) continue;

                int lines = row.BoardLines > 0 ? row.BoardLines : 3;
                string[] emptyLines = new string[lines];
                for (int i = 0; i < lines; i++) emptyLines[i] = FormatEmptyTrainLine();

                // Only show the preview if the board's IP responds to a ping first.
                bool boardPingedNoTrains = IsHostReachable(row.Trains?.FirstOrDefault()?.BoardIp);

                if (boardPingedNoTrains)
                    RenderBoardFrameLines(row.BoardPreview, emptyLines, lines, 13);
            }
        }

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

            // _allBoardData only holds trains matched to an MLDB board, so a train in the MLDB live feed
            // that is linked to another board was not found here and fell back to the feed time
            // (ETA, else SAT = 21:10). Look in every active train row from the DB first, like SLDB,
            // and take the first row of that train that actually has the time.
            string key = (trainNo ?? string.Empty).Trim();

            string dbValue = FindActualTime(_allActiveTrains, key, arrival);
            if (string.IsNullOrWhiteSpace(dbValue))
                dbValue = FindActualTime(_allBoardData, key, arrival);

            return !string.IsNullOrWhiteSpace(dbValue) ? dbValue : feedValue;
        }

        private static string FindActualTime(List<SingleLineDisplayBoardData> rows, string trainNo, bool arrival)
        {
            if (rows == null || string.IsNullOrEmpty(trainNo)) return null;

            return rows
                .Where(x => x != null && string.Equals((x.TrainNumber ?? string.Empty).Trim(), trainNo, StringComparison.OrdinalIgnoreCase))
                .Select(x => arrival ? x.ActualArrivalTime : x.ActualDepartureTime)
                .FirstOrDefault(v => !string.IsNullOrWhiteSpace(v));
        }

        private static string GetExpectedDisplayTime(string actualTime, string scheduledTime, string late)
        {
            string formattedActual = DisplayTextFormatter.FormatTimeOnly(actualTime);
            if (!string.IsNullOrEmpty(formattedActual))
            {
                return formattedActual;
            }

            string formattedSched = DisplayTextFormatter.FormatTimeOnly(scheduledTime);
            if (string.IsNullOrEmpty(formattedSched))
            {
                return "--:--";
            }

            if (!string.IsNullOrWhiteSpace(late) && late != "00:00" && late != "0" && late != "00:00:00")
            {
                if (TimeSpan.TryParse(late, out TimeSpan lateSpan) && lateSpan != TimeSpan.Zero)
                {
                    if (TimeSpan.TryParse(formattedSched, out TimeSpan schedSpan))
                    {
                        TimeSpan total = schedSpan.Add(lateSpan);
                        int hours = ((int)total.TotalHours) % 24;
                        if (hours < 0) hours += 24;
                        return $"{hours:D2}:{total.Minutes:D2}";
                    }
                }
            }

            return formattedSched;
        }

        private static string DetermineExpectedTime(
            string actualArrival, string schedArrival,
            string actualDeparture, string schedDeparture,
            string late, string adFlag, string status)
        {
            bool isDeparture = false;
            if (!string.IsNullOrEmpty(adFlag))
            {
                isDeparture = adFlag.Trim().Equals("D", StringComparison.OrdinalIgnoreCase);
            }
            else if (!string.IsNullOrEmpty(status))
            {
                string u = status.ToUpperInvariant();
                if (u.StartsWith("D") || u.Contains("DEP"))
                    isDeparture = true;
            }

            // If departing, but no scheduled departure is present, fall back to arrival
            if (isDeparture && string.IsNullOrWhiteSpace(schedDeparture) && string.IsNullOrWhiteSpace(actualDeparture))
            {
                isDeparture = false;
            }
            // If arriving, but no scheduled arrival is present, fall back to departure
            if (!isDeparture && string.IsNullOrWhiteSpace(schedArrival) && string.IsNullOrWhiteSpace(actualArrival))
            {
                isDeparture = true;
            }

            if (isDeparture)
            {
                return GetExpectedDisplayTime(actualDeparture, schedDeparture, late);
            }
            else
            {
                return GetExpectedDisplayTime(actualArrival, schedArrival, late);
            }
        }

        /// <summary>
        /// Formats one board line.
        /// <paramref name="train"/> is the English data (used for all status logic),
        /// <paramref name="language"/> selects the train name (E / H / R — empty when that name is missing),
        /// <paramref name="languageStatus"/> is the status text in that language (empty for English),
        /// used instead of the English status label when available.
        /// </summary>
        //private string FormatTrainLineWithStatus(TrainDisplayData train, string language, int flipIndex = 0, string primaryStatus = "", string languageStatus = "")
        // languageIndex: position of the language on the board (picks the part of a '#'-packed STA).
        // frame0Text   : the frame-0 status text in this language (used by flip 1 of a two-part status).
        private string FormatTrainLineWithStatus(TrainDisplayData train, string language, int flipIndex = 0, string primaryStatus = "", string languageStatus = "",
                                                 int languageIndex = 0, string frame0Text = null)
        {
            if (train == null || string.IsNullOrEmpty(train.TrainNo))
            {
                return FormatEmptyTrainLine();
            }

            string trainNo = DisplayTextFormatter.SanitizeForDisplay(train.TrainNo);
            if (trainNo.Length > W_TrainNo) trainNo = trainNo.Substring(0, W_TrainNo);

            // Train name only in the current language — empty when not available (no fallback to English).
            string trainName = TruncateText(DisplayTextFormatter.SanitizeForDisplay(GetTrainNameForLanguage(train, language)), W_TrainName);

            // Status text in the current language (Hindi / Regional), if the live data provides it.
            string localizedStatus = TruncateText(DisplayTextFormatter.SanitizeForDisplay(languageStatus), 20);
            bool hasLocalizedStatus = !string.IsNullOrEmpty(localizedStatus);

            string statusText = DisplayTextFormatter.SanitizeForDisplay(train.Status);
            string effectiveStatus = (!string.IsNullOrEmpty(primaryStatus) ? primaryStatus : statusText).ToUpperInvariant();

            bool isCancelled = DisplayTextFormatter.IsCancelledStatus(train.Status) || effectiveStatus.Contains("CANCEL");
            bool isRescheduled = DisplayTextFormatter.IsRescheduledStatus(train.Status) || effectiveStatus.Contains("RESCHED");
            bool isTerminated = effectiveStatus.Contains("TERMINATED");
            bool isDiverted = effectiveStatus.Contains("DIVERTED");

            // ===== RDSO status handling (0x01 - 0x13) - same rules as SLDB =====
            // Classification uses the English part of the status; the text shown is the
            // per-language STA text, or the matching part of a '#'-packed STA.
            string rdsoEnglishStatus = DisplayTextFormatter.SelectLanguagePart(
                !string.IsNullOrEmpty(primaryStatus) ? primaryStatus : train.Status, 0);
            string rdsoLanguageText = DisplayTextFormatter.SelectLanguagePart(
                !string.IsNullOrEmpty(languageStatus) ? languageStatus : train.Status, languageIndex);

            bool rdsoCancelled = DisplayTextFormatter.IsCancelledStatus(rdsoEnglishStatus);
            bool rdsoTwoPart = !rdsoCancelled && DisplayTextFormatter.IsTwoPartStatus(rdsoEnglishStatus);
            bool rdsoRescheduled = !rdsoCancelled && !rdsoTwoPart && DisplayTextFormatter.IsRescheduledStatus(rdsoEnglishStatus);
            bool rdsoTextOnly = !rdsoCancelled && !rdsoTwoPart && !rdsoRescheduled && DisplayTextFormatter.IsSpecialStatusDetail(rdsoEnglishStatus);

            // Train No + Train Name + status text only (no times, no platform).
            string RdsoStatusLine(string text) => string.Format(
                "{0,-" + W_TrainNo + "} {1,-" + W_TrainName + "}  {2}",
                trainNo, trainName, TruncateText(DisplayTextFormatter.SanitizeForDisplay(text), 20));

            // 0x06 Cancelled: status text only, both halves.
            if (rdsoCancelled || isCancelled)
            {
                bool hasTranslatedCancel =
                    !string.Equals(language, LANG_ENGLISH, StringComparison.OrdinalIgnoreCase) &&
                    rdsoLanguageText.Length > 3 &&
                    !string.Equals(rdsoLanguageText, rdsoEnglishStatus, StringComparison.OrdinalIgnoreCase);
                return RdsoStatusLine(hasTranslatedCancel ? rdsoLanguageText : "CANCELLED");
            }

            // 0x08 Terminated At / 0x10 Diverted / 0x13 Change of Source - Start at:
            // first half = status, second half = station / route. Never times or platform.
            if (rdsoTwoPart)
            {
                string twoPartHead = DisplayTextFormatter.TrySplitTwoPartStatus(rdsoLanguageText, out string splitHead, out _)
                    ? splitHead
                    : (rdsoLanguageText.Length > 0 ? rdsoLanguageText : rdsoEnglishStatus);

                if (flipIndex == 0)
                    return RdsoStatusLine(twoPartHead);

                string twoPartFrame0 = !string.IsNullOrEmpty(frame0Text) ? frame0Text : rdsoLanguageText;
                if (string.IsNullOrEmpty(twoPartFrame0)) twoPartFrame0 = rdsoEnglishStatus;

                string twoPartSecond = DisplayTextFormatter.GetTwoPartSecondFrame(twoPartFrame0, rdsoLanguageText);
                if (twoPartSecond.Length > 0)
                    return RdsoStatusLine(twoPartSecond);

                // Nothing distinct for the second half (e.g. feed flip frame is a time) -> keep the status.
                return RdsoStatusLine(DisplayTextFormatter.TrySplitTwoPartStatus(twoPartFrame0, out string frame0Head, out _)
                    ? frame0Head
                    : twoPartFrame0);
            }

            // 0x04 Has Arrived / 0x07 Indefinite Late (and other text-only statuses): text only, no flip.
            if (rdsoTextOnly)
                return RdsoStatusLine(rdsoLanguageText.Length > 0 ? rdsoLanguageText : rdsoEnglishStatus);

            // 0x0F Rescheduled, first half: status text in this language. Second half falls through
            // to the RESCHEDULED block below (Sch Arr | Exp | Sch Dep | PF).
            if (rdsoRescheduled && flipIndex == 0)
                return RdsoStatusLine(rdsoLanguageText.Length > 3 ? rdsoLanguageText : "RESCHEDULED");

            // (blocks 1-3 below are now handled by the RDSO status handling above)
//            // 1. CANCELLED: Show only Train Number, Train Name, and CANCELLED
//            if (isCancelled)
//            {
//                return string.Format(
//                    "{0,-" + W_TrainNo + "} {1,-" + W_TrainName + "}  {2}",
//                    trainNo, trainName, hasLocalizedStatus ? localizedStatus : "CANCELLED");
//            }

//            // 2. TERMINATED AT: Show ONLY Train Number, Train Name, and Status/Station Name (NO time, NO A/D, NO Platform)
//            if (isTerminated)
//            {
//                if (flipIndex == 0)
//                {
//                    return string.Format(
//                        "{0,-" + W_TrainNo + "} {1,-" + W_TrainName + "}  {2}",
//                        trainNo, trainName, hasLocalizedStatus ? localizedStatus : "TERMINATED AT");
//                }
//                else
//                {
//                    string station = statusText;
//                    if (station.ToUpperInvariant().Contains("TERMINATED AT"))
//                    {
//                        station = station.Substring(station.ToUpperInvariant().IndexOf("TERMINATED AT") + "TERMINATED AT".Length).Trim();
//                        if (string.IsNullOrEmpty(station)) station = statusText;
//                    }
//                    if (station.Length > 20) station = station.Substring(0, 20);

//                    return string.Format(
//                        "{0,-" + W_TrainNo + "} {1,-" + W_TrainName + "}  {2}",
//                        trainNo, trainName, hasLocalizedStatus ? localizedStatus : station);
//                }
//            }

//            // 3. DIVERTED VIA: Show ONLY Train Number, Train Name, and Status/Route (NO time, NO A/D, NO Platform)
//            if (isDiverted)
//            {
//                if (flipIndex == 0)
//                {
//                    return string.Format(
//                        "{0,-" + W_TrainNo + "} {1,-" + W_TrainName + "}  {2}",
//                        trainNo, trainName, hasLocalizedStatus ? localizedStatus : "DIVERTED VIA");
//                }
//                else
//                {
//                    string route = statusText;
//                    if (route.ToUpperInvariant().Contains("DIVERTED VIA"))
//                    {
//                        route = route.Substring(route.ToUpperInvariant().IndexOf("DIVERTED VIA") + "DIVERTED VIA".Length).Trim();
//                        if (string.IsNullOrEmpty(route)) route = statusText;
//                    }
//                    if (route.Length > 20) route = route.Substring(0, 20);

//                    return string.Format(
//                        "{0,-" + W_TrainNo + "} {1,-" + W_TrainName + "}  {2}",
//                        trainNo, trainName, hasLocalizedStatus ? localizedStatus : route);
//                }
//            }

            // 4. RESCHEDULED:
            // Frame 0 (first half of PTO): Show Train Number, Train Name, and RESCHEDULED status
            // Frame 1 (second half of PTO): Show Train Number, Train Name, Expected Time, A/D flag, and Platform number
            if (isRescheduled)
            {
                if (flipIndex == 0)
                {
                    // First half of PTO: show RESCHEDULED status (in the current language when available)
                    return string.Format(
                        "{0,-" + W_TrainNo + "} {1,-" + W_TrainName + "}  {2}",
                        trainNo, trainName, hasLocalizedStatus ? localizedStatus : "RESCHEDULED");
                }
                else
                {
                    // Second half of PTO: show Scheduled Arrival, Expected Time, Scheduled Departure, and Platform number
                    string rescSchArr = DisplayTextFormatter.FormatTimeOnly(!string.IsNullOrEmpty(train.Sat) ? train.Sat : train.Eat);
                    if (string.IsNullOrEmpty(rescSchArr)) rescSchArr = "--:--";
                    if (rescSchArr.Length > W_SchArr) rescSchArr = rescSchArr.Substring(0, W_SchArr);

                    string rescSchDep = DisplayTextFormatter.FormatTimeOnly(!string.IsNullOrEmpty(train.Sdt) ? train.Sdt : train.Edt);
                    if (string.IsNullOrEmpty(rescSchDep)) rescSchDep = "--:--";
                    if (rescSchDep.Length > W_SchDep) rescSchDep = rescSchDep.Substring(0, W_SchDep);

                    string rescActualArr = ResolveActualTime(train.TrainNo, true, train.Eat);
                    string rescActualDep = ResolveActualTime(train.TrainNo, false, train.Edt);

                    string rescExpTime = DetermineExpectedTime(
                        rescActualArr, train.Sat,
                        rescActualDep, train.Sdt,
                        train.Late, train.Adf, train.Status);
                    if (string.IsNullOrEmpty(rescExpTime)) rescExpTime = "--:--";
                    if (rescExpTime.Length > W_ExpTime) rescExpTime = rescExpTime.Substring(0, W_ExpTime);

                    string rescPlatform = DisplayTextFormatter.SanitizeForDisplay(train.Platform);
                    if (rescPlatform.Length > W_Platform) rescPlatform = rescPlatform.Substring(0, W_Platform);

                    //return string.Format(
                    //    "{0,-" + W_TrainNo + "} {1,-" + W_TrainName + "} {2,-" + W_SchArr + "} {3,-" + W_ExpTime + "} {4,-" + W_SchDep + "} {5,-" + W_Platform + "}",
                    //    trainNo, trainName, rescSchArr, rescExpTime, rescSchDep, rescPlatform);

                    // Same as SLDB: Actual Arrival | Actual Departure | Platform.
                    // When the board has no actual time, the time in the feed's flip frame is used.
                    string rescFlipTime = DisplayTextFormatter.FormatTimeOnly(train.Status);
                    if (!TimeSpan.TryParse(rescFlipTime, out _)) rescFlipTime = string.Empty;

                    string rescArrShown = DisplayTextFormatter.FormatTimeOnly(rescActualArr);
                    if (string.IsNullOrEmpty(rescArrShown)) rescArrShown = rescFlipTime;
                    if (string.IsNullOrEmpty(rescArrShown)) rescArrShown = "--:--";
                    if (rescArrShown.Length > W_SchArr) rescArrShown = rescArrShown.Substring(0, W_SchArr);

                    string rescDepShown = DisplayTextFormatter.FormatTimeOnly(rescActualDep);
                    if (string.IsNullOrEmpty(rescDepShown)) rescDepShown = rescFlipTime;
                    if (string.IsNullOrEmpty(rescDepShown)) rescDepShown = "--:--";
                    if (rescDepShown.Length > W_SchDep) rescDepShown = rescDepShown.Substring(0, W_SchDep);

                    return string.Format(
                        "{0,-" + W_TrainNo + "} {1,-" + W_TrainName + "} {2,-" + W_SchArr + "} {3,-" + W_SchDep + "} {4,-" + W_Platform + "}",
                        trainNo, trainName, rescArrShown, rescDepShown, rescPlatform);
                }
            }

            // 5. NORMAL TRAIN LINE: Show TrainNo, TrainName, Scheduled Arrival, Expected Time, Scheduled Departure, Platform
            string schArr = DisplayTextFormatter.FormatTimeOnly(!string.IsNullOrEmpty(train.Sat) ? train.Sat : train.Eat);
            if (string.IsNullOrEmpty(schArr)) schArr = "--:--";
            if (schArr.Length > W_SchArr) schArr = schArr.Substring(0, W_SchArr);

            string schDep = DisplayTextFormatter.FormatTimeOnly(!string.IsNullOrEmpty(train.Sdt) ? train.Sdt : train.Edt);
            if (string.IsNullOrEmpty(schDep)) schDep = "--:--";
            if (schDep.Length > W_SchDep) schDep = schDep.Substring(0, W_SchDep);

            string actualArr = ResolveActualTime(train.TrainNo, true, train.Eat);
            string actualDep = ResolveActualTime(train.TrainNo, false, train.Edt); 

            string expTime = DetermineExpectedTime(
                actualArr, train.Sat,
                actualDep, train.Sdt,
                train.Late, train.Adf, train.Status);
            if (string.IsNullOrEmpty(expTime)) expTime = "--:--";
            if (expTime.Length > W_ExpTime) expTime = expTime.Substring(0, W_ExpTime);

            string pf = DisplayTextFormatter.SanitizeForDisplay(train.Platform);
            if (pf.Length > W_Platform) pf = pf.Substring(0, W_Platform);

            // For normal trains: TrainNo, TrainName, Scheduled Arrival, Expected Time, Scheduled Departure, Platform
            //return string.Format(
            //    "{0,-" + W_TrainNo + "} {1,-" + W_TrainName + "} {2,-" + W_SchArr + "} {3,-" + W_ExpTime + "} {4,-" + W_SchDep + "} {5,-" + W_Platform + "}",
            //    trainNo, trainName, schArr, expTime, schDep, pf);

            // Same as SLDB: TrainNo | TrainName | Actual Arrival | Actual Departure | Platform
            // (ACTUAL_ARRIVAL_TIME / ACTUAL_DEPARTURE_TIME from the DB, feed ETA/EDT only when the DB is empty).
            string arrShown = DisplayTextFormatter.FormatTimeOnly(actualArr);
            if (string.IsNullOrEmpty(arrShown)) arrShown = "--:--";
            if (arrShown.Length > W_SchArr) arrShown = arrShown.Substring(0, W_SchArr);

            string depShown = DisplayTextFormatter.FormatTimeOnly(actualDep);
            if (string.IsNullOrEmpty(depShown)) depShown = "--:--";
            if (depShown.Length > W_SchDep) depShown = depShown.Substring(0, W_SchDep);

            return string.Format(
                "{0,-" + W_TrainNo + "} {1,-" + W_TrainName + "} {2,-" + W_SchArr + "} {3,-" + W_SchDep + "} {4,-" + W_Platform + "}",
                trainNo, trainName, arrShown, depShown, pf);
        }

        private string FormatEmptyTrainLine()
        {
            //return string.Format(
            //    "{0,-" + W_TrainNo + "} {1,-" + W_TrainName + "} {2,-" + W_SchArr + "} {3,-" + W_ExpTime + "} {4,-" + W_SchDep + "} {5,-" + W_Platform + "}",
            //    "-", "-", "-", "-", "-", "-");
            return string.Format(
                "{0,-" + W_TrainNo + "} {1,-" + W_TrainName + "} {2,-" + W_SchArr + "} {3,-" + W_SchDep + "} {4,-" + W_Platform + "}",
                "-", "-", "-", "-", "-");
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (panelPreviewScroll != null)
            {
                panelPreviewScroll.Left = 10;
                panelPreviewScroll.Width = this.Width - 20;
            }
            if (lblTitle != null)
            {
                lblTitle.Left = 10;
            }
        }

        public void LoadActiveBoardData()
        {
            try
            {
                var allSettings = _dbService.GET_BOARD_SETTING(0) ?? new List<BoardSettingInfo>();
                var installedSettings = allSettings
                    .Where(b => b.IsActive == 1 &&
                                !string.IsNullOrEmpty(b.BOARD_NAME) &&
                                (b.BOARD_NAME.IndexOf("Multiline", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                 b.BOARD_NAME.IndexOf("MLDB", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                 b.BOARD_NAME.IndexOf("Multi Line", StringComparison.OrdinalIgnoreCase) >= 0))
                    .ToList();

                var activeTrains = _dbService.GetAllActiveBoards() ?? new List<SingleLineDisplayBoardData>();
                _allActiveTrains = activeTrains;

                var tempBoardList = new List<SingleLineDisplayBoardData>();
                var tempBoardLanguages = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

                foreach (var setting in installedSettings)
                {
                    // Languages configured on this board (e.g. "Hindi+English+Marathi" -> E, H, R)
                    string settingIp = setting.BOARD_IP?.Trim() ?? string.Empty;
                    if (!tempBoardLanguages.ContainsKey(settingIp))
                    {
                        tempBoardLanguages[settingIp] = ParseLanguageCodes(setting.LANGUAGES);
                    }

                    int delayTime = setting.DELAYTIME > 0 ? setting.DELAYTIME : 10;
                    var matches = activeTrains.Where(t =>
                        t.BoardId == setting.BOARD_ID ||
                        (string.Equals(t.BoardIp?.Trim(), setting.BOARD_IP?.Trim(), StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(setting.BOARD_IP)))
                        .ToList();

                    if (matches.Count > 0)
                    {
                        foreach (var match in matches)
                        {
                            match.BoardName = setting.BOARD_NAME;
                            match.BoardLocation = setting.BOARD_LOCATION;
                            match.BoardIp = setting.BOARD_IP;
                            match.FontSize = setting.FONT_SIZE;
                            match.DelayTime = delayTime;
                            match.BoardLine = setting.BOARD_LINE.ToString();
                            tempBoardList.Add(match);
                        }
                    }
                    else
                    {
                        tempBoardList.Add(new SingleLineDisplayBoardData
                        {
                            BoardId = setting.BOARD_ID,
                            BoardName = setting.BOARD_NAME,
                            BoardIp = setting.BOARD_IP,
                            BoardLocation = setting.BOARD_LOCATION,
                            BoardLine = setting.BOARD_LINE.ToString(),
                            IsActive = setting.IsActive == 1,
                            FontSize = setting.FONT_SIZE,
                            DelayTime = delayTime,
                            TrainNumber = string.Empty,
                            PlatformNumber = setting.PF_NO
                        });
                    }
                }

                var tempBoardIdByIp = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                var tempPlatformByIp = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                foreach (var setting in installedSettings)
                {
                    string settingIp = setting.BOARD_IP?.Trim() ?? string.Empty;
                    if (settingIp.Length > 0 && !tempBoardIdByIp.ContainsKey(settingIp))
                        tempBoardIdByIp[settingIp] = setting.BOARD_ID;
                    if (settingIp.Length > 0 && !tempPlatformByIp.ContainsKey(settingIp))
                        tempPlatformByIp[settingIp] = setting.PF_NO?.Trim() ?? string.Empty;
                }
                _mldbBoardIdByIp = tempBoardIdByIp;
                _mldbPlatformByIp = tempPlatformByIp;

                _allBoardData = tempBoardList;
                _boardLanguages = tempBoardLanguages;

                // ✅ FIX #3: Removed the block that was overriding _refreshTimer.Interval
                //           The timer is now permanently set to 2000 ms in the constructor.

                UpdateDynamicUI(tempBoardList);
            }
            catch (Exception ex)
            {
                LogManager.LoggerService.Logger.Error(ex, "Error loading MLDB data.");
                if (lblTitle != null) lblTitle.Text = "Multi Line Display Board (MLDB) - Error Loading Data";
                UpdateDynamicUI(new List<SingleLineDisplayBoardData>());
            }
        }

        private void UpdateDynamicUI(List<SingleLineDisplayBoardData> mldbBoards)
        {
            //var groups = mldbBoards
            //    .GroupBy(b => b.BoardId)
            //    .ToList();
            // One preview per MLDB board setting: group by the board the train was attached to,
            // not by the train row's own BoardId.
            var groups = mldbBoards
                .GroupBy(b => GetMldbBoardKey(b))
                .ToList();

            bool needsRecreate = false;

            if (groups.Count == 0)
            {
                if (_dynamicRows.Count != 1 || _dynamicRows[0].BoardId != 0)
                {
                    needsRecreate = true;
                }
            }
            else
            {
                if (_dynamicRows.Count != groups.Count)
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

                        // Number of lines changed in the board setting -> rebuild so the preview area fits the new line count
                        if (GetConfiguredLineCount(groups[i].First()) != _dynamicRows[i].BoardLines)
                        {
                            needsRecreate = true;
                            break;
                        }
                    }
                }
            }

            if (needsRecreate)
            {
                RecreateBoardControls(groups);
            }

            if (groups.Count == 0)
            {
                var row = _dynamicRows[0];
                if (row.TitleLabel != null) row.TitleLabel.Text = "Multi Line Display Board (MLDB) No Active Boards";
                ClearBoardPreview(row.BoardPreview, row.BoardLines);
            }
            else
            {
                for (int i = 0; i < groups.Count; i++)
                {
                    var group = groups[i];
                    var row = _dynamicRows[i];

                    // ✅ Update the train list for this board every tick so add/remove
                    //    of individual trains reflects immediately.
                    row.Trains = group.ToList();

                    var firstBoard = group.First();
                    row.BoardLines = GetConfiguredLineCount(firstBoard);

                    //string platformStr = !string.IsNullOrEmpty(firstBoard.PlatformNumber) ? firstBoard.PlatformNumber :
                    //                    (!string.IsNullOrEmpty(firstBoard.BoardLocation) ? firstBoard.BoardLocation : "N/A");
                    // Title shows the platform configured in BOARDSETTING (e.g. 0), not the first train's platform.
                    string oldPlatformStr = !string.IsNullOrEmpty(firstBoard.PlatformNumber) ? firstBoard.PlatformNumber :
                                        (!string.IsNullOrEmpty(firstBoard.BoardLocation) ? firstBoard.BoardLocation : "N/A");
                    string platformStr = GetMldbBoardPlatform(firstBoard.BoardIp, oldPlatformStr);
                    if (row.TitleLabel != null) row.TitleLabel.Text = $"Multi Line Display Board (MLDB) IP: {firstBoard.BoardIp} Platform: {platformStr}";

                    int fontSize = firstBoard.FontSize ?? 13;

                    // Only show the preview if the board's IP responds to a ping first.
                    bool boardPinged = IsHostReachable(firstBoard.BoardIp);

                    //if (boardPinged)
                    //    RenderBoardFrame(row.BoardPreview, row.Trains, row.BoardLines, fontSize);
                    // While the live rotation owns the board, don't redraw it from the DB rows (prevents the flash).
                    if (boardPinged && !_liveRotationOwnsBoards)
                        RenderBoardFrame(row.BoardPreview, row.Trains, row.BoardLines, fontSize);
                }
            }
        }

        /// <summary>
        /// BOARD_ID of the MLDB board setting this row belongs to (looked up by the board IP that
        /// LoadActiveBoardData put on the row); the row's own BoardId when the IP is not an MLDB board.
        /// </summary>
        private int GetMldbBoardKey(SingleLineDisplayBoardData row)
        {
            string ip = row?.BoardIp?.Trim() ?? string.Empty;
            if (ip.Length > 0 && _mldbBoardIdByIp != null && _mldbBoardIdByIp.TryGetValue(ip, out int boardId))
                return boardId;

            return row?.BoardId ?? 0;
        }

        /// <summary>
        /// PF_NO configured in BOARDSETTING for this MLDB board IP; <paramref name="fallback"/> when the
        /// board has no platform configured.
        /// </summary>
        private string GetMldbBoardPlatform(string boardIp, string fallback)
        {
            string ip = boardIp?.Trim() ?? string.Empty;
            if (ip.Length > 0 && _mldbPlatformByIp != null &&
                _mldbPlatformByIp.TryGetValue(ip, out string configuredPf) &&
                !string.IsNullOrWhiteSpace(configuredPf))
            {
                return configuredPf;
            }

            return fallback;
        }

        private void RecreateBoardControls(List<IGrouping<int, SingleLineDisplayBoardData>> groups)
        {
            foreach (var row in _dynamicRows)
            {
                if (row.TitleLabel != null)
                {
                    this.Controls.Remove(row.TitleLabel);
                    row.TitleLabel.Dispose();
                }
                if (row.BoardPreview != null)
                {
                    row.BoardPreview.Dispose();
                }
                if (row.ScrollPanel != null)
                {
                    this.Controls.Remove(row.ScrollPanel);
                    row.ScrollPanel.Dispose();
                }
            }
            _dynamicRows.Clear();

            int count = groups.Count == 0 ? 1 : groups.Count;
            int currentY = 12;

            for (int i = 0; i < count; i++)
            {
                var group = groups.Count > 0 ? groups[i] : null;
                int boardId = group != null ? group.Key : 0;

                int lines = group != null ? GetConfiguredLineCount(group.First()) : 3;

                var titleLabel = new MaterialSkin.Controls.MaterialLabel
                {
                    AutoSize = true,
                    Depth = 0,
                    Font = new Font("Roboto", 16F, FontStyle.Regular, GraphicsUnit.Pixel),
                    FontType = MaterialSkin.MaterialSkinManager.fontType.Subtitle1,
                    Location = new Point(17, currentY),
                    Margin = new Padding(0),
                    MouseState = MaterialSkin.MouseState.HOVER,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                    ForeColor = Color.FromArgb(200, 200, 200)
                };

                var scrollPanel = new Panel
                {
                    AutoScroll = true,
                    BackColor = Color.FromArgb(20, 20, 20),
                    Margin = new Padding(0),
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
                };

                int previewHeight = lines * 80;
                int previewWidth = 1800;
                var boardPreview = new MultiLineBoardPreview
                {
                    BackColor = Color.FromArgb(20, 20, 20),
                    FaultColor = Color.FromArgb(244, 67, 54),
                    Location = new Point(0, 0),
                    Margin = new Padding(0),
                    OffColor = Color.FromArgb(60, 60, 60),
                    OnColor = Color.FromArgb(76, 175, 80),
                    Size = new Size(previewWidth, previewHeight)
                };

                scrollPanel.Controls.Add(boardPreview);

                this.Controls.Add(titleLabel);
                this.Controls.Add(scrollPanel);

                int scrollPanelHeight = Math.Max(260, boardPreview.Height + SystemInformation.HorizontalScrollBarHeight + 12);

                titleLabel.Location = new Point(17, currentY);
                currentY += 28;

                scrollPanel.Location = new Point(17, currentY);
                scrollPanel.Size = new Size(Math.Max(300, this.Width - 34), scrollPanelHeight);
                currentY += scrollPanelHeight + 16;

                _dynamicRows.Add(new BoardRowControls
                {
                    BoardId = boardId,
                    Trains = group?.ToList(),
                    TitleLabel = titleLabel,
                    ScrollPanel = scrollPanel,
                    BoardPreview = boardPreview,
                    BoardLines = lines
                });
            }

            _calculatedHeight = currentY + 30;
            this.Height = _calculatedHeight;
        }

        private void ClearBoardPreview(MultiLineBoardPreview preview, int lines)
        {
            int width = 672;
            int totalHeight = lines * 32;

            preview.InitializeMatrix(totalHeight, width);
            preview.Size = new Size(1800, totalHeight * 2);

            bool[,] matrix = new bool[totalHeight, width];
            preview.SetLEDMatrix(matrix);
        }

        private void RenderBoardFrame(MultiLineBoardPreview preview, List<SingleLineDisplayBoardData> trains, int lines, int fontSizeVal)
        {
            int singleLineHeight = 32;
            int width = 672;
            int totalHeight = lines * singleLineHeight;

            preview.InitializeMatrix(totalHeight, width);
            preview.Size = new Size(1800, totalHeight * 2);

            var uniqueTrains = trains
                .GroupBy(t => t.TrainNumber, StringComparer.OrdinalIgnoreCase)
                .Select(g => g.First())
                .OrderBy(t => t.ScheduleArrivalTime)
                .ToList();

            string[] trainLines = new string[lines];
            for (int i = 0; i < lines; i++)
            {
                if (i < uniqueTrains.Count)
                {
                    trainLines[i] = FormatTrainLineWithStatusFromData(uniqueTrains[i]);
                }
                else
                {
                    trainLines[i] = FormatEmptyTrainLine();
                }
            }

            RenderBoardFrameLinesInternal(preview, trainLines, lines, fontSizeVal);
        }

        private void RenderBoardFrameLines(MultiLineBoardPreview preview, string[] trainLines, int lines, int fontSizeVal)
        {
            int singleLineHeight = 32;
            int width = 672;
            int totalHeight = lines * singleLineHeight;

            preview.InitializeMatrix(totalHeight, width);
            preview.Size = new Size(1800, totalHeight * 2);

            RenderBoardFrameLinesInternal(preview, trainLines, lines, fontSizeVal);
        }

        private void RenderBoardFrameLinesInternal(MultiLineBoardPreview preview, string[] trainLines, int lines, int fontSizeVal)
        {
            int singleLineHeight = 32;
            int width = 672;
            int totalHeight = lines * singleLineHeight;

            bool[,] matrix = new bool[totalHeight, width];
            const int scale = 4;
            int scaledWidth = width * scale;
            int scaledHeight = totalHeight * scale;
            int scaledLineHeight = singleLineHeight * scale;

            int fontSize = 15;

            using (Bitmap bmp = new Bitmap(scaledWidth, scaledHeight, PixelFormat.Format32bppArgb))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Black);

                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                using (Font font = new Font("Consolas", fontSize * scale, FontStyle.Bold, GraphicsUnit.Pixel))
                using (SolidBrush brush = new SolidBrush(Color.White))
                {
                    bool frameHasIndicText = trainLines.Take(lines).Any(ContainsNonAsciiText);

                    if (!frameHasIndicText)
                    {
                        // English frame: drawn exactly as before (one Consolas string per line)
                        for (int i = 0; i < lines && i < trainLines.Length; i++)
                        {
                            string lineText = trainLines[i] ?? string.Empty;

                            SizeF textSize = g.MeasureString(lineText, font);
                            float yOffset = i * scaledLineHeight + (scaledLineHeight - textSize.Height) / 2f;
                            float xOffset = 6 * scale;

                            g.DrawString(lineText, font, brush, xOffset, yOffset);
                        }
                    }
                    else
                    {
                        // Hindi / Regional frame: draw column by column so Devanagari / Indic text uses an
                        // Indic-capable font while the columns stay aligned to the Consolas grid.
                        using (Font indicFont = new Font(IndicFontFamily, fontSize * scale, FontStyle.Bold, GraphicsUnit.Pixel))
                        {
                            for (int i = 0; i < lines && i < trainLines.Length; i++)
                            {
                                DrawLineByColumns(g, trainLines[i] ?? string.Empty, font, indicFont, brush,
                                    6 * scale, i * scaledLineHeight, scaledLineHeight, scaledWidth);
                            }
                        }
                    }
                }

                ConvertBitmapToMatrix(bmp, matrix, width, totalHeight, scale);
            }

            preview.SetLEDMatrix(matrix);
        }

        private string FormatTrainLineWithStatusFromData(SingleLineDisplayBoardData data)
        {
            if (data == null || string.IsNullOrEmpty(data.TrainNumber))
            {
                return FormatEmptyTrainLine();
            }

            string trainNo = DisplayTextFormatter.SanitizeForDisplay(data.TrainNumber);
            if (trainNo.Length > W_TrainNo) trainNo = trainNo.Substring(0, W_TrainNo);

            string trainName = DisplayTextFormatter.SanitizeForDisplay(data.TrainNameEnglish ?? "");
            if (trainName.Length > W_TrainName) trainName = trainName.Substring(0, W_TrainName);

            // Status from the DB row: English part of TrainStatusEnglish ('#'-packed split), else ADFlag.
            string dbStatus = DisplayTextFormatter.SelectLanguagePart(
                !string.IsNullOrWhiteSpace(data.TrainStatusEnglish) ? data.TrainStatusEnglish : data.ADFlag, 0);

            // Check if cancelled FIRST
            //bool isCancelled = DisplayTextFormatter.IsCancelledStatus(data.ADFlag);
            bool isCancelled = DisplayTextFormatter.IsCancelledStatus(data.ADFlag) || DisplayTextFormatter.IsCancelledStatus(dbStatus);

            // If cancelled, show ONLY Train No, Train Name, and CANCELLED
            if (isCancelled)
            {
                return string.Format(
                    "{0,-" + W_TrainNo + "} {1,-" + W_TrainName + "}  {2}",
                    trainNo, trainName, "CANCELLED");
            }

            // Check for special status (TERMINATED AT, DIVERTED VIA, RESCHEDULED)
            //bool isSpecialStatus = DisplayTextFormatter.IsSpecialStatusDetail(data.ADFlag);
            // Indefinite Late / Has Arrived / Terminated At / Diverted / Change of Source: text only, no times.
            bool isSpecialStatus = DisplayTextFormatter.IsSpecialStatusDetail(dbStatus) || DisplayTextFormatter.IsSpecialStatusDetail(data.ADFlag);

            if (isSpecialStatus)
            {
                //string status = DisplayTextFormatter.SanitizeForDisplay(data.ADFlag);
                string status = DisplayTextFormatter.SanitizeForDisplay(
                    DisplayTextFormatter.IsSpecialStatusDetail(dbStatus) ? dbStatus : data.ADFlag);
                if (status.Length > 25) status = status.Substring(0, 25);

                return string.Format(
                    "{0,-" + W_TrainNo + "} {1,-" + W_TrainName + "}  {2}",
                    trainNo, trainName, status);
            }

            // For normal trains: NO status, show Scheduled Arrival, Expected Time, Scheduled Departure and Platform
            string schArr = DisplayTextFormatter.FormatTimeOnly(data.ScheduleArrivalTime);
            if (string.IsNullOrEmpty(schArr)) schArr = "--:--";
            if (schArr.Length > W_SchArr) schArr = schArr.Substring(0, W_SchArr);

            string schDep = DisplayTextFormatter.FormatTimeOnly(data.ScheduleDepartureTime);
            if (string.IsNullOrEmpty(schDep)) schDep = "--:--";
            if (schDep.Length > W_SchDep) schDep = schDep.Substring(0, W_SchDep);

            string expTime = DetermineExpectedTime(
                data.ActualArrivalTime, data.ScheduleArrivalTime,
                data.ActualDepartureTime, data.ScheduleDepartureTime,
                data.Late, data.ADFlag, data.TrainStatusEnglish);
            if (string.IsNullOrEmpty(expTime)) expTime = "--:--";
            if (expTime.Length > W_ExpTime) expTime = expTime.Substring(0, W_ExpTime);

            string platform = DisplayTextFormatter.SanitizeForDisplay(data.PlatformNumber);
            if (platform.Length > W_Platform) platform = platform.Substring(0, W_Platform);

            // Normal train: TrainNo, TrainName, Scheduled Arrival, Expected Time, Scheduled Departure, Platform
            //return string.Format(
            //    "{0,-" + W_TrainNo + "} {1,-" + W_TrainName + "} {2,-" + W_SchArr + "} {3,-" + W_ExpTime + "} {4,-" + W_SchDep + "} {5,-" + W_Platform + "}",
            //    trainNo, trainName, schArr, expTime, schDep, platform);

            // Same as SLDB: TrainNo | TrainName | ACTUAL_ARRIVAL_TIME | ACTUAL_DEPARTURE_TIME | Platform
            string dbArrShown = DisplayTextFormatter.FormatTimeOnly(data.ActualArrivalTime);
            if (string.IsNullOrEmpty(dbArrShown)) dbArrShown = "--:--";
            if (dbArrShown.Length > W_SchArr) dbArrShown = dbArrShown.Substring(0, W_SchArr);

            string dbDepShown = DisplayTextFormatter.FormatTimeOnly(data.ActualDepartureTime);
            if (string.IsNullOrEmpty(dbDepShown)) dbDepShown = "--:--";
            if (dbDepShown.Length > W_SchDep) dbDepShown = dbDepShown.Substring(0, W_SchDep);

            return string.Format(
                "{0,-" + W_TrainNo + "} {1,-" + W_TrainName + "} {2,-" + W_SchArr + "} {3,-" + W_SchDep + "} {4,-" + W_Platform + "}",
                trainNo, trainName, dbArrShown, dbDepShown, platform);
        }

        public void UpdateHealthStatus(bool online, double temperatureCelsius, double voltage, TimeSpan uptime)
        {
            foreach (var row in _dynamicRows)
            {
                if (row != null && row.BoardPreview != null && !row.BoardPreview.IsDisposed)
                {
                    row.BoardPreview.Enabled = online;
                }
            }
        }

        private static void ConvertBitmapToMatrix(Bitmap bmp, bool[,] matrix, int width, int height, int scale)
        {
            BitmapData bmpData = bmp.LockBits(
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

            try
            {
                int stride = bmpData.Stride;
                const int bytesPerPixel = 4;
                int byteCount = stride * bmp.Height;
                byte[] pixels = new byte[byteCount];
                System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, pixels, 0, byteCount);

                for (int cellY = 0; cellY < height; cellY++)
                {
                    for (int cellX = 0; cellX < width; cellX++)
                    {
                        bool lit = false;
                        for (int sy = 0; sy < scale && !lit; sy++)
                        {
                            int py = cellY * scale + sy;
                            if (py >= bmp.Height) break;
                            int rowOffset = py * stride;

                            for (int sx = 0; sx < scale && !lit; sx++)
                            {
                                int px = cellX * scale + sx;
                                if (px >= bmp.Width) break;
                                int idx = rowOffset + px * bytesPerPixel;

                                byte b = pixels[idx];
                                byte gCol = pixels[idx + 1];
                                byte r = pixels[idx + 2];

                                int maxComponent = Math.Max(r, Math.Max(gCol, b));
                                if (maxComponent / 255f > 0.45f)
                                {
                                    lit = true;
                                    break;
                                }
                            }
                        }
                        matrix[cellY, cellX] = lit;
                    }
                }
            }
            finally
            {
                bmp.UnlockBits(bmpData);
            }
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

        // =========================================================
        // LINES
        // =========================================================

        /// <summary>
        /// Number of lines configured for the board (BOARD_LINE), 3 when not set.
        /// </summary>
        private static int GetConfiguredLineCount(SingleLineDisplayBoardData board)
        {
            return board != null && int.TryParse(board.BoardLine, out int lines) && lines > 0 ? lines : 3;
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
        private List<string> GetBoardLanguages(string boardIp, List<string> lineConfLanguages)
        {
            string ip = boardIp?.Trim() ?? string.Empty;

            if (_boardLanguages.TryGetValue(ip, out List<string> configured) && configured.Count > 0)
                return configured;

            if (lineConfLanguages != null && lineConfLanguages.Count > 0)
                return lineConfLanguages;

            return new List<string> { LANG_ENGLISH };
        }

        /// <summary>
        /// The live data LineConf carries the languages in "LANG", but TrainStatusProcessor reads "LAN".
        /// Sets "LAN" (English + every language used by the boards) on the local live-data copy so the
        /// processor builds the Hindi / Regional status texts too. English is always included because the
        /// status logic (cancelled / terminated / diverted / rescheduled) works on the English text.
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

        /// <summary>
        /// Special-status text (flip 0 / flip 1) in the requested language, as prepared by TrainStatusProcessor.
        /// Empty for English (the English line keeps its own labels) and for normal statuses.
        /// </summary>
        private static string GetLanguageStatus(JsonObject train, string language, int flipIndex)
        {
            if (string.Equals(language, LANG_ENGLISH, StringComparison.OrdinalIgnoreCase))
                return string.Empty;

            if (train?["STA"] is not JsonObject)
                return string.Empty;

            return DisplayHelper.GetTrainStatus(train, language, flipIndex);
        }

        /// <summary>
        /// Cuts text to at most <paramref name="maxLength"/> characters without splitting a
        /// Hindi / Indic letter from its vowel signs (plain English text is cut exactly like Substring).
        /// </summary>
        private static string TruncateText(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text) || text.Length <= maxLength)
                return text ?? string.Empty;

            var result = new StringBuilder();
            TextElementEnumerator elements = StringInfo.GetTextElementEnumerator(text);
            while (elements.MoveNext())
            {
                string element = elements.GetTextElement();
                if (result.Length + element.Length > maxLength)
                    break;
                result.Append(element);
            }
            return result.ToString();
        }

        private static bool ContainsNonAsciiText(string text)
        {
            return !string.IsNullOrEmpty(text) && text.Any(c => c > 127);
        }

        /// <summary>
        /// Draws one board line in three columns (train no | train name | time, A/D, PF or status),
        /// placed on the Consolas character grid so the columns line up on every line.
        /// Each column uses Consolas for English text and the Indic font for Hindi / Regional text.
        /// </summary>
        private static void DrawLineByColumns(Graphics g, string lineText, Font asciiFont, Font indicFont, Brush brush,
            float xStart, float lineTop, float lineHeight, float maxWidth)
        {
            using (var format = (StringFormat)StringFormat.GenericTypographic.Clone())
            {
                float charWidth = g.MeasureString("0000000000", asciiFont, PointF.Empty, format).Width / 10f;

                int nameColumn = W_TrainNo + 1;
                int restColumn = nameColumn + W_TrainName;

                string trainNoPart = lineText.Length > 0 ? lineText.Substring(0, Math.Min(nameColumn, lineText.Length)) : string.Empty;
                string namePart = lineText.Length > nameColumn ? lineText.Substring(nameColumn, Math.Min(W_TrainName, lineText.Length - nameColumn)) : string.Empty;
                string restPart = lineText.Length > restColumn ? lineText.Substring(restColumn) : string.Empty;

                DrawColumn(trainNoPart, 0, 0);
                DrawColumn(namePart, nameColumn, W_TrainName);
                DrawColumn(restPart, restColumn, 0);

                void DrawColumn(string text, int column, int clipColumns)
                {
                    if (string.IsNullOrWhiteSpace(text))
                        return;

                    // Leading spaces become column offset (typographic drawing does not advance for them reliably)
                    int leadingSpaces = text.Length - text.TrimStart(' ').Length;
                    string value = text.Trim();
                    int startColumn = column + leadingSpaces;
                    int clipWidthColumns = clipColumns > 0 ? clipColumns - leadingSpaces : 0;

                    Font font = ContainsNonAsciiText(value) ? indicFont : asciiFont;
                    float x = xStart + startColumn * charWidth;
                    SizeF size = g.MeasureString(value, font, PointF.Empty, format);
                    float y = lineTop + (lineHeight - size.Height) / 2f;

                    float clipWidth = clipWidthColumns > 0 ? clipWidthColumns * charWidth : maxWidth - x;
                    if (clipWidth <= 0)
                        return;

                    g.SetClip(new RectangleF(x, lineTop, clipWidth, lineHeight));
                    g.DrawString(value, font, brush, x, y, format);
                    g.ResetClip();
                }
            }
        }
    }

    public class MultiLineBoardPreview : Panel
    {
        private bool[,] ledMatrix;
        private int rows = 96;
        private int cols = 672;

        public Color OnColor { get; set; } = Color.FromArgb(76, 175, 80);
        public Color OffColor { get; set; } = Color.FromArgb(60, 60, 60);
        public Color FaultColor { get; set; } = Color.FromArgb(244, 67, 54);

        public MultiLineBoardPreview()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            ResizeRedraw = true;
            InitializeMatrix(96, 672);
        }

        public void InitializeMatrix(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;
            this.ledMatrix = new bool[rows, cols];
            this.Invalidate();
        }

        public void SetLEDMatrix(bool[,] matrix)
        {
            if (matrix.GetLength(0) == rows && matrix.GetLength(1) == cols)
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
            {
                g.FillRectangle(bgBrush, ClientRectangle);
            }

            float pixelW = (float)Width / cols;
            float pixelH = (float)Height / rows;

            float size = Math.Min(pixelW, pixelH) * 0.85f;
            if (size < 1.5f) size = 1.5f;

            for (int row = 0; row < rows; row++)
            {
                if (row > 0 && row % 32 == 0)
                {
                    using (Pen dividerPen = new Pen(Color.FromArgb(30, 100, 100, 120), 1.5f))
                    {
                        float dividerY = row * pixelH;
                        g.DrawLine(dividerPen, 0, dividerY, Width, dividerY);
                    }
                }

                for (int col = 0; col < cols; col++)
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

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }
    }

}