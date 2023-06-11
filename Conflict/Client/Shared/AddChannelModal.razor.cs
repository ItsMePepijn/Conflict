using Conflict.Client.Providers;
using Conflict.Shared.Dto;
using Conflict.Shared.Models;
using Microsoft.AspNetCore.Components.Web;
using System.Net.Http.Json;

namespace Conflict.Client.Shared
{
    partial class AddChannelModal
    {
        public string channelName = string.Empty;
        protected override void OnInitialized()
        {
            StateProvider.OnStateChange += StateHasChanged;
        }

		public async Task KeyDown(KeyboardEventArgs e)
		{
			StateHasChanged();
			if (e.Code == "Enter" || e.Code == "NumpadEnter")
			{
				await CreateChannel();
			}
		}

		public async Task CreateChannel()
		{
			if (!string.IsNullOrEmpty(channelName))
			{
				await Http.PostAsJsonAsync($"api/channels", new CreateChannelDto() { Name = channelName });
				CloseModal();
			}
		}

		public void CloseModal()
		{
			channelName = string.Empty;
			StateProvider.SetState(AddChannelModalStateProvider.States.Hidden);
		}
	}
}
