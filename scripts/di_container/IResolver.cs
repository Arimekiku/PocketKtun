namespace Scripts.DIContainer;

internal interface IResolver
{
    internal bool Inject<TInstance>(TInstance instance) where TInstance : class;
    internal TContract Resolve<TContract>() where TContract : class;
}