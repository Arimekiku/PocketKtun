/*using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace parmesan123_toolkit.Parser
{
    public class LocalizationSerializeRule : ISerializeRule
    {
        private static string SavePath = Application.dataPath + "/StreamingAssets/Localization";

        private LanguageMap _mapping; 
        
        public void SerializeObjects(object[] serializeObjects)
        {
            SerializeLanguageMap(serializeObjects);
            SerializeLocalization(serializeObjects);
        }

        private void SerializeLanguageMap(object[] objects)
        {
            _mapping = objects.FirstOrDefault(o => o.GetType() == typeof(LanguageMap)) as LanguageMap;

            if (_mapping == null)
                throw new NullReferenceException("Parsed language map is null");

            var json = JsonUtils.Serialize(_mapping);
            
            JsonUtils.SaveToFile(json, SavePath, "LanguageMap.json");
        }

        private void SerializeLocalization(object[] objects)
        {
            var localizations = objects.FirstOrDefault(o => o.GetType() == typeof(List<LocalizationFile>)) as List<LocalizationFile>;
            if (localizations == null)
                throw new NullReferenceException("Parsed localization is null");
            
            for (var i = 0; i < localizations.Count; ++i)
            {
                JsonUtils.SaveCompressed(localizations[i], SavePath, $"Localization_{_mapping.GetLanguageCode((byte)i)}.loc");
            }
        } 
    }
}*/