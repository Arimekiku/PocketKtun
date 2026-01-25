namespace Scripts.WindowSystem;

public interface ILoadableWindow : IWindow
{
    public bool IsLoaded { get; }

    public void LoadWindow();
    public void UnloadWindow();
}