using DataBaseHandler;
using Global_Data;
using Helper;
using IPISv2;
using LogManager;
using System.Reflection;
using System.Runtime.InteropServices;

namespace IPIS
{
    public partial class Dashboard : Form
    {
        [DllImport("user32.dll")] public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [DllImport("user32.dll")] public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        private System.Windows.Forms.Timer _deviceTimer;
        private bool _islinkCheckingRunning = false;
        private System.Windows.Forms.Timer _intensityTimer;
        private bool _isIntensityRunning = false;
        const int MOD_ALT = 0x1;
        const int MOD_CONTROL = 0x2;
        const int WM_HOTKEY = 0x0312;
        const int HOTKEY_ID = 1;
        private UC_OnlineTrains _ucOnlineTrains;
        private ucLinkStatus _ucLinkStatus;
        private ucPA _ucPA;
        private ucTVDisplay _ucTVDisplay;
        private ucMessage _ucMessage;
        private ucSetUp _ucSetUp;  // Settings
        //private ucInterface _ucInterface;
        private ucAlerts _ucAlerts;
        private ucUserControl _ucUserControl;
        private ucReports _ucReports;
        private ucVDC _ucVDC;
        private ucRemote _ucRemote;
        private ucHSR _ucHSR;
        private ucHelp _ucHelp;
        private static bool _isFirstLoad = true;

        Image imgNormalRefresh;
        Image imgPressedRefresh;

        Image imgNormallogout;
        Image imgPressedlogout;
        Image imgRedDot;
        bool _capServerOk;   // set by the CapStatusAlert event: true when the CAP server returned data
        bool _ntesOk;        // set by the NtesUpdatedAlert event: true after a successful NTES update
        Image imgDotGreen;

        public Dashboard()
        {
            InitializeComponent();

            imgNormalRefresh = Image.FromFile(AppHelper.GetImagePath("reload.png"));
            imgPressedRefresh = Image.FromFile(AppHelper.GetImagePath("refresh.png"));

            pictureBox_refresh.Image = imgNormalRefresh;
            pictureBox_refresh.MouseDown += pictureBox_refresh_MouseDown;
            pictureBox_refresh.MouseUp += pictureBox_refresh_MouseUp;
            pictureBox_refresh.MouseLeave += pictureBox_refresh_MouseLeave;

            imgNormallogout = Image.FromFile(AppHelper.GetImagePath("Normallogout.png"));
            imgPressedlogout = Image.FromFile(AppHelper.GetImagePath("Pressedlogout.png"));
            imgDotGreen = Image.FromFile(AppHelper.GetImagePath("dot_green.png"));
            imgRedDot = Image.FromFile(AppHelper.GetImagePath("dot_red.png"));
            lblActive.Paint += lblActive_PaintDot;   // green dot drawn right after the CDC MASTER/SLAVE text
            lbl_CAP.Paint += lbl_CAP_PaintDot;       // green/red dot after "CAP :" (CAP server data)
            lblNTES.Paint += lblNTES_PaintDot;       // green/red dot after the NTES text

            pictureBox_logout.Image = imgNormallogout;
            pictureBox_logout.MouseDown += pictureBox_logout_MouseDown; 
            pictureBox_logout.MouseUp += pictureBox_logout_MouseUp;
            pictureBox_logout.MouseLeave += pictureBox_logout_MouseLeave;

            _ucOnlineTrains = new UC_OnlineTrains();
            _ucOnlineTrains.OnEvent += Uc_OnDashboardEvent;
            _ucOnlineTrains.OnEvent += OnSettingsEvent;
            _ucOnlineTrains.Dock = DockStyle.Fill;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY && m.WParam.ToInt32() == HOTKEY_ID)
            {
                //ApiServiceManager.StopApiService();
                Application.Exit();   // OnFormClosing stops the sync service
            }
            base.WndProc(ref m);
        }

