using Godot;
using Scripts.Systems.InteractSystem;

namespace Scripts.Gameplay;

public partial class RotatePingPongReaction : BaseInteractReaction
{
    [Export] private Node3D _target;
    [Export] private int _rotateValue = 140;
    [Export] private float _rotateTimer = 0.7f;

    private float _nextValue;

    public override void _Ready()
    {
        _nextValue = _rotateValue;
    }

    public override void InteractReaction()
    {
        var currentValue = _nextValue;
        _nextValue = Mathf.RadToDeg(_target.Rotation.Y);
        
        var tween = _target.CreateTween();
        tween.SetEase(Tween.EaseType.InOut);
        tween.TweenProperty(_target, "rotation:y", Mathf.DegToRad(currentValue), _rotateTimer);
        tween.Play();
        
        GD.Print("NextValue: " + _nextValue);
    }
}