using System;
using Godot;
using Scripts.DIContainer;
using Scripts.Gameplay.Services;
using Scripts.Systems.InteractSystem;

namespace Scripts.Gameplay.Triggers;

public partial class RaycastCheckTrigger : BaseFocusTrigger
{
    [Export] private CollisionObject3D _targetBody;
    
    [Inject] private IPlayerInteractorService _playerInteractor;
    
    public override event Action<IFocusTrigger> OnFocusEvent;
    public override event Action<IFocusTrigger> OnUnfocusEvent;
    
    public override bool IsFocused { get; }

    private bool _isFocused;

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

    private void CheckFocused(CollisionObject3D target)
    {
        if (_targetBody != target)
        {
            if (_isFocused)
                OnUnfocusEvent!.Invoke(this);
            
            _isFocused = false;
            return;
        }
        
        OnFocusEvent!.Invoke(this);
        _isFocused = true;
    }
}