using DataBaseHandler;
using DataModel;
using Global_Data;
using IPISv2;
using IPISv2.Controls;
using LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace IPIS
{
    public partial class ucPlaylistManager : UserControl
    {
        private int _editingPlaylistId = -1;
        private List<int> _currentPlaylistItemIds = new List<int>();
        private bool _isFormLoading = false;

        string userId = GlobalData.CurrentUserId;
        string userName = GlobalData.CurrentUserName;

        public ucPlaylistManager()
        {
            InitializeComponent();
            BindTVLineCount();
            Load += ucPlaylistManager_Load;
            cmbRepeat.SelectedIndex = 0;
            cmbPlaylistType.SelectedIndex = -1;
            dtpStartDateTime.Value = DateTime.Now;
            dtpEndDateTime.Value = DateTime.Now.AddHours(1);
        }

        private void ucPlaylistManager_Load(object sender, EventArgs e)
        {
            RefreshPlaylistGrid();
        }

        private void RefreshPlaylistGrid()
        {
            LoadPlaylists();

            if (dataGridViewPlaylists.Rows.Count > 0)
                OpenPlaylist(dataGridViewPlaylists.Rows[0]);
            else
                ClearPlaylistForm();
        }

        private void LoadPlaylists()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("SrNo", typeof(int));
                dt.Columns.Add("ID", typeof(int));
                dt.Columns.Add("PlaylistName", typeof(string));
                dt.Columns.Add("PlaylistType", typeof(string));
                dt.Columns.Add("IsActive", typeof(bool));
                dt.Columns.Add("Repeat", typeof(int));
                dt.Columns.Add("StartDateTime", typeof(DateTime));
                dt.Columns.Add("EndDateTime", typeof(DateTime));
                dt.Columns.Add("TVLineCount", typeof(string));

                int sr = 1;
                foreach (var item in _dbService.GetAllPlaylists(userId))
                {
                    dt.Rows.Add(
                        sr++,
                        item.ID,
                        item.PlaylistName,
                        item.PlaylistType,
                        item.IsActive,
                        item.Repeat,
                        item.StartDateTime ?? (object)DBNull.Value,
                        item.EndDateTime ?? (object)DBNull.Value,
                        item.TVLineCount ?? "4"
                    );
                }

                dataGridViewPlaylists.DataSource = dt;
                //HighlightActivePlaylist();
            }
            catch (Exception ex)
            {
                LoggerService.VdbLogger.Error(ex, ex.Message);
            }
        }

        //private void HighlightActivePlaylist()
        //{
        //    try
        //    {
        //        foreach (DataGridViewRow row in dataGridViewPlaylists.Rows)
        //        {
        //            if (row.Cells["IsActive"]?.Value == null || row.Cells["IsActive"].Value == DBNull.Value) continue;

        //            bool isActive = Convert.ToBoolean(row.Cells["IsActive"].Value);
        //            row.DefaultCellStyle.BackColor = isActive ? Color.LightGreen : dataGridViewPlaylists.DefaultCellStyle.BackColor;
        //            row.DefaultCellStyle.ForeColor = isActive ? Color.DarkGreen : dataGridViewPlaylists.DefaultCellStyle.ForeColor;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerService.Logger.Error(ex, ex.Message);
        //    }
        //}

        private void OpenPlaylist(DataGridViewRow row)
        {
            if (row.Cells["ID"].Value == null) return;

            _editingPlaylistId = Convert.ToInt32(row.Cells["ID"].Value);
            FillPlaylistForm(row);
            LoadPlaylistItems(_editingPlaylistId);
            LoadAvailableItems();
            btnSavePlaylist.Text = "Save";
        }

        private void FillPlaylistForm(DataGridViewRow row)
        {
            // Suppress side effects from cmbPlaylistType_SelectedIndexChanged
            // while we programmatically populate the form for a saved playlist.
            _isFormLoading = true;
            try
            {
                txtPlaylistName.Text =
                    row.Cells["PlaylistName"].Value?.ToString() ?? "";

                chkPlaylistActive.Checked =
                    Convert.ToBoolean(row.Cells["IsActive"].Value);

                cmbRepeat.Text =
                    row.Cells["Repeat"].Value?.ToString() ?? "1";

                dtpStartDateTime.Value =
                    ToDateOrDefault(
                        row.Cells["StartDateTime"].Value,
                        DateTime.Now
                    );

                dtpEndDateTime.Value =
                    ToDateOrDefault(
                        row.Cells["EndDateTime"].Value,
                        DateTime.Now.AddHours(1)
                    );

                string playlistType =
                    row.Cells["PlaylistType"].Value?.ToString()?.Trim() ?? "";

                cmbPlaylistType.Text = playlistType;

                bool isTV = playlistType.Equals(
                    "TV",
                    StringComparison.OrdinalIgnoreCase
                );

                lblTVLineCount.Visible = isTV;
                cmbTVLineCount.Visible = isTV;

                if (isTV)
                {
                    string tvLineCount = "4";

                    // Get actual value from bound DataTable
                    if (row.DataBoundItem is DataRowView drv &&
                        drv.Row.Table.Columns.Contains("TVLineCount") &&
                        drv["TVLineCount"] != DBNull.Value)
                    {
                        tvLineCount = drv["TVLineCount"]
                            .ToString()
                            .Trim();
                    }

                    SelectTVLineCount(tvLineCount);
                }
                else
                {
                    cmbTVLineCount.SelectedIndex = -1;
                }
            }
            finally
            {
                _isFormLoading = false;
            }
        }

        private void SelectTVLineCount(string rawValue)
        {
            int index = cmbTVLineCount.FindStringExact(rawValue);

            if (index < 0 && int.TryParse(rawValue, out int numericValue))
            {
                for (int i = 0; i < cmbTVLineCount.Items.Count; i++)
                {
                    if (int.TryParse(cmbTVLineCount.Items[i].ToString(), out int itemValue) &&
                        itemValue == numericValue)
                    {
                        index = i;
                        break;
                    }
                }
            }

            cmbTVLineCount.SelectedIndex = index >= 0 ? index : 0;
        }

        private DateTime ToDateOrDefault(object value, DateTime fallback)
        {
            return value == null || value == DBNull.Value ? fallback : Convert.ToDateTime(value);
        }

        private void LoadPlaylistItems(int playlistId)
        {
            try
            {
                DataTable dt = _dbService.GET_PLAYLIST_DATA(playlistId);

                _currentPlaylistItemIds.Clear();
                if (dt != null)
                {
                    // Build MessageDetails if not already present
                    if (!dt.Columns.Contains("MessageDetails"))
                    {
                        dt.Columns.Add("MessageDetails", typeof(string));
                        foreach (DataRow row in dt.Rows)
                        {
                            row["MessageDetails"] = BuildMessageDetails(row);
                        }
                    }

                    if (!dt.Columns.Contains("DisplayOrder"))
                        dt.Columns.Add("DisplayOrder", typeof(int));

                    if (!dt.Columns.Contains("PlayTimes"))
                    {
                        dt.Columns.Add("PlayTimes", typeof(int));
                        foreach (DataRow row in dt.Rows)
                        {
                            if (dt.Columns.Contains("RepeatCount") && row["RepeatCount"] != DBNull.Value)
                            {
                                if (int.TryParse(row["RepeatCount"].ToString(), out int rc) && rc > 0)
                                    row["PlayTimes"] = rc;
                                else
                                    row["PlayTimes"] = 1;
                            }
                            else
                            {
                                row["PlayTimes"] = 1;
                            }
                        }
                    }
                    else
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            if (row["PlayTimes"] == DBNull.Value && dt.Columns.Contains("RepeatCount") && row["RepeatCount"] != DBNull.Value)
                            {
                                if (int.TryParse(row["RepeatCount"].ToString(), out int rc) && rc > 0)
                                    row["PlayTimes"] = rc;
                                else
                                    row["PlayTimes"] = 1;
                            }
                        }
                    }

                    int sr = 1;
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["DisplayOrder"] == DBNull.Value || Convert.ToInt32(row["DisplayOrder"]) == 0)
                            row["DisplayOrder"] = sr++;

                        if (dt.Columns.Contains("ID") && row["ID"] != DBNull.Value)
                            _currentPlaylistItemIds.Add(Convert.ToInt32(row["ID"]));
                        else if (dt.Columns.Contains("TrueColorID") && row["TrueColorID"] != DBNull.Value)
                            _currentPlaylistItemIds.Add(Convert.ToInt32(row["TrueColorID"]));
                    }
                }

                dataGridViewPlaylistItems.DataSource = dt;

                lblItemCount.Text = dt == null || dt.Rows.Count == 0
                    ? "No items in this playlist yet — add some from the list below."
                    : $"Total Items: {dt.Rows.Count}";
            }
            catch (Exception ex)
            {
                LoggerService.VdbLogger.Error(ex, ex.Message);
            }
        }

        private void LoadAvailableItems()
        {
            try
            {
                DataTable source = _dbService.GET_TRUE_COLOR_DATA(userId);

                if (source == null || source.Rows.Count == 0)
                {
                    dataGridViewAvailableItems.DataSource = null;
                    lblAvailableCount.Text = "Available: 0";
                    return;
                }

                DataTable dt = source.Copy();

                if (dt.Columns.Contains("IsDeleted"))
                {
                    for (int i = dt.Rows.Count - 1; i >= 0; i--)
                    {
                        var delVal = dt.Rows[i]["IsDeleted"];
                        if (delVal != DBNull.Value && (Convert.ToInt32(delVal) == 1 || (delVal is bool b && b)))
                        {
                            dt.Rows.RemoveAt(i);
                        }
                    }
                }

                if (_currentPlaylistItemIds != null && _currentPlaylistItemIds.Count > 0)
                {
                    for (int i = dt.Rows.Count - 1; i >= 0; i--)
                    {
                        if (dt.Columns.Contains("ID") && dt.Rows[i]["ID"] != DBNull.Value)
                        {
                            int id = Convert.ToInt32(dt.Rows[i]["ID"]);
                            if (_currentPlaylistItemIds.Contains(id))
                            {
                                dt.Rows.RemoveAt(i);
                            }
                        }
                    }
                }

                if (!dt.Columns.Contains("SrNo"))
                    dt.Columns.Add("SrNo", typeof(int));

                if (!dt.Columns.Contains("PlayTimes"))
                {
                    dt.Columns.Add("PlayTimes", typeof(int));
                    foreach (DataRow row in dt.Rows)
                    {
                        if (dt.Columns.Contains("RepeatCount") && row["RepeatCount"] != DBNull.Value)
                            row["PlayTimes"] = row["RepeatCount"];
                        else
                            row["PlayTimes"] = 1;
                    }
                }
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["PlayTimes"] == DBNull.Value && dt.Columns.Contains("RepeatCount") && row["RepeatCount"] != DBNull.Value)
                            row["PlayTimes"] = row["RepeatCount"];
                    }
                }

                if (!dt.Columns.Contains("FileName"))
                {
                    dt.Columns.Add("FileName", typeof(string));
                    foreach (DataRow row in dt.Rows)
                    {
                        string fp = dt.Columns.Contains("FilePath") && row["FilePath"] != DBNull.Value ? row["FilePath"].ToString() : "";
                        row["FileName"] = !string.IsNullOrEmpty(fp) ? System.IO.Path.GetFileName(fp) : "";
                    }
                }

                if (!dt.Columns.Contains("MessageDetails"))
                {
                    dt.Columns.Add("MessageDetails", typeof(string));
                    foreach (DataRow row in dt.Rows)
                    {
                        List<string> parts = new List<string>();
                        if (dt.Columns.Contains("MessageEnglish") && row["MessageEnglish"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["MessageEnglish"].ToString()))
                            parts.Add(row["MessageEnglish"].ToString().Trim());
                        if (dt.Columns.Contains("MessageHindi") && row["MessageHindi"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["MessageHindi"].ToString()))
                            parts.Add(row["MessageHindi"].ToString().Trim());
                        if (dt.Columns.Contains("MessageRegional") && row["MessageRegional"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["MessageRegional"].ToString()))
                            parts.Add(row["MessageRegional"].ToString().Trim());

                        row["MessageDetails"] = string.Join(" | ", parts);
                    }
                }

                int sr = 1;
                foreach (DataRow row in dt.Rows)
                {
                    row["SrNo"] = sr++;
                }

                dataGridViewAvailableItems.DataSource = dt;
                lblAvailableCount.Text = $"Available: {dt.Rows.Count}";
            }
            catch (Exception ex)
            {
                LoggerService.VdbLogger.Error(ex, $"Error loading available TrueColor items: {ex.Message}");
            }
        }

        private int ToIntOrZero(DataRow row, string column)
        {
            return row.Table.Columns.Contains(column) && row[column] != DBNull.Value
                ? Convert.ToInt32(row[column]) : 0;
        }

        private string GetCol(DataRow row, string column)
        {
            return row.Table.Columns.Contains(column) && row[column] != DBNull.Value
                ? row[column].ToString() : "";
        }

        private string GetCellText(DataGridViewRow row, string column)
        {
            if (row.DataGridView == null || !row.DataGridView.Columns.Contains(column)) return "";
            var value = row.Cells[column].Value;
            return value == null || value == DBNull.Value ? "" : value.ToString();
        }

        private void dataGridViewPlaylists_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            try
            {
                DataGridViewRow row = dataGridViewPlaylists.Rows[e.RowIndex];
                if (row.Cells["ID"].Value == null) return;

                string column = dataGridViewPlaylists.Columns[e.ColumnIndex].Name;
                int playlistId = Convert.ToInt32(row.Cells["ID"].Value);

                if (column == "Delete") DeletePlaylist(playlistId);
                else if (column == "Preview") PreviewPlaylist(playlistId);
                else OpenPlaylist(row);
            }
            catch (Exception ex)
            {
                LoggerService.VdbLogger.Error(ex, ex.Message);
            }
        }

        private void dataGridViewPlaylistItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            try
            {
                DataGridViewRow row = dataGridViewPlaylistItems.Rows[e.RowIndex];
                string column = dataGridViewPlaylistItems.Columns[e.ColumnIndex].Name;

                if (column == "Remove") RemoveItemFromPlaylist(row);
                else if (column == "Preview")
                    ShowPreview(GetCellText(row, "AdvType"), GetCellText(row, "FilePath"), GetCellText(row, "MessageDetails"));
            }
            catch (Exception ex)
            {
                LoggerService.VdbLogger.Error(ex, ex.Message);
            }
        }

        private void dataGridViewAvailableItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            try
            {
                DataGridViewRow row = dataGridViewAvailableItems.Rows[e.RowIndex];
                string column = dataGridViewAvailableItems.Columns[e.ColumnIndex].Name;

                if (column == "Add") AddItemToPlaylist(row);
                else if (column == "Preview")
                    ShowPreview(GetCellText(row, "AdvType"), GetCellText(row, "FilePath"), GetCellText(row, "MessageDetails"));
            }
            catch (Exception ex)
            {
                LoggerService.VdbLogger.Error(ex, ex.Message);
            }
        }

        private void btnSavePlaylist_Click(object sender, EventArgs e)
        {
            if (!IsPlaylistFormValid()) return;

            try
            {
                bool isNew = _editingPlaylistId == -1;
                TrueColorPlaylist playlist = BuildPlaylistFromForm(isNew);
                int result = _dbService.SAVE_PLAYLIST(playlist);

                if (result == -1)
                {
                    MessageBox.Show("A playlist with this name already exists.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPlaylistName.Focus();
                    return;
                }

                if (result <= 0)
                {
                    MessageBox.Show("Unable to save playlist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _editingPlaylistId = result;
                MessageBox.Show(isNew ? "Playlist saved successfully." : "Playlist updated successfully.",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadPlaylists();
                LoadPlaylistItems(_editingPlaylistId);
                LoadAvailableItems();
                btnSavePlaylist.Text = "Update";
            }
            catch (Exception ex)
            {
                LoggerService.VdbLogger.Error(ex, ex.Message);
            }
        }

        private bool IsPlaylistFormValid()
        {
            if (string.IsNullOrWhiteSpace(txtPlaylistName.Text))
            {
                MessageBox.Show("Please enter Playlist Name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPlaylistName.Focus();
                return false;
            }

            if (cmbPlaylistType.SelectedIndex < 0)
            {
                MessageBox.Show("Please select Playlist Type.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbPlaylistType.Focus();
                return false;
            }

            if (dtpStartDateTime.Value >= dtpEndDateTime.Value)
            {
                MessageBox.Show("End date/time must be after start date/time.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpEndDateTime.Focus();
                return false;
            }

            if (dtpEndDateTime.Value < DateTime.Now)
            {
                MessageBox.Show("End date/time cannot be in the past.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpEndDateTime.Focus();
                return false;
            }

            return true;
        }

        private TrueColorPlaylist BuildPlaylistFromForm(bool isNew)
        {
            int.TryParse(cmbRepeat.Text, out int repeat);
            if (repeat == 0) repeat = 1;

            string itemIds = null;
            if (_currentPlaylistItemIds != null && _currentPlaylistItemIds.Count > 0)
            {
                itemIds = string.Join(",", _currentPlaylistItemIds);
            }

            // Only persist a TVLineCount for TV playlists; for OVD it's meaningless.
            bool isTV = cmbPlaylistType.Text.Equals("TV", StringComparison.OrdinalIgnoreCase);
            string tvLineCount = isTV ? cmbTVLineCount.Text : null;

            return new TrueColorPlaylist
            {
                ID = isNew ? 0 : _editingPlaylistId,
                PlaylistName = txtPlaylistName.Text.Trim(),
                PlaylistType = cmbPlaylistType.Text.Trim(),
                IsActive = chkPlaylistActive.Checked,
                Repeat = repeat,
                StartDateTime = dtpStartDateTime.Value,
                EndDateTime = dtpEndDateTime.Value,
                ItemIDs = itemIds,
                UserId = userId,
                UserName = userName,
                TVLineCount = tvLineCount
            };
        }

        private void DeletePlaylist(int playlistId)
        {
            if (MessageBox.Show("Are you sure you want to delete this playlist?", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            try
            {
                if (!_dbService.DELETE_PLAYLIST(playlistId)) return;

                MessageBox.Show("Playlist deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshPlaylistGrid();
            }
            catch (Exception ex)
            {
                LoggerService.VdbLogger.Error(ex, ex.Message);
            }
        }

        private void AddItemToPlaylist(DataGridViewRow row)
        {
            if (_editingPlaylistId == -1)
            {
                MessageBox.Show("Please save the playlist first before adding items.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (row.Cells["ID"].Value == null) return;
                int trueColorId = Convert.ToInt32(row.Cells["ID"].Value);

                // Get selected item's advertisement type
                string selectedAdvType = GetCellText(row, "AdvType");
                if (selectedAdvType.Equals("Video", StringComparison.OrdinalIgnoreCase))
                {
                    DataTable currentItems = _dbService.GET_PLAYLIST_DATA(_editingPlaylistId);

                    if (currentItems != null && currentItems.Rows.Count > 0)
                    {
                        foreach (DataRow item in currentItems.Rows)
                        {
                            string existingAdvType = "";

                            if (currentItems.Columns.Contains("AdvType") &&
                                item["AdvType"] != DBNull.Value)
                            {
                                existingAdvType = item["AdvType"].ToString();
                            }

                            if (existingAdvType.Equals("Video", StringComparison.OrdinalIgnoreCase))
                            {
                                MessageBox.Show(
                                    "Only one video can be added to a playlist.",
                                    "Video Limit",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);

                                return;
                            }
                        }
                    }
                }

                if (!_dbService.MANAGE_PLAYLIST_ITEM("ADD", _editingPlaylistId, trueColorId)) return;

                LoadPlaylistItems(_editingPlaylistId);
                LoadAvailableItems();
            }
            catch (Exception ex)
            {
                LoggerService.VdbLogger.Error(ex, ex.Message);
            }
        }

        private void RemoveItemFromPlaylist(DataGridViewRow row)
        {
            try
            {
                if (!(row.DataBoundItem is DataRowView drv)) return;
                int trueColorId = Convert.ToInt32(drv["ID"]);

                if (MessageBox.Show("Are you sure you want to remove this item?", "Confirm Remove",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

                if (!_dbService.MANAGE_PLAYLIST_ITEM("REMOVE", _editingPlaylistId, trueColorId)) return;

                LoadPlaylistItems(_editingPlaylistId);
                LoadAvailableItems();
            }
            catch (Exception ex)
            {
                LoggerService.VdbLogger.Error(ex, ex.Message);
            }
        }

        private void PreviewPlaylist(int playlistId)
        {
            try
            {
                DataTable dt = _dbService.GET_PLAYLIST_DATA(playlistId);
                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("No items in this playlist to preview.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Ensure MessageDetails is present in this fresh DataTable
                if (!dt.Columns.Contains("MessageDetails"))
                {
                    dt.Columns.Add("MessageDetails", typeof(string));
                    foreach (DataRow row in dt.Rows)
                    {
                        row["MessageDetails"] = BuildMessageDetails(row);
                    }
                }

                List<FrmAdvertisementPreview.PreviewItem> items = new List<FrmAdvertisementPreview.PreviewItem>();

                foreach (DataRow row in dt.Rows)
                {
                    int duration = ToIntOrZero(row, "PlayDuration");
                    items.Add(new FrmAdvertisementPreview.PreviewItem
                    {
                        AdvType = GetCol(row, "AdvType"),
                        FilePath = GetCol(row, "FilePath"),
                        MessageText = GetCol(row, "MessageDetails"),
                        PlayDuration = duration == 0 ? 5 : duration
                    });
                }

                new FrmAdvertisementPreview { PlaylistItems = items }.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerService.VdbLogger.Error(ex, ex.Message);
            }
        }

        private void ShowPreview(string advType, string filePath, string messageText)
        {
            try
            {
                new FrmAdvertisementPreview
                {
                    AdvertisementType = advType,
                    FilePath = filePath,
                    MessageText = messageText
                }.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerService.VdbLogger.Error(ex, ex.Message);
            }
        }

        private void ClearPlaylistForm()
        {
            _isFormLoading = true;
            try
            {
                _editingPlaylistId = -1;
                txtPlaylistName.Clear();
                cmbPlaylistType.SelectedIndex = -1;
                chkPlaylistActive.Checked = false;
                cmbRepeat.SelectedIndex = 0;
                dtpStartDateTime.Value = DateTime.Now;
                dtpEndDateTime.Value = DateTime.Now.AddHours(1);
                btnSavePlaylist.Text = "Save";

                cmbTVLineCount.SelectedIndex = -1;
                lblTVLineCount.Visible = false;
                cmbTVLineCount.Visible = false;
            }
            finally
            {
                _isFormLoading = false;
            }

            dataGridViewPlaylistItems.DataSource = null;
            _currentPlaylistItemIds.Clear();

            lblItemCount.Text = "Select a playlist on the left, or create a new one.";
            LoadAvailableItems();
        }

        private void btnAddNewPlaylist_Click(object sender, EventArgs e)
        {
            ClearPlaylistForm();
            txtPlaylistName.Focus();
        }

        private string BuildMessageDetails(DataRow row)
        {
            List<string> parts = new List<string>();
            var table = row.Table;

            if (table.Columns.Contains("MessageEnglish") && row["MessageEnglish"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["MessageEnglish"].ToString()))
                parts.Add(row["MessageEnglish"].ToString().Trim());

            if (table.Columns.Contains("MessageHindi") && row["MessageHindi"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["MessageHindi"].ToString()))
                parts.Add(row["MessageHindi"].ToString().Trim());

            if (table.Columns.Contains("MessageRegional") && row["MessageRegional"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["MessageRegional"].ToString()))
                parts.Add(row["MessageRegional"].ToString().Trim());

            return string.Join(" | ", parts);
        }

        private void cmbPlaylistType_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isTV = cmbPlaylistType.Text.Equals(
                "TV",
                StringComparison.OrdinalIgnoreCase
            );

            lblTVLineCount.Visible = isTV;
            cmbTVLineCount.Visible = isTV;

            // While FillPlaylistForm is loading a saved playlist, it will set
            // the correct TVLineCount itself right after this event fires.
            // Don't let this handler override that with the "new playlist" default.
            if (_isFormLoading)
                return;

            if (!isTV)
            {
                cmbTVLineCount.SelectedIndex = -1;
                return;
            }

            // Only default when no value exists (i.e. user just switched to TV).
            if (cmbTVLineCount.SelectedIndex < 0)
            {
                cmbTVLineCount.SelectedIndex = 0; // 4
            }
        }

        private void BindTVLineCount()
        {
            cmbTVLineCount.Items.Clear();

            cmbTVLineCount.Items.AddRange(new object[]
            {
                "4",
                "5",
                "6",
                "7",
                "8"
            });

            cmbTVLineCount.DropDownStyle = ComboBoxStyle.DropDownList;
        }
    }
}