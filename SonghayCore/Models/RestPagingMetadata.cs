using System;
using System.Text;

namespace Songhay.Models
{
    /// <summary>
    /// REST Paging Metadata
    /// </summary>
    public class RestPagingMetadata
    {
        /// <summary>
        /// Gets or sets the size of the result set.
        /// </summary>
        /// <value>
        /// The size of the result set.
        /// </value>
        public int ResultSetSize { get; set; }

        /// <summary>
        /// Gets or sets the total size of the set.
        /// </summary>
        /// <value>
        /// The total size of the set.
        /// </value>
        public int TotalSetSize { get; set; }

        /// <summary>
        /// Gets or sets the start position.
        /// </summary>
        /// <value>
        /// The start position.
        /// </value>
        public int StartPosition { get; set; }

        /// <summary>
        /// Gets or sets the end position.
        /// </summary>
        /// <value>
        /// The end position.
        /// </value>
        public int EndPosition { get; set; }

        /// <summary>
        /// Gets or sets from date.
        /// </summary>
        /// <value>
        /// From date.
        /// </value>
        public DateTime? FromDate { get; set; }

        /// <summary>
        /// Gets or sets to date.
        /// </summary>
        /// <value>
        /// To date.
        /// </value>
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Gets or sets the next URI.
        /// </summary>
        /// <value>
        /// The next URI.
        /// </value>
        public string NextUri { get; set; }

        /// <summary>
        /// Gets or sets the previous URI.
        /// </summary>
        /// <value>
        /// The previous URI.
        /// </value>
        public string PreviousUri { get; set; }

        /// <summary>
        /// Returns the shallow copy from <see cref="object.MemberwiseClone"/>.
        /// </summary>
        /// <returns></returns>
        public RestPagingMetadata ToShallowCopy()
        {
            return this.MemberwiseClone() as RestPagingMetadata;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("resultSetSize: {0}, totalSetSize: {1}, startPosition: {2}, endPosition: {3}",
                this.ResultSetSize, this.TotalSetSize, this.StartPosition, this.EndPosition);

            if (this.FromDate != null) sb.AppendFormat(", fromDate: {0}", this.FromDate);
            if (this.ToDate != null) sb.AppendFormat(", toDate: {0}", this.ToDate);
            if (this.NextUri != null) sb.AppendFormat(", nextUri: {0}", this.NextUri);
            if (this.PreviousUri != null) sb.AppendFormat(", previousUri: {0}", this.PreviousUri);

            return (sb.Length > 0) ? sb.ToString() : base.ToString();
        }
    }
}
