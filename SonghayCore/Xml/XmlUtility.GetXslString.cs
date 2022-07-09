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
    /// for the transformation of the XSLT document
    /// and the XML document.
    /// </summary>
    /// <param name="navigableXsl">The source <see cref="System.Xml.XPath.IXPathNavigable"/> XSL document.</param>
    /// <param name="navigableSet">The source <see cref="System.Xml.XPath.IXPathNavigable"/> XML document.</param>
    public static string? GetXslString(IXPathNavigable? navigableXsl, IXPathNavigable? navigableSet) =>
        GetXslString(navigableXsl, null, navigableSet, null);

    /// <summary>
    /// Returns a <see cref="System.String"/>
    /// for the transformation of the XSLT document
    /// and the XML document.
    /// </summary>
    /// <param name="navigableXsl">
    /// The source <see cref="System.Xml.XPath.IXPathNavigable"/> XSL document.
    /// </param>
    /// <param name="xslArgs">
    /// The <see cref="System.Xml.Xsl.XsltArgumentList"/>.
    /// </param>
    /// <param name="navigableXml">
    /// The source <see cref="System.Xml.XPath.IXPathNavigable"/> XML document.
    /// </param>
    public static string?
        GetXslString(IXPathNavigable? navigableXsl, XsltArgumentList? xslArgs, IXPathNavigable? navigableXml) =>
        GetXslString(navigableXsl, xslArgs, navigableXml, null);

    /// <summary>
    /// Returns a <see cref="System.String"/>
    /// for the transformation of the XSLT document
    /// and the XML document.
    /// </summary>
    /// <param name="navigableXsl">The source <see cref="System.Xml.XPath.IXPathNavigable"/> XSL document.</param>
    /// <param name="xslArgs">The <see cref="System.Xml.Xsl.XsltArgumentList"/>.</param>
    /// <param name="navigableXml">The source <see cref="System.Xml.XPath.IXPathNavigable"/> XML document.</param>
    /// <param name="settings">The settings.</param>
    public static string? GetXslString(IXPathNavigable? navigableXsl, XsltArgumentList? xslArgs,
        IXPathNavigable? navigableXml,
        XmlWriterSettings? settings)
    {
        ArgumentNullException.ThrowIfNull(navigableXsl);
        ArgumentNullException.ThrowIfNull(navigableXml);

        var navigator = navigableXml.CreateNavigator().ToValueOrThrow();
        XslCompiledTransform xslt = new XslCompiledTransform(false);
        xslt.Load(navigableXsl);

        using MemoryStream ms = new MemoryStream();
        using (StringReader sr = new StringReader(navigator.OuterXml))
        {
            XmlReader reader = XmlReader.Create(sr);
            XmlWriter writer = (settings != null) ? XmlWriter.Create(ms, settings) : XmlWriter.Create(ms);
            xslt.Transform(reader, xslArgs, writer, null);
        }

        ms.Position = 0;

        var ret = GetText(ms);

        return ret;
    }
}
