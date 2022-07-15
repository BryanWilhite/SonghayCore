namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="UriTemplate"/>
/// </summary>
public static class UriTemplateExtensions
{
    /// <summary>
    /// Binds the <see cref="UriTemplate"/>
    /// to the specified <c>params</c> by position.
    /// </summary>
    /// <param name="template">The template.</param>
    /// <param name="values">The values.</param>
    public static Uri? BindByPosition(this UriTemplate? template, params string[] values) =>
        template.BindByPosition(baseUri: null, values: values);

    /// <summary>
    /// Binds the <see cref="UriTemplate" />
    /// to the specified <c>params</c> by position.
    /// </summary>
    /// <param name="template">The template.</param>
    /// <param name="baseUri">The base URI.</param>
    /// <param name="values">The values.</param>
    public static Uri? BindByPosition(this UriTemplate? template, Uri? baseUri, params string[] values)
    {
        ArgumentNullException.ThrowIfNull(template);

        var keys = template.GetParameterNames().ToReferenceTypeValueOrThrow().ToArray();

        for (int i = 0; i < keys.Length; i++)
        {
            template.AddParameter(keys[i], values.ElementAtOrDefault(i));
        }

        var resolved = template.Resolve();
        if (baseUri != null)
        {
            return new UriBuilder(baseUri).WithPath(resolved)!.Uri;
        }

        var isAbsolute = Uri.IsWellFormedUriString(resolved, UriKind.Absolute);
        var isRelative = Uri.IsWellFormedUriString(resolved, UriKind.Relative);
        if (!isAbsolute && !isRelative)
            throw new FormatException($"The resolved URI template, {resolved}, is in an unknown format.");

        return isAbsolute ? new Uri(resolved, UriKind.Absolute) : new Uri(resolved, UriKind.Relative);
    }
}
