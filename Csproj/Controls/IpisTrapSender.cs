//using Global_Data;
//using Helper;
//using Lextm.SharpSnmpLib;
//using Lextm.SharpSnmpLib.Messaging;
//using Lextm.SharpSnmpLib.Security;
//using NAudio.CoreAudioApi;
//using System.Net;
//using System.Net.Sockets;
//using System.Security.Cryptography; 
//using System.Text;
//using System.Text.Json;
//using Variable = Lextm.SharpSnmpLib.Variable;
//namespace Ipis.Snmp;

//public enum DeviceType
//{
//    CDC = 1,
//    CDS = 2,
//    PDC = 3,
//}

//public enum EventType
//{
//    Startup = 1,
//    Shutdown = 2,
//    Failure = 3,
//    Status = 4,
//    Info = 5,
//    Predictive = 6,
//}

//public enum EventCode
//{
//    GenericOk = 100,
//    LedModuleFailure = 101,
//    LedShortDetected = 102,
//    LedOpenDetected = 103,
//    LedPanelError = 104,

//    PsuBatteryLow = 111,

//    NetworkLinkDown = 120,
//    NetworkLinkUp = 121,

//    AudioHwFail = 130,
//    AudioOutputRestored = 131,
//    NtesSyncFail = 160,
//    NtesSyncOk = 161,
//    PdcCommTimeout = 170,

//    PredictiveLedDegrade = 200
//}

//public enum Severity
//{
//    Critical = 1,
//    Major = 2,
//    Minor = 3,
//    Warning = 4,
//    Info = 5
//}

//public enum Health
//{
//    Ok = 1,
//    Warning = 2,
//    Minor = 3,
//    Major = 4,
//    Critical = 5
//}

//public sealed class IpisTrapOptions
//{
//    public string NmsIp { get; set; } = Config.TrapNmsIp;
//    public int Port { get; set; } = Config.TrapPort;

//    public string LocalIp => GlobalData._lstAdminSetting?.Ismaster == 1
//            ? "10.0.0.253"
//            : "10.0.0.254";
//    public string Username { get; set; } = Config.TrapUsername;

//    // Configured securely via Config; must match the NMS user.
//    public string AuthPassword { get; set; } = Config.TrapAuthPassword;
//    public string PrivPassword { get; set; } = Config.TrapPrivPassword;

//    public string BaseOid { get; set; } = Config.TrapBaseOid;

//    public string AssetId { get; set; } = Config.TrapAssetId;

//    public string AuthProtocol { get; set; } = Config.TrapAuthProtocol;

//    public string PrivProtocol { get; set; } = Config.TrapPrivProtocol;

//    public uint EnterpriseNumber { get; set; } = Config.TrapEnterpriseNumber;

//    public int TimeoutMs { get; set; } = Config.TrapTimeoutMs;

//    public int[] InformRetrySeconds { get; set; } =
//        new[] { 6, 12, 24 };

//    public int StatusIntervalSeconds { get; set; } = Config.TrapStatusIntervalSeconds;

//    public bool SendStartupOnStart { get; set; } = true;
//    public bool SendShutdownOnStop { get; set; } = true;

//    public bool AudioReportOnEveryTrap { get; set; } = false;
//}

//public sealed class IpisTrapSender : IAsyncDisposable
//{
//    private readonly IpisTrapOptions _o;
//    private readonly DeviceType _deviceType;
//    private readonly string _deviceTypeName;
//    private readonly string _deviceIp;
//    private readonly string _assetId;

//    private readonly DateTime _startedUtc = DateTime.UtcNow;

//    private readonly OctetString _engineId;
//    private readonly uint _engineBoots;

//    private readonly object _loopLock = new();

//    private CancellationTokenSource? _loopCts;
//    private Task? _loopTask;

//    private bool _startupSent;

//    // Audio output state as seen at the last trap send. null = not probed yet.
//    private readonly object _audioLock = new();
//    private bool? _lastAudioOk;
//    private DateTime? _lastAudioOkUtc;

//    private static int _msgId = Environment.TickCount & 0x3FFFFFFF;

//    private string EventOid(int n) => $"{_o.BaseOid}.3.{n}";
//    private string NotificationOid(int n) => $"{_o.BaseOid}.5.{n}";

//    private ObjectIdentifier Obj(int n) =>
//        new(EventOid(n));

//    public IpisTrapSender()
//        : this(
//            new IpisTrapOptions(),
//            Enum.TryParse<DeviceType>(Config.TrapDeviceType, true, out var dt) ? dt : DeviceType.CDC,
//            !string.IsNullOrWhiteSpace(Config.TrapLocalIp) ? Config.TrapLocalIp : Config.MasterIP,
//            Config.TrapAssetId,
//            Config.TrapFirmwareVersion)
//    {
//    }

//    public IpisTrapSender(
//        IpisTrapOptions options,
//        DeviceType deviceType,
//        string deviceIp,
//        string assetId,
//        string? firmwareVersion = null)
//    {
//        _o = options ?? throw new ArgumentNullException(nameof(options));

//        if (!IPAddress.TryParse(deviceIp, out var parsedIp) ||
//            parsedIp.AddressFamily != AddressFamily.InterNetwork)
//        {
//            throw new ArgumentException(
//                "A valid IPv4 device address is required.",
//                nameof(deviceIp));
//        }

//        if (!IPAddress.TryParse(_o.NmsIp, out var parsedNms) ||
//            parsedNms.AddressFamily != AddressFamily.InterNetwork)
//        {
//            throw new ArgumentException("NmsIp must be a valid IPv4 address.");
//        }

//        if (_o.Port is < 1 or > 65535)
//            throw new ArgumentOutOfRangeException(nameof(options.Port));

//        if (string.IsNullOrWhiteSpace(_o.BaseOid))
//            throw new ArgumentException("BaseOid is required.");

//        // Validate that BaseOid is a valid numeric OID.
//        _ = new ObjectIdentifier(_o.BaseOid);

//        if (string.IsNullOrWhiteSpace(_o.Username))
//            throw new ArgumentException("SNMPv3 username is required.");

//        if (string.IsNullOrEmpty(_o.AuthPassword) ||
//            string.IsNullOrEmpty(_o.PrivPassword))
//        {
//            throw new ArgumentException(
//                "SNMPv3 authentication and privacy passwords are required.");
//        }

//        _deviceType = deviceType;
//        _deviceTypeName = deviceType.ToString();
//        _deviceIp = parsedIp.ToString();
//        _assetId = string.IsNullOrWhiteSpace(assetId)
//            ? throw new ArgumentException("Asset ID is required.")
//            : assetId;

//        _engineId = BuildEngineId(_o.EnterpriseNumber, _assetId);
//        _engineBoots = LoadAndIncrementEngineBoots(_assetId);


//    }

//    // ---------------------------------------------------------------
//    // Lifecycle: start / stop
//    // ---------------------------------------------------------------

//    /// <summary>
//    /// Call once when the application starts.
//    /// Sends the startup trap exactly once. Optionally starts the
//    /// periodic heartbeat if StatusIntervalSeconds > 0.
//    /// </summary>
//    public async Task StartAsync(Func<object?>? extraProvider = null)
//    {
//        // Startup trap — sent exactly once.
//        if (_o.SendStartupOnStart && !_startupSent)
//        {
//            _startupSent = true;

//            try
//            {
//                bool startupSent = await SendStartupAsync();
//                Console.WriteLine($"[SNMP] Startup result: {startupSent}");
//            }
//            catch (Exception ex)
//            {
//                Console.Error.WriteLine(
//                    $"[SNMP] Startup error: {ex.GetType().Name}: {ex.Message}");
//            }
//        }

//        // Periodic heartbeat — only if explicitly enabled.
//        if (_o.StatusIntervalSeconds <= 0)
//        {
//            Console.WriteLine("[SNMP] Periodic status disabled.");
//            return;
//        }

//        lock (_loopLock)
//        {
//            if (_loopTask is { IsCompleted: false })
//                return;

//            _loopCts?.Dispose();
//            _loopCts = new CancellationTokenSource();

//            var ct = _loopCts.Token;
//            _loopTask = Task.Run(() => PeriodicLoopAsync(extraProvider, ct));
//        }

//        Console.WriteLine(
//            $"[SNMP] Periodic loop started: {_o.StatusIntervalSeconds} seconds.");
//    }

//    /// <summary>
//    /// Backwards-compatible alias. Prefer <see cref="StartAsync"/>.
//    /// </summary>
//    public Task StartPeriodicStatus(Func<object?>? extraProvider = null)
//        => StartAsync(extraProvider);

