using DataBaseHandler;
using DataModel;
using Global_Data;
using Helper;
using IPIS_V1;
using LibVLCSharp.Shared;
using LibVLCSharp.WinForms;
using LogManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPIS
{
    public partial class ucPA : UserControl
    {
        private string RecordingFolderPath => Path.Combine(AppContext.BaseDirectory, "Recordings");

        private VideoView _videoView;
        private enum PlaybackState
        {
            Stopped,
            Playing,
            Paused
        }
        private PlaybackState _playbackState = PlaybackState.Stopped;
        private int _currentPlayingRow = -1;
        private System.Windows.Forms.Timer _recordTimer;
        private TimeSpan _elapsed;
        private int _hoveredTabIndex = -1;
        public ucPA()
        {
            InitializeComponent();
            _recordTimer = new System.Windows.Forms.Timer();
            _recordTimer.Interval = 1000;
            _recordTimer.Tick += (s, e) =>
            {
                _elapsed = _elapsed.Add(TimeSpan.FromSeconds(1));
                btnReStart.Text = $"STOP {_elapsed:mm\\:ss}";
            };
            this.Load += ucPA_Load;

            // Setup custom drawing to highlight the active secondary PA sub-tabs
            tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl1.DrawItem += tabControl1_DrawItem;
            tabControl1.MouseMove += tabControl1_MouseMove;
            tabControl1.MouseLeave += tabControl1_MouseLeave;
        }
        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (tabControl1.TabCount == 0) return;
            if (e.Index < 0 || e.Index >= tabControl1.TabCount) return;

            TabPage page = tabControl1.TabPages[e.Index];
            Rectangle rect = tabControl1.GetTabRect(e.Index);

            bool isActive = tabControl1.SelectedIndex == e.Index;
            bool isHovered = e.Index == _hoveredTabIndex;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            Color backColor;
            Color textColor;
            Color borderColor;

            if (isActive)
            {
                // Secondary Active tab: Steel Blue background with bold white text
                backColor = Color.SteelBlue;
                textColor = Color.White;
                borderColor = Color.FromArgb(50, 100, 150);
            }
            else if (isHovered)
            {
                // Hover state: Light Steel Blue background with Steel Blue text
                backColor = Color.FromArgb(220, 230, 245);
                textColor = Color.SteelBlue;
                borderColor = Color.FromArgb(170, 190, 220);
            }
            else
            {
                // Inactive state: Soft light grey-blue background with slate gray text
                backColor = Color.FromArgb(242, 245, 250);
                textColor = Color.FromArgb(80, 90, 105);
                borderColor = Color.FromArgb(220, 225, 235);
            }

            // Fill background
            using (SolidBrush backBrush = new SolidBrush(backColor))
            {
                e.Graphics.FillRectangle(backBrush, rect);
            }

            // Draw border outline
            using (Pen borderPen = new Pen(borderColor, 1.5f))
            {
                Rectangle borderRect = new Rectangle(rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
                e.Graphics.DrawRectangle(borderPen, borderRect);
            }

            // Style and draw tab text
            Font font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            TextFormatFlags flags = TextFormatFlags.HorizontalCenter |
                                    TextFormatFlags.VerticalCenter |
                                    TextFormatFlags.EndEllipsis |
                                    TextFormatFlags.SingleLine;

            TextRenderer.DrawText(e.Graphics, page.Text, font, rect, textColor, flags);
            font.Dispose();
        }
        private void tabControl1_MouseMove(object sender, MouseEventArgs e)
        {
            int hoverIndex = -1;
            for (int i = 0; i < tabControl1.TabCount; i++)
            {
                if (tabControl1.GetTabRect(i).Contains(e.Location))
                {
                    hoverIndex = i;
                    break;
                }
            }

            if (hoverIndex != _hoveredTabIndex)
            {
                _hoveredTabIndex = hoverIndex;
                tabControl1.Invalidate();
            }
        }
        private void tabControl1_MouseLeave(object sender, EventArgs e)
        {
            if (_hoveredTabIndex != -1)
            {
                _hoveredTabIndex = -1;
                tabControl1.Invalidate();
            }
        }
        private void CreateVideoPlayer()
        {
            _videoView = new VideoView();
            _videoView.Dock = DockStyle.Fill;

            //gBoxViewAnncList.Controls.Add(_videoView);

            //MediaManager.Instance.AttachVideoView(_videoView);

        }
        private void InitializeRecordingGrid()
        {
            dvRecording.Columns.Clear();

            dvRecording.DataSource = null;
            dvRecording.Columns.Clear();

            dvRecording.ReadOnly = false;
            dvRecording.AllowUserToAddRows = false;
            dvRecording.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            txtRetitle.PlaceholderText = "Please enter your recording title name";
            dvRecording.MultiSelect = false;
            dvRecording.RowHeadersVisible = false;
            dvRecording.AutoGenerateColumns = false;

            dvRecording.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ID",
                HeaderText = "ID",
                DataPropertyName = "ID",
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            dvRecording.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Title",
                HeaderText = "Title",
                DataPropertyName = "Title",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true
            });

            dvRecording.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "FileName",
                HeaderText = "File Name",
                DataPropertyName = "FileName",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true
            });

            dvRecording.Columns.Add(CreateButton("Play", "▶"));
            dvRecording.Columns.Add(CreateButton("Pause", "⏸"));
            dvRecording.Columns.Add(CreateButton("Stop", "⏹"));
            dvRecording.Columns.Add(CreateButton("Del", "🗑"));
        }
        private void InitializeMusicGrid()
        {
            dvMusic.DataSource = null;
            dvMusic.Columns.Clear();

            dvMusic.ReadOnly = false;
            dvMusic.AllowUserToAddRows = false;
            dvMusic.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dvMusic.MultiSelect = false;
            dvMusic.RowHeadersVisible = false;
            dvMusic.AutoGenerateColumns = false;

            dvMusic.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ID",
                HeaderText = "ID",
                DataPropertyName = "ID",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            dvMusic.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Title",
                HeaderText = "Title",
                DataPropertyName = "Title",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true
            });

            dvMusic.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "FileName",
                HeaderText = "File Name",
                DataPropertyName = "FileName",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true
            });
            dvMusic.Columns.Add(CreateButton("Play", "▶"));
            dvMusic.Columns.Add(CreateButton("Pause", "⏸"));
            dvMusic.Columns.Add(CreateButton("Stop", "⏹"));
            dvMusic.Columns.Add(CreateButton("Del", "🗑"));

        }
        private void GetRecordings()
        {
            List<Recordings> recordings = _dbService.GET_RECORDING();
            dvRecording.DataSource = null;
            dvRecording.DataSource = recordings;
        }
        private void dvRecording_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var recording = dvRecording.Rows[e.RowIndex].DataBoundItem as Recordings;

            if (recording == null)
                return;

            string fullPath = Path.Combine(RecordingFolderPath, recording.FileName);
            string clickedColumn = dvRecording.Columns[e.ColumnIndex].Name;

            switch (clickedColumn)
            {
                case "Play":
                    if (_currentPlayingRow != -1 && _currentPlayingRow != e.RowIndex)
                    {
                        Stop("Recording");
                        ResetRowButtons(_currentPlayingRow);
                    }
                    Play(fullPath, "Recording");
                    _currentPlayingRow = e.RowIndex;
                    _playbackState = PlaybackState.Playing;
                    break;

                case "Pause":
                    Pause("Recording");
                    break;

                case "Stop":
                    Stop("Recording");
                    break;
                case "Del":
                    DeleteRecording(recording);
                    break;
            }
        }
        private async void Play(string filePath, string type)
        {
            MediaManager.Instance.MediaPlayer?.Stop();
            _playbackState = PlaybackState.Stopped;
            if (_playbackState == PlaybackState.Playing)
                return;
            _playbackState = PlaybackState.Playing;
            string imagePath = AppHelper.GetImagePath("gifusionwave.gif");
            if (File.Exists(imagePath))
            {
                if (type == "Recording")
                {
                    recordingPicture.Image = Image.FromFile(imagePath);
                    recordingPicture.SizeMode = PictureBoxSizeMode.StretchImage;
                    recordingPicture.Visible = true;
                }
                else
                {
                    musicPicture.Image = Image.FromFile(imagePath);
                    musicPicture.SizeMode = PictureBoxSizeMode.StretchImage;
                    musicPicture.Visible = true;
                }
            }
            await MediaManager.Instance.PlayAsync(filePath);
            recordingPicture.Visible = false;
            musicPicture.Visible = false;
        }
        private void Pause(string type)
        {
            MediaManager.Instance.MediaPlayer?.Pause();
            if (type == "Recording")
                recordingPicture.Visible = false;
            else
                musicPicture.Visible = false;
        }
        private void Stop(string type)
        {
            MediaManager.Instance.MediaPlayer?.Stop();
            _playbackState = PlaybackState.Stopped;

            if (_currentPlayingRow != -1)
                ResetRowButtons(_currentPlayingRow);

            if (type == "Recording")
                recordingPicture.Visible = false;
            else
                musicPicture.Visible = false;
        }
        private void DeleteRecording(Recordings recording)
        {
            var result = MessageBox.Show(
                $"Are you sure you want to delete '{recording.Title}'?",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
                return;
            Stop("Recording");
            try
            {
                string query = "DELETE FROM RECORDING WHERE ID=" + recording.ID;
                _dbService.ExecuteScalar(query);

                string fullPath = Path.Combine(RecordingFolderPath, recording.FileName);

                if (File.Exists(fullPath))
                    File.Delete(fullPath);

                GetRecordings();
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
        }

        private void DeleteAudio(string path)
        {
            try
            {
                if (MessageBox.Show(
                    "Delete this file?", "Vardaan Electronices", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                Stop("Music");

                if (dvMusic.CurrentRow == null)
                    return;

                int musicId = Convert.ToInt32(dvMusic.CurrentRow.Cells["ID"].Value);

                string query = $"DELETE FROM MUSICLIST WHERE ID={musicId}";
                _dbService.ExecuteScalar(query);

                if (File.Exists(path))
                    File.Delete(path);

                BindMusicFiles();
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, "Error deleting music.");

            }
        }

        private void ucPA_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(RecordingFolderPath))
                Directory.CreateDirectory(RecordingFolderPath);

            CreateVideoPlayer();
            InitializeMusicGrid();
            dvMusic.CellContentClick -= dvMusic_CellContentClick;
            dvMusic.CellContentClick += dvMusic_CellContentClick;
            BindMusicFiles();

            InitializeRecordingGrid();
            dvRecording.CellContentClick -= dvRecording_CellContentClick;
            dvRecording.CellContentClick += dvRecording_CellContentClick;
            txtRetitle.PlaceholderText = "recording title";
            GetRecordings();
        }
        private void btnReRefresh_Click(object sender, EventArgs e)
        {
            txtRetitle.Clear();
            GetRecordings();
        }
        //private void BindMusicFiles()
        //{
        //    try
        //    {
        //        string musicFolder = Path.Combine(AppContext.BaseDirectory, "Music");

        //        Directory.CreateDirectory(musicFolder);

        //        DataTable dt = new DataTable();
        //        dt.Columns.Add("ID", typeof(int));
        //        dt.Columns.Add("Title", typeof(string));
        //        dt.Columns.Add("FileName", typeof(string));
        //        dt.Columns.Add("FullPath", typeof(string));

        //        string[] files = Directory.GetFiles(
        //            musicFolder,
        //            "*.*",
        //            SearchOption.TopDirectoryOnly)
        //            .Where(f =>
        //                f.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase) ||
        //                f.EndsWith(".wav", StringComparison.OrdinalIgnoreCase) ||
        //                f.EndsWith(".aac", StringComparison.OrdinalIgnoreCase) ||
        //                f.EndsWith(".wma", StringComparison.OrdinalIgnoreCase))
        //            .ToArray();

        //        int id = 1;

        //        foreach (string file in files)
        //        {
        //            string fileName = Path.GetFileName(file);
        //            string title = Path.GetFileNameWithoutExtension(file);

        //            dt.Rows.Add(id++, title, fileName, file);
        //        }

        //        dvMusic.DataSource = dt;

        //        if (dvMusic.Columns.Contains("FullPath"))
        //            dvMusic.Columns["FullPath"].Visible = false;

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error loading music files: " + ex.Message);
        //    }
        //}

        //private void btnBrowseAudio_Click(object sender, EventArgs e)
        //{
        //    using (OpenFileDialog ofd = new OpenFileDialog())
        //    {
        //        ofd.Title = "Select Audio File";
        //        ofd.Filter = "Audio Files|*.mp3;*.wav;*.aac;*.wma|All Files|*.*";

        //        if (ofd.ShowDialog() == DialogResult.OK)
        //        {
        //            try
        //            {
        //                string destinationFolder = Path.Combine(AppContext.BaseDirectory, "Music");
        //                Directory.CreateDirectory(destinationFolder);
        //                string destinationPath = Path.Combine(destinationFolder, Path.GetFileName(ofd.FileName));
        //                File.Copy(ofd.FileName, destinationPath, true);
        //                BindMusicFiles();
        //                MessageBox.Show($"Audio saved successfully.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            }
        //            catch (Exception ex)
        //            {
        //                LoggerService.Logger.Error(ex, ex.Message);
        //            }
        //        }
        //    }
        //}

        private async Task BindMusicFiles()
        {
            try
            {
                string musicFolder =
                    Path.Combine(AppContext.BaseDirectory, "Music");

                Directory.CreateDirectory(musicFolder);

                var musicList = await _dbService.GetMusicListAsync();

                DataTable dt = new DataTable();

                dt.Columns.Add("ID", typeof(int));
                dt.Columns.Add("Title", typeof(string));
                dt.Columns.Add("FileName", typeof(string));
                dt.Columns.Add("FullPath", typeof(string));

                foreach (var music in musicList)
                {
                    string fileName = music.SOUND_FILE ?? string.Empty;

                    string fullPath = Path.Combine(
                        musicFolder,
                        fileName);

                    dt.Rows.Add(
                        music.ID,
                        music.MUSIC_TITLE ?? string.Empty,
                        fileName,
                        fullPath);
                }

                dvMusic.DataSource = dt;

                if (dvMusic.Columns.Contains("FullPath"))
                    dvMusic.Columns["FullPath"].Visible = false;
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(
                    ex,
                    "Error loading music list.");
            }
        }

        private async void btnBrowseAudio_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select Audio File";
                ofd.Filter = "Audio Files|*.mp3;*.wav;*.aac;*.wma|All Files|*.*";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string musicTitle = txtboxMusicTitle.Text.Trim();

                        if (string.IsNullOrWhiteSpace(musicTitle))
                        {
                            MessageBox.Show(
                                "Please enter music title.","Vardaan Electronices", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        string fileName = Path.GetFileName(ofd.FileName);

                        string destinationFolder = Path.Combine( AppContext.BaseDirectory, "Music");

                        Directory.CreateDirectory(destinationFolder);

                        string destinationPath = Path.Combine( destinationFolder, fileName);

                        File.Copy( ofd.FileName, destinationPath, true);

                        int? musicId = await _dbService.InsertMusicAsync( musicTitle, fileName);

                        if (musicId.HasValue)
                        {
                            await BindMusicFiles();

                            txtboxMusicTitle.Clear();

                            MessageBox.Show( "Audio saved successfully.",  "Vardaan Electronices", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        LoggerService.Logger.Error(ex, ex.Message);
                    }
                }
            }
        }

        private void dvMusic_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            string columnName = dvMusic.Columns[e.ColumnIndex].Name;

            string fileName = dvMusic.Rows[e.RowIndex]
                                .Cells["FileName"]
                                .Value?.ToString();

            if (string.IsNullOrWhiteSpace(fileName))
                return;

            string fullPath = Path.Combine(
                AppContext.BaseDirectory,
                "Music",
                fileName);

            switch (columnName)
            {
                case "Play":
                    if (_currentPlayingRow != -1 && _currentPlayingRow != e.RowIndex)
                    {
                        Stop("Music");
                        ResetRowButtons(_currentPlayingRow);
                    }
                    Play(fullPath, "Music");
                    _currentPlayingRow = e.RowIndex;
                    _playbackState = PlaybackState.Playing;
                    break;

                case "Pause":
                    Pause("Music");
                    _playbackState = PlaybackState.Paused;
                    break;

                case "Stop":
                    Stop("Music");
                    _playbackState = PlaybackState.Stopped;
                    break;

                case "Del":
                    DeleteAudio(fullPath);
                    break;
            }
            UpdateButtonState(e.RowIndex);
        }
        private DataGridViewButtonColumn CreateButton(string name, string text)
        {
            return new DataGridViewButtonColumn
            {
                Name = name,
                HeaderText = name,
                Text = text,
                UseColumnTextForButtonValue = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            };
        }
        private void UpdateButtonState(int rowIndex)
        {
            foreach (DataGridViewRow row in dvMusic.Rows)
            {
                bool isCurrent = row.Index == rowIndex;

                row.Cells["Play"].ReadOnly = isCurrent && _playbackState == PlaybackState.Playing;
            }

            var currentRow = dvMusic.Rows[rowIndex];

            switch (_playbackState)
            {
                case PlaybackState.Playing:
                    currentRow.Cells["Play"].Value = "▶ (Playing)";
                    break;

                case PlaybackState.Paused:
                    currentRow.Cells["Pause"].Value = "⏸ (Paused)";
                    break;

                case PlaybackState.Stopped:
                    ResetRowButtons(rowIndex);
                    break;
            }
        }
        private void ResetRowButtons(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dvMusic.Rows.Count)
                return;

            var row = dvMusic.Rows[rowIndex];

            row.Cells["Play"].Value = "▶";
            row.Cells["Pause"].Value = "⏸";
            row.Cells["Stop"].Value = "⏹";
        }
        private void btnReStart_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtRetitle.Text))
            {
                MessageBox.Show("Input title!!", "vardaan electronics", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!MediaManager.Instance.IsExternalMicrophoneAvailable())
            {
                MessageBox.Show("No microphone detected!",
                    "vardaan electronics", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                if (!MediaManager.Instance.IsRecording)
                {
                    string filePath = MediaManager.Instance.StartRecording(txtRetitle.Text, RecordingFolderPath);
                    string query = "INSERT INTO RECORDING VALUES ('" + txtRetitle.Text + "', '" + Path.GetFileName(filePath) + "', '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss ttt") + "')";
                    LoggerService.Logger.Information(query);
                    string result = _dbService.ExecuteScalar(query);
                    _elapsed = TimeSpan.Zero;
                    btnReStart.Text = "STOP 00:00";
                    _recordTimer.Start();
                }
                else
                {
                    MediaManager.Instance.StopRecording();
                    _recordTimer.Stop();
                    btnReStart.Text = "START";
                    txtRetitle.Text = string.Empty;
                    txtRetitle.PlaceholderText = "recording title";
                    GetRecordings();
                }
            }
        }
        private void dvMusic_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void dvRecording_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
        public void RefreshData()
        {
            BindMusicFiles();
            GetRecordings();
        }
    }
}
