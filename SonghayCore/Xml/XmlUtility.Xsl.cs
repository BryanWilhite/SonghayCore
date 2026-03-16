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
    /// Returns a <see cref="XPathDocument"/>
    /// for the transformation of the XSLT document
    /// and the XML document.
    /// </summary>
    /// <param name="navigableXsl">
    /// The source <see cref="IXPathNavigable"/> XSL document.
    /// </param>
    /// <param name="navigableSet">
    /// The source <see cref="IXPathNavigable"/> XML document.
    /// </param>
    [SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes",
        Justification = "Specific functionality provided by the concrete type may be required.")]
    public static XPathDocument GetXslResult(IXPathNavigable? navigableXsl, IXPathNavigable? navigableSet) =>
        GetXslResult(navigableXsl, null, navigableSet);

    /// <summary>
    /// Returns a <see cref="XPathDocument"/>
    /// for the transformation of the XSLT document
    /// and the XML document.
    /// </summary>
    /// <param name="navigableXsl">
    /// The source <see cref="IXPathNavigable"/> XSL document.
    /// </param>
    /// <param name="xslArgs">
    /// The <see cref="XsltArgumentList"/>.
    /// </param>
    /// <param name="navigableXml">
    /// The source <see cref="IXPathNavigable"/> XML document.
    /// </param>
    [SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes",
        Justification = "Specific functionality provided by the concrete type may be required.")]
    public static XPathDocument GetXslResult(IXPathNavigable? navigableXsl, XsltArgumentList? xslArgs,
        IXPathNavigable? navigableXml)
    {
        ArgumentNullException.ThrowIfNull(navigableXsl);
        ArgumentNullException.ThrowIfNull(navigableXml);

        XslCompiledTransform xslt = new(false);
        xslt.Load(navigableXsl);

        XPathNavigator navigator = navigableXml.CreateNavigator().ToReferenceTypeValueOrThrow();

        using MemoryStream ms = new();
        using (StringReader sr = new(navigator.OuterXml))
        {
            XmlReader reader = XmlReader.Create(sr);
            XmlWriter writer = XmlWriter.Create(ms);
            xslt.Transform(reader, xslArgs, writer, null);
            //FUNKYKB: Setting documentResolver to null prevents namespace URI calls (document() resolution).
        }

        ms.Position = 0;
        XPathDocument ret = new(ms);

        return ret;
    }

    /// <summary>
    /// Returns a <see cref="string"/>
    /// for the transformation of the XSLT document
    /// and the XML document.
    /// </summary>
    /// <param name="navigableXsl">The source <see cref="IXPathNavigable"/> XSL document.</param>
    /// <param name="navigableSet">The source <see cref="IXPathNavigable"/> XML document.</param>
    public static string? GetXslString(IXPathNavigable? navigableXsl, IXPathNavigable? navigableSet) =>
        GetXslString(navigableXsl, null, navigableSet, null);

    /// <summary>
    /// Returns a <see cref="string"/>
    /// for the transformation of the XSLT document
    /// and the XML document.
    /// </summary>
    /// <param name="navigableXsl">
    /// The source <see cref="IXPathNavigable"/> XSL document.
    /// </param>
    /// <param name="xslArgs">
    /// The <see cref="XsltArgumentList"/>.
    /// </param>
    /// <param name="navigableXml">
    /// The source <see cref="IXPathNavigable"/> XML document.
    /// </param>
    public static string?
        GetXslString(IXPathNavigable? navigableXsl, XsltArgumentList? xslArgs, IXPathNavigable? navigableXml) =>
        GetXslString(navigableXsl, xslArgs, navigableXml, null);

    /// <summary>
    /// Returns a <see cref="string"/>
    /// for the transformation of the XSLT document
    /// and the XML document.
    /// </summary>
    /// <param name="navigableXsl">The source <see cref="IXPathNavigable"/> XSL document.</param>
    /// <param name="xslArgs">The <see cref="XsltArgumentList"/>.</param>
    /// <param name="navigableXml">The source <see cref="IXPathNavigable"/> XML document.</param>
    /// <param name="settings">The settings.</param>
    public static string? GetXslString(IXPathNavigable? navigableXsl, XsltArgumentList? xslArgs,
        IXPathNavigable? navigableXml,
        XmlWriterSettings? settings)
    {
        ArgumentNullException.ThrowIfNull(navigableXsl);
        ArgumentNullException.ThrowIfNull(navigableXml);

        XPathNavigator navigator = navigableXml.CreateNavigator().ToReferenceTypeValueOrThrow();
        XslCompiledTransform xslt = new(false);
        xslt.Load(navigableXsl);

        using MemoryStream ms = new();
        using (StringReader sr = new(navigator.OuterXml))
        {
            XmlReader reader = XmlReader.Create(sr);
            XmlWriter writer = (settings != null) ? XmlWriter.Create(ms, settings) : XmlWriter.Create(ms);
            xslt.Transform(reader, xslArgs, writer, null);
        }

        ms.Position = 0;

        string? ret = ms.ToUtf8String();

        return ret;
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
    public static XPathDocument? InputAs<TIn>(TIn? input) where TIn: class
    {
        if (input == null) return null;

        TIn? stronglyOfTIn = null;

        switch (stronglyOfTIn)
        {
            case XmlDocument:
                return GetNavigableDocument(input as XmlDocument);

            case XPathDocument:
                return input as XPathDocument;

            default:
                if (!typeof(TIn).IsAssignableFrom(typeof(string))) return null;

                string? s = input as string;
                s = HtmlUtility.ConvertToXml(s);
                s = LatinGlyphsUtility.Condense(s, basicLatinOnly: false);

                return GetNavigableDocument(s);
        }
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
    /// Returns a <see cref="string"/>
    /// for the transformation of the XSLT file
    /// and the XML file.
    /// </summary>
    /// <param name="xslPath">
    /// Output path to the XSLT file.
    /// </param>
    /// <param name="xmlPath">
    /// Output path to the XML file.
    /// </param>
    public static string? LoadXslTransform(string? xslPath, string? xmlPath) =>
        LoadXslTransform(xslPath, xslArgs: null, xmlPath);

    /// <summary>
    /// Returns a <see cref="string"/>
    /// for the transformation of the XSLT file
    /// and the XML file.
    /// </summary>
    /// <param name="xslPath">
    /// Output path to the XSLT file.
    /// </param>
    /// <param name="commandName">
    /// The value for the <code>cmd</code>-parameter convention.
    /// </param>
    /// <param name="xmlPath">
    /// Output path to the XML file.
    /// </param>
    public static string? LoadXslTransform(string? xslPath, string? commandName, string? xmlPath)
    {
        XsltArgumentList xslArgs = GetXsltArgumentList(commandName);

        return LoadXslTransform(xslPath, xslArgs, xmlPath);
    }

    /// <summary>
    /// Returns a <see cref="string"/>
    /// for the transformation of the XSLT file
    /// and the XML file.
    /// </summary>
    /// <param name="xslPath">
    /// Output path to the XSLT file.
    /// </param>
    /// <param name="xslArgs">
    /// The <see cref="XsltArgumentList"/>.
    /// </param>
    /// <param name="xmlPath">
    /// Output path to the XML file.
    /// </param>
    public static string? LoadXslTransform(string? xslPath, XsltArgumentList? xslArgs, string? xmlPath)
    {
        xslPath.ThrowWhenNullOrWhiteSpace();
        xmlPath.ThrowWhenNullOrWhiteSpace();

        xslArgs ??= new();

        XslCompiledTransform xslt = new(false);
        xslt.Load(xslPath);

        using MemoryStream ms = new();
        using (XmlReader reader = XmlReader.Create(xmlPath))
        {
            XmlWriter writer = XmlWriter.Create(ms);
            xslt.Transform(reader, xslArgs, writer, null);
        }

        string? ret = ms.ToUtf8String();

        return ret;
    }

    /// <summary>
    /// Returns a <see cref="string"/>
    /// for the transformation of the XSLT file
    /// and the specified <see cref="IXPathNavigable"/> in memory.
    /// </summary>
    /// <param name="xslPath">
    /// Output path to the XSLT file.
    /// </param>
    /// <param name="commandName">
    /// The value for the <code>cmd</code>-parameter convention.
    /// </param>
    /// <param name="xmlFragment">
    /// A well-formed XML <see cref="string"/>.
    /// </param>
    /// <remarks>
    /// This convention is used in ASP.NET Web applications.
    /// </remarks>
    public static string? LoadXslTransformForMemoryInput(string? xslPath, string? commandName,
        string? xmlFragment)
    {
        XsltArgumentList xslArgs = GetXsltArgumentList(commandName);

        return LoadXslTransformForMemoryInput(xslPath, xslArgs, xmlFragment);
    }

    /// <summary>
    /// Returns a <see cref="string"/>
    /// for the transformation of the XSLT file
    /// and the specified <see cref="IXPathNavigable"/> in memory.
    /// </summary>
    /// <param name="xslPath">
    /// Output path to the XSLT file.
    /// </param>
    /// <param name="commandName">
    /// The value for the <code>cmd</code>-parameter convention.
    /// </param>
    /// <param name="navigableXml">
    /// The <see cref="IXPathNavigable"/> in memory.
    /// </param>
    /// <remarks>
    /// This convention is used in ASP.NET Web applications.
    /// </remarks>
    public static string? LoadXslTransformForMemoryInput(string? xslPath, string? commandName,
        IXPathNavigable navigableXml)
    {
        ArgumentNullException.ThrowIfNull(navigableXml);

        XPathNavigator navigator = navigableXml.CreateNavigator().ToReferenceTypeValueOrThrow();
        string xmlFragment = navigator.OuterXml;
        XsltArgumentList xslArgs = GetXsltArgumentList(commandName);

        return LoadXslTransformForMemoryInput(xslPath, xslArgs, xmlFragment);
    }

    /// <summary>
    /// Returns a <see cref="string"/>
    /// for the transformation of the XSLT file
    /// and the XML in-memory fragment.
    /// </summary>
    /// <param name="xslPath">
    /// Output path to the XSLT file.
    /// </param>
    /// <param name="xslArgs">
    /// The <see cref="XsltArgumentList"/>.
    /// </param>
    /// <param name="xmlFragment">
    /// A well-formed XML <see cref="string"/>.
    /// </param>
    /// <remarks>
    /// This convention is used in ASP.NET Web applications.
    /// </remarks>
    public static string? LoadXslTransformForMemoryInput(string? xslPath, XsltArgumentList? xslArgs,
        string? xmlFragment)
    {
        xslPath.ThrowWhenNullOrWhiteSpace();
        xmlFragment.ThrowWhenNullOrWhiteSpace();

        xslArgs ??= new();

        XslCompiledTransform xslt = new(false);
        xslt.Load(xslPath);

        using MemoryStream ms = new();
        using (StringReader sr = new(xmlFragment))
        {
            XmlReader reader = XmlReader.Create(sr);
            XmlWriter writer = XmlWriter.Create(ms);
            xslt.Transform(reader, xslArgs, writer, null);
        }

        string? ret = ms.ToUtf8String();

        return ret;
    }

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
    public static TOut? OutputAs<TOut>(IXPathNavigable? navigableDocument) where TOut : class
    {
        if (navigableDocument == null) return default;

        if (typeof(TOut).IsAssignableFrom(typeof(string)))
        {
            return navigableDocument.CreateNavigator()?.OuterXml as TOut;
        }

        TOut? stronglyOfTOut = default(TOut);
        switch (stronglyOfTOut)
        {
            case XmlDocument:
                XmlDocument dom = new();
                dom.LoadXml(navigableDocument.ToString()!);

                return dom as TOut;

            case XPathDocument:
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

    /// <summary>
    /// Transforms the specified navigable documents
    /// and writes to disk with the specified path.
    /// </summary>
    /// <param name="xmlInput">The specified input.</param>
    /// <param name="navigableXsl">
    /// The source <see cref="IXPathNavigable"/> document.
    /// </param>
    /// <param name="outputPath">
    /// The file-system, target path.
    /// </param>
    public static void WriteXslTransform(IXPathNavigable? xmlInput,
        IXPathNavigable? navigableXsl, string? outputPath)
    {
        ArgumentNullException.ThrowIfNull(xmlInput);
        ArgumentNullException.ThrowIfNull(navigableXsl);
        ArgumentNullException.ThrowIfNull(outputPath);

        using FileStream fs = new(outputPath, FileMode.Create);

        XslCompiledTransform xslt = new(false);
        xslt.Load(navigableXsl);

        using StringReader sr = new(xmlInput.CreateNavigator().ToReferenceTypeValueOrThrow().OuterXml);
        XmlReader reader = XmlReader.Create(sr);
        XmlWriter writer = XmlWriter.Create(fs);

        xslt.Transform(reader, null, writer, null);
    }

    /// <summary>
    /// Transforms the specified input and writes to disk.
    /// </summary>
    /// <param name="xmlInput">The specified input.</param>
    /// <param name="navigableXsl">
    /// The source <see cref="IXPathNavigable"/> document.
    /// </param>
    /// <param name="outputPath">
    /// The file-system, target path.
    /// </param>
    public static void WriteXslTransform(XmlReader? xmlInput,
        IXPathNavigable? navigableXsl, string? outputPath)
    {
        ArgumentNullException.ThrowIfNull(xmlInput);
        ArgumentNullException.ThrowIfNull(navigableXsl);
        outputPath.ThrowWhenNullOrWhiteSpace();

        using FileStream fs = new(outputPath, FileMode.Create);

        XslCompiledTransform xslt = new(false);
        xslt.Load(navigableXsl);
        XmlWriter writer = XmlWriter.Create(fs);

        xslt.Transform(xmlInput, null, writer, null);
    }

    static XsltArgumentList GetXsltArgumentList(string? commandName)
    {
        XsltArgumentList xslArgs = new();

        if (!string.IsNullOrWhiteSpace(commandName))
            //CONVENTION: XSL templates use a parameter called “cmd”:
            xslArgs.AddParam("cmd", string.Empty, commandName);

        return xslArgs;
    }
}
