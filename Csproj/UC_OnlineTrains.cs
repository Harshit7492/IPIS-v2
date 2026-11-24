using DataBaseHandler;
using DataModel;
using Global_Data;
using Helper;
using Ipis.Snmp;
using IPISv2.Controls;
using LogManager;
using Newtonsoft.Json;
using PacketGeneration;
using System.Data;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Xml;

namespace IPIS
{
    public partial class UC_OnlineTrains : UserControl
    {
        private System.Windows.Forms.Timer _timerNTES;
        private System.Windows.Forms.Timer _timerCAP;
        private IpisTrapSender _trapSender;
        private System.Windows.Forms.Timer _timerAutoAnnounce;
        private bool _isAutoAnnounceRunning = false;
        string imagePathgifusionwave = string.Empty;
        public bool _isNtesRunning = false;
        private static Image _deleteIcon;
        private readonly Font _coachFont = new Font("Segoe UI", 9F, FontStyle.Bold);
        private readonly Font _coachFont10 = new Font("Segoe UI", 9F, FontStyle.Bold);
        public bool _istimerCAPRunning = false;
        //private bool _isCgdbProcessing = false; 
        private string _selectedTrainNumber = string.Empty;   // replaces _previousSelectedRow: track selection by train number, not by row reference
        //private bool _isTadbProcessing = false;
        private string _currentCoachTrainNumber = string.Empty;
        private readonly Dictionary<string, int> _pendingTadbChanges = new();
        private readonly Dictionary<string, int> _pendingCGDBChanges = new();
        int _columnCount = 14;
        private int highlightedRowIndex = -1;
        //private int previousRow = -1;

        // CUSTOM LOADER FIELDS
        private Panel _loaderPanel;
        private PictureBox _loaderPictureBox;
        private System.Windows.Forms.Timer _loaderAnimationTimer;
        private int _loaderAngle = 0;
        private bool _isLoaderVisible = false;
        private Color _loaderColor = Color.FromArgb(52, 152, 219);
        private readonly Dictionary<string, bool> _pendingRowState = new();
        private System.Windows.Forms.TextBox lastFocusedTextBox = null;
        private List<RemovedItem> removedFromLeft = new();
        private List<RemovedItem> removedFromRight = new();
        private List<BoardInfo> lstboardInfo = new List<BoardInfo>();
        private CapInfo _capInfo;
        private List<trainstatus> _lststatus = new List<trainstatus>();
        string query = string.Empty;
        private AnnouncementManager _manager = new AnnouncementManager();
        public event EventHandler<eventInfo> OnEvent;
        List<train_updated_data> lsttrain = new List<train_updated_data>();
        private List<Plateforms> lstPlateforms = new List<Plateforms>();
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private string _previousStatus = string.Empty;
        public event EventHandler OnInitialLoadComplete;
        public bool isBackwardIntigration = false;

        // FIXED COACH LAYOUT
        private int TOTAL_COACH_BOXES = GlobalData._lstAdminSetting.Total_Coach;
        public UC_OnlineTrains()
        {
            InitializeComponent();

            CreateCustomLoader();

            this.DoubleBuffered = true;
            dgvOnlineTrains.CellClick += dgvOnlineTrains_CellClick;
            //dgvOnlineTrains.CellDoubleClick += dgvOnlineTrains_CellDoubleClick;
            dgvOnlineTrains.EditingControlShowing += dgvOnlineTrains_EditingControlShowing;
            dgvOnlineTrains.CellBeginEdit += dgvOnlineTrains_CellBeginEdit;
            dgvOnlineTrains.Dock = DockStyle.Fill;
            dgvOnlineTrains.DataBindingComplete += dgvOnlineTrains_DataBindingComplete;
            //dgvOnlineTrains.CellMouseEnter += dgvOnlineTrains_CellMouseEnter;
            //dgvOnlineTrains.MouseLeave += dgvOnlineTrains_MouseLeave;
            dgvOnlineTrains.DefaultCellStyle.SelectionBackColor = dgvOnlineTrains.DefaultCellStyle.BackColor;
            dgvOnlineTrains.DefaultCellStyle.SelectionForeColor = Color.Black;

            dgvOnlineTrains.AutoGenerateColumns = false;
            dgvOnlineTrains.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvOnlineTrains.AllowUserToResizeColumns = false;
            dgvOnlineTrains.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvOnlineTrains.RowHeadersVisible = false;
            dgvOnlineTrains.ScrollBars = ScrollBars.Both;
            dgvOnlineTrains.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            cmb_repeat.SelectedIndex = 0;

            this.Disposed += (s, e) =>
            {
                _coachFont?.Dispose();
                _coachFont10?.Dispose();
            };
        }
        private void UC_OnlineTrains_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                ApplyAccessRights();
                LoadOnlineTrains();
                trackBarVolume.Minimum = 0;
                trackBarVolume.Maximum = 100;
                trackBarVolume.Value = 80;
                trackBarVolume.TickFrequency = 10;
                MediaManager.Instance.SetVolume(80);
                SetMediaButtonsState("IDLE");
                ApplySystemLockState();
                imagePathgifusionwave = AppHelper.GetImagePath("gifusionwave.gif");
                pictureboxwave.Image = Image.FromFile(imagePathgifusionwave);
                pictureboxwave.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureboxwave.Visible = false;
                if (GlobalData._lstAdminSetting.IsNtesAutoUpdate == true)
                    StartOrUpdateTimer_AUTO_NTES(GlobalData._lstAdminSetting.NtesTimeMin);

                AutoStart_CAP();

