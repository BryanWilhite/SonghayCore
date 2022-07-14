namespace Songhay.Xml;

public partial class XmlUtility
{
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
                s = LatinGlyphsUtility.Condense(s, basicLatinOnly: false);

                return GetNavigableDocument(s);
        }
    }
}
