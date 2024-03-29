﻿namespace Songhay.Xml;

public static partial class XObjectUtility
{
    /// <summary>
    /// Gets the <see cref="XDeclaration"/>.
    /// </summary>
    public static XDeclaration GetXDeclaration() => GetXDeclaration(XEncoding.Utf08, true);

    /// <summary>
    /// Gets the <see cref="XDeclaration"/>.
    /// </summary>
    /// <param name="encoding">The encoding (<see cref="XEncoding.Utf08"/> by default).</param>
    /// <param name="isStandAlone">When <c>true</c> document is stand-alone (<c>true</c> by default).</param>
    public static XDeclaration GetXDeclaration(string? encoding, bool isStandAlone) =>
        new("1.0", encoding, isStandAlone ? "yes" : "no");
}
