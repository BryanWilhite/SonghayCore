﻿namespace Songhay.Extensions;

/// <summary>
/// Extension of <see cref="char"/>.
/// </summary>
public static class CharExtensions
{
    /// <summary>
    /// Converts an enumeration of <see cref="char"/>
    /// to <see cref="string"/>.
    /// </summary>
    /// <param name="chars">The <see cref="IEnumerable{T}"/> of <see cref="char"/>.</param>
    public static string? FromCharsToString(this IEnumerable<char>? chars)
    {
        if (chars == null) return null;

        var charArray = chars as char[] ?? chars.ToArray();

        return !charArray.Any() ? string.Empty : new string(charArray);
    }
}
