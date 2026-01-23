using Godot;

namespace Scripts.Gameplay;

public partial class PlayerGrabberComponent : Node
{
    [Export] private RayCast3D _rayCast;
    [Export] private PlayerGrabberRibbon _lineRenderer;
    [Export] private Node3D _grabPoint;

    [Export] private float _holdDistance = 2.5f;
    [Export] private float _pullForce = 40f;
    [Export] private float _rotateForce = 8f;
    [Export] private float _maxForce = 60f;

    private RigidBody3D _grabbedObject;
    private Basis _targetBasis;

    public override void _Ready()
    {
        _lineRenderer.EndNode = null;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_grabbedObject == null)
            return;

        ApplyPullForce();
        ApplyRotationForce();
    }

    public override void _Input(InputEvent e)
    {
        if (Input.IsActionJustPressed(Inputs.LeftMouse))
        {
            _grabbedObject = HandleGrabbing();   
            _lineRenderer.EndNode = _grabbedObject;
        }

        if (_grabbedObject == null)
            return;
        if (e is not InputEventMouseMotion motion)
            return;
        
        var yaw = -motion.Relative.X * 0.003f;
        var pitch = -motion.Relative.Y * 0.003f;
        
        _targetBasis = new Basis(Vector3.Up, yaw) *
                       new Basis(Vector3.Right, pitch) *
                       _targetBasis;
    }

    private RigidBody3D HandleGrabbing()
    {
        if (_grabbedObject != null)
            return null;
        if (!_rayCast.IsColliding())
            return null;
        if (_rayCast.GetCollider() is not RigidBody3D body)
            return null;

        _grabbedObject = body;
        _grabbedObject.Sleeping = false;

        _targetBasis = _grabbedObject.GlobalTransform.Basis;

        return body;
    }

    private void ApplyPullForce()
    {
        var delta = _grabPoint.GlobalPosition - _grabbedObject.GlobalPosition;
        var distance = delta.Length();

        if (distance < 0.05f)
            return;

        var strength = Mathf.Clamp(distance * distance, 0f, 1f);
        var force = delta.Normalized() * strength * _pullForce - _grabbedObject.LinearVelocity * 0.9f;

        _grabbedObject.ApplyCentralForce(force);
        _grabbedObject.LinearVelocity = _grabbedObject.LinearVelocity.LimitLength(3.5f);
    }


    private void ApplyRotationForce()
    {
        var current = _grabbedObject.GlobalTransform.Basis.GetRotationQuaternion();
        var target = _targetBasis.GetRotationQuaternion();

        var diff = target * current.Inverse();
        diff = diff.Normalized();

        var angle = diff.GetAngle();
        if (angle <= 0.001f)
            return;
        
        var axis = diff.GetAxis();
        var torque = (axis * angle * _rotateForce).LimitLength(10f);
        _grabbedObject.ApplyTorque(torque);
    }
}
