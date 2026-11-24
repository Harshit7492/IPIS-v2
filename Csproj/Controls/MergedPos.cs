using DataBaseHandler;
using DataModel;
using LogManager;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace IPISv2.Controls
{
    public class MergedPos : Form
    {
        private TextBox txtSearch;
        private NumericUpDown numPosition;
        private DataGridView dgvTrains;
        private Button btnOK;
        private Button btnClose;

        private List<TrainMaster> _allTrains = new List<TrainMaster>();
        private readonly string _mergingTrainNumber;

        public MergedPos(string trainNumber)
        {
            _mergingTrainNumber = trainNumber;

            InitializeForm();
            LoadTrainMaster();
        }

        private void InitializeForm()
        {
            Text = "Merge Train Coach Sequence";
            Size = new Size(1000, 600);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.White;

            // ================= HEADER =================

            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 120,
                Padding = new Padding(15)
            };

            Label lblPosition = new Label
            {
                Text = "Merge Position:",
                Location = new Point(15, 15),
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            pnlHeader.Controls.Add(lblPosition);

            numPosition = new NumericUpDown
            {
                Location = new Point(130, 10),
                Width = 120,
                Minimum = 1,
                Maximum = 100,
                Value = 1
            };

            pnlHeader.Controls.Add(numPosition);

            Label lblSearch = new Label
            {
                Text = "Search Train:",
                Location = new Point(15, 52),
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            pnlHeader.Controls.Add(lblSearch);

            txtSearch = new TextBox
            {
                Location = new Point(130, 48),
                Width = 400,
                Font = new Font("Segoe UI", 10)
            };

            txtSearch.TextChanged += TxtSearch_TextChanged;

            pnlHeader.Controls.Add(txtSearch);

            btnOK = new Button
            {
                Text = "Merge",
                Width = 90,
                Height = 32,
                Location = new Point(780, 75),
                Enabled = false,
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            btnOK.FlatAppearance.BorderSize = 0;
            btnOK.Click += BtnOK_Click;

            pnlHeader.Controls.Add(btnOK);

            btnClose = new Button
            {
                Text = "Cancel",
                Width = 90,
                Height = 32,
                Location = new Point(875, 75)
            };

            btnClose.Click += BtnClose_Click;

            pnlHeader.Controls.Add(btnClose);

            Controls.Add(pnlHeader);

            // ================= GRID =================

            dgvTrains = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            // Train Number
            dgvTrains.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TrainNumber",
                HeaderText = "Train No.",
                DataPropertyName = "TRAIN_NUMBER",
                FillWeight = 15
            });

            // English Name
            dgvTrains.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TrainNameEnglish",
                HeaderText = "Train Name",
                DataPropertyName = "TRAIN_NAME_ENGLISH",
                FillWeight = 30
            });

            // Origin
            dgvTrains.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Origin",
                HeaderText = "Origin",
                DataPropertyName = "ORIGIN_CODE",
                FillWeight = 15
            });

            // Destination
            dgvTrains.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Destination",
                HeaderText = "Destination",
                DataPropertyName = "DESTINATION_CODE",
                FillWeight = 15
            });

            // Coach Sequence
            dgvTrains.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CoachSequence",
                HeaderText = "Coach Sequence",
                DataPropertyName = "COACH_SEQUENCE",
                FillWeight = 40
            });

            dgvTrains.SelectionChanged += DgvTrains_SelectionChanged;

            Controls.Add(dgvTrains);

            dgvTrains.BringToFront();

            AcceptButton = btnOK;
            CancelButton = btnClose;
        }

    
        private void LoadTrainMaster()
        {
            try
            {
                _allTrains = _dbService.GET_TRAIN_MASTER();

                // Remove current train because same train cannot be merged
                _allTrains = _allTrains
                    .Where(x =>
                        !string.Equals(
                            x.TRAIN_NUMBER,
                            _mergingTrainNumber,
                            StringComparison.OrdinalIgnoreCase))
                    .ToList();

                dgvTrains.DataSource = null;
                dgvTrains.DataSource = _allTrains;

                if (dgvTrains.Rows.Count > 0)
                {
                    dgvTrains.Rows[0].Selected = true;
                    btnOK.Enabled = true;
                }
                else
                {
                    btnOK.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(
                    ex,
                    "Error loading Train Master.");

                MessageBox.Show(
                    "Unable to load train details.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // =========================================================
        // SEARCH TRAIN
        // =========================================================

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            string search = txtSearch.Text.Trim();

            if (string.IsNullOrWhiteSpace(search))
            {
                dgvTrains.DataSource = null;
                dgvTrains.DataSource = _allTrains;
                return;
            }

            var filtered = _allTrains
                .Where(x =>
                    (!string.IsNullOrEmpty(x.TRAIN_NUMBER) &&
                     x.TRAIN_NUMBER.Contains(
                         search,
                         StringComparison.OrdinalIgnoreCase))

                    ||

                    (!string.IsNullOrEmpty(x.TRAIN_NAME_ENGLISH) &&
                     x.TRAIN_NAME_ENGLISH.Contains(
                         search,
                         StringComparison.OrdinalIgnoreCase))

                    ||

                    (!string.IsNullOrEmpty(x.TRAIN_NAME_HINDI) &&
                     x.TRAIN_NAME_HINDI.Contains(
                         search,
                         StringComparison.OrdinalIgnoreCase))

                    ||

                    (!string.IsNullOrEmpty(x.TRAIN_NAME_REGIONAL) &&
                     x.TRAIN_NAME_REGIONAL.Contains(
                         search,
                         StringComparison.OrdinalIgnoreCase))

                    ||

                    (!string.IsNullOrEmpty(x.ORIGIN_CODE) &&
                     x.ORIGIN_CODE.Contains(
                         search,
                         StringComparison.OrdinalIgnoreCase))

                    ||

                    (!string.IsNullOrEmpty(x.DESTINATION_CODE) &&
                     x.DESTINATION_CODE.Contains(
                         search,
                         StringComparison.OrdinalIgnoreCase))
                )
                .ToList();

            dgvTrains.DataSource = null;
            dgvTrains.DataSource = filtered;

            btnOK.Enabled = dgvTrains.SelectedRows.Count > 0;
        }

        // =========================================================
        // SELECTION
        // =========================================================

        private void DgvTrains_SelectionChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = dgvTrains.SelectedRows.Count > 0;
        }

        // =========================================================
        // MERGE
        // =========================================================

        private async void BtnOK_Click(object sender, EventArgs e)
        {
            if (dgvTrains.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Please select a train.",
                    "Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            var selectedTrain =
                dgvTrains.SelectedRows[0].DataBoundItem as TrainMaster;

            if (selectedTrain == null)
            {
                MessageBox.Show(
                    "Invalid train selected.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            string selectedTrainNumber = selectedTrain.TRAIN_NUMBER;
            int position = (int)numPosition.Value;
            string mergedTrainEnglish = selectedTrain.TRAIN_NAME_ENGLISH ?? string.Empty;
            string mergedTrainHindi = selectedTrain.TRAIN_NAME_HINDI ?? string.Empty;
            string mergedTrainRegional = selectedTrain.TRAIN_NAME_REGIONAL ?? string.Empty;

            if (string.IsNullOrWhiteSpace(selectedTrainNumber))
            {
                MessageBox.Show(
                    "Selected train number is invalid.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            try
            {
                btnOK.Enabled = false;
                btnOK.Text = "Merging...";

                bool success =
                    await _dbService.MergeTrainCoachSequence(
                        _mergingTrainNumber,
                        selectedTrainNumber,
                        position,
                          mergedTrainEnglish,
    mergedTrainHindi,
    mergedTrainRegional

                        );

                if (success)
                {
                    MessageBox.Show(
                        $"Train {selectedTrainNumber} - " +
                        $"{selectedTrain.TRAIN_NAME_ENGLISH}\n\n" +
                        $"merged successfully at position {position}.",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show(
                        "Unable to merge coach sequence.\n" +
                        "Please check the train and merge position.",
                        "Merge Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(
                    ex,
                    "Error merging train coach sequence.");

                MessageBox.Show(
                    $"Error: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnOK.Enabled = true;
                btnOK.Text = "Merge";
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}