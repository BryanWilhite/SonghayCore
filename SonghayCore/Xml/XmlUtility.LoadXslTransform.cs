using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using Songhay.Extensions;

namespace Songhay.Xml;

public static partial class XmlUtility
{
    /// <summary>
    /// Returns a <see cref="System.String"/>
    /// for the transformation of the XSLT file
    /// and the XML file.
    /// </summary>
    /// <param name="xslPath">
    /// output path to the XSLT file.
    /// </param>
    /// <param name="xmlPath">
    /// output path to the XML file.
    /// </param>
    public static string? LoadXslTransform(string? xslPath, string? xmlPath) =>
        LoadXslTransform(xslPath, xslArgs: null, xmlPath);

    /// <summary>
    /// Returns a <see cref="System.String"/>
    /// for the transformation of the XSLT file
    /// and the XML file.
    /// </summary>
    /// <param name="xslPath">
    /// output path to the XSLT file.
    /// </param>
    /// <param name="commandName">
    /// The value for the <code>cmd</code>-parameter convention.
    /// </param>
    /// <param name="xmlPath">
    /// output path to the XML file.
    /// </param>
    public static string? LoadXslTransform(string? xslPath, string? commandName, string? xmlPath)
    {
        var xslArgs = GetXsltArgumentList(commandName);

        return LoadXslTransform(xslPath, xslArgs, xmlPath);
    }

    /// <summary>
    /// Returns a <see cref="System.String"/>
    /// for the transformation of the XSLT file
    /// and the XML file.
    /// </summary>
    /// <param name="xslPath">
    /// output path to the XSLT file.
    /// </param>
    /// <param name="xslArgs">
    /// The <see cref="System.Xml.Xsl.XsltArgumentList"/>.
    /// </param>
    /// <param name="xmlPath">
    /// output path to the XML file.
    /// </param>
    public static string? LoadXslTransform(string? xslPath, XsltArgumentList? xslArgs, string? xmlPath)
    {
        xslPath.ThrowWhenNullOrWhiteSpace();
        xmlPath.ThrowWhenNullOrWhiteSpace();

        xslArgs ??= new XsltArgumentList();

        XslCompiledTransform xslt = new XslCompiledTransform(false);
        xslt.Load(xslPath);

        using MemoryStream ms = new MemoryStream();
        using (XmlReader reader = XmlReader.Create(xmlPath))
        {
            XmlWriter writer = XmlWriter.Create(ms);
            xslt.Transform(reader, xslArgs, writer, null);
        }

        var ret = GetText(ms);

        return ret;
    }

    /// <summary>
    /// Returns a <see cref="System.String"/>
    /// for the transformation of the XSLT file
    /// and the specified <see cref="System.Xml.XPath.IXPathNavigable"/> in memory.
    /// </summary>
    /// <param name="xslPath">
    /// output path to the XSLT file.
    /// </param>
    /// <param name="commandName">
    /// The value for the <code>cmd</code>-parameter convention.
    /// </param>
    /// <param name="xmlFragment">
    /// A well-formed XML <see cref="System.String"/>.
    /// </param>
    /// <remarks>
    /// This convention is used in ASP.NET Web applications.
    /// </remarks>
    public static string? LoadXslTransformForMemoryInput(string? xslPath, string? commandName,
        string? xmlFragment)
    {
        var xslArgs = GetXsltArgumentList(commandName);

        return LoadXslTransformForMemoryInput(xslPath, xslArgs, xmlFragment);
    }

    /// <summary>
    /// Returns a <see cref="System.String"/>
    /// for the transformation of the XSLT file
    /// and the specified <see cref="System.Xml.XPath.IXPathNavigable"/> in memory.
    /// </summary>
    /// <param name="xslPath">
    /// output path to the XSLT file.
    /// </param>
    /// <param name="commandName">
    /// The value for the <code>cmd</code>-parameter convention.
    /// </param>
    /// <param name="navigableXml">
    /// The <see cref="System.Xml.XPath.IXPathNavigable"/> in memory.
    /// </param>
    /// <remarks>
    /// This convention is used in ASP.NET Web applications.
    /// </remarks>
    public static string? LoadXslTransformForMemoryInput(string? xslPath, string? commandName,
        IXPathNavigable navigableXml)
    {
        ArgumentNullException.ThrowIfNull(navigableXml);

        var navigator = navigableXml.CreateNavigator().ToValueOrThrow();
        var xmlFragment = navigator.OuterXml;
        var xslArgs = GetXsltArgumentList(commandName);

        return LoadXslTransformForMemoryInput(xslPath, xslArgs, xmlFragment);
    }

    /// <summary>
    /// Returns a <see cref="System.String"/>
    /// for the transformation of the XSLT file
    /// and the XML in-memory fragment.
    /// </summary>
    /// <param name="xslPath">
    /// output path to the XSLT file.
    /// </param>
    /// <param name="xslArgs">
    /// The <see cref="System.Xml.Xsl.XsltArgumentList"/>.
    /// </param>
    /// <param name="xmlFragment">
    /// A well-formed XML <see cref="System.String"/>.
    /// </param>
    /// <remarks>
    /// This convention is used in ASP.NET Web applications.
    /// </remarks>
    public static string? LoadXslTransformForMemoryInput(string? xslPath, XsltArgumentList? xslArgs,
        string? xmlFragment)
    {
        xslPath.ThrowWhenNullOrWhiteSpace();
        xmlFragment.ThrowWhenNullOrWhiteSpace();

        xslArgs ??= new XsltArgumentList();

        var xslt = new XslCompiledTransform(false);
        xslt.Load(xslPath);

        using MemoryStream ms = new MemoryStream();
        using (StringReader sr = new StringReader(xmlFragment))
        {
            XmlReader reader = XmlReader.Create(sr);
            XmlWriter writer = XmlWriter.Create(ms);
            xslt.Transform(reader, xslArgs, writer, null);
        }

        var ret = GetText(ms);

        return ret;
    }

    static XsltArgumentList GetXsltArgumentList(string? commandName)
    {
        XsltArgumentList xslArgs = new XsltArgumentList();

        if (!string.IsNullOrWhiteSpace(commandName))
            //CONVENTION: XSL templates use a parameter called “cmd”:
            xslArgs.AddParam("cmd", string.Empty, commandName);

        return xslArgs;
    }
}
