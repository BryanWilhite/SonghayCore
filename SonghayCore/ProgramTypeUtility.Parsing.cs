using System;
using System.Globalization;
using Songhay.Extensions;

namespace Songhay;

/// <summary>
/// Static members for type handling.
/// </summary>
/// <remarks>
/// Most of the Parse methods were originally meant
/// for unboxing values from XML documents.
/// </remarks>
public static partial class ProgramTypeUtility
{
    /// <summary>
    /// Tries to convert the specified value
    /// to the <see cref="Nullable"></see> return type.
    /// </summary>
    /// <param name="value">
    /// The specified <see cref="Object"></see> box.
    /// </param>
    /// <returns>
    /// Always returns a <see cref="Nullable"></see>
    /// as parse failure means <c>HasValue</c>
    /// is false.
    /// </returns>
    public static bool? ParseBoolean(object? value) => ParseBoolean(value, supportBitStrings: false);

    /// <summary>
    /// Tries to convert the specified value
    /// to the <see cref="Nullable"/> return type.
    /// </summary>
    /// <param name="value">
    /// The specified <see cref="Object"/> box.
    /// </param>
    /// <param name="supportBitStrings">
    /// When <c>true</c>, support "1" and "0"
    /// as Boolean strings.
    /// </param>
    /// <returns>
    /// Always returns a <see cref="Nullable"/>
    /// as parse failure means <c>HasValue</c>
    /// is false.
    /// </returns>
    public static bool? ParseBoolean(object? value, bool supportBitStrings)
    {
        string? s = (value != null) ? value.ToString() : string.Empty;

        if (supportBitStrings)
        {
            switch (s?.Trim())
            {
                case "0":
                    return false;
                case "1":
                    return true;
            }
        }

        return bool.TryParse(s, out var bln) ? bln : default(bool?);
    }

    /// <summary>
    /// Tries to convert the specified value
    /// to the <see cref="Nullable"/> return type.
    /// </summary>
    /// <param name="value">
    /// The specified <see cref="Object"/> box.
    /// </param>
    /// <param name="supportBitStrings">
    /// When <c>true</c>, support "1" and "0"
    /// as Boolean strings.
    /// </param>
    /// <param name="defaultValue">
    /// The default value to return when <c>Nullable.HasValue == false</c>.
    /// </param>
    /// <returns>
    /// Always returns a <see cref="Nullable"/>
    /// as parse failure means <c>HasValue</c>
    /// is false.
    /// </returns>
    public static bool? ParseBoolean(object? value, bool supportBitStrings, bool defaultValue)
    {
        bool? bln = ParseBoolean(value, supportBitStrings);

        return bln ?? new bool?(defaultValue);
    }

    /// <summary>
    /// Tries to convert the specified value
    /// to the <see cref="Nullable"/> return type.
    /// </summary>
    /// <param name="value">
    /// The specified <see cref="Object"/> box.
    /// </param>
    /// <returns>
    /// Always returns a <see cref="Nullable"/>
    /// as parse failure means <c>HasValue</c>
    /// is false.
    /// </returns>
    public static byte? ParseByte(object? value)
    {
        string? s = value != null ? value.ToString() : string.Empty;

        return byte.TryParse(s, out var b) ? b : default(byte?);
    }

    /// <summary>
    /// Tries to convert the specified value
    /// to the <see cref="Nullable"/> return type.
    /// </summary>
    /// <param name="value">
    /// The specified <see cref="Object"/> box.
    /// </param>
    /// <param name="defaultValue">
    /// The default value to return when <c>Nullable.HasValue == false</c>.
    /// </param>
    /// <returns>
    /// Always returns a <see cref="Nullable"/>
    /// as parse failure means <c>HasValue</c>
    /// is false.
    /// </returns>
    public static byte? ParseByte(object? value, byte defaultValue)
    {
        byte? b = ParseByte(value);

        return b ?? new byte?(defaultValue);
    }

    /// <summary>
    /// Tries to convert the specified value
    /// to the <see cref="Nullable"/> return type.
    /// </summary>
    /// <param name="value">
    /// The specified <see cref="Object"/> box.
    /// </param>
    /// <returns>
    /// Always returns a <see cref="Nullable"/>
    /// as parse failure means <c>HasValue</c>
    /// is false.
    /// </returns>
    public static DateTime? ParseDateTime(object? value)
    {
        string? s = value != null ? value.ToString() : string.Empty;

        return DateTime.TryParse(s, out var d) ? d : default(DateTime?);
    }

