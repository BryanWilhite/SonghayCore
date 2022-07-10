namespace Songhay.Xml;

/// <summary>
/// Static helper members for XML-related routines.
/// </summary>
public static partial class XObjectUtility
{
    /// <summary>
    /// Returns an object for parsing
    /// and adding to a list of parameters for data access.
    /// </summary>
    /// <param name="node">The <see cref="System.Xml.Linq.XNode"/></param>
    /// <param name="nodeQuery">The <see cref="System.String"/></param>
    /// <param name="throwException">When <code>true</code>, throw an exception for null nodes.</param>
    public static string? GetXAttributeValue(XNode? node, string? nodeQuery, bool throwException) =>
        GetXAttributeValue(node, nodeQuery, throwException, null);

    /// <summary>
    /// Returns an object for parsing
    /// and adding to a list of parameters for data access.
    /// </summary>
    /// <param name="node">The <see cref="System.Xml.Linq.XNode"/></param>
    /// <param name="nodeQuery">The <see cref="System.String"/></param>
    /// <param name="throwException">When <code>true</code>, throw an exception for null nodes.</param>
    /// <param name="defaultValue">Return the specified default value for “zero-length” text nodes</param>
    public static string?
        GetXAttributeValue(XNode? node, string? nodeQuery, bool throwException, string? defaultValue) =>
        GetXAttributeValue(node, nodeQuery, throwException, defaultValue, null);

    /// <summary>
    /// Returns an object for parsing
    /// and adding to a list of parameters for data access.
    /// </summary>
    /// <param name="node">The <see cref="System.Xml.Linq.XNode"/></param>
    /// <param name="nodeQuery">The <see cref="System.String"/></param>
    /// <param name="throwException">When <code>true</code>, throw an exception for null nodes.</param>
    /// <param name="defaultValue">Return the specified default value for “zero-length” text nodes</param>
    /// <param name="resolver">
    /// The <see cref="System.Xml.IXmlNamespaceResolver"/>
    /// to use to resolve prefixes.
    /// </param>
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
            null when throwException => throw new XmlException(string.Format(CultureInfo.CurrentCulture,
                "Element at “{0}” was not found.", nodeQuery)),
            null => defaultValue,
            _ => a.Value
        };
    }

    /// <summary>
    /// Returns an object for parsing
    /// and adding to a list of parameters for data access.
    /// </summary>
    /// <param name="node">The <see cref="System.Xml.Linq.XNode"/></param>
    /// <param name="nodeQuery">The XPath <see cref="System.String"/>.</param>
    /// <param name="throwException">When <code>true</code>, throw an exception for null nodes and nodes that do not parse into the specified type.</param>
    /// <param name="defaultValue">Return a boxing <see cref="System.Object"/> for “zero-length” text nodes.</param>
    /// <typeparam name="T">The type to parse from the node value.</typeparam>
    public static object?
        GetXAttributeValueAndParse<T>(XNode? node, string? nodeQuery, bool throwException, T defaultValue) =>
        GetXAttributeValueAndParse(node, nodeQuery, throwException, defaultValue, null);

    /// <summary>
    /// Returns an object for parsing
    /// and adding to a list of parameters for data access.
    /// </summary>
    /// <typeparam name="T">The type to parse from the node value.</typeparam>
    /// <param name="node">The <see cref="System.Xml.Linq.XNode"/>.</param>
    /// <param name="nodeQuery">The XPath <see cref="System.String"/>.</param>
    /// <param name="throwException">When <code>true</code>, throw an exception for null nodes and nodes that do not parse into the specified type.</param>
    /// <param name="defaultValue">Return a boxing <see cref="System.Object"/> for “zero-length” text nodes.</param>
    /// <param name="resolver">The <see cref="System.Xml.IXmlNamespaceResolver"/>
    /// to use to resolve prefixes.</param>
    /// <returns></returns>
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
                        throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture,
                            "The specified type, “{0},” is not supported.", t.FullName));
                    }

                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Type t = typeof(T);
            var errMsg = string.Format(CultureInfo.CurrentCulture,
                "Parse for “{0}” fails for element in “{1}.” Value to parse: “{2}.” Default Message: “{3}.”",
                t.FullName, nodeQuery, s ?? "Null", ex.Message);
            throw new XmlException(errMsg);
        }

        return o;
    }
}
