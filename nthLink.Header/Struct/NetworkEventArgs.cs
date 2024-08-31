using nthLink.Header.Interface;

namespace nthLink.Header.Struct
{
    public class NetworkEventArgs
    {
        public INetwork Sender { get; }

        public bool IsNetworkAvailable { get; }

        public NetworkEventArgs(INetwork network, bool isNetworkAvailable)
        {
            Sender = network;
            IsNetworkAvailable = isNetworkAvailable;
        }
    }
}