    /// <summary>
    /// Tries to convert the specified value
    /// to the <see cref="Nullable"/> return type.
    /// </summary>
    /// <param name="value">
    /// The specified <see cref="Object"/> box.
    /// </param>
    /// <param name="defaultValue">
    /// The default value to return when <c>Nullable.HasValue == false</c>.
    /// </param>
    /// <returns>
    /// Always returns a <see cref="Nullable"/>
    /// as parse failure means <c>HasValue</c>
    /// is false.
    /// </returns>
    public static DateTime? ParseDateTime(object? value, DateTime defaultValue)
    {
        DateTime? d = ParseDateTime(value);

        return d ?? new DateTime?(defaultValue);
    }

    /// <summary>
    /// Tries to convert the specified value
    /// to the <see cref="Nullable"/> return type.
    /// </summary>
    /// <param name="value">
    /// The specified <see cref="Object"/> box.
    /// </param>
    /// <param name="formatExpression">
    /// The string format expression.
    /// </param>
    /// <returns>
    /// Always returns a <see cref="Nullable"/>
    /// as parse failure means <c>HasValue</c>
    /// is false.
    /// </returns>
    public static string? ParseDateTimeWithFormat(object? value, string formatExpression)
    {
        string? s = (value != null) ? value.ToString() : string.Empty;

        return DateTime.TryParse(s, out var d) ? d.ToString(formatExpression, CultureInfo.CurrentCulture) : null;
    }

    /// <summary>
    /// Tries to convert the specified value
    /// to the <see cref="Nullable"/> return type.
    /// </summary>
    /// <param name="value">
    /// The specified <see cref="Object"/> box.
    /// </param>
    /// <param name="formatExpression">
    /// The string format expression.
    /// </param>
    /// <param name="defaultValue">
    /// The default value to return when <c>Nullable.HasValue == false</c>.
    /// </param>
    /// <returns>
    /// Always returns a <see cref="Nullable"/>
    /// as parse failure means <c>HasValue</c>
    /// is false.
    /// </returns>
    public static string ParseDateTimeWithFormat(object? value, string formatExpression, string defaultValue)
    {
        string? d = ParseDateTimeWithFormat(value, formatExpression);

        return string.IsNullOrWhiteSpace(d) ? defaultValue : d;
    }

    /// <summary>
    /// Tries to convert the specified value
    /// to the <see cref="Nullable"/> return type.
    /// </summary>
    /// <param name="value">
    /// The specified <see cref="Object"/> box.
    /// </param>
    /// <returns>
    /// Always returns a <see cref="Nullable"/>
    /// as parse failure means <c>HasValue</c>
    /// is false.
    /// </returns>
    public static decimal? ParseDecimal(object? value)
    {
        string? s = (value != null) ? value.ToString() : string.Empty;

        return decimal.TryParse(s, out var d) ? d : default(decimal?);
    }

    /// <summary>
    /// Tries to convert the specified value
    /// to the <see cref="Nullable"/> return type.
    /// </summary>
    /// <param name="value">
    /// The specified <see cref="Object"/> box.
    /// </param>
    /// <param name="defaultValue">
    /// The default value to return when <c>Nullable.HasValue == false</c>.
    /// </param>
    /// <returns>
    /// Always returns a <see cref="Nullable"/>
    /// as parse failure means <c>HasValue</c>
    /// is false.
    /// </returns>
    public static decimal? ParseDecimal(object? value, decimal defaultValue)
    {
        decimal? d = ParseDecimal(value);

        return d ?? new decimal?(defaultValue);
    }

    /// <summary>
    /// Tries to convert the specified value
    /// to the <see cref="Nullable"/> return type.
    /// </summary>
    /// <param name="value">
    /// The specified <see cref="Object"/> box.
    /// </param>
    /// <returns>
    /// Always returns a <see cref="Nullable"/>
    /// as parse failure means <c>HasValue</c>
    /// is false.
    /// </returns>
    public static double? ParseDouble(object? value)
    {
        string? s = value != null ? value.ToString() : string.Empty;

        return double.TryParse(s, out var d) ? d : default(double?);
    }

