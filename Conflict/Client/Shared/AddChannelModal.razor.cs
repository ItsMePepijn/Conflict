using Conflict.Client.Providers;
using Conflict.Client.Shared.AppComponents;
using Conflict.Shared.Dto;
using Conflict.Shared.Models;
using Microsoft.AspNetCore.Components.Web;
using System.Net.Http.Json;

namespace Conflict.Client.Shared
{
    partial class AddChannelModal
    {
        public string channelName = string.Empty;
		public long UserId {  get; set; }
        protected async override Task OnInitializedAsync()
        {
            StateProvider.OnStateChange += StateHasChanged;

			var authState = await AuthStateProvider.GetAuthenticationStateAsync();
			UserId = long.Parse(authState.User.Claims.Where(claim => claim.Type == "id").First().Value);
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
				await Http.PostAsJsonAsync($"api/channels", new CreateChannelDto() { Name = channelName, OwnerId = UserId });
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
