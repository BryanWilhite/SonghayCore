namespace Songhay.Abstractions;

/// <summary>
/// A lightweight façade in front of many <c>IActivity*</c> implementations.
/// </summary>
/// <typeparam name="TOutput">a non-nullable type</typeparam>
/// <remarks>
/// <para>
/// This abstraction uses the word <c>Keyed</c>
/// to indicate that the many <c>IActivity*</c> implementations
/// are identified by magic strings to avoid exposing class definitions
/// to downstream consumers.
/// </para>
///
/// <para>
/// To avoid magic strings, consider implementing <see cref="IActivityTaskGroup{TOutput}"/> instead.
/// </para>
/// </remarks>
public interface IActivityKeyedTaskGroup<TOutput> where TOutput: notnull
{
    /// <summary>
    /// Invokes the <c>IActivity*</c> implementation identified by the specified key.
    /// </summary>
    /// <param name="activitySetKey">identifies the <c>IActivity*</c> implementation</param>
    /// <param name="cancellationToken">the <see cref="CancellationToken"/></param>
    /// <param name="args">collects the stringified input arguments of the <c>IActivity*</c> implementation</param>
    /// <remarks>
    /// Note that <c>TOutput</c> is not marked as nullable
    /// with the expectation that <c>*Result</c> classes/records
    /// like <see cref="EndpointResult"/> will be used.
    /// </remarks>
    Task<TOutput> InvokeActivityAsync(string? activitySetKey, CancellationToken cancellationToken,  params string?[] args);
}
