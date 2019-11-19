using System;
using System.ComponentModel.DataAnnotations;

namespace Songhay.Publications.Models
{
    public class SegmentMetadata : ISegment
    {
        [Display(Name = "Client ID", Order = 2)]
        public string ClientId { get; set; }

        [Display(Name = "Incept Date", Order = 5)]
        [Required]
        public DateTime? CreateDate { get; set; }

        [Display(Name = "Is Active?", Order = 4)]
        [Required]
        public bool? IsActive { get; set; }

        [Display(Name = "Parent Segment ID", Order = 0)]
        public int? ParentSegmentId { get; set; }

        [Display(Name = "Segment ID", Order = 1)]
        public int SegmentId { get; set; }

        [Display(Name = "Segment Name", Order = 3)]
        [Required]
        public string SegmentName { get; set; }

        [Display(Name = "Sort Ordinal", Order = 6)]
        public byte? SortOrdinal { get; set; }
    }
}
