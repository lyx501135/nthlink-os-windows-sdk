using nthLink.Header.Interface;
using nthLink.SDK.Interface;
using nthLink.SDK.Kernel;
using nthLink.SDK.Kernel.Tun;
using nthLink.SDK.Model;
using System.Text;

namespace nthLink.SDK.Extension
{
    public static class IContainerRegistryExtension
    {
        public static IContainerRegistry Register<T>(this IContainerRegistry containerRegistry)
        {
            return containerRegistry.Register(typeof(T));
        }
        public static IContainerRegistry Register<TFrom, TTo>(this IContainerRegistry containerRegistry) where TTo : TFrom
        {
            return containerRegistry.Register(typeof(TFrom), typeof(TTo));
        }
        public static IContainerRegistry RegisterInstance<T>(this IContainerRegistry containerRegistry, T instance)
        {
            return instance == null ? containerRegistry : containerRegistry.RegisterInstance(typeof(T), instance);
        }
        public static IContainerRegistry RegisterSingleton<TFrom, TTo>(this IContainerRegistry containerRegistry) where TTo : TFrom
        {
            return containerRegistry.RegisterSingleton(typeof(TFrom), typeof(TTo));
        }
        public static IContainerRegistry RegisterSingleton<T>(this IContainerRegistry containerRegistry)
        {
            return containerRegistry.RegisterSingleton(typeof(T), typeof(T));
        }
        public static IContainerRegistry Register<T>(this IContainerRegistry containerRegistry, string name)
        {
            return containerRegistry.Register(typeof(T), name);
        }
        public static IContainerRegistry Register<TFrom, TTo>(this IContainerRegistry containerRegistry, string name) where TTo : TFrom
        {
            return containerRegistry.Register(typeof(TFrom), typeof(TTo), name);
        }
        public static IContainerRegistry RegisterInstance<T>(this IContainerRegistry containerRegistry, T instance, string name)
        {
            return instance == null ? containerRegistry : containerRegistry.RegisterInstance(typeof(T), instance, name);
        }
        public static IContainerRegistry RegisterSingleton<TFrom, TTo>(this IContainerRegistry containerRegistry, string name) where TTo : TFrom
        {
            return containerRegistry.RegisterSingleton(typeof(TFrom), typeof(TTo), name);
        }
        public static IContainerRegistry RegisterSingleton<T>(this IContainerRegistry containerRegistry, string name)
        {
            return containerRegistry.RegisterSingleton(typeof(T), typeof(T), name);
        }
        public static IContainerRegistry LoadModule(this IContainerRegistry containerRegistry, string path = "Plugins")
        {
            ModuleLoader moduleLoader = new ModuleLoader();

            moduleLoader.AddFolder(Path.Combine(Environment.CurrentDirectory, path));

            moduleLoader.RegistryAsync(containerRegistry).Wait();

            containerRegistry.RegisterInstance<IModuleLoader>(moduleLoader);

            return containerRegistry;
        }
        public static IContainerProvider InitializeModuleAndCreateContainerProvider(this IContainerRegistry containerRegistry)
        {
            IContainerProvider containerProvider = containerRegistry.CreateContainerProvider();

            //create common service instance
            PowerEventImp? powerEventImp = containerProvider.Resolve<PowerEventImp>();

            VpnService? vpnService = containerProvider.Resolve<VpnService>();

            if (vpnService != null)
            {
                vpnService.Init();
            }

            IModuleLoader? moduleLoader = containerProvider.Resolve<IModuleLoader>();

            if (moduleLoader != null)
            {
                moduleLoader.InitializeAsync(containerProvider).Wait();
            }

            return containerProvider;
        }
        public static IContainerRegistry UseDefault(this IContainerRegistry containerRegistry)
        {
            DirectoryServerConfigUpdater directoryServerConfigProvider = new DirectoryServerConfigUpdater();

            return containerRegistry
                //internal
                .RegisterSingleton<IVpsConfigProvider, VpsConfigProviderImp>()
                //public
                .RegisterSingleton(typeof(IEventBus<>), typeof(EventBus<>))
                .RegisterSingleton<IReportService, ReportServiceImp>()
                .RegisterInstance<Encoding>(Encoding.UTF8)
                .RegisterSingleton<IModuleLoader, ModuleLoader>()
                .RegisterSingleton<IApiKeyProvider, ApiKeyProvider>()
                .RegisterSingleton<INetwork, Network>()
                .RegisterSingleton<ILogger, ConsoleLogger>()
                .RegisterSingleton<IJsonConverter, JsonConverter>()
                .RegisterSingleton<PowerEventImp>()
                .RegisterSingleton<IDataPersistence, DataProtector>()
                .RegisterSingleton<IClientInfo, ClientInfoImp>()
                .RegisterSingleton<IFirewall, Firewall>()
                .RegisterSingleton<IDomainNameService, DomainNameService>()
                .RegisterSingleton<IRedirector, Redirector>()
                .RegisterSingleton<ITunService, NetFilter2TunService>()
                .RegisterInstance<IDirectoryServerConfigProvider>(directoryServerConfigProvider)
                .RegisterInstance<IDirectoryServerConfigUpdater>(directoryServerConfigProvider);
        }
    }
}
