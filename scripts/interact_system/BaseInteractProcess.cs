using Godot;
using System;

namespace Scripts.InteractSystem;

[GlobalClass]
public abstract partial class BaseInteractProcess : Node, IInteractProcess
{
    public abstract event Action OnInteractProcessEvent;

    public abstract void InteractProcess();
}