using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;

namespace IPIS
{
    public partial class ucUpdateExe : UserControl
    {
        // Change this to your actual server share / URL
        private const string ServerSourcePath = @"\\YOUR-SERVER\IPIS\Updates\IPIS.exe";

        // The defined install path where files must be replaced.
        private const string DefinedInstallDir =
            @"C:\Program Files\Vardaan Electronics Industries Pvt Ltd\IPISv2";

        private static string InstallDir
        {
            get
            {
                return Directory.Exists(DefinedInstallDir)
                    ? DefinedInstallDir
                    : Application.StartupPath;
            }
        }

        public ucUpdateExe()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Load += ucUpdateExe_Load;
        }

        private void ucUpdateExe_Load(object sender, EventArgs e)
        {
            SetStatus("Ready.", false);
        }

        // ------------------------------------------------------------------
        //  Update from Pendrive
        // ------------------------------------------------------------------
        private void btnPendrive_Click(object sender, EventArgs e)
        {
            // 1. Find all connected, ready pendrives (any USB port)
            var pendrives = new List<DriveInfo>();
            try
            {
                foreach (var drive in DriveInfo.GetDrives())
                {
                    if (drive.DriveType == DriveType.Removable && drive.IsReady)
                        pendrives.Add(drive);
                }
            }
            catch { /* ignore drive enumeration errors */ }

            // 2. No pendrive in any port -> tell the user
            if (pendrives.Count == 0)
            {
                MessageBox.Show(
                    "Please insert the pendrive.",
                    "Update from Pendrive",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 3. Open the pendrive and let the user pick one or more DLL / EXE files
            string[] files;
            using (var ofd = new OpenFileDialog())
            {
                ofd.Title = "Select file(s) to update from Pendrive";
                ofd.InitialDirectory = pendrives[0].RootDirectory.FullName;
                ofd.Multiselect = true;
                ofd.Filter = "DLL / EXE files (*.dll;*.exe)|*.dll;*.exe|All files (*.*)|*.*";
                if (ofd.ShowDialog(this) != DialogResult.OK) return;
                files = ofd.FileNames;
            }

            ApplyUpdate(files, "Pendrive");
        }

        // ------------------------------------------------------------------
        //  Update from Server
        // ------------------------------------------------------------------
        private void btnServer_Click(object sender, EventArgs e)
        {
            if (!File.Exists(ServerSourcePath))
            {
                MessageBox.Show(
                    "Update package not found at:\n" + ServerSourcePath +
                    "\n\nPlease check the network path or contact your administrator.",
                    "Update from Server",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ApplyUpdate(new[] { ServerSourcePath }, "Server");
        }

        // ------------------------------------------------------------------
        //  Shared update routine (replaces old DLL/EXE files in InstallDir)
        // ------------------------------------------------------------------
        private void ApplyUpdate(string[] sourceFiles, string sourceLabel)
        {
            try
            {
                if (sourceFiles == null || sourceFiles.Length == 0 || sourceFiles.Any(f => !File.Exists(f)))
                {
                    MessageBox.Show("Source file not found.", "Update",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string exePath = Application.ExecutablePath;

                // Build source -> target list (same file name, placed in InstallDir)
                var items = sourceFiles
                    .Select(s => new { Source = s, Target = Path.Combine(InstallDir, Path.GetFileName(s)) })
                    .ToList();

                // Prevent selecting files that are already in the install folder
                if (items.Any(i => string.Equals(Path.GetFullPath(i.Source), Path.GetFullPath(i.Target),
                                                 StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("A selected file is already in the install folder.",
                        "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var list = new StringBuilder();
                foreach (var i in items)
                    list.AppendLine("  " + Path.GetFileName(i.Source) +
                                    (File.Exists(i.Target) ? "  (replace)" : "  (new)"));

                var confirm = MessageBox.Show(
                    $"Update IPIS from {sourceLabel}?\n\n" +
                    $"Files:\n{list}\n" +
                    $"Install folder:\n{InstallDir}\n\n" +
                    "The application will close, replace the files, and restart.",
                    "Confirm Update",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes) return;

                SetStatus($"Updating from {sourceLabel}...", true);

                // ---- Build helper batch: wait for exit -> backup -> replace -> restart ----
                int pid = Process.GetCurrentProcess().Id;
                string bat = Path.Combine(Path.GetTempPath(), "ipis_update.bat");
                string backupDir = Path.Combine(InstallDir, "Backup", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

                var b = new StringBuilder();
                string log = Path.Combine(Path.GetTempPath(), "ipis_update.log");

                b.AppendLine("@echo off");
                b.AppendLine("setlocal enabledelayedexpansion");
                b.AppendLine($"echo [%date% %time%] Update started. Target folder: {InstallDir} > \"{log}\"");
                b.AppendLine("ping 127.0.0.1 -n 3 >nul");

                // wait until this app has fully exited (files are unlocked)
                b.AppendLine(":wait");
                b.AppendLine($"tasklist /FI \"PID eq {pid}\" 2>nul | find \"{pid}\" >nul");
                b.AppendLine("if not errorlevel 1 (ping 127.0.0.1 -n 2 >nul & goto wait)");

                // backup old files
                b.AppendLine($"mkdir \"{backupDir}\" >nul 2>&1");
                foreach (var i in items)
                    b.AppendLine($"if exist \"{i.Target}\" copy /Y \"{i.Target}\" \"{backupDir}\\\" >nul 2>&1");

                // replace files (with retry)
                foreach (var i in items)
                    b.AppendLine($"call :docopy \"{i.Source}\" \"{i.Target}\"");

                // restart app and delete this batch file
                b.AppendLine($"start \"\" \"{exePath}\"");
                b.AppendLine("(goto) 2>nul & del \"%~f0\"");

                // subroutine: copy with up to 10 retries, result written to log
                b.AppendLine(":docopy");
                b.AppendLine("set /a n=0");
                b.AppendLine(":retry");
                b.AppendLine("copy /Y %1 %2 >nul 2>&1");
                b.AppendLine($"if not errorlevel 1 (echo OK     %~2 >> \"{log}\" & exit /b)");
                b.AppendLine("set /a n+=1");
                b.AppendLine("if !n! lss 10 (ping 127.0.0.1 -n 2 >nul & goto retry)");
                b.AppendLine($"echo FAILED %~2 >> \"{log}\"");
                b.AppendLine("exit /b");

                File.WriteAllText(bat, b.ToString(), Encoding.Default);

                // ---- Launch helper (elevated if not already admin: Program Files needs it) ----
                var psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/C \"{bat}\"",
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = true
                };

                if (!IsRunningAsAdmin())
                    psi.Verb = "runas"; // shows UAC prompt

                try
                {
                    Process.Start(psi);
                }
                catch (Win32Exception)
                {
                    // user clicked "No" on the UAC prompt
                    SetStatus("Update cancelled.", false);
                    MessageBox.Show("Update cancelled. Administrator permission is required to replace files.",
                        "Update", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Application.Exit();
            }
            catch (Exception ex)
            {
                SetStatus("Update failed.", false);
                MessageBox.Show("Update failed:\n" + ex.Message,
                    "Update", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ------------------------------------------------------------------
        //  Helpers
        // ------------------------------------------------------------------
        private static bool IsRunningAsAdmin()
        {
            try
            {
                using (var id = WindowsIdentity.GetCurrent())
                {
                    return new WindowsPrincipal(id).IsInRole(WindowsBuiltInRole.Administrator);
                }
            }
            catch
            {
                return false;
            }
        }

        private void SetStatus(string text, bool busy)
        {
            if (lblStatus.InvokeRequired)
            {
                lblStatus.BeginInvoke(new Action(() => SetStatus(text, busy)));
                return;
            }

            lblStatus.Text = text;
            progressBar.Visible = busy;
            progressBar.Style = busy ? ProgressBarStyle.Marquee : ProgressBarStyle.Blocks;
            btnPendrive.Enabled = !busy;
            btnServer.Enabled = !busy;
        }

        // Optional: called from ucSetUp.RefreshData() if you wired it in
        public void RefreshData()
        {
            SetStatus("Ready.", false);
        }
    }
}