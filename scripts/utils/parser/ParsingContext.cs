using Google.Apis.Sheets.v4.Data;
using System;

namespace Scripts.Utils;

public abstract class ParsingContext
{
    /// <summary>
    /// The format is <c>{SheetName}!{StartAddress}:{EndAddress}</c>.
    /// </summary>
    public abstract ParserType ContextType { get; }
    public abstract string ParseRange { get; }
    public object TargetObject { get; protected set; }
    public virtual Type TargetType => TargetObject.GetType();

    protected ValueRange ValueRange { get; private set; }
    
    private int _rowCount = -1;
    private int _columnCount = -1;
    
    protected int RowCount
    {
        get
        {
            if (_rowCount != -1)
                return _rowCount;

            _rowCount = ValueRange.GetRowCount();
            return _rowCount;
        }
    }

    protected int ColumnCount
    {
        get
        {
            if (_columnCount != -1)
                return _columnCount;

            _columnCount = ValueRange.GetColumnCount();
            return _columnCount;
        }
    }

    public void SetValueRange(ParsedData parsedData)
    {
        if (string.IsNullOrEmpty(ParseRange))
            throw new ArgumentException($"Property ParseRange must be set or override in {GetType().Name}");

        ValueRange = parsedData.ParsedValueRanges[ParseRange];
    }

    public abstract void ParseTarget();
}