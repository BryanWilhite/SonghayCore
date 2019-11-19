using System;

namespace Songhay.Publications.Models
{
    public partial interface IGenericWebRepository : IDisposable
    {
        void DeleteDocument(int id);

        void DeleteFragment(int id);

        void DeleteSegment(int id);

        IDocument GetDocument(int id);

        IFragment GetFragment(int id);

        ISegment GetSegment(int id);

        ISegment GetSegment(string segmentName);

        void InsertDocument(Document data);

        void InsertFragment(Fragment data);

        void InsertSegment(Segment data);

        bool IsSegmentChildOfParent(Segment data, string parentSegmentName);

        void SetDocument(IDocument data);

        void SetFragment(IFragment data);

        void SetSegment(ISegment data);

        void TouchSegment(int id);
    }
}
