//namespace IPIS
//{
//    partial class ucSetUpBoardSetting
//    {
//        /// <summary> 
//        /// Required designer variable.
//        /// </summary>
//        private System.ComponentModel.IContainer components = null;

//        /// <summary> 
//        /// Clean up any resources being used.
//        /// </summary>
//        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing && (components != null))
//            {
//                components.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        #region Component Designer generated code

//        /// <summary> 
//        /// Required method for Designer support - do not modify 
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucSetUpBoardSetting));
//            tableLayoutPanelBoardSetting = new TableLayoutPanel();
//            panelleftBoardSetting = new Panel();
//            treeViewPort = new TreeView();
//            panelContentBoardSetting = new Panel();
//            pBoxTrainLogo = new PictureBox();
//            tableLayoutPanel2 = new TableLayoutPanel();
//            cmbDisplayBoard = new ComboBox();
//            lblSelectDisplayBoard = new Label();
//            pictureBox_refresh = new PictureBox();
//            tableLayoutPanelBoardSetting.SuspendLayout();
//            panelleftBoardSetting.SuspendLayout();
//            panelContentBoardSetting.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)pBoxTrainLogo).BeginInit();
//            tableLayoutPanel2.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)pictureBox_refresh).BeginInit();
//            SuspendLayout();
//            // 
//            // tableLayoutPanelBoardSetting
//            // 
//            tableLayoutPanelBoardSetting.ColumnCount = 2;
//            tableLayoutPanelBoardSetting.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 29.90196F));
//            tableLayoutPanelBoardSetting.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70.09804F));
//            tableLayoutPanelBoardSetting.Controls.Add(panelleftBoardSetting, 0, 0);
//            tableLayoutPanelBoardSetting.Controls.Add(panelContentBoardSetting, 1, 1);
//            tableLayoutPanelBoardSetting.Controls.Add(tableLayoutPanel2, 1, 0);
//            tableLayoutPanelBoardSetting.Dock = DockStyle.Fill;
//            tableLayoutPanelBoardSetting.Location = new Point(0, 0);
//            tableLayoutPanelBoardSetting.Name = "tableLayoutPanelBoardSetting";
//            tableLayoutPanelBoardSetting.RowCount = 2;
//            tableLayoutPanelBoardSetting.RowStyles.Add(new RowStyle(SizeType.Percent, 6.656805F));
//            tableLayoutPanelBoardSetting.RowStyles.Add(new RowStyle(SizeType.Percent, 93.34319F));
//            tableLayoutPanelBoardSetting.Size = new Size(816, 634);
//            tableLayoutPanelBoardSetting.TabIndex = 0;
//            // 
//            // panelleftBoardSetting
//            // 
//            panelleftBoardSetting.Controls.Add(treeViewPort);
//            panelleftBoardSetting.Dock = DockStyle.Fill;
//            panelleftBoardSetting.Location = new Point(3, 3);
//            panelleftBoardSetting.Name = "panelleftBoardSetting";
//            tableLayoutPanelBoardSetting.SetRowSpan(panelleftBoardSetting, 2);
//            panelleftBoardSetting.Size = new Size(238, 628);
//            panelleftBoardSetting.TabIndex = 2;
//            // 
//            // treeViewPort
//            // 
//            treeViewPort.BackColor = Color.Beige;
//            treeViewPort.Dock = DockStyle.Fill;
//            treeViewPort.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
//            treeViewPort.Location = new Point(0, 0);
//            treeViewPort.Name = "treeViewPort";
//            treeViewPort.Size = new Size(238, 628);
//            treeViewPort.TabIndex = 0;
//            treeViewPort.NodeMouseClick += treeViewPort_NodeMouseClick;
//            // 
//            // panelContentBoardSetting
//            // 
//            panelContentBoardSetting.BackColor = Color.WhiteSmoke;
//            panelContentBoardSetting.BackgroundImageLayout = ImageLayout.Zoom;
//            panelContentBoardSetting.Controls.Add(pBoxTrainLogo);
//            panelContentBoardSetting.Dock = DockStyle.Fill;
//            panelContentBoardSetting.Location = new Point(247, 45);
//            panelContentBoardSetting.Name = "panelContentBoardSetting";
//            panelContentBoardSetting.Size = new Size(566, 586);
//            panelContentBoardSetting.TabIndex = 3;
//            // 
//            // pBoxTrainLogo
//            // 
//            pBoxTrainLogo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
//            pBoxTrainLogo.Image = (Image)resources.GetObject("pBoxTrainLogo.Image");
//            pBoxTrainLogo.Location = new Point(31, 108);
//            pBoxTrainLogo.Name = "pBoxTrainLogo";
//            pBoxTrainLogo.Size = new Size(500, 381);
//            pBoxTrainLogo.SizeMode = PictureBoxSizeMode.Zoom;
//            pBoxTrainLogo.TabIndex = 1;
//            pBoxTrainLogo.TabStop = false;
//            // 
//            // tableLayoutPanel2
//            // 
//            tableLayoutPanel2.BackColor = Color.Beige;
//            tableLayoutPanel2.ColumnCount = 3;
//            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 23.4982338F));
//            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28.26855F));
//            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48.0565376F));
//            tableLayoutPanel2.Controls.Add(cmbDisplayBoard, 2, 0);
//            tableLayoutPanel2.Controls.Add(lblSelectDisplayBoard, 1, 0);
//            tableLayoutPanel2.Controls.Add(pictureBox_refresh, 0, 0);
//            tableLayoutPanel2.Dock = DockStyle.Fill;
//            tableLayoutPanel2.Location = new Point(247, 3);
//            tableLayoutPanel2.Name = "tableLayoutPanel2";
//            tableLayoutPanel2.RowCount = 1;
//            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
//            tableLayoutPanel2.Size = new Size(566, 36);
//            tableLayoutPanel2.TabIndex = 4;
//            // 
//            // cmbDisplayBoard
//            // 
//            cmbDisplayBoard.Dock = DockStyle.Fill;
//            cmbDisplayBoard.DropDownStyle = ComboBoxStyle.DropDownList;
//            cmbDisplayBoard.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
//            cmbDisplayBoard.ForeColor = SystemColors.ControlText;
//            cmbDisplayBoard.FormattingEnabled = true;
//            cmbDisplayBoard.Location = new Point(296, 3);
//            cmbDisplayBoard.Name = "cmbDisplayBoard";
//            cmbDisplayBoard.Size = new Size(267, 27);
//            cmbDisplayBoard.TabIndex = 1;
//            cmbDisplayBoard.TabStop = false;
//            cmbDisplayBoard.SelectedIndexChanged += cmbDisplayBoard_SelectedIndexChanged;
//            // 
//            // lblSelectDisplayBoard
//            // 
//            lblSelectDisplayBoard.AutoSize = true;
//            lblSelectDisplayBoard.BackColor = Color.Beige;
//            lblSelectDisplayBoard.Dock = DockStyle.Fill;
//            lblSelectDisplayBoard.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
//            lblSelectDisplayBoard.ForeColor = Color.DarkRed;
//            lblSelectDisplayBoard.Location = new Point(136, 0);
//            lblSelectDisplayBoard.Name = "lblSelectDisplayBoard";
//            lblSelectDisplayBoard.Size = new Size(154, 36);
//            lblSelectDisplayBoard.TabIndex = 0;
//            lblSelectDisplayBoard.Text = "Display Boards/Hub :";
//            lblSelectDisplayBoard.TextAlign = ContentAlignment.MiddleRight;
//            // 
//            // pictureBox_refresh
//            // 
//            pictureBox_refresh.Dock = DockStyle.Fill;
//            pictureBox_refresh.Image = (Image)resources.GetObject("pictureBox_refresh.Image");
//            pictureBox_refresh.Location = new Point(3, 3);
//            pictureBox_refresh.Name = "pictureBox_refresh";
//            pictureBox_refresh.Size = new Size(127, 30);
//            pictureBox_refresh.SizeMode = PictureBoxSizeMode.Zoom;
//            pictureBox_refresh.TabIndex = 2;
//            pictureBox_refresh.TabStop = false;
//            pictureBox_refresh.Click += pictureBox_refresh_Click;
//            // 
//            // ucSetUpBoardSetting
//            // 
//            AutoScaleDimensions = new SizeF(7F, 15F);
//            AutoScaleMode = AutoScaleMode.Font;
//            Controls.Add(tableLayoutPanelBoardSetting);
//            Name = "ucSetUpBoardSetting";
//            Size = new Size(816, 634);
//            Load += ucSetUpBoardSetting_Load;
//            tableLayoutPanelBoardSetting.ResumeLayout(false);
//            panelleftBoardSetting.ResumeLayout(false);
//            panelContentBoardSetting.ResumeLayout(false);
//            ((System.ComponentModel.ISupportInitialize)pBoxTrainLogo).EndInit();
//            tableLayoutPanel2.ResumeLayout(false);
//            tableLayoutPanel2.PerformLayout();
//            ((System.ComponentModel.ISupportInitialize)pictureBox_refresh).EndInit();
//            ResumeLayout(false);

