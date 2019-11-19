using System;

namespace Songhay.Publications.Models
{
    public interface ISegment
    {
        int SegmentId { get; set; }
        string SegmentName { get; set; }
        Nullable<byte> SortOrdinal { get; set; }
        Nullable<System.DateTime> CreateDate { get; set; }
        Nullable<int> ParentSegmentId { get; set; }
        string ClientId { get; set; }
        Nullable<bool> IsActive { get; set; }
    }
}
