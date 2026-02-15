using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.Localization;

public class LanguageMap
{
    private readonly int _defaultLanguageId;
    private readonly Dictionary<int, string> _languageMap;
    private readonly Dictionary<string, int> _languageCodeMap;

    public int DefaultLanguageId => _defaultLanguageId;
    public string DefaultLanguageCode => _languageMap[DefaultLanguageId];
    private Dictionary<string, int> LanguageCodeMap => _languageCodeMap;

    public IReadOnlyList<int> LangIds => _languageMap.Keys.ToList();
    public IReadOnlyList<string> LangCodes => _languageMap.Values.ToList();

    public LanguageMap(IReadOnlyList<int> languageIds, IReadOnlyList<string> languageCodes, int defaultLanguageId)
    {
        if (languageCodes.Count != languageIds.Count)
            throw new ArgumentException("Language IDs count does not match language Codes count");

        _defaultLanguageId = defaultLanguageId;

        var languageCount = languageIds.Count;
        _languageMap = new Dictionary<int, string>(languageCount);
        _languageCodeMap = new Dictionary<string, int>(languageCount);

        for (var i = 0; i < languageCount; ++i)
        {
            _languageMap.Add(languageIds[i], languageCodes[i]);
            _languageCodeMap.Add(languageCodes[i], languageIds[i]);
        }
    }

    public int GetLanguageId(string languageCode)
    {
        if (LanguageCodeMap.TryGetValue(languageCode, out var id))
            return id;

        //Logger.LogError(LoggerModule.Localization, $"Language with code {languageCode} is not found. Returning default language id.");
        return DefaultLanguageId;
    }

    public string GetLanguageCode(int languageId)
    {
        if (_languageMap.TryGetValue(languageId, out var code))
            return code;

        //Logger.LogError(LoggerModule.Localization, $"Language with id {languageId} is not found. Returning default language code.");
        return DefaultLanguageCode;
    }
}