using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Scripts.Utils;

public static class ReflectionUtils
{
    public static Type[] GetAllNotAbstractTypesInheritFrom<T>() => GetAllNotAbstractTypesInheritFrom(typeof(T));

    public static Type[] GetAllNotAbstractTypesInheritFromWhere<T>(Predicate<Type> predicate) =>
        GetAllNotAbstractTypesInheritFromWhere(typeof(T), predicate);

    public static Type[] GetAllNotAbstractTypesInheritFrom(Type type) =>
        GetAllNotAbstractTypesInheritFromWhere(type, _ => true);

    public static Type[] GetAllNotAbstractTypesInheritFromWhere(Type type, Predicate<Type> where)
    {
        var baseAssembly = type.Assembly;

        var assembles = AppDomain.CurrentDomain.GetAssemblies().Where(a =>
        {
            var refs = a.GetReferencedAssemblies();
            return refs.Any(assembly => assembly.Name == baseAssembly.GetName().Name) || a == baseAssembly;
        });

        var types = new List<Type>();

        foreach (var assembly in assembles)
        {
            Type[] assemblyTypes;

            try
            {
                assemblyTypes = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                assemblyTypes = ex.Types.Where(t => t != null).ToArray();
            }

            types.AddRange(assemblyTypes.Where(t => type.IsAssignableFrom(t) && t != type && !t.IsAbstract &&
                                                    where(t)));
        }

        return types.ToArray();
    }

    public static object[] CreateObjectsByTypes(IEnumerable<Type> types, params object[] constructParams)
    {
        var createdObjects = new List<object>();

        foreach (var type in types)
        {
            if (type.IsAbstract)
                continue;

            var obj = Activator.CreateInstance(type, constructParams);

            if (obj == null)
                throw new InvalidOperationException($"Cannot create instance of type {type.Name}");

            createdObjects.Add(Activator.CreateInstance(type, constructParams));
        }

        return createdObjects.ToArray();
    }

    public static T[] CreateObjectsByTypes<T>(IEnumerable<Type> types, params object[] constructParams)
        where T : class
    {
        var createdObjects = new List<T>();
        var createdType = typeof(T);

        foreach (var type in types)
        {
            if (!createdType.IsAssignableFrom(type))
                throw new ArgumentException($"Type {type.Name} must inherit from {createdType.Name}");

            if (type.IsAbstract)
                continue;

            var obj = Activator.CreateInstance(type, constructParams);

            if (obj is not T tObj)
                throw new
                    InvalidOperationException($"Cannot create instance of type {type.Name} as {createdType.Name}");

            createdObjects.Add(tObj);
        }

        return createdObjects.ToArray();
    }
}