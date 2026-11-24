using DataBaseHandler;
using DataModel;
using Global_Data;
using Helper;
using IPISv2;
using IPISv2.Controls;
using LibVLCSharp.Shared;
using LogManager;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static IPISv2.ucKeyboard;

namespace IPIS
{
    public partial class ucVDC : UserControl
    {

        private int editingId = -1;
        private bool _fileChanged = false;
        private bool _isLoadingRecord = false;
        private Form keyboardForm;
        private ucKeyboard keyboardControl;
        private int _hoveredTabIndex = -1;
        string userId = GlobalData.CurrentUserId;
        string userName = GlobalData.CurrentUserName;
        public ucVDC()
        {
            InitializeComponent();

            txtFilePath.TextChanged += (s, e) =>
            {
                if (!_isLoadingRecord)
                    _fileChanged = true;
            };

            txtMessageHindi.MouseClick += (s, e) =>
            {
                ShowKeyboard(txtMessageHindi, ucKeyboard.KeyboardLanguage.Hindi);
            };

            txtMessageRegional.MouseClick += (s, e) =>
            {
                var language = GetRegionalKeyboardLanguage();
                if (language == null)
                {
                    MessageBox.Show("Regional language not configured.", "Information",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                ShowKeyboard(txtMessageRegional, language.Value);
            };

            ClearForm();
        }

        private static bool GetBool(DataRow row, string column, bool defaultValue)
        {
            if (!row.Table.Columns.Contains(column) || row[column] == DBNull.Value)
                return defaultValue;

            try { return Convert.ToBoolean(row[column]); }
            catch { return defaultValue; }
        }

        private static string GetString(DataRow row, params string[] columns)
        {
            foreach (var column in columns)
            {
                if (!row.Table.Columns.Contains(column) || row[column] == DBNull.Value)
                    continue;

                var value = row[column].ToString();
                if (!string.IsNullOrWhiteSpace(value))
                    return value;
            }
            return string.Empty;
        }

        private static decimal GetDecimal(DataRow row, string column, decimal defaultValue)
        {
            if (!row.Table.Columns.Contains(column) || row[column] == DBNull.Value)
                return defaultValue;

            return decimal.TryParse(row[column].ToString(), out decimal value) ? value : defaultValue;
        }

        private static decimal Clamp(decimal value, decimal min, decimal max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        private static Image LoadIcon(string fileName)
        {
            string path = Path.Combine(Application.StartupPath, "Images", fileName);
            return File.Exists(path) ? Image.FromFile(path) : null;
        }

        // ---------------------------------------------------------------
        // Async copy / conversion of advertisement file
        // ---------------------------------------------------------------
        private async Task<string> CopyAdvertisementFileAsync(
            string sourcePath,
            IProgress<string> progress,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(sourcePath))
                return string.Empty;

            if (!File.Exists(sourcePath))
                throw new FileNotFoundException("Selected file not found.", sourcePath);

            string folder = Path.Combine(Application.StartupPath, "VDC_Advertisements");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string extension = Path.GetExtension(sourcePath).ToLowerInvariant();

            // Convert TIFF/TIF to PNG
            if (extension == ".tiff" || extension == ".tif")
            {
                string newFileName = $"{Guid.NewGuid()}.png";
                string destinationPath = Path.Combine(folder, newFileName);

                await Task.Run(() =>
                {
                    // Use FileStream with FileShare.ReadWrite to avoid file locks,
                    // and draw onto a 32-bit ARGB Bitmap to avoid GDI+ generic errors with indexed/CMYK TIFF formats
                    using (var stream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (var originalImage = Image.FromStream(stream))
                    using (var bitmap = new Bitmap(originalImage.Width, originalImage.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                    {
                        bitmap.SetResolution(originalImage.HorizontalResolution, originalImage.VerticalResolution);
                        using (var g = Graphics.FromImage(bitmap))
                        {
                            g.Clear(Color.Transparent);
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            g.DrawImage(originalImage, 0, 0, originalImage.Width, originalImage.Height);
                        }
                        bitmap.Save(destinationPath, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }, cancellationToken);

                return destinationPath;
            }

            // Convert .dat / .wam / .wmv video to .mp4
            if (extension == ".dat" || extension == ".wam" || extension == ".wmv" || extension == ".avi")
            {
                string newFileName = $"{Guid.NewGuid()}.mp4";
                string destinationPath = Path.Combine(folder, newFileName);

                var (success, convError) = await ConvertVideoToMp4Async(
                    sourcePath, destinationPath, progress, cancellationToken);

                if (success)
                    return destinationPath;

                try { if (File.Exists(destinationPath)) File.Delete(destinationPath); } catch { }
                throw new Exception($"Failed to convert '{Path.GetFileName(sourcePath)}' to MP4.\n\n{convError}");
            }

            // Other files remain unchanged
            string fileName = $"{Guid.NewGuid()}{extension}";
            string destination = Path.Combine(folder, fileName);

            await Task.Run(() => File.Copy(sourcePath, destination, true), cancellationToken);

            return destination;
        }

        // Looks for ffmpeg.exe next to the application first, then on the system PATH.
        private static string FindFfmpegPath()
        {
            string[] localCandidates =
            {
                Path.Combine(Application.StartupPath, "ffmpeg", "ffmpeg.exe"),
                Path.Combine(Application.StartupPath, "ffmpeg", "bin", "ffmpeg.exe"),
                Path.Combine(Application.StartupPath, "ffmpeg.exe")
            };

            foreach (string candidate in localCandidates)
            {
                if (File.Exists(candidate))
                    return candidate;
            }

            string pathVariable = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
            foreach (string directory in pathVariable.Split(Path.PathSeparator))
            {
                try
                {
                    string candidate = Path.Combine(directory.Trim().Trim('"'), "ffmpeg.exe");
                    if (File.Exists(candidate))
                        return candidate;
                }
                catch { }
            }

            return null;
        }

        // ---------------------------------------------------------------
        // Async video-to-MP4 conversion using ffmpeg
        // Returns (Success, Error) via ValueTuple — no "out" parameter,
        // because async methods cannot have out/ref/in parameters (CS1988).
        // ---------------------------------------------------------------
        private async Task<(bool Success, string Error)> ConvertVideoToMp4Async(
            string sourcePath,
            string destinationPath,
            IProgress<string> progress,
            CancellationToken cancellationToken)
        {
            // Encode into a temporary file first so a failed / half-written conversion
            // never ends up at the final path that is saved in the database.
            string tempPath = Path.Combine(
                Path.GetDirectoryName(destinationPath) ?? Application.StartupPath,
                $"{Path.GetFileNameWithoutExtension(destinationPath)}.converting.mp4");

            try
            {
                string ffmpeg = FindFfmpegPath();
                if (ffmpeg == null)
                {
                    return (false, "ffmpeg.exe not found. Copy ffmpeg.exe to: " +
                                   Path.Combine(Application.StartupPath, "ffmpeg", "ffmpeg.exe"));
                }

                // -nostdin              : never wait for keyboard input (no console in a WinForms app)
                // -fflags +genpts       : rebuild missing timestamps (common in .dat / VCD and some .wmv files)
                // -map 0:v:0 -map 0:a:0?: take only the first video + first audio (if any); extra data /
                //                         subtitle streams in .dat / .wmv files make the MP4 muxer fail
                // yadif=deint=interlaced: de-interlace only frames flagged as interlaced (VCD / MPEG sources)
                // scale ...*2           : H.264 yuv420p needs even width / height
                // -profile:v main + yuv420p + aac + faststart: plays in every browser and starts immediately
                var psi = new ProcessStartInfo
                {
                    FileName = ffmpeg,
                    Arguments = "-hide_banner -nostdin -y -fflags +genpts " +
                                $"-i \"{sourcePath}\" " +
                                "-map 0:v:0 -map 0:a:0? -sn -dn " +
                                "-c:v libx264 -preset veryfast -crf 23 -profile:v main -pix_fmt yuv420p " +
                                "-vf \"yadif=deint=interlaced,scale=trunc(iw/2)*2:trunc(ih/2)*2\" " +
                                "-c:a aac -b:a 128k -ac 2 -ar 44100 " +
                                "-movflags +faststart " +
                                $"\"{tempPath}\"",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardInput = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                var log = new StringBuilder();
                using var process = new Process { StartInfo = psi };

                process.OutputDataReceived += (s, e) =>
                {
                    if (e.Data != null)
                    {
                        lock (log) log.AppendLine(e.Data);
                        progress?.Report(e.Data);
                    }
                };
                process.ErrorDataReceived += (s, e) =>
                {
                    if (e.Data != null)
                    {
                        lock (log) log.AppendLine(e.Data);
                        progress?.Report(e.Data);
                    }
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                // Register cancellation so we can kill ffmpeg if needed
                using (cancellationToken.Register(() =>
                {
                    try { if (!process.HasExited) process.Kill(true); } catch { }
                }))
                {
                    try
                    {
                        await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);
                    }
                    catch (OperationCanceledException)
                    {
                        // Process was killed via cancellation; fall through to cancellation check below
                    }
                }

                if (cancellationToken.IsCancellationRequested)
                    return (false, "Conversion was cancelled.");

                if (process.ExitCode != 0)
                {
                    string text;
                    lock (log) text = log.ToString();
                    string error = "ffmpeg exit code " + process.ExitCode + ":\n" +
                                   (text.Length > 1000 ? text.Substring(text.Length - 1000) : text);
                    LoggerService.VdbLogger.Error($"ffmpeg failed for '{sourcePath}': {error}");
                    return (false, error);
                }

                if (!File.Exists(tempPath) || new FileInfo(tempPath).Length == 0)
                    return (false, "ffmpeg finished but the MP4 file is empty.");

                File.Move(tempPath, destinationPath, true);
                return (true, null);
            }
            catch (OperationCanceledException)
            {
                return (false, "Conversion was cancelled.");
            }
            catch (Exception ex)
            {
                LoggerService.VdbLogger.Error(ex, $"Error converting video to MP4: {ex.Message}");
                return (false, ex.Message);
            }
            finally
            {
                try { if (File.Exists(tempPath)) File.Delete(tempPath); } catch { }
            }
        }

        private bool IsSupportedFile(string filePath, string advType)
        {
            string ext = Path.GetExtension(filePath).ToLowerInvariant();
            string[] imageFormats = { ".bmp", ".jpeg", ".jpg", ".png", ".tiff", ".tif" };
            string[] videoFormats = { ".mpeg", ".mp4", ".wmv", ".avi", ".mov", ".dat", ".wam" };

            if (advType.Equals("Image", StringComparison.OrdinalIgnoreCase))
                return imageFormats.Contains(ext);

            if (advType.Equals("Video", StringComparison.OrdinalIgnoreCase))
                return videoFormats.Contains(ext);

            return true;
        }

        private void EnsureGridColumns()
        {
            dataGridView1.Columns.Clear();  // ← ADD THIS LINE FIRST!
            dataGridView1.AutoGenerateColumns = false;

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SrNo",
                HeaderText = "Sr. No.",
                DataPropertyName = "SrNo",
                ReadOnly = true,
                Resizable = DataGridViewTriState.False,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ID",
                HeaderText = "ID",
                DataPropertyName = "ID",
                ReadOnly = true,
                Visible = false
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "AdvType",
                HeaderText = "Adv Type",
                DataPropertyName = "AdvType",
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                MinimumWidth = 100
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PlayDuration",
                HeaderText = "Duration (s)",
                DataPropertyName = "PlayDuration",
                ReadOnly = true,
                Width = 100,
                Resizable = DataGridViewTriState.False,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }

            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PlayTimes",
                HeaderText = "Play Count",
                DataPropertyName = "PlayTimes",
                ReadOnly = true,
                Width = 100,
                Resizable = DataGridViewTriState.False,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });


            dataGridView1.Columns.Add(new DataGridViewImageColumn
            {
                Name = "Edit",
                HeaderText = "[E]",
                Image = LoadIcon("vdcEdit.png"),
                ImageLayout = DataGridViewImageCellLayout.Zoom,
                Width = 35,
                ReadOnly = true,
                Resizable = DataGridViewTriState.False,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
            });

            dataGridView1.Columns.Add(new DataGridViewImageColumn
            {
                Name = "Delete",
                HeaderText = "[D]",
                Image = LoadIcon("vdcDelete.png"),
                ImageLayout = DataGridViewImageCellLayout.Zoom,
                Width = 35,
                ReadOnly = true,
                Resizable = DataGridViewTriState.False,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
            });

            dataGridView1.Columns.Add(new DataGridViewImageColumn
            {
                Name = "Preview",
                HeaderText = "[P]",
                Image = LoadIcon("vdcPreview.png"),
                ImageLayout = DataGridViewImageCellLayout.Zoom,
                Width = 35,
                ReadOnly = true,
                Resizable = DataGridViewTriState.False,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
            });

            foreach (DataGridViewColumn column in dataGridView1.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

            ApplyGridStyle();
        }

        private void ApplyGridStyle()
        {
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;

            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(204, 229, 255);
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;
            dataGridView1.DefaultCellStyle.Padding = new Padding(2);

            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 90, 170);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.ColumnHeadersHeight = 30;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
        }

        private DataTable PrepareDisplayTable(DataTable source)
        {
            DataTable display = source.Copy();

            if (!display.Columns.Contains("SrNo")) display.Columns.Add("SrNo", typeof(int));
            if (!display.Columns.Contains("Active")) display.Columns.Add("Active", typeof(bool));
            if (!display.Columns.Contains("DisplayStatus")) display.Columns.Add("DisplayStatus", typeof(bool));
            if (!display.Columns.Contains("BottomMessage")) display.Columns.Add("BottomMessage", typeof(bool));
            if (!display.Columns.Contains("ShowTime")) display.Columns.Add("ShowTime", typeof(bool));
            if (!display.Columns.Contains("PlayTimes")) display.Columns.Add("PlayTimes", typeof(int));

            int sr = 1;
            foreach (DataRow row in display.Rows)
            {
                row["SrNo"] = sr++;
                row["Active"] = !GetBool(row, "IsDeleted", false);
                row["DisplayStatus"] = GetBool(row, "DisplayStatus", true);
                row["BottomMessage"] = GetBool(row, "BottomMessage", false);
                row["ShowTime"] = GetBool(row, "ShowTime", false);

                if (display.Columns.Contains("RepeatCount") && row["RepeatCount"] != DBNull.Value)
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

            return display;
        }

        public void LoadTrueColorData()
        {
            try
            {
                DataTable data = _dbService.GET_TRUE_COLOR_DATA(userId);

                if (data == null || data.Rows.Count == 0)
                {
                    dataGridView1.DataSource = null;
                    return;
                }

                EnsureGridColumns();
                dataGridView1.DataSource = PrepareDisplayTable(data);
            }
            catch (Exception ex)
            {
                LoggerService.VdbLogger.Error(ex, ex.Message);
            }
        }

        private void RefreshGrid()
        {
            LoadTrueColorData();
        }

        private void EnableEditingUI()
        {
            gbFileInfo.Enabled = true;
            gbPlayInfo.Enabled = true;
            btnSaveUpdate.Enabled = true;
            UpdateUIByAdvertisementType();
        }

        private void ClearAdvertisementFields()
        {
            txtFilePath.Clear();
            txtMessageEnglish.Clear();
            txtMessageHindi.Clear();
            txtMessageRegional.Clear();

            chkDisplayStatus.Checked = true;
            chkBottomMessage.Checked = false;
            chkShowTime.Checked = false;
            numPlayDuration.Value = 10;
            numPlayTimes.Value = 1;
        }

        private void ClearForm(bool resetEditState = true)
        {
            if (resetEditState)
                cmbAdvType.SelectedIndex = 0;

            ClearAdvertisementFields();

            if (resetEditState)
            {
                editingId = -1;
                btnSaveUpdate.Text = "Save";
            }

            _fileChanged = false;
            _isLoadingRecord = false;
        }

        private void UpdateUIByAdvertisementType()
        {
            string advType = (cmbAdvType.Text ?? string.Empty).Trim().ToLowerInvariant();

            bool showFile = advType == "video" || advType == "image";
            bool showMessage = advType == "message";

            txtFilePath.Enabled = showFile;
            btnBrowse.Enabled = showFile;
            gbMessageInfo.Enabled = showMessage;

            chkDisplayStatus.Enabled = showMessage;
            chkBottomMessage.Enabled = showMessage;
        }

        private void ApplyCombinedMessage(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return;

            string[] parts = raw.Contains("|")
                ? raw.Split('|')
                : raw.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            if (parts.Length > 0) txtMessageEnglish.Text = parts[0].Trim();
            if (parts.Length > 1) txtMessageHindi.Text = parts[1].Trim();
            if (parts.Length > 2) txtMessageRegional.Text = parts[2].Trim();
        }

        private void LoadSelectedRecord(DataRow row)
        {
            if (row == null) return;

            try
            {
                _isLoadingRecord = true;

                editingId = row.Table.Columns.Contains("ID") && row["ID"] != DBNull.Value
                    ? Convert.ToInt32(row["ID"])
                    : -1;

                string advType = GetString(row, "AdvType");
                if (!string.IsNullOrEmpty(advType))
                {
                    int index = cmbAdvType.FindStringExact(advType);
                    if (index >= 0)
                        cmbAdvType.SelectedIndex = index;
                    else
                        cmbAdvType.Text = advType;
                }

                txtFilePath.Text = GetString(row, "FilePath");

                txtMessageEnglish.Text = GetString(row, "MESSAGE_ENGLISH", "MessageEnglish");
                txtMessageHindi.Text = GetString(row, "MESSAGE_HINDI", "MessageHindi");
                txtMessageRegional.Text = GetString(row, "MESSAGE_REGIONAL", "MessageRegional");

                if (string.IsNullOrWhiteSpace(txtMessageEnglish.Text) &&
                    string.IsNullOrWhiteSpace(txtMessageHindi.Text) &&
                    string.IsNullOrWhiteSpace(txtMessageRegional.Text))
                {
                    ApplyCombinedMessage(GetString(row, "MessageDetails"));
                }

                chkDisplayStatus.Checked = GetBool(row, "DisplayStatus", true);
                chkBottomMessage.Checked = GetBool(row, "BottomMessage", false);
                chkShowTime.Checked = GetBool(row, "ShowTime", false);

                numPlayDuration.Value = Clamp(
                    GetDecimal(row, "PlayDuration", numPlayDuration.Value),
                    numPlayDuration.Minimum, numPlayDuration.Maximum);

                decimal playCount = GetDecimal(row, "PlayTimes", 0);
                if (playCount <= 0)
                    playCount = GetDecimal(row, "RepeatCount", numPlayTimes.Value);

                numPlayTimes.Value = Clamp(
                    playCount,
                    numPlayTimes.Minimum, numPlayTimes.Maximum);

                _fileChanged = false;

                EnableEditingUI();
                btnSaveUpdate.Text = "Save";
                UpdateUIByAdvertisementType();
            }
            catch (Exception ex)
            {
                LoggerService.VdbLogger.Error(ex, $"Error loading record: {ex.Message}");
            }
            finally
            {
                _isLoadingRecord = false;
            }
        }

        private void ucVDC_Load(object sender, EventArgs e)
        {
            try
            {
                btnSaveUpdate.Enabled = true;
                btnSaveUpdate.Text = "Save";
                LoadTrueColorData();
            }
            catch (Exception ex)
            {
                LoggerService.VdbLogger.Error(ex, ex.Message);
            }
        }

        private void cmbAdvType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoadingRecord || cmbAdvType.SelectedIndex <= 0)
            {
                UpdateUIByAdvertisementType();
                return;
            }

            ClearAdvertisementFields();
            UpdateUIByAdvertisementType();
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            string advType = (cmbAdvType.Text ?? string.Empty).Trim().ToLowerInvariant();

            using OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Supported Files|*.bmp;*.jpeg;*.jpg;*.png;*.tif;*.tiff;*.mpeg;*.mp4;*.wmv;*.avi;*.mov;*.dat;*.wam|" +
                         "Image Files (*.bmp, *.jpeg, *.jpg, *.png, *.tif, *.tiff)|*.bmp;*.jpeg;*.jpg;*.png;*.tif;*.tiff|" +
                         "Video Files (*.mpeg, *.mp4, *.wmv, *.avi, *.mov, *.dat, *.wam)|*.mpeg;*.mp4;*.wmv;*.avi;*.mov;*.dat;*.wam|" +
                         "All Files (*.*)|*.*",
                Title = "Select Advertisement File"
            };

            if (advType == "image")
                ofd.FilterIndex = 2;
            else if (advType == "video")
                ofd.FilterIndex = 3;
            else
                ofd.FilterIndex = 1;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = ofd.FileName;
                _fileChanged = true;
            }
        }

        private async void BtnSaveUpdate_Click(object sender, EventArgs e)
        {
            btnSaveUpdate.Enabled = false;
            btnBrowse.Enabled = false;
            Cursor = Cursors.WaitCursor;

            try
            {
                if (cmbAdvType.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select Advertisement Type.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbAdvType.Focus();
                    return;
                }

                string advType = cmbAdvType.Text.Trim();
                string filePath = txtFilePath.Text.Trim();
                string msgEng = txtMessageEnglish.Text.Trim();
                string msgHin = txtMessageHindi.Text.Trim();
                string msgReg = txtMessageRegional.Text.Trim();

                bool needsFile = advType.Equals("Image", StringComparison.OrdinalIgnoreCase) ||
                                  advType.Equals("Video", StringComparison.OrdinalIgnoreCase);

                if (needsFile && string.IsNullOrWhiteSpace(filePath))
                {
                    MessageBox.Show("Please select a file.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtFilePath.Focus();
                    return;
                }

                bool isMessageType = advType.Equals("Message", StringComparison.OrdinalIgnoreCase);
                bool noMessageEntered = string.IsNullOrWhiteSpace(msgEng) &&
                                        string.IsNullOrWhiteSpace(msgHin) &&
                                        string.IsNullOrWhiteSpace(msgReg);

                if (isMessageType && noMessageEntered)
                {
                    MessageBox.Show("Please enter at least one message (English, Hindi, or Regional).", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMessageEnglish.Focus();
                    return;
                }

                if (advType.Equals("Train Info", StringComparison.OrdinalIgnoreCase) &&
                    _dbService.CHECK_TRAIN_INFO_EXISTS(editingId))
                {
                    MessageBox.Show("Train Info already exists. Only one Train Info record is allowed.",
                        "Duplicate Train Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool isNewRecord = editingId == -1;

                if (!string.IsNullOrWhiteSpace(filePath))
                {
                    if (!IsSupportedFile(filePath, advType))
                    {
                        MessageBox.Show($"Selected file is not supported for {advType}.", "Invalid File",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string folder = Path.Combine(Application.StartupPath, "VDC_Advertisements");
                    string ext = Path.GetExtension(filePath).ToLowerInvariant();
                    bool isTiff = ext == ".tiff" || ext == ".tif";
                    bool isDatOrWam = ext == ".dat" || ext == ".wam" || ext == ".wmv" || ext == ".avi";
                    bool needsConversion = isTiff || isDatOrWam;
                    bool alreadyInFolder = filePath.StartsWith(folder, StringComparison.OrdinalIgnoreCase);

                    // Copy or convert if:
                    // 1. It is a new record
                    // 2. The file was changed (e.g. selected via Browse or typed)
                    // 3. The file requires conversion (TIFF/TIF -> PNG or DAT/WAM/WMV -> MP4)
                    // 4. The file is not yet in the VDC_Advertisements directory
                    if (isNewRecord || _fileChanged || needsConversion || !alreadyInFolder)
                    {
                        // 30 minute overall timeout for the copy/convert operation
                        using (var cts = new CancellationTokenSource(TimeSpan.FromMinutes(30)))
                        {
                            var progress = new Progress<string>(msg =>
                            {
                                // Optional: log ffmpeg progress somewhere
                                // e.g. Debug.WriteLine(msg);
                            });

                            filePath = await CopyAdvertisementFileAsync(filePath, progress, cts.Token);
                        }

                        _fileChanged = false;
                    }
                }

                TrueColor model = new TrueColor
                {
                    ID = editingId,
                    AdvType = advType,
                    FilePath = string.IsNullOrWhiteSpace(filePath) ? null : filePath,
                    MessageEnglish = string.IsNullOrWhiteSpace(msgEng) ? null : msgEng,
                    MessageHindi = string.IsNullOrWhiteSpace(msgHin) ? null : msgHin,
                    MessageRegional = string.IsNullOrWhiteSpace(msgReg) ? null : msgReg,
                    PlayDuration = Convert.ToInt32(numPlayDuration.Value),
                    RepeatCount = Convert.ToInt32(numPlayTimes.Value),
                    IsDeleted = 0,
                    DisplayStatus = chkDisplayStatus.Checked,
                    BottomMessage = chkBottomMessage.Checked,
                    ShowTime = chkShowTime.Checked,
                    UserId = GlobalData.CurrentUserId,
                    UserName = GlobalData.CurrentUserName
                };

                if (isNewRecord)
                {
                    model.ID = _dbService.GET_TRUE_COLOR_NEXT_ID(userId);
                }
                bool result = isNewRecord
                    ? _dbService.INSERT_TRUE_COLOR(model)
                    : _dbService.UPDATE_TRUE_COLOR(model);

                if (result)
                {
                    MessageBox.Show(isNewRecord ? "Record saved successfully." : "Record updated successfully.",
                        isNewRecord ? "Saved" : "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshGrid();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show(isNewRecord ? "Record could not be saved." : "Record could not be updated.",
                        isNewRecord ? "Save Error" : "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Operation was cancelled (timeout or user cancel).", "Cancelled",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                LoggerService.VdbLogger.Error(ex, $"Error in Save/Update button click: {ex.Message}");
                MessageBox.Show($"Error saving advertisement: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSaveUpdate.Enabled = true;
                btnBrowse.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private void DeleteRecord(int id)
        {
            try
            {
                bool result = _dbService.DELETE_TRUE_COLOR(id);

                MessageBox.Show(
                    result ? "Record deleted successfully." : "Record not found or could not be deleted.",
                    result ? "Deleted" : "Delete",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                if (result)
                    RefreshGrid();
            }
            catch (Exception ex)
            {
                LoggerService.VdbLogger.Error(ex, $"Error deleting record: {ex.Message}");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            var row = dataGridView1.Rows[e.RowIndex];
            if (row.Cells["ID"].Value == null || !int.TryParse(row.Cells["ID"].Value.ToString(), out int id))
                return;

            string colName = dataGridView1.Columns[e.ColumnIndex].Name;

            if (colName == "Edit")
            {
                if (row.DataBoundItem is DataRowView drv)
                    LoadSelectedRecord(drv.Row);
            }
            else if (colName == "Delete")
            {
                var confirm = MessageBox.Show("Are you sure you want to delete this record?", "Confirm Delete",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                    DeleteRecord(id);
            }
            else if (colName == "Preview")
            {
                if (row.DataBoundItem is DataRowView drv)
                {
                    ShowAdvertisementPreview(
                        GetString(drv.Row, "AdvType"),
                        GetString(drv.Row, "FilePath"),
                        BuildPreviewMessage(drv.Row));
                }
            }
        }

        private string BuildPreviewMessage(DataRow row)
        {
            string english = GetString(row, "MESSAGE_ENGLISH", "MessageEnglish");
            string hindi = GetString(row, "MESSAGE_HINDI", "MessageHindi");
            string regional = GetString(row, "MESSAGE_REGIONAL", "MessageRegional");

            if (!string.IsNullOrWhiteSpace(english) || !string.IsNullOrWhiteSpace(hindi) || !string.IsNullOrWhiteSpace(regional))
                return string.Join("|", english, hindi, regional);

            return GetString(row, "MessageDetails");
        }

        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0)
                return;

            string colName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (colName == "Edit" || colName == "Delete" || colName == "Preview")
                dataGridView1.Cursor = Cursors.Hand;
        }

        private void dataGridView1_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            switch (dataGridView1.Columns[e.ColumnIndex].Name)
            {
                case "Edit": e.ToolTipText = "Edit Record"; break;
                case "Delete": e.ToolTipText = "Delete Record"; break;
                case "Preview": e.ToolTipText = "Show Preview"; break;
            }
        }

        private void ShowAdvertisementPreview(string advType, string filePath, string messageText)
        {
            try
            {
                FrmAdvertisementPreview frm = new FrmAdvertisementPreview
                {
                    AdvertisementType = advType,
                    FilePath = filePath,
                    MessageText = messageText
                };
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerService.VdbLogger.Error(ex, $"Error showing preview: {ex.Message}");
            }
        }

        #region Keyboard Integration

        private void ShowKeyboard(TextBox targetTextBox, ucKeyboard.KeyboardLanguage language)
        {
            if (targetTextBox == null)
                return;

            if (keyboardForm == null || keyboardForm.IsDisposed)
            {
                keyboardControl = new ucKeyboard { Dock = DockStyle.Fill };

                keyboardForm = new Form
                {
                    TopMost = true,
                    FormBorderStyle = FormBorderStyle.FixedSingle,
                    MaximizeBox = false,
                    MinimizeBox = false,
                    StartPosition = FormStartPosition.Manual,
                    BackColor = Color.FromArgb(135, 206, 250),
                    Size = new Size(800, 320)
                };
                keyboardForm.Controls.Add(keyboardControl);
            }

            keyboardForm.Text = language + " Keyboard";
            keyboardControl.TargetTextBox = targetTextBox;
            keyboardControl.BuildKeyboard(language);

            try
            {
                Rectangle screen = Screen.FromControl(targetTextBox).WorkingArea;
                keyboardForm.StartPosition = FormStartPosition.Manual;
                keyboardForm.Location = new Point(
                        screen.X + (screen.Width - keyboardForm.Width) / 2,
                        screen.Y + (screen.Height - keyboardForm.Height) / 2
                    );
            }
            catch
            {
                keyboardForm.StartPosition = FormStartPosition.CenterScreen;
            }

            keyboardForm.Show();
            keyboardForm.BringToFront();
        }

        private ucKeyboard.KeyboardLanguage? GetRegionalKeyboardLanguage()
        {
            if (GlobalData._lstAdminSetting == null || string.IsNullOrEmpty(GlobalData._lstAdminSetting.STATION_LANGUAGES))
                return null;

            string regionalLanguage = GlobalData._lstAdminSetting.STATION_LANGUAGES
                .Split('+')
                .LastOrDefault()
                ?.Trim();

            if (string.IsNullOrEmpty(regionalLanguage))
                return null;

            return Enum.TryParse(regionalLanguage, true, out ucKeyboard.KeyboardLanguage lang) ? lang : null;
        }

        #endregion

        #region Custom Tab Drawing

        private void tabControlMain_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= tabControlMain.TabCount)
                return;

            TabPage page = tabControlMain.TabPages[e.Index];
            Rectangle rect = tabControlMain.GetTabRect(e.Index);

            bool isActive = tabControlMain.SelectedIndex == e.Index;
            bool isHovered = e.Index == _hoveredTabIndex;

            Color backColor = isActive ? Color.SteelBlue
                : isHovered ? Color.FromArgb(220, 230, 245)
                : Color.FromArgb(242, 245, 250);

            Color textColor = isActive ? Color.White
                : isHovered ? Color.SteelBlue
                : Color.FromArgb(80, 90, 105);

            Color borderColor = isActive ? Color.FromArgb(50, 100, 150)
                : isHovered ? Color.FromArgb(170, 190, 220)
                : Color.FromArgb(220, 225, 235);

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            using (SolidBrush backBrush = new SolidBrush(backColor))
                e.Graphics.FillRectangle(backBrush, rect);

            using (Pen borderPen = new Pen(borderColor, 1.5f))
                e.Graphics.DrawRectangle(borderPen, new Rectangle(rect.X, rect.Y, rect.Width - 1, rect.Height - 1));

            using (Font font = new Font("Segoe UI", 10F, FontStyle.Bold))
            {
                TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                                         TextFormatFlags.EndEllipsis | TextFormatFlags.SingleLine;
                TextRenderer.DrawText(e.Graphics, page.Text, font, rect, textColor, flags);
            }
        }

        private void tabControlMain_MouseMove(object sender, MouseEventArgs e)
        {
            int hoverIndex = -1;
            for (int i = 0; i < tabControlMain.TabCount; i++)
            {
                if (tabControlMain.GetTabRect(i).Contains(e.Location))
                {
                    hoverIndex = i;
                    break;
                }
            }

            if (hoverIndex != _hoveredTabIndex)
            {
                _hoveredTabIndex = hoverIndex;
                tabControlMain.Invalidate();
            }
        }

        private void tabControlMain_MouseLeave(object sender, EventArgs e)
        {
            if (_hoveredTabIndex != -1)
            {
                _hoveredTabIndex = -1;
                tabControlMain.Invalidate();
            }
        }

        #endregion
    }

    // ---------------------------------------------------------------
    // Extension method for .NET Framework / older runtimes
    // (if you're on .NET 5+ you can delete this class)
    // ---------------------------------------------------------------
    internal static class ProcessExtensions
    {
        public static Task WaitForExitAsync(this Process process, CancellationToken cancellationToken = default)
        {
            if (process.HasExited)
                return Task.CompletedTask;

            var tcs = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);

            process.EnableRaisingEvents = true;
            process.Exited += (s, e) => tcs.TrySetResult(null);

            if (cancellationToken != default)
            {
                cancellationToken.Register(() =>
                {
                    try { tcs.TrySetCanceled(cancellationToken); } catch { }
                });
            }

            return tcs.Task;
        }
    }
}