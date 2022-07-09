using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using Songhay.Extensions;

namespace Songhay.Xml;

public static partial class XmlUtility
{
    /// <summary>
    /// Returns a <see cref="System.Xml.XPath.XPathDocument"/>
    /// for the transformation of the XSLT document
    /// and the XML document.
    /// </summary>
    /// <param name="xslSet">
    /// The source <see cref="System.Xml.XPath.IXPathNavigable"/> XSL document.
    /// </param>
    /// <param name="navigableSet">
    /// The source <see cref="System.Xml.XPath.IXPathNavigable"/> XML document.
    /// </param>
    [SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes",
        Justification = "Specific functionality provided by the concrete type may be required.")]
    public static XPathDocument GetXslResult(IXPathNavigable? xslSet, IXPathNavigable? navigableSet) =>
        GetXslResult(xslSet, null, navigableSet);

    /// <summary>
    /// Returns a <see cref="System.Xml.XPath.XPathDocument"/>
    /// for the transformation of the XSLT document
    /// and the XML document.
    /// </summary>
    /// <param name="xslSet">
    /// The source <see cref="System.Xml.XPath.IXPathNavigable"/> XSL document.
    /// </param>
    /// <param name="xslArgs">
    /// The <see cref="System.Xml.Xsl.XsltArgumentList"/>.
    /// </param>
    /// <param name="navigableSet">
    /// The source <see cref="System.Xml.XPath.IXPathNavigable"/> XML document.
    /// </param>
    [SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes",
        Justification = "Specific functionality provided by the concrete type may be required.")]
    public static XPathDocument GetXslResult(IXPathNavigable? xslSet, XsltArgumentList? xslArgs,
        IXPathNavigable? navigableSet)
    {
        if (xslSet == null) throw new ArgumentNullException(nameof(xslSet));
        if (navigableSet == null) throw new ArgumentNullException(nameof(navigableSet));

        XslCompiledTransform xslt = new XslCompiledTransform(false);
        xslt.Load(xslSet);

        var navigator = navigableSet.CreateNavigator().EnsureXPathNavigator();

        using MemoryStream ms = new MemoryStream();
        using (StringReader sr = new StringReader(navigator.OuterXml))
        {
            XmlReader reader = XmlReader.Create(sr);
            XmlWriter writer = XmlWriter.Create(ms);
            xslt.Transform(reader, xslArgs, writer, null);
            //FUNKYKB: Setting documentResolver to null prevents namespace URI calls (document() resolution).
        }

        ms.Position = 0;
        var ret = new XPathDocument(ms);

        return ret;
    }
}
