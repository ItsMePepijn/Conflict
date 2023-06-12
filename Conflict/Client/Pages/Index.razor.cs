using Microsoft.AspNetCore.SignalR.Client;

namespace Conflict.Client.Pages
{
    partial class Index
	{
		public bool IsConnected => ConnectionProvider.HubConnection?.State == HubConnectionState.Connected;

		protected override async Task OnInitializedAsync()
		{
			await AuthStateProvider.GetAuthenticationStateAsync();

			ConnectionProvider.HubConnection.Reconnecting += error =>
			{
				StateHasChanged();
				return Task.CompletedTask;
			};

			ConnectionProvider.HubConnection.Reconnected += connectionId =>
			{
				StateHasChanged();
				return Task.CompletedTask;
			};

			await ConnectionProvider.HubConnection.StartAsync();
		}
	}
}
