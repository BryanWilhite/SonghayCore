using Newtonsoft.Json.Linq;
using Songhay.Models;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Songhay.Publications.Models
{
    public interface IWebKinteSpaceRepository : IGenericWebRepository
    {
        XElement GetDocumentFragmentsForPublication(int documentId);

        IEnumerable<MenuDisplayItemModel> GetDocumentTemplates();

        string GetPresentationDescriptionByDocument(int? documentId);

        DateTime? GetPublicationDate();

        bool? GetPublicationDateFlag();

        XElement GetRssData();

        JObject GetSiteMapDocuments();

        JObject GetSplashIndex();

        Dictionary<string, string> GetTopSegmentIndexes();

        string GetWebLocalPath();

        XElement GetWebPresentationForStaticFile(int? segmentId);

        void SetPublicationDateFlag();
    }
}
