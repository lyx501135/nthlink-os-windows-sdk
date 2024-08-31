using nthLink.Header;
using nthLink.Header.Interface;

namespace nthLink.SDK.Model
{
    public class ApiKeyProvider : IApiKeyProvider
    {
        private readonly IDataPersistence dataPersistence;

        public ApiKeyProvider(IDataPersistence dataPersistence)
        {
            this.dataPersistence = dataPersistence;
        }

        public string GetApiKey()
        {
            return this.dataPersistence.Load("apikey");
        }
    }
}
