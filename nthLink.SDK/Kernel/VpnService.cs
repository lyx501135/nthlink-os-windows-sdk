using nthLink.Header;
using nthLink.Header.Enum;
using nthLink.Header.Interface;
using nthLink.Header.Struct;
using nthLink.SDK.Extension;
using nthLink.SDK.Interface;
using nthLink.SDK.Model;
using System.Runtime.InteropServices;

namespace nthLink.SDK.Kernel;

internal class VpnService
{
    private StateEnum state = StateEnum.Waiting;

    private readonly ILogger logger;
    private readonly IDomainNameService domainNameService;
    private readonly IFirewall firewall;
    private readonly IJsonConverter jsonConverter;
    private readonly IClientInfo clientInfo;
    private readonly INetwork network;
    private readonly IContainerProvider containerProvider;
    private readonly IDirectoryServerConfigUpdater? directoryServerConfigProvider;

    private bool cacheConnection;
    /// <summary>
    /// Key:ProxyType
    /// Value:ProxyGuid
    /// </summary>
    private readonly Dictionary<string, string> proxyDictionary = new Dictionary<string, string>();

    private readonly object vpsKernelLocker = new object();

    private IHttpObfuscator? httpObfuscator;
    private IVpsConfigProvider vpsConfigProvider;
    private IEventBus<ProxyMessage<Server>>? proxyEventBus;
    private string proxyGuid = string.Empty;
    public VpnService(IContainerProvider containerProvider,
        IDomainNameService domainNameService,
        IFirewall firewall,
        IJsonConverter jsonConverter,
        IClientInfo clientInfo,
        IVpsConfigProvider configProvider,
        INetwork network,
        ILogger logger)
    {
        this.containerProvider = containerProvider;
        this.logger = logger;

        this.domainNameService = domainNameService;
        this.firewall = firewall;
        this.jsonConverter = jsonConverter;
        this.clientInfo = clientInfo;
        this.network = network;
        network.NetworkAvailabilityChanged += Network_NetworkAvailabilityChanged;

        this.vpsConfigProvider = configProvider;

        this.directoryServerConfigProvider = containerProvider.Resolve<IDirectoryServerConfigUpdater>();

        this.httpObfuscator = containerProvider.Resolve<IHttpObfuscator>();
    }

    public void Init()
    {
        IEventBus<RegisterProxyArgs>? proxyRegisterChannel =
            this.containerProvider.Resolve<IEventBus<RegisterProxyArgs>>();
        if (proxyRegisterChannel != null)
        {
            proxyRegisterChannel.Subscribe(Const.Channel.VpnService, RegisterProxy);
        };

        IEventBus<PowerModeEnum>? powerModeChannel =
            this.containerProvider.Resolve<IEventBus<PowerModeEnum>>();
        if (powerModeChannel != null)
        {
            powerModeChannel.Subscribe(Const.Channel.PowerEvent, OnPowerEvent);
        };

        IEventBus<VpnServiceFunctionArgs>? functionEventBus =
           this.containerProvider.Resolve<IEventBus<VpnServiceFunctionArgs>>();
        if (functionEventBus != null)
        {
            functionEventBus.Subscribe(Const.Channel.VpnService, OnFunctionRequest);
        };
    }

    private async void OnFunctionRequest(string s, VpnServiceFunctionArgs args)
    {
        if (args.Function == FunctionEnum.Start)
        {
            await ConnectAsync();
        }
        else if (args.Function == FunctionEnum.Stop)
        {
            await StopAsync();
        }
    }

