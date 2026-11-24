//using IPISv2.HealthStatusReport;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Drawing.Drawing2D;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace IPISv2
//{
//    public partial class ucHSR : UserControl
//    {
//        private LivePreview livePreviewControl;
//        private LEDSORTSingleLineDisplayBoard ledSortMatrixControl;
//        private LEDSORTMultilineDisplayBoard ledSortMatrixControlMLDB;
//        private LEDSORTGenericDisplayBoard ledSortGenericDisplayBoard;
//        private bool isSyncing = false;

//        private Color selectedTabBackColor = Color.FromArgb(173, 216, 230); // Light Blue
//        private Color selectedTabForeColor = Color.FromArgb(0, 51, 102);    // Dark Blue for text
//        private Color unselectedTabBackColor = Color.FromArgb(55, 60, 75);  // Dark gray
//        private Color unselectedTabForeColor = Color.FromArgb(200, 200, 210); // Light gray
//        private Color tabBorderColor = Color.FromArgb(100, 110, 130);

//        private Panel panelLEDSortScrollContainer;

//        public ucHSR()
//        {
//            InitializeComponent();
//            if (IsInDesignMode()) return;

//            InitializeLivePreview();
//            InitializeLEDSortMatrix();

//            // Set tab control to use custom drawing
//            tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
//        }

//        private void InitializeLivePreview()
//        {
//            livePreviewControl = new LivePreview();
//            livePreviewControl.Dock = DockStyle.Fill;
//            tabPageLivePreview.Controls.Add(livePreviewControl);

//            livePreviewControl.BoardTypeChanged += (s, ev) => {
//                if (isSyncing) return;
//                isSyncing = true;
//                isSyncing = false;
//            };

//            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;
//        }

//        /// <summary>
//        /// Initialize the LED Sort Matrix controls in a scrollable panel so all boards are displayed vertically.
//        /// </summary>
//        private void InitializeLEDSortMatrix()
//        {
//            panelLEDSortScrollContainer = new Panel
//            {
//                Dock = DockStyle.Fill,
//                AutoScroll = true,
//                BackColor = Color.FromArgb(20, 20, 25)
//            };
//            tabPageLEDSortMatrix.Controls.Add(panelLEDSortScrollContainer);

//            ledSortMatrixControl = new LEDSORTSingleLineDisplayBoard();
//            ledSortMatrixControl.Dock = DockStyle.None;
//            panelLEDSortScrollContainer.Controls.Add(ledSortMatrixControl);

//            ledSortMatrixControlMLDB = new LEDSORTMultilineDisplayBoard();
//            ledSortMatrixControlMLDB.Dock = DockStyle.None;
//            panelLEDSortScrollContainer.Controls.Add(ledSortMatrixControlMLDB);

//            ledSortGenericDisplayBoard = new LEDSORTGenericDisplayBoard();
//            ledSortGenericDisplayBoard.Dock = DockStyle.None;
//            panelLEDSortScrollContainer.Controls.Add(ledSortGenericDisplayBoard);

//            panelLEDSortScrollContainer.Resize += (s, e) => LayoutAllLEDSortBoards();
//            ledSortMatrixControl.SizeChanged += (s, e) => LayoutAllLEDSortBoards();
//            ledSortMatrixControlMLDB.SizeChanged += (s, e) => LayoutAllLEDSortBoards();
//            ledSortGenericDisplayBoard.SizeChanged += (s, e) => LayoutAllLEDSortBoards();

//            LayoutAllLEDSortBoards();
//        }

//        private void LayoutAllLEDSortBoards()
//        {
//            if (panelLEDSortScrollContainer == null) return;

//            int containerWidth = panelLEDSortScrollContainer.ClientSize.Width;
//            if (containerWidth <= 0) containerWidth = tabPageLEDSortMatrix.Width;

//            int margin = 12;
//            int spacing = 16;
//            int colWidth = Math.Max(300, containerWidth - (margin * 2));

