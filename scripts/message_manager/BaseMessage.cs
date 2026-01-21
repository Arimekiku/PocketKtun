using System;

namespace Scripts.MessageManager;

public abstract class BaseMessage<T> : Message where T : Enum
{
    public readonly T Message;

    protected BaseMessage(T message)
    {
        Message = message;
    }
}