    /// <summary>
    /// Tries to convert the specified value
    /// to the <see cref="Nullable"/> return type.
    /// </summary>
    /// <param name="value">
    /// The specified <see cref="Object"/> box.
    /// </param>
    /// <param name="defaultValue">
    /// The default value to return when <c>Nullable.HasValue == false</c>.
    /// </param>
    /// <returns>
    /// Always returns a <see cref="Nullable"/>
    /// as parse failure means <c>HasValue</c>
    /// is false.
    /// </returns>
    public static double? ParseDouble(object? value, double defaultValue)
    {
        double? d = ParseDouble(value);

        return d ?? new double?(defaultValue);
    }

    /// <summary>
    /// Tries to convert the specified value
    /// to the <see cref="Enum"/> return type.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enum.</typeparam>
    /// <param name="value">The value.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns></returns>
    /// <remarks>
    /// For background, see http://stackoverflow.com/a/15017429/22944
    /// </remarks>
    public static TEnum ParseEnum<TEnum>(string value, TEnum defaultValue) where TEnum : struct
    {
        var isDefined = !string.IsNullOrWhiteSpace(value) && Enum.IsDefined(typeof(TEnum), value);

        return isDefined ? (TEnum) Enum.Parse(typeof(TEnum), value) : defaultValue;
    }

    /// <summary>
    /// Tries to convert the specified value
    /// to the <see cref="Nullable"/> return type.
    /// </summary>
    /// <param name="value">
    /// The specified <see cref="Object"/> box.
    /// </param>
    /// <returns>
    /// Always returns a <see cref="Nullable"/>
    /// as parse failure means <c>HasValue</c>
    /// is false.
    /// </returns>
    public static Int16? ParseInt16(object? value)
    {
        string? s = (value != null) ? value.ToString() : string.Empty;

        return Int16.TryParse(s, out var i) ? i : default(Int16?);
    }

    /// <summary>
    /// Tries to convert the specified value
    /// to the <see cref="Nullable"/> return type.
    /// </summary>
    /// <param name="value">
    /// The specified <see cref="Object"/> box.
    /// </param>
    /// <param name="defaultValue">
    /// The default value to return when <c>Nullable.HasValue == false</c>.
    /// </param>
    /// <returns>
    /// Always returns a <see cref="Nullable"/>
    /// as parse failure means <c>HasValue</c>
    /// is false.
    /// </returns>
    public static Int16? ParseInt16(object? value, Int16 defaultValue)
    {
        Int16? i = ParseInt16(value);

        return i ?? new Int16?(defaultValue);
    }

    /// <summary>
    /// Tries to convert the specified value
    /// to the <see cref="Nullable"/> return type.
    /// </summary>
    /// <param name="value">
    /// The specified <see cref="Object"/> box.
    /// </param>
    /// <returns>
    /// Always returns a <see cref="Nullable"/>
    /// as parse failure means <c>HasValue</c>
    /// is false.
    /// </returns>
    public static Int32? ParseInt32(object? value)
    {
        string? s = value != null ? value.ToString() : string.Empty;

        return Int32.TryParse(s, out var i) ? i : default(Int32?);
    }

    /// <summary>
    /// Tries to convert the specified value
    /// to the <see cref="Nullable"/> return type.
    /// </summary>
    /// <param name="value">
    /// The specified <see cref="Object"/> box.
    /// </param>
    /// <param name="defaultValue">
    /// The default value to return when <c>Nullable.HasValue == false</c>.
    /// </param>
    /// <returns>
    /// Always returns a <see cref="Nullable"/>
    /// as parse failure means <c>HasValue</c>
    /// is false.
    /// </returns>
    public static Int32? ParseInt32(object? value, Int32 defaultValue)
    {
        Int32? i = ParseInt32(value);

        return i ?? new Int32?(defaultValue);
    }

