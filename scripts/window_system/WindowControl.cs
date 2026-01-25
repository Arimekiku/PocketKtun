using Scripts.DIContainer;
using Scripts.Services;

namespace Scripts.WindowSystem;

public class WindowControl : IWindowControl
{
    private IWindowRegistry _windowRegistry;
    private ILogger _logger;
    
    [Inject]
    public WindowControl(IWindowRegistry windowRegistry, ILogger logger)
    {
        _windowRegistry = windowRegistry;
        _logger = logger;
    }
    
    public void OpenWindow(WindowData windowData)
    {
        if (!_windowRegistry.IsWindowRegistered(windowData.WindowId))
        {
            _logger.LogError($"Window {windowData.WindowId} dont registered");
            return;
        }
        
        if (_windowRegistry.IsWindowOpened(windowData.WindowId))
        {
            _logger.LogWarning($"Window {windowData.WindowId} already opened");
            return;
        }
        
        var window = _windowRegistry.RegisteredWindows[windowData.WindowId];
        
        _windowRegistry.RegisterOpenWindow(window);
        window.OpenWindow();
    }

    public void CloseWindow(WindowData windowData)
    {
        if (!_windowRegistry.IsWindowRegistered(windowData.WindowId))
        {
            _logger.LogError($"Window {windowData.WindowId} dont registered");
            return;
        }

        if (!_windowRegistry.IsWindowOpened(windowData.WindowId))
        {
            _logger.LogWarning($"Window {windowData.WindowId} already closed");
            return;
        }
        
        var  window = _windowRegistry.RegisteredWindows[windowData.WindowId];
        
        _windowRegistry.RegisterCloseWindow(window);
        window.CloseWindow();
    }

    public void CloseWindow(WindowIds windowId) => CloseWindow(new WindowData(windowId));

    public void CloseAllWindows()
    {
        
    }
}