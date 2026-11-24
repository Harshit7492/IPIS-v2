using System.Data;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Net.NetworkInformation;
using System.Text.Json;
using System.Text.Json.Nodes;
using DataBaseHandler;
using DataModel;
using IPISv2.HealthStatus;
using MaterialSkin;
using MaterialSkin.Controls;
using MemberDomain;

namespace IPISv2.HealthStatusReport
{
    public partial class CoachGuidenceDisplayBoard : UserControl
    {
        public const int BOARD_WIDTH = 96;
        public const int BOARD_HEIGHT = 32;
        public const int PIXEL_SIZE = 9;
        public const int LED_SPACING = 1;

        private const string LANG_ENGLISH = "E";
        private const string LANG_HINDI = "H";
        private const string LANG_REGIONAL = "R";
        private static readonly string[] LanguageDisplayOrder = { LANG_ENGLISH, LANG_HINDI, LANG_REGIONAL };

        private readonly int _defaultDisplayTime = 4;
        private readonly TrainStatusProcessor _statusProcessor = new TrainStatusProcessor();
        private readonly object _lock = new();
        private readonly CancellationTokenSource _displayCts = new();
        private System.Windows.Forms.Timer _displayTimer;
        private bool _isProcessingLiveData = false;
        private bool _liveRotationOwnsBoards = false;
        private int _currentFrameStep = 0;
        private int _calculatedHeight = 460;
        private bool _isLayingOut = false;

        private class CgdbUnitInfo
        {
            public int UnitIndex { get; set; }
            public string Port { get; set; }
            public string CoachPort { get; set; }
            public string UnitIp { get; set; }
            public string CoachCode { get; set; }
            public string CoachHindi { get; set; }
            public string TrainNumber { get; set; }
            public string TrainName { get; set; }
            public string PlatformNumber { get; set; }
        }

        private class CgdbPlatformRow
        {
            public int BoardId { get; set; }
            public string BoardName { get; set; }
            public string PlatformNumber { get; set; }
            public string BoardIp { get; set; }
            public int DelayTime { get; set; }
            public List<string> Languages { get; set; } = new List<string>();

            public Panel HeaderPanel { get; set; }
            public MaterialLabel TitleLabel { get; set; }
            public Label DropdownLabel { get; set; }
            public ComboBox IpComboBox { get; set; }
            public Panel PreviewPanel { get; set; }

            // Single unit display card
            public Panel BoardCard { get; set; }
            public Label IpBadgeLabel { get; set; }
            public Label PageStatusLabel { get; set; }
            public CGDBBoardControl BoardControl { get; set; }

            public List<CgdbUnitInfo> Units { get; set; } = new List<CgdbUnitInfo>();
            public string TrainNumber { get; set; }
            public string TrainName { get; set; }
            public int SelectedUnitIndex { get; set; } = 0;
            public string LastRenderKey { get; set; } = string.Empty;
            public bool LastPingResult { get; set; } = false;
        }

        private readonly List<CgdbPlatformRow> _platformRows = new List<CgdbPlatformRow>();
        private readonly Dictionary<string, string> _coachLegends = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public CoachGuidenceDisplayBoard()
        {
            InitializeComponent();
            if (IsInDesignMode()) return;

            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);
            DoubleBuffered = true;
            BackColor = Color.White;

            LoadCoachLegends();
            LoadActiveBoardData();

            _displayTimer = new System.Windows.Forms.Timer();
            _displayTimer.Interval = 2000;
            _displayTimer.Tick += DisplayTimer_Tick;
            _displayTimer.Start();

            this.Disposed += (s, e) =>
            {
                _displayTimer?.Stop();
                _displayTimer?.Dispose();
                _displayCts?.Cancel();
                _displayCts?.Dispose();
            };

            this.Resize += (s, e) => SyncLayoutWidths();
        }

        private async void DisplayTimer_Tick(object sender, EventArgs e)
        {
            await ProcessAndBroadcastCGDBLiveDataAsync();
        }

        private void SyncLayoutWidths()
        {
            int targetWidth = Math.Max(100, this.Width - 34);
            foreach (var row in _platformRows)
            {
                if (row.HeaderPanel != null && !row.HeaderPanel.IsDisposed)
                {
                    row.HeaderPanel.Width = targetWidth;

                    // Keep Dropdown and its label pinned to the right
                    if (row.IpComboBox != null && row.DropdownLabel != null)
                    {
                        row.IpComboBox.Left = Math.Max(100, row.HeaderPanel.ClientSize.Width - row.IpComboBox.Width - 6);
                        row.DropdownLabel.Left = Math.Max(0, row.IpComboBox.Left - row.DropdownLabel.PreferredWidth - 8);
                    }
                }

                if (row.PreviewPanel != null && !row.PreviewPanel.IsDisposed)
                {
                    row.PreviewPanel.Width = targetWidth;
                }
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (this.Height != _calculatedHeight && _calculatedHeight > 0)
            {
                this.Height = _calculatedHeight;
            }
        }

        private void LoadCoachLegends()
        {
            try
            {
                var legends = _dbService.GET_LEGEND_LIST(0);
                if (legends != null)
                {
                    foreach (var leg in legends)
                    {
                        if (!string.IsNullOrWhiteSpace(leg.LEGEND) && !string.IsNullOrWhiteSpace(leg.LEGEND_HINDI))
                        {
                            _coachLegends[leg.LEGEND.Trim()] = leg.LEGEND_HINDI.Trim();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.LoggerService.Logger.Error(ex, "Error loading coach legends.");
            }
        }

        private string GetCoachHindi(string coachCode)
        {
            if (string.IsNullOrWhiteSpace(coachCode)) return string.Empty;
            string key = coachCode.Trim();
            if (_coachLegends.TryGetValue(key, out string hindi))
                return hindi;
            return coachCode;
        }

        private static string NormalizePlatform(string pf)
        {
            string value = (pf ?? string.Empty).Trim();
            if (value.Length == 0) return value;
            string withoutZeros = value.TrimStart('0');
            return withoutZeros.Length == 0 ? "0" : withoutZeros;
        }

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
                    if (token.Length == 0) continue;

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
                                codes.Add(LANG_REGIONAL);
                            break;
                    }
                }
            }

            return LanguageDisplayOrder.Where(codes.Contains).ToList();
        }

