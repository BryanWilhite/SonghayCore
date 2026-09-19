namespace Songhay.Abstractions;

/// <summary>
/// A lightweight façade in front of many <c>IActivity*</c> implementations.
/// </summary>
/// <typeparam name="TOutput">a non-nullable value or reference type</typeparam>
/// <remarks>
/// <para>
/// Consider using the conventional Result types
/// like <see cref="ProgramOutputResult{TOutput}"/>
/// or <see cref="EndpointContentResult{TContent}"/>
/// for <c>TOutput</c>.
/// </para>
/// <para>
/// This abstraction exposes class definitions to consumers with its <c>TActivity</c> type parameter.
/// To avoid this exposure (coupling), consider implementing <see cref="IActivityKeyedTaskGroup{TOutput}"/> instead.
/// </para>
/// </remarks>
public interface IActivityTaskGroup<TOutput> where TOutput: notnull
{
    /// <summary>
    /// Invokes the <c>IActivity*</c> implementation
    /// identified by the specified key.
    /// </summary>
    /// <typeparam name="TActivity">identifies the <c>IActivity*</c> implementation</typeparam>
    /// <param name="cancellationToken">the <see cref="CancellationToken"/></param>
    /// <param name="args">collects the stringified input arguments of the <c>IActivity*</c> implementation</param>
    /// <remarks>
    /// Note that <c>TOutput</c> is not marked as nullable
    /// with the expectation that <c>*Result</c> classes/records
    /// like <see cref="EndpointResult"/> will be used.
    /// </remarks>
    Task<TOutput> InvokeActivityAsync<TActivity>(CancellationToken cancellationToken, params string?[] args) where TActivity : class;
}
