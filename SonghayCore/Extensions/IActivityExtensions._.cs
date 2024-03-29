﻿namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="IActivity"/>
/// </summary>
// ReSharper disable once InconsistentNaming
public static partial class IActivityExtensions
{
    /// <summary>
    /// Starts the <see cref="IActivity"/>.
    /// </summary>
    /// <param name="activity">The activity.</param>
    /// <param name="args">The arguments.</param>
    /// <param name="traceSource">The trace source.</param>
    /// <returns>The <see cref="TraceSource"/> log.</returns>
    public static string? StartActivity(this IActivity? activity, ProgramArgs? args, TraceSource? traceSource) =>
        activity.StartActivity(args, traceSource, traceWriterGetter: null, flushLog: true);

    /// <summary>
    /// Starts the <see cref="IActivity"/>.
    /// </summary>
    /// <param name="activity">The activity.</param>
    /// <param name="args">The arguments.</param>
    /// <param name="traceSource">The trace source.</param>
    /// <param name="traceWriterGetter">gets the <see cref="TextWriter"/> for the <see cref="TraceSource"/>.</param>
    /// <param name="flushLog">when <c>true</c> return <see cref="TraceSource"/> log</param>
    /// <returns>The <see cref="TraceSource"/> log when <c>flushLog</c> is <c>true</c>.</returns>
    public static string? StartActivity(this IActivity? activity, ProgramArgs? args, TraceSource? traceSource,
        Func<TextWriter>? traceWriterGetter, bool flushLog)
    {
        ArgumentNullException.ThrowIfNull(activity);
        ArgumentNullException.ThrowIfNull(args);

        using var writer = traceWriterGetter?.Invoke() ?? new StringWriter();
        using var listener = new TextWriterTraceListener(writer);

        traceSource?.Listeners.Add(listener);

        string? log = null;

        try
        {
            activity.Start(args);
        }
        finally
        {
            if (flushLog)
            {
                listener.Flush();
                log = writer.ToString();
            }
        }

        return log;
    }

    /// <summary>
    /// Starts the <see cref="IActivity"/>
    /// for the specified output (<typeparamref name="TOutput"/>).
    /// </summary>
    /// <param name="activity">The activity.</param>
    /// <param name="input">The input.</param>
    /// <param name="traceSource">The trace source.</param>
    /// <returns><see cref="ActivityOutput{TOutput}"/></returns>
    public static ActivityOutput<TOutput> StartActivityForOutput<TInput, TOutput>(this IActivity? activity,
        TInput input, TraceSource? traceSource) =>
        activity.StartActivityForOutput<TInput, TOutput>(input, traceSource, traceWriterGetter: null,
            flushLog: true);

    /// <summary>
    /// Starts the <see cref="IActivity"/>
    /// for the specified output (<typeparamref name="TOutput"/>).
    /// </summary>
    /// <param name="activity">The activity.</param>
    /// <param name="input">The input.</param>
    /// <param name="traceSource">The trace source.</param>
    /// <param name="traceWriterGetter">gets the <see cref="TextWriter"/> for the <see cref="TraceSource"/>.</param>
    /// <param name="flushLog">when <c>true</c> return <see cref="TraceSource"/> log</param>
    /// <returns>The <see cref="TraceSource"/> log when <c>flushLog</c> is <c>true</c>.</returns>
    public static ActivityOutput<TOutput> StartActivityForOutput<TInput, TOutput>(this IActivity? activity,
        TInput input, TraceSource? traceSource, Func<TextWriter>? traceWriterGetter, bool flushLog)
    {
        using var writer = traceWriterGetter?.Invoke() ?? new StringWriter();
        using var listener = new TextWriterTraceListener(writer);

        traceSource?.Listeners.Add(listener);

        var activityOutput = new ActivityOutput<TOutput>();

        try
        {
            var activityWithOutput = activity.ToActivityWithOutput<TInput, TOutput>();

            activityOutput.Output = activityWithOutput!.StartForOutput(input);
        }
        finally
        {
            if (flushLog)
            {
                listener.Flush();
                var log = writer.ToString();
                activityOutput.Log = log;
            }
        }

        return activityOutput;
    }

    /// <summary>
    /// Starts the <see cref="IActivity"/>, asynchronously.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    /// <param name="activity">The activity.</param>
    /// <param name="input">The input.</param>
    /// <param name="traceSource">The <see cref="TraceSource"/>.</param>
    /// <returns><see cref="ActivityOutput{TOutput}"/></returns>
    public static async Task<ActivityOutput<TOutput>> StartActivityAsync<TInput, TOutput>(this IActivity? activity,
        TInput input, TraceSource? traceSource) =>
        await activity
            .StartActivityAsync<TInput, TOutput>(input, traceSource, traceWriterGetter: null)
            .ConfigureAwait(continueOnCapturedContext: false);