//            if (ledSortMatrixControl != null)
//            {
//                ledSortMatrixControl.Width = colWidth;
//                ledSortMatrixControl.Visible = true;
//                ledSortMatrixControl.Enabled = true;
//            }

//            if (ledSortMatrixControlMLDB != null)
//            {
//                ledSortMatrixControlMLDB.Width = colWidth;
//                ledSortMatrixControlMLDB.Visible = true;
//                ledSortMatrixControlMLDB.Enabled = true;
//            }

//            if (ledSortGenericDisplayBoard != null)
//            {
//                ledSortGenericDisplayBoard.Width = colWidth;
//                ledSortGenericDisplayBoard.Visible = true;
//                ledSortGenericDisplayBoard.Enabled = true;
//            }

//            int currentY = margin;

//            if (ledSortMatrixControl != null && ledSortMatrixControl.Visible)
//            {
//                ledSortMatrixControl.Location = new Point(margin, currentY);
//                currentY += ledSortMatrixControl.Height + spacing;
//            }

//            if (ledSortMatrixControlMLDB != null && ledSortMatrixControlMLDB.Visible)
//            {
//                ledSortMatrixControlMLDB.Location = new Point(margin, currentY);
//                currentY += ledSortMatrixControlMLDB.Height + spacing;
//            }

//            if (ledSortGenericDisplayBoard != null && ledSortGenericDisplayBoard.Visible)
//            {
//                ledSortGenericDisplayBoard.Location = new Point(margin, currentY);
//                currentY += ledSortGenericDisplayBoard.Height + spacing;
//            }
//        }

//        private void LoadAllLEDSortBoardsData()
//        {
//            ledSortMatrixControl?.LoadActiveBoardData();
//            ledSortMatrixControlMLDB?.LoadActiveBoardData();
//            ledSortGenericDisplayBoard?.LoadActiveBoardData();
//            LayoutAllLEDSortBoards();
//        }

//        private void cmbHSRBoardSelect_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            if (isSyncing) return;

//            // Update LivePreview
//            if (livePreviewControl != null)
//            {
//                isSyncing = true;
//                isSyncing = false;
//            }

//            if (panelLEDSortScrollContainer != null && tabControl1.SelectedTab == tabPageLEDSortMatrix)
//            {
//                Control target = null;

//                if (target != null)
//                {
//                    panelLEDSortScrollContainer.ScrollControlIntoView(target);
//                }
//            }
//        }

//        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            if (tabControl1.SelectedTab == tabPageLivePreview)
//            {
//                if (livePreviewControl != null)
//                {
//                    livePreviewControl.Visible = true;
//                    livePreviewControl.BringToFront();
//                }
//                if (panelLEDSortScrollContainer != null)
//                {
//                    panelLEDSortScrollContainer.Visible = false;
//                }
//            }
//            else if (tabControl1.SelectedTab == tabPageLEDSortMatrix)
//            {
//                if (livePreviewControl != null)
//                {
//                    livePreviewControl.Visible = false;
//                }
//                if (panelLEDSortScrollContainer != null)
//                {
//                    panelLEDSortScrollContainer.Visible = true;
//                    panelLEDSortScrollContainer.BringToFront();
//                }
//                LoadAllLEDSortBoardsData();
//            }
//            // Force redraw of tabs when selection changes
//            tabControl1.Invalidate();
//        }

//        /// <summary>
//        /// Custom drawing for tabs with light blue highlight for selected tab
//        /// </summary>
//        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
//        {
//            TabControl tabControl = sender as TabControl;
//            if (tabControl == null) return;

//            Graphics g = e.Graphics;
//            Rectangle tabRect = tabControl.GetTabRect(e.Index);

//            // Determine if this tab is selected
//            bool isSelected = (e.Index == tabControl.SelectedIndex);

//            // Set colors based on selection state
//            Color backColor = isSelected ? selectedTabBackColor : unselectedTabBackColor;
//            Color foreColor = isSelected ? selectedTabForeColor : unselectedTabForeColor;

