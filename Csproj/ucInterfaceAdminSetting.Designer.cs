//namespace IPIS
//{
//    partial class ucInterfaceAdminSetting
//    {
//        /// <summary> 
//        /// Required designer variable.
//        /// </summary>
//        private System.ComponentModel.IContainer components = null;

//        /// <summary> 
//        /// Clean up any resources being used.
//        /// </summary>
//        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing && (components != null))
//            {
//                components.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        #region Component Designer generated code

//        /// <summary> 
//        /// Required method for Designer support - do not modify 
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            tableLayoutPanel1 = new TableLayoutPanel();
//            groupBox1 = new GroupBox();
//            tableLayoutPanel5 = new TableLayoutPanel();
//            cmbAnnounceLanguage = new ComboBox();
//            label13 = new Label();
//            cmb_maxEthernetport = new ComboBox();
//            cmb_maxPlateform = new ComboBox();
//            cmb_maxcoach = new ComboBox();
//            label4 = new Label();
//            label6 = new Label();
//            label7 = new Label();
//            label3 = new Label();
//            label9 = new Label();
//            label12 = new Label();
//            label11 = new Label();
//            label21 = new Label();
//            label10 = new Label();
//            txtPassKey = new TextBox();
//            txtPFEnd2 = new TextBox();
//            txtPFEnd1 = new TextBox();
//            txtStationCode = new TextBox();
//            txtStationName = new TextBox();
//            cmbRegLang = new ComboBox();
//            groupBox2 = new GroupBox();
//            tableLayoutPanel4 = new TableLayoutPanel();
//            chk_IsMaster = new CheckBox();
//            chk_Autolinkcheck = new CheckBox();
//            chk_StartUp = new CheckBox();
//            chk_AutoAnnc = new CheckBox();
//            chk_resetstate = new CheckBox();
//            cmbNightIntensity = new ComboBox();
//            cmbDayIntensity = new ComboBox();
//            mskSetNight = new MaskedTextBox();
//            mskSetDay = new MaskedTextBox();
//            label8 = new Label();
//            label5 = new Label();
//            label2 = new Label();
//            label1 = new Label();
//            chk_AutoIntensity = new CheckBox();
//            chk_NtesAutoUpdate = new CheckBox();
//            cmbAutoUpdateTime = new ComboBox();
//            groupBox3 = new GroupBox();
//            groupBox4 = new GroupBox();
//            panel1 = new Panel();
//            tableLayoutPanel6 = new TableLayoutPanel();
//            btnSave = new Button();
//            tableLayoutPanel1.SuspendLayout();
//            groupBox1.SuspendLayout();
//            tableLayoutPanel5.SuspendLayout();
//            groupBox2.SuspendLayout();
//            tableLayoutPanel4.SuspendLayout();
//            panel1.SuspendLayout();
//            tableLayoutPanel6.SuspendLayout();
//            SuspendLayout();
//            // 
//            // tableLayoutPanel1
//            // 
//            tableLayoutPanel1.BackColor = Color.Beige;
//            tableLayoutPanel1.ColumnCount = 2;
//            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
//            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
//            tableLayoutPanel1.Controls.Add(groupBox1, 0, 0);
//            tableLayoutPanel1.Controls.Add(groupBox2, 1, 0);
//            tableLayoutPanel1.Controls.Add(groupBox3, 0, 1);
//            tableLayoutPanel1.Controls.Add(groupBox4, 1, 1);
//            tableLayoutPanel1.Controls.Add(panel1, 0, 2);
//            tableLayoutPanel1.Dock = DockStyle.Fill;
//            tableLayoutPanel1.Location = new Point(0, 0);
//            tableLayoutPanel1.Name = "tableLayoutPanel1";
//            tableLayoutPanel1.RowCount = 3;
//            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 65.95744F));
//            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 26.41844F));
//            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 7.8014183F));
//            tableLayoutPanel1.Size = new Size(857, 564);
//            tableLayoutPanel1.TabIndex = 0;
//            // 
//            // groupBox1
//            // 
//            groupBox1.Controls.Add(tableLayoutPanel5);
//            groupBox1.Dock = DockStyle.Fill;
//            groupBox1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
//            groupBox1.Location = new Point(3, 3);
//            groupBox1.Name = "groupBox1";
//            groupBox1.Size = new Size(422, 365);
//            groupBox1.TabIndex = 0;
//            groupBox1.TabStop = false;
//            groupBox1.Text = "Station Setting";
//            // 
//            // tableLayoutPanel5
//            // 
//            tableLayoutPanel5.BackColor = Color.WhiteSmoke;
//            tableLayoutPanel5.ColumnCount = 2;
//            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
//            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
//            tableLayoutPanel5.Controls.Add(cmbAnnounceLanguage, 1, 3);
//            tableLayoutPanel5.Controls.Add(label13, 0, 3);
//            tableLayoutPanel5.Controls.Add(cmb_maxEthernetport, 1, 2);
//            tableLayoutPanel5.Controls.Add(cmb_maxPlateform, 1, 1);
//            tableLayoutPanel5.Controls.Add(cmb_maxcoach, 1, 0);
//            tableLayoutPanel5.Controls.Add(label4, 0, 0);
//            tableLayoutPanel5.Controls.Add(label6, 0, 1);
//            tableLayoutPanel5.Controls.Add(label7, 0, 2);
//            tableLayoutPanel5.Controls.Add(label3, 0, 9);
//            tableLayoutPanel5.Controls.Add(label9, 0, 8);
//            tableLayoutPanel5.Controls.Add(label12, 0, 7);
//            tableLayoutPanel5.Controls.Add(label11, 0, 6);
//            tableLayoutPanel5.Controls.Add(label21, 0, 5);
//            tableLayoutPanel5.Controls.Add(label10, 0, 4);
//            tableLayoutPanel5.Controls.Add(txtPassKey, 1, 9);
//            tableLayoutPanel5.Controls.Add(txtPFEnd2, 1, 8);
//            tableLayoutPanel5.Controls.Add(txtPFEnd1, 1, 7);
//            tableLayoutPanel5.Controls.Add(txtStationCode, 1, 6);
//            tableLayoutPanel5.Controls.Add(txtStationName, 1, 5);
//            tableLayoutPanel5.Controls.Add(cmbRegLang, 1, 4);
//            tableLayoutPanel5.Dock = DockStyle.Fill;
//            tableLayoutPanel5.Location = new Point(3, 22);
//            tableLayoutPanel5.Name = "tableLayoutPanel5";
//            tableLayoutPanel5.RowCount = 10;
//            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10.0100088F));
//            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10.01001F));
//            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10.01001F));
//            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10.0100088F));
//            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10.01001F));
//            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10.01001F));
//            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10.01001F));
//            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10.01001F));
//            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10.01001F));
//            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 9.90991F));
//            tableLayoutPanel5.Size = new Size(416, 340);
//            tableLayoutPanel5.TabIndex = 0;
//            // 
//            // cmbAnnounceLanguage
//            // 
//            cmbAnnounceLanguage.Dock = DockStyle.Fill;
//            cmbAnnounceLanguage.DropDownStyle = ComboBoxStyle.DropDownList;
//            cmbAnnounceLanguage.FormattingEnabled = true;
//            cmbAnnounceLanguage.Items.AddRange(new object[] { "Select Language", "Hindi", "English", "Regional", "Hindi+English", "Hindi+Regional", "English+Regional", "Hindi+English+Regional" });
//            cmbAnnounceLanguage.Location = new Point(211, 105);
//            cmbAnnounceLanguage.Name = "cmbAnnounceLanguage";
//            cmbAnnounceLanguage.Size = new Size(202, 27);
//            cmbAnnounceLanguage.TabIndex = 27;
//            cmbAnnounceLanguage.TabStop = false;
//            cmbAnnounceLanguage.Tag = "";
//            // 
//            // label13
//            // 
//            label13.AutoSize = true;
//            label13.Dock = DockStyle.Fill;
//            label13.Location = new Point(3, 102);
//            label13.Name = "label13";
//            label13.Size = new Size(202, 34);
//            label13.TabIndex = 26;
//            label13.Text = "ANNOUNCE LANGUAGE";
//            // 
//            // cmb_maxEthernetport
//            // 
//            cmb_maxEthernetport.Dock = DockStyle.Fill;
//            cmb_maxEthernetport.DropDownStyle = ComboBoxStyle.DropDownList;
//            cmb_maxEthernetport.FormattingEnabled = true;
//            cmb_maxEthernetport.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20","21","22","23","24" });
//            cmb_maxEthernetport.Location = new Point(211, 71);
//            cmb_maxEthernetport.Name = "cmb_maxEthernetport";
//            cmb_maxEthernetport.Size = new Size(202, 27);
//            cmb_maxEthernetport.TabIndex = 3;
//            cmb_maxEthernetport.TabStop = false;
//            cmb_maxEthernetport.Tag = "";
//            // 
//            // cmb_maxPlateform
//            // 
//            cmb_maxPlateform.Dock = DockStyle.Fill;
//            cmb_maxPlateform.DropDownStyle = ComboBoxStyle.DropDownList;
//            cmb_maxPlateform.FormattingEnabled = true;
//            cmb_maxPlateform.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28" });
//            cmb_maxPlateform.Location = new Point(211, 37);
//            cmb_maxPlateform.Name = "cmb_maxPlateform";
//            cmb_maxPlateform.Size = new Size(202, 27);
//            cmb_maxPlateform.TabIndex = 2;
//            cmb_maxPlateform.TabStop = false;
//            cmb_maxPlateform.Tag = "";
//            // 
//            // cmb_maxcoach
//            // 
//            cmb_maxcoach.Dock = DockStyle.Fill;
//            cmb_maxcoach.DropDownStyle = ComboBoxStyle.DropDownList;
//            cmb_maxcoach.FormattingEnabled = true;
//            cmb_maxcoach.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28" });
//            cmb_maxcoach.Location = new Point(211, 3);
//            cmb_maxcoach.Name = "cmb_maxcoach";
//            cmb_maxcoach.Size = new Size(202, 27);
//            cmb_maxcoach.TabIndex = 1;
//            cmb_maxcoach.TabStop = false;
//            cmb_maxcoach.Tag = "";
//            // 
//            // label4
//            // 
//            label4.AutoSize = true;
//            label4.Dock = DockStyle.Fill;
//            label4.Location = new Point(3, 0);
//            label4.Name = "label4";
//            label4.Size = new Size(202, 34);
//            label4.TabIndex = 0;
//            label4.Text = "MAX COACH";
//            // 
//            // label6
//            // 
//            label6.AutoSize = true;
//            label6.Dock = DockStyle.Fill;
//            label6.Location = new Point(3, 34);
//            label6.Name = "label6";
//            label6.Size = new Size(202, 34);
//            label6.TabIndex = 1;
//            label6.Text = "MAX PLATEFORM";
//            // 
//            // label7
//            // 
//            label7.AutoSize = true;
//            label7.Dock = DockStyle.Fill;
//            label7.Location = new Point(3, 68);
//            label7.Name = "label7";
//            label7.Size = new Size(202, 34);
//            label7.TabIndex = 2;
//            label7.Text = "MAX ETHERNET PORT";
//            // 
//            // label3
//            // 
//            label3.AutoSize = true;
//            label3.Dock = DockStyle.Fill;
//            label3.Location = new Point(3, 306);
//            label3.Name = "label3";
//            label3.Size = new Size(202, 34);
//            label3.TabIndex = 25;
//            label3.Text = "NTES PASS KEY";
//            // 
//            // label9
//            // 
//            label9.AutoSize = true;
//            label9.Dock = DockStyle.Fill;
//            label9.Location = new Point(3, 272);
//            label9.Name = "label9";
//            label9.Size = new Size(202, 34);
//            label9.TabIndex = 8;
//            label9.Text = "PLATEFORM END 2";
//            // 
//            // label12
//            // 
//            label12.AutoSize = true;
//            label12.Dock = DockStyle.Fill;
//            label12.Location = new Point(3, 238);
//            label12.Name = "label12";
//            label12.Size = new Size(202, 34);
//            label12.TabIndex = 7;
//            label12.Text = "PLATEFORM END 1";
//            // 
//            // label11
//            // 
//            label11.AutoSize = true;
//            label11.Dock = DockStyle.Fill;
//            label11.Location = new Point(3, 204);
//            label11.Name = "label11";
//            label11.Size = new Size(202, 34);
//            label11.TabIndex = 6;
//            label11.Text = "STATION CODE";
//            // 
//            // label21
//            // 
//            label21.AutoSize = true;
//            label21.Dock = DockStyle.Fill;
//            label21.Location = new Point(3, 170);
//            label21.Name = "label21";
//            label21.Size = new Size(202, 34);
//            label21.TabIndex = 17;
//            label21.Text = "STATION NAME";
//            // 
//            // label10
//            // 
//            label10.AutoSize = true;
//            label10.Dock = DockStyle.Fill;
//            label10.Location = new Point(3, 136);
//            label10.Name = "label10";
//            label10.Size = new Size(202, 34);
//            label10.TabIndex = 5;
//            label10.Text = "REGIONAL LANGUAGE";
//            // 
//            // txtPassKey
//            // 
//            txtPassKey.Dock = DockStyle.Fill;
//            txtPassKey.Location = new Point(211, 309);
//            txtPassKey.Name = "txtPassKey";
//            txtPassKey.Size = new Size(202, 26);
//            txtPassKey.TabIndex = 9;
//            // 
//            // txtPFEnd2
//            // 
//            txtPFEnd2.Dock = DockStyle.Fill;
//            txtPFEnd2.Location = new Point(211, 275);
//            txtPFEnd2.Name = "txtPFEnd2";
//            txtPFEnd2.Size = new Size(202, 26);
//            txtPFEnd2.TabIndex = 8;
//            // 
//            // txtPFEnd1
//            // 
//            txtPFEnd1.Dock = DockStyle.Fill;
//            txtPFEnd1.Location = new Point(211, 241);
//            txtPFEnd1.Name = "txtPFEnd1";
//            txtPFEnd1.Size = new Size(202, 26);
//            txtPFEnd1.TabIndex = 7;
//            // 
//            // txtStationCode
//            // 
//            txtStationCode.Dock = DockStyle.Fill;
//            txtStationCode.Location = new Point(211, 207);
//            txtStationCode.Name = "txtStationCode";
//            txtStationCode.Size = new Size(202, 26);
//            txtStationCode.TabIndex = 6;
//            // 
//            // txtStationName
//            // 
//            txtStationName.Dock = DockStyle.Fill;
//            txtStationName.Location = new Point(211, 173);
//            txtStationName.Name = "txtStationName";
//            txtStationName.Size = new Size(202, 26);
//            txtStationName.TabIndex = 5;
//            // 
//            // cmbRegLang
//            // 
//            cmbRegLang.Dock = DockStyle.Fill;
//            cmbRegLang.DropDownStyle = ComboBoxStyle.DropDownList;
//            cmbRegLang.FormattingEnabled = true;
//            cmbRegLang.Items.AddRange(new object[] { "Select Language", "Assamese ", "Bengali", "Bodo", "Dogri", "Gujarati", "Kannada", "Kashmiri", "Konkani", "Maithili", "Malayalam", "Manipuri", "Marathi", "Nepali", "Odia", "Punjabi", "Sanskrit", "Santali", "Sindhi", "Tamil", "Telugu", "Urdu" });
//            cmbRegLang.Location = new Point(211, 139);
//            cmbRegLang.Name = "cmbRegLang";
//            cmbRegLang.Size = new Size(202, 27);
//            cmbRegLang.TabIndex = 4;
//            cmbRegLang.TabStop = false;
//            cmbRegLang.Tag = "";
//            // 
//            // groupBox2
//            // 
//            groupBox2.Controls.Add(tableLayoutPanel4);
//            groupBox2.Dock = DockStyle.Fill;
//            groupBox2.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
//            groupBox2.Location = new Point(431, 3);
//            groupBox2.Name = "groupBox2";
//            groupBox2.Size = new Size(423, 365);
//            groupBox2.TabIndex = 1;
//            groupBox2.TabStop = false;
//            groupBox2.Text = "Automatic Functionality";
//            // 
//            // tableLayoutPanel4
//            // 
//            tableLayoutPanel4.BackColor = Color.WhiteSmoke;
//            tableLayoutPanel4.ColumnCount = 3;
//            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42.6858521F));
//            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 24.9400482F));
//            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32.1342926F));
//            tableLayoutPanel4.Controls.Add(chk_IsMaster, 1, 1);
//            tableLayoutPanel4.Controls.Add(chk_Autolinkcheck, 0, 0);
//            tableLayoutPanel4.Controls.Add(chk_StartUp, 1, 0);
//            tableLayoutPanel4.Controls.Add(chk_AutoAnnc, 2, 0);
//            tableLayoutPanel4.Controls.Add(chk_resetstate, 0, 1);
//            tableLayoutPanel4.Controls.Add(cmbNightIntensity, 1, 7);
//            tableLayoutPanel4.Controls.Add(cmbDayIntensity, 1, 6);
//            tableLayoutPanel4.Controls.Add(mskSetNight, 1, 5);
//            tableLayoutPanel4.Controls.Add(mskSetDay, 1, 4);
//            tableLayoutPanel4.Controls.Add(label8, 0, 7);
//            tableLayoutPanel4.Controls.Add(label5, 0, 6);
//            tableLayoutPanel4.Controls.Add(label2, 0, 5);
//            tableLayoutPanel4.Controls.Add(label1, 0, 4);
//            tableLayoutPanel4.Controls.Add(chk_AutoIntensity, 0, 3);
//            tableLayoutPanel4.Controls.Add(chk_NtesAutoUpdate, 0, 2);
//            tableLayoutPanel4.Controls.Add(cmbAutoUpdateTime, 1, 2);
//            tableLayoutPanel4.Dock = DockStyle.Fill;
//            tableLayoutPanel4.Location = new Point(3, 22);
//            tableLayoutPanel4.Name = "tableLayoutPanel4";
//            tableLayoutPanel4.RowCount = 8;
//            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
//            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
//            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
//            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
//            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
//            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
//            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
//            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
//            tableLayoutPanel4.Size = new Size(417, 340);
//            tableLayoutPanel4.TabIndex = 0;
//            // 
//            // chk_IsMaster
//            // 
//            chk_IsMaster.AutoSize = true;
//            chk_IsMaster.Dock = DockStyle.Fill;
//            chk_IsMaster.Location = new Point(181, 44);
//            chk_IsMaster.Margin = new Padding(3, 2, 3, 2);
//            chk_IsMaster.Name = "chk_IsMaster";
//            chk_IsMaster.Size = new Size(98, 38);
//            chk_IsMaster.TabIndex = 25;
//            chk_IsMaster.Text = "IS Master";
//            chk_IsMaster.UseVisualStyleBackColor = true;
//            // 
//            // chk_Autolinkcheck
//            // 
//            chk_Autolinkcheck.AutoSize = true;
//            chk_Autolinkcheck.Dock = DockStyle.Fill;
//            chk_Autolinkcheck.Location = new Point(3, 2);
//            chk_Autolinkcheck.Margin = new Padding(3, 2, 3, 2);
//            chk_Autolinkcheck.Name = "chk_Autolinkcheck";   
//            chk_Autolinkcheck.Size = new Size(172, 38);
//            chk_Autolinkcheck.TabIndex = 10;
//            chk_Autolinkcheck.Text = "AUTO LINK CHECK";
//            chk_Autolinkcheck.UseVisualStyleBackColor = true;
//            // 
//            // chk_StartUp
//            // 
//            chk_StartUp.AutoSize = true;
//            chk_StartUp.Dock = DockStyle.Fill;
//            chk_StartUp.Location = new Point(181, 2);
//            chk_StartUp.Margin = new Padding(3, 2, 3, 2);
//            chk_StartUp.Name = "chk_StartUp";
//            chk_StartUp.Size = new Size(98, 38);
//            chk_StartUp.TabIndex = 11;
//            chk_StartUp.Text = "START UP";
//            chk_StartUp.UseVisualStyleBackColor = true;
//            // 
//            // chk_AutoAnnc
//            // 
//            chk_AutoAnnc.AutoSize = true;
//            chk_AutoAnnc.Dock = DockStyle.Fill;
//            chk_AutoAnnc.Location = new Point(285, 2);
//            chk_AutoAnnc.Margin = new Padding(3, 2, 3, 2);
//            chk_AutoAnnc.Name = "chk_AutoAnnc";
//            chk_AutoAnnc.Size = new Size(129, 38);
//            chk_AutoAnnc.TabIndex = 12;
//            chk_AutoAnnc.Text = "AUTO ANNC";
//            chk_AutoAnnc.UseVisualStyleBackColor = true;
//            // 
//            // chk_resetstate
//            // 
//            chk_resetstate.AutoSize = true;
//            chk_resetstate.Dock = DockStyle.Fill;
//            chk_resetstate.Location = new Point(3, 44);
//            chk_resetstate.Margin = new Padding(3, 2, 3, 2);
//            chk_resetstate.Name = "chk_resetstate";
//            chk_resetstate.Size = new Size(172, 38);
//            chk_resetstate.TabIndex = 13;
//            chk_resetstate.Text = "RESET STATE";
//            chk_resetstate.UseVisualStyleBackColor = true;
//            // 
//            // cmbNightIntensity
//            // 
//            cmbNightIntensity.Dock = DockStyle.Fill;
//            cmbNightIntensity.DropDownStyle = ComboBoxStyle.DropDownList;
//            cmbNightIntensity.FormattingEnabled = true;
//            cmbNightIntensity.Items.AddRange(new object[] { "25", "50", "75", "100" });
//            cmbNightIntensity.Location = new Point(181, 297);
//            cmbNightIntensity.Name = "cmbNightIntensity";
//            cmbNightIntensity.Size = new Size(98, 27);
//            cmbNightIntensity.TabIndex = 24;
//            cmbNightIntensity.TabStop = false;
//            // 
//            // cmbDayIntensity
//            // 
//            cmbDayIntensity.Dock = DockStyle.Fill;
//            cmbDayIntensity.DropDownStyle = ComboBoxStyle.DropDownList;
//            cmbDayIntensity.FormattingEnabled = true;
//            cmbDayIntensity.Items.AddRange(new object[] { "25", "50", "75", "100" });
//            cmbDayIntensity.Location = new Point(181, 254);
//            cmbDayIntensity.Margin = new Padding(3, 2, 3, 2);
//            cmbDayIntensity.Name = "cmbDayIntensity";
//            cmbDayIntensity.Size = new Size(98, 27);
//            cmbDayIntensity.TabIndex = 22;
//            cmbDayIntensity.TabStop = false;
//            // 
//            // mskSetNight
//            // 
//            mskSetNight.Dock = DockStyle.Fill;
//            mskSetNight.Location = new Point(181, 212);
//            mskSetNight.Margin = new Padding(3, 2, 3, 2);
//            mskSetNight.Mask = "00:00";
//            mskSetNight.Name = "mskSetNight";
//            mskSetNight.Size = new Size(98, 26);
//            mskSetNight.TabIndex = 20;
//            mskSetNight.TabStop = false;
//            mskSetNight.ValidatingType = typeof(DateTime);
//            // 
//            // mskSetDay
//            // 
//            mskSetDay.Dock = DockStyle.Fill;
//            mskSetDay.Location = new Point(181, 170);
//            mskSetDay.Margin = new Padding(3, 2, 3, 2);
//            mskSetDay.Mask = "00:00";
//            mskSetDay.Name = "mskSetDay";
//            mskSetDay.Size = new Size(98, 26);
//            mskSetDay.TabIndex = 18;
//            mskSetDay.TabStop = false;
//            mskSetDay.ValidatingType = typeof(DateTime);
//            // 
//            // label8
//            // 
//            label8.AutoSize = true;
//            label8.Dock = DockStyle.Fill;
//            label8.Location = new Point(3, 294);
//            label8.Name = "label8";
//            label8.Size = new Size(172, 46);
//            label8.TabIndex = 23;
//            label8.Text = "NIGHT INTENSITY";
//            // 
//            // label5
//            // 
//            label5.AutoSize = true;
//            label5.Dock = DockStyle.Fill;
//            label5.Location = new Point(3, 252);
//            label5.Name = "label5";
//            label5.Size = new Size(172, 42);
//            label5.TabIndex = 21;
//            label5.Text = "DAY INTENSITY";
//            // 
//            // label2
//            // 
//            label2.AutoSize = true;
//            label2.Dock = DockStyle.Fill;
//            label2.Location = new Point(3, 210);
//            label2.Name = "label2";
//            label2.Size = new Size(172, 42);
//            label2.TabIndex = 19;
//            label2.Text = "NIGHT TIME";
//            // 
//            // label1
//            // 
//            label1.AutoSize = true;
//            label1.Dock = DockStyle.Fill;
//            label1.Location = new Point(3, 168);
//            label1.Name = "label1";
//            label1.Size = new Size(172, 42);
//            label1.TabIndex = 17;
//            label1.Text = "DAY TIME";
//            // 
//            // chk_AutoIntensity
//            // 
//            chk_AutoIntensity.AutoSize = true;
//            chk_AutoIntensity.Dock = DockStyle.Fill;
//            chk_AutoIntensity.Location = new Point(3, 128);
//            chk_AutoIntensity.Margin = new Padding(3, 2, 3, 2);
//            chk_AutoIntensity.Name = "chk_AutoIntensity";
//            chk_AutoIntensity.Size = new Size(172, 38);
//            chk_AutoIntensity.TabIndex = 16;
//            chk_AutoIntensity.Text = "AUTO INTENSITY";
//            chk_AutoIntensity.UseVisualStyleBackColor = true;
//            // 
//            // chk_NtesAutoUpdate
//            // 
//            chk_NtesAutoUpdate.AutoSize = true;
//            chk_NtesAutoUpdate.Dock = DockStyle.Fill;
//            chk_NtesAutoUpdate.Location = new Point(3, 86);
//            chk_NtesAutoUpdate.Margin = new Padding(3, 2, 3, 2);
//            chk_NtesAutoUpdate.Name = "chk_NtesAutoUpdate";
//            chk_NtesAutoUpdate.Size = new Size(172, 38);
//            chk_NtesAutoUpdate.TabIndex = 14;
//            chk_NtesAutoUpdate.Text = "NTES AUTO UPDATE";
//            chk_NtesAutoUpdate.UseVisualStyleBackColor = true;
//            chk_NtesAutoUpdate.CheckedChanged += chk_NtesAutoUpdate_CheckedChanged;
//            // 
//            // cmbAutoUpdateTime
//            // 
//            cmbAutoUpdateTime.Dock = DockStyle.Fill;
//            cmbAutoUpdateTime.DropDownStyle = ComboBoxStyle.DropDownList;
//            cmbAutoUpdateTime.Enabled = true;
//            cmbAutoUpdateTime.FormattingEnabled = true;
//            cmbAutoUpdateTime.Items.AddRange(new object[] {"30", "60", "90", "120", "240", "480" });
//            cmbAutoUpdateTime.Location = new Point(181, 87);
//            cmbAutoUpdateTime.Name = "cmbAutoUpdateTime";
//            cmbAutoUpdateTime.Size = new Size(98, 27);
//            cmbAutoUpdateTime.TabIndex = 15;
//            cmbAutoUpdateTime.TabStop = false;
//            // 
//            // groupBox3
//            // 
//            groupBox3.Dock = DockStyle.Fill;
//            groupBox3.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
//            groupBox3.Location = new Point(3, 374);
//            groupBox3.Name = "groupBox3";
//            groupBox3.Size = new Size(422, 142);
//            groupBox3.TabIndex = 2;
//            groupBox3.TabStop = false;
//            groupBox3.Text = "Maintenance Mode";
//            // 
//            // groupBox4
//            // 
//            groupBox4.Dock = DockStyle.Fill;
//            groupBox4.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
//            groupBox4.Location = new Point(431, 374);
//            groupBox4.Name = "groupBox4";
//            groupBox4.Size = new Size(423, 142);
//            groupBox4.TabIndex = 3;
//            groupBox4.TabStop = false;
//            groupBox4.Text = "Third Party Integration";
//            // 
//            // panel1
//            // 
//            tableLayoutPanel1.SetColumnSpan(panel1, 2);
//            panel1.Controls.Add(tableLayoutPanel6);
//            panel1.Dock = DockStyle.Fill;
//            panel1.Location = new Point(3, 522);
//            panel1.Name = "panel1";
//            panel1.Size = new Size(851, 39);
//            panel1.TabIndex = 4;
//            // 
//            // tableLayoutPanel6
//            // 
//            tableLayoutPanel6.ColumnCount = 5;
//            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
//            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
//            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
//            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
//            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
//            tableLayoutPanel6.Controls.Add(btnSave, 2, 0);
//            tableLayoutPanel6.Dock = DockStyle.Fill;
//            tableLayoutPanel6.Location = new Point(0, 0);
//            tableLayoutPanel6.Name = "tableLayoutPanel6";
//            tableLayoutPanel6.RowCount = 1;
//            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
//            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
//            tableLayoutPanel6.Size = new Size(851, 39);
//            tableLayoutPanel6.TabIndex = 0;
//            // 
//            // btnSave
//            // 
//            btnSave.BackColor = Color.WhiteSmoke;
//            btnSave.Dock = DockStyle.Fill;
//            btnSave.FlatStyle = FlatStyle.Flat;
//            btnSave.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
//            btnSave.ForeColor = Color.Black;
//            btnSave.Location = new Point(343, 3);
//            btnSave.Name = "btnSave";
//            btnSave.Size = new Size(164, 33);
//            btnSave.TabIndex = 25;
//            btnSave.Text = "Save";
//            btnSave.UseVisualStyleBackColor = false;
//            btnSave.Click += btnSave_Click;
//            // 
//            // ucInterfaceAdminSetting
//            // 
//            AutoScaleDimensions = new SizeF(7F, 15F);
//            AutoScaleMode = AutoScaleMode.Font;
//            Controls.Add(tableLayoutPanel1);
//            Name = "ucInterfaceAdminSetting";
//            Size = new Size(857, 564);
//            Load += ucInterfaceAdminSetting_Load;
//            tableLayoutPanel1.ResumeLayout(false);
//            groupBox1.ResumeLayout(false);
//            tableLayoutPanel5.ResumeLayout(false);
//            tableLayoutPanel5.PerformLayout();
//            groupBox2.ResumeLayout(false);
//            tableLayoutPanel4.ResumeLayout(false);
//            tableLayoutPanel4.PerformLayout();
//            panel1.ResumeLayout(false);
//            tableLayoutPanel6.ResumeLayout(false);
//            ResumeLayout(false);

