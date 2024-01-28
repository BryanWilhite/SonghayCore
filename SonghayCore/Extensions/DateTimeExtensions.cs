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
}
