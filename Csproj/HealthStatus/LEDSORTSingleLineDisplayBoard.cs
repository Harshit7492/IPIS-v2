

using DataBaseHandler;
using DataModel;
using System.Drawing.Drawing2D;


namespace IPISv2.HealthStatusReport
{
    public partial class LEDSORTSingleLineDisplayBoard : UserControl
    {
        public const int BOARD_WIDTH = 672;
        public const int BOARD_HEIGHT = 32;

        private bool isOnline = true;

        // Timer
        private System.Windows.Forms.Timer _displayTimer;

        // Current render state
        private int _delayTime = 0;         // Delay time in seconds
        private List<SingleLineDisplayBoardData> _boardList = new List<SingleLineDisplayBoardData>();
        private int _calculatedHeight = 245; // Default fallback height

        // Store fault data separately (key: BoardId, value: FaultData string)
        private Dictionary<int, string> _faultDataCache = new Dictionary<int, string>();

        private class BoardRowControls
        {
            public SingleLineDisplayBoardData BoardData { get; set; }
            public MaterialSkin.Controls.MaterialLabel TitleLabel { get; set; }
            public Panel ScrollPanel { get; set; }
            public LEDSORTSingleLineBoardPreview BoardPreview { get; set; }
        }

        private readonly List<BoardRowControls> _dynamicRows = new List<BoardRowControls>();

        public LEDSORTSingleLineDisplayBoard()
        {
            InitializeComponent();
            if (IsInDesignMode()) return;

            // Hide default designer controls
            if (lblTitle != null) lblTitle.Visible = false;
            if (panelPreviewScroll != null) panelPreviewScroll.Visible = false;

            LoadActiveBoardData();

            _displayTimer = new System.Windows.Forms.Timer();
            _displayTimer.Interval = 10000;
            _displayTimer.Tick += DisplayTimer_Tick;
            _displayTimer.Start();

            this.Disposed += (s, e) =>
            {
                _displayTimer?.Stop();
                _displayTimer?.Dispose();
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

        private void DisplayTimer_Tick(object sender, EventArgs e)
        {
            LoadActiveBoardData();
        }

        /// <summary>
        /// Loads HSR fault data from database for SLDB devices
        /// </summary>
        private List<string> LoadHSRFaultData()
        {
            try
            {
                // Get HSR list from database for SLDB devices
                List<APILiveDataLog> hsrDataList = _dbService.GetHSRList("SLDB").ToList();

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

        /// <summary>
        /// Converts HSR list to fault string format
        /// </summary>
        private string ConvertHSRListToFaultString(List<string> hsrList)
        {
            if (hsrList == null || hsrList.Count == 0)
                return string.Empty;

            // Join all HSR values with # separator
            return string.Join("#", hsrList);
        }

        /// <summary>
        /// Loads the SLDB rows of API_LIVE_DATA_LOG once per refresh.
        /// </summary>
        private List<APILiveDataLog> LoadHSRRows()
        {
            try
            {
                return _dbService.GetHSRList("SLDB") ?? new List<APILiveDataLog>();
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
                        b.BOARD_NAME.IndexOf("Single Line", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        b.BOARD_NAME.IndexOf("SLDB", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        b.BOARD_NAME.IndexOf("SingleLine", StringComparison.OrdinalIgnoreCase) >= 0))
                    .ToList();

                var activeTrains = _dbService.GetAllActiveBoards() ?? new List<SingleLineDisplayBoardData>();

                // Load HSR fault data from database
                //var hsrFaultList = LoadHSRFaultData();

                // Convert HSR list to fault string
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

                            // Store fault data in dictionary
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
                                PlatformNumber = setting.PF_NO
                            };

                            // Store fault data in dictionary
                            //_faultDataCache[newBoard.BoardId] = faultDataToUse;
                            _faultDataCache[newBoard.BoardId] = GetFaultStringForBoard(setting.BOARD_IP, hsrRows);

                            tempBoardList.Add(newBoard);
                        }
                    }
                }
                else
                {
                    // Fallback to active trains filtered for Single Line
                    var sldbTrains = activeTrains
                        .Where(b => b.BoardName != null && (
                            b.BoardName.IndexOf("Single Line", StringComparison.OrdinalIgnoreCase) >= 0 ||
                            b.BoardName.IndexOf("SLDB", StringComparison.OrdinalIgnoreCase) >= 0))
                        .ToList();

                    foreach (var train in sldbTrains)
                    {
                        // Store fault data in dictionary
                        //_faultDataCache[train.BoardId] = faultDataToUse;
                        _faultDataCache[train.BoardId] = GetFaultStringForBoard(train.BoardIp, hsrRows);
                    }
                    tempBoardList = sldbTrains;
                }

                _boardList = tempBoardList;
                UpdateDynamicUI();

                if (_boardList != null && _boardList.Count > 0)
                {
                    _delayTime = _boardList[0].DelayTime ?? 0;
                }
                else
                {
                    _delayTime = 0;
                }
                SetupDisplayTimer();
            }
            catch (Exception)
            {
                // Fallback to demo data
                _boardList = new List<SingleLineDisplayBoardData>();
                UpdateDynamicUI();
                _delayTime = 0;
                SetupDisplayTimer();
            }
        }

