namespace Scripts.WindowSystem;

public interface IWindow
{
    public void OpenWindow();
    public void CloseWindow();
    public void FocusWindow();
    public void UnfocusWindow();
}