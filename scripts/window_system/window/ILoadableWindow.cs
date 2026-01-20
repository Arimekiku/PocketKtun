namespace Scripts.Gameplay.window_system;

public interface ILoadableWindow : IWindow
{
    public bool IsLoaded { get; }

    public void LoadWindow();
    public void UnloadWindow();
}