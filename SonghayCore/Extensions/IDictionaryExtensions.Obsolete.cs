using System;
using System.Collections;
using System.Linq;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="System.Collections.IDictionary"/>
    /// </summary>
    public static partial class IDictionaryExtensions
    {
        /// <summary>
        /// Converts the <see cref="IDictionary"/> into a string or null.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        [Obsolete("Use Dictionary<TKey, TValue>.TryGetValue() instead whiich emerged in .NET 3.5 (see https://stackoverflow.com/a/9382740/22944).")]
        public static string ToStringOrNull(this IDictionary dictionary, string key)
        {
            if (dictionary == null) return null;
            if (!dictionary.Keys.OfType<string>().Contains(key)) return null;
            return dictionary[key].ToString();
        }

        /// <summary>
        /// Converts the <see cref="IDictionary"/> into a value or default.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        [Obsolete("Use Dictionary<TKey, TValue>.TryGetValue() instead whiich emerged in .NET 3.5 (see https://stackoverflow.com/a/9382740/22944).")]
        public static TValue ToValueOrDefault<TValue>(this IDictionary dictionary, string key)
        {
            if (dictionary == null) return default(TValue);
            if (!dictionary.Keys.OfType<string>().Contains(key)) return default(TValue);
            return (TValue)dictionary[key];
        }
    }
}
