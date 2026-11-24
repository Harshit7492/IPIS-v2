using Dapper;
using DataBaseHandler;
using DataModel;
using DataModel_NTS;
using Global_Data;
using Helper;
using IPISv2;
using LogManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using static IPISv2.ucKeyboard;

namespace IPIS
{
    public partial class ucMessage : UserControl
    {
        private int _messageid = 0;
        private Form keyboardForm;
        private ucKeyboard keyboardControl;
        private ColorDialog colorDialog = new ColorDialog();
        public ucMessage()
        {
            InitializeComponent();
        }

        private void ucMessage_Load(object sender, EventArgs e)
        {
            btnSaveMsg.Enabled = (GlobalData.CurrentUserAccessRight != null && GlobalData.CurrentUserAccessRight.GENERAL_MESSAGE == 1);

            List<string> categories = new List<string>();
            categories.Insert(0, "--Choose--");
            categories.Insert(1, "Emmergency Message");
            categories.Insert(2, "Default");
            cmbMsgType.DataSource = categories;

            txtbxMsgHindi.MouseClick += (s, e) =>
            {
                ShowKeyboard(txtbxMsgHindi, KeyboardLanguage.Hindi);
            };
            txtbxMsgReg.MouseClick += (s, e) =>
            {
                var language = GetRegionalKeyboardLanguage();

                if (language == null)
                {
                    MessageBox.Show(
                        "Regional language not configured.",
                        "Information",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }


                ShowKeyboard(txtbxMsgReg, language.Value);
            };

            LoadMessageGrid();

            tabControlMsg.SelectedIndexChanged += (s, ev) =>
                        {
                            if (tabControlMsg.SelectedTab == tabPageColorStatus)
                            {
                                LoadColourStatus();
                                LoadLineColourStatus();
                            }
                        };

            if (tabControlMsg.SelectedTab == tabPageColorStatus)
            {
                LoadColourStatus();
                LoadLineColourStatus();
            }
            tabControlMsg.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControlMsg.DrawItem += tabControlMsg_DrawItem;
        }
        private void LoadColourStatus()
        {
            try
            {
                var trainList = _dbService.GetTrainStatusColours();

                GV_Train_StatusColor.DataSource = null;
                GV_Train_StatusColor.Columns.Clear();
                GV_Train_StatusColor.AutoGenerateColumns = false;
                GV_Train_StatusColor.RowHeadersVisible = false;

                // ID Column
                GV_Train_StatusColor.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "colId",
                    HeaderText = "ID",
                    DataPropertyName = nameof(ColourStatus.Id),
                    ReadOnly = true,
                    Width = 80,
                    Visible = false
                });

                // Status Column
                GV_Train_StatusColor.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "colTrainStatus",
                    HeaderText = "Status",
                    DataPropertyName = nameof(ColourStatus.TrainStatus),
                    ReadOnly = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                });
                GV_Train_StatusColor.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "colArrivalDeparture",
                    HeaderText = "A/D",
                    DataPropertyName = nameof(ColourStatus.ArrivalDeparture),
                    ReadOnly = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

                });

                GV_Train_StatusColor.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "colColour",
                    HeaderText = "Color Status",
                    DataPropertyName = nameof(ColourStatus.StatusColour),
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                });

                GV_Train_StatusColor.DataSource = trainList;

                ApplyColourPreview(GV_Train_StatusColor, "StatusColour", "colColour");

                // Wire event
                GV_Train_StatusColor.CellClick -= GV_Train_StatusColor_CellClick;
                GV_Train_StatusColor.CellClick += GV_Train_StatusColor_CellClick;
            }
            catch (Exception ex)
            {
                LoggerService.MessageLogger.Error(ex, ex.Message);
            }
        }
        
        private void LoadLineColourStatus()
        {
            try
            {
                var lineColours = _dbService.GetLineColours();

                GV_IVD_OVDStatusColour.DataSource = null;
                GV_IVD_OVDStatusColour.Columns.Clear();
                GV_IVD_OVDStatusColour.AutoGenerateColumns = false;
                GV_IVD_OVDStatusColour.RowHeadersVisible = false;

                GV_IVD_OVDStatusColour.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "colId",
                    HeaderText = "ID",
                    DataPropertyName = nameof(LineColourModel.ID),
                    ReadOnly = true,
                    Width = 80,
                });

                GV_IVD_OVDStatusColour.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "colName",
                    HeaderText = "Name",
                    DataPropertyName = nameof(LineColourModel.Name),
                    ReadOnly = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                });

                GV_IVD_OVDStatusColour.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "colColour",
                    HeaderText = "Color",
                    DataPropertyName = nameof(LineColourModel.ColorCode),
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                });


                GV_IVD_OVDStatusColour.DataSource = lineColours;
                // Optional: Paint cell background with stored color
                ApplyColourPreview(GV_IVD_OVDStatusColour, "ColorCode", "colColour");

                GV_IVD_OVDStatusColour.CellClick -= GV_IVD_OVDStatusColour_CellClick;
                GV_IVD_OVDStatusColour.CellClick += GV_IVD_OVDStatusColour_CellClick;
            }
            catch (Exception ex)
            {
                LoggerService.MessageLogger.Error(ex, ex.Message);
            }
        }
        private void cmbMsgType_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void ApplyColourPreview(DataGridView grid, string sourcePropertyName, string colourColumnName)
        {
            try
            {
                foreach (DataGridViewRow row in grid.Rows)
                {
                    if (row.IsNewRow)
                        continue;

                    var bound = row.DataBoundItem;

                    string raw = null;

                    if (bound != null)
                    {
                        var prop = bound.GetType().GetProperty(
                            sourcePropertyName,
                            BindingFlags.IgnoreCase |
                            BindingFlags.Public |
                            BindingFlags.Instance);

                        if (prop != null)
                            raw = prop.GetValue(bound)?.ToString();
                    }

                    if (string.IsNullOrWhiteSpace(raw))
                        raw = row.Cells[colourColumnName].Value?.ToString();

                    if (string.IsNullOrWhiteSpace(raw))
                        continue;

                    if (TryParseColor(raw, out Color color))
                    {
                        var cell = row.Cells[colourColumnName];

                        cell.Style.BackColor = color;
                        cell.Style.SelectionBackColor = color;

                        Color fore = GetReadableForeColor(color);

                        cell.Style.ForeColor = fore;
                        cell.Style.SelectionForeColor = fore;
                    }
                }

                grid.Refresh();
            }
            catch (Exception ex)
            {
                LoggerService.MessageLogger.Error(ex, ex.Message);
            }
        }
        private Color GetReadableForeColor(Color back)
        {
            // simple luminance check
            double luminance = (0.299 * back.R + 0.587 * back.G + 0.114 * back.B) / 255;
            return luminance > 0.5 ? Color.Black : Color.White;
        }
        private bool TryParseColor(string raw, out Color color)
        {
            color = Color.Empty;

            if (string.IsNullOrWhiteSpace(raw))
                return false;

            raw = raw.Trim();

            try
            {
                // 1. Named colors
                Color named = Color.FromName(raw);
                if (named.IsKnownColor)
                {
                    color = named;
                    return true;
                }

                // 2. ARGB integer (e.g. -8388544)
                if (int.TryParse(raw, out int argb))
                {
                    color = Color.FromArgb(argb);
                    return true;
                }

                // 3. Hex (#RRGGBB or RRGGBB)
                if (!raw.StartsWith("#"))
                    raw = "#" + raw;

                color = ColorTranslator.FromHtml(raw);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private string FormatColorValue(Color c)
        {
            // If it's a named known color, persist the name (matches existing DB sample like 'Red')
            if (c.IsNamedColor)
                return c.Name;

            // Otherwise persist as hex #RRGGBB
            return $"#{c.R:X2}{c.G:X2}{c.B:X2}";
        }
        private int? GetIdFromBoundItem(object bound)
        {
            if (bound == null) return null;

            try
            {
                var type = bound.GetType();
                // common property names for id: Id, ID, iD
                var prop = type.GetProperty("Id", BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                        ?? type.GetProperty("ID", BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (prop != null)
                {
                    var val = prop.GetValue(bound);
                    if (val == null) return null;
                    if (val is int iv) return iv;
                    if (int.TryParse(val.ToString(), out int parsed)) return parsed;
                }
            }
            catch (Exception ex)
            {
                LoggerService.MessageLogger.Error(ex, ex.Message);
            }
            return null;
        }
        private void ShowKeyboard(TextBox targetTextBox, ucKeyboard.KeyboardLanguage language)
        {
            if (keyboardForm == null || keyboardForm.IsDisposed)
            {

                keyboardForm = new Form();
                keyboardControl = new ucKeyboard();
                keyboardForm.TopMost = true;
                keyboardForm.FormBorderStyle = FormBorderStyle.FixedSingle;
                keyboardForm.MaximizeBox = false;
                keyboardForm.MinimizeBox = false;
                keyboardForm.StartPosition = FormStartPosition.Manual;
                keyboardForm.BackColor = Color.FromArgb(135, 206, 250);
                keyboardControl.Dock = DockStyle.Fill;
                keyboardForm.Controls.Add(keyboardControl);
                keyboardForm.Size = new Size(800, 320);

            }

            keyboardForm.Text = language.ToString() + " Keyboard";
            keyboardControl.TargetTextBox = targetTextBox;
            keyboardControl.BuildKeyboard(language);

            // position near textbox
            Point pt = targetTextBox.PointToScreen(Point.Empty);

            // Keyboard Location
            keyboardForm.Location = new Point(
               pt.X,
               pt.Y + targetTextBox.Height + 5   // 5 px gap
            );

            keyboardForm.Show();
            keyboardForm.BringToFront();

        }
        private ucKeyboard.KeyboardLanguage? GetRegionalKeyboardLanguage()
        {
            if (GlobalData._lstAdminSetting == null)
                return null;

            string regionalLanguage = GlobalData._lstAdminSetting.STATION_LANGUAGES.Split('+').LastOrDefault()?.Trim();

            if (string.IsNullOrEmpty(regionalLanguage))
                return null;

            if (Enum.TryParse(regionalLanguage, true, out ucKeyboard.KeyboardLanguage lang))
                return lang;

            return null;

        }
        private void LoadMessageGrid()
        {
            try
            {
                var messagesList = _dbService.GET_MESSAGES(0);
                dgvMsg.DataSource = messagesList.Select(m => new
                {
                    m.MESSAGE_ID,
                    m.MESSAGE_CODE,
                    m.MESSAGE_ENGLISH,
                    m.MESSAGE_HINDI,
                    m.MESSAGE_REGIONAL,
                    m.DISPLAY_STATUS
                }).ToList();

                if (dgvMsg.Columns["MESSAGE_ID"] != null)
                    dgvMsg.Columns["MESSAGE_ID"].Visible = false;

                dgvMsg.ReadOnly = true;
                dgvMsg.AllowUserToAddRows = false;
                dgvMsg.AllowUserToDeleteRows = false;
                dgvMsg.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvMsg.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                if (dgvMsg.Columns["MESSAGE_CODE"] != null)
                {
                    dgvMsg.Columns["MESSAGE_CODE"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvMsg.Columns["MESSAGE_CODE"].Width = 120;
                }
                if (dgvMsg.Columns["Edit"] == null)
                {
                    var editCol = new DataGridViewButtonColumn
                    {
                        Name = "Edit",
                        HeaderText = "Edit",
                        Text = "✎",
                        UseColumnTextForButtonValue = true,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                        Width = 70
                    };
                    dgvMsg.Columns.Add(editCol);
                }

                if (dgvMsg.Columns["Delete"] == null)
                {
                    var delCol = new DataGridViewButtonColumn
                    {
                        Name = "Delete",
                        HeaderText = "Delete",
                        Text = "🗑",
                        UseColumnTextForButtonValue = true,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                        Width = 70
                    };
                    dgvMsg.Columns.Add(delCol);
                }

                dgvMsg.CellClick -= dgvMsg_CellClick;
                dgvMsg.CellClick += dgvMsg_CellClick;
            }
            catch (Exception ex)
            {
                LoggerService.MessageLogger.Error(ex, ex.Message);
            }
        }
        private void ClearFields()
        {
            _messageid = 0;
            txtbxMsgEng.Clear();
            txtbxMsgHindi.Clear();
            txtbxMsgReg.Clear();
            //txtbxEngSnd.Clear();
            //txtbxHindiSnd.Clear();
            //txtbxRegSnd.Clear();
            chkbxDisplayStatus.Checked = false;
            //chkbxBottomMsg.Checked = false;
            cmbMsgType.SelectedIndex = 0;
        }
        private void btnSaveMsg_Click(object sender, EventArgs e)
        {
            if (GlobalData.CurrentUserAccessRight == null || GlobalData.CurrentUserAccessRight.GENERAL_MESSAGE != 1)
            {
                btnSaveMsg.Enabled = false;
                return;
            }
            if (cmbMsgType.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select message code!!", "Vardaan Electronics", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var msg = new DataModel.Message
            {
                MESSAGE_ID = _messageid,
                MESSAGE_CODE = cmbMsgType.SelectedItem.ToString(),
                MESSAGE_ENGLISH = txtbxMsgEng.Text,
                MESSAGE_HINDI = txtbxMsgHindi.Text,
                MESSAGE_REGIONAL = txtbxMsgReg.Text,
                //SOUND_ENGLISH = string.IsNullOrWhiteSpace(txtbxEngSnd.Text) ? null : txtbxEngSnd.Text,
                //SOUND_HINDI = string.IsNullOrWhiteSpace(txtbxHindiSnd.Text) ? null : txtbxHindiSnd.Text,
                //SOUND_REGIONAL = string.IsNullOrWhiteSpace(txtbxRegSnd.Text) ? null : txtbxRegSnd.Text,
                DISPLAY_STATUS = chkbxDisplayStatus.Checked ? 1 : 0,
                REGIONAL_BITMAP = string.Empty,
                MESSAGE_FOR_DISPLAY = "1",
                LIVE_STATUS = 1,
                BottomMessageFont = string.Empty


            };
            var result = _dbService.AddOrUpdateMessage(msg);
            MessageBox.Show("Message saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information); _messageid = 0;
            ClearFields();
            LoadMessageGrid();
        }
        private void dgvMsg_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            _messageid = Convert.ToInt32(dgvMsg.Rows[e.RowIndex].Cells["MESSAGE_ID"].Value);
            if (dgvMsg.Columns[e.ColumnIndex].Name == "Edit")
            {
                txtbxMsgEng.Text = dgvMsg.Rows[e.RowIndex].Cells["MESSAGE_ENGLISH"].Value.ToString();
                txtbxMsgHindi.Text = dgvMsg.Rows[e.RowIndex].Cells["MESSAGE_HINDI"].Value.ToString();
                txtbxMsgReg.Text = dgvMsg.Rows[e.RowIndex].Cells["MESSAGE_REGIONAL"].Value.ToString();
                cmbMsgType.Text = dgvMsg.Rows[e.RowIndex].Cells["MESSAGE_CODE"].Value.ToString();
                chkbxDisplayStatus.Checked = Convert.ToInt32(dgvMsg.Rows[e.RowIndex].Cells["DISPLAY_STATUS"].Value) == 1;
            }
            else if (dgvMsg.Columns[e.ColumnIndex].Name == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete this message?", "Confirm Deletion", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string query = "DELETE FROM GENERALMESSAGE WHERE MESSAGE_ID = " + _messageid;
                    string result = _dbService.ExecuteScalar(query);
                    LoadMessageGrid();
                }
            }
        }
        public void ShowCategoryTextBox()
        {
            txtbxMsgType.Visible = true;
            txtbxMsgType.Focus();
        }
        private void txtbxMsgType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string newCat = txtbxMsgType.Text.Trim();
                if (!string.IsNullOrEmpty(newCat))
                {
                    using (var conn = new SqlConnection(Config._dbSelfConnection))
                    {
                        conn.Open();
                        conn.Execute("INSERT INTO GENERALMESSAGE (MESSAGE_CODE, MESSAGE_ENGLISH, MESSAGE_HINDI, MESSAGE_REGIONAL) VALUES (@code,'','','')", new { code = newCat });
                    }
                    txtbxMsgType.Visible = false;
                    cmbMsgType.SelectedItem = newCat;
                    LoadMessageGrid();
                    MessageBox.Show("New category added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                txtbxMsgType.Visible = false;
            }
        }
        //private void btnBrowseEng_Click(object sender, EventArgs e) => SelectSoundFile(txtbxEngSnd);
        //private void btnBrowseHindi_Click(object sender, EventArgs e) => SelectSoundFile(txtbxHindiSnd);
        //private void btnBrowseReg_Click(object sender, EventArgs e) => SelectSoundFile(txtbxRegSnd);
        private void btnClearMsg_Click(object sender, EventArgs e) => ClearFields();
        //private void SelectSoundFile(TextBox targetTextBox)
        //{
        //    using (OpenFileDialog ofd = new OpenFileDialog())
        //    {
        //        ofd.Filter = "WAV files (*.wav)|*.wav";
        //        ofd.Title = "Select Sound File";

        //        if (ofd.ShowDialog() == DialogResult.OK)
        //            targetTextBox.Text = ofd.FileName;
        //    }
        //}
        private void GV_Train_StatusColor_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0)
                    return;

                // Only allow editing the Color Status column
                if (GV_Train_StatusColor.Columns[e.ColumnIndex].Name != "colColour")
                    return;

                if (colorDialog.ShowDialog() != DialogResult.OK)
                    return;

                var row = GV_Train_StatusColor.Rows[e.RowIndex];
                var model = row.DataBoundItem as ColourStatus;

                if (model == null)
                    return;

                string newColorValue = FormatColorValue(colorDialog.Color);

                // Update database
                string query = $@"
                                UPDATE TRAINSTATUS
                                SET StatusColour = '{newColorValue}'
                                WHERE Id = {model.Id}";

                _dbService.ExecuteScalar(query);

                // Update grid immediately
                model.StatusColour = newColorValue;
                row.Cells["colColour"].Value = newColorValue;

                // Optional: update cell background color
                row.Cells["colColour"].Style.BackColor = colorDialog.Color;
                row.Cells["colColour"].Style.SelectionBackColor = colorDialog.Color;

                // Reload from database if desired
                LoadColourStatus();
            }
            catch (Exception ex)
            {
                LoggerService.MessageLogger.Error(ex, ex.Message);
            }

        }

        private void GV_IVD_OVDStatusColour_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

                if (GV_IVD_OVDStatusColour.Columns[e.ColumnIndex].Name != "colColour")
                    return;

                if (colorDialog.ShowDialog() != DialogResult.OK)
                    return;

                string newLineColor = FormatColorValue(colorDialog.Color);

                var row = GV_IVD_OVDStatusColour.Rows[e.RowIndex];
                var bound = row.DataBoundItem;

                var model = new LineColourModel
                {
                    ID = GetIdFromBoundItem(bound) ?? 0,
                    ColorCode = newLineColor
                };

                if (model == null)
                    return;


                // Update database
                string query = $@" UPDATE OVDLINECOLORS SET COLORCODE = '{newLineColor}' WHERE ID = {model.ID}";

                _dbService.ExecuteScalar(query);

                // Update grid immediately
                model.ColorCode = newLineColor;
                row.Cells["colColour"].Value = newLineColor;

                // Optional: update cell background color
                row.Cells["colColour"].Style.BackColor = colorDialog.Color;
                row.Cells["colColour"].Style.SelectionBackColor = colorDialog.Color;

                // Update database

                LoadLineColourStatus();
            }
            catch (Exception ex)
            {
                LoggerService.MessageLogger.Error(ex, ex.Message);
            }

        }

        private void tabControlMsg_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            TabPage tabPage = tabControlMsg.TabPages[e.Index];
            Rectangle tabRect = tabControlMsg.GetTabRect(e.Index);

            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            Color backColor = isSelected
                ? Color.FromArgb(0, 51, 153)   // Active tab
                : SystemColors.Control;        // Normal tab

            Color foreColor = isSelected
                ? Color.White
                : Color.Black;

            using (SolidBrush brush = new SolidBrush(backColor))
            {
                g.FillRectangle(brush, tabRect);
            }

            TextRenderer.DrawText(
                g,
                tabPage.Text,
                tabControlMsg.Font,
                tabRect,
                foreColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

            using (Pen pen = new Pen(Color.Gray))
            {
                g.DrawRectangle(pen, tabRect);
            }
        }
        private void tabControlMsg_SelectedIndexChanged(object sender, EventArgs e)
        {
            tabControlMsg.Invalidate();
        }
        public void RefreshData()
        {
            LoadMessageGrid();
            LoadLineColourStatus();
        }
    }
}
