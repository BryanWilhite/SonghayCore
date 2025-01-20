namespace Songhay.Abstractions;

/// <summary>
/// Defines an Activity, optionally for <see cref="IHost"/> conventions.
/// </summary>
/// <remarks>
/// For more detail, see “Songhay System Activities example”
/// [https://github.com/BryanWilhite/Songhay.HelloWorlds.Activities]
/// </remarks>
public interface IActivity
{
    /// <summary>
    /// Starts with the specified input.
    /// </summary>
    void Start();
}

/// <summary>
/// Defines an Activity, optionally for <see cref="IHost"/> conventions.
/// </summary>
/// <typeparam name="TInput">The type of the input.</typeparam>
/// <remarks>
/// For more detail, see “Songhay System Activities example”
/// [https://github.com/BryanWilhite/Songhay.HelloWorlds.Activities]
/// </remarks>
public interface IActivity<in TInput>
{
    /// <summary>
    /// Starts with the specified input.
    /// </summary>
    /// <param name="input">The input.</param>
    void Start(TInput? input);
}

/// <summary>
/// Defines an Activity with output, optionally for <see cref="IHost"/> conventions.
/// </summary>
/// <typeparam name="TInput">The type of the input.</typeparam>
/// <typeparam name="TOutput">The type of the output.</typeparam>
/// <seealso cref="IActivity{TInput}" />
public interface IActivity<in TInput, out TOutput>
{
    /// <summary>
    /// Starts with the specified input
    /// and synchronously returns <c>TOutput</c>.
    /// </summary>
    /// <param name="input">The input.</param>
    TOutput? Start(TInput? input);
}
