using System;
using System.ComponentModel.DataAnnotations;

namespace Songhay.Publications.Models
{
    public class FragmentMetadata : IFragment
    {
        [Display(Name = "Client ID", Order = 2)]
        public string ClientId { get; set; }

        [Display(Name = "Create Date", Order = 4)]
        [Required]
        public DateTime? CreateDate { get; set; }

        [Display(Name = "Document ID", Order = 0)]
        public int? DocumentId { get; set; }

        [Display(Name = "Expiration Date", Order = 7)]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Display Name", Order = 3)]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public string FragmentDisplayName { get; set; }

        [Display(Name = "Fragment ID", Order = 1)]
        public int FragmentId { get; set; }

        [Display(Name = "Item Name", Order = 2)]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public string FragmentName { get; set; }

        [Display(Name = "Is Active?", Order = 10)]
        public bool? IsActive { get; set; }

        [Display(Name = "Is Next?", Order = 11)]
        public bool? IsNext { get; set; }

        [Display(Name = "Is Previous?", Order = 12)]
        public bool? IsPrevious { get; set; }

        [Display(Name = "Is Wrapper?", Order = 13)]
        public bool? IsWrapper { get; set; }

        [Display(AutoGenerateField = false)]
        public string ItemChar { get; set; }

        [Display(AutoGenerateField = false)]
        public string ItemText { get; set; }

        [Display(Name = "Modification Date", Order = 5)]
        [Required]
        public DateTime? ModificationDate { get; set; }

        [Display(Name = "Next Fragment ID", Order = 7)]
        public int? NextFragmentId { get; set; }

        [Display(Name = "Previous Fragment ID", Order = 8)]
        public int? PrevFragmentId { get; set; }

        [Display(Name = "Sort Ordinal", Order = 6)]
        public byte? SortOrdinal { get; set; }
    }
}
