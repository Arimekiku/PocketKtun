namespace Scripts.Services;

public interface ILogger
{
    public void Log(string message);
    public void LogWarning(string message);
    public void LogError(string message);
}