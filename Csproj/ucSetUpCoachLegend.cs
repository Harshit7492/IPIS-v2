using DataBaseHandler;
using DataModel;
using IPISv2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPIS
{
    public partial class ucSetUpCoachLegend : UserControl
    {
        int editid = 0;
        private Form keyboardForm;
        private ucKeyboard keyboardControl;

        public ucSetUpCoachLegend()
        {
            InitializeComponent();
        }
        private void ucSetUpCoachLegend_Load(object sender, EventArgs e)
        {
            dgvCoachLegend.DataSource = _dbService.GET_LEGEND_LIST();
            dgvCoachLegend.AutoGenerateColumns = false;

            dgvCoachLegend.Columns.Clear();
            dgvCoachLegend.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "LEGEND_CODE",
                HeaderText = "CODE",
                DataPropertyName = "LEGEND_CODE",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            dgvCoachLegend.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "LEGEND",
                HeaderText = "ENGLISH",
                DataPropertyName = "LEGEND",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            dgvCoachLegend.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "LEGEND_HINDI",
                HeaderText = "HINDI",
                DataPropertyName = "LEGEND_HINDI",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            if (dgvCoachLegend.Columns.Contains("SR_NO"))
                dgvCoachLegend.Columns["SR_NO"].Visible = false;

            if (!dgvCoachLegend.Columns.Contains("Edit"))
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
                dgvCoachLegend.Columns.Add(editColumn);
            }
            if (!dgvCoachLegend.Columns.Contains("Delete"))
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
                dgvCoachLegend.Columns.Add(deleteColumn);
            }
            dgvCoachLegend.CellClick -= dgvCoachLegend_CellClick;
            dgvCoachLegend.CellClick += dgvCoachLegend_CellClick;
            txtboxNameHindi.Click += TxtboxNameHindi_Click ;
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

        private void dgvCoachLegend_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvCoachLegend.Rows[e.RowIndex].IsNewRow)
                return;

            if (dgvCoachLegend.Columns[e.ColumnIndex].Name == "colDelete")
            {
                var item = dgvCoachLegend.Rows[e.RowIndex].DataBoundItem as Legend;
                if (item != null)
                    editid = item.SR_NO;
                if (MessageBox.Show($"Are you sure you want to delete Coach Legend?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    string query = "DELETE FROM LEGENDLIST WHERE SR_NO=" + editid;
                    string result = _dbService.ExecuteScalar(query);
                    MessageBox.Show("Coach Legend deleted successfully!", "VARDAAN ELECTRONICS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvCoachLegend.DataSource = _dbService.GET_LEGEND_LIST();
                    txtboxCodeName.Text = string.Empty;
                }
            }

            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (dgvCoachLegend.Columns[e.ColumnIndex].Name == "Edit")
            {
                var item = dgvCoachLegend.Rows[e.RowIndex].DataBoundItem as Legend;
                if (item != null)
                {
                    editid = item.SR_NO;
                    txtboxCode.Text = item.LEGEND_CODE;
                    txtboxNameEnglish.Text = item.LEGEND;
                    txtboxNameHindi.Text = item.LEGEND_HINDI;
                }
            }
        }
        private void btnClearLegend_Click(object sender, EventArgs e)
        {
            txtboxCode.Text = string.Empty;
            txtboxNameEnglish.Text = string.Empty;
            txtboxNameHindi.Text = string.Empty;
            txtboxCodeName.Text = string.Empty;
            dgvCoachLegend.ClearSelection();
        }
        private void btnSaveLegend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtboxCode.Text) || string.IsNullOrWhiteSpace(txtboxNameEnglish.Text))
            {
                MessageBox.Show("Coach Code and Coach Name cannot be empty!", "VARDAAN ELECTRONICS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LegendList _postdata = new LegendList();
            _postdata.SR_NO = (short)editid;
            _postdata.LEGEND_CODE = txtboxCode.Text.Trim();
            _postdata.LEGEND = txtboxNameEnglish.Text.Trim();
            _postdata.LEGEND_HINDI = txtboxNameHindi.Text.Trim();

            bool res = _dbService.ADD_UPDATE_COACH_LEGEND(_postdata);

            if (res)
            {
                MessageBox.Show(
                    "Coach legend has been saved successfully.",
                    "VARDAAN ELECTRONICS",
                    MessageBoxButtons.OK,
                  MessageBoxIcon.Information);
                dgvCoachLegend.DataSource = _dbService.GET_LEGEND_LIST(editid);
                txtboxCode.Clear();
                txtboxNameEnglish.Clear();
                txtboxNameHindi.Clear();
            }
            else
            {
                MessageBox.Show(
                       "The coach name you entered already exists. Please enter a different coach name.",
                       "Duplicate Coach Name",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Warning);
                txtboxCode.Clear();
                txtboxNameEnglish.Clear();
                txtboxNameHindi.Clear();
                txtboxNameEnglish.Focus();
            }
            editid = 0;

        }

        private void txtboxCodeName_TextChanged(object sender, EventArgs e)
        {
            string searchValue = txtboxCodeName.Text.Trim();

            // Clear previous selection
            dgvCoachLegend.ClearSelection();

            // Empty search: remove selection and exit
            if (string.IsNullOrWhiteSpace(searchValue))
                return;

            foreach (DataGridViewRow row in dgvCoachLegend.Rows)
            {
                if (row.IsNewRow)
                    continue;

                string codeVal = row.Cells["LEGEND_CODE"].Value?.ToString() ?? "";
                string englishVal = row.Cells["LEGEND"].Value?.ToString() ?? "";
                string hindiVal = row.Cells["LEGEND_HINDI"].Value?.ToString() ?? "";

                // Partial search in code, English name and Hindi name
                bool isMatch =
                    codeVal.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                    englishVal.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                    hindiVal.Contains(searchValue, StringComparison.OrdinalIgnoreCase);

                if (isMatch)
                {
                    row.Selected = true;

                    dgvCoachLegend.CurrentCell = row.Cells["LEGEND_CODE"];

                    if (row.Index >= 0)
                    {
                        dgvCoachLegend.FirstDisplayedScrollingRowIndex = row.Index;
                    }

                    return; // First matching row
                }
            }
        }


        public void RefreshData()
        {
            dgvCoachLegend.DataSource = _dbService.GET_LEGEND_LIST();
        }
    }
}
