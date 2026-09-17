namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="DateTime"/>.
/// </summary>
public static partial class DateTimeExtensions
{
    /// <summary>
    /// Gets the next weekday.
    /// </summary>
    /// <param name="start">The start.</param>
    /// <param name="day">The day.</param>
    /// <remarks>
    /// by Jon Skeet
    ///
    /// For more detail, see:
    /// http://stackoverflow.com/questions/6346119/asp-net-get-the-next-tuesday
    /// </remarks>
    public static DateTime GetNextWeekday(this DateTime start, DayOfWeek day)
    {
        // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
        int daysToAdd = ((int) day - (int) start.DayOfWeek + 7) % 7;

        return start.AddDays(daysToAdd);
    }

    /// <summary>
    /// Converts the specified <see cref="DateTime"/>
    /// to the JSON-friendly ISO_8601 text format
    /// without milliseconds.
    /// </summary>
    /// <param name="dateTime">the <see cref="DateTime"/></param>
    public static string ToIso8601String(this DateTime dateTime) =>
        dateTime.ToIso8601String(includeTimeMilliseconds: false);

    /// <summary>
    /// Converts the specified <see cref="DateTime"/>
    /// to the JSON-friendly ISO_8601 text format.
    /// </summary>
    /// <param name="dateTime">the <see cref="DateTime"/></param>
    /// <param name="includeTimeMilliseconds">when <c>true</c>, increase resolution to include milliseconds</param>
    /// <remarks>
    /// 📖 https://en.wikipedia.org/wiki/ISO_8601
    /// </remarks>
    public static string ToIso8601String(this DateTime dateTime, bool includeTimeMilliseconds)
    {
        if(dateTime.Kind == DateTimeKind.Utc)
            return dateTime.ToIso8601UtcString(includeTimeMilliseconds);

        string template = includeTimeMilliseconds?
            Iso8601TemplateWithMs
            :
            Iso8601Template;

        return dateTime.ToString(template);
    }

    /// <summary>
    /// Converts the specified <see cref="DateTime"/>
    /// to the JSON-friendly ISO_8601 text format for UTC (a trailing <c>Z</c>)
    /// without milliseconds.
    /// </summary>
    /// <param name="dateTime">the <see cref="DateTime"/></param>
    public static string ToIso8601UtcString(this DateTime dateTime) =>
        dateTime.ToIso8601UtcString(includeTimeMilliseconds: false);

    /// <summary>
    /// Converts the specified <see cref="DateTime"/>
    /// to the JSON-friendly ISO_8601 text format for UTC (a trailing <c>Z</c>).
    /// </summary>
    /// <param name="dateTime">the <see cref="DateTime"/></param>
    /// <param name="includeTimeMilliseconds">when <c>true</c>, increase resolution to include milliseconds</param>
    /// <remarks>
    /// 📖 https://en.wikipedia.org/wiki/ISO_8601
    /// </remarks>
    public static string ToIso8601UtcString(this DateTime dateTime, bool includeTimeMilliseconds)
    {
        string template = includeTimeMilliseconds?
            $"{Iso8601TemplateWithMs}'Z'"
            :
            $"{Iso8601Template}'Z'";

        return dateTime.ToUniversalTime().ToString(template);
    }

    internal const string Iso8601Template = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
    internal const string Iso8601TemplateWithMs = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.'fff";
}
