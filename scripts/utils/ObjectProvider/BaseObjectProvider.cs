using Godot;
using Scripts.DIContainer;
using Scripts.Services;
using System;
using System.Collections.Generic;

namespace Scripts.Utils;

public abstract partial class BaseObjectProvider<TNode, TNodePool> : Node where TNode : Node
                                                                          where TNodePool : BaseNodePool<TNode>
{
    [Export] private string _objectProviderId;
    [Export] protected int _countInPool;
    
    protected readonly Dictionary<string, TNodePool> _pools = new Dictionary<string, TNodePool>();
    protected readonly Dictionary<Type, TNodePool> _typePools = new Dictionary<Type, TNodePool>();
    
    protected ILogger _logger;
    
    public string ProviderId => _objectProviderId;
    
    protected abstract TNode[] Objects { get; }
    
    private string ProviderName => string.IsNullOrEmpty(ProviderId) ? Name.ToString() : _objectProviderId;

    [Inject]
    private void Construct(ILogger logger)
    {
        _logger = logger;
    }

    public override void _Ready()
    {
        foreach (var node in Objects)
        {
            var poolContainer = new Node();
            poolContainer.Name = node.Name + "_pool";
            poolContainer.ProcessMode = ProcessModeEnum.Disabled;
            AddChild(poolContainer);
            
            var newPool = CreateNodePool(node, poolContainer, _countInPool);

            if (!_pools.TryAdd(node.Name.ToString(), newPool))
                _logger.LogWarning($"Node with name {node.Name} already exists, node is not added by name");
            
            if (!_typePools.TryAdd(node.GetType(), newPool))
                _logger.LogWarning($"Node with type {node.GetType().Name} already exists, node is not added by type");
        }
    }
    
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
    
    public TNode GetObject(string objectName)
    {
        if (_pools.TryGetValue(objectName, out TNodePool pool))
            return pool.GetNode();
        
        _logger.LogWarning($"Object with name {objectName} not found in object provider {ProviderName}");
        return null;
    }

    public TType GetObject<TType>() where TType : TNode
    {
        if (_typePools.TryGetValue(typeof(TType), out var value))
            return value.GetNode() as TType;
        
        _logger.LogWarning($"Object with type {typeof(TType)} not found in object provider {ProviderName}");
        return null;
    }

    public void ReturnObject(TNode node)
    {
        if (node == null)
        {
            _logger.LogWarning("Returned object is null.");
            return;
        }

        if (_pools.TryGetValue(node.Name, out var namePool))
        {
            if (namePool.ReturnNode(node))
                return;
        }

        if (_typePools.TryGetValue(node.GetType(), out var typyPool))
        {
            if (typyPool.ReturnNode(node))
                return;
        }
        
        _logger.LogError($"Node {node.Name} not found in object provider {ProviderName}");
    }

    protected abstract TNodePool CreateNodePool(TNode refNode, Node container, int poolSize);
}