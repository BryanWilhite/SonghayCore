namespace Songhay.Models
{
    /// <summary>
    /// Defines a sortable visual.
    /// </summary>
    public interface ISortable
    {
        /// <summary>
        /// Gets or sets the sort ordinal.
        /// </summary>
        /// <value>The sort ordinal.</value>
        byte SortOrdinal { get; set; }
    }
}
