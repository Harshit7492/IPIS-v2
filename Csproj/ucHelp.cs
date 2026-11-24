using LogManager;
using PdfiumViewer;
using System.Xml;

namespace IPIS
{
    public partial class ucHelp : UserControl
    {
        private static readonly string[] DocOptions = { "User Manual", "Specification Docs" };
        private static readonly string AppVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion ?? "";
        private static readonly string[] DocExtensions = { ".pdf" };


        private static readonly string[] ApprovedDocuments =
        {
            "User Manual.pdf",
            "Installation And maintanance manual  Final(4) 10-08.pdf",
            "Troubleshoting manaul_Final.pdf",
            "Specification Docs.pdf",
            "Amendment No. 1 to the specification.pdf",
        };

        private readonly string _helpFile = Path.Combine(AppContext.BaseDirectory, "MyHelp2.xml");
        private readonly string _docsFolder = Path.Combine(AppContext.BaseDirectory, "Docs");

        private readonly Dictionary<string, string> _helpCache = new();                              // topic -> text
        private readonly Dictionary<string, string> _pathCache = new();                              // doc name -> file path
        private readonly Dictionary<string, PdfDocument> _pdfCache = new(StringComparer.OrdinalIgnoreCase); // UI thread only

        // Each new request cancels the previous one; _version drops outdated results.
        private CancellationTokenSource _cts;
        private int _version;
        private bool _released;

        // What the background loader hands back to the UI thread
        private sealed record LoadResult(string Path, PdfDocument Pdf, string Text, bool IsRtf);

        public ucHelp()
        {
            InitializeComponent();
            VisibleChanged += ucHelp_VisibleChanged;
        }

        // Called from Dispose (ucHelp.Designer.cs)
        private void CloseCurrentPdf() => ReleaseResources();

        // ------------------------------------------------------------------
        // Load: help XML is read and parsed off the UI thread
        // ------------------------------------------------------------------
        private async void ucStatusPreview_Load(object sender, EventArgs e)
        {
            //treeDocs.Nodes.Clear();
            //foreach (var name in DocOptions)
            //    treeDocs.Nodes.Add(new TreeNode(name));
            LoadDocumentList();

            try
            {
                var (xml, data) = await Task.Run(() =>
                {
                    if (!File.Exists(_helpFile)) return ((XmlDocument)null, (Dictionary<string, string>)null);

                    var doc = new XmlDocument();
                    doc.Load(_helpFile);

                    var map = new Dictionary<string, string>();
                    ParseNode(doc.DocumentElement, map);
                    return (doc, map);
                });

                if (IsDisposed || xml == null) return;

                _helpCache.Clear();
                foreach (var pair in data)
                    //_helpCache[pair.Key] = pair.Value;
                    _helpCache[pair.Key] = pair.Value?.Replace("{APP_VERSION}", AppVersion);   // fill APP VERSION in MyHelp2.xml

                BuildTree(xml);
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
        }

        private static void ParseNode(XmlNode node, Dictionary<string, string> data)
        {
            if (node == null) return;

            if (node.NodeType == XmlNodeType.Element && !data.ContainsKey(node.Name))
                data[node.Name] = node.InnerText?.Trim();

            foreach (XmlNode child in node.ChildNodes)
                ParseNode(child, data);
        }

        private void BuildTree(XmlDocument doc)
        {
            try
            {
                treeHelp.BeginUpdate();
                treeHelp.Nodes.Clear();

                var root = new TreeNode(doc.DocumentElement.Name);
                treeHelp.Nodes.Add(root);
                AddNodes(doc.DocumentElement, root);
                treeHelp.ExpandAll();
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);   // no MessageBox: it blocks the UI thread
            }
            finally
            {
                treeHelp.EndUpdate();
            }
        }

        private static void AddNodes(XmlNode xmlNode, TreeNode treeNode)
        {
            foreach (XmlNode child in xmlNode.ChildNodes)
            {
                if (child.NodeType != XmlNodeType.Element) continue;

                var node = new TreeNode(child.Name);
                treeNode.Nodes.Add(node);
                AddNodes(child, node);
            }
        }

        // ------------------------------------------------------------------
        // Selection handlers
        // ------------------------------------------------------------------
        private void treeDocs_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //if (e.Node != null) DisplayDoc(e.Node.Text);
            ShowDocNode(e.Node);
        }

