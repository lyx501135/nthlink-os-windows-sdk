using nthLink.SDK.Extension;
using nthLink.Header.Interface;
using nthLink.SDK.Model;

namespace nthLink.SDK
{
    public static class Entry
    {
        public static IContainerRegistry CreateContainerRegistry(string apiKey)
        {
            return new ContainerRegistry()
                .UseDefault()
                .RegisterInstance<IApiKeyProvider>(new CustomsApiKeyProvider(apiKey));
        }

        public static IContainerRegistry CreateContainerRegistry()
        {
            return new ContainerRegistry()
                .UseDefault();
        }

        class CustomsApiKeyProvider : IApiKeyProvider
        {
            private readonly string apiKey;

            public CustomsApiKeyProvider(string apiKey)
            {
                this.apiKey = apiKey;
            }

            public string GetApiKey()
            {
                return this.apiKey;
            }
        }
    }
}
