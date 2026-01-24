using Godot;
using Scripts.Utils;
using System;
using System.Collections.Generic;

namespace Scripts.DIContainer;

internal class GodotContextProvider : IContextProvider
{
    private readonly Context _globalContext = new Context();
    private readonly Dictionary<Node, Context> _sceneContexts = new Dictionary<Node, Context>();
    
    public Context GetContext(object injectedObject)
    {
        if (injectedObject is not Node node)
            return _globalContext;

        var bindInstallerNode = node.FindParent<NodeContextInstallers>();
        if (bindInstallerNode == null) 
            return _globalContext;
        
        var context = _sceneContexts.GetValueOrDefault(bindInstallerNode, null);
        ExceptionsUtils.ThrowIfNull(context, $"Context is not found for {bindInstallerNode.Name} bind installer");
            
        return context;
    }

    public void RegisterBind(Bind newBind, object contextObject = null)
    {
        if (contextObject == null)
        {
            _globalContext.RegisterBind(newBind);
            return;
        }
        
        if (contextObject is not Node contextNode)
            throw new ArgumentException("Context in godot must be a node.", nameof(contextObject));
        
        if (contextNode is not NodeContextInstallers)
            throw new ArgumentException($"Context node {contextNode.Name} is not a SceneBindInstaller.");

        _sceneContexts.TryAdd(contextNode, new Context());
        _sceneContexts[contextNode].RegisterBind(newBind);
    }
}