namespace Songhay;

/// <summary>
/// Members for <see cref="Task"/>.
/// </summary>
public static class TaskUtility
{
    /// <summary>
    /// Delays with a <see cref="Timer"/> task for the specified <see cref="TimeSpan"/>.
    /// </summary>
    /// <param name="timeSpan">The specified <see cref="TimeSpan"/>.</param>
    /// <param name="actionAfterDelay">The continuation action.</param>
    public static Task Delay(TimeSpan timeSpan, Action<Task>? actionAfterDelay) =>
        Delay(timeSpan, actionAfterDelay, null);

    /// <summary>
    /// Delays with a <see cref="Timer"/> task for the specified <see cref="TimeSpan"/>.
    /// </summary>
    /// <param name="timeSpan">The specified <see cref="TimeSpan"/>.</param>
    /// <param name="actionAfterDelay">The continuation action.</param>
    /// <param name="schedulerAfterDelay">The work-queue scheduler.</param>
    public static Task Delay(TimeSpan timeSpan, Action<Task>? actionAfterDelay,
        TaskScheduler? schedulerAfterDelay)
    {
        var task = Delay(timeSpan);

        if (actionAfterDelay != null && schedulerAfterDelay != null)
        {
            task.ContinueWith(actionAfterDelay, schedulerAfterDelay);
        }
        else if (actionAfterDelay != null)
        {
            task.ContinueWith(actionAfterDelay);
        }

        return task;
    }

    static Task Delay(TimeSpan timeSpan)
    {
        var tcs = new TaskCompletionSource<bool>();
        new Timer(_ => tcs.SetResult(true)).Change(timeSpan, new TimeSpan(-1));
        return tcs.Task;
    }
}
