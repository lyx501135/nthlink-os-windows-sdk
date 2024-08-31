namespace nthLink.Header.Interface
{
    public interface IModuleLoader
    {
        bool IsLoaded { get; }
        bool IsResisted { get; }
        Task RegistryAsync(IContainerRegistry containerRegistry);
        Task InitializeAsync(IContainerProvider containerProvider);
        Task UninitializeAsync(IContainerProvider containerProvider);
        void AddFile(string path);
        void AddFile(FileInfo fileInfo);
        void AddFolder(string path);
        void AddFolder(DirectoryInfo directoryInfo);
        void AddModule(IModuleInfo moduleInfo);
    }
}
