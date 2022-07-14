namespace Songhay.Xml;

public static partial class XmlUtility
{
    /// <summary>
    /// Returns an object for parsing
    /// and adding to a list of parameters for data access.
    /// </summary>
    /// <param name="node">The <see cref="IXPathNavigable"/> node.</param>
    /// <param name="nodeQuery">The node query <see cref="string"/>.</param>
    /// <param name="throwException">When <c>true</c>, throw an exception for null nodes.</param>
    public static object? GetNodeValue(IXPathNavigable? node, string? nodeQuery, bool throwException) =>
        GetNodeValue(node, nodeQuery, throwException, null);

    /// <summary>
    /// Returns an object for parsing
    /// and adding to a list of parameters for data access.
    /// </summary>
    /// <param name="node">The <see cref="IXPathNavigable"/> node.</param>
    /// <param name="nodeQuery">The node query <see cref="string"/>.</param>
    /// <param name="throwException">When <c>true</c>, throw an exception for null nodes.</param>
    /// <param name="defaultValue">Return the specified default value for “zero-length” text nodes.</param>
    public static object? GetNodeValue(IXPathNavigable? node, string? nodeQuery, bool throwException,
        object? defaultValue) => GetNodeValue(node, nodeQuery, throwException, defaultValue, null);

    /// <summary>
    /// Returns an object for parsing
    /// and adding to a list of parameters for data access.
    /// </summary>
    /// <param name="node">The <see cref="IXPathNavigable"/> node.</param>
    /// <param name="nodeQuery">The node query <see cref="string"/>.</param>
    /// <param name="throwException">When <c>true</c>, throw an exception for null nodes.</param>
    /// <param name="defaultValue">Return the specified default value for “zero-length” text nodes.</param>
    /// <param name="nsMan">
    /// The <see cref="XmlNamespaceManager"/>
    /// to use to resolve prefixes.
    /// </param>
    public static object? GetNodeValue(IXPathNavigable? node, string? nodeQuery, bool throwException,
        object? defaultValue,
        XmlNamespaceManager? nsMan)
    {
        XPathNavigator? n = GetNavigableNode(node, nodeQuery, nsMan);
        object? p = defaultValue;

        if (n != null)
        {
            if (n.Value.Trim().Length > 0) p = n.Value.Trim();
        }
        else if (throwException)
        {
            throw new XmlException($"Element at “{nodeQuery}” was not found.");
        }

        return p;
    }

    /// <summary>
    /// Returns an object for parsing
    /// and adding to a list of parameters for data access.
    /// </summary>
    /// <param name="node">The <see cref="IXPathNavigable"/> node.</param>
    /// <param name="nodeQuery">The XPath <see cref="string"/>.</param>
    /// <param name="throwException">When <c>true</c>, throw an exception for null nodes and nodes that do not parse into the specified type.</param>
    /// <param name="defaultValue">Return a boxing <see cref="Object"/> for “zero-length” text nodes.</param>
    /// <typeparam name="T">The type to parse from the node value.</typeparam>
    public static object? GetNodeValueAndParse<T>(IXPathNavigable? node, string? nodeQuery, bool throwException,
        T defaultValue) => GetNodeValueAndParse(node, nodeQuery, throwException, defaultValue, null);

    /// <summary>
    /// Returns an object for parsing
    /// and adding to a list of parameters for data access.
    /// </summary>
    /// <param name="node">The <see cref="IXPathNavigable"/> node.</param>
    /// <param name="nodeQuery">The XPath <see cref="string"/>.</param>
    /// <param name="throwException">When <c>true</c>, throw an exception for null nodes and nodes that do not parse into the specified type.</param>
    /// <param name="defaultValue">Return a boxing <see cref="Object"/> for “zero-length” text nodes.</param>
    /// <param name="nsMan">
    /// The <see cref="XmlNamespaceManager"/>
    /// to use to resolve prefixes.
    /// </param>
    /// <typeparam name="T">The type to parse from the node value.</typeparam>
    public static object? GetNodeValueAndParse<T>(IXPathNavigable? node, string? nodeQuery, bool throwException,
        T? defaultValue, XmlNamespaceManager? nsMan)
    {
        object? p = GetNodeValue(node, nodeQuery, throwException, nsMan);

        try
        {
            T? stronglyOfT = default(T);

            if (p == null || string.IsNullOrWhiteSpace(p.ToString()?.Trim()))
            {
                p = defaultValue;
            }
            else
                switch (stronglyOfT)
                {
                    case bool:
                        p = bool.Parse(p.ToString() ?? string.Empty);
                        break;
                    case byte:
                        p = Byte.Parse(p.ToString() ?? string.Empty, CultureInfo.CurrentCulture);
                        break;
                    case DateTime:
                        p = DateTime.Parse(p.ToString() ?? string.Empty, CultureInfo.CurrentCulture);
                        break;
                    case decimal:
                        p = Decimal.Parse(p.ToString() ?? string.Empty, CultureInfo.CurrentCulture);
                        break;
                    case double:
                        p = Double.Parse(p.ToString() ?? string.Empty, CultureInfo.CurrentCulture);
                        break;
                    case short:
                        p = Int16.Parse(p.ToString() ?? string.Empty, CultureInfo.CurrentCulture);
                        break;
                    case int:
                        p = Int32.Parse(p.ToString() ?? string.Empty, CultureInfo.CurrentCulture);
                        break;
                    case long:
                        p = Int64.Parse(p.ToString() ?? string.Empty, CultureInfo.CurrentCulture);
                        break;
                    default:
                    {
                        if (typeof(T).IsAssignableFrom(typeof(string)))
                        {
                            p = p.ToString();
                        }
                        else
                        {
                            Type t = typeof(T);
                            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                                "The specified type, “{0},” is not supported.", t.FullName));
                        }

                        break;
                    }
                }
        }
        catch (Exception ex)
        {
            Type t = typeof(T);
            var errMsg =
                $"Parse for “{t.FullName}” fails for element in “{nodeQuery}.” Value to parse: “{((p == null) ? "Null" : p.ToString())}.” Default Message: “{ex.Message}.”";
            throw new XmlException(errMsg);
        }

        return p;
    }
}
