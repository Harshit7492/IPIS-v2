
namespace DataModel
{
    public class GetSetConfiguration
    {
        private sealed class CommandItem
        {
            public string Display { get; set; }   // text shown in the dropdown
            public string Code { get; set; }      // REST "command" value
            public byte HexOpcode { get; set; }   // legacy HEX opcode (placeholder values)

            public override string ToString() { return Display; }
        }

        private enum DeviceState { NotTested, PreviewReady, InvalidIp }

        private sealed class BoardItem
        {
            public int BoardId { get; set; }
            public int HubId { get; set; }
            public bool IsHub { get; set; }
            public string BoardType { get; set; }   // DISPLAY_TYPE, or BOARD_NAME if blank
            public string BoardName { get; set; }
            public string BoardIp { get; set; }
            public string PfNo { get; set; }

            public override string ToString()
            {
                if (IsHub)
                {
                    return !string.IsNullOrWhiteSpace(BoardName) ? BoardName : "PDC (" + BoardIp + ")";
                }
                string text = !string.IsNullOrWhiteSpace(BoardName) ? BoardName : BoardType;
                return text;
            }
        }

        private sealed class PdcItem
        {
            public int HubId { get; set; }
            public string NodeName { get; set; }
            public string HubIp { get; set; }
            public string PfNo { get; set; }
            public int Port { get; set; } = 8000;

            public override string ToString()
            {
                string cleanName = (NodeName ?? string.Empty).Trim();

                // Strip "Platform Data Controller", "Plateform data controller", "PlatformDataController", and "PDC"
                cleanName = cleanName.Replace("Platform Data Controller", "", StringComparison.OrdinalIgnoreCase);
                cleanName = cleanName.Replace("Plateform data controller", "", StringComparison.OrdinalIgnoreCase);
                cleanName = cleanName.Replace("Plateform Data Controller", "", StringComparison.OrdinalIgnoreCase);
                cleanName = cleanName.Replace("PlatformDataController", "", StringComparison.OrdinalIgnoreCase);
                cleanName = cleanName.Replace("PDC", "", StringComparison.OrdinalIgnoreCase);
                cleanName = cleanName.Trim('-', ' ', '_');

                string pf = !string.IsNullOrWhiteSpace(PfNo) ? PfNo.Trim() : string.Empty;
                string ip = !string.IsNullOrWhiteSpace(HubIp) ? HubIp.Trim() : string.Empty;

                if (!string.IsNullOrWhiteSpace(cleanName))
                {
                    if (!string.IsNullOrWhiteSpace(pf) && !cleanName.Contains(pf, StringComparison.OrdinalIgnoreCase))
                    {
                        string pfLabel = pf.StartsWith("Platform", StringComparison.OrdinalIgnoreCase) || pf.StartsWith("PF", StringComparison.OrdinalIgnoreCase)
                            ? pf
                            : "PF " + pf;
                        return cleanName + " (" + pfLabel + ") - " + ip;
                    }
                    return cleanName + " - " + ip;
                }

                if (!string.IsNullOrWhiteSpace(pf))
                {
                    string pfLabel = pf.StartsWith("Platform", StringComparison.OrdinalIgnoreCase)
                        ? pf
                        : (pf.StartsWith("PF", StringComparison.OrdinalIgnoreCase) ? pf : "Platform " + pf);
                    return pfLabel + " - " + ip;
                }

                return !string.IsNullOrWhiteSpace(ip) ? ip : "Unknown";
            }
        }

        private sealed class PdcDeviceItem
        {
            public string Ip { get; set; }
            public string Pf { get; set; }
            public string AssetId { get; set; }
            public string Int { get; set; }
            public string Dto { get; set; }
            public string Mode { get; set; }
            public bool IsAll { get; set; }

            public override string ToString()
            {
                if (IsAll) return "All Connected CGDBs (Broadcast)";
                string text = Ip;
                if (!string.IsNullOrWhiteSpace(AssetId)) text += " - " + AssetId;
                if (!string.IsNullOrWhiteSpace(Int) && !string.IsNullOrWhiteSpace(Dto))
                    text += " (INT: " + Int + ", DTO: " + Dto + ")";
                return text;
            }
        }

        private static readonly CommandItem[] Commands =
        {
            new CommandItem { Display = "Get Configuration",  Code = "GET_CONFIGURATION", HexOpcode = 0x10 },
            new CommandItem { Display = "Set Configuration",  Code = "SET_CONFIGURATION", HexOpcode = 0x11 },
            new CommandItem { Display = "Soft Reset",         Code = "SOFT_RESET",        HexOpcode = 0x40 },
        };


        
    
}
}
