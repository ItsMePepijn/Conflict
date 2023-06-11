namespace Conflict.Client.Providers
{
    public class AddChannelModalStateProvider
    {
        public enum States
        {
            Hidden,
            Shown
        }
        public event Action? OnStateChange;
        public States CurrentState { get; private set; }

        public void SetState(States newState)
        {
            CurrentState = newState;
            OnStateChange?.Invoke();
        }
    }
}
