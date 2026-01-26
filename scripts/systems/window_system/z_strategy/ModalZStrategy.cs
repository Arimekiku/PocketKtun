using Godot;
using Scripts.DIContainer;

namespace Scripts.Systems.WindowSystem;

[GlobalClass]
public partial class ModalZStrategy : BaseZStrategy
{
    private const float Z_OFF_SET = 300f;
    
    private IWindowRegistry _windowRegistry;

    private float CurrentOpenedZPosition => _windowRegistry.ActiveWindow.VisualZ; 
    
    [Inject]
    public void Construct(IWindowRegistry windowRegistry)
    {
        _windowRegistry = windowRegistry;
    }
    
    public override float GetZ() => CurrentOpenedZPosition +  Z_OFF_SET;
}