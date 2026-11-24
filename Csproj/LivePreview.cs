
using MaterialSkin;
using MaterialSkin.Controls;

namespace IPISv2
{
    public partial class LivePreview : UserControl
    {
        private List<MaterialCard> boardCards = new List<MaterialCard>();
        private const int BOARD_WIDTH = 96;
        private const int BOARD_HEIGHT = 32;
        private const int PIXEL_SIZE = 3;
        private const int BOARD_SPACING = 10;

        // MaterialSkin manager
        private MaterialSkinManager materialSkinManager;
        private bool _isLayingOut = false;
        private string _lastLayoutKey = null;
        private LivePreviewMouseWheelFilter _mouseWheelFilter;

        public LivePreview()
        {
            InitializeComponent();
            if (IsInDesignMode()) return;

            InitializeMaterialSkin();
            InitializeBoardTypes();
            //InitializeBoards();        

            lblBoardType.Visible = false;
            cmbBoardType.Visible = false;
            panelCGDBHeaderControls.Visible = false;

            panelScrollContainer.BringToFront();
            panelScrollContainer.BackColor = Color.FromArgb(20, 20, 25);
            panelScrollContainer.MouseDown += (s, e) => panelScrollContainer.Select();

            EnableDoubleBuffering(panelScrollContainer);

            _mouseWheelFilter = new LivePreviewMouseWheelFilter(this);
            Application.AddMessageFilter(_mouseWheelFilter);

            this.Disposed += (s, e) =>
            {
                if (_mouseWheelFilter != null)
                {
                    Application.RemoveMessageFilter(_mouseWheelFilter);
                    _mouseWheelFilter = null;
                }
            };

            panelScrollContainer.Resize += (s, e) => LayoutAllBoards();

            singleLineDisplayBoard.SizeChanged += (s, e) => LayoutAllBoards();
            multilineDisplayBoard.SizeChanged += (s, e) => LayoutAllBoards();
            agdbDisplayBoard.SizeChanged += (s, e) => LayoutAllBoards();
            coachGuidenceDisplayBoard.SizeChanged += (s, e) => LayoutAllBoards();


            LayoutAllBoards();
        }

        public event EventHandler BoardTypeChanged;

        public int SelectedBoardTypeIndex
        {
            get => cmbBoardType.SelectedIndex;
            set
            {
                if (value >= 0 && value < cmbBoardType.Items.Count)
                {
                    cmbBoardType.SelectedIndex = value;
                }
            }
        }

        private void InitializeBoardTypes()
        {
            cmbBoardType.Items.Clear();
            cmbBoardType.Items.Add("Single Line Display Board (SLDB)");
            cmbBoardType.Items.Add("Multi Line Display Board (MLDB)");
            cmbBoardType.Items.Add("At-a-Glance Display Board (AGDB)");
            //cmbBoardType.Items.Add("Coach Guidance Display Board (CGDB)");

            cmbBoardType.SelectedIndexChanged += CmbBoardType_SelectedIndexChanged;
            cmbBoardType.SelectedIndex = 0; // Default to Single Line Display Board
        }

        private void CmbBoardType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Do not hide the boards; keep all of them visible and laid out vertically
            LayoutAllBoards();
            BoardTypeChanged?.Invoke(this, EventArgs.Empty);
        }

        private void InitializeMaterialSkin()
        {
            // Get the parent form and apply MaterialSkin to it
            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                // Try to cast to MaterialForm
                var materialForm = parentForm as MaterialForm;
                if (materialForm != null)
                {
                    materialSkinManager = MaterialSkinManager.Instance;
                    materialSkinManager.AddFormToManage(materialForm);
                    materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
                    materialSkinManager.ColorScheme = new ColorScheme(
                        primary: Primary.BlueGrey800,
                        darkPrimary: Primary.BlueGrey900,
                        lightPrimary: Primary.BlueGrey500,
                        accent: Accent.LightBlue200,
                        textShade: TextShade.WHITE
                    );
                }
            }

