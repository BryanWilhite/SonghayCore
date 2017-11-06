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
    public class DisplayItemModel : ISortable
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
        /// Represents this instance as a <c>string</c>.
        /// </summary>
        public override string ToString()
        {
            var s = getIdAndName();

            if (this.ItemCategory != null)
                s = string.Format("{0}, ItemCategory: {1}", s, this.ItemCategory);

            if (this.DisplayText != null)
                s = string.Format("{0}, DisplayText: {1}", s, this.DisplayText);

            return s;
        }

        #region ISortable members:

        /// <summary>
        /// Gets or sets the item category.
        /// </summary>
        /// <value>The item category.</value>
        public string ItemCategory { get; set; }

        /// <summary>
        /// Gets or sets the sort ordinal.
        /// </summary>
        /// <value>The sort ordinal.</value>
        public byte SortOrdinal { get; set; }

        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        public string Tag { get; set; }

        #endregion

        string getIdAndName()
        {
            return string.Format("ID: {0}, Item Name: {1}", this.Id, this.ItemName);
        }
    }
}
