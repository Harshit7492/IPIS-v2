namespace IPISv2
{
    partial class ucHSR
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
            tabControl1 = new TabControl();
            tabPageLivePreview = new TabPage();
            tabPageLEDSortMatrix = new TabPage();
            tabControl1.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPageLivePreview);
            tabControl1.Controls.Add(tabPageLEDSortMatrix);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl1.Font = new Font("Microsoft Sans Serif", 9.5F);
            tabControl1.Location = new Point(0, 0);
            tabControl1.Margin = new Padding(5, 6, 5, 6);
            tabControl1.Name = "tabControl1";
            tabControl1.Padding = new Point(10, 3);
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1333, 865);
            tabControl1.TabIndex = 0;
            tabControl1.DrawItem += tabControl1_DrawItem;
            // 
            // tabPageLivePreview
            // 
            tabPageLivePreview.BackColor = Color.FromArgb(245, 247, 250);
            tabPageLivePreview.Location = new Point(4, 33);
            tabPageLivePreview.Margin = new Padding(5, 6, 5, 6);
            tabPageLivePreview.Name = "tabPageLivePreview";
            tabPageLivePreview.Padding = new Padding(5, 6, 5, 6);
            tabPageLivePreview.Size = new Size(1325, 828);
            tabPageLivePreview.TabIndex = 0;
            tabPageLivePreview.Text = "  Live Preview  ";
            tabPageLivePreview.UseVisualStyleBackColor = true;
            // 
            // tabPageLEDSortMatrix
            // 
            tabPageLEDSortMatrix.BackColor = Color.FromArgb(245, 247, 250);
            tabPageLEDSortMatrix.Location = new Point(4, 33);
            tabPageLEDSortMatrix.Margin = new Padding(5, 6, 5, 6);
            tabPageLEDSortMatrix.Name = "tabPageLEDSortMatrix";
            tabPageLEDSortMatrix.Padding = new Padding(5, 6, 5, 6);
            tabPageLEDSortMatrix.Size = new Size(1325, 732);
            tabPageLEDSortMatrix.TabIndex = 1;
            tabPageLEDSortMatrix.Text = "  LEDSort Matrix  ";
            tabPageLEDSortMatrix.UseVisualStyleBackColor = true;
            // 
            // ucHSR
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(tabControl1);
            Margin = new Padding(5, 6, 5, 6);
            Name = "ucHSR";
            Size = new Size(1333, 865);
            tabControl1.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageLivePreview;
        private System.Windows.Forms.TabPage tabPageLEDSortMatrix;
    }
}