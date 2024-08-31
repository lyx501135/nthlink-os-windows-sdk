namespace nthLink.Header.Interface
{
    public interface IMainThreadSyncContext
    {
        Task Post(Action action);
    }
}
