using Scripts.Localization;
using System.Collections.Generic;

namespace Scripts.Utils;

public class LocalizationParsingContext : GenericParsingContext<List<LocalizationFile>>
{
    public override ParserType ContextType => ParserType.Localization;
    public override string ParseRange => "Localization!A6:Z";

    public override void ParseTarget()
    {
        var localizationFiles = new List<LocalizationFile>();

        var ids = GetLocIds();
        var languageCount = ColumnCount - 1;

        for (var i = 0; i < languageCount; ++i)
            localizationFiles.Add(new LocalizationFile(ids, GetLocalizationText(i)));

        Target = localizationFiles;
    }

    private List<string> GetLocIds()
    {
        var ids = new List<string>(RowCount);

        for (var i = 0; i < RowCount; ++i)
            ids.Add(ValueRange.ParseString(i, 0));

        return ids;
    }

    private List<string> GetLocalizationText(int offset)
    {
        var localizationText = new List<string>();

        for (var i = 0; i < RowCount; ++i)
        {
            var text = ValueRange.ParseString(i, offset + 1);
            localizationText.Add(text);
        }

        return localizationText;
    }
}