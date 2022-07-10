namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="IActivity"/>
/// </summary>
// ReSharper disable once InconsistentNaming
public static partial class IActivityExtensions
{
    /// <summary>
    /// Starts the <see cref="IActivity"/>
    /// with <see cref="ConsoleTraceListener"/>.
    /// </summary>
    /// <param name="activity">The activity.</param>
    /// <param name="args">The arguments.</param>
    /// <param name="traceSource">The the <see cref="TraceSource"/>.</param>
    /// <returns>The the <see cref="TraceSource"/> log.</returns>
    public static void StartConsoleActivity(this IActivity? activity, ProgramArgs args, TraceSource? traceSource)
    {
        using var listener = new ConsoleTraceListener();

        traceSource?.Listeners.Add(listener);

        try
        {
            activity?.Start(args);
        }
        finally
        {
            listener.Flush();
        }
    }

    /// <summary>
    /// Starts the <see cref="IActivity"/>, asynchronously
    /// with <see cref="ConsoleTraceListener"/>.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <param name="activity">The activity.</param>
    /// <param name="input">The input.</param>
    /// <param name="traceSource">The the <see cref="TraceSource"/>.</param>
    /// <returns>The the <see cref="TraceSource"/> log.</returns>
    public static async Task StartConsoleActivityAsync<TInput>(this IActivity? activity, TInput? input,
        TraceSource? traceSource)
    {
        ArgumentNullException.ThrowIfNull(activity);

        using var listener = new ConsoleTraceListener();

        traceSource?.Listeners.Add(listener);

        var activityWithTask = activity.ToActivityWithTask<TInput>();
        try
        {
            await activityWithTask!
                .StartAsync(input)
                .ConfigureAwait(continueOnCapturedContext: false);
        }
        finally
        {
            listener.Flush();
        }
    }

    /// <summary>
    /// Starts the <see cref="IActivity"/>, asynchronously
    /// with the specified <see cref="ConsoleTraceListener"/>.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    /// <param name="activity">The activity.</param>
    /// <param name="input">The input.</param>
    /// <param name="traceSource">The the <see cref="TraceSource"/>.</param>
    /// <returns>The the <see cref="TraceSource"/> log.</returns>
    public static async Task<TOutput?> StartConsoleActivityAsync<TInput, TOutput>(this IActivity? activity,
        TInput? input, TraceSource? traceSource)
    {
        ArgumentNullException.ThrowIfNull(activity);

        using var listener = new ConsoleTraceListener();

        traceSource?.Listeners.Add(listener);

        var activityWithTask = activity.ToActivityWithTask<TInput, TOutput>();
        TOutput? output;
        try
        {

            output = await activityWithTask!
                .StartAsync(input)
                .ConfigureAwait(continueOnCapturedContext: false);
        }
        finally
        {
            listener.Flush();
        }

        return output;
    }

    /// <summary>
    /// Starts the <see cref="IActivity"/>, synchronously
    /// with the specified <see cref="ConsoleTraceListener"/>.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    /// <param name="activity">The activity.</param>
    /// <param name="input">The input.</param>
    /// <param name="traceSource">The the <see cref="TraceSource"/>.</param>
    /// <returns>The the <see cref="TraceSource"/> log.</returns>
    public static TOutput? StartConsoleActivityForOutput<TInput, TOutput>(this IActivity? activity, TInput? input,
        TraceSource? traceSource)
    {
        ArgumentNullException.ThrowIfNull(activity);

        using var listener = new ConsoleTraceListener();

        traceSource?.Listeners.Add(listener);

        var activityWithOutput = activity.ToActivityWithOutput<TInput, TOutput>();
        TOutput? output;
        try
        {

            output = activityWithOutput!.StartForOutput(input);
        }
        finally
        {
            listener.Flush();
        }

        return output;
    }
}
