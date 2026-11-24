#nullable enable
// ---------------------------------------------------------------------------
// IpisSnmpAgent.cs
//
// SNMPv3 command responder (UDP 161) for the IPIS CDC, so Edge NMS
// "SNMP GET" polling gets answers instead of timeouts.
//
// RDSO/SPN/TC/108/2019 v2.0 Annexure E:
//   Sec. 1  SNMPv3 only (authPriv)        -> v1/v2c packets are ignored
//   Sec. 2  agent on UDP 161, traps on 162
//   Sec. 4  status objects "must be available via GET and sent in periodic
//           status traps"                 -> same OIDs/values as the
//                                             IpisTrapSender status varbinds
//
// Serves GET, GETNEXT (walk) and GETBULK. SET is refused (notWritable).
// Uses the same SNMPv3 user / auth / priv settings, engine ID and engine
// boots file as IpisTrapSender, so Edge NMS needs only one credential.
//
// This file is self-contained. It does not change IpisTrapSender.
//
// Start it once, AFTER the trap sender has been created, for example in
// Program.cs right after:
//     IpisTrapSender.Instance.StartPeriodicStatus();
// add:
//     IpisSnmpAgent.Instance.Start();
// ---------------------------------------------------------------------------


using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Security;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using Variable = Lextm.SharpSnmpLib.Variable;

namespace Ipis.Snmp;

public sealed class IpisSnmpAgent : IDisposable
{
    public const int DefaultPort = 161;

    // RFC 3414 Sec. 3.2 step 7: 150 second time window.
    private const int TimeWindowSeconds = 150;
    private const int MaxUdpPayload = 65507;
    private const int MinMessageSize = 484;
    private const int MaxBulkRepetitions = 50;
    private const int MaxBulkVarbinds = 200;

    // Windows: stop ICMP "port unreachable" from breaking ReceiveAsync.
    private const int SioUdpConnReset = -1744830452;

    // usmStats counters (RFC 3414) returned in REPORT PDUs.
    private static readonly ObjectIdentifier UsmStatsUnsupportedSecLevels = new("1.3.6.1.6.3.15.1.1.1.0");
    private static readonly ObjectIdentifier UsmStatsNotInTimeWindows = new("1.3.6.1.6.3.15.1.1.2.0");
    private static readonly ObjectIdentifier UsmStatsUnknownUserNames = new("1.3.6.1.6.3.15.1.1.3.0");
    private static readonly ObjectIdentifier UsmStatsUnknownEngineIds = new("1.3.6.1.6.3.15.1.1.4.0");
    private static readonly ObjectIdentifier UsmStatsWrongDigests = new("1.3.6.1.6.3.15.1.1.5.0");
    private static readonly ObjectIdentifier UsmStatsDecryptionErrors = new("1.3.6.1.6.3.15.1.1.6.0");

    private static readonly Lazy<IpisSnmpAgent> _instance =
        new(() => new IpisSnmpAgent());

    public static IpisSnmpAgent Instance => _instance.Value;

    private readonly IpisTrapOptions _o;
    private readonly int _port;
    private readonly string? _bindIp;

    private readonly string _baseOid;
    private readonly string _assetId;
    private readonly string _firmwareVersion;
    private readonly DeviceType _deviceType;

    private readonly OctetString _userName;
    private readonly IAuthenticationProvider _auth;
    private readonly IPrivacyProvider _privacy;   // authPriv
    private readonly IPrivacyProvider _authOnly;  // authNoPriv (authenticated REPORTs)
    private readonly UserRegistry _users = new();

    private readonly OctetString _engineId;
    private int _engineBoots = 1;
    private DateTime _engineStartUtc = DateTime.UtcNow;
    private readonly DateTime _createdUtc = DateTime.UtcNow;

    private readonly object _lock = new();
    private readonly List<UdpClient> _sockets = new();
    private CancellationTokenSource? _cts;

    private readonly object _tableLock = new();
    private readonly SortedDictionary<ObjectIdentifier, Func<CdcStatusSnapshot, ISnmpData>> _entries = new();
    private volatile OidTable _table = OidTable.Empty;

    private int _unsupportedSecLevels;
    private int _notInTimeWindows;
    private int _unknownUserNames;
    private int _unknownEngineIds;
    private int _wrongDigests;
    private int _decryptionErrors;

    private readonly object _logLock = new();
    private readonly Dictionary<string, DateTime> _lastLog = new();

    // Live status (used when nobody sets a StatusProvider).
    private static readonly TimeSpan SnapshotCacheTime = TimeSpan.FromSeconds(2);
    private readonly object _snapLock = new();
    private CdcStatusSnapshot? _cachedSnapshot;
    private DateTime _cachedSnapshotUtc = DateTime.MinValue;

    private readonly object _ntesLock = new();
    private bool? _ntesLastOk;            // null = no NTES result since start
    private DateTime? _ntesLastOkUtc;
    private string _ntesLog = "No NTES sync since application start";

    private readonly string _softwareBuild = ReadSoftwareBuild();

    /// <summary>
    /// Live CDC values. If null, IpisTrapSender.Instance.StatusProvider is
    /// used; if that is also null, the agent's own live status is used
    /// (real audio probe, NTES result from SetNtesStatus, app version).
    /// </summary>
    public Func<CdcStatusSnapshot>? StatusProvider { get; set; }

    /// <summary>
    /// ipisDeviceHealth (2.2.0). If null, health is worked out from the
    /// audio output and the last NTES result.
    /// </summary>
    public Func<Health>? HealthProvider { get; set; }

    /// <summary>
    /// Call with every NTES result (true = data fetched, false = failed).
    /// Feeds 4.13.0 (last sync), 4.14.0 (status), 4.15.0 (log) and 2.2.0.
    /// </summary>
    public void SetNtesStatus(bool ok, string? log = null)
    {
        lock (_ntesLock)
        {
            _ntesLastOk = ok;
            if (ok)
                _ntesLastOkUtc = DateTime.UtcNow;

            _ntesLog = string.IsNullOrWhiteSpace(log)
                ? (ok ? "NTES sync OK" : "NTES sync failed")
                : log.Trim();
        }

        lock (_snapLock)
            _cachedSnapshot = null; // next GET shows the new state at once
    }

