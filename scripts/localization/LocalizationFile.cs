using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Scripts.Localization
{
    public class LocalizationFile
    {
        [JsonInclude] private readonly Dictionary<string, string> _localizationValues;
        
        [JsonConstructor]
        public LocalizationFile(Dictionary<string, string> localizationValues)
        {
            _localizationValues = localizationValues;
        }
        
        public LocalizationFile(List<string> idsList, List<string> textList)
        {
            _localizationValues = new Dictionary<string, string>();
            
            for (var i = 0; i < idsList.Count; ++i)
                _localizationValues.Add(idsList[i], textList[i]);
        }
        
        public string GetText(string locId) => _localizationValues.GetValueOrDefault(locId);
    }
}