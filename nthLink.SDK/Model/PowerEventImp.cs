using Microsoft.Win32;
using nthLink.Header;
using nthLink.Header.Enum;
using nthLink.Header.Interface;

namespace nthLink.SDK.Model
{
    internal class PowerEventImp
    {
        private readonly IContainerProvider containerProvider;

        public event EventHandler<PowerModeEnum>? PowerEvent;

        public PowerEventImp(IContainerProvider containerProvider)
        {
            this.containerProvider = containerProvider;
            SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;
        }

        private void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            if (this.containerProvider.Resolve(typeof(IEventBus<PowerModeEnum>))
                is IEventBus<PowerModeEnum> eventBus)
            {
                if (e.Mode == PowerModes.Suspend)
                {
                    eventBus.Publish(Const.Channel.PowerEvent, PowerModeEnum.Suspend);
                }
                else if (e.Mode == PowerModes.Resume)
                {
                    eventBus.Publish(Const.Channel.PowerEvent, PowerModeEnum.Resume);
                }
            }
        }
    }
}
