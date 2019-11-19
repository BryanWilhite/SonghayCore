using System;

namespace Songhay.Publications.Models
{
    public interface IDocument
    {
        int DocumentId { get; set; }
        string Title { get; set; }
        string DocumentShortName { get; set; }
        string FileName { get; set; }
        string Path { get; set; }
        Nullable<DateTime> CreateDate { get; set; }
        Nullable<DateTime> ModificationDate { get; set; }
        Nullable<int> TemplateId { get; set; }
        Nullable<int> SegmentId { get; set; }
        Nullable<bool> IsRoot { get; set; }
        Nullable<bool> IsActive { get; set; }
        Nullable<byte> SortOrdinal { get; set; }
        string ClientId { get; set; }
        string Tag { get; set; }
    }
}
