using System;
using System.Collections.Generic;
using Scripts.DIContainer;
using Scripts.Services;

namespace Scripts.MessageManager;

public class MessageManager : IMessageManager
{
    private abstract class Callback
    {
        public abstract void Invoke(object message);
    }
        
    private class GenericCallback<TMessage> : Callback where TMessage : Message
    {
        public Action<TMessage> CallbackEvent;

        public override void Invoke(object message)
        {
            CallbackEvent.Invoke(message as TMessage);
        }

        public override bool Equals(object obj)
        {
            if (obj is not GenericCallback<TMessage> other)
                return false;
                
            return CallbackEvent == other.CallbackEvent;
        }

        public override int GetHashCode() => CallbackEvent.GetHashCode();
    }

    [Inject] private ILogger _logger;
    
    private readonly Dictionary<Type, Dictionary<int, HashSet<Callback>>> _subscribedCallbacks = new();

    public void Publish<TType>(BaseMessage<TType> messageData) where TType : Enum
    {
        var messageType = messageData.Message.GetType();

        if (!_subscribedCallbacks.TryGetValue(messageType, out var callbacks))
            return;

        var enumIndex = (int)(object)messageData.Message;

        if (!callbacks.TryGetValue(enumIndex, out var callbacksHashSet))
            return;

        var copyArr = new Callback[callbacksHashSet.Count];

        callbacksHashSet.CopyTo(copyArr);

        for (var i = copyArr.Length - 1; i >= 0; --i)
            copyArr[i].Invoke(messageData);

        _logger.Log($"Message {messageData.Message.ToString()} was published");
    }

    public void Subscribe<TType, TMessage>(TType message, Action<TMessage> callback) where TType : Enum where TMessage : BaseMessage<TType>
    {
        Subscribe(typeof(TType), message, callback);
    }

    public void Subscribe<TType, TMessage>(Type messageType, TType message, Action<TMessage> callback) where TType : Enum where TMessage : BaseMessage<TType>
    {
        var enumIndex = (int)(object)message;
        Subscribe(messageType, enumIndex, callback);
    }

    private void Subscribe<TMessage>(Type messageType, int message, Action<TMessage> callback) where TMessage : Message
    {
        if (!_subscribedCallbacks.TryGetValue(messageType, out var callbacks))
        {
            _subscribedCallbacks.Add(messageType, new Dictionary<int, HashSet<Callback>>());
            callbacks = _subscribedCallbacks[messageType];
        }

        if (!callbacks.TryGetValue(message, out var callbacksHashSet))
        {
            callbacks.Add(message, new HashSet<Callback>());
            callbacksHashSet = callbacks[message];
        }

        var genericCallback = new GenericCallback<TMessage> { CallbackEvent = callback };
            
        if (callbacksHashSet.Contains(genericCallback))
        {
            _logger.LogWarning("Trying to add a callback to an existing in MessageManager");
            return;
        }

        callbacksHashSet.Add(genericCallback);
        _logger.Log($"Action {callback.Method.Name} was subscribed to message {message.ToString()}");
    }

    public void Unsubscribe<TType, TMessage>(TType message, Action<TMessage> callback) where TType : Enum where TMessage : BaseMessage<TType>
    {
        Unsubscribe(typeof(TType), message, callback);
    }

    public void Unsubscribe<TType, TMessage>(Type messageType, TType message, Action<TMessage> callback) where TType : Enum where TMessage : Message
    {
        var enumIndex = (int)(object)message;
        Unsubscribe(messageType, enumIndex, callback);
    }

    public void Unsubscribe<TMessage>(Type messageType, int message, Action<TMessage> callback) where TMessage : Message
    {
        if (!_subscribedCallbacks.TryGetValue(messageType, out var callbacks))
            return;

        if (!callbacks.TryGetValue(message, out var callbacksList))
            return;

        var genericCallback = new GenericCallback<TMessage> { CallbackEvent = callback };
            
        if (!callbacksList.Contains(genericCallback))
            return;

        callbacksList.Remove(genericCallback);
        _logger.Log($"Action {callback.Method.Name} was unsubscribed from message {message.ToString()}");
    }
}