namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="TimeSpan"/>.
/// </summary>
public static class TimeSpanExtensions
{
    /// <summary>
    /// Lists the days for the specified <see cref="TimeSpan"/>.
    /// </summary>
    /// <param name="span">The <see cref="TimeSpan" />.</param>
    public static IList<DateTime> ListDays(this TimeSpan span) => span.ListDays(DateTime.Now);

    /// <summary>
    /// Lists the days for the specified <see cref="TimeSpan"/>
    /// from the specified start <see cref="DateTime"/>.
    /// </summary>
    /// <param name="span">The <see cref="TimeSpan" />.</param>
    /// <param name="startDate">The start date.</param>
    public static IList<DateTime> ListDays(this TimeSpan span, DateTime startDate)
    {
        var days = new List<DateTime>(span.Days);

        for (int i = 0; i < span.Days; i++)
        {
            days.Add(startDate.AddDays(i));
        }

        return days;
    }
}
