namespace Songhay.Abstractions;

/// <summary>
/// A lightweight façade in front of many <c>IActivity*</c> implementations.
/// </summary>
/// <remarks>
/// This abstraction exposes class definitions to consumers with its <c>TActivity</c> type parameter.
/// To avoid this exposure (coupling), consider implementing <see cref="IActivityKeyedTaskGroup{TOutput}"/> instead.
/// </remarks>
/// <typeparam name="TOutput">a non-nullable type</typeparam>
public interface IActivityTaskGroup<TOutput> where TOutput: notnull
{
    /// <summary>
    /// Invokes the <c>IActivity*</c> implementation
    /// identified by the specified key.
    /// </summary>
    /// <typeparam name="TActivity">identifies the <c>IActivity*</c> implementation</typeparam>
    /// <typeparam name="TOutput">the type of the Activity output</typeparam>
    /// <param name="cancellationToken">the <see cref="CancellationToken"/></param>
    /// <param name="args">collects the stringified input arguments of the <c>IActivity*</c> implementation</param>
    /// <remarks>
    /// Note that <c>TOutput</c> is not marked as nullable
    /// with the expectation that <c>*Result</c> classes/records
    /// like <see cref="EndpointResult"/> will be used.
    /// </remarks>
    Task<TOutput> InvokeActivityAsync<TActivity>(CancellationToken cancellationToken, params string?[] args) where TActivity : class;
}
