using Godot;
using System;

namespace Scripts.InteractSystem;

public abstract partial class BaseInteractProcess : Node, IInteractProcess
{
    public event Action OnInteractProcessEvent;

    public abstract void InteractProcess();
}