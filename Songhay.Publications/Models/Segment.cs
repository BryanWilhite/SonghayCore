using Songhay.Publications.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Songhay.Publications.Models
{
    /// <summary>
    /// GenericWeb Segment
    /// </summary>
    public partial class Segment : ISegment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Segment"/> class.
        /// </summary>
        public Segment()
        {
            this.Documents = new List<Document>();
            this.ChildSegments = new List<Segment>();
            this.SegmentDocuments = new List<SegmentDocument>();
        }

        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        /// <value>
        /// The client identifier.
        /// </value>
        [Display(Name = "Client ID", Order = 2)]
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>
        /// The create date.
        /// </value>
        [Display(Name = "Incept Date", Order = 5)]
        [Required]
        public Nullable<DateTime> CreateDate { get; set; }

        /// <summary>
        /// Gets or sets the is active.
        /// </summary>
        /// <value>
        /// The is active.
        /// </value>
        [Display(Name = "Is Active?", Order = 4)]
        [Required]
        public Nullable<bool> IsActive { get; set; }

        /// <summary>
        /// Gets or sets the parent segment identifier.
        /// </summary>
        /// <value>
        /// The parent segment identifier.
        /// </value>
        [Display(Name = "Parent Segment ID", Order = 0)]
        public Nullable<int> ParentSegmentId { get; set; }

        /// <summary>
        /// Gets or sets the segment identifier.
        /// </summary>
        /// <value>
        /// The segment identifier.
        /// </value>
        [Display(Name = "Segment ID", Order = 1)]
        public int SegmentId { get; set; }

        /// <summary>
        /// Gets or sets the name of the segment.
        /// </summary>
        /// <value>
        /// The name of the segment.
        /// </value>
        [Display(Name = "Segment Name", Order = 3)]
        [Required]
        public string SegmentName { get; set; }

        /// <summary>
        /// Gets or sets the sort ordinal.
        /// </summary>
        /// <value>
        /// The sort ordinal.
        /// </value>
        [Display(Name = "Sort Ordinal", Order = 6)]
        public Nullable<byte> SortOrdinal { get; set; }

        /// <summary>
        /// Gets or sets the documents.
        /// </summary>
        /// <value>
        /// The documents.
        /// </value>
        [Display(AutoGenerateField = false)]
        public virtual ICollection<Document> Documents { get; set; }

        /// <summary>
        /// Gets or sets the segments of parent.
        /// </summary>
        /// <value>
        /// The segments of parent.
        /// </value>
        [Display(AutoGenerateField = false)]
        public virtual ICollection<Segment> ChildSegments { get; set; }

        /// <summary>
        /// Gets or sets the parent segment.
        /// </summary>
        /// <value>
        /// The parent segment.
        /// </value>
        [Display(AutoGenerateField = false)]
        public virtual Segment ParentSegment { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="SegmentDocument"/> join.
        /// </summary>
        /// <value>
        /// The <see cref="SegmentDocument"/> join.
        /// </value>
        [Display(AutoGenerateField = false)]
        public virtual ICollection<SegmentDocument> SegmentDocuments { get; set; }

        public override string ToString()
        {
            return this.ToDisplayText();
        }
    }
}
