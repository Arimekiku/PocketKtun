using System.Reflection;

namespace Scripts.DIContainer;

internal abstract class GenericInjectMember<T, Y> : InjectMember where T : MemberInfo
{
    protected GenericInjectMember(T memberInfo) : base(memberInfo)
    {
    }

    protected T TargetMemberInfo => _memberInfo as T;

    public abstract Y ParameterType { get; }
}