namespace Scripts.DIContainer;

internal interface IContextProvider
{
    public Context GetContext(object injectedObject);
    public void RegisterBind(Bind newBind, object contextObject = null);
}