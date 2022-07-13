namespace Songhay.Abstractions;

/// <summary>
/// Extends <see cref="IActivity" /> with output support
/// and input support beyond <see cref="ProgramArgs"/>.
/// </summary>
/// <typeparam name="TInput">The type of the input.</typeparam>
/// <typeparam name="TOutput">The type of the output.</typeparam>
/// <seealso cref="IActivity" />
public interface IActivityWithOutput<in TInput, out TOutput> : IActivity
{
    /// <summary>
    /// Starts with the specified input
    /// and synchronously returns <c>TOutput</c>.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <remarks>
    /// This member is called <c>StartForOutput</c>
    /// instead of <c>Start</c> to prevent compile-time,
    /// overload-clashing with <see cref="IActivity.Start"/>.
    /// </remarks>
    TOutput? StartForOutput(TInput? input);
}
