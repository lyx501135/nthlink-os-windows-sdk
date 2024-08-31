# nthlink SDK for Windows

[![Build Status](https://github.com/teoncom/nthLink-windows/actions/workflows/dotnet-desktop.yml/badge.svg?branch=main)](https://github.com/teoncom/nthLink-windows)

## Introductions

[nthlink](https://www.nthlink.com/) is an anti-censorship mobile application capable of
circumventing Internet censorship and self-recovering from blocking events.

The purpose of nthlink is enabling everyday users in censored regions to gain safer and unfettered
access to the Internet.

## How to use

To use the nthLink SDK, you first need to run it as an administrator. 
You can find more information on how to do this on the [Administrator Wiki](https://en.wikipedia.org/wiki/User_Account_Control). 
Once you are ready to use the SDK, you can download it from the [GitHub releases page](https://github.com/teoncom/nthLink-windows/releases).

And we will need to take the following steps:

1. Create an `IContainerProvider` with your api key, which is a dependency injection container that provides `IEventBus`.
2. Use the resolved `IEventBus<VpnServiceFunctionArgs>` to publish the start message.
3. Use the resolved `IEventBus<VpnServiceFunctionArgs>` to publish stop message to disconnect when connected.
4. The `IEventBus<VpnServiceStateArgs>` channel will publish VPN service message, you can subscribe to the channel.

```csharp
using nthLink.Header;
using nthLink.Header.Enum;
using nthLink.Header.Interface;
using nthLink.Header.Struct;
using nthLink.SDK.Extension;

static async void Main()
{
    IContainerProvider containerProvider = nthLink.SDK.Entry.CreateContainerRegistry("YourApiKey")
     .LoadModule()
	 .InitializeModuleAndCreateContainerProvider();
    
    IEventBus<VpnServiceFunctionArgs>? vpnService = containerProvider.Resolve<IEventBus<VpnServiceFunctionArgs>>();
    
	if(vpnService != null)
	{
		vpnService.Publish(Const.Channel.VpnService,
                       new VpnServiceFunctionArgs(FunctionEnum.Start));
	}
	
	//If you need to be notified when the proxy state changes, you can subscribe to the StateChange event
    if (containerProvider.Resolve<IEventBus<VpnServiceStateArgs>>()
        is IEventBus<VpnServiceStateArgs> eventBus)
    {
        eventBus.Subscribe(Const.Channel.VpnService, OnVpnServiceStateChanged);
    }
	
    if(vpnService != null)
	{
		vpnService.Publish(Const.Channel.VpnService,
                       new VpnServiceFunctionArgs(FunctionEnum.Stop));
	}
}

private void OnVpnServiceStateChanged(string s, VpnServiceStateArgs args)
{
	//The state change event handler
}
```

## Demo

You can also check out this sample repository which shows how to use the nthLink SDK on the WinForms framework.