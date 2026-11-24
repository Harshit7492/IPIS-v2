
using DataBaseHandler;
using DataModel;
using Global_Data;
using Helper;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using IPISv2;
using IPISv2.Controls;
using LogManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using static IPIS.ucInterfaceAdminSetting;

namespace IPIS
{
    public partial class ucSetUpTrainEntry : UserControl
    {
        private int currentRowIndex = -1;
        int _coachcolCount = 8;
        Image imgAdd;
        Image imgremove;
        private Form keyboardForm;
        private ucKeyboard keyboardControl;
        public ucSetUpTrainEntry()
        {
            InitializeComponent();
            imgAdd = Image.FromFile(AppHelper.GetImagePath("add.png"));
            imgremove = Image.FromFile(AppHelper.GetImagePath("trash.png"));
            //this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            //this.btnBrowse2.Click += new System.EventHandler(this.btnBrowse2_Click);
            //this.btnBrowse3.Click += new System.EventHandler(this.btnBrowse3_Click);
            btn_Add.Image = imgAdd;
            btn_Remove.Image = imgremove;
            LoadTrainMaster();
            LoadDropdownData(); // Add this line

        }
        private void ClearForm()
        {
            textBoxTrainNumber.Text = "";
            textBoxTrainName.Text = "";
            textBoxTrainNameHindi.Text = "";
            textBoxTrainNameRegional.Text = "";
            textBoxdestination.Text = "";
            textBoxSource.Text = "";
            textBoxVia1.Text = "";
            textBoxVia2.Text = "";
            textBoxVia3.Text = "";
            textBoxVia4.Text = "";

            Sun.Checked = Mon.Checked = Tues.Checked = Wed.Checked = Thurs.Checked = Fri.Checked = Sat.Checked = false;
            dateTimeArrvTime.Value = DateTime.Today.AddHours(0).AddMinutes(0);
            dateTimeDepartTime.Value = DateTime.Today.AddHours(0).AddMinutes(0);
            txtPFNo.Text = "";
            txtPFEnd.Text = "";
            combBoxTrainType.Text = "";
            comboBoxTrainCategory.Text = "";
            checkBoxStatus.Checked = false;
        }
        private void LoadTrainMaster()
        {
            try
            {
                List<TrainMaster> dt = _dbService.GET_TRAIN_MASTER();
                var selected = dt.Select(t => new
                {
                    TrainNo = t.TRAIN_NUMBER,
                    TrainName = t.TRAIN_NAME_ENGLISH,
                    ArrivalTime = t.ARRIVAL_TIME,
                    DepartureTime = t.DEPARTURE_TIME,
                    PlatformNo = t.PLATFORM_NUMBER,
                    Destination = t.DESTINATION_CODE,
                    TrainType = t.TRAIN_TYPE,
                    TrainCategory = t.TRAIN_CATEOGARY,
                    Status = t.STATUS,
                    TrainNameHindi = t.TRAIN_NAME_HINDI,
                    TrainNameRegional = t.TRAIN_NAME_REGIONAL,
                    PFEnd = t.PF_END,
                    Source = t.ORIGIN_CODE,
                    Via1 = t.VIA1_CODE,
                    Via2 = t.VIA2_CODE,
                    Via3 = t.VIA3_CODE,
                    Via4 = t.VIA4_CODE,
                    RunningDays = t.RUNNING_DAYS
                }).ToList();

                dgvViewTrainEntry.DataSource = selected;
                dgvViewTrainEntry.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvViewTrainEntry.MultiSelect = false;


                dgvViewTrainEntry.Columns["TrainNo"].HeaderText = "Train No.";
                dgvViewTrainEntry.Columns["TrainName"].HeaderText = "Train Name";
                dgvViewTrainEntry.Columns["ArrivalTime"].HeaderText = "Arrival Time";
                dgvViewTrainEntry.Columns["DepartureTime"].HeaderText = "Departure Time";
                dgvViewTrainEntry.Columns["PlatformNo"].HeaderText = "Platform No.";
                dgvViewTrainEntry.Columns["Destination"].HeaderText = "Destination";

                string[] hide = { "TrainType", "TrainCategory", "Status", "TrainNameHindi", "TrainNameRegional", "PFEnd", "Source", "Via1", "Via2", "Via3", "Via4" };
                foreach (var h in hide)
                {
                    if (dgvViewTrainEntry.Columns.Contains(h))
                        dgvViewTrainEntry.Columns[h].Visible = false;
                }
                if (dgvViewTrainEntry.Rows.Count > 0)
                {
                    dgvViewTrainEntry.Rows[0].Selected = true;
                    currentRowIndex = 0;
                    LoadSelectedRowToForm(0);
                    string firstTrainNo = dgvViewTrainEntry.Rows[0].Cells["TrainNo"].Value?.ToString();
                    if (!string.IsNullOrEmpty(firstTrainNo))
                        LoadCoachData(firstTrainNo);
                }
                else
                {
                    ClearForm();
                    currentRowIndex = -1;
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
        }
        //New code
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
                p.Y - keyboardForm.Height - 5
            );

            keyboardForm.Show();
            keyboardForm.BringToFront();
        }
        private void textBoxTrainNameHindi_MouseClick(object sender, EventArgs e)
        {
            //if (ucKeyboard.Visible)
            //{
            //    ucKeyboard.Hide();
            //}
            //else
            //{
            //    ShowKeyboard(
            //        textBoxTrainNameHindi,
            //        ucKeyboard.KeyboardLanguage.Hindi);
            //}

            ShowKeyboard(
                textBoxTrainNameHindi,
                ucKeyboard.KeyboardLanguage.Hindi);
        }
        private void textBoxTrainNameRegional_MouseClick(object sender, EventArgs e)
        {
            var language = GetRegionalKeyboardLanguage();

            if (language == null)
            {
                MessageBox.Show(
                    "Regional language not configured.",
                    "Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            ShowKeyboard(textBoxTrainNameRegional,language.Value);
        }
        private ucKeyboard.KeyboardLanguage? GetRegionalKeyboardLanguage()
        {
            if (GlobalData._lstAdminSetting == null)
                return null;

            string regionalLanguage = GlobalData._lstAdminSetting.STATION_LANGUAGES.Split('+').LastOrDefault()?.Trim();


            if (string.IsNullOrEmpty(regionalLanguage))
                return null;

            if (Enum.TryParse(regionalLanguage, true, out ucKeyboard.KeyboardLanguage lang))
                return lang;

            return null;
        }
        private void LoadSelectedRowToForm(int rowIndex)
        {
            try
            {
                if (rowIndex < 0 || rowIndex >= dgvViewTrainEntry.Rows.Count) return;

                DataGridViewRow row = dgvViewTrainEntry.Rows[rowIndex];

                textBoxTrainNumber.Text = row.Cells["TrainNo"].Value?.ToString();
                textBoxTrainName.Text = row.Cells["TrainName"].Value?.ToString();
                textBoxTrainNameHindi.Text = row.Cells["TrainNameHindi"].Value?.ToString();
                textBoxTrainNameRegional.Text = row.Cells["TrainNameRegional"].Value?.ToString();
                textBoxdestination.Text = row.Cells["Destination"].Value?.ToString();
                textBoxSource.Text = row.Cells["Source"].Value?.ToString();
                textBoxVia1.Text = row.Cells["Via1"].Value?.ToString();
                textBoxVia2.Text = row.Cells["Via2"].Value?.ToString();
                textBoxVia3.Text = row.Cells["Via3"].Value?.ToString();
                textBoxVia4.Text = row.Cells["Via4"].Value?.ToString();

                string runningDays = row.Cells["RunningDays"].Value?.ToString() ?? "";

                Sun.Checked = runningDays.Contains("7");
                Mon.Checked = runningDays.Contains("1");
                Tues.Checked = runningDays.Contains("2");
                Wed.Checked = runningDays.Contains("3");
                Thurs.Checked = runningDays.Contains("4");
                Fri.Checked = runningDays.Contains("5");
                Sat.Checked = runningDays.Contains("6");

                if (row.Cells["ArrivalTime"].Value != null)
                    dateTimeArrvTime.Value = Convert.ToDateTime(row.Cells["ArrivalTime"].Value);

                if (row.Cells["DepartureTime"].Value != null)
                    dateTimeDepartTime.Value = Convert.ToDateTime(row.Cells["DepartureTime"].Value);

                checkBoxStatus.Checked = (row.Cells["Status"].Value?.ToString() == "Y");

                txtPFNo.Text = row.Cells["PlatformNo"].Value?.ToString();
                combBoxTrainType.Text = row.Cells["TrainType"].Value?.ToString();
                txtPFEnd.Text = row.Cells["PFEnd"].Value?.ToString();
                comboBoxTrainCategory.Text = row.Cells["TrainCategory"].Value?.ToString();

                string trainNo = textBoxTrainNumber.Text;
                string trainName = textBoxTrainName.Text;
                lblCoachTrainHeader.Text = $"{trainNo}  {trainName}";
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);

            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBoxTrainNumber.Text))
                {
                    MessageBox.Show("Train Number is required.");
                    return;
                }
                else if (string.IsNullOrWhiteSpace(combBoxTrainType.Text))
                {
                    MessageBox.Show("Train Type is required");
                    return;
                }

                    TrainMaster t = new TrainMaster
                    {
                        TRAIN_NUMBER = textBoxTrainNumber.Text.Trim(),
                        TRAIN_NAME_ENGLISH = textBoxTrainName.Text.Trim(),
                        TRAIN_NAME_HINDI = textBoxTrainNameHindi.Text.Trim(),
                        TRAIN_NAME_REGIONAL = textBoxTrainNameRegional.Text.Trim(),
                        ORIGIN_CODE = textBoxSource.Text.Trim(),
                        DESTINATION_CODE = textBoxdestination.Text.Trim(),
                        VIA1_CODE = textBoxVia1.Text.Trim(),
                        VIA2_CODE = textBoxVia2.Text.Trim(),
                        VIA3_CODE = textBoxVia3.Text.Trim(),
                        VIA4_CODE = textBoxVia4.Text.Trim(),
                        ARRIVAL_TIME = dateTimeArrvTime.Value.ToString("HH:mm"),
                        DEPARTURE_TIME = dateTimeDepartTime.Value.ToString("HH:mm"),
                        PLATFORM_NUMBER = txtPFNo.Text.Trim(),
                        PF_END = txtPFEnd.Text.Trim(),
                        TRAIN_TYPE = combBoxTrainType.Text.Trim(),
                        TRAIN_CATEOGARY = comboBoxTrainCategory.Text.Trim(),
                        STATUS = checkBoxStatus.Checked ? "Y" : "N",
                        RUNNING_DAYS = BuildRunningDaysString(),
                        REGIONAL_BITMAP = null,
                        TRAIN_SORT_TIME = dateTimeArrvTime.Value.ToString("HHmm"),
                    };

                bool result = _dbService.SaveOrUpdateTrain(t);
                LoggerService.SettingsLogger.Information("Train master add/update: {TrainNumber} {TrainName} -> {Result}.", t.TRAIN_NUMBER, t.TRAIN_NAME_ENGLISH, result ? "saved" : "FAILED");

                if (result)
                {
                    MessageBox.Show("Saved successfully!!", "VARDAAN ELECTRONICS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _dbService.InsertTrainMasterLog(t);
                    LoadTrainMaster();
                    ClearForm();
                    lblCoachSummary.Text = BuildCoachLabelText();
                }
                else
                    MessageBox.Show("Save failed. try again!!", "VARDAAN ELECTRONICS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                LoggerService.SettingsLogger.Error(ex, ex.Message);
            }
        }
        private string BuildRunningDaysString()
        {

            var sb = new System.Text.StringBuilder();
            if (Mon.Checked) sb.Append("1");
            if (Tues.Checked) sb.Append("2");
            if (Wed.Checked) sb.Append("3");
            if (Thurs.Checked) sb.Append("4");
            if (Fri.Checked) sb.Append("5");
            if (Sat.Checked) sb.Append("6");
            if (Sun.Checked) sb.Append("7");
            return sb.ToString();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvViewTrainEntry.SelectedRows.Count == 0) return;

            string trainNo = dgvViewTrainEntry.SelectedRows[0].Cells["TrainNo"].Value?.ToString();
            if (string.IsNullOrWhiteSpace(trainNo)) return;

            if (MessageBox.Show("Delete train " + trainNo + " ?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string query = "DELETE FROM TRAINMASTER WHERE TRAIN_NUMBER='" + trainNo + "'";
                string result = _dbService.ExecuteScalar(query);
                LoggerService.SettingsLogger.Information("Train master delete: {TrainNumber}.", trainNo);
                MessageBox.Show("Train deleted successfully!!.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadTrainMaster();
                ClearForm();
            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox("Enter Train Number to search:", "Search Train", "");
                if (string.IsNullOrWhiteSpace(input)) return;

                bool found = false;
                for (int i = 0; i < dgvViewTrainEntry.Rows.Count; i++)
                {
                    var val = dgvViewTrainEntry.Rows[i].Cells["TrainNo"].Value?.ToString();
                    if (val != null && val.Equals(input.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        dgvViewTrainEntry.ClearSelection();
                        dgvViewTrainEntry.Rows[i].Selected = true;
                        dgvViewTrainEntry.FirstDisplayedScrollingRowIndex = i;
                        currentRowIndex = i;
                        LoadSelectedRowToForm(i);
                        found = true;
                        break;
                    }
                }

                if (!found)
                    MessageBox.Show("Train not found.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
            dgvViewTrainEntry.ClearSelection();
            currentRowIndex = -1;
        }
        private void btnLast_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvViewTrainEntry.Rows.Count == 0) return;
                int i = dgvViewTrainEntry.Rows.Count - 1;
                dgvViewTrainEntry.ClearSelection();
                dgvViewTrainEntry.Rows[i].Selected = true;
                dgvViewTrainEntry.FirstDisplayedScrollingRowIndex = i;
                currentRowIndex = i;
                LoadSelectedRowToForm(i);
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvViewTrainEntry.Rows.Count == 0) return;
                int i = Math.Min(dgvViewTrainEntry.Rows.Count - 1, (currentRowIndex < 0 ? 0 : currentRowIndex + 1));
                dgvViewTrainEntry.ClearSelection();
                dgvViewTrainEntry.Rows[i].Selected = true;
                dgvViewTrainEntry.FirstDisplayedScrollingRowIndex = i;
                currentRowIndex = i;
                LoadSelectedRowToForm(i);
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
        }
        private void btnPrev_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvViewTrainEntry.Rows.Count == 0) return;
                int i = Math.Max(0, (currentRowIndex <= 0 ? 0 : currentRowIndex - 1));
                dgvViewTrainEntry.ClearSelection();
                dgvViewTrainEntry.Rows[i].Selected = true;
                dgvViewTrainEntry.FirstDisplayedScrollingRowIndex = i;
                currentRowIndex = i;
                LoadSelectedRowToForm(i);
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
        }
        private void btnFirst_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvViewTrainEntry.Rows.Count == 0) return;
                int i = 0;
                dgvViewTrainEntry.ClearSelection();
                dgvViewTrainEntry.Rows[i].Selected = true;
                dgvViewTrainEntry.FirstDisplayedScrollingRowIndex = i;
                currentRowIndex = i;
                LoadSelectedRowToForm(i);
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
        }
        private void btRefresh_Click(object sender, EventArgs e)
        {
            LoadTrainMaster();
        }
        private void dgvViewTrainEntry_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.RowIndex >= dgvViewTrainEntry.Rows.Count)
                    return;

                currentRowIndex = e.RowIndex;
                LoadSelectedRowToForm(e.RowIndex);

                string trainNumber = dgvViewTrainEntry.Rows[e.RowIndex].Cells["TrainNo"].Value?.ToString();
                if (!string.IsNullOrEmpty(trainNumber))
                    LoadCoachData(trainNumber);
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
        }
        private void LoadCoachData(string trainNumber)
        {
            try
            {
                int _columnCount = 10;
                string query = "SELECT COACH_SEQUENCE FROM TRAINMASTER WHERE TRAIN_NUMBER=" + trainNumber;
                string result = _dbService.QuerySingleOrDefault(query);

                if (string.IsNullOrEmpty(result) || result.All(c => c == '-'))
                {
                    foreach (Control ctrl in tableLayoutPanel8.Controls.OfType<Control>().ToList())
                    {
                        var pos = tableLayoutPanel8.GetPositionFromControl(ctrl);
                        if (pos.Row == 1)
                        {
                            tableLayoutPanel8.Controls.Remove(ctrl);
                            ctrl.Dispose();
                        }
                    }
                    lblCoachSummary.Text = string.Empty;
                    return;
                }
                string[] coachList = result.Split('-');
                foreach (Control ctrl in tableLayoutPanel8.Controls.OfType<Control>().ToList())
                {
                    var pos = tableLayoutPanel8.GetPositionFromControl(ctrl);
                    if (pos.Row == 1)
                    {
                        tableLayoutPanel8.Controls.Remove(ctrl);
                        ctrl.Dispose();
                    }
                }

                TableLayoutPanel innerTable = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    AutoSize = true,
                    AutoScroll = true,
                    ColumnCount = _columnCount,
                    RowCount = (int)Math.Ceiling(coachList.Length / 6.0),
                    Margin = new Padding(5)
                };
                innerTable.ColumnStyles.Clear();
                for (int c = 0; c < _columnCount; c++)
                    innerTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / 6));

                for (int r = 0; r < innerTable.RowCount; r++)
                    innerTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                int col = 0, row = 0;
                for (int i = 0; i < coachList.Length; i++)
                {
                    System.Windows.Forms.TextBox txt = new System.Windows.Forms.TextBox
                    {
                        Name = $"txtCoach{i + 1}",
                        Text = string.IsNullOrEmpty(coachList[i]) ? "-" : coachList[i],
                        Dock = DockStyle.Top,
                        Margin = new Padding(3),
                        TextAlign = HorizontalAlignment.Center
                    };
                    innerTable.Controls.Add(txt, col, row);

                    col++;
                    if (col >= 8)
                    {
                        col = 0;
                        row++;
                    }
                }
                tableLayoutPanel8.Controls.Add(innerTable, 0, 1);
                tableLayoutPanel8.SetColumnSpan(innerTable, tableLayoutPanel8.ColumnCount);
                lblCoachSummary.Text = BuildCoachLabelText();
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
        }
        //public void btnImportJson_Click(object sender, EventArgs e)
        //{
        //    try
        //    {

        //        string responsejson = HttpHandler.Instance.GetMasterTrains(GlobalData._lstAdminSetting);
        //        Root ntsdata = Newtonsoft.Json.JsonConvert.DeserializeObject<Root>(responsejson);

        //        List<TrainMaster> trains = new List<TrainMaster>();

        //        foreach (var item in ntsdata.vTrainList)
        //        {
        //            string arrivalTime = string.IsNullOrEmpty(item.STA) ? "00:00" : item.STA;
        //            string departureTime = string.IsNullOrEmpty(item.STD) ? "00:00" : item.STD;
        //            string sortTime = "";

        //            if (!string.IsNullOrEmpty(item.STA) && DateTime.TryParse(item.STA, out DateTime sta))
        //            {
        //                sortTime = sta.ToString("HHmm");
        //            }
        //            else if (!string.IsNullOrEmpty(item.STD) && DateTime.TryParse(item.STD, out DateTime std))
        //            {
        //                sortTime = std.ToString("HHmm");
        //            }
        //            string query = $"SELECT TOP 1 COACH_SEQUENCE FROM TRAINUPDATEDDATA WHERE TRAIN_NUMBER = '{item.trainNo}'";
        //            string coachSequence = _dbService.ExecuteScalar(query);

        //            // If not found, use API value
        //            if (string.IsNullOrWhiteSpace(coachSequence))
        //            {
        //                coachSequence = item.arrivalCoachClass != null
        //                    ? string.Join(",", item.arrivalCoachClass)
        //                    : "";
        //            }

        //            trains.Add(new TrainMaster
        //            {
        //                TRAIN_NUMBER = item.trainNo ?? "",
        //                TRAIN_NAME_ENGLISH = item.trainName ?? "",
        //                ORIGIN_CODE = item.src ?? "",
        //                DESTINATION_CODE = item.dstn ?? "",
        //                VIA1_CODE = string.Empty,
        //                VIA2_CODE = string.Empty,
        //                VIA3_CODE = string.Empty,
        //                VIA4_CODE = string.Empty,
        //                ARRIVAL_TIME = arrivalTime,
        //                DEPARTURE_TIME = departureTime,
        //                PLATFORM_NUMBER = item.platformNo ?? "",
        //                PF_END = string.Empty,
        //                TRAIN_TYPE = string.Empty,
        //                TRAIN_CATEOGARY = item.trainTypeName ?? "",
        //                STATUS = string.Empty,
        //                //COACH_SEQUENCE = item.arrivalCoachClass != null
        //                //    ? string.Join(",", item.arrivalCoachClass)
        //                //    : "",
        //                COACH_SEQUENCE = coachSequence,
        //                RUNNING_DAYS = (item.daysOfArrival != null &&
        //                                item.daysOfArrival.ToString().ToUpper() == "DAILY")
        //                                ? "1234567"
        //                                : "",
        //                REGIONAL_BITMAP = string.Empty,
        //                TRAIN_SORT_TIME = sortTime
        //            });
        //        }

        //        _dbService.SaveOrUpdateTrainMasterBulk(trains);

        //        LoadTrainMaster();

        //        MessageBox.Show("All trains imported successfully!!",
        //            "Info",
        //            MessageBoxButtons.OK,
        //            MessageBoxIcon.Information);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerService.Logger.Error(ex, ex.Message);
        //    }
        //}


        public async void btnImportJson_Click(object sender, EventArgs e)
        {
            //btnEnableBoards.Enabled = false;
            Cursor = Cursors.WaitCursor;
            try
            {
                string responsejson = await Task.Run(() =>HttpHandler.Instance.GetMasterTrains(GlobalData._lstAdminSetting));

                if (string.IsNullOrWhiteSpace(responsejson))
                {
                    MessageBox.Show("Please check your internet connection.", "Connection Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                List<TrainMaster> trains = await Task.Run(() =>
                {
                    Root ntsdata = Newtonsoft.Json.JsonConvert.DeserializeObject<Root>(responsejson);
                    List<TrainMaster> list = new List<TrainMaster>();

                    if (ntsdata == null || ntsdata.vTrainList == null)
                        return list; // empty list, nothing to import

                    foreach (var item in ntsdata.vTrainList)
                    {
                        string arrivalTime = string.IsNullOrEmpty(item.STA) ? "00:00" : item.STA;
                        string departureTime = string.IsNullOrEmpty(item.STD) ? "00:00" : item.STD;
                        string sortTime = "";
                        if (!string.IsNullOrEmpty(item.STA) && DateTime.TryParse(item.STA, out DateTime sta))
                            sortTime = sta.ToString("HHmm");
                        else if (!string.IsNullOrEmpty(item.STD) && DateTime.TryParse(item.STD, out DateTime std))
                            sortTime = std.ToString("HHmm");

                        string query = $"SELECT TOP 1 COACH_SEQUENCE FROM TRAINUPDATEDDATA WHERE TRAIN_NUMBER = '{item.trainNo}'";
                        string coachSequence = _dbService.ExecuteScalar(query);
                        if (string.IsNullOrWhiteSpace(coachSequence))
                            coachSequence = item.arrivalCoachClass != null ? string.Join(",", item.arrivalCoachClass) : "";

                        list.Add(new TrainMaster
                        {
                            TRAIN_NUMBER = item.trainNo ?? "",
                            TRAIN_NAME_ENGLISH = item.trainName ?? "",
                            ORIGIN_CODE = item.src ?? "",
                            DESTINATION_CODE = item.dstn ?? "",
                            VIA1_CODE = string.Empty,
                            VIA2_CODE = string.Empty,
                            VIA3_CODE = string.Empty,
                            VIA4_CODE = string.Empty,
                            ARRIVAL_TIME = arrivalTime,
                            DEPARTURE_TIME = departureTime,
                            PLATFORM_NUMBER = item.platformNo ?? "",
                            PF_END = string.Empty,
                            TRAIN_TYPE = string.Empty,
                            TRAIN_CATEOGARY = item.trainTypeName ?? "",
                            STATUS = string.Empty,
                            COACH_SEQUENCE = coachSequence,
                            RUNNING_DAYS = (item.daysOfArrival != null && item.daysOfArrival.ToString().ToUpper() == "DAILY") ? "1234567" : "",
                            REGIONAL_BITMAP = string.Empty,
                            TRAIN_SORT_TIME = sortTime
                        });
                    }
                    return list;
                });

                if (trains.Count == 0)
                {
                    MessageBox.Show("No train data found to import.", "Info",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                _dbService.SaveOrUpdateTrainMasterBulk(trains);
                LoggerService.SettingsLogger.Information("Train master import from NTES: {Count} trains saved/updated.", trains.Count);
                LoadTrainMaster();
                MessageBox.Show("All trains imported successfully!!", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                LoggerService.NtesLogger.Error(ex, "NTES GetMasterTrains failed. Station: {Station}", "Error");
                return;

            }
            finally
            {
                //btnImportJson.Enabled = true;
                Cursor = Cursors.Default;
            }
        }
        private void btnClearCoach_Click(object sender, EventArgs e)
        {
            foreach (TableLayoutPanel inner in tableLayoutPanel8.Controls.OfType<TableLayoutPanel>())
            {
                foreach (System.Windows.Forms.TextBox txt in inner.Controls.OfType<System.Windows.Forms.TextBox>())
                {
                    txt.Clear();

                }
            }
            lblCoachSummary.Text = string.Empty;
        }
        private string BuildCoachLabelText()
        {
            var sb = new System.Text.StringBuilder();
            TableLayoutPanel innerTable = tableLayoutPanel8.Controls
                .OfType<TableLayoutPanel>()
                .FirstOrDefault(t => t != null && tableLayoutPanel8.GetRow(t) == 1);

            if (innerTable == null)
                return string.Empty;
            var textBoxes = innerTable.Controls
                .OfType<System.Windows.Forms.TextBox>()
                .Select(tb => new
                {
                    Control = tb,
                    Position = innerTable.GetPositionFromControl(tb)
                })
                .OrderBy(p => p.Position.Row)
                .ThenBy(p => p.Position.Column)
                .Select(p => p.Control)
                .ToList();

            for (int i = 0; i < textBoxes.Count; i++)
            {
                if (i > 0)
                    sb.Append(" -- ");
                sb.Append(textBoxes[i].Text.Trim());
            }

            return sb.ToString();
        }
        private void btnReverse_Click(object sender, EventArgs e)
        {
            try
            {
                var coachTextBoxes = tableLayoutPanel8.Controls
               .OfType<TableLayoutPanel>()
               .SelectMany(tlp => tlp.Controls.OfType<System.Windows.Forms.TextBox>())
               .OrderBy(tb =>
               {
                   string numPart = new string(tb.Name
                       .Where(char.IsDigit)
                       .ToArray());
                   return int.TryParse(numPart, out int n) ? n : 0;
               }).ToList();

                if (coachTextBoxes.Count == 0)
                {
                    MessageBox.Show("Coach data not available!!", "Info",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                List<string> filledValues = coachTextBoxes
                    .Select(tb => tb.Text.Trim())
                    .ToList();
                filledValues.Reverse();

                for (int i = 0; i < coachTextBoxes.Count; i++)
                    coachTextBoxes[i].Text = filledValues[i];
                lblCoachSummary.Text = BuildCoachLabelText();
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
        }
        private void btnCoachSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(lblCoachTrainHeader.Text))
                {
                    MessageBox.Show("Train details are missing!", "VARDAAN ELECTRONICS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string[] headerParts = lblCoachTrainHeader.Text.Trim().Split(' ');
                if (headerParts.Length == 0 || string.IsNullOrWhiteSpace(headerParts[0]))
                {
                    MessageBox.Show("Invalid Train Number!", "VARDAAN ELECTRONICS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string _COACH_SEQUENCE = BuildCoachLabelText();
                _COACH_SEQUENCE = _COACH_SEQUENCE.Replace("--", "-").Replace(" ", "");
                string query = "UPDATE TRAINMASTER SET COACH_SEQUENCE='" + _COACH_SEQUENCE + "' WHERE TRAIN_NUMBER='" + headerParts[0] + "'";
                string result = _dbService.ExecuteScalar(query);
                LoggerService.SettingsLogger.Information("Train master update: coach sequence of {TrainNumber} set to {CoachSequence}.", headerParts[0], _COACH_SEQUENCE);
                lblCoachSummary.Text = BuildCoachLabelText();
                MessageBox.Show("Coach sequence updated!!", "VARDAAN ELECTRONICS", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                LoggerService.SettingsLogger.Error(ex, ex.Message);
            }
        }
        private void btn_Add_Click(object sender, EventArgs e)
        {
            TableLayoutPanel innerTable = tableLayoutPanel8.Controls
                                           .OfType<TableLayoutPanel>()
                                           .FirstOrDefault(t => tableLayoutPanel8.GetRow(t) == 1);

            if (innerTable == null)
            {
                innerTable = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    AutoSize = true,
                    AutoScroll = true,
                    ColumnCount = _coachcolCount,
                    RowCount = 1,
                    Margin = new Padding(5)
                };
                for (int c = 0; c < _coachcolCount; c++)
                    innerTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5f));

                innerTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                tableLayoutPanel8.Controls.Add(innerTable, 0, 1);
                tableLayoutPanel8.SetColumnSpan(innerTable, tableLayoutPanel8.ColumnCount);
            }
            int totalTextBoxes = innerTable.Controls.OfType<System.Windows.Forms.TextBox>().Count();
            int row = totalTextBoxes / _coachcolCount;
            int col = totalTextBoxes % _coachcolCount;
            System.Windows.Forms.TextBox txt = new System.Windows.Forms.TextBox
            {
                Name = $"txtCoach{totalTextBoxes + 1}",
                Text = "",
                Dock = DockStyle.Top,
                Margin = new Padding(3),
                TextAlign = HorizontalAlignment.Center
            };
            if (row >= innerTable.RowCount)
            {
                innerTable.RowCount += 1;
                innerTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }
            innerTable.Controls.Add(txt, col, row);
            lblCoachSummary.Text = BuildCoachLabelText();
        }
        private void btn_Remove_Click(object sender, EventArgs e)
        {
            TableLayoutPanel innerTable = tableLayoutPanel8.Controls
                .OfType<TableLayoutPanel>()
                .FirstOrDefault(t => tableLayoutPanel8.GetRow(t) == 1);

            if (innerTable == null) return;

            Control focused = innerTable.Controls.Cast<Control>()
                                   .FirstOrDefault(c => c.Focused);

            if (focused == null || focused is not System.Windows.Forms.TextBox)
            {
                MessageBox.Show("select coach to delete!!", "VARDAAN ELECTRONICS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            innerTable.Controls.Remove(focused);
            focused.Dispose();

            var allTxt = innerTable.Controls.OfType<System.Windows.Forms.TextBox>()
                                    .OrderBy(t => t.TabIndex)
                                    .ToList();
            innerTable.Controls.Clear();
            innerTable.RowCount = 1;

            for (int i = 0; i < allTxt.Count; i++)
            {
                int row = i / _coachcolCount;
                int col = i % _coachcolCount;
                allTxt[i].Name = $"txtCoach{i + 1}";
                if (row >= innerTable.RowCount)
                {
                    innerTable.RowCount++;
                    innerTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                }
                innerTable.Controls.Add(allTxt[i], col, row);
            }
            lblCoachSummary.Text = BuildCoachLabelText();
        }
        private void BrowseSoundFile(TextBox txt)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select Sound File";
                ofd.Filter = "Audio Files|*.wav;*.mp3|All Files|*.*";
                ofd.Multiselect = false;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    
                    txt.Text = ofd.FileName;
                }
            }
        }

        private void btnSource_Click(object sender, EventArgs e)
        {
            List<StationList> stations = _dbService.GET_ALL_STATIONS();
            using (var dialog = new StationListDialog(stations, targetTextBox: textBoxSource))
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(dialog.SelectedStationCode))
                    {
                        textBoxSource.Text = dialog.SelectedStationCode;
                    }
                }
            }
        }

        private void btnDestination_Click(object sender, EventArgs e)
        {
            List<StationList> stations = _dbService.GET_ALL_STATIONS();
            using (var dialog = new StationListDialog(stations, targetTextBox: textBoxdestination))
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(dialog.SelectedStationCode))
                    {
                        textBoxdestination.Text = dialog.SelectedStationCode;
                    }
                }
            }
        }

        private void btnVia1_Click(object sender, EventArgs e)
        {
            List<StationList> stations = _dbService.GET_ALL_STATIONS();
            using (var dialog = new StationListDialog(stations, targetTextBox: textBoxVia1))
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(dialog.SelectedStationCode))
                    {
                        textBoxVia1.Text = dialog.SelectedStationCode;
                    }
                }
            }
        }

        private void btnVia2_Click(object sender, EventArgs e)
        {
            List<StationList> stations = _dbService.GET_ALL_STATIONS();
            using (var dialog = new StationListDialog(stations, targetTextBox: textBoxVia2))
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(dialog.SelectedStationCode))
                    {
                        textBoxVia2.Text = dialog.SelectedStationCode;
                    }
                }
            }
        }

        private void btnVia3_Click(object sender, EventArgs e)
        {
            List<StationList> stations = _dbService.GET_ALL_STATIONS();
            using (var dialog = new StationListDialog(stations, targetTextBox: textBoxVia3))
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(dialog.SelectedStationCode))
                    {
                        textBoxVia3.Text = dialog.SelectedStationCode;
                    }
                }
            }
        }

        private void btnVia4_Click(object sender, EventArgs e)
        {
            List<StationList> stations = _dbService.GET_ALL_STATIONS();
            using (var dialog = new StationListDialog(stations, targetTextBox: textBoxVia4))
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(dialog.SelectedStationCode))
                    {
                        textBoxVia4.Text = dialog.SelectedStationCode;
                    }
                }
            }
        }

        private void LoadDropdownData()
        {
            try
            {
                // Load Train Types
                string trainTypeQuery = "SELECT TrainType FROM TrainType WHERE IsActive = 1";
                DataTable dtTrainTypes = _dbService.GetTable(trainTypeQuery);

                combBoxTrainType.DataSource = dtTrainTypes;
                combBoxTrainType.DisplayMember = "TrainType";
                combBoxTrainType.ValueMember = "TrainType";
                combBoxTrainType.SelectedIndex = 0;

                // Load Train Categories
                string categoryQuery = "SELECT CATEGORY_NAME FROM TRAIN_CATEGORY WHERE IS_ACTIVE = 1";
                DataTable dtCategories = _dbService.GetTable(categoryQuery);

                comboBoxTrainCategory.DataSource = dtCategories;
                comboBoxTrainCategory.DisplayMember = "CATEGORY_NAME";
                comboBoxTrainCategory.ValueMember = "CATEGORY_NAME";
                comboBoxTrainCategory.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
        }
    }
}