        private void UpdateDynamicUI()
        {

            bool needsRecreate = false;

            if (_boardList == null || _boardList.Count == 0)
            {
                // Show 1 demo board
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
                    // Check if the board IDs match in sequence
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

            // Update content and render frames
            if (_boardList == null || _boardList.Count == 0)
            {
                var row = _dynamicRows[0];
                row.TitleLabel.Text = "Single Line Display Board (SLDB) — No Active Boards";

                // Get fault data from cache (first board)
                string faultData = string.Empty;
                if (_faultDataCache.Count > 0)
                {
                    faultData = _faultDataCache.Values.FirstOrDefault() ?? string.Empty;
                }
                RenderBoardFrame(row.BoardPreview, faultData);
            }
            else
            {
                for (int i = 0; i < _boardList.Count; i++)
                {
                    var board = _boardList[i];
                    var row = _dynamicRows[i];
                    row.BoardData = board; // Update reference

                    string platformStr = !string.IsNullOrEmpty(board.PlatformNumber) ? board.PlatformNumber : (!string.IsNullOrEmpty(board.BoardLocation) ? board.BoardLocation : "N/A");
                    row.TitleLabel.Text = $"Single Line Display Board (SLDB) — IP: {board.BoardIp} — Platform: {platformStr}";

                    // Get fault data from dictionary
                    string faultData = string.Empty;
                    if (_faultDataCache.TryGetValue(board.BoardId, out string cachedFault))
                    {
                        faultData = cachedFault;
                    }

                    RenderBoardFrame(row.BoardPreview, faultData);
                }
            }
        }

        private void RecreateBoardControls()
        {
            // Clean up old dynamic controls
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

                // Create Title Label
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

                // Create Scroll Panel
                var scrollPanel = new Panel
                {
                    AutoScroll = true,
                    BackColor = Color.FromArgb(20, 20, 20),
                    Margin = new Padding(0),
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
                };

                // Create Board Preview
                var boardPreview = new LEDSORTSingleLineBoardPreview
                {
                    BackColor = Color.FromArgb(20, 20, 20),
                    FaultColor = Color.FromArgb(244, 67, 54), // Red for faults
                    Location = new Point(0, 0),
                    Margin = new Padding(0),
                    OffColor = Color.FromArgb(60, 60, 60), // Grey for off LEDs
                    OnColor = Color.White, // White for normal LEDs
                    Size = new Size(4704, 224)
                };

                scrollPanel.Controls.Add(boardPreview);

                // Add to user control
                this.Controls.Add(titleLabel);
                this.Controls.Add(scrollPanel);

                // Set layout positions and size
                int scrollPanelHeight = boardPreview.Height + SystemInformation.HorizontalScrollBarHeight + 4;

                titleLabel.Location = new Point(17, currentY);
                currentY += 24; // Standard height estimation for subtitle label

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

            // Adjust height of this user control to fit all rows
            _calculatedHeight = currentY;
            this.Height = _calculatedHeight;
        }

        /// <summary>
        /// Renders the board frame with all LEDs white, fault LEDs red
        /// </summary>
        private void RenderBoardFrame(LEDSORTSingleLineBoardPreview preview, string faultString)
        {
            int width = BOARD_WIDTH;
            int height = BOARD_HEIGHT;

            // Create matrix with all LEDs ON (white)
            bool[,] matrix = new bool[height, width];
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    matrix[row, col] = true;
                }
            }

            // Set the LED matrix (all white LEDs)
            preview.SetLEDMatrix(matrix);

            // Set faults if any (these will turn red)
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
            foreach (var row in _dynamicRows)
            {
                if (row != null && row.BoardPreview != null && !row.BoardPreview.IsDisposed)
                {
                    row.BoardPreview.Enabled = online;
                }
            }
        }

