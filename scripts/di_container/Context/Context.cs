using Scripts.Utils;
using System;
using System.Collections.Generic;

namespace Scripts.DIContainer;

internal class Context
{
    private readonly Dictionary<Type, BindContainer> _binds = new Dictionary<Type, BindContainer>();
    
    public void RegisterBind(Bind newBind)
    {
        ExceptionsUtils.ThrowIfNull(newBind);
        
        _binds.TryAdd(newBind.ContractType, new BindContainer());
        
        _binds[newBind.ContractType].AddBind(newBind);
    }
    
    public Bind GetBind(Type contractType, Type injectionObjectType = null)
    {
        var bindContainer = _binds[contractType];

        foreach (var bind in bindContainer.ConditionBinds)
        {
            if (bind.BindCondition.CanGiveInstance(injectionObjectType))
                return bind;
        }

        return bindContainer.BindWithoutCondition;
    }

    private class BindContainer()
    {
        private readonly List<Bind> _conditionBinds = new List<Bind>();
        private Bind _bindWithoutCondition;

        public IReadOnlyList<Bind> ConditionBinds => _conditionBinds;
        public Bind BindWithoutCondition => _bindWithoutCondition;
        
        public void AddBind(Bind bind)
        {
            if (bind.HaveCondition)
            {
                _conditionBinds.Add(bind);
                return;
            }
            
            _bindWithoutCondition = bind;
        }
    }
}