using nthLink.Header.Interface;
using nthLink.Header.Struct;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace nthLink.SDK.Model
{
    class Network : INetwork
    {
        private uint flags = 0x00;
        public bool IsNetworkAvailable => InternetGetConnectedState(ref this.flags, 0x00);

        public event EventHandler<NetworkEventArgs>? NetworkAvailabilityChanged;

        private readonly DelayAction delayAction;

        public Network()
        {
            this.delayAction = new DelayAction(RaiseEvent);
            NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;
        }

        private void NetworkChange_NetworkAddressChanged(object? sender, EventArgs e)
        {
            this.delayAction.DoAction();
        }

        private void RaiseEvent()
        {
            if (NetworkAvailabilityChanged != null)
            {
                NetworkAvailabilityChanged.Invoke(this, new NetworkEventArgs(this, IsNetworkAvailable));
            }
        }

        [DllImport("wininet")]
        public static extern bool InternetGetConnectedState(ref uint lpdwFlags, uint dwReserved);
    }
}
