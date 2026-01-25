using Godot;
using Scripts.StateMachine;

namespace Scripts.DispatchSystem;

public class PatrolState: BaseState
{
    private int _currentPointIndex;

    public override void Enter()
    {
        var targetPointPos = _unit.PatrolPoints[_currentPointIndex].GlobalPosition;
        _unit.NavAgent.TargetPosition = targetPointPos;
    }

    public override void Process(double delta)
    {
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
        
        if (_unit.NavAgent.IsNavigationFinished())
        {
            _currentPointIndex = (_currentPointIndex + 1) % _unit.PatrolPoints.Length;
            var targetPointPos = _unit.PatrolPoints[_currentPointIndex].GlobalPosition;
            _unit.NavAgent.TargetPosition = targetPointPos;
        }
    }
}