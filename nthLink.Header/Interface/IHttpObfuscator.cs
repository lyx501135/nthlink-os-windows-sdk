namespace nthLink.Header.Interface
{
    public interface IHttpObfuscator
    {
        event EventHandler<HttpObfuscatorUrlEventArgs>? ObfuscatorUrl;
        void Start(IEnumerable<string> urls, int times, int intervalSecond);
        void Stop();
    }

    public class HttpObfuscatorUrlEventArgs : EventArgs
    {
        public string Url { get; }
        public bool IsRequestGet { get; set; }

        public HttpObfuscatorUrlEventArgs(string url)
        {
            Url = url;
        }
    }
}
