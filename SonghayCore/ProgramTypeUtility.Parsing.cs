using System;
using System.Globalization;

namespace Songhay;

/// <summary>
/// Static members for type handling.
/// </summary>
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
    public static bool? ParseBoolean(object value)
    {
        bool supportBitStrings = false;
        return ParseBoolean(value, supportBitStrings);
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
    /// <returns>
    /// Always returns a <see cref="Nullable"/>
    /// as parse failure means <c>HasValue</c>
    /// is false.
    /// </returns>
    public static bool? ParseBoolean(object value, bool supportBitStrings)
    {
        bool bln;
        string s = (value != null) ? value.ToString() : string.Empty;

        if (supportBitStrings)
        {
            if (s.Trim().Equals("0")) return new bool?(false);
            if (s.Trim().Equals("1")) return new bool?(true);
        }

        if (bool.TryParse(s, out bln)) return bln;
        else return default(bool?);
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
    public static bool? ParseBoolean(object value, bool supportBitStrings, bool defaultValue)
    {
        bool? bln = ProgramTypeUtility.ParseBoolean(value, supportBitStrings);

        if (bln.HasValue) return bln;
        else return new bool?(defaultValue);
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
    public static byte? ParseByte(object value)
    {
        byte b;
        string s = (value != null) ? value.ToString() : string.Empty;
        if (byte.TryParse(s, out b)) return b;
        else return default(byte?);
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
    public static byte? ParseByte(object value, byte defaultValue)
    {
        byte? b = ProgramTypeUtility.ParseByte(value);
        if (b.HasValue) return b;
        else return new byte?(defaultValue);
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
    public static DateTime? ParseDateTime(object value)
    {
        DateTime d;
        string s = (value != null) ? value.ToString() : string.Empty;
        if (DateTime.TryParse(s, out d)) return d;
        else return default(DateTime?);
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
    public static DateTime? ParseDateTime(object value, DateTime defaultValue)
    {
        DateTime? d = ProgramTypeUtility.ParseDateTime(value);
        if (d.HasValue) return d;
        else return new DateTime?(defaultValue);
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
    public static string ParseDateTimeWithFormat(object value, string formatExpression)
    {
        DateTime d;
        string s = (value != null) ? value.ToString() : string.Empty;
        if (DateTime.TryParse(s, out d)) return d.ToString(formatExpression, CultureInfo.CurrentCulture);
        else return null;
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
    public static string ParseDateTimeWithFormat(object value, string formatExpression, string defaultValue)
    {
        string d = ProgramTypeUtility.ParseDateTimeWithFormat(value, formatExpression);
        if (string.IsNullOrWhiteSpace(d)) return defaultValue;
        else return d;
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
    public static decimal? ParseDecimal(object value)
    {
        decimal d;
        string s = (value != null) ? value.ToString() : string.Empty;
        if (decimal.TryParse(s, out d)) return d;
        else return default(decimal?);
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
    public static decimal? ParseDecimal(object value, decimal defaultValue)
    {
        decimal? d = ParseDecimal(value);
        if (d.HasValue) return d;
        else return new decimal?(defaultValue);
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
    public static double? ParseDouble(object value)
    {
        double d;
        string s = (value != null) ? value.ToString() : string.Empty;
        if (double.TryParse(s, out d)) return d;
        else return default(double?);
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
    public static double? ParseDouble(object value, double defaultValue)
    {
        double? d = ProgramTypeUtility.ParseDouble(value);
        if (d.HasValue) return d;
        else return new double?(defaultValue);
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
        var isDefined = string.IsNullOrWhiteSpace(value) ? false : Enum.IsDefined(typeof(TEnum), value);
        return isDefined ? (TEnum)Enum.Parse(typeof(TEnum), value) : defaultValue;
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
    public static Int16? ParseInt16(object value)
    {
        Int16 i;
        string s = (value != null) ? value.ToString() : string.Empty;
        if (Int16.TryParse(s, out i)) return i;
        else return default(Int16?);
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
    public static Int16? ParseInt16(object value, Int16 defaultValue)
    {
        Int16? i = ProgramTypeUtility.ParseInt16(value);
        if (i.HasValue) return i;
        else return new Int16?(defaultValue);
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
    public static Int32? ParseInt32(object value)
    {
        Int32 i;
        string s = (value != null) ? value.ToString() : string.Empty;
        if (Int32.TryParse(s, out i)) return i;
        else return default(Int32?);
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
    public static Int32? ParseInt32(object value, Int32 defaultValue)
    {
        Int32? i = ProgramTypeUtility.ParseInt32(value);
        if (i.HasValue) return i;
        else return new Int32?(defaultValue);
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
    public static Int64? ParseInt64(object value)
    {
        Int64 i;
        string s = (value != null) ? value.ToString() : string.Empty;
        if (Int64.TryParse(s, out i)) return i;
        else return default(Int64?);
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
    public static Int64? ParseInt64(object value, Int64 defaultValue)
    {
        Int64? i = ProgramTypeUtility.ParseInt64(value);
        if (i.HasValue) return i;
        else return new Int64?(defaultValue);
    }

    /// <summary>
    /// Parses the RFC3339 date and time.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <remarks>
    ///     This member is based on patterns in the Argotic Syndication Framework (http://www.codeplex.com/Argotic).
    /// </remarks>
    public static DateTime ParseRfc3339DateTime(string value)
    {
        DateTime minValue = DateTime.MinValue;
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));
        if (!TryParseRfc3339DateTime(value, out minValue))
        {
            throw new FormatException(string.Format(CultureInfo.CurrentCulture, "'{0}' is not a valid RFC-3339 formatted date-time value.", new object[] { value }));
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
        DateTime minValue = DateTime.MinValue;
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));
        if (!TryParseRfc822DateTime(value, out minValue))
        {
            throw new FormatException(string.Format(CultureInfo.CurrentCulture, "'{0}' is not a valid RFC-822 formatted date-time value.", new object[] { value }));
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
    public static string ParseString(object value)
    {
        return (value != null) ? value.ToString() : null;
    }

    /// <summary>
    /// Tries to convert the specified value
    /// to a <see cref="String"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="defaultValue">The default value.</param>
    public static string ParseString(object value, string defaultValue)
    {
        return (value != null) ? value.ToString() : defaultValue;
    }

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
        string[] formats = new string[] { dateTimeFormat.SortableDateTimePattern, dateTimeFormat.UniversalSortableDateTimePattern, "yyyy'-'MM'-'dd'T'HH:mm:ss'Z'", "yyyy'-'MM'-'dd'T'HH:mm:ss.f'Z'", "yyyy'-'MM'-'dd'T'HH:mm:ss.ff'Z'", "yyyy'-'MM'-'dd'T'HH:mm:ss.fff'Z'", "yyyy'-'MM'-'dd'T'HH:mm:ss.ffff'Z'", "yyyy'-'MM'-'dd'T'HH:mm:ss.fffff'Z'", "yyyy'-'MM'-'dd'T'HH:mm:ss.ffffff'Z'", "yyyy'-'MM'-'dd'T'HH:mm:sszzz", "yyyy'-'MM'-'dd'T'HH:mm:ss.ffzzz", "yyyy'-'MM'-'dd'T'HH:mm:ss.fffzzz", "yyyy'-'MM'-'dd'T'HH:mm:ss.ffffzzz", "yyyy'-'MM'-'dd'T'HH:mm:ss.fffffzzz", "yyyy'-'MM'-'dd'T'HH:mm:ss.ffffffzzz" };
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
        string[] formats = new string[] { dateTimeFormat.RFC1123Pattern, "ddd',' d MMM yyyy HH:mm:ss zzz", "ddd',' dd MMM yyyy HH:mm:ss zzz" };
        if (string.IsNullOrWhiteSpace(value))
        {
            result = DateTime.MinValue;
            return false;
        }
        return DateTime.TryParseExact(ReplaceRfc822TimeZoneWithOffset(value), formats, dateTimeFormat, DateTimeStyles.None, out result);
    }

    static string ReplaceRfc822TimeZoneWithOffset(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));
        value = value.Trim();
        if (value.EndsWith("UT", StringComparison.OrdinalIgnoreCase))
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}GMT", new object[] { value.TrimEnd("UT".ToCharArray()) });
        }
        if (value.EndsWith("EST", StringComparison.OrdinalIgnoreCase))
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}-05:00", new object[] { value.TrimEnd("EST".ToCharArray()) });
        }
        if (value.EndsWith("EDT", StringComparison.OrdinalIgnoreCase))
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}-04:00", new object[] { value.TrimEnd("EDT".ToCharArray()) });
        }
        if (value.EndsWith("CST", StringComparison.OrdinalIgnoreCase))
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}-06:00", new object[] { value.TrimEnd("CST".ToCharArray()) });
        }
        if (value.EndsWith("CDT", StringComparison.OrdinalIgnoreCase))
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}-05:00", new object[] { value.TrimEnd("CDT".ToCharArray()) });
        }
        if (value.EndsWith("MST", StringComparison.OrdinalIgnoreCase))
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}-07:00", new object[] { value.TrimEnd("MST".ToCharArray()) });
        }
        if (value.EndsWith("MDT", StringComparison.OrdinalIgnoreCase))
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}-06:00", new object[] { value.TrimEnd("MDT".ToCharArray()) });
        }
        if (value.EndsWith("PST", StringComparison.OrdinalIgnoreCase))
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}-08:00", new object[] { value.TrimEnd("PST".ToCharArray()) });
        }
        if (value.EndsWith("PDT", StringComparison.OrdinalIgnoreCase))
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}-07:00", new object[] { value.TrimEnd("PDT".ToCharArray()) });
        }
        if (value.EndsWith("Z", StringComparison.OrdinalIgnoreCase))
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}GMT", new object[] { value.TrimEnd("Z".ToCharArray()) });
        }
        if (value.EndsWith("A", StringComparison.OrdinalIgnoreCase))
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}-01:00", new object[] { value.TrimEnd("A".ToCharArray()) });
        }
        if (value.EndsWith("M", StringComparison.OrdinalIgnoreCase))
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}-12:00", new object[] { value.TrimEnd("M".ToCharArray()) });
        }
        if (value.EndsWith("N", StringComparison.OrdinalIgnoreCase))
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}+01:00", new object[] { value.TrimEnd("N".ToCharArray()) });
        }
        if (value.EndsWith("Y", StringComparison.OrdinalIgnoreCase))
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}+12:00", new object[] { value.TrimEnd("Y".ToCharArray()) });
        }
        return value;
    }

}