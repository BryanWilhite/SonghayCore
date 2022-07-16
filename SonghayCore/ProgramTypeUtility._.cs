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
    /// Converts Unix time stamps
    /// to <see cref="DateTime"/>.
    /// </summary>
    /// <param name="time">A <see cref="Double"/>.</param>
    /// <returns>A <see cref="DateTime"/>.</returns>
    public static DateTime ConvertDateTimeFromUnixTime(double time) =>
        (new DateTime(1970, 1, 1, 0, 0, 0)).AddSeconds(time);

    /// <summary>
    /// Converts the specified <see cref="DateTime "/> to RFC3339.
    /// </summary>
    /// <param name="utcDateTime">The UTC date and time.</param>
    public static string ConvertDateTimeToRfc3339DateTime(DateTime utcDateTime)
    {
        DateTimeFormatInfo dateTimeFormat = CultureInfo.InvariantCulture.DateTimeFormat;

        var format = (utcDateTime.Kind == DateTimeKind.Local)
                ? "yyyy'-'MM'-'dd'T'HH:mm:ss.ffzzz"
                : "yyyy'-'MM'-'dd'T'HH':'mm':'ss.ff'Z'"
            ;

        return utcDateTime.ToString(format, dateTimeFormat);
    }

    /// <summary>
    /// /// Converts the specified <see cref="DateTime "/> to RFC822.
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
    public static double ConvertDateTimeToUnixTime() =>
        (DateTime.UtcNow
         - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;

    /// <summary>
    /// Converts a <see cref="DateTime"/>
    /// to a Unix time stamp.
    /// </summary>
    /// <param name="dateValue">The <see cref="DateTime"/>.</param>
    /// <returns>A <see cref="Double"/>.</returns>
    public static double ConvertDateTimeToUnixTime(DateTime dateValue) =>
        (dateValue.ToUniversalTime()
         - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;

    /// <summary>
    /// Converts a <see cref="DateTime"/>
    /// to UTC (ISO 8601).
    /// </summary>
    /// <param name="dateValue">The <see cref="DateTime"/>.</param>
    /// <remarks>
    /// For detail, see https://stackoverflow.com/a/1728437/22944.
    /// </remarks>
    public static string ConvertDateTimeToUtc(DateTime dateValue) =>
        dateValue
            .ToUniversalTime()
            .ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");

    /// <summary>
    /// Converts inches as a <see cref="Single"/>
    /// to points.
    /// </summary>
    /// <param name="inches">The inches.</param>
    /// <remarks>
    /// 1 point = 0.013837 inch
    /// </remarks>
    public static float ConvertInchesToPoints(float inches) => inches / 0.01387f;

    /// <summary>
    /// Converts points as a <see cref="Single"/>
    /// to inches.
    /// </summary>
    /// <param name="points">The points.</param>
    /// <remarks>
    /// 1 point = 0.013837 inch
    /// </remarks>
    public static float ConvertPointsToInches(float points) => points * 0.01387f;

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
        var allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?nullable-";
        var chars = new char[passwordLength];
        var r = new Random();

        for (int i = 0; i < passwordLength; i++)
        {
            chars[i] = allowedChars[r.Next(0, allowedChars.Length)];
        }

        return new string(chars);
    }

    /// <summary>
    /// Returns <c>true</c> when the specified value
    /// is <c>null</c> or <see cref="String.Empty"/>.
    /// </summary>
    /// <param name="boxedString">The framework value.</param>
    public static bool IsNullOrEmptyString(object? boxedString) =>
        boxedString == null || string.IsNullOrWhiteSpace(boxedString.ToString());

    /// <summary>
    /// Returns <c>true</c> when the specified value
    /// is an empty array, not an array or null.
    /// </summary>
    /// <param name="boxedArray">The specified value.</param>
    public static bool IsNullOrEmptyOrNotArray(object? boxedArray) =>
        boxedArray is not Array array || (array.Length == 0);

    /// <summary>
    /// Returns the conventional database null
    /// (<see cref="DBNull"/>)
    /// for Microsoft SQL Server systems.
    /// </summary>
    /// <returns><see cref="DBNull"/></returns>
    public static DBNull SqlDatabaseNull() => DBNull.Value;
}
