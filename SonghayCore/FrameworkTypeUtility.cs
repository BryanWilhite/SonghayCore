using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace Songhay
{
    /// <summary>
    /// Static members for type handling.
    /// </summary>
    public static partial class FrameworkTypeUtility
    {
        /// <summary>
        /// Converts Unix time stamps
        /// to <see cref="DateTime"/>.
        /// </summary>
        /// <param name="time">A <see cref="Double"/>.</param>
        /// <returns>A <see cref="DateTime"/>.</returns>
        public static DateTime ConvertDateTimeFromUnixTime(double time)
        {
            return (new DateTime(1970, 1, 1, 0, 0, 0)).AddSeconds(time);
        }

        /// <summary>
        /// Converts the specified <see cref="DateTime "/> to RFC3339.
        /// </summary>
        /// <param name="utcDateTime">The UTC date and time.</param>
        public static string ConvertDateTimeToRfc3339DateTime(DateTime utcDateTime)
        {
            DateTimeFormatInfo dateTimeFormat = CultureInfo.InvariantCulture.DateTimeFormat;
            if(utcDateTime.Kind == DateTimeKind.Local)
            {
                return utcDateTime.ToString("yyyy'-'MM'-'dd'T'HH:mm:ss.ffzzz", dateTimeFormat);
            }
            return utcDateTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.ff'Z'", dateTimeFormat);
        }

        /// <summary>
        /// Converts the specified <see cref="DateTime "/> to RFC822.
        /// </summary>
        /// <param name="dateTime">The date and time.</param>
        /// <remarks>
        /// “Shortcomings of OPML… The RFC 822 date format is considered obsolete,
        /// and amongst other things permits the representation of years as two digits.
        /// (RFC 822 has been superseded by RFC 2822.)
        /// In general, date and time formats should be represented according to RFC 3339.”
        /// http://www.answers.com/topic/opml
        /// </remarks>
        public static string ConvertDateTimeToRfc822DateTime(DateTime dateTime)
        {
            DateTimeFormatInfo dateTimeFormat = CultureInfo.InvariantCulture.DateTimeFormat;
            return dateTime.ToString(dateTimeFormat.RFC1123Pattern, dateTimeFormat);
        }

        /// <summary>
        /// Converts the current time
        /// to a Unix time stamp.
        /// </summary>
        /// <returns>A <see cref="Double"/>.</returns>
        public static double ConvertDateTimeToUnixTime()
        {
            return (DateTime.UtcNow
                - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        }

        /// <summary>
        /// Converts a <see cref="DateTime"/>
        /// to a Unix time stamp.
        /// </summary>
        /// <param name="dateValue">The <see cref="DateTime"/>.</param>
        /// <returns>A <see cref="Double"/>.</returns>
        public static double ConvertDateTimeToUnixTime(DateTime dateValue)
        {
            return (dateValue.ToUniversalTime()
                - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        }

        /// <summary>
        /// Converts a <see cref="DateTime"/>
        /// to UTC (ISO 8601).
        /// </summary>
        /// <param name="dateValue">The <see cref="DateTime"/>.</param>
        /// <remarks>
        /// For detail, see https://stackoverflow.com/a/1728437/22944
        /// </remarks>
        /// <returns></returns>
        public static string ConvertDateTimeToUtc(DateTime dateValue)
        {
            return dateValue
                .ToUniversalTime()
                .ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
        }

        /// <summary>
        /// Converts inches as a <see cref="Single"/>
        /// to points.
        /// </summary>
        /// <param name="inches">The inches.</param>
        /// <remarks>
        /// 1 point = 0.013837 inch
        /// </remarks>
        public static float ConvertInchesToPoints(float inches)
        {
            return inches / 0.01387f;
        }

        /// <summary>
        /// Converts points as a <see cref="Single"/>
        /// to inches.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <remarks>
        /// 1 point = 0.013837 inch
        /// </remarks>
        public static float ConvertPointsToInches(float points)
        {
            return points * 0.01387f;
        }

        /// <summary>
        /// Generates the random password.
        /// </summary>
        /// <param name="passwordLength">Length of the password.</param>
        /// <remarks>
        /// See “Generate random password in C#” by Mads Kristensen
        /// [http://madskristensen.net/post/Generate-random-password-in-C]
        /// </remarks>
        public static string GenerateRandomPassword(int passwordLength)
        {
            var allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";
            var chars = new char[passwordLength];
            var r = new Random();

            for (int i = 0; i < passwordLength; i++)
            {
                chars[i] = allowedChars[r.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

        /// <summary>
        /// Gets the deep clone.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj.</param>
        /// <remarks>
        /// See “How do you do a deep copy an object in .Net (C# specifically)?”
        /// [http://stackoverflow.com/questions/129389/how-do-you-do-a-deep-copy-an-object-in-net-c-specifically]
        /// See “Shallow Copy vs. Deep Copy in .NET”
        /// [http://www.codeproject.com/Articles/28952/Shallow-Copy-vs-Deep-Copy-in-NET]
        /// </remarks>
        [Obsolete("consider using https://github.com/force-net/DeepCloner instead")]
        public static T GetDeepClone<T>(T obj)
        {
            using(var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        /// <summary>
        /// Returns <c>true</c> when the specified value
        /// is <c>null</c> or <see cref="String.Empty"/>.
        /// </summary>
        /// <param name="boxedString">The framework value.</param>
        public static bool IsNullOrEmptyString(object boxedString)
        {
            if(boxedString == null) return true;
            else return string.IsNullOrWhiteSpace(boxedString.ToString());
        }

        /// <summary>
        /// Returns <c>true</c> when the specified value
        /// is an empty array, not an array or null.
        /// </summary>
        /// <param name="boxedArray">The specified value.</param>
        public static bool IsNullOrEmptyOrNotArray(object boxedArray)
        {
            if (boxedArray == null) return true;

            Array array = boxedArray as Array;
            return ((array == null) || (array.Length == 0));
        }

        /// <summary>
        /// Sets the properties of the output <see cref="Type" />.
        /// </summary>
        /// <typeparam name="TIn">The type of the in.</typeparam>
        /// <typeparam name="TOut">The type of the out.</typeparam>
        /// <param name="input">The input.</param>
        /// <param name="output">The output.</param>
        [Obsolete("consider using https://github.com/MarcinJuraszek/CloneExtensions instead")]
        public static void SetProperties<TIn, TOut>(TIn input, TOut output)
            where TIn : class
            where TOut : class
        {
            SetProperties<TIn, TOut>(input, output, null);
        }

        /// <summary>
        /// Sets the properties of the output <see cref="Type" />.
        /// </summary>
        /// <typeparam name="TIn">The type of the in.</typeparam>
        /// <typeparam name="TOut">The type of the out.</typeparam>
        /// <param name="input">the input.</param>
        /// <param name="output">the output.</param>
        /// <param name="includedProperties">The included properties.</param>
        [Obsolete("consider using https://github.com/MarcinJuraszek/CloneExtensions instead")]
        public static void SetProperties<TIn, TOut>(TIn input, TOut output, ICollection<string> includedProperties)
            where TIn : class
            where TOut : class
        {
            if ((input == null) || (output == null)) return;
            Type inType = typeof(TIn);
            Type outType = output.GetType();
            foreach (PropertyInfo info in inType.GetProperties())
            {
                PropertyInfo outfo = ((info != null) && info.CanRead)
                    ? outType.GetProperty(info.Name, info.PropertyType)
                    : null;
                if (outfo != null && outfo.CanWrite
                    && (outfo.PropertyType.Equals(info.PropertyType)))
                {
                    if ((includedProperties != null) && includedProperties.Contains(info.Name))
                        outfo.SetValue(output, info.GetValue(input, null), null);
                    else if (includedProperties == null)
                        outfo.SetValue(output, info.GetValue(input, null), null);
                }
            }
        }

        /// <summary>
        /// Sets the properties, excluding the specified property names.
        /// </summary>
        /// <typeparam name="TIn">The type of the in.</typeparam>
        /// <typeparam name="TOut">The type of the out.</typeparam>
        /// <param name="input">The input.</param>
        /// <param name="output">The output.</param>
        /// <param name="excludedProperties">The excluded properties.</param>
        [Obsolete("consider using https://github.com/MarcinJuraszek/CloneExtensions instead")]
        public static void SetPropertiesExcluding<TIn, TOut>(TIn input, TOut output, ICollection<string> excludedProperties)
            where TIn : class
            where TOut : class
        {
            if ((input == null) || (output == null) || (excludedProperties == null)) return;
            Type inType = typeof(TIn);
            Type outType = output.GetType();
            foreach (PropertyInfo info in inType.GetProperties())
            {
                PropertyInfo outfo = ((info != null) && info.CanRead)
                    ? outType.GetProperty(info.Name, info.PropertyType)
                    : null;
                if (outfo != null && outfo.CanWrite
                    && (outfo.PropertyType.Equals(info.PropertyType)))
                {
                    if (!excludedProperties.Contains(info.Name))
                        outfo.SetValue(output, info.GetValue(input, null), null);
                }
            }
        }

        /// <summary>
        /// Returns the conventional database null
        /// (<see cref="DBNull"/>)
        /// for Microsoft SQL Server systems.
        /// </summary>
        /// <returns><see cref="DBNull"/></returns>
        public static DBNull SqlDatabaseNull()
        {
            return DBNull.Value;
        }
    }
}
