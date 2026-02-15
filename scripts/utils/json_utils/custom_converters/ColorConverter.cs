using Godot;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Environment = System.Environment;

namespace Scripts.Utils.CustomConverters;

public class ColorConverter : JsonConverter<Color>
{
    public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException($"Error reading Color. StackTrace : {Environment.StackTrace}");

        reader.Read();
        var r =  reader.GetInt32();
        reader.Read();
        var g =  reader.GetInt32();
        reader.Read();
        var b =  reader.GetInt32();
        reader.Read();
        var a =  reader.GetInt32();
        
        reader.Read();
        
        return new Color(r, g, b, a);
    }

    public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        writer.WriteNumberValue(value.R);
        writer.WriteNumberValue(value.G);
        writer.WriteNumberValue(value.B);
        writer.WriteNumberValue(value.A);
        writer.WriteEndArray();
    }
}