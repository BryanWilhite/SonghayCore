namespace Songhay.Models
{
    /// <summary>
    /// Defines a sortable visual.
    /// </summary>
    public interface ISortable
    {
        /// <summary>
        /// Gets or sets the item category.
        /// </summary>
        /// <value>The item category.</value>
        string ItemCategory { get; set; }

        /// <summary>
        /// Gets or sets the sort ordinal.
        /// </summary>
        /// <value>The sort ordinal.</value>
        byte SortOrdinal { get; set; }

        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        string Tag { get; set; }
    }
}
