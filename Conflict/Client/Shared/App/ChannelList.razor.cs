using Conflict.Shared.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http.Json;

namespace Conflict.Client.Shared.App
{
    partial class ChannelList
    {
        public List<Channel>? Channels { get; set; } = null;

        protected override async Task OnInitializedAsync()
        {
            var result = await Http.GetFromJsonAsync<Channel[]>("api/channels");
            if(result is not null)
            {
                Channels = new();
                Channels.AddRange(result);
            }

			ConnectionProvider.HubConnection.On<Channel>("ChannelAdded", (channel) =>
            {
                if(Channels is not null)
                {
                    Channels.Add(channel);
                    StateHasChanged();
                }
            });

            ChannelState.OnChange += StateHasChanged;
        }

        public void HandleChannelClick(Channel channel)
        {
            if (ChannelState.CurrentChannel == channel)
                ChannelState.SetChannel(null);
            else
                ChannelState.SetChannel(channel);

		}
    }
}
