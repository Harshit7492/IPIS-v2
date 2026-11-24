using DataBaseHandler;
using DataModel;
using Global_Data;
using Helper;
using LogManager;
using System.Globalization;
using System.Text.RegularExpressions;

namespace IPIS
{
    public partial class ucInterfaceAdminSetting : UserControl
    {
        public event EventHandler<eventInfo> OnEvent;

        private TimeSpan _defaultMaintStart = new TimeSpan(1, 0, 0);
        private TimeSpan _defaultMaintEnd = new TimeSpan(3, 0, 0);
        private TimeSpan? _customMaintStart;
        private TimeSpan? _customMaintEnd;
        private static readonly object _maintGate = new object();
        private static System.Threading.Timer _maintTimer;
        private static DateTime _maintLastFiredDate = DateTime.MinValue;
        private static int _maintRunning;                        // re-entrancy guard
        private static TimeSpan _maintEnd = new TimeSpan(3, 0, 0);
        private static DateTime _maintCfgLoadedAt = DateTime.MinValue;
        private const int MaintCatchUpMinutes = 60;              // still run if up to 60 min late
        public ucInterfaceAdminSetting()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
        }

        private void ucInterfaceAdminSetting_Load(object sender, EventArgs e)
        {
            LoadAdminSettings();
            StartMaintenanceScheduler();
        }

        private void LoadAdminSettings()
        {
            cmb_maxcoach.SelectedIndex = 0;
            cmb_maxPlateform.SelectedIndex = 0;
            cmb_maxEthernetport.SelectedIndex = 0;
            cmbRegLang.SelectedIndex = 0;
            cmbAutoUpdateTime.SelectedIndex = 0;
            cmbDayIntensity.SelectedIndex = 0;
            cmbNightIntensity.SelectedIndex = 0;

            var setting = _dbService.GET_ADMIN_SETTING();
            if (setting == null) return;

            cmb_maxcoach.Text = setting.Total_Coach.ToString();
            cmb_maxPlateform.Text = setting.TOTAL_PLATFORM_DISPLAYED.ToString();
            cmb_maxEthernetport.Text = setting.Ethernet_Port.ToString();
            cmbRegLang.Text = setting.STATION_LANGUAGES.Split('+').LastOrDefault();
            cmbAnnounceLanguage.Text = setting.ANNC_LANGUAGE;
            txtStationName.Text = setting.RUNNING_STATION_NAME;
            txtStationCode.Text = setting.STATION_CODE;
            txtPFEnd1.Text = setting.CAP_PASSKEY;
            txtPFEnd2.Text = setting.CAP_Name;
            txtPassKey.Text = setting.PASS_KEY;
            chk_Autolinkcheck.Checked = Convert.ToBoolean(setting.AUTO_LINK_CHECK);
            chk_StartUp.Checked = true;
            chk_IsMaster.Checked = Convert.ToBoolean(setting.Ismaster);
            chk_AutoAnnc.Checked = Convert.ToBoolean(setting.Auto_Announce);
            chk_resetstate.Checked = Convert.ToBoolean(setting.RESET_STATE);
            chk_NtesAutoUpdate.Checked = Convert.ToBoolean(setting.IsNtesAutoUpdate);
            chk_AutoIntensity.Checked = Convert.ToBoolean(setting.AUTO_INTENSITY);
            mskSetDay.Text = setting.SET_Day_Time;
            mskSetNight.Text = setting.SET_Night_Time;
            cmbDayIntensity.Text = setting.SET_Day_Intensity;
            cmbNightIntensity.Text = setting.SET_Night_Intensity;
            cmbAutoUpdateTime.Text = setting.NtesTimeMin.ToString();

            _defaultMaintStart = setting.DEFAULT_MAINT_START ?? new TimeSpan(1, 0, 0);
            _defaultMaintEnd = setting.DEFAULT_MAINT_END ?? new TimeSpan(3, 0, 0);
            _customMaintStart = setting.MAINT_START_TIME;
            _customMaintEnd = setting.MAINT_END_TIME;

            txtDefaultTime.Text = $"{DateTime.Today.Add(_defaultMaintStart).ToString("hh:mm tt", CultureInfo.InvariantCulture)} - {DateTime.Today.Add(_defaultMaintEnd).ToString("hh:mm tt", CultureInfo.InvariantCulture)}";

            TimeSpan startDisplay = setting.USE_DEFAULT_MAINT ? _defaultMaintStart : (_customMaintStart ?? _defaultMaintStart);
            TimeSpan endDisplay = setting.USE_DEFAULT_MAINT ? _defaultMaintEnd : (_customMaintEnd ?? _defaultMaintEnd);

            mskMaintStart.Text = startDisplay.ToString(@"hh\:mm");
            mskMaintEnd.Text = endDisplay.ToString(@"hh\:mm");

            cmbMaintMode.SelectedIndex = setting.USE_DEFAULT_MAINT ? 0 : 1;
            UpdateMaintControls();
        }

