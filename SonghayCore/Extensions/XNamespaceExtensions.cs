﻿namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="XNamespace"/>
/// </summary>
public static class XNamespaceExtensions
{
    /// <summary>
    /// Converts the <see cref="XNamespace"/> to an <see cref="XName"/>
    /// with the specified element name.
    /// </summary>
    /// <param name="ns">The ns.</param>
    /// <param name="elementName">Name of the element.</param>
    public static XName? ToXName(this XNamespace? ns, string? elementName) => string.IsNullOrWhiteSpace(elementName)
        ? null
        : ns == null
            ? elementName
            : ns + elementName;
}