    /// <summary>
    /// Annexure E Sec. 1: only authPriv requests are answered.
    /// Set to false to also accept authNoPriv.
    /// </summary>
    public bool RequireAuthPriv { get; set; } = true;

    public bool IsRunning
    {
        get { lock (_lock) return _sockets.Count > 0; }
    }

    /// <summary>Optional log sink (also written to Console and Debug output).</summary>
    public event Action<string>? LogMessage;

    public IpisSnmpAgent()
        : this(new IpisTrapOptions(), DefaultPort, null)
    {
    }

    /// <param name="options">Same options the trap sender uses (user, passwords, BaseOid).</param>
    /// <param name="port">UDP port, 161 by default.</param>
    /// <param name="bindIp">
    /// null = listen on every local IPv4 address (one socket each, so the
    /// reply always leaves from the address the NMS polled).
    /// "0.0.0.0" or a single address = listen only there.
    /// </param>
    public IpisSnmpAgent(IpisTrapOptions options, int port = DefaultPort, string? bindIp = null)
    {
        _o = options ?? throw new ArgumentNullException(nameof(options));

        if (port is < 1 or > 65535)
            throw new ArgumentOutOfRangeException(nameof(port));

        if (!string.IsNullOrWhiteSpace(bindIp) &&
            (!IPAddress.TryParse(bindIp, out var parsedBind) ||
             parsedBind.AddressFamily != AddressFamily.InterNetwork))
        {
            throw new ArgumentException("bindIp must be an IPv4 address.", nameof(bindIp));
        }

        if (string.IsNullOrWhiteSpace(_o.BaseOid))
            throw new ArgumentException("BaseOid is required.");

        if (string.IsNullOrWhiteSpace(_o.Username))
            throw new ArgumentException("SNMPv3 username is required.");

        if (string.IsNullOrEmpty(_o.AuthPassword) || string.IsNullOrEmpty(_o.PrivPassword))
            throw new ArgumentException("SNMPv3 authentication and privacy passwords are required.");

        _port = port;
        _bindIp = string.IsNullOrWhiteSpace(bindIp) ? null : bindIp.Trim();

        _baseOid = _o.BaseOid.Trim().TrimEnd('.');
        _ = new ObjectIdentifier(_baseOid); // validate

        _assetId = string.IsNullOrWhiteSpace(_o.AssetId)
            ? throw new ArgumentException("Asset ID is required.")
            : _o.AssetId;

        _firmwareVersion = string.IsNullOrWhiteSpace(Helper.Config.TrapFirmwareVersion)
            ? "0.0.0"
            : Helper.Config.TrapFirmwareVersion;

        _deviceType = Enum.TryParse<DeviceType>(Helper.Config.TrapDeviceType, true, out var dt)
            ? dt
            : DeviceType.CDC;

        _userName = new OctetString(_o.Username);
        _auth = BuildAuthProvider(_o.AuthProtocol, _o.AuthPassword);
        _privacy = BuildPrivProvider(_o.PrivProtocol, _o.PrivPassword, _auth);
        _authOnly = new DefaultPrivacyProvider(_auth);
        _users.Add(_userName, _privacy);

        // Same engine ID as IpisTrapSender: one SNMP engine per device.
        _engineId = BuildEngineId(_o.EnterpriseNumber, _assetId);

        RegisterDefaultObjects();
    }

    // ---------------------------------------------------------------
    // Object table
    // ---------------------------------------------------------------

    private void RegisterDefaultObjects()
    {
        string b = _baseOid;

        // ipisDevice (.2.N.0) - same values as IpisTrapSender.BuildCdcStatusVarbinds
        //Add($"{b}.2.1.0", _ => new OctetString(_assetId));                          // ipisDeviceAssetID
        //Add($"{b}.2.1.0", _ => new OctetString(string.IsNullOrWhiteSpace(_assetId) ? "CDC-DEFAULT" : _assetId));
        Add($"{b}.2.1.0", _ => new OctetString(ReportedAssetId()));                  // ipisDeviceAssetID (App.config, by master/standby)
        Add($"{b}.2.2.0", s => new Integer32((int)GetHealth(s)));                   // ipisDeviceHealth
        Add($"{b}.2.3.0", _ => new TimeTicks(UptimeTicks()));                       // ipisDeviceUptime
        Add($"{b}.2.4.0", _ => new OctetString(_firmwareVersion));                  // ipisFirmwareVersion
        Add($"{b}.2.5.0", _ => new Integer32((int)_deviceType));                    // ipisDeviceType
        Add($"{b}.2.7.0", s => new OctetString(Truncate(s.SoftwareBuild, 255)));    // ipisSoftwareBuild

        // ipisStatus (.4.N.0)
        Add($"{b}.4.7.0", s => new Integer32(s.LoadCurrentMa));                     // ipisLoadCurrent_mA
        Add($"{b}.4.11.0", s => new Integer32((int)s.Audio));                       // ipisAudioOutputStatus
        //Add($"{b}.4.12.0", s => new OctetString(                                    // ipisAudioLastTestTS
        //    s.AudioLastTestUtc.HasValue ? Ts(s.AudioLastTestUtc.Value) : ""));
        //Add($"{b}.4.13.0", s => new OctetString(                                    // ipisNTESLastSyncTS
        //    s.NtesLastSyncUtc.HasValue ? Ts(s.NtesLastSyncUtc.Value) : ""));
        Add($"{b}.4.14.0", s => new Integer32(s.NtesSyncOk ? 1 : 2));               // ipisNTESSyncStatus ok(1)/fail(2)
        Add($"{b}.4.15.0", s => new OctetString(Truncate(s.NtesSyncLog, 255)));     // ipisNTESSyncLog

        // MIB-2 system group (NMS reachability checks use sysUpTime / sysObjectID)
        Add("1.3.6.1.2.1.1.1.0", _ => new OctetString(
            $"Vardaan IPIS {_deviceType} {_assetId} fw {_firmwareVersion}"));     // sysDescr
        Add("1.3.6.1.2.1.1.2.0", _ => new ObjectIdentifier(b));                     // sysObjectID
        Add("1.3.6.1.2.1.1.3.0", _ => new TimeTicks(UptimeTicks()));                // sysUpTime
        Add("1.3.6.1.2.1.1.5.0", _ => new OctetString(Environment.MachineName));    // sysName

        // SNMP-FRAMEWORK-MIB snmpEngine group
        Add("1.3.6.1.6.3.10.2.1.1.0", _ => _engineId);                              // snmpEngineID
        Add("1.3.6.1.6.3.10.2.1.2.0", _ => new Integer32(_engineBoots));            // snmpEngineBoots
        Add("1.3.6.1.6.3.10.2.1.3.0", _ => new Integer32(EngineTime));              // snmpEngineTime
        Add("1.3.6.1.6.3.10.2.1.4.0", _ => new Integer32(MaxUdpPayload));           // snmpEngineMaxMessageSize

        RebuildTable();
    }

