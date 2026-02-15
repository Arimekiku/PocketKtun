using Godot;
using Scripts.DIContainer;

namespace Scripts.WindowSystem;

[GlobalClass]
public partial class OpenWindowButton : Button
{
    [Export] private WindowIds _windowIds;
    [Export] private WindowDataCreator _dataCreator;
    
    private IWindowControl _windowControl;
    
    private WindowData WindowData => _dataCreator == null ? new WindowData(_windowIds) : _dataCreator.CreateData();
    
    [Inject]
    private void Constructor(IWindowControl windowControl)
    {
        _windowControl = windowControl;
    }

    public override void _Ready()
    {
        Pressed += ButtonPressedListener;
    }

    public override void _Notification(int what)
    {
        if (what != NotificationPredelete)
            return;
        
        Pressed -= ButtonPressedListener;
    }

    private void ButtonPressedListener()
    {
        _windowControl.OpenWindow(WindowData);
    }
}
