using System;
using System.Xml.XPath;

namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="XPathNavigator"/>
/// </summary>
public static class XPathNavigatorExtensions
{
    /// <summary>
    /// Ensures the specified <see cref="XPathNavigator"/>
    /// or throws an exception.
    /// </summary>
    /// <param name="navigator"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static XPathNavigator EnsureXPathNavigator(this XPathNavigator? navigator)
    {
        if (navigator == null) throw new ArgumentNullException(nameof(navigator));

        return navigator;
    }
}
