using System;
using System.Xml;
using System.Xml.XPath;

namespace Songhay.Xml;

public static partial class XmlUtility
{
    /// <summary>
    /// Returns true when the node has the specified value.
    /// </summary>
    /// <param name="node">The <see cref="System.Xml.XPath.IXPathNavigable"/></param>
    /// <param name="nodeQuery">The query <see cref="System.String"/></param>
    /// <param name="throwException">When <code>true</code>, throw an exception for null nodes.</param>
    /// <param name="testValue">The specified value to test with the node value.</param>
    public static bool IsNodeValue(IXPathNavigable? node, string? nodeQuery, bool throwException, string? testValue)
    {
        string? s = GetNodeValue(node, nodeQuery, throwException) as string;

        return string.Equals(s, testValue, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Returns true when the node has the specified value.
    /// </summary>
    /// <param name="node">The <see cref="System.Xml.XPath.IXPathNavigable"/></param>
    /// <param name="nodeQuery">The query <see cref="System.String"/></param>
    /// <param name="throwException">When <code>true</code>, throw an exception for null nodes.</param>
    /// <param name="testValue">The specified value to test with the node value.</param>
    /// <param name="comparisonType">The <see cref="System.StringComparison"/> type.</param>
    public static bool IsNodeValue(IXPathNavigable? node, string? nodeQuery, bool throwException, string? testValue,
        StringComparison comparisonType)
    {
        string? s = GetNodeValue(node, nodeQuery, throwException) as string;

        return string.Equals(s, testValue, comparisonType);
    }

    /// <summary>
    /// Returns true when the node has the specified value.
    /// </summary>
    /// <param name="node">The <see cref="System.Xml.XPath.IXPathNavigable"/></param>
    /// <param name="nodeQuery">The query <see cref="System.String"/></param>
    /// <param name="throwException">When <code>true</code>, throw an exception for null nodes.</param>
    /// <param name="testValue">The specified value to test with the node value.</param>
    /// <param name="nsMan">
    /// The <see cref="System.Xml.XmlNamespaceManager"/>
    /// to use to resolve prefixes.
    /// </param>
    public static bool IsNodeValue(IXPathNavigable? node, string? nodeQuery, bool throwException, string? testValue,
        XmlNamespaceManager nsMan)
    {
        string? s = GetNodeValue(node, nodeQuery, throwException, null, nsMan) as string;

        return string.Equals(s, testValue);
    }

    /// <summary>
    /// Returns true when the node has the specified value.
    /// </summary>
    /// <param name="node">The <see cref="System.Xml.XPath.IXPathNavigable"/></param>
    /// <param name="nodeQuery">The query <see cref="System.String"/></param>
    /// <param name="throwException">When <code>true</code>, throw an exception for null nodes.</param>
    /// <param name="testValue">The specified value to test with the node value.</param>
    /// <param name="nsMan">
    /// The <see cref="System.Xml.XmlNamespaceManager"/>
    /// to use to resolve prefixes.
    /// </param>
    /// <param name="comparisonType">The <see cref="System.StringComparison"/> type.</param>
    public static bool IsNodeValue(IXPathNavigable? node, string? nodeQuery, bool throwException, string? testValue,
        XmlNamespaceManager nsMan, StringComparison comparisonType)
    {
        string? s = GetNodeValue(node, nodeQuery, throwException, null, nsMan) as string;

        return string.Equals(s, testValue, comparisonType);
    }

    /// <summary>
    /// Returns true when the node has the value “no”.
    /// </summary>
    /// <param name="node">The <see cref="System.Xml.XPath.IXPathNavigable"/></param>
    /// <param name="nodeQuery">The query <see cref="System.String"/></param>
    /// <param name="throwException">When <code>true</code>, throw an exception for null nodes.</param>
    public static bool IsNoNodeValue(IXPathNavigable? node, string? nodeQuery, bool throwException) =>
        IsNodeValue(node, nodeQuery, throwException, "no", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Returns true when the node has the value “yes”.
    /// </summary>
    /// <param name="node">The <see cref="System.Xml.XPath.IXPathNavigable"/></param>
    /// <param name="nodeQuery">The query <see cref="System.String"/></param>
    /// <param name="throwException">When <code>true</code>, throw an exception for null nodes.</param>
    public static bool IsYesNodeValue(IXPathNavigable? node, string? nodeQuery, bool throwException) =>
        IsNodeValue(node, nodeQuery, throwException, "yes", StringComparison.OrdinalIgnoreCase);
}
