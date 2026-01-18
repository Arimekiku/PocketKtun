using Godot;
using Godot.Collections;

namespace Scripts.Gameplay;

public partial class InteractableComponent : Area3D
{
    [Export] private Array<InteractableTarget> _interactions;
    [Export] private Array<RaycastTarget> _raycastTargets;

    public void Interact()
    {
        foreach (var interactableTarget in _interactions)
            interactableTarget.OnInteract();
    }

    public void RaycastIn()
    {
        foreach (var raycastTarget in _raycastTargets)
            raycastTarget.OnRaycastIn();
    }
    
    public void RaycastOut()
    {
        foreach (var raycastTarget in _raycastTargets)
            raycastTarget.OnRaycastOut();
    }
}