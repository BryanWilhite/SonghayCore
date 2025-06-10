using Polly;

namespace Songhay.Net;

/// <summary>
/// Defines a REST API endpoint
/// </summary>
/// <param name="instanceTag">maps to <see cref="ITaggedInstance.InstanceTag"/></param>
/// <param name="pipeline">the <see cref="ResiliencePipeline"/></param>
/// <remarks>
/// The output of <c>httpRequestMessageGetter</c> will be inside a <c>using</c> block.
/// </remarks>
public class ApiEndpoint(ResiliencePipeline? pipeline, string? instanceTag) : IApiEndpoint
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
    /// <param name="requestData">the data that is conventionally passed through to <see cref="IApiRequestStrategy.GenerateHttpRequestMessage{TData}"/></param>
    public async Task<string> DownloadStringAsync<TData>(IApiRequestStrategy? requestStrategy, TData? requestData)
    {
        ArgumentNullException.ThrowIfNull(requestStrategy);

        string responseString = await pipeline
            .ToReferenceTypeValueOrThrow()
            .ExecuteAsync(async cancellationToken =>
            {
                using var request = requestStrategy.GenerateHttpRequestMessage(requestData);
                using HttpResponseMessage response = await request
                    .SendAsync()
                    .ConfigureAwait(continueOnCapturedContext: false);

                string content = await response.Content.ReadAsStringAsync(cancellationToken);

                if (response.IsSuccessStatusCode) return content;

                StringBuilder sb = new($"Request for `{request.RequestUri?.AbsoluteUri}` returned status `{response.StatusCode}`.");
                sb.AppendLine("Server Response:");
                sb.AppendLine(content);

                throw new HttpRequestException(sb.ToString());
            })
            .ConfigureAwait(continueOnCapturedContext: false);

        return responseString;
    }
}
