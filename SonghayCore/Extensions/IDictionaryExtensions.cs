﻿namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="IDictionary{TKey, TValue}"/>
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
    /// <returns></returns>
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
    /// Tries to get value with the specified key.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="dictionary">The dictionary.</param>
    /// <param name="key">The key.</param>
    public static TValue? TryGetValueWithKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) =>
        dictionary.TryGetValueWithKey(key, throwException: false);

    /// <summary>
    /// Tries to get value with the specified key.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="dictionary">The dictionary.</param>
    /// <param name="key">The key.</param>
    /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
    public static TValue? TryGetValueWithKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey? key,
        bool throwException)
    {
        ArgumentNullException.ThrowIfNull(dictionary);
        ArgumentNullException.ThrowIfNull(key);

        var test = dictionary.TryGetValue(key, out var value);

        return value switch
        {
            null when !test && throwException => throw new NullReferenceException(
                $"The expected value from key, {key}, is not here."),
            null when !test => default,
            _ => value
        };
    }
}