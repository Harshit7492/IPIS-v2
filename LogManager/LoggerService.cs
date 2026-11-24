//using Serilog;

//namespace LogManager
//{
//    public static class LoggerService
//    {
//        private static ILogger _logger;

//        public static ILogger Logger
//        {
//            get
//            {
//                if (_logger == null)
//                {
//                    // Return Serilog's global logger (which is never null) to prevent NullReferenceExceptions
//                    return Log.Logger;
//                }
//                return _logger;
//            }
//            set
//            {
//                _logger = value;
//            }
//        }

//        public static void InitializeLogger()
//        {
//            try
//            {
//                var logConfig = new LoggerConfiguration()
//                    .MinimumLevel.Debug();

//                bool sinkAdded = false;

//                // 1. App-local logs directory: [BaseDirectory]\logs\logs-.log
//                try
//                {
//                    string appLogsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
//                    if (!Directory.Exists(appLogsDir))
//                        Directory.CreateDirectory(appLogsDir);

//                    logConfig = logConfig.WriteTo.File(
//                        Path.Combine(appLogsDir, "logs-.log"),
//                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
//                        rollingInterval: RollingInterval.Day);
//                    sinkAdded = true;
//                }
//                catch { }

//                // 2. Drive root Applogs directory (e.g. C:\Applogs\logsIPIS or E:\Applogs\logsIPIS)
//                try
//                {
//                    string drive = Path.GetPathRoot(AppDomain.CurrentDomain.BaseDirectory);
//                    if (!string.IsNullOrEmpty(drive))
//                    {
//                        string driveLogsDir = Path.Combine(drive, "Applogs", "logsIPIS");
//                        if (!Directory.Exists(driveLogsDir))
//                            Directory.CreateDirectory(driveLogsDir);

//                        logConfig = logConfig.WriteTo.File(
//                            Path.Combine(driveLogsDir, "logs-.log"),
//                            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
//                            rollingInterval: RollingInterval.Day);
//                        sinkAdded = true;
//                    }
//                }
//                catch { }

//                // 3. Fallback to ProgramData if none of the above succeeded
//                if (!sinkAdded)
//                {
//                    try
//                    {
//                        string programDataLogs = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "IPISv2", "logs");
//                        if (!Directory.Exists(programDataLogs))
//                            Directory.CreateDirectory(programDataLogs);

//                        logConfig = logConfig.WriteTo.File(
//                            Path.Combine(programDataLogs, "logs-.log"),
//                            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
//                            rollingInterval: RollingInterval.Day);
//                    }
//                    catch { }
//                }

//                _logger = logConfig.CreateLogger();
//            }
//            catch (Exception ex)
//            {
//                try
//                {
//                    _logger = new LoggerConfiguration().CreateLogger();
//                }
//                catch { }
//            }
//        }
//    }
//}
using System.Text;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;

namespace LogManager
{
    public static class LoggerService
    {
        private static readonly object _lock = new object();
        private static ILogger _logger;
        private static ILogger _generalLogger;
        private static ILogger _ntesLogger;
        private static ILogger _capLogger;
        private static ILogger _settingsLogger;
        private static ILogger _userLogger;
        private static ILogger _hsrLogger;
        private static ILogger _vdbLogger;
        private static ILogger _messageLogger;
        private static ILogger _licenseLogger;

        private const string OutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}";
        private const int RetainedDays = 30;

        public static ILogger Logger => _logger ?? Log.Logger;
        public static ILogger GeneralLogger => _generalLogger ?? Logger;
        public static ILogger NtesLogger => _ntesLogger ?? Logger;
        public static ILogger CapLogger => _capLogger ?? Logger;
        public static ILogger SettingsLogger => _settingsLogger ?? Logger;
        public static ILogger UserLoginLogger => _userLogger ?? Logger;
        public static ILogger HsrLogger => _hsrLogger ?? Logger;
        public static ILogger VdbLogger => _vdbLogger ?? Logger;
        public static ILogger MessageLogger => _messageLogger ?? Logger;
        public static ILogger LicenseLogger => _licenseLogger ?? Logger;

