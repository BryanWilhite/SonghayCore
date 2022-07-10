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
    /// An alternative to <see cref="System.Xml.XPath.XPathNavigator.GetAttribute"/>.
    /// </summary>
    /// <param name="navigable">
    /// The source <see cref="System.Xml.XPath.IXPathNavigable"/>.
    /// </param>
    /// <param name="nodeQuery">
    /// The value to be compiled
    /// into an <see cref="System.Xml.XPath.XPathExpression"/>.
    /// </param>
    [Obsolete("Should be replaced with XmlUtility.GetNodeValue.")]
    public static string? GetAttributeValue(IXPathNavigable? navigable, string? nodeQuery)
    {
        XPathNavigator? node = GetNavigableNode(navigable, nodeQuery);

        return node?.Value;
    }

    /// <summary>
    /// An alternative to <see cref="System.Xml.XPath.XPathNavigator.GetAttribute"/>.
    /// </summary>
    /// <param name="navigable">
    /// The source <see cref="System.Xml.XPath.IXPathNavigable"/>.
    /// </param>
    /// <param name="nodeQuery">
    /// The value to be compiled
    /// into an <see cref="System.Xml.XPath.XPathExpression"/>.
    /// </param>
    /// <param name="nsMan">
    /// The <see cref="System.Xml.XmlNamespaceManager"/>
    /// to use to resolve prefixes.
    /// </param>
    [Obsolete("Should be replaced with XmlUtility.GetNodeValue.")]
    public static string? GetAttributeValue(IXPathNavigable? navigable, string? nodeQuery, XmlNamespaceManager nsMan)
    {
        XPathNavigator? node = GetNavigableNode(navigable, nodeQuery, nsMan);

        return node?.Value;
    }
}
