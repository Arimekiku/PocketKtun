using Godot;

namespace Scripts.Systems.WindowSystem;

[GlobalClass]
public partial class StaticZStrategy : BaseZStrategy
{
    [Export] private float _zPosition;
    
    public override float GetZ() => _zPosition;
}