namespace IPIS
{
    partial class ucPA
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
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            tableLayoutPanelPA = new TableLayoutPanel();
            tabControl1 = new TabControl();
            tabMusic = new TabPage();
            tableLayoutPanel1 = new TableLayoutPanel();
            dvMusic = new DataGridView();
            tableLayoutPanel5 = new TableLayoutPanel();
            tableLayoutPanel6 = new TableLayoutPanel();
            titleMusiclbl = new Label();
            txtboxMusicTitle = new TextBox();
            btnBrowseAudio = new Button();
            musicPicture = new PictureBox();
            tabRecording = new TabPage();
            tableLayoutPanel2 = new TableLayoutPanel();
            dvRecording = new DataGridView();
            tableLayoutPanel3 = new TableLayoutPanel();
            tableLayoutPanel4 = new TableLayoutPanel();
            txtRetitle = new TextBox();
            btnReStart = new Button();
            btnReRefresh = new Button();
            recordingPicture = new PictureBox();
            openFileDialog1 = new OpenFileDialog();
            tableLayoutPanelPA.SuspendLayout();
            tabControl1.SuspendLayout();
            tabMusic.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dvMusic).BeginInit();
            tableLayoutPanel5.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)musicPicture).BeginInit();
            tabRecording.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dvRecording).BeginInit();
            tableLayoutPanel3.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)recordingPicture).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanelPA
            // 
            tableLayoutPanelPA.BackColor = Color.Beige;
            tableLayoutPanelPA.ColumnCount = 1;
            tableLayoutPanelPA.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanelPA.Controls.Add(tabControl1, 0, 0);
            tableLayoutPanelPA.Dock = DockStyle.Fill;
            tableLayoutPanelPA.Location = new Point(0, 0);
            tableLayoutPanelPA.Name = "tableLayoutPanelPA";
            tableLayoutPanelPA.RowCount = 1;
            tableLayoutPanelPA.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanelPA.Size = new Size(1030, 545);
            tableLayoutPanelPA.TabIndex = 0;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabMusic);
            tabControl1.Controls.Add(tabRecording);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tabControl1.Location = new Point(3, 3);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1024, 539);
            tabControl1.TabIndex = 0;
            // 
            // tabMusic
            // 
            tabMusic.Controls.Add(tableLayoutPanel1);
            tabMusic.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tabMusic.Location = new Point(4, 30);
            tabMusic.Name = "tabMusic";
            tabMusic.Padding = new Padding(3);
            tabMusic.Size = new Size(1016, 505);
            tabMusic.TabIndex = 0;
            tabMusic.Text = "Slogan";
            tabMusic.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(dvMusic, 0, 1);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel5, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(1010, 499);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // dvMusic
            // 
            dvMusic.AllowUserToAddRows = false;
            dvMusic.AllowUserToDeleteRows = false;
            dvMusic.AllowUserToResizeColumns = false;
            dvMusic.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.WhiteSmoke;
            dvMusic.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dvMusic.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dvMusic.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dvMusic.BackgroundColor = Color.AliceBlue;
            dvMusic.BorderStyle = BorderStyle.Fixed3D;
            dvMusic.CellBorderStyle = DataGridViewCellBorderStyle.Raised;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle2.BackColor = Color.LightSteelBlue;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.Maroon;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dvMusic.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dvMusic.ColumnHeadersHeight = 29;
            dvMusic.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle3.BackColor = SystemColors.Window;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = Color.White;
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dvMusic.DefaultCellStyle = dataGridViewCellStyle3;
            dvMusic.Dock = DockStyle.Fill;
            dvMusic.EnableHeadersVisualStyles = false;
            dvMusic.GridColor = Color.Brown;
            dvMusic.Location = new Point(3, 53);
            dvMusic.MultiSelect = false;
            dvMusic.Name = "dvMusic";
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle4.BackColor = SystemColors.Control;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle4.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.ActiveCaption;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            dvMusic.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dvMusic.RowHeadersVisible = false;
            dvMusic.RowHeadersWidth = 51;
            dvMusic.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dvMusic.RowTemplate.Height = 35;
            dvMusic.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dvMusic.Size = new Size(1004, 443);
            dvMusic.TabIndex = 2;
            dvMusic.CellContentClick += dvMusic_CellContentClick_1;
            // 
            // tableLayoutPanel5  (input area on the left, animation picture on the right)
            // 
            tableLayoutPanel5.ColumnCount = 2;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 600F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel5.Controls.Add(tableLayoutPanel6, 0, 0);
            tableLayoutPanel5.Controls.Add(musicPicture, 1, 0);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(3, 3);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 1;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel5.Size = new Size(1004, 44);
            tableLayoutPanel5.TabIndex = 3;
            // 
            // tableLayoutPanel6  (label | textbox | button)
            // 
            tableLayoutPanel6.ColumnCount = 3;
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableLayoutPanel6.Controls.Add(titleMusiclbl, 0, 0);
            tableLayoutPanel6.Controls.Add(txtboxMusicTitle, 1, 0);
            tableLayoutPanel6.Controls.Add(btnBrowseAudio, 2, 0);
            tableLayoutPanel6.Dock = DockStyle.Fill;
            tableLayoutPanel6.Location = new Point(3, 3);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 1;
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel6.Size = new Size(594, 38);
            tableLayoutPanel6.TabIndex = 0;
            // 
            // titleMusiclbl
            // 
            titleMusiclbl.AutoSize = true;
            titleMusiclbl.Dock = DockStyle.Fill;
            titleMusiclbl.Location = new Point(3, 0);
            titleMusiclbl.Margin = new Padding(3, 0, 6, 0);
            titleMusiclbl.Name = "titleMusiclbl";
            titleMusiclbl.Size = new Size(113, 38);
            titleMusiclbl.TabIndex = 0;
            titleMusiclbl.Text = "Slogan Title *";
            titleMusiclbl.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtboxMusicTitle
            // 
            txtboxMusicTitle.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtboxMusicTitle.Location = new Point(125, 4);
            txtboxMusicTitle.Margin = new Padding(0, 3, 8, 3);
            txtboxMusicTitle.Name = "txtboxMusicTitle";
            txtboxMusicTitle.PlaceholderText = "Please Enter title";
            txtboxMusicTitle.Size = new Size(311, 29);
            txtboxMusicTitle.TabIndex = 0;
            // 
            // btnBrowseAudio
            // 
            btnBrowseAudio.Dock = DockStyle.Fill;
            btnBrowseAudio.Location = new Point(447, 3);
            btnBrowseAudio.Margin = new Padding(3);
            btnBrowseAudio.Name = "btnBrowseAudio";
            btnBrowseAudio.Size = new Size(144, 32);
            btnBrowseAudio.TabIndex = 4;
            btnBrowseAudio.Text = "Browse Slogan";
            btnBrowseAudio.UseVisualStyleBackColor = true;
            btnBrowseAudio.Click += btnBrowseAudio_Click;
            // 
            // musicPicture
            // 
            musicPicture.BackgroundImageLayout = ImageLayout.Stretch;
            musicPicture.Dock = DockStyle.Fill;
            musicPicture.Location = new Point(603, 3);
            musicPicture.Name = "musicPicture";
            musicPicture.Size = new Size(398, 38);
            musicPicture.TabIndex = 1;
            musicPicture.TabStop = false;
            // 
            // tabRecording
            // 
            tabRecording.Controls.Add(tableLayoutPanel2);
            tabRecording.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tabRecording.Location = new Point(4, 30);
            tabRecording.Name = "tabRecording";
            tabRecording.Padding = new Padding(3);
            tabRecording.Size = new Size(1016, 505);
            tabRecording.TabIndex = 1;
            tabRecording.Text = "Recording";
            tabRecording.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(dvRecording, 0, 1);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel3, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(1010, 499);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // dvRecording
            // 
            dvRecording.AllowUserToAddRows = false;
            dvRecording.AllowUserToDeleteRows = false;
            dvRecording.AllowUserToResizeColumns = false;
            dvRecording.AllowUserToResizeRows = false;
            dataGridViewCellStyle5.BackColor = Color.WhiteSmoke;
            dvRecording.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            dvRecording.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dvRecording.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dvRecording.BackgroundColor = Color.AliceBlue;
            dvRecording.BorderStyle = BorderStyle.Fixed3D;
            dvRecording.CellBorderStyle = DataGridViewCellBorderStyle.Raised;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle6.BackColor = Color.LightSteelBlue;
            dataGridViewCellStyle6.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle6.ForeColor = Color.Maroon;
            dataGridViewCellStyle6.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle6.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            dvRecording.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            dvRecording.ColumnHeadersHeight = 29;
            dvRecording.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle7.BackColor = SystemColors.Window;
            dataGridViewCellStyle7.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle7.ForeColor = Color.Black;
            dataGridViewCellStyle7.SelectionBackColor = Color.White;
            dataGridViewCellStyle7.SelectionForeColor = Color.Black;
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.False;
            dvRecording.DefaultCellStyle = dataGridViewCellStyle7;
            dvRecording.Dock = DockStyle.Fill;
            dvRecording.EnableHeadersVisualStyles = false;
            dvRecording.GridColor = Color.Brown;
            dvRecording.Location = new Point(3, 53);
            dvRecording.MultiSelect = false;
            dvRecording.Name = "dvRecording";
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle8.BackColor = SystemColors.Control;
            dataGridViewCellStyle8.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle8.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = SystemColors.ActiveCaption;
            dataGridViewCellStyle8.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.True;
            dvRecording.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
            dvRecording.RowHeadersVisible = false;
            dvRecording.RowHeadersWidth = 51;
            dvRecording.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dvRecording.RowTemplate.Height = 35;
            dvRecording.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dvRecording.Size = new Size(1004, 443);
            dvRecording.TabIndex = 2;
            dvRecording.CellContentClick += dvRecording_CellContentClick_1;
            // 
            // tableLayoutPanel3  (input area on the left, animation picture on the right)
            // 
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 600F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Controls.Add(tableLayoutPanel4, 0, 0);
            tableLayoutPanel3.Controls.Add(recordingPicture, 1, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 3);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new Size(1004, 44);
            tableLayoutPanel3.TabIndex = 3;
            // 
            // tableLayoutPanel4  (title box | start/stop | refresh)
            // 
            tableLayoutPanel4.ColumnCount = 3;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            tableLayoutPanel4.Controls.Add(txtRetitle, 0, 0);
            tableLayoutPanel4.Controls.Add(btnReStart, 1, 0);
            tableLayoutPanel4.Controls.Add(btnReRefresh, 2, 0);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(3, 3);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 1;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Size = new Size(594, 38);
            tableLayoutPanel4.TabIndex = 0;
            // 
            // txtRetitle
            // 
            txtRetitle.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtRetitle.Location = new Point(3, 4);
            txtRetitle.Margin = new Padding(3, 3, 8, 3);
            txtRetitle.Name = "txtRetitle";
            txtRetitle.Size = new Size(333, 29);
            txtRetitle.TabIndex = 0;
            // 
            // btnReStart
            // 
            btnReStart.Dock = DockStyle.Fill;
            btnReStart.Location = new Point(347, 3);
            btnReStart.Margin = new Padding(3);
            btnReStart.Name = "btnReStart";
            btnReStart.Size = new Size(134, 32);
            btnReStart.TabIndex = 1;
            btnReStart.Text = "START";
            btnReStart.UseVisualStyleBackColor = true;
            btnReStart.Click += btnReStart_Click;
            // 
            // btnReRefresh
            // 
            btnReRefresh.Dock = DockStyle.Fill;
            btnReRefresh.Location = new Point(487, 3);
            btnReRefresh.Margin = new Padding(3);
            btnReRefresh.Name = "btnReRefresh";
            btnReRefresh.Size = new Size(104, 32);
            btnReRefresh.TabIndex = 3;
            btnReRefresh.Text = "REFRESH";
            btnReRefresh.UseVisualStyleBackColor = true;
            btnReRefresh.Click += btnReRefresh_Click;
            // 
            // recordingPicture
            // 
            recordingPicture.BackgroundImageLayout = ImageLayout.Stretch;
            recordingPicture.Dock = DockStyle.Fill;
            recordingPicture.Location = new Point(603, 3);
            recordingPicture.Name = "recordingPicture";
            recordingPicture.Size = new Size(398, 38);
            recordingPicture.TabIndex = 1;
            recordingPicture.TabStop = false;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // ucPA
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tableLayoutPanelPA);
            Name = "ucPA";
            Size = new Size(1030, 545);
            Load += ucPA_Load;
            tableLayoutPanelPA.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabMusic.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dvMusic).EndInit();
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel6.ResumeLayout(false);
            tableLayoutPanel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)musicPicture).EndInit();
            tabRecording.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dvRecording).EndInit();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)recordingPicture).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelPA;
        private TabControl tabControl1;
        private Label titleMusiclbl;
        private TextBox txtboxMusicTitle;
        private TabPage tabMusic;
        private TabPage tabRecording;
        private TableLayoutPanel tableLayoutPanel2;
        private DataGridView dvRecording;
        private TableLayoutPanel tableLayoutPanel3;
        private TableLayoutPanel tableLayoutPanel4;
        private TextBox txtRetitle;
        private Button btnReStart;
        private Button btnReRefresh;
        private PictureBox recordingPicture;
        private TableLayoutPanel tableLayoutPanel1;
        private DataGridView dvMusic;
        private TableLayoutPanel tableLayoutPanel5;
        private TableLayoutPanel tableLayoutPanel6;
        private PictureBox musicPicture;
        private OpenFileDialog openFileDialog1;
        private Button btnBrowseAudio;
    }
}