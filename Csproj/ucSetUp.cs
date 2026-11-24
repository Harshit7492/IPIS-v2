
using Global_Data;
using IPISv2;
using IPISv2.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace IPIS
{
    public partial class ucSetUp : UserControl
    {
        private Dictionary<Button, Func<UserControl>> _tabMapping;

        private readonly Dictionary<Button, UserControl> _cachedControls = new Dictionary<Button, UserControl>();

        private Button _activeButton = null;

        public ucSetUpBoardSetting BoardSettingControl => GetOrCreateControl(btnBoardSetting) as ucSetUpBoardSetting;

        public ucSetUp()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            InitializeTabMapping();
        }

        private void ucSetUp_Load(object sender, EventArgs e)
        {
            ApplyAccessRights();
        }

        private void ApplyAccessRights()
        {
            btnTrainEntry.Enabled = true;

            if (GlobalData.CurrentUserAccessRight != null)
            {
                btnEnableBoards.Enabled = GlobalData.CurrentUserAccessRight.TRAIN_ENTRY == 1;
                btnBoardSetting.Enabled = GlobalData.CurrentUserAccessRight.HUB_CONFIGURATION == 1;
                adminSetting.Enabled = GlobalData.CurrentUserAccessRight.ADMIN_SETTINGS == 1;
            }
            else
            {
                btnEnableBoards.Enabled = false;
                btnBoardSetting.Enabled = false;
                adminSetting.Enabled = false;
            }
        }

        public void InitializeTabMapping()
        {
            // Define which button spins up which UserControl
            _tabMapping = new Dictionary<Button, Func<UserControl>>
            {
                { btnTrainEntry, () => new ucSetUpTrainEntry() },
                { btnCoachLegend, () => new ucSetUpCoachLegend() },
                { btnStationCode, () => new ucSetUpStationCode() },
                { btnBoardSetting, () => new ucSetUpBoardSetting() },
                { adminSetting, () => new ucInterfaceAdminSetting() },
                { btnTrainMasking, () => new ucTrainMasking() },
                { btnVendorIntegration, () => new ucVendorDetails()},
                //{ btnUpdateExe,         () => new ucUpdateExe()}
                {btnSetGetConfig, ()=> new  GetSetConfigurations()}

            };
        }

        /// <summary>
        /// Unified routing method for all tab switches. 
        /// </summary>
        private void HandleTabSwitch(Button clickedButton)
        {
            if (clickedButton == null || !clickedButton.Enabled) return;

            // 1. Optimize UI Highlight (O(1) swap instead of looping all controls)
            if (_activeButton != null)
            {
                _activeButton.BackColor = Color.AliceBlue;
                _activeButton.ForeColor = Color.DarkBlue;
            }

            _activeButton = clickedButton;
            _activeButton.BackColor = Color.LightSteelBlue; // Light, clean selection color
            _activeButton.ForeColor = Color.MidnightBlue;

            // 2. Lazy load or fetch cached control
            UserControl targetControl = GetOrCreateControl(clickedButton);
            if (targetControl != null)
            {
                RenderControl(targetControl);
            }
        }

        private UserControl GetOrCreateControl(Button button)
        {
            if (!_tabMapping.ContainsKey(button)) return null;

            if (!_cachedControls.ContainsKey(button))
            {
                _cachedControls[button] = _tabMapping[button]();
            }
            return _cachedControls[button];
        }

        private void RenderControl(UserControl uc)
        {
            panelMenuRight.SuspendLayout();

            // Clear previous controls safely (removes active UserControl or the initial splash screen panel)
            panelMenuRight.Controls.Clear();

            uc.Dock = DockStyle.Fill;
            panelMenuRight.Controls.Add(uc);
            uc.BringToFront();

            panelMenuRight.ResumeLayout(true);
        }
        private void btnVendorIntegration_Click(object sender, EventArgs e)
        {
            HandleTabSwitch(btnVendorIntegration);
        }
        //private void btnUpdateExe_Click(object sender, EventArgs e)
        //{
        //    HandleTabSwitch(btnUpdateExe);
        //}
        #region Designer Bound Events (Do Not Delete)

        private void btnTrainEntry_Click(object sender, EventArgs e)
        {
            //HandleTabSwitch(btnTrainEntry);
            using (var passwordForm = new PasswordVerificationForm())
            {
                if (passwordForm.ShowDialog(this) == DialogResult.OK)
                {
                    HandleTabSwitch(btnTrainEntry);
                }
            }

        }


        private void btnCoachLegend_Click(object sender, EventArgs e)
        {
            HandleTabSwitch(btnCoachLegend);
        }

        private void btnStationCode_Click(object sender, EventArgs e)
        {
            HandleTabSwitch(btnStationCode);
        }
        private void LoadUserControl(UserControl uc)
        {
            uc.Dock = DockStyle.Fill;
        }

        private void adminSetting_Click(object sender, EventArgs e)
        {
            HandleTabSwitch(adminSetting);

        }

        private void btnBoardSetting_Click(object sender, EventArgs e)
        {
            HandleTabSwitch(btnBoardSetting);
        }

        private void btnEnableBoards_Click(object sender, EventArgs e)
        {
            if (!btnEnableBoards.Enabled) return;
            var trainEntry = GetOrCreateControl(btnTrainEntry) as ucSetUpTrainEntry ?? new ucSetUpTrainEntry();
            trainEntry.btnImportJson_Click(sender, e);
        }
        private void btnTrainMasking_Click(object sender, EventArgs e)
        {
            HandleTabSwitch(btnTrainMasking);
        }

        private void btnSetGetConfig_Click(object sender, EventArgs e)
        {
            HandleTabSwitch(btnSetGetConfig);
        }

        public void RefreshData()
        {
            if (_activeButton == btnCoachLegend && _cachedControls.TryGetValue(btnCoachLegend, out var uc2) && uc2 is ucSetUpCoachLegend coachLegend)
            {
                coachLegend.RefreshData();
            }
            else if (_activeButton == btnStationCode && _cachedControls.TryGetValue(btnStationCode, out var uc3) && uc3 is ucSetUpStationCode stationCode)
            {
                stationCode.RefreshData();
            }
            else if (_activeButton == btnBoardSetting && _cachedControls.TryGetValue(btnBoardSetting, out var uc4) && uc4 is ucSetUpBoardSetting boardSetting)
            {
                boardSetting.RefreshData();
            }
            else if (_activeButton == adminSetting && _cachedControls.TryGetValue(adminSetting, out var uc5) && uc5 is ucInterfaceAdminSetting admin)
            {
                admin.RefreshData();
            }
            else if (_activeButton == btnSetGetConfig && _cachedControls.TryGetValue(btnSetGetConfig, out var uc6) && uc6 is GetSetConfigurations getSetConfig)
            {
                getSetConfig.RefreshData();
            }
        }

        #endregion
    }
}