        public static void InitializeLogger()
        {
            lock (_lock)
            {
                if (_logger != null)
                    return;

                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string drive = Path.GetPathRoot(baseDirectory);

                string localLogs = Path.Combine(baseDirectory, "logs");
                string centralLogs = !string.IsNullOrWhiteSpace(drive) ? Path.Combine(drive, "Applogs", "logsIPIS") : null;
                string programDataLogs = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "IPISv2", "logs");

                string[] roots = { localLogs, centralLogs, programDataLogs };

                _generalLogger = CreateGeneralLogger(roots);
                _ntesLogger = CreateFileLogger("NTES", roots);
                _capLogger = CreateFileLogger("CAP", roots);
                _settingsLogger = CreateFileLogger("SETTINGS", roots);
                _userLogger = CreateFileLogger("USER LOGIN", roots);
                _hsrLogger = CreateFileLogger("HSR", roots);
                _vdbLogger = CreateFileLogger("VDB", roots);
                _messageLogger = CreateFileLogger("MESSAGE", roots);
                _licenseLogger = CreateFileLogger("LICENSE", roots);

                _logger = _generalLogger;   // General is the default logger
                Log.Logger = _logger;
            }
        }

        private static ILogger CreateGeneralLogger(params string[] roots)
        {
            return CreateFileLogger("GENERAL", roots);
        }

        /// <summary>
        /// One method for all loggers: tries each root in order, creates
        /// root\category\ and writes to one file per day (yyyy-MM-dd.log).
        /// </summary>
        private static ILogger CreateFileLogger(string category, params string[] roots)
        {
            foreach (string root in roots)
            {
                if (string.IsNullOrWhiteSpace(root))
                    continue;

                try
                {
                    string directory = Path.Combine(root, category);

                    Directory.CreateDirectory(directory);

                    string probe = Path.Combine(directory, $".write-test-{Guid.NewGuid():N}.tmp");
                    using (File.Create(probe)) { }
                    File.Delete(probe);

                    return new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .WriteTo.Sink(new DailyFileSink(directory, OutputTemplate, RetainedDays))
                        .CreateLogger();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Cannot create {category} logger in {root}: {ex.Message}");
                }
            }

            // Last resort: logger without a file sink
            return new LoggerConfiguration().MinimumLevel.Debug().CreateLogger();
        }

        /// <summary>
        /// Writes each event to <directory>\yyyy-MM-dd.log (one file per day).
        /// </summary>
        private sealed class DailyFileSink : ILogEventSink
        {
            private readonly string _directory;
            private readonly ITextFormatter _formatter;
            private readonly int _retainedDays;
            private readonly object _sync = new object();
            private string _currentDate;

            public DailyFileSink(string directory, string outputTemplate, int retainedDays)
            {
                _directory = directory;
                _formatter = new MessageTemplateTextFormatter(outputTemplate);
                _retainedDays = retainedDays;
            }

            public void Emit(LogEvent logEvent)
            {
                try
                {
                    lock (_sync)
                    {
                        string date = logEvent.Timestamp.ToString("yyyy-MM-dd");

                        if (date != _currentDate)
                        {
                            _currentDate = date;
                            DeleteOldLogs();   // runs once per day
                        }

                        string path = Path.Combine(_directory, date + ".log");

                        using (var stream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                        using (var writer = new StreamWriter(stream, new UTF8Encoding(false)))
                        {
                            _formatter.Format(logEvent, writer);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Logging must never crash the app
                    System.Diagnostics.Debug.WriteLine($"Log write failed: {ex.Message}");
                }
            }

            private void DeleteOldLogs()
            {
                try
                {
                    DateTime cutoff = DateTime.Now.AddDays(-_retainedDays);

                    foreach (string file in Directory.GetFiles(_directory, "*.log"))
                    {
                        if (File.GetLastWriteTime(file) < cutoff)
                            File.Delete(file);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Log cleanup failed in {_directory}: {ex.Message}");
                }
            }
        }
    }
}
