namespace Songhay.Net;

/// <summary>
/// The default implementation of <see cref="IApiRequestStrategy"/>.
/// </summary>
/// <param name="instanceTag">sets <see cref="ITaggedInstance.InstanceTag"/></param>
/// <param name="messageGetter">optional getter of <see cref="HttpRequestMessage"/> based on <see cref="HttpRequestOptions"/></param>
public class ApiRequestStrategy(Func<HttpRequestOptions, HttpRequestMessage>? messageGetter, string? instanceTag) : IApiRequestStrategy
{
    /// <summary>
    /// Returns the conventional ID or tag of this instance.
    /// </summary>
    /// <remarks>
    /// This identifier is intended for asserting that an expected ID is present
    /// which is useful for ensuring that an extension method is operating on the expected instance.
    /// </remarks>
    public string InstanceTag { get; } = instanceTag.ToReferenceTypeValueOrThrow();

    /// <summary>
    /// Returns an <see cref="HttpRequestMessage"/> needed to call the API
    /// </summary>
    /// <param name="requestData">the data used to build a new request</param>
    /// <remarks>
    /// This member supports the following <c>TData</c> types:
    ///
    /// - <see cref="string"/>
    /// - <see cref="HttpRequestOptions"/> for the specified <c>messageGetter</c>
    /// - <see cref="Uri"/>
    /// - <see cref="UriBuilder"/>
    /// - 2 tuples of <see cref="HttpMethod"/> and the previously mentioned types
    /// 
    /// </remarks>
    public HttpRequestMessage GenerateHttpRequestMessage<TData>(TData? requestData)
    {
        return requestData switch
        {
            string location => new HttpRequestMessage(HttpMethod.Get, location),
            HttpRequestOptions options => messageGetter.ToReferenceTypeValueOrThrow().Invoke(options),
            Uri uri => new HttpRequestMessage(HttpMethod.Get, uri),
            UriBuilder builder => new HttpRequestMessage(HttpMethod.Get, builder.Uri),
            Tuple<HttpMethod, string> t => new HttpRequestMessage(t.Item1, t.Item2), 
            Tuple<HttpMethod, Uri> t => new HttpRequestMessage(t.Item1, t.Item2), 
            Tuple<HttpMethod, UriBuilder> t => new HttpRequestMessage(t.Item1, t.Item2.Uri), 
            ValueTuple<HttpMethod, string> t => new HttpRequestMessage(t.Item1, t.Item2), 
            ValueTuple<HttpMethod, Uri> t => new HttpRequestMessage(t.Item1, t.Item2), 
            ValueTuple<HttpMethod, UriBuilder> t => new HttpRequestMessage(t.Item1, t.Item2.Uri), 
            _ => throw new NotSupportedException($"Type `{typeof(TData).Name}` is not supported.")
        };
    }
}
