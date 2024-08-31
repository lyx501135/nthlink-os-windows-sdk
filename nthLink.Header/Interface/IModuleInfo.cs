namespace nthLink.Header.Interface
{
    public interface IModuleInfo
    {
        Task OnRegisterAsync(IContainerRegistry containerRegistry);
        Task OnInitializeAsync(IContainerProvider containerProvider);
        Task OnUninitializedAsync(IContainerProvider containerProvider);
    }
}
