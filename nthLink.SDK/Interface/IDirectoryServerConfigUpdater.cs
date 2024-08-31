using nthLink.Header.Interface;
using nthLink.Header.Struct;

namespace nthLink.SDK.Interface
{
    interface IDirectoryServerConfigUpdater : IDirectoryServerConfigProvider
    {
        void UpdateDirectoryServerConfig(DirectoryServerConfig directoryServerConfig);
    }
}