//            // Create a gradient brush for the tab background
//            using (LinearGradientBrush brush = new LinearGradientBrush(
//                tabRect,
//                backColor,
//                isSelected ? Color.FromArgb(200, 230, 245) : Color.FromArgb(45, 50, 65),
//                LinearGradientMode.Vertical))
//            {
//                g.FillRectangle(brush, tabRect);
//            }

//            // Draw border around tab
//            using (Pen borderPen = new Pen(tabBorderColor, 1))
//            {
//                // For selected tab, draw a thicker bottom border to simulate underline
//                if (isSelected)
//                {
//                    // Draw border with thicker bottom line
//                    g.DrawLine(borderPen, tabRect.Left, tabRect.Bottom - 1, tabRect.Right, tabRect.Bottom - 1);
//                    // Draw left and right borders (optional - for cleaner look)
//                    g.DrawLine(borderPen, tabRect.Left, tabRect.Top, tabRect.Left, tabRect.Bottom - 1);
//                    g.DrawLine(borderPen, tabRect.Right - 1, tabRect.Top, tabRect.Right - 1, tabRect.Bottom - 1);

//                    // Draw a glow effect on selected tab (light blue underline)
//                    using (Pen glowPen = new Pen(Color.FromArgb(100, 173, 216, 230), 3))
//                    {
//                        int glowWidth = 4;
//                        g.DrawLine(glowPen,
//                            tabRect.Left + glowWidth,
//                            tabRect.Bottom - 1,
//                            tabRect.Right - glowWidth,
//                            tabRect.Bottom - 1);
//                    }
//                }
//                else
//                {
//                    // Standard border for unselected tabs
//                    g.DrawRectangle(borderPen, tabRect);
//                }
//            }

//            // Set up text formatting
//            StringFormat stringFormat = new StringFormat();
//            stringFormat.Alignment = StringAlignment.Center;
//            stringFormat.LineAlignment = StringAlignment.Center;

//            // Draw the tab text
//            using (SolidBrush textBrush = new SolidBrush(foreColor))
//            {
//                // Remove padding from tab text for better positioning
//                Rectangle textRect = new Rectangle(
//                    tabRect.X + 2,
//                    tabRect.Y + 2,
//                    tabRect.Width - 4,
//                    tabRect.Height - 4
//                );

//                g.DrawString(
//                    tabControl.TabPages[e.Index].Text,
//                    tabControl.Font,
//                    textBrush,
//                    textRect,
//                    stringFormat
//                );
//            }

//            // Add a small icon or indicator for selected tab
//            if (isSelected)
//            {
//                // Draw a small circle or dot indicator on the selected tab
//                using (SolidBrush indicatorBrush = new SolidBrush(Color.FromArgb(70, 130, 180)))
//                {
//                    Rectangle indicatorRect = new Rectangle(
//                        tabRect.X + 5,
//                        tabRect.Y + 5,
//                        6,
//                        6
//                    );
//                    g.FillEllipse(indicatorBrush, indicatorRect);
//                }
//            }

//            // Clean up
//            stringFormat.Dispose();
//        }

//        private bool IsInDesignMode()
//        {
//            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
//                return true;

//            Control ctrl = this;
//            while (ctrl != null)
//            {
//                if (ctrl.Site != null && ctrl.Site.DesignMode)
//                    return true;
//                ctrl = ctrl.Parent;
//            }
//            return false;
//        }

//        // Optional: Refresh tabs when control is resized
//        protected override void OnResize(EventArgs e)
//        {
//            base.OnResize(e);
//            if (tabControl1 != null)
//                tabControl1.Invalidate();
//        }
//    }
//}

