using System;
using System.Globalization;

namespace Songhay
{
    /// <summary>
    /// Static members for type handling.
    /// </summary>
    public static partial class FrameworkTypeUtility
    {
        /// <summary>
        /// Tries to convert the specified frameworkValue
        /// to the <see cref="Nullable"></see> return type.
        /// </summary>
        /// <param name="frameworkValue">
        /// The specified <see cref="Object"></see> box.
        /// </param>
        /// <returns>
        /// Always returns a <see cref="Nullable"></see>
        /// as parse failure means <c>HasValue</c>
        /// is false.
        /// </returns>
        public static bool? ParseBoolean(object frameworkValue)
        {
            bool supportBitStrings = false;
            return ParseBoolean(frameworkValue, supportBitStrings);
        }

        /// <summary>
        /// Tries to convert the specified frameworkValue
        /// to the <see cref="Nullable"/> return type.
        /// </summary>
        /// <param name="frameworkValue">
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
        public static bool? ParseBoolean(object frameworkValue, bool supportBitStrings)
        {
            bool bln;
            string s = (frameworkValue != null) ? frameworkValue.ToString() : string.Empty;

            if (supportBitStrings)
            {
                if (s.Trim().Equals("0")) return new bool?(false);
                if (s.Trim().Equals("1")) return new bool?(true);
            }

            if (bool.TryParse(s, out bln)) return bln;
            else return default(bool?);
        }

        /// <summary>
        /// Tries to convert the specified frameworkValue
        /// to the <see cref="Nullable"/> return type.
        /// </summary>
        /// <param name="frameworkValue">
        /// The specified <see cref="Object"/> box.
        /// </param>
        /// <param name="supportBitStrings">
        /// When <c>true</c>, support "1" and "0"
        /// as Boolean strings.
        /// </param>
        /// <param name="defaultValue">
        /// The default frameworkValue to return when <c>Nullable.HasValue == false</c>.
        /// </param>
        /// <returns>
        /// Always returns a <see cref="Nullable"/>
        /// as parse failure means <c>HasValue</c>
        /// is false.
        /// </returns>
        public static bool? ParseBoolean(object frameworkValue, bool supportBitStrings, bool defaultValue)
        {
            bool? bln = FrameworkTypeUtility.ParseBoolean(frameworkValue, supportBitStrings);

            if (bln.HasValue) return bln;
            else return new bool?(defaultValue);
        }

        /// <summary>
        /// Tries to convert the specified frameworkValue
        /// to the <see cref="Nullable"/> return type.
        /// </summary>
        /// <param name="frameworkValue">
        /// The specified <see cref="Object"/> box.
        /// </param>
        /// <returns>
        /// Always returns a <see cref="Nullable"/>
        /// as parse failure means <c>HasValue</c>
        /// is false.
        /// </returns>
        public static byte? ParseByte(object frameworkValue)
        {
            byte b;
            string s = (frameworkValue != null) ? frameworkValue.ToString() : string.Empty;
            if (byte.TryParse(s, out b)) return b;
            else return default(byte?);
        }

        /// <summary>
        /// Tries to convert the specified frameworkValue
        /// to the <see cref="Nullable"/> return type.
        /// </summary>
        /// <param name="frameworkValue">
        /// The specified <see cref="Object"/> box.
        /// </param>
        /// <param name="defaultValue">
        /// The default frameworkValue to return when <c>Nullable.HasValue == false</c>.
        /// </param>
        /// <returns>
        /// Always returns a <see cref="Nullable"/>
        /// as parse failure means <c>HasValue</c>
        /// is false.
        /// </returns>
        public static byte? ParseByte(object frameworkValue, byte defaultValue)
        {
            byte? b = FrameworkTypeUtility.ParseByte(frameworkValue);
            if (b.HasValue) return b;
            else return new byte?(defaultValue);
        }

