using System;

namespace Songhay.Models
{
    /// <summary>
    /// Model for display item
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
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the display text.
        /// </summary>
        /// <value>The display text.</value>
        public string DisplayText { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the item name.
        /// </summary>
        /// <value>The item name.</value>
        public string ItemName { get; set; }

        /// <summary>
        /// Gets or sets the resource indicator.
        /// </summary>
        /// <value>
        /// The resource indicator.
        /// </value>
        public Uri ResourceIndicator { get; set; }

        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        public object Tag { get; set; }

        #region ISortable members:

        /// <summary>
        /// Gets or sets the sort ordinal.
        /// </summary>
        /// <value>The sort ordinal.</value>
        public byte SortOrdinal { get; set; }

        #endregion

        #region ITemporal members:

        /// <summary>
        /// End/expiration <see cref="DateTime"/> of the item.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Origin <see cref="DateTime"/> of the item.
        /// </summary>
        public DateTime InceptDate { get; set; }

        /// <summary>
        /// Modification/editorial <see cref="DateTime"/> of the item.
        /// </summary>
        public DateTime ModificationDate { get; set; }

        #endregion

        /// <summary>
        /// Represents this instance as a <c>string</c>.
        /// </summary>
        public override string ToString()
        {
            if (this.DisplayText != null)
                return string.Format("{0}: {1}", this.Id, this.DisplayText);

            if (this.ItemName != null)
                return string.Format("{0}: {1}", this.Id, this.ItemName);

            return string.Format("ID: {0}", this.Id);
        }
    }
}