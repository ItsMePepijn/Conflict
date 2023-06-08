using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace Conflict.Client.Providers
{
	public class HubConnectionProvider
	{
		public HubConnection HubConnection { get; set; }
		public HubConnectionProvider(NavigationManager Navigation)
		{
			HubConnection = new HubConnectionBuilder()
				.WithUrl(Navigation.ToAbsoluteUri("/chathub"))
				.WithAutomaticReconnect(new RetryPolicy())
				.Build();

		}

	}
}
