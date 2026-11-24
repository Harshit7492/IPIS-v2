using DataBaseHandler;
using DataModel;
using MaterialSkin;
using MaterialSkin.Controls;
using System.Drawing.Drawing2D;


namespace IPISv2.HealthStatusReport
{
    public partial class LEDSORTCoachGuidenceDisplayBoard : UserControl
    {
        public const int BOARD_WIDTH = 96;
        public const int BOARD_HEIGHT = 32;

        private bool isOnline = true;
        private System.Windows.Forms.Timer _refreshTimer;
        private int _calculatedHeight = 460;
        private bool _isLayingOut = false;
        private bool _isInitialized = false;

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
            public string FaultString { get; set; } = string.Empty;
        }

        private class CgdbPlatformRow
        {
            public int BoardId { get; set; }
            public string BoardName { get; set; }
            public string PlatformNumber { get; set; }
            public string BoardIp { get; set; }
            public int DelayTime { get; set; }
            public string TrainNumber { get; set; }
            public string TrainName { get; set; }
            public List<CgdbUnitInfo> Units { get; set; } = new List<CgdbUnitInfo>();
            public int SelectedUnitIndex { get; set; } = 0;

            public Panel HeaderPanel { get; set; }
            public MaterialLabel TitleLabel { get; set; }
            public Label DropdownLabel { get; set; }
            public ComboBox IpComboBox { get; set; }
            public Panel PreviewPanel { get; set; }

            // Card controls for the selected CGDB unit
            public Panel BoardCard { get; set; }
            public Label CoachHeaderLabel { get; set; }
            public Label IpBadgeLabel { get; set; }
            public Label FaultStatusLabel { get; set; }
            public LEDSORTCGDBBoardPreview BoardPreview { get; set; }
        }

        private readonly List<CgdbPlatformRow> _platformRows = new List<CgdbPlatformRow>();
        private readonly Dictionary<string, string> _coachLegends = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, string> _faultDataCache = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public LEDSORTCoachGuidenceDisplayBoard()
        {
            InitializeComponent();
            if (IsInDesignMode()) return;

            // Hide fallback designer controls
            if (lblTitle != null) lblTitle.Visible = false;
            if (panelPreviewScroll != null) panelPreviewScroll.Visible = false;

            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);
            DoubleBuffered = true;
            BackColor = Color.White;

