using Godot;
using Scripts.DIContainer;

namespace Scripts.WindowSystem;

[GlobalClass]
public partial class CloseWindowButton : Button
{
    [Export] private WindowIds _windowIds;

    private IWindowControl _windowControl;

    [Inject]
    private void Construct(IWindowControl windowControl)
    {
        _windowControl = windowControl;
    }

    public override void _Ready()
    {
        Pressed +=  ButtonPressedListener;
    }

    public override void _Notification(int what)
    {
        if (what != NotificationPredelete)
            return;
        
        Pressed -= ButtonPressedListener;
    }

    private void ButtonPressedListener()
    {
        _windowControl.CloseWindow(_windowIds);
    }
}