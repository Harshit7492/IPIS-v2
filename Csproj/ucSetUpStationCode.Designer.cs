namespace IPIS
{
    partial class ucSetUpStationCode
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
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel3 = new TableLayoutPanel();
            txtboxStationCodeName = new TextBox();
            panel2 = new Panel();
            dgvStationLegend = new DataGridView();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            panel1 = new Panel();
            label1 = new Label();
            tableLayoutPanel2 = new TableLayoutPanel();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            //label6 = new Label();
            txtboxCode = new TextBox();
            txtboxNameEnglish = new TextBox();
            txtboxNameHindi = new TextBox();
            txtboxNameRegional = new TextBox();
            //cmbSound = new ComboBox();
            //button1 = new Button();
            tableLayoutPanel4 = new TableLayoutPanel();
            btnClearDtationDetails = new Button();
            btnSaveStationDetails = new Button();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvStationLegend).BeginInit();
            panel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            SuspendLayout();
            txtboxNameHindi.Click += TxtboxNameHindi_Click;
            txtboxNameRegional.Click += TxtboxNameRegional_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 44.42916F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55.57084F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 1, 1);
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 5.6231F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 93.43066F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 0.973236F));
            tableLayoutPanel1.Size = new Size(727, 616);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Controls.Add(txtboxStationCodeName, 0, 0);
            tableLayoutPanel3.Controls.Add(panel2, 0, 1);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(326, 37);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 2;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 4.9209137F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 95.07909F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.Size = new Size(398, 569);
            tableLayoutPanel3.TabIndex = 3;
            // 
            // txtboxStationCodeName
            // 
            tableLayoutPanel3.SetColumnSpan(txtboxStationCodeName, 2);
            txtboxStationCodeName.Dock = DockStyle.Fill;
            txtboxStationCodeName.Location = new Point(3, 3);
            txtboxStationCodeName.Name = "txtboxStationCodeName";
            txtboxStationCodeName.PlaceholderText = "Please Enter Station Code";
            txtboxStationCodeName.Size = new Size(392, 23);
            txtboxStationCodeName.TabIndex = 2;
            txtboxStationCodeName.TextChanged += txtboxStationCodeName_TextChanged;
            // 
            // panel2
            // 
            tableLayoutPanel3.SetColumnSpan(panel2, 2);
            panel2.Controls.Add(dgvStationLegend);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 31);
            panel2.Name = "panel2";
            panel2.Size = new Size(392, 535);
            panel2.TabIndex = 3;
            // 
            // dgvStationLegend
            // 
            dataGridViewCellStyle1.BackColor = Color.Beige;
            dgvStationLegend.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvStationLegend.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvStationLegend.BorderStyle = BorderStyle.Fixed3D;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Control;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvStationLegend.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvStationLegend.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvStationLegend.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2 });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Window;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvStationLegend.DefaultCellStyle = dataGridViewCellStyle3;
            dgvStationLegend.Dock = DockStyle.Fill;
            dgvStationLegend.Location = new Point(0, 0);
            dgvStationLegend.Name = "dgvStationLegend";
            dgvStationLegend.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = SystemColors.Control;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle4.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            dgvStationLegend.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dgvStationLegend.RowHeadersVisible = false;
            dgvStationLegend.RowHeadersWidth = 51;
            dgvStationLegend.RowTemplate.Height = 24;
            dgvStationLegend.Size = new Size(392, 535);
            dgvStationLegend.TabIndex = 0;
            // 
            // Column1
            // 
            Column1.HeaderText = "Station Code";
            Column1.MinimumWidth = 6;
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            // 
            // Column2
            // 
            Column2.HeaderText = "Station Name";
            Column2.MinimumWidth = 6;
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            // 
            // panel1
            // 
            panel1.BackColor = Color.Beige;
            tableLayoutPanel1.SetColumnSpan(panel1, 2);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Fill;
            panel1.ForeColor = Color.Maroon;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(721, 28);
            panel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(149, 20);
            label1.TabIndex = 0;
            label1.Text = "View Station Details";
            // 
            // tableLayoutPanel2
            // 

            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34.8209877F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45.1201553F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20.0588512F));
            tableLayoutPanel2.Controls.Add(label2, 0, 0);
            tableLayoutPanel2.Controls.Add(label3, 0, 1);
            tableLayoutPanel2.Controls.Add(label4, 0, 2);
            tableLayoutPanel2.Controls.Add(label5, 0, 3);
            //tableLayoutPanel2.Controls.Add(label6, 0, 4);
            tableLayoutPanel2.Controls.Add(txtboxCode, 1, 0);
            tableLayoutPanel2.Controls.Add(txtboxNameEnglish, 1, 1);
            tableLayoutPanel2.Controls.Add(txtboxNameHindi, 1, 2);
            tableLayoutPanel2.Controls.Add(txtboxNameRegional, 1, 3);
            //tableLayoutPanel2.Controls.Add(cmbSound, 1, 4);
            //tableLayoutPanel2.Controls.Add(button1, 2, 4);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel4, 1, 5);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 37);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 7;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 6.86274529F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 6.86274529F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 6.86274529F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 6.86274529F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 6.86274529F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 6.86274529F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 58.82353F));
            tableLayoutPanel2.Size = new Size(317, 569);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Fill;
            label2.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(3, 0);
            label2.Name = "label2";
            label2.Size = new Size(104, 39);
            label2.TabIndex = 0;
            label2.Text = "Station Code *";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Fill;
            label3.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(3, 78);
            label3.Name = "label3";
            label3.Size = new Size(200, 39);
            label3.TabIndex = 1;
            label3.Text = "Station Name English*";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = DockStyle.Fill;
            label4.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(3, 78);
            label4.Name = "label4";
            label4.Size = new Size(104, 39);
            label4.TabIndex = 2;
            label4.Text = "Station Name Hindi";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Dock = DockStyle.Fill;
            label5.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(3, 117);
            label5.Name = "label5";
            label5.Size = new Size(104, 39);
            label5.TabIndex = 3;
            label5.Text = "Station Name Regional";
            // 
            // label6
            // 
            //label6.AutoSize = true;
            //label6.Dock = DockStyle.Fill;
            //label6.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            //label6.Location = new Point(3, 156);
            //label6.Name = "label6";
            //label6.Size = new Size(104, 39);
            //label6.TabIndex = 4;
            //label6.Text = "Add Sound File";
            // 
            // txtboxCode
            // 
            txtboxCode.Dock = DockStyle.Fill;
            txtboxCode.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtboxCode.Location = new Point(113, 3);
            txtboxCode.Name = "txtboxCode";
            txtboxCode.Size = new Size(137, 26);
            txtboxCode.TabIndex = 5;
            // 
            // txtboxNameEnglish
            // 
            txtboxNameEnglish.Dock = DockStyle.Fill;
            txtboxNameEnglish.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtboxNameEnglish.Location = new Point(113, 42);
            txtboxNameEnglish.Name = "txtboxNameEnglish";
            txtboxNameEnglish.Size = new Size(137, 26);
            txtboxNameEnglish.TabIndex = 6;
            // 
            // txtboxNameHindi
            // 
            txtboxNameHindi.Dock = DockStyle.Fill;
            txtboxNameHindi.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtboxNameHindi.Location = new Point(113, 81);
            txtboxNameHindi.Name = "txtboxNameHindi";
            txtboxNameHindi.Size = new Size(137, 26);
            txtboxNameHindi.TabIndex = 7;
            // 
            // txtboxNameRegional
            // 
            txtboxNameRegional.Dock = DockStyle.Fill;
            txtboxNameRegional.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            txtboxNameRegional.Location = new Point(113, 120);
            txtboxNameRegional.Name = "txtboxNameRegional";
            txtboxNameRegional.Size = new Size(137, 26);
            txtboxNameRegional.TabIndex = 8;
            // 
            // cmbSound
            // 
            //cmbSound.Dock = DockStyle.Fill;
            //cmbSound.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            //cmbSound.FormattingEnabled = true;
            //cmbSound.Location = new Point(113, 159);
            //cmbSound.Name = "cmbSound";
            //cmbSound.Size = new Size(137, 27);
            //cmbSound.TabIndex = 9;
            // 
            // button1
            // 
            //button1.Dock = DockStyle.Fill;
            //button1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            //button1.Location = new Point(256, 159);
            //button1.Name = "button1";
            //button1.Size = new Size(58, 33);
            //button1.TabIndex = 10;
            //button1.Text = "Browse";
            //button1.UseVisualStyleBackColor = true;

            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 2;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Controls.Add(btnClearDtationDetails, 1, 0);
            tableLayoutPanel4.Controls.Add(btnSaveStationDetails, 0, 0);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(113, 197);
            tableLayoutPanel4.Margin = new Padding(3, 2, 3, 2);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 1;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel4.Size = new Size(137, 35);
            tableLayoutPanel4.TabIndex = 16;
            // 
            // btnClearDtationDetails
            // 
            btnClearDtationDetails.Dock = DockStyle.Fill;
            btnClearDtationDetails.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClearDtationDetails.ImageAlign = ContentAlignment.MiddleLeft;
            btnClearDtationDetails.Location = new Point(71, 2);
            btnClearDtationDetails.Margin = new Padding(3, 2, 3, 2);
            btnClearDtationDetails.Name = "btnClearDtationDetails";
            btnClearDtationDetails.Size = new Size(63, 31);
            btnClearDtationDetails.TabIndex = 16;
            btnClearDtationDetails.Text = "Clear";
            btnClearDtationDetails.UseVisualStyleBackColor = true;
            btnClearDtationDetails.Click += btnClearDtationDetails_Click_1;
            // 
            // btnSaveStationDetails
            // 
            btnSaveStationDetails.Dock = DockStyle.Fill;
            btnSaveStationDetails.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSaveStationDetails.ImageAlign = ContentAlignment.MiddleLeft;
            btnSaveStationDetails.Location = new Point(3, 2);
            btnSaveStationDetails.Margin = new Padding(3, 2, 3, 2);
            btnSaveStationDetails.Name = "btnSaveStationDetails";
            btnSaveStationDetails.Size = new Size(62, 31);
            btnSaveStationDetails.TabIndex = 14;
            btnSaveStationDetails.Text = "Save";
            btnSaveStationDetails.UseVisualStyleBackColor = true;
            btnSaveStationDetails.Click += btnSaveStationDetails_Click_1;
            // 
            // ucSetUpStationCode
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BorderStyle = BorderStyle.Fixed3D;
            Controls.Add(tableLayoutPanel1);
            Name = "ucSetUpStationCode";
            Size = new Size(727, 616);
            Load += ucSetUpStationCode_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvStationLegend).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            tableLayoutPanel4.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        //private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtboxCode;
        private System.Windows.Forms.TextBox txtboxNameEnglish;
        private System.Windows.Forms.TextBox txtboxNameHindi;
        private System.Windows.Forms.TextBox txtboxNameRegional;
        //private System.Windows.Forms.ComboBox cmbSound;
        //private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TextBox txtboxStationCodeName;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dgvStationLegend;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private TableLayoutPanel tableLayoutPanel4;
        private Button btnClearDtationDetails;
        private Button btnSaveStationDetails;
    }
}
