using Scripts.Utils.Loaders;
using Scripts.Utils;
using System.Collections.Generic;

namespace Scripts.Localization
{
    public class LocalizationLoader
    {
        private const string LANGUAGE_MAP_FILE_NAME = "LanguageMap.json";
        private static string PathToLocalizationFile => "/streaming_assets/localizations";
        
        private readonly Dictionary<int, JsonLoader<LocalizationFile>> _localizationFilesLoaders;
        
        public LanguageMap LanguageMap { get; }

        public LocalizationLoader()
        {
            LanguageMap = JsonUtils.LoadFromFile<LanguageMap>(PathToLocalizationFile, LANGUAGE_MAP_FILE_NAME);
            
            _localizationFilesLoaders = new Dictionary<int, JsonLoader<LocalizationFile>>();
            
            foreach (var id in LanguageMap.LangIds)
            {
                var fileName = $"Localization_{LanguageMap.GetLanguageCode(id)}.loc";
                
                _localizationFilesLoaders.Add(id, new JsonCompressedLoader<LocalizationFile>(PathToLocalizationFile, fileName));
            }
        }

        public LocalizationFile GetLocalizationFile(int languageId) => _localizationFilesLoaders[languageId].Target;
    }
}