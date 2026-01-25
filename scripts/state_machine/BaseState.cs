using System;

namespace Scripts.StateMachine;

public class BaseState : IState
{
    private readonly Action _onEnter;
    private readonly Action _onExit;
    private readonly Action<double> _onUpdate;

    public BaseState(Action onEnter = null, Action onExit = null, Action<double> onUpdate = null)
    {
        _onEnter = onEnter;
        _onExit = onExit;
        _onUpdate = onUpdate;
    }
    
    public void EnterState() => _onEnter?.Invoke();

    public void ExitState() => _onExit?.Invoke();

    public void Update(double delta) => _onUpdate?.Invoke(delta);
}