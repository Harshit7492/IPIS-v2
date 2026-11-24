using DataModel;
using LogManager;
using System.Text;

namespace DataBaseHandler
{
    public class HttpHandler
    {
      
        private static readonly HttpClient _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(15) };

        public static HttpHandler Instance { get; } = new HttpHandler();
        private HttpHandler() { }

        public string GetMasterTrains(AdminSetting _adminSetting)
        {
            try
            {
                string url = "https://enquiry.indianrail.gov.in/ntessrvc/StationTrainTimeTable";
                string postData = $"{{\"station\":\"{_adminSetting.STATION_CODE}\"}}";

                using var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.TryAddWithoutValidation("authToken", _adminSetting.PASS_KEY);
                request.Content = new StringContent(postData, Encoding.UTF8, "application/json");

                using var response = _httpClient.SendAsync(request).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();
                LoggerService.NtesLogger.Information("NTES GetMasterTrains successful. Station: {Station}, StatusCode: {StatusCode}",_adminSetting.STATION_CODE,(int)response.StatusCode);
                return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                LoggerService.NtesLogger.Error(ex,"NTES GetMasterTrains failed. Station: {Station}",_adminSetting?.STATION_CODE);
               return string.Empty;
            }
        }

        public string GetTrainFromNTES(AdminSetting _adminSetting)
        {
            try
            {
                string url = "https://enquiry.indianrail.gov.in/ntessrvc/LiveStation";
                string postData = $"{{\"station\":\"{_adminSetting.STATION_CODE}\",\"nextMins\":\"{_adminSetting.NtesTimeMin}\"}}";

                using var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.TryAddWithoutValidation("authToken", _adminSetting.PASS_KEY);
                request.Content = new StringContent(postData, Encoding.UTF8, "application/json");

                using var response = _httpClient.SendAsync(request).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();
                LoggerService.NtesLogger.Information( "NTES GetTrainFromNTES successful. Station: {Station}, StatusCode: {StatusCode}", _adminSetting.STATION_CODE, (int)response.StatusCode);
                return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                LoggerService.NtesLogger.Error(ex,"NTES GetTrainFromNTES failed. Station: {Station}",_adminSetting?.STATION_CODE);
                return null;
            }
        }


        public async Task<bool> UPDATE_BACKWARD_PLATEFORM(train_updated_data trainInfo, string _oldPF, string _newPF, string _stationcode, string passkey)
        {
            try
            {
                DateTime runDate = trainInfo.STD > new DateTime(2000, 1, 1) ? trainInfo.STD.Date : DateTime.Today;

                string FormatTime(string timeStr)
                {
                    if (!string.IsNullOrWhiteSpace(timeStr) && DateTime.TryParse(timeStr, out DateTime t))
                        return new DateTime(runDate.Year, runDate.Month, runDate.Day, t.Hour, t.Minute, 0).ToString("dd-MMM-yyyy HH:mm");
                    return string.Empty;
                }

                var payload = new
                {
                    trainNo = trainInfo.TRAIN_NUMBER,
                    startDate = runDate.ToString("dd-MMM-yyyy"),
                    jStation = _stationcode,
                    jStationSrNo = trainInfo.stnSrNo > 0 ? trainInfo.stnSrNo : 1,
                    jStationSTA = FormatTime(trainInfo.SCHEDULE_ARRIVAL_TIME),
                    jStationSTD = FormatTime(trainInfo.SCHEDULE_DEPARTURE_TIME),
                    NTES_OLD_PF = !string.IsNullOrEmpty(_oldPF) ? _oldPF : (trainInfo.ACTUAL_PLATFORM_NUMBER ?? "1"),
                    NTES_NEW_PF = _newPF
                };

                using var request = new HttpRequestMessage(HttpMethod.Post, "https://enquiry.indianrail.gov.in/ntessrvc/IPISUpdateTrainDynamicPF");
                request.Headers.TryAddWithoutValidation("authToken", passkey);
                request.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

                using var response = await _httpClient.SendAsync(request);
                string responseBody = await response.Content.ReadAsStringAsync();

                LoggerService.NtesLogger.Information($"NTES Platform Update Response: {responseBody}");

                return !string.IsNullOrEmpty(responseBody) &&
                       (responseBody.Contains("\"serviceDataResultFlag\":\"Y\"") ||
                        responseBody.IndexOf("successfully", StringComparison.OrdinalIgnoreCase) >= 0);
            }
            catch (Exception ex)
            {
                LoggerService.NtesLogger.Error(ex, "Error in UPDATE_BACKWARD_PLATEFORM: " + ex.Message);
                return false;
            }
        }

    }
}
