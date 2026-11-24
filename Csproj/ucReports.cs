 

using Helpers;
using Microsoft.Reporting.WinForms;
using System.Data;


namespace IPIS
{
    public partial class ucReports : UserControl
    {
        private bool _isLoading = false;
        private bool _isFetchButtonClicked = false;
        //private readonly int[] _reportsWithoutDateFilter = { 5 }; 

        private Panel panelLoader;
        private IPISv2.Controls.LoaderControl reportLoader;
        private Label lblLoaderText;

        public ucReports()
        {
            InitializeComponent();
            InitializeReportLoader();
            LoaderManager.Initialize(this);
            this.Load += ucReports_Load;
            btnFetchReport.Click += btnFetchReport_Click;
            btnRefresh.Click += btnRefresh_Click; // Wire up Refresh button
        }

        private void InitializeReportLoader()
        {
            panelLoader = new Panel();
            panelLoader.Dock = DockStyle.Fill;
            // Professional light green color (soft pastel sage/mint green)
            panelLoader.BackColor = Color.FromArgb(235, 245, 240);
            panelLoader.Name = "panelReportLoader";
            panelLoader.Visible = false;

            reportLoader = new IPISv2.Controls.LoaderControl();
            reportLoader.BackColor = Color.Transparent;
            reportLoader.Size = new Size(120, 120);

            lblLoaderText = new Label();
            lblLoaderText.Text = "Generating report data, please wait...";
            lblLoaderText.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblLoaderText.ForeColor = Color.FromArgb(46, 117, 89); // Deep professional green
            lblLoaderText.AutoSize = true;
            lblLoaderText.TextAlign = ContentAlignment.MiddleCenter;

            // Add controls
            panelLoader.Controls.Add(reportLoader);
            panelLoader.Controls.Add(lblLoaderText);

            // Center the controls in the panel
            panelLoader.Resize += (s, e) =>
            {
                reportLoader.Location = new Point(
                    (panelLoader.Width - reportLoader.Width) / 2,
                    (panelLoader.Height - reportLoader.Height) / 2 - 40
                );
                lblLoaderText.Location = new Point(
                    (panelLoader.Width - lblLoaderText.Width) / 2,
                    reportLoader.Location.Y + reportLoader.Height + 20
                );
            };

            // Add the loader panel to row 1, column 0 of mainTableLayoutPanel
            mainTableLayoutPanel.Controls.Add(panelLoader, 0, 1);
        }

        private void ShowLoader()
        {
            if (panelLoader != null && reportLoader != null)
            {
                reportViewer1.Visible = false;
                lblPlaceholder.Visible = false;
                panelLoader.Visible = true;
                panelLoader.BringToFront();
                reportLoader.StartLoading();
            }
        }

        private void HideLoader()
        {
            if (panelLoader != null && reportLoader != null)
            {
                reportLoader.StopLoading();
                panelLoader.Visible = false;
            }
        }

