using Godot;
using Scripts.Utils;
using Scripts.WindowSystem;

namespace ktun.working;

[GlobalClass]
public partial class TestUnfocusComponent : Node, IUnfocusWindowComponent
{
    private IWindow OwnerWindow => this.FindParent<IWindow>();
    
    public void Unfocus()
    {
        GD.Print($"TestUnfocusComponent process. Owner window {OwnerWindow.Id}");
    }
}