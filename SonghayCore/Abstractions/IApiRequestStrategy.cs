using System.Net.Http;

namespace Songhay.Abstractions;

/// <summary>
/// Defines that an <see cref="HttpRequestMessage"/> needs to be generated.
/// </summary>
public interface IApiRequestStrategy : ITaggedInstance
{
    /// <summary>
    /// Returns an <see cref="HttpRequestMessage"/> needed to call the API
    /// </summary>
    /// <param name="requestData">the data used to build a new request</param>
    HttpRequestMessage GenerateHttpRequestMessage<TData>(TData? requestData);
}
