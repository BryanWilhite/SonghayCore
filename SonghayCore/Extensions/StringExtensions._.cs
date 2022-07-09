using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="string"/>.
/// </summary>
public static partial class StringExtensions
{
    /// <summary>
    /// Returns <c>true</c> when the strings are equal without regard to cultural locales
    /// or casing.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="otherString"></param>
    public static bool EqualsInvariant(this string? input, string? otherString) =>
        input.EqualsInvariant(otherString, ignoreCase: true);

    /// <summary>
    /// Returns <c>true</c> when the strings are equal without regard to cultural locales.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="otherString"></param>
    /// <param name="ignoreCase"></param>
    /// <returns></returns>
    public static bool EqualsInvariant(this string? input, string? otherString, bool ignoreCase)
    {
        return ignoreCase
            ? string.Equals(input, otherString, StringComparison.InvariantCultureIgnoreCase)
            : string.Equals(input, otherString, StringComparison.InvariantCulture);
    }

    /// <summary>
    /// Escapes the interpolation tokens of <see cref="string.Format(string, object[])"/>.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <returns></returns>
    public static string? EscapeInterpolation(this string? input) =>
        string.IsNullOrWhiteSpace(input) ? input : input.Replace("{", "{{").Replace("}", "}}");

    /// <summary>
    /// Converts camel-case <see cref="System.String"/> to <see cref="System.Collections.Generic.IEnumerable&lt;T&gt;"/>.
    /// </summary>
    /// <param name="input">The input.</param>
    public static IEnumerable<string> FromCamelCaseToEnumerable(this string? input) =>
        new string(input.InsertSpacesBeforeCaps().ToArray())
            .Split(' ').Where(i => !string.IsNullOrWhiteSpace(i));

    /// <summary>
    /// Determines whether the specified input is in the comma-delimited values.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <param name="delimitedValues">The delimited values.</param>
    public static bool In(this string? input, string? delimitedValues) => input.In(delimitedValues, ',');

    /// <summary>
    /// Determines whether the specified input is in the delimited values.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <param name="delimitedValues">The delimited values.</param>
    /// <param name="separator">The separator.</param>
    public static bool In(this string? input, string? delimitedValues, char separator)
    {
        if (string.IsNullOrWhiteSpace(input)) return false;
        if (string.IsNullOrWhiteSpace(delimitedValues)) return false;

        var set = delimitedValues.Split(separator);

        return set.Contains(input);
    }

    /// <summary>
    /// Returns <see cref="string"/> in double quotes.
    /// </summary>
    /// <param name="input">The input.</param>
    public static string? InDoubleQuotes(this string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return null;

        input = input.Replace("\"", "\"\"");

        return $"\"{input}\"";
    }

    /// <summary>
    /// Returns <see cref="string"/> in double quotes or default.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <param name="defaultValue">The default value.</param>
    public static string? InDoubleQuotesOrDefault(this string? input, string defaultValue) =>
        string.IsNullOrWhiteSpace(input) ? defaultValue : input.InDoubleQuotes();

    /// <summary>
    /// Inserts the spaces before caps.
    /// </summary>
    /// <param name="input">The input.</param>
    public static IEnumerable<char> InsertSpacesBeforeCaps(this string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) yield return '\0';

