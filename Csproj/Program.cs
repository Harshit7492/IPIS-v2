using Helper;
using Ipis.Snmp;
using LogManager;
using System.Diagnostics;

namespace IPIS
{
    internal static class Program
    {
        private const string MutexName = @"Global\IPISv2_SingleInstance";
        private const string RestartArgument = "/restart";


        [STAThread]
        static void Main(string[] args)
        {
            LoggerService.InitializeLogger();

            bool isRestart = args.Contains(RestartArgument, StringComparer.OrdinalIgnoreCase);

            using Mutex mutex = new Mutex(false, MutexName);

            try
            {
                if (!mutex.WaitOne(isRestart ? 30000 : 0))
                {
                    MessageBox.Show( "IPISv2 is already running on this machine.", "IPISv2", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    return;
                }
            }
            catch (AbandonedMutexException)
            {
                // Previous instance crashed.
                // We can safely take ownership of the mutex.
            }

            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Config.SetConfig();

           
                LicenseResult license = LicenseManager.Validate();
                if (!license.IsValid)
                {
                    MessageBox.Show("IPISv2 cannot start because the license is missing or invalid. Please contact Vardaan Electronics support for assistance.", "VARDAAN - License Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                }

                using (LoginForm loginForm = new LoginForm())
                {
                    if (loginForm.ShowDialog() != DialogResult.OK)
                        return;
                }

                IpisTrapSender.Instance.StartPeriodicStatus();

                Task.Run(() => ApiServiceManager.StartApiService());
                Task.Run(() => IpisSnmpAgent.Instance.Start());
                Task.Run(() => ucInterfaceAdminSetting.StartMaintenanceScheduler());

                Application.Run(new Dashboard());
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Fatal(ex, "Application crashed on startup.");

                MessageBox.Show(
                    $"Startup Error: {ex.Message}\n\n{ex.StackTrace}",
                    "Critical Startup Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                try
                {
                    mutex.ReleaseMutex();
                   
                }
                catch
                {
                    // Mutex may not be owned.
                }
            }
        }

        public static void RestartApplication()
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = Application.ExecutablePath,
                    Arguments = RestartArgument,
                    UseShellExecute = true
                });

                Application.Exit();
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, "Unable to restart the application.");
            }
        }
    }
}