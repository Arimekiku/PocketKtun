using System;
using System.Reflection;

namespace Scripts.DIContainer;

internal abstract class InjectMember
{
    protected readonly MemberInfo _memberInfo;

    protected InjectMember(MemberInfo memberInfo)
    {
        _memberInfo = memberInfo;
    }

    public void Inject(object injectTarget, object dependency)
    {
        if (CheckArgumentMatching(dependency))
            throw new
                ArgumentException($"Dependency type {dependency.GetType().Name} is not matching width needed in member {_memberInfo.Name}");

        DoInject(injectTarget, dependency);
    }

    protected abstract void DoInject(object injectTarget, object dependency);

    protected abstract bool CheckArgumentMatching(object dependency);
}