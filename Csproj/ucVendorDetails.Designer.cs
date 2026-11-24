namespace IPISv2
{
    partial class ucVendorDetails
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
            this.tableMain = new System.Windows.Forms.TableLayoutPanel();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelInput = new System.Windows.Forms.Panel();
            this.lblVendorName = new System.Windows.Forms.Label();
            this.txtVendorName = new System.Windows.Forms.TextBox();
            this.lblVendorCode = new System.Windows.Forms.Label();
            this.txtVendorCode = new System.Windows.Forms.TextBox();
            this.chkIsActive = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblMode = new System.Windows.Forms.Label();
            this.dgvVendors = new System.Windows.Forms.DataGridView();
            this.tableMain.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.panelInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVendors)).BeginInit();
            this.SuspendLayout();
            // 
            // tableMain
            // 
            this.tableMain.ColumnCount = 1;
            this.tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableMain.Controls.Add(this.panelHeader, 0, 0);
            this.tableMain.Controls.Add(this.panelInput, 0, 1);
            this.tableMain.Controls.Add(this.dgvVendors, 0, 2);
            this.tableMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableMain.Location = new System.Drawing.Point(0, 0);
            this.tableMain.Name = "tableMain";
            this.tableMain.RowCount = 3;
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableMain.Size = new System.Drawing.Size(900, 600);
            this.tableMain.TabIndex = 0;
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHeader.Location = new System.Drawing.Point(3, 3);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(894, 44);
            this.panelHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(15, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(220, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Vendor Integration";
            // 
            // panelInput
            // 
            this.panelInput.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelInput.Controls.Add(this.lblVendorName);
            this.panelInput.Controls.Add(this.txtVendorName);
            this.panelInput.Controls.Add(this.lblVendorCode);
            this.panelInput.Controls.Add(this.txtVendorCode);
            this.panelInput.Controls.Add(this.chkIsActive);
            this.panelInput.Controls.Add(this.btnSave);
            this.panelInput.Controls.Add(this.lblMode);
            this.panelInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelInput.Location = new System.Drawing.Point(3, 53);
            this.panelInput.Name = "panelInput";
            this.panelInput.Size = new System.Drawing.Size(894, 74);
            this.panelInput.TabIndex = 1;
            // 
            // lblVendorName
            // 
            this.lblVendorName.AutoSize = true;
            this.lblVendorName.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblVendorName.Location = new System.Drawing.Point(15, 12);
            this.lblVendorName.Name = "lblVendorName";
            this.lblVendorName.Size = new System.Drawing.Size(108, 21);
            this.lblVendorName.TabIndex = 0;
            this.lblVendorName.Text = "Vendor Name";
            // 
            // txtVendorName
            // 
            this.txtVendorName.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtVendorName.Location = new System.Drawing.Point(130, 9);
            this.txtVendorName.Name = "txtVendorName";
            this.txtVendorName.Size = new System.Drawing.Size(220, 29);
            this.txtVendorName.TabIndex = 1;
            // 
            // lblVendorCode
            // 
            this.lblVendorCode.AutoSize = true;
            this.lblVendorCode.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblVendorCode.Location = new System.Drawing.Point(370, 12);
            this.lblVendorCode.Name = "lblVendorCode";
            this.lblVendorCode.Size = new System.Drawing.Size(104, 21);
            this.lblVendorCode.TabIndex = 2;
            this.lblVendorCode.Text = "Vendor Code";
            // 
            // txtVendorCode
            // 
            this.txtVendorCode.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtVendorCode.Location = new System.Drawing.Point(480, 9);
            this.txtVendorCode.Name = "txtVendorCode";
            this.txtVendorCode.Size = new System.Drawing.Size(160, 29);
            this.txtVendorCode.TabIndex = 3;
            // 
            // chkIsActive
            // 
            this.chkIsActive.AutoSize = true;
            this.chkIsActive.Checked = true;
            this.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsActive.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.chkIsActive.Location = new System.Drawing.Point(660, 11);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Size = new System.Drawing.Size(94, 25);
            this.chkIsActive.TabIndex = 4;
            this.chkIsActive.Text = "Is Active";
            this.chkIsActive.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.SeaGreen;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(780, 8);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 32);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // lblMode
            // 
            this.lblMode.AutoSize = true;
            this.lblMode.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic);
            this.lblMode.ForeColor = System.Drawing.Color.DimGray;
            this.lblMode.Location = new System.Drawing.Point(15, 45);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(140, 19);
            this.lblMode.TabIndex = 6;
            this.lblMode.Text = "Adding new vendor";
            // 
            // dgvVendors
            // 
            this.dgvVendors.AllowUserToAddRows = false;
            this.dgvVendors.AllowUserToDeleteRows = false;
            this.dgvVendors.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvVendors.BackgroundColor = System.Drawing.Color.White;
            this.dgvVendors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVendors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvVendors.Location = new System.Drawing.Point(3, 133);
            this.dgvVendors.MultiSelect = false;
            this.dgvVendors.Name = "dgvVendors";
            this.dgvVendors.ReadOnly = true;
            this.dgvVendors.RowHeadersVisible = false;
            this.dgvVendors.RowHeadersWidth = 51;
            this.dgvVendors.RowTemplate.Height = 34;
            this.dgvVendors.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvVendors.Size = new System.Drawing.Size(894, 464);
            this.dgvVendors.TabIndex = 2;
            // 
            // ucVendorDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableMain);
            this.Name = "ucVendorDetails";
            this.Size = new System.Drawing.Size(900, 600);
            this.tableMain.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelInput.ResumeLayout(false);
            this.panelInput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVendors)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableMain;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelInput;
        private System.Windows.Forms.Label lblVendorName;
        private System.Windows.Forms.TextBox txtVendorName;
        private System.Windows.Forms.Label lblVendorCode;
        private System.Windows.Forms.TextBox txtVendorCode;
        private System.Windows.Forms.CheckBox chkIsActive;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.DataGridView dgvVendors;
    }
}