using Godot;
using Scripts.Utils;
using Scripts.Systems.WindowSystem;

namespace ktun.working;

[GlobalClass]
public partial class TestCloseComponent : Node, ICloseWindowComponent
{
    private IWindow OwnerWindow => this.FindParent<IWindow>();
    
    public void Close()
    {
        GD.Print($"TestCloseComponent process. Window owner {OwnerWindow.Id}");
    }
}