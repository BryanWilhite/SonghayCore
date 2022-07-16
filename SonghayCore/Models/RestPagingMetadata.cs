namespace Songhay.Models;

/// <summary>
/// REST Paging Metadata
/// </summary>
public class RestPagingMetadata
{
    /// <summary>
    /// Gets or sets the size of the result set.
    /// </summary>
    public int ResultSetSize { get; set; }

    /// <summary>
    /// Gets or sets the total size of the set.
    /// </summary>
    public int TotalSetSize { get; set; }

    /// <summary>
    /// Gets or sets the start position.
    /// </summary>
    public int StartPosition { get; set; }

    /// <summary>
    /// Gets or sets the end position.
    /// </summary>
    public int EndPosition { get; set; }

    /// <summary>
    /// Gets or sets from date.
    /// </summary>
    public DateTime? FromDate { get; set; }

    /// <summary>
    /// Gets or sets to date.
    /// </summary>
    public DateTime? ToDate { get; set; }

    /// <summary>
    /// Gets or sets the next URI.
    /// </summary>
    public string? NextUri { get; set; }

    /// <summary>
    /// Gets or sets the previous URI.
    /// </summary>
    public string? PreviousUri { get; set; }

    /// <summary>
    /// Returns the shallow copy from <see cref="object.MemberwiseClone"/>.
    /// </summary>
    public RestPagingMetadata? ToShallowCopy()
    {
        return MemberwiseClone() as RestPagingMetadata;
    }

    /// <summary>
    /// Returns a <see cref="String" /> that represents this instance.
    /// </summary>
    /// <returns>
    /// A <see cref="String" /> that represents this instance.
    /// </returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(
            $"resultSetSize: {ResultSetSize}, totalSetSize: {TotalSetSize}, startPosition: {StartPosition}, endPosition: {EndPosition}");

        if (FromDate != null) sb.Append($", fromDate: {FromDate}");
        if (ToDate != null) sb.Append($", toDate: {ToDate}");
        if (NextUri != null) sb.Append($", nextUri: {NextUri}");
        if (PreviousUri != null) sb.Append($", previousUri: {PreviousUri}");

        return sb.Length > 0 ? sb.ToString() : base.ToString() ?? GetType().Name;
    }
}
