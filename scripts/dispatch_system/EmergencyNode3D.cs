using Godot;
using Scripts.DIContainer;
using Scripts.Gameplay.Services;

namespace Scripts.DispatchSystem;

public partial class EmergencyNode3D : Marker3D
{
    [Export] private EventUIHandle _eventUIHandle;
    [Export] private Button _resolveButton;
    
    [Inject] private IDispatcherMapStateService _dispatcherMapStateService;
    
    public Emergency Data { get; private set; }
    
    public void Initialize(EmergencyResourceDefinition emergencyData)
    {
        Data = emergencyData.ToEmergency();
        
        _eventUIHandle.Texture = emergencyData.Icon;
        _eventUIHandle.Initialize();
        
        _dispatcherMapStateService.AddEmergencyEventMarker(this);
    }

    public override void _Ready()
    {
        _eventUIHandle.OnDropEvent += StartResolveSequence;
        _dispatcherMapStateService.OnEmergencyResolved += OnEmergencyResolved;
    }

    public override void _ExitTree()
    {
        _eventUIHandle.OnDropEvent -= StartResolveSequence;
        _dispatcherMapStateService.OnEmergencyResolved -= OnEmergencyResolved;
    }

    private void StartResolveSequence(DispatchUnit3D unit)
    {
        unit.DispatchTo(GlobalPosition, Data);
    }
    
    private void OnEmergencyResolved(Emergency emergency, Character character)
    {
        if (emergency != Data)
            return;
        
        _dispatcherMapStateService.RemoveEmergencyEventMarker(this);
        _resolveButton.Show();
        
        _resolveButton.Pressed += () =>
        {
            _dispatcherMapStateService.RequestReportForm(emergency, character);
        
            QueueFree();
        };
    }
}