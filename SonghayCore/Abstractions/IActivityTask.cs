using Microsoft.Extensions.Hosting;

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
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    Task StartAsync(CancellationToken cancellationToken);
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
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    Task StartAsync(TInput? input, CancellationToken cancellationToken);
}

/// <summary>
/// Defines an Activity, optionally for <see cref="IHost"/> conventions, with <see cref="Task"/> support.
/// </summary>
/// <typeparam name="TInput">The type of the input.</typeparam>
/// <typeparam name="TOutput">The non-nullable type of the output.</typeparam>
/// <seealso cref="IActivity" />
/// <remarks>
/// For detail aound why this definition exists,
/// see https://github.com/BryanWilhite/SonghayCore/issues/83
/// </remarks>
public interface IActivityTask<in TInput, TOutput> where TOutput: notnull
{
    /// <summary>
    /// Starts the <see cref="IActivity"/> asynchronously.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <remarks>
    /// Note that <c>TOutput</c> is not marked as nullable
    /// with the expectation that <c>*Result</c> classes/records
    /// like <see cref="EndpointResult"/> will be used.
    /// </remarks>
    Task<TOutput> StartAsync(TInput? input, CancellationToken cancellationToken);
}
