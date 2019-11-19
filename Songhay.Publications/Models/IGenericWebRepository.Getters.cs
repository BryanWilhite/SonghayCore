using Newtonsoft.Json.Linq;
using Songhay.Models;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Songhay.Publications.Models
{
    public partial interface IGenericWebRepository
    {
        IEnumerable<DisplayItemModel> GetDocumentFragmentMenuItems(int documentId);

        XDocument GetDocumentsForPublication();

        XDocument GetDocumentsForPublication(DateTime? pubDate);

        XDocument GetDocumentsForPublicationBySegment(string segmentName);

        JObject GetDocumentWithAnyFragments(int id, bool useJavaScriptCase);

        DocumentWithContent GetDocumentWithContent(int id);

        IFragment GetFragment(string name);

        IFragment GetFragmentByClientId(string clientId);

        JObject GetFragmentsForAssociations(bool useJavaScriptCase);

        int? GetSegmentId(string segmentName);

        int? GetSegmentId(string segmentName, string parentSegmentName);

        JObject GetSegmentsOfParent(int? parentSegmentId, bool useJavaScriptCase);

        JObject GetSegmentWithAnyChildren(int id, bool useJavaScriptCase);

        Dictionary<string, IEnumerable<MenuDisplayItemModel>> GetSummaryItems();

        JObject GetWebPresentation(string category, int id, bool useJavaScriptCase);

        MenuDisplayItemModel GetWebPresentationDisplayItems(string category, int id);
    }
}
