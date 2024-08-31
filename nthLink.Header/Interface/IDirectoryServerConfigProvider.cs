using nthLink.Header.Struct;
using System.ComponentModel;

namespace nthLink.Header.Interface
{
    public interface IDirectoryServerConfigProvider : INotifyPropertyChanged
    {
        DirectoryServerConfig? DirectoryServerConfig { get; }
    }
}
