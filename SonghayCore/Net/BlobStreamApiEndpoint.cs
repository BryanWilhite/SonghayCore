using Polly;

namespace Songhay.Net;

/// <summary>
/// Defines the default implementation of <see cref="IBlobStreamApiEndpoint"/>
/// </summary>
/// <param name="instanceTag">maps to <see cref="ITaggedInstance.InstanceTag"/></param>
/// <param name="pipeline">the <see cref="ResiliencePipeline"/></param>
public class BlobStreamApiEndpoint(string? instanceTag, ResiliencePipeline? pipeline) : IBlobStreamApiEndpoint
{
    /// <summary>
    /// Returns the conventional ID or tag of this instance.
    /// </summary>
    public string InstanceTag { get; } = instanceTag.ToReferenceTypeValueOrThrow();

    /// <summary>
    /// Download into a <see cref="string"/>
    /// from the API endpoint
    /// </summary>
    /// <param name="requestStrategy">the <see cref="IApiRequestStrategy"/></param>
    /// <param name="path">the path to the BLOB resource</param>
    /// <param name="streamAction">the strategy for receiving the BLOB stream</param>
    /// <exception cref="HttpRequestException"></exception>
    public async Task DownloadStreamAsync(IApiRequestStrategy? requestStrategy, string? path, Action<Stream>? streamAction)
    {
        ArgumentNullException.ThrowIfNull(requestStrategy);

        await pipeline.ToReferenceTypeValueOrThrow().ExecuteAsync(async cancellationToken =>
            {
                using var request = requestStrategy.GenerateHttpRequestMessage(path);
                using HttpResponseMessage response = await request.SendAsync();

                Stream stream = await response.Content.ReadAsStreamAsync(cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    var message = $"Request for `{request.RequestUri?.AbsoluteUri}` returned status `{response.StatusCode}`.";

                    throw new HttpRequestException(message);
                }

                streamAction?.Invoke(stream);
            })
            .ConfigureAwait(continueOnCapturedContext: false);
    }
}
