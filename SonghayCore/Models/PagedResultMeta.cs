using System;

namespace Songhay.Models
{
    /// <summary>
    /// Defines the metadata for a paged set of data.
    /// </summary>
    public class PagedResultMeta
    {
        /// <summary>
        /// Gets or sets the index of the page.
        /// </summary>
        /// <value>The index of the page.</value>
        public int PageIndex { get; set; }

        /// <summary>
        /// Gets the page count.
        /// </summary>
        /// <value>The page count.</value>
        public int PageCount
        {
            get
            {
                return (this.PageSize > 0) ?
                    Convert.ToInt32(Math.Ceiling((this.TotalCount / this.PageSize) * 1d)) + 1 : 0;
            }
        }

        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>The size of the page.</value>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the total count.
        /// </summary>
        /// <value>The total count.</value>
        public int TotalCount { get; set; }
    }
}
