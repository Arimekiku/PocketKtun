using Godot;

namespace Scripts.Utils;

[GlobalClass]
public partial class ObjectProvider3D : BaseObjectProvider<Node3D, NodePool3D>
{
    [Export] private Node3D[] _objects;
    
    protected override Node3D[] Objects => _objects;
    
    public override void _Notification(int what)
    {
        if (what != NotificationPredelete)
            return;
        
        foreach (var pool in _pools.Values)
            pool.Dispose();
        
        foreach (var pool in _typePools.Values)
            pool.Dispose();
        
        _pools.Clear();
        _typePools.Clear();
    }

    protected override NodePool3D CreateNodePool(Node3D refNode, Node container, int poolSize) =>
        new NodePool3D(refNode, container, poolSize);
}