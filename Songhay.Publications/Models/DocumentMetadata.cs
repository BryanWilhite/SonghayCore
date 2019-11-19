using System;
using System.ComponentModel.DataAnnotations;

namespace Songhay.Publications.Models
{
    public class DocumentMetadata : IDocument
    {
        [Display(Name = "Client ID", Order = 2)]
        public string ClientId { get; set; }

        [Display(Name = "Create Date", Order = 7)]
        [Required]
        public DateTime? CreateDate { get; set; }

        [Display(Name = "Document ID", Order = 1)]
        public int DocumentId { get; set; }

        [Display(Name = "Short Name", Order = 4)]
        public string DocumentShortName { get; set; }

        [Display(Name = "File Name", Order = 6)]
        public string FileName { get; set; }

        [Display(Name = "Is Active?", Order = 9)]
        [Required]
        public bool? IsActive { get; set; }

        [Display(Name = "Is Root?", Order = 10)]
        public bool? IsRoot { get; set; }

        [Display(Name = "Modification Date", Order = 8)]
        [Required]
        public DateTime? ModificationDate { get; set; }

        [Display(Name = "Path", Order = 5)]
        public string Path { get; set; }

        [Display(Name = "Segment ID", Order = 0)]
        public int? SegmentId { get; set; }

        [Display(Name = "Sort Ordinal", Order = 6)]
        public byte? SortOrdinal { get; set; }

        [Display(Name = "Document Tag", Order = 11)]
        public string Tag { get; set; }

        [Display(Name = "XSL Template", Order = 12)]
        public int? TemplateId { get; set; }

        [Display(Name = "Document Title", Order = 3)]
        [Required]
        public string Title { get; set; }
    }
}
