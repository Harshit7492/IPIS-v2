using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LogManager;

namespace DataBaseHandler
{
    public class CapInfo
    {
        private readonly string station_code;
        private readonly string user_key;
        private readonly string user_name;
        private readonly string user_password;
        private readonly string audioFilePath;

        private static readonly HttpClient httpClient = new HttpClient();
        public CapInfo(string _station_code, string _user_key, string _user_name, string _user_password)
        {
            station_code = _station_code;
            user_key = _user_key;
            user_name = _user_name;
            user_password = _user_password;
            audioFilePath = Path.Combine(AppContext.BaseDirectory, "CAP_AUDIO");

        }
        public async Task<string> Download_AudioFile(List<string> audioMsg)
        {
            var audioFileNames = new StringBuilder();

            try
            {
                for (int i = 0; i < audioMsg.Count; i++)
                {
                    if (string.IsNullOrEmpty(audioMsg[i]))
                        continue;

                    using var request = new HttpRequestMessage(HttpMethod.Post, audioMsg[i]);
                    AddHeaders(request);

                    var response = await httpClient.SendAsync(request);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        if (!Directory.Exists(audioFilePath))
                            Directory.CreateDirectory(audioFilePath);

                        var bytes = await response.Content.ReadAsByteArrayAsync();

                        string fileName = $"CAPAUDIO_{DateTime.Now:ddMMMyyyyHHmmss}{i}.wav";
                        string fullPath = Path.Combine(audioFilePath, fileName);

                        await File.WriteAllBytesAsync(fullPath, bytes);
                        audioFileNames.Append(fileName + "$");
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerService.CapLogger.Error(ex, ex.Message);
                return string.Empty;
            }
            return audioFileNames.ToString();
        }

        public async Task<string> Check_CapData(string capUrl)
        {
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, capUrl);
                AddHeaders(request);

                var response = await httpClient.SendAsync(request);
                return response.StatusCode == HttpStatusCode.OK
                    ? await response.Content.ReadAsStringAsync()
                    : response.ToString();
            }
            catch (Exception ex)
            {
                LoggerService.CapLogger.Error(ex, ex.Message);
                return string.Empty;
            }
        }

        public async Task<string> Validate_Message(string alert_identifier, string validation_status, string url)
        {
            try
            {
                var ack = new Ack
                {
                    alert_identifier = alert_identifier,
                    validation_status = validation_status
                };

                var json = JsonSerializer.Serialize(ack);
                using var request = new HttpRequestMessage(HttpMethod.Post, url);
                AddHeaders(request);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string timestamp = DateTime.Now.AddMinutes(1)
                        .ToString("yyyy-MM-dd HH:mm:ss.fff");

                    return $"{timestamp}${await response.Content.ReadAsStringAsync()}";
                }
                return response.ToString();
            }
            catch (Exception ex)
            {
                LoggerService.CapLogger.Error(ex, ex.Message);
                return string.Empty;
            }
        }

        public async Task<string> SubmitReport(string url, string alert_identifier, string start_time, string message_display_count, string audio_announcement_count, string status, string failure_reason, string end_time)
        {
            try
            {
                var feedback = new
                {
                    alert_identifier,
                    station_code,
                    start_time,
                    end_time,
                    message_display_count,
                    audio_announcement_count,
                    status,
                    failure_reason
                };

                var json = JsonSerializer.Serialize(feedback);
                using var request = new HttpRequestMessage(HttpMethod.Post, url);
                AddHeaders(request);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(request);

                return response.StatusCode == HttpStatusCode.OK
                    ? await response.Content.ReadAsStringAsync()
                    : response.ToString();
            }
            catch (Exception ex)
            {
                LoggerService.CapLogger.Error(ex, ex.Message);
                return string.Empty;
            }
        }

        private void AddHeaders(HttpRequestMessage request)
        {
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("station_code", station_code);
            request.Headers.Add("user_key", user_key);
            var authBytes = Encoding.UTF8.GetBytes($"{user_name}:{user_password}");
            var authToken = Convert.ToBase64String(authBytes);
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authToken);
        }
    }
    public class Ack
    {
        public string alert_identifier { get; set; }
        public string validation_status { get; set; }
    }
}