//    private async Task PeriodicLoopAsync(
//        Func<object?>? extraProvider,
//        CancellationToken ct)
//    {
//        try
//        {
//            using var timer = new PeriodicTimer(
//                TimeSpan.FromSeconds(
//                    Math.Max(1, _o.StatusIntervalSeconds)));

//            while (await timer.WaitForNextTickAsync(ct))
//            {
//                try
//                {
//                    object? extra = extraProvider?.Invoke() ?? new
//                    {
//                        uptimeSec = (long)(
//                            DateTime.UtcNow - _startedUtc).TotalSeconds,
//                        ts = Ts(DateTime.UtcNow)
//                    };

//                    bool sent = await SendPeriodicStatusAsync(extra);

//                    if (!sent)
//                    {
//                        Console.Error.WriteLine(
//                            $"[SNMP] Periodic status failed. " +
//                            $"NMS={_o.NmsIp}:{_o.Port}, Asset={_assetId}");
//                    }
//                }
//                catch (OperationCanceledException)
//                    when (ct.IsCancellationRequested)
//                {
//                    break;
//                }
//                catch (Exception ex)
//                {
//                    Console.Error.WriteLine(
//                        $"[SNMP] Periodic error: " +
//                        $"{ex.GetType().Name}: {ex.Message}");
//                }
//            }
//        }
//        catch (OperationCanceledException)
//        {
//            // Normal shutdown.
//        }
//    }

//    public async Task StopPeriodicStatusAsync()
//    {
//        CancellationTokenSource? cts;
//        Task? task;

//        lock (_loopLock)
//        {
//            cts = _loopCts;
//            task = _loopTask;

//            _loopCts = null;
//            _loopTask = null;
//        }

//        if (cts != null)
//        {
//            try
//            {
//                cts.Cancel();

//                if (task != null)
//                    await task;
//            }
//            catch (OperationCanceledException)
//            {
//                // Normal cancellation.
//            }
//            finally
//            {
//                cts.Dispose();
//            }

//            Console.WriteLine("[SNMP] Periodic loop stopped.");
//        }

//        if (_o.SendShutdownOnStop)
//        {
//            try
//            {
//                await SendShutdownAsync();
//            }
//            catch (Exception ex)
//            {
//                Console.Error.WriteLine(
//                    $"[SNMP] Shutdown error: {ex.GetType().Name}: {ex.Message}");
//            }
//        }
//    }

//    public async ValueTask DisposeAsync()
//    {
//        await StopPeriodicStatusAsync();
//    }


//    private IAuthenticationProvider BuildAuthProvider()
//    {
//        string protocol = (_o.AuthProtocol ?? "SHA")
//            .Replace("-", "")
//            .Replace("(", "")
//            .Replace(")", "")
//            .ToUpperInvariant();

//        var password = new OctetString(_o.AuthPassword);

//        if (protocol.Contains("MD5"))
//            return new MD5AuthenticationProvider(password);

//        if (protocol.Contains("SHA512"))
//            return new SHA512AuthenticationProvider(password);

//        if (protocol.Contains("SHA384"))
//            return new SHA384AuthenticationProvider(password);

//        if (protocol.Contains("SHA256"))
//            return new SHA256AuthenticationProvider(password);

//        if (protocol is "SHA" or "SHA1" or "SHAHMACSHA96")
//            return new SHA1AuthenticationProvider(password);

//        throw new NotSupportedException(
//            $"Unsupported authentication protocol: {_o.AuthProtocol}");
//    }

//    private IPrivacyProvider BuildPrivProvider(
//        IAuthenticationProvider auth)
//    {
//        string protocol = (_o.PrivProtocol ?? "AES-128")
//            .Replace("-", "")
//            .ToUpperInvariant();

//        var password = new OctetString(_o.PrivPassword);

//        if (protocol.Contains("AES256"))
//            return new AES256PrivacyProvider(password, auth);

//        if (protocol.Contains("AES192"))
//            return new AES192PrivacyProvider(password, auth);

//        if (protocol is "AES" or "AES128")
//            return new AESPrivacyProvider(password, auth);

//        if (protocol == "DES")
//            return new DESPrivacyProvider(password, auth);

//        throw new NotSupportedException(
//            $"Unsupported privacy protocol: {_o.PrivProtocol}");
//    }

//    // ---------------------------------------------------------------
//    // Engine ID and persistent engine boots
//    // ---------------------------------------------------------------

//    private static OctetString BuildEngineId(
//        uint enterprise,
//        string seed)
//    {
//        if (string.IsNullOrWhiteSpace(seed))
//            throw new ArgumentException("Engine ID seed is required.");

//        byte[] enterpriseBytes =
//        {
//            (byte)(enterprise >> 24),
//            (byte)(enterprise >> 16),
//            (byte)(enterprise >> 8),
//            (byte)enterprise
//        };

//        // Enterprise-specific SNMP engine ID format.
//        enterpriseBytes[0] |= 0x80;

//        using var sha256 = SHA256.Create();

//        byte[] hash = sha256.ComputeHash(
//            Encoding.UTF8.GetBytes(seed));

//        // Format: 4-byte enterprise number + 1-byte format + 20-byte suffix.
//        var engineId = new byte[25];

//        Buffer.BlockCopy(enterpriseBytes, 0, engineId, 0, 4);
//        engineId[4] = 0x05;

//        Buffer.BlockCopy(hash, 0, engineId, 5, 20);

//        return new OctetString(engineId);
//    }

//    private static uint LoadAndIncrementEngineBoots(string assetId)
//    {
//        string safeAsset = string.Concat(
//            assetId.Select(c =>
//                char.IsLetterOrDigit(c) || c is '-' or '_'
//                    ? c
//                    : '_'));

//        string path = Path.Combine(
//            Environment.GetFolderPath(
//                Environment.SpecialFolder.CommonApplicationData),
//            "Ipis",
//            $"snmp-engineboots-{safeAsset}.txt");

//        Directory.CreateDirectory(Path.GetDirectoryName(path)!);

//        // Restrict concurrent updates within this process.
//        lock (EngineBootsLock)
//        {
//            uint boots = 1;

//            if (File.Exists(path) &&
//                uint.TryParse(File.ReadAllText(path).Trim(), out uint previous))
//            {
//                if (previous == uint.MaxValue)
//                {
//                    throw new InvalidOperationException(
//                        "SNMP engine boots counter has reached its maximum.");
//                }

//                boots = previous + 1;
//            }

//            string tempPath = path + ".tmp";

//            File.WriteAllText(tempPath, boots.ToString());
//            File.Move(tempPath, path, overwrite: true);

//            return boots;
//        }
//    }

//    private static readonly object EngineBootsLock = new();

//    private string SelectNotificationOid(EventType type, EventCode code)
//    {
//        // Specific notifications take precedence.
//        if (code == EventCode.NetworkLinkDown)
//            return NotificationOid(6); // ipisLinkDown

//        if (code == EventCode.NetworkLinkUp)
//            return NotificationOid(7); // ipisLinkUp

//        if (code == EventCode.NtesSyncFail)
//            return NotificationOid(8); // ipisNTESSyncFail

//        if (code == EventCode.NtesSyncOk)
//            return NotificationOid(9); // ipisNTESSyncOk

//        return type switch
//        {
//            EventType.Startup => NotificationOid(1),
//            EventType.Failure => NotificationOid(2),
//            EventType.Status => NotificationOid(3),
//            EventType.Predictive => NotificationOid(5),
//            EventType.Shutdown => NotificationOid(3),
//            EventType.Info => NotificationOid(3),

//            _ => NotificationOid(3)
//        };
//    }

//    private static bool ShouldUseInform(
//        Severity severity,
//        EventCode code)
//    {
//        return severity is Severity.Critical or Severity.Major
//            || code is EventCode.LedModuleFailure
//                or EventCode.NtesSyncFail
//                or EventCode.PdcCommTimeout;
//    }

//    // ---------------------------------------------------------------
//    // Event payload and send
//    // ---------------------------------------------------------------

//    public async Task<bool> SendEventAsync(
//        EventType type,
//        EventCode code,
//        Severity severity,
//        Health health,
//        string description,
//        object? extra = null,
//        string? suspectedComponent = null,
//        DateTime? lastGoodUtc = null,
//        string? firmwareVersion = null,
//        string? eventId = null,
//        CancellationToken ct = default)
//    {
//        eventId ??= Guid.NewGuid().ToString();

