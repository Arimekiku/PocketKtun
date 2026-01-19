using System;

namespace Scripts.DIContainer;

public class GenericBindContext<TContract, TRealization> : BindContext
    where TContract : class where TRealization : TContract
{
    public GenericBindContext()
    {
        _isReady = false;
        _bind ??= new Bind();
        _bind.ContractType = typeof(TContract);
        _bind.RealizationType = typeof(TRealization);
    }

    public GenericBindContext<TContract, TRealization> FromInstance(TRealization instance)
    {
        _bind.BindInstanceCreator.SingleInstance = instance;

        return this;
    }

    public GenericBindContext<TContract, TRealization> FromMethod(Func<TRealization> method)
    {
        _bind.BindInstanceCreator.SingleInstance = method();
        _bind.BindInstanceCreator.ConstructMethod = () => method;

        return this;
    }

    public GenericBindContext<TContract, TRealization> When(Func<bool> condition)
    {
        _bind.BindCondition.Condition = condition;

        return this;
    }

    public GenericBindContext<TContract, TRealization> InjectedOnlyIn<T>() where T : class
    {
        _bind.BindCondition.AvailableInjectionType = typeof(T);

        return this;
    }
}