namespace IPIS
{
    partial class ucInterface
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucInterface));
            label1 = new Label();
            tableInterfaceMain = new TableLayoutPanel();
            panelLeftMain = new Panel();
            tableInterfaceLeftMenu = new TableLayoutPanel();
            btnAllLed = new Button();
            btnRestoreDatabase = new Button();
            btnBackupDatabase = new Button();
            btnRedundantPC = new Button();
            btnAdminSetting = new Button();
            btnAlerts = new Button();
            btnCCTV = new Button();
            panelRightInterface = new Panel();
            panelMainContent = new Panel();
            pictureBox1 = new PictureBox();
            tableInterfaceMain.SuspendLayout();
            panelLeftMain.SuspendLayout();
            tableInterfaceLeftMenu.SuspendLayout();
            panelRightInterface.SuspendLayout();
            panelMainContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(53, 15);
            label1.TabIndex = 0;
            label1.Text = "Interface";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tableInterfaceMain
            // 
            tableInterfaceMain.ColumnCount = 2;
            tableInterfaceMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableInterfaceMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80F));
            tableInterfaceMain.Controls.Add(panelLeftMain, 0, 0);
            tableInterfaceMain.Controls.Add(panelRightInterface, 1, 0);
            tableInterfaceMain.Dock = DockStyle.Fill;
            tableInterfaceMain.Location = new Point(0, 0);
            tableInterfaceMain.Name = "tableInterfaceMain";
            tableInterfaceMain.RowCount = 1;
            tableInterfaceMain.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableInterfaceMain.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableInterfaceMain.Size = new Size(1162, 488);
            tableInterfaceMain.TabIndex = 1;
            // 
            // panelLeftMain
            // 
            panelLeftMain.Controls.Add(tableInterfaceLeftMenu);
            panelLeftMain.Dock = DockStyle.Fill;
            panelLeftMain.Location = new Point(3, 3);
            panelLeftMain.Name = "panelLeftMain";
            panelLeftMain.Size = new Size(226, 482);
            panelLeftMain.TabIndex = 0;
            // 
            // tableInterfaceLeftMenu
            // 
            tableInterfaceLeftMenu.ColumnCount = 1;
            tableInterfaceLeftMenu.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableInterfaceLeftMenu.Controls.Add(btnAllLed, 0, 6);
            tableInterfaceLeftMenu.Controls.Add(btnRestoreDatabase, 0, 5);
            tableInterfaceLeftMenu.Controls.Add(btnBackupDatabase, 0, 4);
            tableInterfaceLeftMenu.Controls.Add(btnRedundantPC, 0, 3);
            tableInterfaceLeftMenu.Controls.Add(btnAdminSetting, 0, 2);
            tableInterfaceLeftMenu.Controls.Add(btnAlerts, 0, 1);
            tableInterfaceLeftMenu.Controls.Add(btnCCTV, 0, 0);
            tableInterfaceLeftMenu.Dock = DockStyle.Fill;
            tableInterfaceLeftMenu.Location = new Point(0, 0);
            tableInterfaceLeftMenu.Name = "tableInterfaceLeftMenu";
            tableInterfaceLeftMenu.RowCount = 7;
            tableInterfaceLeftMenu.RowStyles.Add(new RowStyle(SizeType.Percent, 14.28571F));
            tableInterfaceLeftMenu.RowStyles.Add(new RowStyle(SizeType.Percent, 14.28571F));
            tableInterfaceLeftMenu.RowStyles.Add(new RowStyle(SizeType.Percent, 14.28571F));
            tableInterfaceLeftMenu.RowStyles.Add(new RowStyle(SizeType.Percent, 14.28571F));
            tableInterfaceLeftMenu.RowStyles.Add(new RowStyle(SizeType.Percent, 14.28571F));
            tableInterfaceLeftMenu.RowStyles.Add(new RowStyle(SizeType.Percent, 14.28571F));
            tableInterfaceLeftMenu.RowStyles.Add(new RowStyle(SizeType.Percent, 14.28571F));
            tableInterfaceLeftMenu.Size = new Size(226, 482);
            tableInterfaceLeftMenu.TabIndex = 0;
            // 
            // btnAllLed
            // 
            btnAllLed.BackColor = Color.AliceBlue;
            btnAllLed.Dock = DockStyle.Fill;
            btnAllLed.FlatStyle = FlatStyle.Flat;
            btnAllLed.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAllLed.ForeColor = Color.DarkBlue;
            btnAllLed.Location = new Point(3, 411);
            btnAllLed.Name = "btnAllLed";
            btnAllLed.Size = new Size(220, 68);
            btnAllLed.TabIndex = 6;
            btnAllLed.Text = "All Led";
            btnAllLed.UseCompatibleTextRendering = true;
            btnAllLed.UseVisualStyleBackColor = false;
            btnAllLed.Click += AllLED_Click;
            // 
            // btnRestoreDatabase
            // 
            btnRestoreDatabase.BackColor = Color.AliceBlue;
            btnRestoreDatabase.Dock = DockStyle.Fill;
            btnRestoreDatabase.FlatStyle = FlatStyle.Flat;
            btnRestoreDatabase.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRestoreDatabase.ForeColor = Color.DarkBlue;
            btnRestoreDatabase.Location = new Point(3, 343);
            btnRestoreDatabase.Name = "btnRestoreDatabase";
            btnRestoreDatabase.Size = new Size(220, 62);
            btnRestoreDatabase.TabIndex = 5;
            btnRestoreDatabase.UseVisualStyleBackColor = false;
            // 
            // btnBackupDatabase
            // 
            btnBackupDatabase.BackColor = Color.AliceBlue;
            btnBackupDatabase.Dock = DockStyle.Fill;
            btnBackupDatabase.FlatStyle = FlatStyle.Flat;
            btnBackupDatabase.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnBackupDatabase.ForeColor = Color.DarkBlue;
            btnBackupDatabase.Location = new Point(3, 275);
            btnBackupDatabase.Name = "btnBackupDatabase";
            btnBackupDatabase.Size = new Size(220, 62);
            btnBackupDatabase.TabIndex = 4;
            btnBackupDatabase.Text = "Backup Database";
            btnBackupDatabase.UseVisualStyleBackColor = false;
            btnBackupDatabase.Click += btnBackupDatabase_Click;
            // 
            // btnRedundantPC
            // 
            btnRedundantPC.BackColor = Color.AliceBlue;
            btnRedundantPC.Dock = DockStyle.Fill;
            btnRedundantPC.FlatStyle = FlatStyle.Flat;
            btnRedundantPC.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRedundantPC.ForeColor = Color.DarkBlue;
            btnRedundantPC.Location = new Point(3, 207);
            btnRedundantPC.Name = "btnRedundantPC";
            btnRedundantPC.Size = new Size(220, 62);
            btnRedundantPC.TabIndex = 3;
            btnRedundantPC.UseVisualStyleBackColor = false;
            // 
            // btnAdminSetting
            // 
            btnAdminSetting.BackColor = Color.AliceBlue;
            btnAdminSetting.Dock = DockStyle.Fill;
            btnAdminSetting.FlatStyle = FlatStyle.Flat;
            btnAdminSetting.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAdminSetting.ForeColor = Color.DarkBlue;
            btnAdminSetting.Location = new Point(3, 139);
            btnAdminSetting.Name = "btnAdminSetting";
            btnAdminSetting.Size = new Size(220, 62);
            btnAdminSetting.TabIndex = 2;
            btnAdminSetting.Text = "Admin Setting";
            btnAdminSetting.UseVisualStyleBackColor = false;
            btnAdminSetting.Click += btnAdminSetting_Click;
            // 
            // btnAlerts
            // 
            btnAlerts.BackColor = Color.AliceBlue;
            btnAlerts.Dock = DockStyle.Fill;
            btnAlerts.FlatStyle = FlatStyle.Flat;
            btnAlerts.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAlerts.ForeColor = Color.DarkBlue;
            btnAlerts.Location = new Point(3, 71);
            btnAlerts.Name = "btnAlerts";
            btnAlerts.Size = new Size(220, 62);
            btnAlerts.TabIndex = 1;
            btnAlerts.UseVisualStyleBackColor = false;
            // 
            // btnCCTV
            // 
            btnCCTV.BackColor = Color.AliceBlue;
            btnCCTV.Dock = DockStyle.Fill;
            btnCCTV.FlatStyle = FlatStyle.Flat;
            btnCCTV.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCCTV.ForeColor = Color.DarkBlue;
            btnCCTV.Location = new Point(3, 3);
            btnCCTV.Name = "btnCCTV";
            btnCCTV.Size = new Size(220, 62);
            btnCCTV.TabIndex = 0;
            btnCCTV.UseVisualStyleBackColor = false;
            // 
            // panelRightInterface
            // 
            panelRightInterface.Controls.Add(panelMainContent);
            panelRightInterface.Dock = DockStyle.Fill;
            panelRightInterface.Location = new Point(235, 3);
            panelRightInterface.Name = "panelRightInterface";
            panelRightInterface.Size = new Size(924, 482);
            panelRightInterface.TabIndex = 1;
            // 
            // panelMainContent
            // 
            panelMainContent.BackColor = Color.WhiteSmoke;
            panelMainContent.BackgroundImageLayout = ImageLayout.Zoom;
            panelMainContent.Controls.Add(pictureBox1);
            panelMainContent.Dock = DockStyle.Fill;
            panelMainContent.Location = new Point(0, 0);
            panelMainContent.Name = "panelMainContent";
            panelMainContent.Size = new Size(924, 482);
            panelMainContent.TabIndex = 2;
            panelMainContent.Paint += panelMainContent_Paint;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(206, 51);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(511, 381);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // ucInterface
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tableInterfaceMain);
            Controls.Add(label1);
            Name = "ucInterface";
            Size = new Size(1162, 488);
            tableInterfaceMain.ResumeLayout(false);
            panelLeftMain.ResumeLayout(false);
            tableInterfaceLeftMenu.ResumeLayout(false);
            panelRightInterface.ResumeLayout(false);
            panelMainContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableInterfaceMain;
        private System.Windows.Forms.Panel panelLeftMain;
        private System.Windows.Forms.TableLayoutPanel tableInterfaceLeftMenu;
        private System.Windows.Forms.Button btnCCTV;
        private System.Windows.Forms.Button btnAllLed;
        private System.Windows.Forms.Button btnRestoreDatabase;
        private System.Windows.Forms.Button btnBackupDatabase;
        private System.Windows.Forms.Button btnRedundantPC;
        private System.Windows.Forms.Button btnAdminSetting;
        private System.Windows.Forms.Panel panelRightInterface;
        private System.Windows.Forms.Panel panelMainContent;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Button btnAlerts;
    }
}
