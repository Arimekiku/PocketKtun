using System;

namespace Scripts.InteractSystem;

public interface IFocusTrigger
{
    public event Action<IFocusTrigger> OnFocusEvent;
    public event Action<IFocusTrigger> OnUnfocusEvent;

    public bool IsFocused { get; }

    public void FocusProcess();
}