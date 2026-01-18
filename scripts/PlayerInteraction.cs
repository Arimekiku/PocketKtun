using Godot;
using Scripts.Gameplay;

public partial class PlayerInteraction : Node3D
{
    [Export] private RayCast3D _rayCast;
    
    private IRaycastable _lastTarget;

    public override void _Process(double delta)
    {
        if (_rayCast.IsColliding())
        {
            var collider = _rayCast.GetCollider();
            if (collider is not IRaycastable hitNode)
                return;
            
            GD.Print("Colliding");
            
            _lastTarget?.OnRaycastOut();
            hitNode.OnRaycastIn();
            _lastTarget = hitNode;
        }
        else if (_lastTarget != null)
        {
            _lastTarget.OnRaycastOut();
            _lastTarget = null;
        }
    }
}