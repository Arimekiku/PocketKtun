using Godot;

namespace Scripts.UI.Buttons;

public partial class ExitWindowButton : Button
{
    [Export] private Control _target;
    
    public override void _Pressed()
    {
        _target.Hide();
    }
}