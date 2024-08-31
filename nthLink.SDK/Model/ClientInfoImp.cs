using nthLink.Header.Interface;
using nthLink.SDK.Extension;
using nthLink.SDK.Kernel;
using System.Reflection;
using System.Text;

namespace nthLink.SDK.Model
{
    class ClientInfoImp : IClientInfo
    {
        private const string ClientGuidKey = "ClientGuid";
        private const string DomainKeysKey = "DomainKeys";
        private const string FavoriteCategoriesKey = "FavoriteCategories";
        private readonly IContainerProvider containerProvider;
        private readonly IDataPersistence dataPersistence;

        public string ClientGuid { get; private set; } = string.Empty;

        public string DomainKeys { get; private set; } = string.Empty;

        public Version AppVersion { get; private set; } = Version.Parse("0.0.0");

        public Version SdkVersion { get; private set; } = Version.Parse("0.0.0");

        public IDictionary<string, int> FavoriteCategories { get; private set; }
            = new Dictionary<string, int>();

        public ClientInfoImp(IContainerProvider containerProvider,
            IDataPersistence dataPersistence)
        {
            this.containerProvider = containerProvider;
            this.dataPersistence = dataPersistence;
            Initialize();
        }

        private void Initialize()
        {
            string clientId = this.dataPersistence.Load(ClientGuidKey);

            if (string.IsNullOrEmpty(clientId))
            {
                clientId = Guid.NewGuid().ToString("D");
                this.dataPersistence.Save(ClientGuidKey, clientId);
            }

            ClientGuid = clientId;

            if (this.containerProvider.Resolve<ISystemReportLog>() is ISystemReportLog systemReportLog)
            {
                systemReportLog.Log(Header.Enum.LogLevelEnum.Info, $"The client id is {ClientGuid}");
            }

            string favoriteCategoriesJson = this.dataPersistence.Load(FavoriteCategoriesKey);

            if (!string.IsNullOrEmpty(favoriteCategoriesJson) &&
                this.containerProvider.Resolve<IJsonConverter>() is IJsonConverter jsonConverter)
            {
                if (jsonConverter.Deserialize<Dictionary<string, int>>(favoriteCategoriesJson)
                    is Dictionary<string, int> favoriteCategories)
                {
                    FavoriteCategories = favoriteCategories;
                }
            }

            ClientGuid = clientId;

            if (this.dataPersistence.Load(DomainKeysKey) is string str &&
                !string.IsNullOrEmpty(str) &&
                this.containerProvider.Resolve<IEncodeDecode>() is IEncodeDecode encodeDecode)
            {
                DomainKeys = encodeDecode.Decrypt(str);
            }

            Version? appVersion = Assembly.GetEntryAssembly()?.GetName().Version;

            if (appVersion != null)
            {
                AppVersion = appVersion;
            }

            Version? sdkVersion = Assembly.GetAssembly(typeof(VpnService))?.GetName().Version;

            if (sdkVersion != null)
            {
                SdkVersion = sdkVersion;
            }
        }

        public void UpdateDomainKeys(IList<string> domainKeys)
        {
            if (this.containerProvider.Resolve<IEncodeDecode>() is IEncodeDecode encodeDecode)
            {
                DomainKeys = MakeDomainKey(domainKeys);

                string newKeys = encodeDecode.Encrypt(DomainKeys);

                //儲存加密的DomainKeys
                this.dataPersistence.Save(DomainKeysKey, newKeys);
            }
        }

        public void UpdateFavoriteCategories(string category, int rate)
        {
            if (FavoriteCategories.ContainsKey(category))
            {
                FavoriteCategories[category] = FavoriteCategories[category] + rate;
            }
            else
            {
                FavoriteCategories.Add(category, rate);
            }

            if (this.containerProvider.Resolve<IJsonConverter>()
                is IJsonConverter jsonConverter)
            {
                string favoriteCategoriesJson = jsonConverter.Serialize(FavoriteCategories);

                if (!string.IsNullOrEmpty(favoriteCategoriesJson))
                {
                    this.dataPersistence.Cache(FavoriteCategoriesKey,
                        favoriteCategoriesJson);
                }
            }
        }

        private string MakeDomainKey(IList<string> domainKeys)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("[");

            for (int i = 0; i < domainKeys.Count; i++)
            {
                if (i != 0)
                {
                    stringBuilder.Append(", ");
                }

                stringBuilder.Append(domainKeys[i]);
            }

            stringBuilder.Append("]");

            return stringBuilder.ToString();
        }
    }
}
