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
    public partial class LEDSORTMultilineDisplayBoard : UserControl
    {
        private bool isOnline = true;
        private List<SingleLineDisplayBoardData> _boardList = new List<SingleLineDisplayBoardData>();
        private System.Windows.Forms.Timer _refreshTimer;
        private int _calculatedHeight = 380;

        // Store fault data separately (key: BoardId, value: FaultData string)
        private Dictionary<int, string> _faultDataCache = new Dictionary<int, string>();

        // Dynamic UI controls
        private class BoardRowControls
        {
            public SingleLineDisplayBoardData BoardData { get; set; }
            public MaterialSkin.Controls.MaterialLabel TitleLabel { get; set; }
            public Panel ScrollPanel { get; set; }
            public LEDSORTMultilineDisplayBoardPreview BoardPreview { get; set; }
        }

        private readonly List<BoardRowControls> _dynamicRows = new List<BoardRowControls>();

        public LEDSORTMultilineDisplayBoard()
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

            this.Disposed += (s, e) =>
            {
                _refreshTimer?.Stop();
                _refreshTimer?.Dispose();
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

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            LoadActiveBoardData();
        }

        /// <summary>
        /// Loads HSR fault data from database for MLDB devices (fallback to SLDB if needed)
        /// </summary>
        private List<string> LoadHSRFaultData()
        {
            try
            {
                List<APILiveDataLog> hsrDataList = _dbService.GetHSRList("MLDB").ToList();
                if (hsrDataList == null || hsrDataList.Count == 0)
                {
                    hsrDataList = _dbService.GetHSRList("SLDB").ToList();
                }

                List<string> hsrList = hsrDataList
                    .Where(h => h != null && !string.IsNullOrEmpty(h.HSR))
                    .Select(h => h.HSR)
                    .ToList();

                return hsrList;
            }
            catch (Exception)
            {
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
        /// Loads the MLDB (and SLDB) rows of API_LIVE_DATA_LOG once per refresh.
        /// </summary>
        private List<APILiveDataLog> LoadHSRRows()
        {
            try
            {
                // MLDB rows first; SLDB rows too, as the old code fell back to SLDB.
                // Each board still only picks the row with its own IP.
                var rows = new List<APILiveDataLog>();
                rows.AddRange(_dbService.GetHSRList("MLDB") ?? new List<APILiveDataLog>());
                rows.AddRange(_dbService.GetHSRList("SLDB") ?? new List<APILiveDataLog>());
                return rows;
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
                        b.BOARD_NAME.IndexOf("Multi Line", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        b.BOARD_NAME.IndexOf("MLDB", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        b.BOARD_NAME.IndexOf("Multiline", StringComparison.OrdinalIgnoreCase) >= 0))
                    .ToList();

                var activeTrains = _dbService.GetAllActiveBoards() ?? new List<SingleLineDisplayBoardData>();

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

                            //_faultDataCache[newBoard.BoardId] = faultDataToUse;
                            _faultDataCache[newBoard.BoardId] = GetFaultStringForBoard(setting.BOARD_IP, hsrRows);
                            tempBoardList.Add(newBoard);
                        }
                    }
                }
                else
                {
                    var mldbTrains = activeTrains
                        .Where(b => b.BoardName != null && (
                            b.BoardName.IndexOf("Multi Line", StringComparison.OrdinalIgnoreCase) >= 0 ||
                            b.BoardName.IndexOf("MLDB", StringComparison.OrdinalIgnoreCase) >= 0 ||
                            b.BoardName.IndexOf("Multiline", StringComparison.OrdinalIgnoreCase) >= 0))
                        .ToList();

                    foreach (var train in mldbTrains)
                    {
                        //_faultDataCache[train.BoardId] = faultDataToUse;
                        _faultDataCache[train.BoardId] = GetFaultStringForBoard(train.BoardIp, hsrRows);
                    }
                    tempBoardList = mldbTrains;
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
                    row.TitleLabel.Text = "Multi Line Display Board (MLDB) — No Active Boards";

                    string faultData = string.Empty;
                    if (_faultDataCache.Count > 0)
                    {
                        faultData = _faultDataCache.Values.FirstOrDefault() ?? string.Empty;
                    }
                    RenderBoardFrame(row.BoardPreview, faultData, 3);
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
                    row.TitleLabel.Text = $"Multi Line Display Board (MLDB) — IP: {board.BoardIp} — Platform: {platformStr}";

                    string faultData = string.Empty;
                    if (_faultDataCache.TryGetValue(board.BoardId, out string cachedFault))
                    {
                        faultData = cachedFault;
                    }

                    int lines = 3;
                    if (int.TryParse(board.BoardLine, out int parsedLines) && parsedLines > 0)
                    {
                        lines = parsedLines;
                    }

                    RenderBoardFrame(row.BoardPreview, faultData, lines);
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
                int lines = 3;
                if (board != null && int.TryParse(board.BoardLine, out int parsedLines) && parsedLines > 0)
                {
                    lines = parsedLines;
                }

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
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
                };

                int previewHeight = lines * 64; // Scale 2 of lines * 32
                var boardPreview = new LEDSORTMultilineDisplayBoardPreview
                {
                    BackColor = Color.FromArgb(20, 20, 20),
                    FaultColor = Color.FromArgb(244, 67, 54),
                    Location = new Point(0, 0),
                    Margin = new Padding(0),
                    OffColor = Color.FromArgb(60, 60, 60),
                    OnColor = Color.White,
                    Size = new Size(1344, previewHeight)
                };

                scrollPanel.Controls.Add(boardPreview);

                this.Controls.Add(titleLabel);
                this.Controls.Add(scrollPanel);

                int scrollPanelHeight = boardPreview.Height + SystemInformation.HorizontalScrollBarHeight + 4;

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

        private void RenderBoardFrame(LEDSORTMultilineDisplayBoardPreview preview, string faultString, int lines)
        {
            int singleLineHeight = 32;
            int width = 672;
            int totalHeight = lines * singleLineHeight;

            preview.InitializeMatrix(totalHeight, width);
            preview.Size = new Size(1344, totalHeight * 2);

            // Create matrix with all LEDs ON (white)
            bool[,] matrix = new bool[totalHeight, width];
            for (int row = 0; row < totalHeight; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    matrix[row, col] = true;
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

    public class LEDSORTMultilineDisplayBoardPreview : Panel
    {
        private bool[,] ledMatrix;
        private bool[,] faultMatrix;
        private int rows = 96;
        private int cols = 672;

        public Color OnColor { get; set; } = Color.White;
        public Color OffColor { get; set; } = Color.FromArgb(60, 60, 60);
        public Color FaultColor { get; set; } = Color.FromArgb(244, 67, 54);

        // true: HSR "R1C1" is the first LED (top-left). false: "R0C0" is the first LED.
        public bool OneBasedAddress { get; set; } = true;

        public LEDSORTMultilineDisplayBoardPreview()
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

            string[] faults = faultString.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string fault in faults)
            {
                string trimmedFault = fault.Trim();
                if (trimmedFault.Length < 8) continue;

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
                                //int calculatedRow = (line - 1) * 32 + row;
                                if (OneBasedAddress)
                                {
                                    row -= 1;
                                    col -= 1;
                                }
                                int calculatedRow = (line - 1) * 32 + row;
                                //if (calculatedRow >= 0 && calculatedRow < rows && col >= 0 && col < cols)
                                if (row >= 0 && row < 32 && calculatedRow >= 0 && calculatedRow < rows && col >= 0 && col < cols)
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

                    bool isActive = ledMatrix != null && ledMatrix[row, col];
                    bool isFault = faultMatrix != null && faultMatrix[row, col];

                    if (isActive)
                    {
                        if (isFault)
                        {
                            using (var glowBrush = new SolidBrush(Color.FromArgb(80, FaultColor)))
                            {
                                float glowSize = size * 1.8f;
                                g.FillEllipse(glowBrush, x - (glowSize - size) / 2f, y - (glowSize - size) / 2f, glowSize, glowSize);
                            }
                            using (var ledBrush = new SolidBrush(FaultColor))
                            {
                                g.FillEllipse(ledBrush, x, y, size, size);
                            }
                        }
                        else
                        {
                            using (var glowBrush = new SolidBrush(Color.FromArgb(40, Color.White)))
                            {
                                float glowSize = size * 1.8f;
                                g.FillEllipse(glowBrush, x - (glowSize - size) / 2f, y - (glowSize - size) / 2f, glowSize, glowSize);
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
                            float cx = col * pixelW + pixelW / 2f;
                            float cy = row * pixelH + pixelH / 2f;
                            float r = size * 1.6f;
                            g.DrawEllipse(ringPen, cx - r, cy - r, r * 2, r * 2);
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