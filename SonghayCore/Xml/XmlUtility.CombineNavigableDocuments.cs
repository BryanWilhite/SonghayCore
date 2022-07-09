using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace Songhay.Xml;

/// <summary>
/// Static helper members for XML-related routines.
/// </summary>
/// <remarks>
/// These definitions are biased toward
/// emitting <see cref="System.Xml.XPath.XPathDocument"/> documents.
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
    public static XPathDocument? CombineNavigableDocuments(IXPathNavigable? parentDocument,
        IXPathNavigable? childDocument) => CombineNavigableDocuments(parentDocument, childDocument, null);

    /// <summary>
    /// Combines two <see cref="System.Xml.XPath.IXPathNavigable"/> documents.
    /// </summary>
    /// <param name="parentDocument">The <see cref="System.Xml.XPath.IXPathNavigable"/> “hosting” document.</param>
    /// <param name="childDocument">The <see cref="System.Xml.XPath.IXPathNavigable"/> child document.</param>
    /// <param name="nodeQuery">The XPath query to the child document location in the “hosting” document. </param>
    [SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes",
        Justification = "Specific functionality provided by the concrete type may be required.")]
    public static XPathDocument? CombineNavigableDocuments(IXPathNavigable? parentDocument,
        IXPathNavigable? childDocument, string? nodeQuery)
    {
        if (parentDocument == null || childDocument == null) return null;

        var dom = new XmlDocument();
        XPathNavigator? domNavigator = dom.CreateNavigator();
        if (domNavigator == null) return null;

        var navigator = parentDocument.CreateNavigator();
        if (navigator == null) return null;

        dom.LoadXml(navigator.OuterXml);

        if (string.IsNullOrWhiteSpace(nodeQuery))
        {
            domNavigator.MoveToFirstChild();

            XPathNavigator? childNavigator = childDocument.CreateNavigator();
            if (childNavigator != null) domNavigator.AppendChild(childNavigator.OuterXml);
            domNavigator.MoveToRoot();
        }
        else
        {
            XPathNodeIterator nodes = domNavigator.Select(nodeQuery);
            foreach (XPathNavigator n in nodes)
            {
                XPathNavigator? childNavigator = childDocument.CreateNavigator();
                if (childNavigator != null) n.AppendChild(childNavigator.OuterXml);
                break;
            }
        }

        using var reader = new StringReader(domNavigator.OuterXml);
        var combinedDocument = new XPathDocument(reader);

        return combinedDocument;
    }
}
