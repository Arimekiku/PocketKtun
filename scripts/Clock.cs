using Godot;
using System;

namespace Scripts.Gameplay;

public partial class Clock : Node3D
{
    [Export] private Node3D _secondHand;
    [Export] private float _secondCorrection = 180.0f;
    [Export] private Node3D _minuteHand;
    [Export] private float _minuteCorrection = 180.0f;
    [Export] private Node3D _hourHand;
    [Export] private float _hourCorrection = 180.0f;
    [Export] private Vector3 _axis = new(0, 0, 1);

    public override void _PhysicsProcess(double delta)
    {
        var now = DateTime.Now;
        var secRot = now.Second * 6.0f;
        var minRot = (now.Minute * 6.0f) + (now.Second * 0.1f);
        var hourRot = (now.Hour % 12 * 30.0f) + (now.Minute * 0.5f);
        
        if (_secondHand != null)
            _secondHand.RotationDegrees = _axis * (_secondCorrection - secRot);
        if (_minuteHand != null)
            _minuteHand.RotationDegrees = _axis * (_minuteCorrection - minRot);
        if (_hourHand != null)
            _hourHand.RotationDegrees = _axis * (_hourCorrection - hourRot);
    }
}