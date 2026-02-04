using Scripts.DIContainer;
using Scripts.Services;
using System;
using System.Collections.Generic;

namespace Scripts.Utils;

public class ObjectProviderRegistry : IObjectProviderRegistry
{
    private readonly Dictionary<string, IObjectProvider> _objectProviders = new Dictionary<string, IObjectProvider>();
    
    private ILogger _logger;
    
    [Inject]
    private void Constructor(ILogger logger)
    {
        _logger = logger;
    }

    public IObjectProvider GetObjectProvider(string objectProviderId)
    {
        _objectProviders.TryGetValue(objectProviderId, out var objectProvider);
        
        ExceptionsUtils.ThrowIfNull(objectProvider, $"ObjectProvider {objectProviderId} is not registered.");
        
        return objectProvider;
    }
    
    public bool TryGetObjectProvider(string objectName, out IObjectProvider objectProvider) => 
        _objectProviders.TryGetValue(objectName, out objectProvider);

    public void RegisterObjectProvider(IObjectProvider provider)
    {
        if (_objectProviders.TryAdd(provider.ProviderId, provider)) 
            return;
        
        _logger.LogWarning($"Object provider {provider.ProviderId} already registered.");
    }

    public void UnregisterObjectProvider(IObjectProvider provider) => _objectProviders.Remove(provider.ProviderId);
}