using System;
using System.Collections.Generic;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="System.TimeSpan"/>.
    /// </summary>
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// Lists the days for the specified <see cref="System.TimeSpan"/>.
        /// </summary>
        /// <param name="span">The span.</param>
        public static IList<DateTime> ListDays(this TimeSpan span)
        {
            return span.ListDays(DateTime.Now);
        }

        /// <summary>
        /// Lists the days for the specified <see cref="System.TimeSpan"/>
        /// from the specified start <see cref="System.DateTime"/>.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="startDate">The start date.</param>
        public static IList<DateTime> ListDays(this TimeSpan span, DateTime startDate)
        {
            if(span == null) return null;

            var days = new List<DateTime>(span.Days);
            for(int i = 0; i < span.Days; i++)
            {
                days.Add(startDate.AddDays(i));
            }
            return days;
        }
    }
}
