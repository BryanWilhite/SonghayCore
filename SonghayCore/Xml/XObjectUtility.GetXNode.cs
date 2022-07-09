using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Songhay.Xml;

/// <summary>
/// Static helper members for XML-related routines.
/// </summary>
public static partial class XObjectUtility
{
    /// <summary>
    /// Gets the <see cref="XNode"/> from the specified XPath query.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="nodeQuery">The node query.</param>
    /// <returns></returns>
    public static XNode? GetXNode(XNode? node, string? nodeQuery) => GetXObject(node, nodeQuery) as XNode;

    /// <summary>
    /// Gets the <see cref="XNode"/> from the specified XPath query.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="nodeQuery">The node query.</param>
    /// <param name="throwException">if node to <c>true</c> [throw exception].</param>
    /// <returns></returns>
    public static XNode? GetXNode(XNode? node, string? nodeQuery, bool throwException) =>
        GetXObject(node, nodeQuery, throwException) as XNode;

    /// <summary>
    /// Gets the <see cref="XNode"/> from the specified XPath query.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="nodeQuery">The node query.</param>
    /// <param name="throwException">if node to <c>true</c> [throw exception].</param>
    /// <param name="resolver">The resolver.</param>
    /// <returns></returns>
    public static XNode? GetXNode(XNode? node, string? nodeQuery, bool throwException, IXmlNamespaceResolver resolver) =>
        GetXObject(node, nodeQuery, throwException, resolver) as XNode;

    /// <summary>
    /// Gets <see cref="IEnumerable{XNode}"/> from the specified XPath query.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="nodeQuery">The node query.</param>
    /// <returns></returns>
    public static IEnumerable<XNode> GetXNodes(XNode? node, string? nodeQuery)
    {
        var nodes = GetXObjects(node, nodeQuery);

        return nodes.OfType<XNode>();
    }

    /// <summary>
    /// Gets <see cref="IEnumerable{XNode}"/> from the specified XPath query.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="nodeQuery">The node query.</param>
    /// <param name="throwException">if node to <c>true</c> [throw exception].</param>
    /// <returns></returns>
    public static IEnumerable<XNode> GetXNodes(XNode? node, string? nodeQuery, bool throwException)
    {
        var nodes = GetXObjects(node, nodeQuery, throwException);

        return nodes.OfType<XNode>();
    }

    /// <summary>
    /// Gets <see cref="IEnumerable{XNode}"/> from the specified XPath query.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="nodeQuery">The node query.</param>
    /// <param name="throwException">if node to <c>true</c> [throw exception].</param>
    /// <param name="resolver">The resolver.</param>
    /// <returns></returns>
    public static IEnumerable<XNode> GetXNodes(XNode? node, string? nodeQuery, bool throwException, IXmlNamespaceResolver? resolver)
    {
        var nodes = GetXObjects(node, nodeQuery, throwException, resolver);

        return nodes.OfType<XNode>();
    }
}
