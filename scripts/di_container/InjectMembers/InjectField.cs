using System;
using System.Reflection;

namespace Scripts.DIContainer;

internal class InjectField : GenericInjectMember<FieldInfo, Type>
{
    public override Type ParameterType => TargetMemberInfo.FieldType;

    public InjectField(FieldInfo fieldInfo) : base(fieldInfo)
    {
    }

    protected override void DoInject(object injectTarget, object dependency)
    {
        TargetMemberInfo.SetValue(injectTarget, dependency);
    }

    protected override bool CheckArgumentMatching(object dependency) =>
        TargetMemberInfo.FieldType == dependency.GetType();
}