using Conflict.Shared;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;

namespace Conflict.Client.Pages
{
	partial class Index
	{
		private HubConnection? hubConnection;
		private readonly List<Message> messages = new List<Message>();
		private readonly string user = "Username";
		private string? messageInput;

		private string loadingMsg = "Connecting to server";
		private bool isLoading = true;

		protected override async Task OnInitializedAsync()
		{
			hubConnection = new HubConnectionBuilder()
				.WithUrl(Navigation.ToAbsoluteUri("/chathub"))
				.WithAutomaticReconnect(new RetryPolicy())
				.Build();


			hubConnection.On<Message>("ReceiveMessage", (message) =>
			{
				messages.Add(message);
				StateHasChanged();
			});

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

		private async Task Send()
		{
			if (hubConnection is not null && !string.IsNullOrEmpty(messageInput))
			{
				await hubConnection.SendAsync("SendMessage", user, messageInput);
				messageInput = string.Empty;
			}
		}

		public bool IsConnected =>
			hubConnection?.State == HubConnectionState.Connected;

		public async ValueTask DisposeAsync()
		{
			if (hubConnection is not null)
			{
				await hubConnection.DisposeAsync();
			}
		}
		public async Task Enter(KeyboardEventArgs e)
		{
			if (e.Code == "Enter" || e.Code == "NumpadEnter")
			{
				await Send();
			}
		}
	}
}
