namespace IPIS
{
    partial class ucPlaylistManager
    {
        private System.ComponentModel.IContainer components = null;

        private TableLayoutPanel tableLayoutPanelMain;
        private SplitContainer splitContainerMain;
        private GroupBox gbPlaylists;
        private DataGridView dataGridViewPlaylists;
        private GroupBox gbPlaylistDetails;
        private TextBox txtPlaylistName;
        private Label lblPlaylistName;
        private Label lblPlaylistType;
        private ComboBox cmbPlaylistType;
        private CheckBox chkPlaylistActive;
        private Button btnSavePlaylist;
        private Button btnAddNewPlaylist;
        private Label lblRepeat;
        private ComboBox cmbRepeat;
        private Label lblStartDateTime;
        private DateTimePicker dtpStartDateTime;
        private Label lblEndDateTime;
        private DateTimePicker dtpEndDateTime;
        private GroupBox gbPlaylistItems;
        private DataGridView dataGridViewPlaylistItems;
        private Label lblItemCount;
        private GroupBox gbAvailableItems;
        private DataGridView dataGridViewAvailableItems;
        private Label lblAvailableCount;
        private Label lblTVLineCount;
        private ComboBox cmbTVLineCount;
        private DataGridViewTextBoxColumn colPlaylistSrNo;
        private DataGridViewTextBoxColumn colPlaylistID;
        private DataGridViewTextBoxColumn colPlaylistName;
        private DataGridViewTextBoxColumn colPlaylistType;
        private DataGridViewCheckBoxColumn colPlaylistActive;
        private DataGridViewTextBoxColumn colPlaylistRepeat;
        private DataGridViewTextBoxColumn colPlaylistStart;
        private DataGridViewTextBoxColumn colPlaylistEnd;
        private DataGridViewButtonColumn colPlaylistEdit;
        private DataGridViewButtonColumn colPlaylistDelete;
        private DataGridViewButtonColumn colPlaylistPreview;

        private DataGridViewTextBoxColumn colItemOrder;
        private DataGridViewTextBoxColumn colItemType;
        private DataGridViewTextBoxColumn colItemDuration;
        private DataGridViewTextBoxColumn colItemCount;
        private DataGridViewTextBoxColumn colItemFilePath;
        private DataGridViewTextBoxColumn colItemMessageDetails;
        private DataGridViewButtonColumn colItemPreview;
        private DataGridViewButtonColumn colItemRemove;

        private DataGridViewTextBoxColumn colAvailSrNo;
        private DataGridViewTextBoxColumn colAvailType;
        private DataGridViewTextBoxColumn colAvailDuration;
        private DataGridViewTextBoxColumn colAvailCount;
        private DataGridViewTextBoxColumn colAvailID;
        private DataGridViewTextBoxColumn colAvailFileName;
        private DataGridViewTextBoxColumn colAvailFilePath;
        private DataGridViewTextBoxColumn colAvailMessageDetails;
        private DataGridViewButtonColumn colAvailAdd;
        private DataGridViewButtonColumn colAvailPreview;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            tableLayoutPanelMain = new TableLayoutPanel();
            splitContainerMain = new SplitContainer();
            gbPlaylists = new GroupBox();
            dataGridViewPlaylists = new DataGridView();
            gbPlaylistDetails = new GroupBox();
            btnSavePlaylist = new Button();
            btnAddNewPlaylist = new Button();
            chkPlaylistActive = new CheckBox();
            txtPlaylistName = new TextBox();
            lblPlaylistName = new Label();
            lblPlaylistType = new Label();
            cmbPlaylistType = new ComboBox();
            lblRepeat = new Label();
            cmbRepeat = new ComboBox();
            lblStartDateTime = new Label();
            dtpStartDateTime = new DateTimePicker();
            lblEndDateTime = new Label();
            dtpEndDateTime = new DateTimePicker();
            gbPlaylistItems = new GroupBox();
            dataGridViewPlaylistItems = new DataGridView();
            lblItemCount = new Label();
            gbAvailableItems = new GroupBox();
            dataGridViewAvailableItems = new DataGridView();
            lblAvailableCount = new Label();
            lblTVLineCount = new Label();
            cmbTVLineCount = new ComboBox();
            colPlaylistSrNo = new DataGridViewTextBoxColumn();
            colPlaylistID = new DataGridViewTextBoxColumn();
            colPlaylistName = new DataGridViewTextBoxColumn();
            colPlaylistType = new DataGridViewTextBoxColumn();
            colPlaylistActive = new DataGridViewCheckBoxColumn();
            colPlaylistRepeat = new DataGridViewTextBoxColumn();
            colPlaylistStart = new DataGridViewTextBoxColumn();
            colPlaylistEnd = new DataGridViewTextBoxColumn();
            colPlaylistEdit = new DataGridViewButtonColumn();
            colPlaylistDelete = new DataGridViewButtonColumn();
            colPlaylistPreview = new DataGridViewButtonColumn();

