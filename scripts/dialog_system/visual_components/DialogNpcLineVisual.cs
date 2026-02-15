using Godot;
using Scripts.DIContainer;
using Scripts.Localization;

namespace Scripts.DialogSystem.Visual;

[GlobalClass]
public partial class DialogNpcLineVisual : VBoxContainer
{
    [Export] private Label _textLabel;
    [Export] private Label _nameLabel;

    private ILocalizationManager _localizationManager;

    [Inject]
    public void Construct(ILocalizationManager localizationManager)
    {
        _localizationManager = localizationManager;
    }

    public void Initialize(DialogBlock dialogBlock)
    {
        SetText(dialogBlock.DialogLine);
    }

    public void Deinitialize()
    {
        _textLabel.Text = "";
        _nameLabel.Text = "";
    }
    
    private void SetText(DialogLine dialogLine)
    {
        _textLabel.Text = _localizationManager.GetLocalization(dialogLine.TextLineId);
        _nameLabel.Text = _localizationManager.GetLocalization(dialogLine.SpeakerId);
    }
}
