namespace IPIS
{
    partial class ucPDCBoardSetting
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            tableLayoutPanel1 = new TableLayoutPanel();
            label1 = new Label();
            groupBox1 = new GroupBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            label3 = new Label();
            txtIP = new TextBox();
            label5 = new Label();
            cmbPfNo = new ComboBox();
            dataGridPDC = new DataGridView();
            tableLayoutPanel3 = new TableLayoutPanel();
            btnRefresh = new Button();
            btnSavePDC = new Button();
            tableLayoutPanel4 = new TableLayoutPanel();
            tableLayoutPanel1.SuspendLayout();
            groupBox1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridPDC).BeginInit();
            tableLayoutPanel3.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.Beige;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(groupBox1, 0, 1);
            tableLayoutPanel1.Controls.Add(dataGridPDC, 0, 2);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 0, 3);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel4, 0, 4);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 5;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6.30137F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 19.31507F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 55.06849F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 9.292035F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 11.0619469F));
            tableLayoutPanel1.Size = new Size(830, 452);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.Maroon;
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(824, 28);
            label1.TabIndex = 0;
            label1.Text = "Platform Data Controller";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.WhiteSmoke;
            groupBox1.Controls.Add(tableLayoutPanel2);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(3, 31);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(824, 80);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "PLATEFORM DATA CONTROLLER";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 6;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20.7823963F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18.7041569F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10.6356964F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30.3178482F));
            tableLayoutPanel2.Controls.Add(label3, 0, 1);
            tableLayoutPanel2.Controls.Add(txtIP, 1, 1);
            tableLayoutPanel2.Controls.Add(label5, 0, 0);
            tableLayoutPanel2.Controls.Add(cmbPfNo, 1, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 22);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(818, 55);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Fill;
            label3.Location = new Point(3, 27);
            label3.Name = "label3";
            label3.Size = new Size(75, 28);
            label3.TabIndex = 1;
            label3.Text = "Hub IP :";
            // 
            // txtIP
            // 
            txtIP.Dock = DockStyle.Fill;
            txtIP.Location = new Point(84, 30);
            txtIP.Name = "txtIP";
            txtIP.Size = new Size(163, 26);
            txtIP.TabIndex = 3;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Dock = DockStyle.Fill;
            label5.Location = new Point(3, 0);
            label5.Name = "label5";
            label5.Size = new Size(75, 27);
            label5.TabIndex = 5;
            label5.Text = "PF No.";
            // 
            // cmbPfNo
            // 
            cmbPfNo.Dock = DockStyle.Fill;
            cmbPfNo.FormattingEnabled = true;
            cmbPfNo.Location = new Point(84, 2);
            cmbPfNo.Margin = new Padding(3, 2, 3, 2);
            cmbPfNo.Name = "cmbPfNo";
            cmbPfNo.Size = new Size(163, 27);
            cmbPfNo.TabIndex = 6;
            // 
            // dataGridPDC
            // 
            dataGridPDC.AllowUserToAddRows = false;
            dataGridPDC.AllowUserToDeleteRows = false;
            dataGridPDC.AllowUserToResizeColumns = false;
            dataGridPDC.AllowUserToResizeRows = false;
            dataGridPDC.BackgroundColor = Color.AliceBlue;
            dataGridPDC.BorderStyle = BorderStyle.Fixed3D;
            dataGridPDC.CellBorderStyle = DataGridViewCellBorderStyle.Raised;
            dataGridPDC.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridPDC.ColumnHeadersHeight = 29;
            dataGridPDC.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            dataGridPDC.DefaultCellStyle = dataGridViewCellStyle1;
            dataGridPDC.Dock = DockStyle.Fill;
            dataGridPDC.Location = new Point(3, 117);
            dataGridPDC.Name = "dataGridPDC";
            dataGridPDC.RowHeadersVisible = false;
            dataGridPDC.RowHeadersWidth = 51;
            dataGridPDC.RowTemplate.Height = 24;
            dataGridPDC.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridPDC.Size = new Size(824, 240);
            dataGridPDC.TabIndex = 2;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 4;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.Controls.Add(btnRefresh, 2, 0);
            tableLayoutPanel3.Controls.Add(btnSavePDC, 1, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 363);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new Size(824, 35);
            tableLayoutPanel3.TabIndex = 3;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.WhiteSmoke;
            btnRefresh.Dock = DockStyle.Fill;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Location = new Point(415, 3);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(200, 29);
            btnRefresh.TabIndex = 6;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnSavePDC
            // 
            btnSavePDC.BackColor = Color.WhiteSmoke;
            btnSavePDC.Dock = DockStyle.Fill;
            btnSavePDC.FlatStyle = FlatStyle.Flat;
            btnSavePDC.Location = new Point(209, 3);
            btnSavePDC.Name = "btnSavePDC";
            btnSavePDC.Size = new Size(200, 29);
            btnSavePDC.TabIndex = 2;
            btnSavePDC.Text = "Save";
            btnSavePDC.UseVisualStyleBackColor = false;
            btnSavePDC.Click += btnSavePDC_Click;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 7;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(3, 404);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 1;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Size = new Size(824, 45);
            tableLayoutPanel4.TabIndex = 4;
            // 
            // ucPDCBoardSetting
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel1);
            Name = "ucPDCBoardSetting";
            Size = new Size(830, 452);
            Load += ucBoardSettingPDC_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            groupBox1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridPDC).EndInit();
            tableLayoutPanel3.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridPDC;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button3;
        private ComboBox cmbPfNo;
        private Button btnSavePDC;
        private Button btnRefresh;
    }
}
