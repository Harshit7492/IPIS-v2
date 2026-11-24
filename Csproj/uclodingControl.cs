using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPIS_V1
{
    public partial class uclodingControl : UserControl
    {
        public uclodingControl()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
            this.Visible = false;
        }
        public void ShowMessage(string message = "Please wait...")
        {
            lblMessage.Text = message;
            this.Visible = true;
            this.BringToFront();
        }

        public void HideLoading()
        {
            this.Visible = false;
        }
    }
}
