﻿namespace Songhay.Xml;

public static partial class XmlUtility
{
    /// <summary>
    /// Returns a <see cref="XmlNamespaceManager"/>
    /// with respect to the document element of the specified
    /// <see cref="IXPathNavigable"/> document.
    /// </summary>
    /// <param name="navigable">
    /// The <see cref="IXPathNavigable"/> document.
    /// </param>
    public static XmlNamespaceManager? GetNamespaceManager(IXPathNavigable? navigable)
    {
        XmlNamespaceManager? nsman = null;
        if (navigable == null) return nsman;

        XPathNavigator? navigator = navigable.CreateNavigator();
        XPathExpression xpath = XPathExpression.Compile("//*[1]");
        var root = navigator?.SelectSingleNode(xpath);

        if (root == null) throw new NullReferenceException("The expected XPath Navigator is not here.");

        nsman = new XmlNamespaceManager(root.NameTable);

        IDictionary<string, string> names = root.GetNamespacesInScope(XmlNamespaceScope.Local);
        foreach (KeyValuePair<string, string> kv in names)
        {
            nsman.AddNamespace(kv.Key, kv.Value);
        }

        return nsman;
    }
}