//        string extraJson = extra switch
//        {
//            null => "{}",
//            string s => s,
//            _ => JsonSerializer.Serialize(extra)
//        };

//        if (Encoding.UTF8.GetByteCount(extraJson) > 1024)
//        {
//            Console.Error.WriteLine(
//                "[SNMP] Event extra JSON exceeds the MIB's 1024-byte limit.");

//            extraJson = JsonSerializer.Serialize(new
//            {
//                truncated = true,
//                originalLength = Encoding.UTF8.GetByteCount(extraJson)
//            });
//        }

//        var vars = new List<Variable>
//        {
//            new(Obj(1), new OctetString(eventId)),
//            new(Obj(2), new OctetString(_assetId)),
//            new(Obj(3), new Integer32((int)_deviceType)),
//            new(Obj(4), new IP(IPAddress.Parse(_deviceIp).GetAddressBytes())),
//            new(Obj(5), new Integer32((int)type)),
//            new(Obj(6), new Integer32((int)code)),
//            new(Obj(7), new Integer32((int)severity)),
//            new(Obj(8), new OctetString(Ts(DateTime.UtcNow))),
//            new(Obj(9), new OctetString(Truncate(description, 255))),
//            new(Obj(10), new OctetString(extraJson)),
//            new(Obj(11), new Integer32((int)health))
//        };

//        if (type == EventType.Failure &&
//            !string.IsNullOrWhiteSpace(suspectedComponent))
//        {
//            vars.Add(new Variable(
//                Obj(12),
//                new OctetString(Truncate(suspectedComponent, 64))));
//        }

//        if (type == EventType.Failure && lastGoodUtc.HasValue)
//        {
//            vars.Add(new Variable(
//                Obj(13),
//                new OctetString(Ts(lastGoodUtc.Value))));
//        }

//        // Obj(14) is ipisMetricName, not firmwareVersion.
//        // Do not place firmware data in the metric-name OID.

//        var auth = BuildAuthProvider();
//        var privacy = BuildPrivProvider(auth);

//        var notificationOid = new ObjectIdentifier(
//            SelectNotificationOid(type, code));

//        uint uptimeTicks = (uint)Math.Max(
//            0,
//            (DateTime.UtcNow - _startedUtc).TotalMilliseconds / 10);

//        int messageId = Interlocked.Increment(ref _msgId)
//            & 0x7FFFFFFF;

//        bool sent;

//        if (ShouldUseInform(severity, code))
//        {
//            sent = await SendInformWithRetriesAsync(
//                messageId,
//                notificationOid,
//                uptimeTicks,
//                vars,
//                privacy,
//                ct);
//        }
//        else
//        {
//            sent = await SendTrapAsync(
//                messageId,
//                notificationOid,
//                uptimeTicks,
//                vars,
//                privacy,
//                ct);
//        }

//        // Every trap send is followed by an audio output check. This is the
//        // only place the audio output is probed.
//        await ReportAudioOutputAfterTrapAsync(code, ct);

//        return sent;
//    }

//    // ---------------------------------------------------------------
//    // Audio output check (runs only from SendEventAsync)
//    // ---------------------------------------------------------------

//    private async Task ReportAudioOutputAfterTrapAsync(
//        EventCode code,
//        CancellationToken ct)
//    {
//        // The audio traps themselves come back through SendEventAsync;
//        // probing again for them would recurse forever.
//        if (code is EventCode.AudioHwFail or EventCode.AudioOutputRestored)
//            return;

//        if (ct.IsCancellationRequested)
//            return;

//        bool ok = ProbeAudioOutput(out string detail);

//        bool report;
//        DateTime? lastGoodUtc;

//        lock (_audioLock)
//        {
//            report = _o.AudioReportOnEveryTrap || _lastAudioOk != ok;
//            _lastAudioOk = ok;

//            if (ok)
//                _lastAudioOkUtc = DateTime.UtcNow;

//            lastGoodUtc = _lastAudioOkUtc;
//        }


//        try
//        {
//            bool audioSent;

//            if (ok)
//            {

//                audioSent = await SendAudioOutputRestoredAsync(
//                    description: $"Audio output OK: {detail}",
//                    component: "Audio Output",
//                    ct: ct);
//            }

//        }
//        catch (OperationCanceledException)
//            when (ct.IsCancellationRequested)
//        {
//            // Caller cancelled; the primary trap has already gone out.
//        }
//        catch (Exception ex)
//        {
//            // Never let the audio check change the result of the primary trap.
//            Console.Error.WriteLine(
//                $"[SNMP] Audio output trap error: " +
//                $"{ex.GetType().Name}: {ex.Message}");
//        }
//    }

//    /// <summary>
//    /// True when Windows has an active default audio render (output) device.
//    /// This sees the Windows endpoint only: an unplugged jack, a missing USB
//    /// DAC or a disabled device is NOT OK; a dead amplifier behind a working
//    /// endpoint is still OK.
//    /// </summary>
//    private static bool ProbeAudioOutput(out string detail)
//    {
//        try
//        {
//            using var enumerator = new MMDeviceEnumerator();

//            if (!enumerator.HasDefaultAudioEndpoint(
//                    DataFlow.Render, Role.Multimedia))
//            {
//                detail = "no default render device";
//                return false;
//            }

//            using var device = enumerator.GetDefaultAudioEndpoint(
//                DataFlow.Render, Role.Multimedia);

//            if (device.State != DeviceState.Active)
//            {
//                detail = $"{device.FriendlyName} state = {device.State}";
//                return false;
//            }

//            detail = device.FriendlyName;
//            return true;
//        }
//        catch (Exception ex)
//        {
//            detail = $"probe failed: {ex.GetType().Name}: {ex.Message}";
//            return false;
//        }
//    }

//    private async Task<bool> SendTrapAsync(
//        int messageId,
//        ObjectIdentifier notificationOid,
//        uint uptimeTicks,
//        IList<Variable> vars,
//        IPrivacyProvider privacy,
//        CancellationToken ct)
//    {
//        try
//        {
//            ct.ThrowIfCancellationRequested();

//            var trap = new TrapV2Message(
//                VersionCode.V3,
//                messageId,
//                messageId,
//                new OctetString(_o.Username),
//                notificationOid,
//                uptimeTicks,
//                vars,
//                privacy,
//                Messenger.MaxMessageSize,
//                _engineId,
//                checked((int)_engineBoots),
//                checked((int)Math.Max(
//                    0,
//                    (DateTime.UtcNow - _startedUtc).TotalSeconds)));

//            using var socket = CreateBoundSocket();

//            var endpoint = new IPEndPoint(
//                IPAddress.Parse(_o.NmsIp),
//                _o.Port);

//            await trap.SendAsync(endpoint, socket)
//                .WaitAsync(
//                    TimeSpan.FromMilliseconds(_o.TimeoutMs),
//                    ct);

//            Console.WriteLine(
//                $"[SNMP] TRAP submitted: " +
//                $"OID={notificationOid}, Asset={_assetId}");

//            // UDP TRAP has no receiver acknowledgement.
//            return true;
//        }
//        catch (OperationCanceledException)
//        {
//            throw;
//        }
//        catch (Exception ex)
//        {
//            Console.Error.WriteLine(
//                $"[SNMP] TRAP failed: {ex.GetType().Name}: {ex.Message}");

//            return false;
//        }
//    }

//    private async Task<bool> SendInformWithRetriesAsync(
//        int baseMessageId,
//        ObjectIdentifier notificationOid,
//        uint uptimeTicks,
//        IList<Variable> vars,
//        IPrivacyProvider privacy,
//        CancellationToken ct)
//    {
//        int[] retries = _o.InformRetrySeconds ?? Array.Empty<int>();
//        int attempts = 1 + retries.Length;

//        var timeout = TimeSpan.FromMilliseconds(_o.TimeoutMs);

//        for (int attempt = 0; attempt < attempts; attempt++)
//        {
//            ct.ThrowIfCancellationRequested();

//            if (attempt > 0)
//            {
//                int delaySeconds = Math.Max(0, retries[attempt - 1]);

//                Console.WriteLine(
//                    $"[SNMP] INFORM retry {attempt + 1}/{attempts} " +
//                    $"after {delaySeconds}s.");

//                await Task.Delay(
//                    TimeSpan.FromSeconds(delaySeconds), ct);
//            }

//            try
//            {
//                int requestId = (baseMessageId + attempt)
//                    & 0x7FFFFFFF;

//                var endpoint = new IPEndPoint(
//                    IPAddress.Parse(_o.NmsIp),
//                    _o.Port);