        /// <summary>
        /// Tries to convert the specified frameworkValue
        /// to the <see cref="Nullable"/> return type.
        /// </summary>
        /// <param name="frameworkValue">
        /// The specified <see cref="Object"/> box.
        /// </param>
        /// <returns>
        /// Always returns a <see cref="Nullable"/>
        /// as parse failure means <c>HasValue</c>
        /// is false.
        /// </returns>
        public static DateTime? ParseDateTime(object frameworkValue)
        {
            DateTime d;
            string s = (frameworkValue != null) ? frameworkValue.ToString() : string.Empty;
            if (DateTime.TryParse(s, out d)) return d;
            else return default(DateTime?);
        }

        /// <summary>
        /// Tries to convert the specified frameworkValue
        /// to the <see cref="Nullable"/> return type.
        /// </summary>
        /// <param name="frameworkValue">
        /// The specified <see cref="Object"/> box.
        /// </param>
        /// <param name="defaultValue">
        /// The default frameworkValue to return when <c>Nullable.HasValue == false</c>.
        /// </param>
        /// <returns>
        /// Always returns a <see cref="Nullable"/>
        /// as parse failure means <c>HasValue</c>
        /// is false.
        /// </returns>
        public static DateTime? ParseDateTime(object frameworkValue, DateTime defaultValue)
        {
            DateTime? d = FrameworkTypeUtility.ParseDateTime(frameworkValue);
            if (d.HasValue) return d;
            else return new DateTime?(defaultValue);
        }

        /// <summary>
        /// Tries to convert the specified frameworkValue
        /// to the <see cref="Nullable"/> return type.
        /// </summary>
        /// <param name="frameworkValue">
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
        public static string ParseDateTimeWithFormat(object frameworkValue, string formatExpression)
        {
            DateTime d;
            string s = (frameworkValue != null) ? frameworkValue.ToString() : string.Empty;
            if (DateTime.TryParse(s, out d)) return d.ToString(formatExpression, CultureInfo.CurrentCulture);
            else return null;
        }

        /// <summary>
        /// Tries to convert the specified frameworkValue
        /// to the <see cref="Nullable"/> return type.
        /// </summary>
        /// <param name="frameworkValue">
        /// The specified <see cref="Object"/> box.
        /// </param>
        /// <param name="formatExpression">
        /// The string format expression.
        /// </param>
        /// <param name="defaultValue">
        /// The default frameworkValue to return when <c>Nullable.HasValue == false</c>.
        /// </param>
        /// <returns>
        /// Always returns a <see cref="Nullable"/>
        /// as parse failure means <c>HasValue</c>
        /// is false.
        /// </returns>
        public static string ParseDateTimeWithFormat(object frameworkValue, string formatExpression, string defaultValue)
        {
            string d = FrameworkTypeUtility.ParseDateTimeWithFormat(frameworkValue, formatExpression);
            if (string.IsNullOrWhiteSpace(d)) return defaultValue;
            else return d;
        }

        /// <summary>
        /// Tries to convert the specified frameworkValue
        /// to the <see cref="Nullable"/> return type.
        /// </summary>
        /// <param name="frameworkValue">
        /// The specified <see cref="Object"/> box.
        /// </param>
        /// <returns>
        /// Always returns a <see cref="Nullable"/>
        /// as parse failure means <c>HasValue</c>
        /// is false.
        /// </returns>
        public static decimal? ParseDecimal(object frameworkValue)
        {
            decimal d;
            string s = (frameworkValue != null) ? frameworkValue.ToString() : string.Empty;
            if (decimal.TryParse(s, out d)) return d;
            else return default(decimal?);
        }

        /// <summary>
        /// Tries to convert the specified frameworkValue
        /// to the <see cref="Nullable"/> return type.
        /// </summary>
        /// <param name="frameworkValue">
        /// The specified <see cref="Object"/> box.
        /// </param>
        /// <param name="defaultValue">
        /// The default frameworkValue to return when <c>Nullable.HasValue == false</c>.
        /// </param>
        /// <returns>
        /// Always returns a <see cref="Nullable"/>
        /// as parse failure means <c>HasValue</c>
        /// is false.
        /// </returns>
        public static decimal? ParseDecimal(object frameworkValue, decimal defaultValue)
        {
            decimal? d = ParseDecimal(frameworkValue);
            if (d.HasValue) return d;
            else return new decimal?(defaultValue);
        }