                if (GlobalData._lstAdminSetting.Auto_Announce == 1)
                {
                    StartAutoAnnouncementTimer();
                }
            }
        }
        public async void LoadOnlineTrains()
        {
            if (dgvOnlineTrains.InvokeRequired)
            {
                dgvOnlineTrains.BeginInvoke(new Action(LoadOnlineTrains));
                return;
            }
            try
            {
                var data = await Task.Run(() =>
                {
                    var trains = _dbService.GET_UPDATED_TRAINS_DATA(0);
                    var statuses = _dbService.Train_Status_list("SELECT DISTINCT STATUS_CODE,STATUS_ENGLISH FROM TRAINSTATUS WHERE STATUS=1");
                    var statusList = _dbService.QueryList("SELECT DISTINCT STATUS_ENGLISH FROM TRAINSTATUS WHERE STATUS=1");
                    var platforms = _dbService.GET_PLATEFORMS(0);

                    if (!statusList.Contains(""))
                        statusList.Insert(0, "");

                    for (int i = 0; i < trains.Count; i++)
                    {
                        trains[i].SR_NO = i + 1;
                        CalculateActualTimes(trains[i]);
                        trains[i].ACTUAL_PLATFORM_NUMBER = trains[i].ACTUAL_PLATFORM_NUMBER?.Trim() ?? "";
                        trains[i].TRAIN_STATUS_ENGLISH = trains[i].TRAIN_STATUS_ENGLISH?.Trim() ?? "";
                        if (!string.IsNullOrEmpty(trains[i].TRAIN_STATUS_ENGLISH) && !statusList.Contains(trains[i].TRAIN_STATUS_ENGLISH))
                        {
                            statusList.Add(trains[i].TRAIN_STATUS_ENGLISH);
                        }
                    }

                    string firstCoachSeq = null;
                    if (trains.Count > 0)
                    {
                        string trNo = trains.FirstOrDefault().TRAIN_NUMBER;

                        firstCoachSeq = _dbService.GetCoachSequence(trNo);
                    }

                    return new { trains, statuses, statusList, platforms, firstCoachSeq };
                });

                if (dgvOnlineTrains.IsDisposed) return;

                lsttrain = data.trains;
                _lststatus = data.statuses;
                lstPlateforms = data.platforms;

                dgvOnlineTrains.SuspendLayout();

                dgvOnlineTrains.Columns.Clear();
                dgvOnlineTrains.AutoGenerateColumns = false;
                dgvOnlineTrains.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "S.NO",
                    HeaderText = "SN",
                    DataPropertyName = "SR_NO",
                    ReadOnly = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                });
                dgvOnlineTrains.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "TRAIN",
                    HeaderText = "TRAIN NO",
                    DataPropertyName = "TRAIN_NUMBER",
                    ReadOnly = true,
                    ToolTipText = "Train Number",
                    //AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                    DefaultCellStyle = new DataGridViewCellStyle
                    {
                        Alignment = DataGridViewContentAlignment.MiddleLeft
                    }
                });
                dgvOnlineTrains.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "TRAIN_NAME",
                    HeaderText = "TRAIN NAME",
                    DataPropertyName = "TRAIN_NAME_ENGLISH",
                    ReadOnly = true,
                    ToolTipText = "Train Name In English",
                    Width = 200,
                    DefaultCellStyle = new DataGridViewCellStyle
                    {
                        Alignment = DataGridViewContentAlignment.MiddleLeft
                    }
                });

                var statusColumn = new DataGridViewComboBoxColumn
                {
                    Name = "STATUS",
                    HeaderText = "STATUS",
                    DataPropertyName = "TRAIN_STATUS_ENGLISH",
                    DataSource = data.statusList,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    FlatStyle = FlatStyle.Flat,
                    DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton,
                    DefaultCellStyle = new DataGridViewCellStyle { NullValue = "" }
                };
                dgvOnlineTrains.Columns.Add(statusColumn);
                dgvOnlineTrains.Columns.Add(new DataGridViewMaskedTextBoxColumn
                {
                    Name = "SAT",
                    HeaderText = "SAT",
                    DataPropertyName = "SCHEDULE_ARRIVAL_TIME",
                    ReadOnly = true,
                    ToolTipText = "Station Arrival Time",
                    Mask = "00:00",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                });
                dgvOnlineTrains.Columns.Add(new DataGridViewMaskedTextBoxColumn
                {
                    Name = "SDT",
                    HeaderText = "SDT",
                    DataPropertyName = "SCHEDULE_DEPARTURE_TIME",
                    ReadOnly = true,
                    ToolTipText = "Station Departure Time",
                    Mask = "00:00",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                });
                dgvOnlineTrains.Columns.Add(new DataGridViewMaskedTextBoxColumn
                {
                    Name = "DELAY",
                    HeaderText = "DELAY",
                    DataPropertyName = "LATE",
                    Mask = "00:00",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                });
                dgvOnlineTrains.Columns.Add(new DataGridViewMaskedTextBoxColumn
                {
                    Name = "EAT",
                    HeaderText = "EAT",
                    DataPropertyName = "ACTUAL_ARRIVAL_TIME",
                    ReadOnly = true,
                    ToolTipText = "Estimated Arrival Time",
                    Mask = "00:00",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                });
                dgvOnlineTrains.Columns.Add(new DataGridViewMaskedTextBoxColumn
                {
                    Name = "EDT",
                    HeaderText = "EDT",
                    DataPropertyName = "ACTUAL_DEPARTURE_TIME",
                    ReadOnly = true,
                    ToolTipText = "Estimated Departure Time",
                    Mask = "00:00",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                });
                var adButtonColumn = new DataGridViewButtonColumn
                {
                    Name = "AD",
                    HeaderText = "A/D",
                    DataPropertyName = "TRAIN_STATUS_CODE",
                    UseColumnTextForButtonValue = false,
                    ReadOnly = true,
                    ToolTipText = "Arrival/Departure",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                };
                dgvOnlineTrains.Columns.Add(adButtonColumn);

                var pfColumn = new DataGridViewComboBoxColumn
                {
                    Name = "PFNO",
                    HeaderText = "PFNO",
                    DataPropertyName = "ACTUAL_PLATFORM_NUMBER",
                    ToolTipText = "Plateform Number",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                    DefaultCellStyle = new DataGridViewCellStyle { NullValue = "" }
                };

                pfColumn.Items.Add("");

                if (lstPlateforms != null)
                {
                    int maxPf = GlobalData._lstAdminSetting != null && GlobalData._lstAdminSetting.TOTAL_PLATFORM_DISPLAYED > 0
                        ? Math.Min(lstPlateforms.Count, GlobalData._lstAdminSetting.TOTAL_PLATFORM_DISPLAYED)
                        : lstPlateforms.Count;

                    for (int pfcount = 0; pfcount < maxPf; pfcount++)
                    {
                        string pfNum = lstPlateforms[pfcount].PF_NUMBER?.Trim();
                        if (!string.IsNullOrEmpty(pfNum) && !pfColumn.Items.Contains(pfNum))
                            pfColumn.Items.Add(pfNum);
                    }
                }

                if (lsttrain != null)
                {
                    foreach (var tr in lsttrain)
                    {
                        string trPf = tr.ACTUAL_PLATFORM_NUMBER?.Trim();
                        if (!string.IsNullOrEmpty(trPf) && !pfColumn.Items.Contains(trPf))
                            pfColumn.Items.Add(trPf);
                    }
                }

                dgvOnlineTrains.Columns.Add(pfColumn);
                var tdColumn = new DataGridViewCheckBoxColumn
                {
                    Name = "TD",
                    HeaderText = "TD",
                    DataPropertyName = "TADB_SEND_DATA",
                    ToolTipText = "",
                    TrueValue = 1,
                    ReadOnly = true,
                    FalseValue = 0,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                };
                dgvOnlineTrains.Columns.Add(tdColumn);
                var cgColumn = new DataGridViewCheckBoxColumn
                {
                    Name = "CG",
                    HeaderText = "CG",
                    DataPropertyName = "CGDB_SEND_DATA",
                    ToolTipText = "Coach Guidence Display Board",
                    TrueValue = 1,
                    ReadOnly = true,
                    FalseValue = 0,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                };
                dgvOnlineTrains.Columns.Add(cgColumn);
                var anncColumn = new DataGridViewCheckBoxColumn
                {
                    Name = "AN",
                    HeaderText = "AN",
                    DataPropertyName = "ANNOUNCEMENT",
                    ToolTipText = "Announcement",
                    TrueValue = 1,
                    ReadOnly = true,          // <-- add
                    FalseValue = 0,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                };
                dgvOnlineTrains.Columns.Add(anncColumn);
                DataGridViewImageColumn deleteColumn = new DataGridViewImageColumn
                {
                    HeaderText = "[]",
                    Name = "Delete",
                    ToolTipText = "Delete",
                    Width = 25,
                    ImageLayout = DataGridViewImageCellLayout.Zoom,
                };
                if (_deleteIcon == null)
                {
                    string imagePath = AppHelper.GetImagePath("delete-button.png");
                    // Load via stream so the file handle is released immediately
                    using (var fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                    {
                        _deleteIcon = Image.FromStream(fs);
                    }
                }
                deleteColumn.Image = _deleteIcon;
                dgvOnlineTrains.Columns.Add(deleteColumn);

                dgvOnlineTrains.DataSource = lsttrain;
                dgvOnlineTrains.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvOnlineTrains.ReadOnly = false;
                if (dgvOnlineTrains.Columns.Contains("TRAIN"))
                    dgvOnlineTrains.Columns["TRAIN"].ReadOnly = true;

                if (lsttrain.Count > 0)
                    ApplyCoachDataUI(lsttrain.FirstOrDefault().TRAIN_NUMBER, data.firstCoachSeq, lsttrain.FirstOrDefault().TRAIN_NAME_ENGLISH, lsttrain.FirstOrDefault().trainReversalFlag);

                dgvOnlineTrains.ResumeLayout();
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
            finally
            {
                OnInitialLoadComplete?.Invoke(this, EventArgs.Empty);
            }
        }
        private void dgvOnlineTrains_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            ApplyTrainRowColors();
        }
        private void dgvOnlineTrains_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        private void dgvOnlineTrains_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            string colName = dgvOnlineTrains.Columns[e.ColumnIndex].Name;
            if (colName == "PFNO" || colName == "STATUS")
            {
                if (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString()))
                {
                    e.Value = "";
                    e.FormattingApplied = true;
                }
            }
        }
        private void CalculateActualTimes(train_updated_data train)
        {
            try
            {

                string oldActualArrival = train.ACTUAL_ARRIVAL_TIME;
                string oldActualDeparture = train.ACTUAL_DEPARTURE_TIME;

                if (string.IsNullOrWhiteSpace(train.LATE))
                {
                    train.ACTUAL_ARRIVAL_TIME = train.SCHEDULE_ARRIVAL_TIME;
                    train.ACTUAL_DEPARTURE_TIME = train.SCHEDULE_DEPARTURE_TIME;
                }
                else if (TimeSpan.TryParse(train.LATE, out TimeSpan delay))
                {

                    if (TimeSpan.TryParse(train.SCHEDULE_ARRIVAL_TIME, out TimeSpan sat))
                    {
                        train.ACTUAL_ARRIVAL_TIME = sat.Add(delay).ToString(@"hh\:mm");
                    }
                    else
                    {

                        train.ACTUAL_ARRIVAL_TIME = train.SCHEDULE_ARRIVAL_TIME;
                    }


                    if (TimeSpan.TryParse(train.SCHEDULE_DEPARTURE_TIME, out TimeSpan sdt))
                    {
                        train.ACTUAL_DEPARTURE_TIME = sdt.Add(delay).ToString(@"hh\:mm");
                    }
                    else
                    {

                        train.ACTUAL_DEPARTURE_TIME = train.SCHEDULE_DEPARTURE_TIME;
                    }
                }

                if (train.ACTUAL_ARRIVAL_TIME != oldActualArrival ||
                    train.ACTUAL_DEPARTURE_TIME != oldActualDeparture)
                {
                    _dbService.UpdateTrainOnlineDgv(train);
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
        }
        private void dgvOnlineTrains_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvOnlineTrains.Rows[e.RowIndex].DataBoundItem is train_updated_data train)
            {
                string colName = dgvOnlineTrains.Columns[e.ColumnIndex].Name;

                if (colName == "DELAY")
                {
                    CalculateActualTimes(train);

                    dgvOnlineTrains.Refresh();
                }

                string[] excludedCols = { "AD", "TD", "CG", "AN", "Delete", "TRAIN" };

                bool isDivertedStatus = false;
                bool isMergedStatus = false;
                if (colName == "STATUS")
                {
                    var statusValue = dgvOnlineTrains.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
                    if (!string.IsNullOrEmpty(statusValue) && (statusValue.ToUpper().Contains("DIVERTED") || statusValue.ToUpper().Contains("TERMINATED") || statusValue.ToUpper().Contains("CHANGE OF SOURCE")))
                    {
                        isDivertedStatus = true;
                    }
                    if (!string.IsNullOrEmpty(statusValue) && statusValue.ToUpper().Contains("MERGE"))
                    {
                        isMergedStatus = true;
                    }
                }

                if (!excludedCols.Contains(colName) && !isDivertedStatus && !isMergedStatus)
                {
                    _dbService.UpdateTrainOnlineDgv(train);
                }

                if (colName == "STATUS")
                {
                    var statusValue = dgvOnlineTrains.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
                    if (!string.IsNullOrEmpty(statusValue) && (statusValue.ToUpper().Contains("DIVERTED") || statusValue.ToUpper().Contains("TERMINATED") || statusValue.ToUpper().Contains("CHANGE OF SOURCE")))
                    {
                        bool isSaved = OpenTrainSearchDialog(statusValue, train.TRAIN_NUMBER);
                        if (isSaved)
                        {
                            _dbService.UpdateTrainOnlineDgv(train);
                        }
                        else
                        {
                            train.TRAIN_STATUS_ENGLISH = _previousStatus;
                        }
                        this.BeginInvoke(new Action(() => LoadOnlineTrains()));
                    }
                    else if (!string.IsNullOrEmpty(statusValue) && statusValue.ToUpper().Contains("MERGE"))
                    {
                        bool isSaved = OpenTrainSearchDialog(statusValue, train.TRAIN_NUMBER);
                        if (isSaved)
                        {
                            _dbService.UpdateTrainOnlineDgv(train);
                        }
                        else
                        {
                            train.TRAIN_STATUS_ENGLISH = _previousStatus;
                        }
                        this.BeginInvoke(new Action(() => LoadOnlineTrains()));
                    }
                }
            }

        }
        private void dgvOnlineTrains_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dgvOnlineTrains.CurrentCell.ColumnIndex >= 0 &&
                dgvOnlineTrains.Columns[dgvOnlineTrains.CurrentCell.ColumnIndex] is DataGridViewComboBoxColumn)
            {
                if (e.Control is System.Windows.Forms.ComboBox cb)
                {
                    cb.SelectionChangeCommitted -= ComboBox_SelectionChangeCommitted;
                    cb.SelectionChangeCommitted += ComboBox_SelectionChangeCommitted;
                }
            }
            if (dgvOnlineTrains.CurrentCell.ColumnIndex == dgvOnlineTrains.Columns["STATUS"].Index)
            {
                var combo = e.Control as System.Windows.Forms.ComboBox;
                if (combo != null)
                {
                    var row = dgvOnlineTrains.CurrentRow?.DataBoundItem as train_updated_data;
                    if (row != null)
                    {
                        combo.DataSource = _lststatus.Where(x => x.STATUS_CODE == row.TRAIN_STATUS_CODE)
                                            .Select(x => x.STATUS_ENGLISH).ToList();
                    }

                }
            }
        }
        private void ApplyTrainRowColors()
        {
            Color highlightColor = Color.FromArgb(221, 160, 221);
            Color selectedColor = Color.FromArgb(230, 245, 255);

            foreach (DataGridViewRow row in dgvOnlineTrains.Rows)
            {
                if (row.IsNewRow) continue;
                var train = row.DataBoundItem as train_updated_data;
                if (train == null) continue;

                bool isGreen =
                    _pendingRowState.TryGetValue(train.TRAIN_NUMBER, out var pending)
                        ? pending
                        : (train.TADB_SEND_DATA == 1 || train.CGDB_SEND_DATA == 1);

                bool isSelected = train.TRAIN_NUMBER == _selectedTrainNumber;

                // Send-state colour wins over selection colour; a white row shows selection.
                Color back = isGreen ? highlightColor
                           : isSelected ? selectedColor
                           : Color.White;

             
                DataGridViewCellStyle style = row.DefaultCellStyle;

                // Only assign when the value actually changes — avoids needless invalidation.
                if (style.BackColor != back)
                {
                    style.BackColor = back;
                    style.SelectionBackColor = back;
                    style.ForeColor = Color.Black;
                    style.SelectionForeColor = Color.Black;
                }
            }
        }
        private void dgvOnlineTrains_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ignore header clicks and clicks that arrive while the grid is being reloaded
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (e.RowIndex >= dgvOnlineTrains.Rows.Count || e.ColumnIndex >= dgvOnlineTrains.Columns.Count) return;

            // Row may not be bound yet (grid rebuilding) -> nothing to do
            if (!(dgvOnlineTrains.Rows[e.RowIndex].DataBoundItem is train_updated_data trainData)) return;

            string trainNumber = trainData.TRAIN_NUMBER;
            if (string.IsNullOrWhiteSpace(trainNumber)) return;
            if (dgvOnlineTrains.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn)
            {
                string colName = dgvOnlineTrains.Columns[e.ColumnIndex].Name;
                if (colName == "STATUS" || colName == "PFNO")
                {
                    object currentValue = dgvOnlineTrains.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                    dgvOnlineTrains.CurrentCell = dgvOnlineTrains.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    dgvOnlineTrains.BeginEdit(true);

                    if (dgvOnlineTrains.EditingControl is System.Windows.Forms.ComboBox cb)
                    {
                        if (currentValue != null && currentValue != DBNull.Value && !string.IsNullOrWhiteSpace(currentValue.ToString()))
                        {
                            cb.SelectedItem = currentValue;

                            if (cb.SelectedItem == null)
                            {
                                for (int i = 0; i < cb.Items.Count; i++)
                                {
                                    if (cb.Items[i].ToString() == currentValue.ToString())
                                    {
                                        cb.SelectedIndex = i;
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            cb.SelectedIndex = cb.Items.Contains("") ? cb.Items.IndexOf("") : -1;
                        }

                        cb.DroppedDown = true;
                    }
                }
                return;
            }



            if (dgvOnlineTrains.Columns[e.ColumnIndex].Name == "AD")
            {
                try
                {
                    if (e.RowIndex >= 0 && e.ColumnIndex == dgvOnlineTrains.Columns["AD"].Index)
                    {
                        var cell = dgvOnlineTrains.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewButtonCell;

                        string currentValue = cell.Value?.ToString();

                        if (currentValue == "A")
                        {
                            cell.Value = "D";
                            query = "UPDATE TRAINUPDATEDDATA SET TRAIN_STATUS_CODE='D' WHERE TRAIN_NUMBER='" + trainNumber + "'";
                        }
                        else
                        {
                            cell.Value = "A";
                            query = "UPDATE TRAINUPDATEDDATA SET TRAIN_STATUS_CODE='A' WHERE TRAIN_NUMBER='" + trainNumber + "'";
                        }
                        string result = _dbService.ExecuteScalar(query);
                    }
                }
                catch (Exception ex)
                {
                    LoggerService.Logger.Error(ex, ex.Message);
                }
            }
            else if (dgvOnlineTrains.Columns[e.ColumnIndex].Name == "TD")
            {
                try
                {
                    var train = dgvOnlineTrains.Rows[e.RowIndex].DataBoundItem as train_updated_data;
                    int currentValue = 0;
                    var cellValue = dgvOnlineTrains.Rows[e.RowIndex].Cells["TD"].Value;
                    if (cellValue != DBNull.Value && cellValue != null)
                        currentValue = Convert.ToInt32(cellValue);

                    int newValue = (currentValue == 1) ? 0 : 1;

                    if (!_pendingRowState.ContainsKey(trainNumber))
                    {
                        bool wasGreen = train != null && (train.TADB_SEND_DATA == 1 || train.CGDB_SEND_DATA == 1);
                        _pendingRowState.Add(trainNumber, wasGreen);
                    }

                    dgvOnlineTrains.Rows[e.RowIndex].Cells["TD"].Value = newValue;
                    if (train != null) train.TADB_SEND_DATA = newValue;

                    if (newValue == 0)
                        dgvOnlineTrains.Rows[e.RowIndex].Cells["AN"].Value = 0; // just UI, no DB call

                    dgvOnlineTrains.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    dgvOnlineTrains.EndEdit();

                    // track for later persistence instead of writing to DB now
                    _pendingTadbChanges[trainNumber] = newValue;
                }
                catch (Exception ex)
                {
                    LoggerService.Logger.Error(ex, ex.Message);
                }
            }
            else if (dgvOnlineTrains.Columns[e.ColumnIndex].Name == "CG")
            {
                try
                {
                    int currentValue = 0;
                    int newValue = 0;
                    var cellValue = dgvOnlineTrains.Rows[e.RowIndex].Cells["CG"].Value;
                    if (cellValue != DBNull.Value && cellValue != null)
                        currentValue = Convert.ToInt32(cellValue);
                    newValue = (currentValue == 1) ? 0 : 1;

                    if (!_pendingRowState.ContainsKey(trainNumber))
                    {
                        bool wasGreen = trainData != null && (trainData.TADB_SEND_DATA == 1 || trainData.CGDB_SEND_DATA == 1);
                        _pendingRowState.Add(trainNumber, wasGreen);
                    }

                    dgvOnlineTrains.Rows[e.RowIndex].Cells["CG"].Value = newValue;
                    if (trainData != null)
                    {
                        trainData.CGDB_SEND_DATA = newValue;
                    }

                    dgvOnlineTrains.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    dgvOnlineTrains.EndEdit();

                    _pendingCGDBChanges[trainNumber] = newValue;
                }
                catch (Exception ex)
                {
                    LoggerService.Logger.Error(ex, ex.Message);
                }
            }
            else if (dgvOnlineTrains.Columns[e.ColumnIndex].Name == "AN")
            {
                try
                {
                    var tdValue = dgvOnlineTrains.Rows[e.RowIndex].Cells["TD"].Value;
                    int tdChecked = (tdValue != DBNull.Value && tdValue != null) ? Convert.ToInt32(tdValue) : 0;

                    if (tdChecked == 0)
                    {
                        dgvOnlineTrains.Rows[e.RowIndex].Cells["AN"].Value = 0;
                        dgvOnlineTrains.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        dgvOnlineTrains.EndEdit();
                        return;
                    }

                    int currentValue = 0;
                    int newValue = 0;
                    var cellValue = dgvOnlineTrains.Rows[e.RowIndex].Cells["AN"].Value;

                    if (cellValue != DBNull.Value && cellValue != null)
                        currentValue = Convert.ToInt32(cellValue);

                    newValue = (currentValue == 1) ? 0 : 1;
                    dgvOnlineTrains.Rows[e.RowIndex].Cells["AN"].Value = newValue;
                    dgvOnlineTrains.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    dgvOnlineTrains.EndEdit();
                    query = "UPDATE TRAINUPDATEDDATA SET ANNOUNCEMENT=" + newValue + " WHERE TRAIN_NUMBER='" + trainNumber + "'";
                    _dbService.ExecuteScalar(query);
                }
                catch (Exception ex)
                {
                    LoggerService.Logger.Error(ex, ex.Message);
                }
            }
            else if (dgvOnlineTrains.Columns[e.ColumnIndex].Name == "Delete")
            {
                try
                {
                    var dt = (List<train_updated_data>)dgvOnlineTrains.DataSource;
                    var confirm = MessageBox.Show(
                        $"Are you sure you want to delete Train {trainNumber}?",
                        "Confirm Delete",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (confirm == DialogResult.Yes)
                    {
                        query = "DELETE FROM TRAINUPDATEDDATA WHERE TRAIN_NUMBER='" + trainNumber + "'";
                        string result = _dbService.ExecuteScalar(query);
                        ApplyTrainRowColors();
                        dt.RemoveAt(e.RowIndex);
                        for (int i = 0; i < dt.Count; i++)
                            dt[i].SR_NO = i + 1;

                        dgvOnlineTrains.DataSource = null;
                        dgvOnlineTrains.DataSource = dt;

                    }
                    return;
                }
                catch (Exception ex)
                {
                    LoggerService.Logger.Error(ex, ex.Message);
                }
            }
            else
            {
                // Load coach details if any information cell is clicked
                string columnName = dgvOnlineTrains.Columns[e.ColumnIndex].Name;
                string[] viewCols = { "S.NO", "TRAIN", "TRAIN_NAME", "SAT", "SDT", "DELAY", "EAT", "EDT" };
                if (viewCols.Contains(columnName))
                {
                    // Remember the selected train and let ApplyTrainRowColors own ALL row colours.
                    // (Old code painted cell.Style on every cell, which permanently masked the
                    // row-level TADB/CGDB highlight.)
                    _selectedTrainNumber = trainNumber;
                    ApplyTrainRowColors();

                    if (!string.IsNullOrEmpty(trainNumber))
                        LoadCoachData(trainNumber, trainData.TRAIN_NAME_ENGLISH, trainData.trainReversalFlag);
                }
            }


        }

        private async Task UpdateBackwardPlatformAsync(train_updated_data trainInfo, string oldPF, string newpfValue)
        {
            try
            {
                bool result = await HttpHandler.Instance.UPDATE_BACKWARD_PLATEFORM(trainInfo, oldPF, newpfValue, GlobalData._lstAdminSetting.STATION_CODE, GlobalData._lstAdminSetting.PASS_KEY);

                if (result)
                {
                    MessageBox.Show("Backward platform updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Backward platform update failed.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                LoggerService.NtesLogger.Error(ex, "Backward platform update failed Erorr", "error");
            }
        }
        private void dgvOnlineTrains_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string columnName = dgvOnlineTrains.Columns[e.ColumnIndex].Name;
            if (columnName == "STATUS")
            {
                _previousStatus = dgvOnlineTrains.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString() ?? "";
            }
            if (columnName == "AN")
            {
                var tdValue = dgvOnlineTrains.Rows[e.RowIndex].Cells["TD"].Value;
                int tdChecked = (tdValue != DBNull.Value && tdValue != null) ? Convert.ToInt32(tdValue) : 0;

                if (tdChecked == 0)
                    e.Cancel = true;
            }
        }
        private void btnLoad_Click(object sender, EventArgs e)
        {
            string trainNo = txtTrainNo.Text.Trim();

            if (!int.TryParse(trainNo, out _))
            {
                MessageBox.Show("Enter a valid Train Number.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            // Check TRAIN_MASK first
            string query = $@"SELECT COUNT(1) FROM TRAIN_MASK WHERE TRAIN_NO = '{trainNo}'";

            var checkTrainMask = _dbService.ExecuteScalar(query);

            // If train is masked -> stop here
            if (int.TryParse(checkTrainMask, out int maskCount) && maskCount > 0)
            {
                MessageBox.Show(
                    $"Train {trainNo} is masked and cannot be added.",
                    "Masked Train",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtTrainNo.Clear();
                txtTrainNo.Focus();
                return;
            }

            // Train is NOT masked -> only then execute these
            string result = _dbService.ADD_TRAIN_TO_ONLINE(trainNo);

            var dt = _dbService.GET_UPDATED_TRAINS_DATA(0);

            for (int i = 0; i < dt.Count; i++)
            {
                dt[i].SR_NO = i + 1;
                CalculateActualTimes(dt[i]);
            }

            dgvOnlineTrains.DataSource = null;
            dgvOnlineTrains.DataSource = dt;

            if (!string.IsNullOrEmpty(result))
            {
                MessageBox.Show(
                    result,
                    "Train Entry",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            txtTrainNo.Clear();
            txtTrainNo.Focus();
        }
        private async void LoadCoachData(string trainNumber, string TrainNameInEnglish, bool trainReversalFlag)
        {
            try
            {


                string result = await Task.Run(() => _dbService.GetCoachSequence(trainNumber));
                ApplyCoachDataUI(trainNumber, result, TrainNameInEnglish, trainReversalFlag);
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
        }
        //private void ApplyCoachDataUI(string trainNumber, string result, string trainName, bool trainReversalFlag)
        //{
        //    try
        //    {
        //        groupBox_Coach.Visible = true;

        //        removedFromLeft = new();
        //        removedFromRight = new();
        //        lastFocusedTextBox = null;
        //        _currentCoachTrainNumber = trainNumber;
        //        groupBox_Coach.ForeColor = Color.SteelBlue;
        //        DateTime date = Convert.ToDateTime(
        //       _dbService.ExecuteScalar( $"SELECT SCHEDULE_DEPARTURE_TIME FROM TRAINUPDATEDDATA WHERE TRAIN_NUMBER = {trainNumber}")
        //   );

        //        string formattedDate = date.ToString("dd-MMM");
        //        groupBox_Coach.Text = trainReversalFlag
        //                  ? $"Train Number:{trainNumber} | {trainName} | {formattedDate} | (Reversal Point)"
        //                  : $"Train Number:{trainNumber} | {trainName} | {formattedDate}";
        //        GroupBox groupBoxCoach = tableLayoutPanel.Controls
        //            .OfType<GroupBox>()
        //            .FirstOrDefault(g => g.Name == "groupBox_Coach");

        //        if (groupBoxCoach == null)
        //            return;

        //        // Coach data
        //        string[] coachList = Array.Empty<string>();

        //        if (!string.IsNullOrWhiteSpace(result) &&
        //            !result.All(c => c == '-'))
        //        {
        //            coachList = result.Split('-');
        //        }

        //        // Check if we can reuse the existing TableLayoutPanel and TextBoxes to prevent flickering
        //        TableLayoutPanel innerTable = groupBoxCoach.Controls
        //            .OfType<TableLayoutPanel>()
        //            .FirstOrDefault(t => t.Name == "dynamicCoachTable");

        //        bool isReused = false;
        //        if (innerTable != null)
        //        {
        //            var textboxes = innerTable.Controls.OfType<System.Windows.Forms.TextBox>().ToList();
        //            // We expect exactly TOTAL_COACH_BOXES (28) textboxes
        //            if (textboxes.Count == TOTAL_COACH_BOXES)
        //            {
        //                // Check if they are named txtCoach1 to txtCoach28
        //                bool namesMatch = true;
        //                for (int index = 0; index < TOTAL_COACH_BOXES; index++)
        //                {
        //                    if (!innerTable.Controls.ContainsKey($"txtCoach{index + 1}"))
        //                    {
        //                        namesMatch = false;
        //                        break;
        //                    }
        //                }

        //                if (namesMatch)
        //                {
        //                    for (int index = 0; index < TOTAL_COACH_BOXES; index++)
        //                    {
        //                        var txt = (System.Windows.Forms.TextBox)innerTable.Controls[$"txtCoach{index + 1}"];
        //                        txt.Text = index < coachList.Length ? coachList[index] : "";
        //                    }
        //                    isReused = true;
        //                }
        //            }
        //        }

        //        if (!isReused)
        //        {
        //            // Remove old controls
        //            foreach (Control ctrl in groupBoxCoach.Controls.OfType<Control>().ToList())
        //            {
        //                groupBoxCoach.Controls.Remove(ctrl);
        //                ctrl.Dispose();
        //            }

        //            // Create fixed coach table
        //            innerTable = new TableLayoutPanel
        //            {
        //                Name = "dynamicCoachTable",
        //                Dock = DockStyle.Fill,
        //                AutoSize = true,
        //                AutoScroll = true,
        //                ColumnCount = _columnCount,   // Set _columnCount = 14
        //                RowCount = 2,
        //                Margin = new Padding(5)
        //            };

        //            innerTable.ColumnStyles.Clear();

        //            for (int i = 0; i < _columnCount; i++)
        //            {
        //                innerTable.ColumnStyles.Add(
        //                    new ColumnStyle(SizeType.Percent, 100F / _columnCount));
        //            }

        //            innerTable.RowStyles.Clear();
        //            innerTable.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        //            innerTable.RowStyles.Add(new RowStyle(SizeType.Percent, 50));

        //            for (int index = 0; index < TOTAL_COACH_BOXES; index++)
        //            {
        //                int row = index / _columnCount;
        //                int col = index % _columnCount;

        //                System.Windows.Forms.TextBox txt =
        //                    new System.Windows.Forms.TextBox
        //                    {
        //                        Name = $"txtCoach{index + 1}",

        //                        Text = index < coachList.Length
        //                            ? coachList[index]
        //                            : "",

        //                        Dock = DockStyle.Fill,
        //                        Margin = new Padding(3),
        //                        TextAlign = HorizontalAlignment.Center,
        //                        Font = _coachFont
        //                    };

        //                txt.GotFocus += (s, e) =>
        //                {
        //                    lastFocusedTextBox =
        //                        (System.Windows.Forms.TextBox)s;
        //                };

        //                innerTable.Controls.Add(txt, col, row);
        //            }

        //            groupBoxCoach.Controls.Add(innerTable);
        //        }

        //        groupBox_Coach.Visible = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerService.Logger.Error(ex, ex.Message);
        //    }
        //}

        private void ApplyCoachDataUI(string trainNumber, string result, string trainName, bool trainReversalFlag)
        {
            try
            {
                groupBox_Coach.Visible = true;

                removedFromLeft = new();
                removedFromRight = new();
                lastFocusedTextBox = null;
                _currentCoachTrainNumber = trainNumber;
                groupBox_Coach.ForeColor = Color.SteelBlue;

                var res= _dbService.ExecuteScalar($"SELECT SCHEDULE_DEPARTURE_TIME FROM TRAINUPDATEDDATA WHERE TRAIN_NUMBER = '{trainNumber}'");

                DateTime date = DateTime.Today;

                if (!string.IsNullOrWhiteSpace(res) &&
                    DateTime.TryParseExact(res, "yyyy-MM-dd HH:mm:ss",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed))
                {
                    date = parsed;
                }

                string formattedDate = date.ToString("dd-MMM", CultureInfo.InvariantCulture);

                groupBox_Coach.Text = trainReversalFlag
                    ? $"Train Number:{trainNumber} | {trainName} | {formattedDate} | (Reversal Point)"
                    : $"Train Number:{trainNumber} | {trainName} | {formattedDate}";

                GroupBox groupBoxCoach = tableLayoutPanel.Controls
                    .OfType<GroupBox>()
                    .FirstOrDefault(g => g.Name == "groupBox_Coach");

                if (groupBoxCoach == null)
                    return;

                // Coach data
                string[] coachList = Array.Empty<string>();

                if (!string.IsNullOrWhiteSpace(result) &&
                    !result.All(c => c == '-'))
                {
                    coachList = result.Split('-');
                }

                // Check if we can reuse the existing TableLayoutPanel and TextBoxes
                // to prevent flickering
                TableLayoutPanel innerTable = groupBoxCoach.Controls
                    .OfType<TableLayoutPanel>()
                    .FirstOrDefault(t => t.Name == "dynamicCoachTable");

                bool isReused = false;

                if (innerTable != null)
                {
                    var textboxes = innerTable.Controls
                        .OfType<System.Windows.Forms.TextBox>()
                        .ToList();

                    // We expect exactly TOTAL_COACH_BOXES textboxes
                    if (textboxes.Count == TOTAL_COACH_BOXES)
                    {
                        bool namesMatch = true;

                        for (int index = 0; index < TOTAL_COACH_BOXES; index++)
                        {
                            if (!innerTable.Controls.ContainsKey($"txtCoach{index + 1}"))
                            {
                                namesMatch = false;
                                break;
                            }
                        }

                        if (namesMatch)
                        {
                            for (int index = 0; index < TOTAL_COACH_BOXES; index++)
                            {
                                var txt = (System.Windows.Forms.TextBox)
                                    innerTable.Controls[$"txtCoach{index + 1}"];

                                txt.Text = index < coachList.Length
                                    ? coachList[index]
                                    : "";

                                txt.Dock = DockStyle.Fill;
                                txt.Margin = new Padding(COACH_HORIZONTAL_GAP, 1, COACH_HORIZONTAL_GAP, 0);
                            }

                            // Update numbering when reversal changes
                            for (int index = 0; index < TOTAL_COACH_BOXES; index++)
                            {
                                var label = innerTable.Controls
                                    .OfType<Label>()
                                    .FirstOrDefault(l => l.Name == $"lblCoach{index + 1}");

                                if (label != null)
                                {
                                    label.Text = (index + 1).ToString();

                                    // Make sure old (already created) labels are not clipped
                                    label.AutoSize = false;
                                    label.MinimumSize = new Size(0, COACH_NUMBER_ROW_HEIGHT);
                                    label.TextAlign = ContentAlignment.TopCenter;
                                    label.Margin = new Padding(COACH_HORIZONTAL_GAP, 0, COACH_HORIZONTAL_GAP, 6);
                                }
                            }

                            // Re-apply row styles so an already-created table also gets the fix
                            innerTable.AutoSize = false;
                            ApplyCoachRowStyles(innerTable);

                            isReused = true;
                        }
                    }
                }

                if (!isReused)
                {
                    // Remove old controls
                    foreach (Control ctrl in groupBoxCoach.Controls.OfType<Control>().ToList())
                    {
                        groupBoxCoach.Controls.Remove(ctrl);
                        ctrl.Dispose();
                    }

                    // Create fixed coach table
                    innerTable = new TableLayoutPanel
                    {
                        Name = "dynamicCoachTable",
                        Dock = DockStyle.Fill,
                        AutoScroll = true,            // AutoSize removed: it conflicts with Dock.Fill
                        ColumnCount = _columnCount,   // 14
                        RowCount = 4,
                        Margin = new Padding(5)
                    };

                    innerTable.ColumnStyles.Clear();

                    for (int i = 0; i < _columnCount; i++)
                    {
                        innerTable.ColumnStyles.Add(new ColumnStyle(  SizeType.Percent, 100F / _columnCount)); 
                    }

    
                    ApplyCoachRowStyles(innerTable);

                    // Always create all boxes
                    for (int index = 0; index < TOTAL_COACH_BOXES; index++)
                    {
                        int coachRow = index / _columnCount;

                        int col = index % _columnCount;

                        // Coach rows: 0 and 2
                        int row = coachRow == 0 ? 0 : 2;

                        System.Windows.Forms.TextBox txt =
                            new System.Windows.Forms.TextBox
                            {
                                Name = $"txtCoach{index + 1}",

                                // Fill coach data if available
                                Text = index < coachList.Length
                                    ? coachList[index]
                                    : "",

                                Dock = DockStyle.Fill,
                                Margin = new Padding(COACH_HORIZONTAL_GAP, 1, COACH_HORIZONTAL_GAP, 0),
                                TextAlign = HorizontalAlignment.Center,
                                Font = _coachFont
                            };

                        txt.GotFocus += (s, e) =>
                        {
                            lastFocusedTextBox =
                                (System.Windows.Forms.TextBox)s;
                        };

                        innerTable.Controls.Add(txt, col, row);

                        // -------------------------------------------------
                        // Coach number directly below the textbox
                        // -------------------------------------------------
                        Label coachNumber =
                            new Label
                            {
                                Name = $"lblCoach{index + 1}",

                                Text = (index + 1).ToString(),
                                 
                                Dock = DockStyle.Fill,
                                AutoSize = false,
                                MinimumSize = new Size(0, COACH_NUMBER_ROW_HEIGHT),
                                TextAlign = ContentAlignment.TopCenter,
                                Font = new Font(
                                    "Segoe UI",
                                    9F,
                                    FontStyle.Bold
                                ),
                                Margin = new Padding(COACH_HORIZONTAL_GAP, 0, COACH_HORIZONTAL_GAP, 6)
                            };

                        // Number rows: 1 and 3
                        int numberRow = coachRow == 0 ? 1 : 3;

                        innerTable.Controls.Add( coachNumber, col, numberRow);
                    }

                    groupBoxCoach.Controls.Add(innerTable);
                }

                groupBox_Coach.Visible = true;
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
        }

        private const int COACH_NUMBER_ROW_HEIGHT = 22;

        private const int COACH_HORIZONTAL_GAP = 4;


        private static void ApplyCoachRowStyles(TableLayoutPanel table)
        {
            table.RowStyles.Clear();

            table.RowStyles.Add(new RowStyle(SizeType.AutoSize));   // Row 0: coach boxes
            table.RowStyles.Add(new RowStyle(SizeType.AutoSize));   // Row 1: numbers
            table.RowStyles.Add(new RowStyle(SizeType.AutoSize));   // Row 2: coach boxes
            table.RowStyles.Add(new RowStyle(SizeType.AutoSize));   // Row 3: numbers
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            txtTrainNo.Text = "";
            txtTrainNo.Focus();
        }
        private void txtTrainNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            System.Windows.Forms.TextBox tb = sender as System.Windows.Forms.TextBox;
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }
            if (!char.IsControl(e.KeyChar) && tb.Text.Length >= 5)
            {
                e.Handled = true;
            }
        }
        //private void btnShiftLeft_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        GroupBox groupBoxCoach = tableLayoutPanel.Controls
        //                .OfType<GroupBox>()
        //                .FirstOrDefault(g => g.Name == "groupBox_Coach");

        //        var innerTable = GetInnerTable();
        //        if (innerTable == null) return;

        //        var values = GetCurrentValues(innerTable);
        //        if (removedFromRight.Count > 0)
        //        {
        //            var last = removedFromRight.Last();
        //            removedFromRight.RemoveAt(removedFromRight.Count - 1);
        //            values.Insert(last.Position, last.Value);
        //            RebuildInnerTableFromValues(innerTable, values);
        //            return;
        //        }
        //        if (values.Count == 0) return;

        //        var removed = new RemovedItem { Position = 0, Value = values[0] };
        //        removedFromLeft.Add(removed);
        //        values.RemoveAt(0);
        //        RebuildInnerTableFromValues(innerTable, values);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerService.Logger.Error(ex, ex.Message);
        //    }
        //}

        private void btnShiftLeft_Click(object sender, EventArgs e)
                {
                    try
                    {
                        var innerTable = GetInnerTable();
                        if (innerTable == null) return;

                        var boxes = innerTable.Controls
                            .OfType<System.Windows.Forms.TextBox>()
                            .OrderBy(tb => int.Parse(tb.Name.Replace("txtCoach", "")))
                            .ToList();

                        if (boxes.Count == 0) return;

                        // Restore the last coach previously shifted out from the right
                        if (removedFromRight.Count > 0)
                        {
                            var last = removedFromRight.Last();
                            removedFromRight.RemoveAt(removedFromRight.Count - 1);

                            int insertIndex = Math.Min(last.Position, boxes.Count - 1);

                            for (int i = boxes.Count - 1; i > insertIndex; i--)
                            {
                                boxes[i].Text = boxes[i - 1].Text;
                            }

                            boxes[insertIndex].Text = last.Value;
                            return;
                        }

                        // Save the first coach before shifting
                        string firstCoach = boxes[0].Text;

                        if (string.IsNullOrWhiteSpace(firstCoach))
                        {
                            MessageBox.Show(
                                "No coach available to shift left.",
                                "Shift Coach",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                            return;
                        }

                        removedFromLeft.Add(new RemovedItem
                        {
                            Position = 0,
                            Value = firstCoach
                        });

                        // Shift coach values left without rebuilding the UI
                        for (int i = 0; i < boxes.Count - 1; i++)
                        {
                            boxes[i].Text = boxes[i + 1].Text;
                        }

                        // Clear the final box
                        boxes[boxes.Count - 1].Clear();
                    }
                    catch (Exception ex)
                    {
                        LoggerService.Logger.Error(ex, ex.Message);
                    }
                }

        private void btnShiftRight_Click(object sender, EventArgs e)
                {
                    try
                    {
                        var innerTable = GetInnerTable();
                        if (innerTable == null) return;

                        var boxes = innerTable.Controls
                            .OfType<System.Windows.Forms.TextBox>()
                            .OrderBy(tb => int.Parse(tb.Name.Replace("txtCoach", "")))
                            .ToList();

                        if (boxes.Count == 0) return;

                        // Restore the last coach previously shifted out from the left
                        if (removedFromLeft.Count > 0)
                        {
                            var last = removedFromLeft.Last();
                            removedFromLeft.RemoveAt(removedFromLeft.Count - 1);

                            int insertIndex = Math.Min(last.Position, boxes.Count - 1);

                            for (int i = boxes.Count - 1; i > insertIndex; i--)
                            {
                                boxes[i].Text = boxes[i - 1].Text;
                            }

                            boxes[insertIndex].Text = last.Value;
                            return;
                        }

                        // Save the last coach before shifting
                        string lastCoach = boxes[boxes.Count - 1].Text;

                        if (string.IsNullOrWhiteSpace(lastCoach))
                        {
                            MessageBox.Show(
                                "No coach available to shift right.",
                                "Shift Coach",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                            return;
                        }

                        removedFromRight.Add(new RemovedItem
                        {
                            Position = boxes.Count - 1,
                            Value = lastCoach
                        });

                        // Shift coach values right without rebuilding the UI
                        for (int i = boxes.Count - 1; i > 0; i--)
                        {
                            boxes[i].Text = boxes[i - 1].Text;
                        }

                        // Clear the first box
                        boxes[0].Clear();
                    }
                    catch (Exception ex)
                    {
                        LoggerService.Logger.Error(ex, ex.Message);
                    }
                }


        //private void btnShiftRight_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        GroupBox groupBoxCoach = tableLayoutPanel.Controls
        //                .OfType<GroupBox>()
        //                .FirstOrDefault(g => g.Name == "groupBox_Coach");

        //        var innerTable = GetInnerTable();
        //        if (innerTable == null) return;

        //        var values = GetCurrentValues(innerTable);

        //        if (removedFromLeft.Count > 0)
        //        {
        //            var last = removedFromLeft.Last();
        //            removedFromLeft.RemoveAt(removedFromLeft.Count - 1);
        //            values.Insert(last.Position, last.Value);
        //            RebuildInnerTableFromValues(innerTable, values);
        //            return;
        //        }
        //        if (values.Count == 0) return;

        //        int pos = values.Count - 1;
        //        var removed = new RemovedItem { Position = pos, Value = values[pos] };
        //        removedFromRight.Add(removed);
        //        values.RemoveAt(pos);
        //        RebuildInnerTableFromValues(innerTable, values);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerService.Logger.Error(ex, ex.Message);
        //    }
        //}
        //private void btnReverse_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string trainNumber = _currentCoachTrainNumber;

        //        GroupBox groupBoxCoach = tableLayoutPanel.Controls
        //            .OfType<GroupBox>()
        //            .FirstOrDefault(g => g.Name == "groupBox_Coach");

        //        if (groupBoxCoach == null)
        //        {
        //            MessageBox.Show("GroupBox not found!", "Error",
        //                MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }

        //        TableLayoutPanel innerTable = groupBoxCoach.Controls
        //            .OfType<TableLayoutPanel>()
        //            .FirstOrDefault(t => t.Name == "dynamicCoachTable");

        //        if (innerTable == null)
        //        {
        //            MessageBox.Show("Coach data not found!", "Info",
        //                MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            return;
        //        }

        //        var coachTextBoxes = innerTable.Controls
        //            .OfType<System.Windows.Forms.TextBox>()
        //            .OrderBy(tb =>
        //            {
        //                string numPart = new string(tb.Name
        //                    .Where(char.IsDigit)
        //                    .ToArray());
        //                return int.TryParse(numPart, out int n) ? n : 0;
        //            })
        //            .ToList();

        //        if (coachTextBoxes.Count == 0)
        //        {
        //            MessageBox.Show("Coach data not available!!", "Info",
        //                MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            return;
        //        }

        //        List<string> filledValues = coachTextBoxes
        //            .Select(tb => tb.Text.Trim())
        //            .ToList();
        //        filledValues.Reverse();

        //        for (int i = 0; i < coachTextBoxes.Count; i++)
        //            coachTextBoxes[i].Text = filledValues[i];

        //        var trainInfo = lsttrain.FirstOrDefault(x => x.TRAIN_NUMBER == trainNumber);

        //        //bool newReversalFlag = trainInfo != null
        //        //    ? !trainInfo.trainReversalFlag
        //        //    : true;

        //        //int flagValue = newReversalFlag ? 1 : 0;

        //        // ---- 3. Build the NEW coach sequence string (this is what was missing) ----
        //        string coachSequence = string.Join("-",
        //            coachTextBoxes
        //                .Select(tb => tb.Text.Trim())
        //                .Where(x => !string.IsNullOrWhiteSpace(x) && x != "-"));

        //        coachSequence = coachSequence.Replace("--", "-").Replace(" ", "");

        //        // ---- 4. Save BOTH the flag AND the new sequence to DB ----
        //        string updateQuery =
        //            $"UPDATE TRAINUPDATEDDATA SET " +
        //            $"COACH_SEQUENCE = '{coachSequence}' " +
        //            $"WHERE TRAIN_NUMBER = '{trainNumber}'";

        //        _dbService.ExecuteScalar(updateQuery);


        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerService.Logger.Error(ex, ex.Message);
        //    }
        //}
        private void btnReverse_Click(object sender, EventArgs e)
        {
            try
            {
                string trainNumber = _currentCoachTrainNumber;

                GroupBox groupBoxCoach = tableLayoutPanel.Controls
                    .OfType<GroupBox>()
                    .FirstOrDefault(g => g.Name == "groupBox_Coach");

                if (groupBoxCoach == null)
                {
                    MessageBox.Show("GroupBox not found!", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                TableLayoutPanel innerTable = groupBoxCoach.Controls
                    .OfType<TableLayoutPanel>()
                    .FirstOrDefault(t => t.Name == "dynamicCoachTable");

                if (innerTable == null)
                {
                    MessageBox.Show("Coach data not found!", "Info",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var coachTextBoxes = innerTable.Controls
                    .OfType<System.Windows.Forms.TextBox>()
                    .OrderBy(tb =>
                    {
                        string numPart = new string(tb.Name
                            .Where(char.IsDigit)
                            .ToArray());
                        return int.TryParse(numPart, out int n) ? n : 0;
                    })
                    .ToList();

                if (coachTextBoxes.Count == 0)
                {
                    MessageBox.Show("Coach data not available!!", "Info",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }


                int firstFilled = coachTextBoxes.FindIndex(tb => !string.IsNullOrWhiteSpace(tb.Text));
                int lastFilled = coachTextBoxes.FindLastIndex(tb => !string.IsNullOrWhiteSpace(tb.Text));

                if (firstFilled < 0)
                {
                    MessageBox.Show("Coach data not available!!", "Info",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // ---- 2. Reverse only that range, in place ----
                List<string> filledValues = coachTextBoxes
                    .Skip(firstFilled)
                    .Take(lastFilled - firstFilled + 1)
                    .Select(tb => tb.Text.Trim())
                    .ToList();

                filledValues.Reverse();

                for (int i = 0; i < filledValues.Count; i++)
                    coachTextBoxes[firstFilled + i].Text = filledValues[i];

                var trainInfo = lsttrain.FirstOrDefault(x => x.TRAIN_NUMBER == trainNumber);

                //bool newReversalFlag = trainInfo != null
                //    ? !trainInfo.trainReversalFlag
                //    : true;

                //int flagValue = newReversalFlag ? 1 : 0;

                // ---- 3. Build the NEW coach sequence string ----
                string coachSequence = string.Join("-",
                    coachTextBoxes
                        .Select(tb => tb.Text.Trim())
                        .Where(x => !string.IsNullOrWhiteSpace(x) && x != "-"));

                coachSequence = coachSequence.Replace("--", "-").Replace(" ", "");

                // ---- 4. Save the new sequence to DB ----
                string updateQuery =
                    $"UPDATE TRAINUPDATEDDATA SET " +
                    $"COACH_SEQUENCE = '{coachSequence}' " +
                    $"WHERE TRAIN_NUMBER = '{trainNumber}'";

                _dbService.ExecuteScalar(updateQuery);
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
        }
        //private void btnRemove_Click(object sender, EventArgs e)
        //{

        //    try
        //    {
        //        GroupBox groupBoxCoach = tableLayoutPanel.Controls
        //            .OfType<GroupBox>()
        //            .FirstOrDefault(g => g.Name == "groupBox_Coach");

        //        if (groupBoxCoach == null)
        //        {
        //            MessageBox.Show("Coach container not found!", "VARDAAN ELECTRONICS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return;
        //        }
        //        TableLayoutPanel innerTable = groupBoxCoach.Controls
        //            .OfType<TableLayoutPanel>()
        //            .FirstOrDefault(t => t.Name == "dynamicCoachTable");

        //        if (innerTable == null)
        //        {
        //            MessageBox.Show("No coach table found!", "VARDAAN ELECTRONICS",
        //                MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            return;
        //        }
        //        System.Windows.Forms.TextBox focusedTextBox = lastFocusedTextBox;
        //        if (focusedTextBox == null || !innerTable.Controls.Contains(focusedTextBox))
        //        {
        //            MessageBox.Show("Please select coach to remove!", "VARDAAN ELECTRONICS",
        //                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return;
        //        }
        //        innerTable.Controls.Remove(focusedTextBox);
        //        focusedTextBox.Dispose();
        //        var textboxes = innerTable.Controls.OfType<System.Windows.Forms.TextBox>().OrderBy(t => innerTable.GetRow(t)).ThenBy(t => innerTable.GetColumn(t)).ToList();
        //        innerTable.Controls.Clear();
        //        int colCount = innerTable.ColumnCount;
        //        int row = 0, col = 0;
        //        foreach (var tb in textboxes)
        //        {
        //            innerTable.Controls.Add(tb, col, row);
        //            col++;
        //            if (col >= colCount)
        //            {
        //                col = 0;
        //                row++;
        //            }
        //        }

        //        innerTable.PerformLayout();
        //        if (!innerTable.Controls.OfType<System.Windows.Forms.TextBox>().Any())
        //        {
        //            groupBoxCoach.Controls.Remove(innerTable);
        //            innerTable.Dispose();

        //            Label lblNoCoach = new Label
        //            {
        //                Name = "lblNoCoach",
        //                Text = "No coach available",
        //                AutoSize = true,
        //                ForeColor = Color.Red,
        //                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
        //                TextAlign = ContentAlignment.MiddleCenter,
        //                Dock = DockStyle.Fill
        //            };
        //            groupBoxCoach.Controls.Add(lblNoCoach);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerService.Logger.Error(ex, ex.Message);
        //    }
        //}

        private void ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (sender is System.Windows.Forms.ComboBox cb)
            {
                var newpfValue = cb.SelectedItem?.ToString() ?? "";
                var currentCell = dgvOnlineTrains.CurrentCell;
                int rowIndex = currentCell?.RowIndex ?? -1;
                int colIndex = currentCell?.ColumnIndex ?? -1;

                if (rowIndex >= 0 && colIndex >= 0)
                {
                    try
                    {
                        string columnName = dgvOnlineTrains.Columns[colIndex].Name;
                        if (columnName.Equals("PFNO", StringComparison.OrdinalIgnoreCase))
                        {
                            dgvOnlineTrains.EndEdit();
                            if (dgvOnlineTrains.Rows[rowIndex].DataBoundItem is train_updated_data train)
                            {
                                string _trainNumber = train.TRAIN_NUMBER;
                                string oldPlatform = train.ACTUAL_PLATFORM_NUMBER ?? "";
                                var confirm = MessageBox.Show($"Are you sure to change PF!!", "vardaan electronics industries.", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                                if (confirm == DialogResult.Yes)
                                {
                                    query = $"UPDATE TRAINUPDATEDDATA SET ACTUAL_PLATFORM_NUMBER='{newpfValue}' WHERE TRAIN_NUMBER='{_trainNumber}'";
                                    string result = _dbService.QuerySingleOrDefault(query);

                                    var trainInfo = lsttrain.FirstOrDefault(x => x.TRAIN_NUMBER == _trainNumber);
                                    if (trainInfo != null)
                                    {
                                        trainInfo.ACTUAL_PLATFORM_NUMBER = newpfValue;
                                    }

                                    dgvOnlineTrains.Rows[rowIndex].Cells["PFNO"].Value = newpfValue;

                                    dgvOnlineTrains.Refresh();
                                    dgvOnlineTrains.InvalidateCell(dgvOnlineTrains.Columns["PFNO"].Index, rowIndex);

                                    if (trainInfo != null)
                                    {
                                        isBackwardIntigration = true;
                                        UpdateBackwardPlatformAsync(trainInfo, oldPlatform, newpfValue);
                                    }

                                    dgvOnlineTrains.CommitEdit(DataGridViewDataErrorContexts.Commit);
                                    dgvOnlineTrains.EndEdit();
                                }
                                else
                                {
                                    // Revert the combo box to previous value
                                    LoadOnlineTrains();
                                }
                            }
                        }
                        else if (columnName.Equals("STATUS", StringComparison.OrdinalIgnoreCase))
                        {
                            string newStatusValue = cb.SelectedItem?.ToString() ?? "";
                            string oldStatus = !string.IsNullOrEmpty(_previousStatus) ? _previousStatus : "";

                            dgvOnlineTrains.EndEdit();

                            if (!string.IsNullOrEmpty(newStatusValue) && string.Equals(oldStatus, newStatusValue, StringComparison.OrdinalIgnoreCase))
                            {
                                bool isSpecial = newStatusValue.ToUpper().Contains("DIVERTED") ||
                                                 newStatusValue.ToUpper().Contains("TERMINATED") ||
                                                 newStatusValue.ToUpper().Contains("CHANGE OF SOURCE") ||
                                                 newStatusValue.ToUpper().Contains("MERGE");

                                if (isSpecial && rowIndex < dgvOnlineTrains.Rows.Count &&
                                    dgvOnlineTrains.Rows[rowIndex].DataBoundItem is train_updated_data train)
                                {
                                    this.BeginInvoke(new Action(() =>
                                    {
                                        try
                                        {
                                            bool isSaved = OpenTrainSearchDialog(newStatusValue, train.TRAIN_NUMBER);
                                            if (isSaved)
                                            {
                                                _dbService.UpdateTrainOnlineDgv(train);
                                            }
                                            LoadOnlineTrains();
                                        }
                                        catch (Exception ex)
                                        {
                                            LoggerService.Logger.Error(ex, ex.Message);
                                        }
                                    }));
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LoggerService.Logger.Error(ex, ex.Message);
                    }
                }
            }
        }
        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                TableLayoutPanel innerTable = GetInnerTable();
                if (innerTable == null)
                {
                    MessageBox.Show("No coach table found!");
                    return;
                }

                // all coach boxes in order: txtCoach1, txtCoach2, ...
                var boxes = innerTable.Controls
                    .OfType<System.Windows.Forms.TextBox>()
                    .OrderBy(tb => int.Parse(tb.Name.Replace("txtCoach", "")))
                    .ToList();

                var selected = boxes.FirstOrDefault(tb => tb == lastFocusedTextBox)
                               ?? boxes.FirstOrDefault(tb => tb.Focused);

                if (selected == null)
                {
                    MessageBox.Show("Click inside a coach box first, then press Remove.");
                    return;
                }
                if (string.IsNullOrWhiteSpace(selected.Text) || selected.Text.Trim() == "-")
                {
                    MessageBox.Show("Selected coach box is empty. Nothing to remove.",
                                    "Remove Coach",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    return;
                }

                string trainNumber = _currentCoachTrainNumber;
                int index = boxes.IndexOf(selected);

                // 1. take all values, remove the selected one
                List<string> values = boxes.Select(tb => tb.Text.Trim()).ToList();
                values.RemoveAt(index);

                // 2. build the new sequence (empty boxes are skipped)
                string seq = string.Join("-", values.Where(x => x != "" && x != "-"));

                // 3. save to database
                string value = seq == "" ? "NULL" : $"'{seq}'";
                _dbService.ExecuteScalar(
                    $"UPDATE TRAINUPDATEDDATA SET COACH_SEQUENCE = {value} WHERE TRAIN_NUMBER = '{trainNumber}'");

                // 4. redraw the coach panel from the saved sequence
                var train = lsttrain.FirstOrDefault(x => x.TRAIN_NUMBER == trainNumber);
                ApplyCoachDataUI(
                    trainNumber,
                    seq,
                    train?.TRAIN_NAME_ENGLISH ?? "",
                    train?.trainReversalFlag ?? false);
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                GroupBox groupBoxCoach = tableLayoutPanel.Controls
                    .OfType<GroupBox>()
                    .FirstOrDefault(g => g.Name == "groupBox_Coach");

                if (groupBoxCoach == null)
                {
                    MessageBox.Show("Coach container not found!", "VARDAAN ELECTRONICS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                TableLayoutPanel innerTable = groupBoxCoach.Controls
                    .OfType<TableLayoutPanel>()
                    .FirstOrDefault(t => t.Name == "dynamicCoachTable");

                if (innerTable == null)
                {
                    innerTable = new TableLayoutPanel
                    {
                        Name = "dynamicCoachTable",
                        Dock = DockStyle.Fill,
                        AutoSize = true,
                        AutoScroll = true,
                        ColumnCount = _columnCount,
                        RowCount = 1,
                        Margin = new Padding(5)
                    };

                    for (int c = 0; c < _columnCount; c++)
                        innerTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / 13));

                    innerTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                    groupBoxCoach.Controls.Add(innerTable);
                }

                int totalTextBoxes = innerTable.Controls.OfType<System.Windows.Forms.TextBox>().Count();

                var maxCoachesLimit = GlobalData._lstAdminSetting.Total_Coach;

                if (totalTextBoxes >= maxCoachesLimit)
                {
                    MessageBox.Show($"Cannot add more coaches! Maximum configuration limit of {maxCoachesLimit} coaches reached as per Admin Settings.",
                                    "VARDAAN ELECTRONICS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return; // Exit method immediately; no box will be rendered
                }

                var lblNoCoach = groupBoxCoach.Controls.OfType<Label>().FirstOrDefault(l => l.Name == "lblNoCoach");
                if (lblNoCoach != null)
                {
                    groupBoxCoach.Controls.Remove(lblNoCoach);
                    lblNoCoach.Dispose();
                }

                int col = totalTextBoxes % innerTable.ColumnCount;
                int row = totalTextBoxes / innerTable.ColumnCount;

                if (row >= innerTable.RowCount)
                {
                    innerTable.RowCount += 1;
                    innerTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                }

                System.Windows.Forms.TextBox txt = new System.Windows.Forms.TextBox
                {
                    Name = $"txtCoach{totalTextBoxes + 1}",
                    Text = "-",
                    Dock = DockStyle.Fill,
                    Margin = new Padding(0),
                    TextAlign = HorizontalAlignment.Center
                };
                txt.GotFocus += (s, ev) => lastFocusedTextBox = (System.Windows.Forms.TextBox)s;
                innerTable.Controls.Add(txt, col, row);
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
                string result = string.Empty;
                if (string.IsNullOrWhiteSpace(groupBox_Coach.Text) || !groupBox_Coach.Text.Contains(":"))
                {
                    MessageBox.Show("Train details are missing!", "VARDAAN ELECTRONICS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string trainNumber = _currentCoachTrainNumber;
                if (string.IsNullOrEmpty(trainNumber))
                {
                    MessageBox.Show("Invalid Train Number!", "VARDAAN ELECTRONICS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var innerTable = groupBox_Coach.Controls
                    .OfType<TableLayoutPanel>()
                    .FirstOrDefault(t => t.Name == "dynamicCoachTable");

                var lblNoCoach = groupBox_Coach.Controls
                    .OfType<Label>()
                    .FirstOrDefault(t => t.Name == "lblNoCoach");

                if (innerTable == null && lblNoCoach != null)
                {
                    query = $"UPDATE TRAINUPDATEDDATA SET COACH_SEQUENCE=null WHERE TRAIN_NUMBER='{trainNumber}'";
                    result = _dbService.ExecuteScalar(query);
                    MessageBox.Show("Coach sequence updated successfully!", "VARDAAN ELECTRONICS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                var coachTextBoxes = innerTable.Controls
                    .OfType<System.Windows.Forms.TextBox>()
                    .OrderBy(tb =>
                    {
                        string numPart = new string(tb.Name.Where(char.IsDigit).ToArray());
                        return int.TryParse(numPart, out int n) ? n : 0;
                    }).ToList();

                if (coachTextBoxes.Count == 0)
                {
                    MessageBox.Show("No coach data found!", "VARDAAN ELECTRONICS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string coachSequence = string.Join("-", coachTextBoxes
                    .Select(tb => tb.Text.Trim())
                    .Where(x => !string.IsNullOrWhiteSpace(x) && x != "-"));

                coachSequence = coachSequence.Replace("--", "-").Replace(" ", "");
                query = $"UPDATE TRAINUPDATEDDATA SET COACH_SEQUENCE='{coachSequence}' WHERE TRAIN_NUMBER='{trainNumber}'";
                result = _dbService.ExecuteScalar(query);
                MessageBox.Show("Coach sequence updated successfully!", "VARDAAN ELECTRONICS", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
        }
        private TableLayoutPanel GetInnerTable()
        {
            GroupBox groupBoxCoach = tableLayoutPanel.Controls
                                   .OfType<GroupBox>()
                                   .FirstOrDefault(g => g.Name == "groupBox_Coach");

            if (groupBoxCoach == null)
                return null;

            TableLayoutPanel innerTable = groupBoxCoach.Controls
                .OfType<TableLayoutPanel>()
                .FirstOrDefault(t => t.Name == "dynamicCoachTable");
            return innerTable;
        }
        private List<string> GetCurrentValues(TableLayoutPanel innerTable)
        {
            return innerTable.Controls
                .OfType<System.Windows.Forms.TextBox>()
                .OrderBy(tb => innerTable.Controls.GetChildIndex(tb))
                .Select(tb => tb.Text)
                .ToList();
        }
        private void RebuildInnerTableFromValues(TableLayoutPanel innerTable, List<string> values)
        {
            innerTable.SuspendLayout();

            foreach (Control c in innerTable.Controls.OfType<Control>().ToList())
            {
                innerTable.Controls.Remove(c);
                c.Dispose();
            }

            int columns = innerTable.ColumnCount > 0 ? innerTable.ColumnCount : 8;
            int newRowCount = (int)Math.Ceiling(values.Count / (double)columns);
            innerTable.RowCount = Math.Max(1, newRowCount);
            innerTable.RowStyles.Clear();
            for (int r = 0; r < innerTable.RowCount; r++)
                innerTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            int idx = 0;
            for (int r = 0; r < innerTable.RowCount; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    if (idx >= values.Count) break;
                    var txt = new System.Windows.Forms.TextBox
                    {
                        Name = $"txtCoach{idx + 1}",
                        Text = values[idx],
                        Dock = DockStyle.Fill,
                        Margin = new Padding(3),
                        TextAlign = HorizontalAlignment.Center
                    };
                    txt.GotFocus += (s, e) => lastFocusedTextBox = (System.Windows.Forms.TextBox)s;
                    innerTable.Controls.Add(txt, c, r);
                    idx++;
                }
            }
            innerTable.ResumeLayout();
        }
        private async void btnFromNtes_Click(object sender, EventArgs e)
        {
            if (!await _semaphore.WaitAsync(0))
                return;

            try
            {
                // Disable immediately
                btnFromNtes.Enabled = false;
                btnFromNtes.Text = "UPDATING...";
                btnFromNtes.BackColor = Color.Orange;

                // Execute operation
                await LoadFromNtesAsync();
                _ = IpisTrapSender.Instance.SendNtesSyncAsync("Data Fetched Successfully from the NTES ");
                btnFromNtes.Text = "NTES UPDATE";
                btnFromNtes.BackColor = Color.Green;


            }
            catch (Exception ex)
            {
                LoggerService.NtesLogger.Error(ex, "NTES GetMasterTrains failed. Station: {Station}", "error");
                _ = IpisTrapSender.Instance.SendNtesSyncFailureAsync( "NTES manual update failed: " + ex.Message);
                btnFromNtes.Text = "NTES UPDATE";
                btnFromNtes.BackColor = Color.Red;


            }
            finally
            {
                // Immediate reset
                btnFromNtes.Enabled = true;
                btnFromNtes.BackColor = Color.SteelBlue;
                _semaphore.Release();
            }
        }
        public void ApplyAccessRights()
        {
            bool canSendTADB_CGDB = GlobalData.CurrentUserAccessRight != null && GlobalData.CurrentUserAccessRight.TADB_CGDB_SEND == 1;
            btn_TADB.Enabled = canSendTADB_CGDB;
            btn_CGDB.Enabled = canSendTADB_CGDB;

            bool canPlayAnnouncement = GlobalData.CurrentUserAccessRight != null && GlobalData.CurrentUserAccessRight.PLAY_ANNOUNCEMENT == 1;
            if (!canPlayAnnouncement)
            {
                btnPlay.Enabled = false;
                btnPause.Enabled = false;
                btnStop.Enabled = false;
                btnCoachAnnouncemet.Enabled = false;
            }
        }

        private async void btn_TADB_Click(object sender, EventArgs e)
        {
            if (GlobalData.CurrentUserAccessRight == null || GlobalData.CurrentUserAccessRight.TADB_CGDB_SEND != 1)
            {
                btn_TADB.Enabled = false;
                return;
            }

            //if (_isTadbProcessing)
            //{
            //    LoggerService.Logger.Information($"TADB click ignored, send running since {DateTime.Now}");
            //    return;
            //}
            string userName = GlobalData._lstLoginInfo.FirstOrDefault()?.user_Name ?? "Admin";
            string userType = GlobalData.CurrentUserAccessRight.USER_TYPE ?? "Operator";
            var pendingChanges = _pendingTadbChanges.ToList();

            //_isTadbProcessing = true;
            btn_TADB.Enabled = false;
            btn_TADB.Text = "TADB SENDING...";

            try
            {
                await Task.Run(() =>
                {
                    foreach (var kvp in pendingChanges)
                    {
                        string trainNumber = kvp.Key;
                        int tadbSendData = kvp.Value;

                        _dbService.ExecuteScalar("UPDATE TRAINUPDATEDDATA SET TADB_SEND_DATA = " + tadbSendData + " WHERE TRAIN_NUMBER = '" + trainNumber + "'");

                        if (tadbSendData == 0)
                            _dbService.ExecuteScalar("UPDATE TRAINUPDATEDDATA SET ANNOUNCEMENT = 0 WHERE TRAIN_NUMBER = '" + trainNumber + "'");

                        _dbService.ADD_LOGDISPLAYBOARD(userName, userType, trainNumber, tadbSendData, DateTime.Now, "VARDAAN");
                    }

                    TrainDetailsFileHelper.WriteTadbTrains(_dbService.GET_UPDATED_TRAINS_DATA(0));

                    clsPacketGeneration.Instance.ASSEMBLE_TADB_DATA(_dbService.GET_BOARD_DETAILS("TADB"));
                });

                await Task.Delay(2000);

                _pendingTadbChanges.Clear();
                _pendingRowState.Clear();
                ApplyTrainRowColors();
                dgvOnlineTrains.Refresh();
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
            finally
            {
                //_isTadbProcessing = false;
                btn_TADB.Enabled = true;
                btn_TADB.Text = "TADB";
            }
        }

        private async void btn_CGDB_Click(object sender, EventArgs e)
        {
            if (GlobalData.CurrentUserAccessRight == null ||
                GlobalData.CurrentUserAccessRight.TADB_CGDB_SEND != 1)
            {
                btn_CGDB.Enabled = false;
                return;
            }

            //if (_isCgdbProcessing)
            //    return;

            var loginInfo = GlobalData._lstLoginInfo.FirstOrDefault();

            string userName = loginInfo?.user_Name ?? "Admin";
            string userType = GlobalData.CurrentUserAccessRight?.USER_TYPE ?? "Operator";

            //_isCgdbProcessing = true;
            btn_CGDB.Enabled = false;
            btn_CGDB.Text = "CGDB Sending...";

            try
            {
                var pendingChanges = _pendingCGDBChanges.ToList();

                await Task.Run(() =>
                {
                    foreach (var kvp in pendingChanges)
                    {
                        string trainNumber = kvp.Key;
                        int cgdbSendData = kvp.Value;
                        string q = "UPDATE TRAINUPDATEDDATA SET CGDB_SEND_DATA=" + cgdbSendData + " WHERE TRAIN_NUMBER='" + trainNumber + "'";

                        _dbService.ExecuteScalar(q);
                        _dbService.LOGCOACHSEQUENCE(userName, userType, trainNumber, cgdbSendData, DateTime.Now);
                    }
                    lstboardInfo = _dbService.GET_BOARD_DETAILS("CGDB");
                    clsPacketGeneration.Instance.ASSEMBLE_TADB_DATA(lstboardInfo);
                });

                await Task.Delay(2000);

                _pendingCGDBChanges.Clear();
                _pendingRowState.Clear();

                ApplyTrainRowColors();
                dgvOnlineTrains.Refresh();
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
            finally
            {
                //_isCgdbProcessing = false;
                btn_CGDB.Enabled = true;
                btn_CGDB.Text = "CGDB";
            }
        }

        public void OnSettingsEvent(object sender, eventInfo e)
        {
            if (e.eventType == "ntsAutoupdate")
            {
                bool IsNtesAutoUpdateActive = GlobalData._lstAdminSetting.IsNtesAutoUpdate;
                int TimeInterval = GlobalData._lstAdminSetting.NtesTimeMin;

                if (IsNtesAutoUpdateActive)
                    StartOrUpdateTimer_AUTO_NTES(TimeInterval);
                else
                    _timerNTES?.Stop();
            }
            else if (e.eventType == "autoAnnounce")
            {
                if (GlobalData._lstAdminSetting.Auto_Announce == 1)
                    StartAutoAnnouncementTimer();
                else
                    StopAutoAnnouncementTimer();
            }

            // Either setting may have flipped → re-evaluate lock
            ApplySystemLockState();
        }
        private void StartOrUpdateTimer_AUTO_NTES(int intervalSeconds)
        {
            if (_timerNTES == null)
            {
                _timerNTES = new System.Windows.Forms.Timer();
                _timerNTES.Tick += async (s, e) =>
                {
                    if (_isNtesRunning) return;

                    _isNtesRunning = true;

                    if (dgvOnlineTrains.IsCurrentCellInEditMode)
                    {
                        return;
                    }
                    try
                    {
                        await LoadFromNtesAsync();
                        _ = IpisTrapSender.Instance.SendNtesSyncAsync("Data Fetched Successfully from the NTES ");
                    }
                    catch (Exception ex)
                    {
                        LoggerService.NtesLogger.Error(ex, "NTES GetMasterTrains failed. Station: {Station}", "error");
                        _ = IpisTrapSender.Instance.SendNtesSyncFailureAsync("NTES manual update failed: " + ex.Message);
                    }
                    finally
                    {
                        _isNtesRunning = false;
                    }
                };
            }

            _timerNTES.Interval = intervalSeconds * 1000;

            if (!_timerNTES.Enabled)
                _timerNTES.Start();
        }
        private void AutoStart_CAP()
        {
            //_capInfo = new CapInfo("Vardaan@Electronics#05", "ee4532bf674b2d5d10aa91dc0141cd941645fa3a752b6710482a66a98caf370e", "IpisClient", "Cdot@user1");
           
            _capInfo = new CapInfo(GlobalData._lstAdminSetting?.CAP_Name, GlobalData._lstAdminSetting?.CAP_PASSKEY, "IpisClient", "Cdot@user1");
            if (_timerCAP == null)
            {
                _timerCAP = new System.Windows.Forms.Timer();
                _timerCAP.Tick += async (s, e) =>
                {
                    if (_istimerCAPRunning) return;

                    _istimerCAPRunning = true;
                    try
                    {
                        await _dbService.Update_CapStatus();
                        await CapOperation();
                    }
                    catch (Exception ex)
                    {
                        LoggerService.CapLogger.Error(ex, "CAP Error Logs");
                    }
                    finally
                    {
                        _istimerCAPRunning = false;
                    }
                };
            }
            _timerCAP.Interval = (5 * 60 * 1000);
            if (!_timerCAP.Enabled)
                _timerCAP.Start();
        }
        private async Task CapOperation()
        {
            try
            {
                string capXml = await _capInfo.Check_CapData("https://sachetdemo.cdot.in/AlertRequestByIPISClientt");

                // CAP status for the Dashboard: ACTIVE when the CAP server answered with HTTP 200.
                // Check_CapData returns "" on no reply/error and response.ToString() ("StatusCode: ...") on a non-200 reply.
                bool capActive = !string.IsNullOrWhiteSpace(capXml) && !capXml.StartsWith("StatusCode:");
                OnEvent?.Invoke(this, new eventInfo(
                    eventType: "CapStatusAlert",
                    message: capActive ? "CAP : ACTIVE" : "CAP : NOT ACTIVE",
                    fromWhere: this.Name,
                    status: capActive));

                if (string.IsNullOrWhiteSpace(capXml) || capXml.Length < 100)
                    return;
                await ProcessCap(capXml);
                await SubmitCapReport();
            }
            catch (Exception ex)
            {
                LoggerService.CapLogger.Error(ex, ex.Message);
            }
        }
        private async Task ProcessCap(string capXml)
        {
            CapModel _capModel = new CapModel();
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(capXml);

            DataSet ds = new DataSet();
            ds.ReadXml(new XmlNodeReader(xmldoc));

            if (ds.Tables["alert"] == null || ds.Tables["info"] == null)
                return;

            string Identifier = ds.Tables["alert"].Rows[0]["identifier"].ToString();
            string AlarmType = ds.Tables["info"].Rows[0]["event"].ToString();
            string Message = ds.Tables["info"].Rows[0]["headline"].ToString();
            string StartDateTime = ds.Tables["info"].Rows[0]["effective"].ToString();
            string EndDateTime = ds.Tables["info"].Rows[0]["expires"].ToString();
            string PlayDuration = string.Empty;
            string PlayUnlimited = string.Empty;
            string UserName = string.Empty;
            string EvenStatus = string.Empty;
            string CreatedTime = string.Empty;
            var audioUrls = new List<string>{ ds.Tables["resource"]?.Rows[0]["uri"]?.ToString(), ds.Tables["resource"]?.Rows[0]["derefUri"]?.ToString() };

            string audioPath = await _capInfo.Download_AudioFile(audioUrls);
            string validateurl = "https://sachetdemo.cdot.in/ValidationStatusFromIPISClientt";
            string validateData = await _capInfo.Validate_Message(Identifier, "Success", validateurl);
            string validateTime = DateTime.Now.AddMinutes(2).ToString("yyyy-MM-dd HH:mm:ss.fff");

            _capModel.Identifier = Identifier;
            _capModel.Message = Message;
            _capModel.AudioOne = audioPath.Split('$').FirstOrDefault();
            _capModel.AudioTwo = audioPath.Split('$').Last();
            _capModel.StartDateTime = StartDateTime;
            _capModel.Validatetime = validateTime;
            _capModel.EndDateTime = EndDateTime;
            await _dbService.Insert_Cap(_capModel);

        }
        private async Task SubmitCapReport()
        {
            try
            {
                DataTable responsedt = new DataTable();
                string Query = "select Identifier,Displaycount,AnnounceCount,ValidateTime,EndDateTime from cap where EvenStatus=3";
                responsedt = _dbService.GetTable(Query);
                var url = "https://sachetdemo.cdot.in/GetDisseminationStatsFromIPISClient";
                if (responsedt.Rows.Count > 0)
                    foreach (DataRow dr in responsedt.Rows)
                    {
                        try
                        {
                            string Identifier = dr["Identifier"].ToString();
                            string Displaycount = dr["Displaycount"].ToString();
                            string AnnounceCount = dr["AnnounceCount"].ToString();
                            string ValidateTime = dr["ValidateTime"].ToString() + ".000";
                            string EndDateTime = dr["EndDateTime"].ToString() + ".000";
                            //string ValidateTime = dr["ValidateTime"].ToString();
                            //string EndDateTime = dr["EndDateTime"].ToString();
                            string Result = "success";
                            string FailReason = "";
                            if (Displaycount.Trim() == "0" && AnnounceCount.Trim() == "0")
                            {
                                Result = "failure";
                                FailReason = "Manually not operated by user";
                            }
                            else
                            {
                                if (Displaycount.Trim() == "0")
                                {
                                    Result = "partial_success";
                                    FailReason = "Display system Network is damage";
                                }
                                else if (AnnounceCount.Trim() == "0")
                                {
                                    Result = "partial_success";
                                    FailReason = "Announcement system Network is damage";
                                }
                            }

                            string CheckCap = await _capInfo.SubmitReport(url, Identifier, ValidateTime, Displaycount, AnnounceCount, Result, FailReason, EndDateTime);
                            Query = "UPDATE CAP set  EvenStatus=4  where  EvenStatus=3";
                            _dbService.ExecuteScalar(Query);
                        }
                        catch (Exception ex)
                        {
                            LoggerService.CapLogger.Error(ex, ex.Message);
                        }
                    }
            }
            catch (Exception ex)
            {
                LoggerService.CapLogger.Error(ex, ex.Message);
            }
        }
        private async Task LoadFromNtesAsync()
        {
            await Task.Run(() =>
            {
                string _ntesResponse = HttpHandler.Instance.GetTrainFromNTES(GlobalData._lstAdminSetting);
                //if (string.IsNullOrEmpty(_ntesResponse))
                //    return;

                if (string.IsNullOrWhiteSpace(_ntesResponse))
                {
                    var failArgs = new eventInfo(
                        eventType: "NtesUpdatedAlert",
                        message: "NTES FAILED AT " + DateTime.Now.ToString("hh:mm:ss tt"),
                        fromWhere: this.Name,
                        status: false
                    );
                    OnEvent?.Invoke(this, failArgs);
                    return;
                }
                Root NtesTrain = JsonConvert.DeserializeObject<Root>(_ntesResponse);

                if (NtesTrain.vTrainList.Count == 0 || NtesTrain == null)
                    return;

                // Build ONE combined "keep list" from both active + cancelled trains
                var allTrainNumbers = NtesTrain.vTrainList
                    .Select(t => t.trainNo)
                    .Concat(NtesTrain.vCancelledTrainList?.Select(t => t.trainNo) ?? Enumerable.Empty<string>())
                    .Where(t => !string.IsNullOrWhiteSpace(t))
                    .Distinct()
                    .ToList();

                if (allTrainNumbers.Count > 0)
                {
                    string inClause = string.Join(",", allTrainNumbers.Select(t => "'" + t.Trim().Replace("'", "''") + "'"));
                    string deleteQuery = $@"DELETE FROM TrainUpdatedData WHERE Train_Number NOT IN ({inClause});";
                    _dbService.ExecuteScalar(deleteQuery);
                }
                // to call it a second time.
                _dbService.InsertTrainEntryBulk(NtesTrain);

                var successArgs = new eventInfo(
                    eventType: "NtesUpdatedAlert",
                    message: "NTES UPDATED AT " + DateTime.Now.ToString("hh:mm:ss tt"),
                    fromWhere: this.Name,
                    status: true
                );
                OnEvent?.Invoke(this, successArgs);
                LoadOnlineTrains();
            });
        }
        private async void btnPlay_Click(object sender, EventArgs e)
        {
            string UserName = GlobalData._lstLoginInfo.FirstOrDefault()?.user_Name;
            string userType = GlobalData.CurrentUserAccessRight?.USER_TYPE ?? "Operator";
            if (GlobalData.CurrentUserAccessRight == null || GlobalData.CurrentUserAccessRight.PLAY_ANNOUNCEMENT != 1)
            {
                btnPlay.Enabled = false;
                btnPause.Enabled = false;
                btnStop.Enabled = false;
                btnCoachAnnouncemet.Enabled = false;
                return;
            }

            try
            {
                if (GlobalData._lstAdminSetting.Auto_Announce == 1)
                {
                    StopAutoAnnouncementTimer();
                }
                if (_manager._isPaused)
                {
                    _manager.Resume();
                    pictureboxwave.Visible = true;
                    BeginInvoke(new Action(() =>
                    {
                        SetMediaButtonsState("PLAYING");
                    }));
                    return;
                }
                _manager._stopRequested = false;
                _manager._isPaused = false;

                SetMediaButtonsState("PLAYING");

                // ✅ NEW — requires BOTH announcement AND TADB enabled
                var playTrains = _dbService.GET_UPDATED_TRAINS_DATA(0)
                    .Where(x => x.ANNOUNCEMENT == 1 && x.TADB_SEND_DATA == 1)
                    .ToList();
                if (!playTrains.Any())
                {
                    MessageBox.Show("Please select a train for announcement.", "Announcement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnPlay.Enabled = true;
                    return;
                }

                foreach (var itemtrain in playTrains.Where(x => x.TRAIN_TYPE == "PAS"))
                {
                    if (itemtrain.TRAIN_STATUS_CODE == "A")
                        itemtrain.TRAIN_TYPE = "ARR";
                    else if (itemtrain.TRAIN_STATUS_CODE == "D")
                        itemtrain.TRAIN_TYPE = "DEP";


                    var announcement = new Announcement
                    {
                        USER_NAME = UserName, // Or get from current user context
                        USER_TYPE = userType, // Or appropriate user type
                        TRAIN_NUMBER = itemtrain.TRAIN_NUMBER,
                        TRAIN_NAME_ENGLISH = itemtrain.TRAIN_NAME_ENGLISH,
                        ARRIVAL_TIME = itemtrain.SCHEDULE_ARRIVAL_TIME,
                        DEPARTURE_TIME = itemtrain.SCHEDULE_DEPARTURE_TIME,
                        TRAIN_STATUS_ENGLISH = itemtrain.TRAIN_STATUS_ENGLISH,
                        PLATFORM_NUMBER = itemtrain.ACTUAL_PLATFORM_NUMBER,
                        TRAIN_DATE = DateTime.Now // Or get from train data if available
                    };

                    _dbService.ADD_ANNOUNCEMENT(announcement);
                }
                pictureboxwave.Visible = true;

                int repeat = int.TryParse(cmb_repeat.Text, out var r) ? r : 1;
                foreach (var itemtrain in playTrains)
                {
                    var announcement = new Announcement
                    {
                        USER_NAME = UserName,
                        USER_TYPE = userType,

                        TRAIN_NUMBER = itemtrain.TRAIN_NUMBER,
                        TRAIN_NAME_ENGLISH = itemtrain.TRAIN_NAME_ENGLISH,

                        ARRIVAL_TIME = itemtrain.SCHEDULE_ARRIVAL_TIME,
                        DEPARTURE_TIME = itemtrain.SCHEDULE_DEPARTURE_TIME,

                        TRAIN_STATUS_ENGLISH = itemtrain.TRAIN_STATUS_ENGLISH,
                        PLATFORM_NUMBER = itemtrain.ACTUAL_PLATFORM_NUMBER,

                        TRAIN_DATE = DateTime.Now
                    };

                    _dbService.ADD_ANNOUNCEMENT(announcement);
                }
                btnCoachAnnouncemet.Enabled = false;

                bool completed = await _manager.RunAnnouncementsAsync(playTrains, repeat, GlobalData._lstAdminSetting.ANNC_LANGUAGE, "regular");



                if (completed)
                {
                    SetMediaButtonsState("STOPPED");
                    pictureboxwave.Visible = false;
                    btnCoachAnnouncemet.Enabled = true;

                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
            finally
            {
                if (_manager._stopRequested)
                    SetMediaButtonsState("STOPPED");

                if (GlobalData._lstAdminSetting.Auto_Announce == 1 && !_manager._isPaused)
                {
                    StartAutoAnnouncementTimer();
                }
            }
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            _manager.Stop();
            SetMediaButtonsState("STOPPED");
            pictureboxwave.Visible = false;

            if (GlobalData._lstAdminSetting.Auto_Announce == 1)
            {
                StartAutoAnnouncementTimer();
            }
        }
        private void btnPause_Click(object sender, EventArgs e)
        {
            _manager.Pause();
            SetMediaButtonsState("PAUSED");
            pictureboxwave.Visible = false;
            if (GlobalData._lstAdminSetting.Auto_Announce == 1)
            {
                StopAutoAnnouncementTimer();
            }

        }
        //private void SetMediaButtonsState(string state)
        //{
        //    if (GlobalData.CurrentUserAccessRight != null && GlobalData.CurrentUserAccessRight.PLAY_ANNOUNCEMENT != 1)
        //    {
        //        btnPlay.Enabled = false;
        //        btnPause.Enabled = false;
        //        btnStop.Enabled = false;
        //        btnCoachAnnouncemet.Enabled = false;
        //        return;
        //    }

        //    switch (state)
        //    {
        //        case "IDLE":
        //            btnPlay.Enabled = true;
        //            btnPause.Enabled = false;
        //            btnStop.Enabled = false;
        //            break;

        //        case "PLAYING":
        //            btnPlay.Enabled = false;
        //            btnPause.Enabled = true;
        //            btnStop.Enabled = true;
        //            break;

        //        case "PAUSED":
        //            btnPlay.Enabled = true;
        //            btnPause.Enabled = false;
        //            btnStop.Enabled = true;
        //            break;

        //        case "STOPPED":
        //            btnPlay.Enabled = true;
        //            btnPause.Enabled = false;
        //            btnStop.Enabled = false;
        //            break;
        //    }
        //}

        private void SetMediaButtonsState(string state)
        {
            if (GlobalData.CurrentUserAccessRight != null &&
                GlobalData.CurrentUserAccessRight.PLAY_ANNOUNCEMENT != 1)
            {
                btnPlay.Enabled = false;
                btnPause.Enabled = false;
                btnStop.Enabled = false;
                btnCoachAnnouncemet.Enabled = false;
                return;
            }

            // ▼▼▼ ADD THESE TWO LINES ▼▼▼
            if (GlobalData._lstAdminSetting.Auto_Announce == 1)
            {
                btnPlay.Enabled = false;
                btnPause.Enabled = false;
                btnStop.Enabled = false;
                btnCoachAnnouncemet.Enabled = false;
                return;
            }
            // ▲▲▲ ADD THESE TWO LINES ▲▲▲

            switch (state)
            {
                case "IDLE": btnPlay.Enabled = true; btnPause.Enabled = false; btnStop.Enabled = false; break;
                case "PLAYING": btnPlay.Enabled = false; btnPause.Enabled = true; btnStop.Enabled = true; break;
                case "PAUSED": btnPlay.Enabled = true; btnPause.Enabled = false; btnStop.Enabled = true; break;
                case "STOPPED": btnPlay.Enabled = true; btnPause.Enabled = false; btnStop.Enabled = false; break;
            }
        }

        private void trackBarVolume_Scroll(object sender, EventArgs e)
        {
            MediaManager.Instance.SetVolume(trackBarVolume.Value);
        }
        private async void btnCoachAnnouncemet_Click(object sender, EventArgs e)
        {
            if (GlobalData.CurrentUserAccessRight == null || GlobalData.CurrentUserAccessRight.PLAY_ANNOUNCEMENT != 1)
            {
                btnCoachAnnouncemet.Enabled = false;
                return;
            }

            var coachTrains = _dbService.GET_UPDATED_TRAINS_DATA(0)
                .Where(x => x.ANNOUNCEMENT == 1 && x.TADB_SEND_DATA == 1)
                .ToList();

            if (!coachTrains.Any())
            {
                MessageBox.Show("Please select a train for announcement.", "Announcement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            btnCoachAnnouncemet.Enabled = false;
            // Prevent manual play/pause/stop from interfering
            btnPlay.Enabled = false;
            btnPause.Enabled = false;
            btnStop.Enabled = false;
            pictureboxwave.Visible = true;
            int repeat = int.TryParse(cmb_repeat.Text, out var r) ? r : 1;

            bool completed = await _manager.RunAnnouncementsAsync(coachTrains, repeat, GlobalData._lstAdminSetting.ANNC_LANGUAGE, "coach");
            if (completed)
            {
                pictureboxwave.Visible = false;
                btnCoachAnnouncemet.Enabled = true;
                btnPlay.Enabled = true;
                btnPause.Enabled = true;
                btnStop.Enabled = true;
            }
        }


        //private void dgvOnlineTrains_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    // Check if the click is on a valid row and column
        //    if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

        //    // Check if the clicked column is the STATUS column
        //    if (dgvOnlineTrains.Columns[e.ColumnIndex].Name == "STATUS")
        //    {

        //        var statusValue = dgvOnlineTrains.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
        //        string trainNumber =
        //    ((train_updated_data)dgvOnlineTrains.Rows[e.RowIndex].DataBoundItem)
        //    .TRAIN_NUMBER;

        //        OpenTrainSearchDialog(statusValue,trainNumber);
        //        this.BeginInvoke(new Action(() => LoadOnlineTrains()));
        //    }
        //}
        private bool OpenTrainSearchDialog(string statusFilter = null, string trainNumber = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(statusFilter) && (statusFilter.ToUpper().Contains("DIVERTED") || statusFilter.ToUpper().Contains("TERMINATED") || statusFilter.ToUpper().Contains("CHANGE OF SOURCE")))
                {
                    List<StationList> stations = _dbService.GET_ALL_STATIONS();

                    using (var dialog = new StationListDialog(stations, trainNumber, isOnlineTrainMode: true))
                    {
                        var result = dialog.ShowDialog(this);
                        return result == DialogResult.OK;
                    }
                }
                if (!string.IsNullOrEmpty(statusFilter) && statusFilter.ToUpper().Contains("MERGE"))
                {
                    using (var dialog = new MergedPos(trainNumber))
                    {
                        var result = dialog.ShowDialog(this);
                        return result == DialogResult.OK;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, "Error opening train search dialog");

            }
            return false;
        }
        private void groupBox_Coach_Enter_1(object sender, EventArgs e)
        {

        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }
        private async Task AutoAnnouncementAsync()
        {
            if (GlobalData._lstAdminSetting.Auto_Announce != 1)
                return;

            if (_isAutoAnnounceRunning)
                return;

            if (_manager._stopRequested || _manager._isPaused)
                return;

            _isAutoAnnounceRunning = true;

            try
            {
                // Get trains marked for announcement
                var playTrains = _dbService.GET_UPDATED_TRAINS_DATA(0).ToList();

                if (!playTrains.Any())
                    return;

                // Update train types for announcement
                foreach (var itemtrain in playTrains.Where(x => x.TRAIN_TYPE == "PAS" || x.TRAIN_TYPE == "PASS"))
                {
                    if (itemtrain.TRAIN_STATUS_CODE == "A")
                        itemtrain.TRAIN_TYPE = "ARR";
                    else if (itemtrain.TRAIN_STATUS_CODE == "D")
                        itemtrain.TRAIN_TYPE = "DEP";
                }

                // Check if there are any trains with A or D status
                bool hasValidTrains = playTrains.Any(x =>
                    x.TRAIN_STATUS_CODE == "A" || x.TRAIN_STATUS_CODE == "D");

                if (!hasValidTrains)
                    return;

                // Start announcement — disable ALL manual buttons
                BeginInvoke(new Action(() =>
                {
                    pictureboxwave.Visible = true;
                    btnPlay.Enabled = false;
                    btnPause.Enabled = false;
                    btnStop.Enabled = false;
                    btnCoachAnnouncemet.Enabled = false;
                }));

                int repeat = int.TryParse(cmb_repeat.Text, out var r) ? r : 1;
                bool completed = await _manager.RunAnnouncementsAsync(
                    playTrains, repeat, GlobalData._lstAdminSetting.ANNC_LANGUAGE, "regular");

                if (completed)
                {
                    BeginInvoke(new Action(() =>
                    {
                        pictureboxwave.Visible = false;
                    }));
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, "Auto Announcement Error: " + ex.Message);
            }
            finally
            {
                _isAutoAnnounceRunning = false;

                // Restore buttons according to access rights
                BeginInvoke(new Action(() =>
                {
                    bool canPlay = GlobalData.CurrentUserAccessRight != null
                                   && GlobalData.CurrentUserAccessRight.PLAY_ANNOUNCEMENT == 1;

                    if (canPlay)
                    {
                        SetMediaButtonsState("IDLE");
                        btnCoachAnnouncemet.Enabled = true;
                    }
                    else
                    {
                        btnPlay.Enabled = false;
                        btnPause.Enabled = false;
                        btnStop.Enabled = false;
                        btnCoachAnnouncemet.Enabled = false;
                    }
                }));
            }
        }
        private void StartAutoAnnouncementTimer()
        {
            if (_timerAutoAnnounce == null)
            {
                _timerAutoAnnounce = new System.Windows.Forms.Timer();
                _timerAutoAnnounce.Tick += async (s, e) =>
                {
                    // Only run if auto-announce is enabled
                    if (GlobalData._lstAdminSetting.Auto_Announce != 1)
                        return;

                    // Don't auto-announce if manual announcement is playing
                    if (_manager._stopRequested || _manager._isPaused)
                        return;

                    await AutoAnnouncementAsync();
                };
            }

            // or use a fixed value
            int intervalSeconds = 10; // Default 30 seconds


            _timerAutoAnnounce.Interval = intervalSeconds * 1000;

            if (!_timerAutoAnnounce.Enabled)
                _timerAutoAnnounce.Start();
        }
        private void StopAutoAnnouncementTimer()
        {
            if (_timerAutoAnnounce != null && _timerAutoAnnounce.Enabled)
            {
                _timerAutoAnnounce.Stop();
            }
        }


        private void CreateCustomLoader()
        {
            _loaderPanel = new Panel
            {
                Name = "loaderPanel",
                Size = new Size(150, 150),
                BackColor = Color.Transparent,  // Changed to Transparent
                Visible = false
            };

            _loaderPictureBox = new PictureBox
            {
                Name = "loaderPictureBox",
                Size = new Size(80, 80),
                Location = new Point(35, 35),
                BackColor = Color.Transparent,
            };

            Label lblLoading = new Label
            {
                Text = "Loading...",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 152, 219),
                AutoSize = true,
                Location = new Point(45, 120),
                BackColor = Color.Transparent
            };

            DrawSpinner(_loaderAngle);

            _loaderPanel.Controls.Add(_loaderPictureBox);
            _loaderPanel.Controls.Add(lblLoading);

            panel1.Controls.Add(_loaderPanel);
            _loaderPanel.BringToFront();

            _loaderAnimationTimer = new System.Windows.Forms.Timer { Interval = 30 };
            _loaderAnimationTimer.Tick += LoaderAnimationTimer_Tick;
        }
        private void DrawSpinner(int angle)
        {
            if (_loaderPictureBox == null) return;

            int size = 80; // Match the PictureBox size
            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.Clear(Color.Transparent);

                int centerX = size / 2;
                int centerY = size / 2;
                int outerRadius = (size / 2) - 5;
                int dotCount = 12;

                for (int i = 0; i < dotCount; i++)
                {
                    // Calculate position
                    double dotAngle = (i / (double)dotCount) * 360 - angle;
                    double radians = dotAngle * Math.PI / 180;

                    int x = (int)(centerX + outerRadius * Math.Cos(radians));
                    int y = (int)(centerY + outerRadius * Math.Sin(radians));

                    // Calculate opacity - create a fading trail effect
                    double opacity = 1.0;
                    if (i < dotCount / 2)
                    {
                        opacity = i / (double)(dotCount / 2);
                    }
                    else
                    {
                        opacity = 1.0 - ((i - dotCount / 2) / (double)(dotCount / 2));
                    }
                    opacity = Math.Max(0.2, opacity);

                    // Size varies based on position
                    int dotSize = (int)(4 + 6 * opacity);
                    int alpha = (int)(255 * opacity);

                    using (SolidBrush brush = new SolidBrush(Color.FromArgb(alpha, _loaderColor)))
                    {
                        g.FillEllipse(brush, x - dotSize / 2, y - dotSize / 2, dotSize, dotSize);
                    }
                }

                // Add a subtle center glow
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddEllipse(centerX - 8, centerY - 8, 16, 16);
                    using (PathGradientBrush brush = new PathGradientBrush(path))
                    {
                        brush.CenterColor = Color.FromArgb(50, _loaderColor);
                        brush.SurroundColors = new Color[] { Color.Transparent };
                        g.FillPath(brush, path);
                    }
                }
            }

            // Dispose old image if exists
            if (_loaderPictureBox.Image != null)
            {
                _loaderPictureBox.Image.Dispose();
            }

            _loaderPictureBox.Image = bmp;
        }

        private void LoaderAnimationTimer_Tick(object sender, EventArgs e)
        {
            _loaderAngle = (_loaderAngle + 8) % 360;
            DrawSpinner(_loaderAngle);
        }

        private void CenterLoader()
        {
            if (_loaderPanel == null || panel1 == null) return;

            // Center the loader panel in the parent container
            int x = (panel1.ClientSize.Width - _loaderPanel.Width) / 2;
            int y = (panel1.ClientSize.Height - _loaderPanel.Height) / 2;
            _loaderPanel.Location = new Point(x, y);
        }


        private void ShowLoader()
        {
            if (_loaderPanel.InvokeRequired)
            {
                _loaderPanel.BeginInvoke(new Action(ShowLoader));
                return;
            }

            CenterLoader();
            _loaderAngle = 0;
            DrawSpinner(_loaderAngle);

            _isLoaderVisible = true;
            _loaderPanel.Visible = true;
            _loaderPanel.BringToFront();

            if (_loaderAnimationTimer != null && !_loaderAnimationTimer.Enabled)
                _loaderAnimationTimer.Start();

            this.Cursor = Cursors.WaitCursor;

        }

        private void HideLoader()
        {
            if (_loaderPanel == null) return;

            if (_loaderPanel.InvokeRequired)
            {
                _loaderPanel.BeginInvoke(new Action(HideLoader));
                return;
            }

            if (_loaderAnimationTimer != null && _loaderAnimationTimer.Enabled)
                _loaderAnimationTimer.Stop();

            _loaderPanel.Visible = false;
            _loaderPanel.SendToBack();

            if (_loaderPictureBox != null && _loaderPictureBox.Image != null)
            {
                _loaderPictureBox.Image.Dispose();
                _loaderPictureBox.Image = null;
            }

            _loaderAngle = 0;
            _isLoaderVisible = false;

            this.Cursor = Cursors.Default;
            // removed: this.Enabled = true;
            // removed: this.Refresh(); / Application.DoEvents();
        }


        private bool IsSystemLocked()
        {
            return GlobalData._lstAdminSetting.Auto_Announce == 1
                && GlobalData._lstAdminSetting.IsNtesAutoUpdate == true;
        }


        private void ApplySystemLockState()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(ApplySystemLockState));
                return;
            }

            bool locked = IsSystemLocked();

            // ----- Grid -----
            dgvOnlineTrains.ReadOnly = locked;
            dgvOnlineTrains.Enabled = true;
            dgvOnlineTrains.BackgroundColor = locked
                ? Color.FromArgb(235, 235, 235)
                : Color.AliceBlue;

            bool canSendTADB_CGDB = GlobalData.CurrentUserAccessRight?.TADB_CGDB_SEND == 1;
            bool canPlayAnnouncement = GlobalData.CurrentUserAccessRight?.PLAY_ANNOUNCEMENT == 1;

            if (locked)
            {
                btn_TADB.Enabled = false;
                btn_CGDB.Enabled = false;
                btnPlay.Enabled = false;
                btnPause.Enabled = false;
                btnStop.Enabled = false;
                btnCoachAnnouncemet.Enabled = false;

                btnLoad.Enabled = false;
                btnSave.Enabled = false;
                btnAdd.Enabled = false;
                btnRemove.Enabled = false;
                btnShiftLeft.Enabled = false;
                btnShiftRight.Enabled = false;
                btnReverse.Enabled = false;
                btn_clear.Enabled = false;

                btnFromNtes.Enabled = false;
                var innerTable = GetInnerTable();
                if (innerTable != null)
                {
                    foreach (Control c in innerTable.Controls)
                        c.Enabled = false;
                }
            }
            else
            {
                btn_TADB.Enabled = canSendTADB_CGDB;
                btn_CGDB.Enabled = canSendTADB_CGDB;

                btnPlay.Enabled = canPlayAnnouncement;
                btnPause.Enabled = false;    // IDLE state
                btnStop.Enabled = false;    // IDLE state
                btnCoachAnnouncemet.Enabled = canPlayAnnouncement;

                // Grid controls always allowed (unless access rights later say otherwise)
                btnLoad.Enabled = true;
                btnSave.Enabled = true;
                btnAdd.Enabled = true;
                btnRemove.Enabled = true;
                btnShiftLeft.Enabled = true;
                btnShiftRight.Enabled = true;
                btnReverse.Enabled = true;
                btn_clear.Enabled = true;

                btnFromNtes.Enabled = true;

                var innerTable = GetInnerTable();
                if (innerTable != null)
                {
                    foreach (Control c in innerTable.Controls)
                        c.Enabled = true;
                }
            }
        }

    }
}


public class RemovedItem
{
    public int Position { get; set; }
    public string Value { get; set; }
}