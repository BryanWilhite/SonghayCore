namespace Songhay.Xml;

public static partial class XmlUtility
{
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
    public static string? GetXslString(IXPathNavigable? navigableXsl, XsltArgumentList? xslArgs, IXPathNavigable? navigableXml) =>
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
            XmlWriter writer = settings != null ? XmlWriter.Create(ms, settings) : XmlWriter.Create(ms);
            xslt.Transform(reader, xslArgs, writer, null);
        }

        ms.Position = 0;

        string? ret = ms.ToUtf8String();

        return ret;
    }

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