        /// <summary>
        /// Tries to convert the specified frameworkValue
        /// to the <see cref="Nullable"/> return type.
        /// </summary>
        /// <param name="frameworkValue">
        /// The specified <see cref="Object"/> box.
        /// </param>
        /// <returns>
        /// Always returns a <see cref="Nullable"/>
        /// as parse failure means <c>HasValue</c>
        /// is false.
        /// </returns>
        public static double? ParseDouble(object frameworkValue)
        {
            double d;
            string s = (frameworkValue != null) ? frameworkValue.ToString() : string.Empty;
            if (double.TryParse(s, out d)) return d;
            else return default(double?);
        }

        /// <summary>
        /// Tries to convert the specified frameworkValue
        /// to the <see cref="Nullable"/> return type.
        /// </summary>
        /// <param name="frameworkValue">
        /// The specified <see cref="Object"/> box.
        /// </param>
        /// <param name="defaultValue">
        /// The default frameworkValue to return when <c>Nullable.HasValue == false</c>.
        /// </param>
        /// <returns>
        /// Always returns a <see cref="Nullable"/>
        /// as parse failure means <c>HasValue</c>
        /// is false.
        /// </returns>
        public static double? ParseDouble(object frameworkValue, double defaultValue)
        {
            double? d = FrameworkTypeUtility.ParseDouble(frameworkValue);
            if (d.HasValue) return d;
            else return new double?(defaultValue);
        }

        /// <summary>
        /// Tries to convert the specified frameworkValue
        /// to the <see cref="Enum"/> return type.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="frameworkValue">The framework value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        /// <remarks>
        /// For background, see http://stackoverflow.com/a/15017429/22944
        /// </remarks>
        public static TEnum ParseEnum<TEnum>(string frameworkValue, TEnum defaultValue) where TEnum : struct
        {
            var isDefined = string.IsNullOrWhiteSpace(frameworkValue) ? false : Enum.IsDefined(typeof(TEnum), frameworkValue);
            return isDefined ? (TEnum)Enum.Parse(typeof(TEnum), frameworkValue) : defaultValue;
        }

        /// <summary>
        /// Tries to convert the specified frameworkValue
        /// to the <see cref="Nullable"/> return type.
        /// </summary>
        /// <param name="frameworkValue">
        /// The specified <see cref="Object"/> box.
        /// </param>
        /// <returns>
        /// Always returns a <see cref="Nullable"/>
        /// as parse failure means <c>HasValue</c>
        /// is false.
        /// </returns>
        public static Int16? ParseInt16(object frameworkValue)
        {
            Int16 i;
            string s = (frameworkValue != null) ? frameworkValue.ToString() : string.Empty;
            if (Int16.TryParse(s, out i)) return i;
            else return default(Int16?);
        }

        /// <summary>
        /// Tries to convert the specified frameworkValue
        /// to the <see cref="Nullable"/> return type.
        /// </summary>
        /// <param name="frameworkValue">
        /// The specified <see cref="Object"/> box.
        /// </param>
        /// <param name="defaultValue">
        /// The default frameworkValue to return when <c>Nullable.HasValue == false</c>.
        /// </param>
        /// <returns>
        /// Always returns a <see cref="Nullable"/>
        /// as parse failure means <c>HasValue</c>
        /// is false.
        /// </returns>
        public static Int16? ParseInt16(object frameworkValue, Int16 defaultValue)
        {
            Int16? i = FrameworkTypeUtility.ParseInt16(frameworkValue);
            if (i.HasValue) return i;
            else return new Int16?(defaultValue);
        }

        /// <summary>
        /// Tries to convert the specified frameworkValue
        /// to the <see cref="Nullable"/> return type.
        /// </summary>
        /// <param name="frameworkValue">
        /// The specified <see cref="Object"/> box.
        /// </param>
        /// <returns>
        /// Always returns a <see cref="Nullable"/>
        /// as parse failure means <c>HasValue</c>
        /// is false.
        /// </returns>
        public static Int32? ParseInt32(object frameworkValue)
        {
            Int32 i;
            string s = (frameworkValue != null) ? frameworkValue.ToString() : string.Empty;
            if (Int32.TryParse(s, out i)) return i;
            else return default(Int32?);
        }

