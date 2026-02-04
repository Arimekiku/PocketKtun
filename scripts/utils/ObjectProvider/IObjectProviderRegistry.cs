namespace Scripts.Utils;

public interface IObjectProviderRegistry
{
    public IObjectProvider GetObjectProvider(string objectProviderId);
    public bool TryGetObjectProvider(string objectName, out IObjectProvider objectProvider);
    public void RegisterObjectProvider(IObjectProvider provider);
    public void UnregisterObjectProvider(IObjectProvider provider);
}