        private static void PrepareLanguagesForProcessing(JsonObject liveData)
        {
            JsonArray? lineConf = liveData["LineConf"] as JsonArray;

            var codes = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { LANG_ENGLISH, LANG_HINDI };
            string lan = string.Join("#", LanguageDisplayOrder.Where(codes.Contains));

            if (lineConf == null)
            {
                lineConf = new JsonArray();
                liveData["LineConf"] = lineConf;
            }

            if (lineConf.Count == 0 || lineConf[0] is not JsonObject config)
            {
                if (lineConf.Count == 0)
                    lineConf.Add(new JsonObject());
                config = lineConf[0] as JsonObject;
            }

            if (config != null)
                config["LAN"] = lan;
        }

        public void LoadActiveBoardData()
        {
            try
            {
                var allSettings = _dbService.GET_BOARD_SETTING(0) ?? new List<BoardSettingInfo>();
                var installedSettings = allSettings.Where(b => b.BOARD_NAME != null &&
                    (b.BOARD_NAME.IndexOf("Coach Guidance", StringComparison.OrdinalIgnoreCase) >= 0 ||
                     b.BOARD_NAME.IndexOf("CGDB", StringComparison.OrdinalIgnoreCase) >= 0 ||
                     b.BOARD_NAME.IndexOf("CoachGuidance", StringComparison.OrdinalIgnoreCase) >= 0)).ToList();

                if (installedSettings.Any(b => b.IsActive == 1))
                {
                    installedSettings = installedSettings.Where(b => b.IsActive == 1).ToList();
                }

                var updatedTrains = _dbService.GET_UPDATED_TRAINS_DATA(0) ?? new List<train_updated_data>();
                var activeTrains = _dbService.GetAllActiveBoards() ?? new List<SingleLineDisplayBoardData>();

                var platformDataList = new List<CgdbPlatformRow>();

                if (installedSettings.Count > 0)
                {
                    foreach (var setting in installedSettings)
                    {
                        string pfStr = !string.IsNullOrWhiteSpace(setting.PF_NO) ? setting.PF_NO.Trim() : "1";
                        if (pfStr == "0") pfStr = "1";

                        var languages = ParseLanguageCodes(setting.LANGUAGES);
                        if (languages.Count == 0)
                        {
                            languages = new List<string> { LANG_ENGLISH, LANG_HINDI };
                        }

                        train_updated_data matchingTrain = updatedTrains.FirstOrDefault(t =>
                            (string.Equals(NormalizePlatform(t.ACTUAL_PLATFORM_NUMBER), NormalizePlatform(pfStr), StringComparison.OrdinalIgnoreCase) ||
                             string.Equals(NormalizePlatform(t.SCHEDULE_PLATFORM_NUMBER), NormalizePlatform(pfStr), StringComparison.OrdinalIgnoreCase)) &&
                            t.CGDB_SEND_DATA == 1);

                        // Priority 2: ANY train with CGDB_SEND_DATA == 1 (explicitly sent to CGDB)
                        if (matchingTrain == null)
                        {
                            matchingTrain = updatedTrains.FirstOrDefault(t => t.CGDB_SEND_DATA == 1);
                        }

                        // Priority 3: Only when no train has CGDB_SEND_DATA == 1, fall back to matching platform
                        if (matchingTrain == null)
                        {
                            matchingTrain = updatedTrains.FirstOrDefault(t =>
                                string.Equals(NormalizePlatform(t.ACTUAL_PLATFORM_NUMBER), NormalizePlatform(pfStr), StringComparison.OrdinalIgnoreCase) ||
                                string.Equals(NormalizePlatform(t.SCHEDULE_PLATFORM_NUMBER), NormalizePlatform(pfStr), StringComparison.OrdinalIgnoreCase));
                        }

                        SingleLineDisplayBoardData dbActiveMatch = null;
                        if (matchingTrain == null && activeTrains.Count > 0)
                        {
                            dbActiveMatch = activeTrains.FirstOrDefault(t =>
                                string.Equals(NormalizePlatform(t.PlatformNumber), NormalizePlatform(pfStr), StringComparison.OrdinalIgnoreCase));
                        }

                        string trainNo = matchingTrain != null ? matchingTrain.TRAIN_NUMBER : (dbActiveMatch != null ? dbActiveMatch.TrainNumber : string.Empty);
                        string trainName = matchingTrain != null ? matchingTrain.TRAIN_NAME_ENGLISH : (dbActiveMatch != null ? dbActiveMatch.TrainNameEnglish : string.Empty);
                        string rawCoachSeq = matchingTrain != null ? matchingTrain.COACH_SEQUENCE : (dbActiveMatch != null ? dbActiveMatch.Coach_Sequence : string.Empty);

                        List<string> coaches = new List<string>();
                        if (!string.IsNullOrWhiteSpace(rawCoachSeq))
                        {
                            coaches = ParseCoaches(rawCoachSeq);
                            if (matchingTrain != null && matchingTrain.trainReversalFlag)
                            {
                                coaches.Reverse();
                            }
                        }

                        var unitList = new List<CgdbUnitInfo>();
                        var ports = !string.IsNullOrWhiteSpace(setting.SELECTED_PF)
                            ? setting.SELECTED_PF.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            : Array.Empty<string>();

                        int totalUnits = ports.Length;
                        if (totalUnits == 0 && setting.BOARD_LINE > 0)
                            totalUnits = setting.BOARD_LINE;
                        if (totalUnits == 0 && coaches.Count > 0)
                            totalUnits = coaches.Count;
                        if (totalUnits == 0)
                            totalUnits = 9;

                        for (int i = 0; i < totalUnits; i++)
                        {
                            string port = "2";
                            string coachPort = (i + 2).ToString();
                            if (i < ports.Length)
                            {
                                var parts = ports[i].Split('+');
                                if (parts.Length > 0 && !string.IsNullOrWhiteSpace(parts[0])) port = parts[0].Trim();
                                if (parts.Length > 1 && !string.IsNullOrWhiteSpace(parts[1])) coachPort = parts[1].Trim();
                            }

                            string unitIp = $"10.0.{pfStr}.{coachPort}";
                            string coachCode = (i < coaches.Count) ? coaches[i] : $"C{i + 1}";
                            string coachHindi = GetCoachHindi(coachCode);

                            unitList.Add(new CgdbUnitInfo
                            {
                                UnitIndex = i + 1,
                                Port = port,
                                CoachPort = coachPort,
                                UnitIp = unitIp,
                                CoachCode = coachCode,
                                CoachHindi = coachHindi,
                                TrainNumber = !string.IsNullOrWhiteSpace(trainNo) ? trainNo : "----",
                                TrainName = trainName ?? string.Empty,
                                PlatformNumber = pfStr
                            });
                        }

                        platformDataList.Add(new CgdbPlatformRow
                        {
                            BoardId = setting.BOARD_ID,
                            BoardName = setting.BOARD_NAME,
                            PlatformNumber = pfStr,
                            BoardIp = setting.BOARD_IP,
                            DelayTime = setting.DELAYTIME > 0 ? setting.DELAYTIME : _defaultDisplayTime,
                            Languages = languages,
                            TrainNumber = trainNo,
                            TrainName = trainName,
                            Units = unitList
                        });
                    }
                }
                else
                {
                    // Fallback demo board if no CGDB board is in settings
                    var demoLanguages = new List<string> { LANG_ENGLISH, LANG_HINDI };
                    train_updated_data matchingTrain = updatedTrains.FirstOrDefault(t => t.CGDB_SEND_DATA == 1) ??
                                                       updatedTrains.FirstOrDefault();

                    List<string> coaches = new List<string>();
                    if (matchingTrain != null && !string.IsNullOrWhiteSpace(matchingTrain.COACH_SEQUENCE))
                    {
                        coaches = ParseCoaches(matchingTrain.COACH_SEQUENCE);
                        if (matchingTrain.trainReversalFlag) coaches.Reverse();
                    }

                    int totalUnits = coaches.Count > 0 ? coaches.Count : 9;
                    var unitList = new List<CgdbUnitInfo>();
                    for (int i = 0; i < totalUnits; i++)
                    {
                        string coachCode = (i < coaches.Count) ? coaches[i] : $"C{i + 1}";
                        unitList.Add(new CgdbUnitInfo
                        {
                            UnitIndex = i + 1,
                            Port = "2",
                            CoachPort = (i + 2).ToString(),
                            UnitIp = $"10.0.1.{i + 2}",
                            CoachCode = coachCode,
                            CoachHindi = GetCoachHindi(coachCode),
                            TrainNumber = matchingTrain != null ? matchingTrain.TRAIN_NUMBER : "----",
                            TrainName = matchingTrain != null ? matchingTrain.TRAIN_NAME_ENGLISH : string.Empty,
                            PlatformNumber = "1"
                        });
                    }

                    platformDataList.Add(new CgdbPlatformRow
                    {
                        BoardId = 0,
                        BoardName = "Coach Guidance Display Board",
                        PlatformNumber = matchingTrain != null && !string.IsNullOrWhiteSpace(matchingTrain.ACTUAL_PLATFORM_NUMBER)
                            ? matchingTrain.ACTUAL_PLATFORM_NUMBER : "1",
                        BoardIp = "10.0.1.2",
                        DelayTime = _defaultDisplayTime,
                        Languages = demoLanguages,
                        TrainNumber = matchingTrain != null ? matchingTrain.TRAIN_NUMBER : string.Empty,
                        TrainName = matchingTrain != null ? matchingTrain.TRAIN_NAME_ENGLISH : string.Empty,
                        Units = unitList
                    });
                }

                UpdateDynamicUI(platformDataList);
            }
            catch (Exception ex)
            {
                LogManager.LoggerService.Logger.Error(ex, "Error loading CGDB data.");
            }
        }

