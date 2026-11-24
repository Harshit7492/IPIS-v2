using DataBaseHandler;
using DataModel;
using System.Net;
using System.Text;

namespace IPISv2
{
    public partial class GetSetConfigurations : UserControl
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
                return !string.IsNullOrWhiteSpace(BoardName) ? BoardName : BoardType;
            }
        }

        private static readonly CommandItem[] Commands =
        {
            new CommandItem { Display = "Get Configuration",  Code = "GET_CONFIGURATION", HexOpcode = 0x10 },
            new CommandItem { Display = "Set Configuration",  Code = "SET_CONFIGURATION", HexOpcode = 0x11 },
            new CommandItem { Display = "Soft Reset",         Code = "SOFT_RESET",        HexOpcode = 0x40 },
        };

        private const string NoResponseText = "No response — UI preview only. No command was sent.";
        private static readonly string[] IntValues = { "25", "50", "75", "100" };

        public GetSetConfigurations()
        {
            InitializeComponent();
            InitializeUi();
        }

        // ------------------------------------------------------------------
        // Initialisation
        // ------------------------------------------------------------------
        private void InitializeUi()
        {
            InitializeDisplayBoardUi();
        }

        private void InitializeDisplayBoardUi()
        {
            LoadBoardTypes();
            LoadIntValues();
            cmbDto.SelectedItem = "60";   // DTO default: 60 seconds
            LoadCommands();
            ResetOutput();
            SetDeviceStatus(DeviceState.NotTested);
        }

        private void LoadBoardTypes()
{
    cmbBoardType.Items.Clear();

    List<BoardSettingInfo> boards = _dbService.GET_BOARD_SETTING(0);

    if (boards != null)
    {
        foreach (BoardSettingInfo b in boards)
        {
            string displayType = b.DISPLAY_TYPE?.Trim() ?? string.Empty;
            string boardName = b.BOARD_NAME?.Trim() ?? string.Empty;

            // Exclude PDC, HUB and CGDB
            bool isExcluded =
                displayType.Contains("PDC", StringComparison.OrdinalIgnoreCase) ||
                boardName.Contains("PDC", StringComparison.OrdinalIgnoreCase) ||
                displayType.Contains("HUB", StringComparison.OrdinalIgnoreCase) ||
                boardName.Contains("HUB", StringComparison.OrdinalIgnoreCase) ||
                displayType.Contains("CGDB", StringComparison.OrdinalIgnoreCase) ||
                boardName.Contains("CGDB", StringComparison.OrdinalIgnoreCase) ||
                boardName.Contains("Coach Guidance Display Board", StringComparison.OrdinalIgnoreCase);

            if (isExcluded)
                continue;

            cmbBoardType.Items.Add(new BoardItem
            {
                BoardId = b.BOARD_ID,
                IsHub = false,
                BoardType = string.IsNullOrWhiteSpace(b.DISPLAY_TYPE)
                            ? b.BOARD_NAME
                            : b.DISPLAY_TYPE,
                BoardName = b.BOARD_NAME,
                BoardIp = b.BOARD_IP,
                PfNo = b.PF_NO
            });
        }
    }

    if (cmbBoardType.Items.Count == 0)
        cmbBoardType.Items.Add("(no boards returned from BOARDSETTING)");

    cmbBoardType.SelectedIndex = 0;
}
        private void LoadIntValues()
        {
            cmbInt.Items.Clear();
            foreach (string v in IntValues)
                cmbInt.Items.Add(v);
            cmbInt.SelectedIndex = 0;
        }

        private void LoadCommands()
        {
            cmbCommand.Items.Clear();
            foreach (CommandItem c in Commands)
                cmbCommand.Items.Add(c);
            cmbCommand.SelectedIndex = 0;
        }

        // ------------------------------------------------------------------
        // Event handlers
        // ------------------------------------------------------------------
        private void cmbBoardType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BoardItem board = cmbBoardType.SelectedItem as BoardItem;
            if (board != null && !string.IsNullOrWhiteSpace(board.BoardIp))
                txtDeviceIp.Text = board.BoardIp.Trim();

            ResetOutput();
            SetDeviceStatus(DeviceState.NotTested);
        }

        private void cmbCommand_SelectedIndexChanged(object sender, EventArgs e)
        {
            // INT / DTO inputs only apply to Set Configuration.
            CommandItem cmd = cmbCommand.SelectedItem as CommandItem;
            bool isSet = cmd != null && cmd.Code == "SET_CONFIGURATION";

            lblInt.Visible = isSet;
            cmbInt.Visible = isSet;
            lblDto.Visible = isSet;
            cmbDto.Visible = isSet;

            // Button name follows the command.
            if (cmd != null && cmd.Code == "GET_CONFIGURATION")
                btnPreview.Text = "Get Configuration";
            else if (isSet)
                btnPreview.Text = "Set Configuration";
            else if (cmd != null && cmd.Code == "SOFT_RESET")
                btnPreview.Text = "Soft Reset";
            else
                btnPreview.Text = "Preview Request";
        }

        private void txtDeviceIp_TextChanged(object sender, EventArgs e)
        {
            SetDeviceStatus(DeviceState.NotTested);
        }

        private void rbMode_CheckedChanged(object sender, EventArgs e)
        {
            // Fires for both radio buttons; only react to the one being selected.
            RadioButton rb = sender as RadioButton;
            if (rb == null || !rb.Checked) return;

            ResetOutput();
            SetDeviceStatus(DeviceState.NotTested);
        }

        private async void btnPreview_Click(object sender, EventArgs e)
        {
            CommandItem selectedCommand = cmbCommand.SelectedItem as CommandItem;
            if (selectedCommand != null && selectedCommand.Code == "SOFT_RESET")
            {
                RequestSoftReset();
                return;
            }

            string ip = txtDeviceIp.Text.Trim();
            if (!IsValidIPv4(ip))
            {
                SetDeviceStatus(DeviceState.InvalidIp);
                MessageBox.Show("Please enter a valid IPv4 address (e.g. 10.0.0.101).",
                    "Invalid IP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDeviceIp.Focus();
                txtDeviceIp.SelectAll();
                return;
            }

            CommandItem cmd = cmbCommand.SelectedItem as CommandItem;
            if (cmd == null) return;

            BoardItem selectedBoard = cmbBoardType.SelectedItem as BoardItem;
            string board = selectedBoard != null ? selectedBoard.BoardType : string.Empty;

            if (cmd.Code == "GET_CONFIGURATION")
            {
                await FetchConfigurationAsync(ip);
                return;
            }

            if (cmd.Code == "SET_CONFIGURATION")
            {
                await SendSetConfigurationAsync(ip);
                return;
            }

            // Anything else is preview only: nothing is transmitted to hardware.
            txtSent.Text = BuildRestRequest(board, ip, cmd);
            txtReceived.Text = NoResponseText;
            SetDeviceStatus(DeviceState.PreviewReady);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ResetOutput();
            SetDeviceStatus(DeviceState.NotTested);
        }

        // ------------------------------------------------------------------
        // Public API
        // ------------------------------------------------------------------
        public async void RefreshData()
        {
            string previousIp = txtDeviceIp.Text.Trim();
            LoadBoardTypes();
            SelectBoardByIp(previousIp);

            pdcTest.ReloadPdcList();

            if (tabControlConfig.SelectedTab == tabPdc)
            {
                await pdcTest.DiscoverDevicesAsync();
            }
            else
            {
                string ip = txtDeviceIp.Text.Trim();
                if (!IsValidIPv4(ip))
                {
                    SetDeviceStatus(DeviceState.InvalidIp);
                    return;
                }
                await FetchConfigurationAsync(ip);
            }
        }

        private void SelectBoardByIp(string ip)
        {
            if (string.IsNullOrWhiteSpace(ip)) return;

            for (int i = 0; i < cmbBoardType.Items.Count; i++)
            {
                BoardItem item = cmbBoardType.Items[i] as BoardItem;

                if (item != null &&
                    string.Equals((item.BoardIp ?? string.Empty).Trim(), ip,
                                  StringComparison.OrdinalIgnoreCase))
                {
                    cmbBoardType.SelectedIndex = i;
                    return;
                }
            }

            txtDeviceIp.Text = ip;
        }

        // ------------------------------------------------------------------
        // Board operations
        // ------------------------------------------------------------------
        private async Task FetchConfigurationAsync(string ip)
        {
            txtSent.Text = BuildConfigGetRequest(ip);
            txtReceived.Text = "Calling http://" + ip + "/IPIS/GetConf ...";

            btnPreview.Enabled = false;
            try
            {
                string response = await BoardApiHandler.Instance.GetConfigurationAsync(ip);

                txtReceived.Text = string.IsNullOrWhiteSpace(response)
                    ? "Empty response from the board."
                    : response;
            }
            finally
            {
                btnPreview.Enabled = true;
            }

            SetDeviceStatus(DeviceState.PreviewReady);
        }

        /// <summary>POST /IPIS/SetConf with the INT / DTO chosen on the screen.</summary>
        private async Task SendSetConfigurationAsync(string ip)
        {
            string intValue = (cmbInt.SelectedItem as string) ?? string.Empty;
            string dtoValue = (cmbDto.SelectedItem as string) ?? string.Empty;

            if (intValue.Length == 0 && dtoValue.Length == 0)
            {
                MessageBox.Show("Select an INT value or enter a DTO value to send.",
                    "Set Configuration", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string standardBody = BoardApiHandler.BuildSetConfigBody(intValue, dtoValue);
            txtSent.Text = BuildConfigSetRequest(ip, standardBody);
            txtReceived.Text = "Calling http://" + ip + "/IPIS/SetConf ...";

            btnPreview.Enabled = false;
            try
            {
                string response = await BoardApiHandler.Instance.SetConfigurationAsync(ip, intValue, dtoValue);

                txtReceived.Text = string.IsNullOrWhiteSpace(response)
                    ? "Empty response from the board."
                    : response;

                if (IsBoardStatusOk(response))
                {
                    txtReceived.Text += Environment.NewLine + Environment.NewLine
                        + SaveConfigurationToDatabase(cmbBoardType.SelectedItem as BoardItem, intValue, dtoValue);
                }
                else
                {
                    txtReceived.Text += Environment.NewLine + Environment.NewLine
                        + "Database not updated: the board did not accept the Set Configuration.";
                }
            }
            finally
            {
                btnPreview.Enabled = true;
            }

            SetDeviceStatus(DeviceState.PreviewReady);
        }

        private static bool IsBoardStatusOk(string response)
        {
            if (string.IsNullOrWhiteSpace(response)) return false;

            try
            {
                using (System.Text.Json.JsonDocument doc = System.Text.Json.JsonDocument.Parse(response))
                {
                    System.Text.Json.JsonElement root = doc.RootElement;

                    return root.ValueKind == System.Text.Json.JsonValueKind.Object
                        && root.TryGetProperty("status", out System.Text.Json.JsonElement status)
                        && status.ValueKind == System.Text.Json.JsonValueKind.String
                        && string.Equals(status.GetString(), "OK", StringComparison.OrdinalIgnoreCase);
                }
            }
            catch
            {
                // "HTTP 400 ..." / "Request failed ..." are not JSON -> not OK.
                return false;
            }
        }

        private static string SaveConfigurationToDatabase(BoardItem board, string intValue, string dtoValue)
        {
            if (board == null || board.BoardId <= 0)
                return "Database not updated: no board selected in Board Type.";

            StringBuilder result = new StringBuilder();

            if (Array.IndexOf(IntValues, intValue) >= 0)
            {
                int intRows = _dbService.UpdateBoardSettingColumn(board.BoardId, "Intensity", intValue);

                result.AppendLine(intRows > 0
                    ? "Database: Intensity = " + intValue + " saved for BOARD_ID " + board.BoardId + "."
                    : "Database: Intensity NOT saved for BOARD_ID " + board.BoardId + " (see the log).");
            }

            if (int.TryParse(dtoValue, out int dtoSeconds))
            {
                int dtoRows = _dbService.UpdateBoardSettingColumn(board.BoardId, "DTO", dtoSeconds);

                result.Append(dtoRows > 0
                    ? "Database: DTO = " + dtoSeconds + " saved for BOARD_ID " + board.BoardId + "."
                    : "Database: DTO NOT saved for BOARD_ID " + board.BoardId + " (check that BOARDSETTING has a DTO column; see the log).");
            }

            return result.ToString().TrimEnd();
        }

        /// <summary>
        /// Soft Reset: toggles BOARDSETTING.RST_Soft_Reset for the selected board. The CDC / service that sends
        /// the reset to the board sets it back to 0. Only this one column is written.
        /// </summary>
        private void RequestSoftReset()
        {
            BoardItem board = cmbBoardType.SelectedItem as BoardItem;
            if (board == null || board.BoardId <= 0)
            {
                MessageBox.Show("Select a board from Board Type first.",
                    "Soft Reset", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string boardText = board.ToString();

            // Current value from BOARDSETTING, so the prompt shows what will change.
            bool? currentValue = ReadSoftResetValue(board.BoardId);
            string currentText = currentValue.HasValue ? (currentValue.Value ? "1" : "0") : "?";
            string newText = currentValue.HasValue ? (currentValue.Value ? "0" : "1") : "?";

            DialogResult answer = MessageBox.Show(
                "Change Soft Reset for " + boardText + " from " + currentText + " to " + newText + "?",
                "Soft Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (answer != DialogResult.Yes) return;

            // Shown in Sent: the statement _dbService.ToggleBoardSettingFlag runs.
            txtSent.Text = "UPDATE BOARDSETTING SET RST_Soft_Reset = CASE WHEN ISNULL(RST_Soft_Reset, 0) = 1 THEN 0 ELSE 1 END"
                         + " WHERE BOARD_ID = " + board.BoardId + ";";

            int rows = _dbService.ToggleBoardSettingFlag(board.BoardId, "RST_Soft_Reset");

            if (rows > 0)
            {
                bool? savedValue = ReadSoftResetValue(board.BoardId);
                string savedText = savedValue.HasValue ? (savedValue.Value ? "1" : "0") : "unknown";

                txtReceived.Text = "Soft Reset changed: RST_Soft_Reset = " + savedText + " for BOARD_ID " + board.BoardId
                    + " (" + boardText + ")" + (currentValue.HasValue ? ", was " + currentText + "." : ".");
                SetDeviceStatus(DeviceState.PreviewReady);
            }
            else if (rows == 0)
            {
                txtReceived.Text = "No row updated: BOARD_ID " + board.BoardId + " was not found in BOARDSETTING.";
            }
            else
            {
                txtReceived.Text = "Soft Reset failed: the BOARDSETTING update returned an error (see the log).";
            }
        }

        /// <summary>RST_Soft_Reset of this board from BOARDSETTING; null when it cannot be read.</summary>
        private static bool? ReadSoftResetValue(int boardId)
        {
            try
            {
                List<BoardSettingInfo> boards = _dbService.GET_BOARD_SETTING(0);
                BoardSettingInfo row = boards?.Find(b => b.BOARD_ID == boardId);
                return row == null ? (bool?)null : row.RST_Soft_Reset;
            }
            catch
            {
                return null;
            }
        }

        // ------------------------------------------------------------------
        // Request builders
        // ------------------------------------------------------------------

        /// <summary>The actual GET issued by BoardApiHandler.GetConfigurationAsync.</summary>
        private static string BuildConfigGetRequest(string ip)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("GET /IPIS/GetConf HTTP/1.1");
            sb.AppendLine("Host: " + ip);
            sb.Append("Accept: */*");
            AppendStationCodeLine(sb);
            return sb.ToString();
        }

        /// <summary>The actual POST issued by BoardApiHandler.SetConfigurationAsync.</summary>
        private static string BuildConfigSetRequest(string ip, string body)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("POST /IPIS/SetConf HTTP/1.1");
            sb.AppendLine("Host: " + ip);
            sb.AppendLine("Accept: */*");
            string stationCode = BoardApiHandler.GetStationCode();
            if (!string.IsNullOrWhiteSpace(stationCode))
                sb.AppendLine("Station-Code: " + stationCode);
            sb.AppendLine("Content-Type: application/json");
            sb.AppendLine("Content-Length: " + Encoding.UTF8.GetByteCount(body ?? string.Empty));
            sb.AppendLine();
            sb.Append(body);
            return sb.ToString();
        }

        private static void AppendStationCodeLine(StringBuilder sb)
        {
            string stationCode = BoardApiHandler.GetStationCode();
            if (!string.IsNullOrWhiteSpace(stationCode))
                sb.Append(Environment.NewLine + "Station-Code: " + stationCode);
        }

        private static string BuildRestRequest(string board, string ip, CommandItem cmd)
        {
            string deviceId = board + "-001";   // replace with the real device id when available

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine("  \"deviceId\": \"" + JsonEscape(deviceId) + "\",");
            sb.AppendLine("  \"ip\": \"" + JsonEscape(ip) + "\",");

            if (cmd.Code == "SET_CONFIGURATION")
            {
                sb.AppendLine("  \"command\": \"" + JsonEscape(cmd.Code) + "\",");

                // A.4.2.5(a): intensity and the period for which the sent data stays valid.
                sb.AppendLine("  \"intensity\": null,");

                if (SupportsColourTable(board))
                {
                    sb.AppendLine("  \"dataValidityTimeSec\": null,");
                    // A.4.2.5(b): colour configuration table, VDB and AVDB only.
                    sb.AppendLine("  \"colourConfigTable\": null");
                }
                else
                {
                    sb.AppendLine("  \"dataValidityTimeSec\": null");
                }
            }
            else
            {
                sb.AppendLine("  \"command\": \"" + JsonEscape(cmd.Code) + "\"");
            }

            sb.Append("}");
            return sb.ToString();
        }

        /// <summary>A.4.2.5(b): the colour configuration table applies to VDB and AVDB only.</summary>
        private static bool SupportsColourTable(string boardType)
        {
            if (string.IsNullOrWhiteSpace(boardType)) return false;

            string t = boardType.Trim().ToUpperInvariant();
            return t.Contains("AVDB") || t.Contains("VDB");
        }

        // ------------------------------------------------------------------
        // Helpers
        // ------------------------------------------------------------------
        private void ResetOutput()
        {
            txtSent.Clear();
            txtReceived.Text = NoResponseText;
        }

        private void SetDeviceStatus(DeviceState state)
        {
            if (lblStatusBadge != null)
                lblStatusBadge.Visible = false;
        }

        private static bool IsValidIPv4(string text)
        {
            IPAddress addr;
            if (!IPAddress.TryParse(text, out addr)) return false;
            return addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork
                   && text.Split('.').Length == 4;
        }

        private static string JsonEscape(string s)
        {
            return (s ?? string.Empty).Replace("\\", "\\\\").Replace("\"", "\\\"");
        }
    }
}