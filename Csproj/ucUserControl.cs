using DataBaseHandler;
using DataModel;
using Helper;
using LogManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace IPIS
{
    public partial class ucUserControl : UserControl
    {
        int editid = 0;
        public ucUserControl()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
        }
        private void ucUserControl_Load(object sender, EventArgs e)
        {
            LoadUsers();
            LoadAccessRights();

            dgvUsers.CellFormatting += dgvUsers_CellFormatting;
            dgvAccessRights.CellFormatting += dgvAccessRights_CellFormatting;
            dgvUsers.CellClick += dgvUsers_CellClick;
            dgvAccessRights.CellClick += dgvAccessRights_CellClick;

            tabUserControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabUserControl.DrawItem += tabUserControl_DrawItem;
        }
        private void LoadUsers()
        {

            dgvUsers.DataSource = _dbService.GET_LOGIN_DETAILS(0);
            cmbAccessType.DataSource = _dbService.GET_ACTIVE_ACCESS_RIGHTS();

            cmbAccessType.DisplayMember = "USER_TYPE";
            cmbAccessType.ValueMember = "ACCESS_ID";
            cmbAccessType.SelectedIndex = -1;
            cmbAccessType.Text = "--Choose--";

            if (dgvUsers.Columns.Contains("Loginid"))
                dgvUsers.Columns["Loginid"].Visible = false;

            if (dgvUsers.Columns.Contains("ACCESS_ID"))
                dgvUsers.Columns["ACCESS_ID"].Visible = false;

            if (!dgvUsers.Columns.Contains("Edit"))
            {
                DataGridViewButtonColumn editColumn = new DataGridViewButtonColumn();
                editColumn.Name = "Edit";
                editColumn.HeaderText = "Edit";
                editColumn.Text = "Edit";
                editColumn.UseColumnTextForButtonValue = true;
                editColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                dgvUsers.Columns.Add(editColumn);
            }
            if (!dgvUsers.Columns.Contains("Del"))
            {
                DataGridViewButtonColumn btndel = new DataGridViewButtonColumn();
                btndel.Name = "Del";
                btndel.HeaderText = "Del";
                btndel.Text = "Del";
                btndel.UseColumnTextForButtonValue = true;
                btndel.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgvUsers.Columns.Add(btndel);
            }
        }
        private void btnSaveUser_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUserName.Text))
            {
                MessageBox.Show("Please enter username.", "VARDAAN ELECTRONICS",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUserName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter password.", "VARDAAN ELECTRONICS",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtRePassword.Text))
            {
                MessageBox.Show("Please confirm password.", "VARDAAN ELECTRONICS",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRePassword.Focus();
                return;
            }

            if (txtPassword.Text != txtRePassword.Text)
            {
                MessageBox.Show("Passwords do not match.", "VARDAAN ELECTRONICS",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtRePassword.Focus();
                return;
            }

            if (cmbAccessType.SelectedValue == null ||
                Convert.ToInt32(cmbAccessType.SelectedValue) == 0)
            {
                MessageBox.Show("Please select user type.", "VARDAAN ELECTRONICS",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbAccessType.Focus();
                return;
            }

            LoginInfo postData = new LoginInfo
            {
                Loginid = editid,
                user_Name = txtUserName.Text.Trim(),
                password = txtPassword.Text,
                ACCESS_ID = Convert.ToInt32(cmbAccessType.SelectedValue),
                IsActive = chkActive.Checked ? 1 : 0
            };

            if (_dbService.ADD_UPDATE_USERS(postData))
            {
                MessageBox.Show(
                    editid > 0
                        ? "User updated successfully."
                        : "User added successfully.",
                    "VARDAAN ELECTRONICS",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadUsers();
                btnClearUser_Click(null, EventArgs.Empty);
            }
            else
            {
                MessageBox.Show(
                    "Unable to save user.",
                    "VARDAAN ELECTRONICS",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        private void dgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (dgvUsers.Columns[e.ColumnIndex].Name == "Edit")
            {
                DataGridViewRow row = dgvUsers.Rows[e.RowIndex];
                editid = Convert.ToInt32(row.Cells["Loginid"].Value);
                if (editid == 1)
                {
                    var userName = row.Cells["user_Name"]?.Value?.ToString() ?? "this user";
                    MessageBox.Show(
                        $"Can't edit {userName}",
                        "Information",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }


                txtUserName.Text = row.Cells["user_Name"].Value.ToString();
                txtPassword.Text = row.Cells["password"].Value.ToString();
                txtRePassword.Text = row.Cells["password"].Value.ToString();
                cmbAccessType.SelectedValue = row.Cells["ACCESS_ID"].Value;

                chkActive.Checked = row.Cells["IsActive"].Value.ToString() == "1";
            }
            if (dgvUsers.Columns[e.ColumnIndex].Name == "Del")
            {
                DataGridViewRow row = dgvUsers.Rows[e.RowIndex];
                int deleteid = Convert.ToInt32(row.Cells["Loginid"].Value);
                if (deleteid == 1)
                {
                    var userName = row.Cells["user_Name"]?.Value?.ToString() ?? "this user";
                    MessageBox.Show(
                       $"Can't delete {userName}", "Information",
                       MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                var confirm = MessageBox.Show(
                       $"Are you sure you want to delete?",
                       "Confirm Delete",
                       MessageBoxButtons.YesNo,
                       MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    string query = "DELETE FROM LOGINUSER WHERE Loginid='" + deleteid + "'";
                    string result = _dbService.ExecuteScalar(query);
                    LoadUsers();
                }
            }
        }
        private void LoadAccessRights()
        {
            dgvAccessRights.DataSource = _dbService.GET_ACCESS_RIGHTS(0);

            if (!dgvAccessRights.Columns.Contains("Edit"))
            {
                DataGridViewButtonColumn editColumn = new DataGridViewButtonColumn();
                editColumn.Name = "Edit";
                editColumn.HeaderText = "Edit";
                editColumn.Text = "Edit";
                editColumn.UseColumnTextForButtonValue = true;
                editColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgvAccessRights.Columns.Add(editColumn);
            }

            if (!dgvAccessRights.Columns.Contains("Del"))
            {
                DataGridViewButtonColumn btndel = new DataGridViewButtonColumn();
                btndel.Name = "Del";
                btndel.HeaderText = "Del";
                btndel.Text = "Del";
                btndel.UseColumnTextForButtonValue = true;
                btndel.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgvAccessRights.Columns.Add(btndel);
            }
            int lastIndex = dgvAccessRights.Columns.Count - 1;
            dgvAccessRights.Columns["Edit"].DisplayIndex = lastIndex - 1;
            dgvAccessRights.Columns["Del"].DisplayIndex = lastIndex;
        }
        private void dgvAccessRights_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (dgvAccessRights.Columns[e.ColumnIndex].Name == "Edit")
            {
                DataGridViewRow row = dgvAccessRights.Rows[e.RowIndex];

                editid = Convert.ToInt32(row.Cells["ACCESS_ID"].Value);
                txtUserType.Text = row.Cells["USER_TYPE"].Value.ToString();

                chkTrainEntry.Checked = row.Cells["TRAIN_ENTRY"].Value.ToString() == "1";
                chkVDC.Checked = row.Cells["VDC"].Value.ToString() == "1";
                chkBoardSetting.Checked = row.Cells["HUB_CONFIGURATION"].Value.ToString() == "1";
                chkGeneralMessage.Checked = row.Cells["GENERAL_MESSAGE"].Value.ToString() == "1";
                chkPlayAnnc.Checked = row.Cells["PLAY_ANNOUNCEMENT"].Value.ToString() == "1";
                //chkRedundantPC.Checked = row.Cells["REDUNDANT_PC"].Value.ToString() == "1";
                chkAdminSetting.Checked = row.Cells["ADMIN_SETTINGS"].Value.ToString() == "1";
                //chkCCTVDisplay.Checked = row.Cells["CCTV_DISPLAY"].Value.ToString() == "1";
                chkUserSettings.Checked = row.Cells["USER_SETTING"].Value.ToString() == "1";
                chkTADBCGDBSend.Checked = row.Cells["TADB_CGDB_SEND"].Value.ToString() == "1";
                chkActiveAccess.Checked = row.Cells["IsActive"].Value.ToString() == "1";
            }
            if (dgvAccessRights.Columns[e.ColumnIndex].Name == "Del")
            {
                DataGridViewRow row = dgvAccessRights.Rows[e.RowIndex];
                int deleteid = Convert.ToInt32(row.Cells["ACCESS_ID"].Value);
                var confirm = MessageBox.Show(
                       $"Are you sure you want to delete?",
                       "Confirm Delete",
                       MessageBoxButtons.YesNo,
                       MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    string query = "DELETE FROM ACCESS_RIGHTS WHERE ACCESS_ID='" + deleteid + "'";
                    string result = _dbService.ExecuteScalar(query);
                    LoadAccessRights();
                }
            }
        }
        private void btnClearAccessRight_Click(object sender, EventArgs e)
        {
            txtUserType.Text = string.Empty;
            chkTrainEntry.Checked = false;
            chkVDC.Checked = false;
            chkBoardSetting.Checked = false;
            chkGeneralMessage.Checked = false;
            chkPlayAnnc.Checked = false;
            //chkRedundantPC.Checked = false;
            chkAdminSetting.Checked = false;
            //chkCCTVDisplay.Checked = false;
            chkUserSettings.Checked = false;
            chkActiveAccess.Checked = false;
            chkTADBCGDBSend.Checked = false;
            editid = 0;
        }
        private void btnSaveAccessRight_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtUserType.Text))
                {
                    MessageBox.Show("Input access right!!", "VARDAAN ELECTRONICS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                AccessRight _postdata = new AccessRight();
                _postdata.ACCESS_ID = editid;
                _postdata.USER_TYPE = txtUserType.Text;
                _postdata.TRAIN_ENTRY = chkTrainEntry.Checked ? 1 : 0;
                _postdata.VDC = chkVDC.Checked ? 1 : 0;
                _postdata.HUB_CONFIGURATION = chkBoardSetting.Checked ? 1 : 0;
                _postdata.GENERAL_MESSAGE = chkGeneralMessage.Checked ? 1 : 0;
                _postdata.PLAY_ANNOUNCEMENT = chkPlayAnnc.Checked ? 1 : 0;
                //_postdata.REDUNDANT_PC = chkRedundantPC.Checked ? 1 : 0;
                _postdata.ADMIN_SETTINGS = chkAdminSetting.Checked ? 1 : 0;
                //_postdata.CCTV_DISPLAY = chkCCTVDisplay.Checked ? 1 : 0;
                _postdata.USER_SETTING = chkUserSettings.Checked ? 1 : 0;
                _postdata.TADB_CGDB_SEND = chkTADBCGDBSend.Checked ? 1 : 0;
                _postdata.IsActive = chkActiveAccess.Checked ? 1 : 0;

                if (_dbService.ADD_UPDATE_ACCESS_RIGHTS(_postdata))
                {
                    MessageBox.Show(editid > 0 ? "Access rights updated successfully!" : "Access rights added successfully!",
                                    "VARDAAN ELECTRONICS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnClearAccessRight_Click(null, EventArgs.Empty);
                    LoadAccessRights();
                    editid = 0;
                }
                else
                {
                    MessageBox.Show("Error while saving", "VARDAAN ELECTRONICS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
        }
        private void btnClearUser_Click(object sender, EventArgs e)
        {
            txtUserName.Clear();
            txtPassword.Clear();
            txtRePassword.Clear();
            cmbAccessType.DisplayMember = "USER_TYPE";
            cmbAccessType.ValueMember = "ACCESS_ID";
            cmbAccessType.SelectedIndex = -1;
            cmbAccessType.Text = "--Choose--";
            cmbAccessType.SelectedIndex = -1;
            chkActive.Checked = false;
            editid = 0;
        }
        private void dgvAccessRights_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        private void dgvAccessRights_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value == null) return;
            string colName = dgvAccessRights.Columns[e.ColumnIndex].Name;
            var yesNoCols = new List<string>
                                {
                                    "TRAIN_ENTRY", "VDC", "HUB_CONFIGURATION", "GENERAL_MESSAGE",
                                    "PLAY_ANNOUNCEMENT", "REDUNDANT_PC", "ADMIN_SETTINGS", "CCTV_DISPLAY",
                                    "USER_SETTING", "TADB_CGDB_SEND", "IsActive"
                                };

            if (yesNoCols.Contains(colName))
            {
                if (e.Value.ToString() == "1")
                    e.Value = "Yes";
                else if (e.Value.ToString() == "0")
                    e.Value = "No";

                e.FormattingApplied = true;
            }
        }
        private void dgvUsers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvUsers.Columns[e.ColumnIndex].Name == "IsActive" && e.Value != null)
            {
                int val;
                if (int.TryParse(e.Value.ToString(), out val))
                {
                    e.Value = val == 1 ? "Yes" : "No";
                    e.FormattingApplied = true;
                }
            }
        }

        private void tabUserControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            TabPage tab = tabUserControl.TabPages[e.Index];
            Rectangle rect = tabUserControl.GetTabRect(e.Index);

            bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            Color backColor = selected
                ? Color.FromArgb(0, 102, 204)   
                : Color.Gainsboro;              

            Color textColor = selected
                ? Color.White
                : Color.Black;

            using (SolidBrush brush = new SolidBrush(backColor))
            {
                g.FillRectangle(brush, rect);
            }

            TextRenderer.DrawText(
                g,
                tab.Text,
                tabUserControl.Font,
                rect,
                textColor,
                TextFormatFlags.HorizontalCenter |
                TextFormatFlags.VerticalCenter);

            using (Pen pen = new Pen(Color.Gray))
            {
                g.DrawRectangle(pen, rect);
            }
        }
        private void tabUserControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            tabUserControl.Invalidate();
        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {

        }

        public void RefreshData()
        {
            LoadUsers();
            LoadAccessRights();
        }
    }
}