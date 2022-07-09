using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.XPath;

namespace Songhay.Xml;

public partial class XmlUtility
{
    /// <summary>
    /// Returns an <see cref="System.Xml.XPath.XPathDocument"/>,
    /// converted from the specified input.
    /// </summary>
    /// <typeparam name="TIn">The <see cref="System.Type"/> of the input.</typeparam>
    /// <param name="input">The input.</param>
    /// <returns>Returns an <see cref="System.Xml.XPath.XPathDocument"/>.</returns>
    [SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes",
        Justification = "Specific functionality provided by the concrete type may be required.")]
    public static XPathDocument? InputAs<TIn>(TIn? input)
    {
        if (input == null) return null;

        TIn? stronglyOfTIn = default(TIn);
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
                s = LatinGlyphsUtility.Condense(s);

                return GetNavigableDocument(s);
        }
    }
}
