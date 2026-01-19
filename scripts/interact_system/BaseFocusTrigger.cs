using Godot;
using System;

namespace Scripts.InteractSystem;

public abstract partial class BaseFocusTrigger : Node, IFocusTrigger
{
    public event Action<IFocusTrigger> OnFocusEvent;
    public event Action<IFocusTrigger> OnUnfocusEvent;

    public bool IsFocused { get; }

    public abstract void FocusProcess();
}