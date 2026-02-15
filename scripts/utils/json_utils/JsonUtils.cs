using Scripts.Utils.CustomConverters;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.Json;

namespace Scripts.Utils;

public static class JsonUtils
{
    private static JsonSerializerOptions _serializerOptions;
    private static JsonSerializerOptions SerializerOptions => _serializerOptions ??= GetOption();
    
    public static TType Deserialize<TType>(string json) => JsonSerializer.Deserialize<TType>(json, SerializerOptions);
    
    public static string Serialize(object value) => JsonSerializer.Serialize(value, SerializerOptions);

    public static TType LoadFromFile<TType>(string filePath, string fileName)
    {
        var fullPath = Path.Combine(filePath, fileName);
        
        if (!File.Exists(fullPath))
            throw new InvalidOperationException($"Failed to load object from file '{fileName}' at path '{fullPath}'. File not found.");
        
        return Deserialize<TType>(File.ReadAllText(fullPath));
    }

    public static void SaveToFile(object value, string filePath, string fileName)
    {
        var fullPath = Path.Combine(filePath, fileName);
        Directory.CreateDirectory(filePath);
        
        File.WriteAllText(fullPath, Serialize(value));
    }
    
    public static TType LoadCompressedFile<TType>(string path, string fileName)
    {
        var fullPath = Path.Combine(path, fileName);
        
        if (!File.Exists(fullPath))
            return default;
        
        using var file = File.OpenRead(Path.Combine(path, fileName));
        using var gzip = new GZipStream(file, CompressionMode.Decompress);
        using var reader = new StreamReader(gzip, Encoding.UTF8);
        var json = reader.ReadToEnd();
        
        return Deserialize<TType>(json);
    }
    
    public static void SaveCompressedFile(object serializedObject, string filePath, string fileName)
    {
        var path = Path.Combine(filePath, fileName);
        Directory.CreateDirectory(filePath);
        
        var json = Serialize(serializedObject);
        var bytes = Encoding.UTF8.GetBytes(json);
        using var file = File.Create(path);
        using var gzip = new GZipStream(file, CompressionMode.Compress);
        
        gzip.Write(bytes, 0, bytes.Length);
    }
    
    private static JsonSerializerOptions GetOption()
    {
        var jsonOption = new JsonSerializerOptions
        {
            WriteIndented = true,
            IncludeFields = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        jsonOption.Converters.Add(new Vector2Converter());
        jsonOption.Converters.Add(new Vector3Converter());
        jsonOption.Converters.Add(new ColorConverter());
            
        return jsonOption;
    }
    
    
}