using Godot;
using Scripts.Utils;
using Scripts.WindowSystem;

namespace ktun.working;

[GlobalClass]
public partial class TestFocusComponent : Node, IFocusWindowComponent
{
    private IWindow OwnerWindow => this.FindParent<IWindow>();
    
    public void Focus()
    {
        GD.Print($"TestFocusComponent process. Window owner {OwnerWindow.Id}");
    }
}