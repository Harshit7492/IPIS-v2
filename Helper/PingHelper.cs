using DataModel;
using System.Net.NetworkInformation;

namespace Helper
{
    public class PingHelper
    {
        public static async Task<PingResult> PingWithRetryAsync(string ip, int timeout = 100, int retry = 2)
        {
            var result = new PingResult
            {
                IP = ip,
                IsSuccess = false,
                LatencyMs = -1,
                Attempts = 0
            };

            for (int i = 1; i <= retry; i++)
            {
                result.Attempts = i;

                try
                {
                    using (Ping ping = new Ping())
                    {
                        var reply = await ping.SendPingAsync(ip, timeout);

                        if (reply.Status == IPStatus.Success)
                        {
                            result.IsSuccess = true;
                            result.LatencyMs = reply.RoundtripTime;
                            return result;  
                        }
                    }
                }
                catch
                {
                     
                } 
                await Task.Delay(10);
            }

            return result;  
        }
    }
}