        private void Uc_OnDashboardEvent(object sender, eventInfo e)
        {
            if (e.eventType == "NtesUpdatedAlert")
            {
                _ntesOk = e.status;   // green dot after "NTES UPDATED AT ...", red after "NTES FAILED AT ..."
                Ipis.Snmp.IpisSnmpAgent.Instance.SetNtesStatus(e.status, e.message);
                if (lblNTES.InvokeRequired)
                {
                    lblNTES.Invoke(new Action(() =>
                    {
                        lblNTES.Text = $"{e.message}";
                        if (e.status)
                            lblNTES.ForeColor = Color.Green;
                        else
                            lblNTES.ForeColor = Color.Red;
                    }));
                }
                else
                {
                    lblNTES.Text = $"{e.message}";
                    if (e.status)
                        lblNTES.ForeColor = Color.Green;
                    else
                        lblNTES.ForeColor = Color.Red;
                }
            }
            else if (e.eventType == "CapStatusAlert")
            {
                // Raised by UC_OnlineTrains after each CAP server check (every 5 minutes).
                _capServerOk = e.status;
                if (lbl_CAP.InvokeRequired)
                    lbl_CAP.Invoke(new Action(() => lbl_CAP.Invalidate()));
                else
                    lbl_CAP.Invalidate();
            }
            else if (e.eventType == "stationNameChangeAlert")
            {
                if (lblStationName.InvokeRequired)
                {
                    lblStationName.Invoke(new Action(() =>
                    {
                        lblStationName.Text = e.message + " STATION PASSENGER INFORMATION SYSTEM";
                    }));
                }
                else
                    lblStationName.Text = e.message + " STATION PASSENGER INFORMATION SYSTEM";
            }
            else if (e.eventType == "systemtype")
            {
                if (lblActive.InvokeRequired)
                {
                    lblActive.Invoke(new Action(() =>
                    {
                        lblActive.Text = Convert.ToInt32(e.message) == 1 ? "CDC MASTER ACTIVE" : "CDC SLAVE ACTIVE";
                    }));
                }
                else
                    lblActive.Text = Convert.ToInt32(e.message) == 1 ? "CDC MASTER ACTIVE" : "CDC SLAVE ACTIVE";
            }
            else if (e.eventType == "modechange")
            {
                if (lblMode.InvokeRequired)
                {
                    lblMode.Invoke(new Action(() =>
                    {
                        lblMode.Text = e.message;
                    }));
                }
                else
                    lblMode.Text = e.message;
            }
        }
        private ucLinkStatus GetLinkStatus()
        {
            if (_ucLinkStatus == null || _ucLinkStatus.IsDisposed)
            {
                _ucLinkStatus = new ucLinkStatus();
                _ucLinkStatus.Dock = DockStyle.Fill;
            }
            return _ucLinkStatus;
        }

        private ucPA GetPA()
        {
            if (_ucPA == null || _ucPA.IsDisposed)
            {
                _ucPA = new ucPA();
                _ucPA.Dock = DockStyle.Fill;
            }
            return _ucPA;
        }

        private ucTVDisplay GetTVDisplay()
        {
            if (_ucTVDisplay == null || _ucTVDisplay.IsDisposed)
            {
                _ucTVDisplay = new ucTVDisplay();
                _ucTVDisplay.Dock = DockStyle.Fill;
            }
            return _ucTVDisplay;
        }

        private ucMessage GetMessage()
        {
            if (_ucMessage == null || _ucMessage.IsDisposed)
            {
                _ucMessage = new ucMessage();
                _ucMessage.Dock = DockStyle.Fill;
            }
            return _ucMessage;
        }
        private ucSetUp GetSetUp()
        {
            if (_ucSetUp == null || _ucSetUp.IsDisposed)
            {
                _ucSetUp = new ucSetUp();
                _ucSetUp.Dock = DockStyle.Fill;
            }
            return _ucSetUp;
        }

        private ucAlerts GetAlerts()
        {
            if (_ucAlerts == null || _ucAlerts.IsDisposed)
            {
                _ucAlerts = new ucAlerts();
                _ucAlerts.Dock = DockStyle.Fill;
            }
            return _ucAlerts;
        }

        private ucUserControl GetUserControl()
        {
            if (_ucUserControl == null || _ucUserControl.IsDisposed)
            {
                _ucUserControl = new ucUserControl();
                _ucUserControl.Dock = DockStyle.Fill;
            }
            return _ucUserControl;
        }

        private ucReports GetReports()
        {
            if (_ucReports == null || _ucReports.IsDisposed)
            {
                _dbService.Maintain45DaysEventLog();
                _ucReports = new ucReports();
                _ucReports.Dock = DockStyle.Fill;
            }
            return _ucReports;
        }