//        }

//        #endregion

//        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBoardSetting;
//        private System.Windows.Forms.Panel panelleftBoardSetting;
//        private System.Windows.Forms.TreeView treeViewPort;
//        private System.Windows.Forms.Panel panelContentBoardSetting;
//        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
//        private System.Windows.Forms.Label lblSelectDisplayBoard;
//        private System.Windows.Forms.ComboBox cmbDisplayBoard;
//        private System.Windows.Forms.PictureBox pBoxTrainLogo;
//        private PictureBox pictureBox_refresh;
//    }
//}

namespace IPIS
{
    partial class ucSetUpBoardSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucSetUpBoardSetting));
            tableLayoutPanelBoardSetting = new TableLayoutPanel();
            panelleftBoardSetting = new Panel();
            treeViewPort = new TreeView();
            panelContentBoardSetting = new Panel();
            pBoxTrainLogo = new PictureBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            cmbDisplayBoard = new ComboBox();
            lblSelectDisplayBoard = new Label();
            pictureBox_refresh = new PictureBox();
            tableLayoutPanelBoardSetting.SuspendLayout();
            panelleftBoardSetting.SuspendLayout();
            panelContentBoardSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pBoxTrainLogo).BeginInit();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox_refresh).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanelBoardSetting
            // 
            tableLayoutPanelBoardSetting.ColumnCount = 2;
            tableLayoutPanelBoardSetting.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 29.90196F));
            tableLayoutPanelBoardSetting.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70.09804F));
            tableLayoutPanelBoardSetting.Controls.Add(panelleftBoardSetting, 0, 0);
            tableLayoutPanelBoardSetting.Controls.Add(panelContentBoardSetting, 1, 1);
            tableLayoutPanelBoardSetting.Controls.Add(tableLayoutPanel2, 1, 0);
            tableLayoutPanelBoardSetting.Dock = DockStyle.Fill;
            tableLayoutPanelBoardSetting.Location = new Point(0, 0);
            tableLayoutPanelBoardSetting.Name = "tableLayoutPanelBoardSetting";
            tableLayoutPanelBoardSetting.RowCount = 2;
            tableLayoutPanelBoardSetting.RowStyles.Add(new RowStyle(SizeType.Percent, 6.656805F));
            tableLayoutPanelBoardSetting.RowStyles.Add(new RowStyle(SizeType.Percent, 93.34319F));
            tableLayoutPanelBoardSetting.Size = new Size(816, 634);
            tableLayoutPanelBoardSetting.TabIndex = 0;
            // 
            // panelleftBoardSetting
            // 
            panelleftBoardSetting.Controls.Add(treeViewPort);
            panelleftBoardSetting.Dock = DockStyle.Fill;
            panelleftBoardSetting.Location = new Point(3, 3);
            panelleftBoardSetting.Name = "panelleftBoardSetting";
            tableLayoutPanelBoardSetting.SetRowSpan(panelleftBoardSetting, 2);
            panelleftBoardSetting.Size = new Size(238, 628);
            panelleftBoardSetting.TabIndex = 2;
            // 
            // treeViewPort
            // 
            treeViewPort.BackColor = Color.Beige;
            treeViewPort.Dock = DockStyle.Fill;
            treeViewPort.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            treeViewPort.Location = new Point(0, 0);
            treeViewPort.Name = "treeViewPort";
            treeViewPort.Size = new Size(238, 628);
            treeViewPort.TabIndex = 0;
            treeViewPort.NodeMouseClick += treeViewPort_NodeMouseClick;
            // 
            // panelContentBoardSetting
            // 
            panelContentBoardSetting.BackColor = Color.WhiteSmoke;
            panelContentBoardSetting.BackgroundImageLayout = ImageLayout.Zoom;
            panelContentBoardSetting.Controls.Add(pBoxTrainLogo);
            panelContentBoardSetting.Dock = DockStyle.Fill;
            panelContentBoardSetting.Location = new Point(247, 45);
            panelContentBoardSetting.Name = "panelContentBoardSetting";
            panelContentBoardSetting.Size = new Size(566, 586);
            panelContentBoardSetting.TabIndex = 3;
            // 
            // pBoxTrainLogo
            // 
            pBoxTrainLogo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pBoxTrainLogo.Image = (Image)resources.GetObject("pBoxTrainLogo.Image");
            pBoxTrainLogo.Location = new Point(31, 108);
            pBoxTrainLogo.Name = "pBoxTrainLogo";
            pBoxTrainLogo.Size = new Size(500, 381);
            pBoxTrainLogo.SizeMode = PictureBoxSizeMode.Zoom;
            pBoxTrainLogo.TabIndex = 1;
            pBoxTrainLogo.TabStop = false;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.BackColor = Color.Beige;
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 23.4982338F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28.26855F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48.0565376F));
            tableLayoutPanel2.Controls.Add(cmbDisplayBoard, 2, 0);
            tableLayoutPanel2.Controls.Add(lblSelectDisplayBoard, 1, 0);
            tableLayoutPanel2.Controls.Add(pictureBox_refresh, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(247, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(566, 36);
            tableLayoutPanel2.TabIndex = 4;
            // 
            // cmbDisplayBoard
            // 
            cmbDisplayBoard.Dock = DockStyle.Fill;
            cmbDisplayBoard.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDisplayBoard.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cmbDisplayBoard.ForeColor = SystemColors.ControlText;
            cmbDisplayBoard.FormattingEnabled = true;
            cmbDisplayBoard.Location = new Point(296, 3);
            cmbDisplayBoard.Name = "cmbDisplayBoard";
            cmbDisplayBoard.Size = new Size(267, 27);
            cmbDisplayBoard.TabIndex = 1;
            cmbDisplayBoard.TabStop = false;
            cmbDisplayBoard.SelectedIndexChanged += cmbDisplayBoard_SelectedIndexChanged;
            // 
            // lblSelectDisplayBoard
            // 
            lblSelectDisplayBoard.AutoSize = true;
            lblSelectDisplayBoard.BackColor = Color.Beige;
            lblSelectDisplayBoard.Dock = DockStyle.Fill;
            lblSelectDisplayBoard.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSelectDisplayBoard.ForeColor = Color.DarkRed;
            lblSelectDisplayBoard.Location = new Point(136, 0);
            lblSelectDisplayBoard.Name = "lblSelectDisplayBoard";
            lblSelectDisplayBoard.Size = new Size(154, 36);
            lblSelectDisplayBoard.TabIndex = 0;
            lblSelectDisplayBoard.Text = "Display Boards/Hub :";
            lblSelectDisplayBoard.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pictureBox_refresh
            // 
            pictureBox_refresh.Dock = DockStyle.Fill;
            pictureBox_refresh.Image = (Image)resources.GetObject("pictureBox_refresh.Image");
            pictureBox_refresh.Location = new Point(3, 3);
            pictureBox_refresh.Name = "pictureBox_refresh";
            pictureBox_refresh.Size = new Size(127, 30);
            pictureBox_refresh.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox_refresh.TabIndex = 2;
            pictureBox_refresh.TabStop = false;
            pictureBox_refresh.Click += pictureBox_refresh_Click;
            // 
            // ucSetUpBoardSetting
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tableLayoutPanelBoardSetting);
            Name = "ucSetUpBoardSetting";
            Size = new Size(816, 634);
            Load += ucSetUpBoardSetting_Load;
            tableLayoutPanelBoardSetting.ResumeLayout(false);
            panelleftBoardSetting.ResumeLayout(false);
            panelContentBoardSetting.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pBoxTrainLogo).EndInit();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox_refresh).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBoardSetting;
        private System.Windows.Forms.Panel panelleftBoardSetting;
        private System.Windows.Forms.TreeView treeViewPort;
        private System.Windows.Forms.Panel panelContentBoardSetting;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label lblSelectDisplayBoard;
        private System.Windows.Forms.ComboBox cmbDisplayBoard;
        private System.Windows.Forms.PictureBox pBoxTrainLogo;
        private PictureBox pictureBox_refresh;
    }
}