        private void SetupDisplayTimer()
        {
            if (_displayTimer == null)
            {
                _displayTimer = new System.Windows.Forms.Timer();
                _displayTimer.Tick += DisplayTimer_Tick;
            }

            _displayTimer.Stop();

            int intervalSeconds = _delayTime > 0 ? _delayTime : 10;
            _displayTimer.Interval = intervalSeconds * 1000;
            _displayTimer.Start();
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
    /// Renders a single-line LED display board with circular bulbs.
    /// Normal LEDs are white, fault LEDs are red.
    /// </summary>
    public class LEDSORTSingleLineBoardPreview : Panel
    {
        private bool[,] ledMatrix;
        private bool[,] faultMatrix;
        private int rows = 32;
        private int cols = 672;

        public Color OnColor { get; set; } = Color.White;
        public Color OffColor { get; set; } = Color.FromArgb(60, 60, 60);
        public Color FaultColor { get; set; } = Color.FromArgb(244, 67, 54);

        // true: HSR "R1C1" is the first LED (top-left). false: "R0C0" is the first LED.
        public bool OneBasedAddress { get; set; } = true;

        public LEDSORTSingleLineBoardPreview()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            ResizeRedraw = true;
            ledMatrix = new bool[rows, cols];
            faultMatrix = new bool[rows, cols];
        }

        public void SetLEDMatrix(bool[,] matrix)
        {
            if (matrix.GetLength(0) == rows && matrix.GetLength(1) == cols)
            {
                ledMatrix = (bool[,])matrix.Clone();
                Invalidate();
            }
        }

        /// <summary>
        /// Sets fault LEDs based on fault string format: "S-L1R10C15#O-L1R20C400"
        /// S = Short circuit fault
        /// O = Open circuit fault
        /// L = Line number
        /// R = Row number
        /// C = Column number
        /// </summary>
        public void SetFaults(string faultString)
        {
            // Reset fault matrix
            faultMatrix = new bool[rows, cols];

            if (string.IsNullOrEmpty(faultString))
                return;

            // Split multiple faults by #
            string[] faults = faultString.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string fault in faults)
            {
                // Parse format: "S-L1R10C15" or "O-L1R20C400"
                string trimmedFault = fault.Trim();
                if (trimmedFault.Length < 8) continue; // Minimum length for "S-L1R10C15"

                // Extract the address part (everything after the first dash)
                int dashIndex = trimmedFault.IndexOf('-');
                if (dashIndex < 0) continue;

                string address = trimmedFault.Substring(dashIndex + 1).Trim();

                // Parse L1R10C15 format
                int line = 1;
                int row = 0;
                int col = 0;

                // Parse Line
                int lIndex = address.IndexOf('L');
                int rIndex = address.IndexOf('R');
                int cIndex = address.IndexOf('C');

                if (lIndex >= 0 && rIndex >= 0 && cIndex >= 0)
                {
                    // Parse Line number
                    string lineStr = address.Substring(lIndex + 1, rIndex - lIndex - 1);
                    if (int.TryParse(lineStr, out line))
                    {
                        // Parse Row number
                        string rowStr = address.Substring(rIndex + 1, cIndex - rIndex - 1);
                        if (int.TryParse(rowStr, out row))
                        {
                            // Parse Column number
                            string colStr = address.Substring(cIndex + 1);
                            if (int.TryParse(colStr, out col))
                            {
                                // Check bounds (line 1 maps to row 0 in our matrix)
                                //if (line == 1 && row >= 0 && row < rows && col >= 0 && col < cols)
                                //{
                                //    faultMatrix[row, col] = true;
                                //}
                                if (OneBasedAddress)
                                {
                                    row -= 1;
                                    col -= 1;
                                }
                                if (line == 1 && row >= 0 && row < rows && col >= 0 && col < cols)
                                {
                                    faultMatrix[row, col] = true;
                                }
                            }
                        }
                    }
                }
            }

            Invalidate();
        }

        /// <summary>
        /// Clears all faults
        /// </summary>
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

            using (var bgBrush = new SolidBrush(Color.FromArgb(15, 15, 20)))
            {
                g.FillRectangle(bgBrush, ClientRectangle);
            }

            float pixelW = (float)Width / cols;
            float pixelH = (float)Height / rows;

            float size = Math.Min(pixelW, pixelH) * 0.95f;
            if (size < 1.5f) size = 1.5f;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    const float margin = 0.2f;

                    float x = col * pixelW + margin;
                    float y = row * pixelH + margin;

                    bool isActive = ledMatrix[row, col];
                    bool isFault = faultMatrix[row, col];

                    if (isActive)
                    {
                        if (isFault)
                        {
                            // Fault LED - Red with glow
                            using (var glowBrush = new SolidBrush(Color.FromArgb(80, FaultColor)))
                            {
                                g.FillEllipse(glowBrush, x - 1.5f, y - 1.5f, size + 3, size + 3);
                            }
                            using (var ledBrush = new SolidBrush(FaultColor))
                            {
                                g.FillEllipse(ledBrush, x, y, size, size);
                            }
                        }
                        else
                        {
                            // Normal LED - White with subtle glow
                            using (var glowBrush = new SolidBrush(Color.FromArgb(40, Color.White)))
                            {
                                g.FillEllipse(glowBrush, x - 1f, y - 1f, size + 2, size + 2);
                            }
                            using (var ledBrush = new SolidBrush(OnColor))
                            {
                                g.FillEllipse(ledBrush, x, y, size, size);
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

            // Red ring around each faulty LED so a single bad LED is easy to spot on the wide board
            using (var ringPen = new Pen(FaultColor, 2f))
            {
                for (int row = 0; row < rows; row++)
                {
                    for (int col = 0; col < cols; col++)
                    {
                        if (!faultMatrix[row, col]) continue;
                        float cx = col * pixelW + 0.2f + size / 2f;
                        float cy = row * pixelH + 0.2f + size / 2f;
                        float r = size * 1.6f;
                        g.DrawEllipse(ringPen, cx - r, cy - r, r * 2, r * 2);
                    }
                }
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Override to prevent flicker
        }
    }
}