using System;
using Godot;

namespace Scripts.Gameplay;

public partial class Computer : Node3D, IInputReceiver
{
    [Export] private SubViewport _viewport;
    [Export] private ComputerControl _control;
    [Export] private Camera3D _camera;
    
    public bool InputReceived { get; private set; }

    public override void _Input(InputEvent @event)
    {
        if (!InputReceived)
            return;
        if (@event.IsActionPressed("ui_cancel"))
            return;
        
        if (@event is InputEventKey)
            _viewport.PushInput(@event);
        
        if (@event is InputEventMouseMotion motion)
        {
            var result = _control.MousePosition + motion.Relative;
            result.X = Math.Clamp(result.X, 0.0f, _viewport.Size.X);
            result.Y = Math.Clamp(result.Y, 0.0f, _viewport.Size.Y);
            _control.UpdateMousePosition(result);

            var newMotion = new InputEventMouseMotion();
            newMotion.Position = result;
            newMotion.GlobalPosition = result;
            newMotion.Relative = motion.Relative;
    
            _viewport.PushInput(newMotion);
            _viewport.WarpMouse(result); 
        }

        if (@event is InputEventMouseButton mouseButton)
        {
            var newEvent = (InputEventMouseButton)mouseButton.Duplicate();
            newEvent.Position = _control.MousePosition;
            newEvent.GlobalPosition = _control.MousePosition;
    
            _viewport.PushInput(newEvent);
        }
    }

    public bool ToggleReceive()
    {
        InputReceived = !InputReceived;
        _camera.Current = InputReceived;
        
        return InputReceived;
    }
}