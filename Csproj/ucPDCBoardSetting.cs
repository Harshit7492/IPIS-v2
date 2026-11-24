using DataBaseHandler;
using DataModel;
using Global_Data;
using Helper;
using LogManager;
using System.Data;

namespace IPIS
{
    public partial class ucPDCBoardSetting : UserControl
    {
        public event EventHandler<eventInfo> OnEvent;
        private int editid = 0;
        private int _IsHub = 1;
        private List<Plateforms> lstPlateforms = new List<Plateforms>();
        public ucPDCBoardSetting()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
            this.SuspendLayout();

            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel1.ResumeLayout();
            this.ResumeLayout();
        }
        private IEnumerable<Control> GetAllControls(Control c)
        {
            foreach (Control child in c.Controls)
            {
                foreach (var gc in GetAllControls(child))
                    yield return gc;

                yield return child;
            }
        }
        private void ucBoardSettingPDC_Load(object sender, EventArgs e)
        {
            LoadHubConfig();
            this.BeginInvoke(new Action(() =>
            {
                txtIP.Focus();
                foreach (var combo in GetAllControls(this).OfType<ComboBox>())
                {
                    combo.DropDownStyle = ComboBoxStyle.DropDownList;
                }
            }));
        }
        private void LoadHubConfig()
        {
            try
            {
                lstPlateforms = _dbService.GET_PLATEFORMS(0);
                var filteredPlatforms = lstPlateforms.OrderBy(x => x.PF_ID).Skip(1)
                                        .Take(GlobalData._lstAdminSetting.TOTAL_PLATFORM_DISPLAYED)
                                        .ToList();
                cmbPfNo.DataSource = filteredPlatforms;
                cmbPfNo.DisplayMember = "PF_NUMBER";
                cmbPfNo.ValueMember = "PF_ID";
                cmbPfNo.SelectedIndex = 0;

                List<HUBCONFIG> _lsthubList = _dbService.Get_Hub_Config(Appdata.currentnodeid);
                var displayList = new List<HUBCONFIG>();
                foreach (var item in _lsthubList)
                {
                    displayList.Add(new HUBCONFIG
                    {
                        ID = item.ID,
                        IsHub = 1,
                        HUB_IP = item.HUB_IP,
                        PF_NUMBER = item.PF_NUMBER,
                        NODE_NAME = item.NODE_NAME,
                        STATUS_TEXT = item.STATUS == 1 ? "Active" : "Inactive"
                    });
                    foreach (var itemchild in item.RelatedBoardsInfo)
                    {
                        var platform = filteredPlatforms.FirstOrDefault(x => x.PF_ID == Convert.ToInt32(itemchild.PF_NO));
                        displayList.Add(new HUBCONFIG
                        {
                            ID = itemchild.BOARD_ID,
                            IsHub = 0,
                            HUB_IP = itemchild.BOARD_IP,
                            PF_NUMBER = platform != null ? platform.PF_NUMBER : Convert.ToString(item.PF_ID),
                            NODE_NAME = itemchild.BOARD_NAME,
                            STATUS_TEXT = itemchild.IsActive == 1 ? "Active" : "Inactive"
                        });
                    }
                }

                if (dataGridPDC.Columns.Count > 0)
                {
                    dataGridPDC.DataSource = displayList;
                    if (dataGridPDC.Columns.Contains("ID"))
                        dataGridPDC.Columns["ID"].Visible = false;
                    if (dataGridPDC.Columns.Contains("IsHub"))
                        dataGridPDC.Columns["IsHub"].Visible = false;
                    return;
                }
                dataGridPDC.Columns.Clear();
                dataGridPDC.AutoGenerateColumns = false;
                dataGridPDC.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridPDC.ReadOnly = false;
                dataGridPDC.CellContentClick += dataGridPDC_CellContentClick;
                var idColumn = new DataGridViewTextBoxColumn
                {
                    Name = "ID",
                    HeaderText = "ID",
                    DataPropertyName = "ID",
                    Visible = false
                };
                dataGridPDC.Columns.Add(idColumn);

                var IsHubColumn = new DataGridViewTextBoxColumn
                {
                    Name = "IsHub",
                    HeaderText = "IsHub",
                    DataPropertyName = "IsHub",
                    Visible = false
                };
                dataGridPDC.Columns.Add(IsHubColumn);

                dataGridPDC.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "NODE_NAME",
                    HeaderText = "NODE_NAME",
                    ReadOnly = true,
                    DataPropertyName = "NODE_NAME",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                });

