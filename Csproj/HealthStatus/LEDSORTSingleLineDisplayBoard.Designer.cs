using MaterialSkin.Controls;

namespace IPISv2.HealthStatusReport
{
    partial class LEDSORTSingleLineDisplayBoard
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
            panelPreviewScroll = new Panel();
            boardPreview = new LEDSORTSingleLineBoardPreview();
            lblTitle = new MaterialLabel();
            panelPreviewScroll.SuspendLayout();
            SuspendLayout();
            // 
            // panelPreviewScroll
            // 
            panelPreviewScroll.AutoScroll = true;
            panelPreviewScroll.BackColor = Color.FromArgb(20, 20, 20);
            panelPreviewScroll.Controls.Add(boardPreview);
            panelPreviewScroll.Location = new Point(17, 58);
            panelPreviewScroll.Margin = new Padding(0);
            panelPreviewScroll.Name = "panelPreviewScroll";
            panelPreviewScroll.Size = new Size(1733, 294);
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
            boardPreview.OnColor = Color.White;
            boardPreview.Size = new Size(4000, 356);
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
            // 
            // LEDSORTSingleLineDisplayBoard
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Yellow;
            Controls.Add(panelPreviewScroll);
            Controls.Add(lblTitle);
            Margin = new Padding(0);
            Name = "LEDSORTSingleLineDisplayBoard";
            Size = new Size(1278, 271);
            panelPreviewScroll.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel panelPreviewScroll;
        private LEDSORTSingleLineBoardPreview boardPreview;
        private MaterialSkin.Controls.MaterialLabel lblTitle;
    }
}