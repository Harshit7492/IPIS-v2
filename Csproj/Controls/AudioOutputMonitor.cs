using NAudio.CoreAudioApi;

namespace Ipis.Snmp
{
    public sealed class AudioOutputMonitor : IDisposable
    {
        // Shared instance so AnnouncementManager and the background loop use the same state.
        public static AudioOutputMonitor Instance { get; } =
            new AudioOutputMonitor(IpisTrapSender.Instance);

        private readonly IpisTrapSender _trap;
        private readonly TimeSpan _pollInterval;
        private readonly TimeSpan _debounce;

        private bool? _lastKnownOk;
        private DateTime? _lastOkUtc;
        private DateTime _lastEventUtc = DateTime.MinValue;
        private readonly object _gate = new();

        private CancellationTokenSource? _cts;
        private Task? _task;
        private bool _disposed;

        public bool? IsAudioOk { get { lock (_gate) return _lastKnownOk; } }

        public AudioOutputMonitor(
            IpisTrapSender trap,
            TimeSpan? pollInterval = null,
            TimeSpan? debounce = null)
        {
            _trap = trap ?? throw new ArgumentNullException(nameof(trap));
            _pollInterval = pollInterval ?? TimeSpan.FromSeconds(3);
            _debounce = debounce ?? TimeSpan.FromSeconds(2);
        }

        public void Start()
        {
            lock (_gate)
            {
                ObjectDisposedException.ThrowIf(_disposed, this);
                if (_task is { IsCompleted: false }) return;

                _cts?.Dispose();
                _cts = new CancellationTokenSource();
                var token = _cts.Token;
                _task = Task.Run(() => LoopAsync(token));
            }
        }

        public async Task StopAsync()
        {
            CancellationTokenSource? cts;
            Task? task;

            lock (_gate)
            {
                cts = _cts; task = _task;
                _cts = null; _task = null;
            }

            if (cts == null) return;

            try
            {
                cts.Cancel();
                if (task != null) await task.ConfigureAwait(false);
            }
            catch (OperationCanceledException) { }
            finally { cts.Dispose(); }
        }

        public void Dispose()
        {
            CancellationTokenSource? cts;
            lock (_gate)
            {
                if (_disposed) return;
                _disposed = true;
                cts = _cts; _cts = null;
            }
            try { cts?.Cancel(); } catch (ObjectDisposedException) { }
        }

        /// <summary>
        /// Probes the audio device right now and ALWAYS sends a trap:
        /// OK trap if audio is OK, NOT OK trap otherwise.
        /// Returns the audio state.
        /// </summary>
        public async Task<bool> CheckAndReportAsync()
        {
            bool ok = ProbeAudioOutput(out string detail);

            lock (_gate)
            {
                _lastKnownOk = ok;
                _lastEventUtc = DateTime.UtcNow;
                if (ok) _lastOkUtc = DateTime.UtcNow;
            }

            await SendStateTrapAsync(ok, detail).ConfigureAwait(false);
            return ok;
        }

        private async Task LoopAsync(CancellationToken ct)
        {
            Console.WriteLine("[Audio] Monitor started.");

            while (!ct.IsCancellationRequested)
            {
                try
                {
                    bool ok = ProbeAudioOutput(out string detail);
                    await HandleTransitionAsync(ok, detail).ConfigureAwait(false);
                }
                catch (OperationCanceledException) when (ct.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"[Audio] Probe error: {ex.GetType().Name}: {ex.Message}");
                }

                try { await Task.Delay(_pollInterval, ct).ConfigureAwait(false); }
                catch (OperationCanceledException) { break; }
            }
        }

        /// <summary>
        /// First probe reports the initial state (OK or NOT OK).
        /// After that a trap is sent only when the state changes.
        /// </summary>
        private async Task HandleTransitionAsync(bool ok, string detail)
        {
            lock (_gate)
            {
                if (_lastKnownOk != null)
                {
                    if (_lastKnownOk.Value == ok) return;                       // no change
                    if (DateTime.UtcNow - _lastEventUtc < _debounce) return;    // too fast
                }

                _lastKnownOk = ok;
                _lastEventUtc = DateTime.UtcNow;
                if (ok) _lastOkUtc = DateTime.UtcNow;
            }

            await SendStateTrapAsync(ok, detail).ConfigureAwait(false);
        }

        private async Task SendStateTrapAsync(bool ok, string detail)
        {
            try
            {
                if (ok)
                {
                    await _trap.SendAudioOutputRestoredAsync(
                        description: $"Audio output OK: {detail}",
                        component: "Audio Output").ConfigureAwait(false);
                }
                else
                {
                    DateTime? lastGood;
                    lock (_gate) lastGood = _lastOkUtc;

                    await _trap.SendAudioOutputFailureAsync(
                        description: $"Audio output NOT OK: {detail}",
                        component: "Audio Output",
                        lastGoodUtc: lastGood).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[Audio] Trap send error: {ex.GetType().Name}: {ex.Message}");
            }
        }

        private static bool ProbeAudioOutput(out string detail)
        {
            try
            {
                using var enumerator = new MMDeviceEnumerator();

                if (!enumerator.HasDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia))
                {
                    detail = "no default render device";
                    return false;
                }

                using var device = enumerator.GetDefaultAudioEndpoint(
                    DataFlow.Render, Role.Multimedia);

                if (device.State != DeviceState.Active)
                {
                    detail = $"{device.FriendlyName} state = {device.State}";
                    return false;
                }

                detail = device.FriendlyName;
                return true;
            }
            catch (Exception ex)
            {
                detail = $"no usable default audio output: {ex.Message}";
                return false;
            }
        }
    }
}