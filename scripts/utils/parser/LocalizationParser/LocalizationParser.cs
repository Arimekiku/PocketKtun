namespace Scripts.Utils.SheetParser;

public class LocalizationParser : Parser
{
    public LocalizationParser(ICredentialProvider credentialProvider) : 
        base(new LocalizationSaver(), credentialProvider)
    {
    }

    protected override bool TryGetSpreadSheet(out string spreadSheet)
    {
        spreadSheet = "1JHg59r2Y7GYRA8HPptNs_Or3tb5SkL4NM9MkOGbDb7g";
        return true;
    }
}