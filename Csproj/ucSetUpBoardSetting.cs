using DataBaseHandler;
using DataModel;
using Global_Data;
using Helper;
using LogManager;
using System.Data;

namespace IPIS
{
    public partial class ucSetUpBoardSetting : UserControl
    {
        public List<string> lstdisplaySelect = new List<string>
                        {"----Select Display Board----"};

        public List<string> lstdisplayBoardswithPDC = new List<string>
                        {"----Select Display Board----","Platform Data Controller",
                            "Single Line/Platform Board","Multiline Display Board",
                            "At a Glance Display Board","Video Display Board","Advance Video Display Board"};

        public List<string> lstdisplayBoardsOnly = new List<string>
                        {"----Select Display Board----",
                            "Single Line/Platform Board","Multiline Display Board",
                            "At a Glance Display Board","Video Display Board","Advance Video Display Board",
                            "Coach Guidance Display Board"};

        List<HUBCONFIG> _lsthubList = new List<HUBCONFIG>();
        List<BoardSettingInfo> _lstdirectboards = new List<BoardSettingInfo>();
        private PictureBox defaultLogo;
        Image imgNormal;
        Image imgPressed;
        public ucSetUpBoardSetting()
        {
            InitializeComponent();
            imgNormal = Image.FromFile(AppHelper.GetImagePath("reload.png"));
            imgPressed = Image.FromFile(AppHelper.GetImagePath("refresh.png"));

            pictureBox_refresh.Image = imgNormal;
            pictureBox_refresh.MouseDown += pictureBox_refresh_MouseDown;
            pictureBox_refresh.MouseUp += pictureBox_refresh_MouseUp;
            pictureBox_refresh.MouseLeave += pictureBox_refresh_MouseLeave;

        }
        private void ucSetUpBoardSetting_Load(object sender, EventArgs e)
        {
            try
            {
                defaultLogo = pBoxTrainLogo;
                UpdateEthernetPorts(GlobalData._lstAdminSetting.Ethernet_Port);
            }
            catch (Exception ex)
            {
                LoggerService.SettingsLogger.Error(ex, ex.Message);
            }
        }
        public void UpdateEthernetPorts(int Enternetports)
        {
            if (treeViewPort == null) return;
            treeViewPort.BeginUpdate();

            try
            {
                treeViewPort.Nodes.Clear();
                TreeNode root = new TreeNode("Central Display Controller");
                _lsthubList = _dbService.Get_Hub_Config(0);
                _lstdirectboards = _dbService.Get_Hub_DirectBoard(0);
                for (int i = 1; i <= Enternetports; i++)
                {
                    var hub = _lsthubList.FirstOrDefault(x => x.HUB_ID == i);
                    var firstHub = _lsthubList.FirstOrDefault();
                    var hubdirect = firstHub?.DirectBoardsInfo;
                    TreeNode parentNode;
                    if (hubdirect != null)
                    {
                        if (hubdirect.Where(x => x.NODE_ID == i).Count() > 0)
                        {
                            string _parentname = hubdirect.Where(x => x.NODE_ID == i).FirstOrDefault().BOARD_NAME;
                            parentNode = new TreeNode(_parentname)
                            {
                                Tag = new NodeTag { BOARD_ID = hubdirect.Where(x => x.NODE_ID == i).FirstOrDefault().BOARD_ID, NodeId = hubdirect.Where(x => x.NODE_ID == i).FirstOrDefault().NODE_ID, NodeName = _parentname, IsParent = true },
                                ForeColor = Color.DarkBlue,
                                NodeFont = new Font(treeViewPort.Font, FontStyle.Bold)
                            };
                            root.Nodes.Add(parentNode);
                            continue;
                        }
                    }
                    if (hub != null)
                    {
                        string _parentname = hub.NODE_NAME;
                        parentNode = new TreeNode(_parentname)
                        {
                            Tag = new NodeTag { NodeId = hub.HUB_ID, NodeName = _parentname, IsParent = true },
                            ForeColor = Color.DarkBlue,
                            NodeFont = new Font(treeViewPort.Font, FontStyle.Bold)
                        };

                        for (int subport = 1; subport <= Enternetports / 3; subport++)
                        {
                            TreeNode childNode;
                            var relatedBoarddata = hub.RelatedBoardsInfo.Where(x => x.HUB_ID == hub.HUB_ID && x.NODE_ID == subport);
                            string _childname = string.Empty;
                            if (relatedBoarddata.Count() == 0)
                            {
                                _childname = $"vacant sub port {subport}";
                                childNode = new TreeNode(_childname)
                                {
                                    Tag = new NodeTag { NodeId = subport, NodeName = _childname, Parent_NodeId = hub.HUB_ID, Parent_NodeName = _parentname, IsParent = false },
                                    ForeColor = Color.DarkBlue,
                                    NodeFont = new Font(treeViewPort.Font, FontStyle.Bold)
                                };
                                parentNode.Nodes.Add(childNode);
                            }
                            else
                            {
                                foreach (var itemchild in relatedBoarddata)
                                {
                                    _childname = itemchild.BOARD_NAME;
                                    bool exists = parentNode.Nodes.Cast<TreeNode>()
                                                                  .Any(n => n.Text.Equals(_childname, StringComparison.OrdinalIgnoreCase));

                                    if (itemchild.NODE_ID == subport)
                                    {
                                        childNode = new TreeNode(_childname)
                                        {
                                            Tag = new NodeTag { BOARD_ID = itemchild.BOARD_ID, NodeId = subport, NodeName = _childname, Parent_NodeId = hub.HUB_ID, Parent_NodeName = _parentname, IsParent = false },
                                            ForeColor = Color.DarkBlue,
                                            NodeFont = new Font(treeViewPort.Font, FontStyle.Bold)
                                        };
                                        parentNode.Nodes.Add(childNode);
                                    }
                                    else
                                    {
                                        _childname = $"vacant sub port {subport}";
                                        childNode = new TreeNode(_childname)
                                        {
                                            Tag = new NodeTag { BOARD_ID = itemchild.BOARD_ID, NodeId = subport, NodeName = _childname, Parent_NodeId = hub.HUB_ID, Parent_NodeName = _parentname, IsParent = false },
                                            ForeColor = Color.DarkBlue,
                                            NodeFont = new Font(treeViewPort.Font, FontStyle.Bold)
                                        };
                                        parentNode.Nodes.Add(childNode);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        string _parentname = $"Vacant Ethernet Port {i}";
                        if (_lsthubList.Count == 0 && _lstdirectboards.Count > 0)
                        {
                            var board = _lstdirectboards.Where(x => x.NODE_ID == i);
                            if (board.Count() > 0)
                            {
                                _parentname = board.FirstOrDefault().BOARD_NAME;
                                parentNode = new TreeNode(_parentname)
                                {
                                    Tag = new NodeTag { BOARD_ID = board.FirstOrDefault().BOARD_ID, NodeId = board.FirstOrDefault().NODE_ID, NodeName = _parentname, IsParent = true },
                                    ForeColor = Color.DarkBlue,
                                    NodeFont = new Font(treeViewPort.Font, FontStyle.Bold)
                                };
                            }
                            else
                            {
                                _parentname = $"Vacant Ethernet Port {i}";
                                parentNode = new TreeNode(_parentname)
                                {
                                    Tag = new NodeTag { NodeId = i, NodeName = _parentname, IsParent = true },
                                    ForeColor = Color.Gray,
                                    NodeFont = new Font(treeViewPort.Font, FontStyle.Italic)
                                };
                            }
                        }
                        else
                        {
                            _parentname = $"Vacant Ethernet Port {i}";
                            parentNode = new TreeNode(_parentname)
                            {
                                Tag = new NodeTag { NodeId = i, NodeName = _parentname, IsParent = true },
                                ForeColor = Color.Gray,
                                NodeFont = new Font(treeViewPort.Font, FontStyle.Italic)
                            };
                        }
                    }
                    root.Nodes.Add(parentNode);
                }

                if (Enternetports == 0)
                    root.Nodes.Add("Valid port numbers not found!!");

                treeViewPort.Nodes.Add(root);
                root.Expand();
            }
            catch (Exception ex)
            {
                LoggerService.SettingsLogger.Error(ex, ex.Message);
            }
            finally
            {
                treeViewPort.EndUpdate();
            }
        }
        private void cmbDisplayBoard_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDisplayBoard.SelectedIndex == 0)
            {
                if (_lsthubList.Count == 0)
                {
                    panelContentBoardSetting.Controls.Clear();
                    if (defaultLogo != null)
                    {
                        panelContentBoardSetting.Controls.Add(defaultLogo);
                        defaultLogo.BringToFront();
                    }
                }
                return;
            }

            string selectedboardType = cmbDisplayBoard.SelectedItem.ToString();
            string _boardtype = string.Empty;
            if (selectedboardType == "Coach Guidance Display Board")
                _boardtype = "CGDB";
            else if (selectedboardType == "Multiline Display Board")
                _boardtype = "MLDB";
            else if (selectedboardType == "At a Glance Display Board")
                _boardtype = "AGDB";
            else if (selectedboardType == "Advance Video Display Board")
                _boardtype = "AVDB";
            else if (selectedboardType == "Video Display Board")
                _boardtype = "VDB";
            else if (selectedboardType == "Platform Data Controller")
                _boardtype = "PDC";
            Appdata.currentnodename = selectedboardType;
            Appdata.boardtype = _boardtype;
            TreeNode selectedNode = treeViewPort.SelectedNode;
            if (selectedNode == null)
                return;

            Appdata.boardName = selectedboardType;
            selectedNode.Text = selectedboardType;
            selectedNode.ForeColor = Color.DarkBlue;
            selectedNode.NodeFont = new Font(treeViewPort.Font, FontStyle.Bold);
            treeViewPort.BeginUpdate();
            treeViewPort.Refresh();
            treeViewPort.EndUpdate();

            panelContentBoardSetting.Controls.Clear();
            UserControl uc = CreateUserControl(selectedboardType);
            if (uc != null)
            {
                uc.Dock = DockStyle.Fill;
                panelContentBoardSetting.Controls.Add(uc);
            }
        }
        private void treeViewPort_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            int NodeId = 0;
            int Parent_NodeId = 0;
            bool IsParent = false;
            string NodeName = string.Empty;
            string Parent_NodeName = string.Empty;

            if (e.Node.Tag is NodeTag tag)
            {

                NodeId = tag.NodeId;
                NodeName = tag.NodeName;
                Parent_NodeId = tag.Parent_NodeId;
                Parent_NodeName = tag.Parent_NodeName;
                IsParent = tag.IsParent;

                Appdata.BOARD_ID = tag.BOARD_ID;
                Appdata.IsParent = tag.IsParent;
                Appdata.childNodeID = NodeId;
                Appdata.parentNodeID = Parent_NodeId;
            }
            if (!string.IsNullOrEmpty(NodeName))
            {
                Appdata.currentnodeid = NodeId;
                Appdata.currentnodename = NodeName;
                Appdata.boardName = NodeName;

                if (NodeName.Equals("Advance Video Display Board", StringComparison.OrdinalIgnoreCase) || NodeName.Equals("AVDB", StringComparison.OrdinalIgnoreCase))
                    Appdata.boardtype = "AVDB";
                else if (NodeName.Equals("Video Display Board", StringComparison.OrdinalIgnoreCase) || NodeName.Equals("VDB", StringComparison.OrdinalIgnoreCase))
                    Appdata.boardtype = "VDB";
                else if (NodeName.Equals("Coach Guidance Display Board", StringComparison.OrdinalIgnoreCase) || NodeName.Equals("CGDB", StringComparison.OrdinalIgnoreCase))
                    Appdata.boardtype = "CGDB";
                else if (NodeName.Equals("Multiline Display Board", StringComparison.OrdinalIgnoreCase) || NodeName.Equals("MLDB", StringComparison.OrdinalIgnoreCase))
                    Appdata.boardtype = "MLDB";
                else if (NodeName.Equals("At a Glance Display Board", StringComparison.OrdinalIgnoreCase) || NodeName.Equals("AGDB", StringComparison.OrdinalIgnoreCase))
                    Appdata.boardtype = "AGDB";
                else if (NodeName.Equals("Platform Data Controller", StringComparison.OrdinalIgnoreCase) || NodeName.Equals("PDC", StringComparison.OrdinalIgnoreCase))
                    Appdata.boardtype = "PDC";
                else if (NodeName.Equals("Single Line/Platform Board", StringComparison.OrdinalIgnoreCase))
                    Appdata.boardtype = "PDC";


                ResetAllNodeStyles(treeViewPort.Nodes);
                if (NodeName.ToLower().Contains("vacant"))
                    e.Node.ForeColor = Color.Red;
                if (NodeName.ToLower().Contains("vacant ethernet port"))
                {
                    cmbDisplayBoard.DataSource = lstdisplayBoardswithPDC;
                    cmbDisplayBoard.SelectedIndex = 0;
                }
                else if (NodeName.ToLower().Contains("vacant sub port"))
                {
                    cmbDisplayBoard.DataSource = lstdisplayBoardsOnly;
                    cmbDisplayBoard.SelectedIndex = 0;
                }
                else
                {
                    cmbDisplayBoard.DataSource = lstdisplaySelect;
                    cmbDisplayBoard.SelectedIndex = 0;
                }

                if (!NodeName.ToLower().Contains("vacant"))
                {
                    panelContentBoardSetting.Controls.Clear();
                    UserControl uc = CreateUserControl(NodeName);
                    if (uc != null)
                    {
                        uc.Dock = DockStyle.Fill;
                        panelContentBoardSetting.Controls.Add(uc);
                    }
                    else
                    {
                        panelContentBoardSetting.Controls.Clear();
                        if (defaultLogo != null)
                        {
                            panelContentBoardSetting.Controls.Add(defaultLogo);
                            defaultLogo.BringToFront();
                        }
                    }
                }
                else
                {
                    panelContentBoardSetting.Controls.Clear();
                    if (defaultLogo != null)
                    {
                        panelContentBoardSetting.Controls.Add(defaultLogo);
                        defaultLogo.BringToFront();
                    }
                }
            }
        }
        private UserControl CreateUserControl(string boardType)
        {
            switch (boardType)
            {
                case "Platform Data Controller":
                    Appdata.boardtype = "PDC";
                    Appdata.boardName = boardType;
                    var ucPDC = new ucPDCBoardSetting();
                    ucPDC.OnEvent += UcPDC_OnEvent;
                    return ucPDC;

                case "Single Line/Platform Board":
                    Appdata.boardtype = "PDC";
                    Appdata.boardName = boardType;
                    var ucPDCx = new ucSingleLineBoardSetting();
                    ucPDCx.OnEvent += UcPDC_OnEvent;
                    return ucPDCx;

                case "Multiline Display Board":
                    Appdata.boardtype = "MLDB";
                    Appdata.boardName = boardType;
                    var ucPDCxx = new ucSingleLineBoardSetting();
                    ucPDCxx.OnEvent += UcPDC_OnEvent;
                    return ucPDCxx;

                case "At a Glance Display Board":
                    Appdata.boardtype = "AGDB";
                    Appdata.boardName = boardType;
                    var ucPDCxxx = new ucSingleLineBoardSetting();
                    ucPDCxxx.OnEvent += UcPDC_OnEvent;
                    return ucPDCxxx;

                case "Video Display Board":
                    Appdata.boardtype = "VDB";
                    Appdata.boardName = boardType;
                    var ucPDCxxxx = new ucSingleLineBoardSetting();
                    ucPDCxxxx.OnEvent += UcPDC_OnEvent;
                    return ucPDCxxxx;

                case "Coach Guidance Display Board":
                    Appdata.boardtype = "CGDB";
                    Appdata.boardName = boardType;
                    var ucPDCxxxxx = new ucCGDBBoardSetting();
                    ucPDCxxxxx.OnEvent += UcPDC_OnEvent;
                    return ucPDCxxxxx;
                case "Advance Video Display Board":
                    Appdata.boardtype = "AVDB";
                    Appdata.boardName = boardType;
                    var ucAdvanceVideo = new ucSingleLineBoardSetting();
                    ucAdvanceVideo.OnEvent += UcPDC_OnEvent;
                    return ucAdvanceVideo;


                default:
                    return null;
            }
        }
        private void UcPDC_OnEvent(object sender, eventInfo e)
        {
            if (e.eventType != "HubConfigSaved")
                return;
            if (e.eventType == "HubConfigSaved")
            {
                cmbDisplayBoard.SelectedIndex = 0;
                UpdateEthernetPorts(GlobalData._lstAdminSetting.Ethernet_Port);
                panelContentBoardSetting.Controls.Clear();
                panelContentBoardSetting.Controls.Add(defaultLogo);
                defaultLogo.BringToFront();
            }
        }
        private void ResetAllNodeStyles(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Text.Contains("Vacant Ethernet Port", StringComparison.OrdinalIgnoreCase) || node.Text.Contains("Vacant Sub Port", StringComparison.OrdinalIgnoreCase))
                {
                    node.ForeColor = Color.Gray;
                    node.NodeFont = new Font(treeViewPort.Font, FontStyle.Italic);
                }
                if (node.Nodes.Count > 0)
                    ResetAllNodeStyles(node.Nodes);
            }
        }
        private void pictureBox_refresh_Click(object sender, EventArgs e)
        {
            UpdateEthernetPorts(GlobalData._lstAdminSetting.Ethernet_Port);
        }
        private void pictureBox_refresh_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox_refresh.Image = imgPressed;
        }
        private void pictureBox_refresh_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox_refresh.Image = imgNormal;

        }
        private void pictureBox_refresh_MouseLeave(object sender, EventArgs e)
        {
            pictureBox_refresh.Image = imgNormal;
        }

        public void RefreshData()
        {
            UpdateEthernetPorts(GlobalData._lstAdminSetting.Ethernet_Port);
        }
    }

    public class NodeTag
    {
        public int BOARD_ID { get; set; }
        public bool IsParent { get; set; }
        public int Parent_NodeId { get; set; }
        public string Parent_NodeName { get; set; }
        public int NodeId { get; set; }
        public string NodeName { get; set; }
    }
}