        foreach (char c in input!)
        {
            if (char.IsUpper(c)) yield return ' ';
            yield return c;
        }
    }

    /// <summary>
    /// Determines whether the specified input is byte.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <returns>
    /// 	<c>true</c> if the specified input is byte; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsByte(this string? input) => input.IsByte(null);

    /// <summary>
    /// Determines whether the specified input is byte.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <param name="secondaryTest">The secondary test.</param>
    /// <returns>
    /// 	<c>true</c> if the specified input is byte; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsByte(this string? input, Predicate<byte>? secondaryTest)
    {
        var isExpectedType = byte.TryParse(input, out var testValue);

        return isExpectedType ? secondaryTest?.Invoke(testValue) ?? isExpectedType : isExpectedType;
    }

    /// <summary>
    /// Determines whether the specified input is decimal.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <returns>
    /// 	<c>true</c> if the specified input is decimal; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsDecimal(this string? input) => input.IsDecimal(null);

    /// <summary>
    /// Determines whether the specified input is decimal.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <param name="secondaryTest">The secondary test.</param>
    /// <returns>
    /// 	<c>true</c> if the specified input is decimal; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsDecimal(this string? input, Predicate<decimal>? secondaryTest)
    {
        var isExpectedType = decimal.TryParse(input, out var testValue);

        return isExpectedType ? secondaryTest?.Invoke(testValue) ?? isExpectedType : isExpectedType;
    }

    /// <summary>
    /// Determines whether the specified input is integer.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <returns>
    ///   <c>true</c> if the specified input is integer; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsInteger(this string? input) => input.IsInteger(null);

    /// <summary>
    /// Determines whether the specified input is integer.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <param name="secondaryTest">The secondary test.</param>
    /// <returns>
    ///   <c>true</c> if the specified input is integer; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsInteger(this string? input, Predicate<int>? secondaryTest)
    {
        var isExpectedType = int.TryParse(input, out var testValue);

        return isExpectedType ? secondaryTest?.Invoke(testValue) ?? isExpectedType : isExpectedType;
    }

    /// <summary>
    /// Determines whether the specified input is long.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <returns>
    /// 	<c>true</c> if the specified input is long; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsLong(this string? input) => input.IsLong(null);

    /// <summary>
    /// Determines whether the specified input is long.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <param name="secondaryTest">The secondary test.</param>
    /// <returns>
    /// 	<c>true</c> if the specified input is long; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsLong(this string? input, Predicate<long>? secondaryTest)
    {
        var isExpectedType = long.TryParse(input, out var testValue);

        return isExpectedType ? secondaryTest?.Invoke(testValue) ?? isExpectedType : isExpectedType;
    }

    /// <summary>
    /// Determines whether the specified input is a telephone number.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <returns>
    ///   <c>true</c> if is telephone number; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsTelephoneNumber(this string? input) =>
        !string.IsNullOrWhiteSpace(input) &&
        Regex.IsMatch(input, @"^\(?[0-9]\d{2}\)?([-., ])?[0-9]\d{2}([-., ])?\d{4}$");

    /// <summary>
    /// Determines whether the specified input is a UNC.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <returns>
    ///   <c>true</c> if is a UNC; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    ///  📚 https://stackoverflow.com/a/47531093/22944
    ///  📚 https://en.wikipedia.org/wiki/Path_(computing)#Uniform_Naming_Convention
    /// </remarks>
    public static bool IsUnc(this string? input) =>
        !string.IsNullOrWhiteSpace(input) &&
        Regex.IsMatch(input, @"^(\\(\\[^\s\\]+)+|([A-Za-z]:(\\)?|[A-z]:(\\[^\s\\]+)+))(\\)?$");

    /// <summary>
    /// Determines whether the specified input looks like an email address.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <returns>
    ///   <c>true</c> if seems to be an email address; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// “In short, don’t expect a single, usable regex to do a proper job.
    /// And the best regex will validate the syntax, not the validity
    /// of an e-mail (jhohn@example.com is correct but it will probably bounce…).”
    /// [http://stackoverflow.com/questions/201323/how-to-use-a-regular-expression-to-validate-an-email-addresses]
    /// </remarks>
    public static bool LooksLikeEmailAddress(this string? input) =>
        !string.IsNullOrWhiteSpace(input) &&
        Regex.IsMatch(input, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

    /// <summary>
    /// Remove accent from strings 
    /// </summary>
    /// <example>
    ///  input:  "Příliš žluťoučký kůň úpěl ďábelské ódy."
    ///  result: "Prilis zlutoucky kun upel dabelske ody."
    /// </example>
    /// <param name="s"></param>
    /// <remarks>
    /// From Tomas Kubes, http://www.codeproject.com/Articles/31050/String-Extension-Collection-for-C
    /// Also, see http://stackoverflow.com/questions/249087/how-do-i-remove-diacritics-accents-from-a-string-in-net
    /// </remarks>
    /// <returns>string without accents</returns>
    public static string RemoveDiacritics(this string s)
    {
        string stFormD = s.Normalize(NormalizationForm.FormD);
        StringBuilder sb = new StringBuilder();

        foreach (var t in stFormD)
        {
            UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(t);
            if (uc != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(t);
            }
        }

        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
}
