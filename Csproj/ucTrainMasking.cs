using DataBaseHandler;
using LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace IPISv2
{
    public partial class ucTrainMasking : UserControl
    {
        private DataTable _trainData;
        private string _selectedTrainNo;
        private int _selectedMaskStatus;

        public ucTrainMasking()
        {
            InitializeComponent();
            LoadMaskedTrains();
        }

        private void LoadMaskedTrains()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                // Get data from TRAIN_MASK table
                string query = @"SELECT TRAIN_NO,ISNULL(TRAIN_NAME, '') AS TRAIN_NAME, ISNULL(REMARKS, '') AS REMARKS, MASKED_DATE FROM TRAIN_MASK ORDER BY TRAIN_NO";

                DataTable dt = _dbService.GetTable(query);

                // Create DataTable
                _trainData = new DataTable();

                // Add columns
                _trainData.Columns.Add("TRAIN_NO", typeof(string));
                _trainData.Columns.Add("TRAIN_NAME", typeof(string));
                _trainData.Columns.Add("REMARKS", typeof(string));
                _trainData.Columns.Add("MASKED_DATE", typeof(DateTime));
                _trainData.Columns.Add("MASKED", typeof(int));

                // Parse the result and add rows
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        try
                        {
                            DataRow newRow = _trainData.NewRow();
                            newRow["TRAIN_NO"] = row["TRAIN_NO"] != DBNull.Value ? row["TRAIN_NO"].ToString().Trim() : "";

                            string name = row["TRAIN_NAME"] != DBNull.Value ? row["TRAIN_NAME"].ToString().Trim() : "";
                            newRow["TRAIN_NAME"] = string.IsNullOrEmpty(name) ? "N/A" : name;

                            newRow["REMARKS"] = row["REMARKS"] != DBNull.Value ? row["REMARKS"].ToString().Trim() : "";

                            DateTime maskDate = DateTime.Now;
                            if (row["MASKED_DATE"] != DBNull.Value && !string.IsNullOrEmpty(row["MASKED_DATE"].ToString()))
                            {
                                DateTime.TryParse(row["MASKED_DATE"].ToString(), out maskDate);
                            }
                            newRow["MASKED_DATE"] = maskDate;
                            newRow["MASKED"] = 1;
                            _trainData.Rows.Add(newRow);
                        }
                        catch (Exception ex)
                        {
                            LoggerService.Logger.Warning($"Failed to parse row. Error: {ex.Message}");
                        }
                    }
                }

                dgvTrains.DataSource = _trainData;
                ConfigureGridColumns();
                UpdateButtonStates();
                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                LoggerService.Logger.Error(ex, "Failed to load masked trains");
          
            }
        }

        private void ConfigureGridColumns()
        {
            if (dgvTrains.Columns != null && dgvTrains.Columns.Count > 0)
            {
                if (dgvTrains.Columns["TRAIN_NO"] != null)
                {
                    dgvTrains.Columns["TRAIN_NO"].HeaderText = "Train No";
                    dgvTrains.Columns["TRAIN_NO"].Width = 120;
                }

                if (dgvTrains.Columns["TRAIN_NAME"] != null)
                {
                    dgvTrains.Columns["TRAIN_NAME"].HeaderText = "Train Name";
                    dgvTrains.Columns["TRAIN_NAME"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }

                if (dgvTrains.Columns["REMARKS"] != null)
                {
                    dgvTrains.Columns["REMARKS"].HeaderText = "Remarks";
                    dgvTrains.Columns["REMARKS"].Width = 220;
                }

                if (dgvTrains.Columns["MASKED_DATE"] != null)
                {
                    dgvTrains.Columns["MASKED_DATE"].HeaderText = "Masked Date";
                    dgvTrains.Columns["MASKED_DATE"].Width = 150;
                    dgvTrains.Columns["MASKED_DATE"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                }

                if (dgvTrains.Columns["MASKED"] != null)
                {
                    dgvTrains.Columns["MASKED"].Visible = false;
                }
            }
        }

        private void DgvTrains_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvTrains.Rows[e.RowIndex];
                if (row.DataBoundItem is DataRowView rowView)
                {
                    _selectedTrainNo = rowView["TRAIN_NO"].ToString();
                    _selectedMaskStatus = Convert.ToInt32(rowView["MASKED"]);
                    btnDelete.Enabled = _selectedMaskStatus == 1;
                }
            }
        }

        private void DgvTrains_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvTrains.Rows[e.RowIndex].DataBoundItem is DataRowView rowView)
            {
                dgvTrains.Rows[e.RowIndex].DefaultCellStyle.BackColor = dgvTrains.DefaultCellStyle.BackColor;
                dgvTrains.Rows[e.RowIndex].DefaultCellStyle.ForeColor = dgvTrains.DefaultCellStyle.ForeColor;
            }
        }

        private void BtnAddTrain_Click(object sender, EventArgs e)
        {
            try
            {
                string trainNo = txtTrainNo.Text.Trim();
                string trainName = txtTrainName.Text.Trim();

                // Validate Train No
                if (string.IsNullOrEmpty(trainNo))
                {
                    MessageBox.Show("Please enter a Train Number to mask.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTrainNo.Focus();
                    return;
                }

                if (trainNo.Length != 5 || !trainNo.All(char.IsDigit))
                {
                    MessageBox.Show("Train Number must be exactly 5 digits (e.g. 08549).", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTrainNo.Focus();
                    txtTrainNo.SelectAll();
                    return;
                }

                // Validate Train Name
                if (string.IsNullOrEmpty(trainName))
                {
                    MessageBox.Show("Please enter a Train Name.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTrainName.Focus();
                    return;
                }

                // Check if train already exists in TRAIN_MASK
                string checkQuery = $"SELECT COUNT(*) FROM TRAIN_MASK WHERE TRAIN_NO = '{trainNo.Replace("'", "''")}'";
                List<string> checkResult = _dbService.QueryList(checkQuery);
                int count = 0;
                if (checkResult != null && checkResult.Count > 0)
                {
                    int.TryParse(checkResult[0], out count);
                }

                if (count > 0)
                {
                    MessageBox.Show($"Train '{trainNo}' is already masked.", "Information",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtTrainNo.Clear();
                    txtTrainName.Clear();
                    txtTrainNo.Focus();
                    return;
                }

                var result = MessageBox.Show(
                    $"Are you sure you want to mask Train {trainNo} - {trainName}?\n\n" +
                    "This train will be hidden from public display and announcements.",
                    "Confirm Train Masking",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return;

                Cursor = Cursors.WaitCursor;

                string remarks = string.IsNullOrWhiteSpace(txtRemarks.Text)
                    ? "Manually masked by user"
                    : txtRemarks.Text.Trim();

                // Pass the train name (not the train number) to be saved in TRAIN_MASK
                int rowsAffected = _dbService.SaveMaskTrains(trainNo, trainName, remarks);

                if (rowsAffected > 0)
                {
                    LoggerService.Logger.Information($"Successfully masked train {trainNo} - {trainName}");
                    MessageBox.Show($"Successfully added Train {trainNo} - {trainName} to masked list.", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    txtTrainNo.Clear();
                    txtTrainName.Clear();
                    txtRemarks.Clear();

                    LoadMaskedTrains();
                }
                else
                {
                    MessageBox.Show($"Failed to mask train {trainNo}. Please try again.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                Cursor = Cursors.Default;
                txtTrainNo.Focus();
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                LoggerService.Logger.Error(ex, "Failed to add masked train");
              
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(_selectedTrainNo))
                {
                    MessageBox.Show("Please select a masked train from the grid to delete.", "Information",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (_selectedMaskStatus != 1)
                {
                    MessageBox.Show("Selected train is not masked. Only masked trains can be deleted.", "Information",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var result = MessageBox.Show(
                    $"Are you sure you want to delete Train {_selectedTrainNo} from the masked list?\n\n" +
                    "This train will become active and visible again.",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return;

                Cursor = Cursors.WaitCursor;

                string deleteQuery = $"DELETE FROM TRAIN_MASK WHERE TRAIN_NO = '{_selectedTrainNo.Replace("'", "''")}'";
                _dbService.ExecuteScalar(deleteQuery);

                LoggerService.Logger.Information($"Deleted masked train {_selectedTrainNo}");
                MessageBox.Show($"Successfully deleted Train {_selectedTrainNo} from masked list.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                _selectedTrainNo = null;
                _selectedMaskStatus = 0;

                LoadMaskedTrains();
                btnDelete.Enabled = false;

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                LoggerService.Logger.Error(ex, "Failed to delete masked train");
             
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadMaskedTrains();
        }

        private void UpdateButtonStates()
        {
            btnDelete.Enabled = !string.IsNullOrEmpty(_selectedTrainNo) && _selectedMaskStatus == 1;
        }

        public void RefreshData()
        {
            LoadMaskedTrains();
        }
    }
}