//        }

//        #endregion

//        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
//        private System.Windows.Forms.GroupBox groupBox1;
//        private System.Windows.Forms.GroupBox groupBox2;
//        private System.Windows.Forms.GroupBox groupBox3;
//        private System.Windows.Forms.GroupBox groupBox4;
//        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
//        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
//        private System.Windows.Forms.ComboBox cmbAutoUpdateTime;
//        private System.Windows.Forms.Label label4;
//        private System.Windows.Forms.Label label6;
//        private System.Windows.Forms.Label label7;
//        private System.Windows.Forms.Label label10;
//        private System.Windows.Forms.Label label11;
//        private System.Windows.Forms.Label label12;
//        private System.Windows.Forms.Label label9;
//        private System.Windows.Forms.ComboBox cmbRegLang;
//        private System.Windows.Forms.TextBox txtStationCode;
//        private System.Windows.Forms.TextBox txtPFEnd1;
//        private System.Windows.Forms.TextBox txtPFEnd2;
//        private System.Windows.Forms.DateTimePicker dateTimePicker1;
//        private System.Windows.Forms.DateTimePicker dateTimePicker2;
//        private System.Windows.Forms.ComboBox comboBox10;
//        private System.Windows.Forms.ComboBox cmbNightIntensity;
//        private System.Windows.Forms.Panel panel1;
//        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
//        private CheckBox chk_StartUp;
//        private MaskedTextBox mskSetDay;
//        private MaskedTextBox mskSetNight;
//        private TextBox txtStationName;
//        private Label label21;
//        private ComboBox cmbDayIntensity; 
//        private Button btnSave;
//        private ComboBox cmb_maxPlateform;
//        private ComboBox cmb_maxcoach;
//        private ComboBox cmb_maxEthernetport;
//        private TextBox txtPassKey;
//        private Label label3;
//        private CheckBox chk_Autolinkcheck;
//        private CheckBox chk_AutoAnnc;
//        private CheckBox chk_AutoIntensity;
//        private CheckBox chk_NtesAutoUpdate;
//        private Label label8;
//        private Label label5;
//        private Label label2;
//        private Label label1;
//        private CheckBox chk_resetstate;
//        private CheckBox chk_IsMaster;
//        private ComboBox cmbAnnounceLanguage;
//        private Label label13;
//    }
//}


