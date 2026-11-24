using DataBaseHandler;
using DataModel;
using Global_Data;
using Helper;
using LogManager;
using System.Collections;


namespace IPIS
{
    public partial class ucCGDBBoardSetting : UserControl
    {
        public event EventHandler<eventInfo> OnEvent;
        private int _boardid = 0;
        private List<Plateforms> lstPlateforms = new List<Plateforms>();
        private List<displayEffects> lstdisplayEffects = new List<displayEffects>();
        private List<letterGap> lstletterGap = new List<letterGap>();
        private List<ScrollingSpeed> lstscrollingSpeed = new List<ScrollingSpeed>();
        private List<FontSize> lstfontsize = new List<FontSize>();
        private List<VideoType> lstVideoType = new List<VideoType>();
        public ucCGDBBoardSetting()
        {
            this.SuspendLayout();
            InitializeComponent();
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
            this.ResumeLayout(false);
        }
        private IEnumerable<Control> GetAllControls(Control c)
        {
            foreach (Control child in c.Controls)
            {
                foreach (var gc in GetAllControls(child))
                    yield return gc;

                yield return child;
            }
        }

        private void LoadConfig(int boardId)
        {
            try
            {
                lblboardHeader.Text = Appdata.currentnodename;
                flow_layout_language.Controls.Clear();
                flow_layoutpanel.AutoScroll = true;
                flow_layoutpanel.FlowDirection = FlowDirection.LeftToRight;
                flow_layoutpanel.WrapContents = true;
                string[] langs = GlobalData._lstAdminSetting.STATION_LANGUAGES.Split('+');
                foreach (var lang in langs)
                {
                    CheckBox chk = new CheckBox();
                    chk.Text = lang;
                    chk.Name = "chk_" + lang;
                    chk.AutoSize = true;
                    flow_layout_language.Controls.Add(chk);
                }

                for (int coach = 1; coach <= GlobalData._lstAdminSetting.Total_Coach; coach++)
                    cmbNoCoach.Items.Add(coach);

                lstPlateforms = _dbService.GET_PLATEFORMS(0);
                var filteredPlatforms = lstPlateforms.OrderBy(x => x.PF_ID)
                                        .Take(GlobalData._lstAdminSetting.TOTAL_PLATFORM_DISPLAYED)
                                        .ToList();
                foreach (var pf in filteredPlatforms)
                {
                    if (pf.PF_NUMBER != null && (pf.PF_NUMBER.Trim() == "-" || pf.PF_NUMBER.Trim() == "--"))
                    {
                        pf.PF_NUMBER = "ALL";
                    }
                }
                cmb_plateformNo.DataSource = filteredPlatforms;
                cmb_plateformNo.DisplayMember = "PF_NUMBER";
                cmb_plateformNo.ValueMember = "PF_ID";
                cmb_plateformNo.SelectedIndex = 0;

                lstdisplayEffects = _dbService.GET_DisplayEffect(0);
                cmbEffect.DataSource = null;
                cmbEffect.DisplayMember = "EFFECT_NAME";
                cmbEffect.ValueMember = "EFFECT_ID";
                cmbEffect.DataSource = lstdisplayEffects;

                //lstletterGap = _dbService.GET_letterGap(0);
                //cmbLetterGap.DataSource = null;
                //cmbLetterGap.DisplayMember = "GAP_NAME";
                //cmbLetterGap.ValueMember = "GAP_ID";
                //cmbLetterGap.DataSource = lstletterGap;

                lstscrollingSpeed = _dbService.GET_ScrollingSpeed(0);
                cmbScrollSpeed.DataSource = null;
                cmbScrollSpeed.DisplayMember = "SCROLLING_TEXT";
                cmbScrollSpeed.ValueMember = "ID";
                cmbScrollSpeed.DataSource = lstscrollingSpeed;

                lstfontsize = _dbService.GET_FontSize(0);
                cmbFontSize.DataSource = null;
                cmbFontSize.DisplayMember = "SIZE";
                cmbFontSize.ValueMember = "SIZE_ID";
                cmbFontSize.DataSource = lstfontsize;

                //lstVideoType = _dbService.GET_VideoType(0);
                //cmbVideoType.DataSource = null;
                //cmbVideoType.DisplayMember = "VIDEO_TYPE";
                //cmbVideoType.ValueMember = "ID";
                //cmbVideoType.DataSource = lstVideoType;

                cmb_plateformNo.SelectedIndex = 0;
                cmbNoCoach.SelectedIndex = 0;
                cmbFontSize.SelectedIndex = 0;
                cmbBoardSize.SelectedIndex = 0;
                cmbDelayTime.SelectedIndex = 0;
                //cmbVideoType.SelectedIndex = 0;
                cmbScrollSpeed.SelectedIndex = 0;
                //cmbLetterGap.SelectedIndex = 0;
                cmbEffect.SelectedIndex = 0;
                //cmbDisplayinfo.SelectedIndex = 0;   // Info Displayed removed from the screen (it was never saved)
                cmbDTO.SelectedItem = "60";   // DTO default: 60 seconds
                cmbPF_END.SelectedIndex = 0;
                if (cmbIntensity.Items.Count > 0)
                    cmbIntensity.SelectedIndex = 0;

                this.BeginInvoke(new Action(() =>
                {
                    foreach (var combo in GetAllControls(this).OfType<ComboBox>())
                    {
                        combo.DropDownStyle = ComboBoxStyle.DropDownList;
                    }
                }));

                var boardList = _dbService.GET_BOARD_SETTING(0);
                if (boardList != null)
                {
                    List<BoardSettingInfo> _data;
                    if (Appdata.IsParent)
                        _data = boardList.Where(x => x.NODE_ID == Appdata.currentnodeid).ToList();
                    else
                        _data = boardList.Where(x => x.NODE_ID == Appdata.currentnodeid && x.HUB_ID == Appdata.parentNodeID).ToList();
                    if (_data.Count() > 0)
                    {
                        _boardid = _data.FirstOrDefault().BOARD_ID;
                        if (_data.FirstOrDefault().IsActive == 1)
                            chkIsActive.Checked = true;
                        else
                            chkIsActive.Checked = false;
                        cmbNoCoach.SelectedItem = Convert.ToInt32(_data.FirstOrDefault().BOARD_LINE);
                        txtLocation.Text = _data.FirstOrDefault().BOARD_LOCATION;
                        cmbFontSize.SelectedValue = Convert.ToInt32(_data.FirstOrDefault().FONT_SIZE.ToString());
                        cmbBoardSize.Text = _data.FirstOrDefault().BOARD_SIZE;
                        cmb_plateformNo.SelectedIndex = Convert.ToInt32(_data.FirstOrDefault().PF_NO.ToString());
                        cmbDelayTime.Text = _data.FirstOrDefault().DELAYTIME.ToString();
                        //cmbVideoType.SelectedValue = Convert.ToInt32(_data.FirstOrDefault().VIDEO_TYPE);
                        cmbScrollSpeed.SelectedValue = Convert.ToInt32(_data.FirstOrDefault().SCROLLING_SPEED);
                        //cmbLetterGap.SelectedValue = Convert.ToInt32(_data.FirstOrDefault().LETTER_GAP);
                        cmbEffect.SelectedValue = Convert.ToInt32(_data.FirstOrDefault().DISPLAY_EFFECT);
                        //cmbDisplayinfo.Text = _data.FirstOrDefault().DISPLAY_TYPE;   // Info Displayed removed from the screen (it was never saved)
                        cmbPF_END.Text = _data.FirstOrDefault().DISPLAY_TYPE;
                        //txtdefaultdata.Text = _data.FirstOrDefault().PLATEFORM_DATA;
                        cmbIntensity.Text = _data.FirstOrDefault().Intensity;
                        checkBoxSoftReset.Checked = _data.FirstOrDefault().RST_Soft_Reset;
                        string[] selectedpf = _data.FirstOrDefault().SELECTED_PF.Split('+');

                        string[] languages = _data.FirstOrDefault().LANGUAGES.Split('+');
                        foreach (Control c in flow_layout_language.Controls)
                        {
                            if (c is CheckBox chk)
                            {
                                if (languages.Contains(chk.Text))
                                    chk.Checked = true;
                                else
                                    chk.Checked = false;
                            }
                        }

                        ArrayList savedPots = new ArrayList();
                        savedPots.AddRange(_data.FirstOrDefault().SELECTED_PF.Split(','));
                        int counter = 0;
                        foreach (GroupBox grp in flow_layoutpanel.Controls.OfType<GroupBox>())
                        {
                            int coachNo = Convert.ToInt32(grp.Tag);

                            string savedPortNo = savedPots[counter].ToString().Split('+').FirstOrDefault();
                            string savedCoachNo = savedPots[counter].ToString().Split('+').LastOrDefault();

                            var textBoxes = grp.Controls
                                .OfType<TableLayoutPanel>()
                                .SelectMany(p => p.Controls.OfType<TextBox>())
                                .ToList();

                            TextBox txtPort = textBoxes.FirstOrDefault(t => t.Name.StartsWith("txtPort"));
                            TextBox txtCoach = textBoxes.FirstOrDefault(t => t.Name.StartsWith("txtCoach"));

                            if (txtPort != null)
                                txtPort.Text = savedPortNo;

                            if (txtCoach != null)
                                txtCoach.Text = savedCoachNo;
                            counter++;
                        }
                    }
                }

                // Auto intensity (admin setting) owns this value; Dashboard re-applies it every minute.
                cmbIntensity.Enabled = GlobalData._lstAdminSetting == null || GlobalData._lstAdminSetting.AUTO_INTENSITY != 1;
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
        }

        private void ucCGDBBoardSetting_Load(object sender, EventArgs e)
        {
            LoadConfig(0);
        }
        private void cmbNoCoach_SelectedIndexChanged(object sender, EventArgs e)
        {
            flow_layoutpanel.Controls.Clear();

            if (!int.TryParse(cmbNoCoach.Text, out int totalCoach))
                return;

            for (int coach = 1; coach <= totalCoach; coach++)
            {
                GroupBox grpCoach = new GroupBox
                {
                    Text = $"C{coach}",
                    Width = 135,
                    Height = 90,
                    Margin = new Padding(4)
                };


                TableLayoutPanel table = new TableLayoutPanel
                {
                    RowCount = 2,
                    ColumnCount = 2,
                    Dock = DockStyle.Fill,
                };

                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

                Label lblPort = new Label
                {
                    Text = "Port",
                    TextAlign = ContentAlignment.MiddleLeft,
                    Dock = DockStyle.Fill
                };

                Label lblCoachId = new Label
                {
                    Text = "Coach",
                    TextAlign = ContentAlignment.MiddleLeft,
                    Dock = DockStyle.Fill
                };

                TextBox txtPort = new TextBox
                {
                    Name = $"txtPort_{coach}",
                    Dock = DockStyle.Fill,
                    Text = 2.ToString()
                };

                TextBox txtCoach = new TextBox
                {
                    Name = $"txtCoach_{coach}",
                    Dock = DockStyle.Fill,
                    Text = (coach + 1).ToString()
                };

                table.Controls.Add(lblPort, 0, 0);
                table.Controls.Add(txtPort, 1, 0);
                table.Controls.Add(lblCoachId, 0, 1);
                table.Controls.Add(txtCoach, 1, 1);

                grpCoach.Controls.Add(table);
                flow_layoutpanel.Controls.Add(grpCoach);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedLangs = flow_layout_language.Controls
                                    .OfType<CheckBox>()
                                    .Where(c => c.Checked)
                                    .Select(c => c.Text)
                                    .ToList();

                string _allports = string.Empty;
                foreach (GroupBox grp in flow_layoutpanel.Controls.OfType<GroupBox>())
                {
                    string coachLabel = grp.Text;

                    TextBox txtPort = grp.Controls
                        .OfType<TableLayoutPanel>()
                        .SelectMany(p => p.Controls.OfType<TextBox>())
                        .FirstOrDefault(t => t.Name.StartsWith("txtPort"));

                    TextBox txtCoach = grp.Controls
                        .OfType<TableLayoutPanel>()
                        .SelectMany(p => p.Controls.OfType<TextBox>())
                        .FirstOrDefault(t => t.Name.StartsWith("txtCoach"));

                    int portNo = int.TryParse(txtPort?.Text, out int p) ? p : 0;
                    int coachId = int.TryParse(txtCoach?.Text, out int c) ? c : 0;
                    _allports += portNo + "+" + coachId + ",";
                }
                _allports = _allports.Trim(',');

                if (string.IsNullOrEmpty(Appdata.boardName))
                    Appdata.boardName = Appdata.currentnodename;

                if (selectedLangs.Count == 0)
                {
                    MessageBox.Show("Please select at least one language!!", "vardaan electronics", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string finalLang = string.Join("+", selectedLangs);

                // Platform this board had before this save (null for a new board).
                string previousPf = GetStoredBoardPf(_boardid);

                BoardSetting _model = new BoardSetting
                {
                    HubId = Appdata.parentNodeID,
                    NodeID = Appdata.childNodeID,
                    BoardName = Appdata.boardName,
                    BoardId = _boardid,
                    BoardLine = Convert.ToInt32(cmbNoCoach.Text),
                    BoardLocation = txtLocation.Text,
                    PfNo = Convert.ToInt32(cmb_plateformNo.SelectedIndex),
                    SELECTED_PF = _allports,
                    //Plateformdata = txtdefaultdata.Text,
                    Languages = finalLang,
                    BoardMessage = "",
                    DisplayEffect = Convert.ToString(cmbEffect.SelectedValue),
                    FontSize = Convert.ToInt32(cmbFontSize.SelectedValue),
                    BoardSize = cmbBoardSize.Text,
                    delayedTime = Convert.ToInt32(cmbDelayTime.Text),
                    //VideoType = Convert.ToString(cmbVideoType.SelectedValue),
                    ScrollingSpeed = Convert.ToString(cmbScrollSpeed.SelectedValue),
                    //letterGap = Convert.ToInt32(cmbLetterGap.SelectedValue),
                    DisplayType = cmbPF_END.Text,
                    IsActive = chkIsActive.Checked ? 1 : 0,
                    Intensity = Convert.ToInt32(cmbIntensity.Text),
                    RST_Soft_Reset = checkBoxSoftReset.Checked,
                    DTO = cmbDTO.Text
                };
                string result = _dbService.SaveOrUpdateBoardSetting(_model);
                if (!string.IsNullOrEmpty(result))
                {
       
                    BoardSettingInfo savedBoard = FindBoardRecord(_boardid);
                    int coachPfNo = PlatformNumberAt(_model.PfNo);
                    if (!_dbService.SyncCgdbCoachConfiguration(coachPfNo, _allports, ParsePf(previousPf),
                            savedBoard?.BOARD_ID, savedBoard?.HUB_ID))
                    {
                        MessageBox.Show("Board saved, but the coach configuration could not be updated. Please save again.",
                            "vardaan electronics", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    MessageBox.Show(result, "vardaan electronics", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    OnEvent?.Invoke(this, new eventInfo("HubConfigSaved", result, this.Name, true));
                }
                else
                    MessageBox.Show("Operation failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
        }


        private static string GetStoredBoardPf(int boardId)
        {
            if (boardId <= 0) return null;

            try
            {
                var board = _dbService.GET_BOARD_SETTING(0)?.FirstOrDefault(b => b.BOARD_ID == boardId);
                return board == null ? null : (board.PF_NO ?? string.Empty).Trim();
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
                return null;
            }
        }

        private static int? ParsePf(string pf)
        {
            return int.TryParse(pf, out int value) ? value : (int?)null;
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show($"Are you sure you want to delete?",
                        "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                string deletedBoardPf = GetStoredBoardPf(Appdata.BOARD_ID);
                string screenBoardPf = GetStoredBoardPf(_boardid);
                int screenPfNo = cmb_plateformNo.SelectedIndex;

                string query = $"DELETE FROM BOARDSETTING WHERE BOARD_ID ={Appdata.BOARD_ID}";
                string resultt = _dbService.QuerySingleOrDefault(query);

           
                RemoveCoachConfigurationOfDeletedBoard(deletedBoardPf, screenBoardPf, screenPfNo);

                OnEvent?.Invoke(this, new eventInfo("HubConfigSaved", resultt, this.Name, true));
            }
        }

        private void RemoveCoachConfigurationOfDeletedBoard(string deletedBoardPf, string screenBoardPf, int screenPfNo)
        {
            try
            {
                bool goneByTree = deletedBoardPf != null && Appdata.BOARD_ID > 0 && GetStoredBoardPf(Appdata.BOARD_ID) == null;
                bool goneByScreen = screenBoardPf != null && _boardid > 0 && GetStoredBoardPf(_boardid) == null;
                int deletedBoardId = goneByTree ? Appdata.BOARD_ID : _boardid;

                // PF values (platform combo index) the board was saved with: BOARDSETTING value and the screen value.
                // Old coach rows (BOARD_ID empty) were saved with these, new rows are found by BOARD_ID.
                var pfList = new List<int>();
                int? storedPf = ParsePf(goneByTree ? deletedBoardPf : screenBoardPf);
                if (storedPf.HasValue) pfList.Add(storedPf.Value);
                if (screenPfNo >= 0 && !pfList.Contains(screenPfNo)) pfList.Add(screenPfNo);

                LoggerService.Logger.Information(
                    "CGDB delete: tree BOARD_ID {TreeId} PF '{TreePf}' gone {GoneTree}; screen BOARD_ID {ScreenId} PF '{ScreenPf}' gone {GoneScreen}; screen PF index {ScreenPfNo}; coach PFs to remove: {Pfs}.",
                    Appdata.BOARD_ID, deletedBoardPf, goneByTree, _boardid, screenBoardPf, goneByScreen, screenPfNo, string.Join(",", pfList));

                if (!(goneByTree || goneByScreen) || pfList.Count == 0)
                    return;

                foreach (int pf in pfList)
                {
                    if (!_dbService.DeleteCgdbCoachConfiguration(PlatformNumberAt(pf), pf, deletedBoardId))
                    {
                        MessageBox.Show("Board deleted, but its coach configuration could not be removed.",
                            "vardaan electronics", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
        }


        private int PlatformNumberAt(int index)
        {
            if (cmb_plateformNo.DataSource is IList<Plateforms> platforms
                && index >= 0 && index < platforms.Count
                && int.TryParse(platforms[index].PF_NUMBER?.Trim(), out int number))
            {
                return number;
            }
            return index;
        }


        private static BoardSettingInfo FindBoardRecord(int boardId)
        {
            try
            {
                var boards = _dbService.GET_BOARD_SETTING(0) ?? new List<BoardSettingInfo>();

                BoardSettingInfo board = boardId > 0 ? boards.FirstOrDefault(b => b.BOARD_ID == boardId) : null;
                if (board != null) return board;

                return Appdata.IsParent
                    ? boards.FirstOrDefault(x => x.NODE_ID == Appdata.currentnodeid)
                    : boards.FirstOrDefault(x => x.NODE_ID == Appdata.currentnodeid && x.HUB_ID == Appdata.parentNodeID);
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
                return null;
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            OnEvent?.Invoke(this, new eventInfo("HubConfigSaved", string.Empty, this.Name, true));
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            LoadConfig(0);
        }
    }
}

