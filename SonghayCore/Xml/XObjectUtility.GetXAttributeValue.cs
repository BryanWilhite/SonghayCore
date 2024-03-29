﻿namespace Songhay.Xml;

public static partial class XObjectUtility
{
    /// <summary>
    /// Returns an object for parsing
    /// and adding to a list of parameters for data access.
    /// </summary>
    /// <param name="node">The <see cref="XNode"/>.</param>
    /// <param name="nodeQuery">The node query <see cref="string"/>.</param>
    /// <param name="throwException">When <code>true</code>, throw an exception for null nodes.</param>
    public static string? GetXAttributeValue(XNode? node, string? nodeQuery, bool throwException) =>
        GetXAttributeValue(node, nodeQuery, throwException, null);

    /// <summary>
    /// Returns an object for parsing
    /// and adding to a list of parameters for data access.
    /// </summary>
    /// <param name="node">The <see cref="XNode"/>.</param>
    /// <param name="nodeQuery">The node query <see cref="string"/>.</param>
    /// <param name="throwException">When <code>true</code>, throw an exception for null nodes.</param>
    /// <param name="defaultValue">Return the specified default value for “zero-length” text nodes.</param>
    public static string?
        GetXAttributeValue(XNode? node, string? nodeQuery, bool throwException, string? defaultValue) =>
        GetXAttributeValue(node, nodeQuery, throwException, defaultValue, null);

    /// <summary>
    /// Returns an object for parsing
    /// and adding to a list of parameters for data access.
    /// </summary>
    /// <param name="node">The <see cref="XNode"/>.</param>
    /// <param name="nodeQuery">The node query <see cref="string"/>.</param>
    /// <param name="throwException">When <code>true</code>, throw an exception for null nodes.</param>
    /// <param name="defaultValue">Return the specified default value for “zero-length” text nodes.</param>
    /// <param name="resolver">The <see cref="IXmlNamespaceResolver"/> to use to resolve prefixes.</param>
    public static string? GetXAttributeValue(XNode? node, string? nodeQuery, bool throwException, string? defaultValue,
        IXmlNamespaceResolver? resolver)
    {
        if (node == null) return defaultValue;

        nodeQuery.ThrowWhenNullOrWhiteSpace();

        var a = resolver == null
            ? ((IEnumerable) node.XPathEvaluate(nodeQuery)).OfType<XAttribute>().FirstOrDefault()
            : ((IEnumerable) node.XPathEvaluate(nodeQuery, resolver)).OfType<XAttribute>().FirstOrDefault();

        return a switch
        {
            null when throwException => throw new XmlException($"Element at “{nodeQuery}” was not found."),
            null => defaultValue,
            _ => a.Value
        };
    }

    /// <summary>
    /// Returns an object for parsing
    /// and adding to a list of parameters for data access.
    /// </summary>
    /// <param name="node">The <see cref="XNode"/>.</param>
    /// <param name="nodeQuery">The XPath <see cref="string"/>.</param>
    /// <param name="throwException">
    /// When <code>true</code>, throw an exception for null nodes
    /// and nodes that do not parse into the specified type.
    /// </param>
    /// <param name="defaultValue">Return a boxing <see cref="Object"/> for “zero-length” text nodes.</param>
    /// <typeparam name="T">The type to parse from the node value.</typeparam>
    public static object?
        GetXAttributeValueAndParse<T>(XNode? node, string? nodeQuery, bool throwException, T defaultValue) =>
        GetXAttributeValueAndParse(node, nodeQuery, throwException, defaultValue, null);

    /// <summary>
    /// Returns an object for parsing
    /// and adding to a list of parameters for data access.
    /// </summary>
    /// <typeparam name="T">The type to parse from the node value.</typeparam>
    /// <param name="node">The <see cref="XNode"/>.</param>
    /// <param name="nodeQuery">The XPath <see cref="string"/>.</param>
    /// <param name="throwException">
    /// When <code>true</code>, throw an exception for null nodes
    /// and nodes that do not parse into the specified type.
    /// </param>
    /// <param name="defaultValue">Return a boxing <see cref="Object"/> for “zero-length” text nodes.</param>
    /// <param name="resolver">The <see cref="IXmlNamespaceResolver"/> to use to resolve prefixes.</param>
    public static object? GetXAttributeValueAndParse<T>(XNode? node, string? nodeQuery, bool throwException,
        T? defaultValue, IXmlNamespaceResolver? resolver)
    {
        object? o;

        var s = GetXAttributeValue(node, nodeQuery, throwException, null, resolver);

        try
        {
            if (string.IsNullOrWhiteSpace(s?.Trim()))
            {
                return defaultValue;
            }

            T? stronglyOfT = default(T);
            switch (stronglyOfT)
            {
                case bool:
                    o = bool.Parse(s);
                    break;
                case byte:
                    o = Byte.Parse(s, CultureInfo.InvariantCulture);
                    break;
                case DateTime:
                    o = DateTime.Parse(s, CultureInfo.InvariantCulture);
                    break;
                case decimal:
                    o = Decimal.Parse(s, CultureInfo.InvariantCulture);
                    break;
                case double:
                    o = Double.Parse(s, CultureInfo.InvariantCulture);
                    break;
                case short:
                    o = Int16.Parse(s, CultureInfo.InvariantCulture);
                    break;
                case int:
                    o = Int32.Parse(s, CultureInfo.InvariantCulture);
                    break;
                case long:
                    o = Int64.Parse(s, CultureInfo.InvariantCulture);
                    break;
                default:
                {
                    if (typeof(T).IsAssignableFrom(typeof(string)))
                    {
                        o = s;
                    }
                    else
                    {
                        Type t = typeof(T);
                        throw new NotSupportedException($"The specified type, “{t.FullName},” is not supported.");
                    }

                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Type t = typeof(T);
            var errMsg =
                $"Parse for “{t.FullName}” fails for element in “{nodeQuery}.” Value to parse: “{s ?? "Null"}.” Default Message: “{ex.Message}.”";
            throw new XmlException(errMsg);
        }

        return o;
    }
}
