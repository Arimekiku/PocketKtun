using Godot;
using Scripts.Gameplay;

namespace Scripts.Systems.DispatchSystem;

public partial class UnitUIHandle : TextureRect
{
    [Export] private DispatchUnit3D _targetUnit;
    [Export] private CharacterUIHandle _dispatchUiHandle;
    [Export] private Camera3D _dispatchCamera;
    [Export] private TextureRect _virtualMouse;
    [Export] private float _dragThreshold = 8f;
    
    private Vector2 _pressPos;
    private VirtualDragPreview _virtualDragPreview;
    private bool _pressed;

    public override void _Process(double delta)
    {
        if (_targetUnit == null || _dispatchCamera == null)
            return;
        
        var screenPos = _dispatchCamera.UnprojectPosition(_targetUnit.GlobalPosition);
        if (_dispatchCamera.IsPositionBehind(_targetUnit.GlobalPosition))
        {
            Hide();
            return;
        }
        
        Show();
        Position = screenPos - (Size / 2);
        
        if (_virtualDragPreview != null)
            _virtualDragPreview.GlobalPosition = _virtualMouse.GlobalPosition - _virtualDragPreview.Size / 2f;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionReleased(Inputs.LeftMouse))
        {
            _virtualDragPreview?.QueueFree();
            _virtualDragPreview = null;
        }
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
            if (_targetUnit.IsBusy)
                return;
            
            ForceDrag(_targetUnit, null);
            
            _virtualDragPreview?.QueueFree();
            _virtualDragPreview = new VirtualDragPreview
            {
                Texture = Texture,
                ExpandMode = ExpandModeEnum.IgnoreSize,
                Size = new Vector2(64, 64)
            };
            AddChild(_virtualDragPreview);
            
            AcceptEvent();
        }
    }

    private void OnClick()
    {
        GD.Print($"Clicked unit: {_targetUnit?.Name}");
        
        _dispatchUiHandle.UpdateDisplay(_targetUnit);
        _dispatchUiHandle.Show();
    }
}