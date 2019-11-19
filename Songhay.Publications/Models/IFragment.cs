
using System;

namespace Songhay.Publications.Models
{
    public interface IFragment
    {
        int FragmentId { get; set; }
        string FragmentName { get; set; }
        string FragmentDisplayName { get; set; }
        Nullable<byte> SortOrdinal { get; set; }
        string ItemChar { get; set; }
        string ItemText { get; set; }
        Nullable<DateTime> CreateDate { get; set; }
        Nullable<DateTime> EndDate { get; set; }
        Nullable<DateTime> ModificationDate { get; set; }
        Nullable<int> DocumentId { get; set; }
        Nullable<int> PrevFragmentId { get; set; }
        Nullable<int> NextFragmentId { get; set; }
        Nullable<bool> IsPrevious { get; set; }
        Nullable<bool> IsNext { get; set; }
        Nullable<bool> IsWrapper { get; set; }
        string ClientId { get; set; }
        Nullable<bool> IsActive { get; set; }
    }
}
