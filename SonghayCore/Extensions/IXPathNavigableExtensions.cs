namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="IXPathNavigable"/>
/// </summary>
// ReSharper disable once InconsistentNaming
public static class IXPathNavigableExtensions
{
    /// <summary>
    /// Converts the specified <see cref="IXPathNavigable"/> node
    /// to its outer-XML string representation.
    /// </summary>
    /// <param name="node">the <see cref="IXPathNavigable"/> node</param>
    public static string? ToOuterXmlString(this IXPathNavigable? node) => node?.CreateNavigator()?.OuterXml;

    /// <summary>
    /// Converts the specified <see cref="IXPathNavigable"/> node
    ///to a <see cref="XmlDocument"/>
    /// </summary>
    /// <param name="node">the <see cref="IXPathNavigable"/> node</param>
    public static XmlDocument? ToXmlDocument(this IXPathNavigable? node)
    {
        string? outerXml = node.ToOuterXmlString();
        if (string.IsNullOrWhiteSpace(outerXml)) return null;

        XmlDocument dom = new();
        dom.LoadXml(outerXml);

        return dom;
    }
}
