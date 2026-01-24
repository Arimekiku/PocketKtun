using System;

namespace Scripts.Utils;

public static class ExceptionsUtils
{
    private static string StackTrace => Environment.StackTrace;
    
    public static void ThrowIfNull(object obj,  string message = "")
    {
        if (obj == null)
            throw new NullReferenceException($"Object is null. {message} StackTrace:\n{StackTrace}");
    }

    public static void ThrowIfNotEquals(object objA, object objB, string message = "")
    {
        if (!Equals(objA, objB))
            throw new ArgumentException($"Objects is not equals. {message} StackTrace:\n{StackTrace}");
    }
}