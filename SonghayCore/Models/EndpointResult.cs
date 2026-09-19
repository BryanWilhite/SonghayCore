namespace Songhay.Models;

/// <summary>
/// Defines the result returned from all storage-related Activities.
/// </summary>
/// <param name="HttpStatusCode">the <see cref="HttpStatusCode"/></param>
/// <param name="RequestId">the identifier of the request</param>
/// <param name="ResponseMessage">any message associated with the request</param>
public record EndpointResult(
    HttpStatusCode HttpStatusCode,
    string? RequestId,
    string? ResponseMessage
);
