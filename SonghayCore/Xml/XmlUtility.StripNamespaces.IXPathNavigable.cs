using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml.XPath;

namespace Songhay.Xml;

public static partial class XmlUtility
{
    /// <summary>
    /// Strip the namespaces from specified document.
    /// </summary>
    /// <param name="navigableSet">
    /// The source <see cref="System.Xml.XPath.IXPathNavigable"/> document.
    /// </param>
    /// <remarks>
    /// WARNING: Stripping namespaces “flattens” the document
    /// and can cause local-name collisions.
    /// 
    /// This routine does not remove namespace prefixes.
    /// 
    /// </remarks>
    [SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes",
        Justification = "Specific functionality provided by the concrete type may be required.")]
    public static XPathDocument? StripNamespaces(IXPathNavigable? navigableSet) => StripNamespaces(navigableSet, false);

    /// <summary>
    /// Strip the namespaces from specified document.
    /// </summary>
    /// <param name="navigableSet">
    /// The source <see cref="System.Xml.XPath.IXPathNavigable"/> document.
    /// </param>
    /// <param name="removeDocType">
    /// When <c>true</c>, removes any DOCTYPE preambles.
    /// </param>
    /// <remarks>
    /// WARNING: Stripping namespaces “flattens” the document
    /// and can cause local-name collisions.
    /// 
    /// This routine does not remove namespace prefixes.
    /// 
    /// </remarks>
    [SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes",
        Justification = "Specific functionality provided by the concrete type may be required.")]
    public static XPathDocument? StripNamespaces(IXPathNavigable? navigableSet, bool removeDocType)
    {
        if(navigableSet == null) return null;

        XPathNavigator? navigator = navigableSet.CreateNavigator();

        string? xmlString = StripNamespaces(navigator?.OuterXml, removeDocType);

        using StringReader s = new StringReader(xmlString!);

        var newXml = new XPathDocument(s);

        return newXml;
    }
}
