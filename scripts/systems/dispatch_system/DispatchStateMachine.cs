using Godot;
using Scripts.DIContainer;
using Scripts.Gameplay.Services;
using Scripts.StateMachine;

namespace Scripts.Systems.DispatchSystem;

public partial class DispatchStateMachine : BaseStateMachine<DispatchStateMachine.DispatchStates>
{
    [Export] private DispatchUnit3D _connectedUnit;
    
    [Inject] private IDispatcherMapStateService _dispatcherMapStateService;
    
    private int _currentPointIndex;
    private Vector3 _target;
    private Emergency _incident;
    
    public enum DispatchStates
    {
        Patrol,
        Responding,
        Working,
    }
    
    protected override void CreateStateMachine()
    {
        InitializeStateMachine();
        InitializeState(DispatchStates.Patrol, onEnter: PatrolEnter, onUpdate: PatrolProcess);
        InitializeState(DispatchStates.Responding, onEnter: RespondingEnter, onUpdate: RespondingProcess);
        InitializeState(DispatchStates.Working, onEnter: WorkingEnter, onUpdate: WorkingProcess, onExit: WorkingExit);
    }
    
    public override void _Ready()
    {
        _connectedUnit.OnDispatchedEvent += OnDispatchCalled;
        
        CreateStateMachine();
        SetState(DispatchStates.Patrol);
    }

    public override void _ExitTree()
    {
        _connectedUnit.OnDispatchedEvent -= OnDispatchCalled;
    }

    public void PatrolEnter()
    {
        var targetPointPos = _connectedUnit.PatrolPoints[_currentPointIndex].GlobalPosition;
        _connectedUnit.NavAgent.TargetPosition = targetPointPos;
        
        _connectedUnit.IsBusy = false;
    }

    public void PatrolProcess(double delta)
    {
        var nextPathPos = _connectedUnit.NavAgent.GetNextPathPosition();
        var currentPos = _connectedUnit.GlobalPosition;
        var newVelocity = (nextPathPos - currentPos).Normalized() * _connectedUnit.Speed;

        if (newVelocity.Length() > 0.1f)
        {
            var targetLook = _connectedUnit.GlobalPosition + newVelocity;
            _connectedUnit.LookAt(new Vector3(targetLook.X, _connectedUnit.GlobalPosition.Y, targetLook.Z), Vector3.Up);
        }

        _connectedUnit.Velocity = newVelocity;
        _connectedUnit.MoveAndSlide();
        
        if (_connectedUnit.NavAgent.IsNavigationFinished())
        {
            _currentPointIndex = (_currentPointIndex + 1) % _connectedUnit.PatrolPoints.Length;
            var targetPointPos = _connectedUnit.PatrolPoints[_currentPointIndex].GlobalPosition;
            _connectedUnit.NavAgent.TargetPosition = targetPointPos;
        }
    }

    public void RespondingEnter()
    {
        _connectedUnit.NavAgent.TargetPosition = _target;
    }

    public void RespondingProcess(double delta)
    {
        if (_connectedUnit.NavAgent.IsNavigationFinished())
        {
            SetState(DispatchStates.Working);
            return;
        }

        var nextPathPos = _connectedUnit.NavAgent.GetNextPathPosition();
        var currentPos = _connectedUnit.GlobalPosition;
        var newVelocity = (nextPathPos - currentPos).Normalized() * _connectedUnit.Speed;

        if (newVelocity.Length() > 0.1f)
        {
            var targetLook = _connectedUnit.GlobalPosition + newVelocity;
            _connectedUnit.LookAt(new Vector3(targetLook.X, _connectedUnit.GlobalPosition.Y, targetLook.Z), Vector3.Up);
        }

        _connectedUnit.Velocity = newVelocity;
        _connectedUnit.MoveAndSlide();
    }
    
    private double _timer;
    private bool _isFinished;
    
    public void WorkingEnter()
    {
        _connectedUnit.Velocity = Vector3.Zero;
        _dispatcherMapStateService.OnEmergencyFreed += OnEmergencyFreed;
        
        GD.Print("Unit started working...");
    }

    public void WorkingExit()
    {
        _timer = 0;
        _isFinished = false;
        _dispatcherMapStateService.OnEmergencyFreed -= OnEmergencyFreed;
    }

    public void WorkingProcess(double delta)
    {
        _timer += delta;
        if (!(_timer >= _incident.TimeToResolve) || _isFinished)
            return;
        
        _isFinished = true;
        _dispatcherMapStateService.ResolveEmergency(_incident, _connectedUnit.Data);
    }
    
    private void OnEmergencyFreed(Emergency emergency)
    {
        if (emergency != _incident)
            return;
        
        SetState(DispatchStates.Patrol);
    }

    private void OnDispatchCalled(Vector3 pos, Emergency emergency)
    {
        _connectedUnit.IsBusy = true;
        _target = pos;
        _incident = emergency;
        
        SetState(DispatchStates.Responding);
    }
}