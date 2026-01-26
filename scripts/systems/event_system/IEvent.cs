using System;

namespace Scripts.Systems.EventSystem;

public interface IEvent
{
    public string EventId { get; }
    
    public event Action<IEvent> OnEventStarted;
    public event Action<IEvent> OnEventEnded;
}