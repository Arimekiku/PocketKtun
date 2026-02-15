using Godot;

namespace Scripts.Utils;

public class NodePool3D : BaseNodePool<Node3D>
{
    public NodePool3D(Node3D refNode, Node container, int poolSize = 0) : base(refNode, container, poolSize)
    {
        refNode.Visible = false;
        
        if (poolSize != 0)
            FillPool(poolSize);
    }
    
    public override Node3D GetNode()
    {
        var firstFreeNode = base.GetNode();
        firstFreeNode.Visible = true;
        
        return firstFreeNode;
    }

    public override bool ReturnNode(Node3D node)
    {
        var isReturned = base.ReturnNode(node);
        
        if (!isReturned)
            return false;
        
        node.Visible = false;
        
        return true;
    }

    protected override Node3D AddNodeInPool()
    {
        var duplicateNode = base.AddNodeInPool();
        duplicateNode.Visible = false;
        
        return duplicateNode;
    }
}