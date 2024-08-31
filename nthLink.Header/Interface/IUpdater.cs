namespace nthLink.Header.Interface
{
    public interface IUpdater
    {
        Task<bool> NeedUpdate();
        Task<string> Download();
    }
}
