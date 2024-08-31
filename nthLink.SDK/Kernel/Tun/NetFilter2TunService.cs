using nthLink.SDK.Extension;
using nthLink.Header.Enum;
using nthLink.Header.Interface;
using nthLink.Header.Struct;
using System.Net;
using System.ServiceProcess;

namespace nthLink.SDK.Kernel.Tun
{
    public class NetFilter2TunService : ITunService
    {
        private const string DriverName = "nfdriver";
        private static readonly ServiceController NFService = new(DriverName);
        private static string SystemDriver => $"{Environment.SystemDirectory}\\drivers\\{DriverName}.sys";
        private static string Driver => Path.Combine(Environment.CurrentDirectory, $"{DriverName}.sys");
        private ServerSettingBase? socks5Server;
        private readonly VpnSettings vpnSettings = new VpnSettings();
        private readonly ILogger logger;
        private readonly IRedirector redirector;
        private readonly IDomainNameService domainNameService;
        private bool isStarted = false;

        public NetFilter2TunService(ILogger logger, IRedirector redirector, IDomainNameService domainNameService)
        {
            this.logger = logger;
            this.redirector = redirector;
            this.domainNameService = domainNameService;
        }

        public async Task<bool> StartAsync(ServerSettingBase server)
        {
            this.logger.Log(LogLevelEnum.Debug, $"{nameof(NetFilter2TunService)} {nameof(StartAsync)}");

            IPAddress? ip = await this.domainNameService.LookupAsync(server.Host);

            if (ip == null)
            {
                this.logger.Log(LogLevelEnum.Error, $"{nameof(NetFilter2TunService)} {nameof(StartAsync)} server ip is null.");

                return false;
            }

            this.socks5Server = server;

            await CheckDriver();

            this.redirector.Dial(RedirectorNameEnum.AIO_FILTERLOOPBACK, this.vpnSettings.FilterLoopback);
            this.redirector.Dial(RedirectorNameEnum.AIO_FILTERINTRANET, this.vpnSettings.FilterIntranet);
            this.redirector.Dial(RedirectorNameEnum.AIO_FILTERPARENT, this.vpnSettings.FilterParent);
            this.redirector.Dial(RedirectorNameEnum.AIO_FILTERICMP, this.vpnSettings.FilterICMP);

            if (this.vpnSettings.FilterICMP)
            {
                this.redirector.Dial(RedirectorNameEnum.AIO_ICMPING, (this.vpnSettings.ICMPDelay).ToString());
            }

            this.redirector.Dial(RedirectorNameEnum.AIO_FILTERTCP, this.vpnSettings.FilterTCP);
            this.redirector.Dial(RedirectorNameEnum.AIO_FILTERUDP, this.vpnSettings.FilterUDP);

            // DNS
            this.redirector.Dial(RedirectorNameEnum.AIO_FILTERDNS, this.vpnSettings.FilterDNS);
            this.redirector.Dial(RedirectorNameEnum.AIO_DNSONLY, this.vpnSettings.HandleOnlyDNS);
            this.redirector.Dial(RedirectorNameEnum.AIO_DNSPROX, this.vpnSettings.DNSProxy);

            if (this.vpnSettings.FilterDNS)
            {
                var dns = IPEndPoint.Parse(this.vpnSettings.DNSHost);
                if (dns.Port == 0)
                    dns.Port = 53;

                this.redirector.Dial(RedirectorNameEnum.AIO_DNSHOST, dns.Address.ToString());
                this.redirector.Dial(RedirectorNameEnum.AIO_DNSPORT, dns.Port.ToString());
            }

            // Server
            this.redirector.Dial(RedirectorNameEnum.AIO_TGTHOST, ip.ToString());
            this.redirector.Dial(RedirectorNameEnum.AIO_TGTPORT, server.Port.ToString());
            //this.redirector.Dial(RedirectorNameEnum.AIO_TGTUSER, server.Username ?? string.Empty);
            //this.redirector.Dial(RedirectorNameEnum.AIO_TGTPASS, server.Password ?? string.Empty);

            // Mode Rule
            if (await Task.Run(DialRule))
            {
                this.isStarted = await this.redirector.InitAsync();

                return this.isStarted;
            }
            else
            {
                return false;
            }
        }

        public Task StopAsync()
        {
            if (this.isStarted)
            {
                return this.redirector.FreeAsync();
            }
            else
            {
                return Task.CompletedTask;
            }
        }

