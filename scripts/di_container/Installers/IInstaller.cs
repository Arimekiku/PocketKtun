using System.Collections.Generic;

namespace Scripts.DIContainer;

internal interface IInstaller
{
    internal IReadOnlyList<BindContext> Binds { get; }
    public void ProcessCreateBinds();
}