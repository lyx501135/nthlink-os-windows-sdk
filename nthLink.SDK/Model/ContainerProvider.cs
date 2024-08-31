using Microsoft.Extensions.DependencyInjection;
using nthLink.Header.Interface;

namespace nthLink.SDK.Model
{
    class ContainerProvider : IContainerProvider
    {
        private readonly IServiceProvider serviceProvider;
        private readonly Dictionary<string, IServiceProvider> namedServiceProvider = new Dictionary<string, IServiceProvider>();
        public ContainerProvider(IServiceCollection serviceCollection, Dictionary<string, IServiceCollection> namedServiceCollection)
        {
            serviceCollection.AddSingleton(typeof(IContainerProvider), this);

            this.serviceProvider = serviceCollection.BuildServiceProvider();

            foreach (var item in namedServiceCollection)
            {
                this.namedServiceProvider.Add(item.Key, item.Value.BuildServiceProvider());
            }
        }

        object? IContainerProvider.Resolve(Type type, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Resolve(this.serviceProvider, type);
            }
            else
            {
                if (this.namedServiceProvider.ContainsKey(name))
                {
                    return Resolve(this.namedServiceProvider[name], type);
                }
                else
                {
                    return null;
                }
            }

        }

        private object? Resolve(IServiceProvider serviceProvider, Type type)
        {
            if (serviceProvider.GetService(type) is object obj)
            {
                return obj;
            }
            else
            {
                if (type.IsClass &&
                    !type.IsAbstract)
                {
                    try
                    {
                        return ActivatorUtilities.CreateInstance(serviceProvider, type);
                    }
                    catch
                    {

                    }
                }
            }

            return null;
        }
    }
}
