using Godot;
using Scripts.DIContainer;
using Scripts.Utils;
using System;

namespace Scripts.Localization;

[GlobalClass]
public partial class LabelLocalizer : Label
{
    public event Action OnTextChangedEvent;

    [Export] private string _locId;

    private ILocalizationManager _localizationManager;
    private string[] _parameters;

    public string LocId
    {
        get => _locId;
        set
        {
            _locId = value;
            _parameters = null;
            UpdateLabelText();
        }
    }

    [Inject]
    public void Construct(ILocalizationManager localizationManager)
    {
        _localizationManager = localizationManager;
    }
    
    public override void _Ready()
    {
        _localizationManager.OnLanguageChangedEvent += OnLanguageChanged;
        UpdateLabelText();
    }
    
    public override void _Notification(int what)
    {
        if (what != NotificationPredelete)
            return;
        
        _localizationManager.OnLanguageChangedEvent -= OnLanguageChanged;
    }

    public void SetTextInLabel(string text)
    {
        _parameters = null;
        Text = text;
        OnTextChangedEvent?.Invoke();
    }
    
    public void SetParameters(params string[] parameters)
    {
        _parameters = parameters;
        UpdateLabelText();
    }
    
    public void SetParameters(string locId, params string[] parameters)
    {
        _parameters = parameters;
        LocId = locId;
    }
    
    private void OnLanguageChanged()
    {
        UpdateLabelText();
    }
    
    protected virtual void UpdateLabelText()
    {
        var text = GetText(LocId);
        
        if (!_parameters.IsNullOrEmpty())
        {
            text = string.Format(text, _parameters);
        }
                
        Text = text;
        OnTextChangedEvent?.Invoke();
    }
    
    private string GetText(string locId) => _localizationManager.GetLocalization(locId);
}