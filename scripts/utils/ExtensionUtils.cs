using System;
using System.Collections.Generic;
using System.Reflection;

namespace Scripts.Utils;

public static class ExtensionUtils
{
    public static bool IsNullOrEmpty<T>(this ICollection<T> collection) => collection == null || collection.Count == 0;

    public static bool IsAttributeDefined<TAttribute>(object obj) where TAttribute : Attribute 
        => obj.GetType().GetCustomAttribute<TAttribute>() != null;
    
    public static bool IsAttributeDefined<TAttribute>(this MemberInfo memberInfo) where TAttribute : Attribute
    {
        return memberInfo.GetCustomAttribute<TAttribute>() != null;
    }

    public static bool TryGetAttribute<TAttribute>(this MemberInfo memberInfo, out TAttribute attribute) where TAttribute : Attribute
    {
        attribute = memberInfo.GetCustomAttribute<TAttribute>();
        return attribute != null;
    }

    public static TAttribute GetCustomAttribute<TAttribute>(this MemberInfo info) where TAttribute : Attribute
    {
        var results = info.GetCustomAttributes(typeof(TAttribute), false);
        return results.Length == 0 ? null : (TAttribute)results[0];
    }
}