        private ucVDC GetVDC()
        {
            if (_ucVDC == null || _ucVDC.IsDisposed)
            {
                _ucVDC = new ucVDC();
                _ucVDC.Dock = DockStyle.Fill;
            }
            return _ucVDC;
        }

        private ucHSR GetHSR()
        {
            if (_ucHSR == null || _ucHSR.IsDisposed)
            {
                _ucHSR = new ucHSR();
                _ucHSR.Dock = DockStyle.Fill;
            }
            return _ucHSR;
        }

        private ucHelp GetHelp()
        {
            if (_ucHelp == null || _ucHelp.IsDisposed)
            {
                _ucHelp = new ucHelp();
                _ucHelp.Dock = DockStyle.Fill;
            }
            return _ucHelp;
        }

        private void ShowUserControl(UserControl uc)
        {
            // Hide all controls instead of destroying them
            foreach (Control c in panelMain.Controls)
            {
                c.Visible = false;
            }

            // Add if not already in panel
            if (!panelMain.Controls.Contains(uc))
            {
                uc.Dock = DockStyle.Fill;
                panelMain.Controls.Add(uc);
            }

            uc.Visible = true;
            uc.BringToFront();
            panelMain.PerformLayout();

        }

        private IPISv2.Controls.AppSplashOverlay _splashOverlay;

