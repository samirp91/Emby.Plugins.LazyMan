using System;
using System.IO;
using System.Threading.Tasks;
using Emby.Plugins.LazyMan.Configuration;
using MediaBrowser.Common.Net;
using MediaBrowser.Model.Logging;

namespace Emby.Plugins.LazyMan.GameApi
{
    public class PowerSportsApi
    {
        private readonly IHttpClient _httpClient;
        private readonly ILogger _logger;
        
        public PowerSportsApi(IHttpClient httpClient, ILogger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<(bool Status, string Response)> GetPlaylistUrlAsync(string league, DateTime date, string mediaId, string cdn)
        {
            var endpoint = $"https://{{0}}/getM3U8.php?league={league}&date={date:yyyy-MM-dd}&id={mediaId}&cdn={cdn}";
            
            var request = new HttpRequestOptions
            {
                Url = string.Format(endpoint, PluginConfiguration.M3U8Url),
                RequestHeaders =
                {
                    // Requires a User-Agent header
                    {"User-Agent", "Mozilla/5.0 Gecko Firefox"}
                }
            };

            _logger.Debug($"[GetStreamUrlAsync] Getting stream url from: {endpoint}");

            var response = await _httpClient.GetResponse(request).ConfigureAwait(false);

            _logger.Debug($"[GetStreamUrlAsync] ResponseCode: {response.StatusCode}");

            string url;
            using (var reader = new StreamReader(response.Content))
            {
                url = await reader.ReadToEndAsync().ConfigureAwait(false);
            }

            _logger.Debug($"[GetStreamUrlAsync] Response: {url}");

            // stream not ready yet
            if (url.Contains("Not"))
            {
                _logger.Warn("[GetStreamUrlAsync] Response contains Not!");
                return (false, url);
            }
                

            // url expired
            if (url.Contains("exp="))
            {
                var expLocation = url.IndexOf("exp=", StringComparison.OrdinalIgnoreCase);
                var expStart = expLocation + 4;
                var expEnd = url.IndexOf("~", expLocation, StringComparison.OrdinalIgnoreCase);
                var expStr = url.Substring(expStart, expEnd - expStart);
                var expiresOn = long.Parse(expStr);
                var currently = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() / 1000;
                if (expiresOn < currently)
                {
                    _logger.Warn("[GetStreamUrlAsync] Stream URL is expired.");
                    return (false, "Stream URL is expired");   
                }
            }
            
            return (true, url);
        }
    }
}