        private void cmbMaintMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateMaintControls();
        }

        private void UpdateMaintControls()
        {
            bool isDefault = cmbMaintMode.SelectedIndex == 0;
            if (isDefault)
            {
                if (TryParse24h(mskMaintStart.Text, out TimeSpan curStart))
                    _customMaintStart = curStart;
                if (TryParse24h(mskMaintEnd.Text, out TimeSpan curEnd))
                    _customMaintEnd = curEnd;

                mskMaintStart.Text = _defaultMaintStart.ToString(@"hh\:mm");
                mskMaintEnd.Text = _defaultMaintEnd.ToString(@"hh\:mm");
                mskMaintStart.Enabled = false;
                mskMaintEnd.Enabled = false;
            }
            else
            {
                mskMaintStart.Enabled = true;
                mskMaintEnd.Enabled = true;
                if (_customMaintStart.HasValue)
                    mskMaintStart.Text = _customMaintStart.Value.ToString(@"hh\:mm");
                if (_customMaintEnd.HasValue)
                    mskMaintEnd.Text = _customMaintEnd.Value.ToString(@"hh\:mm");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbAnnounceLanguage.SelectedIndex == 0)
            {
                MessageBox.Show("Select Announcement Language!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbRegLang.SelectedIndex == 0)
            {
                MessageBox.Show("Select Regional Language!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (string.IsNullOrEmpty(txtStationName.Text))
            {
                MessageBox.Show("Input Station Name!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (string.IsNullOrEmpty(txtStationCode.Text))
            {
                MessageBox.Show("Input Station Code!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (string.IsNullOrEmpty(txtPFEnd1.Text))
            {
                MessageBox.Show("CAP Pass Key  Required !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (string.IsNullOrEmpty(txtPFEnd2.Text))
            {
                MessageBox.Show("CAP Password Required !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (string.IsNullOrWhiteSpace(mskSetDay.Text.Replace(":", "").Trim()))
            {
                MessageBox.Show("Input Day Time!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mskSetDay.Focus();
                return;
            }
            else if (string.IsNullOrWhiteSpace(mskSetNight.Text.Replace(":", "").Trim()))
            {
                MessageBox.Show("Input Night Time!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mskSetNight.Focus();
                return;
            }
            else if (!TryParse24h(mskSetDay.Text, out TimeSpan dayTs))
            {
                MessageBox.Show("Please enter a valid Day Time in 24-hour clock format.", "Invalid Day Time", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mskSetDay.Focus();
                return;
            }
            else if (!TryParse24h(mskSetNight.Text, out TimeSpan nightTs))
            {
                MessageBox.Show("Please enter a valid Night Time in 24-hour clock format.", "Invalid Night Time", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mskSetNight.Focus();
                return;
            }

            bool useDefaultMaint = (cmbMaintMode.SelectedIndex == 0);
            TimeSpan maintStart;
            TimeSpan maintEnd;

            if (useDefaultMaint)
            {
                maintStart = _defaultMaintStart;
                maintEnd = _defaultMaintEnd;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(mskMaintStart.Text.Replace(":", "").Trim()))
                {
                    MessageBox.Show("Input Maintenance Start Time!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    mskMaintStart.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(mskMaintEnd.Text.Replace(":", "").Trim()))
                {
                    MessageBox.Show("Input Maintenance End Time!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    mskMaintEnd.Focus();
                    return;
                }

                if (!TryParse24h(mskMaintStart.Text, out maintStart))
                {
                    MessageBox.Show("Please enter a valid Maintenance Start Time in 24-hour clock format.", "Invalid Maintenance Start Time", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    mskMaintStart.Focus();
                    return;
                }

                if (!TryParse24h(mskMaintEnd.Text, out maintEnd))
                {
                    MessageBox.Show("Please enter a valid Maintenance End Time in 24-hour clock format.", "Invalid Maintenance End Time", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    mskMaintEnd.Focus();
                    return;
                }
            }

            var setting = new AdminSetting
            {
                AUTO_LINK_CHECK = chk_Autolinkcheck.Checked ? 1 : 0,
                RUNNING_STATION_NAME = txtStationName.Text.ToUpper(),
                ANNC_LANGUAGE = cmbAnnounceLanguage.Text,
                TOTAL_PLATFORM_DISPLAYED = Convert.ToInt32(cmb_maxPlateform.Text),
                RESET_STATE = 0,
                CHECK_SCHEDULE = "0",
                AUTO_INTENSITY = chk_AutoIntensity.Checked ? 1 : 0,
                SET_Day_Time = mskSetDay.Text,
                SET_Night_Time = mskSetNight.Text,
                SET_Day_Intensity = cmbDayIntensity.Text,
                SET_Night_Intensity = cmbNightIntensity.Text,
                Ethernet_Port = Convert.ToInt32(cmb_maxEthernetport.Text),
                Auto_Announce = chk_AutoAnnc.Checked ? 1 : 0,
                Total_Coach = Convert.ToInt32(cmb_maxcoach.Text),
                PASS_KEY = txtPassKey.Text,
                STATION_CODE = txtStationCode.Text.ToUpper(),
                NtesTimeMin = Convert.ToInt32(cmbAutoUpdateTime.Text),
                IsNtesAutoUpdate = Convert.ToBoolean(chk_NtesAutoUpdate.Checked),
                CAP_PASSKEY = txtPFEnd1.Text,
                CAP_Name = txtPFEnd2.Text,
                Ismaster = chk_IsMaster.Checked ? 1 : 0,
                STATION_LANGUAGES = "Hindi+English+" + cmbRegLang.Text,
                DEFAULT_MAINT_START = _defaultMaintStart,
                DEFAULT_MAINT_END = _defaultMaintEnd,
                MAINT_START_TIME = maintStart,
                MAINT_END_TIME = maintEnd,
                USE_DEFAULT_MAINT = useDefaultMaint
            };

            bool success = _dbService.UpdateAdminSetting(setting);

            if (!success)
            {
                MessageBox.Show("Unable to save settings.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            StartMaintenanceScheduler();
            ApplyAutoIntensity(setting);

            // Station name change event
            var args = new eventInfo( eventType: "stationNameChangeAlert", message: txtStationName.Text.ToUpper(), fromWhere: this.Name, status: true);

            if (GlobalData._lstAdminSetting.RUNNING_STATION_NAME != txtStationName.Text)
                OnEvent?.Invoke(this, args);

            // Master / Slave change event
            args = new eventInfo(
                eventType: "systemtype",
                message: setting.Ismaster.ToString(),
                fromWhere: this.Name,
                status: true);

            if (GlobalData._lstAdminSetting.Ismaster != setting.Ismaster)
                OnEvent?.Invoke(this, args);

            // Update global configuration
            GlobalData.SetConfig(Config.currentlyloggedInid);

            // NTES auto update event
            args = new eventInfo( eventType: "ntsAutoupdate", message: string.Empty, fromWhere: this.Name, status: true);

            OnEvent?.Invoke(this, args);

            // Mode change event
            string _modetype = chk_NtesAutoUpdate.Checked
                ? "(COPILOT)"
                : "(MANUAL)";

            args = new eventInfo( eventType: "modechange", message: _modetype, fromWhere: this.Name, status: true);

            OnEvent?.Invoke(this, args);

            // Auto link check event
            args = new eventInfo( eventType: "autolinkcheck", message: Convert.ToString(setting.AUTO_LINK_CHECK), fromWhere: this.Name, status: true);

            OnEvent?.Invoke(this, args);

            // Show restart message only once
            DialogResult result = MessageBox.Show(
                "Settings saved successfully.\n\n" +
                "The application needs to restart for the changes to take effect.\n\n" +
                "Click OK to restart the application.",
                "Restart Required",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            if (result == DialogResult.OK)
            {
                Program.RestartApplication();
                return;
            }
        }

        private void chk_NtesAutoUpdate_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_NtesAutoUpdate.Checked)
            {
                cmbAutoUpdateTime.Enabled = true;
            }
            else
            {
                cmbAutoUpdateTime.Enabled = true;
                cmbAutoUpdateTime.SelectedIndex = 0;
            }
        }

        public void RefreshData()
        {
            LoadAdminSettings();
        }

        private static readonly object _intensityGate = new object();

        public static void ApplyAutoIntensity(AdminSetting setting = null)
        {
            lock (_intensityGate)
            {
                try
                {
                    if (setting == null)
                    {
                        setting = _dbService.GET_ADMIN_SETTING();
                        if (setting != null)
                            GlobalData._lstAdminSetting = setting;
                        else
                            setting = GlobalData._lstAdminSetting;
                    }

                    if (setting == null || setting.AUTO_INTENSITY != 1)
                        return;

                    TimeSpan dayTime = new TimeSpan(6, 0, 0);
                    TimeSpan nightTime = new TimeSpan(20, 30, 0);

                    if (TryParseClock(setting.SET_Day_Time, out TimeSpan dt))
                        dayTime = dt;
                    if (TryParseClock(setting.SET_Night_Time, out TimeSpan nt))
                        nightTime = nt;

                    if (nightTime.Hours > 0 && nightTime.Hours < 12)
                        nightTime = nightTime.Add(TimeSpan.FromHours(12));

                    TimeSpan now = DateTime.Now.TimeOfDay;
                    bool isDay = dayTime < nightTime
                        ? (now >= dayTime && now < nightTime)
                        : (now >= dayTime || now < nightTime);

                    string raw = isDay ? setting.SET_Day_Intensity : setting.SET_Night_Intensity;
                    string digits = (raw ?? string.Empty).Trim().TrimEnd('%').Trim();
                    if (!int.TryParse(digits, NumberStyles.Integer, CultureInfo.InvariantCulture, out int intensity)
                        || intensity < 0 || intensity > 100)
                    {
                        LoggerService.SettingsLogger.Warning(
                            "Auto intensity: {Mode} intensity '{Raw}' is not a number, nothing applied.",
                            isDay ? "day" : "night", raw);
                        return;
                    }

                    string sql =
                        $"UPDATE BOARDSETTING SET Intensity = '{intensity}' " +
                        $"WHERE ISNULL(CAST(Intensity AS NVARCHAR(10)), '') <> '{intensity}';";

                    int rows = _dbService.ExecuteNonQuery(sql);

                    if (rows < 0)
                        LoggerService.SettingsLogger.Warning("Auto intensity: BOARDSETTING update failed.");
                    else if (rows > 0)
                        LoggerService.SettingsLogger.Information(
                            "Auto intensity: {Mode} -> {Intensity} applied to {Rows} board(s) (day {Day}, night {Night}).",
                            isDay ? "DAY" : "NIGHT", intensity, rows,
                            dayTime.ToString(@"hh\:mm"), nightTime.ToString(@"hh\:mm"));
                }
                catch (Exception ex)
                {
                    LoggerService.SettingsLogger.Error(ex, "Auto intensity: failed to apply.");
                }
            }
        }

        private static bool TryParseClock(string text, out TimeSpan value)
        {
            value = default;
            if (string.IsNullOrWhiteSpace(text))
                return false;
            text = text.Trim();

            // TimeSpan.TryParse("0830") reads 830 DAYS, hence the < 1 day check.
            if (TimeSpan.TryParse(text, CultureInfo.InvariantCulture, out value) && value < TimeSpan.FromDays(1))
                return true;

            if (DateTime.TryParseExact(text, new[] { "h:mm tt", "hh:mm tt", "h:mmtt", "HHmm", "Hmm" },
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsed))
            {
                value = parsed.TimeOfDay;
                return true;
            }
            return false;
        }

        private static bool TryParse24h(string text, out TimeSpan value)
        {
            value = default;
            if (string.IsNullOrWhiteSpace(text)) return false;

            var m = Regex.Match(text.Trim(), @"^([01]?\d|2[0-3]):([0-5]\d)$");
            if (!m.Success) return false;

            value = new TimeSpan(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value), 0);
            return true;
        }


        private async void btnGetLatestLedMatrix_Click(object sender, EventArgs e)
        {
            try
            {
                btnGetLatestLedMatrix.Enabled = false;
                btnGetLatestLedMatrix.Text = "Loading...";

                int result = await Task.Run(() => _dbService.GetMaintinanceDetails());

                if (result > 0)
                {
                    MessageBox.Show( "Latest LED matrix details retrieved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show( "Unable to retrieve the latest LED matrix details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                LoggerService.SettingsLogger.Error(ex, "Failed to get the latest LED matrix.");

                MessageBox.Show("Unable to get the latest LED matrix.","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            finally
            {
                btnGetLatestLedMatrix.Enabled = true;
                btnGetLatestLedMatrix.Text = "Get Latest Matrix";
            }
        }

        public static void StartMaintenanceScheduler()
        {
            lock (_maintGate)
            {
                _maintCfgLoadedAt = DateTime.MinValue;           // force reload of mode/end time

                if (_maintTimer != null) return;

                _maintTimer = new System.Threading.Timer(
                    OnMaintenanceTick, null,
                    TimeSpan.FromSeconds(5),                     // first check soon after start
                    TimeSpan.FromSeconds(30));                   // then every 30 seconds

                LoggerService.SettingsLogger.Information("Maintenance timer: started.");
            }
        }

        private static void OnMaintenanceTick(object state)
        {
            if (Interlocked.CompareExchange(ref _maintRunning, 1, 0) != 0)
                return;                                         

            try
            {
                DateTime now = DateTime.Now;

                if ((now - _maintCfgLoadedAt) > TimeSpan.FromMinutes(5))
                {
                    var setting = _dbService.GET_ADMIN_SETTING();
                    if (setting == null) return;                 

                    TimeSpan defEnd = setting.DEFAULT_MAINT_END ?? new TimeSpan(3, 0, 0);
                    _maintEnd = setting.USE_DEFAULT_MAINT ? defEnd : (setting.MAINT_END_TIME ?? defEnd);
                    _maintCfgLoadedAt = now;

                    LoggerService.SettingsLogger.Information(
                        "Maintenance timer: mode={Mode}, end time {End}",
                        setting.USE_DEFAULT_MAINT ? "DEFAULT" : "CUSTOM", _maintEnd.ToString(@"hh\:mm"));
                }

                if (_maintLastFiredDate == now.Date) return;     

                TimeSpan sinceEnd = now.TimeOfDay - _maintEnd;
                if (sinceEnd < TimeSpan.Zero || sinceEnd > TimeSpan.FromMinutes(MaintCatchUpMinutes))
                    return;                                     

                _maintLastFiredDate = now.Date;

                int result = _dbService.GetMaintinanceDetails();

                if (result > 0)
                    LoggerService.SettingsLogger.Information("Maintenance timer: LED matrix details retrieved ({Result}).", result);
                else
                    LoggerService.SettingsLogger.Warning("Maintenance timer: GetMaintinanceDetails returned {Result}.", result);
            }
            catch (Exception ex)
            {
                LoggerService.SettingsLogger.Error(ex, "Maintenance timer: tick failed.");
            }
            finally
            {
                Interlocked.Exchange(ref _maintRunning, 0);
            }
        }

        public static void StopMaintenanceScheduler()
        {
            lock (_maintGate)
            {
                _maintTimer?.Dispose();
                _maintTimer = null;
            }
        }
    }
}
