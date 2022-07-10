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
    /// Returns an <see cref="System.Xml.XPath.XPathNavigator"/>
    /// based on the nodeQuery Expression toward the source document.
    /// </summary>
    /// <param name="navigable">
    /// The source <see cref="System.Xml.XPath.IXPathNavigable"/> document.
    /// </param>
    /// <param name="nodeQuery">
    /// The value to be compiled
    /// into an <see cref="System.Xml.XPath.XPathExpression"/>.
    /// </param>
    public static XPathNavigator? GetNavigableNode(IXPathNavigable? navigable, string? nodeQuery)
    {
        if (navigable == null) return null;
        if (string.IsNullOrWhiteSpace(nodeQuery)) return null;

        XPathNavigator? navigator = navigable.CreateNavigator();
        XPathExpression xpath = XPathExpression.Compile(nodeQuery);
        var node = navigator?.SelectSingleNode(xpath);

        return node;
    }

    /// <summary>
    /// Returns an <see cref="System.Xml.XPath.XPathNavigator"/>
    /// based on the nodeQuery Expression toward the source document.
    /// </summary>
    /// <param name="navigable">
    /// The source <see cref="System.Xml.XPath.IXPathNavigable"/> document.
    /// </param>
    /// <param name="nodeQuery">
    /// The value to be compiled
    /// into an <see cref="System.Xml.XPath.XPathExpression"/>.
    /// </param>
    /// <param name="nsMan">
    /// The <see cref="System.Xml.XmlNamespaceManager"/>
    /// to use to resolve prefixes.
    /// </param>
    public static XPathNavigator? GetNavigableNode(IXPathNavigable? navigable, string? nodeQuery,
        XmlNamespaceManager? nsMan)
    {
        if (navigable == null) return null;
        if (string.IsNullOrWhiteSpace(nodeQuery)) return null;

        XPathNavigator? navigator = navigable.CreateNavigator();
        XPathExpression xpath = XPathExpression.Compile(nodeQuery, nsMan);
        var node = navigator?.SelectSingleNode(xpath);

        return node;
    }

    /// <summary>
    /// Returns an <see cref="System.Xml.XPath.XPathNodeIterator"/>
    /// based on the nodeQuery Expression toward the source document.
    /// </summary>
    /// <param name="navigable">
    /// The source <see cref="System.Xml.XPath.IXPathNavigable"/> document.
    /// </param>
    /// <param name="nodeQuery">
    /// The value to be compiled
    /// into an <see cref="System.Xml.XPath.XPathExpression"/>.
    /// </param>
    public static XPathNodeIterator? GetNavigableNodes(IXPathNavigable? navigable, string? nodeQuery)
    {
        if (navigable == null) return null;
        if (string.IsNullOrWhiteSpace(nodeQuery)) return null;

        XPathNavigator? navigator = navigable.CreateNavigator();

        XPathExpression xpath = XPathExpression.Compile(nodeQuery);
        var nodes = navigator?.Select(xpath);

        return nodes;
    }

    /// <summary>
    /// Returns an <see cref="System.Xml.XPath.XPathNodeIterator"/>
    /// based on the nodeQuery Expression toward the source document.
    /// </summary>
    /// <param name="navigable">
    /// The source <see cref="System.Xml.XPath.IXPathNavigable"/> document.
    /// </param>
    /// <param name="nodeQuery">
    /// The value to be compiled
    /// into an <see cref="System.Xml.XPath.XPathExpression"/>.
    /// </param>
    /// <param name="nsMan">
    /// The <see cref="System.Xml.XmlNamespaceManager"/>
    /// to use to resolve prefixes.
    /// </param>
    public static XPathNodeIterator? GetNavigableNodes(IXPathNavigable? navigable, string? nodeQuery,
        XmlNamespaceManager nsMan)
    {
        if (navigable == null) return null;
        if (string.IsNullOrWhiteSpace(nodeQuery)) return null;

        XPathNavigator? navigator = navigable.CreateNavigator();

        XPathExpression xpath = XPathExpression.Compile(nodeQuery, nsMan);
        var nodes = navigator?.Select(xpath);

        return nodes;
    }
}
