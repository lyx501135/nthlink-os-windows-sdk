using nthLink.Header.Struct;

namespace nthLink.Header.Interface
{
    public interface INetwork
    {
        event EventHandler<NetworkEventArgs>? NetworkAvailabilityChanged;
        bool IsNetworkAvailable { get; }
    }
}
