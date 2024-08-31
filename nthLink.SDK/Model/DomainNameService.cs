using nthLink.Header.Enum;
using nthLink.Header.Interface;
using System.Net;
using System.Net.Sockets;

namespace nthLink.SDK.Model
{
    public class DomainNameService : IDomainNameService
    {
        /// <summary>
        /// 暫存 IPV4
        /// </summary>
        private readonly Dictionary<string, IPAddress> cache4 = new();
        /// <summary>
        /// 暫存 IPV6
        /// </summary>
        private readonly Dictionary<string, IPAddress> cache6 = new();
        private readonly ILogger logger;

        public DomainNameService(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task<IPAddress?> LookupAsync(string hostname, AddressFamily inet = AddressFamily.Unspecified, int timeout = 3000)
        {
            try
            {
                switch (inet)
                {
                    case AddressFamily.Unspecified:
                        if (this.cache4.ContainsKey(hostname))
                        {
                            return this.cache4[hostname];
                        }
                        else if (this.cache6.ContainsKey(hostname))
                        {
                            return this.cache6[hostname];
                        }
                        break;
                    case AddressFamily.InterNetwork:
                        if (this.cache4.ContainsKey(hostname))
                        {
                            return this.cache4[hostname];
                        }
                        break;
                    case AddressFamily.InterNetworkV6:
                        if (this.cache6.ContainsKey(hostname))
                        {
                            return this.cache6[hostname];
                        }
                        break;
                    default:
                        return null;
                }

                IPAddress? iPAddress = await LookupNoCacheAsync(hostname, inet, timeout);

                if (iPAddress != null)
                {
                    Dictionary<string, IPAddress> cache;

                    if (iPAddress.AddressFamily == AddressFamily.Unspecified ||
                        iPAddress.AddressFamily == AddressFamily.InterNetwork)
                    {
                        cache = this.cache4;
                    }
                    else
                    {
                        cache = this.cache6;
                    }

                    cache.TryAdd(hostname, iPAddress);
                    return cache[hostname];
                }
            }
            catch (Exception e)
            {
                this.logger.Log(LogLevelEnum.Error, $"Lookup hostname {hostname} failed", e);
            }

            return null;
        }

        private static async Task<IPAddress?> LookupNoCacheAsync(string hostname, AddressFamily inet = AddressFamily.Unspecified, int timeout = 3000)
        {
            IPAddress[] iPAddresses = await Dns.GetHostAddressesAsync(hostname);

            if (iPAddresses != null)
            {
                return iPAddresses.FirstOrDefault(i =>
                inet == AddressFamily.Unspecified ||
                inet == i.AddressFamily);
            }

            return null;
        }

        public void ClearCache()
        {
            this.cache4.Clear();
            this.cache6.Clear();
        }
    }
}