    /// <summary>
    /// Tries to convert the specified value
    /// to the <see cref="Nullable"/> return type.
    /// </summary>
    /// <param name="value">
    /// The specified <see cref="Object"/> box.
    /// </param>
    /// <returns>
    /// Always returns a <see cref="Nullable"/>
    /// as parse failure means <c>HasValue</c>
    /// is false.
    /// </returns>
    public static Int64? ParseInt64(object? value)
    {
        string? s = value != null ? value.ToString() : string.Empty;

        return Int64.TryParse(s, out var i) ? i : default(Int64?);
    }

    /// <summary>
    /// Tries to convert the specified value
    /// to the <see cref="Nullable"/> return type.
    /// </summary>
    /// <param name="value">
    /// The specified <see cref="Object"/> box.
    /// </param>
    /// <param name="defaultValue">
    /// The default value to return when <c>Nullable.HasValue == false</c>.
    /// </param>
    /// <returns>
    /// Always returns a <see cref="Nullable"/>
    /// as parse failure means <c>HasValue</c>
    /// is false.
    /// </returns>
    public static Int64? ParseInt64(object? value, Int64 defaultValue)
    {
        Int64? i = ParseInt64(value);

        return i ?? new Int64?(defaultValue);
    }

    /// <summary>
    /// Parses the RFC3339 date and time.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <remarks>
    ///     This member is based on patterns in the Argotic Syndication Framework (http://www.codeplex.com/Argotic).
    /// </remarks>
    public static DateTime ParseRfc3339DateTime(string? value)
    {
        value.ThrowWhenNullOrWhiteSpace();

        if (!TryParseRfc3339DateTime(value, out var minValue))
        {
            throw new FormatException(string.Format(CultureInfo.CurrentCulture,
                "'{0}' is not a valid RFC-3339 formatted date-time value.", new object[] {value}));
        }

        return minValue;
    }

    /// <summary>
    /// Parses the RFC822 date and time.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <remarks>
    ///     This member is based on patterns in the Argotic Syndication Framework (http://www.codeplex.com/Argotic).
    /// </remarks>
    public static DateTime ParseRfc822DateTime(string value)
    {
        value.ThrowWhenNullOrWhiteSpace();

        if (!TryParseRfc822DateTime(value, out var minValue))
        {
            throw new FormatException(string.Format(CultureInfo.CurrentCulture,
                "'{0}' is not a valid RFC-822 formatted date-time value.", new object[] {value}));
        }

        return minValue;
    }

    /// <summary>
    /// Tries to convert the specified value
    /// to a <see cref="String"/>.
    /// </summary>
    /// <param name="value">
    /// The specified <see cref="Object"/> box.
    /// </param>
    public static string? ParseString(object? value) => value?.ToString();

    /// <summary>
    /// Tries to convert the specified value
    /// to a <see cref="String"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="defaultValue">The default value.</param>
    public static string? ParseString(object? value, string? defaultValue) =>
        value != null ? value.ToString() : defaultValue;

    /// <summary>
    /// Tries the parse RFC3339 date and time.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="result">The result.</param>
    /// <remarks>
    ///     This member is based on patterns in the Argotic Syndication Framework (http://www.codeplex.com/Argotic).
    /// </remarks>
    public static bool TryParseRfc3339DateTime(string value, out DateTime result)
    {
        DateTimeFormatInfo dateTimeFormat = CultureInfo.InvariantCulture.DateTimeFormat;
        string[] formats =
        {
            dateTimeFormat.SortableDateTimePattern, dateTimeFormat.UniversalSortableDateTimePattern,
            "yyyy'-'MM'-'dd'T'HH:mm:ss'Z'", "yyyy'-'MM'-'dd'T'HH:mm:ss.f'Z'", "yyyy'-'MM'-'dd'T'HH:mm:ss.ff'Z'",
            "yyyy'-'MM'-'dd'T'HH:mm:ss.fff'Z'", "yyyy'-'MM'-'dd'T'HH:mm:ss.ffff'Z'",
            "yyyy'-'MM'-'dd'T'HH:mm:ss.fffff'Z'", "yyyy'-'MM'-'dd'T'HH:mm:ss.ffffff'Z'", "yyyy'-'MM'-'dd'T'HH:mm:sszzz",
            "yyyy'-'MM'-'dd'T'HH:mm:ss.ffzzz", "yyyy'-'MM'-'dd'T'HH:mm:ss.fffzzz", "yyyy'-'MM'-'dd'T'HH:mm:ss.ffffzzz",
            "yyyy'-'MM'-'dd'T'HH:mm:ss.fffffzzz", "yyyy'-'MM'-'dd'T'HH:mm:ss.ffffffzzz"
        };

        if (string.IsNullOrWhiteSpace(value))
        {
            result = DateTime.MinValue;
            return false;
        }

        return DateTime.TryParseExact(value, formats, dateTimeFormat, DateTimeStyles.AssumeUniversal, out result);
    }