namespace IPIS
{
    partial class ucInterfaceAdminSetting
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
            tableLayoutPanel1 = new TableLayoutPanel();
            groupBox1 = new GroupBox();
            tableLayoutPanel5 = new TableLayoutPanel();
            cmbAnnounceLanguage = new ComboBox();
            label13 = new Label();
            cmb_maxEthernetport = new ComboBox();
            cmb_maxPlateform = new ComboBox();
            cmb_maxcoach = new ComboBox();
            label4 = new Label();
            label6 = new Label();
            label7 = new Label();
            label3 = new Label();
            label9 = new Label();
            label12 = new Label();
            label11 = new Label();
            label21 = new Label();
            label10 = new Label();
            txtPassKey = new TextBox();
            txtPFEnd2 = new TextBox();
            txtPFEnd1 = new TextBox();
            txtStationCode = new TextBox();
            txtStationName = new TextBox();
            cmbRegLang = new ComboBox();
            groupBox2 = new GroupBox();
            tableLayoutPanel4 = new TableLayoutPanel();
            chk_IsMaster = new CheckBox();
            chk_Autolinkcheck = new CheckBox();
            chk_StartUp = new CheckBox();
            chk_AutoAnnc = new CheckBox();
            chk_resetstate = new CheckBox();
            cmbNightIntensity = new ComboBox();
            cmbDayIntensity = new ComboBox();
            mskSetNight = new MaskedTextBox();
            mskSetDay = new MaskedTextBox();
            label8 = new Label();
            label5 = new Label();
            label2 = new Label();
            label1 = new Label();
            chk_AutoIntensity = new CheckBox();
            chk_NtesAutoUpdate = new CheckBox();
            cmbAutoUpdateTime = new ComboBox();
            groupBox3 = new GroupBox();
            tableLayoutPanel7 = new TableLayoutPanel();
            lblMaintModeCaption = new Label();
            cmbMaintMode = new ComboBox();
            lblDefaultTimeCaption = new Label();
            txtDefaultTime = new TextBox();
            lblCustomStartCaption = new Label();
            mskMaintStart = new MaskedTextBox();
            lblCustomEndCaption = new Label();
            mskMaintEnd = new MaskedTextBox();
            btnGetLatestLedMatrix = new Button();
            groupBox4 = new GroupBox();
            panel1 = new Panel();
            tableLayoutPanel6 = new TableLayoutPanel();
            btnSave = new Button();
            tableLayoutPanel1.SuspendLayout();
            groupBox1.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            groupBox2.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            groupBox3.SuspendLayout();
            tableLayoutPanel7.SuspendLayout();
            panel1.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.Beige;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(groupBox1, 0, 0);
            tableLayoutPanel1.Controls.Add(groupBox2, 1, 0);
            tableLayoutPanel1.Controls.Add(groupBox3, 0, 1);
            tableLayoutPanel1.Controls.Add(groupBox4, 1, 1);
            tableLayoutPanel1.Controls.Add(panel1, 0, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 65.95744F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 26.41844F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 7.8014183F));
            tableLayoutPanel1.Size = new Size(857, 564);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(tableLayoutPanel5);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(422, 365);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Station Setting";
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.BackColor = Color.WhiteSmoke;
            tableLayoutPanel5.ColumnCount = 2;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.Controls.Add(cmbAnnounceLanguage, 1, 3);
            tableLayoutPanel5.Controls.Add(label13, 0, 3);
            tableLayoutPanel5.Controls.Add(cmb_maxEthernetport, 1, 2);
            tableLayoutPanel5.Controls.Add(cmb_maxPlateform, 1, 1);
            tableLayoutPanel5.Controls.Add(cmb_maxcoach, 1, 0);
            tableLayoutPanel5.Controls.Add(label4, 0, 0);
            tableLayoutPanel5.Controls.Add(label6, 0, 1);
            tableLayoutPanel5.Controls.Add(label7, 0, 2);
            tableLayoutPanel5.Controls.Add(label3, 0, 9);
            tableLayoutPanel5.Controls.Add(label9, 0, 8);
            tableLayoutPanel5.Controls.Add(label12, 0, 7);
            tableLayoutPanel5.Controls.Add(label11, 0, 6);
            tableLayoutPanel5.Controls.Add(label21, 0, 5);
            tableLayoutPanel5.Controls.Add(label10, 0, 4);
            tableLayoutPanel5.Controls.Add(txtPassKey, 1, 9);
            tableLayoutPanel5.Controls.Add(txtPFEnd2, 1, 8);
            tableLayoutPanel5.Controls.Add(txtPFEnd1, 1, 7);
            tableLayoutPanel5.Controls.Add(txtStationCode, 1, 6);
            tableLayoutPanel5.Controls.Add(txtStationName, 1, 5);
            tableLayoutPanel5.Controls.Add(cmbRegLang, 1, 4);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(3, 22);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 10;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10.0100088F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10.01001F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10.01001F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10.0100088F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10.01001F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10.01001F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10.01001F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10.01001F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10.01001F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 9.90991F));
            tableLayoutPanel5.Size = new Size(416, 340);
            tableLayoutPanel5.TabIndex = 0;
            // 
            // cmbAnnounceLanguage
            // 
            cmbAnnounceLanguage.Dock = DockStyle.Fill;
            cmbAnnounceLanguage.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbAnnounceLanguage.FormattingEnabled = true;
            cmbAnnounceLanguage.Items.AddRange(new object[] { "Select Language", "Hindi", "English", "Regional", "Hindi+English", "Hindi+Regional", "English+Regional", "Hindi+English+Regional" });
            cmbAnnounceLanguage.Location = new Point(211, 105);
            cmbAnnounceLanguage.Name = "cmbAnnounceLanguage";
            cmbAnnounceLanguage.Size = new Size(202, 27);
            cmbAnnounceLanguage.TabIndex = 27;
            cmbAnnounceLanguage.TabStop = false;
            cmbAnnounceLanguage.Tag = "";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Dock = DockStyle.Fill;
            label13.Location = new Point(3, 102);
            label13.Name = "label13";
            label13.Size = new Size(202, 34);
            label13.TabIndex = 26;
            label13.Text = "ANNOUNCE LANGUAGE";
            // 
            // cmb_maxEthernetport
            // 
            cmb_maxEthernetport.Dock = DockStyle.Fill;
            cmb_maxEthernetport.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb_maxEthernetport.FormattingEnabled = true;
            cmb_maxEthernetport.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24" });
            cmb_maxEthernetport.Location = new Point(211, 71);
            cmb_maxEthernetport.Name = "cmb_maxEthernetport";
            cmb_maxEthernetport.Size = new Size(202, 27);
            cmb_maxEthernetport.TabIndex = 3;
            cmb_maxEthernetport.TabStop = false;
            cmb_maxEthernetport.Tag = "";
            // 
            // cmb_maxPlateform
            // 
            cmb_maxPlateform.Dock = DockStyle.Fill;
            cmb_maxPlateform.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb_maxPlateform.FormattingEnabled = true;
            cmb_maxPlateform.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28" });
            cmb_maxPlateform.Location = new Point(211, 37);
            cmb_maxPlateform.Name = "cmb_maxPlateform";
            cmb_maxPlateform.Size = new Size(202, 27);
            cmb_maxPlateform.TabIndex = 2;
            cmb_maxPlateform.TabStop = false;
            cmb_maxPlateform.Tag = "";
            // 
            // cmb_maxcoach
            // 
            cmb_maxcoach.Dock = DockStyle.Fill;
            cmb_maxcoach.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb_maxcoach.FormattingEnabled = true;
            cmb_maxcoach.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28" });
            cmb_maxcoach.Location = new Point(211, 3);
            cmb_maxcoach.Name = "cmb_maxcoach";
            cmb_maxcoach.Size = new Size(202, 27);
            cmb_maxcoach.TabIndex = 1;
            cmb_maxcoach.TabStop = false;
            cmb_maxcoach.Tag = "";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = DockStyle.Fill;
            label4.Location = new Point(3, 0);
            label4.Name = "label4";
            label4.Size = new Size(202, 34);
            label4.TabIndex = 0;
            label4.Text = "MAX COACH";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Dock = DockStyle.Fill;
            label6.Location = new Point(3, 34);
            label6.Name = "label6";
            label6.Size = new Size(202, 34);
            label6.TabIndex = 1;
            label6.Text = "MAX PLATEFORM";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Dock = DockStyle.Fill;
            label7.Location = new Point(3, 68);
            label7.Name = "label7";
            label7.Size = new Size(202, 34);
            label7.TabIndex = 2;
            label7.Text = "MAX ETHERNET PORT";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Fill;
            label3.Location = new Point(3, 306);
            label3.Name = "label3";
            label3.Size = new Size(202, 34);
            label3.TabIndex = 25;
            label3.Text = "NTES PASS KEY";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Dock = DockStyle.Fill;
            label9.Location = new Point(3, 272);
            label9.Name = "label9";
            label9.Size = new Size(202, 34);
            label9.TabIndex = 8;
            label9.Text = "CAP PASSWORD";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Dock = DockStyle.Fill;
            label12.Location = new Point(3, 238);
            label12.Name = "label12";
            label12.Size = new Size(202, 34);
            label12.TabIndex = 7;
            label12.Text = "CAP PASS KEY";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Dock = DockStyle.Fill;
            label11.Location = new Point(3, 204);
            label11.Name = "label11";
            label11.Size = new Size(202, 34);
            label11.TabIndex = 6;
            label11.Text = "STATION CODE";
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Dock = DockStyle.Fill;
            label21.Location = new Point(3, 170);
            label21.Name = "label21";
            label21.Size = new Size(202, 34);
            label21.TabIndex = 17;
            label21.Text = "STATION NAME";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Dock = DockStyle.Fill;
            label10.Location = new Point(3, 136);
            label10.Name = "label10";
            label10.Size = new Size(202, 34);
            label10.TabIndex = 5;
            label10.Text = "REGIONAL LANGUAGE";
            // 
            // txtPassKey
            // 
            txtPassKey.Dock = DockStyle.Fill;
            txtPassKey.Location = new Point(211, 309);
            txtPassKey.Name = "txtPassKey";
            txtPassKey.Size = new Size(202, 26);
            txtPassKey.TabIndex = 9;
            // 
            // txtPFEnd2
            // 
            txtPFEnd2.Dock = DockStyle.Fill;
            txtPFEnd2.Location = new Point(211, 275);
            txtPFEnd2.Name = "txtPFEnd2";
            txtPFEnd2.Size = new Size(202, 26);
            txtPFEnd2.TabIndex = 8;
            // 
            // txtPFEnd1
            // 
            txtPFEnd1.Dock = DockStyle.Fill;
            txtPFEnd1.Location = new Point(211, 241);
            txtPFEnd1.Name = "txtPFEnd1";
            txtPFEnd1.Size = new Size(202, 26);
            txtPFEnd1.TabIndex = 7;
            // 
            // txtStationCode
            // 
            txtStationCode.Dock = DockStyle.Fill;
            txtStationCode.Location = new Point(211, 207);
            txtStationCode.Name = "txtStationCode";
            txtStationCode.Size = new Size(202, 26);
            txtStationCode.TabIndex = 6;
            // 
            // txtStationName
            // 
            txtStationName.Dock = DockStyle.Fill;
            txtStationName.Location = new Point(211, 173);
            txtStationName.Name = "txtStationName";
            txtStationName.Size = new Size(202, 26);
            txtStationName.TabIndex = 5;
            // 
            // cmbRegLang
            // 
            cmbRegLang.Dock = DockStyle.Fill;
            cmbRegLang.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRegLang.FormattingEnabled = true;
            cmbRegLang.Items.AddRange(new object[] { "Select Language", "Assamese ", "Bengali", "Bodo", "Dogri", "Gujarati", "Kannada", "Kashmiri", "Konkani", "Maithili", "Malayalam", "Manipuri", "Marathi", "Nepali", "Odia", "Punjabi", "Sanskrit", "Santali", "Sindhi", "Tamil", "Telugu", "Urdu" });
            cmbRegLang.Location = new Point(211, 139);
            cmbRegLang.Name = "cmbRegLang";
            cmbRegLang.Size = new Size(202, 27);
            cmbRegLang.TabIndex = 4;
            cmbRegLang.TabStop = false;
            cmbRegLang.Tag = "";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(tableLayoutPanel4);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            groupBox2.Location = new Point(431, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(423, 365);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Automatic Functionality";
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.BackColor = Color.WhiteSmoke;
            tableLayoutPanel4.ColumnCount = 3;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42.6858521F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 24.9400482F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32.1342926F));
            tableLayoutPanel4.Controls.Add(chk_IsMaster, 1, 1);
            tableLayoutPanel4.Controls.Add(chk_Autolinkcheck, 0, 0);
            tableLayoutPanel4.Controls.Add(chk_StartUp, 1, 0);
            tableLayoutPanel4.Controls.Add(chk_AutoAnnc, 2, 0);
            tableLayoutPanel4.Controls.Add(chk_resetstate, 0, 1);
            tableLayoutPanel4.Controls.Add(cmbNightIntensity, 1, 7);
            tableLayoutPanel4.Controls.Add(cmbDayIntensity, 1, 6);
            tableLayoutPanel4.Controls.Add(mskSetNight, 1, 5);
            tableLayoutPanel4.Controls.Add(mskSetDay, 1, 4);
            tableLayoutPanel4.Controls.Add(label8, 0, 7);
            tableLayoutPanel4.Controls.Add(label5, 0, 6);
            tableLayoutPanel4.Controls.Add(label2, 0, 5);
            tableLayoutPanel4.Controls.Add(label1, 0, 4);
            tableLayoutPanel4.Controls.Add(chk_AutoIntensity, 0, 3);
            tableLayoutPanel4.Controls.Add(chk_NtesAutoUpdate, 0, 2);
            tableLayoutPanel4.Controls.Add(cmbAutoUpdateTime, 1, 2);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(3, 22);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 8;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel4.Size = new Size(417, 340);
            tableLayoutPanel4.TabIndex = 0;
            // 
            // chk_IsMaster
            // 
            chk_IsMaster.AutoSize = true;
            chk_IsMaster.Dock = DockStyle.Fill;
            chk_IsMaster.Location = new Point(181, 44);
            chk_IsMaster.Margin = new Padding(3, 2, 3, 2);
            chk_IsMaster.Name = "chk_IsMaster";
            chk_IsMaster.Size = new Size(98, 38);
            chk_IsMaster.TabIndex = 25;
            chk_IsMaster.Text = "IS Master";
            chk_IsMaster.UseVisualStyleBackColor = true;
            // 
            // chk_Autolinkcheck
            // 
            chk_Autolinkcheck.AutoSize = true;
            chk_Autolinkcheck.Dock = DockStyle.Fill;
            chk_Autolinkcheck.Location = new Point(3, 2);
            chk_Autolinkcheck.Margin = new Padding(3, 2, 3, 2);
            chk_Autolinkcheck.Name = "chk_Autolinkcheck";
            chk_Autolinkcheck.Size = new Size(172, 38);
            chk_Autolinkcheck.TabIndex = 10;
            chk_Autolinkcheck.Text = "AUTO LINK CHECK";
            chk_Autolinkcheck.UseVisualStyleBackColor = true;
            // 
            // chk_StartUp
            // 
            chk_StartUp.AutoSize = true;
            chk_StartUp.Dock = DockStyle.Fill;
            chk_StartUp.Location = new Point(181, 2);
            chk_StartUp.Margin = new Padding(3, 2, 3, 2);
            chk_StartUp.Name = "chk_StartUp";
            chk_StartUp.Size = new Size(98, 38);
            chk_StartUp.TabIndex = 11;
            chk_StartUp.Text = "START UP";
            chk_StartUp.UseVisualStyleBackColor = true;
            // 
            // chk_AutoAnnc
            // 
            chk_AutoAnnc.AutoSize = true;
            chk_AutoAnnc.Dock = DockStyle.Fill;
            chk_AutoAnnc.Location = new Point(285, 2);
            chk_AutoAnnc.Margin = new Padding(3, 2, 3, 2);
            chk_AutoAnnc.Name = "chk_AutoAnnc";
            chk_AutoAnnc.Size = new Size(129, 38);
            chk_AutoAnnc.TabIndex = 12;
            chk_AutoAnnc.Text = "AUTO ANNC";
            chk_AutoAnnc.UseVisualStyleBackColor = true;
            // 
            // chk_resetstate
            // 
            chk_resetstate.AutoSize = true;
            chk_resetstate.Dock = DockStyle.Fill;
            chk_resetstate.Location = new Point(3, 44);
            chk_resetstate.Margin = new Padding(3, 2, 3, 2);
            chk_resetstate.Name = "chk_resetstate";
            chk_resetstate.Size = new Size(172, 38);
            chk_resetstate.TabIndex = 13;
            chk_resetstate.Text = "RESET STATE";
            chk_resetstate.UseVisualStyleBackColor = true;
            // 
            // cmbNightIntensity
            // 
            cmbNightIntensity.Dock = DockStyle.Fill;
            cmbNightIntensity.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbNightIntensity.FormattingEnabled = true;
            cmbNightIntensity.Items.AddRange(new object[] { "25", "50", "75", "100" });
            cmbNightIntensity.Location = new Point(181, 297);
            cmbNightIntensity.Name = "cmbNightIntensity";
            cmbNightIntensity.Size = new Size(98, 27);
            cmbNightIntensity.TabIndex = 24;
            cmbNightIntensity.TabStop = false;
            // 
            // cmbDayIntensity
            // 
            cmbDayIntensity.Dock = DockStyle.Fill;
            cmbDayIntensity.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDayIntensity.FormattingEnabled = true;
            cmbDayIntensity.Items.AddRange(new object[] { "25", "50", "75", "100" });
            cmbDayIntensity.Location = new Point(181, 254);
            cmbDayIntensity.Margin = new Padding(3, 2, 3, 2);
            cmbDayIntensity.Name = "cmbDayIntensity";
            cmbDayIntensity.Size = new Size(98, 27);
            cmbDayIntensity.TabIndex = 22;
            cmbDayIntensity.TabStop = false;
            // 
            // mskSetNight
            // 
            mskSetNight.Dock = DockStyle.Fill;
            mskSetNight.Location = new Point(181, 212);
            mskSetNight.Margin = new Padding(3, 2, 3, 2);
            mskSetNight.Mask = "00:00";
            mskSetNight.Name = "mskSetNight";
            mskSetNight.Size = new Size(98, 26);
            mskSetNight.TabIndex = 20;
            mskSetNight.TabStop = false;
            mskSetNight.ValidatingType = typeof(DateTime);
            // 
            // mskSetDay
            // 
            mskSetDay.Dock = DockStyle.Fill;
            mskSetDay.Location = new Point(181, 170);
            mskSetDay.Margin = new Padding(3, 2, 3, 2);
            mskSetDay.Mask = "00:00";
            mskSetDay.Name = "mskSetDay";
            mskSetDay.Size = new Size(98, 26);
            mskSetDay.TabIndex = 18;
            mskSetDay.TabStop = false;
            mskSetDay.ValidatingType = typeof(DateTime);
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Dock = DockStyle.Fill;
            label8.Location = new Point(3, 294);
            label8.Name = "label8";
            label8.Size = new Size(172, 46);
            label8.TabIndex = 23;
            label8.Text = "NIGHT INTENSITY";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Dock = DockStyle.Fill;
            label5.Location = new Point(3, 252);
            label5.Name = "label5";
            label5.Size = new Size(172, 42);
            label5.TabIndex = 21;
            label5.Text = "DAY INTENSITY";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Fill;
            label2.Location = new Point(3, 210);
            label2.Name = "label2";
            label2.Size = new Size(172, 42);
            label2.TabIndex = 19;
            label2.Text = "NIGHT TIME";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(3, 168);
            label1.Name = "label1";
            label1.Size = new Size(172, 42);
            label1.TabIndex = 17;
            label1.Text = "DAY TIME";
            // 
            // chk_AutoIntensity
            // 
            chk_AutoIntensity.AutoSize = true;
            chk_AutoIntensity.Dock = DockStyle.Fill;
            chk_AutoIntensity.Location = new Point(3, 128);
            chk_AutoIntensity.Margin = new Padding(3, 2, 3, 2);
            chk_AutoIntensity.Name = "chk_AutoIntensity";
            chk_AutoIntensity.Size = new Size(172, 38);
            chk_AutoIntensity.TabIndex = 16;
            chk_AutoIntensity.Text = "AUTO INTENSITY";
            chk_AutoIntensity.UseVisualStyleBackColor = true;
            // 
            // chk_NtesAutoUpdate
            // 
            chk_NtesAutoUpdate.AutoSize = true;
            chk_NtesAutoUpdate.Dock = DockStyle.Fill;
            chk_NtesAutoUpdate.Location = new Point(3, 86);
            chk_NtesAutoUpdate.Margin = new Padding(3, 2, 3, 2);
            chk_NtesAutoUpdate.Name = "chk_NtesAutoUpdate";
            chk_NtesAutoUpdate.Size = new Size(172, 38);
            chk_NtesAutoUpdate.TabIndex = 14;
            chk_NtesAutoUpdate.Text = "NTES AUTO UPDATE";
            chk_NtesAutoUpdate.UseVisualStyleBackColor = true;
            chk_NtesAutoUpdate.CheckedChanged += chk_NtesAutoUpdate_CheckedChanged;
            // 
            // cmbAutoUpdateTime
            // 
            cmbAutoUpdateTime.Dock = DockStyle.Fill;
            cmbAutoUpdateTime.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbAutoUpdateTime.Enabled = true;
            cmbAutoUpdateTime.FormattingEnabled = true;
            cmbAutoUpdateTime.Items.AddRange(new object[] { "30", "60", "90", "120", "240", "480" });
            cmbAutoUpdateTime.Location = new Point(181, 87);
            cmbAutoUpdateTime.Name = "cmbAutoUpdateTime";
            cmbAutoUpdateTime.Size = new Size(98, 27);
            cmbAutoUpdateTime.TabIndex = 15;
            cmbAutoUpdateTime.TabStop = false;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(tableLayoutPanel7);
            groupBox3.Dock = DockStyle.Fill;
            groupBox3.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            groupBox3.Location = new Point(3, 374);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(422, 142);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "Maintenance Mode";
            // 
            // tableLayoutPanel7
            // 
            tableLayoutPanel7.BackColor = Color.WhiteSmoke;
            tableLayoutPanel7.ColumnCount = 3;
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel7.Controls.Add(lblMaintModeCaption, 0, 0);
            tableLayoutPanel7.Controls.Add(cmbMaintMode, 1, 0);
            tableLayoutPanel7.Controls.Add(lblDefaultTimeCaption, 0, 1);
            tableLayoutPanel7.Controls.Add(txtDefaultTime, 1, 1);
            tableLayoutPanel7.Controls.Add(lblCustomStartCaption, 0, 2);
            tableLayoutPanel7.Controls.Add(mskMaintStart, 1, 2);
            tableLayoutPanel7.Controls.Add(lblCustomEndCaption, 0, 3);
            tableLayoutPanel7.Controls.Add(mskMaintEnd, 1, 3);
            tableLayoutPanel7.Controls.Add(btnGetLatestLedMatrix, 2, 3);
            tableLayoutPanel7.Dock = DockStyle.Fill;
            tableLayoutPanel7.Location = new Point(3, 22);
            tableLayoutPanel7.Name = "tableLayoutPanel7";
            tableLayoutPanel7.RowCount = 4;
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel7.Size = new Size(416, 117);
            tableLayoutPanel7.TabIndex = 0;
            // 
            // lblMaintModeCaption
            // 
            lblMaintModeCaption.AutoSize = true;
            lblMaintModeCaption.Dock = DockStyle.Fill;
            lblMaintModeCaption.Location = new Point(3, 0);
            lblMaintModeCaption.Name = "lblMaintModeCaption";
            lblMaintModeCaption.Size = new Size(160, 29);
            lblMaintModeCaption.TabIndex = 0;
            lblMaintModeCaption.Text = "MAINTENANCE MODE";
            lblMaintModeCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbMaintMode
            // 
            cmbMaintMode.Dock = DockStyle.Fill;
            cmbMaintMode.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMaintMode.FormattingEnabled = true;
            cmbMaintMode.Items.AddRange(new object[] { "Use Default Time", "Use Custom Time" });
            cmbMaintMode.Location = new Point(169, 3);
            cmbMaintMode.Name = "cmbMaintMode";
            cmbMaintMode.Size = new Size(139, 27);
            cmbMaintMode.TabIndex = 1;
            cmbMaintMode.SelectedIndexChanged += cmbMaintMode_SelectedIndexChanged;
            // 
            // lblDefaultTimeCaption
            // 
            lblDefaultTimeCaption.AutoSize = true;
            lblDefaultTimeCaption.Dock = DockStyle.Fill;
            lblDefaultTimeCaption.Location = new Point(3, 29);
            lblDefaultTimeCaption.Name = "lblDefaultTimeCaption";
            lblDefaultTimeCaption.Size = new Size(160, 29);
            lblDefaultTimeCaption.TabIndex = 2;
            lblDefaultTimeCaption.Text = "DEFAULT TIME";
            lblDefaultTimeCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtDefaultTime
            // 
            txtDefaultTime.BackColor = Color.LightGray;
            txtDefaultTime.Dock = DockStyle.Fill;
            txtDefaultTime.Location = new Point(169, 32);
            txtDefaultTime.Name = "txtDefaultTime";
            txtDefaultTime.ReadOnly = true;
            txtDefaultTime.Size = new Size(139, 26);
            txtDefaultTime.TabIndex = 3;
            txtDefaultTime.TabStop = false;
            txtDefaultTime.Text = "01:00 AM - 03:00 AM";
            txtDefaultTime.TextAlign = HorizontalAlignment.Center;
            // 
            // lblCustomStartCaption
            // 
            lblCustomStartCaption.AutoSize = true;
            lblCustomStartCaption.Dock = DockStyle.Fill;
            lblCustomStartCaption.Location = new Point(3, 58);
            lblCustomStartCaption.Name = "lblCustomStartCaption";
            lblCustomStartCaption.Size = new Size(160, 29);
            lblCustomStartCaption.TabIndex = 4;
            lblCustomStartCaption.Text = "START TIME";
            lblCustomStartCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // mskMaintStart
            // 
            mskMaintStart.Dock = DockStyle.Fill;
            mskMaintStart.Location = new Point(169, 61);
            mskMaintStart.Name = "mskMaintStart";
            mskMaintStart.Size = new Size(139, 26);
            mskMaintStart.TabIndex = 5;
            mskMaintStart.Mask = "00:00";
            mskMaintStart.ValidatingType = typeof(DateTime);
            // 
            // lblCustomEndCaption
            // 
            lblCustomEndCaption.AutoSize = true;
            lblCustomEndCaption.Dock = DockStyle.Fill;
            lblCustomEndCaption.Location = new Point(3, 87);
            lblCustomEndCaption.Name = "lblCustomEndCaption";
            lblCustomEndCaption.Size = new Size(160, 30);
            lblCustomEndCaption.TabIndex = 6;
            lblCustomEndCaption.Text = "END TIME";
            lblCustomEndCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // mskMaintEnd
            // 
            mskMaintEnd.Dock = DockStyle.Fill;
            mskMaintEnd.Location = new Point(169, 90);
            mskMaintEnd.Name = "mskMaintEnd";
            mskMaintEnd.Size = new Size(139, 26);
            mskMaintEnd.TabIndex = 7;
            mskMaintEnd.Mask = "00:00";
            mskMaintEnd.ValidatingType = typeof(DateTime);
            // 
            // btnGetLatestLedMatrix
            // 
            btnGetLatestLedMatrix.BackColor = Color.WhiteSmoke;
            btnGetLatestLedMatrix.Dock = DockStyle.Fill;
            btnGetLatestLedMatrix.FlatStyle = FlatStyle.Flat;
            btnGetLatestLedMatrix.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnGetLatestLedMatrix.Location = new Point(314, 90);
            btnGetLatestLedMatrix.Name = "btnGetLatestLedMatrix";
            btnGetLatestLedMatrix.Size = new Size(99, 24);
            btnGetLatestLedMatrix.TabIndex = 8;
            btnGetLatestLedMatrix.Text = "Get Latest Matrix";
            btnGetLatestLedMatrix.UseVisualStyleBackColor = false;
            btnGetLatestLedMatrix.Click += btnGetLatestLedMatrix_Click;
            // 
            // groupBox4
            // 
            groupBox4.Dock = DockStyle.Fill;
            groupBox4.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            groupBox4.Location = new Point(431, 374);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(423, 142);
            groupBox4.TabIndex = 3;
            groupBox4.TabStop = false;
            groupBox4.Text = "Third Party Integration";
            // 
            // panel1
            // 
            tableLayoutPanel1.SetColumnSpan(panel1, 2);
            panel1.Controls.Add(tableLayoutPanel6);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 522);
            panel1.Name = "panel1";
            panel1.Size = new Size(851, 39);
            panel1.TabIndex = 4;
            // 
            // tableLayoutPanel6
            // 
            tableLayoutPanel6.ColumnCount = 5;
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel6.Controls.Add(btnSave, 2, 0);
            tableLayoutPanel6.Dock = DockStyle.Fill;
            tableLayoutPanel6.Location = new Point(0, 0);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 1;
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel6.Size = new Size(851, 39);
            tableLayoutPanel6.TabIndex = 0;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.WhiteSmoke;
            btnSave.Dock = DockStyle.Fill;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.ForeColor = Color.Black;
            btnSave.Location = new Point(343, 3);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(164, 33);
            btnSave.TabIndex = 25;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // ucInterfaceAdminSetting
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel1);
            Name = "ucInterfaceAdminSetting";
            Size = new Size(857, 564);
            Load += ucInterfaceAdminSetting_Load;
            tableLayoutPanel1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel5.PerformLayout();
            groupBox2.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            groupBox3.ResumeLayout(false);
            tableLayoutPanel7.ResumeLayout(false);
            tableLayoutPanel7.PerformLayout();
            panel1.ResumeLayout(false);
            tableLayoutPanel6.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.ComboBox cmbAutoUpdateTime;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbRegLang;
        private System.Windows.Forms.TextBox txtStationCode;
        private System.Windows.Forms.TextBox txtPFEnd1;
        private System.Windows.Forms.TextBox txtPFEnd2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.ComboBox comboBox10;
        private System.Windows.Forms.ComboBox cmbNightIntensity;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private CheckBox chk_StartUp;
        private MaskedTextBox mskSetDay;
        private MaskedTextBox mskSetNight;
        private TextBox txtStationName;
        private Label label21;
        private ComboBox cmbDayIntensity;
        private Button btnSave;
        private ComboBox cmb_maxPlateform;
        private ComboBox cmb_maxcoach;
        private ComboBox cmb_maxEthernetport;
        private TextBox txtPassKey;
        private Label label3;
        private CheckBox chk_Autolinkcheck;
        private CheckBox chk_AutoAnnc;
        private CheckBox chk_AutoIntensity;
        private CheckBox chk_NtesAutoUpdate;
        private Label label8;
        private Label label5;
        private Label label2;
        private Label label1;
        private CheckBox chk_resetstate;
        private CheckBox chk_IsMaster;
        private ComboBox cmbAnnounceLanguage;
        private Label label13;

        // ===== Maintenance Mode (new controls) =====
        private TableLayoutPanel tableLayoutPanel7;
        private Label lblMaintModeCaption;
        private ComboBox cmbMaintMode;
        private Label lblDefaultTimeCaption;
        private TextBox txtDefaultTime;
        private Label lblCustomStartCaption;
        private MaskedTextBox mskMaintStart;
        private Label lblCustomEndCaption;
        private MaskedTextBox mskMaintEnd;
        private Button btnGetLatestLedMatrix;
    }
}