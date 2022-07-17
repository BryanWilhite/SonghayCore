namespace Songhay.Xml;

public static partial class XObjectUtility
{
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
        var result = GetXObjects(node, nodeQuery, throwException, resolver).ToArray();

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

        var result = resolver == null
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