    private void RegisterProxy(string channel, RegisterProxyArgs args)
    {
        if (channel == Const.Channel.VpnService)
        {
            IEventBus<ProxyStateArgs>? proxyStateEventBus =
                this.containerProvider.Resolve(typeof(IEventBus<ProxyStateArgs>))
                as IEventBus<ProxyStateArgs>;

            if (this.proxyDictionary.ContainsKey(args.ProxyType))
            {
                if (args.OverrideProxy)
                {
                    if (proxyStateEventBus != null)
                    {
                        proxyStateEventBus.Unsubscribe(this.proxyDictionary[args.ProxyType], ProxyStateChangedHandler);
                        proxyStateEventBus.Subscribe(args.ProxyGuid, ProxyStateChangedHandler);
                    }
                    this.proxyDictionary[args.ProxyType] = args.ProxyGuid;
                }
            }
            else
            {
                if (proxyStateEventBus != null)
                {
                    proxyStateEventBus.Subscribe(args.ProxyGuid, ProxyStateChangedHandler);
                }
                this.proxyDictionary.Add(args.ProxyType, args.ProxyGuid);
            }
        }
    }
    private void ProxyStateChangedHandler(string channel, ProxyStateArgs args)
    {
        switch (args.State)
        {
            case StateEnum.Started:
                {
                    if (this.httpObfuscator != null)
                    {
                        if (this.directoryServerConfigProvider != null &&
                            this.directoryServerConfigProvider.DirectoryServerConfig is DirectoryServerConfig config &&
                            int.TryParse(config.obfuMax, out int times) &&
                                int.TryParse(config.obfuInterval, out int intervalSeconds)
                            )
                        {
                            this.httpObfuscator.Start(config.obfuUrls, times, intervalSeconds);
                        }
                    }

                    StateChanged(StateEnum.Started, string.Empty);
                }
                break;
            case StateEnum.Stopped:
                {
                    if (this.httpObfuscator != null)
                    {
                        this.httpObfuscator.Stop();
                    }

                    StateChanged(StateEnum.Stopped, string.Empty);
                }
                break;
            case StateEnum.Waiting:
                break;
            case StateEnum.Starting:
                break;
            case StateEnum.Stopping:
                break;
            case StateEnum.Terminating:
                break;
        }
    }
    private async void OnPowerEvent(string channel, PowerModeEnum e)
    {
        this.logger.Log(LogLevelEnum.Info, $"{nameof(OnPowerEvent)} {e}");

        if (e == PowerModeEnum.Suspend)
        {
            if (this.state == StateEnum.Started)
            {
                await StopAsync();

                this.cacheConnection = true;
            }

            this.logger.Log(LogLevelEnum.Info, $"{nameof(this.cacheConnection)} is {this.cacheConnection}");
        }
        else if (e == PowerModeEnum.Resume)
        {
            this.logger.Log(LogLevelEnum.Info, $"{nameof(this.cacheConnection)} is {this.cacheConnection}");

            if (this.cacheConnection)
            {
                await ConnectAsync();

                this.cacheConnection = false;
            }
        }
    }
    private async void Network_NetworkAvailabilityChanged(object? sender, NetworkEventArgs e)
    {
        this.logger.Log(LogLevelEnum.Info, $"{nameof(e.IsNetworkAvailable)} = {e.IsNetworkAvailable}");

        if (e.IsNetworkAvailable)
        {
            if (this.cacheConnection)
            {
                await ConnectAsync();

                this.cacheConnection = false;
            }
        }
        else
        {
            if (this.state == StateEnum.Started)
            {
                this.cacheConnection = true;

                await StopAsync();
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns>Error message, return string.Empty when success.</returns>
    public async Task ConnectAsync()
    {
        if (this.state == StateEnum.Starting ||
            this.state == StateEnum.Started)
        {
            return;
        }

        string result = string.Empty;

        StateChanged(StateEnum.Starting, string.Empty);

        if (!this.network.IsNetworkAvailable)
        {
            result = "Network is not available";

            this.logger.Log(LogLevelEnum.Error, result);

            StateChanged(StateEnum.Terminating, result);

            return;
        }

        string configString = this.vpsConfigProvider.GetConfig();

        DirectoryServerConfig? directoryServerConfig = this.jsonConverter.Deserialize<DirectoryServerConfig>(configString);

        if (directoryServerConfig == null)
        {
            result = "Get config fail";

            this.logger.Log(LogLevelEnum.Error, result);

            StateChanged(StateEnum.Terminating, result);
        }
        else
        {
            result = await RunProxy(directoryServerConfig);

            if (!string.IsNullOrEmpty(result))
            {
                this.logger.Log(LogLevelEnum.Error, result);

                StateChanged(StateEnum.Terminating, result);
            }
        }
    }

    private async Task<string> RunProxy(DirectoryServerConfig directoryServerConfig)
    {
        if (this.directoryServerConfigProvider != null)
        {
            this.directoryServerConfigProvider.UpdateDirectoryServerConfig(directoryServerConfig);
        }

        if (directoryServerConfig.domainKeys != null &&
            this.clientInfo is ClientInfoImp clientInfoImp)
        {
            clientInfoImp.UpdateDomainKeys(directoryServerConfig.domainKeys);
        }

        Random random = new Random(DateTime.Now.Millisecond);

        int index = random.Next(0, directoryServerConfig.servers.Count);

        Server server = directoryServerConfig.servers[index];

        if (await this.domainNameService.LookupAsync(server.host) == null)
        {
            string msg = $"Lookup Server hostname failed";
            this.logger.Log(LogLevelEnum.Error, msg);
            return msg;
        }

        if (this.proxyDictionary.ContainsKey(server.protocol))
        {
            if (this.containerProvider.Resolve<IEventBus<ProxyMessage<Server>>>()
                is IEventBus<ProxyMessage<Server>> eventBus)
            {
                await StartAsync(this.proxyDictionary[server.protocol], eventBus, server);

                return string.Empty;
            }
        }

        return $"Protocol not exist : {server.protocol}";
    }

    [DllImport("dnsapi")]
    static extern uint DnsFlushResolverCache();

    private async Task StartAsync(string proxyGuid, IEventBus<ProxyMessage<Server>> eventBus, Server server)
    {
        await Task.WhenAll(Task.Run(DnsFlushResolverCache), Task.Run(firewall.Open));

        this.logger.Log(LogLevelEnum.Info, $"Start proxy: {server.protocol} ");

        eventBus.Publish(Const.Channel.ProxyRequest, new ProxyMessage<Server>()
        {
            ProxyGuid = proxyGuid,
            Message = Const.String.Start,
            Args = server,
        });

        this.proxyGuid = proxyGuid;
        this.proxyEventBus = eventBus;
    }

    public Task StopAsync()
    {
        if (this.state == StateEnum.Stopping ||
            this.state == StateEnum.Stopped)
        {
            return Task.CompletedTask;
        }

        if (!string.IsNullOrEmpty(this.proxyGuid) &&
            this.proxyEventBus is IEventBus<ProxyMessage<Server>> bus)
        {
            this.logger.Log(LogLevelEnum.Info, $"Stop proxy");

            StateChanged(StateEnum.Stopping);

            bus.Publish(Const.Channel.ProxyRequest, new ProxyMessage<Server>()
            {
                ProxyGuid = this.proxyGuid,
                Message = Const.String.Stop,
            }); ;

            this.proxyGuid = string.Empty;
            this.proxyEventBus = null;

            StateChanged(StateEnum.Stopped);
        }
        else
        {
            this.logger.Log(LogLevelEnum.Error, $"StopAsync: Current Vps is not exist");
        }

        return Task.CompletedTask;
    }

    private void StateChanged(StateEnum state, string message = "")
    {
        this.state = state;

        if (this.containerProvider.Resolve<IEventBus<VpnServiceStateArgs>>()
                   is IEventBus<VpnServiceStateArgs> eventBus)
        {
            eventBus.Publish(Const.Channel.VpnService,
                new VpnServiceStateArgs(state, message));
        }
    }
}