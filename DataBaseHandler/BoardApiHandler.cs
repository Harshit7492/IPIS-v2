using LogManager;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace DataBaseHandler
{

    public class BoardApiHandler
    {
        private static readonly HttpClient _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(15) };
        public static BoardApiHandler Instance { get; } = new BoardApiHandler();
        private BoardApiHandler() { }
        public async Task<string> GetConfigurationAsync(string boardIp)
        {
            if (string.IsNullOrWhiteSpace(boardIp))
                return "No board IP.";

            string ip = boardIp.Trim();
            string url = "http://" + ip + "/IPIS/GetConf";

            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.TryAddWithoutValidation("Accept", "*/*");
                AddStationCodeHeader(request);

                using var response = await _httpClient.SendAsync(request);
                string body = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    LoggerService.Logger.Error(
                        "GET " + url + " returned " + (int)response.StatusCode + " " + response.ReasonPhrase);

                    return "HTTP " + (int)response.StatusCode + " " + response.ReasonPhrase
                           + Environment.NewLine + Environment.NewLine + body;
                }

                return body;
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, "GET " + url + " failed: " + ex.Message);
                return "Request failed: " + ex.Message;
            }
        }

        public async Task<string> SetConfigurationAsync(string boardIp, string intValue, string dtoValue)
        {
            if (string.IsNullOrWhiteSpace(boardIp))
                return "No board IP.";

            string ip = boardIp.Trim();
            //string url = "http://" + ip + "/IPIS/SetConfig";
            string url = "http://" + ip + "/IPIS/SetConf";
            string body = BuildSetConfigBody(intValue, dtoValue);

            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.TryAddWithoutValidation("Accept", "*/*");
                //request.Headers.TryAddWithoutValidation("X-Requested-With", "ipis");
                AddStationCodeHeader(request);

                request.Content = new StringContent(body, Encoding.UTF8);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                using var response = await _httpClient.SendAsync(request);
                string responseBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    LoggerService.Logger.Error(
                        "POST " + url + " " + body + " returned " + (int)response.StatusCode + " " + response.ReasonPhrase);

                    return "HTTP " + (int)response.StatusCode + " " + response.ReasonPhrase
                           + Environment.NewLine + Environment.NewLine + responseBody;
                }

                return responseBody;
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, "POST " + url + " failed: " + ex.Message);
                return "Request failed: " + ex.Message;
            }
        }

        private static void AddStationCodeHeader(HttpRequestMessage request)
        {
            string stationCode = GetStationCode();

            if (string.IsNullOrWhiteSpace(stationCode))
            {
                LoggerService.Logger.Warning("Board API: STATION_CODE is empty in ADMINSETTING - Station-Code header not sent.");
                return;
            }

            request.Headers.TryAddWithoutValidation("Station-Code", stationCode);
        }

        public static string GetStationCode()
        {
            return _dbService.GET_ADMIN_SETTING()?.STATION_CODE?.Trim() ?? string.Empty;
        }

        public static string BuildSetConfigBody(string intValue, string dtoValue)
        {
            var values = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(intValue)) values["INT"] = intValue.Trim();
            if (!string.IsNullOrWhiteSpace(dtoValue)) values["DTO"] = dtoValue.Trim();

            return JsonSerializer.Serialize(values);
        }

        #region PDC (Platform Data Controller) API

        public async Task<string> GetPdcDevicesAsync(string pdcIp, int port = 8000)
        {
            if (string.IsNullOrWhiteSpace(pdcIp))
                return "No PDC IP specified.";

            string ip = pdcIp.Trim();
            string url = "http://" + ip + ":" + port + "/api/devices";

            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.TryAddWithoutValidation("Accept", "application/json");

                using var response = await _httpClient.SendAsync(request);
                string body = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    LoggerService.Logger.Error(
                        "GET " + url + " returned " + (int)response.StatusCode + " " + response.ReasonPhrase);

                    return "HTTP " + (int)response.StatusCode + " " + response.ReasonPhrase
                           + Environment.NewLine + Environment.NewLine + body;
                }

                // Pretty print JSON response for display
                try
                {
                    using var doc = JsonDocument.Parse(body);
                    return JsonSerializer.Serialize(doc.RootElement, new JsonSerializerOptions { WriteIndented = true });
                }
                catch
                {
                    return body;
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, "GET " + url + " failed: " + ex.Message);
                return "Request to PDC failed: " + ex.Message;
            }
        }

        public async Task<string> GetPdcDeviceConfigAsync(string pdcIp, string targetIp, int port = 8000)
        {
            if (string.IsNullOrWhiteSpace(pdcIp) || string.IsNullOrWhiteSpace(targetIp))
                return "PDC IP or Target IP missing.";

            string url = "http://" + pdcIp.Trim() + ":" + port + "/api/config?ip_address=" + targetIp.Trim();
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.TryAddWithoutValidation("Accept", "application/json");

                using var response = await _httpClient.SendAsync(request);
                string body = await response.Content.ReadAsStringAsync();

                try
                {
                    using var doc = JsonDocument.Parse(body);
                    return JsonSerializer.Serialize(doc.RootElement, new JsonSerializerOptions { WriteIndented = true });
                }
                catch
                {
                    return body;
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, "GET " + url + " failed: " + ex.Message);
                return "Request failed: " + ex.Message;
            }
        }

        public class PdcSetResult
        {
            public bool Success { get; set; }
            public List<string> ConfiguredIps { get; set; } = new List<string>();
            public string FormattedSummary { get; set; }
        }

        public async Task<PdcSetResult> SetPdcConfigurationAsync(string pdcIp, string intValue, string dtoValue, int port = 8000)
        {
            var result = new PdcSetResult();
            if (string.IsNullOrWhiteSpace(pdcIp))
            {
                result.FormattedSummary = "No PDC IP specified.";
                return result;
            }

            string ip = pdcIp.Trim();
            var targetIps = new List<string>();

            // 1. Discover devices under this PDC via REST API
            try
            {
                string devicesJson = await GetPdcDevicesAsync(ip, port);
                using (var doc = JsonDocument.Parse(devicesJson))
                {
                    if (doc.RootElement.TryGetProperty("devices", out var devArray) && devArray.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var dev in devArray.EnumerateArray())
                        {
                            if (dev.TryGetProperty("ip", out var ipProp))
                            {
                                string devIp = ipProp.GetString()?.Trim();
                                if (!string.IsNullOrEmpty(devIp) && !targetIps.Contains(devIp))
                                {
                                    targetIps.Add(devIp);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Warning(ex, "Failed to parse discovered devices from PDC " + ip + ": " + ex.Message);
            }

            // Fallback: if no devices found via PDC discovery, check database for CGDB boards
            if (targetIps.Count == 0)
            {
                try
                {
                    var boards = _dbService.GET_BOARD_SETTING(0);
                    if (boards != null)
                    {
                        foreach (var b in boards)
                        {
                            if (!string.IsNullOrWhiteSpace(b.BOARD_IP) &&
                                (b.DISPLAY_TYPE?.ToUpperInvariant().Contains("CGDB") == true ||
                                 b.BOARD_NAME?.ToUpperInvariant().Contains("CGDB") == true))
                            {
                                string devIp = b.BOARD_IP.Trim();
                                if (!targetIps.Contains(devIp))
                                    targetIps.Add(devIp);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggerService.Logger.Warning(ex, "Failed to fallback to database boards: " + ex.Message);
                }
            }

            if (targetIps.Count == 0)
            {
                result.FormattedSummary = "No CGDB devices discovered under PDC (" + ip + ") and none found in database.";
                return result;
            }

            var sb = new StringBuilder();
            sb.AppendLine("PDC Configuration Broadcast (" + ip + ":" + port + "):");
            sb.AppendLine("Settings -> Intensity: " + (!string.IsNullOrEmpty(intValue) ? intValue : "no change")
                          + ", DTO: " + (!string.IsNullOrEmpty(dtoValue) ? dtoValue + "s" : "no change"));
            sb.AppendLine("Target Devices: " + string.Join(", ", targetIps));
            sb.AppendLine(new string('-', 55));

            int successCount = 0;

            foreach (var devIp in targetIps)
            {
                // Attempt 1: Try PDC API POST /api/config
                try
                {
                    using var pdcReq = new HttpRequestMessage(HttpMethod.Post, "http://" + ip + ":" + port + "/api/config");
                    var pdcDict = new Dictionary<string, string> { { "ip_address", devIp } };
                    if (!string.IsNullOrEmpty(intValue)) pdcDict["INT"] = intValue;
                    if (!string.IsNullOrEmpty(dtoValue)) pdcDict["DTO"] = dtoValue;
                    pdcReq.Content = new StringContent(JsonSerializer.Serialize(pdcDict), Encoding.UTF8, "application/json");
                    await _httpClient.SendAsync(pdcReq);
                }
                catch
                {
                    // Ignore PDC API set failures, direct hardware call follows
                }

                // Attempt 2: Direct call to device /IPIS/SetConf on port 80
                string directResp = await SetConfigurationAsync(devIp, intValue, dtoValue);
                bool ok = IsDirectSetOk(directResp);

                if (ok)
                {
                    successCount++;
                    result.ConfiguredIps.Add(devIp);
                    sb.AppendLine("[SUCCESS] " + devIp + ": Configured (INT=" + intValue + ", DTO=" + dtoValue + ")");
                }
                else
                {
                    sb.AppendLine("[FAILED]  " + devIp + ": " + directResp);
                }
            }

            sb.AppendLine(new string('-', 55));
            sb.AppendLine("Completed: " + successCount + "/" + targetIps.Count + " devices configured successfully.");

            result.Success = successCount > 0;
            result.FormattedSummary = sb.ToString();
            return result;
        }

        public async Task<string> SetSinglePdcDeviceConfigurationAsync(string pdcIp, string devIp, string intValue, string dtoValue, int port = 8000)
        {
            if (string.IsNullOrWhiteSpace(devIp))
                return "No target device IP specified.";

            // 1. Try PDC API POST /api/config
            if (!string.IsNullOrWhiteSpace(pdcIp))
            {
                try
                {
                    using var pdcReq = new HttpRequestMessage(HttpMethod.Post, "http://" + pdcIp.Trim() + ":" + port + "/api/config");
                    var pdcDict = new Dictionary<string, string> { { "ip_address", devIp.Trim() } };
                    if (!string.IsNullOrEmpty(intValue)) pdcDict["INT"] = intValue;
                    if (!string.IsNullOrEmpty(dtoValue)) pdcDict["DTO"] = dtoValue;
                    pdcReq.Content = new StringContent(JsonSerializer.Serialize(pdcDict), Encoding.UTF8, "application/json");
                    await _httpClient.SendAsync(pdcReq);
                }
                catch
                {
                    // Ignore PDC API failures, direct hardware call follows
                }
            }

            // 2. Direct call to device port 80 /IPIS/SetConf
            return await SetConfigurationAsync(devIp, intValue, dtoValue);
        }

        private static bool IsDirectSetOk(string response)
        {
            if (string.IsNullOrWhiteSpace(response)) return false;
            try
            {
                using var doc = JsonDocument.Parse(response);
                if (doc.RootElement.TryGetProperty("status", out var st) &&
                    string.Equals(st.GetString(), "OK", StringComparison.OrdinalIgnoreCase))
                    return true;
                if (doc.RootElement.TryGetProperty("success", out var sc) && sc.GetBoolean())
                    return true;
            }
            catch { }
            return false;
        }

        #endregion
    }
}
