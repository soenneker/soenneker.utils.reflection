using System;
using System.Collections.Generic;
using System.Reflection;

namespace Soenneker.Utils.Reflection;

public static class ReflectionUtil
{
    /// <summary>
    /// Retrieves a dictionary containing the names and values of all public constant fields
    /// defined in the specified generic type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type to extract constant fields from.</typeparam>
    /// <returns>
    /// A dictionary where the keys are the names of the constant fields and the values are their corresponding string values.
    /// </returns>
    /// <remarks>
    /// Only public, static, and literal constant fields of type <see cref="string"/> are included in the result.
    /// </remarks>
    public static Dictionary<string, string> GetConstantsFromType<T>()
    {
        return InternalGetConstantsFromType(typeof(T));
    }

    /// <summary>
    /// Retrieves a dictionary containing the names and values of all public constant fields
    /// defined in the specified type.
    /// </summary>
    /// <param name="type">The type to extract constant fields from.</param>
    /// <returns>
    /// A dictionary where the keys are the names of the constant fields and the values are their corresponding string values.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="type"/> is <c>null</c>.</exception>
    /// <remarks>
    /// Only public, static, and literal constant fields of type <see cref="string"/> are included in the result.
    /// </remarks>
    public static Dictionary<string, string> GetConstantsFromType(Type type)
    {
        return InternalGetConstantsFromType(type);
    }

    /// <summary>
    /// Retrieves a dictionary containing the names and values of all public constant fields
    /// defined in the specified type.
    /// </summary>
    /// <param name="type">The type to extract constant fields from.</param>
    /// <returns>
    /// A dictionary where the keys are the names of the constant fields and the values are their corresponding string values.
    /// </returns>
    /// <remarks>
    /// This method performs the core extraction logic. It only includes public, static, literal constants
    /// of type <see cref="string"/>. Fields that are not constants or are not of type <see cref="string"/> are ignored.
    /// </remarks>
    private static Dictionary<string, string> InternalGetConstantsFromType(Type type)
    {
        // Precompute the capacity to avoid internal resizing of the dictionary.
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);

        var estimatedCapacity = 0;

        foreach (FieldInfo field in fields)
        {
            if (field is {IsLiteral: true, IsInitOnly: false})
            {
                estimatedCapacity++;
            }
        }

        var constantsDictionary = new Dictionary<string, string>(estimatedCapacity);

        foreach (FieldInfo field in fields)
        {
            if (!field.IsLiteral || field.IsInitOnly)
                continue;

            string name = field.Name;
            object? value = field.GetValue(null);
            if (value is string stringValue)
            {
                constantsDictionary[name] = stringValue;
            }
        }

        return constantsDictionary;
    }
}