using Google.Apis.Sheets.v4.Data;
using System.Collections.Generic;

namespace Scripts.Utils.SheetParser;

public class ParsedData
{
    private readonly Dictionary<string, ValueRange> _parsedValueRanges;

    public IReadOnlyDictionary<string, ValueRange> ParsedValueRanges => _parsedValueRanges;

    public ParsedData(BatchGetValuesResponse valuesResponse, List<string> ranges)
    {
        _parsedValueRanges = CreateDataDictionary(valuesResponse, ranges);
    }

    private Dictionary<string, ValueRange> CreateDataDictionary(BatchGetValuesResponse valuesResponse,
                                                                IList<string> ranges)
    {
        var parsedValueRanges = new Dictionary<string, ValueRange>(valuesResponse.ValueRanges.Count);

        for (var i = 0; i < valuesResponse.ValueRanges.Count; ++i)
            parsedValueRanges.Add(ranges[i], valuesResponse.ValueRanges[i]);

        return parsedValueRanges;
    }
}