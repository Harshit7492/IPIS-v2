using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace IPISv2.Controls
{
    public class AppSplashOverlay : Panel
    {
        private System.Windows.Forms.Timer _animationTimer;
        private int _dotCount = 0;
        private Label _lblCliBrand;
        private Label _lblTitle;
        private Label _lblStatus;
        private string _baseStatusText = "Loading application data";

        public AppSplashOverlay()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(15, 23, 42); // Antigravity CLI dark slate theme
            this.DoubleBuffered = true;

            // CLI Brand Tag
            _lblCliBrand = new Label
            {
                Text = "RDSO/SPN/TC/108/2019 , Version-2.0",
                Font = new Font("Consolas", 10F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(148, 163, 184), // Slate-400
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Main Title
            _lblTitle = new Label
            {
                Text = "IP BASED Integrated Passenger Information System",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(253, 185, 39), // Amber / Gold accent
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // CLI Status Line with slow 3-dots loading
            _lblStatus = new Label
            {
                Text = "> " + _baseStatusText + "...",
                Font = new Font("Consolas", 12F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(226, 232, 240), // Off-white terminal text
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter
            };

            this.Controls.Add(_lblCliBrand);
            this.Controls.Add(_lblTitle);
            this.Controls.Add(_lblStatus);

            this.Resize += AppSplashOverlay_Resize;

            // Timer for slow loading of 3 dots (~350ms per dot step)
            _animationTimer = new System.Windows.Forms.Timer();
            _animationTimer.Interval = 350;
            _animationTimer.Tick += (s, e) =>
            {
                // 3 steps for 3 dots: 0 -> 1 -> 2 -> 0 ... (each dot lights exactly once in turn)
                _dotCount = (_dotCount + 1) % 3;
                _lblStatus.Text = "> " + _baseStatusText + GetStatusDots();
                this.Invalidate();
            };
        }

        /// <summary>
        /// Status-line dots matching the lit dot: ".", "..", "..." (padded so the text does not shift).
        /// </summary>
        private string GetStatusDots()
        {
            return new string('.', _dotCount + 1).PadRight(3, ' ');
        }

        public void SetStatus(string status)
        {
            _baseStatusText = status;
            string dots = GetStatusDots();
            if (_lblStatus.InvokeRequired)
            {
                _lblStatus.Invoke(new Action(() => _lblStatus.Text = "> " + _baseStatusText + dots));
            }
            else
            {
                _lblStatus.Text = "> " + _baseStatusText + dots;
            }
        }

        public void Start()
        {
            this.Visible = true;
            this.BringToFront();
            _animationTimer.Start();
            UpdateLayout();
        }

        public void Stop()
        {
            _animationTimer.Stop();
            this.Visible = false;
        }

        private void AppSplashOverlay_Resize(object sender, EventArgs e)
        {
            UpdateLayout();
        }

        private void UpdateLayout()
        {
            int cy = this.Height / 2;

            _lblCliBrand.Size = new Size(Math.Max(this.Width - 40, 100), 25);
            _lblCliBrand.Location = new Point(20, cy - 80);

            _lblTitle.Size = new Size(Math.Max(this.Width - 40, 100), 40);
            _lblTitle.Location = new Point(20, cy - 45);

            _lblStatus.Size = new Size(Math.Max(this.Width - 40, 100), 35);
            _lblStatus.Location = new Point(20, cy + 20);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int cx = this.Width / 2;
            int cy = this.Height / 2 + 75;

            // Draw 3 glowing CLI dots below status text (slow sequence)
            int dotRadius = 5;
            int dotSpacing = 22;
            int startX = cx - dotSpacing;

            for (int i = 0; i < 3; i++)
            {
                int dotX = startX + (i * dotSpacing);
                bool isActive = (i == _dotCount);

                Color dotColor = isActive
                    ? Color.FromArgb(253, 185, 39) // Active amber dot
                    : Color.FromArgb(71, 85, 105);  // Dim slate dot

                using (SolidBrush brush = new SolidBrush(dotColor))
                {
                    g.FillEllipse(brush, dotX - dotRadius, cy - dotRadius, dotRadius * 2, dotRadius * 2);
                }
            }
        }
    }
}
