namespace IPIS
{
    partial class ucUserControl
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
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            contextMenuStrip1 = new ContextMenuStrip(components);
            panelMain = new Panel();
            tabUserControl = new TabControl();
            tabUserCreation = new TabPage();
            tableLayoutPanel3 = new TableLayoutPanel();
            groupBox4 = new GroupBox();
            dgvUsers = new DataGridView();
            tableLayoutPanel6 = new TableLayoutPanel();
            gboxUser = new GroupBox();
            tableLayoutPanel4 = new TableLayoutPanel();
            cmbAccessType = new ComboBox();
            label4 = new Label();
            txtRePassword = new TextBox();
            lblReEnter = new Label();
            txtPassword = new TextBox();
            label2 = new Label();
            txtUserName = new TextBox();
            lblUserName = new Label();
            chkActive = new CheckBox();
            tableLayoutPanel10 = new TableLayoutPanel();
            btnClearUser = new Button();
            btnSaveUser = new Button();
            tabModifyUser = new TabPage();
            tableLayoutPanel1 = new TableLayoutPanel();
            dgvAccessRights = new DataGridView();
            groupBox1 = new GroupBox();
            tableLayoutPanel9 = new TableLayoutPanel();
            chkPlayAnnc = new CheckBox();
            chkVDC = new CheckBox();
            chkTADBCGDBSend = new CheckBox();
            chkUserSettings = new CheckBox();
            chkGeneralMessage = new CheckBox();
            chkBoardSetting = new CheckBox();
            chkAdminSetting = new CheckBox();
            chkTrainEntry = new CheckBox();
            groupBox2 = new GroupBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            txtUserType = new TextBox();
            label5 = new Label();
            chkActiveAccess = new CheckBox();
            tableLayoutPanel5 = new TableLayoutPanel();
            btnClearAccessRight = new Button();
            btnSaveAccessRight = new Button();
            panelMain.SuspendLayout();
            tabUserControl.SuspendLayout();
            tabUserCreation.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsers).BeginInit();
            tableLayoutPanel6.SuspendLayout();
            gboxUser.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            tableLayoutPanel10.SuspendLayout();
            tabModifyUser.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAccessRights).BeginInit();
            groupBox1.SuspendLayout();
            tableLayoutPanel9.SuspendLayout();
            groupBox2.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(20, 20);
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(61, 4);
            // 
            // panelMain
            // 
            panelMain.Controls.Add(tabUserControl);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(0, 0);
            panelMain.Margin = new Padding(3, 2, 3, 2);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(804, 568);
            panelMain.TabIndex = 1;
            // 
            // tabUserControl
            // 
            tabUserControl.Controls.Add(tabUserCreation);
            tabUserControl.Controls.Add(tabModifyUser);
            tabUserControl.Cursor = Cursors.Hand;
            tabUserControl.Dock = DockStyle.Fill;
            tabUserControl.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tabUserControl.HotTrack = true;
            tabUserControl.ItemSize = new Size(150, 28);
            tabUserControl.Location = new Point(0, 0);
            tabUserControl.Name = "tabUserControl";
            tabUserControl.SelectedIndex = 0;
            tabUserControl.Size = new Size(804, 568);
            tabUserControl.TabIndex = 3;
            tabUserControl.SelectedIndexChanged += tabUserControl_SelectedIndexChanged;
            // 
            // tabUserCreation
            // 
            tabUserCreation.BackColor = Color.LightBlue;
            tabUserCreation.BorderStyle = BorderStyle.Fixed3D;
            tabUserCreation.Controls.Add(tableLayoutPanel3);
            tabUserCreation.Location = new Point(4, 32);
            tabUserCreation.Name = "tabUserCreation";
            tabUserCreation.Padding = new Padding(9);
            tabUserCreation.Size = new Size(796, 532);
            tabUserCreation.TabIndex = 0;
            tabUserCreation.Text = "Add New User";
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.BackColor = Color.Beige;
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Controls.Add(groupBox4, 0, 1);
            tableLayoutPanel3.Controls.Add(tableLayoutPanel6, 0, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(9, 9);
            tableLayoutPanel3.Margin = new Padding(3, 2, 3, 2);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 2;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
            tableLayoutPanel3.Size = new Size(774, 510);
            tableLayoutPanel3.TabIndex = 1;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(dgvUsers);
            groupBox4.Dock = DockStyle.Fill;
            groupBox4.Location = new Point(3, 206);
            groupBox4.Margin = new Padding(3, 2, 3, 2);
            groupBox4.Name = "groupBox4";
            groupBox4.Padding = new Padding(3, 2, 3, 2);
            groupBox4.Size = new Size(768, 302);
            groupBox4.TabIndex = 3;
            groupBox4.TabStop = false;
            groupBox4.Text = "User Details";
            // 
            // dgvUsers
            // 
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.AllowUserToDeleteRows = false;
            dgvUsers.AllowUserToResizeColumns = false;
            dgvUsers.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.WhiteSmoke;
            dgvUsers.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsers.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvUsers.BackgroundColor = Color.WhiteSmoke;
            dgvUsers.BorderStyle = BorderStyle.Fixed3D;
            dgvUsers.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvUsers.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle2.BackColor = Color.LightSteelBlue;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.Maroon;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvUsers.ColumnHeadersHeight = 29;
            dgvUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle3.BackColor = SystemColors.Window;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = Color.White;
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvUsers.DefaultCellStyle = dataGridViewCellStyle3;
            dgvUsers.Dock = DockStyle.Fill;
            dgvUsers.EnableHeadersVisualStyles = false;
            dgvUsers.GridColor = Color.Brown;
            dgvUsers.Location = new Point(3, 21);
            dgvUsers.Name = "dgvUsers";
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle4.BackColor = SystemColors.Control;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle4.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.ActiveCaption;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            dgvUsers.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dgvUsers.RowHeadersVisible = false;
            dgvUsers.RowHeadersWidth = 51;
            dgvUsers.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgvUsers.RowTemplate.Height = 35;
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsers.Size = new Size(762, 279);
            dgvUsers.TabIndex = 22;
            dgvUsers.CellFormatting += dgvUsers_CellFormatting;
            // 
            // tableLayoutPanel6
            // 
            tableLayoutPanel6.ColumnCount = 2;
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel6.Controls.Add(gboxUser, 0, 0);
            tableLayoutPanel6.Dock = DockStyle.Fill;
            tableLayoutPanel6.Location = new Point(3, 3);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 1;
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Absolute, 204F));
            tableLayoutPanel6.Size = new Size(768, 198);
            tableLayoutPanel6.TabIndex = 4;
            // 
            // gboxUser
            // 
            gboxUser.Controls.Add(tableLayoutPanel4);
            gboxUser.Dock = DockStyle.Fill;
            gboxUser.Location = new Point(3, 2);
            gboxUser.Margin = new Padding(3, 2, 3, 2);
            gboxUser.Name = "gboxUser";
            gboxUser.Padding = new Padding(3, 2, 3, 2);
            gboxUser.Size = new Size(378, 194);
            gboxUser.TabIndex = 2;
            gboxUser.TabStop = false;
            gboxUser.Text = "Create and Modify User";
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.BackColor = Color.Beige;
            tableLayoutPanel4.ColumnCount = 2;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Controls.Add(cmbAccessType, 1, 3);
            tableLayoutPanel4.Controls.Add(label4, 0, 3);
            tableLayoutPanel4.Controls.Add(txtRePassword, 1, 2);
            tableLayoutPanel4.Controls.Add(lblReEnter, 0, 2);
            tableLayoutPanel4.Controls.Add(txtPassword, 1, 1);
            tableLayoutPanel4.Controls.Add(label2, 0, 1);
            tableLayoutPanel4.Controls.Add(txtUserName, 1, 0);
            tableLayoutPanel4.Controls.Add(lblUserName, 0, 0);
            tableLayoutPanel4.Controls.Add(chkActive, 1, 4);
            tableLayoutPanel4.Controls.Add(tableLayoutPanel10, 1, 5);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(3, 21);
            tableLayoutPanel4.Margin = new Padding(3, 2, 3, 2);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 6;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 14.0350876F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 20.4678364F));
            tableLayoutPanel4.Size = new Size(372, 171);
            tableLayoutPanel4.TabIndex = 0;
            // 
            // cmbAccessType
            // 
            cmbAccessType.Dock = DockStyle.Fill;
            cmbAccessType.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cmbAccessType.FormattingEnabled = true;
            cmbAccessType.Items.AddRange(new object[] { "ADMIN", "SUPERVISOR", "OPERATOR" });
            cmbAccessType.Location = new Point(189, 87);
            cmbAccessType.Name = "cmbAccessType";
            cmbAccessType.Size = new Size(180, 27);
            cmbAccessType.TabIndex = 14;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = DockStyle.Fill;
            label4.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(3, 84);
            label4.Name = "label4";
            label4.Size = new Size(180, 28);
            label4.TabIndex = 13;
            label4.Text = "Access Type :";
            label4.TextAlign = ContentAlignment.TopRight;
            // 
            // txtRePassword
            // 
            txtRePassword.Dock = DockStyle.Fill;
            txtRePassword.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtRePassword.Location = new Point(189, 59);
            txtRePassword.Name = "txtRePassword";
            txtRePassword.PasswordChar = '*';
            txtRePassword.Size = new Size(180, 26);
            txtRePassword.TabIndex = 12;
            // 
            // lblReEnter
            // 
            lblReEnter.AutoSize = true;
            lblReEnter.Dock = DockStyle.Fill;
            lblReEnter.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblReEnter.Location = new Point(3, 56);
            lblReEnter.Name = "lblReEnter";
            lblReEnter.Size = new Size(180, 28);
            lblReEnter.TabIndex = 6;
            lblReEnter.Text = "Re-Enter Password :";
            lblReEnter.TextAlign = ContentAlignment.TopRight;
            // 
            // txtPassword
            // 
            txtPassword.Dock = DockStyle.Fill;
            txtPassword.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtPassword.Location = new Point(189, 31);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(180, 26);
            txtPassword.TabIndex = 5;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Fill;
            label2.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ImageAlign = ContentAlignment.MiddleRight;
            label2.Location = new Point(3, 28);
            label2.Name = "label2";
            label2.Size = new Size(180, 28);
            label2.TabIndex = 4;
            label2.Text = "Enter Password :";
            label2.TextAlign = ContentAlignment.TopRight;
            // 
            // txtUserName
            // 
            txtUserName.Dock = DockStyle.Fill;
            txtUserName.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtUserName.Location = new Point(189, 3);
            txtUserName.Name = "txtUserName";
            txtUserName.Size = new Size(180, 26);
            txtUserName.TabIndex = 3;
            txtUserName.TextChanged += txtUserName_TextChanged;
            // 
            // lblUserName
            // 
            lblUserName.AutoSize = true;
            lblUserName.Dock = DockStyle.Fill;
            lblUserName.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblUserName.Location = new Point(3, 0);
            lblUserName.Name = "lblUserName";
            lblUserName.Size = new Size(180, 28);
            lblUserName.TabIndex = 2;
            lblUserName.Text = "User Name :";
            lblUserName.TextAlign = ContentAlignment.TopRight;
            // 
            // chkActive
            // 
            chkActive.AutoSize = true;
            chkActive.Dock = DockStyle.Fill;
            chkActive.Location = new Point(189, 114);
            chkActive.Margin = new Padding(3, 2, 3, 2);
            chkActive.Name = "chkActive";
            chkActive.Size = new Size(180, 19);
            chkActive.TabIndex = 15;
            chkActive.Text = "Active";
            chkActive.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel10
            // 
            tableLayoutPanel10.ColumnCount = 2;
            tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel10.Controls.Add(btnClearUser, 1, 0);
            tableLayoutPanel10.Controls.Add(btnSaveUser, 0, 0);
            tableLayoutPanel10.Dock = DockStyle.Fill;
            tableLayoutPanel10.Location = new Point(189, 137);
            tableLayoutPanel10.Margin = new Padding(3, 2, 3, 2);
            tableLayoutPanel10.Name = "tableLayoutPanel10";
            tableLayoutPanel10.RowCount = 1;
            tableLayoutPanel10.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel10.RowStyles.Add(new RowStyle(SizeType.Absolute, 37F));
            tableLayoutPanel10.Size = new Size(180, 32);
            tableLayoutPanel10.TabIndex = 16;
            // 
            // btnClearUser
            // 
            btnClearUser.AutoSize = true;
            btnClearUser.BackColor = Color.WhiteSmoke;
            btnClearUser.Dock = DockStyle.Fill;
            btnClearUser.FlatStyle = FlatStyle.Flat;
            btnClearUser.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            btnClearUser.ForeColor = Color.Black;
            btnClearUser.Location = new Point(93, 2);
            btnClearUser.Margin = new Padding(3, 2, 3, 2);
            btnClearUser.Name = "btnClearUser";
            btnClearUser.Size = new Size(84, 28);
            btnClearUser.TabIndex = 21;
            btnClearUser.Text = "Reset";
            btnClearUser.UseVisualStyleBackColor = false;
            btnClearUser.Click += btnClearUser_Click;
            // 
            // btnSaveUser
            // 
            btnSaveUser.AutoSize = true;
            btnSaveUser.BackColor = Color.WhiteSmoke;
            btnSaveUser.Dock = DockStyle.Fill;
            btnSaveUser.FlatStyle = FlatStyle.Flat;
            btnSaveUser.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            btnSaveUser.ForeColor = Color.Black;
            btnSaveUser.Location = new Point(3, 2);
            btnSaveUser.Margin = new Padding(3, 2, 3, 2);
            btnSaveUser.Name = "btnSaveUser";
            btnSaveUser.Size = new Size(84, 28);
            btnSaveUser.TabIndex = 0;
            btnSaveUser.Text = "Save";
            btnSaveUser.UseVisualStyleBackColor = false;
            btnSaveUser.Click += btnSaveUser_Click;
            // 
            // tabModifyUser
            // 
            tabModifyUser.BackColor = Color.LightBlue;
            tabModifyUser.Controls.Add(tableLayoutPanel1);
            tabModifyUser.Location = new Point(4, 32);
            tabModifyUser.Name = "tabModifyUser";
            tabModifyUser.Padding = new Padding(9);
            tabModifyUser.Size = new Size(796, 532);
            tabModifyUser.TabIndex = 1;
            tabModifyUser.Text = "Manage Access";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.Beige;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52.9972763F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 47.0027237F));
            tableLayoutPanel1.Controls.Add(dgvAccessRights, 0, 1);
            tableLayoutPanel1.Controls.Add(groupBox1, 1, 0);
            tableLayoutPanel1.Controls.Add(groupBox2, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(9, 9);
            tableLayoutPanel1.Margin = new Padding(3, 2, 3, 2);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
            tableLayoutPanel1.Size = new Size(778, 514);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // dgvAccessRights
            // 
            dgvAccessRights.AllowUserToAddRows = false;
            dgvAccessRights.AllowUserToDeleteRows = false;
            dgvAccessRights.AllowUserToResizeColumns = false;
            dgvAccessRights.AllowUserToResizeRows = false;
            dataGridViewCellStyle5.BackColor = Color.WhiteSmoke;
            dgvAccessRights.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            dgvAccessRights.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvAccessRights.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvAccessRights.BackgroundColor = Color.WhiteSmoke;
            dgvAccessRights.BorderStyle = BorderStyle.None;
            dgvAccessRights.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvAccessRights.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle6.BackColor = Color.LightSteelBlue;
            dataGridViewCellStyle6.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle6.ForeColor = Color.Maroon;
            dataGridViewCellStyle6.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle6.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            dgvAccessRights.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            dgvAccessRights.ColumnHeadersHeight = 29;
            dgvAccessRights.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            tableLayoutPanel1.SetColumnSpan(dgvAccessRights, 3);
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle7.BackColor = SystemColors.Window;
            dataGridViewCellStyle7.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle7.ForeColor = Color.Black;
            dataGridViewCellStyle7.SelectionBackColor = Color.White;
            dataGridViewCellStyle7.SelectionForeColor = Color.Black;
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.False;
            dgvAccessRights.DefaultCellStyle = dataGridViewCellStyle7;
            dgvAccessRights.Dock = DockStyle.Fill;
            dgvAccessRights.EnableHeadersVisualStyles = false;
            dgvAccessRights.GridColor = Color.Brown;
            dgvAccessRights.Location = new Point(3, 208);
            dgvAccessRights.Name = "dgvAccessRights";
            dgvAccessRights.ReadOnly = true;
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle8.BackColor = SystemColors.Control;
            dataGridViewCellStyle8.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle8.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = SystemColors.ActiveCaption;
            dataGridViewCellStyle8.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.True;
            dgvAccessRights.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
            dgvAccessRights.RowHeadersVisible = false;
            dgvAccessRights.RowHeadersWidth = 51;
            dgvAccessRights.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgvAccessRights.RowTemplate.Height = 35;
            dgvAccessRights.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAccessRights.Size = new Size(772, 303);
            dgvAccessRights.TabIndex = 35;
            dgvAccessRights.CellFormatting += dgvAccessRights_CellFormatting;
            dgvAccessRights.DataError += dgvAccessRights_DataError;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(tableLayoutPanel9);
            groupBox1.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox1.Location = new Point(415, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(360, 199);
            groupBox1.TabIndex = 34;
            groupBox1.TabStop = false;
            groupBox1.Text = "Select Privileges";
            // 
            // tableLayoutPanel9
            // 
            tableLayoutPanel9.AutoSize = true;
            tableLayoutPanel9.ColumnCount = 2;
            tableLayoutPanel9.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 46.9287453F));
            tableLayoutPanel9.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 53.0712547F));
            tableLayoutPanel9.Controls.Add(chkPlayAnnc, 0, 4);
            tableLayoutPanel9.Controls.Add(chkVDC, 1, 0);
            tableLayoutPanel9.Controls.Add(chkTADBCGDBSend, 0, 3);
            tableLayoutPanel9.Controls.Add(chkUserSettings, 1, 2);
            tableLayoutPanel9.Controls.Add(chkGeneralMessage, 0, 2);
            tableLayoutPanel9.Controls.Add(chkBoardSetting, 1, 1);
            tableLayoutPanel9.Controls.Add(chkAdminSetting, 0, 1);
            tableLayoutPanel9.Controls.Add(chkTrainEntry, 0, 0);
            tableLayoutPanel9.Dock = DockStyle.Fill;
            tableLayoutPanel9.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tableLayoutPanel9.Location = new Point(3, 28);
            tableLayoutPanel9.Name = "tableLayoutPanel9";
            tableLayoutPanel9.RowCount = 5;
            tableLayoutPanel9.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel9.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel9.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel9.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel9.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel9.Size = new Size(354, 168);
            tableLayoutPanel9.TabIndex = 0;
            // 
            // chkPlayAnnc
            // 
            chkPlayAnnc.AutoSize = true;
            chkPlayAnnc.Dock = DockStyle.Fill;
            chkPlayAnnc.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            chkPlayAnnc.Location = new Point(3, 135);
            chkPlayAnnc.Name = "chkPlayAnnc";
            chkPlayAnnc.Size = new Size(160, 30);
            chkPlayAnnc.TabIndex = 23;
            chkPlayAnnc.Text = "Play Announcement";
            chkPlayAnnc.UseVisualStyleBackColor = true;
            // 
            // chkVDC
            // 
            chkVDC.AutoSize = true;
            chkVDC.Dock = DockStyle.Fill;
            chkVDC.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            chkVDC.Location = new Point(169, 3);
            chkVDC.Name = "chkVDC";
            chkVDC.Size = new Size(182, 27);
            chkVDC.TabIndex = 20;
            chkVDC.Text = "VDC";
            chkVDC.UseVisualStyleBackColor = true;
            // 
            // chkTADBCGDBSend
            // 
            chkTADBCGDBSend.AutoSize = true;
            chkTADBCGDBSend.Dock = DockStyle.Fill;
            chkTADBCGDBSend.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            chkTADBCGDBSend.Location = new Point(3, 102);
            chkTADBCGDBSend.Name = "chkTADBCGDBSend";
            chkTADBCGDBSend.Size = new Size(160, 27);
            chkTADBCGDBSend.TabIndex = 19;
            chkTADBCGDBSend.Text = "TADB , CGDB Send";
            chkTADBCGDBSend.UseVisualStyleBackColor = true;
            // 
            // chkUserSettings
            // 
            chkUserSettings.AutoSize = true;
            chkUserSettings.Dock = DockStyle.Fill;
            chkUserSettings.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            chkUserSettings.Location = new Point(169, 69);
            chkUserSettings.Name = "chkUserSettings";
            chkUserSettings.Size = new Size(182, 27);
            chkUserSettings.TabIndex = 17;
            chkUserSettings.Text = "User Settings";
            chkUserSettings.UseVisualStyleBackColor = true;
            // 
            // chkGeneralMessage
            // 
            chkGeneralMessage.AutoSize = true;
            chkGeneralMessage.Dock = DockStyle.Fill;
            chkGeneralMessage.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            chkGeneralMessage.Location = new Point(3, 69);
            chkGeneralMessage.Name = "chkGeneralMessage";
            chkGeneralMessage.Size = new Size(160, 27);
            chkGeneralMessage.TabIndex = 16;
            chkGeneralMessage.Text = "General Message";
            chkGeneralMessage.UseVisualStyleBackColor = true;
            // 
            // chkBoardSetting
            // 
            chkBoardSetting.AutoSize = true;
            chkBoardSetting.Dock = DockStyle.Fill;
            chkBoardSetting.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            chkBoardSetting.Location = new Point(169, 36);
            chkBoardSetting.Name = "chkBoardSetting";
            chkBoardSetting.Size = new Size(182, 27);
            chkBoardSetting.TabIndex = 14;
            chkBoardSetting.Text = "Network";
            chkBoardSetting.UseVisualStyleBackColor = true;
            // 
            // chkAdminSetting
            // 
            chkAdminSetting.AutoSize = true;
            chkAdminSetting.Dock = DockStyle.Fill;
            chkAdminSetting.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            chkAdminSetting.Location = new Point(3, 36);
            chkAdminSetting.Name = "chkAdminSetting";
            chkAdminSetting.Size = new Size(160, 27);
            chkAdminSetting.TabIndex = 13;
            chkAdminSetting.Text = "Admin Setting";
            chkAdminSetting.UseVisualStyleBackColor = true;
            // 
            // chkTrainEntry
            // 
            chkTrainEntry.AutoSize = true;
            chkTrainEntry.Dock = DockStyle.Fill;
            chkTrainEntry.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkTrainEntry.Location = new Point(3, 3);
            chkTrainEntry.Name = "chkTrainEntry";
            chkTrainEntry.Size = new Size(160, 27);
            chkTrainEntry.TabIndex = 11;
            chkTrainEntry.Text = "Train Entry";
            chkTrainEntry.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(tableLayoutPanel2);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox2.Location = new Point(3, 2);
            groupBox2.Margin = new Padding(3, 2, 3, 2);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(3, 2, 3, 2);
            groupBox2.Size = new Size(406, 201);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "Create and Modify Access ";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 54.75F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45.25F));
            tableLayoutPanel2.Controls.Add(txtUserType, 1, 0);
            tableLayoutPanel2.Controls.Add(label5, 0, 0);
            tableLayoutPanel2.Controls.Add(chkActiveAccess, 1, 1);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel5, 1, 3);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 27);
            tableLayoutPanel2.Margin = new Padding(3, 2, 3, 2);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 4;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 19.1860466F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 18.60465F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 38.9534874F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 23.2558136F));
            tableLayoutPanel2.Size = new Size(400, 172);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // txtUserType
            // 
            txtUserType.Dock = DockStyle.Fill;
            txtUserType.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtUserType.Location = new Point(222, 2);
            txtUserType.Margin = new Padding(3, 2, 3, 2);
            txtUserType.Name = "txtUserType";
            txtUserType.Size = new Size(175, 29);
            txtUserType.TabIndex = 24;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Dock = DockStyle.Fill;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(3, 0);
            label5.Name = "label5";
            label5.Size = new Size(213, 33);
            label5.TabIndex = 7;
            label5.Text = "Access Type :";
            label5.TextAlign = ContentAlignment.MiddleRight;
            // 
            // chkActiveAccess
            // 
            chkActiveAccess.Dock = DockStyle.Fill;
            chkActiveAccess.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkActiveAccess.Location = new Point(222, 36);
            chkActiveAccess.Name = "chkActiveAccess";
            chkActiveAccess.Size = new Size(175, 26);
            chkActiveAccess.TabIndex = 26;
            chkActiveAccess.Text = "Active";
            chkActiveAccess.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.ColumnCount = 2;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.Controls.Add(btnClearAccessRight, 1, 0);
            tableLayoutPanel5.Controls.Add(btnSaveAccessRight, 0, 0);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(222, 134);
            tableLayoutPanel5.Margin = new Padding(3, 2, 3, 2);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 1;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 39F));
            tableLayoutPanel5.Size = new Size(175, 36);
            tableLayoutPanel5.TabIndex = 49;
            // 
            // btnClearAccessRight
            // 
            btnClearAccessRight.BackColor = Color.WhiteSmoke;
            btnClearAccessRight.Dock = DockStyle.Fill;
            btnClearAccessRight.FlatStyle = FlatStyle.Flat;
            btnClearAccessRight.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            btnClearAccessRight.ForeColor = Color.Black;
            btnClearAccessRight.ImageAlign = ContentAlignment.MiddleLeft;
            btnClearAccessRight.Location = new Point(90, 2);
            btnClearAccessRight.Margin = new Padding(3, 2, 3, 2);
            btnClearAccessRight.Name = "btnClearAccessRight";
            btnClearAccessRight.Size = new Size(82, 32);
            btnClearAccessRight.TabIndex = 50;
            btnClearAccessRight.Text = "Reset";
            btnClearAccessRight.UseVisualStyleBackColor = false;
            btnClearAccessRight.Click += btnClearAccessRight_Click;
            // 
            // btnSaveAccessRight
            // 
            btnSaveAccessRight.BackColor = Color.WhiteSmoke;
            btnSaveAccessRight.Dock = DockStyle.Fill;
            btnSaveAccessRight.FlatStyle = FlatStyle.Flat;
            btnSaveAccessRight.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            btnSaveAccessRight.ForeColor = Color.Black;
            btnSaveAccessRight.ImageAlign = ContentAlignment.MiddleLeft;
            btnSaveAccessRight.Location = new Point(3, 2);
            btnSaveAccessRight.Margin = new Padding(3, 2, 3, 2);
            btnSaveAccessRight.Name = "btnSaveAccessRight";
            btnSaveAccessRight.Size = new Size(81, 32);
            btnSaveAccessRight.TabIndex = 49;
            btnSaveAccessRight.Text = "Save";
            btnSaveAccessRight.UseVisualStyleBackColor = false;
            btnSaveAccessRight.Click += btnSaveAccessRight_Click;
            // 
            // ucUserControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoValidate = AutoValidate.EnablePreventFocusChange;
            BackColor = Color.White;
            Controls.Add(panelMain);
            DoubleBuffered = true;
            Name = "ucUserControl";
            Size = new Size(804, 568);
            Load += ucUserControl_Load;
            panelMain.ResumeLayout(false);
            tabUserControl.ResumeLayout(false);
            tabUserCreation.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvUsers).EndInit();
            tableLayoutPanel6.ResumeLayout(false);
            gboxUser.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            tableLayoutPanel10.ResumeLayout(false);
            tableLayoutPanel10.PerformLayout();
            tabModifyUser.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvAccessRights).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tableLayoutPanel9.ResumeLayout(false);
            tableLayoutPanel9.PerformLayout();
            groupBox2.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            tableLayoutPanel5.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private Panel panelMain;
        private TabControl tabUserControl;
        private TabPage tabUserCreation;
        private TabPage tabModifyUser;
        private TableLayoutPanel tableLayoutPanel3;
        private Panel panel2;
        private GroupBox groupbox2Select;
        private TableLayoutPanel tableLayoutPanel4;
        private CheckBox checkBox11;
        private CheckBox checkBox12;
        private CheckBox checkBox13;
        private CheckBox checkBox14;
        private CheckBox checkBox15;
        private CheckBox checkBox16;
        private CheckBox checkBox17;
        private CheckBox checkBox18;
        private CheckBox checkBox19;
        private CheckBox checkBox20;
        private ComboBox comboBox3;
        private TabPage tabCategoryCreation;
        private Panel panel3;
        private Button button6;
        private Button button5;
        private GroupBox groupBox2;
        private CheckBox checkBox21;
        private CheckBox checkBox22;
        private CheckBox checkBox23;
        private CheckBox checkBox24;
        private CheckBox checkBox25;
        private CheckBox checkBox26;
        private CheckBox checkBox27;
        private CheckBox checkBox28;
        private CheckBox checkBox29;
        private CheckBox checkBox30;
        private TextBox textBox6;
        private TabPage tabModifyCategory;
        private TableLayoutPanel tableLayoutPanel7;
        private Panel panel4;
        private Button button8;
        private Button button7;
        private GroupBox groupBox3;
        private TableLayoutPanel tableLayoutPanel8;
        private CheckBox checkBox31;
        private CheckBox checkBox32;
        private CheckBox checkBox33;
        private CheckBox checkBox34;
        private CheckBox checkBox35;
        private CheckBox checkBox36;
        private CheckBox checkBox37;
        private CheckBox checkBox38;
        private CheckBox checkBox39;
        private CheckBox checkBox40;
        private GroupBox gboxUser;
        private ComboBox cmbAccessType;
        private Label label4;
        private TextBox txtRePassword;
        private Label lblReEnter;
        private TextBox txtPassword;
        private Label label2;
        private TextBox txtUserName;
        private Label lblUserName;
        private GroupBox groupBox4;
        private DataGridView dgvUsers;
        private CheckBox chkActive;
        private TableLayoutPanel tableLayoutPanel1;
        private DataGridView dgvAccessRights;
        private GroupBox groupBox1;
        private TableLayoutPanel tableLayoutPanel9;
        //private CheckBox chkCCTVDisplay;
        private CheckBox chkPlayAnnc;
        private CheckBox chkVDC;
        private CheckBox chkTADBCGDBSend;
        private CheckBox chkUserSettings;
        private CheckBox chkGeneralMessage;
        private CheckBox chkBoardSetting;
        private CheckBox chkAdminSetting;
        private CheckBox chkTrainEntry;
        //private CheckBox chkRedundantPC;
        private TableLayoutPanel tableLayoutPanel2;
        private TextBox txtUserType;
        private Label label5;
        private CheckBox chkActiveAccess;
        private TableLayoutPanel tableLayoutPanel5;
        private Button btnClearAccessRight;
        private Button btnSaveAccessRight;
        private TableLayoutPanel tableLayoutPanel10;
        private Button btnClearUser;
        private Button btnSaveUser;
        private TableLayoutPanel tableLayoutPanel6;
    }
}