    /// <summary>
    /// Starts the <see cref="IActivity"/>, asynchronously.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    /// <param name="activity">The activity.</param>
    /// <param name="input">The input.</param>
    /// <param name="traceSource">The <see cref="TraceSource"/>.</param>
    /// <param name="traceWriterGetter">gets the <see cref="TextWriter"/> for the <see cref="TraceSource"/>.</param>
    /// <returns><see cref="ActivityOutput{TOutput}"/></returns>
    public static async Task<ActivityOutput<TOutput>> StartActivityAsync<TInput, TOutput>(this IActivity? activity,
        TInput input, TraceSource? traceSource, Func<TextWriter>? traceWriterGetter)
    {
        ArgumentNullException.ThrowIfNull(activity);

        await using var writer = traceWriterGetter?.Invoke() ?? new StringWriter();
        using var listener = new TextWriterTraceListener(writer);

        traceSource?.Listeners.Add(listener);

        var activityOutput = new ActivityOutput<TOutput>();

        try
        {
            var activityWithOutput = activity.ToActivityWithTask<TInput, TOutput>();
            activityOutput.Output = await activityWithOutput!
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

    /// <summary>
    /// Starts the <see cref="IActivity"/>, asynchronously.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <param name="activity">The activity.</param>
    /// <param name="input">The input.</param>
    /// <param name="traceSource">The <see cref="TraceSource"/>.</param>
    /// <returns>The <see cref="TraceSource"/> log.</returns>
    public static async Task<string?> StartActivityAsync<TInput>(this IActivity? activity, TInput input,
        TraceSource? traceSource) =>
        await activity
            .StartActivityAsync(input, traceSource, traceWriterGetter: null, flushLog: true)
            .ConfigureAwait(continueOnCapturedContext: false);

    /// <summary>
    /// Starts the <see cref="IActivity"/>, asynchronously.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <param name="activity">The activity.</param>
    /// <param name="input">The input.</param>
    /// <param name="traceSource">The <see cref="TraceSource"/>.</param>
    /// <param name="traceWriterGetter">gets the <see cref="TextWriter"/> for the <see cref="TraceSource"/>.</param>
    /// <param name="flushLog">when <c>true</c> return <see cref="TraceSource"/> log</param>
    /// <returns>The <see cref="TraceSource"/> log when <c>flushLog</c> is <c>true</c>.</returns>
    public static async Task<string?> StartActivityAsync<TInput>(this IActivity? activity, TInput input,
        TraceSource? traceSource, Func<TextWriter>? traceWriterGetter, bool flushLog)
    {
        ArgumentNullException.ThrowIfNull(activity);

        await using var writer = traceWriterGetter?.Invoke() ?? new StringWriter();
        using var listener = new TextWriterTraceListener(writer);

        traceSource?.Listeners.Add(listener);

        string? log = null;

        try
        {
            var activityWithTask = activity.ToActivityWithTask<TInput>();
            await activityWithTask!
                .StartAsync(input)
                .ConfigureAwait(continueOnCapturedContext: false);
        }
        finally
        {
            if (flushLog)
            {
                listener.Flush();
                log = writer.ToString();
            }
        }

        return log;
    }

    /// <summary>
    /// Starts the <see cref="IActivity"/>, asynchronously.
    /// </summary>
    /// <param name="activity">The activity.</param>
    /// <param name="traceSource">The <see cref="TraceSource"/>.</param>
    /// <returns>The <see cref="TraceSource"/> log.</returns>
    public static async Task<string?> StartActivityAsync(this IActivity? activity, TraceSource? traceSource) =>
        await activity
            .StartActivityAsync(traceSource, traceWriterGetter: null, flushLog: true)
            .ConfigureAwait(continueOnCapturedContext: false);

    /// <summary>
    /// Starts the <see cref="IActivity"/>, asynchronously.
    /// </summary>
    /// <param name="activity">The activity.</param>
    /// <param name="traceSource">The <see cref="TraceSource"/>.</param>
    /// <param name="traceWriterGetter">gets the <see cref="TextWriter"/> for the <see cref="TraceSource"/>.</param>
    /// <param name="flushLog">when <c>true</c> return <see cref="TraceSource"/> log</param>
    /// <returns>The <see cref="TraceSource"/> log when <c>flushLog</c> is <c>true</c>; otherwise, <c>null</c>.</returns>
    public static async Task<string?> StartActivityAsync(this IActivity? activity, TraceSource? traceSource,
        Func<TextWriter>? traceWriterGetter, bool flushLog)
    {
        ArgumentNullException.ThrowIfNull(activity);

        await using var writer = traceWriterGetter?.Invoke() ?? new StringWriter();
        using var listener = new TextWriterTraceListener(writer);

        traceSource?.Listeners.Add(listener);

        string? log = null;

        try
        {
            var activityWithTask = activity.ToActivityWithTask();
            await activityWithTask!
                .StartAsync()
                .ConfigureAwait(continueOnCapturedContext: false);
        }
        finally
        {
            if (flushLog)
            {
                listener.Flush();
                log = writer.ToString();
            }
        }

        return log;
    }
}
