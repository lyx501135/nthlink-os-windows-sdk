#pragma warning disable CS8618 // 退出建構函式時，不可為 Null 的欄位必須包含非 Null 值。請考慮宣告為可為 Null。
namespace nthLink.Header.Struct
{
    public class HeadlineNews
    {
        public string title;
        public string excerpt;
        public string image;
        public string url;
        public List<string> categories;
    }

    public class Notification
    {
        public string title;
        public string url;
    }
    //Json Root
    public class DirectoryServerConfig
    {
        public string clientId;
        public List<Server> servers;
        public string redirectUrl;
        public List<HeadlineNews> headlineNews;
        public List<Notification> notifications;
        public List<string> domainKeys;
        public string obfuExcludeHost;
        public string obfuInterval;
        public string obfuMax;
        public List<string> obfuUrls;
        public bool isStatic;
        //TO DO
        //public string custom;
    }

    public class Server
    {
        public string protocol;
        public string host;
        public string port;
        public string password;
        public string sni;
        public string encrypt_method;
        public bool ws;
        public string ws_path;
        public List<string> ips;
        public string ws_host;
    }
}
#pragma warning restore CS8618 // 退出建構函式時，不可為 Null 的欄位必須包含非 Null 值。請考慮宣告為可為 Null。
