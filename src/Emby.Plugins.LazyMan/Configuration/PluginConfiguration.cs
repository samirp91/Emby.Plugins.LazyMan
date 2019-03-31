using System.Collections.Generic;
using MediaBrowser.Model.Plugins;

namespace Emby.Plugins.LazyMan.Configuration
{
    public class PluginConfiguration : BasePluginConfiguration
    {
        public const string M3U8URL = "freegamez.ga";
        
        // Quality : Bitrate
        public static readonly Dictionary<string, (string Title, string File)> FeedQualities =
            new Dictionary<string, (string Title, string File)>
            {
                {"450", ("216p", "450K/450_{0}.m3u8")},
                {"800", ("288p", "800/800_{0}.m3u8")},
                {"1200", ("360p", "1200K/1200_{0}.m3u8")},
                {"1800", ("504p", "1800K/1800_{0}.m3u8")},
                {"2500", ("540p", "2500K/2500_{0}.m3u8")},
                {"3500", ("720p", "3500K/3500_{0}.m3u8")},
                {"5600", ("720p 60fps", "5600K/5600_{0}.m3u8")}
            };
        
        // TODO allow user to change
        // l3c = Level 3
        // akc = Akamai
        public const string Cdn = "l3c";
        
    }
}