using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="IDictionary{TKey, TValue}"/>
    /// </summary>
    public static partial class IDictionaryExtensions
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
                string value = null;
                if (kvp.Value != null)
                    value = kvp.Value.ToString();

                nameValueCollection.Add(kvp.Key.ToString(), value);
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
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">The expected dictionary is not here.</exception>
        /// <exception cref="System.NullReferenceException"></exception>
        public static TValue TryGetValueWithKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary.TryGetValueWithKey(key, throwException: false);
        }

        /// <summary>
        /// Tries to get value with the specified key.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">The expected dictionary is not here.</exception>
        /// <exception cref="System.NullReferenceException"></exception>
        public static TValue TryGetValueWithKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, bool throwException)
        {
            if (dictionary == null) throw new ArgumentNullException("The expected dictionary is not here.");
            if (!dictionary.TryGetValue(key, out var value))
            {
                if (throwException) throw new NullReferenceException($"The expected value from key, {key}, is not here.");
                else return default(TValue);

            }
            return value;
        }
    }
}