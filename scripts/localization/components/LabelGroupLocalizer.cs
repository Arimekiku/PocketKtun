using Godot;

namespace Scripts.Localization;

[GlobalClass]
public partial class LabelGroupLocalizer : LabelLocalizer
{
    [Export] private Label[] _labelGroup;

    protected override void UpdateLabelText()
    {
        base.UpdateLabelText();

        foreach (var label in _labelGroup)
            label.SetText(label.Text);
    }
}