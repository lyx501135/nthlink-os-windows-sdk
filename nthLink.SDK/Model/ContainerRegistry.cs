using Microsoft.Extensions.DependencyInjection;
using nthLink.Header.Interface;

namespace nthLink.SDK.Model
{
    class ContainerRegistry : IContainerRegistry
    {
        private static IServiceCollection CreateServiceCollection() => new ServiceCollection();
        private readonly IServiceCollection serviceCollection = CreateServiceCollection();
        private readonly Dictionary<string, IServiceCollection> namedServiceCollection = new Dictionary<string, IServiceCollection>();

        private IServiceCollection GetServiceCollectionByName(string name)
        {
            IServiceCollection serviceCollection;

            if (string.IsNullOrEmpty(name))
            {
                serviceCollection = this.serviceCollection;
            }
            else
            {
                if (!this.namedServiceCollection.ContainsKey(name))
                {
                    this.namedServiceCollection.Add(name, CreateServiceCollection());
                }

                serviceCollection = this.namedServiceCollection[name];
            }

            return serviceCollection;
        }
        IContainerRegistry IContainerRegistry.RegisterInstance(Type type, object instance, string name)
        {
            if (type != null && instance != null)
            {
                GetServiceCollectionByName(name).AddSingleton(type, instance);
            }
            return this;
        }
        IContainerRegistry IContainerRegistry.Register(Type from, Type to, string name)
        {
            if (from != null && to != null)
            {
                GetServiceCollectionByName(name).AddTransient(from, to);
            }
            return this;
        }
        IContainerRegistry IContainerRegistry.RegisterSingleton(Type from, Type to, string name)
        {
            GetServiceCollectionByName(name).AddSingleton(from, to);
            return this;
        }
        IContainerRegistry IContainerRegistry.Register(Type type, string name)
        {
            GetServiceCollectionByName(name).AddSingleton(type, type);
            return this;
        }
        IContainerProvider IContainerRegistry.CreateContainerProvider()
        {
            return new ContainerProvider(this.serviceCollection, this.namedServiceCollection);
        }
    }
}
