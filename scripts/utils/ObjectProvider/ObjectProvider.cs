using Godot;
using Scripts.DIContainer;
using Scripts.Services;
using System.Collections.Generic;

namespace Scripts.Utils;

[GlobalClass]
public partial class ObjectProvider : Node, IObjectProvider
{
    [Export] private string _objectProviderId;
    [Export] private int _countInPool;
    [Export] private Node[] _objects;
    
    private readonly Dictionary<string, NodePool> _pools =  new Dictionary<string, NodePool>();
    
    private ILogger _logger;
    private IObjectProviderRegistry _objectProviderRegistry;
    
    public string ProviderId => _objectProviderId;
    
    private string ProviderName => string.IsNullOrEmpty(ProviderId) ? Name.ToString() : _objectProviderId;
    
    [Inject]
    private void Constructor(ILogger logger, IObjectProviderRegistry objectProviderRegistry)
    {
        _logger = logger;
        _objectProviderRegistry = objectProviderRegistry;
    }
    
    public override void _Ready()
    {
        foreach (var node in _objects)
        {
            var poolContainer = new Node();
            poolContainer.Name = node.Name + "_pool";
            poolContainer.ProcessMode = ProcessModeEnum.Disabled;
            AddChild(poolContainer);
            
            _pools.Add(node.Name.ToString(), new NodePool(node, poolContainer, _countInPool));
        }
        
        if (!string.IsNullOrEmpty(_objectProviderId))
            _objectProviderRegistry.RegisterObjectProvider(this);
    }

    public override void _Notification(int what)
    {
        if (what != NotificationPredelete)
            return;
        
        foreach (var pool in _pools.Values)
            pool.Dispose();
        
        _pools.Clear();
        
        if (!string.IsNullOrEmpty(_objectProviderId))
            _objectProviderRegistry.UnregisterObjectProvider(this);
    }

    public Node GetObject(string objectName)
    {
        if (!_pools.ContainsKey(objectName))
            _logger.LogWarning($"Object {objectName} not found in object provider {ProviderName}");

        return _pools[objectName].GetNode();
    }

    public void ReturnObject(Node node)
    {
        if (node == null)
        {
            _logger.LogWarning("Returned object is null.");
            return;
        }

        if (!_pools.TryGetValue(node.Name, out var pool))
        {
            _logger.LogError($"Node {node.Name} not found in object provider {ProviderName}");
            return;
        }
        
        pool.ReturnNode(node);
    }
}
