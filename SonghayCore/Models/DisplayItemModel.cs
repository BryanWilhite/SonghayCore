namespace Songhay.Models;

/// <summary>
/// Model for display item.
/// </summary>
/// <remarks>
/// This class was originally developed
/// to compensate for RIA Services not supporting composition
/// of Entity Framework entities
/// where an Entity is the property of another object.
/// </remarks>
public class DisplayItemModel : ISortable, ITemporal
{
    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the display text.
    /// </summary>
    public string? DisplayText { get; set; }

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the item name.
    /// </summary>
    public string? ItemName { get; set; }

    /// <summary>
    /// Gets or sets the resource indicator.
    /// </summary>
    public Uri? ResourceIndicator { get; set; }

    /// <summary>
    /// Gets or sets the tag.
    /// </summary>
    public object? Tag { get; set; }

    #region ISortable members:

    /// <summary>
    /// Gets or sets the sort ordinal.
    /// </summary>
    public byte SortOrdinal { get; set; }

    #endregion

    #region ITemporal members:

    /// <summary>
    /// End/expiration <see cref="DateTime"/> of the item.
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Origin <see cref="DateTime"/> of the item.
    /// </summary>
    public DateTime? InceptDate { get; set; }

    /// <summary>
    /// Modification/editorial <see cref="DateTime"/> of the item.
    /// </summary>
    public DateTime? ModificationDate { get; set; }

    #endregion

    /// <summary>
    /// Represents this instance as a <c>string</c>.
    /// </summary>
    public override string ToString()
    {
        if (DisplayText != null)
            return $"{Id}: {DisplayText}";

        return ItemName != null ? $"{Id}: {ItemName}" : $"ID: {Id}";
    }
}
