using Godot;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.Utils;

public abstract class BaseNodePool<TNode> where TNode : Node
{
    private readonly Dictionary<TNode, bool> _pool = new Dictionary<TNode, bool>();
    private readonly TNode _refNode;
    private readonly Node _container;

    protected BaseNodePool(TNode refNode, Node container, int poolSize = 0)
    {
        _refNode = refNode;
        _container = container;
    }

    public void Dispose()
    {
        foreach (var node in _pool.Keys.Where(GodotObject.IsInstanceValid))
            node.QueueFree();
        
        if (GodotObject.IsInstanceValid(_container))
            _container.QueueFree();
        
        _pool.Clear();
    }
    
    public virtual TNode GetNode()
    {
        var firstFreeNode = _pool.FirstOrDefault(pair => pair.Value).Key ?? AddNodeInPool();
        _pool[firstFreeNode] = true;

        return firstFreeNode;
    }

    public virtual bool ReturnNode(TNode node)
    {
        if (!_pool.ContainsKey(node))
            return false;
        
        node.Reparent(_container);
        _pool[node] = true;

        return true;
    }

    protected void FillPool(int poolSize)
    {
        for (var i = 0; i < poolSize; ++i)
            AddNodeInPool();
    }

    protected virtual TNode AddNodeInPool()
    {
        var duplicateNode = (TNode)_refNode.Duplicate();
        duplicateNode.Name = $"{_refNode.Name}_{_pool.Count}";
        _container.AddChild(duplicateNode);
        _pool.Add(duplicateNode, true);
        
        return duplicateNode;
    }
}