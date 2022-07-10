using Songhay.Extensions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Songhay.Tests.Extensions;

public class TaskExtensionsTest
{
    [Fact]
    public void ShouldDelayByOneSecond()
    {
        var thePast = DateTime.Now;

        Task delayTask = Task.FromResult(() => {});

        delayTask.Delay(TimeSpan.FromSeconds(1), task =>
        {
            Assert.Equal(TaskStatus.RanToCompletion, task.Status);

            var test = DateTime.Now.Second - thePast.Second >= 1;
            Assert.True(test, "The expected delay did not occur.");
        });
    }
}
