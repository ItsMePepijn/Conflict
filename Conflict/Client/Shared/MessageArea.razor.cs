using Conflict.Shared.Dto;
using Conflict.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http.Json;

namespace Conflict.Client.Shared
{
    partial class MessageArea
    {
        [Parameter]
        public HubConnection? hubConnection { get; set; }
        private readonly List<Message> messages = new();
        private string? messageInput;
        private bool IsConnected => hubConnection?.State == HubConnectionState.Connected;

        protected override void OnInitialized()
        {
            hubConnection!.On<Message>("ReceiveMessage", (message) =>
            {
                messages.Add(message);
                StateHasChanged();
            });
        }

        private async Task Send()
        {
            if (hubConnection is not null && !string.IsNullOrEmpty(messageInput))
            {
                MessageDto message = new()
                {
                    Content = messageInput
                };

                await HttpClient.PostAsJsonAsync("api/channels/1/messages", message);
                messageInput = string.Empty;
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
