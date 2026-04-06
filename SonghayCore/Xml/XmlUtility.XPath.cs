namespace Songhay.Xml;

public static partial class XmlUtility
{
    /// <summary>
    /// Combines two <see cref="IXPathNavigable"/> documents.
    /// </summary>
    /// <param name="parentDocument">The <see cref="IXPathNavigable"/> “hosting” document.</param>
    /// <param name="childDocument">The <see cref="IXPathNavigable"/> child document.</param>
    [SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes",
        Justification = "Specific functionality provided by the concrete type may be required.")]
    public static XPathDocument? CombineNavigableDocuments(IXPathNavigable? parentDocument,
        IXPathNavigable? childDocument) => CombineNavigableDocuments(parentDocument, childDocument, null);

    /// <summary>
    /// Combines two <see cref="IXPathNavigable"/> documents.
    /// </summary>
    /// <param name="parentDocument">The <see cref="IXPathNavigable"/> “hosting” document.</param>
    /// <param name="childDocument">The <see cref="IXPathNavigable"/> child document.</param>
    /// <param name="nodeQuery">The XPath query to the child document location in the “hosting” document. </param>
    [SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes",
        Justification = "Specific functionality provided by the concrete type may be required.")]
    public static XPathDocument? CombineNavigableDocuments(IXPathNavigable? parentDocument,
        IXPathNavigable? childDocument, string? nodeQuery)
    {
        if (parentDocument == null || childDocument == null) return null;

        XmlDocument dom = new();
        XPathNavigator? domNavigator = dom.CreateNavigator();
        if (domNavigator == null) return null;

        XPathNavigator? navigator = parentDocument.CreateNavigator();
        if (navigator == null) return null;

        dom.LoadXml(navigator.OuterXml);

        if (string.IsNullOrWhiteSpace(nodeQuery))
        {
            domNavigator.MoveToFirstChild();

            XPathNavigator? childNavigator = childDocument.CreateNavigator();
            if (childNavigator != null) domNavigator.AppendChild(childNavigator.OuterXml);
            domNavigator.MoveToRoot();
        }
        else
        {
            XPathNodeIterator nodes = domNavigator.Select(nodeQuery);
            foreach (XPathNavigator n in nodes)
            {
                XPathNavigator? childNavigator = childDocument.CreateNavigator();
                if (childNavigator != null) n.AppendChild(childNavigator.OuterXml);
                break;
            }
        }

        using StringReader reader = new(domNavigator.OuterXml);
        XPathDocument combinedDocument = new(reader);

        return combinedDocument;
    }

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
        XPathNavigator? navigator = GetNavigableNode(node, nodeQuery, nsMan);
        if (throwException && navigator == null) throw new NullReferenceException("The expected is not here.");

        if (navigator == null || string.IsNullOrWhiteSpace(navigator.ToString().Trim()))
        {
            return defaultValue;
        }

        try
        {
            T? stronglyOfT = default(T);

            switch (stronglyOfT)
            {
                case DateTime: return ProgramTypeUtility.ParseDateTime(navigator.Value);
                case decimal: return ProgramTypeUtility.ParseDecimal(navigator.Value);
                case double: return ProgramTypeUtility.ParseDouble(navigator.Value);
                case long: return ProgramTypeUtility.ParseInt64(navigator.Value);
                case int: return ProgramTypeUtility.ParseInt32(navigator.Value);
                case short: return ProgramTypeUtility.ParseInt16(navigator.Value);
                case byte: return ProgramTypeUtility.ParseByte(navigator.Value);
                case bool: return ProgramTypeUtility.ParseBoolean(navigator.Value);
                case string: return navigator.Value;
                default:
                {
                    Type t = typeof(T);

                    throw new NotSupportedException($"The specified type, `{t.FullName},` is not supported.");
                }
            }
        }
        catch (Exception ex)
        {
            Type t = typeof(T);
            string errMsg = $"Parse for `{t.FullName}` fails for element in `{nodeQuery}.` Value to parse: “{navigator}.” Default Message: “{ex.Message}.”";

            throw new XmlException(errMsg);
        }
    }

    /// <summary>
    /// Returns an <see cref="XPathDocument"/>,
    /// converted from the specified input.
    /// </summary>
    /// <typeparam name="TIn">The <see cref="Type"/> of the input.</typeparam>
    /// <param name="input">The input.</param>
    /// <returns>Returns an <see cref="XPathDocument"/>.</returns>
    /// <remarks>
    /// This member only supports <c>TIn</c> as
    /// <see cref="string"/>, <see cref="XmlDocument"/> or <see cref="XPathDocument"/>.
    /// </remarks>
    [SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes",
        Justification = "Specific functionality provided by the concrete type may be required.")]
    [Obsolete("Use `ToOuterXmlString` and/or `ToXmlDocument` in `IXPathNavigableExtensions` with perhaps `XmlUtility.GetNavigableDocument` instead.")]
    public static XPathDocument? InputAs<TIn>(TIn? input) where TIn: class
    {
        switch (input)
        {
            case null:
                return null;
            case XmlDocument xmlDocument:
                return GetNavigableDocument(xmlDocument);
            case XPathDocument xPathDocument:
                return xPathDocument;
        }

        if (input is not string s) return null;

        s = HtmlUtility.ConvertToXml(s) ?? string.Empty;
        s = LatinGlyphsUtility.Condense(s, basicLatinOnly: false) ?? string.Empty;

        return string.IsNullOrWhiteSpace(s) ? null : GetNavigableDocument(s);
    }

    /// <summary>
    /// Returns true when the node has the specified value.
    /// </summary>
    /// <param name="node">The <see cref="IXPathNavigable"/> node.</param>
    /// <param name="nodeQuery">The node query <see cref="string"/>.</param>
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
    /// <param name="node">The <see cref="IXPathNavigable"/> node.</param>
    /// <param name="nodeQuery">The node query <see cref="string"/>.</param>
    /// <param name="throwException">When <code>true</code>, throw an exception for null nodes.</param>
    /// <param name="testValue">The specified value to test with the node value.</param>
    /// <param name="comparisonType">The <see cref="StringComparison"/> type.</param>
    public static bool IsNodeValue(IXPathNavigable? node, string? nodeQuery, bool throwException, string? testValue,
        StringComparison comparisonType)
    {
        string? s = GetNodeValue(node, nodeQuery, throwException) as string;

        return string.Equals(s, testValue, comparisonType);
    }

    /// <summary>
    /// Returns true when the node has the specified value.
    /// </summary>
    /// <param name="node">The <see cref="IXPathNavigable"/> node.</param>
    /// <param name="nodeQuery">The node query <see cref="string"/>.</param>
    /// <param name="throwException">When <code>true</code>, throw an exception for null nodes.</param>
    /// <param name="testValue">The specified value to test with the node value.</param>
    /// <param name="nsMan">
    /// The <see cref="XmlNamespaceManager"/>
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
    /// <param name="node">The <see cref="IXPathNavigable"/> node.</param>
    /// <param name="nodeQuery">The node query <see cref="string"/>.</param>
    /// <param name="throwException">When <code>true</code>, throw an exception for null nodes.</param>
    /// <param name="testValue">The specified value to test with the node value.</param>
    /// <param name="nsMan">
    /// The <see cref="XmlNamespaceManager"/>
    /// to use to resolve prefixes.
    /// </param>
    /// <param name="comparisonType">The <see cref="StringComparison"/> type.</param>
    public static bool IsNodeValue(IXPathNavigable? node, string? nodeQuery, bool throwException, string? testValue,
        XmlNamespaceManager nsMan, StringComparison comparisonType)
    {
        string? s = GetNodeValue(node, nodeQuery, throwException, null, nsMan) as string;

        return string.Equals(s, testValue, comparisonType);
    }

    /// <summary>
    /// Returns true when the node has the value “no”.
    /// </summary>
    /// <param name="node">The <see cref="IXPathNavigable"/> node.</param>
    /// <param name="nodeQuery">The node query <see cref="string"/>.</param>
    /// <param name="throwException">When <code>true</code>, throw an exception for null nodes.</param>
    public static bool IsNoNodeValue(IXPathNavigable? node, string? nodeQuery, bool throwException) =>
        IsNodeValue(node, nodeQuery, throwException, "no", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Returns true when the node has the value “yes”.
    /// </summary>
    /// <param name="node">The <see cref="IXPathNavigable"/> node.</param>
    /// <param name="nodeQuery">The node query <see cref="string"/>.</param>
    /// <param name="throwException">When <code>true</code>, throw an exception for null nodes.</param>
    public static bool IsYesNodeValue(IXPathNavigable? node, string? nodeQuery, bool throwException) =>
        IsNodeValue(node, nodeQuery, throwException, "yes", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Returns strongly typed output, converting the XML fragment.
    /// </summary>
    /// <typeparam name="TOut">The <see cref="Type"/> of output to return (constrained to <c>class</c>).</typeparam>
    /// <param name="xmlFragment">An XML fragment or document as a well-formed <see cref="string"/>.</param>
    /// <returns>Returns the specified <see cref="Type"/>.</returns>
    /// <remarks>
    /// This member only supports <c>TIn</c> as
    /// <see cref="string"/>, <see cref="XmlDocument"/> or <see cref="XPathDocument"/>.
    /// </remarks>
    [Obsolete("Use `ToOuterXmlString` and/or `ToXmlDocument` in `IXPathNavigableExtensions` with perhaps `XmlUtility.GetNavigableDocument` instead.")]
    public static TOut? OutputAs<TOut>(string xmlFragment) where TOut : class
    {
        if (typeof(TOut).IsAssignableFrom(typeof(string)))
        {
            return xmlFragment as TOut;
        }

        XPathDocument? d = GetNavigableDocument(xmlFragment);

        return OutputAs<TOut>(d);
    }

    /// <summary>
    /// Returns strongly typed output, converting the Navigable document.
    /// </summary>
    /// <typeparam name="TOut">The <see cref="Type"/> of output to return (constrained to <c>class</c>).</typeparam>
    /// <param name="navigableDocument">An <see cref="IXPathNavigable"/>.</param>
    /// <returns>Returns the specified <see cref="Type"/>.</returns>
    /// <remarks>
    /// This member only supports <c>TIn</c> as
    /// <see cref="string"/>, <see cref="XmlDocument"/> or <see cref="XPathDocument"/>.
    /// </remarks>
    [SuppressMessage("ReSharper", "PreferConcreteValueOverDefault")]
    [Obsolete("Use `ToOuterXmlString` and/or `ToXmlDocument` in `IXPathNavigableExtensions` with perhaps `XmlUtility.GetNavigableDocument` instead.")]
    public static TOut? OutputAs<TOut>(IXPathNavigable? navigableDocument) where TOut : class
    {
        if (navigableDocument == null) return null;

        if (typeof(TOut).IsAssignableFrom(typeof(string)))
        {
            return navigableDocument.CreateNavigator()?.OuterXml as TOut;
        }

        if (typeof(TOut).IsAssignableFrom(typeof(XmlDocument)))
        {
            string? outerXml = navigableDocument.CreateNavigator()?.OuterXml;
            if (string.IsNullOrWhiteSpace(outerXml)) return null;

            XmlDocument dom = new();
            dom.LoadXml(outerXml);

            return dom as TOut;
        }

        if (typeof(TOut).IsAssignableFrom(typeof(XPathDocument)))
        {
            return navigableDocument as TOut;
        }

        return default;
    }

    /// <summary>
    /// Strip the namespaces from specified document.
    /// </summary>
    /// <param name="navigable">
    /// The source <see cref="IXPathNavigable"/> document.
    /// </param>
    /// <remarks>
    /// WARNING: Stripping namespaces “flattens” the document
    /// and can cause local-name collisions.
    /// 
    /// This routine does not remove namespace prefixes.
    /// 
    /// </remarks>
    [SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes",
        Justification = "Specific functionality provided by the concrete type may be required.")]
    public static XPathDocument? StripNamespaces(IXPathNavigable? navigable) => StripNamespaces(navigable, false);

    /// <summary>
    /// Strip the namespaces from specified document.
    /// </summary>
    /// <param name="navigable">
    /// The source <see cref="IXPathNavigable"/> document.
    /// </param>
    /// <param name="removeDocType">
    /// When <c>true</c>, removes any DOCTYPE preambles.
    /// </param>
    /// <remarks>
    /// WARNING: Stripping namespaces “flattens” the document
    /// and can cause local-name collisions.
    /// 
    /// This routine does not remove namespace prefixes.
    /// 
    /// </remarks>
    [SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes",
        Justification = "Specific functionality provided by the concrete type may be required.")]
    public static XPathDocument? StripNamespaces(IXPathNavigable? navigable, bool removeDocType)
    {
        if (navigable == null) return null;

        XPathNavigator? navigator = navigable.CreateNavigator();

        string? xmlString = StripNamespaces(navigator?.OuterXml, removeDocType);

        using StringReader s = new(xmlString!);

        XPathDocument newXml = new(s);

        return newXml;
    }
}
