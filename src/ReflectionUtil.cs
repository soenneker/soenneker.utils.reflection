using System;
using System.Collections.Generic;
using System.Reflection;
using Soenneker.Utils.Reflection.Abstract;

namespace Soenneker.Utils.Reflection;

///<inheritdoc cref="IReflectionUtil"/>
public class ReflectionUtil : IReflectionUtil
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

    private static Dictionary<string, string> InternalGetConstantsFromType(IReflect type)
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