namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="IDictionary{TKey, TValue}"/>.
/// </summary>
// ReSharper disable once InconsistentNaming
public static class IDictionaryExtensions
{
    /// <summary>
    /// Converts the <see cref="IDictionary{TKey, TValue}"/>
    /// to the <see cref="NameValueCollection"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="dictionary">The set.</param>
    /// <remarks>
    /// For detail, see https://stackoverflow.com/a/7230446/22944
    /// </remarks>
    public static NameValueCollection ToNameValueCollection<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
    {
        var nameValueCollection = new NameValueCollection();

        foreach (var kvp in dictionary)
        {
            string? value = null;
            if (kvp.Value != null)
                value = kvp.Value.ToString();

            nameValueCollection.Add(kvp.Key!.ToString(), value);
        }

        return nameValueCollection;
    }

    /// <summary>
    /// Converts the <see cref="IDictionary{TKey, TValue}"/>
    /// to a shallow clone.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="dictionary">the <see cref="IDictionary{TKey, TValue}"/></param>
    /// <remarks>
    /// For more detail see “Clone a Dictionary in C#”
    /// [https://www.techiedelight.com/clone-a-dictionary-in-csharp/]
    /// </remarks>
    public static IDictionary<TKey, TValue> ToShallowClone<TKey, TValue>(this IDictionary<TKey, TValue>? dictionary) where TKey: notnull
    {
        ArgumentNullException.ThrowIfNull(dictionary);
 
        return dictionary.ToDictionary(i => i.Key, j => j.Value);
    }

    /// <summary>
    /// Tries to get value with the specified key.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="dictionary">The dictionary.</param>
    /// <param name="key">The key.</param>
    public static TValue? TryGetValueWithKey<TKey, TValue>(this IDictionary<TKey, TValue>? dictionary, TKey key) =>
        dictionary.TryGetValueWithKey(key, throwException: false);

    /// <summary>
    /// Tries to get value with the specified key.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="dictionary">The dictionary.</param>
    /// <param name="key">The key.</param>
    /// <param name="throwException">When <c>true</c>, throw an exception when retrieval fails.</param>
    public static TValue? TryGetValueWithKey<TKey, TValue>(this IDictionary<TKey, TValue>? dictionary,
        TKey? key, bool throwException)
    {
        if (throwException)
        {
            ArgumentNullException.ThrowIfNull(dictionary);
            ArgumentNullException.ThrowIfNull(key);
        }
        else
        {
            if (dictionary == null) return default;
            if (key == null) return default;
        }

        bool test = dictionary.TryGetValue(key, out var value);

        return value switch
        {
            _ when !test && throwException =>
                throw new NullReferenceException($"The expected value from key, {key}, is not here."),
            _ when !test && !throwException => default,
            _ => value
        };
    }

    /// <summary>
    /// Invokes the <see cref="Enumerable.Union{TSource}(System.Collections.Generic.IEnumerable{TSource},System.Collections.Generic.IEnumerable{TSource})"/> method
    /// with the specified dictionaries and calls <see cref="Enumerable.ToDictionary{TKey, TValue}(System.Collections.Generic.IEnumerable{KeyValuePair{TKey, TValue}})"/>
    /// on the result.
    /// </summary>
    /// <param name="dictionary1"></param>
    /// <param name="dictionary2"></param>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <remarks>
    /// When <see cref="Enumerable.Union{TSource}(System.Collections.Generic.IEnumerable{TSource},System.Collections.Generic.IEnumerable{TSource})"/> is called
    /// .NET returns a collection of <see cref="KeyValuePair{TKey, TValue}"/> to avoid losing dictionary entries with the same key.
    /// This member disregards this danger and ensures a dictionary is returned.
    /// </remarks>
    public static IDictionary<TKey, TValue?>? UnionAsDictionary<TKey, TValue>(this IDictionary<TKey, TValue?>? dictionary1, IDictionary<TKey, TValue?>? dictionary2) where TKey : notnull
    {
        if (dictionary1 == null || dictionary2 == null) return dictionary1;

        return dictionary1
            .Union(dictionary2)
            .DistinctBy(kvp => kvp.Key)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    /// <summary>
    /// Returns the <see cref="IDictionary{TKey, TValue}"/>
    /// with the specified pair.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="dictionary">the <see cref="IDictionary{TKey, TValue}"/></param>
    /// <param name="key">the key</param>
    /// <param name="value">the value</param>
    public static IDictionary<TKey, TValue?>? WithPair<TKey, TValue>(this IDictionary<TKey, TValue?>? dictionary, TKey? key, TValue? value) where TKey : notnull
    {
        dictionary?[key!] = value;
 
        return dictionary;
    }

    /// <summary>
    /// Returns the <see cref="IDictionary{TKey, TValue}"/>
    /// with the specified pair.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="dictionary">the <see cref="IDictionary{TKey, TValue}"/></param>
    /// <param name="pair">the <see cref="KeyValuePair{TKey, TValue}"/></param>
    public static IDictionary<TKey, TValue?>? WithPair<TKey, TValue>(this IDictionary<TKey, TValue?>? dictionary,
        KeyValuePair<TKey, TValue?>? pair) where TKey : notnull =>
        pair.HasValue ? dictionary.WithPair(pair.Value.Key, pair.Value.Value) : dictionary;

    /// <summary>
    /// Returns the <see cref="IDictionary{TKey, TValue}" />
    /// with the specified pair.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="dictionary">the <see cref="IDictionary{TKey, TValue}" /></param>
    /// <param name="key">the key</param>
    /// <param name="value">the value</param>
    /// <param name="condition">The condition.</param>
    public static IDictionary<TKey, TValue?>? WithPair<TKey, TValue>(this IDictionary<TKey, TValue?>? dictionary, TKey? key, TValue? value, Func<bool>? condition) where TKey : notnull
    {
        bool isConditionMet = condition?.Invoke() ?? true;

        if (isConditionMet) dictionary.WithPair(key, value);

        return dictionary;
    }

    /// <summary>
    /// Returns the <see cref="IDictionary{TKey, TValue}"/>
    /// with the specified pairs.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="dictionary">the <see cref="IDictionary{TKey, TValue}"/></param>
    /// <param name="pairs">The pairs to add.</param>
    public static IDictionary<TKey, TValue?>? WithPairs<TKey, TValue>(this IDictionary<TKey, TValue?>? dictionary, IEnumerable<KeyValuePair<TKey, TValue?>>? pairs) where TKey : notnull
    {
        if (pairs == null) return dictionary;
 
        foreach (var pair in pairs) dictionary.WithPair(pair);
 
        return dictionary;
    }
}
