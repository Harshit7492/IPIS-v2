using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text.Json;
using LogManager;

namespace Helper
{
    public sealed class LicenseResult
    {
        public bool IsValid { get; init; }
        public string Message { get; init; } = "";
        public string StationName { get; init; } = "";
        public string StationCode { get; init; } = "";
    }

    /// <summary>
    /// Checks license.dat: RSA-PSS-SHA256 signature first, then that a MAC of this PC is in the license.
    /// </summary>
    public static class LicenseManager
    {
        // Vardaan license PUBLIC key (license-public.pem). Built into the exe so it cannot be swapped on site.
        private const string PublicKeyPem = @"-----BEGIN PUBLIC KEY-----
MIIBojANBgkqhkiG9w0BAQEFAAOCAY8AMIIBigKCAYEAnGJzHOgZuk/ZVAmSfU5Z
GFaVMHf7uMyaTIlowveQwG3Fp4fj8r/9pITOOg+w5GkiME5ECga6KalYXOdar77n
EoZ4Rl7gA2kzTQYoBvKNXy3wFj/mTxiUP4dbG/+J8DpsfbrPTHI2JFDR9W6N2aX/
nq2lQ48USu5cZgJR81mzkwFoD/z6e5lg2Z7JZVA82yVmA5EFcW0YfIOus2orL6zv
fv/q/04V2G9Dkbr+jq4M+6fgkTX5epApyWyfMj92+4FVArkEsddqvJbpZwSSc0l+
YNiQPocFpyYkTRE2qfxqu/oHNaBnqx4rtUbKujbe8oHMq9JPBtGpaiUV1goCgy9y
sa4ahDcXfTtEVMZGW60pR9LhpFm9YUIo0rc7sWR7blFeJqjO/fglALQUHxlfI8yf
R71g8unT4c7uGz0aiaPudYE0BXuu9PDnJP9pxjzzbtwJEtqVrsnHDf/uyHmiAwTs
GviH7HanqM7ygGhPYgUFWOxVOyJ7MRrIs/sXrAjZmhY1AgMBAAE=
-----END PUBLIC KEY-----";

      
        public static string LicensePath
        {
            get
            {
                string configured = string.IsNullOrWhiteSpace(Config.LicenseFilePath)
                    ? @"license.dat"
                    : Config.LicenseFilePath.Trim();

                return Path.IsPathRooted(configured)
                    ? configured
                    : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configured);
            }
        }

        public static string LicenseFolder => Path.GetDirectoryName(LicensePath);

        /// <summary>Checks the license file set in App.config (LicenseFilePath).</summary>
        public static LicenseResult Validate() => ValidateFile(LicensePath);

        public static LicenseResult ValidateFile(string path)
        {
            try
            {
                if (!File.Exists(path))
                    return Fail("License file not found: " + path);

                using JsonDocument outer = JsonDocument.Parse(File.ReadAllText(path));
                JsonElement root = outer.RootElement;

                if (GetString(root, "format") != "MAC-LICENSE-SIGNED")
                    return Fail("This is not an IPISv2 license file.");
                if (GetString(root, "algorithm") != "RSA-PSS-SHA256")
                    return Fail("Unsupported license algorithm.");

                byte[] payload = Convert.FromBase64String(GetString(root, "payloadBase64"));
                byte[] signature = Convert.FromBase64String(GetString(root, "signatureBase64"));

                // 1. Signature: the payload must be exactly what Vardaan signed.
                using (RSA rsa = RSA.Create())
                {
                    rsa.ImportFromPem(PublicKeyPem);
                    if (!rsa.VerifyData(payload, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pss))
                        return Fail("License signature is not valid (the file was changed or not issued by Vardaan).");
                }

                // 2. Read the signed payload (only after the signature is OK).
                using JsonDocument inner = JsonDocument.Parse(payload);
                JsonElement data = inner.RootElement;

                if (GetString(data, "format") != "MAC-LICENSE")
                    return Fail("License payload format is not supported.");

                string stationName = GetString(data, "stationName");
                string stationCode = GetString(data, "stationCode");

                HashSet<string> licensedMacs = new HashSet<string>();
                if (data.TryGetProperty("macAddresses", out JsonElement macs) && macs.ValueKind == JsonValueKind.Array)
                {
                    foreach (JsonElement mac in macs.EnumerateArray())
                        licensedMacs.Add(NormalizeMac(mac.GetString()));
                }

                // 3. MAC: at least one network card of this PC must be in the license.
                string matched = GetLocalMacs().FirstOrDefault(m => licensedMacs.Contains(NormalizeMac(m)));
                if (matched == null)
                    return Fail($"This PC is not licensed for {stationName} ({stationCode}). None of its network card addresses is in the license.");

                LoggerService.LicenseLogger.Information(
                    "License valid: {Station} ({Code}), matched MAC {Mac}.", stationName, stationCode, matched);

                return new LicenseResult
                {
                    IsValid = true,
                    Message = "License valid.",
                    StationName = stationName,
                    StationCode = stationCode
                };
            }
            catch (Exception ex)
            {
                LoggerService.LicenseLogger.Error(ex, "License file could not be read: {Path}", path);
                return new LicenseResult { IsValid = false, Message = "License file could not be read: " + ex.Message };
            }
        }

       
        /// <summary>MACs of this PC's network cards, as 28:6B:35:0B:7E:6C. Disabled cards are not listed by Windows.</summary>
        public static List<string> GetLocalMacs()
        {
            var list = new List<string>();
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Loopback ||
                    nic.NetworkInterfaceType == NetworkInterfaceType.Tunnel)
                    continue;

                byte[] bytes = nic.GetPhysicalAddress().GetAddressBytes();
                if (bytes.Length != 6 || bytes.All(b => b == 0))
                    continue;

                string mac = string.Join(":", bytes.Select(b => b.ToString("X2")));
                if (!list.Contains(mac))
                    list.Add(mac);
            }
            return list;
        }

        private static string NormalizeMac(string mac) =>
            new string((mac ?? "").Where(Uri.IsHexDigit).ToArray()).ToUpperInvariant();

        private static string GetString(JsonElement element, string name) =>
            element.TryGetProperty(name, out JsonElement value) && value.ValueKind == JsonValueKind.String
                ? value.GetString() ?? ""
                : "";

        private static LicenseResult Fail(string message)
        {
            LoggerService.LicenseLogger.Warning("License check failed: {Reason}", message);
            LoggerService.LicenseLogger.Warning("MAC addresses of this PC: {Macs}", string.Join(", ", GetLocalMacs()));
            return new LicenseResult { IsValid = false, Message = message };
        }
    }
}