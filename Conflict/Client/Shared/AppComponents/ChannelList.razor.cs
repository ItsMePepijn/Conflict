﻿using Conflict.Shared.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http.Json;

namespace Conflict.Client.Shared.AppComponents
{
	partial class ChannelList
	{
		public List<Channel>? Channels { get; set; } = null;
		public long UserId { get; set; }

		protected override async Task OnInitializedAsync()
		{
			var authState = await AuthStateProvider.GetAuthenticationStateAsync();
			UserId = long.Parse(authState.User.Claims.Where(claim => claim.Type == "id").First().Value);
			await LoadChannels();

			ConnectionProvider.HubConnection.On<Channel>("ChannelAdded", async (channel) =>
			{
				if (Channels is not null)
				{
					Channels.Add(channel);
					StateHasChanged();
					await LoadChannels();
				}
			});

			ConnectionProvider.HubConnection.On("ChannelInfoChanged", async () =>
			{
				await LoadChannels();

				// Leave channel if its deleted
				if(Channels is not null)
					if (!Channels.Any(c => c.Id == ChannelState.CurrentChannel?.Id)) ChannelState.SetChannel(null);
			});


			ChannelState.OnChange += StateHasChanged;
		}

		public async Task LoadChannels()
		{
			var result = await Http.GetFromJsonAsync<Channel[]>("api/channels");
			if (result is not null)
			{
				Channels = new();
				Channels.AddRange(result);
				StateHasChanged();
			}
		}

		public void HandleChannelClick(Channel channel)
		{
			if (ChannelState.CurrentChannel == channel)
				ChannelState.SetChannel(null);
			else
				ChannelState.SetChannel(channel);
		}

		public async Task DeleteChannel(long channelId)
		{
			ChannelState.SetChannel(null);

			// Remove channel from client first for better client-side performance
			Channels?.Remove(Channels.Find(c => c.Id == channelId)!);
			await Http.DeleteAsync($"/api/channels/{channelId}");
		}
	}
}