        private void treeHelp_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ShowNode(e.Node);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == DocumentPage)
            {
                //if (treeDocs.SelectedNode != null) DisplayDoc(treeDocs.SelectedNode.Text);
                if (treeDocs.SelectedNode != null) ShowDocNode(treeDocs.SelectedNode);
                else if (treeDocs.Nodes.Count > 0) treeDocs.SelectedNode = treeDocs.Nodes[0];
                else ShowText("");
            }
            else if (tabControl1.SelectedTab == ContentsPage)
            {
                ShowNode(treeHelp.SelectedNode);
            }
        }

        private void ShowNode(TreeNode node)
        {
            if (node == null) { ShowText(""); return; }

            string key = node.Text;

            if (key == "Documents")
            {
                ShowText("DOCUMENTS & SPECIFICATIONS\n\n"
                       + "Please select a document from the tree on the left to view details:\n\n"
                       //+ string.Join("\n", DocOptions.Select(d => "• " + d)));
                       + string.Join("\n", treeDocs.Nodes.Cast<TreeNode>().Select(n => "• " + n.Text)));
            }
            else if (DocOptions.Contains(key))
            {
                DisplayDoc(key);
            }
            else if (key.Equals("Help Manual Document", StringComparison.OrdinalIgnoreCase))
            {
                DisplayDoc("User Manual");
            }
            else
            {
                ShowText(_helpCache.TryGetValue(key, out var text) ? text : "");
            }
        }

        // ------------------------------------------------------------------
        // Document display: lookup, file read and PDF parsing run in the background
        // ------------------------------------------------------------------
        private async void DisplayDoc(string key)
        {
            CancelPendingLoad();
            _released = false;
            int version = ++_version;
            _cts = new CancellationTokenSource();
            CancellationToken token = _cts.Token;

            try
            {
                _pathCache.TryGetValue(key, out string cachedPath);

                // Already opened: show instantly
                if (cachedPath != null && _pdfCache.TryGetValue(cachedPath, out var cachedPdf))
                {
                    ShowPdf(cachedPdf);
                    return;
                }

                ShowText($"Loading {key}...");

                LoadResult result = await Task.Run(() => LoadInBackground(key, cachedPath, token), token);

                // Superseded, or control hidden/disposed while loading
                if (version != _version || IsDisposed)
                {
                    result.Pdf?.Dispose();
                    return;
                }

                _pathCache[key] = result.Path;

                if (result.Pdf != null)
                {
                    // Another node may have opened the same file meanwhile
                    if (_pdfCache.TryGetValue(result.Path, out var existing))
                    {
                        result.Pdf.Dispose();
                        ShowPdf(existing);
                    }
                    else
                    {
                        _pdfCache[result.Path] = result.Pdf;
                        ShowPdf(result.Pdf);
                    }
                }
                else
                {
                    ShowText(result.Text, result.IsRtf);
                }
            }
            catch (OperationCanceledException)
            {
                // superseded or hidden
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
                if (version == _version && !IsDisposed)
                    ShowText($"Error displaying document: {ex.Message}");
            }
        }

        // Runs on a thread-pool thread: must not touch any controls.
        private LoadResult LoadInBackground(string key, string cachedPath, CancellationToken token)
        {
            string path = !string.IsNullOrEmpty(cachedPath) && File.Exists(cachedPath)
                ? cachedPath
                : FindDocFile(key);

            token.ThrowIfCancellationRequested();

            if (path == null)
                return new LoadResult(key, null, NotFoundMessage(key), false);

            string ext = Path.GetExtension(path).ToLowerInvariant();

            if (ext == ".pdf")
            {
                var pdf = PdfDocument.Load(path);
                if (token.IsCancellationRequested)
                {
                    pdf.Dispose();
                    token.ThrowIfCancellationRequested();
                }
                return new LoadResult(path, pdf, null, false);
            }

            return new LoadResult(path, null, File.ReadAllText(path), ext == ".rtf");
        }

        private string FindDocFile(string option)
        {
            string file = Path.Combine(_docsFolder, option + ".pdf");

            return File.Exists(file) ? file : null;
        }
        private string NotFoundMessage(string option)
        {
            string safeName = option.Replace("'", "").Replace(" ", "_");
            return "";
        }

        // ------------------------------------------------------------------
        // Documents tab: every file in the Docs folder, with Open / Download
        // ------------------------------------------------------------------

        /// <summary>
        /// Lists the approved documents (ApprovedDocuments) found in the Docs folder next to the exe.
        /// Node text = file name, Tag = full path. Other files in the folder are never shown.
        /// </summary>
        private void LoadDocumentList()
        {
            treeDocs.BeginUpdate();
            try
            {
                treeDocs.Nodes.Clear();

                if (!Directory.Exists(_docsFolder)) return;

                //var files = Directory.GetFiles(_docsFolder)
                //    .Where(f => DocExtensions.Contains(Path.GetExtension(f).ToLowerInvariant()))
                //    .Where(f => !Path.GetFileName(f).StartsWith("~$"))
                //    .Where(f => new FileInfo(f).Length > 0)
                //    .OrderBy(f => Path.GetFileName(f), StringComparer.OrdinalIgnoreCase);
                //
                //foreach (string file in files)
                //    treeDocs.Nodes.Add(new TreeNode(Path.GetFileName(file)) { Tag = file });

                // Only the approved documents (see ApprovedDocuments) that exist and are not empty.
                foreach (string name in ApprovedDocuments)
                {
                    string file = Path.Combine(_docsFolder, name);
                    if (File.Exists(file) && new FileInfo(file).Length > 0)
                        treeDocs.Nodes.Add(new TreeNode(name) { Tag = file });
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
            finally
            {
                treeDocs.EndUpdate();
                UpdateDocButtons();
            }
        }

        /// <summary>PDF: shown in the viewer. Other files: a note to use Open / Download.</summary>
        private void ShowDocNode(TreeNode node)
        {
            UpdateDocButtons();

            if (node == null) { ShowText(""); return; }

            string path = node.Tag as string;
            if (string.IsNullOrEmpty(path))
            {
                DisplayDoc(node.Text);
                return;
            }

            if (!File.Exists(path))
            {
                CancelPendingLoad();
                _version++;
                ShowText("File not found:\n" + path);
                return;
            }

            if (Path.GetExtension(path).Equals(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                _pathCache[node.Text] = path;   // DisplayDoc loads this exact file
                DisplayDoc(node.Text);
                return;
            }

            // Not a PDF: cannot be shown inside the screen.
            CancelPendingLoad();
            _version++;
            ShowText(Path.GetFileName(path) + "\n\n"
                   + "This document cannot be shown inside IPIS.\n"
                   + "Click \"Open\" to view it in its own program (for example Word), "
                   + "or \"Download\" to save a copy.");
        }

        private string GetSelectedDocPath()
        {
            string path = treeDocs.SelectedNode?.Tag as string;
            return !string.IsNullOrEmpty(path) && File.Exists(path) ? path : null;
        }

        private void UpdateDocButtons()
        {
            bool hasFile = GetSelectedDocPath() != null;
            btnOpenDoc.Enabled = hasFile;
            btnDownloadDoc.Enabled = hasFile;
        }

        /// <summary>Opens the selected document in its default program (PDF reader, Word ...).</summary>
        private void btnOpenDoc_Click(object sender, EventArgs e)
        {
            string path = GetSelectedDocPath();
            if (path == null) return;

            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(path) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
                MessageBox.Show("Could not open " + Path.GetFileName(path) + ".\n"
                    + "Check that a program for this file type (for example Word) is installed.\n\n" + ex.Message,
                    "Open Document", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>Saves a copy of the selected document where the user chooses.</summary>
        private void btnDownloadDoc_Click(object sender, EventArgs e)
        {
            string path = GetSelectedDocPath();
            if (path == null) return;

            string ext = Path.GetExtension(path);

            using (var dialog = new SaveFileDialog())
            {
                dialog.Title = "Download Document";
                dialog.FileName = Path.GetFileName(path);
                dialog.Filter = ext.TrimStart('.').ToUpperInvariant() + " file (*" + ext + ")|*" + ext + "|All files (*.*)|*.*";
                dialog.OverwritePrompt = true;

                if (dialog.ShowDialog(this) != DialogResult.OK) return;

                try
                {
                    File.Copy(path, dialog.FileName, true);
                    MessageBox.Show("Saved:\n" + dialog.FileName, "Download Document",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    LoggerService.Logger.Error(ex, ex.Message);
                    MessageBox.Show("Could not save the document.\n\n" + ex.Message, "Download Document",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        // ------------------------------------------------------------------
        // Viewers (UI thread)
        // ------------------------------------------------------------------
        private void ShowText(string text, bool isRtf = false)
        {
            pdfViewer.Visible = false;
            txtHelp.Visible = true;
            txtHelp.BringToFront();

            if (isRtf) txtHelp.Rtf = text;
            else txtHelp.Text = text ?? "";
        }

        private void ShowPdf(PdfDocument pdf)
        {
            txtHelp.Visible = false;
            pdfViewer.Visible = true;
            pdfViewer.BringToFront();
            pdfViewer.ShowBookmarks = false;

            if (!ReferenceEquals(pdfViewer.Document, pdf))
            {
                pdfViewer.Document = pdf;
                pdfViewer.ZoomMode = PdfViewerZoomMode.FitWidth;
            }
        }

        // ------------------------------------------------------------------
        // Hide / show: free PDFs while another screen is on display
        // ------------------------------------------------------------------
        private void ucHelp_VisibleChanged(object sender, EventArgs e)
        {
            if (!Visible)
            {
                ReleaseResources();
            }
            else if (_released)
            {
                if (tabControl1.SelectedTab == DocumentPage && treeDocs.SelectedNode != null)
                    //DisplayDoc(treeDocs.SelectedNode.Text);
                    ShowDocNode(treeDocs.SelectedNode);
                else
                    ShowNode(treeHelp.SelectedNode);
            }
        }

        private void CancelPendingLoad()
        {
            if (_cts == null) return;
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }

        private void ReleaseResources()
        {
            CancelPendingLoad();
            _version++;   // a load still running will dispose its own result

            try
            {
                if (pdfViewer != null && !pdfViewer.IsDisposed)
                    pdfViewer.Document = null;
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }

            foreach (var pdf in _pdfCache.Values)
            {
                try { pdf.Dispose(); }
                catch (Exception ex) { LoggerService.Logger.Error(ex, ex.Message); }
            }
            _pdfCache.Clear();
            _released = true;
        }
    }
}