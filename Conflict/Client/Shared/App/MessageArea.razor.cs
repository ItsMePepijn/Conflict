﻿using Conflict.Shared.Dto;
using Conflict.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http.Json;

namespace Conflict.Client.Shared.App
{
    partial class MessageArea
    {
        private List<MessageDto>? messages = null;
        private string? messageInput;
        private bool IsConnected => ConnectionProvider.HubConnection.State == HubConnectionState.Connected;

        protected override void OnInitialized()
        {
            ChannelState.OnChange += LoadMessages;

			ConnectionProvider.HubConnection.On<MessageDto>("ReceiveMessage", (message) =>
            {
                if (messages is not null && message.ChannelId == ChannelState.Id)
                {
                    messages.Insert(0, message);
                    StateHasChanged();
                }
            });
		}

        private async Task Send()
        {
            if (ConnectionProvider.HubConnection is not null && ChannelState.Id is not null && !string.IsNullOrEmpty(messageInput))
            {
                SendMessageDto message = new()
                {
                    Content = messageInput
                };

                messageInput = string.Empty;
                await Http.PostAsJsonAsync($"api/channels/{ChannelState.CurrentChannel?.Id}/messages", message);
            }

        }
        public async Task Enter(KeyboardEventArgs e)
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                await Send();
            }
        }

        public async void LoadMessages()
        {
            StateHasChanged();
            if (ChannelState.Id is not null)
            {
                MessageDto[]? result = await Http.GetFromJsonAsync<MessageDto[]>($"api/channels/{ChannelState.Id}/messages");
                if (result is not null)
                {
                    messages = new();
                    messages.AddRange(result);
                }
                StateHasChanged();
            }
        }
    }
}
