namespace IPISv2.Controls
{
    partial class FrmAdvertisementPreview
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAdvertisementPreview));
            btnClose = new Button();
            pnlBottom = new Panel();
            pnlPreview = new Panel();
            pictureBoxPreview = new PictureBox();
            rtbPreview = new RichTextBox();
            axWindowsMediaPlayer1 = new AxWMPLib.AxWindowsMediaPlayer();
            pnlBottom.SuspendLayout();
            pnlPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPreview).BeginInit();
            ((System.ComponentModel.ISupportInitialize)axWindowsMediaPlayer1).BeginInit();
            SuspendLayout();
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnClose.Location = new Point(379, 10);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(100, 35);
            btnClose.TabIndex = 0;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // pnlBottom
            // 
            pnlBottom.Controls.Add(btnClose);
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Location = new Point(0, 489);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new Size(878, 55);
            pnlBottom.TabIndex = 2;
            // 
            // pnlPreview
            // 
            pnlPreview.BorderStyle = BorderStyle.FixedSingle;
            pnlPreview.Controls.Add(axWindowsMediaPlayer1);
            pnlPreview.Controls.Add(rtbPreview);
            pnlPreview.Controls.Add(pictureBoxPreview);
            pnlPreview.Dock = DockStyle.Fill;
            pnlPreview.Location = new Point(0, 0);
            pnlPreview.Name = "pnlPreview";
            pnlPreview.Size = new Size(878, 489);
            pnlPreview.TabIndex = 3;
            // 
            // pictureBoxPreview
            // 
            pictureBoxPreview.BackColor = Color.Black;
            pictureBoxPreview.Location = new Point(112, 61);
            pictureBoxPreview.Name = "pictureBoxPreview";
            pictureBoxPreview.Dock = DockStyle.Fill;
            pictureBoxPreview.Size = new Size(150, 75);
            pictureBoxPreview.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxPreview.TabIndex = 0;
            pictureBoxPreview.TabStop = false;
            pictureBoxPreview.Visible = false;
            // 
            // rtbPreview
            // 
            rtbPreview.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rtbPreview.Location = new Point(460, 106);
            rtbPreview.Name = "rtbPreview";
            rtbPreview.Dock = DockStyle.Fill;
            rtbPreview.ReadOnly = true;
            rtbPreview.Size = new Size(150, 144);
            rtbPreview.TabIndex = 1;
            rtbPreview.Text = "";
            rtbPreview.Visible = false;
            // 
            // axWindowsMediaPlayer1
            // 
            axWindowsMediaPlayer1.Dock = DockStyle.Fill;
            axWindowsMediaPlayer1.Enabled = true;
            axWindowsMediaPlayer1.Location = new Point(0, 0);
            axWindowsMediaPlayer1.Name = "axWindowsMediaPlayer1";
            axWindowsMediaPlayer1.OcxState = (AxHost.State)resources.GetObject("axWindowsMediaPlayer1.OcxState");
            axWindowsMediaPlayer1.Size = new Size(876, 487);
            axWindowsMediaPlayer1.TabIndex = 2;
            axWindowsMediaPlayer1.Visible = false;
            // 
            // FrmAdvertisementPreview
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(878, 544);
            Controls.Add(pnlPreview);
            Controls.Add(pnlBottom);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmAdvertisementPreview";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Preview";
            pnlBottom.ResumeLayout(false);
            pnlPreview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxPreview).EndInit();
            ((System.ComponentModel.ISupportInitialize)axWindowsMediaPlayer1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button btnClose;
        private Panel pnlBottom;
        private Panel pnlPreview;
        private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1;
        private RichTextBox rtbPreview;
        private PictureBox pictureBoxPreview;
    }
}