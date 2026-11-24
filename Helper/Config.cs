
using System.Configuration;

namespace Helper
{
    public static class Config
    {
        public static int currentlyloggedInid { get; set; }

        public static bool old_Ann_Patrn { get; set; } = true;

        // SNMP Trap Configuration
        public static string TrapNmsIp { get; set; } = "10.0.0.252";
        public static int TrapPort { get; set; } = 162;
        public static string TrapLocalIp { get; set; } = "10.0.0.254";
        public static string TrapDeviceType { get; set; } = "CDC";
        public static string TrapAssetId { get; set; } = "CDC_TRAP_01";
        public static string TrapFirmwareVersion { get; set; } = "1.0.0";
        public static int TrapStatusIntervalSeconds { get; set; } = 0;

        // SNMPv3 Security Configuration
        public static string TrapUsername { get; set; } = "ipisuser";
        public static string TrapAuthPassword { get; set; } = "Vardaan!@#123";
        public static string TrapPrivPassword { get; set; } = "Vardaan!@#123";
        public static string TrapAuthProtocol { get; set; } = "SHA";
        public static string TrapPrivProtocol { get; set; } = "AES-128";

        // SNMP OID Configuration
        public static string TrapBaseOid { get; set; } = "1.3.6.1.4.1.99999";
        public static uint TrapEnterpriseNumber { get; set; } = 65319;

        // Primary and Secondary Server Configuration
        public static string MasterIP { get; set; } = "10.0.0.253";
        public static string SlaveIP { get; set; } = "10.0.0.254";
        public static int ServerPort { get; set; } = 31511;
        public static int TrapTimeoutMs { get; set; } = 3000;

        public static string SlaveMachineName { get; set; }

        // License file (App.config key "LicenseFilePath"): relative to the IPISv2.exe folder, or a full path
        public static string LicenseFilePath { get; set; } = @"license.dat";

        // Database Configuration
        public static int AvailableDataInDays { get; set; } = 10;
        public static string _dbPassword { get; set; } = "sapassword@123";
        public static string _dbSelfConnection { get; set; }
        public static string _dbRemoteConnection { get; set; }

        // Media Configuration
        public static string videopath { get; set; }


    
        public static void SetConfig()
        {
            // Media path
            videopath = GetSetting("videopath", videopath);

            // Announcement pattern
            string value = ConfigurationManager.AppSettings["old_Ann_Patrn"];

            if (bool.TryParse(value, out bool oldPattern))
            {
                old_Ann_Patrn = oldPattern;
            }

            // Secondary machine name
            SlaveMachineName = GetSetting( "SlaveMachineName", SlaveMachineName);

            // License file path
            LicenseFilePath = GetSetting("LicenseFilePath", LicenseFilePath);

            // Application server port
            value = ConfigurationManager.AppSettings["ServerPort"];

            if (int.TryParse(value, out int serverPort) &&
                serverPort >= 1 && serverPort <= 65535)
            {
                ServerPort = serverPort;
            }

            // SNMP NMS IP address
            TrapNmsIp = GetSetting( "TrapNmsIp", TrapNmsIp );

            // SNMP trap receiver port
            value = ConfigurationManager.AppSettings["TrapPort"];

            if (int.TryParse(value, out int trapPort) &&
                trapPort >= 1 && trapPort <= 65535)
            {
                TrapPort = trapPort;
            }

            // SNMP local IP address
            TrapLocalIp = GetSetting("TrapLocalIp", TrapLocalIp);

            // SNMP device type (CDC, CDS, PDC, etc.)
            TrapDeviceType = GetSetting("TrapDeviceType", TrapDeviceType);

            // SNMP asset ID
            TrapAssetId = GetSetting("TrapAssetId",TrapAssetId );

            // Firmware version
            TrapFirmwareVersion = GetSetting( "TrapFirmwareVersion", TrapFirmwareVersion);

            // SNMPv3 username
            TrapUsername = GetSetting( "TrapUsername", TrapUsername);

            // SNMPv3 authentication password
            TrapAuthPassword = GetSetting( "TrapAuthPassword", TrapAuthPassword, allowEmpty: true );

            // SNMPv3 privacy password
            TrapPrivPassword = GetSetting( "TrapPrivPassword", TrapPrivPassword, allowEmpty: true);

            // SNMPv3 authentication protocol
            TrapAuthProtocol = GetSetting( "TrapAuthProtocol", TrapAuthProtocol );

            // SNMPv3 privacy protocol
            TrapPrivProtocol = GetSetting("TrapPrivProtocol",TrapPrivProtocol);

            // SNMP enterprise/base OID
            TrapBaseOid = GetSetting( "TrapBaseOid", TrapBaseOid );

            value = ConfigurationManager.AppSettings[ "TrapEnterpriseNumber"];

            if (uint.TryParse(value, out uint enterpriseNumber))
            {
                TrapEnterpriseNumber = enterpriseNumber;
            }

            // SNMP trap timeout
            value = ConfigurationManager.AppSettings["TrapTimeoutMs"];

            if (int.TryParse(value, out int timeout) && timeout > 0)
            {
                TrapTimeoutMs = timeout;
            }

            // SNMP periodic status interval
            value = ConfigurationManager.AppSettings["TrapStatusIntervalSeconds"];

            if (int.TryParse(value, out int statusInterval) && statusInterval >= 0)
            {
                TrapStatusIntervalSeconds = statusInterval;
            }

            // Available historical data retention period
            value = ConfigurationManager.AppSettings[ "AvailableDataInDays"];

            if (int.TryParse(value, out int availableDays) &&
                availableDays > 0)
            {
                AvailableDataInDays = availableDays;
            }

            // Database connection strings
            _dbSelfConnection = "Data Source=" + Environment.MachineName + @"\SqlExpress;" + "Initial Catalog=IPISv2;" + "Integrated Security=False;" + "Persist Security Info=False;" +"Connect Timeout=8;" + "User ID=sa;" +"Password=" + _dbPassword;

            if (!string.IsNullOrWhiteSpace(SlaveMachineName))
            {
                _dbRemoteConnection = "Data Source=" + SlaveMachineName.Trim() + @"\SqlExpress;" + "Initial Catalog=IPISv2;" + "Integrated Security=False;" + "Persist Security Info=False;" + "Connect Timeout=8;" + "User ID=sa;" +"Password=" + _dbPassword;
            }
            else
            {
                _dbRemoteConnection = null;
            }
        }

        /// <summary>
        /// Reads a string setting while preserving its default value
        /// when the key is missing or empty.
        /// </summary>
        private static string GetSetting(string key,string defaultValue,bool allowEmpty = false)
        {
            string value = ConfigurationManager.AppSettings[key];

            if (value == null)
            {
                return defaultValue;
            }

            if (!allowEmpty && string.IsNullOrWhiteSpace(value))
            {
                return defaultValue;
            }

            return allowEmpty ? value : value.Trim();
        }
    }
}
