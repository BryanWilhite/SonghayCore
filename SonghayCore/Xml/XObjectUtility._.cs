namespace Songhay.Xml;

/// <summary>
/// Static helper members for XML-related routines.
/// </summary>
/// <remarks>
/// These definitions are biased toward emitting <see cref="XPathDocument"/> documents.
/// However, many accept any input implementing the <see cref="IXPathNavigable"/> interface.
/// </remarks>
public static partial class XObjectUtility
{
    /// <summary>
    /// Get the CDATA value from the specified <see cref="XElement"/>.
    /// </summary>
    /// <param name="element">The <see cref="XElement"/>.</param>
    public static string? GetCDataValue(XElement? element) => GetCDataValue(element?.FirstNode);

    /// <summary>
    /// Get the CDATA value from the specified <see cref="XNode"/>.
    /// </summary>
    /// <param name="node">The <see cref="XNode"/>.</param>
    public static string? GetCDataValue(XNode? node) => (node as XCData)?.Value;

    /// <summary>
    /// Gets the <see cref="XNode" /> into a <c>local-name()</c>, XPath-predicate query.
    /// </summary>
    /// <param name="childElementName">Name of the child element.</param>
    public static string? GetLocalNameXPathQuery(string? childElementName) =>
        GetLocalNameXPathQuery(namespacePrefixOrUri: null, childElementName: childElementName,
            childAttributeName: null);

    /// <summary>
    /// Gets the <see cref="XNode" /> into a <c>local-name()</c>, XPath-predicate query.
    /// </summary>
    /// <param name="namespacePrefixOrUri">The namespace prefix or URI.</param>
    /// <param name="childElementName">Name of the child element.</param>
    public static string? GetLocalNameXPathQuery(string? namespacePrefixOrUri, string? childElementName) =>
        GetLocalNameXPathQuery(namespacePrefixOrUri, childElementName, childAttributeName: null);

    /// <summary>
    /// Gets the <see cref="XNode" /> into a <c>local-name()</c>, XPath-predicate query.
    /// </summary>
    /// <param name="namespacePrefixOrUri">The namespace prefix or URI.</param>
    /// <param name="childElementName">Name of the child element.</param>
    /// <param name="childAttributeName">Name of the child attribute.</param>
    /// <remarks>
    /// This routine is useful when namespace-resolving is not desirable or available.
    /// </remarks>
    public static string? GetLocalNameXPathQuery(string? namespacePrefixOrUri, string? childElementName,
        string? childAttributeName)
    {
        if (string.IsNullOrWhiteSpace(childElementName)) return null;

        if (string.IsNullOrWhiteSpace(childAttributeName))
        {
            return string.IsNullOrWhiteSpace(namespacePrefixOrUri)
                ? $"./*[local-name()='{childElementName}']"
                : $"./*[namespace-uri()='{namespacePrefixOrUri}' and local-name()='{childElementName}']";
        }

        return string.IsNullOrWhiteSpace(namespacePrefixOrUri)
            ? $"./*[local-name()='{childElementName}']/@{childAttributeName}"
            : $"./*[namespace-uri()='{namespacePrefixOrUri}' and local-name()='{childElementName}']/@{childAttributeName}";
    }

    /// <summary>
    /// Gets the element or attribute value of the specified element.
    /// </summary>
    /// <param name="currentNode">The current element.</param>
    /// <param name="query">The xpath query.</param>
    public static string? GetValue(XNode? currentNode, string? query) => GetValue(currentNode, query, true);

    /// <summary>
    /// Gets the element or attribute value of the current element.
    /// </summary>
    /// <param name="currentNode">The current <see cref="XNode"/>.</param>
    /// <param name="query">The xpath query.</param>
    /// <param name="throwException">if set to <c>true</c> throw exception.</param>
    public static string? GetValue(XNode? currentNode, string? query, bool throwException)
    {
        XObject? node = GetXObject(currentNode, query);

        return node switch
        {
            null when throwException => throw new ArgumentException("The XPath query returns a null node", "query"),
            null => null,
            _ => node.NodeType switch
            {
                XmlNodeType.Element => (node as XElement)?.Value,
                XmlNodeType.Attribute => GetXAttributeValue(currentNode, query, throwException),
                _ => null
            }
        };
    }