            // Apply material styling to controls
            ApplyMaterialTheme();
        }

        private void ApplyMaterialTheme()
        {
            StyleControlsRecursive(this.Controls);
        }

        private void StyleControlsRecursive(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                if (control is Label label)
                {
                    label.Font = new Font("Roboto", 10, FontStyle.Regular);
                    label.ForeColor = Color.White;
                }
                else if (control is Button button)
                {
                    button.FlatStyle = FlatStyle.Flat;
                    button.BackColor = Color.FromArgb(66, 165, 245);
                    button.ForeColor = Color.White;
                    button.FlatAppearance.BorderSize = 0;
                    button.Font = new Font("Roboto", 10, FontStyle.Bold);
                    button.Cursor = Cursors.Hand;
                }
                else if (control is ComboBox comboBox)
                {
                    comboBox.Font = new Font("Roboto", 10);
                    comboBox.ForeColor = Color.Black;
                    comboBox.BackColor = Color.White;
                    comboBox.FlatStyle = FlatStyle.Standard;
                }
                else if (control is NumericUpDown numericUpDown)
                {
                    numericUpDown.Font = new Font("Roboto", 10);
                    numericUpDown.ForeColor = Color.Black;
                    numericUpDown.BackColor = Color.White;
                }

                if (control.HasChildren)
                {
                    StyleControlsRecursive(control.Controls);
                }
            }
        }

       



        private void ShowBoard(int index)
        {
            if (index == 0) // "ALL"
            {
                int currentY = 10;
                for (int i = 0; i < boardCards.Count; i++)
                {
                    boardCards[i].Enabled = true;
                    boardCards[i].Visible = true;
                    boardCards[i].Location = new Point(10, currentY);
                    currentY += boardCards[i].Height + BOARD_SPACING;
                }
                panelBoardContainer.Size = new Size(
                    760,
                    currentY + 10
                );
            }
            else // Specific board
            {
                int boardIdx = index - 1;
                for (int i = 0; i < boardCards.Count; i++)
                {
                    bool isSelected = (i == boardIdx);
                    boardCards[i].Enabled = isSelected;
                    boardCards[i].Visible = isSelected;
                    if (isSelected)
                    {
                        boardCards[i].Location = new Point(10, 10);
                    }
                }
                panelBoardContainer.Size = new Size(
                    760,
                    170 + 20
                );
            }

            LayoutAllBoards();
        }

        private void LayoutAllBoards()
        {
            // Ignore nested calls: setting a board's Width below raises SizeChanged -> LayoutAllBoards
            if (_isLayingOut) return;
            _isLayingOut = true;

            try
            {
                LayoutAllBoardsCore();
            }
            finally
            {
                _isLayingOut = false;
            }
        }

        private void LayoutAllBoardsCore()
        {
            // Ensure all boards and panelCGDBHeaderControls are child controls of panelScrollContainer
            if (!panelScrollContainer.Controls.Contains(singleLineDisplayBoard))
                panelScrollContainer.Controls.Add(singleLineDisplayBoard);
            if (!panelScrollContainer.Controls.Contains(multilineDisplayBoard))
                panelScrollContainer.Controls.Add(multilineDisplayBoard);
            if (!panelScrollContainer.Controls.Contains(agdbDisplayBoard))
                panelScrollContainer.Controls.Add(agdbDisplayBoard);
            if (!panelScrollContainer.Controls.Contains(coachGuidenceDisplayBoard))
                panelScrollContainer.Controls.Add(coachGuidenceDisplayBoard);

            // Disable Fill dock style so controls honor their bounds inside the scrollable container
            singleLineDisplayBoard.Dock = DockStyle.None;
            multilineDisplayBoard.Dock = DockStyle.None;
            agdbDisplayBoard.Dock = DockStyle.None;
            coachGuidenceDisplayBoard.Dock = DockStyle.None;

            // Make sure all controls are visible and enabled
            singleLineDisplayBoard.Visible = true;
            singleLineDisplayBoard.Enabled = true;

            multilineDisplayBoard.Visible = true;
            multilineDisplayBoard.Enabled = true;

            agdbDisplayBoard.Visible = true;
            agdbDisplayBoard.Enabled = true;

            coachGuidenceDisplayBoard.Visible = true;
            coachGuidenceDisplayBoard.Enabled = true;

            panelCGDBHeaderControls.Visible = false;
            panelCGDBHeaderControls.Enabled = false;

            // Calculate columns and layout
            int containerWidth = panelScrollContainer.ClientSize.Width;
            if (containerWidth <= 0) containerWidth = this.Width;

            // Margins and vertical spacing
            int margin = 12;
            int spacing = 16;
            int colWidth = containerWidth - (margin * 2);

            var boards = new List<Control>
            {
                singleLineDisplayBoard,
                multilineDisplayBoard,
                agdbDisplayBoard,
                coachGuidenceDisplayBoard
            };

            // Nothing changed since the last layout (same width, same board heights) -> keep positions.
            // Boards resize themselves on every data refresh; re-laying out then is not needed.
            string layoutKey = colWidth + "|" + string.Join("|", boards.ConvertAll(b => b.Height.ToString()));
            if (layoutKey == _lastLayoutKey)
                return;

            // Set the width of all boards
            singleLineDisplayBoard.Width = colWidth;
            multilineDisplayBoard.Width = colWidth;
            agdbDisplayBoard.Width = colWidth;
            coachGuidenceDisplayBoard.Width = colWidth;

            Point scrollOffset = panelScrollContainer.AutoScrollPosition;

            int currentY = margin;
            for (int i = 0; i < boards.Count; i++)
            {
                Control board = boards[i];
                board.Location = new Point(margin + scrollOffset.X, currentY + scrollOffset.Y);
                currentY += board.Height + spacing;
            }

            panelScrollContainer.AutoScrollMinSize = new Size(0, currentY + margin);

            panelBoardContainer.Location = new Point(scrollOffset.X, scrollOffset.Y);
            panelBoardContainer.Size = new Size(containerWidth, currentY);

            // Key after layout (board widths/heights may have adjusted themselves while being set)
            _lastLayoutKey = colWidth + "|" + string.Join("|", boards.ConvertAll(b => b.Height.ToString()));
        }

        private sealed class LivePreviewMouseWheelFilter : IMessageFilter
        {
            private readonly LivePreview _owner;

            public LivePreviewMouseWheelFilter(LivePreview owner)
            {
                _owner = owner;
            }

            public bool PreFilterMessage(ref Message m)
            {
                const int WM_MOUSEWHEEL = 0x020A;
                if (m.Msg == WM_MOUSEWHEEL && _owner != null && !_owner.IsDisposed && _owner.Visible)
                {
                    var container = _owner.panelScrollContainer;
                    if (container != null && !container.IsDisposed && container.Visible)
                    {
                        Point mouseScreenPos = Cursor.Position;
                        Rectangle screenBounds = container.RectangleToScreen(container.ClientRectangle);
                        if (screenBounds.Contains(mouseScreenPos))
                        {
                            int delta = (short)((m.WParam.ToInt64() >> 16) & 0xFFFF);
                            if (delta != 0)
                            {
                                int notches = delta / 120;
                                if (notches == 0) notches = delta > 0 ? 1 : -1;
                                int scrollStep = 90 * notches;

                                int currentScrollY = -container.AutoScrollPosition.Y;
                                int newY = currentScrollY - scrollStep;
                                if (newY < 0) newY = 0;

                                container.AutoScrollPosition = new Point(-container.AutoScrollPosition.X, newY);
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Turns on double buffering for a standard Panel (the property is protected, so it is set via reflection).
        /// </summary>
        private static void EnableDoubleBuffering(Control control)
        {
            try
            {
                typeof(Control)
                    .GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                    ?.SetValue(control, true, null);
            }
            catch
            {
                // Non-fatal: only reduces flicker
            }
        }

        private void cmbBoardSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowBoard(cmbBoardSelect.SelectedIndex);
        }

  
        private bool IsInDesignMode()
        {
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                return true;

            Control ctrl = this;
            while (ctrl != null)
            {
                if (ctrl.Site != null && ctrl.Site.DesignMode)
                    return true;
                ctrl = ctrl.Parent;
            }
            return false;
        }
    }

}