    private void Add(string oid, Func<CdcStatusSnapshot, ISnmpData> getter)
    {
        _entries[new ObjectIdentifier(oid)] = getter;
    }

    /// <summary>
    /// Adds or replaces a read-only object, e.g. board-wise OIDs later:
    /// agent.Register("1.3.6.1.4.1.65319.4.1.17.0", () => new Integer32(1));
    /// </summary>
    public void Register(string oid, Func<ISnmpData> getter)
    {
        if (getter == null)
            throw new ArgumentNullException(nameof(getter));

        Register(oid, _ => getter());
    }

    /// <summary>Adds or replaces a read-only object that uses the CDC snapshot.</summary>
    public void Register(string oid, Func<CdcStatusSnapshot, ISnmpData> getter)
    {
        if (getter == null)
            throw new ArgumentNullException(nameof(getter));

        var id = new ObjectIdentifier(oid);

        lock (_tableLock)
        {
            _entries[id] = getter;
            RebuildTable();
        }
    }

    public bool Unregister(string oid)
    {
        var id = new ObjectIdentifier(oid);

        lock (_tableLock)
        {
            bool removed = _entries.Remove(id);
            if (removed)
                RebuildTable();
            return removed;
        }
    }

    private void RebuildTable()
    {
        lock (_tableLock)
        {
            _table = new OidTable(
                _entries.Keys.ToArray(),
                _entries.Values.ToArray());
        }
    }

    private sealed class OidTable
    {
        public static readonly OidTable Empty =
            new(Array.Empty<ObjectIdentifier>(), Array.Empty<Func<CdcStatusSnapshot, ISnmpData>>());

        public OidTable(ObjectIdentifier[] oids, Func<CdcStatusSnapshot, ISnmpData>[] getters)
        {
            Oids = oids;
            Getters = getters;
        }

        public ObjectIdentifier[] Oids { get; }
        public Func<CdcStatusSnapshot, ISnmpData>[] Getters { get; }
    }

    // ---------------------------------------------------------------
    // Lifecycle
    // ---------------------------------------------------------------

    /// <summary>
    /// Opens UDP 161 and starts answering. Returns false if no socket
    /// could be opened (port busy, e.g. the Windows "SNMP Service").
    /// </summary>
    public bool Start()
    {
        lock (_lock)
        {
            if (_cts != null)
                return _sockets.Count > 0;

            try
            {
                _ = IpisTrapSender.Instance;
            }
            catch (Exception ex)
            {
                Log("sender", $"[SNMP-AGENT] IpisTrapSender not available: {ex.GetType().Name}: {ex.Message}");
            }

            // Give the trap sender the same live values, so traps and GET
            // agree (only if nobody has set its StatusProvider already).
            try
            {
                if (IpisTrapSender.Instance.StatusProvider == null)
                    IpisTrapSender.Instance.StatusProvider = GetLiveSnapshot;
            }
            catch
            {
                // sender not available; GET still works
            }

            _engineBoots = ReadEngineBoots(_assetId);
            _engineStartUtc = DateTime.UtcNow;

            var cts = new CancellationTokenSource();
            var token = cts.Token;

            foreach (var ip in GetBindAddresses())
            {
                try
                {
                    var udp = CreateSocket(ip);
                    _sockets.Add(udp);
                    _ = Task.Run(() => ReceiveLoopAsync(udp, token));
                    Log(null, $"[SNMP-AGENT] Listening on {ip}:{_port} (SNMPv3 user '{_o.Username}', engineBoots {_engineBoots}).");
                }
                catch (SocketException ex) when (ex.SocketErrorCode is SocketError.AddressAlreadyInUse or SocketError.AccessDenied)
                {
                    Log(null, $"[SNMP-AGENT] UDP {ip}:{_port} is already in use. " +
                              "Stop the Windows 'SNMP Service' (services.msc) or find the owner with: netstat -ano -p udp | findstr :161");
                }
                catch (Exception ex)
                {
                    Log(null, $"[SNMP-AGENT] Cannot listen on {ip}:{_port}: {ex.GetType().Name}: {ex.Message}");
                }
            }

            if (_sockets.Count == 0)
            {
                cts.Dispose();
                Log(null, "[SNMP-AGENT] Not started: no UDP socket could be opened.");
                return false;
            }

            _cts = cts;
            return true;
        }
    }

