using System;
using Scripts.DispatchSystem;

namespace Scripts.Gameplay.Services;

public interface IDispatcherMapStateService
{
    public event Action<Emergency, Character> OnEmergencyResolved;
    public event Action<Emergency, Character> OnReportFormRequested;
    public event Action<Emergency> OnEmergencyFreed;
    
    public void RequestReportForm(Emergency emergency, Character character);

    public void ResolveEmergency(Emergency emergency, Character character);
    public void FreeEmergency(Emergency emergency);
    
    public void AddEmergencyEventMarker(EmergencyNode3D emergencyNode);
    public void RemoveEmergencyEventMarker(EmergencyNode3D emergencyNode);
}