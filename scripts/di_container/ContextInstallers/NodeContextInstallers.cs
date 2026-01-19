using Godot;

namespace Scripts.DIContainer;

[GlobalClass]
internal partial class NodeContextInstallers : GodotContextInstallers
{
    [Export] private Node _overrideContextNode;
    
    public override object ContextObject => _overrideContextNode ?? this;
}