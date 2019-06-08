using Songhay.Extensions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Songhay.Tests.Extensions
{

    public class TaskExtensionsTest
    {
        public void ShouldDelayByOneSecond()
        {
            var thePast = DateTime.Now;

            Task delayTask = null;
            delayTask = delayTask.Delay(TimeSpan.FromSeconds(1), i =>
            {
                var test = DateTime.Now.Second - thePast.Second >= 1;
                Assert.True(test, "The expected delay did not occur.");
            });
        }
    }
}
