using DataBaseHandler;
using DataModel;
using Global_Data;
using IPISv2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPIS
{
    public partial class ucSetUpStationCode : UserControl
    {
        private Form keyboardForm;
        private ucKeyboard keyboardControl;
        public ucSetUpStationCode()
        {
            InitializeComponent();
        }
        private void ucSetUpStationCode_Load(object sender, EventArgs e)
        {
            dgvStationLegend.DataSource = _dbService.GET_ALL_STATIONS();
            dgvStationLegend.AutoGenerateColumns = false;
            dgvStationLegend.Columns.Clear();
            

            dgvStationLegend.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "STATION_CODE",
                DataPropertyName = "STATION_CODE",
                HeaderText = "CODE"
            });
            dgvStationLegend.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "STATION_NAME_ENGLISH",
                DataPropertyName = "STATION_NAME_ENGLISH",
                HeaderText = "ENGLISH"
            });
            dgvStationLegend.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "STATION_NAME_HINDI",
                DataPropertyName = "STATION_NAME_HINDI",
                HeaderText = "HINDI"
            });
            dgvStationLegend.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "STATION_NAME_REGIONAL",
                DataPropertyName = "STATION_NAME_REGIONAL",
                HeaderText = "REGIONAL"
            });

            if (!dgvStationLegend.Columns.Contains("Edit"))
            {
                var editColumn = new DataGridViewButtonColumn
                {
                    Name = "Edit",
                    HeaderText = "Edit",
                    Text = "✏️",
                    UseColumnTextForButtonValue = true,
                    DefaultCellStyle = new DataGridViewCellStyle
                    {
                        BackColor = Color.LightBlue,
                        ForeColor = Color.Black,
                        Alignment = DataGridViewContentAlignment.MiddleCenter
                    },
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
                };

                dgvStationLegend.Columns.Add(editColumn);
            }
            if (!dgvStationLegend.Columns.Contains("Delete"))
            {
                var deleteColumn = new DataGridViewButtonColumn
                {
                    Name = "colDelete",
                    HeaderText = "Delete",
                    Text = "🗑️",
                    UseColumnTextForButtonValue = true,
                    DefaultCellStyle = new DataGridViewCellStyle
                    {
                        BackColor = Color.WhiteSmoke,
                        ForeColor = Color.Red,
                        Alignment = DataGridViewContentAlignment.MiddleCenter
                    },
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
                };
                dgvStationLegend.Columns.Add(deleteColumn);

            }
            dgvStationLegend.CellClick -= dgvStationLegend_CellClick;
            dgvStationLegend.CellClick += dgvStationLegend_CellClick;
        }
        private void dgvStationLegend_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvStationLegend.Columns[e.ColumnIndex].Name == "colDelete")
            {
                DataGridViewColumn stationCol = dgvStationLegend.Columns
                    .Cast<DataGridViewColumn>()
                    .FirstOrDefault(c => c.Name.Equals("STATION_CODE", StringComparison.OrdinalIgnoreCase));

                if (stationCol == null)
                {
                    MessageBox.Show("Station Code column not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                object cellValue = dgvStationLegend.Rows[e.RowIndex].Cells[stationCol.Index].Value;

                if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                {
                    MessageBox.Show("Station code is empty for this row.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string stationCode = cellValue.ToString();

                if (MessageBox.Show($"Are you sure you want to delete station {stationCode}?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    string query = "DELETE FROM STATION WHERE STATION_CODE='" + stationCode + "'";
                    string result = _dbService.ExecuteScalar(query);
                    MessageBox.Show("Station deleted successfully!", "VARDAAN ELECTRONICS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvStationLegend.DataSource = _dbService.GET_ALL_STATIONS();

                }
            }
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (dgvStationLegend.Columns[e.ColumnIndex].Name == "Edit")
            {
                var row = dgvStationLegend.Rows[e.RowIndex];
                var item = row.DataBoundItem as StationList;
                if (item != null)
                {
                    txtboxCode.Text = item.STATION_CODE;
                    txtboxNameEnglish.Text = item.STATION_NAME_ENGLISH;
                    txtboxNameHindi.Text = item.STATION_NAME_HINDI;
                    txtboxNameRegional.Text = item.STATION_NAME_REGIONAL;
                }
            }
        }
        private void btnSaveStationDetails_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtboxCode.Text) || string.IsNullOrWhiteSpace(txtboxNameEnglish.Text))
            {
                MessageBox.Show("Station Code and Station Name cannot be empty!",
                                "VARDAAN ELECTRONICS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            StationList _postdata = new StationList
            {
                STATION_CODE = txtboxCode.Text.Trim(),
                STATION_NAME_ENGLISH = txtboxNameEnglish.Text.Trim(),
                STATION_NAME_HINDI = txtboxNameHindi.Text.Trim(),
                STATION_NAME_REGIONAL = txtboxNameRegional.Text.Trim()
            };

            if (_dbService.ADD_UPDATE_STATION_CODE(_postdata))
            {
                MessageBox.Show("Station saved successfully!", "VARDAAN ELECTRONICS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvStationLegend.DataSource = _dbService.GET_ALL_STATIONS();
                txtboxCode.Clear();
                txtboxNameEnglish.Clear();
                txtboxNameHindi.Clear();
                txtboxNameRegional.Clear();
            }
            else
                MessageBox.Show("Error while saving!!", "VARDAAN ELECTRONICS", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void btnClearDtationDetails_Click_1(object sender, EventArgs e)
        {
            txtboxCode.Text = string.Empty;
            txtboxNameEnglish.Text = string.Empty;
            txtboxNameHindi.Text = string.Empty;
            txtboxStationCodeName.Text = string.Empty;
            txtboxNameRegional.Text = string.Empty;
            dgvStationLegend.ClearSelection();
        }

        private void txtboxStationCodeName_TextChanged(object sender, EventArgs e)
        {
            string searchValue = txtboxStationCodeName.Text.Trim();

            dgvStationLegend.ClearSelection();

            if (string.IsNullOrWhiteSpace(searchValue))
                return;

            foreach (DataGridViewRow row in dgvStationLegend.Rows)
            {
                if (row.IsNewRow)
                    continue;

                string code = row.Cells["STATION_CODE"].Value?.ToString() ?? "";
                string english = row.Cells["STATION_NAME_ENGLISH"].Value?.ToString() ?? "";
                string hindi = row.Cells["STATION_NAME_HINDI"].Value?.ToString() ?? "";
                string regional = row.Cells["STATION_NAME_REGIONAL"].Value?.ToString() ?? "";

                bool isMatch =
                    code.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                    english.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                    hindi.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                    regional.Contains(searchValue, StringComparison.OrdinalIgnoreCase);

                if (isMatch)
                {
                    row.Selected = true;

                    dgvStationLegend.CurrentCell =
                        row.Cells["STATION_CODE"];

                    dgvStationLegend.FirstDisplayedScrollingRowIndex =
                        row.Index;

                    return;
                }
            }
        }

        private ucKeyboard.KeyboardLanguage? GetRegionalKeyboardLanguage()
        {
            if (GlobalData._lstAdminSetting == null)
                return null;

            string regionalLanguage = GlobalData._lstAdminSetting.STATION_LANGUAGES
                                        .Split('+')
                                        .LastOrDefault()?
                                        .Trim();


            if (string.IsNullOrEmpty(regionalLanguage))
                return null;

            if (Enum.TryParse(regionalLanguage, true, out ucKeyboard.KeyboardLanguage lang))
                return lang;

            return null;
        }
        private void TxtboxNameHindi_Click(object sender, EventArgs e)
        {
            ucKeyboard.KeyboardLanguage language = ucKeyboard.KeyboardLanguage.Hindi;

            if (keyboardForm != null && !keyboardForm.IsDisposed && keyboardForm.Visible)
            {
                keyboardForm.Close();
                return;
            }

            if (keyboardForm == null || keyboardForm.IsDisposed)
            {
                keyboardForm = new Form();
                keyboardControl = new ucKeyboard();

                keyboardForm.TopMost = true;
                keyboardForm.FormBorderStyle = FormBorderStyle.FixedSingle;
                keyboardForm.MaximizeBox = false;
                keyboardForm.MinimizeBox = false;
                keyboardForm.StartPosition = FormStartPosition.Manual;
                keyboardForm.BackColor = Color.FromArgb(135, 206, 250);

                keyboardControl.Dock = DockStyle.Fill;
                keyboardForm.Controls.Add(keyboardControl);

                keyboardForm.Size = new Size(800, 320);
                keyboardForm.Text = language + " Keyboard";
            }

            keyboardControl.TargetTextBox = txtboxNameHindi;
            keyboardControl.BuildKeyboard(language);

            Point p = txtboxNameHindi.PointToScreen(Point.Empty);

            keyboardForm.Location = new Point(
                p.X,
                p.Y
            );

            keyboardForm.Show();
            keyboardForm.BringToFront();
        }
        private void TxtboxNameRegional_Click(object sender,EventArgs e)
        {
            var regionalLanguage = GetRegionalKeyboardLanguage();
            ShowKeyboard(
                txtboxNameRegional,
                regionalLanguage.Value);
        }
        private void ShowKeyboard(TextBox targetTextBox, ucKeyboard.KeyboardLanguage language)
        {
            if (keyboardForm != null && !keyboardForm.IsDisposed && keyboardForm.Visible)
            {
                keyboardForm.Close();
                return;
            }


            if (keyboardForm == null || keyboardForm.IsDisposed)
            {
                keyboardForm = new Form();
                keyboardControl = new ucKeyboard();
                keyboardForm.TopMost = true;
                keyboardForm.FormBorderStyle = FormBorderStyle.FixedSingle;
                keyboardForm.MaximizeBox = false;
                keyboardForm.MinimizeBox = false;
                keyboardForm.StartPosition = FormStartPosition.Manual;
                keyboardForm.BackColor = Color.FromArgb(135, 206, 250);

                keyboardControl.Dock = DockStyle.Fill;
                keyboardForm.Controls.Add(keyboardControl);

                keyboardForm.Size = new Size(800, 320);
                keyboardForm.Text = language.ToString() + " Keyboard";
            }

            keyboardControl.TargetTextBox = targetTextBox;

            keyboardControl.BuildKeyboard(language);

            // position near textbox
            Point p = targetTextBox.PointToScreen(Point.Empty);

            keyboardForm.Location = new Point(
                p.X,
                p.Y 
            );

            keyboardForm.Show();
            keyboardForm.BringToFront();
        }

        public void RefreshData()
        {
            dgvStationLegend.DataSource = _dbService.GET_ALL_STATIONS();
        }
    }
}
