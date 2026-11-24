namespace IPIS
{
    partial class ucCGDBBoardSetting
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
            lblboardHeader = new Label();
            tableLayoutPanel2 = new TableLayoutPanel();
            groupBox1 = new GroupBox();
            tableLayoutPanel4 = new TableLayoutPanel();
            cmbDisplayinfo = new ComboBox();
            cmbEffect = new ComboBox();
            //cmbLetterGap = new ComboBox();
            cmbScrollSpeed = new ComboBox();
            cmbDelayTime = new ComboBox();
            cmbBoardSize = new ComboBox();
            cmbFontSize = new ComboBox();
            cmbIntensity = new ComboBox();
            label25 = new Label();
            label23 = new Label();
            //label21 = new Label();
            label19 = new Label();
            label15 = new Label();
            label13 = new Label();
            label11 = new Label();
            intensityLbl = new Label();
            lblDto = new Label();
            cmbDTO = new ComboBox();
            tableLayoutPanel3 = new TableLayoutPanel();
            groupBox3 = new GroupBox();
            tableLayoutPanel7 = new TableLayoutPanel();
            flow_layout_language = new FlowLayoutPanel();
            tableLayoutPanel8 = new TableLayoutPanel();
            lblSoftReset = new Label();
            checkBoxSoftReset = new CheckBox();
            groupBox2 = new GroupBox();
            tableLayoutPanel5 = new TableLayoutPanel();
            cmb_plateformNo = new ComboBox();
            label1 = new Label();
            txtLocation = new TextBox();
            label10 = new Label();
            label6 = new Label();
            label2 = new Label();
            cmbNoCoach = new ComboBox();
            chkIsActive = new CheckBox();
            cmbPF_END = new ComboBox();
            tableLayoutPanel6 = new TableLayoutPanel();
            btnSave = new Button();
            btndelete = new Button();
            btn_close = new Button();
            flow_layoutpanel = new FlowLayoutPanel();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            groupBox1.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            groupBox3.SuspendLayout();
            tableLayoutPanel7.SuspendLayout();
            tableLayoutPanel8.SuspendLayout();
            groupBox2.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.Beige;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(lblboardHeader, 0, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel6, 0, 3);
            tableLayoutPanel1.Controls.Add(flow_layoutpanel, 0, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tableLayoutPanel1.ForeColor = SystemColors.ControlText;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 3.98565078F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 61.53846F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 27.75264F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6.636501F));
            tableLayoutPanel1.Size = new Size(875, 884);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // lblboardHeader
            // 
            lblboardHeader.AutoSize = true;
            lblboardHeader.Dock = DockStyle.Fill;
            lblboardHeader.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblboardHeader.ForeColor = Color.Maroon;
            lblboardHeader.Location = new Point(3, 0);
            lblboardHeader.Name = "lblboardHeader";
            lblboardHeader.Size = new Size(869, 35);
            lblboardHeader.TabIndex = 0;
            lblboardHeader.Text = "CGDB";
            lblboardHeader.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(groupBox1, 1, 0);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel3, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 39);
            tableLayoutPanel2.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(869, 536);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.WhiteSmoke;
            groupBox1.Controls.Add(tableLayoutPanel4);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(437, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(429, 530);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Display Property";
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.BackColor = Color.Beige;
            tableLayoutPanel4.ColumnCount = 2;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            //tableLayoutPanel4.Controls.Add(cmbDisplayinfo, 1, 5);   // Info Displayed removed
            tableLayoutPanel4.Controls.Add(cmbEffect, 1, 4);
            //tableLayoutPanel4.Controls.Add(cmbLetterGap, 1, 4);
            tableLayoutPanel4.Controls.Add(cmbScrollSpeed, 1, 3);
            tableLayoutPanel4.Controls.Add(cmbDelayTime, 1, 2);
            tableLayoutPanel4.Controls.Add(cmbBoardSize, 1, 1);
            tableLayoutPanel4.Controls.Add(cmbFontSize, 1, 0);
            tableLayoutPanel4.Controls.Add(cmbIntensity, 1, 1);
            //tableLayoutPanel4.Controls.Add(label25, 0, 5);   // Info Displayed removed
            tableLayoutPanel4.Controls.Add(label23, 0, 4);
            //tableLayoutPanel4.Controls.Add(label21, 0, 4);
            tableLayoutPanel4.Controls.Add(label19, 0, 3);
            tableLayoutPanel4.Controls.Add(label15, 0, 2);
            tableLayoutPanel4.Controls.Add(label13, 0, 1);
            tableLayoutPanel4.Controls.Add(label11, 0, 0);
            tableLayoutPanel4.Controls.Add(intensityLbl, 0, 1);
            // DTO label and combo box (row 5, moved up after Info Displayed was removed)
            //tableLayoutPanel4.Controls.Add(lblDto, 0, 6);
            //tableLayoutPanel4.Controls.Add(cmbDTO, 1, 6);
            tableLayoutPanel4.Controls.Add(lblDto, 0, 5);
            tableLayoutPanel4.Controls.Add(cmbDTO, 1, 5);
            // Soft Reset label and check box (row 6), moved here from Language Selection
            //tableLayoutPanel4.Controls.Add(lblSoftReset, 0, 7);
            //tableLayoutPanel4.Controls.Add(checkBoxSoftReset, 1, 7);
            tableLayoutPanel4.Controls.Add(lblSoftReset, 0, 6);
            tableLayoutPanel4.Controls.Add(checkBoxSoftReset, 1, 6);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(3, 26);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            //tableLayoutPanel4.RowCount = 6;
            //tableLayoutPanel4.RowCount = 7;
            //tableLayoutPanel4.RowCount = 8;
            tableLayoutPanel4.RowCount = 7;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            //tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));   // Info Displayed row removed
            // DTO row
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            // Soft Reset row
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel4.Size = new Size(423, 501);
            tableLayoutPanel4.TabIndex = 0;
            // 
            // cmbDisplayinfo
            // 
            cmbDisplayinfo.Dock = DockStyle.Fill;
            cmbDisplayinfo.FormattingEnabled = true;
            cmbDisplayinfo.Items.AddRange(new object[] { "BOTH", "ARR", "DEP" });
            cmbDisplayinfo.Location = new Point(214, 361);
            cmbDisplayinfo.Name = "cmbDisplayinfo";
            cmbDisplayinfo.Size = new Size(206, 31);
            cmbDisplayinfo.TabIndex = 14;
            // 
            // cmbEffect
            // 
            cmbEffect.Dock = DockStyle.Fill;
            cmbEffect.FormattingEnabled = true;
            cmbEffect.Location = new Point(214, 290);
            cmbEffect.Name = "cmbEffect";
            cmbEffect.Size = new Size(206, 31);
            cmbEffect.TabIndex = 13;
            // 
            // cmbLetterGap
            // 
            //cmbLetterGap.Dock = DockStyle.Fill;
            //cmbLetterGap.FormattingEnabled = true;
            //cmbLetterGap.Location = new Point(214, 290);
            //cmbLetterGap.Name = "cmbLetterGap";
            //cmbLetterGap.Size = new Size(206, 31);
            //cmbLetterGap.TabIndex = 12;
            // 
            // cmbScrollSpeed
            // 
            cmbScrollSpeed.Dock = DockStyle.Fill;
            cmbScrollSpeed.FormattingEnabled = true;
            cmbScrollSpeed.Location = new Point(214, 218);
            cmbScrollSpeed.Name = "cmbScrollSpeed";
            cmbScrollSpeed.Size = new Size(206, 31);
            cmbScrollSpeed.TabIndex = 11;
            // 
            // cmbDelayTime
            // 
            cmbDelayTime.Dock = DockStyle.Fill;
            cmbDelayTime.FormattingEnabled = true;
            cmbDelayTime.Items.AddRange(new object[] { "5", "10", "20", "30", "40", "50", "60", "70", "80", "90", "100", "120" });
            cmbDelayTime.Location = new Point(214, 146);
            cmbDelayTime.Name = "cmbDelayTime";
            cmbDelayTime.Size = new Size(206, 31);
            cmbDelayTime.TabIndex = 9;
            // 
            // cmbBoardSize
            // 
            cmbBoardSize.Dock = DockStyle.Fill;
            cmbBoardSize.FormattingEnabled = true;
            cmbBoardSize.Items.AddRange(new object[] { "16*336" });
            cmbBoardSize.Location = new Point(214, 74);
            cmbBoardSize.Name = "cmbBoardSize";
            cmbBoardSize.Size = new Size(206, 31);
            cmbBoardSize.TabIndex = 8;
            cmbBoardSize.Visible = false;
            // 
            // cmbFontSize
            // 
            cmbFontSize.Dock = DockStyle.Fill;
            cmbFontSize.FormattingEnabled = true;
            cmbFontSize.Location = new Point(214, 3);
            cmbFontSize.Name = "cmbFontSize";
            cmbFontSize.Size = new Size(206, 31);
            cmbFontSize.TabIndex = 7;
            // 
            // cmbIntensity
            // 
            cmbIntensity.Dock = DockStyle.Fill;
            cmbIntensity.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbIntensity.FormattingEnabled = true;
            cmbIntensity.Items.AddRange(new object[] { "25", "50", "75", "100" });
            cmbIntensity.Location = new Point(214, 74);
            cmbIntensity.Name = "cmbIntensity";
            cmbIntensity.Size = new Size(206, 31);
            cmbIntensity.TabIndex = 21;
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.Dock = DockStyle.Fill;
            label25.Location = new Point(3, 358);
            label25.Name = "label25";
            label25.Size = new Size(205, 72);
            label25.TabIndex = 18;
            label25.Text = "Info Displayed";
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.Dock = DockStyle.Fill;
            label23.Location = new Point(3, 286);
            label23.Name = "label23";
            label23.Size = new Size(205, 71);
            label23.TabIndex = 16;
            label23.Text = "Effect";
            // 
            // label21
            // 
            //label21.AutoSize = true;
            //label21.Dock = DockStyle.Fill;
            //label21.Location = new Point(3, 286);
            //label21.Name = "label21";
            //label21.Size = new Size(205, 72);
            //label21.TabIndex = 14;
            //label21.Text = "Letter Gap";
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Dock = DockStyle.Fill;
            label19.Location = new Point(3, 215);
            label19.Name = "label19";
            label19.Size = new Size(205, 71);
            label19.TabIndex = 12;
            label19.Text = "Scrolling Speed";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Dock = DockStyle.Fill;
            label15.Location = new Point(3, 143);
            label15.Name = "label15";
            label15.Size = new Size(205, 72);
            label15.TabIndex = 8;
            label15.Text = "PTO (in Sec)";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Dock = DockStyle.Fill;
            label13.Location = new Point(3, 72);
            label13.Name = "label13";
            label13.Size = new Size(205, 71);
            label13.TabIndex = 6;
            label13.Text = "Board Size";
            label13.Visible = false;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Dock = DockStyle.Fill;
            label11.Location = new Point(3, 0);
            label11.Name = "label11";
            label11.Size = new Size(205, 72);
            label11.TabIndex = 4;
            label11.Text = "Font Size";
            // 
            // intensityLbl
            // 
            intensityLbl.AutoSize = true;
            intensityLbl.Dock = DockStyle.Fill;
            intensityLbl.Location = new Point(3, 72);
            intensityLbl.Name = "intensityLbl";
            intensityLbl.Size = new Size(205, 71);
            intensityLbl.TabIndex = 19;
            intensityLbl.Text = "Board Intensity";
            // 
            // lblDto
            // 
            lblDto.AutoSize = true;
            lblDto.Dock = DockStyle.Fill;
            lblDto.Location = new Point(3, 432);
            lblDto.Name = "lblDto";
            lblDto.Size = new Size(205, 69);
            lblDto.TabIndex = 22;
            lblDto.Text = "DTO";
            // 
            // cmbDTO
            // 
            cmbDTO.Dock = DockStyle.Fill;
            cmbDTO.FormattingEnabled = true;
            // DTO in seconds: 60 to 240 in steps of 30
            cmbDTO.Items.AddRange(new object[] { "60", "90", "120", "150", "180", "210", "240" });
            cmbDTO.Location = new Point(214, 435);
            cmbDTO.Name = "cmbDTO";
            cmbDTO.Size = new Size(206, 31);
            cmbDTO.TabIndex = 23;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Controls.Add(groupBox3, 0, 1);
            tableLayoutPanel3.Controls.Add(groupBox2, 0, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 4);
            tableLayoutPanel3.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 2;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 71.969696F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 28.030304F));
            tableLayoutPanel3.Size = new Size(428, 528);
            tableLayoutPanel3.TabIndex = 3;
            // 
            // groupBox3
            // 
            groupBox3.BackColor = Color.WhiteSmoke;
            groupBox3.Controls.Add(tableLayoutPanel7);
            groupBox3.Dock = DockStyle.Fill;
            groupBox3.Location = new Point(3, 383);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(422, 142);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "Language Selection";
            // 
            // tableLayoutPanel7
            // 
            tableLayoutPanel7.ColumnCount = 2;
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tableLayoutPanel7.Controls.Add(flow_layout_language, 0, 0);
            //tableLayoutPanel7.Controls.Add(tableLayoutPanel8, 1, 0);
            tableLayoutPanel7.SetColumnSpan(flow_layout_language, 2);
            tableLayoutPanel7.Dock = DockStyle.Fill;
            tableLayoutPanel7.Location = new Point(3, 26);
            tableLayoutPanel7.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel7.Name = "tableLayoutPanel7";
            tableLayoutPanel7.RowCount = 1;
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel7.Size = new Size(416, 113);
            tableLayoutPanel7.TabIndex = 1;
            // 
            // flow_layout_language
            // 
            flow_layout_language.BackColor = Color.Beige;
            flow_layout_language.Dock = DockStyle.Fill;
            flow_layout_language.Location = new Point(3, 3);
            flow_layout_language.Name = "flow_layout_language";
            flow_layout_language.Size = new Size(243, 107);
            flow_layout_language.TabIndex = 0;
            // 
            // tableLayoutPanel8
            // 
            tableLayoutPanel8.ColumnCount = 2;
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            //tableLayoutPanel8.Controls.Add(lblSoftReset, 0, 0);
            //tableLayoutPanel8.Controls.Add(checkBoxSoftReset, 1, 0);
            tableLayoutPanel8.Dock = DockStyle.Fill;
            tableLayoutPanel8.Location = new Point(252, 3);
            tableLayoutPanel8.Name = "tableLayoutPanel8";
            tableLayoutPanel8.RowCount = 1;
            tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel8.Size = new Size(161, 107);
            tableLayoutPanel8.TabIndex = 1;
            // 
            // lblSoftReset
            // 
            lblSoftReset.AutoSize = true;
            lblSoftReset.Dock = DockStyle.Fill;
            //lblSoftReset.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblSoftReset.Location = new Point(3, 0);
            lblSoftReset.Name = "lblSoftReset";
            lblSoftReset.Size = new Size(74, 107);
            lblSoftReset.TabIndex = 20;
            lblSoftReset.Text = "Soft Reset";
            //lblSoftReset.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // checkBoxSoftReset
            // 
            checkBoxSoftReset.Dock = DockStyle.Fill;
            checkBoxSoftReset.Location = new Point(83, 4);
            checkBoxSoftReset.Margin = new Padding(3, 4, 3, 4);
            checkBoxSoftReset.Name = "checkBoxSoftReset";
            checkBoxSoftReset.Size = new Size(75, 99);
            checkBoxSoftReset.TabIndex = 21;
            checkBoxSoftReset.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.BackColor = Color.WhiteSmoke;
            groupBox2.Controls.Add(tableLayoutPanel5);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(3, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(422, 374);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Display Address";
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.BackColor = Color.Beige;
            tableLayoutPanel5.ColumnCount = 2;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52F));
            tableLayoutPanel5.Controls.Add(cmb_plateformNo, 1, 1);
            tableLayoutPanel5.Controls.Add(label1, 0, 4);
            tableLayoutPanel5.Controls.Add(txtLocation, 1, 2);
            tableLayoutPanel5.Controls.Add(label10, 0, 2);
            tableLayoutPanel5.Controls.Add(label6, 0, 1);
            tableLayoutPanel5.Controls.Add(label2, 0, 0);
            tableLayoutPanel5.Controls.Add(cmbNoCoach, 1, 0);
            //tableLayoutPanel5.Controls.Add(chkIsActive, 1, 5);   // IsActive hidden from the screen (value still loaded and saved)
            tableLayoutPanel5.Controls.Add(cmbPF_END, 1, 4);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(3, 26);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 6;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel5.Size = new Size(416, 345);
            tableLayoutPanel5.TabIndex = 0;
            // 
            // cmb_plateformNo
            // 
            cmb_plateformNo.Dock = DockStyle.Fill;
            cmb_plateformNo.Enabled = false;
            cmb_plateformNo.FormattingEnabled = true;
            cmb_plateformNo.Location = new Point(202, 61);
            cmb_plateformNo.Margin = new Padding(3, 4, 3, 4);
            cmb_plateformNo.Name = "cmb_plateformNo";
            cmb_plateformNo.Size = new Size(211, 31);
            cmb_plateformNo.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(3, 228);
            label1.Name = "label1";
            label1.Size = new Size(193, 57);
            label1.TabIndex = 20;
            label1.Text = "PF END";
            // 
            // txtLocation
            // 
            txtLocation.Dock = DockStyle.Fill;
            txtLocation.Location = new Point(202, 117);
            txtLocation.Name = "txtLocation";
            txtLocation.Size = new Size(211, 30);
            txtLocation.TabIndex = 3;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Dock = DockStyle.Fill;
            label10.Location = new Point(3, 114);
            label10.Name = "label10";
            label10.Size = new Size(193, 57);
            label10.TabIndex = 8;
            label10.Text = "Installed Location";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Dock = DockStyle.Fill;
            label6.Location = new Point(3, 57);
            label6.Name = "label6";
            label6.Size = new Size(193, 57);
            label6.TabIndex = 4;
            label6.Text = "PF Number";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Fill;
            label2.Location = new Point(3, 0);
            label2.Name = "label2";
            label2.Size = new Size(193, 57);
            label2.TabIndex = 15;
            label2.Text = "No of Coaches";
            // 
            // cmbNoCoach
            // 
            cmbNoCoach.Dock = DockStyle.Fill;
            cmbNoCoach.FormattingEnabled = true;
            cmbNoCoach.Location = new Point(202, 4);
            cmbNoCoach.Margin = new Padding(3, 4, 3, 4);
            cmbNoCoach.Name = "cmbNoCoach";
            cmbNoCoach.Size = new Size(211, 31);
            cmbNoCoach.TabIndex = 1;
            cmbNoCoach.SelectedIndexChanged += cmbNoCoach_SelectedIndexChanged;
            // 
            // chkIsActive
            // 
            chkIsActive.AutoSize = true;
            chkIsActive.Checked = true;
            chkIsActive.CheckState = CheckState.Checked;
            chkIsActive.Location = new Point(202, 289);
            chkIsActive.Margin = new Padding(3, 4, 3, 4);
            chkIsActive.Name = "chkIsActive";
            chkIsActive.Size = new Size(94, 27);
            chkIsActive.TabIndex = 6;
            chkIsActive.Text = "IsActive";
            chkIsActive.UseVisualStyleBackColor = true;
            // 
            // cmbPF_END
            // 
            cmbPF_END.Dock = DockStyle.Fill;
            cmbPF_END.FormattingEnabled = true;
            cmbPF_END.Items.AddRange(new object[] { "UP", "DOWN" });
            cmbPF_END.Location = new Point(202, 232);
            cmbPF_END.Margin = new Padding(3, 4, 3, 4);
            cmbPF_END.Name = "cmbPF_END";
            cmbPF_END.Size = new Size(211, 31);
            cmbPF_END.TabIndex = 5;
            // 
            // tableLayoutPanel6
            // 
            tableLayoutPanel6.ColumnCount = 5;
            tableLayoutPanel1.SetColumnSpan(tableLayoutPanel6, 2);
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel6.Controls.Add(btnSave, 2, 0);
            tableLayoutPanel6.Controls.Add(btndelete, 3, 0);
            tableLayoutPanel6.Controls.Add(btn_close, 4, 0);
            tableLayoutPanel6.Dock = DockStyle.Fill;
            tableLayoutPanel6.Location = new Point(3, 827);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 1;
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel6.Size = new Size(869, 54);
            tableLayoutPanel6.TabIndex = 5;
            // 
            // btnSave
            // 
            btnSave.Dock = DockStyle.Fill;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.ForeColor = Color.Maroon;
            btnSave.Location = new Point(349, 3);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(167, 48);
            btnSave.TabIndex = 15;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btndelete
            // 
            btndelete.Dock = DockStyle.Fill;
            btndelete.FlatStyle = FlatStyle.Flat;
            btndelete.ForeColor = Color.Maroon;
            btndelete.Location = new Point(522, 3);
            btndelete.Name = "btndelete";
            btndelete.Size = new Size(167, 48);
            btndelete.TabIndex = 16;
            btndelete.Text = "Delete";
            btndelete.UseVisualStyleBackColor = true;
            btndelete.Click += btndelete_Click;
            // 
            // btn_close
            // 
            btn_close.FlatStyle = FlatStyle.Flat;
            btn_close.ForeColor = Color.Maroon;
            btn_close.Location = new Point(695, 3);
            btn_close.Name = "btn_close";
            btn_close.Size = new Size(167, 41);
            btn_close.TabIndex = 17;
            btn_close.Text = "Close";
            btn_close.UseVisualStyleBackColor = true;
            btn_close.Click += btn_close_Click;
            // 
            // flow_layoutpanel
            // 
            flow_layoutpanel.Dock = DockStyle.Fill;
            flow_layoutpanel.Location = new Point(3, 583);
            flow_layoutpanel.Margin = new Padding(3, 4, 3, 4);
            flow_layoutpanel.Name = "flow_layoutpanel";
            flow_layoutpanel.Size = new Size(869, 237);
            flow_layoutpanel.TabIndex = 6;
            // 
            // ucCGDBBoardSetting
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel1);
            Margin = new Padding(3, 4, 3, 4);
            Name = "ucCGDBBoardSetting";
            Size = new Size(875, 884);
            Load += ucCGDBBoardSetting_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            tableLayoutPanel7.ResumeLayout(false);
            tableLayoutPanel8.ResumeLayout(false);
            tableLayoutPanel8.PerformLayout();
            groupBox2.ResumeLayout(false);
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel5.PerformLayout();
            tableLayoutPanel6.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private TableLayoutPanel tableLayoutPanel1;
        private Label lblboardHeader;
        private TableLayoutPanel tableLayoutPanel2;
        private GroupBox groupBox1;
        private TableLayoutPanel tableLayoutPanel4;
        private ComboBox cmbDisplayinfo;
        private ComboBox cmbEffect;
        //private ComboBox cmbLetterGap;
        private ComboBox cmbScrollSpeed;
        private ComboBox cmbDelayTime;
        private ComboBox cmbBoardSize;
        private ComboBox cmbFontSize;
        private Label label25;
        private Label label23;
        //private Label label21;
        private Label label19;
        private Label label15;
        private Label label13;
        private Label label11;
        private TableLayoutPanel tableLayoutPanel3;
        private GroupBox groupBox2;
        private TableLayoutPanel tableLayoutPanel5;
        private TextBox txtLocation;
        private Label label10;
        private Label label2;
        private ComboBox cmbNoCoach;
        private CheckBox chkIsActive;
        private GroupBox groupBox3;
        private FlowLayoutPanel flow_layout_language;
        private Label label1;
        private ComboBox cmbPF_END;
        private TableLayoutPanel tableLayoutPanel6;
        private Button btndelete;
        private Button btnSave;
        private FlowLayoutPanel flow_layoutpanel;
        private Button btn_close;
        private ComboBox cmb_plateformNo;
        private Label label6;
        private Label intensityLbl;
        private ComboBox cmbIntensity;
        private Label lblDto;
        private ComboBox cmbDTO;
        private Label lblSoftReset;
        private CheckBox checkBoxSoftReset;
        private TableLayoutPanel tableLayoutPanel7;
        private TableLayoutPanel tableLayoutPanel8;
    }
}