using nthLink.Header.Enum;

namespace nthLink.Header.Interface
{
    public interface IRedirector
    {
        public Task<bool> RegisterAsync(string path);
        public Task<bool> UnregisterAsync(string path);
        public bool Dial(RedirectorNameEnum name, bool value);
        public bool Dial(RedirectorNameEnum name, string value);
        public Task<bool> InitAsync();
        public Task<bool> FreeAsync();
    }
}
