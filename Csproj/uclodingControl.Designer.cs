namespace IPIS_V1
{
    partial class uclodingControl
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
            panel1 = new Panel();
            login_progressBar = new ProgressBar();
            lblMessage = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(lblMessage);
            panel1.Controls.Add(login_progressBar);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(517, 71);
            panel1.TabIndex = 0;
            // 
            // login_progressBar
            // 
            login_progressBar.Location = new Point(59, 28);
            login_progressBar.Name = "login_progressBar";
            login_progressBar.Size = new Size(344, 23);
            login_progressBar.Style = ProgressBarStyle.Marquee;
            login_progressBar.TabIndex = 0;
            // 
            // lblMessage
            // 
            lblMessage.AutoSize = true;
            lblMessage.Location = new Point(199, 10);
            lblMessage.Name = "lblMessage";
            lblMessage.Size = new Size(62, 15);
            lblMessage.TabIndex = 1;
            lblMessage.Text = "loading.....";
            // 
            // uslodingControl
            // 
            panel1.BackColor = Color.FromArgb(120, 0, 0, 0);
            login_progressBar.Anchor = AnchorStyles.None;
            lblMessage.Anchor = AnchorStyles.None;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panel1);
            Name = "uslodingControl";
            Size = new Size(517, 71);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private ProgressBar login_progressBar;
        private Label lblMessage;
    }
}