        private void HideSplashOverlay()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => HideSplashOverlay()));
                return;
            }

            if (_splashOverlay != null && !_splashOverlay.IsDisposed)
            {
                _splashOverlay.Stop();
                this.Controls.Remove(_splashOverlay);
                _splashOverlay.Dispose();
                _splashOverlay = null;
                _isFirstLoad = false;
            }

            // Splash running on its own thread (see Dashboard_Load).
            if (_isFirstLoad)
            {
                IPISv2.Controls.SplashScreenHost.Close();
                _isFirstLoad = false;
                this.Activate();
            }
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            //lbl_CAP.Text = "CAP :" "Active";
            lbl_CAP.Text = "CAP :";
            lbUserName.Text = "USER NAME : " + GlobalData._lstLoginInfo.FirstOrDefault()?.user_Name;

            if (_isFirstLoad)
            {
                //_splashOverlay = new IPISv2.Controls.AppSplashOverlay();
                //_splashOverlay.Dock = DockStyle.Fill;
                //this.Controls.Add(_splashOverlay);
                //this.Controls.SetChildIndex(_splashOverlay, 0);
                //_splashOverlay.BringToFront();
                //_splashOverlay.Start();
                //_splashOverlay.SetStatus("Loading application data");
                //_splashOverlay.Refresh();

                // The overlay lived on this (main) thread, so its dots froze while the Online Trains
                // grid was built here. The splash now runs on its own thread and keeps animating.
                IPISv2.Controls.SplashScreenHost.Show();

                if (_ucOnlineTrains != null)
                {
                    _ucOnlineTrains.OnInitialLoadComplete += (s, ev) =>
                    {
                        HideSplashOverlay();
                    };
                }
                else
                {
                    HideSplashOverlay();
                }
            }

            // Show Online Trains (lazy loaded)
            ShowUserControl(_ucOnlineTrains);

            lblDateTime.Text = DateTime.Now.ToString("MMMM dd, yyyy - hh:mm:ss tt");
            LoggerService.Logger.Information("IPIS STARTED!" + DateTime.Now);
            lblActive.Text = GlobalData._lstAdminSetting.Ismaster == 1 ? "CDC MASTER ACTIVE" : "CDC SLAVE ACTIVE";
            string stationName = GlobalData._lstAdminSetting.RUNNING_STATION_NAME;
            lblStationName.Text = stationName + " STATION PASSENGER INFORMATION SYSTEM";
            lblMode.Text = GlobalData._lstAdminSetting.IsNtesAutoUpdate == true ? "(COPILOT)" : "(MANUAL)";
            lblconnected_device.Text = "CONNECTED DEVICE 0:0";

            ApplyTabAccessRights();

            // Day/night intensity: applied now and re-checked every minute so the
            // switch happens at the configured times without anyone opening a form.
            StartAutoIntensityMonitor();

            tabControl_main.SizeMode = TabSizeMode.Fixed;
            tabControl_main.DrawMode = TabDrawMode.OwnerDrawFixed;
            typeof(Control).GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.SetValue(tabControl_main, true, null);
            tabControl_main.DrawItem += tabControl_main_DrawItem;
            tabControl_main.Selecting += tabControl_main_Selecting;
            tabControl_main.MouseMove += tabControl_main_MouseMove;
            tabControl_main.MouseLeave += tabControl_main_MouseLeave;
            tabControl_main.SizeChanged += (s, ev) => AdjustTabWidth();
            this.BeginInvoke(new Action(() => { AdjustTabWidth(); }));

            if (GlobalData._lstAdminSetting.AUTO_LINK_CHECK > 0)
                StartConnectedDeviceMonitor(10000);
            else
                ConnectedDeviceAsync();
        }

        private void ApplyTabAccessRights()
        {
            if (tab_VDC != null)
                tab_VDC.Enabled = (GlobalData.CurrentUserAccessRight != null && GlobalData.CurrentUserAccessRight.VDC == 1);
            if (tab_links != null)
                tab_links.Enabled = (GlobalData.CurrentUserAccessRight != null && GlobalData.CurrentUserAccessRight.HUB_CONFIGURATION == 1);
            if (tab_UserControl != null)
                tab_UserControl.Enabled = (GlobalData.CurrentUserAccessRight != null && GlobalData.CurrentUserAccessRight.USER_SETTING == 1);
        }

        private bool IsTabDisabled(TabPage page)
        {
            if (page == null) return false;

            if (page.Name == "tab_VDC")
            {
                return GlobalData.CurrentUserAccessRight == null || GlobalData.CurrentUserAccessRight.VDC != 1;
            }
            else if (page.Name == "tab_links")
            {
                return GlobalData.CurrentUserAccessRight == null || GlobalData.CurrentUserAccessRight.HUB_CONFIGURATION != 1;
            }
            else if (page.Name == "tab_UserControl")
            {
                return GlobalData.CurrentUserAccessRight == null || GlobalData.CurrentUserAccessRight.USER_SETTING != 1;
            }
            return !page.Enabled;
        }


        private void tabControl_main_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedTab = tabControl_main.SelectedTab;

            if (selectedTab.Name == "tab_Onlinetrain")
            {
                ShowUserControl(_ucOnlineTrains);
            }
            else if (selectedTab.Name == "tab_links")
                ShowUserControl(GetLinkStatus());
            else if (selectedTab.Name == "tab_PA")
                ShowUserControl(GetPA());
            else if (selectedTab.Name == "tab_TVDisplay")
                ShowUserControl(GetTVDisplay());
            else if (selectedTab.Name == "tab_Message")
                ShowUserControl(GetMessage());
            else if (selectedTab.Name == "tab_Settings")
                ShowUserControl(GetSetUp());
            else if (selectedTab.Name == "tab_cap")
                ShowUserControl(GetAlerts());
            //else if (selectedTab.Name == "tab_Interface")
            //    ShowUserControl(GetInterface());
            else if (selectedTab.Name == "tab_UserControl")
                ShowUserControl(GetUserControl());
            else if (selectedTab.Name == "tab_Reports")
                ShowUserControl(GetReports());
            else if (selectedTab.Name == "tab_HSR")
                ShowUserControl(GetHSR());
            else if (selectedTab.Name == "tab_VDC")
                ShowUserControl(GetVDC());
            else if (selectedTab.Name == "tab_Help")
                ShowUserControl(GetHelp());
        }

        private void tabControl_main_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == null) return;

            if (IsTabDisabled(e.TabPage))
            {
                e.TabPage.Enabled = false;
                e.Cancel = true;
            }
        }

        private void AdjustTabWidth()
        {
            int tabCount = tabControl_main.TabCount;
            if (tabCount == 0) return;
            int width = (tabControl_main.Width - 25) / tabCount;
            if (width <= 0)
                width = 50;
            if (tabControl_main.ItemSize.Width == width)
                return;
            tabControl_main.ItemSize = new Size(width, 34);
        }

        private void timerDateTime_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = DateTime.Now.ToString("MMMM dd, yyyy - hh:mm:ss tt");
        }

        private void pictureBox_logout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to log out?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                int LogedInUserId = Config.currentlyloggedInid;
                _dbService.AddLogOutHistory(LogedInUserId);
                this.Hide();

                ApiServiceManager.StopApiService();

                Application.Exit();   // OnFormClosing stops the sync service
            }
        }

        private void pictureBox_refresh_Click(object sender, EventArgs e)
        {
            var selectedTab = tabControl_main.SelectedTab;
            if (selectedTab == null) return;

            switch (selectedTab.Name)
            {
                case "tab_Onlinetrain":
                    if (_ucOnlineTrains != null && !_ucOnlineTrains.IsDisposed)
                    {
                        _ucOnlineTrains.LoadOnlineTrains();
                    }
                    break;

                case "tab_links":
                    if (_ucLinkStatus != null && !_ucLinkStatus.IsDisposed)
                    {
                        _ucLinkStatus.RefreshData();
                    }
                    break;

                case "tab_PA":
                    if (_ucPA != null && !_ucPA.IsDisposed)
                    {
                        _ucPA.RefreshData();
                    }
                    break;

                case "tab_TVDisplay":
                    if (_ucTVDisplay != null && !_ucTVDisplay.IsDisposed)
                    {
                        _ucTVDisplay.RefreshData();
                    }
                    break;

                case "tab_Message":
                    if (_ucMessage != null && !_ucMessage.IsDisposed)
                    {
                        _ucMessage.RefreshData();
                    }
                    break;

                case "tab_cap":
                    if (_ucAlerts != null && !_ucAlerts.IsDisposed)
                    {
                        _ucAlerts.RefreshData();
                    }
                    break;

                case "tab_UserControl":
                    if (_ucUserControl != null && !_ucUserControl.IsDisposed)
                    {
                        _ucUserControl.RefreshData();
                    }
                    break;

                case "tab_Reports":
                    if (_ucReports != null && !_ucReports.IsDisposed)
                    {
                        _ucReports.RefreshData();
                    }
                    break;

                case "tab_VDC":
                    if (_ucVDC != null && !_ucVDC.IsDisposed)
                    {
                        _ucVDC.LoadTrueColorData();
                    }
                    break;
                case "tab_Settings":
                    if (_ucSetUp != null && !_ucSetUp.IsDisposed)
                    {
                        _ucSetUp.RefreshData();
                    }
                    break;
            }
        }

        private void pictureBox_refresh_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox_refresh.Image = imgPressedRefresh;
        }

        private void pictureBox_refresh_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox_refresh.Image = imgNormalRefresh;
        }

        private void pictureBox_refresh_MouseLeave(object sender, EventArgs e)
        {
            pictureBox_refresh.Image = imgNormalRefresh;
        }

        private void pictureBox_logout_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox_logout.Image = imgPressedlogout;
        }

        private void pictureBox_logout_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox_logout.Image = imgNormallogout;
        }

        // Draws the green dot just after lblActive's centred text, so it follows the text
        // when it changes between CDC MASTER ACTIVE and CDC SLAVE ACTIVE.
        private void lblActive_PaintDot(object sender, PaintEventArgs e)
        {
            DrawDotAfterText(lblActive, imgDotGreen, e.Graphics);
        }

        // CAP : green dot when the last CAP server check returned data, red until then or when it did not.
        private void lbl_CAP_PaintDot(object sender, PaintEventArgs e)
        {
            DrawDotAfterText(lbl_CAP, _capServerOk ? imgDotGreen : imgRedDot, e.Graphics);
        }

        // NTES: red dot until an NTES update succeeds, green after a successful update, red after a failed one.
        private void lblNTES_PaintDot(object sender, PaintEventArgs e)
        {
            DrawDotAfterText(lblNTES, _ntesOk ? imgDotGreen : imgRedDot, e.Graphics);
        }

        // Draws a status dot 4 px after a label's text (vertically centred); works for left- and centre-aligned text.
        private static void DrawDotAfterText(Label label, Image dot, Graphics g)
        {
            if (dot == null || string.IsNullOrEmpty(label.Text))
                return;

            Size textSize = TextRenderer.MeasureText(g, label.Text, label.Font);
            int textRight;
            switch (label.TextAlign)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.BottomLeft:
                    textRight = label.Padding.Left + textSize.Width;
                    break;
                case ContentAlignment.TopRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.BottomRight:
                    textRight = label.ClientSize.Width - label.Padding.Right;
                    break;
                default:
                    textRight = (label.ClientSize.Width + textSize.Width) / 2;
                    break;
            }

            int x = Math.Min(textRight + 4, label.ClientSize.Width - dot.Width);
            int y = (label.ClientSize.Height - dot.Height) / 2;

            g.DrawImage(dot, x, y, dot.Width, dot.Height);
        }

        private void pictureBox_logout_MouseLeave(object sender, EventArgs e)
        {
            pictureBox_logout.Image = imgNormallogout;
        }

        private void StartAutoIntensityMonitor()
        {
            if (_intensityTimer != null)
                return;

            _intensityTimer = new System.Windows.Forms.Timer();
            _intensityTimer.Interval = 60000;
            _intensityTimer.Tick += async (s, e) =>
            {
                if (_isIntensityRunning) return;
                _isIntensityRunning = true;
                try
                {
                    // DB round trip off the UI thread; the service is locked internally.
                    await Task.Run(() => ucInterfaceAdminSetting.ApplyAutoIntensity());
                }
                catch (Exception ex)
                {
                    LoggerService.Logger.Error(ex, ex.Message);
                }
                finally
                {
                    _isIntensityRunning = false;
                }
            };
            _intensityTimer.Start();

       
            Task.Run(() =>
            {
                try { ucInterfaceAdminSetting.ApplyAutoIntensity(); }
                catch (Exception ex) { LoggerService.Logger.Error(ex, ex.Message); }
            });
        }

        private void StartConnectedDeviceMonitor(int interval)
        {
            if (_deviceTimer != null)
            {
                _deviceTimer.Interval = interval;
                if (!_deviceTimer.Enabled)
                    _deviceTimer.Start();
                return;
            }
            _deviceTimer = new System.Windows.Forms.Timer();
            _deviceTimer.Interval = interval;
            _deviceTimer.Tick += async (s, e) =>
            {
                if (_islinkCheckingRunning) return;
                _islinkCheckingRunning = true;
                try
                {
                    await ConnectedDeviceAsync();
                }
                catch (Exception ex)
                {
                    LoggerService.Logger.Error(ex, ex.Message);
                }
                finally
                {
                    _islinkCheckingRunning = false;
                }
            };
            interval = 30000;
            _deviceTimer.Start();
        }

        private async Task ConnectedDeviceAsync()
        {
            var allips = await Task.Run(() => _dbService.GET_ALL_DEVICE_IPS());

            if (allips == null || allips.Count == 0)
            {
                UpdateLabel(0, 0);
                return;
            }

            int total = allips.Count;
            var semaphore = new SemaphoreSlim(20);
            var tasks = allips.Select(async ip =>
            {
                await semaphore.WaitAsync();
                try
                {
                    return await PingHelper.PingWithRetryAsync(ip);
                }
                finally
                {
                    semaphore.Release();
                }
            });

            var results = await Task.WhenAll(tasks);
            int connected = results.Count(x => x.IsSuccess);
            UpdateLabel(connected, total);
        }

        private void UpdateLabel(int connected, int total)
        {
            if (lblconnected_device.IsDisposed) return;

            if (lblconnected_device.InvokeRequired)
            {
                lblconnected_device.Invoke(new Action(() =>
                {
                    lblconnected_device.Text = $"CONNECTED DEVICE {connected}:{total}";
                }));
            }
            else
            {
                lblconnected_device.Text = $"CONNECTED DEVICE {connected}:{total}";
            }
        }

        public void OnSettingsEvent(object sender, eventInfo e)
        {
            if (e.eventType != "autolinkcheck")
                return;

            int interval = 30000;
            if (GlobalData._lstAdminSetting.AUTO_LINK_CHECK > 0)
                StartConnectedDeviceMonitor(interval);
            else
            {
                if (_deviceTimer != null)
                    _deviceTimer.Stop();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _deviceTimer?.Stop();
            _intensityTimer?.Stop();
            UnregisterHotKey(this.Handle, HOTKEY_ID);
            this.Hide();

            // Clean up event subscriptions
            if (_ucOnlineTrains != null && !_ucOnlineTrains.IsDisposed)
            {
                _ucOnlineTrains.OnEvent -= Uc_OnDashboardEvent;
                _ucOnlineTrains.OnEvent -= OnSettingsEvent;
                //_ucOnlineTrains.Dispose();
            }

            GlobalData.StopSyncAsync();
         
            ApiServiceManager.StopApiService(TimeSpan.FromSeconds(1), onlyIfStartedByApplication: true);
            base.OnFormClosing(e);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            RegisterHotKey(this.Handle,
                           HOTKEY_ID,
                           MOD_CONTROL | MOD_ALT,
                           (int)Keys.C);
        }

        private int _hoveredTabIndex = -1;

        private void tabControl_main_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (tabControl_main.TabCount == 0) return;
            if (e.Index < 0 || e.Index >= tabControl_main.TabCount) return;

            TabPage page = tabControl_main.TabPages[e.Index];
            Rectangle rect = tabControl_main.GetTabRect(e.Index);

            bool isActive = tabControl_main.SelectedIndex == e.Index;
            bool isHovered = e.Index == _hoveredTabIndex;
            bool isDisabled = IsTabDisabled(page);

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            Color backColor;
            Color textColor;
            Color borderColor;

            if (isDisabled)
            {
                backColor = Color.FromArgb(220, 224, 230);
                textColor = Color.FromArgb(150, 155, 165);
                borderColor = Color.FromArgb(195, 200, 208);
            }
            else if (isActive)
            {
                backColor = Color.FromArgb(0, 51, 153);
                textColor = Color.White;
                borderColor = Color.FromArgb(0, 40, 120);
            }
            else if (isHovered)
            {
                backColor = Color.FromArgb(190, 210, 245);
                textColor = Color.FromArgb(0, 51, 153);
                borderColor = Color.FromArgb(150, 180, 230);
            }
            else
            {
                backColor = Color.FromArgb(235, 240, 248);
                textColor = Color.FromArgb(64, 74, 90);
                borderColor = Color.FromArgb(210, 218, 230);
            }

            using (SolidBrush backBrush = new SolidBrush(backColor))
            {
                e.Graphics.FillRectangle(backBrush, rect);
            }

            using (Pen borderPen = new Pen(borderColor, 1.5f))
            {
                Rectangle borderRect = new Rectangle(rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
                e.Graphics.DrawRectangle(borderPen, borderRect);
            }

            Font font = new Font("Segoe UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point);
            TextFormatFlags flags = TextFormatFlags.HorizontalCenter |
                                    TextFormatFlags.VerticalCenter |
                                    TextFormatFlags.EndEllipsis |
                                    TextFormatFlags.SingleLine;

            TextRenderer.DrawText(e.Graphics, page.Text, font, rect, textColor, flags);
            font.Dispose();
        }

        private void tabControl_main_MouseMove(object sender, MouseEventArgs e)
        {
            int hoverIndex = -1;
            for (int i = 0; i < tabControl_main.TabCount; i++)
            {
                if (tabControl_main.GetTabRect(i).Contains(e.Location))
                {
                    hoverIndex = i;
                    break;
                }
            }

            if (hoverIndex >= 0 && hoverIndex < tabControl_main.TabCount)
            {
                TabPage page = tabControl_main.TabPages[hoverIndex];
                if (IsTabDisabled(page))
                {
                    tabControl_main.Cursor = Cursors.No;
                }
                else
                {
                    tabControl_main.Cursor = Cursors.Default;
                }
            }
            else
            {
                tabControl_main.Cursor = Cursors.Default;
            }

            if (hoverIndex != _hoveredTabIndex)
            {
                int oldIndex = _hoveredTabIndex;
                _hoveredTabIndex = hoverIndex;

                if (oldIndex >= 0 && oldIndex < tabControl_main.TabCount)
                {
                    tabControl_main.Invalidate(tabControl_main.GetTabRect(oldIndex));
                }
                if (hoverIndex >= 0 && hoverIndex < tabControl_main.TabCount)
                {
                    tabControl_main.Invalidate(tabControl_main.GetTabRect(hoverIndex));
                }
            }
        }

        private void tabControl_main_MouseLeave(object sender, EventArgs e)
        {
            tabControl_main.Cursor = Cursors.Default;
            if (_hoveredTabIndex != -1)
            {
                int oldIndex = _hoveredTabIndex;
                _hoveredTabIndex = -1;
                if (oldIndex >= 0 && oldIndex < tabControl_main.TabCount)
                {
                    tabControl_main.Invalidate(tabControl_main.GetTabRect(oldIndex));
                }
            }
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e) { }

        private void flowLayoutPanel1_Paint_1(object sender, PaintEventArgs e) { }


    }
}
