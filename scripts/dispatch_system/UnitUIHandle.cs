using Godot;
using Scripts.Gameplay;

namespace Scripts.DispatchSystem;

public partial class UnitUIHandle : TextureRect
{
    [Export] public DispatchUnit3D TargetUnit;
    [Export] private CharacterUIHandle _dispatchUiHandle;
    [Export] private Camera3D _dispatchCamera;
    [Export] private float _dragThreshold = 8f;
    
    private Vector2 _pressPos;
    private bool _pressed;

    public override void _Process(double delta)
    {
        if (TargetUnit == null || _dispatchCamera == null)
            return;
        
        var screenPos = _dispatchCamera.UnprojectPosition(TargetUnit.GlobalPosition);
        if (_dispatchCamera.IsPositionBehind(TargetUnit.GlobalPosition))
        {
            Hide();
            return;
        }
        
        Show();
        Position = screenPos - (Size / 2);
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event.IsActionPressed(Inputs.LeftMouse))
        {
            _pressed = true;
            _pressPos = GetViewport().GetMousePosition();
        }

        if (@event.IsActionReleased(Inputs.LeftMouse))
        {
            if (_pressed && GetGlobalRect().HasPoint(_pressPos))
                OnClick();

            _pressed = false;
        }

        if (@event is InputEventMouseMotion motion && _pressed)
        {
            if (_pressPos.DistanceSquaredTo(motion.Position) <= _dragThreshold * _dragThreshold) 
                return;
            
            AcceptEvent();
        }
    }

    public override Variant _GetDragData(Vector2 atPosition)
    {
        if (TargetUnit.IsBusy)
            return new Variant();
        
        var preview = new TextureRect
        {
            Texture = Texture,
            ExpandMode = ExpandModeEnum.IgnoreSize,
            Size = new Vector2(64, 64)
        };

        SetDragPreview(preview);
        return TargetUnit;
    }

    private void OnClick()
    {
        GD.Print($"Clicked unit: {TargetUnit?.Name}");
        
        _dispatchUiHandle.UpdateDisplay(TargetUnit);
        _dispatchUiHandle.Show();
    }
}