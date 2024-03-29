﻿namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="Array"/>.
/// </summary>
public static class ArrayExtensions
{
    /// <summary>
    /// Gets the next item in the specified <see cref="Array"/>.
    /// </summary>
    /// <typeparam name="T">The array <see cref="Type"/>.</typeparam>
    /// <param name="array">The array.</param>
    /// <param name="item">The item.</param>
    public static T? Next<T>(this Array? array, T? item)
    {
        if (array == null) return default;

        var indexOfKey = Array.IndexOf(array, item);

        ++indexOfKey;

        if (indexOfKey >= array.Length) return default(T);

        return (T) array.GetValue(indexOfKey)!;
    }

    /// <summary>
    /// Gets the previous item in the specified <see cref="Array"/>.
    /// </summary>
    /// <typeparam name="T">The array <see cref="Type"/>.</typeparam>
    /// <param name="array">The array.</param>
    /// <param name="item">The item.</param>
    public static T? Previous<T>(this Array? array, T? item)
    {
        if (array == null) return default;

        var indexOfKey = Array.IndexOf(array, item);

        --indexOfKey;

        if (indexOfKey < 0) return default(T);

        return (T) array.GetValue(indexOfKey)!;
    }
}
