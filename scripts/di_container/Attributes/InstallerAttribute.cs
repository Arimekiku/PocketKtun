using JetBrains.Annotations;
using System;

namespace Scripts.DIContainer;


[MeansImplicitUse(ImplicitUseTargetFlags.WithMembers)]
[AttributeUsage(AttributeTargets.Class)]
public class InstallerAttribute : Attribute
{
    public bool CreateOnInit { get; private set; }

    public InstallerAttribute(bool createOnInit = true)
    {
        CreateOnInit = createOnInit;
    }
}