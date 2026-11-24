

namespace IPISv2.Controls
{
    public class SplashForm : Form
    {
        private AppSplashOverlay _overlay;

        public SplashForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.FromArgb(15, 23, 42); // Dark slate blue matching loader theme
            this.DoubleBuffered = true;
            this.ShowInTaskbar = false;

            _overlay = new AppSplashOverlay();
            this.Controls.Add(_overlay);
            _overlay.Dock = DockStyle.Fill;
            _overlay.Start();
        }

        public void SetStatus(string status)
        {
            if (_overlay != null && !_overlay.IsDisposed)
            {
                _overlay.SetStatus(status);
                Application.DoEvents();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _overlay?.Stop();
            base.OnFormClosing(e);
        }
    }

    /// <summary>
    /// Shows SplashForm on its own UI thread, so its dots keep animating while the
    /// main window is busy (e.g. building the Online Trains grid on the main thread).
    /// Show() returns once the splash is on screen; Close() can be called from any thread.
    /// </summary>
    public static class SplashScreenHost
    {
        private static readonly object _sync = new object();
        private static SplashForm _form;
        private static Thread _thread;
        private static bool _closeRequested;

        public static void Show()
        {
            lock (_sync)
            {
                if (_thread != null) return;
                _closeRequested = false;
            }

            using (var shown = new ManualResetEventSlim(false))
            {
                var thread = new Thread(() =>
                {
                    var form = new SplashForm { TopMost = true };

                    form.Shown += (s, e) =>
                    {
                        bool closeNow;
                        lock (_sync)
                        {
                            _form = form;
                            closeNow = _closeRequested;
                        }

                        try { shown.Set(); } catch (ObjectDisposedException) { }

                        // Close() was called before the splash appeared.
                        if (closeNow) form.Close();
                    };

                    form.FormClosed += (s, e) =>
                    {
                        lock (_sync)
                        {
                            _form = null;
                            _thread = null;
                        }
                    };

                    Application.Run(form);
                });

                thread.SetApartmentState(ApartmentState.STA);
                thread.IsBackground = true;
                thread.Name = "SplashScreen";

                lock (_sync) { _thread = thread; }
                thread.Start();

                // Wait until the splash is on screen so it covers the main window from its first frame.
                shown.Wait(3000);
            }
        }

        public static void Close()
        {
            SplashForm form;
            lock (_sync)
            {
                _closeRequested = true;
                form = _form;
            }

            if (form == null || form.IsDisposed || !form.IsHandleCreated) return;

            try
            {
                form.BeginInvoke(new Action(() =>
                {
                    if (!form.IsDisposed) form.Close();
                }));
            }
            catch (InvalidOperationException)
            {
                // Splash window already gone.
            }
        }
    }
}
