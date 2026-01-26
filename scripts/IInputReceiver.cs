namespace Scripts.Gameplay;

public interface IInputReceiver
{
    public bool InputReceived { get; }

    public bool ToggleReceive();
}