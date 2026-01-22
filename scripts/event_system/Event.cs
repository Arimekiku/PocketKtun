using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Scripts.DIContainer;
using Scripts.Gameplay.Messages;
using Scripts.MessageManager;

namespace Scripts.EventSystem;

public partial class Event : Node, IEvent
{
    public event Action<IEvent> OnEventStarted;
    public event Action<IEvent> OnEventEnded;

    public string EventId { get; set; }
    
    private readonly List<GameMessage> _messageQueue = [];
    
    // TODO: Reactions, fail and spawn contexts
    private int _currentProcesserIndex;

    [Inject] private IMessageManager _messageManager;

    public override void _Ready()
    {
        var uniqueByType = _messageQueue
            .GroupBy(m => m.Message)
            .Select(g => g.First())
            .ToList();
        
        foreach (var processedData in uniqueByType)
        {
            var messageType = processedData.Message;
            
            _messageManager.Subscribe<GameMessages, GameMessage>(messageType, ProcessEvent);
        }
        
        OnEventStarted?.Invoke(this);
    }

    public override void _ExitTree()
    {
        var uniqueByType = _messageQueue
            .GroupBy(m => m.Message)
            .Select(g => g.First())
            .ToList();
        
        foreach (var processedData in uniqueByType)
        {
            var messageType = processedData.Message;
            
            _messageManager.Unsubscribe<GameMessages, GameMessage>(messageType, ProcessEvent);
        }
    }

    public void AddProcessedData(GameMessage data)
    {
        _messageQueue.Add(data);
    }

    private void ProcessEvent(GameMessage message)
    {
        _currentProcesserIndex++;
        if (_currentProcesserIndex < _messageQueue.Count)
            return;
        
        OnEventEnded?.Invoke(this);
    }
}