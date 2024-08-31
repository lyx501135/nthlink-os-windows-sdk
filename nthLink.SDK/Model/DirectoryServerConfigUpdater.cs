using nthLink.Header.Interface;
using nthLink.Header.Struct;
using nthLink.SDK.Interface;

namespace nthLink.SDK.Model
{
    public class DirectoryServerConfigUpdater : NotifyPropertyChangedBase, IDirectoryServerConfigUpdater
    {
        private DirectoryServerConfig? directoryServerConfig;
        public DirectoryServerConfig? DirectoryServerConfig
        {
            get { return this.directoryServerConfig; }
        }

        public void UpdateDirectoryServerConfig(DirectoryServerConfig directoryServerConfig)
        {
            SetProperty(ref this.directoryServerConfig, directoryServerConfig, nameof(DirectoryServerConfig));
        }
    }
}
