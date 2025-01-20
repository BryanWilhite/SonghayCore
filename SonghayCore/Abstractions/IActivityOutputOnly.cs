namespace Songhay.Abstractions;

/// <summary>
/// Defines an Activity, optionally for <see cref="IHost"/> conventions, with <see cref="Task"/> support.
/// </summary>
/// <typeparam name="TOutput">The type of the output.</typeparam>
/// <seealso cref="IActivity" />
/// <remarks>
/// For detail around why this definition exists,
/// see https://github.com/BryanWilhite/SonghayCore/issues/83
/// </remarks>
public interface IActivityOutputOnly<out TOutput>
{
    /// <summary>
    /// Starts the <see cref="IActivity" /> asynchronously.
    /// </summary>
    TOutput? StartAsync();
}
