namespace Scripts.Utils;

public interface IParserSettingsProvider
{
    public string AccountCredentialJson { get; }

    public bool TryGetSpreadSheet(ParserType parserType,out string spreadSheet);
    public string GetSpreadSheet(ParserType parserType);
}