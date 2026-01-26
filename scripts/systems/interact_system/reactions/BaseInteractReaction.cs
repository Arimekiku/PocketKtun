using Godot;

namespace Scripts.Systems.InteractSystem;

[GlobalClass]
public abstract partial class BaseInteractReaction : Node, IInteractReaction, IFocusReaction, IUnfocusReaction
{
    public virtual void InteractReaction() { }
    
    public virtual void FocusReaction() { }

    public virtual void UnfocusReaction() { }
}