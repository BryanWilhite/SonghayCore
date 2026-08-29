namespace Songhay.Abstractions;

/// <summary>
/// A lightweight façade in front of many <c>IActivity*</c> implementations.
/// </summary>
/// <remarks>
/// This abstraction exposes formal classes to consumers.
/// To avoid this exposure (coupling), consider implementing <see cref="IActivityKeyedTaskGroup"/> instead.
/// </remarks>
public interface IActivityTaskGroup
{
    /// <summary>
    /// Invokes the <c>IActivity*</c> implementation
    /// identified by the specified key.
    /// </summary>
    /// <typeparam name="TActivity">identifies the <c>IActivity*</c> implementation</typeparam>
    /// <param name="args">collects the stringified input arguments of the <c>IActivity*</c> implementation</param>
    /// <returns>
    /// Returns the stringified output of <c>IActivity*</c> implementation.
    /// </returns>
    Task<string?> InvokeActivityAsync<TActivity>(params string[] args) where TActivity : class;
}
