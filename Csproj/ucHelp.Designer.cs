namespace IPIS
{
    partial class ucHelp
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
            if (disposing)
            {
                CloseCurrentPdf();
                if (components != null)
                {
                    components.Dispose();
                }
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
            tableLayoutPanel1 = new TableLayoutPanel();
            panelRight = new Panel();
            txtHelp = new RichTextBox();
            pdfViewer = new PdfiumViewer.PdfViewer();
            tabControl1 = new TabControl();
            ContentsPage = new TabPage();
            treeHelp = new TreeView();
            DocumentPage = new TabPage();
            treeDocs = new TreeView();
            tlpDocButtons = new TableLayoutPanel();
            btnOpenDoc = new Button();
            btnDownloadDoc = new Button();
            tableLayoutPanel1.SuspendLayout();
            panelRight.SuspendLayout();
            tabControl1.SuspendLayout();
            ContentsPage.SuspendLayout();
            DocumentPage.SuspendLayout();
            tlpDocButtons.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 26.06541F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 73.9345856F));
            tableLayoutPanel1.Controls.Add(panelRight, 1, 0);
            tableLayoutPanel1.Controls.Add(tabControl1, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(1009, 560);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panelRight
            // 
            panelRight.Controls.Add(txtHelp);
            panelRight.Controls.Add(pdfViewer);
            panelRight.Dock = DockStyle.Fill;
            panelRight.Location = new Point(266, 3);
            panelRight.Name = "panelRight";
            panelRight.Size = new Size(740, 554);
            panelRight.TabIndex = 2;
            // 
            // txtHelp
            // 
            txtHelp.Dock = DockStyle.Fill;
            txtHelp.Font = new Font("Cambria", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtHelp.Location = new Point(0, 0);
            txtHelp.Name = "txtHelp";
            txtHelp.ReadOnly = true;
            txtHelp.Size = new Size(740, 554);
            txtHelp.TabIndex = 0;
            txtHelp.Text = "";
            // 
            // pdfViewer
            // 
            pdfViewer.Dock = DockStyle.Fill;
            pdfViewer.Location = new Point(0, 0);
            pdfViewer.Name = "pdfViewer";
            pdfViewer.ShowBookmarks = false;
            pdfViewer.Size = new Size(740, 554);
            pdfViewer.TabIndex = 1;
            pdfViewer.Visible = false;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(ContentsPage);
            tabControl1.Controls.Add(DocumentPage);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tabControl1.Location = new Point(3, 3);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(257, 554);
            tabControl1.TabIndex = 1;
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
            // 
            // ContentsPage
            // 
            ContentsPage.BorderStyle = BorderStyle.Fixed3D;
            ContentsPage.Controls.Add(treeHelp);
            ContentsPage.Font = new Font("Cambria", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ContentsPage.Location = new Point(4, 26);
            ContentsPage.Name = "ContentsPage";
            ContentsPage.Padding = new Padding(3);
            ContentsPage.Size = new Size(249, 524);
            ContentsPage.TabIndex = 0;
            ContentsPage.Text = "Help Content";
            ContentsPage.UseVisualStyleBackColor = true;
            // 
            // treeHelp
            // 
            treeHelp.Dock = DockStyle.Fill;
            treeHelp.HideSelection = false;
            treeHelp.Location = new Point(3, 3);
            treeHelp.Name = "treeHelp";
            treeHelp.Size = new Size(239, 514);
            treeHelp.TabIndex = 0;
            treeHelp.AfterSelect += treeHelp_AfterSelect;
            // 
            // DocumentPage
            // 
            DocumentPage.BorderStyle = BorderStyle.Fixed3D;
            DocumentPage.Controls.Add(treeDocs);
            DocumentPage.Controls.Add(tlpDocButtons);
            DocumentPage.Font = new Font("Cambria", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            DocumentPage.Location = new Point(4, 26);
            DocumentPage.Name = "DocumentPage";
            DocumentPage.Padding = new Padding(3);
            DocumentPage.Size = new Size(249, 524);
            DocumentPage.TabIndex = 1;
            DocumentPage.Text = "Software Operation Manual";
            DocumentPage.UseVisualStyleBackColor = true;
            // 
            // treeDocs
            // 
            treeDocs.Dock = DockStyle.Fill;
            treeDocs.HideSelection = false;
            treeDocs.Location = new Point(3, 3);
            treeDocs.Name = "treeDocs";
            treeDocs.Size = new Size(239, 514);
            treeDocs.TabIndex = 0;
            treeDocs.AfterSelect += treeDocs_AfterSelect;
            // 
            // tlpDocButtons (Open / Download under the document list)
            // 
            tlpDocButtons.ColumnCount = 2;
            tlpDocButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpDocButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpDocButtons.Controls.Add(btnOpenDoc, 0, 0);
            tlpDocButtons.Controls.Add(btnDownloadDoc, 1, 0);
            tlpDocButtons.Dock = DockStyle.Bottom;
            tlpDocButtons.Location = new Point(3, 477);
            tlpDocButtons.Name = "tlpDocButtons";
            tlpDocButtons.RowCount = 1;
            tlpDocButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpDocButtons.Size = new Size(239, 40);
            tlpDocButtons.TabIndex = 1;
            // 
            // btnOpenDoc
            // 
            btnOpenDoc.Dock = DockStyle.Fill;
            btnOpenDoc.Enabled = false;
            btnOpenDoc.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnOpenDoc.Location = new Point(3, 3);
            btnOpenDoc.Name = "btnOpenDoc";
            btnOpenDoc.Size = new Size(113, 34);
            btnOpenDoc.TabIndex = 0;
            btnOpenDoc.Text = "Open";
            btnOpenDoc.UseVisualStyleBackColor = true;
            btnOpenDoc.Click += btnOpenDoc_Click;
            // 
            // btnDownloadDoc
            // 
            btnDownloadDoc.Dock = DockStyle.Fill;
            btnDownloadDoc.Enabled = false;
            btnDownloadDoc.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDownloadDoc.Location = new Point(122, 3);
            btnDownloadDoc.Name = "btnDownloadDoc";
            btnDownloadDoc.Size = new Size(114, 34);
            btnDownloadDoc.TabIndex = 1;
            btnDownloadDoc.Text = "Download";
            btnDownloadDoc.UseVisualStyleBackColor = true;
            btnDownloadDoc.Click += btnDownloadDoc_Click;
            // 
            // ucHelp
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Beige;
            Controls.Add(tableLayoutPanel1);
            ForeColor = Color.DarkBlue;
            Margin = new Padding(3, 2, 3, 2);
            Name = "ucHelp";
            Size = new Size(1009, 560);
            Load += ucStatusPreview_Load;
            tableLayoutPanel1.ResumeLayout(false);
            panelRight.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            ContentsPage.ResumeLayout(false);
            tlpDocButtons.ResumeLayout(false);
            DocumentPage.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panelRight;
        private RichTextBox txtHelp;
        private PdfiumViewer.PdfViewer pdfViewer;
        private TabControl tabControl1;
        private TabPage ContentsPage;
        private TreeView treeHelp;
        private TabPage DocumentPage;
        private TreeView treeDocs;
        private TableLayoutPanel tlpDocButtons;
        private Button btnOpenDoc;
        private Button btnDownloadDoc;
    }
}