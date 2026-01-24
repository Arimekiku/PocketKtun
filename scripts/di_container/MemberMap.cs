using Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Scripts.DIContainer;

internal class MemberMap : IMemberMap
{
    private Dictionary<Type, Dictionary<MemberTypes, InjectMember[]>> _membersMap;

    public MemberMap()
    {
        CreateMemberMap();
    }
    
    public void CreateMemberMap()
    {
        var membersMap = new Dictionary<Type, Dictionary<MemberTypes, InjectMember[]>>();

        var types = ReflectionInjectUtils.TypesWithInjectAttribute;

        foreach (var type in types)
        {
            membersMap.Add(type, new Dictionary<MemberTypes, InjectMember[]>());

            var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            var injectFields = GetInjectMembers(() => type.GetFields(bindingFlags), 
                                                m => new InjectField(m));
            membersMap[type].Add(MemberTypes.Field, injectFields);
            var injectProperties = GetInjectMembers(() => type.GetProperties(bindingFlags),
                                                    m => new InjectProperty(m));
            membersMap[type].Add(MemberTypes.Property, injectProperties);
            var injectMethods = GetInjectMembers(() => type.GetMethods(bindingFlags),
                                                 m => new InjectMethod(m));
            membersMap[type].Add(MemberTypes.Method, injectMethods);
        }

        _membersMap = membersMap;
    }

    public InjectMember[] GetMembers(Type instanceType, MemberTypes memberType)
    {
        ExceptionsUtils.ThrowIfNull(_membersMap, "Member map is not created");
        
        return _membersMap[instanceType][memberType];
    }

    public bool ClassIsRegistered(Type type) => _membersMap.ContainsKey(type);
    
    private InjectMember[] GetInjectMembers<TMember>(Func<TMember[]> membersGetter,
                                                     Func<TMember, InjectMember> createMethod) where TMember : MemberInfo
    {
        var injectMembers = new List<InjectMember>();

        foreach (var memberInfo in membersGetter())
        {
            if (!memberInfo.IsAttributeDefined<InjectAttribute>())
                continue;

            injectMembers.Add(createMethod(memberInfo));
        }

        return injectMembers.ToArray();
    }
}