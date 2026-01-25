using Godot;
using Scripts.Utils;
using System;

namespace Scripts.DIContainer;

internal class NodeContextRegister : IDisposable
{
    private readonly IContextProvider _contextProvider;
    private readonly SceneTree _sceneTree;
    
    public NodeContextRegister(IContextProvider contextProvider)
    {
        _contextProvider = contextProvider;
        _sceneTree = Engine.GetMainLoop() as SceneTree;
        
        ExceptionsUtils.ThrowIfNull(_sceneTree);

        _sceneTree!.NodeAdded += AddedNodeListener;
    }
    
    public void Dispose()
    {
        _sceneTree.NodeAdded -= AddedNodeListener;
        GC.SuppressFinalize(this);
    }
    
    private void AddedNodeListener(Node node)
    {
        if (node is NodeContextInstallers nodeContextInstallers)
        {
            nodeContextInstallers.InstallBinds();
            nodeContextInstallers.RegisterBinds(_contextProvider);
            return;
        }

        if (node is not GlobalContextInstallers globalContextInstallers) 
            return;
        
        globalContextInstallers.InstallBinds();
        globalContextInstallers.RegisterBinds(_contextProvider);
    }
}