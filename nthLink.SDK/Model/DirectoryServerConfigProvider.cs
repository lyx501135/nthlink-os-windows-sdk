using nthLink.Header.Interface;
using nthLink.Header.Struct;

namespace nthLink.SDK.Model
{
    public class DirectoryServerConfigProvider : NotifyPropertyChangedBase, IDirectoryServerConfigProvider
    {
        private DirectoryServerConfig? directoryServerConfig;
        public DirectoryServerConfig? DirectoryServerConfig
        {
            get { return this.directoryServerConfig; }
            set { SetProperty(ref this.directoryServerConfig, value); }
        }
    }
}
