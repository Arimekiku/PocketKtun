using Godot;

namespace Scripts.DIContainer;

[GlobalClass]
internal partial class NodeContext : GodotContext
{
    [Export] private Node _overrideContextNode;
    
    public override object ContextObject => _overrideContextNode ?? this;
}