using Godot;
using Scripts.Utils;
using Scripts.Systems.WindowSystem;

namespace ktun.working;

[GlobalClass]
public partial class TestOpenComponent : Node, IOpenWindowComponent
{
    private IWindow OwnerWindow => this.FindParent<IWindow>();
    
    public void Open()
    {
        GD.Print($"TestOpenComponent process. Owner window {OwnerWindow.Id}");
    }
}