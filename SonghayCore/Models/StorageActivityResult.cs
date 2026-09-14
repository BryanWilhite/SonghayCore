namespace Songhay.Models;

/// <summary>
/// Defines the result returned from all storage-related Activities.
/// </summary>
/// <typeparam name="TContent">the type of the content returned in the response</typeparam>
/// <param name="HttpStatusCode">the <see cref="HttpStatusCode"/></param>
/// <param name="RequestId">the identifier of the request</param>
/// <param name="ResponseMessage">any message associated with the request</param>
/// <param name="Content">any content associated with the response</param>
public record StorageActivityResult<TContent>(
    HttpStatusCode HttpStatusCode,
    string? RequestId,
    string? ResponseMessage,
    TContent? Content
);

/// <summary>
/// Defines the result returned from all storage-related Activities.
/// </summary>
/// <param name="HttpStatusCode">the <see cref="HttpStatusCode"/></param>
/// <param name="RequestId">the identifier of the request</param>
/// <param name="ResponseMessage">any message associated with the request</param>
public record StorageActivityResult(
    HttpStatusCode HttpStatusCode,
    string? RequestId,
    string? ResponseMessage
);