//                using var cts =
//                    CancellationTokenSource.CreateLinkedTokenSource(ct);

//                cts.CancelAfter(timeout);

//                // Discover the authoritative engine ID of the NMS.
//                var discovery = new Discovery( requestId,
//                    requestId,
//                    Messenger.MaxMessageSize);

//                var reportMessage = await discovery.GetResponseAsync(
//                    endpoint, cts.Token);

//                if (reportMessage is not ReportMessage report)
//                {
//                    throw new InvalidOperationException(
//                        "SNMPv3 engine discovery did not return a Report.");
//                }

//                var inform = new InformRequestMessage( VersionCode.V3, requestId, requestId, new OctetString(_o.Username), notificationOid, uptimeTicks, vars, privacy, Messenger.MaxMessageSize, report);

//                var response = await inform.GetResponseAsync( endpoint, cts.Token);

//                if (response.Pdu().ErrorStatus.ToInt32() != 0)
//                {
//                    throw new InvalidOperationException(
//                        $"INFORM error status: " +
//                        $"{response.Pdu().ErrorStatus}");
//                }

//                Console.WriteLine(
//                    $"[SNMP] INFORM ACK received: " +
//                    $"OID={notificationOid}, Asset={_assetId}");

//                return true;
//            }
//            catch (OperationCanceledException)
//                when (ct.IsCancellationRequested)
//            {
//                throw;
//            }
//            catch (OperationCanceledException)
//            {
//                Console.Error.WriteLine(
//                    $"[SNMP] INFORM attempt {attempt + 1} timed out.");
//            }
//            catch (Exception ex)
//            {
//                Console.Error.WriteLine(
//                    $"[SNMP] INFORM attempt {attempt + 1} failed: " +
//                    $"{ex.GetType().Name}: {ex.Message}");
//            }
//        }

//        Console.Error.WriteLine(
//            "[SNMP] INFORM was not acknowledged after all retries.");

//        // Explicit fallback: success here means the TRAP was submitted,
//        // not that an INFORM was acknowledged.
//        return await SendTrapAsync(
//            (baseMessageId + 0x1000) & 0x7FFFFFFF,
//            notificationOid,
//            uptimeTicks,
//            vars,
//            privacy,
//            ct);
//    }

//    private Socket CreateBoundSocket()
//    {
//        var socket = new Socket(
//            AddressFamily.InterNetwork,
//            SocketType.Dgram,
//            ProtocolType.Udp);

//        try
//        {
//            IPAddress bindAddress = IPAddress.Any;

//            if (!string.IsNullOrWhiteSpace(_o.LocalIp) &&
//                IPAddress.TryParse(_o.LocalIp, out var parsed) &&
//                parsed.AddressFamily == AddressFamily.InterNetwork &&
//                IsLocalAddress(parsed))
//            {
//                bindAddress = parsed;
//            }

//            socket.Bind(new IPEndPoint(bindAddress, 0));

//            return socket;
//        }
//        catch
//        {
//            socket.Dispose();
//            throw;
//        }
//    }

//    private static bool IsLocalAddress(IPAddress address)
//    {
//        if (IPAddress.IsLoopback(address))
//            return true;

//        foreach (var nic in
//            System.Net.NetworkInformation.NetworkInterface
//                .GetAllNetworkInterfaces())
//        {
//            if (nic.OperationalStatus !=
//                System.Net.NetworkInformation.OperationalStatus.Up)
//            {
//                continue;
//            }

//            foreach (var item in nic.GetIPProperties().UnicastAddresses)
//            {
//                if (item.Address.Equals(address))
//                    return true;
//            }
//        }

//        return false;
//    }

//    // ---------------------------------------------------------------
//    // Convenience senders
//    // ---------------------------------------------------------------

//    public Task<bool> SendStartupAsync() => SendEventAsync( EventType.Startup, EventCode.GenericOk, Severity.Info, Health.Ok, $"{_deviceTypeName} started successfully");

//    public Task<bool> SendShutdownAsync() => SendEventAsync( EventType.Shutdown, EventCode.GenericOk, Severity.Info, Health.Ok, $"{_deviceTypeName} shutdown");

//    public Task<bool> SendPeriodicStatusAsync(object? extra = null) => SendEventAsync( EventType.Status, EventCode.GenericOk, Severity.Info, Health.Ok, $"{_deviceTypeName} periodic status",
//            extra ?? new
//            {
//                uptimeSec = (long)(
//                    DateTime.UtcNow - _startedUtc).TotalSeconds,
//                ts = Ts(DateTime.UtcNow)
//            });




//    public Task<bool> SendNtesSyncFailureAsync(string description = "NTES synchronization failure") =>SendEventAsync( EventType.Failure,EventCode.NtesSyncFail, Severity.Major, Health.Major, description);

//    public Task<bool> SendNtesSyncAsync(string description = "NTES Is Connected With Your CDC") => SendEventAsync( EventType.Info, EventCode.NtesSyncOk, Severity.Info, Health.Ok, description);



//    public Task<bool> SendAudioOutputRestoredAsync(
//        string description = "Audio output restored",
//        string? component = "Audio Output",
//        CancellationToken ct = default)
//    {
//        return SendEventAsync(
//            EventType.Info,
//            EventCode.AudioOutputRestored,
//            Severity.Info,
//            Health.Ok,
//            description,
//            extra: new
//            {
//                subsystem = "Audio",
//                status = "OK",
//                component
//            },
//            ct: ct
//        );
//    }
//    public Task<bool> SendAudioOutputFailureAsync(
//    string description = "Audio output NOT OK",
//    string? component = "Audio Output",
//    DateTime? lastGoodUtc = null,
//    CancellationToken ct = default)
//    {
//        return SendEventAsync(
//            EventType.Failure,
//            EventCode.AudioHwFail,
//            Severity.Major,
//            Health.Major,
//            description,
//            extra: new
//            {
//                subsystem = "Audio",
//                status = "NOT_OK",
//                component
//            },
//            suspectedComponent: component,
//            lastGoodUtc: lastGoodUtc,
//            ct: ct);
//    }
//    private static string Ts(DateTime value) =>
//        value.ToUniversalTime().ToString(
//            "yyyy-MM-dd'T'HH:mm:ss'Z'",
//            System.Globalization.CultureInfo.InvariantCulture);

//    private static string Truncate(string? value, int maxLength)
//    {
//        value ??= string.Empty;

//        return value.Length <= maxLength
//            ? value
//            : value[..maxLength];
//    }

//    private static readonly Lazy<IpisTrapSender> _instance =
//        new(() => new IpisTrapSender());

//    public static IpisTrapSender Instance => _instance.Value;
//}


using Global_Data;
using Helper;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Security;
using NAudio.CoreAudioApi;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Variable = Lextm.SharpSnmpLib.Variable;
namespace Ipis.Snmp;

public enum DeviceType
{
    CDC = 1,
    CDS = 2,
    PDC = 3,
}

public enum EventType
{
    Startup = 1,
    Shutdown = 2,
    Failure = 3,
    Status = 4,
    Info = 5,
    Predictive = 6,
    LedTestResult = 7,
}

public enum EventCode
{
    GenericOk = 100,
    LedModuleFailure = 101,
    LedShortDetected = 102,
    LedOpenDetected = 103,
    LedPanelError = 104,

    PsuAcFail = 110,
    PsuBatteryLow = 111,

    NetworkLinkDown = 120,
    NetworkLinkUp = 121,

    AudioHwFail = 130,
    AudioOutputRestored = 131,

    FanFail = 140,
    HighTemperature = 150,

    NtesSyncFail = 160,
    NtesSyncOk = 161,
    PdcCommTimeout = 170,

    PredictiveLedDegrade = 200
}

public enum Severity
{
    Critical = 1,
    Major = 2,
    Minor = 3,
    Warning = 4,
    Info = 5
}

public enum Health
{
    Ok = 1,
    Warning = 2,
    Minor = 3,
    Major = 4,
    Critical = 5
}

/// <summary>ipisAudioOutputStatus (CSV OID 4.11.0).</summary>
public enum AudioStatus
{
    Ok = 1,
    Muted = 2,
    NoSignal = 3,
    HwFault = 4
}

