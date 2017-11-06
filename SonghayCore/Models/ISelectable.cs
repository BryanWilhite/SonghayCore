namespace Songhay.Models
{
    /// <summary>
    /// Defines a selectable visual.
    /// </summary>
    public interface ISelectable
    {
        /// <summary>
        /// Gets or sets whether this is default selection.
        /// </summary>
        /// <value>
        /// This is default selection.
        /// </value>
        bool? IsDefaultSelection { get; set; }

        /// <summary>
        /// Gets or sets whether this is enabled.
        /// </summary>
        /// <value>
        /// This is enabled.
        /// </value>
        bool? IsEnabled { get; set; }

        /// <summary>
        /// Gets or sets whether this is selected.
        /// </summary>
        /// <value>
        /// <c>true</c> when this is selected.
        /// </value>
        bool? IsSelected { get; set; }
    }
}
