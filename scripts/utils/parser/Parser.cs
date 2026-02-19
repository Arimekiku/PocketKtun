using Godot;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Scripts.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.Utils.SheetParser;

public abstract class Parser
{
    private readonly ParseSaver _saver;
    private ICredentialProvider _credentialProvider;
    
    private List<ParsingContext> _parsingContexts;
    private SheetsService _sheetsService;
    private ParsedData _parsedData;
    private string _spreadSheet;
    private bool _isInitialized;
    
    public Parser(ParseSaver saver, ICredentialProvider credentialProvider)
    {
        _saver = saver;
        _saver.Initialize();
        _credentialProvider = credentialProvider;
    }
    
    public void Initialize()
    {
        if (!TryGetSpreadSheet(out var spreadSheet))
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
        SaveParsedData();
    }
    
    protected abstract bool TryGetSpreadSheet(out string spreadSheet);
    
    private List<ParsingContext> CreateParsingContexts()
    {
        var parsingContextTypes = ReflectionUtils.GetAllNotAbstractTypesInheritFrom<ParsingContext>();
        return ReflectionUtils.CreateObjectsByTypes<ParsingContext>(parsingContextTypes, _saver);
    }
    
    private SheetsService GetSheetService() =>
        new SheetsService(new BaseClientService.Initializer
        {
            HttpClientInitializer = GetGoogleCredential(),
            ApplicationName = (string)ProjectSettings.GetSetting("application/config/name")
        });

    private GoogleCredential GetGoogleCredential()
    {
        var json = _credentialProvider.GetCredentialJson();
        var credential = GoogleCredential.FromJson(json);

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
        foreach (var parsingContext in _parsingContexts.Where(context => context.IsInitialized))
            parsingContext.ParseTarget();
    }

    private void SaveParsedData()
    {
        _saver?.Save();
    }
}