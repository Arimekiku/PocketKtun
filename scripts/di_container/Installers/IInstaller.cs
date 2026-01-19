using System.Collections.Generic;

namespace Scripts.DIContainer;

internal interface IInstaller
{
    internal IReadOnlyList<BindContext> CreatedBinds { get; }
    public void ProcessCreateBinds();
}