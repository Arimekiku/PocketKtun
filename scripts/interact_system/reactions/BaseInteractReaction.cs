using Godot;

namespace Scripts.InteractSystem;

public abstract partial class BaseInteractReaction : Node, IInteractReaction, IFocusReaction, IUnfocusReaction
{
    public virtual void InteractReaction() { }
    
    public virtual void FocusReaction() { }

    public void UnfocusReaction() { }
}