        private bool CheckCppRegex(string r, bool clear = true)
        {
            try
            {
                if (r.StartsWith("!"))
                    return this.redirector.Dial(RedirectorNameEnum.AIO_ADDNAME, r.Substring(1));

                return this.redirector.Dial(RedirectorNameEnum.AIO_ADDNAME, r);
            }
            finally
            {
                if (clear)
                    this.redirector.Dial(RedirectorNameEnum.AIO_CLRNAME, string.Empty);
            }
        }

        public bool CheckRules(IEnumerable<string> rules, out IEnumerable<string> results)
        {
            results = rules.Where(r => !CheckCppRegex(r, false));
            this.redirector.Dial(RedirectorNameEnum.AIO_CLRNAME, string.Empty);
            return !results.Any();
        }

        private bool DialRule()
        {
            this.redirector.Dial(RedirectorNameEnum.AIO_CLRNAME, string.Empty);

            List<string> invalidList = new List<string>();

            if (this.vpnSettings != null && this.vpnSettings.Bypass.Count > 0)
            {
                foreach (var s in this.vpnSettings.Bypass)
                {
                    if (!this.redirector.Dial(RedirectorNameEnum.AIO_BYPNAME, s))
                        invalidList.Add(s);
                }
            }

            if (this.vpnSettings != null && this.vpnSettings.Handle.Count > 0)
            {
                foreach (var s in this.vpnSettings.Handle)
                {
                    if (!this.redirector.Dial(RedirectorNameEnum.AIO_ADDNAME, s))
                        invalidList.Add(s);
                }
            }

            if (invalidList.Count > 0)
            {
                this.logger.Log(LogLevelEnum.Error, GenerateInvalidRulesMessage(invalidList));

                return false;
            }
            else
            {
                this.redirector.Dial(RedirectorNameEnum.AIO_BYPNAME, $"^{Util.GetWorkDirectory().ToRegexString()}");

                return true;
            }
        }

        public static string GenerateInvalidRulesMessage(IEnumerable<string> rules)
        {
            return $"{string.Join("\n", rules)}\nAbove rules does not conform to C++ regular expression syntax";
        }

        private async Task CheckDriver()
        {
            this.logger.Log(LogLevelEnum.Debug, $"{nameof(NetFilter2TunService)} {nameof(CheckDriver)}");

            this.logger.Log(LogLevelEnum.Debug, $"{nameof(NetFilter2TunService)} {nameof(Driver)} = {Driver}");

            Version binFileVersion = Util.GetFileVersion(Driver);

            this.logger.Log(LogLevelEnum.Info, $"Built-in  {DriverName} driver version: {binFileVersion}");

            if (File.Exists(SystemDriver))
            {
                Version systemFileVersion = Util.GetFileVersion(SystemDriver);

                this.logger.Log(LogLevelEnum.Info, $"Installed {DriverName} driver version: {systemFileVersion}");

                if (binFileVersion > systemFileVersion)
                {
                    this.logger.Log(LogLevelEnum.Warn, $"Update {DriverName} driver");

                    await UninstallDriverAsync();

                    await InstallDriverAsync();
                }
            }
            else
            {
                await InstallDriverAsync();
            }
        }

        /// <summary>
        /// 安裝 Driver
        /// </summary>
        public async Task InstallDriverAsync()
        {
            this.logger.Log(LogLevelEnum.Info, $"Install {DriverName} driver, path {Driver}");

            if (!File.Exists(Driver))
            {
                this.logger.Log(LogLevelEnum.Error, "builtin driver files missing, can't install NF driver");
                return;
            }

            try
            {
                File.Copy(Driver, SystemDriver);
            }
            catch (Exception e)
            {
                this.logger.Log(LogLevelEnum.Error, $"Copy {DriverName}.sys failed\n", e);
                return;
            }

            if (await this.redirector.RegisterAsync(DriverName))
            {
                this.logger.Log(LogLevelEnum.Info, $"Install {DriverName} driver finished");
            }
            else
            {
                this.logger.Log(LogLevelEnum.Error, $"Register {DriverName} failed");
            }
        }

        /// <summary>
        ///  反安裝 Driver
        /// </summary>
        public async Task<bool> UninstallDriverAsync()
        {
            this.logger.Log(LogLevelEnum.Info, $"Uninstall {DriverName}");
            try
            {
                if (NFService.Status == ServiceControllerStatus.Running)
                {
                    NFService.Stop();
                    NFService.WaitForStatus(ServiceControllerStatus.Stopped);
                }
            }
            catch { }

            await this.redirector.UnregisterAsync(DriverName);

            if (File.Exists(SystemDriver))
                File.Delete(SystemDriver);

            return true;
        }
    }
}
