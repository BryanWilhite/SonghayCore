namespace Songhay.Extensions;

// ReSharper disable once InconsistentNaming
public static partial class IActivityExtensions
{
    /// <summary>
    /// Converts the specified <see cref="IActivity" />
    /// to <see cref="IActivityWithOutput{TInput,TOutput}" />.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    /// <param name="activity">The activity.</param>
    public static IActivityWithOutput<TInput?, TOutput?>?
        ToActivityWithOutput<TInput, TOutput>(this IActivity? activity) =>
        activity as IActivityWithOutput<TInput?, TOutput?>;

    /// <summary>
    /// Converts the specified <see cref="IActivity" />
    /// to <see cref="IActivityWithTask{TInput,TOutput}" />.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    /// <param name="activity">The activity.</param>
    public static IActivityWithTask<TInput?, TOutput?>? ToActivityWithTask<TInput, TOutput>(this IActivity? activity) =>
        activity as IActivityWithTask<TInput?, TOutput?>;

    /// <summary>
    /// Converts the specified <see cref="IActivity" />
    /// to <see cref="IActivityWithTask{TInput}" />.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <param name="activity">The activity.</param>
    public static IActivityWithTask<TInput?>? ToActivityWithTask<TInput>(this IActivity? activity) =>
        activity as IActivityWithTask<TInput?>;

    /// <summary>
    /// Converts the specified <see cref="IActivity" />
    /// to <see cref="IActivityWithTask" />.
    /// </summary>
    /// <param name="activity">The activity.</param>
    public static IActivityWithTask? ToActivityWithTask(this IActivity? activity) => activity as IActivityWithTask;

    /// <summary>
    /// Converts the specified <see cref="IActivity" />
    /// to <see cref="IActivityWithTaskOutput{TOutput}" />.
    /// </summary>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    /// <param name="activity">The activity.</param>
    public static IActivityWithTaskOutput<TOutput?>? ToActivityWithTaskOutput<TOutput>(this IActivity? activity) =>
        activity as IActivityWithTaskOutput<TOutput?>;
}