        private void ucReports_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now.Date;
            dateTimePicker2.Value = DateTime.Now.Date;
            dateTimePicker1.ValueChanged += DateFilters_Changed;
            dateTimePicker2.ValueChanged += DateFilters_Changed;

            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }
        }

        private void DateFilters_Changed(object sender, EventArgs e)
        {
            if (dateTimePicker2.Value.Date < dateTimePicker1.Value.Date)
            {
                MessageBox.Show("The 'To' date cannot be earlier than the 'From' date.", "Invalid Range", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dateTimePicker2.Value = dateTimePicker1.Value;
                return;
            }
        }

        // ============================================================
        // FETCH BUTTON - Loads with Date Filter
        // ============================================================
        private async void btnFetchReport_Click(object sender, EventArgs e)
        {
            if (_isLoading) return;

            if (dateTimePicker2.Value.Date < dateTimePicker1.Value.Date)
            {
                MessageBox.Show("The 'To' date cannot be earlier than the 'From' date.", "Invalid Range", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a report type first.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _isFetchButtonClicked = true;
            await LoadSelectedReport();
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            if (_isLoading) return;

            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a report type first.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Reset the flag to false - this will use Fill() without date filter
            _isFetchButtonClicked = false;
            await LoadSelectedReport();

            // Show a message to indicate refresh
            lblPlaceholder.Visible = false;
            reportViewer1.Visible = true;
        }

        private async void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
            {
                comboBox1.SelectedIndex = 0;
                return;
            }

            _isFetchButtonClicked = false;


            
            {
                lblPlaceholder.Visible = true;
                lblPlaceholder.Text = "This report does not require date filtering.";
                reportViewer1.Visible = false;
                reportViewer1.Reset();
                reportViewer1.RefreshReport();

                // Load without date filter
                _isFetchButtonClicked = false;
                await LoadSelectedReport();
            }

            headerTableLayoutPanel.Refresh();
            mainTableLayoutPanel.Refresh();
        }

        private async Task LoadSelectedReport()
        {
            if (_isLoading) return;

            _isLoading = true;
            btnFetchReport.Enabled = false;
            btnRefresh.Enabled = false;
            ShowLoader();

            try
            {
                switch (comboBox1.SelectedIndex)
                {
                    //case 0: await LoadVDCLogReportAsync(); break;
                    case 0: await LoadCGDBLogReportAsync(); break;
                    case 1: await LoadTADBLogReportAsync(); break;
                    case 2: await LoadAnnouncementLogReportAsync(); break;
                    case 3: await LoadLogInUserLogReportAsync(); break;
                    //case 4: await LoadTrainDetailsReportAsync(); break;
                    //case 5: await LoadTrainLogReportAsync(); break;
                    case 4: await LoadTrainMasterAsync(); break;
                    case 5: await LoadLedMatrixFaultReportAsync(); break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                HideLoader();
                _isLoading = false;
                btnFetchReport.Enabled = true;
                btnRefresh.Enabled = true;
            }
        }

        // ============================================================
        // 1. VDC LOG REPORT
        // ============================================================
        //private async Task LoadVDCLogReportAsync()
        //{
        //    try
        //    {
        //        reportViewer1.Reset();

        //        var databaseTable = new IPISv2.ReportDataSets.VDCLogReportDataSet.LOGVDCDataTable();
        //        var adapter = new IPISv2.ReportDataSets.VDCLogReportDataSetTableAdapters.LOGVDCTableAdapter();
        //        adapter.Connection.ConnectionString = Helper.Config._dbSelfConnection;

        //        if (_isFetchButtonClicked)
        //        {
        //            // Fetch button clicked - use date filter
        //            string fromDate = dateTimePicker1.Value.Date.ToString("yyyy-MM-dd");
        //            string toDate = dateTimePicker2.Value.Date.ToString("yyyy-MM-dd");
        //            await Task.Run(() => adapter.FillByDateRange(databaseTable, fromDate, toDate));
        //        }
        //        else
        //        {
        //            // Refresh or auto-load - use Fill without date filter
        //            await Task.Run(() => adapter.Fill(databaseTable));
        //        }

        //        if (databaseTable.Rows.Count == 0)
        //        {
        //            ShowNoDataMessage(_isFetchButtonClicked ?
        //                "No VDC Log data found for the selected date range." :
        //                "No VDC Log data found.");
        //            return;
        //        }

        //        reportViewer1.LocalReport.ReportPath = GetReportPath("VDCLogReport.rdlc");
        //        reportViewer1.LocalReport.DataSources.Clear();
        //        ReportDataSource rds = new ReportDataSource("VDCLogReportDataSet", (DataTable)databaseTable);
        //        reportViewer1.LocalReport.DataSources.Add(rds);
        //        reportViewer1.Visible = true;
        //        lblPlaceholder.Visible = false;
        //        reportViewer1.RefreshReport();
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleReportError("VDC Log Report", ex);
        //    }
        //}

        // ============================================================
        // 2. CGDB LOG REPORT
        // ============================================================
        private async Task LoadCGDBLogReportAsync()
        {
            try
            {
                reportViewer1.Reset();

                var databaseTable = new IPISv2.ReportDataSets.CGDBLogReportDataSet.LOGCOACHSEQUENCEDataTable();
                var adapter = new IPISv2.ReportDataSets.CGDBLogReportDataSetTableAdapters.LOGCOACHSEQUENCETableAdapter();
                adapter.Connection.ConnectionString = Helper.Config._dbSelfConnection;

                if (_isFetchButtonClicked)
                {
                    string fromDate = dateTimePicker1.Value.Date.ToString("yyyy-MM-dd") + " 00:00:00";
                    string toDate = dateTimePicker2.Value.Date.ToString("yyyy-MM-dd") + " 23:59:59";
                    await Task.Run(() => adapter.FillByDateRange(databaseTable, fromDate, toDate));
                }
                else
                {
                    await Task.Run(() => adapter.Fill(databaseTable));
                }

                if (databaseTable.Rows.Count == 0)
                {
                    ShowNoDataMessage(_isFetchButtonClicked ?
                        "No CGDB Log data found for the selected date range." :
                        "No CGDB Log data found.");
                    return;
                }

                reportViewer1.LocalReport.ReportPath = GetReportPath("CGDBLogReport.rdlc");
                reportViewer1.LocalReport.DataSources.Clear();
                ReportDataSource rds = new ReportDataSource("CGDBLogReportDataSet", (DataTable)databaseTable);
                reportViewer1.LocalReport.DataSources.Add(rds);
                reportViewer1.Visible = true;
                lblPlaceholder.Visible = false;
                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                HandleReportError("CGDB Log Report", ex);
            }
        }

        // ============================================================
        // 3. TADB LOG REPORT
        // ============================================================
        private async Task LoadTADBLogReportAsync()
        {
            try
            {
                reportViewer1.Reset();

                var databaseTable = new IPISv2.ReportDataSets.TADBLogReportDataSet.LOGDISPLAYBOARDDataTable();
                var adapter = new IPISv2.ReportDataSets.TADBLogReportDataSetTableAdapters.LOGDISPLAYBOARDTableAdapter();
                adapter.Connection.ConnectionString = Helper.Config._dbSelfConnection;

                if (_isFetchButtonClicked)
                {
                    string fromDate = dateTimePicker1.Value.Date.ToString("yyyy-MM-dd") + " 00:00:00";
                    string toDate = dateTimePicker2.Value.Date.ToString("yyyy-MM-dd") + " 23:59:59";
                    await Task.Run(() => adapter.FillByDateRange(databaseTable, fromDate, toDate));
                }
                else
                {
                    await Task.Run(() => adapter.Fill(databaseTable));
                }

                if (databaseTable.Rows.Count == 0)
                {
                    ShowNoDataMessage(_isFetchButtonClicked ?
                        "No TADB Log data found for the selected date range." :
                        "No TADB Log data found.");
                    return;
                }

                reportViewer1.LocalReport.ReportPath = GetReportPath("TADBLogReport.rdlc");
                reportViewer1.LocalReport.DataSources.Clear();
                ReportDataSource rds = new ReportDataSource("TADBLogReportDataSet", (DataTable)databaseTable);
                reportViewer1.LocalReport.DataSources.Add(rds);
                reportViewer1.Visible = true;
                lblPlaceholder.Visible = false;
                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                HandleReportError("TADB Log Report", ex);
            }
        }

        private async Task LoadAnnouncementLogReportAsync()
        {
            try
            {
                reportViewer1.Reset();

                var databaseTable = new IPISv2.ReportDataSets.AnnouncementLogReportDataSet.LOGANNOUNCEDataTable();
                var adapter = new IPISv2.ReportDataSets.AnnouncementLogReportDataSetTableAdapters.LOGANNOUNCETableAdapter();
                adapter.Connection.ConnectionString = Helper.Config._dbSelfConnection;

                if (_isFetchButtonClicked)
                {
                    string fromDate = dateTimePicker1.Value.Date.ToString("yyyy-MM-dd") + " 00:00:00";
                    string toDate = dateTimePicker2.Value.Date.ToString("yyyy-MM-dd") + " 23:59:59";
                    await Task.Run(() => adapter.FillByDateRange(databaseTable, fromDate, toDate));
                }
                else
                {
                    await Task.Run(() => adapter.Fill(databaseTable));
                }

                if (databaseTable.Rows.Count == 0)
                {
                    ShowNoDataMessage(_isFetchButtonClicked ?
                        "No Announcement Log data found for the selected date range." :
                        "No Announcement Log data found.");
                    return;
                }

                reportViewer1.LocalReport.ReportPath = GetReportPath("AnnouncementLogReport.rdlc");
                reportViewer1.LocalReport.DataSources.Clear();
                ReportDataSource rds = new ReportDataSource("AnnouncementLogReportDataSet", (DataTable)databaseTable);
                reportViewer1.LocalReport.DataSources.Add(rds);
                reportViewer1.Visible = true;
                lblPlaceholder.Visible = false;
                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                HandleReportError("Announcement Log Report", ex);
            }
        }

        private async Task LoadTrainMasterAsync()
        {
            try
            {
                reportViewer1.Reset();

                var databaseTable = new IPISv2.ReportDataSets.LogTrainMasterDataset.LOGTRAINMASTERDataTable();
                var adapter = new IPISv2.ReportDataSets.LogTrainMasterDatasetTableAdapters.LOGTRAINMASTERTableAdapter();
                adapter.Connection.ConnectionString = Helper.Config._dbSelfConnection;

                if (_isFetchButtonClicked)
                {
                    //string fromDate = dateTimePicker1.Value.Date.ToString("yyyy-MM-dd");
                    //string toDate = dateTimePicker2.Value.Date.ToString("yyyy-MM-dd");
                    await Task.Run(() => adapter.FillByDateRange(
                        databaseTable,
                        (DateTime?)dateTimePicker1.Value.Date,
                        (DateTime?)dateTimePicker2.Value.Date.AddDays(1).AddTicks(-1)));
                }
                else
                {
                    await Task.Run(() => adapter.Fill(databaseTable));
                }

                if (databaseTable.Rows.Count == 0)
                {
                    ShowNoDataMessage(_isFetchButtonClicked ?
                        "No Train Master data found for the selected date range." :
                        "No Train Master data found.");
                    return;
                }

                reportViewer1.LocalReport.ReportPath = GetReportPath("LogTrainMaster.rdlc");
                reportViewer1.LocalReport.DataSources.Clear();
                ReportDataSource rds = new ReportDataSource("LogTrainMasterDataset", (DataTable)databaseTable);
                reportViewer1.LocalReport.DataSources.Add(rds);
                reportViewer1.Visible = true;
                lblPlaceholder.Visible = false;
                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                HandleReportError("Train Master Report", ex);
            }
        }


        // 5. USER LOGIN REPORT
        private async Task LoadLogInUserLogReportAsync()
        {
            try
            {
                reportViewer1.Reset();

                var databaseTable = new IPISv2.ReportDataSets.UserLogInReportDataSet.UserLoginHistoryDataTable();
                var adapter = new IPISv2.ReportDataSets.UserLogInReportDataSetTableAdapters.UserLoginHistoryTableAdapter();
                adapter.Connection.ConnectionString = Helper.Config._dbSelfConnection;

                if (_isFetchButtonClicked)
                {
                    //string fromDate = dateTimePicker1.Value.Date.ToString("yyyy-MM-dd");
                    //string toDate = dateTimePicker2.Value.Date.ToString("yyyy-MM-dd");
                    await Task.Run(() => adapter.FillByDateRange(
                        databaseTable,
                        (DateTime?)dateTimePicker1.Value.Date,
                        (DateTime?)dateTimePicker2.Value.Date.AddDays(1).AddTicks(-1)));
                }
                else
                {
                    await Task.Run(() => adapter.Fill(databaseTable));
                }
              
                if (databaseTable.Rows.Count == 0)
                {
                    ShowNoDataMessage(_isFetchButtonClicked ?
                        "No User Login data found for the selected date range." :
                        "No User Login data found.");
                    return;
                }

                reportViewer1.LocalReport.ReportPath = GetReportPath("UserLogInReport.rdlc");
                reportViewer1.LocalReport.DataSources.Clear();
                ReportDataSource rds = new ReportDataSource("UserLogInReportDataSet", (DataTable)databaseTable);
                reportViewer1.LocalReport.DataSources.Add(rds);
                reportViewer1.Visible = true;
                lblPlaceholder.Visible = false;
                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                HandleReportError("User Login Report", ex);
            }
        }

        // ============================================================
        // LED MATRIX OPEN/SHORT REPORT
        // ============================================================
        private async Task LoadLedMatrixFaultReportAsync()
        {
            try
            {
                reportViewer1.Reset();

                var databaseTable =
                    new IPISv2.Report.ReportDataSets.LEDMatrixFaultReportDataSet
                        .LOGLED_SHORT_OPENDataTable();

                var adapter =
                    new IPISv2.Report.ReportDataSets.LEDMatrixFaultReportDataSetTableAdapters
                        .LOGLED_SHORT_OPENTableAdapter();

                adapter.Connection.ConnectionString = Helper.Config._dbSelfConnection;

                if (_isFetchButtonClicked)
                {
                    await Task.Run(() => adapter.FillByDateRange(
                        databaseTable,
                        (DateTime?)dateTimePicker1.Value.Date,
                        (DateTime?)dateTimePicker2.Value.Date
                            .AddDays(1)
                            .AddTicks(-1)));
                }
                else
                {
                    await Task.Run(() => adapter.Fill(databaseTable));
                }

                if (databaseTable.Rows.Count == 0)
                {
                    ShowNoDataMessage(
                        _isFetchButtonClicked
                            ? "No LED open/short faults found for the selected date range."
                            : "No LED open/short faults found.");

                    return;
                }

                reportViewer1.LocalReport.ReportPath =
                    GetReportPath("LEDMatrixFaultReport.rdlc");

                reportViewer1.LocalReport.DataSources.Clear();

                ReportDataSource reportDataSource = new ReportDataSource(
                    "LEDMatrixFaultDataSet",
                    (DataTable)databaseTable
                );

                reportViewer1.LocalReport.DataSources.Add(reportDataSource);

                reportViewer1.Visible = true;
                lblPlaceholder.Visible = false;

                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                HandleReportError("LED Matrix Open/Short Report", ex);
            }
        }
 
        private static string GetReportPath(string rdlcFileName)
        {
            string[] possiblePaths = new[]
            {
                Path.Combine(Application.StartupPath, "Report", "TrainReports", rdlcFileName),
                Path.Combine(Application.StartupPath, "Reports", "TrainReports", rdlcFileName),
                Path.Combine(Application.StartupPath, "TrainReports", rdlcFileName),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Report", "TrainReports", rdlcFileName),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "TrainReports", rdlcFileName),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TrainReports", rdlcFileName)
            };

            foreach (var path in possiblePaths)
            {
                if (File.Exists(path))
                    return path;
            }

            return Path.Combine(Application.StartupPath, "Report", "TrainReports", rdlcFileName);
        }

        private void ShowNoDataMessage(string message)
        {
            lblPlaceholder.Visible = true;
            lblPlaceholder.Text = message;
            reportViewer1.Visible = false;
        }

        private void HandleReportError(string reportName, Exception ex)
        {
            MessageBox.Show($"Failed to load {reportName}: {ex.Message}", "Report Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            lblPlaceholder.Visible = true;
            lblPlaceholder.Text = $"Error loading {reportName}: {ex.Message}";
            reportViewer1.Visible = false;
        }



        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e) { }
        private void panel2_Paint(object sender, PaintEventArgs e) { }

        public void RefreshData()
        {
            btnFetchReport_Click(this, EventArgs.Empty);
        }
    }
}
