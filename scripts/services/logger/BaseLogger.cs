using Godot;

namespace Scripts.Services;

public class BaseLogger : ILogger
{
    public void Log(string message)
    {
        GD.Print(message);
    }

    public void LogWarning(string message)
    {
        GD.PrintRich($"[color=yellow]{message}[/color]");
    }

    public void LogError(string message)
    {
        GD.PrintErr(message);
    }
}