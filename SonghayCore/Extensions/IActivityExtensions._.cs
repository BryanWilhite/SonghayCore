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

                var activityOutput = new ActivityOutput<TOutput>();

                try
                {
                    var activityWithOutput = activity.ToActivityWithTask<TInput, TOutput>();
                    activityOutput.Output = await activityWithOutput
                        .StartAsync(input)
                        .ConfigureAwait(continueOnCapturedContext: false);
                }
                finally
                {
                    listener.Flush();
                    activityOutput.Log = writer.ToString();
                }

                return activityOutput;
            }
        }

        /// <summary>
        /// Starts the <see cref="IActivity"/>, asynchronously.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <param name="activity">The activity.</param>
        /// <param name="input">The input.</param>
        /// <param name="traceSource">The trace source.</param>
        /// <returns>The trace source log.</returns>
        /// <exception cref="NullReferenceException">The expected {nameof(IActivity)} is not here.</exception>
        public static async Task<string> StartActivityAsync<TInput>(this IActivity activity, TInput input, TraceSource traceSource)
        {
            if (activity == null) throw new NullReferenceException($"The expected {nameof(IActivity)} is not here.");

            using (var writer = new StringWriter())
            using (var listener = new TextWriterTraceListener(writer))
            {
                traceSource?.Listeners.Add(listener);

                var log = string.Empty;

                try
                {
                    var activityWithTask = activity.ToActivityWithTask<TInput>();
                    await activityWithTask
                        .StartAsync(input)
                        .ConfigureAwait(continueOnCapturedContext: false);
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
        /// <param name="activity">The activity.</param>
        /// <param name="traceSource">The trace source.</param>
        /// <returns>The trace source log.</returns>
        /// <exception cref="NullReferenceException">The expected {nameof(IActivity)} is not here.</exception>
        public static async Task<string> StartActivityAsync(this IActivity activity, TraceSource traceSource)
        {
            if (activity == null) throw new NullReferenceException($"The expected {nameof(IActivity)} is not here.");

            using (var writer = new StringWriter())
            using (var listener = new TextWriterTraceListener(writer))
            {
                traceSource?.Listeners.Add(listener);

                var log = string.Empty;

                try
                {
                    var activityWithTask = activity.ToActivityWithTask();
                    await activityWithTask
                        .StartAsync()
                        .ConfigureAwait(continueOnCapturedContext: false);
                }
                finally
                {
                    listener.Flush();
                    log = writer.ToString();
                }

                return log;
            }
        }

#if NET5_0

        /// <summary>
        /// Starts the <see cref="IActivity"/>
        /// with <see cref="ConsoleTraceListener"/>.
        /// </summary>
        /// <param name="activity">The activity.</param>
        /// <param name="args">The arguments.</param>
        /// <param name="traceSource">The trace source.</param>
        /// <returns>The trace source log.</returns>
        public static void StartConsoleActivity(this IActivity activity, ProgramArgs args, TraceSource traceSource)
        {
            using (var listener = new ConsoleTraceListener())
            {
                traceSource?.Listeners.Add(listener);

                try
                {
                    activity.Start(args);
                }
                finally
                {
                    listener.Flush();
                }
            }
        }

        /// <summary>
        /// Starts the <see cref="IActivity"/>, asynchronously
        /// with <see cref="ConsoleTraceListener"/>.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <param name="activity">The activity.</param>
        /// <param name="input">The input.</param>
        /// <param name="traceSource">The trace source.</param>
        /// <returns>The trace source log.</returns>
        /// <exception cref="NullReferenceException">The expected {nameof(IActivity)} is not here.</exception>
        public static async Task StartConsoleActivityAsync<TInput>(this IActivity activity, TInput input, TraceSource traceSource)
        {
            if (activity == null) throw new NullReferenceException($"The expected {nameof(IActivity)} is not here.");

            using (var listener = new ConsoleTraceListener())
            {
                traceSource?.Listeners.Add(listener);

                try
                {
                    var activityWithTask = activity.ToActivityWithTask<TInput>();
                    await activityWithTask
                        .StartAsync(input)
                        .ConfigureAwait(continueOnCapturedContext: false);
                }
                finally
                {
                    listener.Flush();
                }
            }
        }

        /// <summary>
        /// Starts the <see cref="IActivity"/>, asynchronously
        /// with <see cref="ConsoleTraceListener"/>.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="activity">The activity.</param>
        /// <param name="input">The input.</param>
        /// <param name="traceSource">The trace source.</param>
        /// <returns>The trace source log.</returns>
        /// <exception cref="NullReferenceException">The expected {nameof(IActivity)} is not here.</exception>
        public static async Task<TOutput> StartConsoleActivityAsync<TInput, TOutput>(this IActivity activity, TInput input, TraceSource traceSource)
        {
            if (activity == null) throw new NullReferenceException($"The expected {nameof(IActivity)} is not here.");

            using (var listener = new ConsoleTraceListener())
            {
                traceSource?.Listeners.Add(listener);

                TOutput output = default(TOutput);

                try
                {
                    var activityWithOutput = activity.ToActivityWithTask<TInput, TOutput>();

                    output = await activityWithOutput
                        .StartAsync(input)
                        .ConfigureAwait(continueOnCapturedContext: false);
                }
                finally
                {
                    listener.Flush();
                }

                return output;
            }
        }

#endif
    }
}