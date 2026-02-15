using System.Collections.Generic;

namespace Scripts.Localization
{
    public class LocalizationFile
    {
        private readonly Dictionary<string, string> _localizationValue;
        
        public LocalizationFile(Dictionary<string, string> localizationValue)
        {
            _localizationValue = localizationValue;
        }
        
        public LocalizationFile(List<string> idsList, List<string> textList)
        {
            _localizationValue = new Dictionary<string, string>();
            
            for (var i = 0; i < idsList.Count; ++i)
                _localizationValue.Add(idsList[i], textList[i]);
        }
        
        public string GetText(string locId) => _localizationValue.GetValueOrDefault(locId);
    }
}