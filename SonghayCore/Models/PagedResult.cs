using System.Collections.Generic;

namespace Songhay.Models
{
    /// <summary>
    /// Defines a paged set of data.
    /// </summary>
    public class PagedResult
    {
        /// <summary>
        /// Gets or sets the metadata.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        public PagedResultMeta Metadata { get; set; }

        /// <summary>
        /// Gets or sets the paged result set.
        /// </summary>
        /// <value>The paged result set.</value>
        public IEnumerable<DisplayItemModel> PagedResultSet { get; set; }
    }
}