    /// <summary>
    /// Tries the parse RFC822 date and time.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="result">The result.</param>
    /// <remarks>
    ///     This member is based on patterns in the Argotic Syndication Framework (http://www.codeplex.com/Argotic).
    /// </remarks>
    public static bool TryParseRfc822DateTime(string value, out DateTime result)
    {
        DateTimeFormatInfo dateTimeFormat = CultureInfo.InvariantCulture.DateTimeFormat;
        string[] formats =
            {dateTimeFormat.RFC1123Pattern, "ddd',' d MMM yyyy HH:mm:ss zzz", "ddd',' dd MMM yyyy HH:mm:ss zzz"};

        if (string.IsNullOrWhiteSpace(value))
        {
            result = DateTime.MinValue;
            return false;
        }

        return DateTime.TryParseExact(ReplaceRfc822TimeZoneWithOffset(value), formats, dateTimeFormat,
            DateTimeStyles.None, out result);
    }

    static string ReplaceRfc822TimeZoneWithOffset(string? value)
    {
        value.ThrowWhenNullOrWhiteSpace();

        value = value.Trim();
        if (value.EndsWith("UT", StringComparison.OrdinalIgnoreCase))
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}GMT",
                new object[] {value.TrimEnd("UT".ToCharArray())});
        }

        if (value.EndsWith("EST", StringComparison.OrdinalIgnoreCase))
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}-05:00",
                new object[] {value.TrimEnd("EST".ToCharArray())});
        }

        if (value.EndsWith("EDT", StringComparison.OrdinalIgnoreCase))
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}-04:00",
                new object[] {value.TrimEnd("EDT".ToCharArray())});
        }

        if (value.EndsWith("CST", StringComparison.OrdinalIgnoreCase))
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}-06:00",
                new object[] {value.TrimEnd("CST".ToCharArray())});
        }

        if (value.EndsWith("CDT", StringComparison.OrdinalIgnoreCase))
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}-05:00",
                new object[] {value.TrimEnd("CDT".ToCharArray())});
        }

        if (value.EndsWith("MST", StringComparison.OrdinalIgnoreCase))
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}-07:00",
                new object[] {value.TrimEnd("MST".ToCharArray())});
        }

        if (value.EndsWith("MDT", StringComparison.OrdinalIgnoreCase))
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}-06:00",
                new object[] {value.TrimEnd("MDT".ToCharArray())});
        }

        if (value.EndsWith("PST", StringComparison.OrdinalIgnoreCase))
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}-08:00",
                new object[] {value.TrimEnd("PST".ToCharArray())});
        }

        if (value.EndsWith("PDT", StringComparison.OrdinalIgnoreCase))
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}-07:00",
                new object[] {value.TrimEnd("PDT".ToCharArray())});
        }

        if (value.EndsWith("Z", StringComparison.OrdinalIgnoreCase))
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}GMT", new object[] {value.TrimEnd("Z".ToCharArray())});
        }

        if (value.EndsWith("A", StringComparison.OrdinalIgnoreCase))
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}-01:00",
                new object[] {value.TrimEnd("A".ToCharArray())});
        }

        if (value.EndsWith("M", StringComparison.OrdinalIgnoreCase))
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}-12:00",
                new object[] {value.TrimEnd("M".ToCharArray())});
        }

        if (value.EndsWith("N", StringComparison.OrdinalIgnoreCase))
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}+01:00",
                new object[] {value.TrimEnd("N".ToCharArray())});
        }

        if (value.EndsWith("Y", StringComparison.OrdinalIgnoreCase))
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}+12:00",
                new object[] {value.TrimEnd("Y".ToCharArray())});
        }

        return value;
    }
}
