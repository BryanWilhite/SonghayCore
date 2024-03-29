﻿namespace Songhay.Models;

/// <summary>
/// The conventional <see cref="XObject"/> metadata.
/// </summary>
public static class XObjectMetadata
{
    /// <summary>
    /// The sitemaps.org namespace.
    /// </summary>
    public static readonly XNamespace SiteMapNamespace = "http://www.sitemaps.org/schemas/sitemap/0.9";

    /// <summary>
    /// DOCTYPE XHTML Transitional declaration.
    /// </summary>
    public static XDocumentType XhtmlDocTypeTransitional =>
        new("xhtml",
            "-//W3C//DTD XHTML 1.0 Transitional//EN",
            "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd", null);
}
