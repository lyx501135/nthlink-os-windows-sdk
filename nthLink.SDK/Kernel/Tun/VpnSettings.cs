using nthLink.Header;

namespace nthLink.SDK.Kernel.Tun
{
    internal class VpnSettings
    {
        public bool FilterTCP { get; set; } = true;
        public bool FilterUDP { get; set; } = true;
        public bool FilterDNS { get; set; } = false;
        public bool FilterParent { get; set; } = true;
        public bool HandleOnlyDNS { get; set; } = true;
        public bool DNSProxy { get; set; } = true;
        public string DNSHost { get; set; } = $"{Const.String.DefaultPrimaryDNS}:53";
        public int ICMPDelay { get; set; } = 10;
        public bool FilterICMP { get; set; } = false;
        public bool FilterLoopback { get; set; } = false;
        public bool FilterIntranet { get; set; } = true;
        public List<string> Bypass { get; set; } = new List<string>()
        {
            "BitComet", // https://www.bitcomet.com/tw
            "uTorrent", // https://www.utorrent.com/intl/zh_tw/
            "BitTorrent", // https://www.bittorrent.com/
            "qbittorrent", // https://www.qbittorrent.org/
            "transmission-daemon", // https://transmissionbt.com/
            "deluged", // https://deluge-torrent.org/
            "vuze", // https://www.vuze.com/
            "FrostWire", // https://www.frostwire.com/
            "emule", // https://www.emule-project.net/home/perl/general.cgi?l=1
            "Shareaza", // http://shareaza.sourceforge.net/
            "Freenet", // https://freenetproject.org/
            "Soulseek" // https://www.slsknet.org/
        };
        public List<string> Handle { get; set; } = new List<string>() { ".*" };
    }
}