    public void Stop()
    {
        CancellationTokenSource? cts;
        List<UdpClient> sockets;

        lock (_lock)
        {
            cts = _cts;
            _cts = null;
            sockets = _sockets.ToList();
            _sockets.Clear();
        }

        if (cts == null)
            return;

        try { cts.Cancel(); } catch { /* ignore */ }

        foreach (var s in sockets)
        {
            try { s.Dispose(); } catch { /* ignore */ }
        }

        cts.Dispose();
        Log(null, "[SNMP-AGENT] Stopped.");
    }

    public void Dispose() => Stop();

    private List<IPAddress> GetBindAddresses()
    {
        var list = new List<IPAddress>();

        if (_bindIp != null)
        {
            list.Add(IPAddress.Parse(_bindIp));
            return list;
        }

        list.Add(IPAddress.Loopback);

        try
        {
            foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus != OperationalStatus.Up)
                    continue;

                foreach (var ua in nic.GetIPProperties().UnicastAddresses)
                {
                    var ip = ua.Address;
                    if (ip.AddressFamily == AddressFamily.InterNetwork &&
                        !IPAddress.IsLoopback(ip) &&
                        !list.Contains(ip))
                    {
                        list.Add(ip);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Log(null, $"[SNMP-AGENT] Address scan failed, using 0.0.0.0: {ex.Message}");
            list.Clear();
            list.Add(IPAddress.Any);
        }

        return list;
    }

    private UdpClient CreateSocket(IPAddress ip)
    {
        var udp = new UdpClient(AddressFamily.InterNetwork);

        try
        {
            if (OperatingSystem.IsWindows())
            {
                try
                {
                    udp.Client.IOControl(SioUdpConnReset, new byte[4], null);
                }
                catch
                {
                    // Not critical.
                }
            }

            udp.Client.Bind(new IPEndPoint(ip, _port));
            return udp;
        }
        catch
        {
            udp.Dispose();
            throw;
        }
    }

    private async Task ReceiveLoopAsync(UdpClient udp, CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            UdpReceiveResult rx;

            try
            {
                rx = await udp.ReceiveAsync(ct).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (ObjectDisposedException)
            {
                break;
            }
            catch (SocketException ex)
            {
                if (ct.IsCancellationRequested)
                    break;

                Log("rx", $"[SNMP-AGENT] Receive error: {ex.SocketErrorCode} {ex.Message}");
                continue;
            }

            byte[]? reply = null;

            try
            {
                reply = HandlePacket(rx.Buffer, rx.RemoteEndPoint);
            }
            catch (Exception ex)
            {
                Log("bad:" + rx.RemoteEndPoint.Address,
                    $"[SNMP-AGENT] Bad packet from {rx.RemoteEndPoint}: {ex.GetType().Name}: {ex.Message}");
            }

            if (reply == null)
                continue;

            try
            {
                await udp.SendAsync(reply, reply.Length, rx.RemoteEndPoint).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (ct.IsCancellationRequested || ex is ObjectDisposedException)
                    break;

                Log("tx", $"[SNMP-AGENT] Send error to {rx.RemoteEndPoint}: {ex.GetType().Name}: {ex.Message}");
            }
        }
    }

    // ---------------------------------------------------------------
    // SNMPv3 message processing (RFC 3412 / RFC 3414)
    // ---------------------------------------------------------------

    private byte[]? HandlePacket(byte[] buffer, IPEndPoint remote)
    {
        // 1. Outer message: only SNMPv3 is answered (Annexure E Sec. 1).
        if (DataFactory.CreateSnmpData(buffer) is not Sequence body ||
            body.Length < 1 ||
            body[0] is not Integer32 version)
        {
            return null;
        }

        if (version.ToInt32() != (int)VersionCode.V3 || body.Length != 4)
        {
            Log("v12:" + remote.Address,
                $"[SNMP-AGENT] Ignored SNMP v1/v2c request from {remote}. Annexure E requires SNMPv3 - set Edge NMS credential to v3 authPriv.");
            return null;
        }

        var header = new Header(body[1]);

        if (body[2] is not OctetString rawParameters)
            return null;

        var sp = new SecurityParameters(rawParameters);
        bool reportable = (header.SecurityLevel & Levels.Reportable) == Levels.Reportable;
        int requestId = TryGetPlainRequestId(body[3]);
        OctetString contextName = TryGetPlainContextName(body[3]);
        OctetString user = sp.UserName ?? OctetString.Empty;

        // 2. Engine discovery, or a stale engine ID cached by the NMS.
        if (sp.EngineId == null || sp.EngineId.GetRaw().Length == 0 || sp.EngineId != _engineId)
        {
            int c = Interlocked.Increment(ref _unknownEngineIds);
            if (sp.EngineId != null && sp.EngineId.GetRaw().Length > 0)
            {
                Log("eng:" + remote.Address,
                    $"[SNMP-AGENT] {remote} used a different engine ID ({sp.EngineId.ToHexString()}); sending discovery REPORT with {_engineId.ToHexString()}.");
            }

            return reportable
                ? BuildPlainReport(header, user, requestId, contextName, UsmStatsUnknownEngineIds, c)
                : null;
        }

        // 3. User name.
        if (user.GetRaw().Length == 0 || _users.Find(user) == null)
        {
            int c = Interlocked.Increment(ref _unknownUserNames);
            Log("usr:" + remote.Address,
                $"[SNMP-AGENT] {remote} unknown user '{user}'. Expected '{_o.Username}' (Config TrapUsername).");
            return reportable
                ? BuildPlainReport(header, user, requestId, contextName, UsmStatsUnknownUserNames, c)
                : null;
        }

        // 4. Security level.
        var level = header.SecurityLevel & (Levels.Authentication | Levels.Privacy);
        bool hasAuth = (level & Levels.Authentication) == Levels.Authentication;
        bool hasPriv = (level & Levels.Privacy) == Levels.Privacy;

        if (!hasAuth || (RequireAuthPriv && !hasPriv))
        {
            int c = Interlocked.Increment(ref _unsupportedSecLevels);
            Log("lvl:" + remote.Address,
                $"[SNMP-AGENT] {remote} security level {level} rejected. Use authPriv ({_o.AuthProtocol}/{_o.PrivProtocol}).");
            return reportable
                ? BuildPlainReport(header, user, requestId, contextName, UsmStatsUnsupportedSecLevels, c)
                : null;
        }

        // 5. Authenticate + decrypt.
        ISnmpMessage request;

        try
        {
            var messages = MessageFactory.ParseMessages(buffer, 0, buffer.Length, _users);
            if (messages.Count == 0)
                return null;

            request = messages[0];
        }
        catch (Exception ex)
        {
            int c = Interlocked.Increment(ref _decryptionErrors);
            Log("dec:" + remote.Address,
                $"[SNMP-AGENT] {remote} decrypt/parse failed ({ex.GetType().Name}). Check the privacy password/protocol ({_o.PrivProtocol}).");
            return reportable
                ? BuildPlainReport(header, user, requestId, contextName, UsmStatsDecryptionErrors, c)
                : null;
        }

        if (request is MalformedMessage)
        {
            int c = Interlocked.Increment(ref _decryptionErrors);
            Log("dec:" + remote.Address,
                $"[SNMP-AGENT] {remote} decryption failed. Check the privacy password/protocol ({_o.PrivProtocol}).");
            return reportable
                ? BuildPlainReport(header, user, requestId, contextName, UsmStatsDecryptionErrors, c)
                : null;
        }

        if (request.Parameters.IsInvalid)
        {
            int c = Interlocked.Increment(ref _wrongDigests);
            Log("dig:" + remote.Address,
                $"[SNMP-AGENT] {remote} wrong digest. Check the authentication password/protocol ({_o.AuthProtocol}).");
            return reportable
                ? BuildPlainReport(header, user, requestId, contextName, UsmStatsWrongDigests, c)
                : null;
        }

        // 6. Timeliness (only after authentication succeeded).
        int now = EngineTime;
        int msgBoots = request.Parameters.EngineBoots?.ToInt32() ?? -1;
        int msgTime = request.Parameters.EngineTime?.ToInt32() ?? -1;

        if (msgBoots != _engineBoots || Math.Abs((long)msgTime - now) > TimeWindowSeconds)
        {
            int c = Interlocked.Increment(ref _notInTimeWindows);
            return reportable
                ? BuildAuthenticatedReport(header, user, requestId, contextName, UsmStatsNotInTimeWindows, c)
                : null;
        }

        // 7. PDU dispatch.
        return Dispatch(request, level);
    }

    private byte[]? Dispatch(ISnmpMessage request, Levels level)
    {
        var pdu = request.Pdu();
        var reqVars = pdu.Variables;
        var table = _table;

        CdcStatusSnapshot? snapshot = null;
        CdcStatusSnapshot Snap() => snapshot ??= GetSnapshot();

        ErrorCode error = ErrorCode.NoError;
        int errorIndex = 0;
        List<Variable> vars;
        bool isBulk = false;

        switch (pdu.TypeCode)
        {
            case SnmpType.GetRequestPdu:
                vars = new List<Variable>(reqVars.Count);
                for (int i = 0; i < reqVars.Count; i++)
                {
                    var v = GetExact(table, reqVars[i].Id, Snap, out bool failed);
                    if (failed)
                    {
                        error = ErrorCode.GenError;
                        errorIndex = i + 1;
                        vars = reqVars.ToList();
                        break;
                    }
                    vars.Add(v);
                }
                break;

            case SnmpType.GetNextRequestPdu:
                vars = new List<Variable>(reqVars.Count);
                for (int i = 0; i < reqVars.Count; i++)
                {
                    var v = GetNext(table, reqVars[i].Id, Snap, out bool failed);
                    if (failed)
                    {
                        error = ErrorCode.GenError;
                        errorIndex = i + 1;
                        vars = reqVars.ToList();
                        break;
                    }
                    vars.Add(v);
                }
                break;

            case SnmpType.GetBulkRequestPdu:
                isBulk = true;
                vars = GetBulk(
                    table,
                    reqVars,
                    pdu.ErrorStatus.ToInt32(),   // non-repeaters
                    pdu.ErrorIndex.ToInt32(),    // max-repetitions
                    Snap,
                    out bool bulkFailed,
                    out int bulkFailedIndex);
                if (bulkFailed)
                {
                    error = ErrorCode.GenError;
                    errorIndex = bulkFailedIndex;
                    vars = reqVars.ToList();
                    isBulk = false;
                }
                break;

            case SnmpType.SetRequestPdu:
                vars = reqVars.ToList();
                error = ErrorCode.NotWritable;
                errorIndex = reqVars.Count > 0 ? 1 : 0;
                break;

            default:
                // Traps, informs, responses, reports are not for a command responder.
                return null;
        }

        int limit = Math.Min(Math.Max(MinMessageSize, request.Header.MaxSize), MaxUdpPayload);
        byte[] bytes = BuildResponse(request, level, error, errorIndex, vars);

        if (bytes.Length > limit && isBulk)
        {
            while (bytes.Length > limit && vars.Count > 1)
            {
                vars = vars.Take(vars.Count / 2).ToList();
                bytes = BuildResponse(request, level, error, errorIndex, vars);
            }
        }

        if (bytes.Length > limit)
            bytes = BuildResponse(request, level, ErrorCode.TooBig, 0, new List<Variable>());

        return bytes;
    }

    private static Variable GetExact(
        OidTable table,
        ObjectIdentifier oid,
        Func<CdcStatusSnapshot> snap,
        out bool failed)
    {
        failed = false;
        int idx = Array.BinarySearch(table.Oids, oid);

        if (idx >= 0)
            return Read(table, idx, snap, out failed);

        // Known object but wrong instance (e.g. missing ".0") -> noSuchInstance.
        uint[] q = oid.ToNumerical();
        foreach (var known in table.Oids)
        {
            uint[] k = known.ToNumerical();
            if (k.Length > 1 && q.Length >= k.Length - 1 && StartsWith(q, k, k.Length - 1))
                return new Variable(oid, new NoSuchInstance());
        }

        return new Variable(oid, new NoSuchObject());
    }

    private static Variable GetNext(
        OidTable table,
        ObjectIdentifier oid,
        Func<CdcStatusSnapshot> snap,
        out bool failed)
    {
        failed = false;
        int idx = Array.BinarySearch(table.Oids, oid);
        int next = idx >= 0 ? idx + 1 : ~idx;

        if (next >= table.Oids.Length)
            return new Variable(oid, new EndOfMibView());

        return Read(table, next, snap, out failed);
    }

    private static List<Variable> GetBulk(
        OidTable table,
        IList<Variable> reqVars,
        int nonRepeaters,
        int maxRepetitions,
        Func<CdcStatusSnapshot> snap,
        out bool failed,
        out int failedIndex)
    {
        failed = false;
        failedIndex = 0;

        int n = Math.Clamp(nonRepeaters, 0, reqVars.Count);
        int m = Math.Clamp(maxRepetitions, 0, MaxBulkRepetitions);
        var result = new List<Variable>();

        for (int i = 0; i < n; i++)
        {
            var v = GetNext(table, reqVars[i].Id, snap, out failed);
            if (failed)
            {
                failedIndex = i + 1;
                return result;
            }
            result.Add(v);
        }

        int repeaters = reqVars.Count - n;
        if (repeaters <= 0 || m == 0)
            return result;

        var current = new ObjectIdentifier[repeaters];
        for (int j = 0; j < repeaters; j++)
            current[j] = reqVars[n + j].Id;

        for (int r = 0; r < m; r++)
        {
            bool allEnd = true;

            for (int j = 0; j < repeaters; j++)
            {
                var v = GetNext(table, current[j], snap, out failed);
                if (failed)
                {
                    failedIndex = n + j + 1;
                    return result;
                }

                result.Add(v);
                current[j] = v.Id;

                if (v.Data is not EndOfMibView)
                    allEnd = false;

                if (result.Count >= MaxBulkVarbinds)
                    return result;
            }

            if (allEnd)
                break;
        }

        return result;
    }

    private static Variable Read(
        OidTable table,
        int index,
        Func<CdcStatusSnapshot> snap,
        out bool failed)
    {
        failed = false;
        var oid = table.Oids[index];

        try
        {
            var data = table.Getters[index](snap());
            return new Variable(oid, data ?? new NoSuchInstance());
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[SNMP-AGENT] Value error for {oid}: {ex.GetType().Name}: {ex.Message}");
            failed = true;
            return new Variable(oid, new NoSuchInstance());
        }
    }

    private static bool StartsWith(uint[] value, uint[] prefix, int prefixLength)
    {
        if (value.Length < prefixLength)
            return false;

        for (int i = 0; i < prefixLength; i++)
        {
            if (value[i] != prefix[i])
                return false;
        }

        return true;
    }

    // ---------------------------------------------------------------
    // Message builders
    // ---------------------------------------------------------------

    private byte[] BuildResponse(
        ISnmpMessage request,
        Levels level,
        ErrorCode error,
        int errorIndex,
        IList<Variable> vars)
    {
        bool priv = (level & Levels.Privacy) == Levels.Privacy;
        var privacy = priv ? _privacy : _authOnly;

        var header = new Header(
            new Integer32(request.MessageId()),
            new Integer32(MaxUdpPayload),
            level); // no Reportable flag on a Response

        var parameters = new SecurityParameters(
            _engineId,
            new Integer32(_engineBoots),
            new Integer32(EngineTime),
            request.Parameters.UserName,
            _auth.CleanDigest,
            priv ? privacy.Salt : OctetString.Empty);

        var scope = new Scope(
            _engineId,
            request.Scope.ContextName ?? OctetString.Empty,
            new ResponsePdu(request.RequestId(), error, errorIndex, vars));

        return new ResponseMessage(
            VersionCode.V3,
            header,
            parameters,
            scope,
            privacy,
            true,
            null).ToBytes();
    }

    // Unauthenticated REPORT: discovery, unknown user, wrong level, bad digest, decrypt error.
    private byte[] BuildPlainReport(
        Header requestHeader,
        OctetString user,
        int requestId,
        OctetString contextName,
        ObjectIdentifier counterOid,
        int counter)
    {
        var header = new Header(
            new Integer32(requestHeader.MessageId),
            new Integer32(MaxUdpPayload),
            (Levels)0);

        var parameters = new SecurityParameters(
            _engineId,
            new Integer32(_engineBoots),
            new Integer32(EngineTime),
            user,
            OctetString.Empty,
            OctetString.Empty);

        var scope = new Scope(
            _engineId,
            contextName,
            new ReportPdu(
                requestId,
                ErrorCode.NoError,
                0,
                new List<Variable> { new(counterOid, new Counter32(unchecked((uint)counter))) }));

        return new ReportMessage(
            VersionCode.V3,
            header,
            parameters,
            scope,
            DefaultPrivacyProvider.DefaultPair,
            null).ToBytes();
    }

    // Authenticated REPORT (notInTimeWindow) so the manager can trust our boots/time.
    private byte[] BuildAuthenticatedReport(
        Header requestHeader,
        OctetString user,
        int requestId,
        OctetString contextName,
        ObjectIdentifier counterOid,
        int counter)
    {
        var header = new Header(
            new Integer32(requestHeader.MessageId),
            new Integer32(MaxUdpPayload),
            Levels.Authentication);

        var parameters = new SecurityParameters(
            _engineId,
            new Integer32(_engineBoots),
            new Integer32(EngineTime),
            user,
            _auth.CleanDigest,
            OctetString.Empty);

        var scope = new Scope(
            _engineId,
            contextName,
            new ReportPdu(
                requestId,
                ErrorCode.NoError,
                0,
                new List<Variable> { new(counterOid, new Counter32(unchecked((uint)counter))) }));

        return new ReportMessage(
            VersionCode.V3,
            header,
            parameters,
            scope,
            _authOnly,
            null).ToBytes();
    }

    private static int TryGetPlainRequestId(ISnmpData scopedPdu)
    {
        try
        {
            if (scopedPdu is Sequence seq)
                return new Scope(seq).Pdu.RequestId.ToInt32();
        }
        catch
        {
            // Encrypted or malformed: request-id unknown.
        }

        return 0;
    }

    private static OctetString TryGetPlainContextName(ISnmpData scopedPdu)
    {
        try
        {
            if (scopedPdu is Sequence seq)
                return new Scope(seq).ContextName ?? OctetString.Empty;
        }
        catch
        {
            // ignore
        }

        return OctetString.Empty;
    }

    // ---------------------------------------------------------------
    // Values
    // ---------------------------------------------------------------

    private CdcStatusSnapshot GetSnapshot()
    {
        try
        {
            Func<CdcStatusSnapshot>? provider = StatusProvider;

            if (provider == null)
            {
                try { provider = IpisTrapSender.Instance.StatusProvider; }
                catch { provider = null; }
            }

            return provider?.Invoke() ?? GetLiveSnapshot();
        }
        catch (Exception ex)
        {
            Log("snap", $"[SNMP-AGENT] StatusProvider error: {ex.GetType().Name}: {ex.Message}");
            return GetLiveSnapshot();
        }
    }

    // Real values measured here. Cached 2 s because Edge NMS polls each
    // OID separately and the audio probe is a COM call.
    private CdcStatusSnapshot GetLiveSnapshot()
    {
        lock (_snapLock)
        {
            if (_cachedSnapshot != null &&
                DateTime.UtcNow - _cachedSnapshotUtc < SnapshotCacheTime)
            {
                return _cachedSnapshot;
            }
        }

        var audio = ProbeAudio(out string audioDetail);
        var now = DateTime.UtcNow;

        CdcStatusSnapshot snap;
        lock (_ntesLock)
        {
            snap = new CdcStatusSnapshot
            {
                // No current sensor on the CDC PC: stays 0 unless a
                // StatusProvider supplies a measured value.
                LoadCurrentMa = 0,
                Audio = audio,
                AudioLastTestUtc = now,
                NtesLastSyncUtc = _ntesLastOkUtc,
                NtesSyncOk = _ntesLastOk == true,
                NtesSyncLog = _ntesLog,
                SoftwareBuild = _softwareBuild
            };
        }

        if (audio != AudioStatus.Ok)
            Log("aud", $"[SNMP-AGENT] Audio output {audio}: {audioDetail}");

        lock (_snapLock)
        {
            _cachedSnapshot = snap;
            _cachedSnapshotUtc = now;
        }

        return snap;
    }

    // ok(1) active and not muted, muted(2) muted or volume 0,
    // noSignal(3) speaker/headphone unplugged, hwFault(4) no usable device.
    private static AudioStatus ProbeAudio(out string detail)
    {
        try
        {
            using var enumerator = new NAudio.CoreAudioApi.MMDeviceEnumerator();

            if (!enumerator.HasDefaultAudioEndpoint(
                    NAudio.CoreAudioApi.DataFlow.Render,
                    NAudio.CoreAudioApi.Role.Multimedia))
            {
                int unplugged = 0;
                try
                {
                    unplugged = enumerator.EnumerateAudioEndPoints(
                        NAudio.CoreAudioApi.DataFlow.Render,
                        NAudio.CoreAudioApi.DeviceState.Unplugged).Count;
                }
                catch
                {
                    // ignore
                }

                if (unplugged > 0)
                {
                    detail = "audio output unplugged";
                    return AudioStatus.NoSignal;
                }

                detail = "no default audio output device";
                return AudioStatus.HwFault;
            }

            using var device = enumerator.GetDefaultAudioEndpoint(
                NAudio.CoreAudioApi.DataFlow.Render,
                NAudio.CoreAudioApi.Role.Multimedia);

            if (device.State == NAudio.CoreAudioApi.DeviceState.Unplugged)
            {
                detail = $"{device.FriendlyName} unplugged";
                return AudioStatus.NoSignal;
            }

            if (device.State != NAudio.CoreAudioApi.DeviceState.Active)
            {
                detail = $"{device.FriendlyName} state = {device.State}";
                return AudioStatus.HwFault;
            }

            try
            {
                var volume = device.AudioEndpointVolume;
                if (volume.Mute || volume.MasterVolumeLevelScalar <= 0.001f)
                {
                    detail = $"{device.FriendlyName} muted / volume 0";
                    return AudioStatus.Muted;
                }
            }
            catch
            {
                // volume not readable: device is still active
            }

            detail = device.FriendlyName;
            return AudioStatus.Ok;
        }
        catch (Exception ex)
        {
            detail = $"audio probe failed: {ex.Message}";
            return AudioStatus.HwFault;
        }
    }

    // Worst current condition wins.
    private Health GetHealth(CdcStatusSnapshot s)
    {
        if (HealthProvider != null)
        {
            try
            {
                return HealthProvider();
            }
            catch
            {
                // fall back to the computed value
            }
        }

        bool ntesFailed;
        lock (_ntesLock)
            ntesFailed = _ntesLastOk == false;

        var health = Health.Ok;

        if (s.Audio == AudioStatus.Muted)
            health = Health.Warning;

        if (s.Audio is AudioStatus.NoSignal or AudioStatus.HwFault)
            health = Health.Major;           // same severity as the audio failure trap

        if (ntesFailed && health < Health.Major)
            health = Health.Major;           // same severity as the NTES failure trap

        if (ntesFailed && (s.Audio is AudioStatus.NoSignal or AudioStatus.HwFault))
            health = Health.Critical;        // announcements and train data both down

        return health;
    }

    private static string ReadSoftwareBuild()
    {
        // Same source as ucHelp ("APP VERSION : {APP_VERSION}" in MyHelp2.xml):
        // the exe FileVersion, set by <FileVersion> in IPISv2.csproj.
        try
        {
            string? fileVersion = FileVersionInfo
                .GetVersionInfo(Application.ExecutablePath)
                .FileVersion;

            if (!string.IsNullOrWhiteSpace(fileVersion))
                return Truncate(fileVersion.Trim(), 255);
        }
        catch
        {
            // fall through
        }

        try
        {
            return System.Reflection.Assembly.GetEntryAssembly()?
                .GetName().Version?.ToString() ?? "0.0.0";
        }
        catch
        {
            return "0.0.0";
        }
    }

    private uint UptimeTicks()
    {
        double ticks = (DateTime.UtcNow - _createdUtc).TotalMilliseconds / 10;
        return (uint)Math.Clamp(ticks, 0, uint.MaxValue);
    }

    private int EngineTime
    {
        get
        {
            double s = (DateTime.UtcNow - _engineStartUtc).TotalSeconds;
            return (int)Math.Clamp(s, 0, int.MaxValue);
        }
    }

    // ---------------------------------------------------------------
    // SNMPv3 providers / engine ID (same rules as IpisTrapSender)
    // ---------------------------------------------------------------

    private static IAuthenticationProvider BuildAuthProvider(string? authProtocol, string authPassword)
    {
        string protocol = (authProtocol ?? "SHA")
            .Replace("-", "")
            .Replace("(", "")
            .Replace(")", "")
            .ToUpperInvariant();

        var password = new OctetString(authPassword);

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

        throw new NotSupportedException($"Unsupported authentication protocol: {authProtocol}");
    }

    private static IPrivacyProvider BuildPrivProvider(
        string? privProtocol,
        string privPassword,
        IAuthenticationProvider auth)
    {
        string protocol = (privProtocol ?? "AES-128")
            .Replace("-", "")
            .ToUpperInvariant();

        var password = new OctetString(privPassword);

        if (protocol.Contains("AES256"))
            return new AES256PrivacyProvider(password, auth);

        if (protocol.Contains("AES192"))
            return new AES192PrivacyProvider(password, auth);

        if (protocol is "AES" or "AES128")
            return new AESPrivacyProvider(password, auth);

        if (protocol == "DES")
            return new DESPrivacyProvider(password, auth);

        throw new NotSupportedException($"Unsupported privacy protocol: {privProtocol}");
    }

    private static OctetString BuildEngineId(uint enterprise, string seed)
    {
        byte[] enterpriseBytes =
        {
            (byte)(enterprise >> 24),
            (byte)(enterprise >> 16),
            (byte)(enterprise >> 8),
            (byte)enterprise
        };

        enterpriseBytes[0] |= 0x80;

        using var sha256 = SHA256.Create();
        byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(seed));

        var engineId = new byte[25];
        Buffer.BlockCopy(enterpriseBytes, 0, engineId, 0, 4);
        engineId[4] = 0x05;
        Buffer.BlockCopy(hash, 0, engineId, 5, 20);

        return new OctetString(engineId);
    }

