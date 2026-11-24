namespace IPISv2
{
    partial class PDCAndCGDBPortTest
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

            this.tlpPdc = new System.Windows.Forms.TableLayoutPanel();

            this.pnlPdcDevice = new System.Windows.Forms.Panel();
            this.lblPdcDeviceTitle = new System.Windows.Forms.Label();
            this.lblPdcSelect = new System.Windows.Forms.Label();
            this.cmbPdcList = new System.Windows.Forms.ComboBox();
            this.lblPdcIp = new System.Windows.Forms.Label();
            this.txtPdcIp = new System.Windows.Forms.TextBox();
            this.lblPdcStatusCaption = new System.Windows.Forms.Label();
            this.lblPdcStatusBadge = new System.Windows.Forms.Label();

            this.pnlPdcCommand = new System.Windows.Forms.Panel();
            this.lblPdcCommandTitle = new System.Windows.Forms.Label();
            this.lblPdcCommand = new System.Windows.Forms.Label();
            this.cmbPdcCommand = new System.Windows.Forms.ComboBox();

            // CGDB board selection controls
            this.lblCgdbBoardIp = new System.Windows.Forms.Label();
            this.cmbCgdbBoardIp = new System.Windows.Forms.ComboBox();

            this.lblPdcInt = new System.Windows.Forms.Label();
            this.cmbPdcInt = new System.Windows.Forms.ComboBox();
            this.lblPdcDto = new System.Windows.Forms.Label();
            this.cmbPdcDto = new System.Windows.Forms.ComboBox();

            // Port and Asset ID textboxes
            this.lblPdcPort = new System.Windows.Forms.Label();
            this.txtPdcPort = new System.Windows.Forms.TextBox();
            this.lblCgdbAssetId = new System.Windows.Forms.Label();
            this.txtCgdbAssetId = new System.Windows.Forms.TextBox();

            this.btnPdcExecute = new System.Windows.Forms.Button();
            this.btnPdcClear = new System.Windows.Forms.Button();
            this.lblPdcNote = new System.Windows.Forms.Label();

            this.tlpPdcData = new System.Windows.Forms.TableLayoutPanel();

            this.pnlPdcSent = new System.Windows.Forms.Panel();
            this.lblPdcSentTitle = new System.Windows.Forms.Label();
            this.txtPdcSent = new System.Windows.Forms.TextBox();

            this.pnlPdcReceived = new System.Windows.Forms.Panel();
            this.lblPdcReceivedTitle = new System.Windows.Forms.Label();
            this.txtPdcReceived = new System.Windows.Forms.TextBox();

            this.tlpPdc.SuspendLayout();
            this.pnlPdcDevice.SuspendLayout();
            this.pnlPdcCommand.SuspendLayout();
            this.tlpPdcData.SuspendLayout();
            this.pnlPdcSent.SuspendLayout();
            this.pnlPdcReceived.SuspendLayout();
            this.SuspendLayout();

            // 
            // tlpPdc
            // 
            this.tlpPdc.Padding = new System.Windows.Forms.Padding(8);
            this.tlpPdc.ColumnCount = 1;

            this.tlpPdc.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(
                    System.Windows.Forms.SizeType.Percent,
                    100F));

            this.tlpPdc.Controls.Add(this.pnlPdcDevice, 0, 0);
            this.tlpPdc.Controls.Add(this.pnlPdcCommand, 0, 1);
            this.tlpPdc.Controls.Add(this.tlpPdcData, 0, 2);

            this.tlpPdc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPdc.MinimumSize =
                new System.Drawing.Size(740, 560);
            this.tlpPdc.Name = "tlpPdc";
            this.tlpPdc.RowCount = 3;

            this.tlpPdc.RowStyles.Add(
                new System.Windows.Forms.RowStyle(
                    System.Windows.Forms.SizeType.Absolute,
                    96F));

            this.tlpPdc.RowStyles.Add(
                new System.Windows.Forms.RowStyle(
                    System.Windows.Forms.SizeType.Absolute,
                    280F));

            this.tlpPdc.RowStyles.Add(
                new System.Windows.Forms.RowStyle(
                    System.Windows.Forms.SizeType.Percent,
                    100F));

            // 
            // pnlPdcDevice
            // 
            this.pnlPdcDevice.BorderStyle =
                System.Windows.Forms.BorderStyle.Fixed3D;

            this.pnlPdcDevice.Controls.Add(this.txtPdcIp);
            this.pnlPdcDevice.Controls.Add(this.lblPdcIp);
            this.pnlPdcDevice.Controls.Add(this.cmbPdcList);
            this.pnlPdcDevice.Controls.Add(this.lblPdcSelect);
            this.pnlPdcDevice.Controls.Add(this.lblPdcDeviceTitle);

            this.pnlPdcDevice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPdcDevice.Margin =
                new System.Windows.Forms.Padding(0, 0, 0, 8);

            this.pnlPdcDevice.Name = "pnlPdcDevice";

            // lblPdcDeviceTitle
            this.lblPdcDeviceTitle.Dock =
                System.Windows.Forms.DockStyle.Top;

            this.lblPdcDeviceTitle.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10.8F,
                    System.Drawing.FontStyle.Bold);

            this.lblPdcDeviceTitle.BackColor =
                System.Drawing.Color.Beige;

            this.lblPdcDeviceTitle.ForeColor =
                System.Drawing.Color.Maroon;

            this.lblPdcDeviceTitle.Size =
                new System.Drawing.Size(100, 28);

            this.lblPdcDeviceTitle.Padding =
                new System.Windows.Forms.Padding(6, 0, 0, 0);

            this.lblPdcDeviceTitle.TextAlign =
                System.Drawing.ContentAlignment.MiddleLeft;

            this.lblPdcDeviceTitle.Name = "lblPdcDeviceTitle";
            this.lblPdcDeviceTitle.Text = "Platform Hub Selection";

            // lblPdcSelect
            this.lblPdcSelect.AutoSize = true;

            this.lblPdcSelect.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10.2F,
                    System.Drawing.FontStyle.Bold);

            this.lblPdcSelect.Location =
                new System.Drawing.Point(10, 36);

            this.lblPdcSelect.Name = "lblPdcSelect";
            this.lblPdcSelect.Text = "Platform Hub";

            // cmbPdcList
            this.cmbPdcList.DropDownStyle =
                System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.cmbPdcList.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10.2F,
                    System.Drawing.FontStyle.Bold);

            this.cmbPdcList.Location =
                new System.Drawing.Point(12, 58);

            this.cmbPdcList.Name = "cmbPdcList";

            this.cmbPdcList.Size =
                new System.Drawing.Size(340, 25);

            this.cmbPdcList.SelectedIndexChanged +=
                new System.EventHandler(
                    this.cmbPdcList_SelectedIndexChanged);

            // lblPdcIp
            this.lblPdcIp.AutoSize = true;

            this.lblPdcIp.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10.2F,
                    System.Drawing.FontStyle.Bold);

            this.lblPdcIp.Location =
                new System.Drawing.Point(375, 36);

            this.lblPdcIp.Name = "lblPdcIp";
            this.lblPdcIp.Text = "PDC IP Address";

            // txtPdcIp
            this.txtPdcIp.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10.2F,
                    System.Drawing.FontStyle.Bold);

            this.txtPdcIp.Location =
                new System.Drawing.Point(377, 58);

            this.txtPdcIp.Name = "txtPdcIp";
            this.txtPdcIp.ReadOnly = true;

            this.txtPdcIp.Size =
                new System.Drawing.Size(340, 25);

            this.txtPdcIp.TextChanged +=
                new System.EventHandler(
                    this.txtPdcIp_TextChanged);

            // lblPdcStatusCaption
            this.lblPdcStatusCaption.AutoSize = true;

            this.lblPdcStatusCaption.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10.2F,
                    System.Drawing.FontStyle.Bold);

            this.lblPdcStatusCaption.Location =
                new System.Drawing.Point(10, 94);

            this.lblPdcStatusCaption.Name = "lblPdcStatusCaption";
            this.lblPdcStatusCaption.Text = "PDC status";
            this.lblPdcStatusCaption.Visible = false;

            // lblPdcStatusBadge
            this.lblPdcStatusBadge.Anchor =
                System.Windows.Forms.AnchorStyles.Top |
                System.Windows.Forms.AnchorStyles.Right;

            this.lblPdcStatusBadge.AutoSize = true;

            this.lblPdcStatusBadge.BackColor =
                System.Drawing.Color.FromArgb(230, 233, 238);

            this.lblPdcStatusBadge.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    8F,
                    System.Drawing.FontStyle.Bold);

            this.lblPdcStatusBadge.Location =
                new System.Drawing.Point(650, 92);

            this.lblPdcStatusBadge.Name = "lblPdcStatusBadge";

            this.lblPdcStatusBadge.Padding =
                new System.Windows.Forms.Padding(6, 2, 6, 2);

            this.lblPdcStatusBadge.Text = "Not tested";
            this.lblPdcStatusBadge.Visible = false;

            // 
            // pnlPdcCommand
            // 
            this.pnlPdcCommand.BorderStyle =
                System.Windows.Forms.BorderStyle.Fixed3D;

            this.pnlPdcCommand.Controls.Add(this.lblPdcNote);
            this.pnlPdcCommand.Controls.Add(this.btnPdcClear);
            this.pnlPdcCommand.Controls.Add(this.btnPdcExecute);

            // CGDB board controls
            this.pnlPdcCommand.Controls.Add(this.cmbCgdbBoardIp);
            this.pnlPdcCommand.Controls.Add(this.lblCgdbBoardIp);

            // Port + Asset ID textboxes
            this.pnlPdcCommand.Controls.Add(this.txtCgdbAssetId);
            this.pnlPdcCommand.Controls.Add(this.lblCgdbAssetId);
            this.pnlPdcCommand.Controls.Add(this.txtPdcPort);
            this.pnlPdcCommand.Controls.Add(this.lblPdcPort);

            this.pnlPdcCommand.Controls.Add(this.cmbPdcDto);
            this.pnlPdcCommand.Controls.Add(this.lblPdcDto);

            this.pnlPdcCommand.Controls.Add(this.cmbPdcInt);
            this.pnlPdcCommand.Controls.Add(this.lblPdcInt);

            this.pnlPdcCommand.Controls.Add(this.cmbPdcCommand);
            this.pnlPdcCommand.Controls.Add(this.lblPdcCommand);
            this.pnlPdcCommand.Controls.Add(this.lblPdcCommandTitle);

            this.pnlPdcCommand.Dock =
                System.Windows.Forms.DockStyle.Fill;

            this.pnlPdcCommand.Margin =
                new System.Windows.Forms.Padding(0, 0, 0, 8);

            this.pnlPdcCommand.Name = "pnlPdcCommand";
            this.pnlPdcCommand.Size =
                new System.Drawing.Size(764, 274);

            // lblPdcCommandTitle
            this.lblPdcCommandTitle.Dock =
                System.Windows.Forms.DockStyle.Top;

            this.lblPdcCommandTitle.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10.8F,
                    System.Drawing.FontStyle.Bold);

            this.lblPdcCommandTitle.BackColor =
                System.Drawing.Color.Beige;

            this.lblPdcCommandTitle.ForeColor =
                System.Drawing.Color.Maroon;

            this.lblPdcCommandTitle.Size =
                new System.Drawing.Size(100, 28);

            this.lblPdcCommandTitle.Padding =
                new System.Windows.Forms.Padding(6, 0, 0, 0);

            this.lblPdcCommandTitle.TextAlign =
                System.Drawing.ContentAlignment.MiddleLeft;

            this.lblPdcCommandTitle.Name = "lblPdcCommandTitle";
            this.lblPdcCommandTitle.Text =
                "Command & Configuration";

            // lblPdcCommand
            this.lblPdcCommand.AutoSize = true;

            this.lblPdcCommand.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10.2F,
                    System.Drawing.FontStyle.Bold);

            this.lblPdcCommand.Location =
                new System.Drawing.Point(10, 36);

            this.lblPdcCommand.Name = "lblPdcCommand";
            this.lblPdcCommand.Text = "Select Command";

            // cmbPdcCommand
            this.cmbPdcCommand.Anchor =
                System.Windows.Forms.AnchorStyles.Top |
                System.Windows.Forms.AnchorStyles.Left |
                System.Windows.Forms.AnchorStyles.Right;

            this.cmbPdcCommand.DropDownStyle =
                System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.cmbPdcCommand.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10.2F,
                    System.Drawing.FontStyle.Bold);

            this.cmbPdcCommand.Location =
                new System.Drawing.Point(12, 58);

            this.cmbPdcCommand.Name = "cmbPdcCommand";

            this.cmbPdcCommand.Size =
                new System.Drawing.Size(705, 25);

            this.cmbPdcCommand.SelectedIndexChanged +=
                new System.EventHandler(
                    this.cmbPdcCommand_SelectedIndexChanged);

            // 
            // lblCgdbBoardIp
            // 
            this.lblCgdbBoardIp.AutoSize = true;

            this.lblCgdbBoardIp.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10.2F,
                    System.Drawing.FontStyle.Bold);

            this.lblCgdbBoardIp.Location =
                new System.Drawing.Point(10, 88);

            this.lblCgdbBoardIp.Name = "lblCgdbBoardIp";
            this.lblCgdbBoardIp.Text = "Select CGDB Board IP";
            this.lblCgdbBoardIp.Visible = false;

            // 
            // cmbCgdbBoardIp
            // 
            this.cmbCgdbBoardIp.Anchor =
                System.Windows.Forms.AnchorStyles.Top |
                System.Windows.Forms.AnchorStyles.Left |
                System.Windows.Forms.AnchorStyles.Right;

            this.cmbCgdbBoardIp.DropDownStyle =
                System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.cmbCgdbBoardIp.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10.2F,
                    System.Drawing.FontStyle.Bold);

            this.cmbCgdbBoardIp.Location =
                new System.Drawing.Point(12, 110);

            this.cmbCgdbBoardIp.Name = "cmbCgdbBoardIp";

            // *** WIRE THE EVENT — this was missing ***
            this.cmbCgdbBoardIp.SelectedIndexChanged +=
                new System.EventHandler(
                    this.cmbCgdbBoardIp_SelectedIndexChanged);

            this.cmbCgdbBoardIp.Size =
                new System.Drawing.Size(705, 25);

            this.cmbCgdbBoardIp.Visible = false;

            // 
            // lblPdcInt
            // 
            this.lblPdcInt.AutoSize = true;

            this.lblPdcInt.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10.2F,
                    System.Drawing.FontStyle.Bold);

            this.lblPdcInt.Location =
                new System.Drawing.Point(10, 142);

            this.lblPdcInt.Name = "lblPdcInt";
            this.lblPdcInt.Text = "INT (Intensity)";
            this.lblPdcInt.Visible = false;

            // cmbPdcInt
            this.cmbPdcInt.DropDownStyle =
                System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.cmbPdcInt.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10.2F,
                    System.Drawing.FontStyle.Bold);

            this.cmbPdcInt.Location =
                new System.Drawing.Point(12, 164);

            this.cmbPdcInt.Name = "cmbPdcInt";

            this.cmbPdcInt.Size =
                new System.Drawing.Size(200, 25);

            this.cmbPdcInt.Visible = false;

            // 
            // lblPdcDto
            // 
            this.lblPdcDto.AutoSize = true;

            this.lblPdcDto.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10.2F,
                    System.Drawing.FontStyle.Bold);

            this.lblPdcDto.Location =
                new System.Drawing.Point(230, 142);

            this.lblPdcDto.Name = "lblPdcDto";
            this.lblPdcDto.Text = "DTO (seconds)";
            this.lblPdcDto.Visible = false;

            // cmbPdcDto
            this.cmbPdcDto.DropDownStyle =
                System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.cmbPdcDto.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10.2F,
                    System.Drawing.FontStyle.Bold);

            this.cmbPdcDto.Items.AddRange(
                new object[]
                {
                    "30", "60", "90", "120",
                    "150", "180", "210", "240"
                });

            this.cmbPdcDto.Location =
                new System.Drawing.Point(232, 164);

            this.cmbPdcDto.Name = "cmbPdcDto";

            this.cmbPdcDto.Size =
                new System.Drawing.Size(150, 25);

            this.cmbPdcDto.Visible = false;

            // 
            // lblPdcPort
            // 
            this.lblPdcPort.AutoSize = true;

            this.lblPdcPort.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10.2F,
                    System.Drawing.FontStyle.Bold);

            this.lblPdcPort.Location =
                new System.Drawing.Point(400, 142);

            this.lblPdcPort.Name = "lblPdcPort";
            this.lblPdcPort.Text = "Port";
            this.lblPdcPort.Visible = false;

            // 
            // txtPdcPort
            // 
            this.txtPdcPort.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10.2F,
                    System.Drawing.FontStyle.Bold);

            this.txtPdcPort.Location =
                new System.Drawing.Point(402, 164);

            this.txtPdcPort.Name = "txtPdcPort";

            this.txtPdcPort.ReadOnly = true;

            this.txtPdcPort.Size =
                new System.Drawing.Size(100, 25);

            this.txtPdcPort.Visible = false;

            // 
            // lblCgdbAssetId
            // 
            this.lblCgdbAssetId.AutoSize = true;

            this.lblCgdbAssetId.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10.2F,
                    System.Drawing.FontStyle.Bold);

            this.lblCgdbAssetId.Location =
                new System.Drawing.Point(520, 142);

            this.lblCgdbAssetId.Name = "lblCgdbAssetId";
            this.lblCgdbAssetId.Text = "Asset ID";
            this.lblCgdbAssetId.Visible = false;

            // 
            // txtCgdbAssetId
            // 
            this.txtCgdbAssetId.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10.2F,
                    System.Drawing.FontStyle.Bold);

            this.txtCgdbAssetId.Location =
                new System.Drawing.Point(522, 164);

            this.txtCgdbAssetId.Name = "txtCgdbAssetId";

            this.txtCgdbAssetId.ReadOnly = false;

            this.txtCgdbAssetId.Size =
                new System.Drawing.Size(195, 25);

            this.txtCgdbAssetId.Visible = false;

            // 
            // btnPdcExecute
            // 
            this.btnPdcExecute.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10.2F,
                    System.Drawing.FontStyle.Bold);

            this.btnPdcExecute.Location =
                new System.Drawing.Point(12, 198);

            this.btnPdcExecute.Name = "btnPdcExecute";

            this.btnPdcExecute.Size =
                new System.Drawing.Size(345, 34);

            this.btnPdcExecute.Text = "Get Configuration";
            this.btnPdcExecute.UseVisualStyleBackColor = true;

            this.btnPdcExecute.Click +=
                new System.EventHandler(
                    this.btnPdcExecute_Click);

            // 
            // btnPdcClear
            // 
            this.btnPdcClear.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10.2F,
                    System.Drawing.FontStyle.Bold);

            this.btnPdcClear.Location =
                new System.Drawing.Point(372, 198);

            this.btnPdcClear.Name = "btnPdcClear";

            this.btnPdcClear.Size =
                new System.Drawing.Size(345, 34);

            this.btnPdcClear.Text = "Clear";
            this.btnPdcClear.UseVisualStyleBackColor = true;

            this.btnPdcClear.Click +=
                new System.EventHandler(
                    this.btnPdcClear_Click);

            // 
            // lblPdcNote
            // 
            this.lblPdcNote.AutoSize = true;

            this.lblPdcNote.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    8F);

            this.lblPdcNote.ForeColor =
                System.Drawing.Color.FromArgb(90, 100, 115);

            this.lblPdcNote.Location =
                new System.Drawing.Point(10, 238);

            this.lblPdcNote.Name = "lblPdcNote";

            this.lblPdcNote.Text =
                "Get Configuration discovers CGDB units. " +
                "Set Configuration updates INT and DTO for the selected CGDB board only. " +
                "Soft Reset resets the selected CGDB board.";

            // 
            // tlpPdcData
            // 
            this.tlpPdcData.ColumnCount = 2;

            this.tlpPdcData.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(
                    System.Windows.Forms.SizeType.Percent,
                    50F));

            this.tlpPdcData.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(
                    System.Windows.Forms.SizeType.Percent,
                    50F));

            this.tlpPdcData.Controls.Add(this.pnlPdcSent, 0, 0);
            this.tlpPdcData.Controls.Add(this.pnlPdcReceived, 1, 0);

            this.tlpPdcData.Dock =
                System.Windows.Forms.DockStyle.Fill;

            this.tlpPdcData.Margin =
                new System.Windows.Forms.Padding(0);

            this.tlpPdcData.MinimumSize =
                new System.Drawing.Size(0, 150);

            this.tlpPdcData.Name = "tlpPdcData";
            this.tlpPdcData.RowCount = 1;

            this.tlpPdcData.RowStyles.Add(
                new System.Windows.Forms.RowStyle(
                    System.Windows.Forms.SizeType.Percent,
                    100F));

            // 
            // pnlPdcSent
            // 
            this.pnlPdcSent.BorderStyle =
                System.Windows.Forms.BorderStyle.Fixed3D;

            this.pnlPdcSent.Controls.Add(this.txtPdcSent);
            this.pnlPdcSent.Controls.Add(this.lblPdcSentTitle);

            this.pnlPdcSent.Dock =
                System.Windows.Forms.DockStyle.Fill;

            this.pnlPdcSent.Margin =
                new System.Windows.Forms.Padding(0, 0, 4, 0);

            this.pnlPdcSent.Name = "pnlPdcSent";

            // lblPdcSentTitle
            this.lblPdcSentTitle.Dock =
                System.Windows.Forms.DockStyle.Top;

            this.lblPdcSentTitle.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10.8F,
                    System.Drawing.FontStyle.Bold);

            this.lblPdcSentTitle.BackColor =
                System.Drawing.Color.Beige;

            this.lblPdcSentTitle.ForeColor =
                System.Drawing.Color.Maroon;

            this.lblPdcSentTitle.Size =
                new System.Drawing.Size(100, 28);

            this.lblPdcSentTitle.Padding =
                new System.Windows.Forms.Padding(6, 0, 0, 0);

            this.lblPdcSentTitle.TextAlign =
                System.Drawing.ContentAlignment.MiddleLeft;

            this.lblPdcSentTitle.Name = "lblPdcSentTitle";
            this.lblPdcSentTitle.Text = "Sent Request";

            // txtPdcSent
            this.txtPdcSent.Anchor =
                System.Windows.Forms.AnchorStyles.Top |
                System.Windows.Forms.AnchorStyles.Bottom |
                System.Windows.Forms.AnchorStyles.Left |
                System.Windows.Forms.AnchorStyles.Right;

            this.txtPdcSent.BackColor =
                System.Drawing.SystemColors.Window;

            this.txtPdcSent.BorderStyle =
                System.Windows.Forms.BorderStyle.Fixed3D;

            this.txtPdcSent.Font =
                new System.Drawing.Font(
                    "Consolas",
                    9F);

            this.txtPdcSent.Location =
                new System.Drawing.Point(10, 38);

            this.txtPdcSent.Multiline = true;
            this.txtPdcSent.Name = "txtPdcSent";
            this.txtPdcSent.ReadOnly = true;
            this.txtPdcSent.ScrollBars =
                System.Windows.Forms.ScrollBars.Vertical;

            this.txtPdcSent.Size =
                new System.Drawing.Size(340, 100);

            // 
            // pnlPdcReceived
            // 
            this.pnlPdcReceived.BorderStyle =
                System.Windows.Forms.BorderStyle.Fixed3D;

            this.pnlPdcReceived.Controls.Add(this.txtPdcReceived);
            this.pnlPdcReceived.Controls.Add(this.lblPdcReceivedTitle);

            this.pnlPdcReceived.Dock =
                System.Windows.Forms.DockStyle.Fill;

            this.pnlPdcReceived.Margin =
                new System.Windows.Forms.Padding(4, 0, 0, 0);

            this.pnlPdcReceived.Name = "pnlPdcReceived";

            // lblPdcReceivedTitle
            this.lblPdcReceivedTitle.Dock =
                System.Windows.Forms.DockStyle.Top;

            this.lblPdcReceivedTitle.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10.8F,
                    System.Drawing.FontStyle.Bold);

            this.lblPdcReceivedTitle.BackColor =
                System.Drawing.Color.Beige;

            this.lblPdcReceivedTitle.ForeColor =
                System.Drawing.Color.Maroon;

            this.lblPdcReceivedTitle.Size =
                new System.Drawing.Size(100, 28);

            this.lblPdcReceivedTitle.Padding =
                new System.Windows.Forms.Padding(6, 0, 0, 0);

            this.lblPdcReceivedTitle.TextAlign =
                System.Drawing.ContentAlignment.MiddleLeft;

            this.lblPdcReceivedTitle.Name = "lblPdcReceivedTitle";
            this.lblPdcReceivedTitle.Text = "Received Data";

            // txtPdcReceived
            this.txtPdcReceived.Anchor =
                System.Windows.Forms.AnchorStyles.Top |
                System.Windows.Forms.AnchorStyles.Bottom |
                System.Windows.Forms.AnchorStyles.Left |
                System.Windows.Forms.AnchorStyles.Right;

            this.txtPdcReceived.BackColor =
                System.Drawing.SystemColors.Window;

            this.txtPdcReceived.BorderStyle =
                System.Windows.Forms.BorderStyle.Fixed3D;

            this.txtPdcReceived.Font =
                new System.Drawing.Font(
                    "Consolas",
                    9F);

            this.txtPdcReceived.Location =
                new System.Drawing.Point(10, 38);

            this.txtPdcReceived.Multiline = true;
            this.txtPdcReceived.Name = "txtPdcReceived";
            this.txtPdcReceived.ReadOnly = true;

            this.txtPdcReceived.ScrollBars =
                System.Windows.Forms.ScrollBars.Vertical;

            this.txtPdcReceived.Size =
                new System.Drawing.Size(340, 100);

            // 
            // PDCAndCGDBPortTest
            // 
            this.AutoScaleMode =
                System.Windows.Forms.AutoScaleMode.Font;

            this.AutoScroll = true;
            this.AutoScrollMinSize =
                new System.Drawing.Size(740, 560);

            this.MinimumSize =
                new System.Drawing.Size(740, 560);

            this.Controls.Add(this.tlpPdc);

            this.Name = "PDCAndCGDBPortTest";

            this.Size =
                new System.Drawing.Size(772, 720);

            this.pnlPdcReceived.ResumeLayout(false);
            this.pnlPdcReceived.PerformLayout();

            this.pnlPdcSent.ResumeLayout(false);
            this.pnlPdcSent.PerformLayout();

            this.tlpPdcData.ResumeLayout(false);

            this.pnlPdcCommand.ResumeLayout(false);
            this.pnlPdcCommand.PerformLayout();

            this.pnlPdcDevice.ResumeLayout(false);
            this.pnlPdcDevice.PerformLayout();

            this.tlpPdc.ResumeLayout(false);

            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpPdc;

        private System.Windows.Forms.Panel pnlPdcDevice;
        private System.Windows.Forms.Label lblPdcDeviceTitle;
        private System.Windows.Forms.Label lblPdcSelect;
        private System.Windows.Forms.ComboBox cmbPdcList;
        private System.Windows.Forms.Label lblPdcIp;
        private System.Windows.Forms.TextBox txtPdcIp;
        private System.Windows.Forms.Label lblPdcStatusCaption;
        private System.Windows.Forms.Label lblPdcStatusBadge;

        private System.Windows.Forms.Panel pnlPdcCommand;
        private System.Windows.Forms.Label lblPdcCommandTitle;
        private System.Windows.Forms.Label lblPdcCommand;
        private System.Windows.Forms.ComboBox cmbPdcCommand;

        // CGDB board selection controls
        private System.Windows.Forms.Label lblCgdbBoardIp;
        private System.Windows.Forms.ComboBox cmbCgdbBoardIp;

        private System.Windows.Forms.Label lblPdcInt;
        private System.Windows.Forms.ComboBox cmbPdcInt;
        private System.Windows.Forms.Label lblPdcDto;
        private System.Windows.Forms.ComboBox cmbPdcDto;

        // Port and Asset ID textboxes
        private System.Windows.Forms.Label lblPdcPort;
        private System.Windows.Forms.TextBox txtPdcPort;
        private System.Windows.Forms.Label lblCgdbAssetId;
        private System.Windows.Forms.TextBox txtCgdbAssetId;

        private System.Windows.Forms.Button btnPdcExecute;
        private System.Windows.Forms.Button btnPdcClear;
        private System.Windows.Forms.Label lblPdcNote;

        private System.Windows.Forms.TableLayoutPanel tlpPdcData;

        private System.Windows.Forms.Panel pnlPdcSent;
        private System.Windows.Forms.Label lblPdcSentTitle;
        private System.Windows.Forms.TextBox txtPdcSent;

        private System.Windows.Forms.Panel pnlPdcReceived;
        private System.Windows.Forms.Label lblPdcReceivedTitle;
        private System.Windows.Forms.TextBox txtPdcReceived;
    }
}