        /// <summary>
        /// Tries to convert the specified frameworkValue
        /// to the <see cref="Nullable"/> return type.
        /// </summary>
        /// <param name="frameworkValue">
        /// The specified <see cref="Object"/> box.
        /// </param>
        /// <param name="defaultValue">
        /// The default frameworkValue to return when <c>Nullable.HasValue == false</c>.
        /// </param>
        /// <returns>
        /// Always returns a <see cref="Nullable"/>
        /// as parse failure means <c>HasValue</c>
        /// is false.
        /// </returns>
        public static Int32? ParseInt32(object frameworkValue, Int32 defaultValue)
        {
            Int32? i = FrameworkTypeUtility.ParseInt32(frameworkValue);
            if (i.HasValue) return i;
            else return new Int32?(defaultValue);
        }

        /// <summary>
        /// Tries to convert the specified frameworkValue
        /// to the <see cref="Nullable"/> return type.
        /// </summary>
        /// <param name="frameworkValue">
        /// The specified <see cref="Object"/> box.
        /// </param>
        /// <returns>
        /// Always returns a <see cref="Nullable"/>
        /// as parse failure means <c>HasValue</c>
        /// is false.
        /// </returns>
        public static Int64? ParseInt64(object frameworkValue)
        {
            Int64 i;
            string s = (frameworkValue != null) ? frameworkValue.ToString() : string.Empty;
            if (Int64.TryParse(s, out i)) return i;
            else return default(Int64?);
        }

        /// <summary>
        /// Tries to convert the specified frameworkValue
        /// to the <see cref="Nullable"/> return type.
        /// </summary>
        /// <param name="frameworkValue">
        /// The specified <see cref="Object"/> box.
        /// </param>
        /// <param name="defaultValue">
        /// The default frameworkValue to return when <c>Nullable.HasValue == false</c>.
        /// </param>
        /// <returns>
        /// Always returns a <see cref="Nullable"/>
        /// as parse failure means <c>HasValue</c>
        /// is false.
        /// </returns>
        public static Int64? ParseInt64(object frameworkValue, Int64 defaultValue)
        {
            Int64? i = FrameworkTypeUtility.ParseInt64(frameworkValue);
            if (i.HasValue) return i;
            else return new Int64?(defaultValue);
        }

        /// <summary>
        /// Parses the RFC3339 date and time.
        /// </summary>
        /// <param name="frameworkValue">The framework value.</param>
        /// <remarks>
        ///     This member is based on patterns in the Argotic Syndication Framework (http://www.codeplex.com/Argotic).
        /// </remarks>
        public static DateTime ParseRfc3339DateTime(string frameworkValue)
        {
            DateTime minValue = DateTime.MinValue;
            if (string.IsNullOrWhiteSpace(frameworkValue)) throw new ArgumentNullException("frameworkValue", "The specified Framework Value is null.");
            if (!TryParseRfc3339DateTime(frameworkValue, out minValue))
            {
                throw new FormatException(string.Format(CultureInfo.CurrentCulture, "'{0}' is not a valid RFC-3339 formatted date-time Framework Value.", new object[] { frameworkValue }));
            }
            return minValue;
        }

        /// <summary>
        /// Parses the RFC822 date and time.
        /// </summary>
        /// <param name="frameworkValue">The framework value.</param>
        /// <remarks>
        ///     This member is based on patterns in the Argotic Syndication Framework (http://www.codeplex.com/Argotic).
        /// </remarks>
        public static DateTime ParseRfc822DateTime(string frameworkValue)
        {
            DateTime minValue = DateTime.MinValue;
            if (string.IsNullOrWhiteSpace(frameworkValue)) throw new ArgumentNullException("frameworkValue", "The specified Framework Value is null.");
            if (!TryParseRfc822DateTime(frameworkValue, out minValue))
            {
                throw new FormatException(string.Format(CultureInfo.CurrentCulture, "'{0}' is not a valid RFC-822 formatted date-time Framework Value.", new object[] { frameworkValue }));
            }
            return minValue;
        }

