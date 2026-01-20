namespace Scripts.Gameplay.window_system;

public interface IWindow
{
    public void OpenWindow();
    public void CloseWindow();
    public void FocusWindow();
    public void UnfocusWindow();
}