using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Scripts.DIContainer;

internal static class ReflectionInjectUtils
{
    private static List<Type> _typesWithInjectAttribute;
    public static IReadOnlyList<Type> TypesWithInjectAttribute =>
        _typesWithInjectAttribute ??= GetTypesWithInjectAttribute();

    private static List<Type> GetTypesWithInjectAttribute()
    {
        var types = new List<Type>();
        var injectAssembly = typeof(InjectAttribute).Assembly;

        var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a =>
        {
            var refs = a.GetReferencedAssemblies();
            return refs.Any(r => r.Name == injectAssembly.GetName().Name) || a == injectAssembly;
        });

        foreach (var assembly in assemblies)
            types.AddRange(GetTypesFromAssemblyWhere(assembly, CheckMembersForInject));

        return types;

        bool CheckMembersForInject(Type type)
        {
            var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            return type.GetMembers(bindingFlags)
                       .Any(m => m.MemberType is MemberTypes.Field or MemberTypes.Property or MemberTypes.Method
                                 && m.GetCustomAttribute<InjectAttribute>() != null);
        }
    }

    private static List<Type> GetTypesFromAssemblyWhere(Assembly assembly, Predicate<Type> predicate) =>
        assembly.GetTypes().Where(type => predicate(type)).ToList();
}