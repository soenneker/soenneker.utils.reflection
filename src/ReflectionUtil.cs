using System;
using System.Collections.Generic;
using System.Reflection;

namespace Soenneker.Utils.Reflection;

public static class ReflectionUtil
{
    public static Dictionary<string, string> GetConstantsFromType<T>()
    {
        Type type = typeof(T);
        return InternalGetConstantsFromType(type);
    }

    public static Dictionary<string, string> GetConstantsFromType(Type type)
    {
        return InternalGetConstantsFromType(type);
    }

    private static Dictionary<string, string> InternalGetConstantsFromType(Type type)
    {
        var constantsDictionary = new Dictionary<string, string>();

        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);

        foreach (FieldInfo field in fields)
        {
            if (!field.IsLiteral || field.IsInitOnly)
                continue;

            string name = field.Name;
            var value = field.GetValue(null)!.ToString()!;
            constantsDictionary[name] = value;
        }

        return constantsDictionary;
    }
}