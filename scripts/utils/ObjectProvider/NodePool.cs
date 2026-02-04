using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.Utils;

internal class NodePool
{
    private readonly Dictionary<Node, bool>  _pool;

    private Node _refNode;
    private Node _container;
    
    public NodePool(Node refNode, Node container, int poolSize = 0)
    {
        _refNode = refNode;
        _container = container;

        _pool = new Dictionary<Node, bool>();
        
        if (poolSize != 0)
            FillPool(poolSize);
    }

    public void Dispose()
    {
        _container.Free();
        
        foreach (var node in _pool.Keys)
            node.Free();
        
        _pool.Clear();
    }
    
    public Node GetNode()
    {
        var firstFreeNode = _pool.FirstOrDefault(pair => pair.Value).Key ?? AddNodeInPool();
        _pool[firstFreeNode] = false;
        
        return firstFreeNode;
    }

    public void ReturnNode(Node node)
    {
        node.Reparent(_container);
        _pool[node] = true;
    }

    private void FillPool(int numOfItems)
    {
        for (var i = 0; i < numOfItems; ++i)
            AddNodeInPool();
    }

    private Node AddNodeInPool()
    {
        var duplicateNode = _refNode.Duplicate();
        _container.AddChild(duplicateNode);
        _pool.Add(duplicateNode, true);
        
        return duplicateNode;
    } 
    
}