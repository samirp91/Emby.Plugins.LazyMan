using System.Net;
using Emby.Plugins.LazyMan.Configuration;

namespace Emby.Plugins.LazyMan.Utils
{
    public static class PingTest
    {
        /*
         * mf.svc.nhl.com
         * mlb-ws-mf.media.mlb.com
         * playback.svcs.mlb.com
         */
        
        public static bool IsMatch(string testHost)
        {
            var validIp = Dns.GetHostAddresses(PluginConfiguration.M3U8URL)[0];
            var testIp = Dns.GetHostAddresses(testHost)[0];

            return Equals(validIp, testIp);
        }
    }
}