using System;
using System.Reflection;

namespace Scripts.DIContainer;

internal class InjectMethod : GenericInjectMember<MethodInfo, ParameterInfo[]>
{
    public override ParameterInfo[] ParameterType => TargetMemberInfo.GetParameters();

    public InjectMethod(MethodInfo methodInfo) : base(methodInfo)
    {
    }

    protected override void DoInject(object injectTarget, object dependency)
    {
        var arguments = dependency as object[];

        TargetMemberInfo.Invoke(injectTarget, arguments);
    }

    protected override bool CheckArgumentMatching(object dependency)
    {
        var methodParameters = TargetMemberInfo.GetParameters();

        if (dependency is not object[] objects)
            throw new ArgumentException("Inject method can inject oly array objects");

        for (var i = 0; i < methodParameters.Length; ++i)
        {
            if (methodParameters[i].ParameterType != objects[i].GetType())
                throw new
                    ArgumentException($"Dependency type {objects[i].GetType().Name} dont match width method parameter {methodParameters[i].ParameterType.Name}");
        }

        return true;
    }
}