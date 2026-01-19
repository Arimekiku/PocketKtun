using System;

namespace Scripts.DIContainer;

public class BindInstanceCreator
{
    public object SingleInstance;
    public Func<object> ConstructMethod;
    public LiveScope LiveScope;
    
    public void AsSingle() => LiveScope = LiveScope.AsSingle;

    public void AsTransient() => LiveScope = LiveScope.AsTransient;
}