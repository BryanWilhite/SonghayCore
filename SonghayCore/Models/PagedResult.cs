namespace Songhay.Models;

/// <summary>
/// Defines a paged set of data.
/// </summary>
public class PagedResult
{
    /// <summary>
    /// Gets or sets the metadata.
    /// </summary>
    public PagedResultMeta? Metadata { get; set; }

    /// <summary>
    /// Gets or sets the paged result set.
    /// </summary>
    public IEnumerable<DisplayItemModel> PagedResultSet { get; set; } = [];
}
