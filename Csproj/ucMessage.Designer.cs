namespace IPIS
{
    partial class ucMessage
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            tableLayoutPanelMsg = new TableLayoutPanel();
            tabControlMsg = new TabControl();
            tabPageAddMsg = new TabPage();
            tableLayoutPanel1 = new TableLayoutPanel();
            dgvMsg = new DataGridView();
            tableLayoutPanel2 = new TableLayoutPanel();
            //chkbxBottomMsg = new CheckBox();
            chkbxDisplayStatus = new CheckBox();
            tableLayoutPanel7 = new TableLayoutPanel();
            btnSaveMsg = new Button();
            btnClearMsg = new Button();
            cmbMsgType = new ComboBox();
            lblMsgCode = new Label();
            lblMsgEng = new Label();
            lblMsgHindi = new Label();
            lblMsgRegional = new Label();
            //lblMsgEngSnd = new Label();
            //lblMsgHindiSnd = new Label();
            //lblMsgRegionalSnd = new Label();
            txtbxMsgEng = new TextBox();
            txtbxMsgHindi = new TextBox();
            txtbxMsgReg = new TextBox();
            //txtbxEngSnd = new TextBox();
            //txtbxHindiSnd = new TextBox();
            //txtbxRegSnd = new TextBox();
            //btnBrowseEng = new Button();
            //btnBrowseHindi = new Button();
            //btnBrowseReg = new Button();
            tabPageColorStatus = new TabPage();
            tableLayoutPanel3 = new TableLayoutPanel();
            grpTrainStatusColour = new GroupBox();
            GV_Train_StatusColor = new DataGridView();
            grpIVDOVDStatusColour = new GroupBox();
            GV_IVD_OVDStatusColour = new DataGridView();
            tableLayoutPanelMsg.SuspendLayout();
            tabControlMsg.SuspendLayout();
            tabPageAddMsg.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMsg).BeginInit();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel7.SuspendLayout();
            tabPageColorStatus.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            grpTrainStatusColour.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GV_Train_StatusColor).BeginInit();
            grpIVDOVDStatusColour.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GV_IVD_OVDStatusColour).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanelMsg
            // 
            tableLayoutPanelMsg.BackColor = Color.Beige;
            tableLayoutPanelMsg.ColumnCount = 2;
            tableLayoutPanelMsg.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanelMsg.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanelMsg.Controls.Add(tabControlMsg, 0, 0);
            tableLayoutPanelMsg.Dock = DockStyle.Fill;
            tableLayoutPanelMsg.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tableLayoutPanelMsg.Location = new Point(0, 0);
            tableLayoutPanelMsg.Margin = new Padding(4, 5, 4, 5);
            tableLayoutPanelMsg.Name = "tableLayoutPanelMsg";
            tableLayoutPanelMsg.RowCount = 1;
            tableLayoutPanelMsg.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanelMsg.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanelMsg.Size = new Size(1026, 900);
            tableLayoutPanelMsg.TabIndex = 0;
            // 
            // tabControlMsg
            // 
            tabControlMsg.Controls.Add(tabPageAddMsg);
            tabControlMsg.Controls.Add(tabPageColorStatus);
            tabControlMsg.SelectedIndexChanged += tabControlMsg_SelectedIndexChanged;
            tabControlMsg.Dock = DockStyle.Fill;
            tabControlMsg.Location = new Point(4, 5);
            tabControlMsg.Margin = new Padding(4, 5, 4, 5);
            tabControlMsg.Name = "tabControlMsg";
            tabControlMsg.SelectedIndex = 0;
            tabControlMsg.Size = new Size(998, 890);
            tabControlMsg.TabIndex = 2;
            // 
            // tabPageAddMsg
            // 
            tabPageAddMsg.Controls.Add(tableLayoutPanel1);
            tabPageAddMsg.Location = new Point(4, 37);
            tabPageAddMsg.Margin = new Padding(4, 5, 4, 5);
            tabPageAddMsg.Name = "tabPageAddMsg";
            tabPageAddMsg.Padding = new Padding(4, 5, 4, 5);
            tabPageAddMsg.Size = new Size(990, 849);
            tabPageAddMsg.TabIndex = 0;
            tabPageAddMsg.Text = "ADD MSG";
            tabPageAddMsg.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.Beige;
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.9639683F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 29.799427F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9.455587F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 47.99427F));
            tableLayoutPanel1.Controls.Add(dgvMsg, 0, 9);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 1, 7);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel7, 1, 8);
            tableLayoutPanel1.Controls.Add(cmbMsgType, 1, 0);
            tableLayoutPanel1.Controls.Add(lblMsgCode, 0, 0);
            tableLayoutPanel1.Controls.Add(lblMsgEng, 0, 1);
            tableLayoutPanel1.Controls.Add(lblMsgHindi, 0, 2);
            tableLayoutPanel1.Controls.Add(lblMsgRegional, 0, 3);
            //tableLayoutPanel1.Controls.Add(lblMsgEngSnd, 0, 4);
            //tableLayoutPanel1.Controls.Add(lblMsgHindiSnd, 0, 5);
            //tableLayoutPanel1.Controls.Add(lblMsgRegionalSnd, 0, 6);
            tableLayoutPanel1.Controls.Add(txtbxMsgEng, 1, 1);
            tableLayoutPanel1.Controls.Add(txtbxMsgHindi, 1, 2);
            tableLayoutPanel1.Controls.Add(txtbxMsgReg, 1, 3);
            //tableLayoutPanel1.Controls.Add(txtbxEngSnd, 1, 4);
            //tableLayoutPanel1.Controls.Add(txtbxHindiSnd, 1, 5);
            //tableLayoutPanel1.Controls.Add(txtbxRegSnd, 1, 6);
            //tableLayoutPanel1.Controls.Add(btnBrowseEng, 2, 4);
            //tableLayoutPanel1.Controls.Add(btnBrowseHindi, 2, 5);
            //tableLayoutPanel1.Controls.Add(btnBrowseReg, 2, 6);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.ForeColor = Color.DarkBlue;
            tableLayoutPanel1.Location = new Point(4, 5);
            tableLayoutPanel1.Margin = new Padding(4, 3, 4, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 10;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6.44121456F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6.3025794F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6.3025794F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6.3025794F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6.3025794F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6.3025794F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6.3025794F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6.96791363F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 7.81251F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 40.9628868F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
            tableLayoutPanel1.Size = new Size(982, 839);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // dgvMsg
            // 
            dgvMsg.AllowUserToAddRows = false;
            dgvMsg.AllowUserToDeleteRows = false;
            dgvMsg.AllowUserToResizeColumns = false;
            dgvMsg.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.WhiteSmoke;
            dgvMsg.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvMsg.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvMsg.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvMsg.BackgroundColor = Color.WhiteSmoke;
            dgvMsg.BorderStyle = BorderStyle.None;
            dgvMsg.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvMsg.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle2.BackColor = Color.LightSteelBlue;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.Maroon;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvMsg.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvMsg.ColumnHeadersHeight = 29;
            dgvMsg.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            tableLayoutPanel1.SetColumnSpan(dgvMsg, 4);
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle3.BackColor = SystemColors.Window;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = Color.DarkBlue;
            dataGridViewCellStyle3.SelectionBackColor = Color.White;
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvMsg.DefaultCellStyle = dataGridViewCellStyle3;
            dgvMsg.Dock = DockStyle.Fill;
            dgvMsg.EnableHeadersVisualStyles = false;
            dgvMsg.GridColor = Color.Brown;
            dgvMsg.Location = new Point(4, 494);
            dgvMsg.Margin = new Padding(4, 5, 4, 5);
            dgvMsg.Name = "dgvMsg";
            dgvMsg.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle4.BackColor = SystemColors.Control;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle4.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.ActiveCaption;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            dgvMsg.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dgvMsg.RowHeadersVisible = false;
            dgvMsg.RowHeadersWidth = 51;
            dgvMsg.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgvMsg.RowTemplate.Height = 35;
            dgvMsg.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMsg.Size = new Size(974, 340);
            dgvMsg.TabIndex = 38;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            //tableLayoutPanel2.Controls.Add(chkbxBottomMsg, 1, 0);
            tableLayoutPanel2.Controls.Add(chkbxDisplayStatus, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(131, 369);
            tableLayoutPanel2.Margin = new Padding(4, 3, 4, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(284, 52);
            tableLayoutPanel2.TabIndex = 8;
            // 
            // chkbxBottomMsg
            // 
            //chkbxBottomMsg.AutoSize = true;
            //chkbxBottomMsg.Dock = DockStyle.Fill;
            //chkbxBottomMsg.Location = new Point(146, 3);
            //chkbxBottomMsg.Margin = new Padding(4, 3, 4, 3);
            //chkbxBottomMsg.Name = "chkbxBottomMsg";
            //chkbxBottomMsg.Size = new Size(134, 46);
            //chkbxBottomMsg.TabIndex = 1;
            //chkbxBottomMsg.Text = "Bottom Message";
            //chkbxBottomMsg.UseVisualStyleBackColor = true;
            // 
            // chkbxDisplayStatus
            // 
            chkbxDisplayStatus.AutoSize = true;
            chkbxDisplayStatus.Dock = DockStyle.Fill;
            chkbxDisplayStatus.Location = new Point(4, 3);
            chkbxDisplayStatus.Margin = new Padding(4, 3, 4, 3);
            chkbxDisplayStatus.Name = "chkbxDisplayStatus";
            chkbxDisplayStatus.Size = new Size(134, 46);
            chkbxDisplayStatus.TabIndex = 0;
            chkbxDisplayStatus.Text = "Active";
            chkbxDisplayStatus.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel7
            // 
            tableLayoutPanel7.ColumnCount = 2;
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel7.Controls.Add(btnSaveMsg, 0, 0);
            tableLayoutPanel7.Controls.Add(btnClearMsg, 1, 0);
            tableLayoutPanel7.Dock = DockStyle.Fill;
            tableLayoutPanel7.Location = new Point(131, 427);
            tableLayoutPanel7.Margin = new Padding(4, 3, 4, 3);
            tableLayoutPanel7.Name = "tableLayoutPanel7";
            tableLayoutPanel7.RowCount = 1;
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Absolute, 57F));
            tableLayoutPanel7.Size = new Size(284, 59);
            tableLayoutPanel7.TabIndex = 39;
            // 
            // btnSaveMsg
            // 
            btnSaveMsg.BackColor = Color.WhiteSmoke;
            btnSaveMsg.Dock = DockStyle.Fill;
            btnSaveMsg.FlatStyle = FlatStyle.Flat;
            btnSaveMsg.Location = new Point(4, 3);
            btnSaveMsg.Margin = new Padding(4, 3, 4, 3);
            btnSaveMsg.Name = "btnSaveMsg";
            btnSaveMsg.Size = new Size(134, 53);
            btnSaveMsg.TabIndex = 0;
            btnSaveMsg.Text = "Save";
            btnSaveMsg.UseVisualStyleBackColor = false;
            btnSaveMsg.Click += btnSaveMsg_Click;
            // 
            // btnClearMsg
            // 
            btnClearMsg.BackColor = Color.WhiteSmoke;
            btnClearMsg.Dock = DockStyle.Fill;
            btnClearMsg.FlatStyle = FlatStyle.Flat;
            btnClearMsg.Location = new Point(146, 3);
            btnClearMsg.Margin = new Padding(4, 3, 4, 3);
            btnClearMsg.Name = "btnClearMsg";
            btnClearMsg.Size = new Size(134, 53);
            btnClearMsg.TabIndex = 1;
            btnClearMsg.Text = "Clear";
            btnClearMsg.UseVisualStyleBackColor = false;
            btnClearMsg.Click += btnClearMsg_Click;
            // 
            // cmbMsgType
            // 
            cmbMsgType.Dock = DockStyle.Fill;
            cmbMsgType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMsgType.FormattingEnabled = true;
            cmbMsgType.ImeMode = ImeMode.NoControl;
            cmbMsgType.Location = new Point(131, 3);
            cmbMsgType.Margin = new Padding(4, 3, 4, 3);
            cmbMsgType.Name = "cmbMsgType";
            cmbMsgType.Size = new Size(284, 36);
            cmbMsgType.TabIndex = 20;
            cmbMsgType.SelectedIndexChanged += cmbMsgType_SelectedIndexChanged;
            // 
            // lblMsgCode
            // 
            lblMsgCode.AutoSize = true;
            lblMsgCode.Dock = DockStyle.Fill;
            lblMsgCode.Location = new Point(4, 0);
            lblMsgCode.Margin = new Padding(4, 0, 4, 0);
            lblMsgCode.Name = "lblMsgCode";
            lblMsgCode.Size = new Size(119, 54);
            lblMsgCode.TabIndex = 1;
            lblMsgCode.Text = "Message Code *";
            // 
            // lblMsgEng
            // 
            lblMsgEng.AutoSize = true;
            lblMsgEng.Dock = DockStyle.Fill;
            lblMsgEng.Location = new Point(4, 54);
            lblMsgEng.Margin = new Padding(4, 0, 4, 0);
            lblMsgEng.Name = "lblMsgEng";
            lblMsgEng.Size = new Size(119, 52);
            lblMsgEng.TabIndex = 2;
            lblMsgEng.Text = "Message English";
            // 
            // lblMsgHindi
            // 
            lblMsgHindi.AutoSize = true;
            lblMsgHindi.Dock = DockStyle.Fill;
            lblMsgHindi.Location = new Point(4, 106);
            lblMsgHindi.Margin = new Padding(4, 0, 4, 0);
            lblMsgHindi.Name = "lblMsgHindi";
            lblMsgHindi.Size = new Size(119, 52);
            lblMsgHindi.TabIndex = 3;
            lblMsgHindi.Text = "Message Hindi";
            // 
            // lblMsgRegional
            // 
            lblMsgRegional.AutoSize = true;
            lblMsgRegional.Dock = DockStyle.Fill;
            lblMsgRegional.Location = new Point(4, 158);
            lblMsgRegional.Margin = new Padding(4, 0, 4, 0);
            lblMsgRegional.Name = "lblMsgRegional";
            lblMsgRegional.Size = new Size(119, 52);
            lblMsgRegional.TabIndex = 4;
            lblMsgRegional.Text = "Message Regional";
            // 
            // lblMsgEngSnd
            // 
            //lblMsgEngSnd.AutoSize = true;
            //lblMsgEngSnd.Dock = DockStyle.Fill;
            //lblMsgEngSnd.Location = new Point(4, 210);
            //lblMsgEngSnd.Margin = new Padding(4, 0, 4, 0);
            //lblMsgEngSnd.Name = "lblMsgEngSnd";
            //lblMsgEngSnd.Size = new Size(119, 52);
            //lblMsgEngSnd.TabIndex = 5;
            //lblMsgEngSnd.Text = "English Sound";
            // 
            // lblMsgHindiSnd
            // 
            //lblMsgHindiSnd.AutoSize = true;
            //lblMsgHindiSnd.Dock = DockStyle.Fill;
            //lblMsgHindiSnd.Location = new Point(4, 262);
            //lblMsgHindiSnd.Margin = new Padding(4, 0, 4, 0);
            //lblMsgHindiSnd.Name = "lblMsgHindiSnd";
            //lblMsgHindiSnd.Size = new Size(119, 52);
            //lblMsgHindiSnd.TabIndex = 6;
            //lblMsgHindiSnd.Text = "Hindi Sound";
            // 
            // lblMsgRegionalSnd
            // 
            //lblMsgRegionalSnd.AutoSize = true;
            //lblMsgRegionalSnd.Dock = DockStyle.Fill;
            //lblMsgRegionalSnd.Location = new Point(4, 314);
            //lblMsgRegionalSnd.Margin = new Padding(4, 0, 4, 0);
            //lblMsgRegionalSnd.Name = "lblMsgRegionalSnd";
            //lblMsgRegionalSnd.Size = new Size(119, 52);
            //lblMsgRegionalSnd.TabIndex = 7;
            //lblMsgRegionalSnd.Text = "Regional Sound";
            // 
            // txtbxMsgEng
            // 
            txtbxMsgEng.Dock = DockStyle.Fill;
            txtbxMsgEng.Location = new Point(131, 57);
            txtbxMsgEng.Margin = new Padding(4, 3, 4, 3);
            txtbxMsgEng.Name = "txtbxMsgEng";
            txtbxMsgEng.PlaceholderText = "Please Enter English Message";
            txtbxMsgEng.Size = new Size(284, 35);
            txtbxMsgEng.TabIndex = 10;
            // 
            // txtbxMsgHindi
            // 
            txtbxMsgHindi.Dock = DockStyle.Fill;
            txtbxMsgHindi.Location = new Point(131, 109);
            txtbxMsgHindi.Margin = new Padding(4, 3, 4, 3);
            txtbxMsgHindi.Name = "txtbxMsgHindi";
            txtbxMsgHindi.PlaceholderText = "Please Enter Hindi Message";
            txtbxMsgHindi.Size = new Size(284, 35);
            txtbxMsgHindi.TabIndex = 11;
            // 
            // txtbxMsgReg
            // 
            txtbxMsgReg.Dock = DockStyle.Fill;
            txtbxMsgReg.Location = new Point(131, 161);
            txtbxMsgReg.Margin = new Padding(4, 3, 4, 3);
            txtbxMsgReg.Name = "txtbxMsgReg";
            txtbxMsgReg.PlaceholderText = "Please Enter Regional Message";
            txtbxMsgReg.Size = new Size(284, 35);
            txtbxMsgReg.TabIndex = 12;
            // 
            // txtbxEngSnd
            // 
            //txtbxEngSnd.Dock = DockStyle.Fill;
            //txtbxEngSnd.Location = new Point(131, 213);
            //txtbxEngSnd.Margin = new Padding(4, 3, 4, 3);
            //txtbxEngSnd.Name = "txtbxEngSnd";
            //txtbxEngSnd.PlaceholderText = "Please Add English Sound File";
            //txtbxEngSnd.Size = new Size(284, 35);
            //txtbxEngSnd.TabIndex = 13;
            // 
            // txtbxHindiSnd
            // 
            //txtbxHindiSnd.Dock = DockStyle.Fill;
            //txtbxHindiSnd.Location = new Point(131, 265);
            //txtbxHindiSnd.Margin = new Padding(4, 3, 4, 3);
            //txtbxHindiSnd.Name = "txtbxHindiSnd";
            //txtbxHindiSnd.PlaceholderText = "Please Add Hindi Sound File";
            //txtbxHindiSnd.Size = new Size(284, 35);
            //txtbxHindiSnd.TabIndex = 14;
            // 
            // txtbxRegSnd
            // 
            //txtbxRegSnd.Dock = DockStyle.Fill;
            //txtbxRegSnd.Location = new Point(131, 317);
            //txtbxRegSnd.Margin = new Padding(4, 3, 4, 3);
            //txtbxRegSnd.Name = "txtbxRegSnd";
            //txtbxRegSnd.PlaceholderText = "Please Add Regional File";
            //txtbxRegSnd.Size = new Size(284, 35);
            //txtbxRegSnd.TabIndex = 15;
            // 
            // btnBrowseEng
            // 
            //btnBrowseEng.Dock = DockStyle.Fill;
            //btnBrowseEng.Location = new Point(423, 213);
            //btnBrowseEng.Margin = new Padding(4, 3, 4, 3);
            //btnBrowseEng.Name = "btnBrowseEng";
            //btnBrowseEng.Size = new Size(84, 46);
            //btnBrowseEng.TabIndex = 16;
            //btnBrowseEng.Text = "Browse";
            //btnBrowseEng.UseVisualStyleBackColor = true;
            //btnBrowseEng.Click += btnBrowseEng_Click;
            // 
            // btnBrowseHindi
            // 
            //btnBrowseHindi.Dock = DockStyle.Fill;
            //btnBrowseHindi.Location = new Point(423, 265);
            //btnBrowseHindi.Margin = new Padding(4, 3, 4, 3);
            //btnBrowseHindi.Name = "btnBrowseHindi";
            //btnBrowseHindi.Size = new Size(84, 46);
            //btnBrowseHindi.TabIndex = 17;
            //btnBrowseHindi.Text = "Browse";
            //btnBrowseHindi.UseVisualStyleBackColor = true;
            //btnBrowseHindi.Click += btnBrowseHindi_Click;
            // 
            // btnBrowseReg
            // 
            //btnBrowseReg.Dock = DockStyle.Fill;
            //btnBrowseReg.Location = new Point(423, 317);
            //btnBrowseReg.Margin = new Padding(4, 3, 4, 3);
            //btnBrowseReg.Name = "btnBrowseReg";
            //btnBrowseReg.Size = new Size(84, 46);
            //btnBrowseReg.TabIndex = 18;
            //btnBrowseReg.Text = "Browse";
            //btnBrowseReg.UseVisualStyleBackColor = true;
            //btnBrowseReg.Click += btnBrowseReg_Click;
            // 
            // tabPageColorStatus
            // 
            tabPageColorStatus.Controls.Add(tableLayoutPanel3);
            tabPageColorStatus.Location = new Point(4, 37);
            tabPageColorStatus.Margin = new Padding(4, 5, 4, 5);
            tabPageColorStatus.Name = "tabPageColorStatus";
            tabPageColorStatus.Size = new Size(990, 849);
            tabPageColorStatus.TabIndex = 3;
            tabPageColorStatus.Text = " COLOUR STATUS";
            tabPageColorStatus.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Controls.Add(grpTrainStatusColour, 0, 0);
            tableLayoutPanel3.Controls.Add(grpIVDOVDStatusColour, 1, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(0, 0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new Size(990, 849);
            tableLayoutPanel3.TabIndex = 0;
            // 
            // grpTrainStatusColour
            // 
            grpTrainStatusColour.Controls.Add(GV_Train_StatusColor);
            grpTrainStatusColour.Dock = DockStyle.Fill;
            grpTrainStatusColour.Location = new Point(3, 3);
            grpTrainStatusColour.Name = "grpTrainStatusColour";
            grpTrainStatusColour.Size = new Size(489, 843);
            grpTrainStatusColour.TabIndex = 7;
            grpTrainStatusColour.TabStop = false;
            grpTrainStatusColour.Text = "Train Status Colour";
            // 
            // GV_Train_StatusColor
            // 
            GV_Train_StatusColor.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            GV_Train_StatusColor.Dock = DockStyle.Fill;
            GV_Train_StatusColor.Location = new Point(3, 31);
            GV_Train_StatusColor.Name = "GV_Train_StatusColor";
            GV_Train_StatusColor.RowHeadersWidth = 62;
            GV_Train_StatusColor.Size = new Size(483, 809);
            GV_Train_StatusColor.TabIndex = 0;
            GV_Train_StatusColor.CellClick += GV_Train_StatusColor_CellClick;
            // 
            // grpIVDOVDStatusColour
            // 
            grpIVDOVDStatusColour.Controls.Add(GV_IVD_OVDStatusColour);
            grpIVDOVDStatusColour.Dock = DockStyle.Fill;
            grpIVDOVDStatusColour.Location = new Point(498, 3);
            grpIVDOVDStatusColour.Name = "grpIVDOVDStatusColour";
            grpIVDOVDStatusColour.Size = new Size(489, 843);
            grpIVDOVDStatusColour.TabIndex = 5;
            grpIVDOVDStatusColour.TabStop = false;
            grpIVDOVDStatusColour.Text = "VDB/AVDB Color Setting";
            // 
            // GV_IVD_OVDStatusColour
            // 
            GV_IVD_OVDStatusColour.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            GV_IVD_OVDStatusColour.Dock = DockStyle.Fill;
            GV_IVD_OVDStatusColour.Location = new Point(3, 31);
            GV_IVD_OVDStatusColour.Name = "GV_IVD_OVDStatusColour";
            GV_IVD_OVDStatusColour.RowHeadersWidth = 62;
            GV_IVD_OVDStatusColour.Size = new Size(483, 809);
            GV_IVD_OVDStatusColour.TabIndex = 0;
            GV_IVD_OVDStatusColour.CellClick += GV_IVD_OVDStatusColour_CellClick;
            // 
            // ucMessage
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tableLayoutPanelMsg);
            Margin = new Padding(4, 5, 4, 5);
            Name = "ucMessage";
            Size = new Size(1026, 900);
            Load += ucMessage_Load;
            tableLayoutPanelMsg.ResumeLayout(false);
            tabControlMsg.ResumeLayout(false);
            tabPageAddMsg.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMsg).EndInit();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            tableLayoutPanel7.ResumeLayout(false);
            tabPageColorStatus.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            grpTrainStatusColour.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)GV_Train_StatusColor).EndInit();
            grpIVDOVDStatusColour.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)GV_IVD_OVDStatusColour).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMsg;
        private System.Windows.Forms.TabControl tabControlMsg;
        private System.Windows.Forms.TabPage tabPageAddMsg;
        private System.Windows.Forms.TabPage tabPageColorStatus;
        private TableLayoutPanel tableLayoutPanel1;
        private Label label1;
        private Label lblMsgCode;
        private Label lblMsgEng;
        private Label lblMsgHindi;
        private Label lblMsgRegional;
        //private Label lblMsgEngSnd;
        //private Label lblMsgHindiSnd;
        //private Label lblMsgRegionalSnd;
        private TextBox txtbxMsgEng;
        private TextBox txtbxMsgHindi;
        private TextBox txtbxMsgReg;
        //private TextBox txtbxEngSnd;
        //private TextBox txtbxHindiSnd;
        //private TextBox txtbxRegSnd;
        //private Button btnBrowseEng;
        //private Button btnBrowseHindi;
        //private Button btnBrowseReg;
        private ComboBox cmbMsgType;
        private DataGridView dgvMsg;
        private TableLayoutPanel tableLayoutPanel7;
        private Button btnSaveMsg;
        private Button btnClearMsg;
        private TextBox txtbxMsgType;
        private TableLayoutPanel tableLayoutPanel2;
        //private CheckBox chkbxBottomMsg;
        private CheckBox chkbxDisplayStatus;
        private TableLayoutPanel tableLayoutPanel3;
        private GroupBox grpTrainStatusColour;
        private GroupBox grpIVDOVDStatusColour;
        private DataGridView GV_Train_StatusColor;
        private DataGridView GV_IVD_OVDStatusColour;
    }
}