    // Reads (does not increment) the boots counter IpisTrapSender maintains.
    private static int ReadEngineBoots(string assetId)
    {
        try
        {
            string safeAsset = string.Concat(
                assetId.Select(c => char.IsLetterOrDigit(c) || c is '-' or '_' ? c : '_'));

            string path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "Ipis",
                $"snmp-engineboots-{safeAsset}.txt");

            if (File.Exists(path) &&
                uint.TryParse(File.ReadAllText(path).Trim(), out uint boots) &&
                boots > 0)
            {
                return (int)Math.Min(boots, (uint)int.MaxValue);
            }
        }
        catch
        {
            // fall through
        }

        return 1;
    }

    // ---------------------------------------------------------------
    // Helpers
    // ---------------------------------------------------------------

    // ipisDeviceAssetID for 2.1.0, taken from App.config by role
    // (GlobalData._lstAdminSetting.Ismaster, via IpisTrapOptions.LocalIp):
    //   10.0.0.253 (master)  -> TrapAssetIdMaster
    //   10.0.0.254 (standby) -> TrapAssetIdClient
    // Read on every GET, so a master/standby switch shows at once.
    // Engine ID, boots file and traps are not affected.
    private string ReportedAssetId()
    {
        string key = _o.LocalIp == IpisTrapOptions.MasterIp
            ? "TrapAssetIdMaster"
            : "TrapAssetIdClient";

        try
        {
            string? value = System.Configuration.ConfigurationManager.AppSettings[key];
            if (!string.IsNullOrWhiteSpace(value))
                return value.Trim();
        }
        catch
        {
            // fall back below
        }

        string fallback = _o.AssetId;
        return string.IsNullOrWhiteSpace(fallback) ? "CDC-DEFAULT" : fallback;
    }

    private static string Ts(DateTime value) =>
        value.ToUniversalTime().ToString(
            "yyyy-MM-dd'T'HH:mm:ss'Z'",
            System.Globalization.CultureInfo.InvariantCulture);

    private static string Truncate(string? value, int maxLength)
    {
        value ??= string.Empty;
        return value.Length <= maxLength ? value : value[..maxLength];
    }

    // key == null: always log. Otherwise at most once per 60 s per key.
    private void Log(string? key, string message)
    {
        if (key != null)
        {
            lock (_logLock)
            {
                var now = DateTime.UtcNow;
                if (_lastLog.TryGetValue(key, out var last) && (now - last).TotalSeconds < 60)
                    return;
                _lastLog[key] = now;
            }
        }

        Console.WriteLine(message);
        Debug.WriteLine(message);

        try
        {
            LogMessage?.Invoke(message);
        }
        catch
        {
            // never let logging break the agent
        }
    }
}
