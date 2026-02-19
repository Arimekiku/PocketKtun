using Godot;
using System.IO;

namespace Scripts.Utils.SheetParser;

public class CredentialProvider : ICredentialProvider
{
    private string JsonPath => $"{ProjectSettings.GlobalizePath("res://")}/settings/token.json";


    public string GetCredentialJson() => File.ReadAllText(JsonPath);
}