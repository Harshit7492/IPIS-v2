using Helper;
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
    public partial class ucInterfaceRedundantPC : UserControl
    {
        public ucInterfaceRedundantPC()
        {
            InitializeComponent();
            this.Load += ucInterfaceRedundantPC_Load;
        }
        private void ucInterfaceRedundantPC_Load(object sender, EventArgs e)
        {


            if (Config.MasterIP.Contains("253"))
            {
                lblmachineIP.Text = Config.MasterIP;
                lblMachinetype.Text = "Primary Server";
            }
            else
            {
                lblmachineIP.Text = Config.SlaveIP;
                lblMachinetype.Text = "Secondary Server";
            }
        }
    }
}
