namespace IPIS
{
    partial class ucUpdateExe
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

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.tableButtons = new System.Windows.Forms.TableLayoutPanel();
            this.btnPendrive = new System.Windows.Forms.Button();
            this.btnServer = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.panelHeader.SuspendLayout();
            this.tableButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.DarkBlue;
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(983, 60);
            this.panelHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(20, 14);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(160, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Update EXE";
            // 
            // tableButtons
            // 
            this.tableButtons.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableButtons.ColumnCount = 2;
            this.tableButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableButtons.Controls.Add(this.btnPendrive, 0, 0);
            this.tableButtons.Controls.Add(this.btnServer, 1, 0);
            this.tableButtons.Location = new System.Drawing.Point(190, 200);
            this.tableButtons.Name = "tableButtons";
            this.tableButtons.RowCount = 1;
            this.tableButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableButtons.Size = new System.Drawing.Size(600, 180);
            this.tableButtons.TabIndex = 1;
            // 
            // btnPendrive
            // 
            this.btnPendrive.BackColor = System.Drawing.Color.AliceBlue;
            this.btnPendrive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPendrive.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPendrive.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnPendrive.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnPendrive.Location = new System.Drawing.Point(20, 20);
            this.btnPendrive.Margin = new System.Windows.Forms.Padding(20);
            this.btnPendrive.Name = "btnPendrive";
            this.btnPendrive.Size = new System.Drawing.Size(260, 140);
            this.btnPendrive.TabIndex = 0;
            this.btnPendrive.Text = "Update from Pendrive";
            this.btnPendrive.UseVisualStyleBackColor = false;
            this.btnPendrive.Click += new System.EventHandler(this.btnPendrive_Click);
            // 
            // btnServer
            // 
            this.btnServer.BackColor = System.Drawing.Color.AliceBlue;
            this.btnServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnServer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnServer.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnServer.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnServer.Location = new System.Drawing.Point(320, 20);
            this.btnServer.Margin = new System.Windows.Forms.Padding(20);
            this.btnServer.Name = "btnServer";
            this.btnServer.Size = new System.Drawing.Size(260, 140);
            this.btnServer.TabIndex = 1;
            this.btnServer.Text = "Update from Server";
            this.btnServer.UseVisualStyleBackColor = false;
            this.btnServer.Click += new System.EventHandler(this.btnServer_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            this.lblStatus.ForeColor = System.Drawing.Color.DimGray;
            this.lblStatus.Location = new System.Drawing.Point(190, 400);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(600, 30);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "Ready.";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.progressBar.Location = new System.Drawing.Point(190, 440);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(600, 25);
            this.progressBar.TabIndex = 3;
            this.progressBar.Visible = false;
            // 
            // ucUpdateExe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.tableButtons);
            this.Controls.Add(this.panelHeader);
            this.Name = "ucUpdateExe";
            this.Size = new System.Drawing.Size(983, 601);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.tableButtons.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TableLayoutPanel tableButtons;
        private System.Windows.Forms.Button btnPendrive;
        private System.Windows.Forms.Button btnServer;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}