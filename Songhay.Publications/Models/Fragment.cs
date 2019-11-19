using Songhay.Publications.Extensions;
using Songhay.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Songhay.Publications.Models
{
    /// <summary>
    /// GenericWeb Fragment
    /// </summary>
    public partial class Fragment : IFragment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Fragment"/> class.
        /// </summary>
        public Fragment()
        {
            this.NextFragments = new List<Fragment>();
            this.PrevFragments = new List<Fragment>();
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
        [Display(Name = "Create Date", Order = 4)]
        [Required]
        public Nullable<DateTime> CreateDate { get; set; }

        /// <summary>
        /// Gets or sets the document identifier.
        /// </summary>
        /// <value>
        /// The document identifier.
        /// </value>
        [Display(Name = "Document ID", Order = 0)]
        public Nullable<int> DocumentId { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        [Display(Name = "Expiration Date", Order = 7)]
        public Nullable<DateTime> EndDate { get; set; }

        /// <summary>
        /// Gets or sets the display name of the fragment.
        /// </summary>
        /// <value>
        /// The display name of the fragment.
        /// </value>
        [Display(Name = "Display Name", Order = 3)]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public string FragmentDisplayName { get; set; }

        /// <summary>
        /// Gets or sets the fragment identifier.
        /// </summary>
        /// <value>
        /// The fragment identifier.
        /// </value>
        [Display(Name = "Fragment ID", Order = 1)]
        public int FragmentId { get; set; }

        /// <summary>
        /// Gets or sets the name of the fragment.
        /// </summary>
        /// <value>
        /// The name of the fragment.
        /// </value>
        [Display(Name = "Item Name", Order = 2)]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public string FragmentName { get; set; }

        /// <summary>
        /// Gets or sets the is active.
        /// </summary>
        /// <value>
        /// The is active.
        /// </value>
        [Display(Name = "Is Active?", Order = 10)]
        public Nullable<bool> IsActive { get; set; }

        /// <summary>
        /// Gets or sets the is next.
        /// </summary>
        /// <value>
        /// The is next.
        /// </value>
        [Display(Name = "Is Next?", Order = 11)]
        public Nullable<bool> IsNext { get; set; }

        /// <summary>
        /// Gets or sets the is previous.
        /// </summary>
        /// <value>
        /// The is previous.
        /// </value>
        [Display(Name = "Is Previous?", Order = 12)]
        public Nullable<bool> IsPrevious { get; set; }

        /// <summary>
        /// Gets or sets the is wrapper.
        /// </summary>
        /// <value>
        /// The is wrapper.
        /// </value>
        [Display(Name = "Is Wrapper?", Order = 13)]
        public Nullable<bool> IsWrapper { get; set; }

        /// <summary>
        /// Gets or sets the item character.
        /// </summary>
        /// <value>
        /// The item character.
        /// </value>
        [Display(AutoGenerateField = false)]
        public string ItemChar { get; set; }

        /// <summary>
        /// Gets or sets the item text.
        /// </summary>
        /// <value>
        /// The item text.
        /// </value>
        [Display(AutoGenerateField = false)]
        public string ItemText { get; set; }

        /// <summary>
        /// Gets or sets the modification date.
        /// </summary>
        /// <value>
        /// The modification date.
        /// </value>
        [Display(Name = "Modification Date", Order = 5)]
        [Required]
        public Nullable<DateTime> ModificationDate { get; set; }

        /// <summary>
        /// Gets or sets the next fragment identifier.
        /// </summary>
        /// <value>
        /// The next fragment identifier.
        /// </value>
        [Display(Name = "Next Fragment ID", Order = 7)]
        public Nullable<int> NextFragmentId { get; set; }

        /// <summary>
        /// Gets or sets the previous fragment identifier.
        /// </summary>
        /// <value>
        /// The previous fragment identifier.
        /// </value>
        [Display(Name = "Previous Fragment ID", Order = 8)]
        public Nullable<int> PrevFragmentId { get; set; }

        /// <summary>
        /// Gets or sets the sort ordinal.
        /// </summary>
        /// <value>
        /// The sort ordinal.
        /// </value>
        [Display(Name = "Sort Ordinal", Order = 6)]
        public Nullable<byte> SortOrdinal { get; set; }

        /// <summary>
        /// Gets or sets the document.
        /// </summary>
        /// <value>
        /// The document.
        /// </value>
        [Display(AutoGenerateField = false)]
        public virtual Document Document { get; set; }

        /// <summary>
        /// Gets or sets the next fragments.
        /// </summary>
        /// <value>
        /// The next fragments.
        /// </value>
        [Display(AutoGenerateField = false)]
        public virtual ICollection<Fragment> NextFragments { get; set; }

        /// <summary>
        /// Gets or sets the next fragment.
        /// </summary>
        /// <value>
        /// The next fragment.
        /// </value>
        [Display(AutoGenerateField = false)]
        public virtual Fragment NextFragment { get; set; }

        /// <summary>
        /// Gets or sets the previous fragments.
        /// </summary>
        /// <value>
        /// The previous fragments.
        /// </value>
        [Display(AutoGenerateField = false)]
        public virtual ICollection<Fragment> PrevFragments { get; set; }

        /// <summary>
        /// Gets or sets the previous fragment.
        /// </summary>
        /// <value>
        /// The previous fragment.
        /// </value>
        [Display(AutoGenerateField = false)]
        public virtual Fragment PrevFragment { get; set; }

        public override string ToString()
        {
            return this.ToDisplayText();
        }
    }
}
