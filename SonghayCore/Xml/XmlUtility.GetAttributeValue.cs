namespace Songhay.Xml;

public static partial class XmlUtility
{
    /// <summary>
    /// An alternative to <see cref="XPathNavigator.GetAttribute"/>.
    /// </summary>
    /// <param name="navigable">
    /// The source <see cref="IXPathNavigable"/>.
    /// </param>
    /// <param name="nodeQuery">
    /// The value to be compiled
    /// into an <see cref="XPathExpression"/>.
    /// </param>
    [Obsolete("Should be replaced with XmlUtility.GetNodeValue.")]
    public static string? GetAttributeValue(IXPathNavigable? navigable, string? nodeQuery)
    {
        XPathNavigator? node = GetNavigableNode(navigable, nodeQuery);

        return node?.Value;
    }

    /// <summary>
    /// An alternative to <see cref="XPathNavigator.GetAttribute"/>.
    /// </summary>
    /// <param name="navigable">
    /// The source <see cref="IXPathNavigable"/>.
    /// </param>
    /// <param name="nodeQuery">
    /// The value to be compiled
    /// into an <see cref="XPathExpression"/>.
    /// </param>
    /// <param name="nsMan">
    /// The <see cref="XmlNamespaceManager"/>
    /// to use to resolve prefixes.
    /// </param>
    [Obsolete("Should be replaced with XmlUtility.GetNodeValue.")]
    public static string? GetAttributeValue(IXPathNavigable? navigable, string? nodeQuery, XmlNamespaceManager nsMan)
    {
        XPathNavigator? node = GetNavigableNode(navigable, nodeQuery, nsMan);

        return node?.Value;
    }
}
