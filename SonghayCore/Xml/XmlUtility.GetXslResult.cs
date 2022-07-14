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

        XslCompiledTransform xslt = new XslCompiledTransform(false);
        xslt.Load(navigableXsl);

        var navigator = navigableXml.CreateNavigator().ToReferenceTypeValueOrThrow();

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
