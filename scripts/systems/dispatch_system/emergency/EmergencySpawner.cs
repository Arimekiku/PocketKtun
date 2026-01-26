using System.Collections.Generic;
using Godot;
using Scripts.DIContainer;
using Scripts.Gameplay.Services;
using Scripts.Services;
using Scripts.Utils;

namespace Scripts.Systems.DispatchSystem;

public partial class EmergencySpawner : Node
{
    [Export] public PackedScene EmergencyTemplate;
    [Export] public EmergencyResourceDefinition[] PossibleEvents;
    [Export] public NavigationRegion3D NavRegion;

    [Inject] private ILogger _logger;
    [Inject] private IDispatcherMapStateService _dispatcherMapStateService;
    
    private readonly List<Emergency> _emergencies = [];
    
    private Timer _spawnTimer;

    public override void _Ready()
    {
        _spawnTimer = new Timer();
        _spawnTimer.WaitTime = 5.0f;
        _spawnTimer.Autostart = true;
        _spawnTimer.Timeout += SpawnRandomEmergency;
        AddChild(_spawnTimer);
        
        _dispatcherMapStateService.OnEmergencyFreed += OnEmergencyFreed;
    }

    public override void _ExitTree()
    {
        _dispatcherMapStateService.OnEmergencyFreed -= OnEmergencyFreed;
    }

    private void SpawnRandomEmergency()
    {
        if (_emergencies.Count >= 5)
        {
            _logger.Log("Can't exceed the maximum amount of emergencies! Abort...");
            return;
        }
        
        var randomEvent = PossibleEvents.PickRandom();
        var randomPos = GetRandomNavPos();
        
        var instance = EmergencyTemplate.Instantiate<EmergencyNode3D>();
        AddChild(instance);
        
        instance.GlobalPosition = randomPos;
        instance.Initialize(randomEvent);
        
        _emergencies.Add(instance.Data);
        _logger.Log("Spawned emergency: " + randomEvent.Name + " at " + randomPos);
    }

    private Vector3 GetRandomNavPos()
    {
        var map = NavRegion.GetNavigationMap();
        
        return NavigationServer3D.MapGetRandomPoint(map, 1, true);
    }

    private void OnEmergencyFreed(Emergency emergency)
    {
        _emergencies.Remove(emergency);
    }
}