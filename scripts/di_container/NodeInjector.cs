using Godot;
using Scripts.Utils;
using System;

namespace Scripts.DIContainer;

internal class NodeInjector : IDisposable
{
    private readonly DiContainer _diContainer;
    private readonly SceneTree _sceneTree;
    
    public NodeInjector(DiContainer diContainer)
    {
        _diContainer = diContainer;
        _sceneTree = Engine.GetMainLoop() as SceneTree;
        
        ExceptionsUtils.ThrowIfNull(_sceneTree);

        _sceneTree!.NodeAdded += AddedNodeListener;
    }

    public void Dispose()
    {
        _sceneTree.NodeAdded -= AddedNodeListener;
    }

    private void AddedNodeListener(Node node) => _diContainer.Inject(node);
}