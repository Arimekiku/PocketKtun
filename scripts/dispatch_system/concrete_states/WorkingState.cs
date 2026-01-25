using Godot;
using Scripts.DIContainer;
using Scripts.Gameplay.Services;
using Scripts.StateMachine;

namespace Scripts.DispatchSystem;

public class WorkingState : BaseState
{
    [Inject] private IDispatcherMapStateService _dispatcherMapStateService;
    
    private readonly Emergency _incident;
    
    private double _timer;
    private bool _isFinished;
    
    public WorkingState(Emergency incident)
    {
        DiInitializer.Instance.DiContainer.Inject(this);
        
        _incident = incident;
    }

    public override void Enter()
    {
        _unit.Velocity = Vector3.Zero;
        _dispatcherMapStateService.OnEmergencyFreed += OnEmergencyFreed;
        
        GD.Print("Unit started working...");
    }

    public override void Exit()
    {
        _dispatcherMapStateService.OnEmergencyFreed -= OnEmergencyFreed;
    }

    public override void Process(double delta)
    {
        _timer += delta;

        if (_timer >= _incident.TimeToResolve && !_isFinished)
        {
            _isFinished = true;
            _dispatcherMapStateService.ResolveEmergency(_incident, _unit.Data);
        }
    }
    
    private void OnEmergencyFreed(Emergency emergency)
    {
        if (emergency != _incident)
            return;
        
        _unit.ChangeState(new PatrolState());
    }
}