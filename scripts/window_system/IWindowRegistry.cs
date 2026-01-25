using System;
using System.Collections.Generic;

namespace Scripts.WindowSystem;

public interface IWindowRegistry
{
    public event Action<IWindow> OnActiveWindowChangedEvent;
    public event Action<IWindow> OnWindowRegisteredEvent;
    
    public IWindow ActiveWindow { get; }
    public IReadOnlyDictionary<WindowIds, IWindow> OpenedWindows { get; }
    public IReadOnlyDictionary<WindowIds, IWindow> RegisteredWindows { get; }
    
    public void RegisterWindow(IWindow window);
    public void UnregisterWindow(IWindow window);
    public void RegisterOpenWindow(IWindow window);
    public void RegisterCloseWindow(IWindow window);
    public bool IsWindowActive(WindowIds window);
    public bool IsWindowRegistered(WindowIds window);
    public bool IsWindowOpened(WindowIds window);
}