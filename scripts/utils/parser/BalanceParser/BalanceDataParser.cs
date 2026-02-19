namespace Scripts.Utils.SheetParser;

public class BalanceDataParser : Parser
{
    public BalanceDataParser(ICredentialProvider credentialProvider) : 
        base(new BalanceSaver(), credentialProvider) { }

    protected override bool TryGetSpreadSheet(out string spreadSheet)
    {
        spreadSheet = "16MnwnD5V6PK04pbW4aRobgCDWwI8AKoYFDbUlTBhnHw";
        return true;
    }
}