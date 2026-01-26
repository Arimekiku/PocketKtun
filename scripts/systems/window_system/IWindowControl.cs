namespace Scripts.Systems.WindowSystem;

public interface IWindowControl
{
    public void OpenWindow(WindowData windowData);
    public void CloseWindow(WindowData windowData);
    public void CloseWindow(WindowIds windowId);
    public void CloseAllWindows();
}