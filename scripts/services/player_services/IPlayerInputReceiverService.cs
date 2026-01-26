using System;

namespace Scripts.Gameplay.Services;

public interface IPlayerInputReceiverService : IInputReceiver
{
    public event Action<bool> OnInputToggledEvent;
}