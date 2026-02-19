using Scripts.Localization;
using Scripts.Utils.SheetParser;
using System.Collections.Generic;

namespace Scripts.ParsingContexts;

public class LocalizationContext : GenericParsingContext<LocalizationSaver>
{
    public override string ParseRange => "Localization!A1:Q";
    
    public LocalizationContext(ParseSaver saver) : base(saver) { }
    
    public override void ParseTarget()
    {
        Saver.LanguageMap = ParseLanguageMap();
        Saver.LocalizationFiles = ParseLanguageFiles();
    }

    private LanguageMap ParseLanguageMap()
    {
        var ids = new List<int>();
        var languageCodes = new List<string>();
        
        var defaultLanguage = ParseInt(0, 1);
        
        for (var i = 1; i < ColumnCount; ++i)
        {
            ids.Add(i - 1);
            languageCodes.Add(ParseString(2, i));
        }
        
        return new LanguageMap(ids, languageCodes, defaultLanguage);
    }

    private LocalizationFile[] ParseLanguageFiles()
    {
        var ids = new List<string>();
        
        for (var i = 3; i < RowCount; ++i)
            ids.Add(ParseString(i, 0));

        var files = new List<LocalizationFile>();
        
        for (var i = 1; i < ColumnCount; ++i)
        {
            var textList = new List<string>();
            
            for (var j = 3; j < RowCount; ++j)
                textList.Add(ParseString(j, i));
            
            files.Add(new LocalizationFile(ids, textList));
        }
        
        return files.ToArray();
    }
}