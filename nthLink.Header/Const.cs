namespace nthLink.Header
{
    public static class Const
    {
        public static class String
        {
            public const string CompanyUrl = "https://www." + ProductName + ".com";

            public const string ProductName = "nthLink";

            public const string SingleInstanceLock = $"{nameof(nthLink)}_{nameof(SingleInstanceLock)}";

            public const string DefaultPrimaryDNS = "8.8.8.8";

            public const string Start = "Start";

            public const string Stop = "Stop";
        }

        public static class Channel
        {
            public const string Default = "";
            public const string VpnService = "VpnService";
            public const string ProxyStateChange = "ProxyStateChange";
            public const string ProxyRequest = "ProxyRequest";
            public const string PowerEvent = "PowerEvent";
            public const string NetWorkEvent = "NetWorkEvent";
        }
    }
}
