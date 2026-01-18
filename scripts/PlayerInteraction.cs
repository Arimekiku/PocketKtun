using Godot;

namespace Scripts.Gameplay;

public partial class PlayerInteraction : Node3D
{
    [Export] private RayCast3D _rayCast;
    
    private IRaycastable _lastTarget;

    public override void _Process(double delta)
    {
        if (!_rayCast.IsColliding())
        {
            _lastTarget?.OnRaycastOut();
            _lastTarget = null;
        }
        
        var collider = _rayCast.GetCollider();
        if (collider is not IRaycastable target)
            return;
        
        if (_lastTarget == target)
            _lastTarget?.OnRaycastOut();
        
        target.OnRaycastIn();
        _lastTarget = target;
    }
}