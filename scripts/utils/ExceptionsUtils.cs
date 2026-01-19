using System;

namespace Scripts.Utils;

public static class ExceptionsUtils
{
    public static void ThrowExceptionIfNull(object obj,  string message = "")
    {
        if (obj == null)
            throw new NullReferenceException($"Object is null. {message} StackTrace:\n{Environment.StackTrace}");
    }
}