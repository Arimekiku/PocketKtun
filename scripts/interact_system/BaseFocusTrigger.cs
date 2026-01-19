using Godot;
using System;

namespace Scripts.InteractSystem;

[GlobalClass]
public abstract partial class BaseFocusTrigger : Node, IFocusTrigger
{
    public abstract event Action<IFocusTrigger> OnFocusEvent;
    public abstract event Action<IFocusTrigger> OnUnfocusEvent;

    public abstract bool IsFocused { get; }

    public abstract void FocusProcess();
}