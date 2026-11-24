namespace IPIS
{
    partial class UC_OnlineTrains
    {

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
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UC_OnlineTrains));
            panel1 = new Panel();
            tableLayoutPanelOnlineTrains = new TableLayoutPanel();
            dgvOnlineTrains = new DataGridView();
            tableLayoutPanel2 = new TableLayoutPanel();
            gBoxDisplay = new GroupBox();
            tableLayoutPanel3 = new TableLayoutPanel();
            btn_TADB = new Button();
            btn_CGDB = new Button();
            gBoxAnnc = new GroupBox();
            tableLayoutPanel4 = new TableLayoutPanel();
            tableLayoutPanel1 = new TableLayoutPanel();
            lblrepeat = new Label();
            cmb_repeat = new ComboBox();
            tableLayoutPanel5 = new TableLayoutPanel();
            btnPause = new Button();
            btnPlay = new Button();
            btnStop = new Button();
            trackBarVolume = new TrackBar();
            pictureboxwave = new PictureBox();
            panel2 = new Panel();
            groupBox1 = new GroupBox();
            label1 = new Label();
            tableLayoutPanel8 = new TableLayoutPanel();
            btn_clear = new Button();
            txtTrainNo = new TextBox();
            tableLayoutPanel9 = new TableLayoutPanel();
            btnLoad = new Button();
            tableLayoutPanel = new TableLayoutPanel();
            groupBox_Coach = new GroupBox();
            groupBox4 = new GroupBox();
            tableLayoutPanel15 = new TableLayoutPanel();
            btnFromNtes = new Button();
            btnCoachAnnouncemet = new Button();
            groupBox2 = new GroupBox();
            tableLayoutPanel6 = new TableLayoutPanel();
            btnSave = new Button();
            btnAdd = new Button();
            btnRemove = new Button();
            btnReverse = new Button();
            btnShiftRight = new Button();
            btnShiftLeft = new Button();
            panel1.SuspendLayout();
            tableLayoutPanelOnlineTrains.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvOnlineTrains).BeginInit();
            tableLayoutPanel2.SuspendLayout();
            gBoxDisplay.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            gBoxAnnc.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarVolume).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureboxwave).BeginInit();
            panel2.SuspendLayout();
            groupBox1.SuspendLayout();
            tableLayoutPanel8.SuspendLayout();
            tableLayoutPanel.SuspendLayout();
            groupBox4.SuspendLayout();
            tableLayoutPanel15.SuspendLayout();
            groupBox2.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(tableLayoutPanelOnlineTrains);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(939, 514);
            panel1.TabIndex = 0;
            // 
            // tableLayoutPanelOnlineTrains
            // 
            tableLayoutPanelOnlineTrains.BackColor = SystemColors.WindowText;
            tableLayoutPanelOnlineTrains.ColumnCount = 2;
            tableLayoutPanelOnlineTrains.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 88F));
            tableLayoutPanelOnlineTrains.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12F));
            tableLayoutPanelOnlineTrains.Controls.Add(dgvOnlineTrains, 0, 0);
            tableLayoutPanelOnlineTrains.Controls.Add(tableLayoutPanel2, 1, 0);
            tableLayoutPanelOnlineTrains.Controls.Add(tableLayoutPanel, 0, 1);
            tableLayoutPanelOnlineTrains.Dock = DockStyle.Fill;
            tableLayoutPanelOnlineTrains.Location = new Point(0, 0);
            tableLayoutPanelOnlineTrains.Name = "tableLayoutPanelOnlineTrains";
            tableLayoutPanelOnlineTrains.RowCount = 2;
            tableLayoutPanelOnlineTrains.RowStyles.Add(new RowStyle(SizeType.Percent, 79.08353F));
            tableLayoutPanelOnlineTrains.RowStyles.Add(new RowStyle(SizeType.Percent, 20.9164772F));
            tableLayoutPanelOnlineTrains.Size = new Size(939, 514);
            tableLayoutPanelOnlineTrains.TabIndex = 1;
            // 
            // dgvOnlineTrains
            // 
            dgvOnlineTrains.AllowUserToAddRows = false;
            dgvOnlineTrains.AllowUserToDeleteRows = false;
            dgvOnlineTrains.AllowUserToResizeColumns = false;
            dgvOnlineTrains.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.WhiteSmoke;
            dgvOnlineTrains.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvOnlineTrains.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvOnlineTrains.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvOnlineTrains.BackgroundColor = Color.AliceBlue;
            dgvOnlineTrains.BorderStyle = BorderStyle.Fixed3D;
            dgvOnlineTrains.CellBorderStyle = DataGridViewCellBorderStyle.Raised;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle2.BackColor = Color.LightSteelBlue;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.Maroon;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvOnlineTrains.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvOnlineTrains.ColumnHeadersHeight = 29;
            dgvOnlineTrains.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle3.BackColor = SystemColors.Window;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = Color.White;
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvOnlineTrains.DefaultCellStyle = dataGridViewCellStyle3;
            dgvOnlineTrains.Dock = DockStyle.Fill;
            dgvOnlineTrains.EnableHeadersVisualStyles = false;
            dgvOnlineTrains.GridColor = Color.Brown;
            dgvOnlineTrains.Location = new Point(3, 3);
            dgvOnlineTrains.MultiSelect = false;
            dgvOnlineTrains.Name = "dgvOnlineTrains";
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle4.BackColor = SystemColors.Control;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle4.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.ActiveCaption;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            dgvOnlineTrains.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dgvOnlineTrains.RowHeadersVisible = false;
            dgvOnlineTrains.RowHeadersWidth = 51;
            dgvOnlineTrains.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgvOnlineTrains.RowTemplate.Height = 35;
            dgvOnlineTrains.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvOnlineTrains.Size = new Size(820, 400);
            dgvOnlineTrains.TabIndex = 0;
            dgvOnlineTrains.CellValueChanged += dgvOnlineTrains_CellValueChanged;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(gBoxDisplay, 0, 0);
            tableLayoutPanel2.Controls.Add(gBoxAnnc, 0, 1);
            tableLayoutPanel2.Controls.Add(panel2, 0, 2);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(829, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 3;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 22.5F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 39.75F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 37.4074059F));
            tableLayoutPanel2.Size = new Size(107, 400);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // gBoxDisplay
            // 
            gBoxDisplay.BackColor = Color.Black;
            gBoxDisplay.Controls.Add(tableLayoutPanel3);
            gBoxDisplay.Dock = DockStyle.Fill;
            gBoxDisplay.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gBoxDisplay.ForeColor = Color.White;
            gBoxDisplay.Location = new Point(3, 3);
            gBoxDisplay.Name = "gBoxDisplay";
            gBoxDisplay.Size = new Size(101, 84);
            gBoxDisplay.TabIndex = 0;
            gBoxDisplay.TabStop = false;
            gBoxDisplay.Text = "Display Boards";
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Controls.Add(btn_TADB, 0, 0);
            tableLayoutPanel3.Controls.Add(btn_CGDB, 0, 1);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 22);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 2;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 19F));
            tableLayoutPanel3.Size = new Size(95, 59);
            tableLayoutPanel3.TabIndex = 0;
            // 
            // btn_TADB
            // 
            btn_TADB.BackColor = Color.SteelBlue;
            btn_TADB.Dock = DockStyle.Fill;
            btn_TADB.FlatStyle = FlatStyle.Flat;
            btn_TADB.ForeColor = Color.White;
            btn_TADB.ImageAlign = ContentAlignment.TopCenter;
            btn_TADB.Location = new Point(3, 2);
            btn_TADB.Margin = new Padding(3, 2, 3, 2);
            btn_TADB.Name = "btn_TADB";
            btn_TADB.Padding = new Padding(4, 0, 4, 0);
            btn_TADB.Size = new Size(89, 25);
            btn_TADB.TabIndex = 0;
            btn_TADB.Text = "TADB";
            btn_TADB.UseVisualStyleBackColor = false;
            btn_TADB.Click += btn_TADB_Click;
            // 
            // btn_CGDB
            // 
            btn_CGDB.BackColor = Color.SteelBlue;
            btn_CGDB.Dock = DockStyle.Fill;
            btn_CGDB.FlatStyle = FlatStyle.Flat;
            btn_CGDB.ForeColor = Color.White;
            btn_CGDB.ImageAlign = ContentAlignment.TopCenter;
            btn_CGDB.Location = new Point(3, 31);
            btn_CGDB.Margin = new Padding(3, 2, 3, 2);
            btn_CGDB.Name = "btn_CGDB";
            btn_CGDB.Padding = new Padding(4, 0, 4, 0);
            btn_CGDB.Size = new Size(89, 26);
            btn_CGDB.TabIndex = 1;
            btn_CGDB.Text = "CGDB";
            btn_CGDB.UseVisualStyleBackColor = false;
            btn_CGDB.Click += btn_CGDB_Click;
            // 
            // gBoxAnnc
            // 
            gBoxAnnc.BackColor = Color.Black;
            gBoxAnnc.Controls.Add(tableLayoutPanel4);
            gBoxAnnc.Dock = DockStyle.Fill;
            gBoxAnnc.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gBoxAnnc.ForeColor = Color.White;
            gBoxAnnc.Location = new Point(3, 93);
            gBoxAnnc.Name = "gBoxAnnc";
            gBoxAnnc.Size = new Size(101, 153);
            gBoxAnnc.TabIndex = 1;
            gBoxAnnc.TabStop = false;
            gBoxAnnc.Text = "Announcement";
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.BackColor = Color.FromArgb(255, 192, 192);
            tableLayoutPanel4.ColumnCount = 1;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Controls.Add(tableLayoutPanel1, 0, 0);
            tableLayoutPanel4.Controls.Add(tableLayoutPanel5, 0, 1);
            tableLayoutPanel4.Controls.Add(trackBarVolume, 0, 2);
            tableLayoutPanel4.Controls.Add(pictureboxwave, 0, 3);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(3, 22);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 4;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel4.Size = new Size(95, 128);
            tableLayoutPanel4.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 66.6666641F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.Controls.Add(lblrepeat, 0, 0);
            tableLayoutPanel1.Controls.Add(cmb_repeat, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 2);
            tableLayoutPanel1.Margin = new Padding(3, 2, 3, 2);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(89, 28);
            tableLayoutPanel1.TabIndex = 3;
            // 
            // lblrepeat
            // 
            lblrepeat.AutoSize = true;
            lblrepeat.Dock = DockStyle.Fill;
            lblrepeat.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblrepeat.ForeColor = Color.Black;
            lblrepeat.Location = new Point(3, 0);
            lblrepeat.Name = "lblrepeat";
            lblrepeat.Size = new Size(53, 28);
            lblrepeat.TabIndex = 0;
            lblrepeat.Text = "Repeat";
            lblrepeat.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmb_repeat
            // 
            cmb_repeat.Dock = DockStyle.Fill;
            cmb_repeat.FormattingEnabled = true;
            cmb_repeat.Items.AddRange(new object[] { "1", "2", "3", "4", "5" });
            cmb_repeat.Location = new Point(62, 2);
            cmb_repeat.Margin = new Padding(3, 2, 3, 2);
            cmb_repeat.Name = "cmb_repeat";
            cmb_repeat.Size = new Size(24, 27);
            cmb_repeat.TabIndex = 1;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.ColumnCount = 3;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel5.Controls.Add(btnPause, 2, 0);
            tableLayoutPanel5.Controls.Add(btnPlay, 1, 0);
            tableLayoutPanel5.Controls.Add(btnStop, 0, 0);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(3, 34);
            tableLayoutPanel5.Margin = new Padding(3, 2, 3, 2);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 1;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel5.Size = new Size(89, 28);
            tableLayoutPanel5.TabIndex = 4;
            // 
            // btnPause
            // 
            btnPause.BackColor = Color.Black;
            btnPause.Dock = DockStyle.Fill;
            btnPause.FlatStyle = FlatStyle.Flat;
            btnPause.ForeColor = Color.Black;
            btnPause.Image = (Image)resources.GetObject("btnPause.Image");
            btnPause.Location = new Point(58, 0);
            btnPause.Margin = new Padding(0);
            btnPause.Name = "btnPause";
            btnPause.Size = new Size(31, 28);
            btnPause.TabIndex = 8;
            btnPause.UseVisualStyleBackColor = false;
            btnPause.Click += btnPause_Click;
            // 
            // btnPlay
            // 
            btnPlay.BackColor = Color.Black;
            btnPlay.Dock = DockStyle.Fill;
            btnPlay.FlatStyle = FlatStyle.Flat;
            btnPlay.ForeColor = Color.Black;
            btnPlay.Image = (Image)resources.GetObject("btnPlay.Image");
            btnPlay.Location = new Point(29, 0);
            btnPlay.Margin = new Padding(0);
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new Size(29, 28);
            btnPlay.TabIndex = 4;
            btnPlay.UseVisualStyleBackColor = false;
            btnPlay.Click += btnPlay_Click;
            // 
            // btnStop
            // 
            btnStop.BackColor = Color.Black;
            btnStop.Dock = DockStyle.Fill;
            btnStop.FlatStyle = FlatStyle.Flat;
            btnStop.ForeColor = Color.Black;
            btnStop.Image = (Image)resources.GetObject("btnStop.Image");
            btnStop.Location = new Point(0, 0);
            btnStop.Margin = new Padding(0);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(29, 28);
            btnStop.TabIndex = 3;
            btnStop.UseVisualStyleBackColor = false;
            btnStop.Click += btnStop_Click;
            // 
            // trackBarVolume
            // 
            trackBarVolume.Dock = DockStyle.Fill;
            trackBarVolume.Location = new Point(3, 66);
            trackBarVolume.Margin = new Padding(3, 2, 3, 2);
            trackBarVolume.Name = "trackBarVolume";
            trackBarVolume.Size = new Size(89, 28);
            trackBarVolume.TabIndex = 5;
            trackBarVolume.Scroll += trackBarVolume_Scroll;
            // 
            // pictureboxwave
            // 
            pictureboxwave.Dock = DockStyle.Fill;
            pictureboxwave.Location = new Point(3, 99);
            pictureboxwave.Name = "pictureboxwave";
            pictureboxwave.Size = new Size(89, 26);
            pictureboxwave.TabIndex = 6;
            pictureboxwave.TabStop = false;
            // 
            // panel2
            // 
            panel2.BackColor = Color.AliceBlue;
            panel2.Controls.Add(groupBox1);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 252);
            panel2.Name = "panel2";
            panel2.Size = new Size(101, 145);
            panel2.TabIndex = 2;
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.Black;
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(tableLayoutPanel8);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox1.ForeColor = Color.White;
            groupBox1.Location = new Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(101, 145);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Grid Control";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ImageAlign = ContentAlignment.BottomLeft;
            label1.Location = new Point(5, 22);
            label1.Name = "label1";
            label1.Size = new Size(101, 19);
            label1.TabIndex = 0;
            label1.Text = "Train Number";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel8
            // 
            tableLayoutPanel8.ColumnCount = 1;
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel8.Controls.Add(btn_clear, 0, 3);
            tableLayoutPanel8.Controls.Add(txtTrainNo, 0, 1);
            tableLayoutPanel8.Controls.Add(tableLayoutPanel9, 0, 0);
            tableLayoutPanel8.Controls.Add(btnLoad, 0, 2);
            tableLayoutPanel8.Dock = DockStyle.Fill;
            tableLayoutPanel8.Location = new Point(3, 22);
            tableLayoutPanel8.Name = "tableLayoutPanel8";
            tableLayoutPanel8.RowCount = 4;
            tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel8.Size = new Size(95, 120);
            tableLayoutPanel8.TabIndex = 0;
            // 
            // btn_clear
            // 
            btn_clear.BackColor = Color.SteelBlue;
            btn_clear.Dock = DockStyle.Fill;
            btn_clear.FlatStyle = FlatStyle.Flat;
            btn_clear.Location = new Point(3, 92);
            btn_clear.Margin = new Padding(3, 2, 3, 2);
            btn_clear.Name = "btn_clear";
            btn_clear.Size = new Size(89, 26);
            btn_clear.TabIndex = 8;
            btn_clear.Text = "CLEAR";
            btn_clear.UseVisualStyleBackColor = false;
            btn_clear.Click += btn_clear_Click;
            // 
            // txtTrainNo
            // 
            txtTrainNo.Dock = DockStyle.Fill;
            txtTrainNo.Location = new Point(3, 32);
            txtTrainNo.Margin = new Padding(3, 2, 3, 2);
            txtTrainNo.Name = "txtTrainNo";
            txtTrainNo.Size = new Size(89, 26);
            txtTrainNo.TabIndex = 7;
            txtTrainNo.KeyPress += txtTrainNo_KeyPress;
            // 
            // tableLayoutPanel9
            // 
            tableLayoutPanel9.ColumnCount = 2;
            tableLayoutPanel9.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel9.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel9.Dock = DockStyle.Fill;
            tableLayoutPanel9.Location = new Point(3, 2);
            tableLayoutPanel9.Margin = new Padding(3, 2, 3, 2);
            tableLayoutPanel9.Name = "tableLayoutPanel9";
            tableLayoutPanel9.RowCount = 1;
            tableLayoutPanel9.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel9.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel9.Size = new Size(89, 26);
            tableLayoutPanel9.TabIndex = 3;
            // 
            // btnLoad
            // 
            btnLoad.BackColor = Color.SteelBlue;
            btnLoad.Dock = DockStyle.Fill;
            btnLoad.FlatStyle = FlatStyle.Flat;
            btnLoad.Location = new Point(3, 62);
            btnLoad.Margin = new Padding(3, 2, 3, 2);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(89, 26);
            btnLoad.TabIndex = 4;
            btnLoad.Text = "LOAD";
            btnLoad.UseVisualStyleBackColor = false;
            btnLoad.Click += btnLoad_Click;
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.BackColor = Color.Black;
            tableLayoutPanel.ColumnCount = 3;
            tableLayoutPanelOnlineTrains.SetColumnSpan(tableLayoutPanel, 2);
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 75.0267944F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.8981781F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel.Controls.Add(groupBox_Coach, 0, 0);
            tableLayoutPanel.Controls.Add(groupBox4, 2, 0);
            tableLayoutPanel.Controls.Add(groupBox2, 1, 0);
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tableLayoutPanel.ForeColor = Color.White;
            tableLayoutPanel.Location = new Point(3, 409);
            tableLayoutPanel.MinimumSize = new Size(0, 115);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 1;
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel.Size = new Size(933, 102);
            tableLayoutPanel.TabIndex = 2;
            // 
            // groupBox_Coach
            // 
            groupBox_Coach.BackColor = Color.Black;
            groupBox_Coach.Dock = DockStyle.Fill;
            groupBox_Coach.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox_Coach.ForeColor = Color.White;
            groupBox_Coach.Location = new Point(3, 3);
            groupBox_Coach.Name = "groupBox_Coach";
            groupBox_Coach.Size = new Size(694, 96);
            groupBox_Coach.TabIndex = 7;
            groupBox_Coach.TabStop = false;
            groupBox_Coach.Text = "Train Number : ";
            groupBox_Coach.Enter += groupBox_Coach_Enter_1;
            // 
            // groupBox4
            // 
            groupBox4.BackColor = Color.Black;
            groupBox4.Controls.Add(tableLayoutPanel15);
            groupBox4.Dock = DockStyle.Fill;
            groupBox4.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox4.ForeColor = Color.White;
            groupBox4.Location = new Point(842, 3);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(88, 96);
            groupBox4.TabIndex = 5;
            groupBox4.TabStop = false;
            groupBox4.Text = "NTES";
            // 
            // tableLayoutPanel15
            // 
            tableLayoutPanel15.ColumnCount = 1;
            tableLayoutPanel15.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel15.Controls.Add(btnFromNtes, 0, 0);
            tableLayoutPanel15.Controls.Add(btnCoachAnnouncemet, 0, 1);
            tableLayoutPanel15.Dock = DockStyle.Fill;
            tableLayoutPanel15.Location = new Point(3, 22);
            tableLayoutPanel15.Name = "tableLayoutPanel15";
            tableLayoutPanel15.RowCount = 2;
            tableLayoutPanel15.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel15.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel15.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel15.Size = new Size(82, 71);
            tableLayoutPanel15.TabIndex = 0;
            // 
            // btnFromNtes
            // 
            btnFromNtes.BackColor = Color.SteelBlue;
            btnFromNtes.Dock = DockStyle.Fill;
            btnFromNtes.FlatStyle = FlatStyle.Flat;
            btnFromNtes.Location = new Point(3, 2);
            btnFromNtes.Margin = new Padding(3, 2, 3, 2);
            btnFromNtes.Name = "btnFromNtes";
            btnFromNtes.Size = new Size(76, 31);
            btnFromNtes.TabIndex = 0;
            btnFromNtes.Text = "NTES UPDATE";
            btnFromNtes.UseVisualStyleBackColor = false;
            btnFromNtes.Click += btnFromNtes_Click;
            // 
            // btnCoachAnnouncemet
            // 
            btnCoachAnnouncemet.BackColor = Color.SteelBlue;
            btnCoachAnnouncemet.Dock = DockStyle.Fill;
            btnCoachAnnouncemet.FlatStyle = FlatStyle.Flat;
            btnCoachAnnouncemet.Location = new Point(3, 37);
            btnCoachAnnouncemet.Margin = new Padding(3, 2, 3, 2);
            btnCoachAnnouncemet.Name = "btnCoachAnnouncemet";
            btnCoachAnnouncemet.Size = new Size(76, 32);
            btnCoachAnnouncemet.TabIndex = 3;
            btnCoachAnnouncemet.Text = "COACH ANN";
            btnCoachAnnouncemet.UseVisualStyleBackColor = false;
            btnCoachAnnouncemet.Click += btnCoachAnnouncemet_Click;
            // 
            // groupBox2
            // 
            groupBox2.BackColor = Color.Black;
            groupBox2.Controls.Add(tableLayoutPanel6);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox2.ForeColor = Color.White;
            groupBox2.Location = new Point(703, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(133, 96);
            groupBox2.TabIndex = 6;
            groupBox2.TabStop = false;
            groupBox2.Text = "Coach Controls";
            // 
            // tableLayoutPanel6
            // 
            tableLayoutPanel6.ColumnCount = 2;
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel6.Controls.Add(btnSave, 1, 2);
            tableLayoutPanel6.Controls.Add(btnAdd, 0, 2);
            tableLayoutPanel6.Controls.Add(btnRemove, 1, 1);
            tableLayoutPanel6.Controls.Add(btnReverse, 0, 1);
            tableLayoutPanel6.Controls.Add(btnShiftRight, 1, 0);
            tableLayoutPanel6.Controls.Add(btnShiftLeft, 0, 0);
            tableLayoutPanel6.Dock = DockStyle.Fill;
            tableLayoutPanel6.Location = new Point(3, 22);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 3;
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33334F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));
            tableLayoutPanel6.Size = new Size(127, 71);
            tableLayoutPanel6.TabIndex = 0;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.SteelBlue;
            btnSave.Dock = DockStyle.Fill;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.Location = new Point(66, 48);
            btnSave.Margin = new Padding(3, 2, 3, 2);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(58, 21);
            btnSave.TabIndex = 14;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnAdd
            // 
            btnAdd.BackColor = Color.SteelBlue;
            btnAdd.Dock = DockStyle.Fill;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAdd.Location = new Point(3, 48);
            btnAdd.Margin = new Padding(3, 2, 3, 2);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(57, 21);
            btnAdd.TabIndex = 13;
            btnAdd.Text = "Add";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnRemove
            // 
            btnRemove.BackColor = Color.SteelBlue;
            btnRemove.Dock = DockStyle.Fill;
            btnRemove.FlatStyle = FlatStyle.Flat;
            btnRemove.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRemove.Location = new Point(66, 25);
            btnRemove.Margin = new Padding(3, 2, 3, 2);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new Size(58, 19);
            btnRemove.TabIndex = 12;
            btnRemove.Text = "Remove";
            btnRemove.UseVisualStyleBackColor = false;
            btnRemove.Click += btnRemove_Click;
            // 
            // btnReverse
            // 
            btnReverse.BackColor = Color.SteelBlue;
            btnReverse.Dock = DockStyle.Fill;
            btnReverse.FlatStyle = FlatStyle.Flat;
            btnReverse.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnReverse.Location = new Point(3, 25);
            btnReverse.Margin = new Padding(3, 2, 3, 2);
            btnReverse.Name = "btnReverse";
            btnReverse.Size = new Size(57, 19);
            btnReverse.TabIndex = 11;
            btnReverse.Text = "Reverse";
            btnReverse.UseVisualStyleBackColor = false;
            btnReverse.Click += btnReverse_Click;
            // 
            // btnShiftRight
            // 
            btnShiftRight.BackColor = Color.SteelBlue;
            btnShiftRight.Dock = DockStyle.Fill;
            btnShiftRight.FlatStyle = FlatStyle.Flat;
            btnShiftRight.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnShiftRight.Location = new Point(66, 2);
            btnShiftRight.Margin = new Padding(3, 2, 3, 2);
            btnShiftRight.Name = "btnShiftRight";
            btnShiftRight.Size = new Size(58, 19);
            btnShiftRight.TabIndex = 10;
            btnShiftRight.Text = ">>";
            btnShiftRight.UseVisualStyleBackColor = false;
            btnShiftRight.Click += btnShiftRight_Click;
            // 
            // btnShiftLeft
            // 
            btnShiftLeft.BackColor = Color.SteelBlue;
            btnShiftLeft.Dock = DockStyle.Fill;
            btnShiftLeft.FlatStyle = FlatStyle.Flat;
            btnShiftLeft.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnShiftLeft.Location = new Point(3, 2);
            btnShiftLeft.Margin = new Padding(3, 2, 3, 2);
            btnShiftLeft.Name = "btnShiftLeft";
            btnShiftLeft.Size = new Size(57, 19);
            btnShiftLeft.TabIndex = 9;
            btnShiftLeft.Text = "<<";
            btnShiftLeft.UseVisualStyleBackColor = false;
            btnShiftLeft.Click += btnShiftLeft_Click;
            // 
            // UC_OnlineTrains
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = Color.WhiteSmoke;
            Controls.Add(panel1);
            Location = new Point(100, 100);
            Name = "UC_OnlineTrains";
            Size = new Size(939, 514);
            Load += UC_OnlineTrains_Load;
            panel1.ResumeLayout(false);
            tableLayoutPanelOnlineTrains.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvOnlineTrains).EndInit();
            tableLayoutPanel2.ResumeLayout(false);
            gBoxDisplay.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            gBoxAnnc.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)trackBarVolume).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureboxwave).EndInit();
            panel2.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tableLayoutPanel8.ResumeLayout(false);
            tableLayoutPanel8.PerformLayout();
            tableLayoutPanel.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            tableLayoutPanel15.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            tableLayoutPanel6.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureboxLoader;
        private TableLayoutPanel tableLayoutPanelOnlineTrains;
        private DataGridView dgvOnlineTrains;
        private TableLayoutPanel tableLayoutPanel2;
        private GroupBox gBoxDisplay;
        private TableLayoutPanel tableLayoutPanel3;
        private Button btn_TADB;
        private Button btn_CGDB;
        private GroupBox gBoxAnnc;
        private TableLayoutPanel tableLayoutPanel4;
        private TableLayoutPanel tableLayoutPanel1;
        private Label lblrepeat;
        private ComboBox cmb_repeat;
        private TableLayoutPanel tableLayoutPanel5;
        private Button btnPause;
        private Button btnPlay;
        private Button btnStop;
        private TrackBar trackBarVolume;
        private Panel panel2;
        private GroupBox groupBox1;
        private TableLayoutPanel tableLayoutPanel8;
        private TableLayoutPanel tableLayoutPanel9;
        private Button btnLoad;
        private TableLayoutPanel tableLayoutPanel;
        private GroupBox groupBox4;
        private TableLayoutPanel tableLayoutPanel15;
        private Button btnFromNtes;
        private Button btnShiftRight;
        private Button btnCoachAnnouncemet;
        private TextBox txtTrainNo;
        private Label label1;
        private Button btn_clear;
        private GroupBox groupBox2;
        private TableLayoutPanel tableLayoutPanel6;
        private Button btnSave;
        private Button btnAdd;
        private Button btnRemove;
        private Button btnReverse;
        private Button btnShiftLeft;
        private GroupBox groupBox_Coach;
        private PictureBox pictureboxwave;
    }
}
