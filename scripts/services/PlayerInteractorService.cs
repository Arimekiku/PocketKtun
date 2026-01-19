using System;
using Scripts.InteractSystem;

namespace Scripts.Gameplay.Services;

public class PlayerInteractorService : IPlayerInteractorService
{
    public event Action<BaseFocusTrigger> OnFocusChangedEvent;
    
    public void RaiseWithRaycast(BaseFocusTrigger trigger)
    {
        OnFocusChangedEvent!.Invoke(trigger);
    }
}