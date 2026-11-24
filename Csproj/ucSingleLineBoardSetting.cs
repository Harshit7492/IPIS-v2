

using DataBaseHandler;
using DataModel;
using Global_Data;
using Helper;
using LogManager;

namespace IPIS
{
    public partial class ucSingleLineBoardSetting : UserControl
    {
        public event EventHandler<eventInfo> OnEvent;
        private int _boardid = 0;
        private List<Plateforms> lstPlateforms = new List<Plateforms>();
        private List<displayEffects> lstdisplayEffects = new List<displayEffects>();
        private List<letterGap> lstletterGap = new List<letterGap>();
        private List<ScrollingSpeed> lstscrollingSpeed = new List<ScrollingSpeed>();
        private List<FontSize> lstfontsize = new List<FontSize>();
        private List<VideoType> lstVideoType = new List<VideoType>();

        public ucSingleLineBoardSetting()
        {
            InitializeComponent();
        }

        private void ucSingleLineBoardSetting_Load(object sender, EventArgs e)
        {
            LoadConfig(0);
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

        private string _currentBoardName = string.Empty;

        /// <summary>
        /// True when the current board is a Video Display Board ("VDB")
        /// or Advance Video Display Board ("AVDB").
        /// </summary>
        private bool IsVideoDisplayBoard
        {
            get
            {
                return IsAdvanceVideoDisplayBoard || CheckIsVDB();
            }
        }

        /// <summary>
        /// True only for Advance Video Display Board ("AVDB").
        /// AVDB shows Module 1 only; VDB shows Module 1, 2 and 3.
        /// </summary>
        private bool IsAdvanceVideoDisplayBoard
        {
            get
            {
                return CheckIsAVDB();
            }
        }

        private bool CheckIsAVDB()
        {
            return IsMatch(Appdata.boardtype, "AVDB", "Advance Video Display Board")
                || IsMatch(Appdata.currentnodename, "AVDB", "Advance Video Display Board")
                || IsMatch(Appdata.boardName, "AVDB", "Advance Video Display Board")
                || IsMatch(lblboardHeader?.Text, "AVDB", "Advance Video Display Board")
                || IsMatch(_currentBoardName, "AVDB", "Advance Video Display Board");
        }

        private bool CheckIsVDB()
        {
            return IsMatch(Appdata.boardtype, "VDB", "Video Display Board")
                || IsMatch(Appdata.currentnodename, "VDB", "Video Display Board")
                || IsMatch(Appdata.boardName, "VDB", "Video Display Board")
                || IsMatch(lblboardHeader?.Text, "VDB", "Video Display Board")
                || IsMatch(_currentBoardName, "VDB", "Video Display Board");
        }

        private static bool IsMatch(string value, string shortCode, string fullName)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            string trimmed = value.Trim();
            if (string.Equals(trimmed, shortCode, StringComparison.OrdinalIgnoreCase)) return true;
            if (string.Equals(trimmed, fullName, StringComparison.OrdinalIgnoreCase)) return true;
            if (shortCode == "VDB" && trimmed.IndexOf("Advance", StringComparison.OrdinalIgnoreCase) >= 0)
                return false;
            if (trimmed.IndexOf(fullName, StringComparison.OrdinalIgnoreCase) >= 0) return true;
            if (trimmed.IndexOf(shortCode, StringComparison.OrdinalIgnoreCase) >= 0) return true;
            return false;
        }

