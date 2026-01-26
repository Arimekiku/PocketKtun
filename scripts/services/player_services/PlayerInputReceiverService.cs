using System;

namespace Scripts.Gameplay.Services;

public class PlayerInputReceiverService: IPlayerInputReceiverService
{
    public event Action<bool> OnInputToggledEvent;

    public bool InputReceived { get; private set; } = true;
    
    public bool ToggleReceive()
    {
        InputReceived = !InputReceived;
        
        OnInputToggledEvent?.Invoke(InputReceived);
        return InputReceived;
    }
}