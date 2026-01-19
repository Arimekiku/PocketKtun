using System;

namespace Scripts.DIContainer;

public class BindCondition
{
    public Type AvailableInjectionType;
    public Func<bool> Condition;

    public bool IsHaveCondition => Condition != null && AvailableInjectionType != null;
    
    public bool CanGiveInstance(Type injectionType)
    {
        var typeMatch = AvailableInjectionType == null || AvailableInjectionType == injectionType;
        var condition = Condition == null || Condition();
        return typeMatch && condition;
    }
}