        /// <summary>
        /// Tries to convert the specified frameworkValue
        /// to a <see cref="String"/>.
        /// </summary>
        /// <param name="frameworkValue">
        /// The specified <see cref="Object"/> box.
        /// </param>
        public static string ParseString(object frameworkValue)
        {
            return (frameworkValue != null) ? frameworkValue.ToString() : null;
        }

        /// <summary>
        /// Tries to convert the specified frameworkValue
        /// to a <see cref="String"/>.
        /// </summary>
        /// <param name="frameworkValue">The frameworkValue.</param>
        /// <param name="defaultValue">The default frameworkValue.</param>
        public static string ParseString(object frameworkValue, string defaultValue)
        {
            return (frameworkValue != null) ? frameworkValue.ToString() : defaultValue;
        }

        /// <summary>
        /// Tries the parse RFC3339 date and time.
        /// </summary>
        /// <param name="frameworkValue">The framework value.</param>
        /// <param name="result">The result.</param>
        /// <remarks>
        ///     This member is based on patterns in the Argotic Syndication Framework (http://www.codeplex.com/Argotic).
        /// </remarks>
        public static bool TryParseRfc3339DateTime(string frameworkValue, out DateTime result)
        {
            DateTimeFormatInfo dateTimeFormat = CultureInfo.InvariantCulture.DateTimeFormat;
            string[] formats = new string[] { dateTimeFormat.SortableDateTimePattern, dateTimeFormat.UniversalSortableDateTimePattern, "yyyy'-'MM'-'dd'T'HH:mm:ss'Z'", "yyyy'-'MM'-'dd'T'HH:mm:ss.f'Z'", "yyyy'-'MM'-'dd'T'HH:mm:ss.ff'Z'", "yyyy'-'MM'-'dd'T'HH:mm:ss.fff'Z'", "yyyy'-'MM'-'dd'T'HH:mm:ss.ffff'Z'", "yyyy'-'MM'-'dd'T'HH:mm:ss.fffff'Z'", "yyyy'-'MM'-'dd'T'HH:mm:ss.ffffff'Z'", "yyyy'-'MM'-'dd'T'HH:mm:sszzz", "yyyy'-'MM'-'dd'T'HH:mm:ss.ffzzz", "yyyy'-'MM'-'dd'T'HH:mm:ss.fffzzz", "yyyy'-'MM'-'dd'T'HH:mm:ss.ffffzzz", "yyyy'-'MM'-'dd'T'HH:mm:ss.fffffzzz", "yyyy'-'MM'-'dd'T'HH:mm:ss.ffffffzzz" };
            if (string.IsNullOrWhiteSpace(frameworkValue))
            {
                result = DateTime.MinValue;
                return false;
            }
            return DateTime.TryParseExact(frameworkValue, formats, dateTimeFormat, DateTimeStyles.AssumeUniversal, out result);
        }

        /// <summary>
        /// Tries the parse RFC822 date and time.
        /// </summary>
        /// <param name="frameworkValue">The framework value.</param>
        /// <param name="result">The result.</param>
        /// <remarks>
        ///     This member is based on patterns in the Argotic Syndication Framework (http://www.codeplex.com/Argotic).
        /// </remarks>
        public static bool TryParseRfc822DateTime(string frameworkValue, out DateTime result)
        {
            DateTimeFormatInfo dateTimeFormat = CultureInfo.InvariantCulture.DateTimeFormat;
            string[] formats = new string[] { dateTimeFormat.RFC1123Pattern, "ddd',' d MMM yyyy HH:mm:ss zzz", "ddd',' dd MMM yyyy HH:mm:ss zzz" };
            if (string.IsNullOrWhiteSpace(frameworkValue))
            {
                result = DateTime.MinValue;
                return false;
            }
            return DateTime.TryParseExact(ReplaceRfc822TimeZoneWithOffset(frameworkValue), formats, dateTimeFormat, DateTimeStyles.None, out result);
        }

