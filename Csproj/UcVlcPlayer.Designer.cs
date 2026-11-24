namespace IPIS_V1
{
    partial class UcVlcPlayer
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
            VLC_VideoView = new LibVLCSharp.WinForms.VideoView();
            ((System.ComponentModel.ISupportInitialize)VLC_VideoView).BeginInit();
            SuspendLayout();
            // 
            // VLC_VideoView
            // 
            VLC_VideoView.BackColor = Color.Black;
            VLC_VideoView.Dock = DockStyle.Fill;
            VLC_VideoView.Location = new Point(0, 0);
            VLC_VideoView.MediaPlayer = null;
            VLC_VideoView.Name = "VLC_VideoView";
            VLC_VideoView.Size = new Size(409, 112);
            VLC_VideoView.TabIndex = 0;
            VLC_VideoView.Text = "videoView1";
            // 
            // UcVlcPlayer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(VLC_VideoView);
            Name = "UcVlcPlayer";
            Size = new Size(409, 112);
            ((System.ComponentModel.ISupportInitialize)VLC_VideoView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private LibVLCSharp.WinForms.VideoView VLC_VideoView;
    }
}
