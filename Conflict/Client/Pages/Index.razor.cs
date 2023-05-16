using Conflict.Shared;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;

namespace Conflict.Client.Pages
{
    partial class Index
    {
        private HubConnection? hubConnection;
        private List<string> messages = new List<string>();
        private string user = "Username";
        private string? messageInput;

        protected override async Task OnInitializedAsync()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(Navigation.ToAbsoluteUri("/chathub"))
                .Build();


            hubConnection.On<string, Message>("ReceiveMessage", (user, message) =>
            {
                var encodedMsg = $"{user} :: {message.Content}";
                messages.Add(encodedMsg);
                StateHasChanged();
            });

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
