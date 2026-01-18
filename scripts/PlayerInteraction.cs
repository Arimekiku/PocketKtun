using Godot;

namespace Scripts.Gameplay;

public partial class PlayerInteraction : Node3D
{
    [Export] private RayCast3D _rayCast;
    
    private InteractableComponent _lastTarget;
    
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed(Inputs.Interact))
        {
            if (_lastTarget == null)
                return;
            
            _lastTarget.Interact();
        }
    }

    public override void _Process(double delta)
    {
        if (!_rayCast.IsColliding())
        {
            _lastTarget?.RaycastOut();
            _lastTarget = null;
        }
        
        var collider = _rayCast.GetCollider();
        if (collider is not InteractableComponent target)
            return;
        
        if (_lastTarget == target)
            _lastTarget?.RaycastOut();
        
        target.RaycastIn();
        _lastTarget = target;
    }
}