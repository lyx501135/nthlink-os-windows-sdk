using nthLink.Header;
using nthLink.Header.Enum;
using nthLink.Header.Interface;
using nthLink.Header.Struct;
using nthLink.SDK.Extension;

namespace nthLink.SDK.Demo
{
    public partial class Form1 : Form
    {
        private readonly IEventBus<VpnServiceFunctionArgs>? vpnService;
        public Form1(IContainerProvider containerProvider)
        {
            InitializeComponent();

            this.vpnService = containerProvider.Resolve<IEventBus<VpnServiceFunctionArgs>>();

            if (containerProvider.Resolve<IEventBus<VpnServiceStateArgs>>()
                   is IEventBus<VpnServiceStateArgs> eventBus)
            {
                eventBus.Subscribe(Const.Channel.VpnService, OnVpnServiceStateChanged);
            }
        }

        private void OnVpnServiceStateChanged(string s, VpnServiceStateArgs args)
        {
            this.BeginInvoke(() =>
            {
                switch (args.State)
                {
                    case StateEnum.Waiting:
                    case StateEnum.Starting:
                        this.ConnectButton.Enabled = false;
                        this.ConnectButton.Visible = true;
                        this.DisconnectButton.Enabled = false;
                        this.DisconnectButton.Visible = false;
                        break;
                    case StateEnum.Started:
                        this.ConnectButton.Enabled = false;
                        this.ConnectButton.Visible = false;
                        this.DisconnectButton.Enabled = true;
                        this.DisconnectButton.Visible = true;
                        break;
                    case StateEnum.Stopping:
                        this.ConnectButton.Enabled = false;
                        this.ConnectButton.Visible = false;
                        this.DisconnectButton.Enabled = false;
                        this.DisconnectButton.Visible = true;
                        break;
                    case StateEnum.Stopped:
                        this.ConnectButton.Enabled = true;
                        this.ConnectButton.Visible = true;
                        this.DisconnectButton.Enabled = false;
                        this.DisconnectButton.Visible = false;
                        break;
                    case StateEnum.Terminating:
                        this.ErrorLabel.Text = args.Message;
                        this.ConnectButton.Enabled = true;
                        this.ConnectButton.Visible = true;
                        break;
                }

                this.MessageLabel.Text = args.State.ToString();
            });
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            if (this.vpnService != null)
            {
                this.ErrorLabel.Text = string.Empty;

                this.vpnService.Publish(Const.Channel.VpnService,
                       new VpnServiceFunctionArgs(FunctionEnum.Start));
            }
        }

        private void DisconnectButton_Click(object sender, EventArgs e)
        {
            if (this.vpnService != null)
            {
                this.ErrorLabel.Text = string.Empty;

                this.vpnService.Publish(Const.Channel.VpnService,
                       new VpnServiceFunctionArgs(FunctionEnum.Stop));
            }
        }
    }
}