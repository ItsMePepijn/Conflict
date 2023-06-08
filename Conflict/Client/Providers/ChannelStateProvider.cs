namespace Conflict.Client.Providers
{
    public class ChannelStateProvider
    {
        public long? Id { get; private set; } = null;

        public event Action? OnChange;

        public void SetChannel(long? channelId)
        {
            Id = channelId;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
