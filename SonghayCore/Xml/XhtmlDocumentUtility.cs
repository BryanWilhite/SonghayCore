using Songhay.Models;
using System.Linq;
using System.Xml.Linq;

namespace Songhay.Xml
{
    /// <summary>
    /// Static members for XHTML Documents.
    /// </summary>
    public static partial class XhtmlDocumentUtility
    {
        /// <summary>
        /// XHTML Namespace
        /// </summary>
        public static XNamespace xhtml { get { return "http://www.w3.org/1999/xhtml"; } }

        /// <summary>
        /// Loads the document.
        /// </summary>
        /// <param name="document">The XML document.</param>
        /// <param name="webPath">The public web path.</param>
        public static XhtmlDocument GetDocument(XDocument document, string webPath)
        {
            return GetDocument(document, webPath, true);
        }

        /// <summary>
        /// Loads the document.
        /// </summary>
        /// <param name="document">The XML document.</param>
        /// <param name="webPath">The public web path.</param>
        /// <param name="useXhtmlNamespace">if set to <c>true</c> use XHTML namespace (<c>true</c> by default).</param>
        public static XhtmlDocument GetDocument(XDocument document, string webPath, bool useXhtmlNamespace)
        {
            if(document == null) return null;

            var heading = useXhtmlNamespace ?
                document.Root
                    .Element(xhtml + "body")
                    .Element(xhtml + "h1") :
                document.Root
                    .Element("body")
                    .Element("h1");

            var title = useXhtmlNamespace ?
                document.Root
                    .Element(xhtml + "head")
                    .Element(xhtml + "title")
                    .Value :
                document.Root
                    .Element("head")
                    .Element("title")
                    .Value;

            var d = new XhtmlDocument
            {
                Header = (heading == null) ? null : heading.Value,
                Location = webPath,
                Title = title
            };

            return d;
        }

        /// <summary>
        /// Loads the document.
        /// </summary>
        /// <param name="pathToDocument">The path to document.</param>
        /// <param name="webPath">The public web path.</param>
        public static XhtmlDocument LoadDocument(string pathToDocument, string webPath)
        {
            var xd = XDocument.Load(pathToDocument);
            var hasAttributes = xd.Root.HasAttributes;
            var hasXhtmlNamespace = false;
            if(hasAttributes) hasXhtmlNamespace = xd.Root.Attributes("xmlns").Count() > 0;
            if(hasAttributes && hasXhtmlNamespace)
            {
                return GetDocument(xd, webPath);
            }
            else
            {
                return GetDocument(xd, webPath, false);
            }
        }
    }
}
