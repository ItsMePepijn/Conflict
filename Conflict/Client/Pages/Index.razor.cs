using Microsoft.AspNetCore.SignalR.Client;

namespace Conflict.Client.Pages
{
    partial class Index
	{
		private HubConnection? hubConnection;
		public bool IsConnected => hubConnection?.State == HubConnectionState.Connected;

		private string loadingMsg = "Connecting to server";
		private bool isLoading = true;

		protected override async Task OnInitializedAsync()
		{
			hubConnection = new HubConnectionBuilder()
				.WithUrl(Navigation.ToAbsoluteUri("/chathub"))
				.WithAutomaticReconnect(new RetryPolicy())
				.Build();

			hubConnection.Reconnecting += error =>
			{
				StateHasChanged();
				return Task.CompletedTask;
			};

			hubConnection.Reconnected += connectionId =>
			{
				StateHasChanged();
				return Task.CompletedTask;
			};

			hubConnection.Closed += Exception =>
			{
				loadingMsg = "Failed to connect";
				isLoading = false;
				StateHasChanged();
				return Task.CompletedTask;
			};

			await hubConnection.StartAsync();
		}
	}
}
