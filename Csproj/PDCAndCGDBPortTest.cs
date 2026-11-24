using DataBaseHandler;
using DataModel;
using Lextm.SharpSnmpLib;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace IPISv2
{
    public partial class PDCAndCGDBPortTest : UserControl
    {
        private enum DeviceState
        {
            NotTested,
            PreviewReady,
            InvalidIp
        }

        private const int PdcPort = 8000;

        private const string CmdGet = "Get Configuration";
        private const string CmdSet = "Set Configuration";
        private const string CmdReset = "Soft Reset";

        private static readonly HttpClient _pdcHttpClient =
            new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(15)
            };

        private static readonly JsonSerializerOptions JsonOptions =
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

        private static readonly string[] IntValues =
        {
            "25", "50", "75", "100"
        };

        private const string NoResponseText =
            "No response — UI preview only. No command was sent.";

        // ============================================================
        // HUB ITEM
        // ============================================================

        private sealed class PdcItem
        {
            public int HubId { get; set; }
            public string NodeName { get; set; }
            public string HubIp { get; set; }
            public string DownLinkIp { get; set; }
            public string PfNo { get; set; }

            public override string ToString()
            {
                string down = (DownLinkIp ?? string.Empty).Trim();

                if (!string.IsNullOrWhiteSpace(down))
                    return down;

                return "HUB " + HubId +
                       " - HubIPDownLink not returned (HUB_IP " +
                       HubIp + ")";
            }
        }

        // ============================================================
        // CGDB DROPDOWN ITEM  (IP ONLY — Asset ID is NOT displayed)
        // ============================================================

        private sealed class CgdbBoardItem
        {
            public string Ip { get; set; }
            public string AssetId { get; set; }

            public override string ToString()
            {
                return Ip;   // <-- IP only
            }
        }

        // ============================================================
        // PDC API RESPONSE MODELS
        // ============================================================

        private sealed class PdcDevicesResponse
        {
            public bool success { get; set; }
            public string message { get; set; }
            public int total_devices { get; set; }

            public List<PdcDevice> devices { get; set; }
            public List<PdcNetwork> networks { get; set; }

            public int scanned_hosts { get; set; }
            public string scan_method { get; set; }

            public PdcSwitch switch_info { get; set; }
        }

        private sealed class PdcDevice
        {
            public string ip { get; set; }
            public string vendor { get; set; }
            public string device { get; set; }
            public string PF { get; set; }
            public string ASSET { get; set; }

            public string INT { get; set; }
            public string DTO { get; set; }
            public string MODE { get; set; }
            public string FW { get; set; }

            public string mac { get; set; }

            public int switch_port { get; set; }
            public string switch_port_name { get; set; }
            public int macs_on_port { get; set; }

            public string connection_status { get; set; }
            public string last_discovery { get; set; }
        }

        private sealed class PdcNetwork
        {
            public string @interface { get; set; }
            public string local_ip { get; set; }
            public string network { get; set; }
            public bool default_route { get; set; }
        }

        private sealed class PdcSwitch
        {
            public bool enabled { get; set; }
            public string host { get; set; }
            public string status { get; set; }
            public int matched { get; set; }

            public List<string> not_found { get; set; }
        }

        // ============================================================
        // CONSTRUCTOR
        // ============================================================

        public PDCAndCGDBPortTest()
        {
            InitializeComponent();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            InitializePdcUi();
        }

        // ============================================================
        // PUBLIC METHODS
        // ============================================================

        public void ReloadPdcList()
        {
            LoadPdcList();
        }

        public Task DiscoverDevicesAsync()
        {
            return FetchPdcDevicesAsync();
        }

        // ============================================================
        // INITIALIZATION
        // ============================================================

        private void InitializePdcUi()
        {
            txtPdcIp.ReadOnly = false;

            LoadPdcCommands();
            LoadPdcIntValues();

            cmbPdcDto.SelectedItem = "60";

            // Port textbox is read-only; value comes from
            // COACHCONFIGURATION.PORT_NO for the selected board.
            txtPdcPort.ReadOnly = true;
            txtPdcPort.Text = string.Empty;

            // Asset ID textbox is READ-ONLY — value comes
            // from COACHCONFIGURATION, not from the user.
            txtCgdbAssetId.ReadOnly = false;
            txtCgdbAssetId.Text = string.Empty;

            // Dropdown is populated from COACHCONFIGURATION.
            LoadCgdbBoardDropdown();

            ResetPdcOutput();

            SetPdcStatus(DeviceState.NotTested);

            LoadPdcList();

            cmbPdcCommand_SelectedIndexChanged(
                cmbPdcCommand,
                EventArgs.Empty);
        }

        // ============================================================
        // LOAD HUB CONFIGURATION
        // ============================================================

        private void LoadPdcList()
        {
            cmbPdcList.Items.Clear();

            try
            {
                List<HUBCONFIG> hubs =
                    _dbService.Get_Hub_Config(0);

                if (hubs != null)
                {
                    foreach (HUBCONFIG h in hubs)
                    {
                        cmbPdcList.Items.Add(new PdcItem
                        {
                            HubId = h.HUB_ID,
                            NodeName = h.NODE_NAME,

                            HubIp =
                                (h.HUB_IP ?? string.Empty).Trim(),

                            DownLinkIp =
                                (h.HubIPDownLink ?? string.Empty).Trim(),

                            PfNo = h.PF_NUMBER
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.LoggerService.Logger.Warning(
                    ex,
                    "Failed to load HUBCONFIG into cmbPdcList: " +
                    ex.Message);

                txtPdcReceived.Text =
                    "Could not read HUBCONFIG (see the log).";

                txtPdcIp.Clear();

                return;
            }

            if (cmbPdcList.Items.Count > 0)
            {
                cmbPdcList.SelectedIndex = 0;
            }
            else
            {
                txtPdcIp.Clear();

                txtPdcReceived.Text =
                    "Get_Hub_Config returned no rows.";
            }
        }

        // ============================================================
        // LOAD COMMANDS
        // ============================================================

        private void LoadPdcCommands()
        {
            cmbPdcCommand.Items.Clear();

            cmbPdcCommand.Items.Add(CmdGet);
            cmbPdcCommand.Items.Add(CmdSet);
            cmbPdcCommand.Items.Add(CmdReset);

            cmbPdcCommand.SelectedIndex = 0;
        }

        private void LoadPdcIntValues()
        {
            cmbPdcInt.Items.Clear();

            foreach (string value in IntValues)
            {
                cmbPdcInt.Items.Add(value);
            }

            cmbPdcInt.SelectedIndex = 0;
        }

        // ============================================================
        // EVENT HANDLERS
        // ============================================================

        private void cmbPdcList_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            if (cmbPdcList.SelectedItem is PdcItem item)
            {
                string ip =
                    !string.IsNullOrWhiteSpace(item.DownLinkIp)
                        ? item.DownLinkIp
                        : item.HubIp;

                txtPdcIp.Text = ip;
            }

            // Refresh the CGDB board dropdown from COACHCONFIGURATION
            // when the selected hub changes.
            LoadCgdbBoardDropdown();

            if (!string.IsNullOrEmpty(txtCgdbAssetId.Text))
            {
                txtCgdbAssetId.Text = string.Empty;
            }

            if (!string.IsNullOrEmpty(txtPdcPort.Text))
            {
                txtPdcPort.Text = string.Empty;
            }

            ResetPdcOutput();

            SetPdcStatus(DeviceState.NotTested);
        }

        private void txtPdcIp_TextChanged(
            object sender,
            EventArgs e)
        {
            SetPdcStatus(DeviceState.NotTested);
        }

        private void cmbPdcCommand_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            string command =
                (cmbPdcCommand.SelectedItem?.ToString() ?? string.Empty)
                .Trim();

            bool isSet = string.Equals(
                command, CmdSet, StringComparison.OrdinalIgnoreCase);

            bool isReset = string.Equals(
                command, CmdReset, StringComparison.OrdinalIgnoreCase);

            // INT / DTO / Port / Asset ID only for Set.
            lblPdcInt.Visible = isSet;
            cmbPdcInt.Visible = isSet;

            lblPdcDto.Visible = isSet;
            cmbPdcDto.Visible = isSet;

            lblPdcPort.Visible = isSet;
            txtPdcPort.Visible = isSet;

            lblCgdbAssetId.Visible = isSet;
            txtCgdbAssetId.Visible = isSet;

            // CGDB board dropdown is shown for Set AND Soft Reset.
            bool showBoard = isSet || isReset;

            lblCgdbBoardIp.Visible = showBoard;
            cmbCgdbBoardIp.Visible = showBoard;

            // Bring Set-mode controls to front so they're never covered.
            if (isSet)
            {
                lblPdcPort.BringToFront();
                txtPdcPort.BringToFront();
                lblCgdbAssetId.BringToFront();
                txtCgdbAssetId.BringToFront();
                lblPdcInt.BringToFront();
                cmbPdcInt.BringToFront();
                lblPdcDto.BringToFront();
                cmbPdcDto.BringToFront();
            }

            if (showBoard)
            {
                cmbCgdbBoardIp.BringToFront();
                lblCgdbBoardIp.BringToFront();
            }

            btnPdcExecute.Text =
                isSet ? CmdSet
                : isReset ? CmdReset
                : CmdGet;

            pnlPdcCommand.PerformLayout();

            if (pnlPdcCommand.Parent is
                System.Windows.Forms.TableLayoutPanel tlp)
            {
                tlp.PerformLayout();
            }
        }

        // ============================================================
        // CGDB BOARD SELECTION → AUTO-FILL ASSET ID + PORT FROM DB
        // ============================================================

        private void cmbCgdbBoardIp_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            if (!(cmbCgdbBoardIp.SelectedItem is CgdbBoardItem board))
            {
                txtCgdbAssetId.Text = string.Empty;
                txtPdcPort.Text = string.Empty;
                return;
            }

            string selectedIp = board.Ip;

            if (string.IsNullOrWhiteSpace(selectedIp) ||
                !IsValidIPv4(selectedIp))
            {
                txtCgdbAssetId.Text = string.Empty;
                txtPdcPort.Text = string.Empty;
                return;
            }

            // Asset ID for THIS board only.
            string asset = GetCoachConfigurationAssetId(selectedIp);
            txtCgdbAssetId.Text =
                string.IsNullOrWhiteSpace(asset) ? string.Empty : asset;

            // PORT_NO for THIS board only.
            string port = GetCoachConfigurationPortNo(selectedIp);
            txtPdcPort.Text =
                string.IsNullOrWhiteSpace(port) ? string.Empty : port;

            // Sanity check in the debug window.
            System.Diagnostics.Debug.WriteLine(
                $"[PDC] Selected board → IP='{selectedIp}' " +
                $"PORT_NO='{port}' ASSET_ID='{asset}'");
        }

        private async void btnPdcExecute_Click(
            object sender,
            EventArgs e)
        {
            string command =
                cmbPdcCommand.SelectedItem?.ToString()
                ?? string.Empty;

            if (string.Equals(
                command, CmdSet, StringComparison.OrdinalIgnoreCase))
            {
                await SendPdcSetConfigurationAsync();
            }
            else if (string.Equals(
                command, CmdReset, StringComparison.OrdinalIgnoreCase))
            {
                await SendPdcSoftResetAsync();
            }
            else
            {
                await FetchPdcDevicesAsync();
            }
        }

        private void btnPdcClear_Click(
            object sender,
            EventArgs e)
        {
            ResetPdcOutput();

            SetPdcStatus(DeviceState.NotTested);
        }

        // ============================================================
        // GET CONFIGURATION
        // GET /api/devices
        // ============================================================

        private async Task FetchPdcDevicesAsync()
        {
            if (!TryGetPdcEndpoint(out string ip, out int port))
                return;

            txtPdcSent.Text =
                BuildConfigPdcGetRequest(ip, port);

            txtPdcReceived.Text =
                "Querying CGDB devices from PDC at http://" +
                ip + ":" + port + "/api/devices ...";

            btnPdcExecute.Enabled = false;

            try
            {
                string response =
                    await BoardApiHandler.Instance.GetPdcDevicesAsync(
                        ip,
                        port);

                if (string.IsNullOrWhiteSpace(response))
                {
                    txtPdcReceived.Text =
                        "Empty response from PDC.";

                    SetPdcStatus(DeviceState.InvalidIp);

                    return;
                }

                PdcDevicesResponse parsed =
                    JsonSerializer.Deserialize<PdcDevicesResponse>(
                        response,
                        JsonOptions);

                if (parsed == null)
                {
                    txtPdcReceived.Text =
                        "Unable to deserialize PDC response.";

                    SetPdcStatus(DeviceState.InvalidIp);

                    return;
                }

                if (!parsed.success)
                {
                    txtPdcReceived.Text =
                        "PDC API failed: " + parsed.message;

                    SetPdcStatus(DeviceState.InvalidIp);

                    return;
                }

                List<PdcDevice> devices =
                    parsed.devices ?? new List<PdcDevice>();

                StringBuilder output = new StringBuilder();

                output.AppendLine("PDC GET CONFIGURATION");
                output.AppendLine("==========================================");

                output.AppendLine("PDC IP          : " + ip);
                output.AppendLine("API             : GET /api/devices");
                output.AppendLine("Total Devices   : " + parsed.total_devices);
                output.AppendLine("Returned Devices: " + devices.Count);

                output.AppendLine();

                output.AppendLine(
                    FormatDevicesResponse(parsed));

                output.AppendLine();

                output.AppendLine(
                    UpdateDiscoveredCgdbDevices(devices));

                // After the DB is updated, refresh the dropdown
                // from COACHCONFIGURATION so newly discovered
                // boards appear.
                LoadCgdbBoardDropdown();

                txtPdcReceived.Text =
                    output.ToString().TrimEnd();

                SetPdcStatus(DeviceState.PreviewReady);
            }
            catch (JsonException ex)
            {
                txtPdcReceived.Text =
                    "Invalid JSON response from PDC: " + ex.Message;

                SetPdcStatus(DeviceState.InvalidIp);
            }
            catch (TaskCanceledException)
            {
                txtPdcReceived.Text =
                    "PDC request timed out. Check PDC connectivity.";

                SetPdcStatus(DeviceState.InvalidIp);
            }
            catch (Exception ex)
            {
                txtPdcReceived.Text =
                    "PDC query failed: " + ex.Message;

                LogManager.LoggerService.Logger.Error(
                    ex,
                    "PDC Get Configuration failed.");

                SetPdcStatus(DeviceState.InvalidIp);
            }
            finally
            {
                btnPdcExecute.Enabled = true;
            }
        }

        // ============================================================
        // LOAD CGDB DROPDOWN  (IPs FROM COACHCONFIGURATION)
        // ============================================================

        private void LoadCgdbBoardDropdown()
        {
            // Remember the currently selected IP so we can restore it.
            string previousIp =
                (cmbCgdbBoardIp.SelectedItem as CgdbBoardItem)?.Ip;

            cmbCgdbBoardIp.BeginUpdate();

            try
            {
                cmbCgdbBoardIp.Items.Clear();

                List<string> ips = GetCoachConfigurationIps();

                if (ips.Count == 0)
                {
                    cmbCgdbBoardIp.Text = string.Empty;
                    txtCgdbAssetId.Text = string.Empty;
                    txtPdcPort.Text = string.Empty;
                    return;
                }

                foreach (string ip in ips)
                {
                    cmbCgdbBoardIp.Items.Add(
                        new CgdbBoardItem
                        {
                            Ip = ip,
                            AssetId = null
                        });
                }

                // Restore previous selection if possible.
                int restoreIndex = 0;

                if (!string.IsNullOrWhiteSpace(previousIp))
                {
                    for (int i = 0; i < cmbCgdbBoardIp.Items.Count; i++)
                    {
                        if (cmbCgdbBoardIp.Items[i] is CgdbBoardItem item &&
                            string.Equals(item.Ip, previousIp,
                                          StringComparison.OrdinalIgnoreCase))
                        {
                            restoreIndex = i;
                            break;
                        }
                    }
                }

                cmbCgdbBoardIp.SelectedIndex = restoreIndex;
            }
            finally
            {
                cmbCgdbBoardIp.EndUpdate();
            }

            // Ensure the textboxes reflect the CURRENT selection,
            // even if SelectedIndexChanged didn't fire (index same).
            cmbCgdbBoardIp_SelectedIndexChanged(
                cmbCgdbBoardIp, EventArgs.Empty);
        }

        // ============================================================
        // UPDATE COACHCONFIGURATION
        // Now writes PORT_NO even when ASSET / FW are missing.
        // ============================================================

        private string UpdateDiscoveredCgdbDevices(
            List<PdcDevice> devices)
        {
            int updated = 0;
            int ipNotFound = 0;
            int failed = 0;
            int skipped = 0;

            StringBuilder result = new StringBuilder();

            result.AppendLine("CGDB DATABASE UPDATE");
            result.AppendLine("------------------------------------------");

            if (devices == null || devices.Count == 0)
            {
                result.AppendLine(
                    "No discovered devices to update.");

                return result.ToString().TrimEnd();
            }

            foreach (PdcDevice device in devices)
            {
                if (device == null)
                {
                    skipped++;

                    result.AppendLine(
                        "Unknown device : Skipped.");

                    continue;
                }

                string ip =
                    device.ip?.Trim();

                string assetId =
                    device.ASSET?.Trim();

                string firmwareId =
                    device.FW?.Trim();

                // PORT_NO is an INT column → use switch_port (int).
                int portNo = device.switch_port;

                if (string.IsNullOrWhiteSpace(ip))
                {
                    skipped++;

                    result.AppendLine(
                        "Unknown IP : Skipped - IP address is missing.");

                    continue;
                }

                // -------------------------------------------------
                // NOTE: we no longer skip when asset/fw are null.
                // We still write PORT_NO for those IPs.
                // -------------------------------------------------
                bool assetMissing =
                    string.IsNullOrWhiteSpace(assetId) ||
                    string.IsNullOrWhiteSpace(firmwareId);

                int intensity = 0;
                int dto = 0;

                if (!assetMissing)
                {
                    int.TryParse(device.INT, out intensity);
                    int.TryParse(device.DTO, out dto);
                }

                try
                {
                    string dbResult =
                        _dbService.ADDUPDATECGDBASSETID(
                            ip,
                            assetMissing ? null : assetId,
                            assetMissing ? null : firmwareId,
                            intensity,
                            portNo,
                            dto);

                    if (string.Equals(
                        dbResult,
                        "Updated Successfully",
                        StringComparison.OrdinalIgnoreCase))
                    {
                        updated++;

                        result.AppendLine(
                            $"{ip} : Updated successfully - " +
                            (assetMissing
                                ? "(asset/fw not provided, PORT_NO only) "
                                : $"Asset={assetId}, FW={firmwareId}, " +
                                  $"INT={intensity}, DTO={dto}, ") +
                            $"PORT_NO={portNo}");
                    }
                    else if (string.Equals(
                        dbResult,
                        "IP address not found",
                        StringComparison.OrdinalIgnoreCase))
                    {
                        ipNotFound++;

                        result.AppendLine(
                            $"{ip} : IP address not found in CGDB configuration - Skipped.");
                    }
                    else
                    {
                        failed++;

                        result.AppendLine(
                            $"{ip} : Database returned - " +
                            (dbResult ?? "No result returned."));
                    }
                }
                catch (Exception ex)
                {
                    failed++;

                    result.AppendLine(
                        $"{ip} : Database update failed - " +
                        ex.Message);
                }
            }

            result.AppendLine();

            result.AppendLine("DATABASE SUMMARY");
            result.AppendLine("------------------------------------------");

            result.AppendLine(
                $"Total discovered : {devices.Count}");

            result.AppendLine(
                $"Updated          : {updated}");

            result.AppendLine(
                $"IP not found     : {ipNotFound}");

            result.AppendLine(
                $"Failed           : {failed}");

            result.AppendLine(
                $"Skipped          : {skipped}");

            return result.ToString().TrimEnd();
        }

        // ============================================================
        // SET CONFIGURATION
        // POST /api/config
        // Asset ID + PORT_NO are pulled FROM THE DATABASE.
        // ============================================================

        private async Task SendPdcSetConfigurationAsync()
        {
            // PDC server IP.
            if (!TryGetPdcEndpoint(
                out string pdcIp,
                out int port))
            {
                return;
            }

            // Selected CGDB board (IP only).
            if (!(cmbCgdbBoardIp.SelectedItem
                  is CgdbBoardItem selectedBoard))
            {
                MessageBox.Show(
                    "Please select a CGDB board IP. " +
                    "First execute Get Configuration to discover boards.",
                    "Set Configuration",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cmbCgdbBoardIp.Focus();

                return;
            }

            string boardIp =
                selectedBoard.Ip;

            if (string.IsNullOrWhiteSpace(boardIp) ||
                !IsValidIPv4(boardIp))
            {
                MessageBox.Show(
                    "Selected CGDB board IP is invalid.",
                    "Set Configuration",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            // ------------------------------------------------
            // Asset ID comes from COACHCONFIGURATION, not PDC.
            // ------------------------------------------------
            string assetId =
                GetCoachConfigurationAssetId(boardIp);

            if (string.IsNullOrWhiteSpace(assetId))
            {
                MessageBox.Show(
                    "No Asset ID found in COACHCONFIGURATION for IP " +
                    boardIp + ". " +
                    "Run Get Configuration first to register the board.",
                    "Set Configuration",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            // Keep the read-only textbox in sync.
            txtCgdbAssetId.Text = assetId;

            // ------------------------------------------------
            // PORT_NO comes from COACHCONFIGURATION, not from
            // the PDC response and not from PdcPort constant.
            // ------------------------------------------------
            string portNoFromDb =
                GetCoachConfigurationPortNo(boardIp);

            // Sync the read-only port textbox.
            txtPdcPort.Text =
                string.IsNullOrWhiteSpace(portNoFromDb)
                    ? string.Empty
                    : portNoFromDb;

            // INT.
            string intValue =
                cmbPdcInt.SelectedItem?.ToString()
                ?? string.Empty;

            // DTO.
            string dtoValue =
                cmbPdcDto.SelectedItem?.ToString()
                ?? string.Empty;

            if (!int.TryParse(intValue, out int intensity))
            {
                MessageBox.Show(
                    "Please select a valid INT value.",
                    "Set Configuration",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (!int.TryParse(dtoValue, out int dto))
            {
                MessageBox.Show(
                    "Please select a valid DTO value.",
                    "Set Configuration",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            // Only Asset ID is sent to the Set API.
            var requestBody = new
            {
                ip_address = boardIp,
                ASSET_ID = assetId
            };

            string jsonBody =
                JsonSerializer.Serialize(requestBody);

            string apiUrl =
                $"http://{pdcIp}:{port}/api/config";

            txtPdcSent.Text =
                BuildConfigPdcSetRequest(
                    pdcIp,
                    jsonBody,
                    port);

            txtPdcReceived.Text =
                "Setting configuration for CGDB board " +
                boardIp + " ...";

            btnPdcExecute.Enabled = false;

            try
            {
                using var content = new StringContent(
                    jsonBody,
                    Encoding.UTF8,
                    "application/json");

                using HttpResponseMessage response =
                    await _pdcHttpClient.PostAsync(
                        apiUrl,
                        content);

                string responseBody =
                    await response.Content.ReadAsStringAsync();

                StringBuilder output = new StringBuilder();

                output.AppendLine("CGDB SET CONFIGURATION");
                output.AppendLine("==========================================");

                output.AppendLine("PDC IP      : " + pdcIp);
                output.AppendLine("CGDB IP     : " + boardIp);
                output.AppendLine(
                    "Asset ID    : " + assetId +
                    " (from COACHCONFIGURATION)");
                output.AppendLine(
                    "PORT_NO     : " +
                    (string.IsNullOrWhiteSpace(portNoFromDb)
                        ? "(not set)"
                        : portNoFromDb) +
                    " (from COACHCONFIGURATION)");
                output.AppendLine("INT         : " + intensity);
                output.AppendLine("DTO         : " + dto);
                output.AppendLine("HTTP Port   : " + port);

                output.AppendLine(
                    "HTTP Status : " +
                    (int)response.StatusCode + " " +
                    response.StatusCode);

                output.AppendLine();

                output.AppendLine("REQUEST JSON");
                output.AppendLine("------------------------------------------");
                output.AppendLine(jsonBody);

                output.AppendLine();

                output.AppendLine("PDC RESPONSE");
                output.AppendLine("------------------------------------------");

                output.AppendLine(
                    string.IsNullOrWhiteSpace(responseBody)
                        ? "Empty response"
                        : responseBody);

                bool apiSuccess = false;

                if (response.IsSuccessStatusCode &&
                    !string.IsNullOrWhiteSpace(responseBody))
                {
                    try
                    {
                        using JsonDocument document =
                            JsonDocument.Parse(responseBody);

                        if (document.RootElement.ValueKind ==
                            JsonValueKind.Object &&
                            document.RootElement.TryGetProperty(
                                "success",
                                out JsonElement successElement))
                        {
                            apiSuccess =
                                successElement.ValueKind ==
                                JsonValueKind.True;
                        }
                    }
                    catch (JsonException)
                    {
                        apiSuccess = false;
                    }
                }

                if (apiSuccess)
                {
                    output.AppendLine();

                    output.AppendLine(
                        "Configuration applied successfully.");

                    string dbResult =
                        SavePdcConfigurationToDatabase(
                            new List<string> { boardIp },
                            intValue,
                            dtoValue);

                    if (!string.IsNullOrWhiteSpace(dbResult))
                    {
                        output.AppendLine();

                        output.AppendLine(dbResult);
                    }

                    SetPdcStatus(DeviceState.PreviewReady);
                }
                else
                {
                    output.AppendLine();

                    output.AppendLine(
                        "Configuration was not confirmed by PDC API.");

                    output.AppendLine(
                        "BOARDSETTING was not updated.");

                    SetPdcStatus(DeviceState.InvalidIp);
                }

                txtPdcReceived.Text =
                    output.ToString().TrimEnd();
            }
            catch (TaskCanceledException)
            {
                txtPdcReceived.Text =
                    "Set Configuration timed out. " +
                    "Check PDC connectivity.";

                SetPdcStatus(DeviceState.InvalidIp);
            }
            catch (Exception ex)
            {
                txtPdcReceived.Text =
                    "Set Configuration failed: " + ex.Message;

                LogManager.LoggerService.Logger.Error(
                    ex,
                    "PDC Set Configuration failed.");

                SetPdcStatus(DeviceState.InvalidIp);
            }
            finally
            {
                btnPdcExecute.Enabled = true;
            }
        }

        // ============================================================
        // SOFT RESET
        // POST /api/reset
        // ============================================================

        private async Task SendPdcSoftResetAsync()
        {
            if (!TryGetPdcEndpoint(
                out string pdcIp,
                out int port))
            {
                return;
            }

            if (!(cmbCgdbBoardIp.SelectedItem
                  is CgdbBoardItem selectedBoard))
            {
                MessageBox.Show(
                    "Please select a CGDB board IP to reset. " +
                    "First execute Get Configuration to discover boards.",
                    "Soft Reset",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cmbCgdbBoardIp.Focus();

                return;
            }

            string boardIp =
                selectedBoard.Ip;

            if (string.IsNullOrWhiteSpace(boardIp) ||
                !IsValidIPv4(boardIp))
            {
                MessageBox.Show(
                    "Selected CGDB board IP is invalid.",
                    "Soft Reset",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            var requestBody = new
            {
                ip_address = boardIp
            };

            string jsonBody =
                JsonSerializer.Serialize(requestBody);

            string apiUrl =
                $"http://{pdcIp}:{port}/api/reset";

            txtPdcSent.Text =
                BuildConfigPdcResetRequest(
                    pdcIp,
                    jsonBody,
                    port);

            txtPdcReceived.Text =
                "Sending soft reset to CGDB board " +
                boardIp + " ...";

            btnPdcExecute.Enabled = false;

            try
            {
                using var content = new StringContent(
                    jsonBody,
                    Encoding.UTF8,
                    "application/json");

                using HttpResponseMessage response =
                    await _pdcHttpClient.PostAsync(
                        apiUrl,
                        content);

                string responseBody =
                    await response.Content.ReadAsStringAsync();

                StringBuilder output = new StringBuilder();

                output.AppendLine("CGDB SOFT RESET");
                output.AppendLine("==========================================");

                output.AppendLine("PDC IP      : " + pdcIp);
                output.AppendLine("CGDB IP     : " + boardIp);
                output.AppendLine("API         : POST /api/reset");
                output.AppendLine("PORT        : " + port);

                output.AppendLine(
                    "HTTP Status : " +
                    (int)response.StatusCode + " " +
                    response.StatusCode);

                output.AppendLine();

                output.AppendLine("REQUEST JSON");
                output.AppendLine("------------------------------------------");
                output.AppendLine(jsonBody);

                output.AppendLine();

                output.AppendLine("PDC RESPONSE");
                output.AppendLine("------------------------------------------");

                output.AppendLine(
                    string.IsNullOrWhiteSpace(responseBody)
                        ? "Empty response"
                        : responseBody);

                bool apiSuccess = false;

                if (response.IsSuccessStatusCode &&
                    !string.IsNullOrWhiteSpace(responseBody))
                {
                    try
                    {
                        using JsonDocument document =
                            JsonDocument.Parse(responseBody);

                        if (document.RootElement.ValueKind ==
                            JsonValueKind.Object &&
                            document.RootElement.TryGetProperty(
                                "success",
                                out JsonElement successElement))
                        {
                            apiSuccess =
                                successElement.ValueKind ==
                                JsonValueKind.True;
                        }
                    }
                    catch (JsonException)
                    {
                        apiSuccess = false;
                    }
                }

                output.AppendLine();

                if (apiSuccess)
                {
                    output.AppendLine(
                        "Soft reset command accepted by PDC.");

                    SetPdcStatus(DeviceState.PreviewReady);
                }
                else
                {
                    output.AppendLine(
                        "Soft reset was not confirmed by PDC API.");

                    SetPdcStatus(DeviceState.InvalidIp);
                }

                txtPdcReceived.Text =
                    output.ToString().TrimEnd();
            }
            catch (TaskCanceledException)
            {
                txtPdcReceived.Text =
                    "Soft reset timed out. " +
                    "Check PDC connectivity.";

                SetPdcStatus(DeviceState.InvalidIp);
            }
            catch (Exception ex)
            {
                txtPdcReceived.Text =
                    "Soft reset failed: " + ex.Message;

                LogManager.LoggerService.Logger.Error(
                    ex,
                    "PDC Soft Reset failed.");

                SetPdcStatus(DeviceState.InvalidIp);
            }
            finally
            {
                btnPdcExecute.Enabled = true;
            }
        }

        // ============================================================
        // DATABASE LOOKUP — COACHCONFIGURATION.DEVICE_ASSET_ID BY IP
        // ============================================================

        private static string GetCoachConfigurationAssetId(string ip)
        {
            if (string.IsNullOrWhiteSpace(ip) || !IsValidIPv4(ip))
                return string.Empty;

            try
            {
                string assetId = (_dbService.ExecuteScalar(
                    "SELECT TOP 1 DEVICE_ASSET_ID " +
                    "FROM dbo.COACHCONFIGURATION " +
                    "WHERE IP = '" + ip + "'") ?? string.Empty).Trim();

                return assetId;
            }
            catch (Exception ex)
            {
                LogManager.LoggerService.Logger.Error(
                    ex,
                    "GetCoachConfigurationAssetId failed for IP " + ip);

                return string.Empty;
            }
        }

        // ============================================================
        // DATABASE LOOKUP — COACHCONFIGURATION.PORT_NO BY IP
        // ============================================================

        private static string GetCoachConfigurationPortNo(string ip)
        {
            if (string.IsNullOrWhiteSpace(ip) || !IsValidIPv4(ip))
                return string.Empty;

            try
            {
                string port = (_dbService.ExecuteScalar(
                    "SELECT TOP 1 CONVERT(VARCHAR(20), PORT_NO) " +
                    "FROM dbo.COACHCONFIGURATION " +
                    "WHERE IP = '" + ip + "'") ?? string.Empty).Trim();

                return string.IsNullOrWhiteSpace(port)
                    ? "(null)"
                    : port;
            }
            catch (Exception ex)
            {
                LogManager.LoggerService.Logger.Error(
                    ex,
                    "GetCoachConfigurationPortNo failed for IP " + ip);

                return string.Empty;
            }
        }

        // ============================================================
        // DATABASE LOOKUP — ALL CGDB IPs FROM COACHCONFIGURATION
        // ============================================================

        private static List<string> GetCoachConfigurationIps()
        {
            var ips = new List<string>();

            try
            {
                string raw = _dbService.ExecuteScalar(
                    "SELECT STRING_AGG(IP, '|') " +
                    "FROM dbo.COACHCONFIGURATION " +
                    "WHERE PORT_TYPE = 'CGDB' " +
                    "  AND IP IS NOT NULL");

                if (string.IsNullOrWhiteSpace(raw))
                    return ips;

                foreach (string part in raw.Split('|'))
                {
                    string ip = part?.Trim();

                    if (string.IsNullOrWhiteSpace(ip))
                        continue;

                    if (!IsValidIPv4(ip))
                        continue;

                    if (ips.Contains(ip))
                        continue;

                    ips.Add(ip);
                }
            }
            catch (Exception ex)
            {
                LogManager.LoggerService.Logger.Error(
                    ex,
                    "GetCoachConfigurationIps failed.");
            }

            return ips;
        }

        // ============================================================
        // SAVE INT / DTO TO BOARDSETTING
        // ============================================================

        private string SavePdcConfigurationToDatabase(
            List<string> configuredIps,
            string intValue,
            string dtoValue)
        {
            if (configuredIps == null ||
                configuredIps.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("BOARDSETTING DATABASE UPDATES");
            sb.AppendLine("------------------------------------------");

            try
            {
                List<BoardSettingInfo> allBoards =
                    _dbService.GET_BOARD_SETTING(0);

                if (allBoards == null)
                {
                    sb.AppendLine(
                        "Could not retrieve BOARDSETTING records.");

                    return sb.ToString().TrimEnd();
                }

                int updatedCount = 0;
                int notFoundCount = 0;

                foreach (string devIp in configuredIps)
                {
                    var matchingBoard = allBoards.Find(
                        b => string.Equals(
                            (b.BOARD_IP ?? string.Empty).Trim(),
                            devIp.Trim(),
                            StringComparison.OrdinalIgnoreCase));

                    if (matchingBoard == null)
                    {
                        notFoundCount++;

                        sb.AppendLine(
                            $"{devIp} : No matching BOARDSETTING row found.");

                        continue;
                    }

                    if (Array.IndexOf(
                        IntValues,
                        intValue) >= 0)
                    {
                        _dbService.UpdateBoardSettingColumn(
                            matchingBoard.BOARD_ID,
                            "Intensity",
                            intValue);
                    }

                    if (int.TryParse(
                        dtoValue,
                        out int dtoSeconds))
                    {
                        _dbService.UpdateBoardSettingColumn(
                            matchingBoard.BOARD_ID,
                            "DTO",
                            dtoSeconds);
                    }

                    updatedCount++;

                    sb.AppendLine(
                        $"{devIp} (BOARD_ID {matchingBoard.BOARD_ID}) : " +
                        $"Intensity={intValue}, DTO={dtoValue} saved.");
                }

                sb.AppendLine();

                sb.AppendLine("SUMMARY");
                sb.AppendLine("Updated     : " + updatedCount);
                sb.AppendLine("IP not found: " + notFoundCount);
            }
            catch (Exception ex)
            {
                sb.AppendLine(
                    "Database update error: " + ex.Message);
            }

            return sb.ToString().TrimEnd();
        }

        // ============================================================
        // FORMAT GET CONFIGURATION RESPONSE
        // ============================================================

        private static string FormatDevicesResponse(
            PdcDevicesResponse parsed)
        {
            if (parsed == null)
                return "No PDC response.";

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("DISCOVERED CGDB DEVICES");
            sb.AppendLine("==========================================");

            sb.AppendLine(
                "Message       : " + parsed.message);

            sb.AppendLine(
                "Total Devices : " + parsed.total_devices);

            sb.AppendLine(
                "Scanned Hosts : " + parsed.scanned_hosts);

            sb.AppendLine(
                "Scan Method   : " + parsed.scan_method);

            sb.AppendLine();

            if (parsed.devices == null ||
                parsed.devices.Count == 0)
            {
                sb.AppendLine("No CGDB devices discovered.");
            }
            else
            {
                int index = 1;

                foreach (PdcDevice d in parsed.devices)
                {
                    sb.AppendLine(
                        "DEVICE " + index);

                    sb.AppendLine(
                        "------------------------------------------");

                    sb.AppendLine("IP              : " + d.ip);
                    sb.AppendLine("ASSET ID        : " + d.ASSET);
                    sb.AppendLine("VENDOR          : " + d.vendor);
                    sb.AppendLine("DEVICE          : " + d.device);
                    sb.AppendLine("PF              : " + d.PF);

                    sb.AppendLine("INTENSITY       : " + d.INT);
                    sb.AppendLine("DTO             : " + d.DTO);
                    sb.AppendLine("MODE            : " + d.MODE);

                    sb.AppendLine("FIRMWARE        : " + d.FW);

                    sb.AppendLine("MAC ADDRESS     : " + d.mac);

                    sb.AppendLine(
                        "SWITCH PORT     : " + d.switch_port);

                    sb.AppendLine(
                        "SWITCH PORT NAME: " + d.switch_port_name);

                    sb.AppendLine(
                        "MACs ON PORT    : " + d.macs_on_port);

                    sb.AppendLine(
                        "CONNECTION      : " + d.connection_status);

                    sb.AppendLine(
                        "LAST DISCOVERY  : " + d.last_discovery);

                    sb.AppendLine();

                    index++;
                }
            }

            // Network information.
            sb.AppendLine("NETWORK INFORMATION");
            sb.AppendLine("------------------------------------------");

            if (parsed.networks != null)
            {
                foreach (PdcNetwork network in parsed.networks)
                {
                    sb.AppendLine(
                        "Interface    : " + network.@interface);

                    sb.AppendLine(
                        "Local IP     : " + network.local_ip);

                    sb.AppendLine(
                        "Network      : " + network.network);

                    sb.AppendLine(
                        "Default Route: " + network.default_route);

                    sb.AppendLine();
                }
            }

            // Switch information.
            sb.AppendLine("SWITCH INFORMATION");
            sb.AppendLine("------------------------------------------");

            if (parsed.switch_info != null)
            {
                sb.AppendLine(
                    "Enabled      : " + parsed.switch_info.enabled);

                sb.AppendLine(
                    "Host         : " + parsed.switch_info.host);

                sb.AppendLine(
                    "Status       : " + parsed.switch_info.status);

                sb.AppendLine(
                    "Matched      : " + parsed.switch_info.matched);

                sb.AppendLine(
                    "Not Found    : " +
                    string.Join(
                        ", ",
                        parsed.switch_info.not_found
                        ?? new List<string>()));
            }
            else
            {
                sb.AppendLine("Switch information unavailable.");
            }

            return sb.ToString().TrimEnd();
        }

        // ============================================================
        // BUILD REQUESTS
        // ============================================================

        private static string BuildConfigPdcGetRequest(
            string ip,
            int port)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("GET /api/devices HTTP/1.1");
            sb.AppendLine("Host: " + ip + ":" + port);
            sb.Append("Accept: application/json");

            return sb.ToString();
        }

        private static string BuildConfigPdcSetRequest(
            string ip,
            string body,
            int port)
        {
            return BuildJsonPostRequest(
                "/api/config", ip, body, port);
        }

        private static string BuildConfigPdcResetRequest(
            string ip,
            string body,
            int port)
        {
            return BuildJsonPostRequest(
                "/api/reset", ip, body, port);
        }

        private static string BuildJsonPostRequest(
            string path,
            string ip,
            string body,
            int port)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("POST " + path + " HTTP/1.1");

            sb.AppendLine(
                "Host: " + ip + ":" + port);

            sb.AppendLine(
                "Accept: application/json");

            sb.AppendLine(
                "Content-Type: application/json");

            sb.AppendLine(
                "Content-Length: " +
                Encoding.UTF8.GetByteCount(
                    body ?? string.Empty));

            sb.AppendLine();

            sb.Append(body);

            return sb.ToString();
        }

        // ============================================================
        // PDC ENDPOINT VALIDATION
        // ============================================================

        private bool TryGetPdcEndpoint(
            out string ip,
            out int port)
        {
            ip = txtPdcIp.Text.Trim();

            port = PdcPort;

            if (!IsValidIPv4(ip))
            {
                SetPdcStatus(DeviceState.InvalidIp);

                MessageBox.Show(
                    "No valid PDC IPv4 address is selected. " +
                    "Pick a hub or type an IP.",
                    "Invalid IP",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtPdcIp.Focus();

                return false;
            }

            return true;
        }

        // ============================================================
        // RESET OUTPUT
        // ============================================================

        private void ResetPdcOutput()
        {
            txtPdcSent.Clear();

            txtPdcReceived.Text = NoResponseText;
        }

        // ============================================================
        // STATUS BADGE
        // ============================================================

        private void SetPdcStatus(DeviceState state)
        {
            if (lblPdcStatusBadge != null)
                lblPdcStatusBadge.Visible = false;
        }

        // ============================================================
        // IPV4 VALIDATION
        // ============================================================

        private static bool IsValidIPv4(string text)
        {
            if (!IPAddress.TryParse(
                text,
                out IPAddress addr))
            {
                return false;
            }

            return addr.AddressFamily ==
                   System.Net.Sockets.AddressFamily.InterNetwork
                   && text.Split('.').Length == 4;
        }
    }
}