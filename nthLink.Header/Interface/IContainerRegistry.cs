namespace nthLink.Header.Interface
{
    public interface IContainerRegistry
    {
        IContainerRegistry RegisterInstance(Type type, object instance, string name = "");
        IContainerRegistry Register(Type type, string name = "");
        IContainerRegistry Register(Type from, Type to, string name = "");
        IContainerRegistry RegisterSingleton(Type from, Type to, string name = "");
        IContainerProvider CreateContainerProvider();
    }
}
