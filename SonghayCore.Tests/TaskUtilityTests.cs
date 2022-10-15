namespace Songhay.Tests;

public class TaskUtilityTests
{
    [Fact]
    public void ShouldDelayByOneSecond()
    {
        var thePast = DateTime.Now;

        TaskUtility.Delay(TimeSpan.FromSeconds(1), task =>
        {
            Assert.Equal(TaskStatus.RanToCompletion, task.Status);

            var test = DateTime.Now.Second - thePast.Second >= 1;
            Assert.True(test, "The expected delay did not occur.");
        });
    }
}
