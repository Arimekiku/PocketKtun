using Godot;
using Scripts.StateMachine;

namespace Scripts.DispatchSystem;

public class RespondingState : BaseState
{
    private readonly Vector3 _target;
    private readonly Emergency _incident;

    public RespondingState(Vector3 target, Emergency emergency)
    {
        _target = target;
        _incident = emergency;
    }

    public override void Enter()
    {
        _unit.NavAgent.TargetPosition = _target;
    }

    public override void PhysicsProcess(double delta)
    {
        if (_unit.NavAgent.IsNavigationFinished())
        {
            _unit.ChangeState(new WorkingState(_incident));
            return;
        }

        var nextPathPos = _unit.NavAgent.GetNextPathPosition();
        var currentPos = _unit.GlobalPosition;
        var newVelocity = (nextPathPos - currentPos).Normalized() * _unit.Speed;

        if (newVelocity.Length() > 0.1f)
        {
            var targetLook = _unit.GlobalPosition + newVelocity;
            _unit.LookAt(new Vector3(targetLook.X, _unit.GlobalPosition.Y, targetLook.Z), Vector3.Up);
        }

        _unit.Velocity = newVelocity;
        _unit.MoveAndSlide();
    }
}