using Scripts.Localization;
using System.Collections.Generic;

namespace Scripts.Utils;

public class LanguagesParsingContext : GenericParsingContext<LanguageMap>
{
    public override ParserType ContextType => ParserType.Localization;
    public override string ParseRange => "Localization!A1:Z5";

    public override void ParseTarget()
    {
        var languagesIds = new List<int>();
        var languagesCodes = new List<string>();
        
        var defaultLanguageId = ValueRange.ParseInt(0, 1);

        for (var i = 0; i < ColumnCount - 1; ++i)
        {
            languagesIds.Add((byte)i);
            languagesCodes.Add(ValueRange.ParseString(4, i + 1));
        }

        Target = new LanguageMap(languagesIds, languagesCodes, defaultLanguageId);
    }
}