            colItemOrder = new DataGridViewTextBoxColumn();
            colItemType = new DataGridViewTextBoxColumn();
            colItemDuration = new DataGridViewTextBoxColumn();
            colItemCount = new DataGridViewTextBoxColumn();
            colItemFilePath = new DataGridViewTextBoxColumn();
            colItemMessageDetails = new DataGridViewTextBoxColumn();
            colItemPreview = new DataGridViewButtonColumn();
            colItemRemove = new DataGridViewButtonColumn();

            colAvailSrNo = new DataGridViewTextBoxColumn();
            colAvailType = new DataGridViewTextBoxColumn();
            colAvailDuration = new DataGridViewTextBoxColumn();
            colAvailCount = new DataGridViewTextBoxColumn();
            colAvailID = new DataGridViewTextBoxColumn();
            colAvailFileName = new DataGridViewTextBoxColumn();
            colAvailFilePath = new DataGridViewTextBoxColumn();
            colAvailMessageDetails = new DataGridViewTextBoxColumn();
            colAvailAdd = new DataGridViewButtonColumn();
            colAvailPreview = new DataGridViewButtonColumn();

            tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).BeginInit();
            splitContainerMain.Panel1.SuspendLayout();
            splitContainerMain.Panel2.SuspendLayout();
            splitContainerMain.SuspendLayout();
            gbPlaylists.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewPlaylists).BeginInit();
            gbPlaylistDetails.SuspendLayout();
            gbPlaylistItems.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewPlaylistItems).BeginInit();
            gbAvailableItems.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewAvailableItems).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            tableLayoutPanelMain.ColumnCount = 1;
            tableLayoutPanelMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanelMain.Controls.Add(splitContainerMain, 0, 0);
            tableLayoutPanelMain.Dock = DockStyle.Fill;
            tableLayoutPanelMain.Location = new Point(0, 0);
            tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            tableLayoutPanelMain.RowCount = 1;
            tableLayoutPanelMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanelMain.Size = new Size(1500, 875);
            tableLayoutPanelMain.TabIndex = 0;
            // 
            // splitContainerMain
            // 
            splitContainerMain.Dock = DockStyle.Fill;
            splitContainerMain.Location = new Point(3, 3);
            splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            splitContainerMain.Panel1.Controls.Add(gbPlaylists);
            splitContainerMain.Panel1.Controls.Add(gbPlaylistDetails);
            // 
            // splitContainerMain.Panel2
            // 
            splitContainerMain.Panel2.Controls.Add(gbPlaylistItems);
            splitContainerMain.Panel2.Controls.Add(gbAvailableItems);
            splitContainerMain.Size = new Size(1494, 869);
            //splitContainerMain.SplitterDistance = 697;
            // Left (playlists) 58% / right (items) 42% of the 1494 px width
            splitContainerMain.SplitterDistance = 867;
            splitContainerMain.TabIndex = 0;
            // 
            // gbPlaylists
            // 
            gbPlaylists.Controls.Add(dataGridViewPlaylists);
            gbPlaylists.Dock = DockStyle.Fill;
            gbPlaylists.Location = new Point(0, 0);
            gbPlaylists.Name = "gbPlaylists";
            gbPlaylists.Padding = new Padding(8);
            gbPlaylists.Size = new Size(697, 434);
            gbPlaylists.TabIndex = 0;
            gbPlaylists.TabStop = false;
            gbPlaylists.Text = "Playlists";
            // 
            // colPlaylistSrNo .. colPlaylistPreview
            // 
            colPlaylistSrNo.Name = "SrNo";
            colPlaylistSrNo.HeaderText = "Sr. No.";
            colPlaylistSrNo.DataPropertyName = "SrNo";
            colPlaylistSrNo.Width = 55;
            colPlaylistSrNo.ReadOnly = true;
            colPlaylistSrNo.SortMode = DataGridViewColumnSortMode.NotSortable;

            colPlaylistID.Name = "ID";
            colPlaylistID.DataPropertyName = "ID";
            colPlaylistID.Visible = false;

            colPlaylistName.Name = "PlaylistName";
            colPlaylistName.HeaderText = "Name";
            colPlaylistName.DataPropertyName = "PlaylistName";
            colPlaylistName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colPlaylistName.ReadOnly = true;
            colPlaylistName.SortMode = DataGridViewColumnSortMode.NotSortable;

            colPlaylistType.Name = "PlaylistType";
            colPlaylistType.HeaderText = "Type";
            colPlaylistType.DataPropertyName = "PlaylistType";
            colPlaylistType.Width = 80;
            colPlaylistType.ReadOnly = true;
            colPlaylistType.SortMode = DataGridViewColumnSortMode.NotSortable;

            colPlaylistActive.Name = "IsActive";
            colPlaylistActive.HeaderText = "Active";
            colPlaylistActive.DataPropertyName = "IsActive";
            colPlaylistActive.Width = 70;
            colPlaylistActive.ReadOnly = true;
            colPlaylistActive.SortMode = DataGridViewColumnSortMode.NotSortable;

            colPlaylistRepeat.Name = "Repeat";
            colPlaylistRepeat.HeaderText = "Repeat";
            colPlaylistRepeat.DataPropertyName = "Repeat";
            colPlaylistRepeat.Width = 70;
            colPlaylistRepeat.ReadOnly = true;
            colPlaylistRepeat.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colPlaylistRepeat.SortMode = DataGridViewColumnSortMode.NotSortable;

            colPlaylistStart.Name = "StartDateTime";
            colPlaylistStart.HeaderText = "STime";
            colPlaylistStart.DataPropertyName = "StartDateTime";
            colPlaylistStart.ReadOnly = true;
            colPlaylistStart.DefaultCellStyle.Format = "dd-MM-yyyy HH:mm";
            colPlaylistStart.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colPlaylistStart.SortMode = DataGridViewColumnSortMode.NotSortable;

            colPlaylistEnd.Name = "EndDateTime";
            colPlaylistEnd.HeaderText = "ETime";
            colPlaylistEnd.DataPropertyName = "EndDateTime";
            colPlaylistEnd.ReadOnly = true;
            colPlaylistEnd.DefaultCellStyle.Format = "dd-MM-yyyy HH:mm";
            colPlaylistEnd.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colPlaylistEnd.SortMode = DataGridViewColumnSortMode.NotSortable;

            colPlaylistEdit.Name = "Edit";
            colPlaylistEdit.HeaderText = "[E]";
            colPlaylistEdit.Text = "✏";
            colPlaylistEdit.UseColumnTextForButtonValue = true;
            colPlaylistEdit.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            colPlaylistDelete.Name = "Delete";
            colPlaylistDelete.HeaderText = "[D]";
            colPlaylistDelete.Text = "✖";
            colPlaylistDelete.UseColumnTextForButtonValue = true;
            colPlaylistDelete.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            colPlaylistPreview.Name = "Preview";
            colPlaylistPreview.HeaderText = "[P]";
            colPlaylistPreview.Text = "👁";
            colPlaylistPreview.UseColumnTextForButtonValue = true;
            colPlaylistPreview.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // 
            // dataGridViewPlaylists
            // 
            dataGridViewPlaylists.AllowUserToAddRows = false;
            dataGridViewPlaylists.AllowUserToDeleteRows = false;
            dataGridViewPlaylists.AutoGenerateColumns = false;
            dataGridViewPlaylists.BackgroundColor = Color.Beige;
            dataGridViewPlaylists.ColumnHeadersHeight = 34;
            dataGridViewPlaylists.Columns.AddRange(new DataGridViewColumn[] {
                colPlaylistSrNo, colPlaylistID, colPlaylistName, colPlaylistType,
                colPlaylistActive, colPlaylistRepeat, colPlaylistStart, colPlaylistEnd,
                colPlaylistEdit, colPlaylistDelete, colPlaylistPreview });
            dataGridViewPlaylists.Dock = DockStyle.Fill;
            dataGridViewPlaylists.GridColor = Color.Gray;
            dataGridViewPlaylists.Location = new Point(8, 32);
            dataGridViewPlaylists.Name = "dataGridViewPlaylists";
            dataGridViewPlaylists.ReadOnly = true;
            dataGridViewPlaylists.RowHeadersVisible = false;
            dataGridViewPlaylists.RowHeadersWidth = 62;
            dataGridViewPlaylists.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewPlaylists.Size = new Size(681, 394);
            dataGridViewPlaylists.TabIndex = 0;
            dataGridViewPlaylists.CellClick += dataGridViewPlaylists_CellClick;
            // 
            // gbPlaylistDetails
            // 
            gbPlaylistDetails.Controls.Add(btnSavePlaylist);
            gbPlaylistDetails.Controls.Add(btnAddNewPlaylist);
            gbPlaylistDetails.Controls.Add(chkPlaylistActive);
            gbPlaylistDetails.Controls.Add(txtPlaylistName);
            gbPlaylistDetails.Controls.Add(lblPlaylistName);
            gbPlaylistDetails.Controls.Add(lblPlaylistType);
            gbPlaylistDetails.Controls.Add(cmbPlaylistType);
            gbPlaylistDetails.Controls.Add(lblTVLineCount);
            gbPlaylistDetails.Controls.Add(cmbTVLineCount);
            gbPlaylistDetails.Controls.Add(lblRepeat);
            gbPlaylistDetails.Controls.Add(cmbRepeat);
            gbPlaylistDetails.Controls.Add(lblStartDateTime);
            gbPlaylistDetails.Controls.Add(dtpStartDateTime);
            gbPlaylistDetails.Controls.Add(lblEndDateTime);
            gbPlaylistDetails.Controls.Add(dtpEndDateTime);
            gbPlaylistDetails.Dock = DockStyle.Bottom;
            gbPlaylistDetails.Location = new Point(0, 434);
            gbPlaylistDetails.Name = "gbPlaylistDetails";
            gbPlaylistDetails.Padding = new Padding(12);
            gbPlaylistDetails.Size = new Size(697, 435);
            gbPlaylistDetails.TabIndex = 1;
            gbPlaylistDetails.TabStop = false;
            gbPlaylistDetails.Text = "Playlist Details";
            // 
            // btnSavePlaylist
            // 
            btnSavePlaylist.Location = new Point(180, 350);
            btnSavePlaylist.Name = "btnSavePlaylist";
            btnSavePlaylist.Size = new Size(140, 45);
            btnSavePlaylist.TabIndex = 11;
            btnSavePlaylist.Text = "Save";
            btnSavePlaylist.UseVisualStyleBackColor = true;
            btnSavePlaylist.Click += btnSavePlaylist_Click;
            // 
            // btnAddNewPlaylist
            // 
            btnAddNewPlaylist.Location = new Point(330, 350);
            btnAddNewPlaylist.Name = "btnAddNewPlaylist";
            btnAddNewPlaylist.Size = new Size(140, 45);
            btnAddNewPlaylist.TabIndex = 12;
            btnAddNewPlaylist.Text = "Add New";
            btnAddNewPlaylist.UseVisualStyleBackColor = true;
            btnAddNewPlaylist.Click += btnAddNewPlaylist_Click;
            // 
            // chkPlaylistActive
            // 
            chkPlaylistActive.AutoSize = true;
            chkPlaylistActive.Location = new Point(180, 140);
            chkPlaylistActive.Name = "chkPlaylistActive";
            chkPlaylistActive.Size = new Size(86, 29);
            chkPlaylistActive.TabIndex = 4;
            chkPlaylistActive.Text = "Active";
            chkPlaylistActive.UseVisualStyleBackColor = true;
            // 
            // txtPlaylistName
            // 
            txtPlaylistName.Location = new Point(180, 36);
            txtPlaylistName.Name = "txtPlaylistName";
            txtPlaylistName.Size = new Size(300, 31);
            txtPlaylistName.TabIndex = 1;
            // 
            // lblPlaylistName
            // 
            lblPlaylistName.AutoSize = true;
            lblPlaylistName.Location = new Point(20, 40);
            lblPlaylistName.Name = "lblPlaylistName";
            lblPlaylistName.Size = new Size(130, 25);
            lblPlaylistName.TabIndex = 0;
            lblPlaylistName.Text = "Playlist Name*:";
            // 
            // lblPlaylistType
            // 
            lblPlaylistType.AutoSize = true;
            lblPlaylistType.Location = new Point(20, 90);
            lblPlaylistType.Name = "lblPlaylistType";
            lblPlaylistType.Size = new Size(120, 25);
            lblPlaylistType.TabIndex = 2;
            lblPlaylistType.Text = "Playlist Type*:";
            // 
            // cmbPlaylistType
            // 
            cmbPlaylistType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPlaylistType.Items.AddRange(new object[] { "VDB", "TV","AVD" });
            cmbPlaylistType.Location = new Point(180, 86);
            cmbPlaylistType.Name = "cmbPlaylistType";
            cmbPlaylistType.Size = new Size(200, 33);
            cmbPlaylistType.TabIndex = 3;
            cmbPlaylistType.SelectedIndexChanged += cmbPlaylistType_SelectedIndexChanged;
            // 
            // lblRepeat
            // 
            lblRepeat.AutoSize = true;
            lblRepeat.Location = new Point(20, 190);
            lblRepeat.Name = "lblRepeat";
            lblRepeat.Size = new Size(70, 25);
            lblRepeat.TabIndex = 5;
            lblRepeat.Text = "Repeat:";
            // 
            // cmbRepeat
            // 
            cmbRepeat.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRepeat.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6" });
            cmbRepeat.Location = new Point(180, 186);
            cmbRepeat.Name = "cmbRepeat";
            cmbRepeat.Size = new Size(100, 33);
            cmbRepeat.TabIndex = 6;
            // 
            // lblStartDateTime
            // 
            lblStartDateTime.AutoSize = true;
            lblStartDateTime.Location = new Point(20, 240);
            lblStartDateTime.Name = "lblStartDateTime";
            lblStartDateTime.Size = new Size(139, 25);
            lblStartDateTime.TabIndex = 7;
            lblStartDateTime.Text = "Start Date/Time:";
            // 
            // dtpStartDateTime
            // 
            dtpStartDateTime.CustomFormat = "dd-MM-yyyy HH:mm";
            dtpStartDateTime.Format = DateTimePickerFormat.Custom;
            dtpStartDateTime.Location = new Point(180, 236);
            dtpStartDateTime.Name = "dtpStartDateTime";
            dtpStartDateTime.ShowUpDown = true;
            dtpStartDateTime.Size = new Size(300, 31);
            dtpStartDateTime.TabIndex = 8;
            // 
            // lblEndDateTime
            // 
            lblEndDateTime.AutoSize = true;
            lblEndDateTime.Location = new Point(20, 290);
            lblEndDateTime.Name = "lblEndDateTime";
            lblEndDateTime.Size = new Size(133, 25);
            lblEndDateTime.TabIndex = 9;
            lblEndDateTime.Text = "End Date/Time:";
            // 
            // dtpEndDateTime
            // 
            dtpEndDateTime.CustomFormat = "dd-MM-yyyy HH:mm";
            dtpEndDateTime.Format = DateTimePickerFormat.Custom;
            dtpEndDateTime.Location = new Point(180, 286);
            dtpEndDateTime.Name = "dtpEndDateTime";
            dtpEndDateTime.ShowUpDown = true;
            dtpEndDateTime.Size = new Size(300, 31);
            dtpEndDateTime.TabIndex = 10;
            // 
            // gbPlaylistItems
            // 
            gbPlaylistItems.Controls.Add(dataGridViewPlaylistItems);
            gbPlaylistItems.Controls.Add(lblItemCount);
            gbPlaylistItems.Dock = DockStyle.Fill;
            gbPlaylistItems.Location = new Point(0, 0);
            gbPlaylistItems.Name = "gbPlaylistItems";
            gbPlaylistItems.Padding = new Padding(12);
            gbPlaylistItems.Size = new Size(793, 434);
            gbPlaylistItems.TabIndex = 0;
            gbPlaylistItems.TabStop = false;
            gbPlaylistItems.Text = "Playlist Items (currently in this playlist)";
            // 
            // colItemOrder .. colItemRemove
            // 
            colItemOrder.Name = "DisplayOrder";
            colItemOrder.HeaderText = "Sr.no";
            colItemOrder.DataPropertyName = "DisplayOrder";
            colItemOrder.Width = 50;
            colItemOrder.ReadOnly = true;
            colItemOrder.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colItemOrder.SortMode = DataGridViewColumnSortMode.NotSortable;

            colItemType.Name = "AdvType";
            colItemType.HeaderText = "Type";
            colItemType.DataPropertyName = "AdvType";
            colItemType.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colItemType.ReadOnly = true;
            colItemType.SortMode = DataGridViewColumnSortMode.NotSortable;

            colItemDuration.Name = "PlayDuration";
            colItemDuration.HeaderText = "Duration (sec)";
            colItemDuration.DataPropertyName = "PlayDuration";
            colItemDuration.ReadOnly = true;
            colItemDuration.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colItemDuration.SortMode = DataGridViewColumnSortMode.NotSortable;

            colItemCount.Name = "PlayTimes";
            colItemCount.HeaderText = "Count";
            colItemCount.DataPropertyName = "PlayTimes";
            colItemCount.ReadOnly = true;
            colItemCount.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colItemCount.SortMode = DataGridViewColumnSortMode.NotSortable;

            colItemFilePath.Name = "FilePath";
            colItemFilePath.DataPropertyName = "FilePath";
            colItemFilePath.Visible = false;

            colItemMessageDetails.Name = "MessageDetails";
            colItemMessageDetails.DataPropertyName = "MessageDetails";
            colItemMessageDetails.Visible = false;

            colItemPreview.Name = "Preview";
            colItemPreview.HeaderText = "[P]";
            colItemPreview.Text = "👁";
            colItemPreview.UseColumnTextForButtonValue = true;
            colItemPreview.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            colItemRemove.Name = "Remove";
            colItemRemove.HeaderText = "[R]";
            colItemRemove.Text = "✖";
            colItemRemove.UseColumnTextForButtonValue = true;
            colItemRemove.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // 
            // dataGridViewPlaylistItems
            // 
            dataGridViewPlaylistItems.AllowUserToAddRows = false;
            dataGridViewPlaylistItems.AllowUserToDeleteRows = false;
            dataGridViewPlaylistItems.AutoGenerateColumns = false;
            dataGridViewPlaylistItems.BackgroundColor = Color.Beige;
            dataGridViewPlaylistItems.ColumnHeadersHeight = 34;
            dataGridViewPlaylistItems.Columns.AddRange(new DataGridViewColumn[] {
                colItemOrder, colItemType, colItemDuration, colItemCount,
                colItemFilePath, colItemMessageDetails, colItemPreview, colItemRemove });
            dataGridViewPlaylistItems.Dock = DockStyle.Fill;
            dataGridViewPlaylistItems.Location = new Point(12, 36);
            dataGridViewPlaylistItems.Name = "dataGridViewPlaylistItems";
            dataGridViewPlaylistItems.ReadOnly = true;
            dataGridViewPlaylistItems.RowHeadersVisible = false;
            dataGridViewPlaylistItems.RowHeadersWidth = 62;
            dataGridViewPlaylistItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewPlaylistItems.Size = new Size(769, 361);
            dataGridViewPlaylistItems.TabIndex = 0;
            dataGridViewPlaylistItems.CellClick += dataGridViewPlaylistItems_CellClick;
            // 
            // lblItemCount
            // 
            lblItemCount.Dock = DockStyle.Bottom;
            lblItemCount.Location = new Point(12, 397);
            lblItemCount.Name = "lblItemCount";
            lblItemCount.Size = new Size(769, 25);
            lblItemCount.TabIndex = 1;
            lblItemCount.Text = "Select a playlist on the left, or create a new one.";
            lblItemCount.TextAlign = ContentAlignment.MiddleRight;



            // 
            // lblTVLineCount
            // 
            lblTVLineCount.AutoSize = true;
            lblTVLineCount.Location = new Point(300, 142);
            lblTVLineCount.Name = "lblTVLineCount";
            lblTVLineCount.Size = new Size(110, 25);
            lblTVLineCount.TabIndex = 13;
            lblTVLineCount.Text = "No. of Lines:";

            // 
            // cmbTVLineCount
            // 
            cmbTVLineCount.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTVLineCount.Items.AddRange(new object[] {"4","5","6","7","8"});
            cmbTVLineCount.Location = new Point(420, 137);
            cmbTVLineCount.Name = "cmbTVLineCount";
            cmbTVLineCount.Size = new Size(80, 33);
            cmbTVLineCount.TabIndex = 14;
            // 
            // gbAvailableItems
            // 
            gbAvailableItems.Controls.Add(dataGridViewAvailableItems);
            gbAvailableItems.Controls.Add(lblAvailableCount);
            gbAvailableItems.Dock = DockStyle.Bottom;
            gbAvailableItems.Location = new Point(0, 434);
            gbAvailableItems.Name = "gbAvailableItems";
            gbAvailableItems.Padding = new Padding(12);
            gbAvailableItems.Size = new Size(793, 435);
            gbAvailableItems.TabIndex = 1;
            gbAvailableItems.TabStop = false;
            gbAvailableItems.Text = "Available Items (not yet in this playlist)";
            // 
            // colAvailSrNo .. colAvailPreview
            // 
            colAvailSrNo.Name = "SrNo";
            colAvailSrNo.HeaderText = "Sr. No.";
            colAvailSrNo.DataPropertyName = "SrNo";
            colAvailSrNo.Width = 70;
            colAvailSrNo.ReadOnly = true;

            colAvailType.Name = "AdvType";
            colAvailType.HeaderText = "Adv Type";
            colAvailType.DataPropertyName = "AdvType";
            colAvailType.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colAvailType.ReadOnly = true;

            colAvailDuration.Name = "PlayDuration";
            colAvailDuration.HeaderText = "Play Duration";
            colAvailDuration.DataPropertyName = "PlayDuration";
            colAvailDuration.Width = 110;
            colAvailDuration.ReadOnly = true;
            colAvailDuration.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            colAvailCount.Name = "PlayTimes";
            colAvailCount.HeaderText = "Play Count";
            colAvailCount.DataPropertyName = "PlayTimes";
            colAvailCount.Width = 100;
            colAvailCount.ReadOnly = true;
            colAvailCount.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            colAvailID.Name = "ID";
            colAvailID.DataPropertyName = "ID";
            colAvailID.Visible = false;

            colAvailFileName.Name = "FileName";
            colAvailFileName.DataPropertyName = "FileName";
            colAvailFileName.Visible = false;

            colAvailFilePath.Name = "FilePath";
            colAvailFilePath.DataPropertyName = "FilePath";
            colAvailFilePath.Visible = false;

            colAvailMessageDetails.Name = "MessageDetails";
            colAvailMessageDetails.DataPropertyName = "MessageDetails";
            colAvailMessageDetails.Visible = false;

            colAvailAdd.Name = "Add";
            colAvailAdd.HeaderText = "Add";
            colAvailAdd.Text = "➕";
            colAvailAdd.UseColumnTextForButtonValue = true;
            colAvailAdd.Width = 60;

            colAvailPreview.Name = "Preview";
            colAvailPreview.HeaderText = "Preview";
            colAvailPreview.Text = "👁";
            colAvailPreview.UseColumnTextForButtonValue = true;
            colAvailPreview.Width = 60;
            // 
            // dataGridViewAvailableItems
            // 
            dataGridViewAvailableItems.AllowUserToAddRows = false;
            dataGridViewAvailableItems.AllowUserToDeleteRows = false;
            dataGridViewAvailableItems.AutoGenerateColumns = false;
            dataGridViewAvailableItems.BackgroundColor = Color.Beige;
            dataGridViewAvailableItems.ColumnHeadersHeight = 34;
            dataGridViewAvailableItems.Columns.AddRange(new DataGridViewColumn[] {
                colAvailSrNo, colAvailType, colAvailDuration, colAvailCount, colAvailID,
                colAvailFileName, colAvailFilePath, colAvailMessageDetails, colAvailAdd, colAvailPreview });
            dataGridViewAvailableItems.Dock = DockStyle.Fill;
            dataGridViewAvailableItems.Location = new Point(12, 36);
            dataGridViewAvailableItems.Name = "dataGridViewAvailableItems";
            dataGridViewAvailableItems.ReadOnly = true;
            dataGridViewAvailableItems.RowHeadersVisible = false;
            dataGridViewAvailableItems.RowHeadersWidth = 62;
            dataGridViewAvailableItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewAvailableItems.Size = new Size(769, 362);
            dataGridViewAvailableItems.TabIndex = 0;
            dataGridViewAvailableItems.CellClick += dataGridViewAvailableItems_CellClick;
            // 
            // lblAvailableCount
            // 
            lblAvailableCount.Dock = DockStyle.Bottom;
            lblAvailableCount.Location = new Point(12, 398);
            lblAvailableCount.Name = "lblAvailableCount";
            lblAvailableCount.Size = new Size(769, 25);
            lblAvailableCount.TabIndex = 1;
            lblAvailableCount.Text = "Available: 0";
            lblAvailableCount.TextAlign = ContentAlignment.MiddleRight;
            // 
            // ucPlaylistManager
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tableLayoutPanelMain);
            Name = "ucPlaylistManager";
            Size = new Size(1500, 875);
            tableLayoutPanelMain.ResumeLayout(false);
            splitContainerMain.Panel1.ResumeLayout(false);
            splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).EndInit();
            splitContainerMain.ResumeLayout(false);
            gbPlaylists.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewPlaylists).EndInit();
            gbPlaylistDetails.ResumeLayout(false);
            gbPlaylistDetails.PerformLayout();
            gbPlaylistItems.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewPlaylistItems).EndInit();
            gbAvailableItems.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewAvailableItems).EndInit();
            ResumeLayout(false);
        }
    }
}