        /// <summary>
        /// Module visibility rules:
        ///  - VDB  : Module 1, Module 2, Module 3 visible
        ///  - AVDB : Module 1 only
        ///  - all others : none
        /// </summary>
        private void ToggleModuleControls()
        {
            bool isVDB = !IsAdvanceVideoDisplayBoard && IsVideoDisplayBoard; // VDB only
            bool isAVDB = IsAdvanceVideoDisplayBoard;

            // Module 1 visible for both VDB and AVDB
            lblModule1.Visible = txtModule1.Visible = (isVDB || isAVDB);

            // Modules 2 and 3 visible only for VDB
            lblModule2.Visible = txtModule2.Visible = isVDB;
            lblModule3.Visible = txtModule3.Visible = isVDB;

            // Dynamically collapse unused module rows so they do not take empty space
            if (tableLayoutPanel4 != null && tableLayoutPanel4.RowStyles.Count >= 10)
            {
                for (int i = 0; i <= 6; i++)
                {
                    if (tableLayoutPanel4.RowStyles.Count > i)
                        tableLayoutPanel4.RowStyles[i] = new RowStyle(SizeType.Absolute, 42F);
                }

                if (isVDB)
                {
                    tableLayoutPanel4.RowStyles[7] = new RowStyle(SizeType.Absolute, 42F);
                    tableLayoutPanel4.RowStyles[8] = new RowStyle(SizeType.Absolute, 42F);
                    tableLayoutPanel4.RowStyles[9] = new RowStyle(SizeType.Absolute, 42F);
                }
                else if (isAVDB)
                {
                    tableLayoutPanel4.RowStyles[7] = new RowStyle(SizeType.Absolute, 42F);
                    tableLayoutPanel4.RowStyles[8] = new RowStyle(SizeType.Absolute, 0F);
                    tableLayoutPanel4.RowStyles[9] = new RowStyle(SizeType.Absolute, 0F);
                }
                else
                {
                    tableLayoutPanel4.RowStyles[7] = new RowStyle(SizeType.Absolute, 0F);
                    tableLayoutPanel4.RowStyles[8] = new RowStyle(SizeType.Absolute, 0F);
                    tableLayoutPanel4.RowStyles[9] = new RowStyle(SizeType.Absolute, 0F);
                }

                if (tableLayoutPanel4.RowStyles.Count >= 11)
                {
                    tableLayoutPanel4.RowStyles[10] = new RowStyle(SizeType.Percent, 100F);
                }
            }
        }

