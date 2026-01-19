using System;
using Scripts.InteractSystem;

namespace Scripts.Gameplay.Services;

public interface IPlayerInteractorService
{
    public event Action<BaseFocusTrigger> OnFocusChangedEvent;
    
    public void RaiseWithRaycast(BaseFocusTrigger trigger);
}