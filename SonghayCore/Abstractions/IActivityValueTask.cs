using Microsoft.Extensions.Hosting;

namespace Songhay.Abstractions;

/// <summary>
/// Defines an Activity, optionally for <see cref="IHost"/> conventions, with <see cref="ValueTask"/> support.
/// </summary>
/// <seealso cref="IActivity" />
/// <remarks>
/// For detail around why this definition exists,
/// see https://github.com/BryanWilhite/SonghayCore/issues/83
/// </remarks>
public interface IActivityValueTask
{
    /// <summary>
    /// Starts the <see cref="IActivity"/> asynchronously.
    /// </summary>
    ValueTask StartAsync();
}

/// <summary>
/// Defines an Activity, optionally for <see cref="IHost"/> conventions, with <see cref="ValueTask"/> support.
/// </summary>
/// <typeparam name="TInput">The type of the input.</typeparam>
/// <seealso cref="IActivity" />
/// <remarks>
/// For detail around why this definition exists,
/// see https://github.com/BryanWilhite/SonghayCore/issues/83
/// </remarks>
public interface IActivityValueTask<in TInput>
{
    /// <summary>
    /// Starts the <see cref="IActivity" /> asynchronously.
    /// </summary>
    /// <param name="input">The input.</param>
    ValueTask StartAsync(TInput? input);
}

/// <summary>
/// Defines an Activity, optionally for <see cref="IHost"/> conventions, with <see cref="ValueTask"/> support.
/// </summary>
/// <typeparam name="TInput">The type of the input.</typeparam>
/// <typeparam name="TOutput">The type of the output.</typeparam>
/// <seealso cref="IActivity" />
/// <remarks>
/// For detail aound why this definition exists,
/// see https://github.com/BryanWilhite/SonghayCore/issues/83
/// </remarks>
public interface IActivityValueTask<in TInput, TOutput>
{
    /// <summary>
    /// Starts the <see cref="IActivity"/> asynchronously.
    /// </summary>
    ValueTask<TOutput?> StartAsync(TInput? input);
}
