using Godot;

namespace Scripts.Gameplay;

[GlobalClass]
public abstract partial class RaycastTarget : Node
{
    public abstract void OnRaycastIn();
    public abstract void OnRaycastOut();
}