using Songhay.Publications.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Songhay.Publications.Models
{
    /// <summary>
    /// GenericWeb Document
    /// </summary>
    public partial class Document : IDocument
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Document"/> class.
        /// </summary>
        public Document()
        {
            this.Fragments = new List<Fragment>();
            this.WebKeywords = new List<WebKeyword>();
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
        [Display(Name = "Create Date", Order = 7)]
        [Required]
        public Nullable<DateTime> CreateDate { get; set; }

        /// <summary>
        /// Gets or sets the document identifier.
        /// </summary>
        /// <value>
        /// The document identifier.
        /// </value>
        [Display(Name = "Document ID", Order = 1)]
        public int DocumentId { get; set; }

        /// <summary>
        /// Gets or sets the short name of the document.
        /// </summary>
        /// <value>
        /// The short name of the document.
        /// </value>
        [Display(Name = "Short Name", Order = 4)]
        public string DocumentShortName { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        [Display(Name = "File Name", Order = 6)]
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the is active.
        /// </summary>
        /// <value>
        /// The is active.
        /// </value>
        [Display(Name = "Is Active?", Order = 9)]
        [Required]
        public Nullable<bool> IsActive { get; set; }

        /// <summary>
        /// Gets or sets the is root.
        /// </summary>
        /// <value>
        /// The is root.
        /// </value>
        [Display(Name = "Is Root?", Order = 10)]
        public Nullable<bool> IsRoot { get; set; }

        /// <summary>
        /// Gets or sets the modification date.
        /// </summary>
        /// <value>
        /// The modification date.
        /// </value>
        [Display(Name = "Modification Date", Order = 8)]
        [Required]
        public Nullable<DateTime> ModificationDate { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        [Display(Name = "Path", Order = 5)]
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the segment identifier.
        /// </summary>
        /// <value>
        /// The segment identifier.
        /// </value>
        [Display(Name = "Segment ID", Order = 0)]
        public Nullable<int> SegmentId { get; set; }

        /// <summary>
        /// Gets or sets the sort ordinal.
        /// </summary>
        /// <value>
        /// The sort ordinal.
        /// </value>
        [Display(Name = "Sort Ordinal", Order = 6)]
        public Nullable<byte> SortOrdinal { get; set; }

        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        [Display(Name = "Document Tag", Order = 11)]
        public string Tag { get; set; }

        /// <summary>
        /// Gets or sets the template identifier.
        /// </summary>
        /// <value>
        /// The template identifier.
        /// </value>
        [Display(Name = "XSL Template", Order = 12)]
        public Nullable<int> TemplateId { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [Display(Name = "Document Title", Order = 3)]
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the segment.
        /// </summary>
        /// <value>
        /// The segment.
        /// </value>
        [Display(AutoGenerateField = false)]
        public virtual Segment Segment { get; set; }

        /// <summary>
        /// Gets or sets the fragments.
        /// </summary>
        /// <value>
        /// The fragments.
        /// </value>
        [Display(AutoGenerateField = false)]
        public virtual ICollection<Fragment> Fragments { get; set; }

        /// <summary>
        /// Gets or sets the web keywords.
        /// </summary>
        /// <value>
        /// The web keywords.
        /// </value>
        [Display(AutoGenerateField = false)]
        public virtual ICollection<WebKeyword> WebKeywords { get; set; }


        /// <summary>
        /// Gets or sets the <see cref="SegmentDocument"/> join.
        /// </summary>
        /// <value>
        /// The <see cref="SegmentDocument"/> join.
        /// </value>
        [Display(AutoGenerateField = false)]
        public virtual ICollection<SegmentDocument> SegmentDocuments { get; set; }

        /// <summary>
        /// Converts the <see cref="Error"/> into a string.
        /// </summary>
        public override string ToString()
        {
            return this.ToDisplayText();
        }
    }
}
