using System.Xml;
using System.Xml.XPath;

namespace Songhay.Xml;

public static partial class XmlUtility
{
    /// <summary>
    /// Returns strongly typed output, converting the XML fragment.
    /// </summary>
    /// <typeparam name="TOut">The <see cref="System.Type"/> of output to return (constrained to <c>class</c>).</typeparam>
    /// <param name="xmlFragment">An XML fragment or document as a well-formed <see cref="System.String"/>.</param>
    /// <returns>Returns the specified <see cref="System.Type"/>.</returns>
    public static TOut? OutputAs<TOut>(string xmlFragment) where TOut : class
    {
        if (typeof(TOut).IsAssignableFrom(typeof(string)))
        {
            return xmlFragment as TOut;
        }

        XPathDocument? d = GetNavigableDocument(xmlFragment);

        return OutputAs<TOut>(d);
    }

    /// <summary>
    /// Returns strongly typed output, converting the Navigable document.
    /// </summary>
    /// <typeparam name="TOut">The <see cref="System.Type"/> of output to return (constrained to <c>class</c>).</typeparam>
    /// <param name="navigableDocument">An <see cref="System.Xml.XPath.IXPathNavigable"/>.</param>
    /// <returns>Returns the specified <see cref="System.Type"/>.</returns>
    public static TOut? OutputAs<TOut>(IXPathNavigable? navigableDocument) where TOut : class
    {
        if (navigableDocument == null) return default;

        if (typeof(TOut).IsAssignableFrom(typeof(string)))
        {
            return navigableDocument.CreateNavigator()?.OuterXml as TOut;
        }

        TOut? stronglyOfTOut = default(TOut);
        switch (stronglyOfTOut)
        {
            case XmlDocument:
                XmlDocument dom = new XmlDocument();
                dom.LoadXml(navigableDocument.ToString()!);

                return dom as TOut;

            case XPathDocument:
                return navigableDocument as TOut;
        }

        return default;
    }
}
