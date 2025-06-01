namespace Songhay.Abstractions;

/// <summary>
/// Defines how an API should be accessed.
/// </summary>
public interface IApiEndpoint : ITaggedInstance
{
    /// <summary>
    /// Download into a <see cref="string"/>
    /// from the API endpoint
    /// </summary>
    /// <param name="requestStrategy">the <see cref="IApiRequestStrategy"/> required for the endpoint</param>
    /// <param name="requestData">the data that is conventionally passed through to <see cref="IApiRequestStrategy.GenerateHttpRequestMessage{TData}"/></param>
    Task<string> DownloadStringAsync<TData>(IApiRequestStrategy? requestStrategy, TData? requestData);
}