        private void LoadConfig(int boardId)
        {
            try
            {
                lblboardHeader.Text = Appdata.currentnodename;
                cmbNoOfLine.SelectedIndex = 0;
                if (Appdata.currentnodename == "Single Line/Platform Board")
                    cmbNoOfLine.Enabled = false;
                else
                    cmbNoOfLine.Enabled = true;

                flow_layout_language.Controls.Clear();
                string[] langs = GlobalData._lstAdminSetting.STATION_LANGUAGES.Split('+');
                foreach (var lang in langs)
                {
                    CheckBox chk = new CheckBox();
                    chk.Text = lang;
                    chk.Name = "chk_" + lang;
                    chk.AutoSize = true;

                    flow_layout_language.Controls.Add(chk);
                }

                flow_layout_Plateform.Controls.Clear();
                lstPlateforms = _dbService.GET_PLATEFORMS(0);
                foreach (var item in lstPlateforms)
                {
                    if (flow_layout_Plateform.Controls.Count < GlobalData._lstAdminSetting.TOTAL_PLATFORM_DISPLAYED)
                    {
                        CheckBox chk = new CheckBox();
                        string pfText = item.PF_NUMBER?.ToString()?.Trim() ?? string.Empty;
                        chk.Text = (pfText == "-" || pfText == "--") ? "ALL" : pfText;
                        chk.Name = "chk_" + item.PF_NUMBER;
                        chk.AutoSize = true;
                        chk.Tag = item.PF_ID;
                        flow_layout_Plateform.Controls.Add(chk);
                    }
                }

                lstdisplayEffects = _dbService.GET_DisplayEffect(0);
                cmbEffect.DataSource = null;
                cmbEffect.DisplayMember = "EFFECT_NAME";
                cmbEffect.ValueMember = "EFFECT_ID";
                cmbEffect.DataSource = lstdisplayEffects;

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

                cmbFontSize.SelectedIndex = 0;
                cmbBoardSize.SelectedIndex = 0;
                cmbDelayTime.SelectedIndex = 0;
                cmbScrollSpeed.SelectedIndex = 0;
                cmbEffect.SelectedIndex = 0;
                cmbDisplayinfo.SelectedIndex = 0;
                cmbDTO.SelectedItem = "60";
                if (cmbIntensity.Items.Count > 0)
                    cmbIntensity.SelectedIndex = 0;

                this.BeginInvoke(new Action(() =>
                {
                    txtIpConfig.Focus();
                    foreach (var combo in GetAllControls(this).OfType<ComboBox>())
                    {
                        combo.DropDownStyle = ComboBoxStyle.DropDownList;
                    }
                }));

                var boardList = _dbService.GET_BOARD_SETTING(0);
                if (boardList != null)
                {
                    List<BoardSettingInfo> _data = null;
                    if (Appdata.BOARD_ID > 0)
                    {
                        var boardById = boardList.FirstOrDefault(x => x.BOARD_ID == Appdata.BOARD_ID);
                        if (boardById != null)
                            _data = new List<BoardSettingInfo> { boardById };
                    }
                    if (_data == null)
                    {
                        if (Appdata.IsParent)
                            _data = boardList.Where(x => x.NODE_ID == Appdata.currentnodeid).ToList();
                        else
                            _data = boardList.Where(x => x.NODE_ID == Appdata.currentnodeid && x.HUB_ID == Appdata.parentNodeID).ToList();
                    }

                    txtModule1.Text = string.Empty;
                    txtModule2.Text = string.Empty;
                    txtModule3.Text = string.Empty;

                    if (_data.Count() > 0)
                    {
                        _currentBoardName = _data.FirstOrDefault().BOARD_NAME ?? string.Empty;
                        _boardid = _data.FirstOrDefault().BOARD_ID;
                        if (_data.FirstOrDefault().IsActive == 1)
                            chkIsActive.Checked = true;
                        else
                            chkIsActive.Checked = false;
                        txtIpConfig.Text = _data.FirstOrDefault().BOARD_IP;
                        txtLocation.Text = _data.FirstOrDefault().BOARD_LOCATION;
                        cmbFontSize.SelectedValue = Convert.ToInt32(_data.FirstOrDefault().FONT_SIZE.ToString());
                        cmbBoardSize.Text = _data.FirstOrDefault().BOARD_SIZE;
                        cmbDelayTime.Text = _data.FirstOrDefault().DELAYTIME.ToString();
                        cmbScrollSpeed.SelectedValue = Convert.ToInt32(_data.FirstOrDefault().SCROLLING_SPEED);
                        cmbEffect.SelectedValue = Convert.ToInt32(_data.FirstOrDefault().DISPLAY_EFFECT);
                        cmbDisplayinfo.Text = _data.FirstOrDefault().DISPLAY_TYPE;
                        cmbNoOfLine.Text = _data.FirstOrDefault().BOARD_LINE.ToString();
                        cmbIntensity.Text = _data.FirstOrDefault().Intensity;
                        checkBoxSoftReset.Checked = _data.FirstOrDefault().RST_Soft_Reset;
                        cmbDTO.Text = _data.FirstOrDefault().DTO;

                        var selectedpf = new HashSet<string>(_data.FirstOrDefault().SELECTED_PF.Split('+'));
                        foreach (CheckBox chk in flow_layout_Plateform.Controls.OfType<CheckBox>())
                            chk.Checked = selectedpf.Contains(chk.Tag.ToString());

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

                        // Load module values by splitting the comma-separated string
                        if (IsVideoDisplayBoard)
                        {
                            string rawIps = _data.FirstOrDefault().MODULE_IPS ?? string.Empty;
                            string[] ips = rawIps.Split(new[] { ',' }, StringSplitOptions.None);

                            txtModule1.Text = ips.Length > 0 ? ips[0].Trim() : string.Empty;

                            if (!IsAdvanceVideoDisplayBoard)
                            {
                                txtModule2.Text = ips.Length > 1 ? ips[1].Trim() : string.Empty;
                                txtModule3.Text = ips.Length > 2 ? ips[2].Trim() : string.Empty;
                            }
                        }
                    }
                }

                // show/hide module controls based on board type
                ToggleModuleControls();

                // Auto intensity (admin setting) owns this value; Dashboard re-applies it every minute.
                cmbIntensity.Enabled = GlobalData._lstAdminSetting == null || GlobalData._lstAdminSetting.AUTO_INTENSITY != 1;
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            bool state = chkSelectAll.Checked;
            foreach (Control ctrl in flow_layout_Plateform.Controls)
            {
                if (ctrl is CheckBox cb)
                {
                    cb.Checked = state;
                }
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

                var selectedplateform = flow_layout_Plateform.Controls
                                    .OfType<CheckBox>()
                                    .Where(c => c.Checked)
                                    .Select(c => Convert.ToInt32(c.Tag))
                                    .ToList();

                if (string.IsNullOrEmpty(Appdata.boardName))
                    Appdata.boardName = !string.IsNullOrEmpty(Appdata.currentnodename) ? Appdata.currentnodename : _currentBoardName;

                if (string.IsNullOrEmpty(txtIpConfig.Text))
                {
                    txtIpConfig.Focus();
                    MessageBox.Show("Please input board IP!!", "vardaan electronics", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (string.IsNullOrEmpty(txtLocation.Text))
                {
                    txtLocation.Focus();
                    MessageBox.Show("Please input installed location!!", "vardaan electronics", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (selectedLangs.Count == 0)
                {
                    MessageBox.Show("Please select at least one language!!", "vardaan electronics", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (selectedplateform.Count == 0)
                {
                    MessageBox.Show("Please select at least one plateform!!", "vardaan electronics", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (selectedplateform.Count > 1)
                {
                    MessageBox.Show("Please select only one plateform!!", "vardaan electronics",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // module validation
                if (IsVideoDisplayBoard)
                {
                    if (string.IsNullOrWhiteSpace(txtModule1.Text))
                    {
                        MessageBox.Show("Please enter Module 1 IP!!",
                            "vardaan electronics", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!IsAdvanceVideoDisplayBoard)
                    {
                        if (string.IsNullOrWhiteSpace(txtModule2.Text) ||
                            string.IsNullOrWhiteSpace(txtModule3.Text))
                        {
                            MessageBox.Show("Please enter Module 2 and Module 3 IP!!",
                                "vardaan electronics", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }

                string finalLang = string.Join("+", selectedLangs);
                string finalplateforms = string.Join("+", selectedplateform);

                BoardSetting _model = new BoardSetting
                {
                    HubId = Appdata.parentNodeID,
                    NodeID = Appdata.childNodeID,
                    BoardName = Appdata.boardName,
                    BoardId = _boardid,
                    Boardip = txtIpConfig.Text,
                    BoardLine = Convert.ToInt32(cmbNoOfLine.Text),
                    BoardLocation = txtLocation.Text,
                    PfNo = 0,
                    SELECTED_PF = finalplateforms,
                    Languages = finalLang,
                    BoardMessage = "",
                    DisplayEffect = Convert.ToString(cmbEffect.SelectedValue),
                    FontSize = Convert.ToInt32(cmbFontSize.SelectedValue),
                    BoardSize = cmbBoardSize.Text,
                    delayedTime = Convert.ToInt32(cmbDelayTime.Text),
                    ScrollingSpeed = Convert.ToString(cmbScrollSpeed.SelectedValue),
                    DisplayType = cmbDisplayinfo.Text,
                    IsActive = chkIsActive.Checked ? 1 : 0,
                    Intensity = Convert.ToInt32(cmbIntensity.Text),
                    RST_Soft_Reset = checkBoxSoftReset.Checked,
                    DTO = cmbDTO.Text,

                    // Combine IPs based on the board type
                    MODULE_IPS = IsAdvanceVideoDisplayBoard ? txtModule1.Text.Trim() :
                                IsVideoDisplayBoard ? $"{txtModule1.Text.Trim()},{txtModule2.Text.Trim()},{txtModule3.Text.Trim()}" :
                                null
                };
                string result = _dbService.SaveOrUpdateBoardSetting(_model);
                if (!string.IsNullOrEmpty(result))
                {
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

        private void btndelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show($"Are you sure you want to delete?",
                     "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                string query = $"DELETE FROM BOARDSETTING WHERE BOARD_ID ={Appdata.BOARD_ID}";
                string resultt = _dbService.QuerySingleOrDefault(query);
                OnEvent?.Invoke(this, new eventInfo("HubConfigSaved", resultt, this.Name, true));
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