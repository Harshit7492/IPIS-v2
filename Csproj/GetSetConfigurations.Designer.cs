namespace IPISv2
{
    partial class GetSetConfigurations
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
            components = new System.ComponentModel.Container();

            // ---- Tabs ----
            this.tabControlConfig = new System.Windows.Forms.TabControl();
            this.tabDisplayBoards = new System.Windows.Forms.TabPage();
            this.tabPdc = new System.Windows.Forms.TabPage();

            // ---- PDC test control (separate UserControl) ----
            this.pdcTest = new IPISv2.PDCAndCGDBPortTest();

            // ---- Board Testing layout ----
            this.tlpBoard = new System.Windows.Forms.TableLayoutPanel();

            // Device Selection card
            this.pnlDevice = new System.Windows.Forms.Panel();
            this.lblDeviceTitle = new System.Windows.Forms.Label();
            this.lblBoardType = new System.Windows.Forms.Label();
            this.cmbBoardType = new System.Windows.Forms.ComboBox();
            this.lblDeviceIp = new System.Windows.Forms.Label();
            this.txtDeviceIp = new System.Windows.Forms.TextBox();
            this.lblStatusCaption = new System.Windows.Forms.Label();
            this.lblStatusBadge = new System.Windows.Forms.Label();

            // Command & Communication card
            this.pnlCommand = new System.Windows.Forms.Panel();
            this.lblCommandTitle = new System.Windows.Forms.Label();
            this.lblMode = new System.Windows.Forms.Label();
            this.rbRest = new System.Windows.Forms.RadioButton();
            this.lblSelectCommand = new System.Windows.Forms.Label();
            this.cmbCommand = new System.Windows.Forms.ComboBox();
            this.lblInt = new System.Windows.Forms.Label();
            this.cmbInt = new System.Windows.Forms.ComboBox();
            this.lblDto = new System.Windows.Forms.Label();
            this.cmbDto = new System.Windows.Forms.ComboBox();
            this.btnPreview = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.lblNote = new System.Windows.Forms.Label();

            // Sent / Received (Board)
            this.tlpData = new System.Windows.Forms.TableLayoutPanel();
            this.pnlSent = new System.Windows.Forms.Panel();
            this.lblSentTitle = new System.Windows.Forms.Label();
            this.txtSent = new System.Windows.Forms.TextBox();
            this.pnlReceived = new System.Windows.Forms.Panel();
            this.lblReceivedTitle = new System.Windows.Forms.Label();
            this.txtReceived = new System.Windows.Forms.TextBox();

            this.tabControlConfig.SuspendLayout();
            this.tabDisplayBoards.SuspendLayout();
            this.tabPdc.SuspendLayout();
            this.tlpBoard.SuspendLayout();
            this.pnlDevice.SuspendLayout();
            this.pnlCommand.SuspendLayout();
            this.tlpData.SuspendLayout();
            this.pnlSent.SuspendLayout();
            this.pnlReceived.SuspendLayout();
            this.SuspendLayout();

            // 
            // tlpBoard  (3 rows: device card, command card, data panels)
            // 
            this.tlpBoard.Padding = new System.Windows.Forms.Padding(8);
            this.tlpBoard.ColumnCount = 1;
            this.tlpBoard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBoard.Controls.Add(this.pnlDevice, 0, 0);
            this.tlpBoard.Controls.Add(this.pnlCommand, 0, 1);
            this.tlpBoard.Controls.Add(this.tlpData, 0, 2);
            this.tlpBoard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpBoard.MinimumSize = new System.Drawing.Size(740, 530);
            this.tlpBoard.Name = "tlpBoard";
            this.tlpBoard.RowCount = 3;
            this.tlpBoard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 96F));
            this.tlpBoard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 262F));
            this.tlpBoard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            // 
            // pnlDevice
            // 
            this.pnlDevice.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlDevice.Controls.Add(this.txtDeviceIp);
            this.pnlDevice.Controls.Add(this.lblDeviceIp);
            this.pnlDevice.Controls.Add(this.cmbBoardType);
            this.pnlDevice.Controls.Add(this.lblBoardType);
            this.pnlDevice.Controls.Add(this.lblDeviceTitle);
            this.pnlDevice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDevice.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.pnlDevice.Name = "pnlDevice";

            this.lblDeviceTitle.AutoSize = false;
            this.lblDeviceTitle.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold);
            this.lblDeviceTitle.BackColor = System.Drawing.Color.Beige;
            this.lblDeviceTitle.ForeColor = System.Drawing.Color.Maroon;
            this.lblDeviceTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDeviceTitle.Size = new System.Drawing.Size(100, 28);
            this.lblDeviceTitle.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.lblDeviceTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblDeviceTitle.Name = "lblDeviceTitle";
            this.lblDeviceTitle.Text = "Device Selection";

            this.lblBoardType.AutoSize = true;
            this.lblBoardType.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.lblBoardType.Location = new System.Drawing.Point(10, 36);
            this.lblBoardType.Name = "lblBoardType";
            this.lblBoardType.Text = "Board Type";

            this.cmbBoardType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoardType.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.cmbBoardType.Location = new System.Drawing.Point(12, 58);
            this.cmbBoardType.Name = "cmbBoardType";
            this.cmbBoardType.Size = new System.Drawing.Size(340, 25);
            this.cmbBoardType.SelectedIndexChanged += new System.EventHandler(this.cmbBoardType_SelectedIndexChanged);

            this.lblDeviceIp.AutoSize = true;
            this.lblDeviceIp.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.lblDeviceIp.Location = new System.Drawing.Point(375, 36);
            this.lblDeviceIp.Name = "lblDeviceIp";
            this.lblDeviceIp.Text = "Device IP";

            this.txtDeviceIp.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.txtDeviceIp.Location = new System.Drawing.Point(377, 58);
            this.txtDeviceIp.Name = "txtDeviceIp";
            this.txtDeviceIp.ReadOnly = false;
            this.txtDeviceIp.Size = new System.Drawing.Size(340, 25);
            this.txtDeviceIp.Text = "10.0.0.101";
            this.txtDeviceIp.TextChanged += new System.EventHandler(this.txtDeviceIp_TextChanged);

            this.lblStatusCaption.AutoSize = true;
            this.lblStatusCaption.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.lblStatusCaption.Location = new System.Drawing.Point(10, 94);
            this.lblStatusCaption.Name = "lblStatusCaption";
            this.lblStatusCaption.Text = "Device status";
            this.lblStatusCaption.Visible = false;

            this.lblStatusBadge.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.lblStatusBadge.AutoSize = true;
            this.lblStatusBadge.BackColor = System.Drawing.Color.FromArgb(230, 233, 238);
            this.lblStatusBadge.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblStatusBadge.Location = new System.Drawing.Point(650, 92);
            this.lblStatusBadge.Name = "lblStatusBadge";
            this.lblStatusBadge.Padding = new System.Windows.Forms.Padding(6, 2, 6, 2);
            this.lblStatusBadge.Text = "Not tested";
            this.lblStatusBadge.Visible = false;
            // 
            // pnlCommand
            // 
            this.pnlCommand.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlCommand.Controls.Add(this.lblNote);
            this.pnlCommand.Controls.Add(this.btnClear);
            this.pnlCommand.Controls.Add(this.btnPreview);
            this.pnlCommand.Controls.Add(this.cmbCommand);
            this.pnlCommand.Controls.Add(this.lblInt);
            this.pnlCommand.Controls.Add(this.cmbInt);
            this.pnlCommand.Controls.Add(this.lblDto);
            this.pnlCommand.Controls.Add(this.cmbDto);
            this.pnlCommand.Controls.Add(this.lblSelectCommand);
            this.pnlCommand.Controls.Add(this.rbRest);
            this.pnlCommand.Controls.Add(this.lblMode);
            this.pnlCommand.Controls.Add(this.lblCommandTitle);
            this.pnlCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCommand.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.pnlCommand.Name = "pnlCommand";
            this.pnlCommand.Size = new System.Drawing.Size(764, 256);

            this.lblCommandTitle.AutoSize = false;
            this.lblCommandTitle.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold);
            this.lblCommandTitle.BackColor = System.Drawing.Color.Beige;
            this.lblCommandTitle.ForeColor = System.Drawing.Color.Maroon;
            this.lblCommandTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCommandTitle.Size = new System.Drawing.Size(100, 28);
            this.lblCommandTitle.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.lblCommandTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblCommandTitle.Name = "lblCommandTitle";
            this.lblCommandTitle.Text = "Command & Communication";

            this.lblMode.AutoSize = true;
            this.lblMode.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.lblMode.Location = new System.Drawing.Point(10, 36);
            this.lblMode.Name = "lblMode";
            this.lblMode.Text = "Communication Mode";

            this.rbRest.AutoSize = true;
            this.rbRest.Checked = true;
            this.rbRest.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.rbRest.Location = new System.Drawing.Point(12, 58);
            this.rbRest.Name = "rbRest";
            this.rbRest.TabStop = true;
            this.rbRest.Text = "REST API / JSON — Version 2.0";
            this.rbRest.CheckedChanged += new System.EventHandler(this.rbMode_CheckedChanged);

            this.lblSelectCommand.AutoSize = true;
            this.lblSelectCommand.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.lblSelectCommand.Location = new System.Drawing.Point(10, 88);
            this.lblSelectCommand.Name = "lblSelectCommand";
            this.lblSelectCommand.Text = "Select Command";

            this.cmbCommand.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.cmbCommand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCommand.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.cmbCommand.Location = new System.Drawing.Point(12, 110);
            this.cmbCommand.Name = "cmbCommand";
            this.cmbCommand.Size = new System.Drawing.Size(705, 25);
            this.cmbCommand.SelectedIndexChanged += new System.EventHandler(this.cmbCommand_SelectedIndexChanged);

            // Set Configuration inputs (shown only when "Set Configuration" is selected)
            this.lblInt.AutoSize = true;
            this.lblInt.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.lblInt.Location = new System.Drawing.Point(10, 142);
            this.lblInt.Name = "lblInt";
            this.lblInt.Text = "INT (Intensity)";

            this.cmbInt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbInt.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.cmbInt.Location = new System.Drawing.Point(12, 164);
            this.cmbInt.Name = "cmbInt";
            this.cmbInt.Size = new System.Drawing.Size(200, 25);

            this.lblDto.AutoSize = true;
            this.lblDto.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.lblDto.Location = new System.Drawing.Point(230, 142);
            this.lblDto.Name = "lblDto";
            this.lblDto.Text = "DTO (seconds)";

            // DTO in seconds: 60 to 240 in steps of 30
            this.cmbDto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDto.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.cmbDto.Items.AddRange(new object[] { "60", "90", "120", "150", "180", "210", "240" });
            this.cmbDto.Location = new System.Drawing.Point(232, 164);
            this.cmbDto.Name = "cmbDto";
            this.cmbDto.Size = new System.Drawing.Size(200, 25);

            this.btnPreview.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.btnPreview.Location = new System.Drawing.Point(12, 198);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(345, 34);
            this.btnPreview.Text = "Preview Request";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);

            this.btnClear.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.btnClear.Location = new System.Drawing.Point(372, 198);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(345, 34);
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);

            this.lblNote.AutoSize = true;
            this.lblNote.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblNote.ForeColor = System.Drawing.Color.FromArgb(90, 100, 115);
            this.lblNote.Location = new System.Drawing.Point(10, 238);
            this.lblNote.Name = "lblNote";
            this.lblNote.Text = "Get and Set Configuration are sent to the board. Soft Reset updates RST_Soft_Reset in BOARDSETTING.";
            // 
            // tlpData  (Sent | Received)
            // 
            this.tlpData.ColumnCount = 2;
            this.tlpData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpData.Controls.Add(this.pnlSent, 0, 0);
            this.tlpData.Controls.Add(this.pnlReceived, 1, 0);
            this.tlpData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpData.Margin = new System.Windows.Forms.Padding(0);
            this.tlpData.MinimumSize = new System.Drawing.Size(0, 150);
            this.tlpData.Name = "tlpData";
            this.tlpData.RowCount = 1;
            this.tlpData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            // 
            // pnlSent
            // 
            this.pnlSent.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlSent.Controls.Add(this.txtSent);
            this.pnlSent.Controls.Add(this.lblSentTitle);
            this.pnlSent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSent.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.pnlSent.Name = "pnlSent";
            this.pnlSent.Size = new System.Drawing.Size(362, 150);

            this.lblSentTitle.AutoSize = false;
            this.lblSentTitle.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold);
            this.lblSentTitle.BackColor = System.Drawing.Color.Beige;
            this.lblSentTitle.ForeColor = System.Drawing.Color.Maroon;
            this.lblSentTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSentTitle.Size = new System.Drawing.Size(100, 28);
            this.lblSentTitle.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.lblSentTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblSentTitle.Name = "lblSentTitle";
            this.lblSentTitle.Text = "Sent Data";

            this.txtSent.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.txtSent.BackColor = System.Drawing.SystemColors.Window;
            this.txtSent.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtSent.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtSent.Location = new System.Drawing.Point(10, 38);
            this.txtSent.Multiline = true;
            this.txtSent.Name = "txtSent";
            this.txtSent.ReadOnly = true;
            this.txtSent.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSent.Size = new System.Drawing.Size(340, 100);
            // 
            // pnlReceived
            // 
            this.pnlReceived.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlReceived.Controls.Add(this.txtReceived);
            this.pnlReceived.Controls.Add(this.lblReceivedTitle);
            this.pnlReceived.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlReceived.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.pnlReceived.Name = "pnlReceived";
            this.pnlReceived.Size = new System.Drawing.Size(362, 150);

            this.lblReceivedTitle.AutoSize = false;
            this.lblReceivedTitle.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold);
            this.lblReceivedTitle.BackColor = System.Drawing.Color.Beige;
            this.lblReceivedTitle.ForeColor = System.Drawing.Color.Maroon;
            this.lblReceivedTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblReceivedTitle.Size = new System.Drawing.Size(100, 28);
            this.lblReceivedTitle.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.lblReceivedTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblReceivedTitle.Name = "lblReceivedTitle";
            this.lblReceivedTitle.Text = "Received Data";

            this.txtReceived.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.txtReceived.BackColor = System.Drawing.SystemColors.Window;
            this.txtReceived.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtReceived.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtReceived.Location = new System.Drawing.Point(10, 38);
            this.txtReceived.Multiline = true;
            this.txtReceived.Name = "txtReceived";
            this.txtReceived.ReadOnly = true;
            this.txtReceived.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtReceived.Size = new System.Drawing.Size(340, 100);
            // 
            // pdcTest  (PDCAndCGDBPortTest user control)
            // 
            this.pdcTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pdcTest.Location = new System.Drawing.Point(4, 4);
            this.pdcTest.MinimumSize = new System.Drawing.Size(740, 560);
            this.pdcTest.Name = "pdcTest";
            this.pdcTest.Size = new System.Drawing.Size(764, 712);
            this.pdcTest.TabIndex = 0;
            // 
            // tabControlConfig
            // 
            this.tabControlConfig.Controls.Add(this.tabDisplayBoards);
            this.tabControlConfig.Controls.Add(this.tabPdc);
            this.tabControlConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlConfig.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.tabControlConfig.ItemSize = new System.Drawing.Size(200, 32);
            this.tabControlConfig.Location = new System.Drawing.Point(0, 0);
            this.tabControlConfig.Name = "tabControlConfig";
            this.tabControlConfig.SelectedIndex = 0;
            this.tabControlConfig.Size = new System.Drawing.Size(780, 760);
            this.tabControlConfig.TabIndex = 0;
            // 
            // tabDisplayBoards
            // 
            this.tabDisplayBoards.AutoScroll = true;
            this.tabDisplayBoards.AutoScrollMinSize = new System.Drawing.Size(740, 530);
            this.tabDisplayBoards.Controls.Add(this.tlpBoard);
            this.tabDisplayBoards.Location = new System.Drawing.Point(4, 36);
            this.tabDisplayBoards.Name = "tabDisplayBoards";
            this.tabDisplayBoards.Padding = new System.Windows.Forms.Padding(4);
            this.tabDisplayBoards.Size = new System.Drawing.Size(772, 720);
            this.tabDisplayBoards.Text = "Display Boards";
            this.tabDisplayBoards.UseVisualStyleBackColor = true;
            // 
            // tabPdc
            // 
            this.tabPdc.AutoScroll = true;
            this.tabPdc.AutoScrollMinSize = new System.Drawing.Size(740, 560);
            this.tabPdc.Controls.Add(this.pdcTest);
            this.tabPdc.Location = new System.Drawing.Point(4, 36);
            this.tabPdc.Name = "tabPdc";
            this.tabPdc.Padding = new System.Windows.Forms.Padding(4);
            this.tabPdc.Size = new System.Drawing.Size(772, 720);
            this.tabPdc.Text = "Platform Data Controller (PDC)";
            this.tabPdc.UseVisualStyleBackColor = true;
            // 
            // GetSetConfigurations
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlConfig);
            this.Name = "GetSetConfigurations";
            this.Size = new System.Drawing.Size(780, 760);

            this.pnlReceived.ResumeLayout(false);
            this.pnlReceived.PerformLayout();
            this.pnlSent.ResumeLayout(false);
            this.pnlSent.PerformLayout();
            this.tlpData.ResumeLayout(false);
            this.pnlCommand.ResumeLayout(false);
            this.pnlCommand.PerformLayout();
            this.pnlDevice.ResumeLayout(false);
            this.pnlDevice.PerformLayout();
            this.tlpBoard.ResumeLayout(false);
            this.tabDisplayBoards.ResumeLayout(false);
            this.tabPdc.ResumeLayout(false);
            this.tabControlConfig.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        // Tabs
        private System.Windows.Forms.TabControl tabControlConfig;
        private System.Windows.Forms.TabPage tabDisplayBoards;
        private System.Windows.Forms.TabPage tabPdc;

        // PDC test (separate control)
        private IPISv2.PDCAndCGDBPortTest pdcTest;

        // Board testing
        private System.Windows.Forms.TableLayoutPanel tlpBoard;
        private System.Windows.Forms.Panel pnlDevice;
        private System.Windows.Forms.Label lblDeviceTitle;
        private System.Windows.Forms.Label lblBoardType;
        private System.Windows.Forms.ComboBox cmbBoardType;
        private System.Windows.Forms.Label lblDeviceIp;
        private System.Windows.Forms.TextBox txtDeviceIp;
        private System.Windows.Forms.Label lblStatusCaption;
        private System.Windows.Forms.Label lblStatusBadge;

        private System.Windows.Forms.Panel pnlCommand;
        private System.Windows.Forms.Label lblCommandTitle;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.RadioButton rbRest;
        private System.Windows.Forms.Label lblSelectCommand;
        private System.Windows.Forms.ComboBox cmbCommand;
        private System.Windows.Forms.Label lblInt;
        private System.Windows.Forms.ComboBox cmbInt;
        private System.Windows.Forms.Label lblDto;
        private System.Windows.Forms.ComboBox cmbDto;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label lblNote;

        private System.Windows.Forms.TableLayoutPanel tlpData;
        private System.Windows.Forms.Panel pnlSent;
        private System.Windows.Forms.Label lblSentTitle;
        private System.Windows.Forms.TextBox txtSent;
        private System.Windows.Forms.Panel pnlReceived;
        private System.Windows.Forms.Label lblReceivedTitle;
        private System.Windows.Forms.TextBox txtReceived;
    }
}