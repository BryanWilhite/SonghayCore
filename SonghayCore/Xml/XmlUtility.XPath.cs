namespace Songhay.Xml;

public static partial class XmlUtility
{
    /// <summary>
    /// Returns a <see cref="XmlNamespaceManager"/>
    /// with respect to the document element of the specified
    /// <see cref="IXPathNavigable"/> document.
    /// </summary>
    /// <param name="navigable">
    /// The <see cref="IXPathNavigable"/> document.
    /// </param>
    public static XmlNamespaceManager? GetNamespaceManager(IXPathNavigable? navigable)
    {
        XmlNamespaceManager? nsman = null;
        if (navigable == null) return nsman;

        XPathNavigator? navigator = navigable.CreateNavigator();
        XPathExpression xpath = XPathExpression.Compile("//*[1]");
        XPathNavigator? root = navigator?.SelectSingleNode(xpath);

        if (root == null) throw new NullReferenceException("The expected XPath Navigator is not here.");

        nsman = new XmlNamespaceManager(root.NameTable);

        IDictionary<string, string> names = root.GetNamespacesInScope(XmlNamespaceScope.Local);
        foreach (KeyValuePair<string, string> kv in names)
        {
            nsman.AddNamespace(kv.Key, kv.Value);
        }

        return nsman;
    }

