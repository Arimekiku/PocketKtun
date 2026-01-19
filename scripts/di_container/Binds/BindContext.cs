using System;

namespace Scripts.DIContainer;

public abstract class BindContext
{
    protected bool _isReady;
    protected Bind _bind;

    public Bind Bind => _bind;
    
    public void AsSingle()
    {
        _bind.BindInstanceCreator.AsSingle();
        _isReady = true;
    }

    public void AsTransient()
    {
        _bind.BindInstanceCreator.AsTransient();
        _isReady = true;
    }
    
    public Bind Build()
    {
        if (_bind == null || !_isReady)
            throw new Exception("Bind nor ready or is null");

        return _bind;
    }
}