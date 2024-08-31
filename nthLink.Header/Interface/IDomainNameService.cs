using System.Net;
using System.Net.Sockets;

namespace nthLink.Header.Interface
{
    public interface IDomainNameService
    {
        public Task<IPAddress?> LookupAsync(string hostname, AddressFamily inet = AddressFamily.Unspecified, int timeout = 3000);
        public void ClearCache();
    }
}
