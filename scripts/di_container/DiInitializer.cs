using Godot;
using GodotInterfaceExport;
using Scripts.Services;

namespace Scripts.DIContainer;

[GlobalClass]
internal partial class DiInitializer : Node
{
    private IContextProvider _contextProvider;
    private IResolver _resolver;
    private NodeInjector _nodeInjector;
    private NodeContextRegister _nodeContextRegister;
    private DiContainer _diContainer;

    public DiContainer DiContainer => _diContainer;
    
    public override void _EnterTree()
    {
        _contextProvider = new GodotContextProvider();
        _resolver = new Resolver(_contextProvider);
        _nodeContextRegister = new NodeContextRegister(_contextProvider);
        _diContainer = new DiContainer(_resolver);
        _nodeInjector = new NodeInjector(_diContainer);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _nodeInjector.Dispose();
            _nodeContextRegister.Dispose();
        }
        
        base.Dispose(disposing);
    }
}