using Global_Data;
using Helper;
using IPISv2;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPIS
{
    public partial class ucInterface : UserControl
    {
        public event EventHandler<eventInfo> OnEvent;
        public ucInterface()
        {
            InitializeComponent();
        }

        private void LoadUserControl(UserControl uc)
        {
            panelRightInterface.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            panelRightInterface.Controls.Add(uc);
        }


        private void AllLED_Click(object sender, EventArgs e)
        {

        }

      



        private void btnAdminSetting_Click(object sender, EventArgs e)
        {
            if (GlobalData.CurrentUserAccessRight == null || GlobalData.CurrentUserAccessRight.ADMIN_SETTINGS != 1)
            {
                MessageBox.Show("You do not have access rights for Admin Settings.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var adminSettingUC = new ucInterfaceAdminSetting();
            adminSettingUC.OnEvent += AdminUC_OnEvent;
            LoadUserControl(adminSettingUC);
        }
        private void AdminUC_OnEvent(object sender, eventInfo e)
        {
            OnEvent?.Invoke(this, e);
        }
        private void btnRedundantPC_Click(object sender, EventArgs e)
        {
            LoadUserControl(new ucInterfaceRedundantPC());
        }

        private void btnBackupDatabase_Click(object sender, EventArgs e)
        {

        }

        private void panelMainContent_Paint(object sender, PaintEventArgs e)
        {

        } 
       
    }
}
