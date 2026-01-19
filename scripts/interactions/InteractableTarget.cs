using Godot;

namespace Scripts.Gameplay;

[GlobalClass]
public abstract partial class InteractableTarget : Node
{
    public abstract void OnInteract();
}