using Godot;
using Godot.Collections;

namespace Scripts.Utils;

[GlobalClass]
public partial class GodotParserSettingsProvider : Resource, IParserSettingsProvider
{
    [Export] private string _accountCredentialJson;
    [Export] private Dictionary<ParserType, string> _spreadSheets;
    
    public string AccountCredentialJson => _accountCredentialJson;
    
    public bool TryGetSpreadSheet(ParserType parserType, out string spreadSheet)
    {
        spreadSheet = GetSpreadSheet(parserType);
        
        return !string.IsNullOrEmpty(spreadSheet);
    }

    public string GetSpreadSheet(ParserType parserType) =>  _spreadSheets[parserType];
}