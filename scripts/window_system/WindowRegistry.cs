using Godot;
using Scripts.DIContainer;
using Scripts.Services;
using Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.WindowSystem;

public class WindowRegistry : IWindowRegistry, IDisposable
{
    public event Action<IWindow> OnActiveWindowChangedEvent;
    public event Action<IWindow> OnWindowRegisteredEvent;

    private readonly Dictionary<WindowIds, IWindow> _openedWindows = new  Dictionary<WindowIds, IWindow>();
    private readonly Dictionary<WindowIds, IWindow> _registeredWindows = new Dictionary<WindowIds, IWindow>();
    
    private ILogger _logger;
    private SceneTree _sceneTree;
    
    public IWindow ActiveWindow { get; private set; }
    public IReadOnlyDictionary<WindowIds, IWindow> OpenedWindows => _openedWindows;
    public IReadOnlyDictionary<WindowIds, IWindow> RegisteredWindows => _registeredWindows;

    [Inject]
    public WindowRegistry(ILogger logger)
    {
        _logger = logger;
        Subscribe();
    }

    public void Dispose()
    {
        Unsubscribe();
    }

    public void RegisterWindow(IWindow window)
    {
        _registeredWindows.Add(window.Id, window);
        OnWindowRegisteredEvent?.Invoke(window);
        window.Initialize();
        
        _logger.Log($"Window {window.Id} registered");
    }

    public void UnregisterWindow(IWindow window)
    {
        _registeredWindows.Remove(window.Id);
        window.Dispose();
        
        _logger.Log($"Window {window.Id} registered");
    }

    public void RegisterOpenWindow(IWindow window)
    {
        ActiveWindow?.UnfocusWindow();
        
        ActiveWindow = window;
        _openedWindows.Add(window.Id, window);
        OnActiveWindowChangedEvent?.Invoke(window);
    }

    public void  RegisterCloseWindow(IWindow window)
    {
        _openedWindows.Remove(window.Id);
        var lastWindow = _openedWindows.LastOrDefault().Value;
        ActiveWindow = lastWindow;
        
        ActiveWindow?.FocusWindow();
    }
    
    public bool IsWindowOpened(WindowIds window) => OpenedWindows.ContainsKey(window);
    
    public bool IsWindowRegistered(WindowIds window) => RegisteredWindows.ContainsKey(window);
    
    public bool IsWindowActive(WindowIds window) => ActiveWindow.Id == window;

    private void Subscribe()
    {
        _sceneTree = Engine.GetMainLoop() as SceneTree;

        ExceptionsUtils.ThrowIfNull(_sceneTree);
        
        _sceneTree!.NodeAdded += AddedNodeListener;
        _sceneTree!.NodeRemoved += RemoveNodeListener;
    }

    private void Unsubscribe()
    {
        _sceneTree.NodeAdded -= AddedNodeListener;
        _sceneTree.NodeRemoved -= RemoveNodeListener;
    }

    private void AddedNodeListener(Node node)
    {
        if (node is not IWindow window)
            return;
        
        RegisterWindow(window);
    }

    private void RemoveNodeListener(Node node)
    {
        if (node is not IWindow window)
            return;
        
        UnregisterWindow(window);
    }
}