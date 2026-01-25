using Godot;

namespace Scripts.WindowSystem;

[GlobalClass]
public abstract partial class BaseZStrategy : Node
{
    public abstract float GetZ();
}