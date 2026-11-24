

namespace IPISv2.Controls
{
    public partial class LoaderControl : UserControl
    {
        private System.Windows.Forms.Timer animationTimer;
        private int angle = 0;
        private bool isSpinning = false;

        public LoaderControl()
        {
            InitializeComponent();

            // Set up the control
            this.BackColor = Color.FromArgb(240, 240, 240, 180);
            this.Size = new Size(120, 120);
            this.Visible = false;
            this.Name = "LoaderControl";
            this.DoubleBuffered = true; // For smoother animation

            // Initialize timer
            animationTimer = new System.Windows.Forms.Timer();
            animationTimer.Interval = 15; // 15ms for smooth animation
            animationTimer.Tick += (s, e) => { angle = (angle + 6) % 360; this.Invalidate(); };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (!isSpinning) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            int cx = this.Width / 2;
            int cy = this.Height / 2;
            int r = Math.Min(this.Width, this.Height) / 2 - 15;

            // Background circle
            using (Pen pen = new Pen(Color.FromArgb(200, 200, 200), 3))
            {
                g.DrawEllipse(pen, cx - r, cy - r, r * 2, r * 2);
            }

            // Spinning arc
            using (Pen pen = new Pen(Color.FromArgb(52, 73, 94), 5))
            {
                pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                g.DrawArc(pen, cx - r, cy - r, r * 2, r * 2, angle, 150);
            }

            // Center dot
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(52, 73, 94)))
            {
                g.FillEllipse(brush, cx - 5, cy - 5, 10, 10);
            }

            // Loading text
            using (Font font = new Font("Segoe UI", 11F, FontStyle.Bold))
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(44, 62, 80)))
            {
                string text = "Loading...";
                SizeF textSize = g.MeasureString(text, font);
                float textX = (this.Width - textSize.Width) / 2;
                float textY = cy + r + 15;
                g.DrawString(text, font, brush, textX, textY);
            }
        }

        public void StartLoading()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(StartLoading));
                return;
            }

            isSpinning = true;
            angle = 0;
            this.Visible = true;
            this.BringToFront();
            animationTimer.Start();
            this.Refresh();
        }

        public void StopLoading()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(StopLoading));
                return;
            }

            isSpinning = false;
            animationTimer.Stop();
            this.Visible = false;
            this.Refresh();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                animationTimer?.Stop();
                animationTimer?.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}