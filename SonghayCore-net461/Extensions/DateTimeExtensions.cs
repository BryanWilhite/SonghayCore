using System;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="System.DateTime"/>.
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
            int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            return start.AddDays(daysToAdd);
        }

        /// <summary>
        /// Converts the <see cref="DateTime"/> into a “pretty” date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <remarks>
        /// This member was taken from NBlog developer, Chris Fulstow.
        /// [https://github.com/ChrisFulstow/NBlog/blob/master/NBlog.Web/Application/Infrastructure/DateExtensions.cs]
        /// </remarks>
        public static string ToPrettyDate(this DateTime date)
        {
            var timeSince = DateTime.Now.Subtract(date);
            if (timeSince.TotalMilliseconds < 1) return "not yet";
            if (timeSince.TotalMinutes < 1) return "just now";
            if (timeSince.TotalMinutes < 2) return "1 minute ago";
            if (timeSince.TotalMinutes < 60) return string.Format("{0} minutes ago", timeSince.Minutes);
            if (timeSince.TotalMinutes < 120) return "1 hour ago";
            if (timeSince.TotalHours < 24) return string.Format("{0} hours ago", timeSince.Hours);
            if (timeSince.TotalDays == 1) return "yesterday";
            if (timeSince.TotalDays < 7) return string.Format("{0} day(s) ago", timeSince.Days);
            if (timeSince.TotalDays < 14) return "last week";
            if (timeSince.TotalDays < 21) return "2 weeks ago";
            if (timeSince.TotalDays < 28) return "3 weeks ago";
            if (timeSince.TotalDays < 60) return "last month";
            if (timeSince.TotalDays < 365) return string.Format("{0} months ago", Math.Round(timeSince.TotalDays / 30));
            if (timeSince.TotalDays < 730) return "last year";
            return string.Format("{0} years ago", Math.Round(timeSince.TotalDays / 365));
        }
    }
}
