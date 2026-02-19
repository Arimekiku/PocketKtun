using Godot;
using Scripts.Localization;
using System;

namespace Scripts.Utils.SheetParser;

public class LocalizationSaver : ParseSaver
{
    public LanguageMap LanguageMap;
    public LocalizationFile[] LocalizationFiles;

    private string SavePath = $"{ProjectSettings.GlobalizePath("res://")}/streaming_assets/localization";
    
    public override void Initialize()
    {
        
    }

    public override void Save()
    {
        SaveLanguageMap();
        SaveLocalization();
    }
    
    private void SaveLanguageMap()
    {
        if (LanguageMap == null)
            throw new NullReferenceException("Parsed language map is null");

        JsonUtils.SaveToFile(LanguageMap, SavePath, "LanguageMap.json");
    }
    
    private void SaveLocalization()
    {
        if (LocalizationFiles == null)
            throw new NullReferenceException("Parsed localization is null");

        for (var i = 0; i < LocalizationFiles.Length; ++i)
            JsonUtils.SaveToFile(LocalizationFiles[i], SavePath, $"Localization_{LanguageMap.GetLanguageCode(i)}.loc");
    }
}