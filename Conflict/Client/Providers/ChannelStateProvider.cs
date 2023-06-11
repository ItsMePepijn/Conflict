using Conflict.Shared.Models;

namespace Conflict.Client.Providers
{
    public class ChannelStateProvider
    {
        public Channel? CurrentChannel { get; private set; } = null;

        public event Action? OnChange;

        public void SetChannel(Channel? channel)
        {
            CurrentChannel = channel;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
