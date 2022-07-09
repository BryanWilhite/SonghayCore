using System;
using System.Xml.Linq;

namespace Songhay.Xml;

/// <summary>
/// Static helper members for XML-related routines.
/// </summary>
public static partial class XObjectUtility
{
    /// <summary>
    /// Get the CDATA value from the specified <see cref="XElement"/>.
    /// </summary>
    /// <param name="element">The <see cref="XElement"/></param>
    public static string? GetCDataValue(XElement? element) => GetCDataValue(element?.FirstNode);

    /// <summary>
    /// Get the CDATA value from the specified <see cref="XNode"/>.
    /// </summary>
    /// <param name="node">The <see cref="XNode"/></param>
    public static string? GetCDataValue(XNode? node)
    {
        var cData = node as XCData;

        return cData?.Value;
    }

    /// <summary>
    /// Gets the <see cref="XNode" /> into a <c>local-name()</c>, XPath-predicate query.
    /// </summary>
    /// <param name="childElementName">Name of the child element.</param>
    /// <returns></returns>
    public static string? GetLocalNameXPathQuery(string? childElementName) =>
        GetLocalNameXPathQuery(namespacePrefixOrUri: null, childElementName: childElementName,
            childAttributeName: null);

    /// <summary>
    /// Gets the <see cref="XNode" /> into a <c>local-name()</c>, XPath-predicate query.
    /// </summary>
    /// <param name="namespacePrefixOrUri">The namespace prefix or URI.</param>
    /// <param name="childElementName">Name of the child element.</param>
    /// <returns></returns>
    public static string? GetLocalNameXPathQuery(string? namespacePrefixOrUri, string? childElementName) =>
        GetLocalNameXPathQuery(namespacePrefixOrUri, childElementName, childAttributeName: null);

    /// <summary>
    /// Gets the <see cref="XNode" /> into a <c>local-name()</c>, XPath-predicate query.
    /// </summary>
    /// <param name="namespacePrefixOrUri">The namespace prefix or URI.</param>
    /// <param name="childElementName">Name of the child element.</param>
    /// <param name="childAttributeName">Name of the child attribute.</param>
    /// <returns></returns>
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
    /// Gets the element or attribute value of the specified element.
    /// </summary>
    /// <param name="currentNode">The current </param>
    /// <param name="query">The xpath query.</param>
    /// <param name="throwException">if set to <c>true</c> throw exception.</param>
    public static string? GetValue(XNode? currentNode, string? query, bool throwException)
    {
        var node = GetXObject(currentNode, query);

        return node switch
        {
            null when throwException => throw new ArgumentException("The XPath query returns a null node", "query"),
            null => null,
            _ => node.NodeType switch
            {
                System.Xml.XmlNodeType.Element => (node as XElement)?.Value,
                System.Xml.XmlNodeType.Attribute => GetXAttributeValue(currentNode, query, throwException),
                _ => null
            }
        };
    }
}
