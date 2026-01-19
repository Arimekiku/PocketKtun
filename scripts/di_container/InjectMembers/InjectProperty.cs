using System;
using System.Reflection;

namespace Scripts.DIContainer;

internal class InjectProperty : GenericInjectMember<PropertyInfo, Type>
{
    public override Type ParameterType => TargetMemberInfo.PropertyType;

    public InjectProperty(PropertyInfo propertyInfo) : base(propertyInfo)
    {
    }

    protected override void DoInject(object injectTarget, object dependency)
    {
        TargetMemberInfo.SetValue(injectTarget, dependency);
    }

    protected override bool CheckArgumentMatching(object dependency)
    {
        if (!TargetMemberInfo.CanWrite)
            throw new ArgumentException("Property cannot be written");

        return TargetMemberInfo.PropertyType == dependency.GetType();
    }
}