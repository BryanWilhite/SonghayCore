using System.Xml.Linq;

namespace Songhay.Models
{
    /// <summary>
    /// <see cref="XObject"/> metadata
    /// </summary>
    public static class XObjectMetadata
    {
        /// <summary>
        /// The sitemaps.org namespace
        /// </summary>
        public static readonly XNamespace SiteMapNamespace = "http://www.sitemaps.org/schemas/sitemap/0.9";

        /// <summary>
        /// DOCTYPE XHTML Transitional
        /// </summary>
        public static XDocumentType XhtmlDocTypeTransitional
        {
            get
            {
                return
                new XDocumentType("xhtml",
                    "-//W3C//DTD XHTML 1.0 Transitional//EN",
                    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd", null);
            }
        }
    }
}
