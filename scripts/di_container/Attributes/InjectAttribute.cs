using JetBrains.Annotations;
using System;

namespace Scripts.DIContainer;

[MeansImplicitUse]
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Constructor)]
public class InjectAttribute : Attribute
{
}