using IPISv2.HealthStatusReport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPISv2
{
    public partial class ucHSR : UserControl
    {
        private LivePreview livePreviewControl;
        private LEDSORTSingleLineDisplayBoard ledSortMatrixControl;
        private LEDSORTMultilineDisplayBoard ledSortMatrixControlMLDB;
        private LEDSORTGenericDisplayBoard ledSortGenericDisplayBoard;
        private LEDSORTCoachGuidenceDisplayBoard ledSortCoachGuidenceDisplayBoard;
        private bool isSyncing = false;

        private Color selectedTabBackColor = Color.FromArgb(173, 216, 230); // Light Blue
        private Color selectedTabForeColor = Color.FromArgb(0, 51, 102);    // Dark Blue for text
        private Color unselectedTabBackColor = Color.FromArgb(55, 60, 75);  // Dark gray
        private Color unselectedTabForeColor = Color.FromArgb(200, 200, 210); // Light gray
        private Color tabBorderColor = Color.FromArgb(100, 110, 130);

        private Panel panelLEDSortScrollContainer;

        public ucHSR()
        {
            InitializeComponent();
            if (IsInDesignMode()) return;

            InitializeLivePreview();
            InitializeLEDSortMatrix();

            // Set tab control to use custom drawing
            tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
        }

        private void InitializeLivePreview()
        {
            livePreviewControl = new LivePreview();
            livePreviewControl.Dock = DockStyle.Fill;
            tabPageLivePreview.Controls.Add(livePreviewControl);

            livePreviewControl.BoardTypeChanged += (s, ev) => {
                if (isSyncing) return;
                isSyncing = true;
                isSyncing = false;
            };

            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;
        }

        /// <summary>
        /// Initialize the LED Sort Matrix controls in a scrollable panel so all boards are displayed vertically.
        /// </summary>
        private void InitializeLEDSortMatrix()
        {
            panelLEDSortScrollContainer = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.FromArgb(20, 20, 25)
            };
            tabPageLEDSortMatrix.Controls.Add(panelLEDSortScrollContainer);

            ledSortMatrixControl = new LEDSORTSingleLineDisplayBoard();
            ledSortMatrixControl.Dock = DockStyle.None;
            panelLEDSortScrollContainer.Controls.Add(ledSortMatrixControl);

            ledSortMatrixControlMLDB = new LEDSORTMultilineDisplayBoard();
            ledSortMatrixControlMLDB.Dock = DockStyle.None;
            panelLEDSortScrollContainer.Controls.Add(ledSortMatrixControlMLDB);

            ledSortGenericDisplayBoard = new LEDSORTGenericDisplayBoard();
            ledSortGenericDisplayBoard.Dock = DockStyle.None;
            panelLEDSortScrollContainer.Controls.Add(ledSortGenericDisplayBoard);

            ledSortCoachGuidenceDisplayBoard = new LEDSORTCoachGuidenceDisplayBoard();
            ledSortCoachGuidenceDisplayBoard.Dock = DockStyle.None;
            panelLEDSortScrollContainer.Controls.Add(ledSortCoachGuidenceDisplayBoard);

            panelLEDSortScrollContainer.Resize += (s, e) => LayoutAllLEDSortBoards();
            ledSortMatrixControl.SizeChanged += (s, e) => LayoutAllLEDSortBoards();
            ledSortMatrixControlMLDB.SizeChanged += (s, e) => LayoutAllLEDSortBoards();
            ledSortGenericDisplayBoard.SizeChanged += (s, e) => LayoutAllLEDSortBoards();
            ledSortCoachGuidenceDisplayBoard.SizeChanged += (s, e) => LayoutAllLEDSortBoards();

            LayoutAllLEDSortBoards();
        }

        private bool _isLayingOutLEDSort = false;

        private void LayoutAllLEDSortBoards()
        {
            if (panelLEDSortScrollContainer == null) return;

            // Setting a board's Width raises SizeChanged -> LayoutAllLEDSortBoards; ignore those nested calls
            if (_isLayingOutLEDSort) return;
            _isLayingOutLEDSort = true;

            panelLEDSortScrollContainer.SuspendLayout();
            try
            {
                LayoutAllLEDSortBoardsCore();
            }
            finally
            {
                panelLEDSortScrollContainer.ResumeLayout(true);
                _isLayingOutLEDSort = false;
            }
        }

        private void LayoutAllLEDSortBoardsCore()
        {
            int containerWidth = panelLEDSortScrollContainer.ClientSize.Width;
            if (containerWidth <= 0) containerWidth = tabPageLEDSortMatrix.Width;

            int margin = 12;
            int spacing = 16;
            int colWidth = Math.Max(300, containerWidth - (margin * 2));

            if (ledSortMatrixControl != null)
            {
                ledSortMatrixControl.Width = colWidth;
                ledSortMatrixControl.Visible = true;
                ledSortMatrixControl.Enabled = true;
            }

            if (ledSortMatrixControlMLDB != null)
            {
                ledSortMatrixControlMLDB.Width = colWidth;
                ledSortMatrixControlMLDB.Visible = true;
                ledSortMatrixControlMLDB.Enabled = true;
            }

            if (ledSortGenericDisplayBoard != null)
            {
                ledSortGenericDisplayBoard.Width = colWidth;
                ledSortGenericDisplayBoard.Visible = true;
                ledSortGenericDisplayBoard.Enabled = true;
            }

            if (ledSortCoachGuidenceDisplayBoard != null)
            {
                ledSortCoachGuidenceDisplayBoard.Width = colWidth;
                ledSortCoachGuidenceDisplayBoard.Visible = true;
                ledSortCoachGuidenceDisplayBoard.Enabled = true;
            }

            // Locations inside an AutoScroll panel are relative to the current scroll position
            // (AutoScrollPosition is negative when scrolled down), so add it to get the real position.
            Point scrollOffset = panelLEDSortScrollContainer.AutoScrollPosition;
            int currentY = margin;

            if (ledSortMatrixControl != null && ledSortMatrixControl.Visible)
            {
                ledSortMatrixControl.Location = new Point(margin + scrollOffset.X, currentY + scrollOffset.Y);
                currentY += ledSortMatrixControl.Height + spacing;
            }

            if (ledSortMatrixControlMLDB != null && ledSortMatrixControlMLDB.Visible)
            {
                ledSortMatrixControlMLDB.Location = new Point(margin + scrollOffset.X, currentY + scrollOffset.Y);
                currentY += ledSortMatrixControlMLDB.Height + spacing;
            }

            if (ledSortGenericDisplayBoard != null && ledSortGenericDisplayBoard.Visible)
            {
                ledSortGenericDisplayBoard.Location = new Point(margin + scrollOffset.X, currentY + scrollOffset.Y);
                currentY += ledSortGenericDisplayBoard.Height + spacing;
            }

            if (ledSortCoachGuidenceDisplayBoard != null && ledSortCoachGuidenceDisplayBoard.Visible)
            {
                ledSortCoachGuidenceDisplayBoard.Location = new Point(margin + scrollOffset.X, currentY + scrollOffset.Y);
                currentY += ledSortCoachGuidenceDisplayBoard.Height + spacing;
            }

            // Scroll range covers every board (whatever their dynamic heights) plus a bottom margin
            panelLEDSortScrollContainer.AutoScrollMinSize = new Size(0, currentY + margin);
        }

        private void LoadAllLEDSortBoardsData()
        {
            ledSortMatrixControl?.LoadActiveBoardData();
            ledSortMatrixControlMLDB?.LoadActiveBoardData();
            ledSortGenericDisplayBoard?.LoadActiveBoardData();
            ledSortCoachGuidenceDisplayBoard?.LoadActiveBoardData();
            LayoutAllLEDSortBoards();
        }

        private void cmbHSRBoardSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isSyncing) return;

            // Update LivePreview
            if (livePreviewControl != null)
            {
                isSyncing = true;
                isSyncing = false;
            }

            if (panelLEDSortScrollContainer != null && tabControl1.SelectedTab == tabPageLEDSortMatrix)
            {
                Control target = null;

                if (target != null)
                {
                    panelLEDSortScrollContainer.ScrollControlIntoView(target);
                }
            }
        }

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPageLivePreview)
            {
                if (livePreviewControl != null)
                {
                    livePreviewControl.Visible = true;
                    livePreviewControl.BringToFront();
                }
                if (panelLEDSortScrollContainer != null)
                {
                    panelLEDSortScrollContainer.Visible = false;
                }
            }
            else if (tabControl1.SelectedTab == tabPageLEDSortMatrix)
            {
                if (livePreviewControl != null)
                {
                    livePreviewControl.Visible = false;
                }
                if (panelLEDSortScrollContainer != null)
                {
                    panelLEDSortScrollContainer.Visible = true;
                    panelLEDSortScrollContainer.BringToFront();
                }
                LoadAllLEDSortBoardsData();
            }
            // Force redraw of tabs when selection changes
            tabControl1.Invalidate();
        }

        /// <summary>
        /// Custom drawing for tabs with light blue highlight for selected tab
        /// </summary>
        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabControl tabControl = sender as TabControl;
            if (tabControl == null) return;

            Graphics g = e.Graphics;
            Rectangle tabRect = tabControl.GetTabRect(e.Index);

            // Determine if this tab is selected
            bool isSelected = (e.Index == tabControl.SelectedIndex);

            // Set colors based on selection state
            Color backColor = isSelected ? selectedTabBackColor : unselectedTabBackColor;
            Color foreColor = isSelected ? selectedTabForeColor : unselectedTabForeColor;

            // Create a gradient brush for the tab background
            using (LinearGradientBrush brush = new LinearGradientBrush(
                tabRect,
                backColor,
                isSelected ? Color.FromArgb(200, 230, 245) : Color.FromArgb(45, 50, 65),
                LinearGradientMode.Vertical))
            {
                g.FillRectangle(brush, tabRect);
            }

            // Draw border around tab
            using (Pen borderPen = new Pen(tabBorderColor, 1))
            {
                // For selected tab, draw a thicker bottom border to simulate underline
                if (isSelected)
                {
                    // Draw border with thicker bottom line
                    g.DrawLine(borderPen, tabRect.Left, tabRect.Bottom - 1, tabRect.Right, tabRect.Bottom - 1);
                    // Draw left and right borders (optional - for cleaner look)
                    g.DrawLine(borderPen, tabRect.Left, tabRect.Top, tabRect.Left, tabRect.Bottom - 1);
                    g.DrawLine(borderPen, tabRect.Right - 1, tabRect.Top, tabRect.Right - 1, tabRect.Bottom - 1);

                    // Draw a glow effect on selected tab (light blue underline)
                    using (Pen glowPen = new Pen(Color.FromArgb(100, 173, 216, 230), 3))
                    {
                        int glowWidth = 4;
                        g.DrawLine(glowPen,
                            tabRect.Left + glowWidth,
                            tabRect.Bottom - 1,
                            tabRect.Right - glowWidth,
                            tabRect.Bottom - 1);
                    }
                }
                else
                {
                    // Standard border for unselected tabs
                    g.DrawRectangle(borderPen, tabRect);
                }
            }

            // Set up text formatting
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            // Draw the tab text
            using (SolidBrush textBrush = new SolidBrush(foreColor))
            {
                // Remove padding from tab text for better positioning
                Rectangle textRect = new Rectangle(
                    tabRect.X + 2,
                    tabRect.Y + 2,
                    tabRect.Width - 4,
                    tabRect.Height - 4
                );

                g.DrawString(
                    tabControl.TabPages[e.Index].Text,
                    tabControl.Font,
                    textBrush,
                    textRect,
                    stringFormat
                );
            }

            // Add a small icon or indicator for selected tab
            if (isSelected)
            {
                // Draw a small circle or dot indicator on the selected tab
                using (SolidBrush indicatorBrush = new SolidBrush(Color.FromArgb(70, 130, 180)))
                {
                    Rectangle indicatorRect = new Rectangle(
                        tabRect.X + 5,
                        tabRect.Y + 5,
                        6,
                        6
                    );
                    g.FillEllipse(indicatorBrush, indicatorRect);
                }
            }

            // Clean up
            stringFormat.Dispose();
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

        // Optional: Refresh tabs when control is resized
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (tabControl1 != null)
                tabControl1.Invalidate();
        }
    }
}