namespace Scripts.Utils;

public class LocalizationParser : Parser
{
    protected override ParserType ParserType => ParserType.Localization;
    
    public LocalizationParser(ISerializeRule serializeRule, IParserSettingsProvider parserSettingsProvider) : 
        base(serializeRule, parserSettingsProvider)
    {
    }
}