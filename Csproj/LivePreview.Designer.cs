namespace IPISv2
{
    partial class LivePreview
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel panelBoardContainer;
        private System.Windows.Forms.Panel panelScrollContainer;
        private System.Windows.Forms.Panel panelCGDBHeaderControls;
        private System.Windows.Forms.Label lblBoardCount;
        private System.Windows.Forms.NumericUpDown numericBoardCount;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblSelectBoard;
        private System.Windows.Forms.ComboBox cmbBoardSelect;
        private System.Windows.Forms.Label lblBoardType;
        private System.Windows.Forms.ComboBox cmbBoardType;
        private IPISv2.HealthStatusReport.SingleLineDisplayBoard singleLineDisplayBoard;
        private IPISv2.HealthStatusReport.MultilineDisplayBoard multilineDisplayBoard;
        private IPISv2.HealthStatusReport.GenericDisplayBoard agdbDisplayBoard;
        private IPISv2.HealthStatusReport.CoachGuidenceDisplayBoard coachGuidenceDisplayBoard;

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
            this.panelBoardContainer = new System.Windows.Forms.Panel();
            this.panelScrollContainer = new System.Windows.Forms.Panel();
            this.panelCGDBHeaderControls = new System.Windows.Forms.Panel();
            this.lblBoardCount = new System.Windows.Forms.Label();
            this.numericBoardCount = new System.Windows.Forms.NumericUpDown();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblSelectBoard = new System.Windows.Forms.Label();
            this.cmbBoardSelect = new System.Windows.Forms.ComboBox();
            this.lblBoardType = new System.Windows.Forms.Label();
            this.cmbBoardType = new System.Windows.Forms.ComboBox();
            this.singleLineDisplayBoard = new IPISv2.HealthStatusReport.SingleLineDisplayBoard();
            this.multilineDisplayBoard = new IPISv2.HealthStatusReport.MultilineDisplayBoard();
            this.agdbDisplayBoard = new IPISv2.HealthStatusReport.GenericDisplayBoard();
            this.coachGuidenceDisplayBoard = new IPISv2.HealthStatusReport.CoachGuidenceDisplayBoard();
            this.panelScrollContainer.SuspendLayout();
            this.panelCGDBHeaderControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericBoardCount)).BeginInit();
            this.SuspendLayout();
            // 
            // panelScrollContainer
            // 
            this.panelScrollContainer.AutoScroll = true;
            this.panelScrollContainer.Controls.Add(this.panelBoardContainer);
            this.panelScrollContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelScrollContainer.Location = new System.Drawing.Point(0, 45);
            this.panelScrollContainer.Name = "panelScrollContainer";
            this.panelScrollContainer.Size = new System.Drawing.Size(100, 405);
            this.panelScrollContainer.TabIndex = 0;
            this.panelScrollContainer.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            // 
            // panelBoardContainer
            // 
            this.panelBoardContainer.AutoSize = true;
            this.panelBoardContainer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelBoardContainer.BackColor = System.Drawing.Color.FromArgb(30, 30, 40);
            this.panelBoardContainer.Location = new System.Drawing.Point(0, 0);
            this.panelBoardContainer.Name = "panelBoardContainer";
            this.panelBoardContainer.Size = new System.Drawing.Size(0, 0);
            this.panelBoardContainer.TabIndex = 0;
            // 
            // panelCGDBHeaderControls
            // 
            this.panelCGDBHeaderControls.BackColor = System.Drawing.Color.Transparent;
            this.panelCGDBHeaderControls.Controls.Add(this.lblBoardCount);
            this.panelCGDBHeaderControls.Controls.Add(this.numericBoardCount);
            this.panelCGDBHeaderControls.Controls.Add(this.btnRefresh);
            this.panelCGDBHeaderControls.Controls.Add(this.lblSelectBoard);
            this.panelCGDBHeaderControls.Controls.Add(this.cmbBoardSelect);
            this.panelCGDBHeaderControls.Location = new System.Drawing.Point(260, 0);
            this.panelCGDBHeaderControls.Name = "panelCGDBHeaderControls";
            this.panelCGDBHeaderControls.Size = new System.Drawing.Size(500, 45);
            this.panelCGDBHeaderControls.TabIndex = 8;
            // 
            // lblBoardCount
            // 
            this.lblBoardCount.AutoSize = true;
            this.lblBoardCount.Font = new System.Drawing.Font("Roboto", 9.5F);
            this.lblBoardCount.ForeColor = System.Drawing.Color.White;
            this.lblBoardCount.Location = new System.Drawing.Point(5, 12);
            this.lblBoardCount.Name = "lblBoardCount";
            this.lblBoardCount.Size = new System.Drawing.Size(118, 19);
            this.lblBoardCount.TabIndex = 0;
            this.lblBoardCount.Text = "Number of Boards:";
            // 
            // lblSelectBoard
            // 
            this.lblSelectBoard.AutoSize = true;
            this.lblSelectBoard.Font = new System.Drawing.Font("Roboto", 9.5F);
            this.lblSelectBoard.ForeColor = System.Drawing.Color.White;
            this.lblSelectBoard.Location = new System.Drawing.Point(275, 12);
            this.lblSelectBoard.Name = "lblSelectBoard";
            this.lblSelectBoard.Size = new System.Drawing.Size(89, 19);
            this.lblSelectBoard.TabIndex = 3;
            this.lblSelectBoard.Text = "Select Board:";
            // 
            // cmbBoardSelect
            // 
            this.cmbBoardSelect.BackColor = System.Drawing.Color.FromArgb(40, 40, 50);
            this.cmbBoardSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoardSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbBoardSelect.Font = new System.Drawing.Font("Roboto", 9.5F);
            this.cmbBoardSelect.ForeColor = System.Drawing.Color.White;
            this.cmbBoardSelect.FormattingEnabled = true;
            this.cmbBoardSelect.Location = new System.Drawing.Point(365, 8);
            this.cmbBoardSelect.Name = "cmbBoardSelect";
            this.cmbBoardSelect.Size = new System.Drawing.Size(120, 27);
            this.cmbBoardSelect.TabIndex = 4;
            this.cmbBoardSelect.SelectedIndexChanged += new System.EventHandler(this.cmbBoardSelect_SelectedIndexChanged);
            // 
            // lblBoardType
            // 
            this.lblBoardType.AutoSize = true;
            this.lblBoardType.Font = new System.Drawing.Font("Roboto", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblBoardType.ForeColor = System.Drawing.Color.White;
            this.lblBoardType.Location = new System.Drawing.Point(10, 12);
            this.lblBoardType.Name = "lblBoardType";
            this.lblBoardType.Size = new System.Drawing.Size(80, 19);
            this.lblBoardType.TabIndex = 6;
            this.lblBoardType.Text = "Board Type:";
            // 
            // cmbBoardType
            // 
            this.cmbBoardType.BackColor = System.Drawing.Color.FromArgb(40, 40, 50);
            this.cmbBoardType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoardType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbBoardType.Font = new System.Drawing.Font("Roboto", 9.5F, System.Drawing.FontStyle.Bold);
            this.cmbBoardType.ForeColor = System.Drawing.Color.White;
            this.cmbBoardType.FormattingEnabled = true;
            this.cmbBoardType.Location = new System.Drawing.Point(95, 8);
            this.cmbBoardType.Name = "cmbBoardType";
            this.cmbBoardType.Size = new System.Drawing.Size(160, 27);
            this.cmbBoardType.TabIndex = 7;
            // 
            // singleLineDisplayBoard
            // 
            this.singleLineDisplayBoard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.singleLineDisplayBoard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.singleLineDisplayBoard.Location = new System.Drawing.Point(0, 45);
            this.singleLineDisplayBoard.Name = "singleLineDisplayBoard";
            this.singleLineDisplayBoard.Size = new System.Drawing.Size(800, 240);
            this.singleLineDisplayBoard.TabIndex = 9;
            this.singleLineDisplayBoard.Visible = false;
            // 
            // agdbDisplayBoard
            // 
            this.agdbDisplayBoard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.agdbDisplayBoard.Location = new System.Drawing.Point(0, 45);
            this.agdbDisplayBoard.Name = "agdbDisplayBoard";
            this.agdbDisplayBoard.Size = new System.Drawing.Size(800, 285);
            this.agdbDisplayBoard.TabIndex = 11;
            this.agdbDisplayBoard.Visible = false;
            // 
            // coachGuidenceDisplayBoard
            // 
            this.coachGuidenceDisplayBoard.BackColor = System.Drawing.Color.White;
            this.coachGuidenceDisplayBoard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.coachGuidenceDisplayBoard.Location = new System.Drawing.Point(0, 45);
            this.coachGuidenceDisplayBoard.Name = "coachGuidenceDisplayBoard";
            this.coachGuidenceDisplayBoard.Size = new System.Drawing.Size(800, 240);
            this.coachGuidenceDisplayBoard.TabIndex = 12;
            this.coachGuidenceDisplayBoard.Visible = false;
            // 
            // LivePreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.Controls.Add(this.panelScrollContainer);
            this.Controls.Add(this.singleLineDisplayBoard);
            this.Controls.Add(this.multilineDisplayBoard);
            this.Controls.Add(this.agdbDisplayBoard);
            this.Controls.Add(this.coachGuidenceDisplayBoard);
            this.Controls.Add(this.panelCGDBHeaderControls);
            this.Controls.Add(this.cmbBoardType);
            this.Controls.Add(this.lblBoardType);
            this.Name = "LivePreview";
            this.Size = new System.Drawing.Size(950, 350);
            this.panelScrollContainer.ResumeLayout(false);
            this.panelScrollContainer.PerformLayout();
            this.panelCGDBHeaderControls.ResumeLayout(false);
            this.panelCGDBHeaderControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericBoardCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}