            this.Load += LEDSORTCoachGuidenceDisplayBoard_Load;
        }

        private void LEDSORTCoachGuidenceDisplayBoard_Load(object sender, EventArgs e)
        {
            if (_isInitialized) return;

            LoadCoachLegends();
            LoadActiveBoardData();

            _refreshTimer = new System.Windows.Forms.Timer();
            _refreshTimer.Interval = 10000;
            _refreshTimer.Tick += RefreshTimer_Tick;
            _refreshTimer.Start();

            this.Disposed += (s, ev) =>
            {
                _refreshTimer?.Stop();
                _refreshTimer?.Dispose();
            };

            this.Resize += (s, ev) => SyncLayoutWidths();
            _isInitialized = true;
        }

        private void SyncLayoutWidths()
        {
            int targetWidth = Math.Max(100, this.Width - 34);
            foreach (var row in _platformRows)
            {
                if (row.HeaderPanel != null && !row.HeaderPanel.IsDisposed)
                {
                    row.HeaderPanel.Width = targetWidth;

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

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            LoadActiveBoardData();
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
                LogManager.LoggerService.Logger.Error(ex, "Error loading coach legends in LEDSORT CGDB.");
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

        private List<APILiveDataLog> LoadHSRRows()
        {
            try
            {
                var rows = new List<APILiveDataLog>();
                rows.AddRange(_dbService.GetHSRList("CGDB") ?? new List<APILiveDataLog>());
                rows.AddRange(_dbService.GetHSRList("PDC") ?? new List<APILiveDataLog>());
                return rows;
            }
            catch (Exception)
            {
                return new List<APILiveDataLog>();
            }
        }

        private string GetFaultStringForIp(string ipAddress, List<APILiveDataLog> hsrRows)
        {
            string ip = ipAddress?.Trim();
            APILiveDataLog log = null;

            if (!string.IsNullOrEmpty(ip) && hsrRows != null)
            {
                log = hsrRows
                    .Where(h => h != null &&
                        (string.Equals(h.DeviceIP?.Trim(), ip, StringComparison.OrdinalIgnoreCase) ||
                         string.Equals(h.DisplayIP?.Trim(), ip, StringComparison.OrdinalIgnoreCase)))
                    .OrderByDescending(h => h.LogTime)
                    .FirstOrDefault();
            }

            if (log != null && !string.IsNullOrWhiteSpace(log.HSR))
                return IsHsrAllOk(log.HSR) ? string.Empty : log.HSR.Trim();

            return string.Empty;
        }

        private static bool IsHsrAllOk(string hsr)
        {
            string letters = new string(hsr.Where(char.IsLetter).ToArray());
            return letters.Equals("ALLOK", StringComparison.OrdinalIgnoreCase);
        }

        public void LoadActiveBoardData()
        {
            try
            {
                var allSettings = _dbService.GET_BOARD_SETTING(0) ?? new List<BoardSettingInfo>();
                var installedSettings = allSettings.Where(b => b.IsActive == 1 && b.BOARD_NAME != null &&
                    (b.BOARD_NAME.IndexOf("Coach Guidance", StringComparison.OrdinalIgnoreCase) >= 0 ||
                     b.BOARD_NAME.IndexOf("CGDB", StringComparison.OrdinalIgnoreCase) >= 0 ||
                     b.BOARD_NAME.IndexOf("CoachGuidance", StringComparison.OrdinalIgnoreCase) >= 0)).ToList();

                var updatedTrains = _dbService.GET_UPDATED_TRAINS_DATA(0) ?? new List<train_updated_data>();
                var hsrRows = LoadHSRRows();

                var platformDataList = new List<CgdbPlatformRow>();

                if (installedSettings.Count > 0)
                {
                    foreach (var setting in installedSettings)
                    {
                        string pfStr = !string.IsNullOrWhiteSpace(setting.PF_NO) ? setting.PF_NO.Trim() : "1";
                        if (pfStr == "0") pfStr = "1";

                        train_updated_data matchingTrain = updatedTrains.FirstOrDefault(t =>
                            (string.Equals(t.ACTUAL_PLATFORM_NUMBER?.Trim(), pfStr, StringComparison.OrdinalIgnoreCase) ||
                             string.Equals(t.SCHEDULE_PLATFORM_NUMBER?.Trim(), pfStr, StringComparison.OrdinalIgnoreCase)) &&
                            t.CGDB_SEND_DATA == 1);

                        // Priority 2: ANY train with CGDB_SEND_DATA == 1
                        if (matchingTrain == null)
                        {
                            matchingTrain = updatedTrains.FirstOrDefault(t => t.CGDB_SEND_DATA == 1);
                        }

                        // Priority 3: Only when no train has CGDB_SEND_DATA == 1, fall back to matching platform
                        if (matchingTrain == null)
                        {
                            matchingTrain = updatedTrains.FirstOrDefault(t =>
                                string.Equals(t.ACTUAL_PLATFORM_NUMBER?.Trim(), pfStr, StringComparison.OrdinalIgnoreCase) ||
                                string.Equals(t.SCHEDULE_PLATFORM_NUMBER?.Trim(), pfStr, StringComparison.OrdinalIgnoreCase));
                        }

                        List<string> coaches = new List<string>();
                        if (matchingTrain != null && !string.IsNullOrWhiteSpace(matchingTrain.COACH_SEQUENCE))
                        {
                            coaches = ParseCoaches(matchingTrain.COACH_SEQUENCE);
                            if (matchingTrain.trainReversalFlag)
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
                                if (parts.Length > 0) port = parts[0];
                                if (parts.Length > 1) coachPort = parts[1];
                            }

                            string unitIp = $"10.0.{pfStr}.{coachPort}";
                            string coachCode = (i < coaches.Count) ? coaches[i] : $"C{i + 1}";
                            string coachHindi = GetCoachHindi(coachCode);

                            string fault = GetFaultStringForIp(unitIp, hsrRows);
                            if (string.IsNullOrEmpty(fault) && totalUnits == 1)
                            {
                                fault = GetFaultStringForIp(setting.BOARD_IP, hsrRows);
                            }
                            _faultDataCache[unitIp] = fault;

                            unitList.Add(new CgdbUnitInfo
                            {
                                UnitIndex = i + 1,
                                Port = port,
                                CoachPort = coachPort,
                                UnitIp = unitIp,
                                CoachCode = coachCode,
                                CoachHindi = coachHindi,
                                TrainNumber = matchingTrain != null ? matchingTrain.TRAIN_NUMBER : "----",
                                TrainName = matchingTrain != null ? matchingTrain.TRAIN_NAME_ENGLISH : string.Empty,
                                PlatformNumber = pfStr,
                                FaultString = fault
                            });
                        }

                        platformDataList.Add(new CgdbPlatformRow
                        {
                            BoardId = setting.BOARD_ID,
                            BoardName = setting.BOARD_NAME,
                            PlatformNumber = pfStr,
                            BoardIp = setting.BOARD_IP,
                            DelayTime = setting.DELAYTIME > 0 ? setting.DELAYTIME : 10,
                            TrainNumber = matchingTrain != null ? matchingTrain.TRAIN_NUMBER : string.Empty,
                            TrainName = matchingTrain != null ? matchingTrain.TRAIN_NAME_ENGLISH : string.Empty,
                            Units = unitList
                        });
                    }
                }
                else
                {
                    // Fallback demo board if no CGDB board is configured
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
                        string unitIp = $"10.0.1.{i + 2}";
                        string fault = GetFaultStringForIp(unitIp, hsrRows);
                        _faultDataCache[unitIp] = fault;

                        unitList.Add(new CgdbUnitInfo
                        {
                            UnitIndex = i + 1,
                            Port = "2",
                            CoachPort = (i + 2).ToString(),
                            UnitIp = unitIp,
                            CoachCode = coachCode,
                            CoachHindi = GetCoachHindi(coachCode),
                            TrainNumber = matchingTrain != null ? matchingTrain.TRAIN_NUMBER : "----",
                            TrainName = matchingTrain != null ? matchingTrain.TRAIN_NAME_ENGLISH : string.Empty,
                            PlatformNumber = "1",
                            FaultString = fault
                        });
                    }

                 
                }

                UpdateDynamicUI(platformDataList);
            }
            catch (Exception ex)
            {
                LogManager.LoggerService.Logger.Error(ex, "Error loading LEDSORT CGDB data.");
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

                for (int p = 0; p < _platformRows.Count && p < newPlatforms.Count; p++)
                {
                    var existingRow = _platformRows[p];
                    var newData = newPlatforms[p];

                    existingRow.TrainNumber = newData.TrainNumber;
                    existingRow.TrainName = newData.TrainName;
                    existingRow.Units = newData.Units;
                    existingRow.DelayTime = newData.DelayTime;

                    string titleText;
                    if (!string.IsNullOrWhiteSpace(existingRow.TrainNumber))
                    {
                        titleText = $"Coach Guidance Display Board (CGDB) — Platform: {existingRow.PlatformNumber} — Train: {existingRow.TrainNumber} {existingRow.TrainName}";
                    }
                    else
                    {
                        titleText = $"Coach Guidance Display Board (CGDB) — Platform: {existingRow.PlatformNumber} — No Active Train";
                    }

                    if (existingRow.TitleLabel != null && existingRow.TitleLabel.Text != titleText)
                    {
                        existingRow.TitleLabel.Text = titleText;
                    }

                    SyncDropdownItems(existingRow);
                    UpdateSingleBoardView(existingRow);
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

                // Preview panel containing the selected CGDB board card
                int boardW = 960;
                int boardH = 320;
                int cardPadding = 12;
                int headerH = 38;
                int cardW = boardW + (cardPadding * 2); // 984 px
                int cardH = headerH + boardH + cardPadding + 4; // 374 px
                int previewH = cardH + 16; // 390 px

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

                var boardCard = new Panel
                {
                    Location = new Point(12, 8),
                    Size = new Size(cardW, cardH),
                    BackColor = Color.FromArgb(24, 24, 34)
                };
                boardCard.Paint += (s, e) =>
                {
                    using (var pen = new Pen(Color.FromArgb(45, 50, 65), 1.5f))
                    {
                        e.Graphics.DrawRectangle(pen, 0, 0, boardCard.Width - 1, boardCard.Height - 1);
                    }
                };

                var lblIpBadge = new Label
                {
                    Text = "IP: 10.0.1.2",
                    Font = new Font("Roboto", 11F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(100, 210, 255),
                    Location = new Point(12, 9),
                    AutoSize = true
                };
                boardCard.Controls.Add(lblIpBadge);



                var boardPreview = new LEDSORTCGDBBoardPreview
                {
                    Location = new Point(cardPadding, headerH),
                    Size = new Size(boardW, boardH),
                    FaultColor = Color.FromArgb(244, 67, 54),
                    OffColor = Color.FromArgb(60, 60, 60),
                    OnColor = Color.White
                };
                boardCard.Controls.Add(boardPreview);

                previewPanel.Controls.Add(boardCard);
                currentY += previewPanel.Height + 16;

                var platformRow = new CgdbPlatformRow
                {
                    BoardId = platformData.BoardId,
                    BoardName = platformData.BoardName,
                    PlatformNumber = platformData.PlatformNumber,
                    BoardIp = platformData.BoardIp,
                    DelayTime = platformData.DelayTime,
                    HeaderPanel = headerPanel,
                    TitleLabel = titleLabel,
                    DropdownLabel = lblSelectIp,
                    IpComboBox = cmbIp,
                    PreviewPanel = previewPanel,
                    BoardCard = boardCard,
                    CoachHeaderLabel = null,
                    IpBadgeLabel = lblIpBadge,
                    BoardPreview = boardPreview,
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
                        UpdateSingleBoardView(platformRow);
                    }
                };

                _platformRows.Add(platformRow);
                SyncDropdownItems(platformRow);
                UpdateSingleBoardView(platformRow);
            }

            _calculatedHeight = currentY + 10;
            this.Height = _calculatedHeight;
            this.ResumeLayout(true);
        }

        private void UpdateSingleBoardView(CgdbPlatformRow row)
        {
            if (row.Units == null || row.Units.Count == 0 || row.BoardPreview == null || row.BoardPreview.IsDisposed)
                return;

            int idx = row.SelectedUnitIndex;
            if (idx < 0 || idx >= row.Units.Count) idx = 0;
            row.SelectedUnitIndex = idx;

            var unit = row.Units[idx];

            if (row.IpBadgeLabel != null)
            {
                row.IpBadgeLabel.Text = $"IP: {unit.UnitIp}";
            }

            // Render LEDSORT frame with all LEDs white, and faulty LEDs in red + red rings
            RenderBoardFrame(row.BoardPreview, unit.FaultString);

            if (row.FaultStatusLabel != null)
            {
                if (row.BoardPreview.FaultCount > 0)
                {
                    row.FaultStatusLabel.Text = $"Faults: {row.BoardPreview.FaultCount} ({row.BoardPreview.ShortCount} Short, {row.BoardPreview.OpenCount} Open)";
                    row.FaultStatusLabel.ForeColor = Color.FromArgb(244, 67, 54);
                    if (row.IpBadgeLabel != null)
                    {
                        row.IpBadgeLabel.ForeColor = Color.FromArgb(244, 67, 54);
                    }
                }
                else
                {
                    row.FaultStatusLabel.Text = "Status: ALL OK • 0 Faults";
                    row.FaultStatusLabel.ForeColor = Color.FromArgb(76, 175, 80);
                    if (row.IpBadgeLabel != null)
                    {
                        row.IpBadgeLabel.ForeColor = Color.FromArgb(100, 210, 255);
                    }
                }
            }
        }

        /// <summary>
        /// Renders the board frame with all LEDs white, and fault LEDs red.
        /// </summary>
        private void RenderBoardFrame(LEDSORTCGDBBoardPreview preview, string faultString)
        {
            if (preview == null || preview.IsDisposed) return;

            int width = BOARD_WIDTH;
            int height = BOARD_HEIGHT;

            // Create matrix with all LEDs ON (white)
            bool[,] matrix = new bool[height, width];
            for (int r = 0; r < height; r++)
            {
                for (int c = 0; c < width; c++)
                {
                    matrix[r, c] = true;
                }
            }

            preview.SetLEDMatrix(matrix);

            if (!string.IsNullOrEmpty(faultString))
            {
                preview.SetFaults(faultString);
            }
            else
            {
                preview.ClearFaults();
            }

            preview.Invalidate();
        }

        public void UpdateHealthStatus(bool online, double temperatureCelsius, double voltage, TimeSpan uptime)
        {
            isOnline = online;
            if (boardPreview != null && !boardPreview.IsDisposed)
            {
                boardPreview.Enabled = online;
            }
            foreach (var row in _platformRows)
            {
                if (row != null && row.BoardPreview != null && !row.BoardPreview.IsDisposed)
                {
                    row.BoardPreview.Enabled = online;
                }
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

    /// <summary>
    /// Renders a Coach Guidance Display Board (CGDB) LED matrix (96 columns x 32 rows).
    /// Normal LEDs are white, fault LEDs are red with a prominent red ring.
    /// </summary>
    public class LEDSORTCGDBBoardPreview : Panel
    {
        private bool[,] ledMatrix;
        private bool[,] faultMatrix;
        private int rows = 32;
        private int cols = 96;

        public Color OnColor { get; set; } = Color.White;
        public Color OffColor { get; set; } = Color.FromArgb(60, 60, 60);
        public Color FaultColor { get; set; } = Color.FromArgb(244, 67, 54);

        // true: HSR "R1C1" is the first LED (top-left). false: "R0C0" is the first LED.
        public bool OneBasedAddress { get; set; } = true;

        public float LedSizeMultiplier { get; set; } = 1.0f;

        public int FaultCount { get; private set; } = 0;
        public int ShortCount { get; private set; } = 0;
        public int OpenCount { get; private set; } = 0;

        public LEDSORTCGDBBoardPreview()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            ResizeRedraw = true;
            ledMatrix = new bool[rows, cols];
            faultMatrix = new bool[rows, cols];
            Size = new Size(960, 320);
        }

        public void SetLEDMatrix(bool[,] matrix)
        {
            if (matrix != null && matrix.GetLength(0) == rows && matrix.GetLength(1) == cols)
            {
                ledMatrix = (bool[,])matrix.Clone();
                Invalidate();
            }
        }

        /// <summary>
        /// Sets fault LEDs based on fault string format: "S-L1R10C15#O-L1R20C50"
        /// S = Short circuit fault
        /// O = Open circuit fault
        /// L = Line number
        /// R = Row number
        /// C = Column number
        /// </summary>
        public void SetFaults(string faultString)
        {
            faultMatrix = new bool[rows, cols];
            FaultCount = 0;
            ShortCount = 0;
            OpenCount = 0;

            if (string.IsNullOrEmpty(faultString))
                return;

            string[] faults = faultString.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string fault in faults)
            {
                string trimmedFault = fault.Trim();
                if (trimmedFault.Length < 6) continue;

                char faultType = char.ToUpperInvariant(trimmedFault[0]);

                int dashIndex = trimmedFault.IndexOf('-');
                if (dashIndex < 0) continue;

                string address = trimmedFault.Substring(dashIndex + 1).Trim();

                int line = 1;
                int row = 0;
                int col = 0;

                int lIndex = address.IndexOf('L');
                int rIndex = address.IndexOf('R');
                int cIndex = address.IndexOf('C');

                if (rIndex >= 0 && cIndex > rIndex)
                {
                    if (lIndex >= 0 && rIndex > lIndex)
                    {
                        string lineStr = address.Substring(lIndex + 1, rIndex - lIndex - 1);
                        int.TryParse(lineStr, out line);
                    }

                    string rowStr = address.Substring(rIndex + 1, cIndex - rIndex - 1);
                    if (int.TryParse(rowStr, out row))
                    {
                        string colStr = address.Substring(cIndex + 1);
                        if (int.TryParse(colStr, out col))
                        {
                            if (OneBasedAddress)
                            {
                                row -= 1;
                                col -= 1;
                            }

                            if (line == 1 && row >= 0 && row < rows && col >= 0 && col < cols)
                            {
                                if (!faultMatrix[row, col])
                                {
                                    faultMatrix[row, col] = true;
                                    FaultCount++;
                                    if (faultType == 'S') ShortCount++;
                                    else if (faultType == 'O') OpenCount++;
                                }
                            }
                        }
                    }
                }
            }

            Invalidate();
        }

        public void ClearFaults()
        {
            faultMatrix = new bool[rows, cols];
            FaultCount = 0;
            ShortCount = 0;
            OpenCount = 0;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            using (var bgBrush = new SolidBrush(Color.FromArgb(15, 15, 20)))
            {
                g.FillRectangle(bgBrush, ClientRectangle);
            }

            float pixelW = (float)Width / cols;
            float pixelH = (float)Height / rows;

            float size = Math.Min(pixelW, pixelH) * 0.85f * LedSizeMultiplier;
            if (size < 2.0f) size = 2.0f;

            float totalWidth = size * cols;
            float totalHeight = size * rows;
            float startX = (Width - totalWidth) / 2;
            float startY = (Height - totalHeight) / 2;

            // Draw horizontal module dividers every 16 rows
            for (int row = 0; row <= rows; row += 16)
            {
                if (row > 0 && row < rows)
                {
                    float dividerY = startY + row * size;
                    using (Pen dividerPen = new Pen(Color.FromArgb(30, 100, 100, 120), 1.5f))
                    {
                        g.DrawLine(dividerPen, startX, dividerY, startX + totalWidth, dividerY);
                    }
                }
            }

            // Draw vertical module dividers every 16 columns
            for (int col = 0; col <= cols; col += 16)
            {
                if (col > 0 && col < cols)
                {
                    float dividerX = startX + col * size;
                    using (Pen dividerPen = new Pen(Color.FromArgb(30, 100, 100, 120), 1.5f))
                    {
                        g.DrawLine(dividerPen, dividerX, startY, dividerX, startY + totalHeight);
                    }
                }
            }

            // Draw LEDs
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    float x = startX + col * size;
                    float y = startY + row * size;

                    bool isActive = ledMatrix != null && ledMatrix[row, col];
                    bool isFault = faultMatrix != null && faultMatrix[row, col];

                    if (isActive)
                    {
                        if (isFault)
                        {
                            // Fault LED - Red with glow
                            using (var glowBrush = new SolidBrush(Color.FromArgb(80, FaultColor)))
                            {
                                float glowSize = size * 1.8f;
                                g.FillEllipse(glowBrush, x - (glowSize - size) / 2f, y - (glowSize - size) / 2f, glowSize, glowSize);
                            }
                            using (var ledBrush = new SolidBrush(FaultColor))
                            {
                                g.FillEllipse(ledBrush, x, y, size, size);
                            }
                            if (size > 3)
                            {
                                using (var highlightBrush = new SolidBrush(Color.FromArgb(80, Color.White)))
                                {
                                    g.FillEllipse(highlightBrush, x + size * 0.15f, y + size * 0.15f, size * 0.3f, size * 0.3f);
                                }
                            }
                        }
                        else
                        {
                            // Normal LED - White with subtle glow
                            using (var glowBrush = new SolidBrush(Color.FromArgb(40, Color.White)))
                            {
                                float glowSize = size * 1.8f;
                                g.FillEllipse(glowBrush, x - (glowSize - size) / 2f, y - (glowSize - size) / 2f, glowSize, glowSize);
                            }
                            using (var ledBrush = new SolidBrush(OnColor))
                            {
                                g.FillEllipse(ledBrush, x, y, size, size);
                            }
                            if (size > 3)
                            {
                                using (var highlightBrush = new SolidBrush(Color.FromArgb(60, Color.White)))
                                {
                                    g.FillEllipse(highlightBrush, x + size * 0.15f, y + size * 0.15f, size * 0.3f, size * 0.3f);
                                }
                            }
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

            // Red ring around each faulty LED so a single bad LED is easy to spot on the board
            if (faultMatrix != null)
            {
                using (var ringPen = new Pen(FaultColor, 2f))
                {
                    for (int row = 0; row < rows; row++)
                    {
                        for (int col = 0; col < cols; col++)
                        {
                            if (!faultMatrix[row, col]) continue;
                            float cx = startX + col * size + size / 2f;
                            float cy = startY + row * size + size / 2f;
                            float r = size * 1.6f;
                            g.DrawEllipse(ringPen, cx - r, cy - r, r * 2, r * 2);
                        }
                    }
                }
            }

            // Border around board
            using (var borderPen = new Pen(Color.FromArgb(45, 50, 65), 1))
            {
                g.DrawRectangle(borderPen, 0, 0, Width - 1, Height - 1);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Override to prevent flickering
        }
    }
}
