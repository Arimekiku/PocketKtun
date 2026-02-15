using System;

namespace Scripts.DIContainer;

internal interface IContextProvider
{
    public Context GlobalContext { get; }
    
    public Context GetContext(object injectedObject, Type contractType);
    public void RegisterBind(Bind newBind, object contextObject = null);
}