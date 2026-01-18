using Godot;
using System;

public partial class Clock : Node3D
{
    [Export] private Node3D _hourHand;
    [Export] private Node3D _minuteHand;

    private const float CORRECTION = 180.0f;
    
    public override void _Process(double delta)
    {
        var now = DateTime.Now;
        var minRot = (now.Minute * 6.0f) + (now.Second * 0.1f);
        var hourRot = (now.Hour % 12 * 30.0f) + (now.Minute * 0.5f);

        _minuteHand.RotationDegrees = new Vector3(0, 0, CORRECTION - minRot);
        _hourHand.RotationDegrees = new Vector3(0, 0, CORRECTION - hourRot);
    }
}