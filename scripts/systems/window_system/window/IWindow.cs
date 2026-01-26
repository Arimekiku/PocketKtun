using System;

namespace Scripts.Systems.WindowSystem;

public interface IWindow : IDisposable
{
    public WindowIds Id { get; }
    public float VisualZ { get; }

    public void Initialize();
    public void OpenWindow();
    public void CloseWindow();
    public void FocusWindow();
    public void UnfocusWindow();
}