/// <summary>
/// Live CDC values for the scalar OIDs in ipis_cdc_snmp_oids_spec.csv.
/// Fill this from your application via IpisTrapSender.StatusProvider.
/// </summary>
public sealed class CdcStatusSnapshot
{
    public int LoadCurrentMa { get; init; }                         // 4.7.0
    public AudioStatus Audio { get; init; } = AudioStatus.Ok;       // 4.11.0
    public DateTime? AudioLastTestUtc { get; init; }                // 4.12.0
    public DateTime? NtesLastSyncUtc { get; init; }                 // 4.13.0
    public bool NtesSyncOk { get; init; }                           // 4.14.0
    public string NtesSyncLog { get; init; } = "";                  // 4.15.0
    public string SoftwareBuild { get; init; } = "";                // 2.7.0
}

public sealed class IpisTrapOptions
{
    public string NmsIp { get; set; } = Config.TrapNmsIp;
    public int Port { get; set; } = Config.TrapPort;

    // Fixed identities for the two redundant CDC PCs (RDSO Annexure-E:
    // unique ipisDeviceAssetID per device, format DIV-STN-DEV-XXXX).
    public const string MasterIp = "10.0.0.253";
    public const string StandbyIp = "10.0.0.254";
    public const string MasterAssetId = "DIV01-STN05-CDC-0001";
    public const string StandbyAssetId = "DIV01-STN05-CDC-0002";

    public static bool IsMaster => GlobalData._lstAdminSetting?.Ismaster == 1;

    // Master = 10.0.0.253, Standby = 10.0.0.254 (also the source bind address).
    public string LocalIp => IsMaster ? MasterIp : StandbyIp;
    public string Username { get; set; } = Config.TrapUsername;

    // Configured securely via Config; must match the NMS user.
    public string AuthPassword { get; set; } = Config.TrapAuthPassword;
    public string PrivPassword { get; set; } = Config.TrapPrivPassword;

    public string BaseOid { get; set; } = Config.TrapBaseOid;

    // Derived from master/standby role unless explicitly overridden.
    private string? _assetIdOverride;
    public string AssetId
    {
        get => _assetIdOverride ?? (IsMaster ? MasterAssetId : StandbyAssetId);
        set => _assetIdOverride = value;
    }

    public string AuthProtocol { get; set; } = Config.TrapAuthProtocol;

    public string PrivProtocol { get; set; } = Config.TrapPrivProtocol;

    public uint EnterpriseNumber { get; set; } = Config.TrapEnterpriseNumber;

    public int TimeoutMs { get; set; } = Config.TrapTimeoutMs;

    public int[] InformRetrySeconds { get; set; } =
        new[] { 6, 12, 24 };

    public int StatusIntervalSeconds { get; set; } = Config.TrapStatusIntervalSeconds;

    public bool SendStartupOnStart { get; set; } = true;
    public bool SendShutdownOnStop { get; set; } = true;

    public bool AudioReportOnEveryTrap { get; set; } = false;
}

public sealed class IpisTrapSender : IAsyncDisposable
{
    private readonly IpisTrapOptions _o;
    private readonly DeviceType _deviceType;
    private readonly string _deviceTypeName;
    private readonly string _deviceIp;
    private readonly string _assetId;
    private readonly string _firmwareVersion;

    private readonly DateTime _startedUtc = DateTime.UtcNow;

    private readonly OctetString _engineId;
    private readonly uint _engineBoots;

    private readonly object _loopLock = new();

    private CancellationTokenSource? _loopCts;
    private Task? _loopTask;

    private bool _startupSent;

    // Audio output state as seen at the last trap send. null = not probed yet.
    private readonly object _audioLock = new();
    private bool? _lastAudioOk;
    private DateTime? _lastAudioOkUtc;

    private static int _msgId = Environment.TickCount & 0x3FFFFFFF;

    /// <summary>
    /// Supplies live values for the CSV scalar OIDs (load current, audio,
    /// NTES, software build). If null, defaults are sent.
    /// </summary>
    public Func<CdcStatusSnapshot>? StatusProvider { get; set; }

    private string EventOid(int n) => $"{_o.BaseOid}.3.{n}";
    private string NotificationOid(int n) => $"{_o.BaseOid}.5.{n}";

    private ObjectIdentifier Obj(int n) =>
        new(EventOid(n));

    // CSV scalar OIDs (instance .0): ipisDevice = .2.N.0, ipisStatus = .4.N.0
    private ObjectIdentifier Dev(int n) => new($"{_o.BaseOid}.2.{n}.0");
    private ObjectIdentifier Stat(int n) => new($"{_o.BaseOid}.4.{n}.0");

    public IpisTrapSender()
        : this(new IpisTrapOptions())
    {
    }

    // Asset ID and device IP follow the master/standby role:
    //   Master  (Ismaster == 1): 10.0.0.253  DIV01-STN05-CDC-0001
    //   Standby               : 10.0.0.254  DIV01-STN05-CDC-0002
    private IpisTrapSender(IpisTrapOptions options)
        : this(
            options,
            Enum.TryParse<DeviceType>(Config.TrapDeviceType, true, out var dt) ? dt : DeviceType.CDC,
            options.LocalIp,
            options.AssetId,
            Config.TrapFirmwareVersion)
    {
    }

    public IpisTrapSender(
        IpisTrapOptions options,
        DeviceType deviceType,
        string deviceIp,
        string assetId,
        string? firmwareVersion = null)
    {
        _o = options ?? throw new ArgumentNullException(nameof(options));

        if (!IPAddress.TryParse(deviceIp, out var parsedIp) ||
            parsedIp.AddressFamily != AddressFamily.InterNetwork)
        {
            throw new ArgumentException(
                "A valid IPv4 device address is required.",
                nameof(deviceIp));
        }

        if (!IPAddress.TryParse(_o.NmsIp, out var parsedNms) ||
            parsedNms.AddressFamily != AddressFamily.InterNetwork)
        {
            throw new ArgumentException("NmsIp must be a valid IPv4 address.");
        }

        if (_o.Port is < 1 or > 65535)
            throw new ArgumentOutOfRangeException(nameof(options.Port));

        if (string.IsNullOrWhiteSpace(_o.BaseOid))
            throw new ArgumentException("BaseOid is required.");

        // Validate that BaseOid is a valid numeric OID.
        _ = new ObjectIdentifier(_o.BaseOid);

        if (string.IsNullOrWhiteSpace(_o.Username))
            throw new ArgumentException("SNMPv3 username is required.");

        if (string.IsNullOrEmpty(_o.AuthPassword) ||
            string.IsNullOrEmpty(_o.PrivPassword))
        {
            throw new ArgumentException(
                "SNMPv3 authentication and privacy passwords are required.");
        }

        _deviceType = deviceType;
        _deviceTypeName = deviceType.ToString();
        _deviceIp = parsedIp.ToString();
        _assetId = string.IsNullOrWhiteSpace(assetId)
            ? throw new ArgumentException("Asset ID is required.")
            : assetId;
        _firmwareVersion = string.IsNullOrWhiteSpace(firmwareVersion)
            ? "0.0.0"
            : firmwareVersion;

        _engineId = BuildEngineId(_o.EnterpriseNumber, _assetId);
        _engineBoots = LoadAndIncrementEngineBoots(_assetId);
    }

    // ---------------------------------------------------------------
    // Lifecycle: start / stop
    // ---------------------------------------------------------------

    /// <summary>
    /// Call once when the application starts.
    /// Sends the startup trap exactly once. Optionally starts the
    /// periodic heartbeat if StatusIntervalSeconds > 0.
    /// </summary>
    public async Task StartAsync(Func<object?>? extraProvider = null)
    {
        // Startup trap - sent exactly once.
        if (_o.SendStartupOnStart && !_startupSent)
        {
            _startupSent = true;

            try
            {
                bool startupSent = await SendStartupAsync();
                Console.WriteLine($"[SNMP] Startup result: {startupSent}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(
                    $"[SNMP] Startup error: {ex.GetType().Name}: {ex.Message}");
            }
        }

        // Periodic heartbeat - only if explicitly enabled.
        if (_o.StatusIntervalSeconds <= 0)
        {
            Console.WriteLine("[SNMP] Periodic status disabled.");
            return;
        }

        lock (_loopLock)
        {
            if (_loopTask is { IsCompleted: false })
                return;

            _loopCts?.Dispose();
            _loopCts = new CancellationTokenSource();

            var ct = _loopCts.Token;
            _loopTask = Task.Run(() => PeriodicLoopAsync(extraProvider, ct));
        }

        Console.WriteLine(
            $"[SNMP] Periodic loop started: {_o.StatusIntervalSeconds} seconds.");
    }

    /// <summary>
    /// Backwards-compatible alias. Prefer <see cref="StartAsync"/>.
    /// </summary>
    public Task StartPeriodicStatus(Func<object?>? extraProvider = null)
        => StartAsync(extraProvider);

