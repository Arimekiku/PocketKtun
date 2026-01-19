using Godot;
using Scripts.Services;

namespace Scripts.DIContainer;

public class DiContainer
{
    private readonly IResolver _resolver;

    internal DiContainer(IResolver resolver)
    {
        _resolver = resolver;
    }
    
    public void Inject<TInstance>(TInstance instance) where TInstance : class
    { 
        if (_resolver.Inject(instance))
            GD.Print($"Injected complete in {typeof(TInstance)}");
    }
    
    public TContract Resole<TContract>() where TContract : class => _resolver.Resolve<TContract>();
}