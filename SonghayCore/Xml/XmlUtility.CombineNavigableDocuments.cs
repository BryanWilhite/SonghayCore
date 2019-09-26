using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace Songhay.Xml
{
    /// <summary>
    /// Static helper members for XML-related routines.
    /// </summary>
    /// <remarks>
    /// These definitions are biased toward
    /// emitting <see cref="System.Xml.XPath.XPathDocument"/> sets.
    /// However, many accept any input implementing the
    /// <see cref="System.Xml.XPath.IXPathNavigable"/> interface.
    /// </remarks>
    public static partial class XmlUtility
    {
        /// <summary>
        /// Combines two <see cref="System.Xml.XPath.IXPathNavigable"/> documents.
        /// </summary>
        /// <param name="parentDocument">The <see cref="System.Xml.XPath.IXPathNavigable"/> “hosting” document.</param>
        /// <param name="childDocument">The <see cref="System.Xml.XPath.IXPathNavigable"/> child document.</param>
        [SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes",
            Justification = "Specific functionality provided by the concrete type may be required.")]
        public static XPathDocument CombineNavigableDocuments(IXPathNavigable parentDocument, IXPathNavigable childDocument)
        {
            return XmlUtility.CombineNavigableDocuments(parentDocument, childDocument, null);
        }

        /// <summary>
        /// Combines two <see cref="System.Xml.XPath.IXPathNavigable"/> documents.
        /// </summary>
        /// <param name="parentDocument">The <see cref="System.Xml.XPath.IXPathNavigable"/> “hosting” document.</param>
        /// <param name="childDocument">The <see cref="System.Xml.XPath.IXPathNavigable"/> child document.</param>
        /// <param name="setQuery">The XPath query to the child document loaction in the “hosting” document. </param>
        [SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes",
            Justification = "Specific functionality provided by the concrete type may be required.")]
        public static XPathDocument CombineNavigableDocuments(IXPathNavigable parentDocument, IXPathNavigable childDocument, string setQuery)
        {
            if((parentDocument == null) || (childDocument == null)) return null;

            XmlDocument dom = new XmlDocument();
            dom.LoadXml(parentDocument.CreateNavigator().OuterXml);

            XPathNavigator domNavigator = dom.CreateNavigator();

            if(!string.IsNullOrWhiteSpace(setQuery))
            {
                XPathNodeIterator nodes = domNavigator.Select(setQuery);
                foreach(XPathNavigator n in nodes)
                {
                    n.AppendChild(childDocument.CreateNavigator().OuterXml);
                    break;
                }
            }
            else
            {
                domNavigator.MoveToFirstChild();

                domNavigator.AppendChild(childDocument.CreateNavigator().OuterXml);
                domNavigator.MoveToRoot();
            }

            XPathDocument combinedDocument = null;
            using(StringReader reader = new StringReader(domNavigator.OuterXml))
            {
                combinedDocument = new XPathDocument(reader);
            }
            return combinedDocument;
        }
    }
}
