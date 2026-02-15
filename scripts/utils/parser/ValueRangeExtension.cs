using Godot;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Environment = System.Environment;

namespace Scripts.Utils;

public static class ValueRangeExtension
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

    public static int GetRowCount(this ValueRange range) => range.Values?.Count ?? 0;

    public static int GetColumnCount(this ValueRange range) => range.Values?.Count > 0
        ? range.Values.Max(row => row?.Count ?? 0)
        : 0;

    public static int ParseInt(this ValueRange valueRange, int row, int column)
    {
        ThrowIfOutOfRange(valueRange, row, column);

        if (valueRange.Values.Count > row && valueRange.Values[row].Count > column)
            return NotSaveParse(valueRange, row, column, int.Parse);

        GD.PushWarning($"Value in parsing range {valueRange.Range} at row: {row + 1}, column: {column + 1} are empty. Returning default value.");
        return 0;
    }

    public static string ParseString(this ValueRange valueRange, int row, int column)
    {
        ThrowIfOutOfRange(valueRange, row, column);

        if (valueRange.Values.Count > row && valueRange.Values[row].Count > column)
            return valueRange.Values[row][column].ToString();

        GD.PushWarning($"Value in parsing range {valueRange.Range} at row: {row + 1}, column: {column + 1} are empty. Returning default value.");
        return "";
    }

    public static float ParseFloat(this ValueRange valueRange, int row, int column)
    {
        ThrowIfOutOfRange(valueRange, row, column);

        if (valueRange.Values.Count > row && valueRange.Values[row].Count > column)
            return NotSaveParse(valueRange, row, column, s => float.Parse(s, CultureInfo.InvariantCulture));

        GD.PushWarning($"Value in parsing range {valueRange.Range} at row: {row + 1}, column: {column + 1} are empty. Returning default value.");
        return 0;
    }

    public static bool ParseBool(this ValueRange valueRange, int row, int column)
    {
        ThrowIfOutOfRange(valueRange, row, column);

        if (valueRange.Values.Count <= row || valueRange.Values[row].Count <= column)
        {
            GD.PushWarning($"Value in parsing range {valueRange.Range} at row: {row + 1}, column: {column + 1} are empty. Returning default value.");
            return false;
        }

        var value = valueRange.Values[row][column].ToString()?.Trim();

        if (string.IsNullOrEmpty(value))
            return false;

        return BoolMap.TryGetValue(value, out var boolValue) && boolValue;
    }

    public static TEnum ParseEnum<TEnum>(this ValueRange valueRange, int row, int column) where TEnum : struct
    {
        ThrowIfOutOfRange(valueRange, row, column);

        if (valueRange.Values.Count > row && valueRange.Values[row].Count > column)
            return NotSaveParse(valueRange, row, column, Enum.Parse<TEnum>);

        GD.PushWarning($"Value in parsing range {valueRange.Range} at row: {row + 1}, column: {column + 1} are empty. Returning default value.");
        return default;
    }

    public static DateTime ParseDateTime(this ValueRange valueRange, int row, int column)
    {
        ThrowIfOutOfRange(valueRange, row, column);

        if (valueRange.Values.Count > row && valueRange.Values[row].Count > column)
            return NotSaveParse(valueRange, row, column, d => DateTime.Parse(d, CultureInfo.InvariantCulture));

        GD.PushWarning($"Value in parsing range {valueRange.Range} at row: {row + 1}, column: {column + 1} are empty. Returning default value.");
        return default;
    }

    public static T ParseJson<T>(this ValueRange valueRange, int row, int column)
    {
        ThrowIfOutOfRange(valueRange, row, column);

        if (valueRange.Values.Count > row && valueRange.Values[row].Count > column)
            return NotSaveParse(valueRange, row, column, JsonUtils.Deserialize<T>);

        GD.PushWarning($"Value in parsing range {valueRange.Range} at row: {row + 1}, column: {column + 1} are empty. Returning default value.");

        return default;
    }

    public static bool TryParseInt(this ValueRange valueRange, int row, int column, out int result)
    {
        return SaveParse(valueRange, row, column, out result, ParseInt);
    }

    public static bool TryParseString(this ValueRange valueRange, int row, int column, out string result)
    {
        return SaveParse(valueRange, row, column, out result, ParseString);
    }

    public static bool TryParseFloat(this ValueRange valueRange, int row, int column, out float result)
    {
        return SaveParse(valueRange, row, column, out result, ParseFloat);
    }

    public static bool TryParseEnum<TEnum>(this ValueRange valueRange, int row, int column, out TEnum result)
        where TEnum : struct
    {
        return SaveParse(valueRange, row, column, out result, ParseEnum<TEnum>);
    }

    public static bool TryParseDateTime(this ValueRange valueRange, int row, int column, out DateTime result)
    {
        return SaveParse(valueRange, row, column, out result, ParseDateTime);
    }

    public static bool TryParseJson<T>(this ValueRange valueRange, int row, int column, out T result)
    {
        return SaveParse(valueRange, row, column, out result, ParseJson<T>);
    }

    private static bool SaveParse<T>(ValueRange valueRange, int row, int column, out T result,
                                     Func<ValueRange, int, int, T> parseFunc)
    {
        try
        {
            result = parseFunc(valueRange, row, column);
            return true;
        }
        catch (Exception e)
        {
            GD.PushError(e.Message);
            result = default;
            return false;
        }
    }

    private static T NotSaveParse<T>(ValueRange valueRange, int row, int column, Func<string, T> parseFunc)
    {
        try
        {
            var stringValue = valueRange.Values[row][column].ToString()?.Trim();
            return parseFunc(stringValue);
        }
        catch (Exception e)
        {
            throw new
                ArgumentException($"Parsing error in parsing range {valueRange.Range} at row: {row + 1}, column: {column + 1}. Exception text: {e.Message}\n{e.StackTrace}");
        }
    }

    private static void ThrowIfOutOfRange(ValueRange valueRange, int row, int column)
    {
        if (row >= valueRange.GetRowCount())
            throw new ArgumentOutOfRangeException(nameof(row),
                                                  $"Row {row} exceeds row count in parsing range {valueRange.Range}: {valueRange.Values.Count}. \nStackTrace:{Environment.StackTrace}");

        if (column >= valueRange.GetColumnCount())
            throw new ArgumentOutOfRangeException(nameof(column),
                                                  $"Column {column} exceeds column count in parsing range {valueRange.Range}: {valueRange.Values[row].Count}. \nStackTrace:{Environment.StackTrace}");
    }

    public static void Test(this ParsingContext parsingContext, int row, int column)
    {
    }
}