using Godot;
using Scripts.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.DIContainer;

[GlobalClass]
internal abstract partial class GodotContextInstallers : Node, IContextInstallers
{
    [Export] private Installer[] _contextInstallers;

    protected List<Bind> _createdBinds;
    
    public abstract object ContextObject { get; }

    public void RegisterBinds(IContextProvider contextProvider)
    {
        ExceptionsUtils.ThrowIfNull(_createdBinds);

        foreach (var bind in _createdBinds)
            contextProvider.RegisterBind(bind, ContextObject);
    }

    public void InstallBinds()
    {
        _createdBinds = new List<Bind>();
        
        foreach (var installer in GetInstallers())
        {
            installer.ProcessCreateBinds();
            _createdBinds.AddRange(installer.CreatedBinds.Select(bindContext => bindContext.Bind));
        }
    }

    public IInstaller[] GetInstallers() => _contextInstallers.Select(installer => installer as IInstaller).ToArray();
}