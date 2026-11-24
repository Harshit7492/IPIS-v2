

using DataBaseHandler;
using DataModel;
using Global_Data;
using Helper;
using LogManager;

namespace IPIS
{
    public partial class ucLinkStatus : UserControl
    {
        List<HUBCONFIG> _lstpdc = new List<HUBCONFIG>();
        List<BoardInfo> _lstboardinfo = new List<BoardInfo>();
        private int _hoveredTabIndex = -1;
        private static readonly Font DotFont = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point);
        private const string EdgeNmsIp = "10.0.0.252";

        public ucLinkStatus()
        {
            InitializeComponent();
            tabLinkStatus.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabLinkStatus.DrawItem += tabLinkStatus_DrawItem;
            tabLinkStatus.MouseMove += tabLinkStatus_MouseMove;
            tabLinkStatus.MouseLeave += tabLinkStatus_MouseLeave;
        }

        private void tabLinkStatus_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (tabLinkStatus.TabCount == 0) return;
            if (e.Index < 0 || e.Index >= tabLinkStatus.TabCount) return;

            TabPage page = tabLinkStatus.TabPages[e.Index];
            Rectangle rect = tabLinkStatus.GetTabRect(e.Index);

            bool isActive = tabLinkStatus.SelectedIndex == e.Index;
            bool isHovered = e.Index == _hoveredTabIndex;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            Color backColor;
            Color textColor;
            Color borderColor;

            if (isActive)
            {
                backColor = Color.SteelBlue;
                textColor = Color.White;
                borderColor = Color.FromArgb(50, 100, 150);
            }
            else if (isHovered)
            {
                backColor = Color.FromArgb(220, 230, 245);
                textColor = Color.SteelBlue;
                borderColor = Color.FromArgb(170, 190, 220);
            }
            else
            {
                backColor = Color.FromArgb(242, 245, 250);
                textColor = Color.FromArgb(80, 90, 105);
                borderColor = Color.FromArgb(220, 225, 235);
            }

            using (SolidBrush backBrush = new SolidBrush(backColor))
            {
                e.Graphics.FillRectangle(backBrush, rect);
            }

            using (Pen borderPen = new Pen(borderColor, 1.5f))
            {
                Rectangle borderRect = new Rectangle(rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
                e.Graphics.DrawRectangle(borderPen, borderRect);
            }

            Font font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.SingleLine;

            TextRenderer.DrawText(e.Graphics, page.Text, font, rect, textColor, flags);
            font.Dispose();
        }

        private void tabLinkStatus_MouseMove(object sender, MouseEventArgs e)
        {
            int hoverIndex = -1;
            for (int i = 0; i < tabLinkStatus.TabCount; i++)
            {
                if (tabLinkStatus.GetTabRect(i).Contains(e.Location))
                {
                    hoverIndex = i;
                    break;
                }
            }

            if (hoverIndex != _hoveredTabIndex)
            {
                _hoveredTabIndex = hoverIndex;
                tabLinkStatus.Invalidate();
            }
        }

        private void tabLinkStatus_MouseLeave(object sender, EventArgs e)
        {
            if (_hoveredTabIndex != -1)
            {
                _hoveredTabIndex = -1;
                tabLinkStatus.Invalidate();
            }
        }

        private void ucLinkStatus_Load(object sender, EventArgs e)
        {
            BindPDCGrid();
        }

