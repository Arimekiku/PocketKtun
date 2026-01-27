using Godot;
using System;

namespace Scripts.Gameplay;

public partial class PlayerGrabberComponent : Node
{
    [Export] private RayCast3D _rayCast;
    [Export] private Marker3D _grabPoint;

    [ExportGroup("Physics Constants")]
    [Export] private float _pullPower = 30f;
    [Export] private float _dampening = 2.5f;
    [Export] private float _rotatePower = 15f;
    [Export] private float _maxForce = 200f;

    private RigidBody3D _grabbed;
    private Vector3 _hitOffset;
    private Basis _relativeBasisOffset;
    
    private float _originalDamping;
    private float _originalAngularDamping;

    public override void _PhysicsProcess(double delta)
    {
        if (_grabbed == null || !IsInstanceValid(_grabbed)) return;

        ApplySmoothPhysics();
    }

    public override void _Input(InputEvent e)
    {
        if (Input.IsActionJustPressed(Inputs.LeftMouse)) 
        {
            if (_grabbed == null) TryGrab();
            else Release();
        }
    }

    private void TryGrab()
    {
        if (!_rayCast.IsColliding()) return;
        
        var collider = _rayCast.GetCollider();
        if (collider is RigidBody3D body)
        {
            _grabbed = body;

            Vector3 hitPosition = _rayCast.GetCollisionPoint();
            _hitOffset = hitPosition - _grabbed.GlobalPosition;

            _relativeBasisOffset = _grabPoint.GlobalTransform.Basis.Inverse() * _grabbed.GlobalTransform.Basis;

            _originalDamping = _grabbed.LinearDamp;
            _originalAngularDamping = _grabbed.AngularDamp;
            
            _grabbed.LinearDamp = 5.0f;
            _grabbed.AngularDamp = 5.0f;
            _grabbed.Sleeping = false;
        }
    }

    private void Release()
    {
        if (_grabbed != null)
        {
            _grabbed.LinearDamp = _originalDamping;
            _grabbed.AngularDamp = _originalAngularDamping;
            _grabbed = null;
        }
    }

    private void ApplySmoothPhysics()
    {
        Vector3 currentHitPos = _grabbed.GlobalPosition + _hitOffset;
        Vector3 targetPos = _grabPoint.GlobalPosition;
        
        Vector3 velocityAtPoint = _grabbed.LinearVelocity + _grabbed.AngularVelocity.Cross(_hitOffset);
        Vector3 force = (targetPos - currentHitPos) * _pullPower - (velocityAtPoint * _dampening);
        
        _grabbed.ApplyForce(force.LimitLength(_maxForce), _hitOffset);

        Basis targetBasis = _grabPoint.GlobalTransform.Basis * _relativeBasisOffset;
        
        Quaternion currentRot = _grabbed.GlobalTransform.Basis.GetRotationQuaternion();
        Quaternion targetRot = targetBasis.GetRotationQuaternion();
        
        Quaternion diff = targetRot * currentRot.Inverse();
        
        var axis = diff.GetAxis();
        var angle = diff.GetAngle();

        if (angle > 0.001f)
        {
            if (angle > MathF.PI) angle -= 2.0f * MathF.PI;

            Vector3 torque = (axis * angle * _rotatePower) - (_grabbed.AngularVelocity * _dampening);
            _grabbed.ApplyTorque(torque);
        }
    }
}