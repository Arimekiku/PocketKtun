using System;
using System.Linq.Expressions;

namespace Scripts.Utils;

public static class EnumConverter<TEnum> where TEnum : Enum
{
    public static Func<int, TEnum> ToEnum = CreateIntToEnumConverter(); 
    public static Func<TEnum, int> ToInt = CreateEnumToIntConverter();

    private static Func<int, TEnum> CreateIntToEnumConverter()
    {
        var parameter = Expression.Parameter(typeof(int));
        var cast = Expression.Convert(parameter, typeof(TEnum));
        
        return Expression.Lambda<Func<int, TEnum>>(cast, parameter).Compile();
    }
    
    private static Func<TEnum, int> CreateEnumToIntConverter()
    {
        var parameter = Expression.Parameter(typeof(TEnum));
        var cast = Expression.Convert(parameter, typeof(int));
        
        return Expression.Lambda<Func<TEnum, int>>(cast, parameter).Compile();
    }
}