namespace Scripts.Utils;

public class BalanceDataParser : Parser
{
    protected override ParserType ParserType => ParserType.Balance;
    
    public BalanceDataParser(ISerializeRule serializeRule, IParserSettingsProvider settingsProvider) :
        base(serializeRule, settingsProvider)
    {
    }
}