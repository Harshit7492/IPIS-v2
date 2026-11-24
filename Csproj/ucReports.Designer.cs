namespace IPIS
{
    partial class ucReports
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
            components = new System.ComponentModel.Container();
            mainTableLayoutPanel = new TableLayoutPanel();
            headerTableLayoutPanel = new TableLayoutPanel();
            label1 = new Label();
            comboBox1 = new ComboBox();
            panel1 = new Panel();
            dateTimePicker1 = new DateTimePicker();
            label2 = new Label();
            panel2 = new Panel();
            dateTimePicker2 = new DateTimePicker();
            label3 = new Label();
            btnFetchReport = new Button();
            btnRefresh = new Button();
            reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            lblPlaceholder = new Label();
            toolTip1 = new ToolTip(components);
            mainTableLayoutPanel.SuspendLayout();
            headerTableLayoutPanel.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // mainTableLayoutPanel
            // 
            mainTableLayoutPanel.BackColor = Color.FromArgb(248, 249, 251);
            mainTableLayoutPanel.ColumnCount = 1;
            mainTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainTableLayoutPanel.Controls.Add(headerTableLayoutPanel, 0, 0);
            mainTableLayoutPanel.Controls.Add(reportViewer1, 0, 1);
            mainTableLayoutPanel.Controls.Add(lblPlaceholder, 0, 1);
            mainTableLayoutPanel.Dock = DockStyle.Fill;
            mainTableLayoutPanel.Location = new Point(0, 0);
            mainTableLayoutPanel.Margin = new Padding(0);
            mainTableLayoutPanel.Name = "mainTableLayoutPanel";
            mainTableLayoutPanel.RowCount = 2;
            mainTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 120F));
            mainTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainTableLayoutPanel.Size = new Size(1059, 907);
            mainTableLayoutPanel.TabIndex = 0;
            // 
            // headerTableLayoutPanel
            // 
            headerTableLayoutPanel.BackColor = Color.White;
            headerTableLayoutPanel.ColumnCount = 7;
            headerTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            headerTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            headerTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F));
            headerTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F));
            headerTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            headerTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            headerTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            headerTableLayoutPanel.Controls.Add(label1, 0, 0);
            headerTableLayoutPanel.Controls.Add(comboBox1, 1, 0);
            headerTableLayoutPanel.Controls.Add(panel1, 2, 0);
            headerTableLayoutPanel.Controls.Add(panel2, 3, 0);
            headerTableLayoutPanel.Controls.Add(btnFetchReport, 4, 0);
            headerTableLayoutPanel.Controls.Add(btnRefresh, 5, 0);
            headerTableLayoutPanel.Dock = DockStyle.Fill;
            headerTableLayoutPanel.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            headerTableLayoutPanel.Location = new Point(0, 0);
            headerTableLayoutPanel.Margin = new Padding(0);
            headerTableLayoutPanel.Name = "headerTableLayoutPanel";
            headerTableLayoutPanel.Padding = new Padding(10);
            headerTableLayoutPanel.RowCount = 1;
            headerTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            headerTableLayoutPanel.Size = new Size(1059, 120);
            headerTableLayoutPanel.TabIndex = 0;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.None;
            label1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(44, 62, 80);
            label1.Location = new Point(10, 10);
            label1.Margin = new Padding(0);
            label1.Name = "label1";
            label1.Size = new Size(150, 100);
            label1.TabIndex = 0;
            label1.Text = "View Report :";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // comboBox1
            // 
            comboBox1.Anchor = AnchorStyles.None;
            comboBox1.BackColor = Color.Silver;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.FlatStyle = FlatStyle.Flat;
            comboBox1.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            comboBox1.ForeColor = Color.Black;
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] {"CGDB Send Data Report", "TADB Send Data Report", "Announce Data Report", "User Login Report","Train master Report", "LED Open/Short Report" });
            comboBox1.Location = new Point(163, 41);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(800, 50);
            comboBox1.TabIndex = 0;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // panel1
            // 
            panel1.BackColor = Color.Transparent;
            panel1.Controls.Add(dateTimePicker1);
            panel1.Controls.Add(label2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(219, 10);
            panel1.Margin = new Padding(5, 0, 5, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(210, 100);
            panel1.TabIndex = 2;
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Anchor = AnchorStyles.None;
            dateTimePicker1.BackColor = Color.White;
            dateTimePicker1.CalendarFont = new Font("Segoe UI", 9F);
            dateTimePicker1.CalendarMonthBackground = Color.White;
            dateTimePicker1.CalendarTitleBackColor = Color.FromArgb(52, 73, 94);
            dateTimePicker1.CalendarTitleForeColor = Color.White;
            dateTimePicker1.CalendarTrailingForeColor = Color.Gray;
            dateTimePicker1.Font = new Font("Segoe UI", 10F);
            dateTimePicker1.Format = DateTimePickerFormat.Short;
            dateTimePicker1.Location = new Point(83, 33);
            dateTimePicker1.Margin = new Padding(5);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(120, 34);
            dateTimePicker1.TabIndex = 1;
            dateTimePicker1.Value = new DateTime(2026, 7, 2, 0, 0, 0, 0);
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.None;
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            label2.ForeColor = Color.FromArgb(44, 62, 80);
            label2.Location = new Point(10, 33);
            label2.Margin = new Padding(0);
            label2.Name = "label2";
            label2.Size = new Size(76, 30);
            label2.TabIndex = 0;
            label2.Text = "From :";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel2
            // 
            panel2.BackColor = Color.Transparent;
            panel2.Controls.Add(dateTimePicker2);
            panel2.Controls.Add(label3);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(439, 10);
            panel2.Margin = new Padding(5, 0, 5, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(210, 100);
            panel2.TabIndex = 3;
            // 
            // dateTimePicker2
            // 
            dateTimePicker2.Anchor = AnchorStyles.None;
            dateTimePicker2.BackColor = Color.White;
            dateTimePicker2.CalendarFont = new Font("Segoe UI", 9F);
            dateTimePicker2.CalendarMonthBackground = Color.White;
            dateTimePicker2.CalendarTitleBackColor = Color.FromArgb(52, 73, 94);
            dateTimePicker2.CalendarTitleForeColor = Color.White;
            dateTimePicker2.CalendarTrailingForeColor = Color.Gray;
            dateTimePicker2.Font = new Font("Segoe UI", 10F);
            dateTimePicker2.Format = DateTimePickerFormat.Short;
            dateTimePicker2.Location = new Point(75, 33);
            dateTimePicker2.Margin = new Padding(5);
            dateTimePicker2.Name = "dateTimePicker2";
            dateTimePicker2.Size = new Size(120, 34);
            dateTimePicker2.TabIndex = 1;
            dateTimePicker2.Value = new DateTime(2026, 7, 2, 0, 0, 0, 0);
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.None;
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            label3.ForeColor = Color.FromArgb(44, 62, 80);
            label3.Location = new Point(10, 33);
            label3.Margin = new Padding(0);
            label3.Name = "label3";
            label3.Size = new Size(48, 30);
            label3.TabIndex = 0;
            label3.Text = "To :";       
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // btnFetchReport
            // 
            btnFetchReport.Anchor = AnchorStyles.None;
            btnFetchReport.BackColor = Color.FromArgb(21, 101, 192);       // Deep, professional blue            btnFetchReport.FlatAppearance.BorderSize = 0;
            btnFetchReport.FlatStyle = FlatStyle.Flat;
            btnFetchReport.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnFetchReport.ForeColor = Color.White; 
            btnFetchReport.Location = new Point(664, 41);
            btnFetchReport.Margin = new Padding(2);
            btnFetchReport.Name = "btnFetchReport";
            btnFetchReport.Size = new Size(140, 42);
            btnFetchReport.TabIndex = 6;
            btnFetchReport.Text = "Fetch Report";
            btnFetchReport.TextAlign = ContentAlignment.MiddleCenter;
            btnFetchReport.UseVisualStyleBackColor = false;
            btnFetchReport.Click += btnFetchReport_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.Anchor = AnchorStyles.None;
            btnRefresh.BackColor = Color.FromArgb(46, 204, 113);
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(784, 41);
            btnRefresh.Margin = new Padding(5);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(100, 38);
            btnRefresh.TabIndex = 7;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // reportViewer1
            // 
            reportViewer1.BackColor = Color.FromArgb(248, 249, 251);
            reportViewer1.Dock = DockStyle.Fill;
            reportViewer1.Location = new Point(4, 124);
            reportViewer1.Margin = new Padding(4);
            reportViewer1.Name = "reportViewer1";
            reportViewer1.ServerReport.BearerToken = null;
            reportViewer1.Size = new Size(1051, 779);
            reportViewer1.TabIndex = 4;
            reportViewer1.Visible = false;
            reportViewer1.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.PageWidth;
            // 
            // lblPlaceholder
            // 
            lblPlaceholder.BackColor = Color.FromArgb(248, 249, 251);
            lblPlaceholder.Dock = DockStyle.Fill;
            lblPlaceholder.Font = new Font("Segoe UI", 12F, FontStyle.Italic);
            lblPlaceholder.ForeColor = Color.FromArgb(120, 130, 140);
            lblPlaceholder.Location = new Point(4, 124);
            lblPlaceholder.Margin = new Padding(4);
            lblPlaceholder.Name = "lblPlaceholder";
            lblPlaceholder.Size = new Size(1051, 779);
            lblPlaceholder.TabIndex = 5;
            lblPlaceholder.Text = "Select a report type above to get started.";
            lblPlaceholder.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // toolTip1
            // 
            toolTip1.AutomaticDelay = 200;
            toolTip1.AutoPopDelay = 8000;
            toolTip1.InitialDelay = 200;
            toolTip1.ReshowDelay = 100;
            // 
            // ucReports
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(248, 249, 251);
            Controls.Add(mainTableLayoutPanel);
            Margin = new Padding(4, 5, 4, 5);
            Name = "ucReports";
            Size = new Size(1059, 907);
            mainTableLayoutPanel.ResumeLayout(false);
            headerTableLayoutPanel.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }



        // Main TableLayoutPanel
        private System.Windows.Forms.TableLayoutPanel mainTableLayoutPanel;
        // Header TableLayoutPanel
        private System.Windows.Forms.TableLayoutPanel headerTableLayoutPanel;
        // Controls
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnFetchReport;
        private System.Windows.Forms.Button btnRefresh; // NEW
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.Label lblPlaceholder;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}