        private void tabLinkStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabLinkStatus.SelectedTab.Name == "TAB_HUB")
            {
                BindPDCGrid();
            }
            else if (tabLinkStatus.SelectedTab.Name == "TAB_TADB")
            {
                BindTADBGrid();
            }
            else if (tabLinkStatus.SelectedTab.Name == "TAB_CGDB")
            {
                BindCGDBGrid();
            }
            else if (tabLinkStatus.SelectedTab.Name == "TAB_VDB")
            {
                BindVDBGrid();
            }
            else if (tabLinkStatus.SelectedTab.Name == "TAB_CLIENT")
            {
                BindClientGrid();
            }
        }

        #region PDC LINK CHECK
        private void InitializePDCGrid()
        {
            dgvPdcStatus.Columns.Clear();
            dgvPdcStatus.ReadOnly = true;
            dgvPdcStatus.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPdcStatus.MultiSelect = false;
            dgvPdcStatus.RowHeadersVisible = false;
            dgvPdcStatus.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

            dgvPdcStatus.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ID",
                HeaderText = "ID",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            dgvPdcStatus.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NAME",
                HeaderText = "NAME",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 100,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            dgvPdcStatus.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PLATFORM",
                HeaderText = "PLATFORM",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 100,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            dgvPdcStatus.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PDCIP",
                HeaderText = "IP ADDRESS",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Width = 130,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            dgvPdcStatus.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "STATUS",
                HeaderText = "LINK STATUS",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Width = 120,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
            dgvPdcStatus.SelectionChanged += (s, e) =>
            {
                dgvPdcStatus.ClearSelection();
            };
        }
        private async void BindPDCGrid()
        {
            InitializePDCGrid();
            _lstpdc = _dbService.GET_HUBCONFIG(0);
            int counter = 1;
            foreach (var item in _lstpdc)
            {
                dgvPdcStatus.Rows.Add(
                    counter,
                    item.NODE_NAME,
                    item.PF_ID,
                    item.HUB_IP,
                    ""
                );
                counter++;
            }

            await CheckPDCStatusAsync();
        }

        private async Task CheckPDCStatusAsync()
        {
            try
            {
                for (int i = 0; i < dgvPdcStatus.Rows.Count; i++)
                {
                    var row = dgvPdcStatus.Rows[i];

                    string ip = row.Cells["PDCIP"].Value?.ToString();

                    if (string.IsNullOrWhiteSpace(ip))
                        continue;

                    row.Cells["STATUS"].Value = "wait...";
                    dgvPdcStatus.Refresh();

                    var result = await PingHelper.PingWithRetryAsync(ip);
                    SetLinkStatus(row, result.IsSuccess);
                }
            }
            catch (Exception ex) { }
        }
        #endregion

        #region TADB LINK CHECK
        private void InitializeTADBGrid()
        {
            dgvTADBStatus.Columns.Clear();
            dgvTADBStatus.ReadOnly = true;
            dgvTADBStatus.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTADBStatus.MultiSelect = false;
            dgvTADBStatus.RowHeadersVisible = false;
            dgvTADBStatus.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgvTADBStatus.RowTemplate.Height = 40;
            dgvTADBStatus.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ID",
                HeaderText = "ID",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            dgvTADBStatus.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NAME",
                HeaderText = "BOARD NAME",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 100,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
            dgvTADBStatus.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "LOCATION",
                HeaderText = "INSTALLED LOCATION",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 100,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
            dgvTADBStatus.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PLATFORM",
                HeaderText = "PLATFORM",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 80,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
            dgvTADBStatus.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PDCI",
                HeaderText = "Number of Lines",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 70
            });
            dgvTADBStatus.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Boardip",
                HeaderText = "BOARD IP",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Width = 130,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
            dgvTADBStatus.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "INTENSITY",
                HeaderText = "INTENSITY",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Width = 100,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
            dgvTADBStatus.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "STATUS",
                HeaderText = "LINK STATUS",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Width = 120,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
            dgvTADBStatus.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DETAILSSEND",
                HeaderText = "DATA SEND STATUS",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 100,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            dgvTADBStatus.SelectionChanged += (s, e) =>
            {
                dgvTADBStatus.ClearSelection();
            };
        }

        private async void BindTADBGrid()
        {
            InitializeTADBGrid();

            _lstboardinfo = _dbService.GET_BOARD_DETAILS("TADB");
            List<BoardSettingInfo> boardSettings = _dbService.GET_BOARD_SETTING(0) ?? new List<BoardSettingInfo>();

            int counter = 1;

            foreach (var item in _lstboardinfo)
            {
                var info = _dbService.GET_BOARD_DETAILS_HealthStatus("TADB", item.BOARD_IP);

                string pf = item.SELECTED_PF;

                if (item.SELECTED_PF == "0")
                    pf = "All";

                int NoOfLine = item.BOARD_LINE;

                string details = string.Join(", ", new[] { info.TrainInfo, info.Messages }.Where(x => !string.IsNullOrWhiteSpace(x)));
                if (string.IsNullOrWhiteSpace(details))
                {
                    details = " ";
                }

                BoardSettingInfo boardSetting =
                    boardSettings.FirstOrDefault(b => item.BOARD_ID > 0 && b.BOARD_ID == item.BOARD_ID)
                    ?? boardSettings.FirstOrDefault(b => !string.IsNullOrWhiteSpace(item.BOARD_IP)
                                                         && string.Equals(b.BOARD_IP?.Trim(), item.BOARD_IP.Trim(), StringComparison.OrdinalIgnoreCase))
                    ?? boardSettings.FirstOrDefault(b => !string.IsNullOrWhiteSpace(item.SELECTED_PF)
                                                         && string.Equals(b.SELECTED_PF?.Trim(), item.SELECTED_PF.Trim(), StringComparison.OrdinalIgnoreCase)
                                                         && string.Equals(b.PF_NO?.Trim(), item.PF_NO?.Trim(), StringComparison.OrdinalIgnoreCase))
                    ?? boardSettings.FirstOrDefault(b => item.NODE_ID > 0 && b.HUB_ID == item.HUB_ID && b.NODE_ID == item.NODE_ID);
                string intensity = boardSetting?.Intensity?.Trim() ?? string.Empty;

                dgvTADBStatus.Rows.Add(
                    counter,
                    item.BOARD_NAME,
                    item.BOARD_LOCATION,
                    pf,
                    NoOfLine,
                    item.BOARD_IP,
                    intensity,
                    "",
                    details
                );

                counter++;
            }

            await CheckTADBtatusAsync();
        }
        private async Task CheckTADBtatusAsync()
        {
            try
            {
                for (int i = 0; i < dgvTADBStatus.Rows.Count; i++)
                {
                    var row = dgvTADBStatus.Rows[i];

                    string ip = row.Cells["Boardip"].Value?.ToString();

                    if (string.IsNullOrWhiteSpace(ip))
                        continue;

                    row.Cells["STATUS"].Value = "wait...";
                    dgvTADBStatus.Refresh();
                    var result = await PingHelper.PingWithRetryAsync(ip);
                    SetLinkStatus(row, result.IsSuccess);
                }
            }
            catch (Exception ex) { }
        }
        #endregion

        #region CGDB LINK CHECK
        private void InitializeCGDBGrid()
        {
            dgvCGDBStatus.Columns.Clear();
            dgvCGDBStatus.ReadOnly = true;
            dgvCGDBStatus.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCGDBStatus.MultiSelect = false;
            dgvCGDBStatus.RowHeadersVisible = false;

            dgvCGDBStatus.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ID",
                HeaderText = "ID",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                Width = 50,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            dgvCGDBStatus.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PDCNAME",
                HeaderText = "PDC",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 120,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
            dgvCGDBStatus.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PDCIP",
                HeaderText = "PDC IP",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Width = 130,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
            dgvCGDBStatus.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PLATFORM",
                HeaderText = "PLATFORM",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 80,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            dgvCGDBStatus.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CGDBIP",
                HeaderText = "CGDB IP",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Width = 130,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
            dgvCGDBStatus.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "INTENSITY",
                HeaderText = "INTENSITY",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Width = 100,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
            dgvCGDBStatus.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "STATUS",
                HeaderText = "LINK STATUS",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Width = 120,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
            dgvCGDBStatus.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DETAILSSEND",
                HeaderText = "DATA SEND STATUS",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 100,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            dgvCGDBStatus.SelectionChanged += (s, e) =>
            {
                dgvCGDBStatus.ClearSelection();
            };
        }
        private async void BindCGDBGrid()
        {
            InitializeCGDBGrid();

            _lstboardinfo = _dbService.GET_BOARD_DETAILS("CGDB");
            List<BoardSettingInfo> boardSettings = _dbService.GET_BOARD_SETTING(0) ?? new List<BoardSettingInfo>();

            int counter = 1;

            foreach (var item in _lstboardinfo)
            {
                BoardSettingInfo boardSetting =
                    boardSettings.FirstOrDefault(b => item.BOARD_ID > 0 && b.BOARD_ID == item.BOARD_ID)
                    ?? boardSettings.FirstOrDefault(b => !string.IsNullOrWhiteSpace(item.BOARD_IP)
                                                         && string.Equals(b.BOARD_IP?.Trim(), item.BOARD_IP.Trim(), StringComparison.OrdinalIgnoreCase))
                    ?? boardSettings.FirstOrDefault(b => !string.IsNullOrWhiteSpace(item.SELECTED_PF)
                                                         && string.Equals(b.SELECTED_PF?.Trim(), item.SELECTED_PF.Trim(), StringComparison.OrdinalIgnoreCase)
                                                         && string.Equals(b.PF_NO?.Trim(), item.PF_NO?.Trim(), StringComparison.OrdinalIgnoreCase))
                    ?? boardSettings.FirstOrDefault(b => item.NODE_ID > 0 && b.HUB_ID == item.HUB_ID && b.NODE_ID == item.NODE_ID);
                string intensity = boardSetting?.Intensity?.Trim() ?? string.Empty;

                var pfList = item.SELECTED_PF.Split(',');

                foreach (var pf in pfList)
                {
                    var parts = pf.Split('+');

                    string lastPart = parts.Length > 1
                        ? parts.Last()
                        : parts[0];

                    string ip = $"10.0.{item.PF_NO}.{lastPart}";

                    var info = _dbService.GET_BOARD_DETAILS_HealthStatus("CGDB", ip);

                    string details = string.Join(", ", new[] { info.TrainInfo, info.Messages }.Where(x => !string.IsNullOrWhiteSpace(x)));
                    if (string.IsNullOrWhiteSpace(details))
                    {
                        details = " ";
                    }

                    dgvCGDBStatus.Rows.Add(
                        counter,
                        "Platform Data Controller",
                        item.HUB_IP,
                        item.HUB_PF,
                        ip,
                        intensity,
                        "",
                        details
                    );

                    counter++;
                }
            }

            await CheckCGDBtatusAsync();
        }

        private async Task CheckCGDBtatusAsync()
        {
            try
            {
                for (int i = 0; i < dgvCGDBStatus.Rows.Count; i++)
                {
                    var row = dgvCGDBStatus.Rows[i];

                    string ip = row.Cells["CGDBIP"].Value?.ToString();

                    if (string.IsNullOrWhiteSpace(ip))
                        continue;

                    row.Cells["STATUS"].Value = "wait...";
                    dgvCGDBStatus.Refresh();
                    var result = await PingHelper.PingWithRetryAsync(ip);
                    SetLinkStatus(row, result.IsSuccess);

                    if (!result.IsSuccess)
                    {
                        row.Cells["DETAILSSEND"].Value = " ";
                    }
                }
            }
            catch (Exception ex) { }
        }
        #endregion

        #region VDB / AVDB LINK CHECK
        private void InitializeVDBGrid()
        {
            dgvVDBStatus.Columns.Clear();
            dgvVDBStatus.ReadOnly = true;
            dgvVDBStatus.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVDBStatus.MultiSelect = false;
            dgvVDBStatus.RowHeadersVisible = false;
            dgvVDBStatus.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgvVDBStatus.RowTemplate.Height = 35;

            dgvVDBStatus.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SR_NO",
                HeaderText = "SR NO",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            dgvVDBStatus.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NAME",
                HeaderText = "BOARD NAME",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 120,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            // Main Board IP
            dgvVDBStatus.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "BOARD_IP",
                HeaderText = "BOARD IP",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Width = 140,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            // Module IP
            dgvVDBStatus.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MODULE_IP",
                HeaderText = "MODULE IP",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Width = 140,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            dgvVDBStatus.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "STATUS",
                HeaderText = "LINK STATUS",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Width = 150,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            dgvVDBStatus.SelectionChanged += (s, e) =>
            {
                dgvVDBStatus.ClearSelection();
            };
        }
        private async void BindVDBGrid()
        {
            InitializeVDBGrid();

            var boardList = _dbService.GET_BOARD_SETTING(0)
                           ?? new List<BoardSettingInfo>();

            var vdbBoards = boardList.Where(x =>
                (string.Equals(x.BOARD_NAME, "Video Display Board", StringComparison.OrdinalIgnoreCase)
                 || string.Equals(x.BOARD_NAME, "Advance Video Display Board", StringComparison.OrdinalIgnoreCase))
                && !string.IsNullOrWhiteSpace(x.MODULE_IPS))
                .ToList();

            int counter = 1;

            foreach (var board in vdbBoards)
            {
                string boardIp = board.BOARD_IP?.Trim() ?? string.Empty;

                string[] moduleIps = board.MODULE_IPS?
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    ?? Array.Empty<string>();

                for (int i = 0; i < moduleIps.Length; i++)
                {
                    string moduleIp = moduleIps[i].Trim();

                    if (string.IsNullOrWhiteSpace(moduleIp))
                        continue;

                    string moduleName = $"{board.BOARD_NAME} - Module {i + 1}";

                    dgvVDBStatus.Rows.Add(
                        counter,
                        moduleName,
                        boardIp,
                        moduleIp,
                        "wait..."
                    );

                    counter++;
                }
            }

            await CheckVDBStatusAsync();
        }
        private async Task CheckVDBStatusAsync()
        {
            try
            {
                for (int i = 0; i < dgvVDBStatus.Rows.Count; i++)
                {
                    var row = dgvVDBStatus.Rows[i];

                    string moduleIp = row.Cells["MODULE_IP"].Value?.ToString();

                    if (string.IsNullOrWhiteSpace(moduleIp))
                        continue;

                    row.Cells["STATUS"].Value = "wait...";
                    dgvVDBStatus.Refresh();

                    var result = await PingHelper.PingWithRetryAsync(moduleIp);

                    SetLinkStatus(row, result.IsSuccess);
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
        }
        #endregion

        private void InitializeClientGrid()
        {
            dgvClientStatus.Columns.Clear();
            dgvClientStatus.ReadOnly = true;
            dgvClientStatus.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvClientStatus.MultiSelect = false;
            dgvClientStatus.RowHeadersVisible = false;

            dgvClientStatus.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ID",
                HeaderText = "ID",
                Width = 80,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            dgvClientStatus.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MACHINE",
                HeaderText = "DEVICES",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                Width = 100,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            dgvClientStatus.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "IP",
                HeaderText = "IP ADDRESS",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Width = 230,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            dgvClientStatus.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "STATUS",
                HeaderText = "LINK STATUS",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Width = 220,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });


            dgvClientStatus.SelectionChanged += (s, e) => dgvClientStatus.ClearSelection();
        }

        private async void BindClientGrid()
        {
            InitializeClientGrid();

            string peerIp = GlobalData._lstAdminSetting?.Ismaster == 1
                ? "10.0.0.253"   // MASTER -> ping SLAVE
                : "10.0.0.254";  // SLAVE -> ping MASTER

            int peerRow = dgvClientStatus.Rows.Add(
                1,
                "Peer Client",
                peerIp,
                "wait..."
            );
            dgvClientStatus.Rows.Add(2, "Edge NMS", EdgeNmsIp, "wait...");


            await CheckClientStatusAsync();
        }

        private async Task CheckClientStatusAsync()
        {
            try
            {
                for (int i = 0; i < dgvClientStatus.Rows.Count; i++)
                {
                    var row = dgvClientStatus.Rows[i];
                    string ip = row.Cells["IP"].Value?.ToString();

                    if (string.IsNullOrWhiteSpace(ip))
                        continue;

                    row.Cells["STATUS"].Value = "wait...";
                    dgvClientStatus.Refresh();

                    var result = await PingHelper.PingWithRetryAsync(ip);
                    SetLinkStatus(row, result.IsSuccess);
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
        }

        private void dgvPdcStatus_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void RefreshData()
        {
            if (tabLinkStatus != null && tabLinkStatus.SelectedTab != null)
            {
                if (tabLinkStatus.SelectedTab.Name == "TAB_HUB")
                    BindPDCGrid();
                else if (tabLinkStatus.SelectedTab.Name == "TAB_TADB")
                    BindTADBGrid();
                else if (tabLinkStatus.SelectedTab.Name == "TAB_CGDB")
                    BindCGDBGrid();
                else if (tabLinkStatus.SelectedTab.Name == "TAB_VDB")
                    BindVDBGrid();
                else if (tabLinkStatus.SelectedTab.Name == "TAB_CLIENT")
                    BindClientGrid();
                else
                    BindPDCGrid();
            }
            else
            {
                BindPDCGrid();
            }
        }


        private void SetLinkStatus(DataGridViewRow row, bool isOk)
        {
            var cell = row.Cells["STATUS"];

            cell.Value = "●";
            cell.Style.ForeColor = isOk ? Color.Green : Color.Red;
            cell.Style.SelectionForeColor = cell.Style.ForeColor;
            cell.Style.Font = DotFont;
            cell.ToolTipText = isOk ? "OK" : "NOT OK";
        }
    }
}