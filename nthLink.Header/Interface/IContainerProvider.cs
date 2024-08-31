namespace nthLink.Header.Interface
{
    public interface IContainerProvider
    {
        object? Resolve(Type type, string name = "");
    }
}
