using Godot;

namespace Scripts.Utils;

public interface IObjectProvider
{
    public string ProviderId { get; }
    
    public Node GetObject(string objectName);
    public void ReturnObject(Node node);
}