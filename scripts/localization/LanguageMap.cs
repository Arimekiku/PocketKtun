using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Scripts.Localization;

public class LanguageMap
{
    [JsonInclude] private readonly int _defaultLanguageId;
    [JsonInclude] private readonly Dictionary<int, string> _languageMap;
    [JsonInclude] private readonly Dictionary<string, int> _languageCodeMap;

    [JsonIgnore] public int DefaultLanguageId => _defaultLanguageId;
    [JsonIgnore] public string DefaultLanguageCode => _languageMap[DefaultLanguageId];
    [JsonIgnore] private Dictionary<string, int> LanguageCodeMap => _languageCodeMap;

    [JsonIgnore] public IReadOnlyList<int> LangIds => _languageMap.Keys.ToList();
    [JsonIgnore] public IReadOnlyList<string> LangCodes => _languageMap.Values.ToList();

    [JsonConstructor]
    public LanguageMap(Dictionary<int, string> languageMap, Dictionary<string, int> languageCodeMap, int defaultLanguageId)
    {
        _languageMap = languageMap;
        _languageCodeMap = languageCodeMap;
        _defaultLanguageId = defaultLanguageId;
    }
    
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