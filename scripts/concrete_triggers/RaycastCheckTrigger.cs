using System;
using Scripts.DIContainer;
using Scripts.Gameplay.Services;
using Scripts.InteractSystem;

namespace Scripts.Gameplay.Triggers;

public partial class RaycastCheckTrigger : BaseFocusTrigger
{
    [Inject] private IPlayerInteractorService _playerInteractor;
    
    public override event Action<IFocusTrigger> OnFocusEvent;
    public override event Action<IFocusTrigger> OnUnfocusEvent;
    
    public override bool IsFocused { get; }

    public override void _Ready()
    {
        _playerInteractor.OnFocusChangedEvent += CheckFocused;
    }

    public override void _ExitTree()
    {
        _playerInteractor.OnFocusChangedEvent -= CheckFocused;
    }

    public override void FocusProcess()
    { }

    private void CheckFocused(BaseFocusTrigger target)
    {
        if (target != this)
        {
            OnUnfocusEvent?.Invoke(this);
            return;
        }
        
        OnFocusEvent?.Invoke(this);
    }
}