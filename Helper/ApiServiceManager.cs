using LogManager;
using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;

namespace Helper
{
    /// <summary>
    /// Starts / stops the IPIS API Windows service. Never throws — every failure is logged and returned as false.
    /// </summary>
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    public static class ApiServiceManager
    {
        private const string ServiceName = "IPIS_SERVICE";
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);

        /// <summary>
        /// True only when THIS application started the service, so logout stops only a service the app started.
        /// </summary>
        public static bool StartedByApplication { get; private set; }

        public static bool StartApiService(TimeSpan? timeout = null)
        {
            try
            {
                using var service = OpenService();
                if (service == null)
                {
                    LoggerService.Logger.Warning($"{ServiceName} is not installed — continuing without it.");
                    return false;
                }

                if (service.Status == ServiceControllerStatus.Running)
                    return true;

                // A stop in progress must finish before the service can be started again
                if (service.Status == ServiceControllerStatus.StopPending)
                    WaitForStatus(service, ServiceControllerStatus.Stopped, timeout ?? DefaultTimeout);

                bool startedHere = false;
                if (service.Status == ServiceControllerStatus.Stopped)
                {
                    service.Start();
                    startedHere = true;
                }
                else if (service.Status == ServiceControllerStatus.Paused)
                {
                    service.Continue();
                }

                bool running = WaitForStatus(service, ServiceControllerStatus.Running, timeout ?? DefaultTimeout);

                if (running && startedHere)
                    StartedByApplication = true;

                if (!running)
                    LoggerService.Logger.Warning($"{ServiceName} did not start (status: {service.Status}). Check the Windows event log for the service error.");

                return running;
            }
            catch (Exception ex)
            {
                // e.g. access denied (starting a service needs administrator rights)
                LoggerService.Logger.Error(ex, $"Could not start {ServiceName}.");
                return false;
            }
        }

        /// <summary>
        /// Stops the service. With onlyIfStartedByApplication = true it stops it only if this app started it.
        /// </summary>
        public static bool StopApiService(TimeSpan? timeout = null, bool onlyIfStartedByApplication = false)
        {
            try
            {
                if (onlyIfStartedByApplication && !StartedByApplication)
                    return true; // not started by this app — leave it running

                using var service = OpenService();
                if (service == null || service.Status == ServiceControllerStatus.Stopped)
                {
                    StartedByApplication = false;
                    return true;
                }

                if (service.CanStop)
                    service.Stop();

                bool stopped = WaitForStatus(service, ServiceControllerStatus.Stopped, timeout ?? DefaultTimeout);

                if (stopped)
                    StartedByApplication = false;
                else
                    LoggerService.Logger.Warning($"{ServiceName} did not stop (status: {service.Status}).");

                return stopped;
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, $"Could not stop {ServiceName}.");
                return false;
            }
        }

        public static bool IsApiServiceRunning()
        {
            try
            {
                using var service = OpenService();
                return service != null && service.Status == ServiceControllerStatus.Running;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the service, or null when it is not installed.
        /// </summary>
        private static ServiceController OpenService()
        {
            var service = new ServiceController(ServiceName);
            try
            {
                _ = service.Status; // throws InvalidOperationException when the service does not exist
                return service;
            }
            catch (InvalidOperationException)
            {
                service.Dispose();
                return null;
            }
        }

        /// <summary>
        /// Waits until the service reaches <paramref name="target"/> or the timeout expires.
        /// While waiting for Running it gives up immediately when the service falls back to Stopped
        /// (the service crashed on start) instead of waiting for the full timeout.
        /// </summary>
        private static bool WaitForStatus(ServiceController service, ServiceControllerStatus target, TimeSpan timeout)
        {
            var clock = Stopwatch.StartNew();

            while (clock.Elapsed < timeout)
            {
                service.Refresh();

                if (service.Status == target)
                    return true;

                if (target == ServiceControllerStatus.Running && service.Status == ServiceControllerStatus.Stopped)
                    return false;

                Thread.Sleep(250);
            }

            service.Refresh();
            return service.Status == target;
        }
    }
}
