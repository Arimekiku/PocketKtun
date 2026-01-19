using Godot;
using Scripts.DIContainer;
using Scripts.Gameplay.Services;
using Scripts.Gameplay.Triggers;

namespace Scripts.Gameplay;

public partial class PlayerInteractionUpdater : Node3D
{
    [Export] private RayCast3D _rayCast;
    
    [Inject] private IPlayerInteractorService _playerInteractor;

    private RaycastCheckTrigger _lastTarget;
    
    public override void _Process(double delta)
    {
        if (!_rayCast.IsColliding())
        {
            if (_lastTarget != null)
                _playerInteractor.RaiseWithRaycast(null);
            
            _lastTarget = null;
            return;
        }
        
        var collider = _rayCast.GetCollider();
        if (collider is not RaycastCheckTrigger target)
            return;
        
        _lastTarget = target;
        _playerInteractor.RaiseWithRaycast(target);
    }
}