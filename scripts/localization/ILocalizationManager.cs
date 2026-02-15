using System;

namespace Scripts.Localization;

public interface ILocalizationManager
{
    public event Action OnLanguageChangedEvent;
    
    public void ChangeLanguage(string language);
    public void ChangeLanguage(int languageId);
    public string GetLocalization(string locId);
}