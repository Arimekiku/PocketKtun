using System;
using Godot;
using Scripts.InteractSystem;

namespace Scripts.Gameplay.Processes;

public partial class InteractPressProcess : BaseInteractProcess
{
    public override event Action OnInteractProcessEvent;

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed(Inputs.Interact))
            InteractProcess();
    }

    public override void InteractProcess()
    {
        OnInteractProcessEvent!.Invoke();
    }
}