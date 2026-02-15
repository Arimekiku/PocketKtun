using Godot;
using System;

namespace Scripts.Localization;

public class LocalizationManager : ILocalizationManager
{
    public event Action OnLanguageChangedEvent;
    
    private LocalizationLoader _localizationLoader;
    private LocalizationFile _currentLocalizationFile;
    
    public int CurrentLanguageId { get; private set; }
    public LanguageMap LanguageMap { get; private set; }
    
    private string SystemLanguageCode => OS.GetLocaleLanguage();
    
    public LocalizationManager()
    {
        _localizationLoader = new LocalizationLoader();
        LanguageMap = _localizationLoader.LanguageMap;
        
        ChangeLanguage(SystemLanguageCode);
    }
    
    public void Initialize()
    {
        _localizationLoader = new LocalizationLoader();
        LanguageMap = _localizationLoader.LanguageMap;

        // ToDo Need to add saving of user selected language and setting it during initialization

        ChangeLanguage(SystemLanguageCode);
    }
    
    public void ChangeLanguage(string language)
    {
        var languageId = LanguageMap.GetLanguageId(language);
        ChangeLanguage(languageId);
    }
    
    public void ChangeLanguage(int languageId)
    {
        _currentLocalizationFile = _localizationLoader.GetLocalizationFile(languageId);
        CurrentLanguageId = languageId;

        OnLanguageChangedEvent?.Invoke();
    }
    
    public string GetLocalization(string locId)
    {
        var text = _currentLocalizationFile.GetText(locId);
        return string.IsNullOrEmpty(text)
            ? $"{LanguageMap.GetLanguageCode(CurrentLanguageId).ToUpper()}_{locId}"
            : text;
    }
}