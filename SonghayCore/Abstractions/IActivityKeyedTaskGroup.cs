namespace Songhay.Abstractions;

/// <summary>
/// A lightweight façade in front of many <c>IActivity*</c> implementations.
/// </summary>
/// <remarks>
/// This abstraction uses the word <c>Keyed</c>
/// to indicate that the many <c>IActivity*</c> implementations
/// are identified by magic strings to avoid exposing formal classes
/// to consumers.
///
/// To avoid magic strings, consider implementing <see cref="IActivityTaskGroup"/> instead.
/// </remarks>
public interface IActivityKeyedTaskGroup
{
    /// <summary>
    /// Invokes the <c>IActivity*</c> implementation
    /// identified by the specified key.
    /// </summary>
    /// <param name="activitySetKey">identifies the <c>IActivity*</c> implementation</param>
    /// <param name="args">collects the stringified input arguments of the <c>IActivity*</c> implementation</param>
    /// <returns>
    /// Returns the stringified output of <c>IActivity*</c> implementation.
    /// </returns>
    Task<string?> InvokeActivityAsync(string activitySetKey, params string[] args);
}
