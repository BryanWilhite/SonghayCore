namespace Songhay.Xml;

public static partial class XObjectUtility
{
    /// <summary>
    /// Gets the <see cref="XElement" />.
    /// </summary>
    /// <param name="rootElement">The root element name.</param>
    /// <param name="innerXml">The inner XML.</param>
    public static XElement GetXElement(string? rootElement, object? innerXml)
    {
        rootElement.ThrowWhenNullOrWhiteSpace();

        return XElement.Parse($"<{rootElement}>{innerXml}</{rootElement}>");
    }

    /// <summary>
    /// Gets the <see cref="XElement" />.
    /// </summary>
    /// <param name="root">The root <see cref="XNode"/>.</param>
    /// <param name="pathToElement">The XPath to element.</param>
    public static XElement? GetXElement(XNode? root, string? pathToElement)
    {
        ArgumentNullException.ThrowIfNull(root);
        pathToElement.ThrowWhenNullOrWhiteSpace();

        return root.XPathSelectElement(pathToElement);
    }

    /// <summary>
    /// Gets the <see cref="IEnumerable{XElement}"/>.
    /// </summary>
    /// <param name="currentElement">The current element.</param>
    /// <param name="query">The xpath query.</param>
    public static IEnumerable<XElement> GetXElements(XNode? currentElement, string? query) =>
        GetXNodes(currentElement, query).OfType<XElement>();
}
