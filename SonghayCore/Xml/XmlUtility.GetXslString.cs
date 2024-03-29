﻿namespace Songhay.Xml;

public static partial class XmlUtility
{
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

        var navigator = navigableXml.CreateNavigator().ToReferenceTypeValueOrThrow();
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
