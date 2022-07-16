namespace Songhay.Models;

/// <summary>
/// Defines the metadata for a paged set of data.
/// </summary>
public class PagedResultMeta
{
    /// <summary>
    /// Gets or sets the index of the page.
    /// </summary>
    public int PageIndex { get; set; }

    /// <summary>
    /// Gets the page count.
    /// </summary>
    public int PageCount => PageSize > 0 ? Convert.ToInt32(Math.Ceiling((TotalCount / PageSize) * 1d)) + 1 : 0;

    /// <summary>
    /// Gets or sets the size of the page.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Gets or sets the total count.
    /// </summary>
    public int TotalCount { get; set; }
}
