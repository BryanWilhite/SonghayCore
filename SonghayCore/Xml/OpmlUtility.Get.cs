namespace Songhay.Xml;

/// <summary>
/// Static helpers for OPML.
/// </summary>
public static class OpmlUtility
{
    /// <summary>
    /// Conventional namespace for OPML documents.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static XNamespace rx => "http://songhaysystem.com/schemas/opml.xsd";

    /// <summary>
    /// Gets the <see cref="OpmlBody"/>.
    /// </summary>
    /// <param name="root">The root.</param>
    /// <param name="ns">The namespace.</param>
    public static OpmlBody? GetBody(XContainer? root, XNamespace? ns)
    {
        if (root == null) return null;

        var data = new OpmlBody
        {
            Outlines = GetOutlines(root, ns)
        };

        return data;
    }

    /// <summary>
    /// Gets the <see cref="OpmlDocument"/>.
    /// </summary>
    /// <param name="path">The path.</param>
    public static OpmlDocument? GetDocument(string? path)
    {
        var xd = XDocument.Load(path!);

        XNamespace? ns = xd.Root?.GetDefaultNamespace();
        if (ns != null && !string.IsNullOrWhiteSpace(ns.ToString())) return GetDocument(xd.Root, ns);

        ns = xd.Root?.GetNamespaceOfPrefix(nameof(rx));
        if (ns == rx) return GetDocument(xd.Root, rx);

        return GetRawDocument(xd);
    }

    /// <summary>
    /// Gets the <see cref="OpmlDocument"/>.
    /// </summary>
    /// <param name="xml">The XML.</param>
    /// <param name="ns">The ns.</param>
    public static OpmlDocument? GetDocument(string? xml, XNamespace? ns)
    {
        var xd = XDocument.Parse(xml!);
        var opml = GetDocument(xd.Root, ns);

        return opml;
    }

    /// <summary>
    /// Gets the <see cref="OpmlDocument"/>.
    /// </summary>
    /// <param name="root">The root.</param>
    /// <param name="ns">The conventional namespace.</param>
    public static OpmlDocument? GetDocument(XContainer? root, XNamespace? ns)
    {
        if (root == null) return null;

        var data = new OpmlDocument
        {
            OpmlBody = GetBody(root.Element(ns.ToXName("body")!), ns),
            OpmlHead = GetHead(root.Element(ns.ToXName("head")!), ns)
        };

        return data;
    }

    /// <summary>
    /// Gets the <see cref="OpmlHead"/>.
    /// </summary>
    /// <param name="root">The root.</param>
    /// <param name="ns">The namespace.</param>
    public static OpmlHead? GetHead(XContainer? root, XNamespace? ns)
    {
        if (root == null) return null;

        var ownerEmail = ns.ToXName("ownerEmail");
        var ownerName = ns.ToXName("ownerName");
        var title = ns.ToXName("title");
        var dateCreated = ns.ToXName("dateCreated");
        var dateModified = ns.ToXName("dateModified");

        return GetHead(root, ownerEmail, ownerName, title, dateCreated, dateModified);
    }

    /// <summary>
    /// Gets the <see cref="OpmlOutline"/>.
    /// </summary>
    /// <param name="root">The root.</param>
    /// <param name="ns">The namespace.</param>
    public static OpmlOutline? GetOutline(XElement? root, XNamespace? ns)
    {
        if (root == null) return null;

        var data = GetOutline(root);
        data.Outlines = GetOutlines(root, ns);

        return data;
    }

    /// <summary>
    /// Gets the array of <see cref="OpmlOutline"/>.
    /// </summary>
    /// <param name="root">The root.</param>
    /// <param name="ns">The namespace.</param>
    public static OpmlOutline[] GetOutlines(XContainer? root, XNamespace? ns)
    {
        if (root == null) return Enumerable.Empty<OpmlOutline>().ToArray();

        var data = new List<OpmlOutline>();
        root
            .Elements(ns.ToXName("outline"))
            .ForEachInEnumerable(o =>
            {
                var outline = GetOutline(o, ns);
                if (outline == null) return;

                outline.Outlines = GetOutlines(o, ns);
                data.Add(outline);
            });

        return data.ToArray();
    }

    static OpmlBody? GetBody(XContainer? root)
    {
        if (root == null) return null;

        var data = new OpmlBody
        {
            Outlines = GetOutlines(root)
        };

        return data;
    }

    static OpmlHead GetHead(XContainer? root, XName? ownerEmail, XName? ownerName, XName? title, XName? dateCreated,
        XName? dateModified)
    {
        var data = new OpmlHead
        {
            OwnerEmail = root?.Elements(ownerEmail).ToElementValueOrNull(),
            OwnerName = root?.Elements(ownerName).ToElementValueOrNull(),
            Title = root?.Elements(title).ToElementValueOrNull()
        };

        string? s;

        s = root?.Elements(dateCreated).ToElementValueOrNull();
        if (!string.IsNullOrWhiteSpace(s)) data.DateCreated = ProgramTypeUtility.ParseRfc822DateTime(s);

        s = root?.Elements(dateModified).ToElementValueOrNull();
        if (!string.IsNullOrWhiteSpace(s)) data.DateModified = ProgramTypeUtility.ParseRfc822DateTime(s);

        return data;
    }

    static OpmlDocument? GetRawDocument(XContainer? root)
    {
        if (root == null) return null;

        var data = new OpmlDocument
        {
            OpmlBody = GetBody(root.Descendants("body").FirstOrDefault()),
            OpmlHead = GetHead(root.Descendants("head").FirstOrDefault(), "ownerEmail", "ownerName", "title",
                "dateCreated", "dateModified")
        };

        return data;
    }

    static OpmlOutline GetOutline(XElement? root)
    {
        var data = new OpmlOutline
        {
            Category = root.ToAttributeValueOrNull("category"),
            Id = root.ToAttributeValueOrNull("id"),
            Text = root.ToAttributeValueOrNull("text"),
            Title = root.ToAttributeValueOrNull("title"),
            OutlineType = root.ToAttributeValueOrNull("type"),
            Url = root.ToAttributeValueOrNull("url"),
            XmlUrl = root.ToAttributeValueOrNull("xmlUrl")
        };

        if (data.Url != null)
            data.Url = Environment.ExpandEnvironmentVariables(data.Url);
        if (data.XmlUrl != null)
            data.XmlUrl = Environment.ExpandEnvironmentVariables(data.XmlUrl);

        return data;
    }

    static OpmlOutline[] GetOutlines(XContainer? root)
    {
        if (root == null) return Enumerable.Empty<OpmlOutline>().ToArray();

        var data = new List<OpmlOutline>();
        root
            .Elements("outline")
            .ForEachInEnumerable(o =>
            {
                var outline = GetOutline(o);
                outline.Outlines = GetOutlines(o);
                data.Add(outline);
            });

        return data.ToArray();
    }
}
