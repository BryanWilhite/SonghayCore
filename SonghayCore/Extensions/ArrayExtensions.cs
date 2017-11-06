using System;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="System.Array"/>
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Gets the next item in the specified array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">The array.</param>
        /// <param name="item">The item.</param>
        public static T Next<T>(this Array array, T item)
        {
            if (array == null) return default(T);

            var indexOfKey = Array.IndexOf(array, item);
            ++indexOfKey;
            if (indexOfKey >= array.Length) return default(T);
            return (T)array.GetValue(indexOfKey);
        }

        /// <summary>
        /// Gets the previous item in the specified array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">The array.</param>
        /// <param name="item">The item.</param>
        public static T Previous<T>(this Array array, T item)
        {
            if (array == null) return default(T);

            var indexOfKey = Array.IndexOf(array, item);
            --indexOfKey;
            if (indexOfKey < 0) return default(T);
            return (T)array.GetValue(indexOfKey);
        }
    }
}
