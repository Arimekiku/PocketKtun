using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.Utils;

public class NodePool2D : BaseNodePool<CanvasItem>
{
    public NodePool2D(CanvasItem refNode, Node container, int poolSize = 0) : base(refNode, container, poolSize)
    {
        refNode.Visible = false;
        
        if (poolSize != 0)
            FillPool(poolSize);
    }
    
    public override CanvasItem GetNode()
    {
        var firstFreeNode = base.GetNode();
        firstFreeNode.Visible = true;
        
        return firstFreeNode;
    }

    public override bool ReturnNode(CanvasItem node)
    {
        var isReturned = base.ReturnNode(node);
        
        if (!isReturned)
            return false;
        
        node.Visible = false;
        
        return true;
    }

    protected override CanvasItem AddNodeInPool()
    {
        var duplicateNode = base.AddNodeInPool();
        duplicateNode.Visible = false;
        
        return duplicateNode;
    }
}