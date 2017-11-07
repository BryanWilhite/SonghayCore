using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Songhay.Extensions.Tests
{
    using Songhay.Extensions;

    /// <summary>
    /// Tests for extensions of <see cref="System.TimeSpan"/>.
    /// </summary>
    [TestClass]
    public class TimeSpanExtensionsTest
    {
        [TestMethod]
        public void ShouldFindFridaysInTimeSpan()
        {
            //reference: http://stackoverflow.com/questions/248273/count-number-of-mondays-in-a-given-date-range

            var spanOfSixtyDays = new TimeSpan(60, 0, 0, 0);
            var setOfDates = spanOfSixtyDays.ListDays(DateTime.Now);

            Assert.IsTrue(setOfDates.Count == 60,
                "The expected number of days is not here.");

            var fridays = setOfDates.Where(i => i.DayOfWeek == DayOfWeek.Friday);

            Assert.IsTrue(fridays.Count() > 0,
                "The expected Friday days are not here.");
            Assert.IsTrue(fridays.First() == setOfDates.First(i => i.DayOfWeek == DayOfWeek.Friday),
                "The expected first Friday day is not here.");
            Assert.IsTrue(fridays.Last() == setOfDates.Last(i => i.DayOfWeek == DayOfWeek.Friday),
                "The expected last Friday day is not here.");
        }
    }
}
