using System;
using System.Collections.Generic;
using Scripts.Systems.DispatchSystem;

namespace Scripts.Gameplay.Services;

public class DispatcherMapStateService : IDispatcherMapStateService
{
    private Dictionary<Emergency, EmergencyNode3D> _emergencies = [];
    
    public event Action<Emergency, Character> OnEmergencyResolved;
    public event Action<Emergency, Character> OnReportFormRequested;
    public event Action<Emergency> OnEmergencyFreed;

    public void RequestReportForm(Emergency emergency, Character character)
    {
        OnReportFormRequested!.Invoke(emergency, character);
    }

    public void ResolveEmergency(Emergency emergency, Character character)
    {
        OnEmergencyResolved!.Invoke(emergency, character);
    }

    public void FreeEmergency(Emergency emergency)
    {
        OnEmergencyFreed!.Invoke(emergency);
    }

    public void AddEmergencyEventMarker(EmergencyNode3D emergencyNode)
    {
        _emergencies.Add(emergencyNode.Data, emergencyNode);
    }

    public void RemoveEmergencyEventMarker(EmergencyNode3D emergencyNode)
    {
        _emergencies.Remove(emergencyNode.Data);
    }
}