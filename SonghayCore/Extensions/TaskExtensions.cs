using System;
using System.Threading;
using System.Threading.Tasks;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="Task"/>
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Delays with a <see cref="Timer"/> task for the specified <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="task">Instance of </param>
        /// <param name="timeSpan">The specified timespan</param>
        /// <param name="actionAfterDelay">The continuation action</param>
        public static Task Delay(this Task task, TimeSpan timeSpan, Action<Task> actionAfterDelay)
        {
            return task.Delay(timeSpan, actionAfterDelay, null);
        }

        /// <summary>
        /// Delays with a <see cref="Timer"/> task for the specified <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="task">Instance of </param>
        /// <param name="timeSpan">The specified timespan</param>
        /// <param name="actionAfterDelay">The continuation action</param>
        /// <param name="schedulerAfterDelay">Work-queue scheduler</param>
        /// <remarks>
        /// The syntax to get this running may seem a bit strange:
        ///
        ///     this._delayTask = this._delayTask.Delay(TimeSpan.FromSeconds(1), i =>
        ///     {
        ///         //do stuff after one second…
        ///     });
        /// 
        /// This is done to both initialize the Task and then return its reference until the Task is completed.
        ///
        /// </remarks>
        public static Task Delay(this Task task, TimeSpan timeSpan, Action<Task> actionAfterDelay, TaskScheduler schedulerAfterDelay)
        {
            if (task != null && !task.IsCompleted) return task;
            task = Delay(timeSpan);
            if ((actionAfterDelay != null) && (schedulerAfterDelay != null)) task.ContinueWith(actionAfterDelay, schedulerAfterDelay);
            else if (actionAfterDelay != null) task.ContinueWith(actionAfterDelay);
            return task;
        }

        static Task Delay(TimeSpan timeSpan)
        {
            var tcs = new TaskCompletionSource<bool>();
            new Timer(_ => tcs.SetResult(true)).Change(timeSpan, new TimeSpan(-1));
            return tcs.Task;
        }
    }
}
