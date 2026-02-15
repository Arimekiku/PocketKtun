using System;

namespace Scripts.Utils;

public abstract class GenericParsingContext<T> : ParsingContext
{
    private T _target;
    
    public override Type TargetType => typeof(T);
    
    public T Target
    {
        get => _target;
        protected set
        {
            _target = value;
            TargetObject = value;
        }
    }
}