        private static List<string> ParseCoaches(string rawCoachSequence)
        {
            if (string.IsNullOrWhiteSpace(rawCoachSequence))
                return new List<string>();

            return rawCoachSequence
                .Split(new[] { ',', ' ', '-', '#', '|', ';', '\t' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.Trim())
                .Where(c => c.Length > 0)
                .ToList();
        }

        private static void SetLabelTextIfChanged(Label label, string text)
        {
            if (label == null || label.IsDisposed) return;
            if (!string.Equals(label.Text, text, StringComparison.Ordinal))
            {
                label.Text = text;
            }
        }

        private static void SetLabelTextIfChanged(MaterialSkin.Controls.MaterialLabel label, string text)
        {
            if (label == null || label.IsDisposed) return;
            if (!string.Equals(label.Text, text, StringComparison.Ordinal))
            {
                label.Text = text;
            }
        }

        private void UpdateDynamicUI(List<CgdbPlatformRow> newPlatforms)
        {
            if (_isLayingOut) return;
            _isLayingOut = true;

            try
            {
                bool needsRecreate = false;
                if (_platformRows.Count != newPlatforms.Count)
                {
                    needsRecreate = true;
                }
                else
                {
                    for (int i = 0; i < newPlatforms.Count; i++)
                    {
                        if (_platformRows[i].BoardId != newPlatforms[i].BoardId ||
                            _platformRows[i].Units.Count != newPlatforms[i].Units.Count)
                        {
                            needsRecreate = true;
                            break;
                        }
                    }
                }

                if (needsRecreate)
                {
                    RecreatePlatformControls(newPlatforms);
                }

                // Update text, dropdown items, and active preview
                for (int p = 0; p < _platformRows.Count && p < newPlatforms.Count; p++)
                {
                    var existingRow = _platformRows[p];
                    var newData = newPlatforms[p];

                    if (!string.Equals(existingRow.TrainNumber, newData.TrainNumber, StringComparison.Ordinal))
                    {
                        existingRow.TrainNumber = newData.TrainNumber;
                        existingRow.LastRenderKey = string.Empty;
                    }
                    existingRow.TrainName = newData.TrainName;
                    existingRow.Units = newData.Units;
                    existingRow.DelayTime = newData.DelayTime;
                    existingRow.Languages = newData.Languages;

                    // Update Title on the left
                    string titleText;
                    if (!string.IsNullOrWhiteSpace(existingRow.TrainNumber) && existingRow.TrainNumber != "----")
                    {
                        titleText = $"Coach Guidance Display Board (CGDB) — Platform: {existingRow.PlatformNumber} — Train: {existingRow.TrainNumber} {existingRow.TrainName}".Trim();
                    }
                    else
                    {
                        titleText = $"Coach Guidance Display Board (CGDB) — Platform: {existingRow.PlatformNumber} — No Active Train";
                    }

                    SetLabelTextIfChanged(existingRow.TitleLabel, titleText);

                    // Update dropdown items on the right (ONLY IPs)
                    SyncDropdownItems(existingRow);

                    // If live rotation is not owning the board, render initial state
                    if (!_liveRotationOwnsBoards)
                    {
                        RenderActiveFrameForPlatform(existingRow, _currentFrameStep);
                    }
                }
            }
            finally
            {
                _isLayingOut = false;
            }
        }

        private void SyncDropdownItems(CgdbPlatformRow row)
        {
            if (row.IpComboBox == null) return;

            var combo = row.IpComboBox;
            var expectedItems = new List<string>();
            foreach (var unit in row.Units)
            {
                expectedItems.Add(unit.UnitIp);
            }

            bool changed = combo.Items.Count != expectedItems.Count;
            if (!changed)
            {
                for (int i = 0; i < expectedItems.Count; i++)
                {
                    if (combo.Items[i].ToString() != expectedItems[i])
                    {
                        changed = true;
                        break;
                    }
                }
            }

            if (changed)
            {
                int savedIndex = combo.SelectedIndex >= 0 ? combo.SelectedIndex : 0;
                combo.BeginUpdate();
                combo.Items.Clear();
                foreach (var item in expectedItems)
                {
                    combo.Items.Add(item);
                }

                if (savedIndex >= combo.Items.Count)
                    savedIndex = Math.Max(0, combo.Items.Count - 1);

                combo.SelectedIndex = savedIndex;
                row.SelectedUnitIndex = savedIndex;
                combo.EndUpdate();
            }
        }

        private void RecreatePlatformControls(List<CgdbPlatformRow> newPlatforms)
        {
            this.SuspendLayout();

            // Clear old controls
            foreach (var row in _platformRows)
            {
                if (row.HeaderPanel != null)
                {
                    this.Controls.Remove(row.HeaderPanel);
                    row.HeaderPanel.Dispose();
                }
                if (row.PreviewPanel != null)
                {
                    this.Controls.Remove(row.PreviewPanel);
                    row.PreviewPanel.Dispose();
                }
            }
            _platformRows.Clear();

            int currentY = 10;
            int targetWidth = Math.Max(100, this.Width - 34);

            for (int p = 0; p < newPlatforms.Count; p++)
            {
                var platformData = newPlatforms[p];

                // Header container panel holding title on left and IP dropdown on the right
                var headerPanel = new Panel
                {
                    Location = new Point(17, currentY),
                    Size = new Size(targetWidth, 34),
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                    BackColor = Color.Transparent
                };

                var titleLabel = new MaterialLabel
                {
                    AutoSize = true,
                    Depth = 0,
                    Font = new Font("Roboto", 16F, FontStyle.Regular, GraphicsUnit.Pixel),
                    FontType = MaterialSkinManager.fontType.Subtitle1,
                    Location = new Point(0, 4),
                    Margin = new Padding(0),
                    MouseState = MouseState.HOVER,
                    ForeColor = Color.White
                };
                headerPanel.Controls.Add(titleLabel);

                // Dropdown on the right side
                var cmbIp = new ComboBox
                {
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Font = new Font("Roboto", 9F, FontStyle.Regular),
                    BackColor = Color.FromArgb(40, 40, 52),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Size = new Size(160, 27),
                    Anchor = AnchorStyles.Top | AnchorStyles.Right
                };
                cmbIp.Location = new Point(Math.Max(100, headerPanel.ClientSize.Width - cmbIp.Width - 6), 3);

                var lblSelectIp = new Label
                {
                    Text = "Select CGDB IP:",
                    Font = new Font("Roboto", 9.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(200, 200, 220),
                    Anchor = AnchorStyles.Top | AnchorStyles.Right,
                    AutoSize = true
                };
                lblSelectIp.Location = new Point(Math.Max(0, cmbIp.Left - lblSelectIp.PreferredWidth - 8), 7);

                headerPanel.Controls.Add(lblSelectIp);
                headerPanel.Controls.Add(cmbIp);

                headerPanel.Resize += (s, e) =>
                {
                    cmbIp.Left = Math.Max(100, headerPanel.ClientSize.Width - cmbIp.Width - 6);
                    lblSelectIp.Left = Math.Max(0, cmbIp.Left - lblSelectIp.PreferredWidth - 8);
                };

                this.Controls.Add(headerPanel);
                currentY += 36;

                int boardW = BOARD_WIDTH * (PIXEL_SIZE + LED_SPACING); // 960 px
                int boardH = BOARD_HEIGHT * (PIXEL_SIZE + LED_SPACING); // 320 px
                int cardPadding = 12;
                int headerH = 38;
                int cardW = boardW + (cardPadding * 2); // 984 px
                int cardH = headerH + boardH + cardPadding + 4; // 374 px
                int previewH = cardH + 20; // 394 px

                var previewPanel = new Panel
                {
                    BackColor = Color.FromArgb(16, 16, 22),
                    Location = new Point(17, currentY),
                    Size = new Size(targetWidth, previewH),
                    AutoScroll = true,
                    Margin = new Padding(0),
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
                };
                this.Controls.Add(previewPanel);

                // Selected CGDB Board Card
                var boardCard = new Panel
                {
                    Location = new Point(12, 10),
                    Size = new Size(cardW, cardH),
                    BackColor = Color.FromArgb(24, 24, 34)
                };
                boardCard.Paint += (s, e) =>
                {
                    using (var pen = new Pen(Color.FromArgb(40, 160, 90), 1.5f))
                    {
                        e.Graphics.DrawRectangle(pen, 0, 0, boardCard.Width - 1, boardCard.Height - 1);
                    }
                };

                // IP Badge at top left of card (NO Coach 1 • ENG)
                var lblIpBadge = new Label
                {
                    Text = "IP: 10.0.1.2",
                    Font = new Font("Roboto", 10.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(100, 210, 255),
                    Location = new Point(cardPadding, 9),
                    AutoSize = true
                };
                boardCard.Controls.Add(lblIpBadge);

                // Frame / Status indicator at top right of card
                var lblPageStatus = new Label
                {
                    Text = "Train No",
                    Font = new Font("Roboto", 9.5F, FontStyle.Regular),
                    ForeColor = Color.FromArgb(180, 180, 200),
                    Location = new Point(cardW - 320, 10),
                    Size = new Size(305, 20),
                    TextAlign = ContentAlignment.MiddleRight,
                    Anchor = AnchorStyles.Top | AnchorStyles.Right
                };
                boardCard.Controls.Add(lblPageStatus);

                var boardControl = new CGDBBoardControl(BOARD_WIDTH, BOARD_HEIGHT, PIXEL_SIZE)
                {
                    Location = new Point(cardPadding, headerH),
                    OnColor = Color.FromArgb(76, 175, 80),
                    OffColor = Color.FromArgb(35, 35, 45)
                };
                boardCard.Controls.Add(boardControl);

                previewPanel.Controls.Add(boardCard);

                currentY += previewPanel.Height + 16;

                var platformRow = new CgdbPlatformRow
                {
                    BoardId = platformData.BoardId,
                    BoardName = platformData.BoardName,
                    PlatformNumber = platformData.PlatformNumber,
                    BoardIp = platformData.BoardIp,
                    DelayTime = platformData.DelayTime,
                    Languages = platformData.Languages,
                    HeaderPanel = headerPanel,
                    TitleLabel = titleLabel,
                    DropdownLabel = lblSelectIp,
                    IpComboBox = cmbIp,
                    PreviewPanel = previewPanel,
                    BoardCard = boardCard,
                    IpBadgeLabel = lblIpBadge,
                    PageStatusLabel = lblPageStatus,
                    BoardControl = boardControl,
                    Units = platformData.Units,
                    TrainNumber = platformData.TrainNumber,
                    TrainName = platformData.TrainName,
                    SelectedUnitIndex = 0
                };

                cmbIp.SelectedIndexChanged += (s, e) =>
                {
                    if (cmbIp.SelectedIndex >= 0 && cmbIp.SelectedIndex < platformRow.Units.Count)
                    {
                        platformRow.SelectedUnitIndex = cmbIp.SelectedIndex;
                        platformRow.LastRenderKey = string.Empty;
                        RenderActiveFrameForPlatform(platformRow, _currentFrameStep);
                    }
                };

                _platformRows.Add(platformRow);
                SyncDropdownItems(platformRow);
            }

            _calculatedHeight = currentY + 10;
            this.Height = _calculatedHeight;
            this.ResumeLayout(true);
        }

        private static bool IsHostReachable(string ipAddress, int timeoutMs = 500)
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

        private static void ClearBoard(CGDBBoardControl boardControl)
        {
            if (boardControl == null || boardControl.IsDisposed) return;
            bool[,] empty = new bool[BOARD_HEIGHT, BOARD_WIDTH];
            boardControl.SetLEDMatrix(empty);
        }

        public async Task ProcessAndBroadcastCGDBLiveDataAsync()
        {
            if (_isProcessingLiveData)
            {
                LogManager.LoggerService.Logger.Debug("CGDB tick skipped (previous cycle still running).");
                return;
            }
            _isProcessingLiveData = true;
            bool liveRenderedThisCycle = false;

            try
            {
                LoadActiveBoardData();

                var deviceList = new List<DeviceInfo>();
                var displayList = new List<DeviceInfo>();

                if (_platformRows != null)
                {
                    foreach (var row in _platformRows)
                    {
                        if (row == null || row.Units == null || row.Units.Count == 0)
                            continue;

                        int selectedIdx = Math.Max(0, Math.Min(row.SelectedUnitIndex, row.Units.Count - 1));
                        var unit = row.Units[selectedIdx];
                        if (unit == null || string.IsNullOrWhiteSpace(unit.UnitIp))
                            continue;

                        string unitIp = unit.UnitIp.Trim();
                        bool reachable = IsHostReachable(unitIp);
                        row.LastPingResult = reachable;

                        if (!reachable)
                        {
                            LogManager.LoggerService.Logger.Debug($"CGDB unit {unitIp} not reachable — skipping in payload.");
                            continue;
                        }

                        deviceList.Add(new DeviceInfo
                        {
                            Vendor = "VARDAAN",
                            Device = "CGDB",
                            IP = unitIp,
                            PF = row.PlatformNumber ?? "1",
                            INT = "50",
                            HSR = "ALL OK"
                        });

                        displayList.Add(new DeviceInfo
                        {
                            Device = "CGDB",
                            IP = unitIp,
                            INT = "50",
                            HSR = "ALL OK"
                        });
                    }
                }

                JsonObject? processedOutput = null;

                if (deviceList.Count > 0)
                {
                    try
                    {
                        var request = new LiveDataRequest
                        {
                            Device = deviceList,
                            DisplayList = displayList
                        };
                        string rawJson = JsonSerializer.Serialize(request);
                        var liveData = await _dbService.GetLiveDataDisplayAsync(request, rawJson);

                        if (liveData != null)
                        {
                            JsonObject? jsonObject = JsonSerializer.SerializeToNode(liveData) as JsonObject;
                            if (jsonObject != null)
                            {
                                PrepareLanguagesForProcessing(jsonObject);
                                processedOutput = _statusProcessor.Process(jsonObject);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogManager.LoggerService.Logger.Warning(ex, "Could not fetch live API data for CGDB; using database train data.");
                    }
                }

                liveRenderedThisCycle = true;
                await UpdateBoardsWithLiveDataAsync(processedOutput, _displayCts.Token);
            }
            catch (OperationCanceledException)
            {
                LogManager.LoggerService.Logger.Information("ProcessAndBroadcastCGDBLiveDataAsync was cancelled.");
            }
            catch (Exception ex)
            {
                LogManager.LoggerService.Logger.Error(ex, "Error while processing and broadcasting CGDB live data.");
            }
            finally
            {
                _liveRotationOwnsBoards = liveRenderedThisCycle;
                _isProcessingLiveData = false;
            }
        }

        private async Task UpdateBoardsWithLiveDataAsync(JsonObject? data, CancellationToken token)
        {
            if (data != null && data["TrainList"] is JsonArray liveTrains && liveTrains.Count > 0)
            {
                foreach (var row in _platformRows)
                {
                    if (string.IsNullOrWhiteSpace(row.TrainNumber) || row.TrainNumber == "----")
                        continue;

                    // Match strictly by Train Number so we never overwrite the selected CGDB train with a different train
                    JsonObject? matchingTrainObj = liveTrains.OfType<JsonObject>().FirstOrDefault(t =>
                        string.Equals(DisplayHelper.GetString(t, "TNO")?.Trim(), row.TrainNumber.Trim(), StringComparison.OrdinalIgnoreCase));

                    if (matchingTrainObj != null)
                    {
                        string tne = DisplayHelper.GetString(matchingTrainObj, "TNE");
                        string ccd = DisplayHelper.GetString(matchingTrainObj, "CCD");

                        if (!string.IsNullOrWhiteSpace(tne)) row.TrainName = tne;

                        if (!string.IsNullOrWhiteSpace(ccd))
                        {
                            var coaches = ParseCoaches(ccd);
                            for (int i = 0; i < row.Units.Count; i++)
                            {
                                var u = row.Units[i];
                                u.TrainNumber = row.TrainNumber;
                                u.TrainName = row.TrainName;
                                if (i < coaches.Count)
                                {
                                    u.CoachCode = coaches[i];
                                    u.CoachHindi = GetCoachHindi(u.CoachCode);
                                }
                            }
                        }
                    }
                }
            }

            int pto = _platformRows.Count > 0 && _platformRows[0].DelayTime > 0 ? _platformRows[0].DelayTime : _defaultDisplayTime;
            if (pto < 2) pto = 4;

            bool anyHindi = _platformRows.Any(r => r.Languages != null && r.Languages.Contains(LANG_HINDI));
            int steps = anyHindi ? 3 : 2;
            double slotSeconds = (double)pto / steps;

            for (int step = 0; step < steps; step++)
            {
                token.ThrowIfCancellationRequested();
                _currentFrameStep = step;

                Action renderAll = () =>
                {
                    foreach (var row in _platformRows)
                    {
                        RenderActiveFrameForPlatform(row, step);
                    }
                };

                if (this.InvokeRequired)
                    this.Invoke(renderAll);
                else
                    renderAll();

                await DisplayHelper.DelaySeconds(slotSeconds, token);
            }
        }

        private void RenderActiveFrameForPlatform(CgdbPlatformRow row, int frameStep)
        {
            if (row == null || row.Units == null || row.Units.Count == 0 || row.BoardControl == null || row.BoardControl.IsDisposed)
                return;

            int idx = row.SelectedUnitIndex;
            if (idx < 0 || idx >= row.Units.Count) idx = 0;
            row.SelectedUnitIndex = idx;
            var unit = row.Units[idx];

            bool isReachable = IsHostReachable(unit.UnitIp);
            row.LastPingResult = isReachable;

            if (!isReachable)
            {
                string offlineKey = $"{unit.UnitIp}_offline";
                if (!string.Equals(row.LastRenderKey, offlineKey, StringComparison.Ordinal))
                {
                    ClearBoard(row.BoardControl);
                    row.LastRenderKey = offlineKey;
                }

                SetLabelTextIfChanged(row.IpBadgeLabel, $"IP: {unit.UnitIp} • Offline");
                if (row.IpBadgeLabel != null)
                    row.IpBadgeLabel.ForeColor = Color.FromArgb(244, 67, 54);

                SetLabelTextIfChanged(row.PageStatusLabel, "Device Offline • No Ping Response");
                if (row.PageStatusLabel != null)
                    row.PageStatusLabel.ForeColor = Color.FromArgb(244, 67, 54);

                return;
            }

            SetLabelTextIfChanged(row.IpBadgeLabel, $"IP: {unit.UnitIp} • Online");
            if (row.IpBadgeLabel != null)
                row.IpBadgeLabel.ForeColor = Color.FromArgb(76, 175, 80);

            bool hasTrain = !string.IsNullOrWhiteSpace(row.TrainNumber) && row.TrainNumber != "----";

            if (!hasTrain)
            {
                string noTrainKey = $"{unit.UnitIp}_notrain";
                if (!string.Equals(row.LastRenderKey, noTrainKey, StringComparison.Ordinal))
                {
                    RenderCgdbUnit(row.BoardControl, "----", string.Empty, false, true);
                    row.LastRenderKey = noTrainKey;
                }

                SetLabelTextIfChanged(row.PageStatusLabel, "No Train Information Available");
                if (row.PageStatusLabel != null)
                    row.PageStatusLabel.ForeColor = Color.FromArgb(180, 180, 200);

                return;
            }

            bool hasHindi = row.Languages != null && row.Languages.Contains(LANG_HINDI);
            int totalSteps = hasHindi ? 3 : 2;
            int step = frameStep % totalSteps;

            string renderKey = "";
            string statusText = "";

            if (step == 0)
            {
                // Page 1: Train Number
                renderKey = $"{unit.UnitIp}_train_{row.TrainNumber}";
                statusText = $"Train No: {row.TrainNumber}";

                if (!string.Equals(row.LastRenderKey, renderKey, StringComparison.Ordinal))
                {
                    RenderCgdbUnit(row.BoardControl, row.TrainNumber, string.Empty, false, true);
                    row.LastRenderKey = renderKey;
                }
            }
            else if (step == 1)
            {
                // Page 2: Coach Code (English)
                renderKey = $"{unit.UnitIp}_coach_en_{unit.CoachCode}";
                statusText = $"Coach (ENG): {unit.CoachCode}";

                if (!string.Equals(row.LastRenderKey, renderKey, StringComparison.Ordinal))
                {
                    RenderCgdbUnit(row.BoardControl, row.TrainNumber, unit.CoachCode, false, false);
                    row.LastRenderKey = renderKey;
                }
            }
            else
            {
                // Page 3: Coach Code (Hindi)
                string coachHindi = !string.IsNullOrWhiteSpace(unit.CoachHindi) ? unit.CoachHindi : unit.CoachCode;
                renderKey = $"{unit.UnitIp}_coach_hi_{coachHindi}";
                statusText = $"Coach (HIN): {coachHindi}";

                if (!string.Equals(row.LastRenderKey, renderKey, StringComparison.Ordinal))
                {
                    RenderCgdbUnit(row.BoardControl, row.TrainNumber, coachHindi, true, false);
                    row.LastRenderKey = renderKey;
                }
            }

            SetLabelTextIfChanged(row.PageStatusLabel, statusText);
            if (row.PageStatusLabel != null)
                row.PageStatusLabel.ForeColor = Color.FromArgb(180, 180, 200);
        }

        private void RenderCgdbUnit(CGDBBoardControl boardControl, string trainNumber, string coachText, bool isHindi, bool isTrainNoPage)
        {
            if (boardControl == null) return;

            int width = BOARD_WIDTH;
            int height = BOARD_HEIGHT;
            bool[,] matrix = new bool[height, width];
            const int scale = 4;

            using (Bitmap bmp = new Bitmap(width * scale, height * scale, PixelFormat.Format32bppArgb))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Black);
                g.SmoothingMode = SmoothingMode.None;
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = PixelOffsetMode.None;
                g.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;

                using (var brush = new SolidBrush(Color.White))
                using (var sf = new StringFormat(StringFormat.GenericTypographic))
                {
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    RectangleF boardRect = new RectangleF(0, 0, bmp.Width, bmp.Height);

                    if (isTrainNoPage)
                    {
                        // Page 1: Show ONLY the Train Number centered across the full board
                        string tNo = !string.IsNullOrWhiteSpace(trainNumber) ? trainNumber.Trim() : "----";
                        int fontSize = 22;
                        if (tNo.Length > 5) fontSize = 18;
                        if (tNo.Length > 7) fontSize = 15;

                        using (Font fontTrain = new Font("Arial", fontSize * scale, FontStyle.Bold, GraphicsUnit.Pixel))
                        {
                            g.DrawString(tNo, fontTrain, brush, boardRect, sf);
                        }
                    }
                    else
                    {
                        // Page 2 or 3: Show ONLY the Coach Code across the full board
                        string fontFam = isHindi ? "Nirmala UI" : "Arial";
                        int coachFontSize = 24;
                        if (coachText != null)
                        {
                            if (coachText.Length > 3) coachFontSize = 19;
                            if (coachText.Length > 5) coachFontSize = 15;
                        }

                        using (Font fontCoach = new Font(fontFam, coachFontSize * scale, FontStyle.Bold, GraphicsUnit.Pixel))
                        {
                            g.DrawString(coachText ?? string.Empty, fontCoach, brush, boardRect, sf);
                        }
                    }
                }

                ConvertBitmapToMatrix(bmp, matrix, width, height, scale);
            }

            boardControl.SetLEDMatrix(matrix);
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
                            if (py >= bmp.Height) continue;
                            int rowOffset = py * stride;

                            for (int sx = 0; sx < scale; sx++)
                            {
                                int px = cellX * scale + sx;
                                if (px >= bmp.Width) continue;
                                int idx = rowOffset + px * bytesPerPixel;
                                if (idx + 2 >= pixels.Length) continue;

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
    }

    public class CGDBBoardControl : Panel
    {
        private bool[,] ledMatrix;
        private int width;
        private int height;
        private int pixelSize;
        private const int LED_SPACING = 1;

        public Color OnColor { get; set; } = Color.FromArgb(76, 175, 80);
        public Color OffColor { get; set; } = Color.FromArgb(35, 35, 45);

        public CGDBBoardControl(int boardWidth, int boardHeight, int pixelSize)
        {
            this.width = boardWidth;
            this.height = boardHeight;
            this.pixelSize = pixelSize;
            this.ledMatrix = new bool[boardHeight, boardWidth];

            int totalW = boardWidth * (pixelSize + LED_SPACING);
            int totalH = boardHeight * (pixelSize + LED_SPACING);
            this.Size = new Size(totalW, totalH);

            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
            this.BackColor = Color.FromArgb(14, 14, 20);
        }

        public void SetLEDMatrix(bool[,] matrix)
        {
            if (matrix != null && matrix.GetLength(0) == height && matrix.GetLength(1) == width)
            {
                ledMatrix = (bool[,])matrix.Clone();
                this.Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (var bgBrush = new SolidBrush(Color.FromArgb(14, 14, 20)))
            {
                g.FillRectangle(bgBrush, this.ClientRectangle);
            }

            using (var borderPen = new Pen(Color.FromArgb(45, 50, 65), 1))
            {
                g.DrawRectangle(borderPen, 0, 0, this.Width - 1, this.Height - 1);
            }

            int ledSize = pixelSize;
            int spacing = LED_SPACING;
            int cellStep = ledSize + spacing;

            for (int row = 0; row < height; row++)
            {
                int y = row * cellStep;
                for (int col = 0; col < width; col++)
                {
                    int x = col * cellStep;
                    bool isActive = ledMatrix[row, col];
                    DrawMaterialLED(g, x, y, ledSize, isActive);
                }
            }
        }

        private void DrawMaterialLED(Graphics g, int x, int y, int size, bool isActive)
        {
            if (isActive)
            {
                Color accentColor = OnColor;
                Color glowColor = Color.FromArgb(65, OnColor);

                // Glow effect
                using (var glowBrush = new SolidBrush(glowColor))
                {
                    int glowSize = size + 4;
                    g.FillEllipse(glowBrush, x - 2, y - 2, glowSize, glowSize);
                }

                // LED body
                using (var ledBrush = new SolidBrush(accentColor))
                {
                    g.FillEllipse(ledBrush, x, y, size, size);
                }

                // Highlight
                using (var highlightBrush = new SolidBrush(Color.FromArgb(130, 255, 255, 255)))
                {
                    int hlSize = Math.Max(2, size / 3);
                    g.FillEllipse(highlightBrush, x + 2, y + 1, hlSize, hlSize);
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

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Override to prevent flickering
        }
    }
}
