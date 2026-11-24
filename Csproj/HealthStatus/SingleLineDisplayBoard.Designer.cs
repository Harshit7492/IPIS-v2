using MaterialSkin.Controls;

namespace IPISv2.HealthStatusReport
{
    partial class SingleLineDisplayBoard
    {
        private System.ComponentModel.IContainer components = null;

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
            panelHeader = new Panel();
            lblBoardTitle = new MaterialLabel();
            panelPreviewScroll = new Panel();
            boardPreview = new SingleLineBoardPreview();
            lblTitle = new MaterialLabel();
            panelHeader.SuspendLayout();
            panelPreviewScroll.SuspendLayout();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panelHeader.BackColor = Color.White;
            panelHeader.Controls.Add(lblBoardTitle);
            panelHeader.Location = new Point(17, 8);
            panelHeader.Margin = new Padding(0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(1244, 25);
            panelHeader.TabIndex = 2;
            // 
            // lblBoardTitle
            // 
            lblBoardTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblBoardTitle.AutoSize = true;
            lblBoardTitle.Depth = 0;
            lblBoardTitle.Font = new Font("Roboto", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            lblBoardTitle.FontType = MaterialSkin.MaterialSkinManager.fontType.Subtitle1;
            lblBoardTitle.ForeColor = Color.White;
            lblBoardTitle.Location = new Point(12, 6);
            lblBoardTitle.Margin = new Padding(0);
            lblBoardTitle.MouseState = MaterialSkin.MouseState.HOVER;
            lblBoardTitle.Name = "lblBoardTitle";
            lblBoardTitle.Size = new Size(237, 19);
            lblBoardTitle.TabIndex = 0;
            lblBoardTitle.Text = "Single Line Display Board (SLDB)";
            // 
            // panelPreviewScroll
            // 
            panelPreviewScroll.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panelPreviewScroll.AutoScroll = true;
            panelPreviewScroll.BackColor = Color.FromArgb(20, 20, 20);
            panelPreviewScroll.Controls.Add(boardPreview);
            panelPreviewScroll.Location = new Point(17, 77);
            panelPreviewScroll.Margin = new Padding(0);
            panelPreviewScroll.Name = "panelPreviewScroll";
            panelPreviewScroll.Size = new Size(1244, 228);
            panelPreviewScroll.TabIndex = 1;
            // 
            // boardPreview
            // 
            boardPreview.BackColor = Color.FromArgb(20, 20, 20);
            boardPreview.FaultColor = Color.FromArgb(244, 67, 54);
            boardPreview.Location = new Point(0, 0);
            boardPreview.Margin = new Padding(0);
            boardPreview.Name = "boardPreview";
            boardPreview.OffColor = Color.FromArgb(60, 60, 60);
            boardPreview.OnColor = Color.FromArgb(76, 175, 80);
            boardPreview.Size = new Size(4704, 224);
            boardPreview.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Depth = 0;
            lblTitle.Font = new Font("Roboto", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            lblTitle.FontType = MaterialSkin.MaterialSkinManager.fontType.Subtitle1;
            lblTitle.Location = new Point(17, 12);
            lblTitle.Margin = new Padding(0);
            lblTitle.MouseState = MaterialSkin.MouseState.HOVER;
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(182, 19);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Board 1 — North Platform";
            lblTitle.Visible = false;
            // 
            // SingleLineDisplayBoard
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(panelHeader);
            Controls.Add(panelPreviewScroll);
            Controls.Add(lblTitle);
            Margin = new Padding(0);
            Name = "SingleLineDisplayBoard";
            Size = new Size(1278, 310);
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            panelPreviewScroll.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        // Designer variables
        private System.Windows.Forms.Panel panelHeader;
        private MaterialSkin.Controls.MaterialLabel lblBoardTitle;
        //private MaterialSkin.Controls.MaterialLabel lblTrainInfo;
        private System.Windows.Forms.Panel panelPreviewScroll;
        private SingleLineBoardPreview boardPreview;
        private MaterialSkin.Controls.MaterialLabel lblTitle;
    }
}