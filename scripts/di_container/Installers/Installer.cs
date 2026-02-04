using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.DIContainer;

[GlobalClass]
public abstract partial class Installer: Node, IInstaller
{
    private readonly List<BindContext> _bindsContexts = new List<BindContext>();

    public IReadOnlyList<BindContext> Binds => _bindsContexts;

    public abstract void ProcessCreateBinds();

    protected GenericBindContext<TContract, TRealization> CreateBind<TContract, TRealization>() where TContract : class
        where TRealization : TContract
    {
        var bindContext = new GenericBindContext<TContract, TRealization>();
        _bindsContexts.Add(bindContext);
        return bindContext;
    }
}