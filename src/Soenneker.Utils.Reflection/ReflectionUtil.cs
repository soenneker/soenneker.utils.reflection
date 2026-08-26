using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Reflection;

namespace Soenneker.Utils.Reflection;

/// <summary>
/// Represents the reflection util.
/// </summary>
public static class ReflectionUtil
{
    private static readonly ConcurrentDictionary<Type, KeyValuePair<string, string>[]> _constantCache = new();

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
        ArgumentNullException.ThrowIfNull(type);

        KeyValuePair<string, string>[] constants = _constantCache.GetOrAdd(type, static t =>
        {
            FieldInfo[] fields = t.GetFields(BindingFlags.Public | BindingFlags.Static);
            var values = new List<KeyValuePair<string, string>>(fields.Length);

            foreach (FieldInfo field in fields)
            {
                if (!field.IsLiteral || field.IsInitOnly || field.GetValue(null) is not string value)
                    continue;

                values.Add(new KeyValuePair<string, string>(field.Name, value));
            }

            return values.ToArray();
        });

        var constantsDictionary = new Dictionary<string, string>(constants.Length);
        for (var i = 0; i < constants.Length; i++)
            constantsDictionary.Add(constants[i].Key, constants[i].Value);

        return constantsDictionary;
    }
}
