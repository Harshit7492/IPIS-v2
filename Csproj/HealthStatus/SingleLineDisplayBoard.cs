using DataBaseHandler;
using DataModel;
using IPISv2.HealthStatus;
using MemberDomain;
using System.Data;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace IPISv2.HealthStatusReport
{
    public partial class SingleLineDisplayBoard : UserControl
    {
        public const int BOARD_WIDTH = 672;
        public const int BOARD_HEIGHT = 32;
        private readonly int _defaultDisplayTime = 5;
        private readonly TrainStatusProcessor _statusProcessor;
        private readonly object _lock = new();
        private readonly CancellationTokenSource _displayCts = new();
        private System.Windows.Forms.Timer _displayTimer;
        private bool _isProcessingLiveData = false;

        // True while live data keeps arriving: the PTO rotation owns the boards and the
        // database reload must not draw on them. Cleared by the first cycle that produces
        // no live data, so the database fallback resumes when the feed goes down.
        private bool _liveRotationOwnsBoards = false;

        // Current render state
        private int _delayTime = 0;
        private List<SingleLineDisplayBoardData> _boardList = new List<SingleLineDisplayBoardData>();
        // Per-train rows straight from the database, used to resolve ACTUAL_* times
        // for whichever train is currently on screen.
        private List<SingleLineDisplayBoardData> _allActiveTrains = new List<SingleLineDisplayBoardData>();
        private int _calculatedHeight = 245;

        private class BoardRowControls
        {
            public SingleLineDisplayBoardData BoardData { get; set; }
            public Panel HeaderPanel { get; set; }
            public MaterialSkin.Controls.MaterialLabel TitleLabel { get; set; }
            public MaterialSkin.Controls.MaterialLabel TrainInfoLabel { get; set; }
            public Panel ScrollPanel { get; set; }
            public SingleLineBoardPreview BoardPreview { get; set; }

            // ✅ cache the last text rendered so we can skip redundant repaints
            public string LastRenderedText { get; set; }
        }

        private readonly List<BoardRowControls> _dynamicRows = new List<BoardRowControls>();

        private const string LANG_ENGLISH = "E";
        private const string LANG_HINDI = "H";
        private const string LANG_REGIONAL = "R";

        // Display order on the board: English -> Hindi -> Regional
        private static readonly string[] LanguageDisplayOrder = { LANG_ENGLISH, LANG_HINDI, LANG_REGIONAL };

        // Languages configured per board (key = board IP), filled in LoadActiveBoardData
        private Dictionary<string, List<string>> _boardLanguages =
            new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

        public SingleLineDisplayBoard()
        {
            InitializeComponent();
            if (IsInDesignMode()) return;

            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);
            DoubleBuffered = true;

            _statusProcessor = new TrainStatusProcessor();

            // Hide default designer controls
            if (lblTitle != null) lblTitle.Visible = false;
            if (panelPreviewScroll != null) panelPreviewScroll.Visible = false;

            // Hide header panel initially (will be shown when boards are loaded)
            if (panelHeader != null) panelHeader.Visible = false;

            // Create the timer ONCE here.
            _displayTimer = new System.Windows.Forms.Timer();
            _displayTimer.Interval = 2000;
            _displayTimer.Tick += DisplayTimer_Tick;
            _displayTimer.Start();

            LoadActiveBoardData();

            this.Disposed += (s, e) =>
            {
                _displayTimer?.Stop();
                _displayTimer?.Dispose();
                _displayCts?.Cancel();
                _displayCts?.Dispose();
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

        private async void DisplayTimer_Tick(object sender, EventArgs e)
        {
            await ProcessAndBroadcastLCDTVLiveDataAsync();
        }

        public void LoadActiveBoardData()
        {
            try
            {
                var allSettings = _dbService.GET_BOARD_SETTING(0) ?? new List<BoardSettingInfo>();
                var installedSettings = allSettings.Where(b => b.BOARD_NAME != null &&
                    (b.BOARD_NAME.IndexOf("Single Line", StringComparison.OrdinalIgnoreCase) >= 0 ||
                     b.BOARD_NAME.IndexOf("SLDB", StringComparison.OrdinalIgnoreCase) >= 0 ||
                     b.BOARD_NAME.IndexOf("SingleLine", StringComparison.OrdinalIgnoreCase) >= 0)).ToList();

                var activeTrains = _dbService.GetAllActiveBoards() ?? new List<SingleLineDisplayBoardData>();
                _allActiveTrains = activeTrains;

                var tempBoardList = new List<SingleLineDisplayBoardData>();
                var tempBoardLanguages = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

                if (installedSettings.Count > 0)
                {
                    foreach (var setting in installedSettings)
                    {
                        string settingIp = setting.BOARD_IP?.Trim() ?? string.Empty;
                        if (!tempBoardLanguages.ContainsKey(settingIp))
                        {
                            tempBoardLanguages[settingIp] = ParseLanguageCodes(setting.LANGUAGES);
                        }

                        var candidates = activeTrains.Where(t => t.BoardId == setting.BOARD_ID ||
                            (string.Equals(t.BoardIp?.Trim(), setting.BOARD_IP?.Trim(), StringComparison.OrdinalIgnoreCase) &&
                             !string.IsNullOrEmpty(setting.BOARD_IP))).ToList();

                        SingleLineDisplayBoardData match = null;
                        string selectedPf = setting.SELECTED_PF?.Trim();

                        if (!string.IsNullOrEmpty(selectedPf))
                        {
                            var selectedPlatforms = selectedPf
                                .Split(new[] { ',', '+' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(p => p.Trim())
                                .Where(p => p.Length > 0)
                                .ToList();

                            match = candidates.FirstOrDefault(t =>
                                !string.IsNullOrEmpty(t.PlatformNumber) &&
                                selectedPlatforms.Any(p => string.Equals(p, t.PlatformNumber.Trim(), StringComparison.OrdinalIgnoreCase)));
                        }

                        if (match == null)
                        {
                            match = candidates.FirstOrDefault();
                        }

                        if (match != null)
                        {
                            match.BoardName = setting.BOARD_NAME;
                            match.BoardLocation = setting.BOARD_LOCATION;
                            match.BoardIp = setting.BOARD_IP;
                            match.FontSize = setting.FONT_SIZE;
                            match.DelayTime = setting.DELAYTIME;
                            match.BoardLine = setting.BOARD_LINE.ToString();

                            // ✅ Store the configured SELECTED_PF on the board so the live-data
                            //    payload can use the PF configured for THIS specific board IP.
                            match.SelectedPf = selectedPf;

                            tempBoardList.Add(match);
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
                                DelayTime = setting.DELAYTIME,
                                TrainNumber = string.Empty,
                                PlatformNumber = setting.SELECTED_PF,
                                SelectedPf = selectedPf
                            });
                        }
                    }
                }
                else
                {
                    var sldbTrains = activeTrains.Where(b => b.BoardName != null &&
                        (b.BoardName.IndexOf("Single Line", StringComparison.OrdinalIgnoreCase) >= 0 ||
                         b.BoardName.IndexOf("SLDB", StringComparison.OrdinalIgnoreCase) >= 0)).ToList();
                    tempBoardList = sldbTrains;
                }

                _boardList = tempBoardList;
                _boardLanguages = tempBoardLanguages;
                UpdateDynamicUI();

                if (_boardList != null && _boardList.Count > 0)
                {
                    _delayTime = _boardList[0].DelayTime ?? 0;
                }
                else
                {
                    _delayTime = 0;
                }
            }
            catch (Exception ex)
            {
                LogManager.LoggerService.Logger.Error(ex, "Error loading SLDB data.");
                _boardList = new List<SingleLineDisplayBoardData>();
                UpdateDynamicUI();
                _delayTime = 0;
            }
        }

        /// <summary>
        /// Checks whether the given board IP responds to a ping.
        /// Used to gate preview rendering AND to decide which boards are included in the payload.
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

        /// <summary>
        /// ✅ FLICKER FIX: only assigns Text when it actually differs from the current value.
        /// </summary>
        private static void SetLabelTextIfChanged(MaterialSkin.Controls.MaterialLabel label, string text)
        {
            if (label == null) return;
            if (!string.Equals(label.Text, text, StringComparison.Ordinal))
            {
                label.Text = text;
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
                if (_dynamicRows != null && _dynamicRows.Count > 0)
                {
                    var row = _dynamicRows[0];
                    SetLabelTextIfChanged(row.TitleLabel, "Single Line Display Board (SLDB) — No Active Boards");
                    SetLabelTextIfChanged(row.TrainInfoLabel, "No Train Information Available");

                    if (row.BoardPreview != null && row.LastRenderedText != "")
                    {
                        ClearBoardPreview(row.BoardPreview);
                        row.LastRenderedText = "";
                    }
                }
            }
            else
            {
                for (int i = 0; i < _boardList.Count; i++)
                {
                    if (_dynamicRows == null || i >= _dynamicRows.Count) break;
                    var board = _boardList[i];
                    var row = _dynamicRows[i];
                    row.BoardData = board;

                    string platformStr = !string.IsNullOrEmpty(board.PlatformNumber) ? board.PlatformNumber :
                                        (!string.IsNullOrEmpty(board.BoardLocation) ? board.BoardLocation : "N/A");

                    SetLabelTextIfChanged(row.TitleLabel, $"Single Line Display Board (SLDB) — IP: {board.BoardIp} — Platform: {platformStr}");

                    // The 2-second tick reloads the database before every live cycle. Drawing
                    // that row here flashed the plain time line (arrival, departure, platform)
                    // at each cycle boundary - invisible on time statuses, but on Diverted,
                    // Terminated At, Indefinite Late and Change of Source it showed times and
                    // platform for a moment. While live data is flowing, leave the board alone.
                    if (_liveRotationOwnsBoards)
                        continue;

                    string displayText = GetBoardDisplayText(board);
                    int fontSize = board.FontSize ?? 13;

                    if (string.Equals(row.LastRenderedText, displayText, StringComparison.Ordinal))
                        continue;

                    if (row.BoardPreview != null && IsHostReachable(board.BoardIp))
                    {
                        RenderBoardFrame(row.BoardPreview, displayText, fontSize);
                        row.LastRenderedText = displayText;
                    }
                }
            }
        }

        private void RecreateBoardControls()
        {
            for (int i = 1; i < _dynamicRows.Count; i++) // Skip index 0 (designer controls)
            {
                var row = _dynamicRows[i];
                if (row.TitleLabel != null)
                {
                    this.Controls.Remove(row.TitleLabel);
                    row.TitleLabel.Dispose();
                }
                if (row.TrainInfoLabel != null)
                {
                    this.Controls.Remove(row.TrainInfoLabel);
                    row.TrainInfoLabel.Dispose();
                }
                if (row.HeaderPanel != null)
                {
                    this.Controls.Remove(row.HeaderPanel);
                    row.HeaderPanel.Dispose();
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
            int currentY = 8;

            for (int i = 0; i < count; i++)
            {
                var board = (_boardList != null && _boardList.Count > 0) ? _boardList[i] : null;

                Panel headerPanel;
                MaterialSkin.Controls.MaterialLabel titleLabel;
                MaterialSkin.Controls.MaterialLabel trainInfoLabel;
                Panel scrollPanel;
                SingleLineBoardPreview boardPreview;

                if (i == 0)
                {
                    headerPanel = this.panelHeader;
                    titleLabel = this.lblBoardTitle;
                    scrollPanel = this.panelPreviewScroll;
                    boardPreview = this.boardPreview;

                    headerPanel.Visible = true;
                    scrollPanel.Visible = true;

                    headerPanel.Location = new Point(17, currentY);
                    headerPanel.Size = new Size(Math.Max(300, this.Width - 34), 25);
                    currentY += 25 + 4;

                    scrollPanel.Location = new Point(17, currentY);
                    scrollPanel.Size = new Size(Math.Max(300, this.Width - 34), boardPreview.Height + SystemInformation.HorizontalScrollBarHeight + 4);
                    currentY += scrollPanel.Height + 16;

                    trainInfoLabel = null;
                }
                else
                {
                    headerPanel = CreateHeaderPanel();
                    titleLabel = CreateTitleLabel();
                    scrollPanel = CreateScrollPanel();
                    boardPreview = CreateBoardPreview();

                    headerPanel.Controls.Add(titleLabel);
                    scrollPanel.Controls.Add(boardPreview);

                    this.Controls.Add(headerPanel);
                    this.Controls.Add(scrollPanel);

                    headerPanel.Location = new Point(17, currentY);
                    headerPanel.Size = new Size(Math.Max(300, this.Width - 34), 25);
                    currentY += 25 + 4;

                    scrollPanel.Location = new Point(17, currentY);
                    scrollPanel.Size = new Size(Math.Max(300, this.Width - 34), boardPreview.Height + SystemInformation.HorizontalScrollBarHeight + 4);
                    currentY += scrollPanel.Height + 16;

                    trainInfoLabel = null;
                }

                _dynamicRows.Add(new BoardRowControls
                {
                    BoardData = board,
                    HeaderPanel = headerPanel,
                    TitleLabel = titleLabel,
                    TrainInfoLabel = trainInfoLabel,
                    ScrollPanel = scrollPanel,
                    BoardPreview = boardPreview,
                    LastRenderedText = null
                });
            }

            _calculatedHeight = currentY;
            this.Height = _calculatedHeight;
        }

        private Panel CreateHeaderPanel()
        {
            return new Panel
            {
                BackColor = panelHeader.BackColor,
                Margin = new Padding(0),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Height = 25
            };
        }

        private MaterialSkin.Controls.MaterialLabel CreateTitleLabel()
        {
            return new MaterialSkin.Controls.MaterialLabel
            {
                AutoSize = true,
                Depth = 0,
                Font = lblBoardTitle.Font,
                FontType = lblBoardTitle.FontType,
                Location = lblBoardTitle.Location,
                Margin = new Padding(0),
                MouseState = MaterialSkin.MouseState.HOVER,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
        }

        private Panel CreateScrollPanel()
        {
            var p = new Panel
            {
                AutoScroll = true,
                BackColor = Color.FromArgb(20, 20, 20),
                Margin = new Padding(0),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            EnableComposited(p);
            return p;
        }

        private SingleLineBoardPreview CreateBoardPreview()
        {
            return new SingleLineBoardPreview
            {
                BackColor = Color.FromArgb(20, 20, 20),
                FaultColor = Color.FromArgb(244, 67, 54),
                Location = new Point(0, 0),
                Margin = new Padding(0),
                OffColor = Color.FromArgb(60, 60, 60),
                OnColor = Color.FromArgb(76, 175, 80),
                Size = new Size(4704, 224)
            };
        }

        private const int WS_EX_COMPOSITED = 0x02000000;

        private static void EnableComposited(Control c)
        {
            if (c == null) return;
            try
            {
                if (c.IsHandleCreated)
                {
                    var ex = GetWindowLong(c.Handle, -20);
                    SetWindowLong(c.Handle, -20, ex | WS_EX_COMPOSITED);
                }
                else
                {
                    c.HandleCreated += (s, e) =>
                    {
                        var ex = GetWindowLong(c.Handle, -20);
                        SetWindowLong(c.Handle, -20, ex | WS_EX_COMPOSITED);
                    };
                }
            }
            catch { /* non-fatal */ }
        }

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        /// <summary>
        /// Database ACTUAL_* wins; f0.Eat/f0.Edt degrade to the scheduled time when the
        /// live API omits ETA/EDT. Resolved per train number, so it tracks the PTO
        /// rotation instead of sticking on the first train shown.
        /// </summary>
        private string ResolveActualTime(string trainNo, bool arrival, string feedValue)
        {
            var row = _allActiveTrains?.FirstOrDefault(x =>
                string.Equals((x.TrainNumber ?? string.Empty).Trim(),
                              (trainNo ?? string.Empty).Trim(),
                              StringComparison.OrdinalIgnoreCase));

            string dbValue = row == null
                ? null
                : (arrival ? row.ActualArrivalTime : row.ActualDepartureTime);

            return !string.IsNullOrWhiteSpace(dbValue) ? dbValue : feedValue;
        }

        /// <summary>
        /// Shallow copy used as render scratch, so writes made while cycling trains
        /// never leak back into _boardList and get picked up by the other render path.
        /// </summary>
        private static SingleLineDisplayBoardData CloneBoardData(SingleLineDisplayBoardData source)
        {
            var copy = new SingleLineDisplayBoardData();
            if (source == null) return copy;

            foreach (var prop in typeof(SingleLineDisplayBoardData).GetProperties())
            {
                if (prop.CanRead && prop.CanWrite)
                    prop.SetValue(copy, prop.GetValue(source));
            }

            return copy;
        }

        private string GetBoardDisplayText(SingleLineDisplayBoardData board)
        {
            if (string.IsNullOrEmpty(board.TrainNumber))
            {
                return string.Empty;
            }

            // The database row's ADFlag can carry the A/D flag rather than the status, so
            // classify on TrainStatusEnglish when present. Otherwise a text-only status is
            // drawn as a time line whenever the board falls back to database data.
            string status = DisplayTextFormatter.SelectLanguagePart(
                !string.IsNullOrWhiteSpace(board.TrainStatusEnglish) ? board.TrainStatusEnglish : board.ADFlag,
                0);

            bool isCancelled = DisplayTextFormatter.IsCancelledStatus(status);

            // Always the actual times - no fallback to the scheduled ones.
            string arrivalTime = board.ActualArrivalTime;
            string departureTime = board.ActualDepartureTime;

            return DisplayTextFormatter.FormatDisplayText(
                board.TrainNumber,
                board.TrainNameEnglish,
                arrivalTime,
                departureTime,
                status,
                board.PlatformNumber,
                isCancelled
            );
        }

        private void RenderBoardFrame(SingleLineBoardPreview preview, string displayText, int fontSize, string language = LANG_ENGLISH)
        {
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

                int clampedFontSize = Math.Max(16, Math.Min(fontSize > 0 && fontSize != 13 ? fontSize : 18, 26));

                if (!string.IsNullOrWhiteSpace(displayText))
                {
                    using (Font font = new Font(GetFontFamilyForLanguage(language), clampedFontSize * scale, FontStyle.Bold, GraphicsUnit.Pixel))
                    using (SolidBrush brush = new SolidBrush(Color.White))
                    {
                        SizeF textSize = g.MeasureString(displayText, font);
                        float y = ((height * scale) - textSize.Height) / 2f;
                        float x = 5 * scale;

                        g.DrawString(displayText, font, brush, x, y);
                    }
                }

                ConvertBitmapToMatrix(bmp, matrix, width, height, scale);
            }

            preview.SetLEDMatrix(matrix);
        }

        private void ClearBoardPreview(SingleLineBoardPreview preview)
        {
            if (preview == null) return;

            bool[,] empty = new bool[BOARD_HEIGHT, BOARD_WIDTH];
            preview.SetLEDMatrix(empty);
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
                            int rowOffset = py * stride;

                            for (int sx = 0; sx < scale; sx++)
                            {
                                int px = cellX * scale + sx;
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

        private string ResolveSelectedPf(SingleLineDisplayBoardData board)
        {
            if (board == null) return string.Empty;

            if (!string.IsNullOrWhiteSpace(board.SelectedPf))
                return board.SelectedPf.Trim();

            if (!string.IsNullOrWhiteSpace(board.PlatformNumber))
                return board.PlatformNumber.Trim();

            return string.Empty;
        }

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
        private List<string> GetBoardLanguages(SingleLineDisplayBoardData board, List<string> lineConfLanguages)
        {
            string ip = board?.BoardIp?.Trim() ?? string.Empty;

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
        /// status logic (cancelled / rescheduled / departure) works on the English text.
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

            if (lineConf.Count == 0 || lineConf[0] is not JsonObject config)
            {
                if (lineConf.Count == 0)
                    lineConf.Add(new JsonObject());
                config = lineConf[0] as JsonObject;
            }

            if (config != null)
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
        /// Arial has no Devanagari / Indic glyphs, so Hindi and Regional text use an Indic-capable font.
        /// </summary>
        private static string GetFontFamilyForLanguage(string language)
        {
            if (string.Equals(language, LANG_HINDI, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(language, LANG_REGIONAL, StringComparison.OrdinalIgnoreCase))
            {
                return "Nirmala UI";
            }

            return "Arial";
        }

        /// <summary>
        /// Updates one board row with the given text (only when the board pings and the text changed).
        /// </summary>
        private void ShowTextOnBoardRow(BoardRowControls row, string text, int fontSize, string language, bool boardPinged)
        {
            bool textChanged = !string.Equals(row.LastRenderedText, text, StringComparison.Ordinal);

            Action update = () =>
            {
                SetLabelTextIfChanged(row.TrainInfoLabel, text);
                if (boardPinged && textChanged)
                {
                    RenderBoardFrame(row.BoardPreview, text, fontSize, language);
                    row.LastRenderedText = text;
                }
            };

            if (this.InvokeRequired)
                this.Invoke(update);
            else
                update();
        }

        public async Task ProcessAndBroadcastLCDTVLiveDataAsync()
        {
            // Guard against overlapping ticks.
            if (_isProcessingLiveData)
            {
                LogManager.LoggerService.Logger.Debug("SLDB tick skipped (previous cycle still running).");
                return;
            }
            _isProcessingLiveData = true;
            bool liveRenderedThisCycle = false;

            try
            {
                // Reload board settings + active trains from DB on every tick.
                LoadActiveBoardData();

                // ✅ Build the Device/DisplayList payload dynamically:
                //    - Iterate every configured board in _boardList
                //    - Ping its IP via IsHostReachable
                //    - Only include reachable boards
                //    - Use the SELECTED_PF configured for THAT specific board
                var deviceList = new List<DeviceInfo>();
                var displayList = new List<DeviceInfo>();

                if (_boardList != null)
                {
                    foreach (var board in _boardList)
                    {
                        if (board == null || string.IsNullOrWhiteSpace(board.BoardIp))
                            continue;

                        string boardIp = board.BoardIp.Trim();

                        // Ping the board IP first.
                        bool reachable = IsHostReachable(boardIp);
                        if (!reachable)
                        {
                            LogManager.LoggerService.Logger.Debug(
                                $"SLDB board {boardIp} not reachable — skipping in payload.");
                            continue;
                        }

                        // Get the PF configured for this board IP.
                        string boardPf = ResolveSelectedPf(board);

                        deviceList.Add(new DeviceInfo
                        {
                            Vendor = "VARDAAN",
                            Device = "SLDB",
                            IP = boardIp,
                            PF = boardPf ?? string.Empty,
                            INT = "50",
                            HSR = "ALL OK"
                        });

                        displayList.Add(new DeviceInfo
                        {
                            Device = "AVDB",
                            IP = boardIp,
                            INT = "50",
                            HSR = "ALL OK"
                        });
                    }
                }

                if (deviceList.Count == 0)
                {
                    LogManager.LoggerService.Logger.Warning(
                        "No reachable SLDB boards — skipping live-data request.");
                    return;
                }

                var request = new LiveDataRequest
                {
                    Device = deviceList,
                    DisplayList = displayList
                };

                string rawJson = JsonSerializer.Serialize(request);

                var liveData = await _dbService.GetLiveDataDisplayAsync(request, rawJson);

                if (liveData == null)
                {
                    LogManager.LoggerService.Logger.Warning("No live data received from database.");
                    return;
                }

                JsonObject? jsonObject = JsonSerializer.SerializeToNode(liveData) as JsonObject;

                if (jsonObject == null)
                {
                    LogManager.LoggerService.Logger.Warning("Failed to convert live data to JsonObject.");
                    return;
                }

                // Make TrainStatusProcessor build the status texts for every configured language (E / H / R).
                PrepareLanguagesForProcessing(jsonObject);

                JsonObject processedOutput = _statusProcessor.Process(jsonObject);

                // Update boards with live data. From here the PTO rotation draws the boards.
                liveRenderedThisCycle = true;
                await UpdateBoardsWithLiveDataAsync(processedOutput, _displayCts.Token);

                var finalPayload = new JsonObject
                {
                    ["LiveData"] = processedOutput
                };

                string finalJson = finalPayload.ToJsonString();
            }
            catch (OperationCanceledException)
            {
                LogManager.LoggerService.Logger.Information("ProcessAndBroadcastLCDTVLiveDataAsync was cancelled.");
            }
            catch (Exception ex)
            {
                LogManager.LoggerService.Logger.Error(ex, "Error while processing and broadcasting LCD/TV live data.");
            }
            finally
            {
                _liveRotationOwnsBoards = liveRenderedThisCycle;
                _isProcessingLiveData = false;
            }
        }

        private async Task UpdateBoardsWithLiveDataAsync(JsonObject data, CancellationToken token)
        {
            JsonArray? trains = data["TrainList"] as JsonArray;
            JsonArray? lineConf = data["LineConf"] as JsonArray;

            if (trains == null || trains.Count == 0)
            {
                UpdateBoardWithNoTrains();
                return;
            }

            // Languages sent in LineConf — used for boards that have no language configured.
            List<string> lineConfLanguages = GetLineConfLanguages(lineConf);

            int pto = DisplayHelper.GetPto(lineConf, _defaultDisplayTime);
            if (pto <= 0) pto = 10;

            int rowsCount = Math.Max(1, _dynamicRows.Count);
            List<List<JsonObject?>> pages = DisplayPageHelper.CreatePages(trains, rowsCount);

            if (pages.Count == 0)
            {
                UpdateBoardWithNoTrains();
                return;
            }

            // Languages of every board row, in display order: E, or E + H, or E + H + R.
            var rowLanguages = new List<List<string>>();
            for (int b = 0; b < _dynamicRows.Count; b++)
            {
                var configuredBoard = (b < _boardList.Count) ? _boardList[b] : null;
                rowLanguages.Add(GetBoardLanguages(configuredBoard, lineConfLanguages));
            }

            // Every page is shown once per configured language (English -> Hindi -> Regional),
            // each language for the full PTO time.
            int languageSteps = rowLanguages.Count > 0 ? Math.Max(1, rowLanguages.Max(l => l.Count)) : 1;

            // Each configured language gets an equal share of the PTO, so a train still
            // occupies one PTO in total: 2 languages -> PTO/2 each, 3 -> PTO/3 each.
            double languageSlotSeconds = pto / (double)Math.Max(1, languageSteps);

            for (int pageIndex = 0; pageIndex < pages.Count; pageIndex++)
            {
                List<JsonObject?> page = pages[pageIndex];

                for (int languageStep = 0; languageStep < languageSteps; languageStep++)
                {
                    token.ThrowIfCancellationRequested();

                    bool pageHasSpecialFlip = false;

                    // Step 1: Render Frame 0 for all active boards on current page
                    for (int b = 0; b < _dynamicRows.Count; b++)
                    {
                        token.ThrowIfCancellationRequested();

                        // Render scratch copy: this loop writes each train's values onto the object,
                        // and _boardList[b] is rendered by the other path too. Mutating the shared
                        // entry left the last train's details on screen.
                        var board = CloneBoardData((b < _boardList.Count) ? _boardList[b] : new SingleLineDisplayBoardData());
                        var row = _dynamicRows[b];

                        List<string> boardLanguages = rowLanguages[b];
                        string language = boardLanguages[languageStep % boardLanguages.Count];

                        JsonObject? train = (b < page.Count) ? page[b] : null;

                        if (train != null)
                        {
                            // Status logic (cancelled / rescheduled / departure / flip) always uses the English data.
                            TrainDisplayData f0 = TrainDisplayHelper.BuildTrainData(train, LANG_ENGLISH, 0);
                            TrainDisplayData f0Language = language == LANG_ENGLISH
                                ? f0
                                : TrainDisplayHelper.BuildTrainData(train, language, 0);

                            // Train name only in the current language — empty when not available.
                            string trainName = GetTrainNameForLanguage(f0, language);

                            board.TrainNumber = f0.TrainNo;
                            board.TrainNameEnglish = GetTrainNameForLanguage(f0, LANG_ENGLISH);
                            // f0.Eat/f0.Edt fall back to the scheduled time when the live API
                            // omits ETA/EDT, which overwrote the correct ACTUAL_ARRIVAL_TIME /
                            // ACTUAL_DEPARTURE_TIME already loaded from the database.
                            // Database wins; the feed is used only when the database is empty.
                            //board.ActualArrivalTime = f0.Eat;
                            //board.ActualDepartureTime = f0.Edt;
                            board.ActualArrivalTime = ResolveActualTime(f0.TrainNo, true, f0.Eat);
                            board.ActualDepartureTime = ResolveActualTime(f0.TrainNo, false, f0.Edt);
                            board.PlatformNumber = f0.Platform;
                            board.ADFlag = f0.Status;

                            bool isCancelled = DisplayTextFormatter.IsCancelledStatus(board.ADFlag);
                            bool isRescheduled = DisplayTextFormatter.IsRescheduledStatus(f0.Status);

                            string textF0;
                            if (isRescheduled)
                            {
                                // First half of PTO: show the status (RESCHEDULED in the current language)
                                string rescheduledText = language == LANG_ENGLISH || string.IsNullOrWhiteSpace(f0Language.Status)
                                    ? "RESCHEDULED"
                                    : f0Language.Status;

                                textF0 = DisplayTextFormatter.FormatSpecialStatusTrain(
                                    board.TrainNumber,
                                    trainName,
                                    rescheduledText
                                );
                            }
                            else if (!isCancelled &&
                                     DisplayTextFormatter.IsSpecialStatusDetail(DisplayTextFormatter.SanitizeForDisplay(f0.Status)))
                            {
                                // Text-only statuses (0x04/0x07/0x08/0x10/0x13) in the current language.
                                // Classification uses the English status; the text shown is the current
                                // language's, falling back to English when the feed omits it.
                                // No language gate: English and Hindi must render identically, and the
                                // two-part split below has to apply to English as well.
                                string statusText = !string.IsNullOrWhiteSpace(f0Language.Status)
                                    ? f0Language.Status
                                    : f0.Status;

                                // The feed may pack every language into one '#'-separated string.
                                // Take this language's part so English and Hindi never share a line.
                                int languageIndex0 = boardLanguages.Count > 0
                                    ? languageStep % boardLanguages.Count
                                    : 0;
                                statusText = DisplayTextFormatter.SelectLanguagePart(statusText, languageIndex0);

                                // Two-part statuses (0x08/0x10/0x13) show the prefix on this
                                // frame and the station or route on the flip.
                                string headText = DisplayTextFormatter.TrySplitTwoPartStatus(
                                        statusText, out string h0, out _)
                                    ? h0
                                    : statusText;

                                textF0 = DisplayTextFormatter.FormatSpecialStatusTrain(
                                    board.TrainNumber,
                                    trainName,
                                    headText
                                );
                            }
                            else
                            {
                                textF0 = DisplayTextFormatter.FormatDisplayText(
                                    board.TrainNumber,
                                    trainName,
                                    board.ActualArrivalTime,
                                    board.ActualDepartureTime,
                                    board.ADFlag,
                                    board.PlatformNumber,
                                    isCancelled
                                );
                            }

                            int fontSize = board.FontSize ?? 13;

                            // A text-only status (0x04 Has Arrived, 0x07 Indefinite Late,
                            // Cancelled) has nothing to put on a second frame, so it must not
                            // split the slot in half. It holds the full slot, then the cycle
                            // moves on to the next configured language.
                            // Rescheduled and the two-part statuses (0x08/0x10/0x13) do have a
                            // second frame, so they still flip.
                            bool isTextOnlyStatus =
                                isCancelled ||
                                DisplayTextFormatter.IsSpecialStatusDetail(
                                    DisplayTextFormatter.SanitizeForDisplay(f0.Status));

                            // 0x08/0x10/0x13 flip to the station or route. The station may be
                            // inside the same string or in the feed's flip frame, and the feed
                            // may not set FlipRequired for them, so they decide for themselves.
                            bool isTwoPartStatus = DisplayTextFormatter.IsTwoPartStatus(
                                DisplayTextFormatter.SelectLanguagePart(f0.Status, 0));

                            bool hasSecondFrame = isRescheduled;
                            if (isTwoPartStatus)
                            {
                                TrainDisplayData f1English = TrainDisplayHelper.BuildTrainData(train, LANG_ENGLISH, 1);
                                hasSecondFrame = DisplayTextFormatter.GetTwoPartSecondFrame(
                                    DisplayTextFormatter.SelectLanguagePart(f0.Status, 0),
                                    DisplayTextFormatter.SelectLanguagePart(f1English.Status, 0)).Length > 0;
                            }

                            if ((f0.FlipRequired || f0.SpecialStatus || isTwoPartStatus) &&
                                (hasSecondFrame || !isTextOnlyStatus))
                            {
                                pageHasSpecialFlip = true;
                            }

                            bool boardPinged0 = IsHostReachable(board.BoardIp);
                            ShowTextOnBoardRow(row, textF0, fontSize, language, boardPinged0);
                        }
                        else
                        {
                            bool boardPingedWelcome = IsHostReachable(board.BoardIp);
                            bool needsClear = !string.IsNullOrEmpty(row.LastRenderedText);

                            if (this.InvokeRequired)
                            {
                                this.Invoke(new Action(() => {
                                    SetLabelTextIfChanged(row.TrainInfoLabel, "No Train Information Available");
                                    if (boardPingedWelcome && needsClear)
                                    {
                                        ClearBoardPreview(row.BoardPreview);
                                        row.LastRenderedText = "";
                                    }
                                }));
                            }
                            else
                            {
                                SetLabelTextIfChanged(row.TrainInfoLabel, "No Train Information Available");
                                if (boardPingedWelcome && needsClear)
                                {
                                    ClearBoardPreview(row.BoardPreview);
                                    row.LastRenderedText = "";
                                }
                            }
                        }
                    }

                    // Step 2: Handle half PTO delay and Frame 1 special status flip
                    if (pageHasSpecialFlip)
                    {
                        await DisplayHelper.DelaySeconds(languageSlotSeconds / 2.0, token);

                        for (int b = 0; b < _dynamicRows.Count; b++)
                        {
                            token.ThrowIfCancellationRequested();

                            // Render scratch copy: this loop writes each train's values onto the object,
                            // and _boardList[b] is rendered by the other path too. Mutating the shared
                            // entry left the last train's details on screen.
                            var board = CloneBoardData((b < _boardList.Count) ? _boardList[b] : new SingleLineDisplayBoardData());
                            var row = _dynamicRows[b];

                            List<string> boardLanguages = rowLanguages[b];
                            string language = boardLanguages[languageStep % boardLanguages.Count];

                            JsonObject? train = (b < page.Count) ? page[b] : null;

                            if (train != null)
                            {
                                TrainDisplayData f0 = TrainDisplayHelper.BuildTrainData(train, LANG_ENGLISH, 0);
                                int fontSize = board.FontSize ?? 13;

                                // pageHasSpecialFlip is per page: one Rescheduled train elsewhere on
                                // the page makes every board re-render here. A text-only status has
                                // no second frame, so re-rendering it dropped it onto the normal
                                // time+platform line for half a slot. Leave those rows untouched.
                                bool f1IsCancelled = DisplayTextFormatter.IsCancelledStatus(f0.Status);

                                bool f1IsTextOnly =
                                    f1IsCancelled ||
                                    DisplayTextFormatter.IsSpecialStatusDetail(
                                        DisplayTextFormatter.SanitizeForDisplay(f0.Status));

                                bool f1IsTwoPart = DisplayTextFormatter.IsTwoPartStatus(
                                    DisplayTextFormatter.SelectLanguagePart(f0.Status, 0));

                                bool f1HasSecondFrame = DisplayTextFormatter.IsRescheduledStatus(f0.Status);
                                if (f1IsTwoPart)
                                {
                                    TrainDisplayData f1Check = TrainDisplayHelper.BuildTrainData(train, LANG_ENGLISH, 1);
                                    f1HasSecondFrame = DisplayTextFormatter.GetTwoPartSecondFrame(
                                        DisplayTextFormatter.SelectLanguagePart(f0.Status, 0),
                                        DisplayTextFormatter.SelectLanguagePart(f1Check.Status, 0)).Length > 0;
                                }

                                if (f1IsTextOnly && !f1HasSecondFrame)
                                {
                                    // Nothing to show on frame 1 - keep frame 0 on screen.
                                }
                                else if (f0.FlipRequired || f0.SpecialStatus || f1IsTwoPart)
                                {
                                    TrainDisplayData f1 = TrainDisplayHelper.BuildTrainData(train, LANG_ENGLISH, 1);
                                    TrainDisplayData f1Language = language == LANG_ENGLISH
                                        ? f1
                                        : TrainDisplayHelper.BuildTrainData(train, language, 1);

                                    string trainName = GetTrainNameForLanguage(f0, language);
                                    board.ADFlag = f1.Status;

                                    bool isCancelled = DisplayTextFormatter.IsCancelledStatus(f0.Status) || DisplayTextFormatter.IsCancelledStatus(f1.Status);
                                    // A two-part status never takes the rescheduled branch - that one
                                    // prints arrival, departure and platform.
                                    bool isRescheduled = !f1IsTwoPart &&
                                        (DisplayTextFormatter.IsRescheduledStatus(f0.Status) || DisplayTextFormatter.IsRescheduledStatus(f1.Status));

                                    string textF1;
                                    if (isRescheduled)
                                    {
                                        // Spec 0x0F second half: the normal line - arrival, departure
                                        // and platform. The feed may carry the revised time in the flip
                                        // frame, so that is used when the board has no actual time.
                                        string expTime = DisplayTextFormatter.FormatTimeOnly(f1.Status);

                                        string reschArrival = !string.IsNullOrWhiteSpace(board.ActualArrivalTime)
                                            ? board.ActualArrivalTime
                                            : expTime;

                                        string reschDeparture = !string.IsNullOrWhiteSpace(board.ActualDepartureTime)
                                            ? board.ActualDepartureTime
                                            : expTime;

                                        textF1 = DisplayTextFormatter.FormatDisplayText(
                                            board.TrainNumber,
                                            trainName,
                                            reschArrival,
                                            reschDeparture,
                                            "RESCHEDULED",
                                            board.PlatformNumber,
                                            isCancelled
                                        );
                                    }
                                    else
                                    {
                                        // Other special statuses: flip text in the current language (English if missing)
                                        string flipText = language == LANG_ENGLISH || string.IsNullOrWhiteSpace(f1Language.Status)
                                            ? f1.Status
                                            : f1Language.Status;

                                        // Same '#'-packed language handling as frame 0.
                                        int languageIndex1 = boardLanguages.Count > 0
                                            ? languageStep % boardLanguages.Count
                                            : 0;
                                        flipText = DisplayTextFormatter.SelectLanguagePart(flipText, languageIndex1);

                                        if (f1IsTwoPart)
                                        {
                                            // 0x08/0x10/0x13 second half: station or route only, never a
                                            // time. When there is nothing distinct, hold frame 0's text.
                                            TrainDisplayData f0Lang1 = language == LANG_ENGLISH
                                                ? f0
                                                : TrainDisplayHelper.BuildTrainData(train, language, 0);

                                            string head1 = DisplayTextFormatter.SelectLanguagePart(
                                                !string.IsNullOrWhiteSpace(f0Lang1.Status) ? f0Lang1.Status : f0.Status,
                                                languageIndex1);

                                            string second = DisplayTextFormatter.GetTwoPartSecondFrame(head1, flipText);

                                            if (second.Length > 0)
                                                flipText = second;
                                            else
                                                flipText = DisplayTextFormatter.TrySplitTwoPartStatus(head1, out string hh, out _)
                                                    ? hh
                                                    : head1;
                                        }
                                        else if (DisplayTextFormatter.TrySplitTwoPartStatus(flipText, out _, out string t1))
                                        {
                                            flipText = t1;
                                        }

                                        textF1 = DisplayTextFormatter.FormatSpecialStatusTrain(
                                            f0.TrainNo,
                                            trainName,
                                            flipText
                                        );
                                    }

                                    bool boardPinged1 = IsHostReachable(board.BoardIp);
                                    ShowTextOnBoardRow(row, textF1, fontSize, language, boardPinged1);
                                }
                            }
                        }

                        await DisplayHelper.DelaySeconds(languageSlotSeconds / 2.0, token);
                    }
                    else
                    {
                        await DisplayHelper.DelaySeconds(languageSlotSeconds, token);
                    }
                }
            }
        }

        private void UpdateBoardWithNoTrains()
        {
            if (_boardList == null || _boardList.Count == 0) return;

            foreach (var row in _dynamicRows)
            {
                int fontSize = row.BoardData?.FontSize ?? 13;
                bool boardPingedNoTrains = IsHostReachable(row.BoardData?.BoardIp);
                bool needsClear = !string.IsNullOrEmpty(row.LastRenderedText);

                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => {
                        SetLabelTextIfChanged(row.TrainInfoLabel, "No Train Information Available");
                        if (boardPingedNoTrains && needsClear)
                        {
                            ClearBoardPreview(row.BoardPreview);
                            row.LastRenderedText = "";
                        }
                    }));
                }
                else
                {
                    SetLabelTextIfChanged(row.TrainInfoLabel, "No Train Information Available");
                    if (boardPingedNoTrains && needsClear)
                    {
                        ClearBoardPreview(row.BoardPreview);
                        row.LastRenderedText = "";
                    }
                }
            }
        }
    }

    /// <summary>
    /// Renders a single-line LED display board with circular bulbs.
    /// </summary>
    public class SingleLineBoardPreview : Panel
    {
        private bool[,] ledMatrix;
        private int rows = 32;
        private int cols = 672;

        public Color OnColor { get; set; } = Color.FromArgb(255, 193, 7);
        public Color OffColor { get; set; } = Color.FromArgb(60, 60, 60);
        public Color FaultColor { get; set; } = Color.FromArgb(244, 67, 54);

        public SingleLineBoardPreview()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            ResizeRedraw = true;
            ledMatrix = new bool[rows, cols];
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

            float size = Math.Min(pixelW, pixelH) * 0.65f;
            if (size < 1.5f) size = 1.5f;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    const float margin = 0.2f;

                    float x = col * pixelW + margin;
                    float y = row * pixelH + margin;

                    bool isActive = ledMatrix[row, col];

                    if (isActive)
                    {
                        using (var glowBrush = new SolidBrush(Color.FromArgb(60, OnColor)))
                        {
                            g.FillEllipse(glowBrush, x - 1f, y - 1f, size + 2, size + 1);
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
            // Override to prevent flicker
        }
    }

    public static class DisplayTextFormatter
    {
        //public static string FormatDisplayText(string trainNumber, string trainName, string arrivalTime, string departureTime, string status, string platformNumber, bool isCancelled = false, string defaultMessage = "")
        //{
        //    if (string.IsNullOrEmpty(trainNumber))
        //    {
        //        return string.Empty;
        //    }

        //    string sanitizedTrainNo = SanitizeForDisplay(trainNumber);
        //    string sanitizedTrainName = SanitizeForDisplay(trainName);
        //    string sanitizedArrival = FormatTimeOnly(arrivalTime);
        //    string sanitizedDeparture = FormatTimeOnly(departureTime);
        //    string sanitizedStatus = SanitizeForDisplay(status);
        //    string sanitizedPlatform = SanitizeForDisplay(platformNumber);

        //    bool isCancelledStatus = isCancelled || IsCancelledStatus(sanitizedStatus);

        //    if (isCancelledStatus)
        //    {
        //        return FormatCancelledTrain(sanitizedTrainNo, sanitizedTrainName);
        //    }

        //    if (IsSpecialStatusDetail(sanitizedStatus))
        //    {
        //        return FormatSpecialStatusTrain(sanitizedTrainNo, sanitizedTrainName, sanitizedStatus);
        //    }

        //    string timeToShow = sanitizedArrival;
        //    string timeLabel = "A";

        //    if (IsDepartureStatus(sanitizedStatus))
        //    {
        //        timeToShow = sanitizedDeparture;
        //        timeLabel = "D";
        //    }

        //    if (string.IsNullOrEmpty(timeToShow))
        //    {
        //        timeToShow = sanitizedDeparture;
        //        timeLabel = "D";
        //    }

        //    if (string.IsNullOrEmpty(timeToShow))
        //    {
        //        timeToShow = "N/A";
        //        timeLabel = "";
        //    }

        //    string platformDisplay = !string.IsNullOrEmpty(sanitizedPlatform) ?
        //                             $"{timeLabel} {sanitizedPlatform}" :
        //                             timeLabel;

        //    if (string.IsNullOrEmpty(sanitizedPlatform) && !string.IsNullOrEmpty(timeLabel))
        //    {
        //        platformDisplay = timeLabel;
        //    }

        //    return $"{sanitizedTrainNo,-8} {sanitizedTrainName,-20} {timeToShow,-10} {platformDisplay,-6}";
        //}

        /// <param name="status">English status, used only to classify.</param>
        /// <param name="displayStatus">Status text in the current language; falls back to <paramref name="status"/>.</param>
        public static string FormatDisplayText(string trainNumber, string trainName, string arrivalTime, string departureTime, string status, string platformNumber, bool isCancelled = false, string defaultMessage = "", string displayStatus = null)
        {
            if (string.IsNullOrEmpty(trainNumber))
            {
                return string.Empty;
            }

            string sanitizedTrainNo = SanitizeForDisplay(trainNumber);
            string sanitizedTrainName = SanitizeForDisplay(trainName);
            string sanitizedArrival = FormatTimeOnly(arrivalTime);
            string sanitizedDeparture = FormatTimeOnly(departureTime);
            string sanitizedStatus = SanitizeForDisplay(status);
            string sanitizedPlatform = SanitizeForDisplay(platformNumber);

            bool isCancelledStatus = isCancelled || IsCancelledStatus(sanitizedStatus);

            // Classification uses the English status; the text shown uses the current
            // language so the Hindi frame reads the Hindi wording, not "CANCELLED".
            string shownStatus = string.IsNullOrWhiteSpace(displayStatus)
                ? sanitizedStatus
                : SanitizeForDisplay(displayStatus);

            if (isCancelledStatus)
            {
                return FormatCancelledTrain(sanitizedTrainNo, sanitizedTrainName, shownStatus);
            }

            if (IsSpecialStatusDetail(sanitizedStatus))
            {
                return FormatSpecialStatusTrain(sanitizedTrainNo, sanitizedTrainName, shownStatus);
            }

            string arrivalDisplay = string.IsNullOrEmpty(sanitizedArrival) ? "--:--" : sanitizedArrival;
            string departureDisplay = string.IsNullOrEmpty(sanitizedDeparture) ? "--:--" : sanitizedDeparture;

            string platformDisplay = !string.IsNullOrEmpty(sanitizedPlatform)
                ? $"  {sanitizedPlatform}"
                : string.Empty;

            return $"{sanitizedTrainNo,-8} {sanitizedTrainName,-20} {arrivalDisplay,-6} {departureDisplay,-6} {platformDisplay}".TrimEnd();
        }
        private static string FormatCancelledTrain(string trainNumber, string trainName, string statusText = "")
        {
            // Spec 0x06 / 0x0B: status text only - no arrival, departure or platform.
            string text = string.IsNullOrWhiteSpace(statusText) ? "CANCELLED" : statusText;
            return $"{trainNumber,-8} {trainName,-25} {text}";
        }

        public static string FormatSpecialStatusTrain( string trainNumber, string trainName, string statusText)
        {
            if (string.IsNullOrEmpty(trainNumber))
            {
                return string.Empty;
            }

            string sanitizedTrainNo = SanitizeForDisplay(trainNumber);
            string sanitizedTrainName = SanitizeForDisplay(trainName);
            string sanitizedStatus = SanitizeForDisplay(statusText);

            return $"{sanitizedTrainNo,-8} {sanitizedTrainName,-25} {sanitizedStatus}";
        }

        /// <summary>How a train status is rendered, per the RDSO IPIS specification.</summary>
        public enum StatusDisplayKind
        {
            /// <summary>Normal line: train no, name, arrival, departure, platform.</summary>
            TimeAndPlatform,
            /// <summary>Status text replaces the times and platform (0x04, 0x06-0x08, 0x0B, 0x10, 0x13).</summary>
            TextOnly,
            /// <summary>Status text on the first flip, then the normal line (0x0F Rescheduled).</summary>
            TextThenTimeAndPlatform
        }

        /// <summary>
        /// RDSO status table, codes 0x01-0x13. Matched on the English status text,
        /// most specific key first. A/D is deliberately absent: it is not displayed.
        /// </summary>
        private static readonly (string Key, StatusDisplayKind Kind)[] StatusTable =
        {
            ("HAS ARRIVED",         StatusDisplayKind.TextOnly),                 // 0x04
            ("INDEFINITE LATE",     StatusDisplayKind.TextOnly),                 // 0x07
            ("TERMINATED AT",       StatusDisplayKind.TextOnly),                 // 0x08
            ("CHANGE OF SOURCE",    StatusDisplayKind.TextOnly),                 // 0x13
            ("START AT",            StatusDisplayKind.TextOnly),                 // 0x13
            ("DIVERTED",            StatusDisplayKind.TextOnly),                 // 0x10
            ("CANCEL",              StatusDisplayKind.TextOnly),                 // 0x06, 0x0B
            ("CNL",                 StatusDisplayKind.TextOnly),
            ("RESCH",               StatusDisplayKind.TextThenTimeAndPlatform),  // 0x0F
            ("WILL ARRIVE SHORTLY", StatusDisplayKind.TimeAndPlatform),          // 0x02
            ("IS ARRIVING",         StatusDisplayKind.TimeAndPlatform),          // 0x03
            ("RUNNING RIGHT TIME",  StatusDisplayKind.TimeAndPlatform),          // 0x01, 0x0A
            ("RUNNING LATE",        StatusDisplayKind.TimeAndPlatform),          // 0x05
            ("DELAY DEPARTURE",     StatusDisplayKind.TimeAndPlatform),          // 0x11
            ("IS READY TO LEAVE",   StatusDisplayKind.TimeAndPlatform),          // 0x0C
            ("IS ON PLATFORM",      StatusDisplayKind.TimeAndPlatform),          // 0x0D
            ("PLATFORM CHANGE",     StatusDisplayKind.TimeAndPlatform),          // 0x09, 0x12
            ("DEPARTED",            StatusDisplayKind.TimeAndPlatform),          // 0x0E
            ("ARRIVED",             StatusDisplayKind.TextOnly),
        };

        /// <summary>Legacy short codes that must match exactly rather than by substring.</summary>
        private static readonly Dictionary<string, StatusDisplayKind> ShortStatusCodes =
            new Dictionary<string, StatusDisplayKind>(StringComparer.OrdinalIgnoreCase)
            {
                { "C",   StatusDisplayKind.TextOnly },
                { "CL",  StatusDisplayKind.TextOnly },
                { "X",   StatusDisplayKind.TextOnly },
                { "RES", StatusDisplayKind.TextThenTimeAndPlatform },
                { "SCH", StatusDisplayKind.TextThenTimeAndPlatform }
            };

        /// <summary>
        /// Classifies a status into one of the three display shapes. Unknown statuses
        /// fall back to the normal line, so a new status never blanks a board.
        /// </summary>
        public static StatusDisplayKind GetStatusKind(string status)
        {
            string upper = SanitizeForDisplay(status).ToUpperInvariant();
            if (upper.Length == 0) return StatusDisplayKind.TimeAndPlatform;

            if (ShortStatusCodes.TryGetValue(upper, out StatusDisplayKind shortKind))
                return shortKind;

            foreach (var entry in StatusTable)
            {
                if (upper.Contains(entry.Key))
                    return entry.Kind;
            }

            if (upper.StartsWith("AT ") || upper.StartsWith("VIA "))
                return StatusDisplayKind.TextOnly;

            return StatusDisplayKind.TimeAndPlatform;
        }

        /// <summary>
        /// Spec 0x08 / 0x10 / 0x13 arrive as one string, e.g. "Terminated At Kanpur".
        /// Splits it into the prefix phrase and the station or route that follows.
        /// Returns false when the status is not two-part, or carries no remainder.
        /// </summary>
        private static readonly string[] TwoPartStatusPrefixes =
        {
            "TERMINATED AT",      // 0x08
            "DIVERTED VIA",       // 0x10
            "DIVERTED",           // 0x10
            "CHANGE OF SOURCE",   // 0x13
            "START AT"            // 0x13
        };

        /// <summary>
        /// Some statuses arrive with every language in one string, separated by '#',
        /// e.g. "Indefinite Late#अनिश्चित देरी से". Returns the part for the given
        /// language index, the last part when there are fewer parts than languages,
        /// and the text unchanged when there is no separator.
        /// </summary>
        public static string SelectLanguagePart(string text, int languageIndex)
        {
            string raw = SanitizeForDisplay(text);
            if (raw.Length == 0 || raw.IndexOf('#') < 0) return raw;

            string[] parts = raw.Split('#');
            var usable = new List<string>();

            foreach (string part in parts)
            {
                string trimmed = part.Trim();
                if (trimmed.Length > 0) usable.Add(trimmed);
            }

            if (usable.Count == 0) return raw;
            if (languageIndex < 0) languageIndex = 0;

            return languageIndex < usable.Count ? usable[languageIndex] : usable[usable.Count - 1];
        }

        public static bool TrySplitTwoPartStatus(string status, out string head, out string tail)
        {
            string raw = SanitizeForDisplay(status);
            head = raw;
            tail = string.Empty;

            if (raw.Length == 0) return false;

            string upper = raw.ToUpperInvariant();

            foreach (string prefix in TwoPartStatusPrefixes)
            {
                if (!upper.StartsWith(prefix)) continue;

                string remainder = raw.Substring(prefix.Length).Trim();
                if (remainder.Length == 0) return false;

                head = raw.Substring(0, prefix.Length);
                tail = remainder;
                return true;
            }

            return false;
        }

        /// <summary>True for 0x08 / 0x10 / 0x13, with or without a station in the text.</summary>
        public static bool IsTwoPartStatus(string status)
        {
            string upper = SanitizeForDisplay(status).ToUpperInvariant();
            if (upper.Length == 0) return false;

            foreach (string prefix in TwoPartStatusPrefixes)
            {
                if (upper.StartsWith(prefix)) return true;
            }

            return false;
        }

        /// <summary>
        /// Second-half text for 0x08 / 0x10 / 0x13: the station or route only.
        /// Taken from the remainder of a combined string ("Diverted via Agra"), or else
        /// from the feed's flip frame ("Diverted" then "Agra"). Never returns a time, so
        /// the flip can never show arrival/departure. Empty when there is nothing distinct.
        /// </summary>
        public static string GetTwoPartSecondFrame(string frame0Text, string frame1Text)
        {
            if (TrySplitTwoPartStatus(frame0Text, out _, out string tail0)) return tail0;
            if (TrySplitTwoPartStatus(frame1Text, out _, out string tail1)) return tail1;

            string first = SanitizeForDisplay(frame0Text);
            string second = SanitizeForDisplay(frame1Text);

            bool secondIsTime = Regex.IsMatch(second, @"^\d{1,2}:\d{2}(:\d{2})?$");

            if (second.Length > 0 &&
                !secondIsTime &&
                !string.Equals(second, first, StringComparison.OrdinalIgnoreCase))
            {
                return second;
            }

            return string.Empty;
        }

        public static bool IsSpecialStatusDetail(string status)
        {
            if (string.IsNullOrEmpty(status)) return false;

            // Cancelled has its own renderer, so it is not treated as a generic text status.
            return GetStatusKind(status) == StatusDisplayKind.TextOnly && !IsCancelledStatus(status);
        }

        public static bool IsRescheduledStatus(string status)
        {
            if (string.IsNullOrEmpty(status)) return false;
            string upper = status.ToUpperInvariant().Trim();
            return GetStatusKind(upper) == StatusDisplayKind.TextThenTimeAndPlatform;
        }

        public static string SanitizeForDisplay(string raw)
        {
            if (string.IsNullOrEmpty(raw)) return string.Empty;

            var sb = new StringBuilder(raw.Length);
            foreach (char c in raw)
            {
                if (c == '\r' || c == '\n' || c == '\t' || c == '\0')
                    continue;

                if (char.IsControl(c))
                    continue;

                if (c == '\u00A0')
                {
                    sb.Append(' ');
                    continue;
                }

                sb.Append(c);
            }

            string cleaned = Regex.Replace(sb.ToString(), @"\s{2,}", " ");
            return cleaned.Trim();
        }

        public static string FormatTimeOnly(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return string.Empty;

            string cleaned = SanitizeForDisplay(raw);

            Match match = Regex.Match(cleaned, @"\b([01]?\d|2[0-3]):[0-5]\d(?::[0-5]\d)?\b");
            if (match.Success)
            {
                string timeVal = match.Value;
                if (timeVal.Length > 5)
                    timeVal = timeVal.Substring(0, 5);
                return timeVal;
            }

            if (DateTime.TryParse(cleaned, out DateTime dt))
            {
                return dt.ToString("HH:mm");
            }

            return cleaned;
        }

        public static bool IsCancelledStatus(string status)
        {
            if (string.IsNullOrEmpty(status)) return false;

            string upperStatus = status.ToUpperInvariant().Trim();
            return upperStatus == "C" ||
                   upperStatus == "CNL" ||
                   upperStatus == "CANCELLED" ||
                   upperStatus == "CL" ||
                   upperStatus == "X" ||
                   upperStatus.Contains("CANCEL") ||
                   upperStatus.Contains("CNL");
        }

        public static bool IsDepartureStatus(string status)
        {
            if (string.IsNullOrEmpty(status)) return false;

            string upperStatus = status.ToUpperInvariant().Trim();
            return upperStatus == "D" ||
                   upperStatus == "DEP" ||
                   upperStatus == "DEPARTURE" ||
                   upperStatus == "DEPART" ||
                   upperStatus == "DP" ||
                   upperStatus.Contains("DEP");
        }
    }
}