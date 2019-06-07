#if NET452 || NET462

using System;
using System.ComponentModel;

namespace Songhay.ComponentModel
{
    /// <summary>
    /// Static members
    /// for <see cref="System.ComponentModel.BackgroundWorker"/>.
    /// </summary>
    [Obsolete("Since .NET 4, use System.Threading.Tasks (TPL)")]
    public static class BackgroundWorkerUtility
    {
        /// <summary>
        /// Encapsulates a thread of work that cannot be canceled.
        /// </summary>
        /// <param name="workAction">The work action.</param>
        /// <param name="workCompleteAction">The work complete action.</param>
        public static void DoWork(Action<object, DoWorkEventArgs> workAction,
            Action<object, RunWorkerCompletedEventArgs> workCompleteAction)
        {
            BackgroundWorkerUtility.DoWork(workAction, workCompleteAction, null);
        }

        /// <summary>
        /// Does the work.
        /// </summary>
        /// <param name="workAction">The work action.</param>
        /// <param name="workCompleteAction">The work complete action.</param>
        /// <param name="workProgressAction">The work progress action.</param>
        public static void DoWork(Action<object, DoWorkEventArgs> workAction,
            Action<object, RunWorkerCompletedEventArgs> workCompleteAction,
            Action<object, ProgressChangedEventArgs> workProgressAction)
        {
            if(workAction == null) throw new ArgumentNullException("workAction", "The work Action is null.");
            if(workCompleteAction == null) throw new ArgumentNullException("workCompleteAction", "The work complete Action is null.");

            var worker = new BackgroundWorker();

            worker.DoWork += (o, args) => workAction.Invoke(o, args);

            worker.WorkerReportsProgress = (workProgressAction != null);

            if(workProgressAction != null)
                worker.ProgressChanged += (o, args) =>
                    workProgressAction.Invoke(o, args);

            worker.RunWorkerCompleted += (o, args) =>
            {
                try
                {
                    workCompleteAction.Invoke(o, args);
                }
                finally
                {
                    worker.Dispose();
                }
            };

            worker.RunWorkerAsync();
        }
    }
}

#endif
