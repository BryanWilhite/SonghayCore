namespace Songhay.Xml;

public static partial class XObjectUtility
{
    /// <summary>
    /// Gets the <see cref="XNode"/> from the specified XPath query.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="nodeQuery">The node query.</param>
    public static XNode? GetXNode(XNode? node, string? nodeQuery) => GetXObject(node, nodeQuery) as XNode;

    /// <summary>
    /// Gets the <see cref="XNode"/> from the specified XPath query.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="nodeQuery">The node query.</param>
    /// <param name="throwException">When <code>true</code>, throw an exception for null nodes.</param>
    public static XNode? GetXNode(XNode? node, string? nodeQuery, bool throwException) =>
        GetXObject(node, nodeQuery, throwException) as XNode;

    /// <summary>
    /// Gets the <see cref="XNode"/> from the specified XPath query.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="nodeQuery">The node query.</param>
    /// <param name="throwException">When <code>true</code>, throw an exception for null nodes.</param>
    /// <param name="resolver">The resolver.</param>
    public static XNode? GetXNode(XNode? node, string? nodeQuery, bool throwException,
        IXmlNamespaceResolver resolver) =>
        GetXObject(node, nodeQuery, throwException, resolver) as XNode;

    /// <summary>
    /// Gets <see cref="IEnumerable{XNode}"/> from the specified XPath query.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="nodeQuery">The node query.</param>
    public static IEnumerable<XNode> GetXNodes(XNode? node, string? nodeQuery) =>
        GetXObjects(node, nodeQuery).OfType<XNode>();

    /// <summary>
    /// Gets <see cref="IEnumerable{XNode}"/> from the specified XPath query.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="nodeQuery">The node query.</param>
    /// <param name="throwException">When <code>true</code>, throw an exception for null nodes.</param>
    public static IEnumerable<XNode> GetXNodes(XNode? node, string? nodeQuery, bool throwException) =>
        GetXObjects(node, nodeQuery, throwException).OfType<XNode>();

    /// <summary>
    /// Gets <see cref="IEnumerable{XNode}"/> from the specified XPath query.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="nodeQuery">The node query.</param>
    /// <param name="throwException">When <code>true</code>, throw an exception for null nodes.</param>
    /// <param name="resolver">The resolver.</param>
    public static IEnumerable<XNode> GetXNodes(XNode? node, string? nodeQuery, bool throwException,
        IXmlNamespaceResolver? resolver) =>
        GetXObjects(node, nodeQuery, throwException, resolver).OfType<XNode>();
}
