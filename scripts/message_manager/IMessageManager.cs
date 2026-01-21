using System;

namespace Scripts.MessageManager;

public interface IMessageManager
{
    public void Publish<TType>(BaseMessage<TType> message) where TType : Enum;
    public void Subscribe<TMessage, TCallback>(TMessage message, Action<TCallback> callback) where TMessage : Enum where TCallback : BaseMessage<TMessage>;
    public void Unsubscribe<TMessage, TCallback>(TMessage message, Action<TCallback> callback) where TMessage : Enum where TCallback : BaseMessage<TMessage>;
}
