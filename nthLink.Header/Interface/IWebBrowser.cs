using nthLink.Header.Enum;

namespace nthLink.Header.Interface
{
    public interface IWebBrowser
    {
        void OpenUrl(string url, EventSourceTypeEnum eventSourceType);
        void OpenUrlWithoutReport(string url);
    }
}
