using System;
using System.Reflection;

namespace Scripts.DIContainer;

internal interface IMemberMap
{
    public void CreateMemberMap();
    public InjectMember[] GetMembers(Type instanceType, MemberTypes  memberType);
    public bool ClassIsRegistered(Type type);
}