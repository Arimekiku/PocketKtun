using Godot;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.Utils;

public abstract class Parser
{
    private readonly ISerializeRule _serializeRule;
    private readonly IParserSettingsProvider _parserSettingsProvider;
    
    private List<ParsingContext> _parsingContexts;
    private SheetsService _sheetsService;
    private ParsedData _parsedData;
    private string _spreadSheet;
    private bool _isInitialized;
    
    protected abstract ParserType ParserType { get; }
    
    public Parser(ISerializeRule serializeRule, IParserSettingsProvider parserSettingsProvider)
    {
        _parserSettingsProvider = parserSettingsProvider;
        _serializeRule = serializeRule;
    }
    
    public void Initialize()
    {
        if (!_parserSettingsProvider.TryGetSpreadSheet(ParserType, out var spreadSheet))
            return;
        _spreadSheet = spreadSheet;
        _sheetsService = GetSheetService();
        _isInitialized = true;
    }
    
    public void ParseData()
    {
        if (!_isInitialized)
        {
            GD.PrintErr("Parser is not initialized");
            return;
        }

        _parsingContexts = CreateParsingContexts();
        GetDataFromGoogleSheets();
        SetParsedDataInContexts();
        ParsedFromReceivedData();
        SerializeParsedData();
    }
    
    private List<ParsingContext> CreateParsingContexts()
    {
        var parsingTypesContext = ReflectionUtils.GetAllNotAbstractTypesInheritFrom<ParsingContext>();
        return ReflectionUtils.CreateObjectsByTypes<ParsingContext>(parsingTypesContext)
                              .Where(context => context.ContextType == ParserType).ToList();
    }
    
    private SheetsService GetSheetService() =>
        new SheetsService(new BaseClientService.Initializer
        {
            HttpClientInitializer = GetGoogleCredential(),
            ApplicationName = (string)ProjectSettings.GetSetting("application/config/name")
        });

    private GoogleCredential GetGoogleCredential()
    {
        var json = _spreadSheet;
        var credential = CredentialFactory.FromJson<GoogleCredential>(json);

        return credential;
    }

    private void GetDataFromGoogleSheets()
    {
        var ranges = GetRanges();
        var request = _sheetsService.Spreadsheets.Values.BatchGet(_spreadSheet);
        request.Ranges = ranges;

        var batchResponse = request.Execute();

        _parsedData = new ParsedData(batchResponse, ranges);
    }
    
    private List<string> GetRanges() => _parsingContexts.Select(r => r.ParseRange).ToList();
    
    private void SetParsedDataInContexts()
    {
        foreach (var context in _parsingContexts)
            context.SetValueRange(_parsedData);
    }

    private void ParsedFromReceivedData()
    {
        foreach (var parsingContext in _parsingContexts)
            parsingContext.ParseTarget();
    }

    private void SerializeParsedData()
    {
        _serializeRule?.SerializeObjects(_parsingContexts.Select(c => c.TargetObject).ToArray());
    }
}