    /// <summary>
    /// Gets the <see cref="XDeclaration"/>.
    /// </summary>
    public static XDeclaration GetXDeclaration() => GetXDeclaration(XEncoding.Utf08, true);

    /// <summary>
    /// Gets the <see cref="XDeclaration"/>.
    /// </summary>
    /// <param name="encoding">The encoding (<see cref="XEncoding.Utf08"/> by default).</param>
    /// <param name="isStandAlone">When <c>true</c> document is stand-alone (<c>true</c> by default).</param>
    public static XDeclaration GetXDeclaration(string? encoding, bool isStandAlone) =>
        new("1.0", encoding, isStandAlone ? "yes" : "no");

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

    /// <summary>
    /// Gets the <see cref="XObject"/> from the specified XPath query.
    /// </summary>
    /// <param name="node">The <see cref="XNode"/> node.</param>
    /// <param name="nodeQuery">The node query <see cref="string"/>.</param>
    public static XObject? GetXObject(XNode? node, string? nodeQuery) =>
        GetXObject(node, nodeQuery, throwException: false, resolver: null);

    /// <summary>
    /// Gets the <see cref="XObject"/> from the specified XPath query.
    /// </summary>
    /// <param name="node">The <see cref="XNode"/> node.</param>
    /// <param name="nodeQuery">The node query <see cref="string"/>.</param>
    /// <param name="throwException">When <code>true</code>, throw an exception for null nodes.</param>
    public static XObject? GetXObject(XNode? node, string? nodeQuery, bool throwException) =>
        GetXObject(node, nodeQuery, throwException, resolver: null);

    /// <summary>
    /// Gets the <see cref="XObject"/>.
    /// </summary>
    /// <param name="node">The <see cref="XNode"/> node.</param>
    /// <param name="nodeQuery">The node query <see cref="string"/>.</param>
    /// <param name="throwException">When <code>true</code>, throw an exception for null nodes.</param>
    /// <param name="resolver">
    /// The <see cref="IXmlNamespaceResolver"/>
    /// to use to resolve prefixes.
    /// </param>
    public static XObject? GetXObject(XNode? node, string? nodeQuery, bool throwException,
        IXmlNamespaceResolver? resolver)
    {
        XObject[] result = GetXObjects(node, nodeQuery, throwException, resolver).ToArray();

        return result.Any() ? result.FirstOrDefault() : null;
    }

    /// <summary>
    /// Gets <see cref="IEnumerable{XObject}"/> from the specified XPath query.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="nodeQuery">The node query.</param>
    public static IEnumerable<XObject> GetXObjects(XNode? node, string? nodeQuery) =>
        GetXObjects(node, nodeQuery, throwException: false, resolver: null);

    /// <summary>
    /// Gets <see cref="IEnumerable{XObject}"/> from the specified XPath query.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="nodeQuery">The node query.</param>
    /// <param name="throwException">When <code>true</code>, throw an exception for null nodes.</param>
    public static IEnumerable<XObject> GetXObjects(XNode? node, string? nodeQuery, bool throwException) =>
        GetXObjects(node, nodeQuery, throwException, resolver: null);

    /// <summary>
    /// Gets <see cref="IEnumerable{XObject}"/> from the specified XPath query.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="nodeQuery">The node query.</param>
    /// <param name="throwException">When <code>true</code>, throw an exception for null nodes.</param>
    /// <param name="resolver">The resolver.</param>
    public static IEnumerable<XObject> GetXObjects(XNode? node, string? nodeQuery, bool throwException,
        IXmlNamespaceResolver? resolver)
    {
        nodeQuery.ThrowWhenNullOrWhiteSpace();

        if (node == null) return Enumerable.Empty<XObject>();

        IEnumerable<XObject>? result = resolver == null
            ? ((IEnumerable) node.XPathEvaluate(nodeQuery)).OfType<XObject>()
            : ((IEnumerable) node.XPathEvaluate(nodeQuery, resolver)).OfType<XObject>();

        return result switch
        {
            null when throwException =>
                throw new XmlException($"Element at “{nodeQuery}” was not found."),
            null => Enumerable.Empty<XObject>(),
            _ => result
        };
    }
}
