namespace Songhay.Web.Models;

/// <summary>
/// Defines the URIs that will be processed by the API app.
/// </summary>
public class ApiUriSet : Dictionary<string, Uri>
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public ApiUriSet() : base() { }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public ApiUriSet(IDictionary<string, Uri> dictionary) : base(dictionary) { }
}