    private async Task PeriodicLoopAsync(
        Func<object?>? extraProvider,
        CancellationToken ct)
    {
        try
        {
            using var timer = new PeriodicTimer(
                TimeSpan.FromSeconds(
                    Math.Max(1, _o.StatusIntervalSeconds)));

            while (await timer.WaitForNextTickAsync(ct))
            {
                try
                {
                    object? extra = extraProvider?.Invoke() ?? new
                    {
                        uptimeSec = (long)(
                            DateTime.UtcNow - _startedUtc).TotalSeconds,
                        ts = Ts(DateTime.UtcNow)
                    };

                    bool sent = await SendPeriodicStatusAsync(extra);

                    if (!sent)
                    {
                        Console.Error.WriteLine(
                            $"[SNMP] Periodic status failed. " +
                            $"NMS={_o.NmsIp}:{_o.Port}, Asset={_assetId}");
                    }
                }
                catch (OperationCanceledException)
                    when (ct.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(
                        $"[SNMP] Periodic error: " +
                        $"{ex.GetType().Name}: {ex.Message}");
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Normal shutdown.
        }
    }

    public async Task StopPeriodicStatusAsync()
    {
        CancellationTokenSource? cts;
        Task? task;

        lock (_loopLock)
        {
            cts = _loopCts;
            task = _loopTask;

            _loopCts = null;
            _loopTask = null;
        }

        if (cts != null)
        {
            try
            {
                cts.Cancel();

                if (task != null)
                    await task;
            }
            catch (OperationCanceledException)
            {
                // Normal cancellation.
            }
            finally
            {
                cts.Dispose();
            }

            Console.WriteLine("[SNMP] Periodic loop stopped.");
        }

        if (_o.SendShutdownOnStop)
        {
            try
            {
                await SendShutdownAsync();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(
                    $"[SNMP] Shutdown error: {ex.GetType().Name}: {ex.Message}");
            }
        }
    }

    public async ValueTask DisposeAsync()
    {
        await StopPeriodicStatusAsync();
    }

    // ---------------------------------------------------------------
    // SNMPv3 providers
    // ---------------------------------------------------------------

    private IAuthenticationProvider BuildAuthProvider()
    {
        string protocol = (_o.AuthProtocol ?? "SHA")
            .Replace("-", "")
            .Replace("(", "")
            .Replace(")", "")
            .ToUpperInvariant();

        var password = new OctetString(_o.AuthPassword);

        if (protocol.Contains("MD5"))
            return new MD5AuthenticationProvider(password);

        if (protocol.Contains("SHA512"))
            return new SHA512AuthenticationProvider(password);

        if (protocol.Contains("SHA384"))
            return new SHA384AuthenticationProvider(password);

        if (protocol.Contains("SHA256"))
            return new SHA256AuthenticationProvider(password);

        if (protocol is "SHA" or "SHA1" or "SHAHMACSHA96")
            return new SHA1AuthenticationProvider(password);

        throw new NotSupportedException(
            $"Unsupported authentication protocol: {_o.AuthProtocol}");
    }

    private IPrivacyProvider BuildPrivProvider(
        IAuthenticationProvider auth)
    {
        string protocol = (_o.PrivProtocol ?? "AES-128")
            .Replace("-", "")
            .ToUpperInvariant();

        var password = new OctetString(_o.PrivPassword);

        if (protocol.Contains("AES256"))
            return new AES256PrivacyProvider(password, auth);

        if (protocol.Contains("AES192"))
            return new AES192PrivacyProvider(password, auth);

        if (protocol is "AES" or "AES128")
            return new AESPrivacyProvider(password, auth);

        if (protocol == "DES")
            return new DESPrivacyProvider(password, auth);

        throw new NotSupportedException(
            $"Unsupported privacy protocol: {_o.PrivProtocol}");
    }

    // ---------------------------------------------------------------
    // Engine ID and persistent engine boots
    // ---------------------------------------------------------------

    private static OctetString BuildEngineId(
        uint enterprise,
        string seed)
    {
        if (string.IsNullOrWhiteSpace(seed))
            throw new ArgumentException("Engine ID seed is required.");

        byte[] enterpriseBytes =
        {
            (byte)(enterprise >> 24),
            (byte)(enterprise >> 16),
            (byte)(enterprise >> 8),
            (byte)enterprise
        };

        // Enterprise-specific SNMP engine ID format.
        enterpriseBytes[0] |= 0x80;

        using var sha256 = SHA256.Create();

        byte[] hash = sha256.ComputeHash(
            Encoding.UTF8.GetBytes(seed));

        // Format: 4-byte enterprise number + 1-byte format + 20-byte suffix.
        var engineId = new byte[25];

        Buffer.BlockCopy(enterpriseBytes, 0, engineId, 0, 4);
        engineId[4] = 0x05;

        Buffer.BlockCopy(hash, 0, engineId, 5, 20);

        return new OctetString(engineId);
    }

    private static readonly object EngineBootsLock = new();

    private static uint LoadAndIncrementEngineBoots(string assetId)
    {
        string safeAsset = string.Concat(
            assetId.Select(c =>
                char.IsLetterOrDigit(c) || c is '-' or '_'
                    ? c
                    : '_'));

        string path = Path.Combine(
            Environment.GetFolderPath(
                Environment.SpecialFolder.CommonApplicationData),
            "Ipis",
            $"snmp-engineboots-{safeAsset}.txt");

        Directory.CreateDirectory(Path.GetDirectoryName(path)!);

        // Restrict concurrent updates within this process.
        lock (EngineBootsLock)
        {
            uint boots = 1;

            if (File.Exists(path) &&
                uint.TryParse(File.ReadAllText(path).Trim(), out uint previous))
            {
                if (previous == uint.MaxValue)
                {
                    throw new InvalidOperationException(
                        "SNMP engine boots counter has reached its maximum.");
                }

                boots = previous + 1;
            }

            string tempPath = path + ".tmp";

            File.WriteAllText(tempPath, boots.ToString());
            File.Move(tempPath, path, overwrite: true);

            return boots;
        }
    }

    // ---------------------------------------------------------------
    // Notification selection and delivery policy
    // ---------------------------------------------------------------

    private string SelectNotificationOid(EventType type, EventCode code)
    {
        // Specific notifications take precedence.
        if (code == EventCode.NetworkLinkDown)
            return NotificationOid(6); // ipisLinkDown

        if (code == EventCode.NetworkLinkUp)
            return NotificationOid(7); // ipisLinkUp

        if (code == EventCode.NtesSyncFail)
            return NotificationOid(8); // ipisNTESSyncFail

        // NOTE: .5.9 (ipisNTESSyncOk) is not in the Annexure-E notification
        // list. Confirm it exists in the published MIB.
        if (code == EventCode.NtesSyncOk)
            return NotificationOid(9); // ipisNTESSyncOk

        return type switch
        {
            EventType.Startup => NotificationOid(1),       // ipisStartup
            EventType.Failure => NotificationOid(2),       // ipisFailure
            EventType.Status => NotificationOid(3),        // ipisPeriodicStatus
            EventType.LedTestResult => NotificationOid(4), // ipisLedTestResult
            EventType.Predictive => NotificationOid(5),    // ipisPredictiveAlert
            EventType.Shutdown => NotificationOid(3),
            EventType.Info => NotificationOid(3),

            _ => NotificationOid(3)
        };
    }

    private static bool ShouldUseInform(
        EventType type,
        Severity severity,
        EventCode code)
    {
        // Annexure-E section 8: critical/major -> INFORM,
        // predictive alerts -> INFORM recommended.
        return severity is Severity.Critical or Severity.Major
            || type == EventType.Predictive
            || code is EventCode.LedModuleFailure
                or EventCode.NtesSyncFail
                or EventCode.PdcCommTimeout;
    }

    // ---------------------------------------------------------------
    // CDC status varbinds (CSV scalar OIDs)
    // ---------------------------------------------------------------

    private List<Variable> BuildCdcStatusVarbinds(
        Health health,
        CdcStatusSnapshot s)
    {
        uint ticks = (uint)Math.Max(
            0,
            (DateTime.UtcNow - _startedUtc).TotalMilliseconds / 10);

        return new List<Variable>
        {
            new(Dev(1), new OctetString(_assetId)),                    // ipisDeviceAssetID
            new(Dev(2), new Integer32((int)health)),                   // ipisDeviceHealth
            new(Dev(3), new TimeTicks(ticks)),                         // ipisDeviceUptime
            new(Dev(4), new OctetString(_firmwareVersion)),            // ipisFirmwareVersion
            new(Dev(5), new Integer32((int)_deviceType)),              // ipisDeviceType
            new(Dev(7), new OctetString(Truncate(s.SoftwareBuild, 255))), // ipisSoftwareBuild

            new(Stat(7), new Integer32(s.LoadCurrentMa)),              // ipisLoadCurrent_mA
            new(Stat(11), new Integer32((int)s.Audio)),                // ipisAudioOutputStatus
            new(Stat(12), new OctetString(                             // ipisAudioLastTestTS
                s.AudioLastTestUtc.HasValue ? Ts(s.AudioLastTestUtc.Value) : "")),
            new(Stat(13), new OctetString(                             // ipisNTESLastSyncTS
                s.NtesLastSyncUtc.HasValue ? Ts(s.NtesLastSyncUtc.Value) : "")),

            // TODO: confirm the SMI type of 4.14 in the published MIB.
            // Assumed enum: ok(1), fail(2).
            new(Stat(14), new Integer32(s.NtesSyncOk ? 1 : 2)),        // ipisNTESSyncStatus
            new(Stat(15), new OctetString(Truncate(s.NtesSyncLog, 255))) // ipisNTESSyncLog
        };
    }

    private CdcStatusSnapshot GetSnapshot()
    {
        try
        {
            return StatusProvider?.Invoke() ?? new CdcStatusSnapshot();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(
                $"[SNMP] StatusProvider error: {ex.GetType().Name}: {ex.Message}");
            return new CdcStatusSnapshot();
        }
    }

    // ---------------------------------------------------------------
    // Event payload and send
    // ---------------------------------------------------------------

    public async Task<bool> SendEventAsync(
        EventType type,
        EventCode code,
        Severity severity,
        Health health,
        string description,
        object? extra = null,
        string? suspectedComponent = null,
        DateTime? lastGoodUtc = null,
        string? firmwareVersion = null,   // kept for compatibility; unused
        string? eventId = null,
        IEnumerable<Variable>? statusVarbinds = null,
        CancellationToken ct = default)
    {
        eventId ??= Guid.NewGuid().ToString();

        string extraJson = extra switch
        {
            null => "{}",
            string s => s,
            _ => JsonSerializer.Serialize(extra)
        };

        if (Encoding.UTF8.GetByteCount(extraJson) > 1024)
        {
            Console.Error.WriteLine(
                "[SNMP] Event extra JSON exceeds the MIB's 1024-byte limit.");

            extraJson = JsonSerializer.Serialize(new
            {
                truncated = true,
                originalLength = Encoding.UTF8.GetByteCount(extraJson)
            });
        }

        var vars = new List<Variable>
        {
            new(Obj(1), new OctetString(eventId)),
            new(Obj(2), new OctetString(_assetId)),
            new(Obj(3), new Integer32((int)_deviceType)),
            new(Obj(4), new IP(IPAddress.Parse(_deviceIp).GetAddressBytes())),
            new(Obj(5), new Integer32((int)type)),
            new(Obj(6), new Integer32((int)code)),
            new(Obj(7), new Integer32((int)severity)),
            new(Obj(8), new OctetString(Ts(DateTime.UtcNow))),
            new(Obj(9), new OctetString(Truncate(description, 255))),
            new(Obj(10), new OctetString(extraJson)),
            new(Obj(11), new Integer32((int)health))
        };

        if (type == EventType.Failure &&
            !string.IsNullOrWhiteSpace(suspectedComponent))
        {
            vars.Add(new Variable(
                Obj(12),
                new OctetString(Truncate(suspectedComponent, 64))));
        }

        if (type == EventType.Failure && lastGoodUtc.HasValue)
        {
            vars.Add(new Variable(
                Obj(13),
                new OctetString(Ts(lastGoodUtc.Value))));
        }

        // Obj(14) is ipisMetricName, not firmwareVersion. Firmware version is
        // sent via the CSV OID (2.4.0) inside statusVarbinds.

        if (statusVarbinds != null)
            vars.AddRange(statusVarbinds);

        var auth = BuildAuthProvider();
        var privacy = BuildPrivProvider(auth);

        var notificationOid = new ObjectIdentifier(
            SelectNotificationOid(type, code));

        uint uptimeTicks = (uint)Math.Max(
            0,
            (DateTime.UtcNow - _startedUtc).TotalMilliseconds / 10);

        int messageId = Interlocked.Increment(ref _msgId)
            & 0x7FFFFFFF;

        bool sent;

        if (ShouldUseInform(type, severity, code))
        {
            sent = await SendInformWithRetriesAsync(
                messageId,
                notificationOid,
                uptimeTicks,
                vars,
                privacy,
                ct);
        }
        else
        {
            sent = await SendTrapAsync(
                messageId,
                notificationOid,
                uptimeTicks,
                vars,
                privacy,
                ct);
        }

        // Every trap send is followed by an audio output check. This is the
        // only place the audio output is probed.
        await ReportAudioOutputAfterTrapAsync(code, ct);

        return sent;
    }

    // ---------------------------------------------------------------
    // Audio output check (runs only from SendEventAsync)
    // ---------------------------------------------------------------

    private async Task ReportAudioOutputAfterTrapAsync(
        EventCode code,
        CancellationToken ct)
    {
        // The audio traps themselves come back through SendEventAsync;
        // probing again for them would recurse forever.
        if (code is EventCode.AudioHwFail or EventCode.AudioOutputRestored)
            return;

        if (ct.IsCancellationRequested)
            return;

        bool ok = ProbeAudioOutput(out string detail);

        bool report;
        DateTime? lastGoodUtc;

        lock (_audioLock)
        {
            bool firstProbe = _lastAudioOk is null;

            report = _o.AudioReportOnEveryTrap
                  || (firstProbe && !ok)                    // first probe, already broken
                  || (!firstProbe && _lastAudioOk != ok);   // state changed

            _lastAudioOk = ok;

            if (ok)
                _lastAudioOkUtc = DateTime.UtcNow;

            lastGoodUtc = _lastAudioOkUtc;
        }

        if (!report)
            return;

        try
        {
            if (ok)
            {
                await SendAudioOutputRestoredAsync(
                    description: $"Audio output OK: {detail}",
                    component: "Audio Output",
                    ct: ct);
            }
            else
            {
                await SendAudioOutputFailureAsync(
                    description: $"Audio output NOT OK: {detail}",
                    component: "Audio Output",
                    lastGoodUtc: lastGoodUtc,
                    ct: ct);
            }
        }
        catch (OperationCanceledException)
            when (ct.IsCancellationRequested)
        {
            // Caller cancelled; the primary trap has already gone out.
        }
        catch (Exception ex)
        {
            // Never let the audio check change the result of the primary trap.
            Console.Error.WriteLine(
                $"[SNMP] Audio output trap error: " +
                $"{ex.GetType().Name}: {ex.Message}");
        }
    }

    /// <summary>
    /// True when Windows has an active default audio render (output) device.
    /// This sees the Windows endpoint only: an unplugged jack, a missing USB
    /// DAC or a disabled device is NOT OK; a dead amplifier behind a working
    /// endpoint is still OK.
    /// </summary>
    private static bool ProbeAudioOutput(out string detail)
    {
        try
        {
            using var enumerator = new MMDeviceEnumerator();

            if (!enumerator.HasDefaultAudioEndpoint(
                    DataFlow.Render, Role.Multimedia))
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
            detail = $"probe failed: {ex.GetType().Name}: {ex.Message}";
            return false;
        }
    }

    // ---------------------------------------------------------------
    // Transport: TRAP and INFORM
    // ---------------------------------------------------------------

    private async Task<bool> SendTrapAsync(
        int messageId,
        ObjectIdentifier notificationOid,
        uint uptimeTicks,
        IList<Variable> vars,
        IPrivacyProvider privacy,
        CancellationToken ct)
    {
        try
        {
            ct.ThrowIfCancellationRequested();

            var trap = new TrapV2Message(
                VersionCode.V3,
                messageId,
                messageId,
                new OctetString(_o.Username),
                notificationOid,
                uptimeTicks,
                vars,
                privacy,
                Messenger.MaxMessageSize,
                _engineId,
                checked((int)_engineBoots),
                checked((int)Math.Max(
                    0,
                    (DateTime.UtcNow - _startedUtc).TotalSeconds)));

            using var socket = CreateBoundSocket();

            var endpoint = new IPEndPoint(
                IPAddress.Parse(_o.NmsIp),
                _o.Port);

            await trap.SendAsync(endpoint, socket)
                .WaitAsync(
                    TimeSpan.FromMilliseconds(_o.TimeoutMs),
                    ct);

            Console.WriteLine(
                $"[SNMP] TRAP submitted: " +
                $"OID={notificationOid}, Asset={_assetId}");

            // UDP TRAP has no receiver acknowledgement.
            return true;
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(
                $"[SNMP] TRAP failed: {ex.GetType().Name}: {ex.Message}");

            return false;
        }
    }

    private async Task<bool> SendInformWithRetriesAsync(
        int baseMessageId,
        ObjectIdentifier notificationOid,
        uint uptimeTicks,
        IList<Variable> vars,
        IPrivacyProvider privacy,
        CancellationToken ct)
    {
        int[] retries = _o.InformRetrySeconds ?? Array.Empty<int>();
        int attempts = 1 + retries.Length;

        var timeout = TimeSpan.FromMilliseconds(_o.TimeoutMs);

        for (int attempt = 0; attempt < attempts; attempt++)
        {
            ct.ThrowIfCancellationRequested();

            if (attempt > 0)
            {
                int delaySeconds = Math.Max(0, retries[attempt - 1]);

                Console.WriteLine(
                    $"[SNMP] INFORM retry {attempt + 1}/{attempts} " +
                    $"after {delaySeconds}s.");

                await Task.Delay(
                    TimeSpan.FromSeconds(delaySeconds), ct);
            }

            try
            {
                int requestId = (baseMessageId + attempt)
                    & 0x7FFFFFFF;

                var endpoint = new IPEndPoint(
                    IPAddress.Parse(_o.NmsIp),
                    _o.Port);

                using var cts =
                    CancellationTokenSource.CreateLinkedTokenSource(ct);

                cts.CancelAfter(timeout);

                // Discover the authoritative engine ID of the NMS.
                var discovery = new Discovery(
                    requestId,
                    requestId,
                    Messenger.MaxMessageSize);

                var reportMessage = await discovery.GetResponseAsync(
                    endpoint, cts.Token);

                if (reportMessage is not ReportMessage report)
                {
                    throw new InvalidOperationException(
                        "SNMPv3 engine discovery did not return a Report.");
                }

                var inform = new InformRequestMessage(
                    VersionCode.V3,
                    requestId,
                    requestId,
                    new OctetString(_o.Username),
                    notificationOid,
                    uptimeTicks,
                    vars,
                    privacy,
                    Messenger.MaxMessageSize,
                    report);

                var response = await inform.GetResponseAsync(
                    endpoint, cts.Token);

                if (response.Pdu().ErrorStatus.ToInt32() != 0)
                {
                    throw new InvalidOperationException(
                        $"INFORM error status: " +
                        $"{response.Pdu().ErrorStatus}");
                }

                Console.WriteLine(
                    $"[SNMP] INFORM ACK received: " +
                    $"OID={notificationOid}, Asset={_assetId}");

                return true;
            }
            catch (OperationCanceledException)
                when (ct.IsCancellationRequested)
            {
                throw;
            }
            catch (OperationCanceledException)
            {
                Console.Error.WriteLine(
                    $"[SNMP] INFORM attempt {attempt + 1} timed out.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(
                    $"[SNMP] INFORM attempt {attempt + 1} failed: " +
                    $"{ex.GetType().Name}: {ex.Message}");
            }
        }

        Console.Error.WriteLine(
            "[SNMP] INFORM was not acknowledged after all retries.");

        // Explicit fallback (Annexure-E section 2): success here means the
        // TRAP was submitted, not that an INFORM was acknowledged.
        return await SendTrapAsync(
            (baseMessageId + 0x1000) & 0x7FFFFFFF,
            notificationOid,
            uptimeTicks,
            vars,
            privacy,
            ct);
    }

    private Socket CreateBoundSocket()
    {
        var socket = new Socket(
            AddressFamily.InterNetwork,
            SocketType.Dgram,
            ProtocolType.Udp);

        try
        {
            IPAddress bindAddress = IPAddress.Any;

            if (!string.IsNullOrWhiteSpace(_o.LocalIp) &&
                IPAddress.TryParse(_o.LocalIp, out var parsed) &&
                parsed.AddressFamily == AddressFamily.InterNetwork &&
                IsLocalAddress(parsed))
            {
                bindAddress = parsed;
            }

            socket.Bind(new IPEndPoint(bindAddress, 0));

            return socket;
        }
        catch
        {
            socket.Dispose();
            throw;
        }
    }

    private static bool IsLocalAddress(IPAddress address)
    {
        if (IPAddress.IsLoopback(address))
            return true;

        foreach (var nic in
            System.Net.NetworkInformation.NetworkInterface
                .GetAllNetworkInterfaces())
        {
            if (nic.OperationalStatus !=
                System.Net.NetworkInformation.OperationalStatus.Up)
            {
                continue;
            }

            foreach (var item in nic.GetIPProperties().UnicastAddresses)
            {
                if (item.Address.Equals(address))
                    return true;
            }
        }

        return false;
    }

    // ---------------------------------------------------------------
    // Convenience senders
    // ---------------------------------------------------------------

    public Task<bool> SendStartupAsync() =>
        SendEventAsync(
            EventType.Startup,
            EventCode.GenericOk,
            Severity.Info,
            Health.Ok,
            $"{_deviceTypeName} started successfully",
            statusVarbinds: BuildCdcStatusVarbinds(Health.Ok, GetSnapshot()));

    public Task<bool> SendShutdownAsync() =>
        SendEventAsync(
            EventType.Shutdown,
            EventCode.GenericOk,
            Severity.Info,
            Health.Ok,
            $"{_deviceTypeName} shutdown");

    public Task<bool> SendPeriodicStatusAsync(object? extra = null) =>
        SendEventAsync(
            EventType.Status,
            EventCode.GenericOk,
            Severity.Info,
            Health.Ok,
            $"{_deviceTypeName} periodic status",
            extra: extra ?? new
            {
                uptimeSec = (long)(DateTime.UtcNow - _startedUtc).TotalSeconds,
                ts = Ts(DateTime.UtcNow)
            },
            statusVarbinds: BuildCdcStatusVarbinds(Health.Ok, GetSnapshot()));

    public Task<bool> SendNtesSyncFailureAsync(
        string description = "NTES synchronization failure") =>
        SendEventAsync(
            EventType.Failure,
            EventCode.NtesSyncFail,
            Severity.Major,
            Health.Major,
            description,
            statusVarbinds: BuildCdcStatusVarbinds(Health.Major, GetSnapshot()));

    public Task<bool> SendNtesSyncAsync(
        string description = "NTES Is Connected With Your CDC") =>
        SendEventAsync(
            EventType.Info,
            EventCode.NtesSyncOk,
            Severity.Info,
            Health.Ok,
            description,
            statusVarbinds: BuildCdcStatusVarbinds(Health.Ok, GetSnapshot()));

    public Task<bool> SendAudioOutputRestoredAsync(
        string description = "Audio output restored",
        string? component = "Audio Output",
        CancellationToken ct = default)
    {
        return SendEventAsync(
            EventType.Info,
            EventCode.AudioOutputRestored,
            Severity.Info,
            Health.Ok,
            description,
            extra: new
            {
                subsystem = "Audio",
                status = "OK",
                component
            },
            ct: ct);
    }

    public Task<bool> SendAudioOutputFailureAsync(
        string description = "Audio output NOT OK",
        string? component = "Audio Output",
        DateTime? lastGoodUtc = null,
        CancellationToken ct = default)
    {
        return SendEventAsync(
            EventType.Failure,
            EventCode.AudioHwFail,
            Severity.Major,
            Health.Major,
            description,
            extra: new
            {
                subsystem = "Audio",
                status = "NOT_OK",
                component
            },
            suspectedComponent: component,
            lastGoodUtc: lastGoodUtc,
            ct: ct);
    }

    // ---------------------------------------------------------------
    // Helpers
    // ---------------------------------------------------------------

    private static string Ts(DateTime value) =>
        value.ToUniversalTime().ToString(
            "yyyy-MM-dd'T'HH:mm:ss'Z'",
            System.Globalization.CultureInfo.InvariantCulture);

    private static string Truncate(string? value, int maxLength)
    {
        value ??= string.Empty;

        return value.Length <= maxLength
            ? value
            : value[..maxLength];
    }

    private static readonly Lazy<IpisTrapSender> _instance =
        new(() => new IpisTrapSender());

    public static IpisTrapSender Instance => _instance.Value;
}