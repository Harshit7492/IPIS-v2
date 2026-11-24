namespace IPIS
{
    partial class ucVDC
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            tabControlMain = new TabControl();
            tabVDCControl = new TabPage();
            tableLayoutPanel1 = new TableLayoutPanel();
            dataGridView1 = new DataGridView();
            gbFileInfo = new GroupBox();
            tableLayoutFile = new TableLayoutPanel();
            lblAdvType = new Label();
            cmbAdvType = new ComboBox();
            lblFilePath = new Label();
            txtFilePath = new TextBox();
            btnBrowse = new Button();
            gbPlayInfo = new GroupBox();
            tableLayoutPlay = new TableLayoutPanel();
            chkDisplayStatus = new CheckBox();
            chkBottomMessage = new CheckBox();
            chkShowTime = new CheckBox();
            playDurationPanel = new FlowLayoutPanel();
            lblPlayDuration = new Label();
            numPlayDuration = new NumericUpDown();
            playTimesPanel = new FlowLayoutPanel();
            lblPlayTimes = new Label();
            numPlayTimes = new NumericUpDown();
            btnSaveUpdate = new Button();
            gbMessageInfo = new GroupBox();
            tableLayoutMessage = new TableLayoutPanel();
            lblMessageEnglish = new Label();
            txtMessageEnglish = new TextBox();
            lblMessageHindi = new Label();
            txtMessageHindi = new TextBox();
            lblMessageRegional = new Label();
            txtMessageRegional = new TextBox();
            tabPlaylistManager = new TabPage();
            ucPlaylistManager1 = new ucPlaylistManager();
            tabControlMain.SuspendLayout();
            tabVDCControl.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            gbFileInfo.SuspendLayout();
            tableLayoutFile.SuspendLayout();
            gbPlayInfo.SuspendLayout();
            tableLayoutPlay.SuspendLayout();
            playDurationPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numPlayDuration).BeginInit();
            playTimesPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numPlayTimes).BeginInit();
            gbMessageInfo.SuspendLayout();
            tableLayoutMessage.SuspendLayout();
            tabPlaylistManager.SuspendLayout();
            SuspendLayout();
            // 
            // tabControlMain
            // 
            tabControlMain.Controls.Add(tabVDCControl);
            tabControlMain.Controls.Add(tabPlaylistManager);
            tabControlMain.Dock = DockStyle.Fill;
            tabControlMain.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControlMain.Location = new Point(0, 0);
            tabControlMain.Name = "tabControlMain";
            tabControlMain.Padding = new Point(16, 6);
            tabControlMain.SelectedIndex = 0;
            tabControlMain.Size = new Size(1478, 836);
            tabControlMain.TabIndex = 0;
            tabControlMain.DrawItem += tabControlMain_DrawItem;
            tabControlMain.MouseLeave += tabControlMain_MouseLeave;
            tabControlMain.MouseMove += tabControlMain_MouseMove;
            // 
            // tabVDCControl
            // 
            tabVDCControl.Controls.Add(tableLayoutPanel1);
            tabVDCControl.Location = new Point(4, 40);
            tabVDCControl.Name = "tabVDCControl";
            tabVDCControl.Padding = new Padding(3);
            tabVDCControl.Size = new Size(1470, 792);
            tabVDCControl.TabIndex = 0;
            tabVDCControl.Text = "VDC Control";
            tabVDCControl.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.WhiteSmoke;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(dataGridView1, 0, 0);
            tableLayoutPanel1.Controls.Add(gbFileInfo, 0, 1);
            tableLayoutPanel1.Controls.Add(gbPlayInfo, 1, 1);
            tableLayoutPanel1.Controls.Add(gbMessageInfo, 0, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tableLayoutPanel1.Location = new Point(3, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 52F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 18F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            tableLayoutPanel1.Size = new Size(1464, 786);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.BackgroundColor = Color.Beige;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tableLayoutPanel1.SetColumnSpan(dataGridView1, 2);
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.GridColor = Color.LightGray;
            dataGridView1.Location = new Point(4, 5);
            dataGridView1.Margin = new Padding(4, 5, 4, 5);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.RowTemplate.Height = 35;
            dataGridView1.Size = new Size(1456, 398);
            dataGridView1.TabIndex = 1;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            dataGridView1.CellMouseEnter += dataGridView1_CellMouseEnter;
            dataGridView1.CellToolTipTextNeeded += dataGridView1_CellToolTipTextNeeded;
            // 
            // gbFileInfo
            // 
            gbFileInfo.Controls.Add(tableLayoutFile);
            gbFileInfo.Dock = DockStyle.Fill;
            gbFileInfo.Location = new Point(4, 413);
            gbFileInfo.Margin = new Padding(4, 5, 4, 5);
            gbFileInfo.Name = "gbFileInfo";
            gbFileInfo.Padding = new Padding(4, 5, 4, 5);
            gbFileInfo.Size = new Size(724, 131);
            gbFileInfo.TabIndex = 2;
            gbFileInfo.TabStop = false;
            gbFileInfo.Text = "File Information";
            // 
            // tableLayoutFile
            // 
            tableLayoutFile.ColumnCount = 3;
            tableLayoutFile.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableLayoutFile.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutFile.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutFile.Controls.Add(lblAdvType, 0, 0);
            tableLayoutFile.Controls.Add(cmbAdvType, 1, 0);
            tableLayoutFile.Controls.Add(lblFilePath, 0, 1);
            tableLayoutFile.Controls.Add(txtFilePath, 1, 1);
            tableLayoutFile.Controls.Add(btnBrowse, 2, 1);
            tableLayoutFile.Dock = DockStyle.Fill;
            tableLayoutFile.Location = new Point(4, 33);
            tableLayoutFile.Margin = new Padding(4);
            tableLayoutFile.Name = "tableLayoutFile";
            tableLayoutFile.RowCount = 2;
            tableLayoutFile.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutFile.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutFile.Size = new Size(716, 93);
            tableLayoutFile.TabIndex = 0;
            // 
            // lblAdvType
            // 
            lblAdvType.Dock = DockStyle.Fill;
            lblAdvType.Location = new Point(3, 2);
            lblAdvType.Margin = new Padding(3, 2, 3, 2);
            lblAdvType.Name = "lblAdvType";
            lblAdvType.Size = new Size(144, 42);
            lblAdvType.TabIndex = 0;
            lblAdvType.Text = "Adv. Type:";
            lblAdvType.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbAdvType
            // 
            tableLayoutFile.SetColumnSpan(cmbAdvType, 2);
            cmbAdvType.Dock = DockStyle.Fill;
            cmbAdvType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbAdvType.Font = new Font("Segoe UI", 10F);
            cmbAdvType.Items.AddRange(new object[] { "----Select Adv Type----", "Train Info", "Video", "Message", "Image" });
            cmbAdvType.Location = new Point(153, 5);
            cmbAdvType.Margin = new Padding(3, 5, 3, 5);
            cmbAdvType.Name = "cmbAdvType";
            cmbAdvType.Size = new Size(560, 36);
            cmbAdvType.TabIndex = 1;
            cmbAdvType.SelectedIndexChanged += cmbAdvType_SelectedIndexChanged;
            // 
            // lblFilePath
            // 
            lblFilePath.Dock = DockStyle.Fill;
            lblFilePath.Location = new Point(3, 48);
            lblFilePath.Margin = new Padding(3, 2, 3, 2);
            lblFilePath.Name = "lblFilePath";
            lblFilePath.Size = new Size(144, 43);
            lblFilePath.TabIndex = 2;
            lblFilePath.Text = "File Path:";
            lblFilePath.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtFilePath
            // 
            txtFilePath.Dock = DockStyle.Fill;
            txtFilePath.Font = new Font("Segoe UI", 10F);
            txtFilePath.Location = new Point(153, 51);
            txtFilePath.Margin = new Padding(3, 5, 3, 5);
            txtFilePath.Name = "txtFilePath";
            txtFilePath.Size = new Size(460, 34);
            txtFilePath.TabIndex = 3;
            // 
            // btnBrowse
            // 
            btnBrowse.Dock = DockStyle.Fill;
            btnBrowse.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnBrowse.Location = new Point(619, 49);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(94, 41);
            btnBrowse.TabIndex = 4;
            btnBrowse.Text = "Browse";
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += BtnBrowse_Click;
            // 
            // gbPlayInfo
            // 
            gbPlayInfo.Controls.Add(tableLayoutPlay);
            gbPlayInfo.Dock = DockStyle.Fill;
            gbPlayInfo.Location = new Point(736, 413);
            gbPlayInfo.Margin = new Padding(4, 5, 4, 5);
            gbPlayInfo.Name = "gbPlayInfo";
            gbPlayInfo.Padding = new Padding(4, 5, 4, 5);
            tableLayoutPanel1.SetRowSpan(gbPlayInfo, 2);
            gbPlayInfo.Size = new Size(724, 368);
            gbPlayInfo.TabIndex = 3;
            gbPlayInfo.TabStop = false;
            gbPlayInfo.Text = "Play Information";
            // 
            // tableLayoutPlay
            // 
            tableLayoutPlay.ColumnCount = 2;
            tableLayoutPlay.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPlay.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPlay.Controls.Add(chkDisplayStatus, 0, 0);
            tableLayoutPlay.Controls.Add(chkBottomMessage, 1, 0);
            tableLayoutPlay.Controls.Add(chkShowTime, 0, 1);
            tableLayoutPlay.Controls.Add(playDurationPanel, 1, 1);
            tableLayoutPlay.Controls.Add(playTimesPanel, 0, 2);
            tableLayoutPlay.Controls.Add(btnSaveUpdate, 0, 3);
            tableLayoutPlay.Dock = DockStyle.Fill;
            tableLayoutPlay.Location = new Point(4, 33);
            tableLayoutPlay.Name = "tableLayoutPlay";
            tableLayoutPlay.RowCount = 4;
            tableLayoutPlay.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPlay.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPlay.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPlay.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPlay.Size = new Size(716, 330);
            tableLayoutPlay.TabIndex = 0;
            // 
            // chkDisplayStatus
            // 
            chkDisplayStatus.AutoSize = true;
            chkDisplayStatus.CheckAlign = ContentAlignment.MiddleRight;
            chkDisplayStatus.Checked = true;
            chkDisplayStatus.CheckState = CheckState.Checked;
            chkDisplayStatus.Dock = DockStyle.Left;
            chkDisplayStatus.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            chkDisplayStatus.Location = new Point(3, 10);
            chkDisplayStatus.Margin = new Padding(3, 10, 3, 3);
            chkDisplayStatus.Name = "chkDisplayStatus";
            chkDisplayStatus.Size = new Size(173, 69);
            chkDisplayStatus.TabIndex = 0;
            chkDisplayStatus.Text = "Display Status";
            chkDisplayStatus.UseVisualStyleBackColor = true;
            // 
            // chkBottomMessage
            // 
            chkBottomMessage.AutoSize = true;
            chkBottomMessage.CheckAlign = ContentAlignment.MiddleRight;
            chkBottomMessage.Dock = DockStyle.Left;
            chkBottomMessage.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            chkBottomMessage.Location = new Point(361, 10);
            chkBottomMessage.Margin = new Padding(3, 10, 3, 3);
            chkBottomMessage.Name = "chkBottomMessage";
            chkBottomMessage.Size = new Size(197, 69);
            chkBottomMessage.TabIndex = 1;
            chkBottomMessage.Text = "Bottom Message";
            chkBottomMessage.UseVisualStyleBackColor = true;
            // 
            // chkShowTime
            // 
            chkShowTime.AutoSize = true;
            chkShowTime.CheckAlign = ContentAlignment.MiddleRight;
            chkShowTime.Dock = DockStyle.Left;
            chkShowTime.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            chkShowTime.Location = new Point(3, 92);
            chkShowTime.Margin = new Padding(3, 10, 3, 3);
            chkShowTime.Name = "chkShowTime";
            chkShowTime.Size = new Size(200, 69);
            chkShowTime.TabIndex = 2;
            chkShowTime.Text = "Show Time to TV";
            chkShowTime.UseVisualStyleBackColor = true;
            // 
            // playDurationPanel
            // 
            playDurationPanel.Controls.Add(lblPlayDuration);
            playDurationPanel.Controls.Add(numPlayDuration);
            playDurationPanel.Dock = DockStyle.Fill;
            playDurationPanel.Location = new Point(361, 85);
            playDurationPanel.Name = "playDurationPanel";
            playDurationPanel.Size = new Size(352, 76);
            playDurationPanel.TabIndex = 3;
            playDurationPanel.WrapContents = false;
            // 
            // lblPlayDuration
            // 
            lblPlayDuration.AutoSize = true;
            lblPlayDuration.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPlayDuration.Location = new Point(3, 8);
            lblPlayDuration.Margin = new Padding(3, 8, 3, 3);
            lblPlayDuration.Name = "lblPlayDuration";
            lblPlayDuration.Size = new Size(176, 28);
            lblPlayDuration.TabIndex = 0;
            lblPlayDuration.Text = "Play Duration (s):";
            lblPlayDuration.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // numPlayDuration
            // 
            numPlayDuration.Font = new Font("Segoe UI", 10F);
            numPlayDuration.Location = new Point(185, 5);
            numPlayDuration.Margin = new Padding(3, 5, 3, 3);
            numPlayDuration.Maximum = new decimal(new int[] { 360000, 0, 0, 0 });
            numPlayDuration.Name = "numPlayDuration";
            numPlayDuration.Size = new Size(80, 34);
            numPlayDuration.TabIndex = 1;
            numPlayDuration.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // playTimesPanel
            // 
            playTimesPanel.Controls.Add(lblPlayTimes);
            playTimesPanel.Controls.Add(numPlayTimes);
            playTimesPanel.Dock = DockStyle.Fill;
            playTimesPanel.Location = new Point(3, 167);
            playTimesPanel.Name = "playTimesPanel";
            playTimesPanel.Size = new Size(352, 76);
            playTimesPanel.TabIndex = 4;
            playTimesPanel.WrapContents = false;
            // 
            // lblPlayTimes
            // 
            lblPlayTimes.AutoSize = true;
            lblPlayTimes.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPlayTimes.Location = new Point(3, 8);
            lblPlayTimes.Margin = new Padding(3, 8, 3, 3);
            lblPlayTimes.Name = "lblPlayTimes";
            lblPlayTimes.Size = new Size(119, 28);
            lblPlayTimes.TabIndex = 0;
            lblPlayTimes.Text = "Play Count:";
            lblPlayTimes.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // numPlayTimes
            // 
            numPlayTimes.Font = new Font("Segoe UI", 10F);
            numPlayTimes.Location = new Point(128, 5);
            numPlayTimes.Margin = new Padding(3, 5, 3, 3);
            numPlayTimes.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numPlayTimes.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numPlayTimes.Name = "numPlayTimes";
            numPlayTimes.Size = new Size(80, 34);
            numPlayTimes.TabIndex = 1;
            numPlayTimes.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // btnSaveUpdate
            // 
            btnSaveUpdate.Anchor = AnchorStyles.None;
            tableLayoutPlay.SetColumnSpan(btnSaveUpdate, 2);
            btnSaveUpdate.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSaveUpdate.Location = new Point(300, 267);
            btnSaveUpdate.Margin = new Padding(4);
            btnSaveUpdate.Name = "btnSaveUpdate";
            btnSaveUpdate.Size = new Size(116, 42);
            btnSaveUpdate.TabIndex = 6;
            btnSaveUpdate.Text = "Save";
            btnSaveUpdate.UseVisualStyleBackColor = true;
            btnSaveUpdate.Click += BtnSaveUpdate_Click;
            // 
            // gbMessageInfo
            // 
            gbMessageInfo.Controls.Add(tableLayoutMessage);
            gbMessageInfo.Dock = DockStyle.Fill;
            gbMessageInfo.Location = new Point(4, 554);
            gbMessageInfo.Margin = new Padding(4, 5, 4, 5);
            gbMessageInfo.Name = "gbMessageInfo";
            gbMessageInfo.Padding = new Padding(4, 5, 4, 5);
            gbMessageInfo.Size = new Size(724, 227);
            gbMessageInfo.TabIndex = 4;
            gbMessageInfo.TabStop = false;
            gbMessageInfo.Text = "Message Information";
            // 
            // tableLayoutMessage
            // 
            tableLayoutMessage.ColumnCount = 2;
            tableLayoutMessage.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
            tableLayoutMessage.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutMessage.Controls.Add(lblMessageEnglish, 0, 0);
            tableLayoutMessage.Controls.Add(txtMessageEnglish, 1, 0);
            tableLayoutMessage.Controls.Add(lblMessageHindi, 0, 1);
            tableLayoutMessage.Controls.Add(txtMessageHindi, 1, 1);
            tableLayoutMessage.Controls.Add(lblMessageRegional, 0, 2);
            tableLayoutMessage.Controls.Add(txtMessageRegional, 1, 2);
            tableLayoutMessage.Dock = DockStyle.Fill;
            tableLayoutMessage.Location = new Point(4, 33);
            tableLayoutMessage.Margin = new Padding(4);
            tableLayoutMessage.Name = "tableLayoutMessage";
            tableLayoutMessage.Padding = new Padding(4);
            tableLayoutMessage.RowCount = 3;
            tableLayoutMessage.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));
            tableLayoutMessage.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));
            tableLayoutMessage.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));
            tableLayoutMessage.Size = new Size(716, 189);
            tableLayoutMessage.TabIndex = 0;
            // 
            // lblMessageEnglish
            // 
            lblMessageEnglish.Dock = DockStyle.Fill;
            lblMessageEnglish.Location = new Point(7, 6);
            lblMessageEnglish.Margin = new Padding(3, 2, 3, 2);
            lblMessageEnglish.Name = "lblMessageEnglish";
            lblMessageEnglish.Size = new Size(154, 56);
            lblMessageEnglish.TabIndex = 0;
            lblMessageEnglish.Text = "Message English:";
            lblMessageEnglish.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtMessageEnglish
            // 
            txtMessageEnglish.Dock = DockStyle.Fill;
            txtMessageEnglish.Font = new Font("Segoe UI", 10F);
            txtMessageEnglish.Location = new Point(167, 18);
            txtMessageEnglish.Margin = new Padding(3, 14, 3, 3);
            txtMessageEnglish.Name = "txtMessageEnglish";
            txtMessageEnglish.PlaceholderText = "Please Enter English Message";
            txtMessageEnglish.Size = new Size(542, 34);
            txtMessageEnglish.TabIndex = 1;
            // 
            // lblMessageHindi
            // 
            lblMessageHindi.Dock = DockStyle.Fill;
            lblMessageHindi.Location = new Point(7, 66);
            lblMessageHindi.Margin = new Padding(3, 2, 3, 2);
            lblMessageHindi.Name = "lblMessageHindi";
            lblMessageHindi.Size = new Size(154, 56);
            lblMessageHindi.TabIndex = 2;
            lblMessageHindi.Text = "Message Hindi:";
            lblMessageHindi.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtMessageHindi
            // 
            txtMessageHindi.Dock = DockStyle.Fill;
            txtMessageHindi.Font = new Font("Segoe UI", 10F);
            txtMessageHindi.Location = new Point(167, 78);
            txtMessageHindi.Margin = new Padding(3, 14, 3, 3);
            txtMessageHindi.Name = "txtMessageHindi";
            txtMessageHindi.PlaceholderText = "Please Enter Hindi Message";
            txtMessageHindi.Size = new Size(542, 34);
            txtMessageHindi.TabIndex = 3;
            // 
            // lblMessageRegional
            // 
            lblMessageRegional.Dock = DockStyle.Fill;
            lblMessageRegional.Location = new Point(7, 126);
            lblMessageRegional.Margin = new Padding(3, 2, 3, 2);
            lblMessageRegional.Name = "lblMessageRegional";
            lblMessageRegional.Size = new Size(154, 57);
            lblMessageRegional.TabIndex = 4;
            lblMessageRegional.Text = "Message Regional:";
            lblMessageRegional.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtMessageRegional
            // 
            txtMessageRegional.Dock = DockStyle.Fill;
            txtMessageRegional.Font = new Font("Segoe UI", 10F);
            txtMessageRegional.Location = new Point(167, 138);
            txtMessageRegional.Margin = new Padding(3, 14, 3, 3);
            txtMessageRegional.Name = "txtMessageRegional";
            txtMessageRegional.PlaceholderText = "Please Enter Regional Message";
            txtMessageRegional.Size = new Size(542, 34);
            txtMessageRegional.TabIndex = 5;
            // 
            // tabPlaylistManager
            // 
            tabPlaylistManager.Controls.Add(ucPlaylistManager1);
            tabPlaylistManager.Location = new Point(4, 40);
            tabPlaylistManager.Name = "tabPlaylistManager";
            tabPlaylistManager.Padding = new Padding(3);
            tabPlaylistManager.Size = new Size(1470, 792);
            tabPlaylistManager.TabIndex = 1;
            tabPlaylistManager.Text = "Playlist Manager";
            tabPlaylistManager.UseVisualStyleBackColor = true;
            // 
            // ucPlaylistManager1
            // 
            ucPlaylistManager1.Dock = DockStyle.Fill;
            ucPlaylistManager1.Location = new Point(3, 3);
            ucPlaylistManager1.Margin = new Padding(4);
            ucPlaylistManager1.Name = "ucPlaylistManager1";
            ucPlaylistManager1.Size = new Size(1464, 786);
            ucPlaylistManager1.TabIndex = 0;
            // 
            // ucVDC
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tabControlMain);
            Margin = new Padding(4, 5, 4, 5);
            Name = "ucVDC";
            Size = new Size(1478, 836);
            Load += ucVDC_Load;
            tabControlMain.ResumeLayout(false);
            tabVDCControl.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            gbFileInfo.ResumeLayout(false);
            tableLayoutFile.ResumeLayout(false);
            tableLayoutFile.PerformLayout();
            gbPlayInfo.ResumeLayout(false);
            tableLayoutPlay.ResumeLayout(false);
            tableLayoutPlay.PerformLayout();
            playDurationPanel.ResumeLayout(false);
            playDurationPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numPlayDuration).EndInit();
            playTimesPanel.ResumeLayout(false);
            playTimesPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numPlayTimes).EndInit();
            gbMessageInfo.ResumeLayout(false);
            tableLayoutMessage.ResumeLayout(false);
            tableLayoutMessage.PerformLayout();
            tabPlaylistManager.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabVDCControl;
        private System.Windows.Forms.TabPage tabPlaylistManager;
        private IPIS.ucPlaylistManager ucPlaylistManager1;

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutFile;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPlay;
        private System.Windows.Forms.TableLayoutPanel tableLayoutMessage;

        private System.Windows.Forms.DataGridView dataGridView1;

        private System.Windows.Forms.GroupBox gbFileInfo;
        private System.Windows.Forms.GroupBox gbPlayInfo;
        private System.Windows.Forms.GroupBox gbMessageInfo;

        private System.Windows.Forms.Label lblAdvType;
        private System.Windows.Forms.ComboBox cmbAdvType;
        private System.Windows.Forms.Label lblFilePath;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnBrowse;

        private System.Windows.Forms.CheckBox chkDisplayStatus;
        private System.Windows.Forms.CheckBox chkBottomMessage;
        private System.Windows.Forms.CheckBox chkShowTime;
        private System.Windows.Forms.FlowLayoutPanel playDurationPanel;
        private System.Windows.Forms.Label lblPlayDuration;
        private System.Windows.Forms.NumericUpDown numPlayDuration;
        private System.Windows.Forms.FlowLayoutPanel playTimesPanel;
        private System.Windows.Forms.Label lblPlayTimes;
        private System.Windows.Forms.NumericUpDown numPlayTimes;

        private System.Windows.Forms.Label lblMessageEnglish;
        private System.Windows.Forms.TextBox txtMessageEnglish;
        private System.Windows.Forms.Label lblMessageHindi;
        private System.Windows.Forms.TextBox txtMessageHindi;
        private System.Windows.Forms.Label lblMessageRegional;
        private System.Windows.Forms.TextBox txtMessageRegional;

        private System.Windows.Forms.Button btnSaveUpdate;
    }
}