using DataBaseHandler;
using DataModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace IPISv2.HealthStatusReport
{
    public partial class LEDSORTGenericDisplayBoard : UserControl
    {
        private bool isOnline = true;
        private List<SingleLineDisplayBoardData> _boardList = new List<SingleLineDisplayBoardData>();
        private System.Windows.Forms.Timer _refreshTimer;
        private int _calculatedHeight = 380;
        private bool _isInitialized = false;

        // Store fault data separately (key: BoardId, value: FaultData string)
        private Dictionary<int, string> _faultDataCache = new Dictionary<int, string>();

        // Dynamic UI controls
        private class BoardRowControls
        {
            public SingleLineDisplayBoardData BoardData { get; set; }
            public MaterialSkin.Controls.MaterialLabel TitleLabel { get; set; }
            public Panel ScrollPanel { get; set; }
            public LEDSORTAGDBBoardPreview BoardPreview { get; set; }
        }

        private readonly List<BoardRowControls> _dynamicRows = new List<BoardRowControls>();

        public LEDSORTGenericDisplayBoard()
        {
            InitializeComponent();
            if (IsInDesignMode()) return;

            if (lblTitle != null) lblTitle.Visible = false;
            if (panelPreviewScroll != null) panelPreviewScroll.Visible = false;

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

            _isInitialized = true;
            this.Load += GenericDisplayBoard_Load;
        }

        private void GenericDisplayBoard_Load(object sender, EventArgs e)
        {
            if (_isInitialized) return;

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

            _isInitialized = true;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (this.Height != _calculatedHeight)
            {
                this.Height = _calculatedHeight;
            }
            // Update board sizes when form resizes
            if (_isInitialized)
            {
                UpdateBoardSizes();
            }
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            LoadActiveBoardData();
        }

        /// <summary>
        /// Loads HSR fault data from database for AGDB devices
        /// </summary>
        private List<string> LoadHSRFaultData()
        {
            try
            {
                // Get HSR list from database for AGDB devices
                List<APILiveDataLog> hsrDataList = _dbService.GetHSRList("AGDB").ToList();

                // Extract HSR values and filter out null or empty
                List<string> hsrList = hsrDataList
                    .Where(h => h != null && !string.IsNullOrEmpty(h.HSR))
                    .Select(h => h.HSR)
                    .ToList();

                return hsrList;
            }
            catch (Exception ex)
            {
                // Log error if needed
                return new List<string>();
            }
        }

        private string ConvertHSRListToFaultString(List<string> hsrList)
        {
            if (hsrList == null || hsrList.Count == 0)
                return string.Empty;

            return string.Join("#", hsrList);
        }

        /// <summary>
        /// Loads the AGDB rows of API_LIVE_DATA_LOG once per refresh.
        /// </summary>
        private List<APILiveDataLog> LoadHSRRows()
        {
            try
            {
                return _dbService.GetHSRList("AGDB") ?? new List<APILiveDataLog>();
            }
            catch (Exception)
            {
                return new List<APILiveDataLog>();
            }
        }

        /// <summary>
        /// Returns the HSR fault string of one board, matched on its IP
        /// (DeviceIP when the board posts to the CDC, DisplayIP when it posts through a PDC).
        /// "ALLOK" / "All OK" / "AllOK" means no faults. No row means no faults shown.
        /// </summary>
        private string GetFaultStringForBoard(string boardIp, List<APILiveDataLog> hsrRows)
        {
            string ip = boardIp?.Trim();
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
                var installedSettings = allSettings
                    .Where(b => b.BOARD_NAME != null && (
                        b.BOARD_NAME.IndexOf("At a Glance", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        b.BOARD_NAME.IndexOf("At-a-Glance", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        b.BOARD_NAME.IndexOf("AGDB", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        b.BOARD_NAME.IndexOf("AtAGlance", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        b.BOARD_NAME.IndexOf("Generic", StringComparison.OrdinalIgnoreCase) >= 0))
                    .ToList();

                var activeTrains = _dbService.GetAllActiveBoards() ?? new List<SingleLineDisplayBoardData>();

                // Load HSR fault data using your method
                //var hsrFaultList = LoadHSRFaultData();
                //string faultDataToUse = ConvertHSRListToFaultString(hsrFaultList);

                // Each board now gets only its own HSR (matched by IP), not all boards joined together
                var hsrRows = LoadHSRRows();

                var tempBoardList = new List<SingleLineDisplayBoardData>();

                if (installedSettings.Count > 0)
                {
                    foreach (var setting in installedSettings)
                    {
                        var match = activeTrains.FirstOrDefault(t =>
                            t.BoardId == setting.BOARD_ID ||
                            (string.Equals(t.BoardIp?.Trim(), setting.BOARD_IP?.Trim(), StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(setting.BOARD_IP)));

                        if (match != null)
                        {
                            match.BoardName = setting.BOARD_NAME;
                            match.BoardLocation = setting.BOARD_LOCATION;
                            match.BoardIp = setting.BOARD_IP;
                            match.FontSize = setting.FONT_SIZE;
                            match.DelayTime = setting.DELAYTIME;
                            match.BoardLine = setting.BOARD_LINE.ToString();
                            if (string.IsNullOrEmpty(match.PlatformNumber))
                            {
                                match.PlatformNumber = !string.IsNullOrWhiteSpace(setting.SELECTED_PF) ? setting.SELECTED_PF : setting.PF_NO;
                            }

                            //_faultDataCache[match.BoardId] = faultDataToUse;
                            _faultDataCache[match.BoardId] = GetFaultStringForBoard(setting.BOARD_IP, hsrRows);
                            tempBoardList.Add(match);
                        }
                        else
                        {
                            var newBoard = new SingleLineDisplayBoardData
                            {
                                BoardId = setting.BOARD_ID,
                                BoardName = setting.BOARD_NAME,
                                BoardIp = setting.BOARD_IP,
                                BoardLocation = setting.BOARD_LOCATION,
                                BoardLine = setting.BOARD_LINE.ToString(),
                                IsActive = setting.IsActive == 1,
                                FontSize = setting.FONT_SIZE,
                                DelayTime = setting.DELAYTIME,
                                TrainNumber = string.Empty,
                                PlatformNumber = !string.IsNullOrWhiteSpace(setting.SELECTED_PF) ? setting.SELECTED_PF : setting.PF_NO
                            };

                            //_faultDataCache[newBoard.BoardId] = faultDataToUse;
                            _faultDataCache[newBoard.BoardId] = GetFaultStringForBoard(setting.BOARD_IP, hsrRows);
                            tempBoardList.Add(newBoard);
                        }
                    }
                }
                else
                {
                    var agdbTrains = activeTrains
                        .Where(b => b.BoardName != null && (
                            b.BoardName.IndexOf("At a Glance", StringComparison.OrdinalIgnoreCase) >= 0 ||
                            b.BoardName.IndexOf("At-a-Glance", StringComparison.OrdinalIgnoreCase) >= 0 ||
                            b.BoardName.IndexOf("AGDB", StringComparison.OrdinalIgnoreCase) >= 0 ||
                            b.BoardName.IndexOf("AtAGlance", StringComparison.OrdinalIgnoreCase) >= 0 ||
                            b.BoardName.IndexOf("Generic", StringComparison.OrdinalIgnoreCase) >= 0))
                        .ToList();

                    foreach (var train in agdbTrains)
                    {
                        //_faultDataCache[train.BoardId] = faultDataToUse;
                        _faultDataCache[train.BoardId] = GetFaultStringForBoard(train.BoardIp, hsrRows);
                    }
                    tempBoardList = agdbTrains;
                }

                _boardList = tempBoardList;
                UpdateDynamicUI();
            }
            catch (Exception)
            {
                _boardList = new List<SingleLineDisplayBoardData>();
                UpdateDynamicUI();
            }
        }

        private void UpdateBoardSizes()
        {
            if (_boardList == null || _boardList.Count == 0)
            {
                if (_dynamicRows.Count > 0)
                {
                    var row = _dynamicRows[0];
                    if (row.BoardPreview != null && !row.BoardPreview.IsDisposed)
                    {
                        string faultData = string.Empty;
                        if (_faultDataCache.Count > 0)
                        {
                            faultData = _faultDataCache.Values.FirstOrDefault() ?? string.Empty;
                        }
                        RenderAGDBFrame(row, faultData);
                    }
                }
            }
            else
            {
                for (int i = 0; i < _boardList.Count && i < _dynamicRows.Count; i++)
                {
                    var row = _dynamicRows[i];
                    if (row.BoardPreview != null && !row.BoardPreview.IsDisposed)
                    {
                        string faultData = string.Empty;
                        if (_faultDataCache.TryGetValue(_boardList[i].BoardId, out string cachedFault))
                        {
                            faultData = cachedFault;
                        }
                        RenderAGDBFrame(row, faultData);
                    }
                }
            }
        }

        private void UpdateDynamicUI()
        {
            bool needsRecreate = false;

            if (_boardList == null || _boardList.Count == 0)
            {
                if (_dynamicRows.Count != 1 || _dynamicRows[0].BoardData != null)
                {
                    needsRecreate = true;
                }
            }
            else
            {
                if (_dynamicRows.Count != _boardList.Count)
                {
                    needsRecreate = true;
                }
                else
                {
                    for (int i = 0; i < _boardList.Count; i++)
                    {
                        if (_dynamicRows[i].BoardData == null || _dynamicRows[i].BoardData.BoardId != _boardList[i].BoardId)
                        {
                            needsRecreate = true;
                            break;
                        }
                    }
                }
            }

            if (needsRecreate)
            {
                RecreateBoardControls();
            }

            if (_boardList == null || _boardList.Count == 0)
            {
                if (_dynamicRows.Count > 0)
                {
                    var row = _dynamicRows[0];
                    row.TitleLabel.Text = "At-a-Glance Display Board (AGDB) — No Active Boards";

                    string faultData = string.Empty;
                    if (_faultDataCache.Count > 0)
                    {
                        faultData = _faultDataCache.Values.FirstOrDefault() ?? string.Empty;
                    }
                    RenderAGDBFrame(row, faultData);
                }
            }
            else
            {
                for (int i = 0; i < _boardList.Count; i++)
                {
                    var board = _boardList[i];
                    var row = _dynamicRows[i];
                    row.BoardData = board;

                    string platformStr = !string.IsNullOrEmpty(board.PlatformNumber) ? board.PlatformNumber : (!string.IsNullOrEmpty(board.BoardLocation) ? board.BoardLocation : "N/A");
                    row.TitleLabel.Text = $"At-a-Glance Display Board (AGDB) — IP: {board.BoardIp} — Platform: {platformStr}";

                    string faultData = string.Empty;
                    if (_faultDataCache.TryGetValue(board.BoardId, out string cachedFault))
                    {
                        faultData = cachedFault;
                    }

                    RenderAGDBFrame(row, faultData);
                }
            }
        }

        private void RecreateBoardControls()
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

            int count = (_boardList == null || _boardList.Count == 0) ? 1 : _boardList.Count;
            int currentY = 12;

            for (int i = 0; i < count; i++)
            {
                var board = (_boardList != null && _boardList.Count > 0) ? _boardList[i] : null;

                // Calculate LED size based on available width
                int availableWidth = Math.Max(300, this.Width - 50);
                int ledSize = Math.Max(3, availableWidth / 384);
                int boardWidth = ledSize * 384;
                int boardHeight = ledSize * 64;

                var titleLabel = new MaterialSkin.Controls.MaterialLabel
                {
                    AutoSize = true,
                    Depth = 0,
                    Font = new Font("Roboto", 16F, FontStyle.Regular, GraphicsUnit.Pixel),
                    FontType = MaterialSkin.MaterialSkinManager.fontType.Subtitle1,
                    Location = new Point(17, currentY),
                    Margin = new Padding(0),
                    MouseState = MaterialSkin.MouseState.HOVER,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
                };

                var scrollPanel = new Panel
                {
                    AutoScroll = true,
                    BackColor = Color.FromArgb(20, 20, 20),
                    Margin = new Padding(0),
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                    AutoSize = false
                };

                var boardPreview = new LEDSORTAGDBBoardPreview
                {
                    BackColor = Color.FromArgb(20, 20, 20),
                    FaultColor = Color.FromArgb(244, 67, 54),
                    Location = new Point(0, 0),
                    Margin = new Padding(0),
                    OffColor = Color.FromArgb(60, 60, 60),
                    OnColor = Color.White,
                    Size = new Size(boardWidth, boardHeight)
                };

                scrollPanel.Controls.Add(boardPreview);

                this.Controls.Add(titleLabel);
                this.Controls.Add(scrollPanel);

                int scrollPanelHeight = boardHeight + 2;

                titleLabel.Location = new Point(17, currentY);
                currentY += 24;

                scrollPanel.Location = new Point(17, currentY);
                scrollPanel.Size = new Size(Math.Max(300, this.Width - 34), scrollPanelHeight);
                currentY += scrollPanelHeight + 20;

                _dynamicRows.Add(new BoardRowControls
                {
                    BoardData = board,
                    TitleLabel = titleLabel,
                    ScrollPanel = scrollPanel,
                    BoardPreview = boardPreview
                });
            }

            _calculatedHeight = currentY;
            this.Height = _calculatedHeight;
        }

        private void RenderAGDBFrame(BoardRowControls row, string faultString)
        {
            int width = 384;
            int height = 64;

            int availableWidth = Math.Max(300, this.Width - 50);
            int ledSize = Math.Max(3, availableWidth / 384);
            int boardWidth = ledSize * 384;
            int boardHeight = ledSize * 64;

            row.BoardPreview.Size = new Size(boardWidth, boardHeight);
            row.BoardPreview.InitializeMatrix(height, width);

            // Create matrix with all LEDs ON (white)
            bool[,] matrix = new bool[height, width];
            for (int r = 0; r < height; r++)
            {
                for (int c = 0; c < width; c++)
                {
                    matrix[r, c] = true;
                }
            }

            row.BoardPreview.SetLEDMatrix(matrix);

            if (!string.IsNullOrEmpty(faultString))
            {
                row.BoardPreview.SetFaults(faultString);
            }
            else
            {
                row.BoardPreview.ClearFaults();
            }

            row.BoardPreview.Invalidate();

            if (row.ScrollPanel != null)
            {
                int scrollPanelHeight = boardHeight + 2;
                row.ScrollPanel.Size = new Size(row.ScrollPanel.Width, scrollPanelHeight);
            }
        }

        public void UpdateHealthStatus(bool online, double temperatureCelsius, double voltage, TimeSpan uptime)
        {
            isOnline = online;

            foreach (var row in _dynamicRows)
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

    public class LEDSORTAGDBBoardPreview : Panel
    {
        private bool[,] ledMatrix;
        private bool[,] faultMatrix;
        private int rows = 64;
        private int cols = 384;

        public Color OnColor { get; set; } = Color.White;
        public Color OffColor { get; set; } = Color.FromArgb(60, 60, 60);
        public Color FaultColor { get; set; } = Color.FromArgb(244, 67, 54);

        // true: HSR "R1C1" is the first LED (top-left). false: "R0C0" is the first LED.
        public bool OneBasedAddress { get; set; } = true;

        public float LedSizeMultiplier { get; set; } = 1.2f;

        public LEDSORTAGDBBoardPreview()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            ResizeRedraw = true;
            InitializeMatrix(64, 384);
        }

        public void InitializeMatrix(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;
            this.ledMatrix = new bool[rows, cols];
            this.faultMatrix = new bool[rows, cols];
            this.Invalidate();
        }

        public void SetLEDMatrix(bool[,] matrix)
        {
            if (matrix != null && matrix.GetLength(0) == rows && matrix.GetLength(1) == cols)
            {
                ledMatrix = (bool[,])matrix.Clone();
                Invalidate();
            }
        }

        public void SetFaults(string faultString)
        {
            faultMatrix = new bool[rows, cols];

            if (string.IsNullOrEmpty(faultString))
                return;

            // Handle multiple fault entries separated by #
            string[] faults = faultString.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string fault in faults)
            {
                string trimmedFault = fault.Trim();
                if (trimmedFault.Length < 8) continue;

                // Check for "S-L1R10C15" or "O-L1R20C400" format
                int dashIndex = trimmedFault.IndexOf('-');
                if (dashIndex < 0) continue;

                string address = trimmedFault.Substring(dashIndex + 1).Trim();

                int line = 1;
                int row = 0;
                int col = 0;

                int lIndex = address.IndexOf('L');
                int rIndex = address.IndexOf('R');
                int cIndex = address.IndexOf('C');

                if (lIndex >= 0 && rIndex >= 0 && cIndex >= 0)
                {
                    string lineStr = address.Substring(lIndex + 1, rIndex - lIndex - 1);
                    if (int.TryParse(lineStr, out line))
                    {
                        string rowStr = address.Substring(rIndex + 1, cIndex - rIndex - 1);
                        if (int.TryParse(rowStr, out row))
                        {
                            string colStr = address.Substring(cIndex + 1);
                            if (int.TryParse(colStr, out col))
                            {
                                //int calculatedRow = (line > 1) ? (line - 1) * 64 + row : row;
                                if (OneBasedAddress)
                                {
                                    row -= 1;
                                    col -= 1;
                                }
                                int calculatedRow = (line > 1) ? (line - 1) * 64 + row : row;
                                if (calculatedRow >= 0 && calculatedRow < rows && col >= 0 && col < cols)
                                {
                                    faultMatrix[calculatedRow, col] = true;
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

            // Draw horizontal dividers every 16 rows
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

            // Draw vertical dividers every 16 columns
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
                            // Glow effect for fault LEDs
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
                            // Glow effect for normal LEDs
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

            // Red ring around each faulty LED so a single bad LED is easy to spot
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
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Do nothing to avoid flickering
        }
    }
}