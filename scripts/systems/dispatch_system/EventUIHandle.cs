using System;
using Godot;

namespace Scripts.Systems.DispatchSystem;

public partial class EventUIHandle : TextureRect
{
    [Export] private Node3D _positionMarker;

    public event Action<DispatchUnit3D> OnDropEvent;

    public void Initialize()
    {
        var camera = GetViewport().GetCamera3D();
        if (_positionMarker == null || camera == null)
            return;
        
        var screenPos = camera.UnprojectPosition(_positionMarker.GlobalPosition);
        Position = screenPos - (Size / 2);
        GD.Print(Position);
    }

    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        return data.As<Node3D>() is DispatchUnit3D;
    }

    public override void _DropData(Vector2 atPosition, Variant data)
    {
        var unit = data.As<DispatchUnit3D>();
        GD.Print($"Dispatching {unit.Data.Name} to event");

        OnDropEvent!.Invoke(unit);
    }
}