        static string ReplaceRfc822TimeZoneWithOffset(string frameworkValue)
        {
            if (string.IsNullOrWhiteSpace(frameworkValue)) throw new ArgumentNullException("frameworkValue", "The specified Framework Value is null.");
            frameworkValue = frameworkValue.Trim();
            if (frameworkValue.EndsWith("UT", StringComparison.OrdinalIgnoreCase))
            {
                return string.Format(CultureInfo.CurrentCulture, "{0}GMT", new object[] { frameworkValue.TrimEnd("UT".ToCharArray()) });
            }
            if (frameworkValue.EndsWith("EST", StringComparison.OrdinalIgnoreCase))
            {
                return string.Format(CultureInfo.CurrentCulture, "{0}-05:00", new object[] { frameworkValue.TrimEnd("EST".ToCharArray()) });
            }
            if (frameworkValue.EndsWith("EDT", StringComparison.OrdinalIgnoreCase))
            {
                return string.Format(CultureInfo.CurrentCulture, "{0}-04:00", new object[] { frameworkValue.TrimEnd("EDT".ToCharArray()) });
            }
            if (frameworkValue.EndsWith("CST", StringComparison.OrdinalIgnoreCase))
            {
                return string.Format(CultureInfo.CurrentCulture, "{0}-06:00", new object[] { frameworkValue.TrimEnd("CST".ToCharArray()) });
            }
            if (frameworkValue.EndsWith("CDT", StringComparison.OrdinalIgnoreCase))
            {
                return string.Format(CultureInfo.CurrentCulture, "{0}-05:00", new object[] { frameworkValue.TrimEnd("CDT".ToCharArray()) });
            }
            if (frameworkValue.EndsWith("MST", StringComparison.OrdinalIgnoreCase))
            {
                return string.Format(CultureInfo.CurrentCulture, "{0}-07:00", new object[] { frameworkValue.TrimEnd("MST".ToCharArray()) });
            }
            if (frameworkValue.EndsWith("MDT", StringComparison.OrdinalIgnoreCase))
            {
                return string.Format(CultureInfo.CurrentCulture, "{0}-06:00", new object[] { frameworkValue.TrimEnd("MDT".ToCharArray()) });
            }
            if (frameworkValue.EndsWith("PST", StringComparison.OrdinalIgnoreCase))
            {
                return string.Format(CultureInfo.CurrentCulture, "{0}-08:00", new object[] { frameworkValue.TrimEnd("PST".ToCharArray()) });
            }
            if (frameworkValue.EndsWith("PDT", StringComparison.OrdinalIgnoreCase))
            {
                return string.Format(CultureInfo.CurrentCulture, "{0}-07:00", new object[] { frameworkValue.TrimEnd("PDT".ToCharArray()) });
            }
            if (frameworkValue.EndsWith("Z", StringComparison.OrdinalIgnoreCase))
            {
                return string.Format(CultureInfo.CurrentCulture, "{0}GMT", new object[] { frameworkValue.TrimEnd("Z".ToCharArray()) });
            }
            if (frameworkValue.EndsWith("A", StringComparison.OrdinalIgnoreCase))
            {
                return string.Format(CultureInfo.CurrentCulture, "{0}-01:00", new object[] { frameworkValue.TrimEnd("A".ToCharArray()) });
            }
            if (frameworkValue.EndsWith("M", StringComparison.OrdinalIgnoreCase))
            {
                return string.Format(CultureInfo.CurrentCulture, "{0}-12:00", new object[] { frameworkValue.TrimEnd("M".ToCharArray()) });
            }
            if (frameworkValue.EndsWith("N", StringComparison.OrdinalIgnoreCase))
            {
                return string.Format(CultureInfo.CurrentCulture, "{0}+01:00", new object[] { frameworkValue.TrimEnd("N".ToCharArray()) });
            }
            if (frameworkValue.EndsWith("Y", StringComparison.OrdinalIgnoreCase))
            {
                return string.Format(CultureInfo.CurrentCulture, "{0}+12:00", new object[] { frameworkValue.TrimEnd("Y".ToCharArray()) });
            }
            return frameworkValue;
        }

    }
}
