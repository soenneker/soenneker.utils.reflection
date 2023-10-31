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
        var constantsDictionary = new Dictionary<string, string>();

        Type type = typeof(T);
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);

        foreach (FieldInfo field in fields)
        {
            if (!field.IsLiteral || field.IsInitOnly) 
                continue;
            
            string name = field.Name;
            string value = field.GetValue(null)!.ToString()!;
            constantsDictionary[name] = value;
        }

        return constantsDictionary;
    }
}