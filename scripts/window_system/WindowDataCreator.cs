using Godot;

namespace Scripts.WindowSystem;

[GlobalClass]
public abstract partial class WindowDataCreator : Node
{
    public abstract WindowData CreateData();
}
