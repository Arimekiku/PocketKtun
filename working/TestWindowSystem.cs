using Godot;
using Scripts.DIContainer;
using Scripts.Systems.WindowSystem;
using System;


public partial class TestWindowSystem : Node
{
    private IWindowControl _windowControl;

    [Inject]
    public void Construct(IWindowControl windowControl)
    {
        _windowControl = windowControl;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("move_up"))
            _windowControl.OpenWindow(new WindowData(WindowIds.TestId1));

        if (@event.IsActionPressed("mode_down"))
            _windowControl.CloseWindow(WindowIds.TestId1);
        
        if (@event.IsActionPressed("move_left"))
            _windowControl.OpenWindow(new WindowData(WindowIds.TestId2));
        
        if (@event.IsActionPressed("move_right"))
            _windowControl.CloseWindow(WindowIds.TestId2);
    }
}