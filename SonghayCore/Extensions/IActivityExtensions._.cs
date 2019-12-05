using Songhay.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="IActivity"/>
    /// </summary>
    public static partial class IActivityExtensions
    {
        /// <summary>
        /// Starts the <see cref="IActivity"/>.
        /// </summary>
        /// <param name="activity">The activity.</param>
        /// <param name="args">The arguments.</param>
        /// <param name="traceSource">The trace source.</param>
        /// <returns>The trace source log.</returns>
        public static string StartActivity(this IActivity activity, ProgramArgs args, TraceSource traceSource)
        {
            using (var writer = new StringWriter())
            using (var listener = new TextWriterTraceListener(writer))
            {
                traceSource?.Listeners.Add(listener);

                string log = null;

                try
                {
                    activity.Start(args);
                }
                finally
                {
                    listener.Flush();
                    log = writer.ToString();
                }

                return log;
            }
        }

        /// <summary>
        /// Starts the <see cref="IActivity"/>, asynchronously.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="activity">The activity.</param>
        /// <param name="input">The input.</param>
        /// <param name="traceSource">The trace source.</param>
        /// <returns><see cref="ActivityOutput{TOutput}"/></returns>
        /// <exception cref="NullReferenceException">The expected {nameof(IActivity)} is not here.</exception>
        public static async Task<ActivityOutput<TOutput>> StartActivityAsync<TInput, TOutput>(this IActivity activity, TInput input, TraceSource traceSource)
        {
            if (activity == null) throw new NullReferenceException($"The expected {nameof(IActivity)} is not here.");

            using (var writer = new StringWriter())
            using (var listener = new TextWriterTraceListener(writer))
            {
                traceSource?.Listeners.Add(listener);

                ActivityOutput<TOutput> thing = null;

                try
                {
                    var activityWithOutput = activity.ToActivityWithOutput<TInput, TOutput>();
                    thing.Output = await activityWithOutput.StartAsync(input);
                }
                finally
                {
                    listener.Flush();
                    thing.Log = writer.ToString();
                }

                return thing;
            }
        }

        /// <summary>
        /// Converts the specified <see cref="IActivity" />
        /// to <see cref="IActivityWithOutput{TInput,TOutput}" />.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="activity">The activity.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">The expected Activity name is not here.
        /// or</exception>
        public static IActivityWithOutput<TInput, TOutput> ToActivityWithOutput<TInput, TOutput>(this IActivity activity)
        {
            return activity.ToActivityWithOutput<TInput, TOutput>(throwException: true);
        }

        /// <summary>
        /// Converts the specified <see cref="IActivity" />
        /// to <see cref="IActivityWithOutput{TInput,TOutput}" />.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="activity">The activity.</param>
        /// <param name="throwException">if set to <c>true</c> throw exception.</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException">The expected IActivityOutput{TInput, TOutput} is not here.</exception>
        /// <exception cref="ArgumentNullException">The expected Activity name is not here.
        /// or</exception>
        public static IActivityWithOutput<TInput, TOutput> ToActivityWithOutput<TInput, TOutput>(this IActivity activity, bool throwException)
        {
            if (activity == null) return null;

            var output = activity as IActivityWithOutput<TInput, TOutput>;

            if (throwException && (output == null))
                throw new NullReferenceException($"The expected {nameof(IActivityWithOutput<TInput, TOutput>)} is not here.");

            return output;
        }
    }
}