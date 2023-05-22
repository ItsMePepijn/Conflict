using Microsoft.AspNetCore.Components;


namespace Conflict.Client.Shared
{
	partial class Loading
	{
		[Parameter]
		public string Message { get; set; } = string.Empty;
		[Parameter]
		public bool IsLoading { get; set; } = true;
		public string dots { get; set; } = string.Empty;
		private Timer? timer;

		protected override void OnInitialized()
		{
			if (IsLoading)
			{
				int delay = 400;
				timer = new System.Threading.Timer((object? stateInfo) =>
				{
					if (dots.Length < 3)
						dots += ".";
					else
						dots = ".";

					StateHasChanged();
				}, new System.Threading.AutoResetEvent(false), delay, delay); // fire every 2000 milliseconds
			}
		}
	}

}