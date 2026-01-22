using System.Collections.Generic;
using Godot;
using Scripts.DIContainer;
using Scripts.Services;

namespace Scripts.EventSystem;

public partial class EventProcesser : Node
{
    [Export(PropertyHint.File, "*.json")] private string _startingEvents;

    [Inject] private ILogger _logger;
    
    private readonly EventParser _eventParser = new();
    
    private readonly Dictionary<StringName, IEvent> _eventPool = [];
    private List<IEvent> _activeEvents = [];
    
    public override void _EnterTree()
    {
        var events = _eventParser.ParseEventSheet(_startingEvents);

        foreach (var @event in events)
        {
            _eventPool.Add(@event.EventId, @event);
            @event.OnEventStarted += OnEventStarted;
            @event.OnEventEnded += OnEventEnded;
        }
    }

    public override void _Ready()
    {
        foreach (var (_, @event) in _eventPool)
            AddChild(@event as Event);
    }
    
    private void OnEventStarted(IEvent @event)
    {
        _logger.Log("Event started: " + @event.EventId);
        
        _eventPool.Remove(@event.EventId);
        _activeEvents.Add(@event);
    }
    
    private void OnEventEnded(IEvent @event)
    {
        _logger.Log("Event ended: " + @event.EventId);
        
        _activeEvents.Remove(@event);
    }
}