using Microsoft.Extensions.Hosting;

namespace Songhay.Abstractions;

/// <summary>
/// Defines an Activity, optionally for <see cref="IHost"/> conventions, with <see cref="Task"/> support.
/// </summary>
/// <typeparam name="TOutput">The non-nullable type of the output.</typeparam>
/// <seealso cref="IActivity" />
/// <remarks>
/// For detail around why this definition exists,
/// see https://github.com/BryanWilhite/SonghayCore/issues/83
/// </remarks>
public interface IActivityOutputOnly<out TOutput> where TOutput: notnull
{
    /// <summary>
    /// Starts the <see cref="IActivity" /> asynchronously.
    /// </summary>
    /// <remarks>
    /// Note that <c>TOutput</c> is not marked as nullable
    /// with the expectation that <c>*Result</c> classes/records
    /// like <see cref="EndpointResult"/> will be used.
    /// </remarks>
    TOutput Start();
}
