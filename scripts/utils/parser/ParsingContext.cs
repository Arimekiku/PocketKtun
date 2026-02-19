using Godot;
using Google.Apis.Sheets.v4.Data;
using Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Environment = System.Environment;

namespace Scripts.Utils.SheetParser;

public abstract class ParsingContext
{
    private static readonly Dictionary<string, bool> BoolMap =
        new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase)
        {
            ["true"] = true,
            ["1"] = true,
            ["yes"] = true,
            ["y"] = true,
            ["on"] = true,
            ["enabled"] = true,
            ["enable"] = true,
            ["+"] = true,

            ["false"] = false,
            ["0"] = false,
            ["no"] = false,
            ["n"] = false,
            ["off"] = false,
            ["disabled"] = false,
            ["disable"] = false,
            ["-"] = false
        };
    
    public bool IsInitialized { get; private set; }
    
    /// <summary>
    /// The format is <c>{SheetName}!{StartAddress}:{EndAddress}</c>.
    /// </summary>
    public abstract string ParseRange { get; }
    
    
    protected ParseSaver _parseSaver;
    
    private ValueRange valueRange;
    private int _rowCount = -1;
    private int _columnCount = -1;
    
    protected int RowCount
    {
        get
        {
            if (_rowCount != -1)
                return _rowCount;

            _rowCount = valueRange.Values?.Count ?? 0;
            return _rowCount;
        }
    }

    protected int ColumnCount
    {
        get
        {
            if (_columnCount != -1)
                return _columnCount;

            _columnCount = valueRange.Values?.Count > 0 ? valueRange.Values.Max(row => row?.Count ?? 0) : 0;
            return _columnCount;
        }
    }

    public ParsingContext(ParseSaver saver)
    {
        _parseSaver = saver;
    }
    
    public abstract void ParseTarget();

    public void SetValueRange(ParsedData parsedData)
    {
        if (string.IsNullOrEmpty(ParseRange))
            throw new ArgumentException($"Property ParseRange must be set or override in {GetType().Name}");
        
        if (!parsedData.ParsedValueRanges.TryGetValue(ParseRange, out var range))
            return;
            
        valueRange = range;
        IsInitialized = true;
    }
    
    protected int ParseInt(int row, int column)
    {
        ThrowIfOutOfRange(row, column);

        var stringValue = GetStringFromValueRange(row, column);
        
        if (!string.IsNullOrEmpty(stringValue))
            return NotSaveParse(stringValue, row, column, int.Parse);

        return 0;
    }

    protected string ParseString(int row, int column)
    {
        ThrowIfOutOfRange(row, column);

        var stringValue = GetStringFromValueRange(row, column);
        
        if (!string.IsNullOrEmpty(stringValue))
            return valueRange.Values[row][column].ToString();

        return "";
    }

    protected float ParseFloat(int row, int column)
    {
        ThrowIfOutOfRange(row, column);

        var stringValue = GetStringFromValueRange(row, column);
        
        if (!string.IsNullOrEmpty(stringValue))
            return NotSaveParse(stringValue, row, column, s => float.Parse(s, CultureInfo.InvariantCulture));

        return 0;
    }

    protected bool ParseBool(int row, int column)
    {
        ThrowIfOutOfRange(row, column);
        
        var stringValue = GetStringFromValueRange(row, column);
        
        if (string.IsNullOrEmpty(stringValue))
            return false;

        return BoolMap.TryGetValue(stringValue, out var boolValue) && boolValue;
    }

    protected TEnum ParseEnum<TEnum>(int row, int column) where TEnum : struct
    {
        ThrowIfOutOfRange(row, column);
        
        var stringValue = GetStringFromValueRange(row, column);
        
        if (!string.IsNullOrEmpty(stringValue))
            return NotSaveParse(stringValue, row, column, Enum.Parse<TEnum>);

        return default;
    }

    protected DateTime ParseDateTime(int row, int column)
    {
        ThrowIfOutOfRange(row, column);
        
        var stringValue = GetStringFromValueRange(row, column);
        
        if (!string.IsNullOrEmpty(stringValue))
            return NotSaveParse(stringValue, row, column, d => DateTime.Parse(d, CultureInfo.InvariantCulture));

        return default;
    }

    protected T ParseJson<T>(int row, int column)
    {
        ThrowIfOutOfRange(row, column);

        var stringValue = GetStringFromValueRange(row, column);
        
        if (!string.IsNullOrEmpty(stringValue))
            return NotSaveParse(stringValue, row, column, JsonUtils.Deserialize<T>);

        return default;
    }

    protected bool TryParseInt(int row, int column, out int result) => SaveParse(row, column, out result, ParseInt);

    protected bool TryParseString(int row, int column, out string result) => SaveParse(row, column, out result, ParseString);

    protected bool TryParseFloat(int row, int column, out float result) => SaveParse(row, column, out result, ParseFloat);

    protected bool TryParseEnum<TEnum>(int row, int column, out TEnum result) where TEnum : struct => SaveParse(row, column, out result, ParseEnum<TEnum>);

    protected bool TryParseDateTime(int row, int column, out DateTime result) => SaveParse(row, column, out result, ParseDateTime);

    protected bool TryParseJson<T>(int row, int column, out T result) => SaveParse(row, column, out result, ParseJson<T>);

    private void ThrowIfOutOfRange(int row, int column)
    {
        if (row >= RowCount)
            throw new ArgumentOutOfRangeException(nameof(row), $"Row {row} exceeds row count in parsing range {valueRange.Range}: {valueRange.Values.Count}. \nStackTrace:{Environment.StackTrace}");

        if (column >= ColumnCount)
            throw new ArgumentOutOfRangeException(nameof(column), $"Column {column} exceeds column count in parsing range {valueRange.Range}: {valueRange.Values[row].Count}. \nStackTrace:{Environment.StackTrace}");
    }

    private string GetStringFromValueRange(int row, int column)
    {
        var columnList = valueRange.Values[row];
        return columnList.Count <= column ? "" : columnList[column].ToString()?.Trim();
    }
    
    private static bool SaveParse<T>(int row, int column, out T result, Func<int, int, T> parseFunc)
    {
        try
        {
            result = parseFunc(row, column);
            return true;
        }
        catch (Exception e)
        {
            GD.PushError(e.Message);
            result = default;
            return false;
        }
    }

    private T NotSaveParse<T>(string value, int row, int column, Func<string, T> parseFunc)
    {
        try
        {
            return parseFunc(value);
        }
        catch (Exception e)
        {
            throw new ArgumentException($"Parsing error in parsing range {valueRange.Range} at row: {row + 1}, column: {column + 1}. Exception text: {e.Message}\n{e.StackTrace}");
        }
    }
}