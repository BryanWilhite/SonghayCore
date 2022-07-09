using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Songhay.Extensions;

/// <summary>
/// Extensions of <c>Nullable</c> types.
/// </summary>
public static class NullableExtensions
{
    /// <summary>
    /// Rounds the specified decimal.
    /// </summary>
    /// <param name="decimalValue">The decimal value.</param>
    /// <param name="decimals">The decimals.</param>
    /// <returns></returns>
    /// <remarks>
    /// For more detail see http://anderly.com/2009/08/08/silverlight-midpoint-rounding-solution/
    /// </remarks>
    public static decimal Round(this decimal? decimalValue, int decimals)
    {
        var d = decimalValue.GetValueOrDefault();
        decimal factor = Convert.ToDecimal(Math.Pow(10, decimals));
        int sign = Math.Sign(d);

        return Decimal.Truncate(d * factor + 0.5m * sign) / factor;
    }

    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/>
    /// when the specified enumerable is null or empty.
    /// </summary>
    /// <param name="enumerable">the <see cref="IEnumerable{T}"/></param>
    /// <param name="paramName">the name of the variable holding the <see cref="IEnumerable{T}"/></param>
    /// <typeparam name="T">the type of the <see cref="IEnumerable{T}"/></typeparam>
    /// <exception cref="ArgumentNullException"></exception>
    public static void ThrowWhenNullOrEmpty<T>( [NotNull] this IEnumerable<T>? enumerable,
        [CallerArgumentExpression("enumerable")] string? paramName = null)
    {
        if (enumerable != null && enumerable.Any()) return;

        var message = string.IsNullOrWhiteSpace(paramName)
            ? "The expected collection is not here."
            : $"The expected `{paramName}` is not here.";

        throw new ArgumentNullException(message);
    }

    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/>
    /// when <see cref="string.IsNullOrWhiteSpace"/> is <c>true</c>.
    /// </summary>
    /// <param name="value">the <see cref="string"/> value</param>
    /// <param name="paramName">the name of the variable holding the <see cref="string"/></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <remarks>
    /// This member borrows heavily from <see cref="System.ArgumentNullException.ThrowIfNull"/>.
    ///
    /// The <see cref="NotNullAttribute"/> is applied to this member based
    /// on the following statement from Microsoft:
    ///
    /// “Callers can pass a variable with the null value,
    /// but the argument is guaranteed to never be null
    /// if the method returns without throwing an exception.”
    ///
    /// [ see https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/attributes/nullable-analysis#postconditions-maybenull-and-notnull ]
    /// </remarks>
    public static void ThrowWhenNullOrWhiteSpace( [NotNull] this string? value,
        [CallerArgumentExpression("value")] string? paramName = null)
    {
        if (!string.IsNullOrWhiteSpace(value)) return;

        var message = string.IsNullOrWhiteSpace(paramName)
            ? "The expected `string` is not here."
            : $"The expected `{paramName}` is not here.";

        throw new ArgumentNullException(message);
    }

    /// <summary>
    /// Returns the non-null type
    /// or throws an <see cref="ArgumentNullException"/>.
    /// </summary>
    /// <param name="nullable">the nullable</param>
    /// <param name="paramName">the name of the variable holding the nullable</param>
    /// <typeparam name="T">the type</typeparam>
    /// <exception cref="ArgumentNullException"></exception>
    /// <remarks>
    /// This member borrows heavily from <see cref="System.ArgumentNullException.ThrowIfNull"/>.
    ///
    /// The <see cref="NotNullAttribute"/> is applied to this member based
    /// on the following statement from Microsoft:
    ///
    /// “Callers can pass a variable with the null value,
    /// but the argument is guaranteed to never be null
    /// if the method returns without throwing an exception.”
    ///
    /// [ see https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/attributes/nullable-analysis#postconditions-maybenull-and-notnull ]
    /// </remarks>
    public static T ToValueOrThrow<T>( [NotNull] this T? nullable, [CallerArgumentExpression("nullable")] string? paramName = null)
    {
        var message = string.IsNullOrWhiteSpace(paramName)
            ? $"The expected `{typeof(T).Name}` is not here."
            : $"The expected `{paramName}` is not here.";

        return nullable == null ? throw new ArgumentNullException(message) : nullable;
    }
}
