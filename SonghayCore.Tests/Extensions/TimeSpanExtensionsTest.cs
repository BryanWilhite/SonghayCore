using System;
using System.Linq;
using Xunit;

namespace Songhay.Extensions.Tests;

public class TimeSpanExtensionsTest
{
    public void ShouldFindFridaysInTimeSpan()
    {
        //reference: http://stackoverflow.com/questions/248273/count-number-of-mondays-in-a-given-date-range

        var spanOfSixtyDays = new TimeSpan(60, 0, 0, 0);
        var setOfDates = spanOfSixtyDays.ListDays(DateTime.Now);

        Assert.True(setOfDates.Count == 60,
            "The expected number of days is not here.");

        var fridays = setOfDates.Where(i => i.DayOfWeek == DayOfWeek.Friday);

        Assert.True(fridays.Count() > 0,
            "The expected Friday days are not here.");
        Assert.True(fridays.First() == setOfDates.First(i => i.DayOfWeek == DayOfWeek.Friday),
            "The expected first Friday day is not here.");
        Assert.True(fridays.Last() == setOfDates.Last(i => i.DayOfWeek == DayOfWeek.Friday),
            "The expected last Friday day is not here.");
    }
}