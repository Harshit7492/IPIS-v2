using DataBaseHandler;
using Helper;

namespace IPIS
{
    partial class Dashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                int LogedInUserId = Config.currentlyloggedInid;
                _dbService.AddLogOutHistory(LogedInUserId);
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Dashboard));
            timerDateTime = new System.Windows.Forms.Timer(components);
            tableLayoutDashboardMain = new TableLayoutPanel();
            pnlHeader = new Panel();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            lblMode = new Label();
            lblStationName = new Label();
            picboxLogo = new PictureBox();
            pictureBox_logout = new PictureBox();
            pictureBox_refresh = new PictureBox();
            tableLayoutPanel3 = new TableLayoutPanel();
            lblconnected_device = new Label();
            lbUserName = new Label();
            lblDateTime = new Label();
            lblNTES = new Label();
            lbl_CAP = new Label();
            lblActive = new Label();
            panelMain = new Panel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            tableLayoutPanel4 = new TableLayoutPanel();
            tabControl_main = new TabControl();
            tab_Onlinetrain = new TabPage();
            tab_links = new TabPage();
            tab_PA = new TabPage();
            tab_Message = new TabPage();
            tab_Settings = new TabPage();
            tab_UserControl = new TabPage();
            tab_cap = new TabPage();
            tab_Reports = new TabPage();
            tab_HSR = new TabPage();
            tab_VDC = new TabPage();
            tab_Help = new TabPage();
            tableLayoutDashboardMain.SuspendLayout();
            pnlHeader.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picboxLogo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_logout).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_refresh).BeginInit();
            tableLayoutPanel3.SuspendLayout();
            panelMain.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            tabControl_main.SuspendLayout();
            SuspendLayout();
            // 
            // timerDateTime
            // 
            timerDateTime.Enabled = true;
            timerDateTime.Interval = 1000;
            timerDateTime.Tick += timerDateTime_Tick;
            // 
            // tableLayoutDashboardMain
            // 
            tableLayoutDashboardMain.ColumnCount = 1;
            tableLayoutDashboardMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutDashboardMain.Controls.Add(pnlHeader, 0, 0);
            tableLayoutDashboardMain.Controls.Add(panelMain, 0, 2);
            tableLayoutDashboardMain.Controls.Add(tableLayoutPanel4, 0, 1);
            tableLayoutDashboardMain.Dock = DockStyle.Fill;
            tableLayoutDashboardMain.Location = new Point(0, 0);
            tableLayoutDashboardMain.Margin = new Padding(4, 5, 4, 5);
            tableLayoutDashboardMain.Name = "tableLayoutDashboardMain";
            tableLayoutDashboardMain.RowCount = 3;
            tableLayoutDashboardMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 157F));
            tableLayoutDashboardMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 75F));
            tableLayoutDashboardMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 190F));
            tableLayoutDashboardMain.Size = new Size(1514, 877);
            tableLayoutDashboardMain.TabIndex = 1;
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.LightSkyBlue;
            pnlHeader.Controls.Add(tableLayoutPanel1);
            pnlHeader.Dock = DockStyle.Fill;
            pnlHeader.Location = new Point(4, 5);
            pnlHeader.Margin = new Padding(4, 5, 4, 5);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1506, 147);
            pnlHeader.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.Transparent;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10.3734436F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 89.62656F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(4, 3, 4, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 67.52137F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 32.4786339F));
            tableLayoutPanel1.Size = new Size(1506, 147);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.BackColor = Color.Transparent;
            tableLayoutPanel2.ColumnCount = 5;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.7139959F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 72.46088F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.528156F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 3.09891629F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 3.19805884F));
            tableLayoutPanel2.Controls.Add(lblMode, 2, 0);
            tableLayoutPanel2.Controls.Add(lblStationName, 1, 0);
            tableLayoutPanel2.Controls.Add(picboxLogo, 0, 0);
            tableLayoutPanel2.Controls.Add(pictureBox_logout, 4, 0);
            tableLayoutPanel2.Controls.Add(pictureBox_refresh, 3, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.ForeColor = Color.Transparent;
            tableLayoutPanel2.Location = new Point(4, 3);
            tableLayoutPanel2.Margin = new Padding(4, 3, 4, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(1498, 93);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // lblMode
            // 
            lblMode.AutoSize = true;
            lblMode.Dock = DockStyle.Fill;
            lblMode.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMode.ForeColor = Color.Tomato;
            lblMode.Location = new Point(1279, 0);
            lblMode.Margin = new Padding(4, 0, 4, 0);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(119, 93);
            lblMode.TabIndex = 17;
            lblMode.Text = "MANUAL";
            lblMode.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblStationName
            // 
            lblStationName.AutoSize = true;
            lblStationName.BackColor = Color.Transparent;
            lblStationName.Dock = DockStyle.Fill;
            lblStationName.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblStationName.ForeColor = SystemColors.Desktop;
            lblStationName.Location = new Point(194, 0);
            lblStationName.Margin = new Padding(4, 0, 4, 0);
            lblStationName.Name = "lblStationName";
            lblStationName.Size = new Size(1077, 93);
            lblStationName.TabIndex = 12;
            lblStationName.Text = "NEW DELHI STATION PASSENGER INFORMATION SYSTEM";
            lblStationName.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // picboxLogo
            // 
            picboxLogo.BackColor = Color.Transparent;
            picboxLogo.Dock = DockStyle.Fill;
            picboxLogo.Image = (Image)resources.GetObject("picboxLogo.Image");
            picboxLogo.Location = new Point(0, 0);
            picboxLogo.Margin = new Padding(0);
            picboxLogo.Name = "picboxLogo";
            picboxLogo.Size = new Size(190, 93);
            picboxLogo.SizeMode = PictureBoxSizeMode.StretchImage;
            picboxLogo.TabIndex = 11;
            picboxLogo.TabStop = false;
            // 
            // pictureBox_logout
            // 
            pictureBox_logout.Dock = DockStyle.Fill;
            pictureBox_logout.Image = (Image)resources.GetObject("pictureBox_logout.Image");
            pictureBox_logout.InitialImage = null;
            pictureBox_logout.Location = new Point(1452, 5);
            pictureBox_logout.Margin = new Padding(4, 5, 4, 5);
            pictureBox_logout.Name = "pictureBox_logout";
            pictureBox_logout.Size = new Size(42, 83);
            pictureBox_logout.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox_logout.TabIndex = 15;
            pictureBox_logout.TabStop = false;
            pictureBox_logout.Click += pictureBox_logout_Click;
            // 
            // pictureBox_refresh
            // 
            pictureBox_refresh.Dock = DockStyle.Fill;
            pictureBox_refresh.Image = (Image)resources.GetObject("pictureBox_refresh.Image");
            pictureBox_refresh.Location = new Point(1406, 5);
            pictureBox_refresh.Margin = new Padding(4, 5, 4, 5);
            pictureBox_refresh.Name = "pictureBox_refresh";
            pictureBox_refresh.Size = new Size(38, 83);
            pictureBox_refresh.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox_refresh.TabIndex = 16;
            pictureBox_refresh.TabStop = false;
            pictureBox_refresh.Click += pictureBox_refresh_Click;
            pictureBox_refresh.MouseDown += pictureBox_refresh_MouseDown;
            pictureBox_refresh.MouseUp += pictureBox_refresh_MouseUp;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 6;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel3.Controls.Add(lblconnected_device, 1, 0);
            tableLayoutPanel3.Controls.Add(lbUserName, 0, 0);
            tableLayoutPanel3.Controls.Add(lblDateTime, 5, 0);
            tableLayoutPanel3.Controls.Add(lblNTES, 4, 0);
            //tableLayoutPanel3.Controls.Add(lbl_version, 3, 0);
            tableLayoutPanel3.Controls.Add(lbl_CAP, 3, 0);
            tableLayoutPanel3.Controls.Add(lblActive, 2, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(4, 102);
            tableLayoutPanel3.Margin = new Padding(4, 3, 4, 3);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new Size(1498, 42);
            tableLayoutPanel3.TabIndex = 1;
            // 
            // lblconnected_device
            // 
            lblconnected_device.AutoSize = true;
            lblconnected_device.Dock = DockStyle.Fill;
            lblconnected_device.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblconnected_device.ForeColor = Color.Tomato;
            lblconnected_device.Location = new Point(253, 0);
            lblconnected_device.Margin = new Padding(4, 0, 4, 0);
            lblconnected_device.Name = "lblconnected_device";
            lblconnected_device.Size = new Size(241, 42);
            lblconnected_device.TabIndex = 16;
            lblconnected_device.Text = " -----------";
            lblconnected_device.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbUserName
            // 
            lbUserName.AutoSize = true;
            lbUserName.Dock = DockStyle.Fill;
            lbUserName.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbUserName.ForeColor = Color.Tomato;
            lbUserName.Location = new Point(4, 0);
            lbUserName.Margin = new Padding(4, 0, 4, 0);
            lbUserName.Name = "lbUserName";
            lbUserName.Size = new Size(241, 42);
            lbUserName.TabIndex = 6;
            lbUserName.Text = " -----------";
            lbUserName.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblDateTime
            // 
            lblDateTime.AutoSize = true;
            lblDateTime.Dock = DockStyle.Fill;
            lblDateTime.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDateTime.ForeColor = Color.Tomato;
            lblDateTime.Location = new Point(1249, 0);
            lblDateTime.Margin = new Padding(4, 0, 4, 0);
            lblDateTime.Name = "lblDateTime";
            lblDateTime.Size = new Size(245, 42);
            lblDateTime.TabIndex = 14;
            lblDateTime.Text = "August 12, 2025 - 11:41:19 AM";
            lblDateTime.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblNTES
            // 
            lblNTES.AutoSize = true;
            lblNTES.Dock = DockStyle.Fill;
            lblNTES.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNTES.ForeColor = Color.Tomato;
            lblNTES.Location = new Point(1000, 0);
            lblNTES.Margin = new Padding(4, 0, 4, 0);
            lblNTES.Name = "lblNTES";
            lblNTES.Size = new Size(241, 42);
            lblNTES.TabIndex = 9;
            lblNTES.Text = "NTES :";
            lblNTES.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lbl_version
            // 
            lbl_CAP.AutoSize = true;
            lbl_CAP.Dock = DockStyle.Fill;
            lbl_CAP.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_CAP.ForeColor = Color.Tomato;
            lbl_CAP.Location = new Point(751, 0);
            lbl_CAP.Margin = new Padding(4, 0, 4, 0);
            lbl_CAP.Name = "lbl_CAP";
            lbl_CAP.Size = new Size(241, 42);
            lbl_CAP.TabIndex = 15;
            lbl_CAP.Text = " -----------";
            lbl_CAP.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblActive
            // 
            lblActive.AutoSize = true;
            lblActive.Dock = DockStyle.Fill;
            lblActive.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblActive.ForeColor = Color.Tomato;
            lblActive.Location = new Point(502, 0);
            lblActive.Margin = new Padding(4, 0, 4, 0);
            lblActive.Name = "lblActive";
            lblActive.Size = new Size(241, 42);
            lblActive.TabIndex = 7;
            lblActive.Text = "CDC MASTER ACTIVE";
            lblActive.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelMain
            // 
            panelMain.BackColor = Color.White;
            panelMain.Controls.Add(flowLayoutPanel1);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(4, 237);
            panelMain.Margin = new Padding(4, 5, 4, 5);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(1506, 635);
            panelMain.TabIndex = 3;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Margin = new Padding(4, 5, 4, 5);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(1506, 635);
            flowLayoutPanel1.TabIndex = 1;
            flowLayoutPanel1.WrapContents = false;
            flowLayoutPanel1.Paint += flowLayoutPanel1_Paint_1;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 1;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Controls.Add(tabControl_main, 0, 0);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(4, 162);
            tableLayoutPanel4.Margin = new Padding(4, 5, 4, 5);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 1;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
            tableLayoutPanel4.Size = new Size(1506, 65);
            tableLayoutPanel4.TabIndex = 4;
            // 
            // tabControl_main
            // 
            tabControl_main.Appearance = TabAppearance.Buttons;
            tabControl_main.Controls.Add(tab_Onlinetrain);
            tabControl_main.Controls.Add(tab_links);
            tabControl_main.Controls.Add(tab_PA);
            tabControl_main.Controls.Add(tab_Message);
            tabControl_main.Controls.Add(tab_Settings);
            tabControl_main.Controls.Add(tab_UserControl);
            tabControl_main.Controls.Add(tab_cap);
            tabControl_main.Controls.Add(tab_Reports);
            tabControl_main.Controls.Add(tab_HSR);
            tabControl_main.Controls.Add(tab_VDC);
            tabControl_main.Controls.Add(tab_Help);
            tabControl_main.Dock = DockStyle.Fill;
            tabControl_main.Font = new Font("Cambria", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tabControl_main.Location = new Point(4, 5);
            tabControl_main.Margin = new Padding(4, 5, 4, 5);
            tabControl_main.Name = "tabControl_main";
            tabControl_main.Padding = new Point(30, 5);
            tabControl_main.SelectedIndex = 0;
            tabControl_main.ShowToolTips = true;
            tabControl_main.Size = new Size(1498, 55);
            tabControl_main.TabIndex = 0;
            tabControl_main.SelectedIndexChanged += tabControl_main_SelectedIndexChanged;
            // 
            // tab_Onlinetrain
            // 
            tab_Onlinetrain.Font = new Font("Cambria", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tab_Onlinetrain.Location = new Point(4, 44);
            tab_Onlinetrain.Margin = new Padding(4, 5, 4, 5);
            tab_Onlinetrain.Name = "tab_Onlinetrain";
            tab_Onlinetrain.Padding = new Padding(4, 5, 4, 5);
            tab_Onlinetrain.Size = new Size(1490, 7);
            tab_Onlinetrain.TabIndex = 0;
            tab_Onlinetrain.Text = "Online Trains";
            tab_Onlinetrain.UseVisualStyleBackColor = true;
            // 
            // tab_links
            // 
            tab_links.Location = new Point(4, 44);
            tab_links.Margin = new Padding(4, 5, 4, 5);
            tab_links.Name = "tab_links";
            tab_links.Padding = new Padding(4, 5, 4, 5);
            tab_links.Size = new Size(1489, 7);
            tab_links.TabIndex = 1;
            tab_links.Text = "Links";
            tab_links.ToolTipText = "Link Status";
            tab_links.UseVisualStyleBackColor = true;
            // 
            // tab_PA
            // 
            tab_PA.Location = new Point(4, 44);
            tab_PA.Margin = new Padding(4, 5, 4, 5);
            tab_PA.Name = "tab_PA";
            tab_PA.Size = new Size(1489, 7);
            tab_PA.TabIndex = 2;
            tab_PA.Text = "PA";
            tab_PA.ToolTipText = "Public Address";
            tab_PA.UseVisualStyleBackColor = true;
            // 
            // tab_Message
            // 
            tab_Message.Location = new Point(4, 44);
            tab_Message.Margin = new Padding(4, 5, 4, 5);
            tab_Message.Name = "tab_Message";
            tab_Message.Size = new Size(1489, 7);
            tab_Message.TabIndex = 3;
            tab_Message.Text = "Messages";
            tab_Message.UseVisualStyleBackColor = true;
            // 
            // tab_Settings
            // 
            tab_Settings.Location = new Point(4, 44);
            tab_Settings.Margin = new Padding(4, 5, 4, 5);
            tab_Settings.Name = "tab_Settings";
            tab_Settings.Size = new Size(1489, 7);
            tab_Settings.TabIndex = 4;
            tab_Settings.Text = "Settings";
            tab_Settings.UseVisualStyleBackColor = true;

            // 
            // tab_UserControl
            // 
            tab_UserControl.Location = new Point(4, 44);
            tab_UserControl.Margin = new Padding(4, 5, 4, 5);
            tab_UserControl.Name = "tab_UserControl";
            tab_UserControl.Size = new Size(1489, 7);
            tab_UserControl.TabIndex = 6;
            tab_UserControl.Text = "User Controls";
            tab_UserControl.UseVisualStyleBackColor = true;
            // 
            // tab_cap
            // 
            tab_cap.Location = new Point(4, 44);
            tab_cap.Margin = new Padding(4, 5, 4, 5);
            tab_cap.Name = "tab_cap";
            tab_cap.Size = new Size(1489, 7);
            tab_cap.TabIndex = 7;
            tab_cap.Text = "CAP";
            tab_cap.ToolTipText = "Common Alerting Protocol (CAP) Alerts";
            tab_cap.UseVisualStyleBackColor = true;
            // 
            // tab_Reports
            // 
            tab_Reports.Location = new Point(4, 44);
            tab_Reports.Margin = new Padding(4, 5, 4, 5);
            tab_Reports.Name = "tab_Reports";
            tab_Reports.Size = new Size(1489, 7);
            tab_Reports.TabIndex = 8;
            tab_Reports.Text = "Reports";
            tab_Reports.UseVisualStyleBackColor = true;
            // 
            // tab_HSR
            // 
            tab_HSR.Location = new Point(4, 44);
            tab_HSR.Margin = new Padding(4, 5, 4, 5);
            tab_HSR.Name = "tab_HSR";
            tab_HSR.Size = new Size(1489, 7);
            tab_HSR.TabIndex = 9;
            tab_HSR.Text = "HSR";
            tab_HSR.ToolTipText = "Health Status Report";
            tab_HSR.UseVisualStyleBackColor = true;
            // 
            // tab_VDC
            // 
            tab_VDC.Location = new Point(4, 44);
            tab_VDC.Margin = new Padding(4, 5, 4, 5);
            tab_VDC.Name = "tab_VDC";
            tab_VDC.Size = new Size(1489, 7);
            tab_VDC.TabIndex = 10;
            tab_VDC.Text = "VDC";
            tab_VDC.ToolTipText = "Video Display Control";
            tab_VDC.UseVisualStyleBackColor = true;
            // 
            // tab_Help
            // 
            tab_Help.Location = new Point(4, 44);
            tab_Help.Margin = new Padding(4, 5, 4, 5);
            tab_Help.Name = "tab_Help";
            tab_Help.Size = new Size(1489, 7);
            tab_Help.TabIndex = 11;
            tab_Help.Text = "Help";
            tab_Help.UseVisualStyleBackColor = true;
            // 
            // Dashboard
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(1514, 877);
            ControlBox = false;
            Controls.Add(tableLayoutDashboardMain);
            DoubleBuffered = true;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 5, 4, 5);
            Name = "Dashboard";
            WindowState = FormWindowState.Maximized;
            Load += Dashboard_Load;
            tableLayoutDashboardMain.ResumeLayout(false);
            pnlHeader.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picboxLogo).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_logout).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_refresh).EndInit();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            panelMain.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tabControl_main.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timerDateTime;
        private TableLayoutPanel tableLayoutDashboardMain;
        private Panel pnlHeader;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Label lblStationName;
        private PictureBox picboxLogo;
        private TableLayoutPanel tableLayoutPanel3;
        private Label lblDateTime;
        private Label lblNTES;
        private Label lblActive;
        private Label lbUserName;
        private Panel panelMain;
        private FlowLayoutPanel flowLayoutPanel1;
        private PictureBox pictureBox_logout;
        private PictureBox pictureBox_refresh;
        private TableLayoutPanel tableLayoutPanel4;
        private TabControl tabControl_main;
        private TabPage tab_Onlinetrain;
        private TabPage tab_links;
        private TabPage tab_PA;
        private TabPage tab_Message;
        private TabPage tab_Settings;
        private TabPage tab_UserControl;
        private TabPage tab_Reports;
        private TabPage tab_HSR;
        private TabPage tab_VDC;
        private TabPage tab_Help;
        private Label lbl_CAP;
        private Label lblMode;
        private Label lblconnected_device;
        private TabPage tab_cap;
    }
}