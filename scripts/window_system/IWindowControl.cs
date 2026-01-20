namespace Scripts.Gameplay.window_system;

public interface IWindowControl
{
    public void OpenWindow(WindowData windowData);
    public void CloseWindow(WindowData windowData);
}