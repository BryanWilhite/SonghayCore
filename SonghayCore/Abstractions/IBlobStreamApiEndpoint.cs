namespace Songhay.Abstractions;

/// <summary>
/// Defines the retrieval of a remote BLOB as a <see cref="Stream"/>.
/// </summary>
public interface IBlobStreamApiEndpoint : ITaggedInstance
{
    /// <summary>
    /// Downloads a BLOB with the specified name
    /// into a <see cref="Stream"/>
    /// </summary>
    /// <param name="requestStrategy">the <see cref="IApiRequestStrategy"/> required for the endpoint</param>
    /// <param name="streamAction">The action to take with the <see cref="Stream"/> associated with the BLOB</param>
    /// <param name="path">any <see cref="Uri.PathAndQuery"/> and/or <see cref="Uri.Fragment"/></param>
    Task DownloadStreamAsync(IApiRequestStrategy? requestStrategy, string? path, Action<Stream>? streamAction);
}
