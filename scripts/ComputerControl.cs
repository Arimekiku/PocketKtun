using Godot;

namespace Scripts.Gameplay;

public partial class ComputerControl : Control
{
    [Export] private Control _mousePointer;
    
    public Vector2 MousePosition => _mousePointer.Position;

    public void UpdateMousePosition(Vector2 position)
    {
        _mousePointer.Position = position;
    }
}