                dataGridPDC.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "HUB_IP",
                    HeaderText = "HUB IP",
                    ReadOnly = true,
                    DataPropertyName = "HUB_IP",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                });

                dataGridPDC.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "PF_NUMBER",
                    HeaderText = "PF",
                    ReadOnly = true,
                    DataPropertyName = "PF_NUMBER",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                });

                dataGridPDC.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "STATUS_TEXT",
                    HeaderText = "STATUS",
                    ReadOnly = true,
                    DataPropertyName = "STATUS_TEXT",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                });
                DataGridViewImageColumn editColumn = new DataGridViewImageColumn
                {
                    HeaderText = "",
                    Name = "edit",
                    ImageLayout = DataGridViewImageCellLayout.Zoom,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                };

                string imagePath = AppHelper.GetImagePath("edit.png");
                editColumn.Image = Image.FromFile(imagePath);
                dataGridPDC.Columns.Add(editColumn);

                DataGridViewImageColumn deleteColumn = new DataGridViewImageColumn
                {
                    HeaderText = "",
                    Name = "Delete",
                    ImageLayout = DataGridViewImageCellLayout.Zoom,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                };

                imagePath = AppHelper.GetImagePath("delete-button.png");
                deleteColumn.Image = Image.FromFile(imagePath);
                dataGridPDC.Columns.Add(deleteColumn);

                dataGridPDC.DataSource = displayList;
                dataGridPDC.Dock = DockStyle.Fill;


                if (_lsthubList.Count > 0)
                {
                    //txtHub_Id.Text = _lsthubList.FirstOrDefault().HUB_ID.ToString();
                    //txtIP.Text = _lsthubList.FirstOrDefault().HUB_IP;
                    //cmbPfNo.Text = _lsthubList.FirstOrDefault().PF_NUMBER;
                    //this.AssignedPort = _lsthubList.FirstOrDefault().ASSIGNED_PORT;
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
        }
        private void dataGridPDC_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;

                string columnName = dataGridPDC.Columns[e.ColumnIndex].Name;
                editid = Convert.ToInt32(dataGridPDC.Rows[e.RowIndex].Cells["ID"].Value);
                if (columnName == "edit")
                {
                    string nodeName = dataGridPDC.Rows[e.RowIndex].Cells["NODE_NAME"].Value?.ToString();
                    string ip = dataGridPDC.Rows[e.RowIndex].Cells["HUB_IP"].Value?.ToString();
                    string pf = dataGridPDC.Rows[e.RowIndex].Cells["PF_NUMBER"].Value?.ToString();
                    cmbPfNo.SelectedValue = lstPlateforms.Where(x => x.PF_NUMBER == pf).FirstOrDefault().PF_ID;
                    _IsHub = Convert.ToInt32(dataGridPDC.Rows[e.RowIndex].Cells["IsHub"].Value?.ToString());
                    txtIP.Text = ip;

                }
                else if (columnName == "Delete")
                {

                    string nodeName = dataGridPDC.Rows[e.RowIndex].Cells["NODE_NAME"].Value?.ToString();
                    string hubIp = dataGridPDC.Rows[e.RowIndex].Cells["HUB_IP"].Value?.ToString();
                    int IsHub = Convert.ToInt32(dataGridPDC.Rows[e.RowIndex].Cells["IsHub"].Value?.ToString());
                    DialogResult result = MessageBox.Show($"Are you sure you want to delete?",
                        "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                   
                        List<BoardSettingInfo> boardsOfRow = GetBoardsOfGridRow(IsHub, editid);

                        int queryresult = _dbService.USP_DELETE_BOARD(IsHub, editid);

                        RemoveCoachConfigurationOfDeletedBoards(boardsOfRow);
                        MessageBox.Show($"Deleted successfully!!", "vardaan electronics industries.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadHubConfig();
                        OnEvent?.Invoke(this, new eventInfo("HubConfigSaved", queryresult.ToString(), this.Name, true));
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
        }

        private static List<BoardSettingInfo> GetBoardsOfGridRow(int isHub, int id)
        {
            try
            {
                if (isHub == 0)
                {
                    return (_dbService.GET_BOARD_SETTING(0) ?? new List<BoardSettingInfo>())
                        .Where(b => b.BOARD_ID == id)
                        .ToList();
                }

                HUBCONFIG hub = _dbService.Get_Hub_Config(Appdata.currentnodeid)?.FirstOrDefault(h => h.ID == id);
                return hub?.RelatedBoardsInfo?.ToList() ?? new List<BoardSettingInfo>();
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
                return new List<BoardSettingInfo>();
            }
        }

 
        private static void RemoveCoachConfigurationOfDeletedBoards(List<BoardSettingInfo> boards)
        {
            if (boards == null || boards.Count == 0) return;

            try
            {
                var remainingIds = new HashSet<int>(
                    (_dbService.GET_BOARD_SETTING(0) ?? new List<BoardSettingInfo>()).Select(b => b.BOARD_ID));

                foreach (BoardSettingInfo board in boards)
                {
                    if (board == null || board.BOARD_ID <= 0 || remainingIds.Contains(board.BOARD_ID))
                        continue;   // not deleted -> keep its coaches

                    bool isCgdb = !string.IsNullOrEmpty(board.BOARD_NAME) &&
                                  (board.BOARD_NAME.IndexOf("CGDB", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                   board.BOARD_NAME.IndexOf("COACH", StringComparison.OrdinalIgnoreCase) >= 0);

                    int? savedPf = int.TryParse(board.PF_NO?.Trim(), out int pf) ? pf : (int?)null;

                    LoggerService.Logger.Information(
                        "PDC delete: board {BoardId} '{BoardName}' (PF '{Pf}', CGDB {IsCgdb}) deleted - removing its coach configuration.",
                        board.BOARD_ID, board.BOARD_NAME, board.PF_NO, isCgdb);

                    // Old rows without BOARD_ID are only cleaned for a CGDB board (by the PF it saved them with).
                    _dbService.DeleteCgdbCoachConfiguration(savedPf ?? 0, isCgdb ? savedPf : null, board.BOARD_ID);
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
        }

        private void btnSavePDC_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbPfNo.SelectedItem != null)
                {
                    if (cmbPfNo.SelectedItem.ToString() == "Select")
                    {
                        MessageBox.Show($"Please select PF Number!!", "vardaan electronics industries.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cmbPfNo.Focus();
                        return;
                    }
                    else if (string.IsNullOrWhiteSpace(txtIP.Text))
                    {
                        MessageBox.Show($"Please enter HUB IP!!", "vardaan electronics industries.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtIP.Focus();
                        return;
                    }

                    HUBCONFIG hubConfig = new HUBCONFIG
                    {
                        ID = editid,
                        IsHub = _IsHub,
                        PF_ID = Convert.ToInt32(cmbPfNo.Text),
                        HUB_IP = txtIP.Text,
                        NODE_NAME = Appdata.currentnodename,
                        STATUS = 1,
                        HUB_ID = Appdata.currentnodeid
    ,
                    };
                    string result = _dbService.Add_Update_HubConfig(hubConfig);
                    editid = 0;
                    _IsHub = 1;
                    if (result.ToLower().Contains("success"))
                    {
                        LoadHubConfig();
                        cmbPfNo.SelectedIndex = 0;
                        txtIP.Text = string.Empty;
                        OnEvent?.Invoke(this, new eventInfo("HubConfigSaved", result, this.Name, true));
                        MessageBox.Show($"{result}", "vardaan electronics industries.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(result))
                            result = "Failed to save configuration!!";
                        MessageBox.Show(result, "vardaan electronics industries.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {

        }
    }
}

