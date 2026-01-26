using System;

namespace Scripts.Systems.InteractSystem;

public interface IInteractable
{
    public event Action<IInteractable> OnInteractEvent;
    public event Action<IInteractable> OnFocusEvent;
    public event Action<IInteractable> OnUnfocusEvent;
    
    public void Interact();
    public void Focus();
    public void Unfocus();
}