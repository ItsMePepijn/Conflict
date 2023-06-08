
namespace Conflict.Client.Shared
{
	partial class Loading
	{
		public string Message { get; set; } = "Connecting to server";
		public bool IsLoading { get; set; } = true;
		public string Dots { get; set; } = string.Empty;

		protected override void OnInitialized()
		{

			int delay = 400;
			Timer timer = new Timer((object? stateInfo) =>
			{
				if (IsLoading)
				{
					if (Dots.Length < 3)
						Dots += ".";
					else
						Dots = ".";
				}
				else Dots = string.Empty;

				StateHasChanged();
			}, new AutoResetEvent(false), delay, delay);

			ConnectionProvider.HubConnection.Closed += Exception =>
			{
				Message = "Failed to connect";
				IsLoading = false;
				StateHasChanged();
				return Task.CompletedTask;
			};
		}

		private void Reload()
		{
			Navigation.NavigateTo(Navigation.Uri, forceLoad: true);
		}
	}

}