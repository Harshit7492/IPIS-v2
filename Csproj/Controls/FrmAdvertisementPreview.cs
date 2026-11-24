using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPISv2.Controls
{
    public partial class FrmAdvertisementPreview : Form
    {
        public class PreviewItem
        {
            public string AdvType { get; set; } = string.Empty;
            public string FilePath { get; set; } = string.Empty;
            public string MessageText { get; set; } = string.Empty;
            public int PlayDuration { get; set; }
        }

        public string AdvertisementType { get; set; } = string.Empty;

        public string FilePath { get; set; } = string.Empty;

        public string MessageText { get; set; } = string.Empty;

        public DataTable? TrainInfoTable { get; set; }

        public List<PreviewItem>? PlaylistItems { get; set; }
        private int _currentItemIndex = 0;
        private System.Windows.Forms.Timer? _playlistTimer;

        public FrmAdvertisementPreview()
        {
            InitializeComponent();
            this.Load += FrmAdvertisementPreview_Load;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_playlistTimer != null)
            {
                _playlistTimer.Stop();
                _playlistTimer.Dispose();
                _playlistTimer = null;
            }
            try
            {
                if (axWindowsMediaPlayer1 != null)
                {
                    axWindowsMediaPlayer1.Ctlcontrols.stop();
                }
            }
            catch { }
            base.OnFormClosing(e);
        }

        private void FrmAdvertisementPreview_Load(object sender, EventArgs e)
        {
            if (PlaylistItems != null && PlaylistItems.Count > 0)
            {
                _currentItemIndex = 0;
                PlayPlaylistItem(_currentItemIndex);
            }
            else
            {
                switch (AdvertisementType)
                {
                    case "Image":
                        ShowImagePreview(FilePath);
                        break;

                    case "Video":
                        ShowVideoPreview(FilePath);
                        break;

                    case "Message":
                    case "GENERAL MESSAGE":
                        ShowMessagePreview(MessageText);
                        break;
                }
            }
        }

        private void PlayPlaylistItem(int index)
        {
            if (PlaylistItems == null || PlaylistItems.Count == 0) return;

            if (index < 0 || index >= PlaylistItems.Count)
            {
                _currentItemIndex = 0;
                index = 0;
            }

            var item = PlaylistItems[index];

            switch (item.AdvType)
            {
                case "Image":
                    ShowImagePreview(item.FilePath);
                    break;

                case "Video":
                    ShowVideoPreview(item.FilePath);
                    break;

                case "Message":
                case "GENERAL MESSAGE":
                    ShowMessagePreview(item.MessageText);
                    break;
                default:
                    HideAllPreviewControls();
                    break;
            }

            int duration = item.PlayDuration;
            if (duration <= 0)
            {
                duration = 5;
            }

            if (_playlistTimer == null)
            {
                _playlistTimer = new System.Windows.Forms.Timer();
                _playlistTimer.Tick += PlaylistTimer_Tick;
            }
            else
            {
                _playlistTimer.Stop();
            }

            _playlistTimer.Interval = duration * 1000;
            _playlistTimer.Start();
        }

        private void PlaylistTimer_Tick(object? sender, EventArgs e)
        {
            _currentItemIndex++;
            PlayPlaylistItem(_currentItemIndex);
        }

        private void HideAllPreviewControls()
        {
            pictureBoxPreview.Visible = false;

            rtbPreview.Visible = false;

            try
            {
                axWindowsMediaPlayer1.Ctlcontrols.stop();
            }
            catch { }
            axWindowsMediaPlayer1.Visible = false;
        }
        //Image Preview
        private void ShowImagePreview(string imagePath)
        {

            HideAllPreviewControls();

            if (!File.Exists(imagePath))
            {
                MessageBox.Show("Image not found.");
                return;
            }

            string ext = Path.GetExtension(imagePath).ToLowerInvariant();

            if (!new[]
            {
                ".bmp",
                ".jpeg",
                ".jpg",
                ".png",
                ".tiff",
                ".tif"
            }.Contains(ext))
            {
                MessageBox.Show("Unsupported image format.");
                return;
            }

            pictureBoxPreview.Image = Image.FromFile(imagePath);

            pictureBoxPreview.Visible = true;
        }
        //Video Preview
        private void ShowVideoPreview(string videoPath)
        {
            HideAllPreviewControls();

            if (!File.Exists(videoPath))
            {
                MessageBox.Show("Video file not found.");
                return;
            }

            string ext = Path.GetExtension(videoPath).ToLowerInvariant();

            if (!new[]
            {
                ".mpeg",
                ".mp4",
                ".wmv",
                ".avi",
                ".mov",
                ".dat",
                ".wam"
            }.Contains(ext))
            {
                MessageBox.Show("Unsupported video format.");
                return;
            }

            axWindowsMediaPlayer1.Visible = true;

            axWindowsMediaPlayer1.URL = videoPath;

            axWindowsMediaPlayer1.Ctlcontrols.stop();

            axWindowsMediaPlayer1.Ctlcontrols.play();
        }
        private void ShowMessagePreview(string message)
        {
            HideAllPreviewControls();

            rtbPreview.Clear();
            rtbPreview.Text = message;

            rtbPreview.SelectionStart = 0;
            rtbPreview.SelectionLength = 0;

            rtbPreview.Visible = true;
        }
    }
}