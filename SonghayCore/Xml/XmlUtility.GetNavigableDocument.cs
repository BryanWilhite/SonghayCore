namespace Songhay.Xml;

public static partial class XmlUtility
{
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
        var d = new XPathDocument(reader);

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
        var navigator = navigable?.CreateNavigator();
        if (navigator == null) return null;

        using StringReader reader = new StringReader(navigator.OuterXml);
        var d = new XPathDocument(reader);

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
}
