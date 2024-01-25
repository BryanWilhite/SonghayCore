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
        ArgumentNullException.ThrowIfNull(dictionary);
        ArgumentNullException.ThrowIfNull(key);

        var test = dictionary.TryGetValue(key, out var value);

        return value switch
        {
            _ when !test && throwException => throw new NullReferenceException(
                $"The expected value from key, {key}, is not here."),
            _ => value
        };
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
    public static IDictionary<TKey, TValue?> WithPair<TKey, TValue>(this IDictionary<TKey, TValue?>? dictionary, TKey? key, TValue? value) where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(dictionary);
 
        dictionary[key!] = value;
 
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
    public static IDictionary<TKey, TValue?> WithPair<TKey, TValue>(this IDictionary<TKey, TValue?>? dictionary,
        KeyValuePair<TKey, TValue?>? pair) where TKey : notnull =>
        pair.HasValue ?
        dictionary.WithPair(pair.Value.Key, pair.Value.Value)
        :
        new Dictionary<TKey, TValue?>();

 
    /// <summary>
    /// Returns the <see cref="IDictionary{TKey, TValue}"/>
    /// with the specified pairs.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="dictionary">the <see cref="IDictionary{TKey, TValue}"/></param>
    /// <param name="pairs">The pairs to add.</param>
    public static IDictionary<TKey, TValue?> WithPairs<TKey, TValue>(this IDictionary<TKey, TValue?>? dictionary, IEnumerable<KeyValuePair<TKey, TValue?>>? pairs) where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(dictionary);
 
        if (pairs == null) return dictionary;
 
        foreach (var pair in pairs)
        {
            dictionary[pair.Key] = pair.Value;
        }
 
        return dictionary;
    }
}
