using Godot;

namespace Scripts.Gameplay;

public partial class VirtualDragPreview : TextureRect
{
    public override void _Ready()
    {
        MouseFilter = MouseFilterEnum.Ignore;
        ZIndex = 9999;
    }
}