    /// <summary>
    /// Returns an <see cref="XPathDocument"/>
    /// based on the specified <see cref="string"/>.
    /// </summary>
    /// <param name="xmlFragment">
    /// A well-formed XML <see cref="string"/>.
    /// </param>
    [SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes",
        Justification = "Specific functionality provided by the concrete type may be required.")]
    public static XPathDocument? GetNavigableDocument(string? xmlFragment)
    {
        if (string.IsNullOrWhiteSpace(xmlFragment)) return null;

        using StringReader reader = new StringReader(xmlFragment);
        XPathDocument d = new XPathDocument(reader);

        return d;
    }

    /// <summary>
    /// Returns an <see cref="XPathDocument"/>
    /// based on the specified <see cref="XmlNode"/>.
    /// </summary>
    /// <param name="navigable">
    /// The source <see cref="IXPathNavigable"/> document.
    /// </param>
    /// <remarks>
    /// Use this member to convert <see cref="XmlDocument"/> documents.
    /// </remarks>
    [SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes",
        Justification = "Specific functionality provided by the concrete type may be required.")]
    public static XPathDocument? GetNavigableDocument(IXPathNavigable? navigable)
    {
        XPathNavigator? navigator = navigable?.CreateNavigator();
        if (navigator == null) return null;

        using StringReader reader = new StringReader(navigator.OuterXml);
        XPathDocument d = new XPathDocument(reader);

        return d;
    }

    /// <summary>
    /// Returns an <see cref="XPathDocument"/>
    /// based on the specified <see cref="Stream"/>.
    /// </summary>
    /// <param name="stream">The stream.</param>
    [SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes",
        Justification = "Specific functionality provided by the concrete type may be required.")]
    public static XPathDocument? GetNavigableDocument(Stream? stream)
    {
        if (stream == null) return null;
        if (stream.Position != 0) stream.Position = 0;

        XPathDocument d = new XPathDocument(stream);

        return d;
    }

    /// <summary>
    /// Returns an <see cref="XPathNavigator"/>
    /// based on the nodeQuery Expression toward the source document.
    /// </summary>
    /// <param name="navigable">
    /// The source <see cref="IXPathNavigable"/> document.
    /// </param>
    /// <param name="nodeQuery">
    /// The value to be compiled
    /// into an <see cref="XPathExpression"/>.
    /// </param>
    public static XPathNavigator? GetNavigableNode(IXPathNavigable? navigable, string? nodeQuery)
    {
        if (navigable == null) return null;
        if (string.IsNullOrWhiteSpace(nodeQuery)) return null;

        XPathNavigator? navigator = navigable.CreateNavigator();
        XPathExpression xpath = XPathExpression.Compile(nodeQuery);
        XPathNavigator? node = navigator?.SelectSingleNode(xpath);

        return node;
    }

    /// <summary>
    /// Returns an <see cref="XPathNavigator"/>
    /// based on the nodeQuery Expression toward the source document.
    /// </summary>
    /// <param name="navigable">
    /// The source <see cref="IXPathNavigable"/> document.
    /// </param>
    /// <param name="nodeQuery">
    /// The value to be compiled
    /// into an <see cref="XPathExpression"/>.
    /// </param>
    /// <param name="nsMan">
    /// The <see cref="XmlNamespaceManager"/>
    /// to use to resolve prefixes.
    /// </param>
    public static XPathNavigator? GetNavigableNode(IXPathNavigable? navigable, string? nodeQuery,
        XmlNamespaceManager? nsMan)
    {
        if (navigable == null) return null;
        if (string.IsNullOrWhiteSpace(nodeQuery)) return null;

        XPathNavigator? navigator = navigable.CreateNavigator();
        XPathExpression xpath = XPathExpression.Compile(nodeQuery, nsMan);
        XPathNavigator? node = navigator?.SelectSingleNode(xpath);

        return node;
    }

    /// <summary>
    /// Returns an <see cref="XPathNodeIterator"/>
    /// based on the nodeQuery Expression toward the source document.
    /// </summary>
    /// <param name="navigable">
    /// The source <see cref="IXPathNavigable"/> document.
    /// </param>
    /// <param name="nodeQuery">
    /// The value to be compiled
    /// into an <see cref="XPathExpression"/>.
    /// </param>
    public static XPathNodeIterator? GetNavigableNodes(IXPathNavigable? navigable, string? nodeQuery)
    {
        if (navigable == null) return null;
        if (string.IsNullOrWhiteSpace(nodeQuery)) return null;

        XPathNavigator? navigator = navigable.CreateNavigator();

        XPathExpression xpath = XPathExpression.Compile(nodeQuery);
        XPathNodeIterator? nodes = navigator?.Select(xpath);

        return nodes;
    }

    /// <summary>
    /// Returns an <see cref="XPathNodeIterator"/>
    /// based on the nodeQuery Expression toward the source document.
    /// </summary>
    /// <param name="navigable">
    /// The source <see cref="IXPathNavigable"/> document.
    /// </param>
    /// <param name="nodeQuery">
    /// The value to be compiled
    /// into an <see cref="XPathExpression"/>.
    /// </param>
    /// <param name="nsMan">
    /// The <see cref="XmlNamespaceManager"/>
    /// to use to resolve prefixes.
    /// </param>
    public static XPathNodeIterator? GetNavigableNodes(IXPathNavigable? navigable, string? nodeQuery,
        XmlNamespaceManager nsMan)
    {
        if (navigable == null) return null;
        if (string.IsNullOrWhiteSpace(nodeQuery)) return null;

        XPathNavigator? navigator = navigable.CreateNavigator();

        XPathExpression xpath = XPathExpression.Compile(nodeQuery, nsMan);
        XPathNodeIterator? nodes = navigator?.Select(xpath);

        return nodes;
    }

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
                        p = byte.Parse(p.ToString() ?? string.Empty, CultureInfo.CurrentCulture);
                        break;
                    case DateTime:
                        p = DateTime.Parse(p.ToString() ?? string.Empty, CultureInfo.CurrentCulture);
                        break;
                    case decimal:
                        p = decimal.Parse(p.ToString() ?? string.Empty, CultureInfo.CurrentCulture);
                        break;
                    case double:
                        p = double.Parse(p.ToString() ?? string.Empty, CultureInfo.CurrentCulture);
                        break;
                    case short:
                        p = short.Parse(p.ToString() ?? string.Empty, CultureInfo.CurrentCulture);
                        break;
                    case int:
                        p = int.Parse(p.ToString() ?? string.Empty, CultureInfo.CurrentCulture);
                        break;
                    case long:
                        p = long.Parse(p.ToString() ?? string.Empty, CultureInfo.CurrentCulture);
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
            string errMsg =
                $"Parse for “{t.FullName}” fails for element in “{nodeQuery}.” Value to parse: “{((p == null) ? "Null" : p.ToString())}.” Default Message: “{ex.Message}.”";
            throw new XmlException(errMsg);
        }

        return p;
    }
}
