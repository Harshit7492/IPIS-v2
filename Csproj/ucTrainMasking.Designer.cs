namespace IPISv2
{
    partial class ucTrainMasking
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
            this.tableLayoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.grpAddTrain = new System.Windows.Forms.GroupBox();
            this.lblTrainNo = new System.Windows.Forms.Label();
            this.txtTrainNo = new System.Windows.Forms.TextBox();
            this.lblTrainName = new System.Windows.Forms.Label();
            this.txtTrainName = new System.Windows.Forms.TextBox();
            this.lblRemarks = new System.Windows.Forms.Label();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.btnAddTrain = new System.Windows.Forms.Button();
            this.dgvTrains = new System.Windows.Forms.DataGridView();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();

            this.tableLayoutMain.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.grpAddTrain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrains)).BeginInit();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutMain
            // 
            this.tableLayoutMain.ColumnCount = 1;
            this.tableLayoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutMain.Controls.Add(this.panelHeader, 0, 0);
            this.tableLayoutMain.Controls.Add(this.dgvTrains, 0, 1);
            this.tableLayoutMain.Controls.Add(this.panelButtons, 0, 2);
            this.tableLayoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutMain.Name = "tableLayoutMain";
            this.tableLayoutMain.RowCount = 3;
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.tableLayoutMain.Size = new System.Drawing.Size(983, 601);
            this.tableLayoutMain.TabIndex = 0;
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.White;
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Controls.Add(this.grpAddTrain);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHeader.Location = new System.Drawing.Point(3, 3);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(977, 114);
            this.panelHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblTitle.Location = new System.Drawing.Point(12, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(364, 35);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "TRAIN MASKING MANAGEMENT";
            // 
            // grpAddTrain
            // 
            this.grpAddTrain.Controls.Add(this.lblTrainNo);
            this.grpAddTrain.Controls.Add(this.txtTrainNo);
            this.grpAddTrain.Controls.Add(this.lblTrainName);
            this.grpAddTrain.Controls.Add(this.txtTrainName);
            this.grpAddTrain.Controls.Add(this.lblRemarks);
            this.grpAddTrain.Controls.Add(this.txtRemarks);
            this.grpAddTrain.Controls.Add(this.btnAddTrain);
            this.grpAddTrain.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpAddTrain.ForeColor = System.Drawing.Color.DarkBlue;
            this.grpAddTrain.Location = new System.Drawing.Point(12, 44);
            this.grpAddTrain.Name = "grpAddTrain";
            this.grpAddTrain.Size = new System.Drawing.Size(860, 65);
            this.grpAddTrain.TabIndex = 1;
            this.grpAddTrain.TabStop = false;
            this.grpAddTrain.Text = "Add Train to Masking List";
            // 
            // lblTrainNo
            // 
            this.lblTrainNo.AutoSize = true;
            this.lblTrainNo.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblTrainNo.ForeColor = System.Drawing.Color.Black;
            this.lblTrainNo.Location = new System.Drawing.Point(10, 30);
            this.lblTrainNo.Name = "lblTrainNo";
            this.lblTrainNo.Size = new System.Drawing.Size(73, 21);
            this.lblTrainNo.TabIndex = 0;
            this.lblTrainNo.Text = "Train No:";
            // 
            // txtTrainNo
            // 
            this.txtTrainNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTrainNo.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtTrainNo.Location = new System.Drawing.Point(85, 26);
            this.txtTrainNo.Name = "txtTrainNo";
            this.txtTrainNo.Size = new System.Drawing.Size(110, 29);
            this.txtTrainNo.TabIndex = 1;
            // 
            // lblTrainName
            // 
            this.lblTrainName.AutoSize = true;
            this.lblTrainName.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblTrainName.ForeColor = System.Drawing.Color.Black;
            this.lblTrainName.Location = new System.Drawing.Point(205, 30);
            this.lblTrainName.Name = "lblTrainName";
            this.lblTrainName.Size = new System.Drawing.Size(92, 21);
            this.lblTrainName.TabIndex = 2;
            this.lblTrainName.Text = "Train Name:";
            // 
            // txtTrainName
            // 
            this.txtTrainName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTrainName.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtTrainName.Location = new System.Drawing.Point(300, 26);
            this.txtTrainName.Name = "txtTrainName";
            this.txtTrainName.Size = new System.Drawing.Size(200, 29);
            this.txtTrainName.TabIndex = 3;
            // 
            // lblRemarks
            // 
            this.lblRemarks.AutoSize = true;
            this.lblRemarks.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblRemarks.ForeColor = System.Drawing.Color.Black;
            this.lblRemarks.Location = new System.Drawing.Point(510, 30);
            this.lblRemarks.Name = "lblRemarks";
            this.lblRemarks.Size = new System.Drawing.Size(74, 21);
            this.lblRemarks.TabIndex = 4;
            this.lblRemarks.Text = "Remarks:";
            // 
            // txtRemarks
            // 
            this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRemarks.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtRemarks.Location = new System.Drawing.Point(585, 26);
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(160, 29);
            this.txtRemarks.TabIndex = 5;
            // 
            // btnAddTrain
            // 
            this.btnAddTrain.BackColor = System.Drawing.Color.White;
            this.btnAddTrain.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddTrain.FlatAppearance.BorderSize = 1;
            this.btnAddTrain.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnAddTrain.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(235, 238, 250);
            this.btnAddTrain.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(215, 222, 245);
            this.btnAddTrain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddTrain.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAddTrain.ForeColor = System.Drawing.Color.Black;
            this.btnAddTrain.Location = new System.Drawing.Point(760, 23);
            this.btnAddTrain.Name = "btnAddTrain";
            this.btnAddTrain.Padding = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnAddTrain.Size = new System.Drawing.Size(80, 32);
            this.btnAddTrain.TabIndex = 6;
            this.btnAddTrain.Text = "+ ADD";
            this.btnAddTrain.UseVisualStyleBackColor = false;
            this.btnAddTrain.Click += new System.EventHandler(this.BtnAddTrain_Click);
            // 
            // dgvTrains
            // 
            this.dgvTrains.AllowUserToAddRows = false;
            this.dgvTrains.AllowUserToDeleteRows = false;
            this.dgvTrains.BackgroundColor = System.Drawing.Color.White;
            this.dgvTrains.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvTrains.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTrains.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTrains.Location = new System.Drawing.Point(3, 123);
            this.dgvTrains.Name = "dgvTrains";
            this.dgvTrains.ReadOnly = true;
            this.dgvTrains.RowHeadersVisible = false;
            this.dgvTrains.RowTemplate.Height = 28;
            this.dgvTrains.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTrains.Size = new System.Drawing.Size(977, 410);
            this.dgvTrains.TabIndex = 1;
            this.dgvTrains.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvTrains_CellClick);
            this.dgvTrains.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.DgvTrains_RowPrePaint);
            // 
            // panelButtons
            // 
            this.panelButtons.BackColor = System.Drawing.Color.White;
            this.panelButtons.Controls.Add(this.btnDelete);
            this.panelButtons.Controls.Add(this.btnRefresh);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelButtons.Location = new System.Drawing.Point(3, 539);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(977, 59);
            this.panelButtons.TabIndex = 2;
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.White;
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelete.Enabled = false;
            this.btnDelete.FlatAppearance.BorderSize = 1;
            this.btnDelete.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDelete.ForeColor = System.Drawing.Color.Black;
            this.btnDelete.Location = new System.Drawing.Point(12, 10);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(220, 40);
            this.btnDelete.TabIndex = 0;
            this.btnDelete.Text = "DELETE SELECTED";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.White;
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.FlatAppearance.BorderSize = 1;
            this.btnRefresh.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.btnRefresh.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(235, 245, 252);
            this.btnRefresh.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(215, 235, 250);
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.Black;
            this.btnRefresh.Location = new System.Drawing.Point(242, 10);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(140, 40);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "REFRESH";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // ucTrainMasking
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.tableLayoutMain);
            this.Name = "ucTrainMasking";
            this.Size = new System.Drawing.Size(983, 601);
            this.tableLayoutMain.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.grpAddTrain.ResumeLayout(false);
            this.grpAddTrain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrains)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutMain;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox grpAddTrain;
        private System.Windows.Forms.Label lblTrainNo;
        private System.Windows.Forms.TextBox txtTrainNo;
        private System.Windows.Forms.Label lblTrainName;
        private System.Windows.Forms.TextBox txtTrainName;
        private System.Windows.Forms.Label lblRemarks;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Button btnAddTrain;
        private System.Windows.Forms.DataGridView dgvTrains;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnRefresh;
    }
}