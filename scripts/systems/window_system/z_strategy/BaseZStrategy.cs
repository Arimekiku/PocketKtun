using Godot;

namespace Scripts.Systems.WindowSystem;

[GlobalClass]
public abstract partial class BaseZStrategy : Node
{
    public abstract float GetZ();
}