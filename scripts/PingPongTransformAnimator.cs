using Godot;

public partial class PingPongTransformAnimator: Node
{
    [Export] private Node3D _target;
    [Export] private Vector3 _targetPosition;
    [Export] private Vector3 _targetRotationDegrees;
    [Export] private Vector3 _targetScale = Vector3.One;
    [Export] private float _duration = 1.0f;
    
    private Vector3 _initialPosition;
    private Vector3 _initialRotationDegrees;
    private Vector3 _initialScale;
    private Tween _tween;
    
    private bool _pingPongValue;
    
    public override void _Ready()
    {
        _initialPosition = _target.Position;
        _initialRotationDegrees = _target.RotationDegrees;
        _initialScale = _target.Scale;
    }

    public void Animate()
    {
        _tween?.Kill();
        _tween = CreateTween();
        _tween.SetParallel();
        
        var nextPosition = _pingPongValue ? _targetPosition : _initialPosition;
        _tween.TweenProperty(_target, "position", nextPosition, _duration)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.InOut);
        
        var nextRotationDegrees = _pingPongValue ? _targetRotationDegrees : _initialRotationDegrees;
        _tween.TweenProperty(_target, "rotation_degrees", nextRotationDegrees, _duration)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.InOut);
        
        var nextScale = _pingPongValue ? _targetScale : _initialScale;
        _tween.TweenProperty(_target, "scale", nextScale, _duration)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.InOut);
        
        _tween.Play();
        _pingPongValue = !_pingPongValue;
    }
}