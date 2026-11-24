using DataBaseHandler;
using DataModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace IPISv2.Controls
{
    public class StationListDialog : Form
    {
        private TextBox txtSearch;
        private DataGridView dgvStations;
        private Button btnOK;
        private Button btnClose;
        private List<StationList> _allStations;
        private string _trainNumber;
        private TextBox _targetTextBox;
        private bool _isOnlineTrainMode;

        public string SelectedStationCode { get; private set; }
        public string SelectedStationName { get; private set; }
        public StationList SelectedStation { get; private set; }

        public StationListDialog(List<StationList> stations, string trainNumber = null, TextBox targetTextBox = null, bool isOnlineTrainMode = false)
        {
            _allStations = stations ?? new List<StationList>();
            _trainNumber = trainNumber;
            _targetTextBox = targetTextBox;
            _isOnlineTrainMode = isOnlineTrainMode || (!string.IsNullOrEmpty(trainNumber) && targetTextBox == null);
            SelectedStationCode = null;
            SelectedStationName = null;
            SelectedStation = null;
            InitializeForm();
            LoadStations();
        }

        private void InitializeForm()
        {
            this.Text = "Stations List";
            this.Size = new Size(420, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 75,
                Padding = new Padding(12)
            };

            Label lblSearch = new Label
            {
                Text = "Search Station Code or Name:",
                Location = new Point(12, 10),
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            pnlHeader.Controls.Add(lblSearch);

            txtSearch = new TextBox
            {
                Location = new Point(12, 32),
                Width = 375,
                Font = new Font("Segoe UI", 10)
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;
            pnlHeader.Controls.Add(txtSearch);

            this.Controls.Add(pnlHeader);

            dgvStations = new DataGridView
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

            dgvStations.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "StationCode",
                HeaderText = "Station Code",
                DataPropertyName = "STATION_CODE",
                Width = 130
            });

            dgvStations.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "StationName",
                HeaderText = "Station Name (English)",
                DataPropertyName = "STATION_NAME_ENGLISH"
            });

            dgvStations.DoubleClick += DgvStations_DoubleClick;

            this.Controls.Add(dgvStations);

            Panel pnlFooter = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 55,
                Padding = new Padding(10)
            };

            btnOK = new Button
            {
                Text = "OK",
                Width = 75,
                Height = 30,
                Location = new Point(220, 12),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                UseVisualStyleBackColor = true
            };
            btnOK.Click += BtnOK_Click;

            btnClose = new Button
            {
                Text = "Close",
                DialogResult = DialogResult.Cancel,
                Width = 75,
                Height = 30,
                Location = new Point(305, 12),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                UseVisualStyleBackColor = true
            };

            pnlFooter.Controls.Add(btnOK);
            pnlFooter.Controls.Add(btnClose);
            this.Controls.Add(pnlFooter);

            this.AcceptButton = btnOK;
            this.CancelButton = btnClose;

            dgvStations.BringToFront();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            ConfirmSelection();
        }

        private void DgvStations_DoubleClick(object sender, EventArgs e)
        {
            ConfirmSelection();
        }

        private void ConfirmSelection()
        {
            if (dgvStations.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a station from the list.",
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var station = (StationList)dgvStations.SelectedRows[0].DataBoundItem;
            if (station == null) return;

            SelectedStationCode = station.STATION_CODE;
            SelectedStationName = station.STATION_NAME_ENGLISH;
            SelectedStation = station;

            if (_targetTextBox != null)
            {
                _targetTextBox.Text = SelectedStationCode;
            }

            if (_isOnlineTrainMode && !string.IsNullOrEmpty(_trainNumber))
            {
                try
                {
                    _dbService.AddMergeTrainDetails(_trainNumber, station.STATION_CODE, station.STATION_NAME_ENGLISH);
                    MessageBox.Show($"Station {station.STATION_CODE} added successfully.",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void LoadStations()
        {
            dgvStations.DataSource = _allStations.ToList();
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            string term = txtSearch.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(term))
            {
                dgvStations.DataSource = _allStations.ToList();
            }
            else
            {
                var filtered = _allStations.Where(s =>
                    (s.STATION_CODE != null && s.STATION_CODE.ToLower().Contains(term)) ||
                    (s.STATION_NAME_ENGLISH != null && s.STATION_NAME_ENGLISH.ToLower().Contains(term))
                ).ToList();
                dgvStations.DataSource = filtered;
            }
        }
    }
}