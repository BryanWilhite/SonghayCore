namespace Songhay.Abstractions;

/// <summary>
/// Defines an Activity, optionally for <see cref="IHost"/> conventions, with <see cref="Task"/> support.
/// </summary>
/// <seealso cref="IActivity" />
/// <remarks>
/// For detail around why this definition exists,
/// see https://github.com/BryanWilhite/SonghayCore/issues/83
/// </remarks>
public interface IActivityTask
{
    /// <summary>
    /// Starts the <see cref="IActivity"/> asynchronously.
    /// </summary>
    Task StartAsync();
}

/// <summary>
/// Defines an Activity, optionally for <see cref="IHost"/> conventions, with <see cref="Task"/> support.
/// </summary>
/// <typeparam name="TInput">The type of the input.</typeparam>
/// <seealso cref="IActivity" />
/// <remarks>
/// For detail around why this definition exists,
/// see https://github.com/BryanWilhite/SonghayCore/issues/83
/// </remarks>
public interface IActivityTask<in TInput>
{
    /// <summary>
    /// Starts the <see cref="IActivity" /> asynchronously.
    /// </summary>
    /// <param name="input">The input.</param>
    Task StartAsync(TInput? input);
}

/// <summary>
/// Defines an Activity, optionally for <see cref="IHost"/> conventions, with <see cref="Task"/> support.
/// </summary>
/// <typeparam name="TInput">The type of the input.</typeparam>
/// <typeparam name="TOutput">The type of the output.</typeparam>
/// <seealso cref="IActivity" />
/// <remarks>
/// For detail aound why this definition exists,
/// see https://github.com/BryanWilhite/SonghayCore/issues/83
/// </remarks>
public interface IActivityTask<in TInput, TOutput> : IActivity
{
    /// <summary>
    /// Starts the <see cref="IActivity"/> asynchronously.
    /// </summary>
    Task<TOutput?> StartAsync(TInput? input);
}
