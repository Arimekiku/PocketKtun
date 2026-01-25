using Scripts.DispatchSystem;

namespace Scripts.StateMachine;

public abstract class BaseState
{
    protected DispatchUnit3D _unit;

    public void Init(DispatchUnit3D unit) => _unit = unit;

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Process(double delta) { }
    public virtual void PhysicsProcess(double delta) { }
}