using System.Collections.Generic;

namespace Scripts.DIContainer;

internal interface IContextInstallers
{
    public object ContextObject { get; }
    
    public void InstallBinds();
    public void RegisterBinds(IContextProvider contextProvider);
    public IInstaller[] GetInstallers();
}