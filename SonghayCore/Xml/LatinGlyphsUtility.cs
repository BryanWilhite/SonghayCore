using Songhay.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Songhay.Xml;

/// <summary>
/// Condenses and expands Latin glyphs.
/// </summary>
public static class LatinGlyphsUtility
{
    /// <summary>
    /// Condenses selected decimal entities
    /// into their Latin glyph equivalent.
    /// </summary>
    /// <param name="entityText">
    /// The <see cref="string"/>
    /// containing the decimal entities.
    /// </param>
    /// <returns>
    /// Returns a <see cref="string"/>
    /// with Latin glyphs.
    /// </returns>
    public static string? Condense(string? entityText)
    {
        if (string.IsNullOrEmpty(entityText)) return entityText;

        foreach (var datum in GetGlyphsForCondenseOrExpand())
        {
            if (datum.HtmlEntityName?.Length > 0 && entityText.Contains(datum.HtmlEntityName))
                entityText = Regex.Replace(entityText, Regex.Escape(datum.HtmlEntityName), datum.Character!,
                    RegexOptions.IgnoreCase);

            if (entityText.Contains(datum.XmlEntityNumber!))
                entityText = Regex.Replace(entityText, Regex.Escape(datum.XmlEntityNumber!), datum.Character!,
                    RegexOptions.IgnoreCase);
        }

        return entityText;
    }

    /// <summary>
    /// Expands selected Latin glyphs
    /// into their decimal entity equivalent.
    /// </summary>
    /// <param name="glyphText">
    /// The <see cref="string"/>
    /// containing the glyphs.
    /// </param>
    /// <returns>
    /// Returns a <see cref="string"/>
    /// with decimal entities.
    /// </returns>
    public static string? Expand(string? glyphText)
    {
        if (string.IsNullOrEmpty(glyphText)) return glyphText;

        foreach (var datum in GetGlyphsForCondenseOrExpand())
        {
            if (!glyphText.Contains(datum.Character!)) continue;
            glyphText = glyphText.Replace(datum.Character!, datum.XmlEntityNumber);
        }

        return glyphText;
    }

    /// <summary>
    /// Gets the glyphs.
    /// </summary>
    /// <returns></returns>
    public static ProgramGlyph[] GetGlyphs() => Glyphs.Value;

    /// <summary>
    /// Gets the glyphs for <see cref="Condense(string)"/> or <see cref="Expand(string)"/>.
    /// </summary>
    /// <returns></returns>
    public static ProgramGlyph[] GetGlyphsForCondenseOrExpand()
    {
        var basicLatin = Glyphs.Value
            .Where(g => g.Character?.Length > 0)
            .Where(g => !string.IsNullOrWhiteSpace(g.HtmlEntityName))
            .Where(g => g.UnicodeInteger < 128);

        var beyondBasicLatin = Glyphs.Value
            .Where(g => g.Character?.Length > 0)
            .Where(g => !string.IsNullOrWhiteSpace(g.HtmlEntityName))
            .Where(g => g.UnicodeInteger > 127);

        return basicLatin.Union(beyondBasicLatin).ToArray();
    }

    /// <summary>
    /// Removes the URL encodings.
    /// </summary>
    /// <param name="input">The input.</param>
    public static string RemoveUrlEncodings(string input) =>
        RemoveUrlEncodings(input, includeWindows1252UrlEncoding: false);

    /// <summary>
    /// Removes the URL encodings.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <param name="includeWindows1252UrlEncoding">if set to <c>true</c> [include windows1252 URL encoding].</param>
    /// <returns></returns>
    public static string RemoveUrlEncodings(string input, bool includeWindows1252UrlEncoding)
    {
        foreach (var glyph in GetGlyphs())
        {
            if (includeWindows1252UrlEncoding)
                input = Regex.Replace(input, Regex.Escape(glyph.Utf8UrlEncoding!), string.Empty,
                    RegexOptions.IgnoreCase);

            input = Regex.Replace(input, Regex.Escape(glyph.Utf8UrlEncoding!), string.Empty, RegexOptions.IgnoreCase);
        }

        return input;
    }

    static readonly Lazy<ProgramGlyph[]> Glyphs =
        new(() => new ProgramGlyph[]
            {
                new()
                {
                    UnicodePoint = "20", UnicodeGroup = "Basic Latin", UnicodeName = "Space", Character = " ",
                    Windows1252UrlEncoding = "%20", Utf8UrlEncoding = "%20", HtmlEntityName = "",
                    XmlEntityNumber = "&#32;"
                },
                new()
                {
                    UnicodePoint = "21", UnicodeGroup = "Basic Latin", UnicodeName = "Exclamation Mark",
                    Character = "!", Windows1252UrlEncoding = "%21", Utf8UrlEncoding = "%21", HtmlEntityName = "",
                    XmlEntityNumber = "&#33;"
                },
                new()
                {
                    UnicodePoint = "22", UnicodeGroup = "Basic Latin", UnicodeName = "Quotation Mark", Character = "\"",
                    Windows1252UrlEncoding = "%22", Utf8UrlEncoding = "%22", HtmlEntityName = "&quot;",
                    XmlEntityNumber = "&#34;"
                },
                new()
                {
                    UnicodePoint = "23", UnicodeGroup = "Basic Latin", UnicodeName = "Number Sign", Character = "#",
                    Windows1252UrlEncoding = "%23", Utf8UrlEncoding = "%23", HtmlEntityName = "",
                    XmlEntityNumber = "&#35;"
                },
                new()
                {
                    UnicodePoint = "24", UnicodeGroup = "Basic Latin", UnicodeName = "Dollar Sign", Character = "$",
                    Windows1252UrlEncoding = "%24", Utf8UrlEncoding = "%24", HtmlEntityName = "",
                    XmlEntityNumber = "&#36;"
                },
                new()
                {
                    UnicodePoint = "25", UnicodeGroup = "Basic Latin", UnicodeName = "Percent Sign", Character = "%",
                    Windows1252UrlEncoding = "%25", Utf8UrlEncoding = "%25", HtmlEntityName = "",
                    XmlEntityNumber = "&#37;"
                },
                new()
                {
                    UnicodePoint = "26", UnicodeGroup = "Basic Latin", UnicodeName = "Ampersand", Character = "&",
                    Windows1252UrlEncoding = "%26", Utf8UrlEncoding = "%26", HtmlEntityName = "&amp;",
                    XmlEntityNumber = "&#38;"
                },
                new()
                {
                    UnicodePoint = "27", UnicodeGroup = "Basic Latin", UnicodeName = "Apostrophe", Character = "'",
                    Windows1252UrlEncoding = "%27", Utf8UrlEncoding = "%27", HtmlEntityName = "&apos;",
                    XmlEntityNumber = "&#39;"
                },
                new()
                {
                    UnicodePoint = "28", UnicodeGroup = "Basic Latin", UnicodeName = "Left Parenthesis",
                    Character = "(", Windows1252UrlEncoding = "%28", Utf8UrlEncoding = "%28", HtmlEntityName = "",
                    XmlEntityNumber = "&#40;"
                },
                new()
                {
                    UnicodePoint = "29", UnicodeGroup = "Basic Latin", UnicodeName = "Right Parenthesis",
                    Character = ")", Windows1252UrlEncoding = "%29", Utf8UrlEncoding = "%29", HtmlEntityName = "",
                    XmlEntityNumber = "&#41;"
                },
                new()
                {
                    UnicodePoint = "2a", UnicodeGroup = "Basic Latin", UnicodeName = "Asterisk", Character = "*",
                    Windows1252UrlEncoding = "%2A", Utf8UrlEncoding = "%2A", HtmlEntityName = "",
                    XmlEntityNumber = "&#42;"
                },
                new()
                {
                    UnicodePoint = "2b", UnicodeGroup = "Basic Latin", UnicodeName = "Plus Sign", Character = "+",
                    Windows1252UrlEncoding = "%2B", Utf8UrlEncoding = "%2B", HtmlEntityName = "",
                    XmlEntityNumber = "&#43;"
                },
                new()
                {
                    UnicodePoint = "2c", UnicodeGroup = "Basic Latin", UnicodeName = "Comma", Character = ",",
                    Windows1252UrlEncoding = "%2C", Utf8UrlEncoding = "%2C", HtmlEntityName = "",
                    XmlEntityNumber = "&#44;"
                },
                new()
                {
                    UnicodePoint = "2d", UnicodeGroup = "Basic Latin", UnicodeName = "Hyphen-Minus", Character = "-",
                    Windows1252UrlEncoding = "%2D", Utf8UrlEncoding = "%2D", HtmlEntityName = "",
                    XmlEntityNumber = "&#45;"
                },
                new()
                {
                    UnicodePoint = "2e", UnicodeGroup = "Basic Latin", UnicodeName = "Full Stop", Character = ".",
                    Windows1252UrlEncoding = "%2E", Utf8UrlEncoding = "%2E", HtmlEntityName = "",
                    XmlEntityNumber = "&#46;"
                },
                new()
                {
                    UnicodePoint = "2f", UnicodeGroup = "Basic Latin", UnicodeName = "Solidus", Character = "/",
                    Windows1252UrlEncoding = "%2F", Utf8UrlEncoding = "%2F", HtmlEntityName = "",
                    XmlEntityNumber = "&#47;"
                },
                new()
                {
                    UnicodePoint = "30", UnicodeGroup = "Basic Latin", UnicodeName = "Digit Zero", Character = "",
                    Windows1252UrlEncoding = "%30", Utf8UrlEncoding = "%30", HtmlEntityName = "",
                    XmlEntityNumber = "&#48;"
                },
                new()
                {
                    UnicodePoint = "31", UnicodeGroup = "Basic Latin", UnicodeName = "Digit One", Character = "",
                    Windows1252UrlEncoding = "%31", Utf8UrlEncoding = "%31", HtmlEntityName = "",
                    XmlEntityNumber = "&#49;"
                },
                new()
                {
                    UnicodePoint = "32", UnicodeGroup = "Basic Latin", UnicodeName = "Digit Two", Character = "",
                    Windows1252UrlEncoding = "%32", Utf8UrlEncoding = "%32", HtmlEntityName = "",
                    XmlEntityNumber = "&#50;"
                },
                new()
                {
                    UnicodePoint = "33", UnicodeGroup = "Basic Latin", UnicodeName = "Digit Three", Character = "",
                    Windows1252UrlEncoding = "%33", Utf8UrlEncoding = "%33", HtmlEntityName = "",
                    XmlEntityNumber = "&#51;"
                },
                new()
                {
                    UnicodePoint = "34", UnicodeGroup = "Basic Latin", UnicodeName = "Digit Four", Character = "",
                    Windows1252UrlEncoding = "%34", Utf8UrlEncoding = "%34", HtmlEntityName = "",
                    XmlEntityNumber = "&#52;"
                },
                new()
                {
                    UnicodePoint = "35", UnicodeGroup = "Basic Latin", UnicodeName = "Digit Five", Character = "",
                    Windows1252UrlEncoding = "%35", Utf8UrlEncoding = "%35", HtmlEntityName = "",
                    XmlEntityNumber = "&#53;"
                },
                new()
                {
                    UnicodePoint = "36", UnicodeGroup = "Basic Latin", UnicodeName = "Digit Six", Character = "",
                    Windows1252UrlEncoding = "%36", Utf8UrlEncoding = "%36", HtmlEntityName = "",
                    XmlEntityNumber = "&#54;"
                },
                new()
                {
                    UnicodePoint = "37", UnicodeGroup = "Basic Latin", UnicodeName = "Digit Seven", Character = "",
                    Windows1252UrlEncoding = "%37", Utf8UrlEncoding = "%37", HtmlEntityName = "",
                    XmlEntityNumber = "&#55;"
                },
                new()
                {
                    UnicodePoint = "38", UnicodeGroup = "Basic Latin", UnicodeName = "Digit Eight", Character = "",
                    Windows1252UrlEncoding = "%38", Utf8UrlEncoding = "%38", HtmlEntityName = "",
                    XmlEntityNumber = "&#56;"
                },
                new()
                {
                    UnicodePoint = "39", UnicodeGroup = "Basic Latin", UnicodeName = "Digit Nine", Character = "",
                    Windows1252UrlEncoding = "%39", Utf8UrlEncoding = "%39", HtmlEntityName = "",
                    XmlEntityNumber = "&#57;"
                },
                new()
                {
                    UnicodePoint = "3a", UnicodeGroup = "Basic Latin", UnicodeName = "Colon", Character = ":",
                    Windows1252UrlEncoding = "%3A", Utf8UrlEncoding = "%3A", HtmlEntityName = "",
                    XmlEntityNumber = "&#58;"
                },
                new()
                {
                    UnicodePoint = "3b", UnicodeGroup = "Basic Latin", UnicodeName = "Semicolon", Character = ";",
                    Windows1252UrlEncoding = "%3B", Utf8UrlEncoding = "%3B", HtmlEntityName = "",
                    XmlEntityNumber = "&#59;"
                },
                new()
                {
                    UnicodePoint = "3c", UnicodeGroup = "Basic Latin", UnicodeName = "Less-Than Sign", Character = "<",
                    Windows1252UrlEncoding = "%3C", Utf8UrlEncoding = "%3C", HtmlEntityName = "&lt;",
                    XmlEntityNumber = "&#60;"
                },
                new()
                {
                    UnicodePoint = "3d", UnicodeGroup = "Basic Latin", UnicodeName = "Equals Sign", Character = "=",
                    Windows1252UrlEncoding = "%3D", Utf8UrlEncoding = "%3D", HtmlEntityName = "",
                    XmlEntityNumber = "&#61;"
                },
                new()
                {
                    UnicodePoint = "3e", UnicodeGroup = "Basic Latin", UnicodeName = "Greater-Than Sign",
                    Character = ">", Windows1252UrlEncoding = "%3E", Utf8UrlEncoding = "%3E", HtmlEntityName = "&gt;",
                    XmlEntityNumber = "&#62;"
                },
                new()
                {
                    UnicodePoint = "3f", UnicodeGroup = "Basic Latin", UnicodeName = "Question Mark", Character = "?",
                    Windows1252UrlEncoding = "%3F", Utf8UrlEncoding = "%3F", HtmlEntityName = "",
                    XmlEntityNumber = "&#63;"
                },
                new()
                {
                    UnicodePoint = "40", UnicodeGroup = "Basic Latin", UnicodeName = "Commercial At", Character = "@",
                    Windows1252UrlEncoding = "%40", Utf8UrlEncoding = "%40", HtmlEntityName = "",
                    XmlEntityNumber = "&#64;"
                },
                new()
                {
                    UnicodePoint = "41", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Capital Letter A",
                    Character = "A", Windows1252UrlEncoding = "%41", Utf8UrlEncoding = "%41", HtmlEntityName = "",
                    XmlEntityNumber = "&#65;"
                },
                new()
                {
                    UnicodePoint = "42", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Capital Letter B",
                    Character = "B", Windows1252UrlEncoding = "%42", Utf8UrlEncoding = "%42", HtmlEntityName = "",
                    XmlEntityNumber = "&#66;"
                },
                new()
                {
                    UnicodePoint = "43", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Capital Letter C",
                    Character = "C", Windows1252UrlEncoding = "%43", Utf8UrlEncoding = "%43", HtmlEntityName = "",
                    XmlEntityNumber = "&#67;"
                },
                new()
                {
                    UnicodePoint = "44", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Capital Letter D",
                    Character = "D", Windows1252UrlEncoding = "%44", Utf8UrlEncoding = "%44", HtmlEntityName = "",
                    XmlEntityNumber = "&#68;"
                },
                new()
                {
                    UnicodePoint = "45", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Capital Letter E",
                    Character = "E", Windows1252UrlEncoding = "%45", Utf8UrlEncoding = "%45", HtmlEntityName = "",
                    XmlEntityNumber = "&#69;"
                },
                new()
                {
                    UnicodePoint = "46", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Capital Letter F",
                    Character = "F", Windows1252UrlEncoding = "%46", Utf8UrlEncoding = "%46", HtmlEntityName = "",
                    XmlEntityNumber = "&#70;"
                },
                new()
                {
                    UnicodePoint = "47", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Capital Letter G",
                    Character = "G", Windows1252UrlEncoding = "%47", Utf8UrlEncoding = "%47", HtmlEntityName = "",
                    XmlEntityNumber = "&#71;"
                },
                new()
                {
                    UnicodePoint = "48", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Capital Letter H",
                    Character = "H", Windows1252UrlEncoding = "%48", Utf8UrlEncoding = "%48", HtmlEntityName = "",
                    XmlEntityNumber = "&#72;"
                },
                new()
                {
                    UnicodePoint = "49", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Capital Letter I",
                    Character = "I", Windows1252UrlEncoding = "%49", Utf8UrlEncoding = "%49", HtmlEntityName = "",
                    XmlEntityNumber = "&#73;"
                },
                new()
                {
                    UnicodePoint = "4a", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Capital Letter J",
                    Character = "J", Windows1252UrlEncoding = "%4A", Utf8UrlEncoding = "%4A", HtmlEntityName = "",
                    XmlEntityNumber = "&#74;"
                },
                new()
                {
                    UnicodePoint = "4b", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Capital Letter K",
                    Character = "K", Windows1252UrlEncoding = "%4B", Utf8UrlEncoding = "%4B", HtmlEntityName = "",
                    XmlEntityNumber = "&#75;"
                },
                new()
                {
                    UnicodePoint = "4c", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Capital Letter L",
                    Character = "L", Windows1252UrlEncoding = "%4C", Utf8UrlEncoding = "%4C", HtmlEntityName = "",
                    XmlEntityNumber = "&#76;"
                },
                new()
                {
                    UnicodePoint = "4d", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Capital Letter M",
                    Character = "M", Windows1252UrlEncoding = "%4D", Utf8UrlEncoding = "%4D", HtmlEntityName = "",
                    XmlEntityNumber = "&#77;"
                },
                new()
                {
                    UnicodePoint = "4e", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Capital Letter N",
                    Character = "N", Windows1252UrlEncoding = "%4E", Utf8UrlEncoding = "%4E", HtmlEntityName = "",
                    XmlEntityNumber = "&#78;"
                },
                new()
                {
                    UnicodePoint = "4f", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Capital Letter O",
                    Character = "O", Windows1252UrlEncoding = "%4F", Utf8UrlEncoding = "%4F", HtmlEntityName = "",
                    XmlEntityNumber = "&#79;"
                },
                new()
                {
                    UnicodePoint = "50", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Capital Letter P",
                    Character = "P", Windows1252UrlEncoding = "%50", Utf8UrlEncoding = "%50", HtmlEntityName = "",
                    XmlEntityNumber = "&#80;"
                },
                new()
                {
                    UnicodePoint = "51", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Capital Letter Q",
                    Character = "Q", Windows1252UrlEncoding = "%51", Utf8UrlEncoding = "%51", HtmlEntityName = "",
                    XmlEntityNumber = "&#81;"
                },
                new()
                {
                    UnicodePoint = "52", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Capital Letter R",
                    Character = "R", Windows1252UrlEncoding = "%52", Utf8UrlEncoding = "%52", HtmlEntityName = "",
                    XmlEntityNumber = "&#82;"
                },
                new()
                {
                    UnicodePoint = "53", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Capital Letter S",
                    Character = "S", Windows1252UrlEncoding = "%53", Utf8UrlEncoding = "%53", HtmlEntityName = "",
                    XmlEntityNumber = "&#83;"
                },
                new()
                {
                    UnicodePoint = "54", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Capital Letter T",
                    Character = "T", Windows1252UrlEncoding = "%54", Utf8UrlEncoding = "%54", HtmlEntityName = "",
                    XmlEntityNumber = "&#84;"
                },
                new()
                {
                    UnicodePoint = "55", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Capital Letter U",
                    Character = "U", Windows1252UrlEncoding = "%55", Utf8UrlEncoding = "%55", HtmlEntityName = "",
                    XmlEntityNumber = "&#85;"
                },
                new()
                {
                    UnicodePoint = "56", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Capital Letter V",
                    Character = "V", Windows1252UrlEncoding = "%56", Utf8UrlEncoding = "%56", HtmlEntityName = "",
                    XmlEntityNumber = "&#86;"
                },
                new()
                {
                    UnicodePoint = "57", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Capital Letter W",
                    Character = "W", Windows1252UrlEncoding = "%57", Utf8UrlEncoding = "%57", HtmlEntityName = "",
                    XmlEntityNumber = "&#87;"
                },
                new()
                {
                    UnicodePoint = "58", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Capital Letter X",
                    Character = "X", Windows1252UrlEncoding = "%58", Utf8UrlEncoding = "%58", HtmlEntityName = "",
                    XmlEntityNumber = "&#88;"
                },
                new()
                {
                    UnicodePoint = "59", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Capital Letter Y",
                    Character = "Y", Windows1252UrlEncoding = "%59", Utf8UrlEncoding = "%59", HtmlEntityName = "",
                    XmlEntityNumber = "&#89;"
                },
                new()
                {
                    UnicodePoint = "5a", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Capital Letter Z",
                    Character = "Z", Windows1252UrlEncoding = "%5A", Utf8UrlEncoding = "%5A", HtmlEntityName = "",
                    XmlEntityNumber = "&#90;"
                },
                new()
                {
                    UnicodePoint = "5b", UnicodeGroup = "Basic Latin", UnicodeName = "Left Square Bracket",
                    Character = "[", Windows1252UrlEncoding = "%5B", Utf8UrlEncoding = "%5B", HtmlEntityName = "",
                    XmlEntityNumber = "&#91;"
                },
                new()
                {
                    UnicodePoint = "5c", UnicodeGroup = "Basic Latin", UnicodeName = "Reverse Solidus",
                    Character = "\\", Windows1252UrlEncoding = "%5C", Utf8UrlEncoding = "%5C", HtmlEntityName = "",
                    XmlEntityNumber = "&#92;"
                },
                new()
                {
                    UnicodePoint = "5d", UnicodeGroup = "Basic Latin", UnicodeName = "Right Square Bracket",
                    Character = "]", Windows1252UrlEncoding = "%5D", Utf8UrlEncoding = "%5D", HtmlEntityName = "",
                    XmlEntityNumber = "&#93;"
                },
                new()
                {
                    UnicodePoint = "5e", UnicodeGroup = "Basic Latin", UnicodeName = "Circumflex Accent",
                    Character = "^", Windows1252UrlEncoding = "%5E", Utf8UrlEncoding = "%5E", HtmlEntityName = "",
                    XmlEntityNumber = "&#94;"
                },
                new()
                {
                    UnicodePoint = "5f", UnicodeGroup = "Basic Latin", UnicodeName = "Low Line", Character = "nullable",
                    Windows1252UrlEncoding = "%5F", Utf8UrlEncoding = "%5F", HtmlEntityName = "",
                    XmlEntityNumber = "&#95;"
                },
                new()
                {
                    UnicodePoint = "60", UnicodeGroup = "Basic Latin", UnicodeName = "Grave Accent", Character = "`",
                    Windows1252UrlEncoding = "%60", Utf8UrlEncoding = "%60", HtmlEntityName = "",
                    XmlEntityNumber = "&#96;"
                },
                new()
                {
                    UnicodePoint = "61", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Small Letter A",
                    Character = "a", Windows1252UrlEncoding = "%61", Utf8UrlEncoding = "%61", HtmlEntityName = "",
                    XmlEntityNumber = "&#97;"
                },
                new()
                {
                    UnicodePoint = "62", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Small Letter B",
                    Character = "b", Windows1252UrlEncoding = "%62", Utf8UrlEncoding = "%62", HtmlEntityName = "",
                    XmlEntityNumber = "&#98;"
                },
                new()
                {
                    UnicodePoint = "63", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Small Letter C",
                    Character = "c", Windows1252UrlEncoding = "%63", Utf8UrlEncoding = "%63", HtmlEntityName = "",
                    XmlEntityNumber = "&#99;"
                },
                new()
                {
                    UnicodePoint = "64", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Small Letter D",
                    Character = "d", Windows1252UrlEncoding = "%64", Utf8UrlEncoding = "%64", HtmlEntityName = "",
                    XmlEntityNumber = "&#100;"
                },
                new()
                {
                    UnicodePoint = "65", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Small Letter E",
                    Character = "e", Windows1252UrlEncoding = "%65", Utf8UrlEncoding = "%65", HtmlEntityName = "",
                    XmlEntityNumber = "&#101;"
                },
                new()
                {
                    UnicodePoint = "66", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Small Letter F",
                    Character = "f", Windows1252UrlEncoding = "%66", Utf8UrlEncoding = "%66", HtmlEntityName = "",
                    XmlEntityNumber = "&#102;"
                },
                new()
                {
                    UnicodePoint = "67", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Small Letter G",
                    Character = "g", Windows1252UrlEncoding = "%67", Utf8UrlEncoding = "%67", HtmlEntityName = "",
                    XmlEntityNumber = "&#103;"
                },
                new()
                {
                    UnicodePoint = "68", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Small Letter H",
                    Character = "h", Windows1252UrlEncoding = "%68", Utf8UrlEncoding = "%68", HtmlEntityName = "",
                    XmlEntityNumber = "&#104;"
                },
                new()
                {
                    UnicodePoint = "69", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Small Letter I",
                    Character = "i", Windows1252UrlEncoding = "%69", Utf8UrlEncoding = "%69", HtmlEntityName = "",
                    XmlEntityNumber = "&#105;"
                },
                new()
                {
                    UnicodePoint = "6a", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Small Letter J",
                    Character = "j", Windows1252UrlEncoding = "%6A", Utf8UrlEncoding = "%6A", HtmlEntityName = "",
                    XmlEntityNumber = "&#106;"
                },
                new()
                {
                    UnicodePoint = "6b", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Small Letter K",
                    Character = "k", Windows1252UrlEncoding = "%6B", Utf8UrlEncoding = "%6B", HtmlEntityName = "",
                    XmlEntityNumber = "&#107;"
                },
                new()
                {
                    UnicodePoint = "6c", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Small Letter L",
                    Character = "l", Windows1252UrlEncoding = "%6C", Utf8UrlEncoding = "%6C", HtmlEntityName = "",
                    XmlEntityNumber = "&#108;"
                },
                new()
                {
                    UnicodePoint = "6d", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Small Letter M",
                    Character = "m", Windows1252UrlEncoding = "%6D", Utf8UrlEncoding = "%6D", HtmlEntityName = "",
                    XmlEntityNumber = "&#109;"
                },
                new()
                {
                    UnicodePoint = "6e", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Small Letter N",
                    Character = "n", Windows1252UrlEncoding = "%6E", Utf8UrlEncoding = "%6E", HtmlEntityName = "",
                    XmlEntityNumber = "&#110;"
                },
                new()
                {
                    UnicodePoint = "6f", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Small Letter O",
                    Character = "o", Windows1252UrlEncoding = "%6F", Utf8UrlEncoding = "%6F", HtmlEntityName = "",
                    XmlEntityNumber = "&#111;"
                },
                new()
                {
                    UnicodePoint = "70", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Small Letter P",
                    Character = "p", Windows1252UrlEncoding = "%70", Utf8UrlEncoding = "%70", HtmlEntityName = "",
                    XmlEntityNumber = "&#112;"
                },
                new()
                {
                    UnicodePoint = "71", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Small Letter Q",
                    Character = "q", Windows1252UrlEncoding = "%71", Utf8UrlEncoding = "%71", HtmlEntityName = "",
                    XmlEntityNumber = "&#113;"
                },
                new()
                {
                    UnicodePoint = "72", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Small Letter R",
                    Character = "r", Windows1252UrlEncoding = "%72", Utf8UrlEncoding = "%72", HtmlEntityName = "",
                    XmlEntityNumber = "&#114;"
                },
                new()
                {
                    UnicodePoint = "73", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Small Letter S",
                    Character = "s", Windows1252UrlEncoding = "%73", Utf8UrlEncoding = "%73", HtmlEntityName = "",
                    XmlEntityNumber = "&#115;"
                },
                new()
                {
                    UnicodePoint = "74", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Small Letter T",
                    Character = "t", Windows1252UrlEncoding = "%74", Utf8UrlEncoding = "%74", HtmlEntityName = "",
                    XmlEntityNumber = "&#116;"
                },
                new()
                {
                    UnicodePoint = "75", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Small Letter U",
                    Character = "u", Windows1252UrlEncoding = "%75", Utf8UrlEncoding = "%75", HtmlEntityName = "",
                    XmlEntityNumber = "&#117;"
                },
                new()
                {
                    UnicodePoint = "76", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Small Letter V",
                    Character = "v", Windows1252UrlEncoding = "%76", Utf8UrlEncoding = "%76", HtmlEntityName = "",
                    XmlEntityNumber = "&#118;"
                },
                new()
                {
                    UnicodePoint = "77", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Small Letter W",
                    Character = "w", Windows1252UrlEncoding = "%77", Utf8UrlEncoding = "%77", HtmlEntityName = "",
                    XmlEntityNumber = "&#119;"
                },
                new()
                {
                    UnicodePoint = "78", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Small Letter X",
                    Character = "x", Windows1252UrlEncoding = "%78", Utf8UrlEncoding = "%78", HtmlEntityName = "",
                    XmlEntityNumber = "&#120;"
                },
                new()
                {
                    UnicodePoint = "79", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Small Letter Y",
                    Character = "y", Windows1252UrlEncoding = "%79", Utf8UrlEncoding = "%79", HtmlEntityName = "",
                    XmlEntityNumber = "&#121;"
                },
                new()
                {
                    UnicodePoint = "7a", UnicodeGroup = "Basic Latin", UnicodeName = "Latin Small Letter Z",
                    Character = "z", Windows1252UrlEncoding = "%7A", Utf8UrlEncoding = "%7A", HtmlEntityName = "",
                    XmlEntityNumber = "&#122;"
                },
                new()
                {
                    UnicodePoint = "7b", UnicodeGroup = "Basic Latin", UnicodeName = "Left Curly Bracket",
                    Character = "{", Windows1252UrlEncoding = "%7B", Utf8UrlEncoding = "%7B", HtmlEntityName = "",
                    XmlEntityNumber = "&#123;"
                },
                new()
                {
                    UnicodePoint = "7c", UnicodeGroup = "Basic Latin", UnicodeName = "Vertical Line", Character = "|",
                    Windows1252UrlEncoding = "%7C", Utf8UrlEncoding = "%7C", HtmlEntityName = "",
                    XmlEntityNumber = "&#124;"
                },
                new()
                {
                    UnicodePoint = "7d", UnicodeGroup = "Basic Latin", UnicodeName = "Right Curly Bracket",
                    Character = "}", Windows1252UrlEncoding = "%7D", Utf8UrlEncoding = "%7D", HtmlEntityName = "",
                    XmlEntityNumber = "&#125;"
                },
                new()
                {
                    UnicodePoint = "7e", UnicodeGroup = "Basic Latin", UnicodeName = "Tilde", Character = "~",
                    Windows1252UrlEncoding = "%7E", Utf8UrlEncoding = "%7E", HtmlEntityName = "",
                    XmlEntityNumber = "&#126;"
                },
                new()
                {
                    UnicodePoint = "7f", UnicodeGroup = "Basic Latin", UnicodeName = "Delete", Character = "",
                    Windows1252UrlEncoding = "%7F", Utf8UrlEncoding = "%7F", HtmlEntityName = "",
                    XmlEntityNumber = "&#127;"
                },
                new()
                {
                    UnicodePoint = "80", UnicodeGroup = "Latin-1 Supplement", UnicodeName = "Control 0080",
                    Character = "`", Windows1252UrlEncoding = "%80", Utf8UrlEncoding = "%E2%82%AC", HtmlEntityName = "",
                    XmlEntityNumber = "&#128;"
                },
                new()
                {
                    UnicodePoint = "81", UnicodeGroup = "Latin-1 Supplement", UnicodeName = "Control 0081",
                    Character = "", Windows1252UrlEncoding = "%81", Utf8UrlEncoding = "%81", HtmlEntityName = "",
                    XmlEntityNumber = "&#129;"
                },
                new()
                {
                    UnicodePoint = "201a", UnicodeGroup = "General Punctuation",
                    UnicodeName = "Single Low-9 Quotation Mark", Character = "‚", Windows1252UrlEncoding = "%82",
                    Utf8UrlEncoding = "%E2%80%9A", HtmlEntityName = "&sbquo;", XmlEntityNumber = "&#8218;"
                },
                new()
                {
                    UnicodePoint = "192", UnicodeGroup = "Latin Extended-B",
                    UnicodeName = "Latin Small Letter F With Hook", Character = "ƒ", Windows1252UrlEncoding = "%83",
                    Utf8UrlEncoding = "%C6%92", HtmlEntityName = "&fnof;", XmlEntityNumber = "&#402;"
                },
                new()
                {
                    UnicodePoint = "201e", UnicodeGroup = "General Punctuation",
                    UnicodeName = "Double Low-9 Quotation Mark", Character = "„", Windows1252UrlEncoding = "%84",
                    Utf8UrlEncoding = "%E2%80%9E", HtmlEntityName = "&bdquo;", XmlEntityNumber = "&#8222;"
                },
                new()
                {
                    UnicodePoint = "2026", UnicodeGroup = "General Punctuation", UnicodeName = "Horizontal Ellipsis",
                    Character = "…", Windows1252UrlEncoding = "%85", Utf8UrlEncoding = "%E2%80%A6",
                    HtmlEntityName = "&hellip;", XmlEntityNumber = "&#8230;"
                },
                new()
                {
                    UnicodePoint = "2020", UnicodeGroup = "General Punctuation", UnicodeName = "Dagger",
                    Character = "†", Windows1252UrlEncoding = "%86", Utf8UrlEncoding = "%E2%80%A0",
                    HtmlEntityName = "&dagger;", XmlEntityNumber = "&#8224;"
                },
                new()
                {
                    UnicodePoint = "2021", UnicodeGroup = "General Punctuation", UnicodeName = "Double Dagger",
                    Character = "‡", Windows1252UrlEncoding = "%87", Utf8UrlEncoding = "%E2%80%A1",
                    HtmlEntityName = "&Dagger;", XmlEntityNumber = "&#8225;"
                },
                new()
                {
                    UnicodePoint = "2c6", UnicodeGroup = "Spacing Modifier Letters",
                    UnicodeName = "Modifier Letter Circumflex Accent", Character = "ˆ", Windows1252UrlEncoding = "%88",
                    Utf8UrlEncoding = "%CB%86", HtmlEntityName = "&circ;", XmlEntityNumber = "&#710;"
                },
                new()
                {
                    UnicodePoint = "2030", UnicodeGroup = "General Punctuation", UnicodeName = "Per Mille Sign",
                    Character = "‰", Windows1252UrlEncoding = "%89", Utf8UrlEncoding = "%E2%80%B0",
                    HtmlEntityName = "&permil;", XmlEntityNumber = "&#8240;"
                },
                new()
                {
                    UnicodePoint = "160", UnicodeGroup = "Latin Extended-A",
                    UnicodeName = "Latin Capital Letter S With Caron", Character = "Š", Windows1252UrlEncoding = "%8A",
                    Utf8UrlEncoding = "%C5%A0", HtmlEntityName = "&Scaron;", XmlEntityNumber = "&#352;"
                },
                new()
                {
                    UnicodePoint = "2039", UnicodeGroup = "General Punctuation",
                    UnicodeName = "Single Left-Pointing Angle Quotation Mark", Character = "‹",
                    Windows1252UrlEncoding = "%8B", Utf8UrlEncoding = "%E2%80%B9", HtmlEntityName = "&lsaquo;",
                    XmlEntityNumber = "&#8249;"
                },
                new()
                {
                    UnicodePoint = "152", UnicodeGroup = "Latin Extended-A", UnicodeName = "Latin Capital Ligature Oe",
                    Character = "Œ", Windows1252UrlEncoding = "%8C", Utf8UrlEncoding = "%C5%92",
                    HtmlEntityName = "&OElig;", XmlEntityNumber = "&#338;"
                },
                new()
                {
                    UnicodePoint = "8d", UnicodeGroup = "Latin-1 Supplement", UnicodeName = "Reverse Line Feed",
                    Character = "", Windows1252UrlEncoding = "%8D", Utf8UrlEncoding = "%C5%8D", HtmlEntityName = "",
                    XmlEntityNumber = "&#141;"
                },
                new()
                {
                    UnicodePoint = "17d", UnicodeGroup = "Latin Extended-A",
                    UnicodeName = "Latin Capital Letter Z With Caron", Character = "Ž", Windows1252UrlEncoding = "%8E",
                    Utf8UrlEncoding = "%C5%BD", HtmlEntityName = "", XmlEntityNumber = "&#381;"
                },
                new()
                {
                    UnicodePoint = "8f", UnicodeGroup = "Latin-1 Supplement", UnicodeName = "Single Shift Three",
                    Character = "", Windows1252UrlEncoding = "%8F", Utf8UrlEncoding = "%8F", HtmlEntityName = "",
                    XmlEntityNumber = "&#143;"
                },
                new()
                {
                    UnicodePoint = "90", UnicodeGroup = "Latin-1 Supplement", UnicodeName = "Device Control String",
                    Character = "", Windows1252UrlEncoding = "%90", Utf8UrlEncoding = "%C2%90", HtmlEntityName = "",
                    XmlEntityNumber = "&#144;"
                },
                new()
                {
                    UnicodePoint = "2018", UnicodeGroup = "General Punctuation",
                    UnicodeName = "Left Single Quotation Mark", Character = "‘", Windows1252UrlEncoding = "%91",
                    Utf8UrlEncoding = "%E2%80%98", HtmlEntityName = "&lsquo;", XmlEntityNumber = "&#8216;"
                },
                new()
                {
                    UnicodePoint = "2019", UnicodeGroup = "General Punctuation",
                    UnicodeName = "Right Single Quotation Mark", Character = "’", Windows1252UrlEncoding = "%92",
                    Utf8UrlEncoding = "%E2%80%99", HtmlEntityName = "&rsquo;", XmlEntityNumber = "&#8217;"
                },
                new()
                {
                    UnicodePoint = "201c", UnicodeGroup = "General Punctuation",
                    UnicodeName = "Left Double Quotation Mark", Character = "“", Windows1252UrlEncoding = "%93",
                    Utf8UrlEncoding = "%E2%80%9C", HtmlEntityName = "&ldquo;", XmlEntityNumber = "&#8220;"
                },
                new()
                {
                    UnicodePoint = "201d", UnicodeGroup = "General Punctuation",
                    UnicodeName = "Right Double Quotation Mark", Character = "”", Windows1252UrlEncoding = "%94",
                    Utf8UrlEncoding = "%E2%80%9D", HtmlEntityName = "&rdquo;", XmlEntityNumber = "&#8221;"
                },
                new()
                {
                    UnicodePoint = "2022", UnicodeGroup = "General Punctuation", UnicodeName = "Bullet",
                    Character = "•", Windows1252UrlEncoding = "%95", Utf8UrlEncoding = "%E2%80%A2",
                    HtmlEntityName = "&bull;", XmlEntityNumber = "&#8226;"
                },
                new()
                {
                    UnicodePoint = "2013", UnicodeGroup = "General Punctuation", UnicodeName = "En Dash",
                    Character = "–", Windows1252UrlEncoding = "%96", Utf8UrlEncoding = "%E2%80%93",
                    HtmlEntityName = "&ndash;", XmlEntityNumber = "&#8211;"
                },
                new()
                {
                    UnicodePoint = "2014", UnicodeGroup = "General Punctuation", UnicodeName = "Em Dash",
                    Character = "—", Windows1252UrlEncoding = "%97", Utf8UrlEncoding = "%E2%80%94",
                    HtmlEntityName = "&mdash;", XmlEntityNumber = "&#8212;"
                },
                new()
                {
                    UnicodePoint = "2dc", UnicodeGroup = "Spacing Modifier Letters", UnicodeName = "Small Tilde",
                    Character = "˜", Windows1252UrlEncoding = "%98", Utf8UrlEncoding = "%CB%9C",
                    HtmlEntityName = "&tilde;", XmlEntityNumber = "&#732;"
                },
                new()
                {
                    UnicodePoint = "2122", UnicodeGroup = "Letterlike Symbols", UnicodeName = "Trade Mark Sign",
                    Character = "™", Windows1252UrlEncoding = "%99", Utf8UrlEncoding = "%E2%84",
                    HtmlEntityName = "&trade;", XmlEntityNumber = "&#8482;"
                },
                new()
                {
                    UnicodePoint = "161", UnicodeGroup = "Latin Extended-A",
                    UnicodeName = "Latin Small Letter S With Caron", Character = "š", Windows1252UrlEncoding = "%9A",
                    Utf8UrlEncoding = "%C5%A1", HtmlEntityName = "&scaron;", XmlEntityNumber = "&#353;"
                },
                new()
                {
                    UnicodePoint = "203a", UnicodeGroup = "General Punctuation",
                    UnicodeName = "Single Right-Pointing Angle Quotation Mark", Character = "›",
                    Windows1252UrlEncoding = "%9B", Utf8UrlEncoding = "%E2%80", HtmlEntityName = "&rsaquo;",
                    XmlEntityNumber = "&#8250;"
                },
                new()
                {
                    UnicodePoint = "153", UnicodeGroup = "Latin Extended-A", UnicodeName = "Latin Small Ligature Oe",
                    Character = "œ", Windows1252UrlEncoding = "%9C", Utf8UrlEncoding = "%C5%93",
                    HtmlEntityName = "&oelig;", XmlEntityNumber = "&#339;"
                },
                new()
                {
                    UnicodePoint = "9d", UnicodeGroup = "Latin-1 Supplement", UnicodeName = "Operating System Command",
                    Character = "", Windows1252UrlEncoding = "%9D", Utf8UrlEncoding = "%9D", HtmlEntityName = "",
                    XmlEntityNumber = "&#157;"
                },
                new()
                {
                    UnicodePoint = "17e", UnicodeGroup = "Latin Extended-A",
                    UnicodeName = "Latin Small Letter Z With Caron", Character = "ž", Windows1252UrlEncoding = "%9E",
                    Utf8UrlEncoding = "%C5%BE", HtmlEntityName = "", XmlEntityNumber = "&#382;"
                },
                new()
                {
                    UnicodePoint = "178", UnicodeGroup = "Latin Extended-A",
                    UnicodeName = "Latin Capital Letter Y With Diaeresis", Character = "Ÿ",
                    Windows1252UrlEncoding = "%9F", Utf8UrlEncoding = "%C5%B8", HtmlEntityName = "&Yuml;",
                    XmlEntityNumber = "&#376;"
                },
                new()
                {
                    UnicodePoint = "a0", UnicodeGroup = "Latin-1 Supplement", UnicodeName = "No-Break Space",
                    Character = "", Windows1252UrlEncoding = "%A0", Utf8UrlEncoding = "%C2%A0",
                    HtmlEntityName = "&nbsp;", XmlEntityNumber = "&#160;"
                },
                new()
                {
                    UnicodePoint = "a1", UnicodeGroup = "Latin-1 Supplement", UnicodeName = "Inverted Exclamation Mark",
                    Character = "¡", Windows1252UrlEncoding = "%A1", Utf8UrlEncoding = "%C2%A1",
                    HtmlEntityName = "&iexcl;", XmlEntityNumber = "&#161;"
                },
                new()
                {
                    UnicodePoint = "a2", UnicodeGroup = "Latin-1 Supplement", UnicodeName = "Cent Sign",
                    Character = "¢", Windows1252UrlEncoding = "%A2", Utf8UrlEncoding = "%C2%A2",
                    HtmlEntityName = "&cent;", XmlEntityNumber = "&#162;"
                },
                new()
                {
                    UnicodePoint = "a3", UnicodeGroup = "Latin-1 Supplement", UnicodeName = "Pound Sign",
                    Character = "£", Windows1252UrlEncoding = "%A3", Utf8UrlEncoding = "%C2%A3",
                    HtmlEntityName = "&pound;", XmlEntityNumber = "&#163;"
                },
                new()
                {
                    UnicodePoint = "a4", UnicodeGroup = "Latin-1 Supplement", UnicodeName = "Currency Sign",
                    Character = "¤", Windows1252UrlEncoding = "%A4", Utf8UrlEncoding = "%C2%A4",
                    HtmlEntityName = "&curren;", XmlEntityNumber = "&#164;"
                },
                new()
                {
                    UnicodePoint = "a5", UnicodeGroup = "Latin-1 Supplement", UnicodeName = "Yen Sign", Character = "¥",
                    Windows1252UrlEncoding = "%A5", Utf8UrlEncoding = "%C2%A5", HtmlEntityName = "&yen;",
                    XmlEntityNumber = "&#165;"
                },
                new()
                {
                    UnicodePoint = "a6", UnicodeGroup = "Latin-1 Supplement", UnicodeName = "Broken Bar",
                    Character = "¦", Windows1252UrlEncoding = "%A6", Utf8UrlEncoding = "%C2%A6",
                    HtmlEntityName = "&brvbar;", XmlEntityNumber = "&#166;"
                },
                new()
                {
                    UnicodePoint = "a7", UnicodeGroup = "Latin-1 Supplement", UnicodeName = "Section Sign",
                    Character = "§", Windows1252UrlEncoding = "%A7", Utf8UrlEncoding = "%C2%A7",
                    HtmlEntityName = "&sect;", XmlEntityNumber = "&#167;"
                },
                new()
                {
                    UnicodePoint = "a8", UnicodeGroup = "Latin-1 Supplement", UnicodeName = "Diaeresis",
                    Character = "¨", Windows1252UrlEncoding = "%A8", Utf8UrlEncoding = "%C2%A8",
                    HtmlEntityName = "&uml;", XmlEntityNumber = "&#168;"
                },
                new()
                {
                    UnicodePoint = "a9", UnicodeGroup = "Latin-1 Supplement", UnicodeName = "Copyright Sign",
                    Character = "©", Windows1252UrlEncoding = "%A9", Utf8UrlEncoding = "%C2%A9",
                    HtmlEntityName = "&copy;", XmlEntityNumber = "&#169;"
                },
                new()
                {
                    UnicodePoint = "aa", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Feminine Ordinal Indicator", Character = "ª", Windows1252UrlEncoding = "%AA",
                    Utf8UrlEncoding = "%C2%AA", HtmlEntityName = "&ordf;", XmlEntityNumber = "&#170;"
                },
                new()
                {
                    UnicodePoint = "ab", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Left-Pointing Double Angle Quotation Mark", Character = "«",
                    Windows1252UrlEncoding = "%AB", Utf8UrlEncoding = "%C2%AB", HtmlEntityName = "&laquo;",
                    XmlEntityNumber = "&#171;"
                },
                new()
                {
                    UnicodePoint = "ac", UnicodeGroup = "Latin-1 Supplement", UnicodeName = "Not Sign", Character = "¬",
                    Windows1252UrlEncoding = "%AC", Utf8UrlEncoding = "%C2%AC", HtmlEntityName = "&not;",
                    XmlEntityNumber = "&#172;"
                },
                new()
                {
                    UnicodePoint = "ad", UnicodeGroup = "Latin-1 Supplement", UnicodeName = "Soft Hyphen",
                    Character = "­", Windows1252UrlEncoding = "%AD", Utf8UrlEncoding = "%C2%AD",
                    HtmlEntityName = "&shy;", XmlEntityNumber = "&#173;"
                },
                new()
                {
                    UnicodePoint = "ae", UnicodeGroup = "Latin-1 Supplement", UnicodeName = "Registered Sign",
                    Character = "®", Windows1252UrlEncoding = "%AE", Utf8UrlEncoding = "%C2%AE",
                    HtmlEntityName = "&reg;", XmlEntityNumber = "&#174;"
                },
                new()
                {
                    UnicodePoint = "af", UnicodeGroup = "Latin-1 Supplement", UnicodeName = "Macron", Character = "¯",
                    Windows1252UrlEncoding = "%AF", Utf8UrlEncoding = "%C2%AF", HtmlEntityName = "&macr;",
                    XmlEntityNumber = "&#175;"
                },
                new()
                {
                    UnicodePoint = "b0", UnicodeGroup = "Latin-1 Supplement", UnicodeName = "Degree Sign",
                    Character = "°", Windows1252UrlEncoding = "%B0", Utf8UrlEncoding = "%C2%B0",
                    HtmlEntityName = "&deg;", XmlEntityNumber = "&#176;"
                },
                new()
                {
                    UnicodePoint = "b1", UnicodeGroup = "Latin-1 Supplement", UnicodeName = "Plus-Minus Sign",
                    Character = "±", Windows1252UrlEncoding = "%B1", Utf8UrlEncoding = "%C2%B1",
                    HtmlEntityName = "&plusmn;", XmlEntityNumber = "&#177;"
                },
                new()
                {
                    UnicodePoint = "b2", UnicodeGroup = "Latin-1 Supplement", UnicodeName = "Superscript Two",
                    Character = "²", Windows1252UrlEncoding = "%B2", Utf8UrlEncoding = "%C2%B2",
                    HtmlEntityName = "&sup2;", XmlEntityNumber = "&#178;"
                },
                new()
                {
                    UnicodePoint = "b3", UnicodeGroup = "Latin-1 Supplement", UnicodeName = "Superscript Three",
                    Character = "³", Windows1252UrlEncoding = "%B3", Utf8UrlEncoding = "%C2%B3",
                    HtmlEntityName = "&sup3;", XmlEntityNumber = "&#179;"
                },
                new()
                {
                    UnicodePoint = "b4", UnicodeGroup = "Latin-1 Supplement", UnicodeName = "Acute Accent",
                    Character = "´", Windows1252UrlEncoding = "%B4", Utf8UrlEncoding = "%C2%B4",
                    HtmlEntityName = "&acute;", XmlEntityNumber = "&#180;"
                },
                new()
                {
                    UnicodePoint = "b5", UnicodeGroup = "Latin-1 Supplement", UnicodeName = "Micro Sign",
                    Character = "µ", Windows1252UrlEncoding = "%B5", Utf8UrlEncoding = "%C2%B5",
                    HtmlEntityName = "&micro;", XmlEntityNumber = "&#181;"
                },
                new()
                {
                    UnicodePoint = "b6", UnicodeGroup = "Latin-1 Supplement", UnicodeName = "Pilcrow Sign",
                    Character = "¶", Windows1252UrlEncoding = "%B6", Utf8UrlEncoding = "%C2%B6",
                    HtmlEntityName = "&para;", XmlEntityNumber = "&#182;"
                },
                new()
                {
                    UnicodePoint = "b7", UnicodeGroup = "Latin-1 Supplement", UnicodeName = "Middle Dot",
                    Character = "·", Windows1252UrlEncoding = "%B7", Utf8UrlEncoding = "%C2%B7",
                    HtmlEntityName = "&middot;", XmlEntityNumber = "&#183;"
                },
                new()
                {
                    UnicodePoint = "b8", UnicodeGroup = "Latin-1 Supplement", UnicodeName = "Cedilla", Character = "¸",
                    Windows1252UrlEncoding = "%B8", Utf8UrlEncoding = "%C2%B8", HtmlEntityName = "&cedil;",
                    XmlEntityNumber = "&#184;"
                },
                new()
                {
                    UnicodePoint = "b9", UnicodeGroup = "Latin-1 Supplement", UnicodeName = "Superscript One",
                    Character = "¹", Windows1252UrlEncoding = "%B9", Utf8UrlEncoding = "%C2%B9",
                    HtmlEntityName = "&sup1;", XmlEntityNumber = "&#185;"
                },
                new()
                {
                    UnicodePoint = "ba", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Masculine Ordinal Indicator", Character = "º", Windows1252UrlEncoding = "%BA",
                    Utf8UrlEncoding = "%C2%BA", HtmlEntityName = "&ordm;", XmlEntityNumber = "&#186;"
                },
                new()
                {
                    UnicodePoint = "bb", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Right-Pointing Double Angle Quotation Mark", Character = "»",
                    Windows1252UrlEncoding = "%BB", Utf8UrlEncoding = "%C2%BB", HtmlEntityName = "&raquo;",
                    XmlEntityNumber = "&#187;"
                },
                new()
                {
                    UnicodePoint = "bc", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Vulgar Fraction One Quarter", Character = "¼", Windows1252UrlEncoding = "%BC",
                    Utf8UrlEncoding = "%C2%BC", HtmlEntityName = "&frac14;", XmlEntityNumber = "&#188;"
                },
                new()
                {
                    UnicodePoint = "bd", UnicodeGroup = "Latin-1 Supplement", UnicodeName = "Vulgar Fraction One Half",
                    Character = "½", Windows1252UrlEncoding = "%BD", Utf8UrlEncoding = "%C2%BD",
                    HtmlEntityName = "&frac12;", XmlEntityNumber = "&#189;"
                },
                new()
                {
                    UnicodePoint = "be", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Vulgar Fraction Three Quarters", Character = "¾", Windows1252UrlEncoding = "%BE",
                    Utf8UrlEncoding = "%C2%BE", HtmlEntityName = "&frac34;", XmlEntityNumber = "&#190;"
                },
                new()
                {
                    UnicodePoint = "bf", UnicodeGroup = "Latin-1 Supplement", UnicodeName = "Inverted Question Mark",
                    Character = "¿", Windows1252UrlEncoding = "%BF", Utf8UrlEncoding = "%C2%BF",
                    HtmlEntityName = "&iquest;", XmlEntityNumber = "&#191;"
                },
                new()
                {
                    UnicodePoint = "c0", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Capital Letter A With Grave", Character = "À", Windows1252UrlEncoding = "%C0",
                    Utf8UrlEncoding = "%C3%80", HtmlEntityName = "&Agrave;", XmlEntityNumber = "&#192;"
                },
                new()
                {
                    UnicodePoint = "c1", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Capital Letter A With Acute", Character = "Á", Windows1252UrlEncoding = "%C1",
                    Utf8UrlEncoding = "%C3%81", HtmlEntityName = "&Aacute;", XmlEntityNumber = "&#193;"
                },
                new()
                {
                    UnicodePoint = "c2", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Capital Letter A With Circumflex", Character = "Â",
                    Windows1252UrlEncoding = "%C2", Utf8UrlEncoding = "%C3%82", HtmlEntityName = "&Acirc;",
                    XmlEntityNumber = "&#194;"
                },
                new()
                {
                    UnicodePoint = "c3", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Capital Letter A With Tilde", Character = "Ã", Windows1252UrlEncoding = "%C3",
                    Utf8UrlEncoding = "%C3%83", HtmlEntityName = "&Atilde;", XmlEntityNumber = "&#195;"
                },
                new()
                {
                    UnicodePoint = "c4", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Capital Letter A With Diaeresis", Character = "Ä",
                    Windows1252UrlEncoding = "%C4", Utf8UrlEncoding = "%C3%84", HtmlEntityName = "&Auml;",
                    XmlEntityNumber = "&#196;"
                },
                new()
                {
                    UnicodePoint = "c5", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Capital Letter A With Ring Above", Character = "Å",
                    Windows1252UrlEncoding = "%C5", Utf8UrlEncoding = "%C3%85", HtmlEntityName = "&Aring;",
                    XmlEntityNumber = "&#197;"
                },
                new()
                {
                    UnicodePoint = "c6", UnicodeGroup = "Latin-1 Supplement", UnicodeName = "Latin Capital Letter Ae",
                    Character = "Æ", Windows1252UrlEncoding = "%C6", Utf8UrlEncoding = "%C3%86",
                    HtmlEntityName = "&AElig;", XmlEntityNumber = "&#198;"
                },
                new()
                {
                    UnicodePoint = "c7", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Capital Letter C With Cedilla", Character = "Ç",
                    Windows1252UrlEncoding = "%C7", Utf8UrlEncoding = "%C3%87", HtmlEntityName = "&Ccedil;",
                    XmlEntityNumber = "&#199;"
                },
                new()
                {
                    UnicodePoint = "c8", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Capital Letter E With Grave", Character = "È", Windows1252UrlEncoding = "%C8",
                    Utf8UrlEncoding = "%C3%88", HtmlEntityName = "&Egrave;", XmlEntityNumber = "&#200;"
                },
                new()
                {
                    UnicodePoint = "c9", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Capital Letter E With Acute", Character = "É", Windows1252UrlEncoding = "%C9",
                    Utf8UrlEncoding = "%C3%89", HtmlEntityName = "&Eacute;", XmlEntityNumber = "&#201;"
                },
                new()
                {
                    UnicodePoint = "ca", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Capital Letter E With Circumflex", Character = "Ê",
                    Windows1252UrlEncoding = "%CA", Utf8UrlEncoding = "%C3%8A", HtmlEntityName = "&Ecirc;",
                    XmlEntityNumber = "&#202;"
                },
                new()
                {
                    UnicodePoint = "cb", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Capital Letter E With Diaeresis", Character = "Ë",
                    Windows1252UrlEncoding = "%CB", Utf8UrlEncoding = "%C3%8B", HtmlEntityName = "&Euml;",
                    XmlEntityNumber = "&#203;"
                },
                new()
                {
                    UnicodePoint = "cc", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Capital Letter I With Grave", Character = "Ì", Windows1252UrlEncoding = "%CC",
                    Utf8UrlEncoding = "%C3%8C", HtmlEntityName = "&Igrave;", XmlEntityNumber = "&#204;"
                },
                new()
                {
                    UnicodePoint = "cd", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Capital Letter I With Acute", Character = "Í", Windows1252UrlEncoding = "%CD",
                    Utf8UrlEncoding = "%C3%8D", HtmlEntityName = "&Iacute;", XmlEntityNumber = "&#205;"
                },
                new()
                {
                    UnicodePoint = "ce", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Capital Letter I With Circumflex", Character = "Î",
                    Windows1252UrlEncoding = "%CE", Utf8UrlEncoding = "%C3%8E", HtmlEntityName = "&Icirc;",
                    XmlEntityNumber = "&#206;"
                },
                new()
                {
                    UnicodePoint = "cf", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Capital Letter I With Diaeresis", Character = "Ï",
                    Windows1252UrlEncoding = "%CF", Utf8UrlEncoding = "%C3%8F", HtmlEntityName = "&Iuml;",
                    XmlEntityNumber = "&#207;"
                },
                new()
                {
                    UnicodePoint = "d0", UnicodeGroup = "Latin-1 Supplement", UnicodeName = "Latin Capital Letter Eth",
                    Character = "Ð", Windows1252UrlEncoding = "%D0", Utf8UrlEncoding = "%C3%90",
                    HtmlEntityName = "&ETH;", XmlEntityNumber = "&#208;"
                },
                new()
                {
                    UnicodePoint = "d1", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Capital Letter N With Tilde", Character = "Ñ", Windows1252UrlEncoding = "%D1",
                    Utf8UrlEncoding = "%C3%91", HtmlEntityName = "&Ntilde;", XmlEntityNumber = "&#209;"
                },
                new()
                {
                    UnicodePoint = "d2", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Capital Letter O With Grave", Character = "Ò", Windows1252UrlEncoding = "%D2",
                    Utf8UrlEncoding = "%C3%92", HtmlEntityName = "&Ograve;", XmlEntityNumber = "&#210;"
                },
                new()
                {
                    UnicodePoint = "d3", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Capital Letter O With Acute", Character = "Ó", Windows1252UrlEncoding = "%D3",
                    Utf8UrlEncoding = "%C3%93", HtmlEntityName = "&Oacute;", XmlEntityNumber = "&#211;"
                },
                new()
                {
                    UnicodePoint = "d4", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Capital Letter O With Circumflex", Character = "Ô",
                    Windows1252UrlEncoding = "%D4", Utf8UrlEncoding = "%C3%94", HtmlEntityName = "&Ocirc;",
                    XmlEntityNumber = "&#212;"
                },
                new()
                {
                    UnicodePoint = "d5", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Capital Letter O With Tilde", Character = "Õ", Windows1252UrlEncoding = "%D5",
                    Utf8UrlEncoding = "%C3%95", HtmlEntityName = "&Otilde;", XmlEntityNumber = "&#213;"
                },
                new()
                {
                    UnicodePoint = "d6", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Capital Letter O With Diaeresis", Character = "Ö",
                    Windows1252UrlEncoding = "%D6", Utf8UrlEncoding = "%C3%96", HtmlEntityName = "&Ouml;",
                    XmlEntityNumber = "&#214;"
                },
                new()
                {
                    UnicodePoint = "d7", UnicodeGroup = "Latin-1 Supplement", UnicodeName = "Multiplication Sign",
                    Character = "×", Windows1252UrlEncoding = "%D7", Utf8UrlEncoding = "%C3%97",
                    HtmlEntityName = "&times;", XmlEntityNumber = "&#215;"
                },
                new()
                {
                    UnicodePoint = "d8", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Capital Letter O With Stroke", Character = "Ø", Windows1252UrlEncoding = "%D8",
                    Utf8UrlEncoding = "%C3%98", HtmlEntityName = "&Oslash;", XmlEntityNumber = "&#216;"
                },
                new()
                {
                    UnicodePoint = "d9", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Capital Letter U With Grave", Character = "Ù", Windows1252UrlEncoding = "%D9",
                    Utf8UrlEncoding = "%C3%99", HtmlEntityName = "&Ugrave;", XmlEntityNumber = "&#217;"
                },
                new()
                {
                    UnicodePoint = "da", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Capital Letter U With Acute", Character = "Ú", Windows1252UrlEncoding = "%DA",
                    Utf8UrlEncoding = "%C3%9A", HtmlEntityName = "&Uacute;", XmlEntityNumber = "&#218;"
                },
                new()
                {
                    UnicodePoint = "db", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Capital Letter U With Circumflex", Character = "Û",
                    Windows1252UrlEncoding = "%DB", Utf8UrlEncoding = "%C3%9B", HtmlEntityName = "&Ucirc;",
                    XmlEntityNumber = "&#219;"
                },
                new()
                {
                    UnicodePoint = "dc", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Capital Letter U With Diaeresis", Character = "Ü",
                    Windows1252UrlEncoding = "%DC", Utf8UrlEncoding = "%C3%9C", HtmlEntityName = "&Uuml;",
                    XmlEntityNumber = "&#220;"
                },
                new()
                {
                    UnicodePoint = "dd", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Capital Letter Y With Acute", Character = "Ý", Windows1252UrlEncoding = "%DD",
                    Utf8UrlEncoding = "%C3%9D", HtmlEntityName = "&Yacute;", XmlEntityNumber = "&#221;"
                },
                new()
                {
                    UnicodePoint = "de", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Capital Letter Thorn", Character = "Þ", Windows1252UrlEncoding = "%DE",
                    Utf8UrlEncoding = "%C3%9E", HtmlEntityName = "&THORN;", XmlEntityNumber = "&#222;"
                },
                new()
                {
                    UnicodePoint = "df", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Small Letter Sharp S", Character = "ß", Windows1252UrlEncoding = "%DF",
                    Utf8UrlEncoding = "%C3%9F", HtmlEntityName = "&szlig;", XmlEntityNumber = "&#223;"
                },
                new()
                {
                    UnicodePoint = "e0", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Small Letter A With Grave", Character = "à", Windows1252UrlEncoding = "%E0",
                    Utf8UrlEncoding = "%C3%A0", HtmlEntityName = "&agrave;", XmlEntityNumber = "&#224;"
                },
                new()
                {
                    UnicodePoint = "e1", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Small Letter A With Acute", Character = "á", Windows1252UrlEncoding = "%E1",
                    Utf8UrlEncoding = "%C3%A1", HtmlEntityName = "&aacute;", XmlEntityNumber = "&#225;"
                },
                new()
                {
                    UnicodePoint = "e2", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Small Letter A With Circumflex", Character = "â",
                    Windows1252UrlEncoding = "%E2", Utf8UrlEncoding = "%C3%A2", HtmlEntityName = "&acirc;",
                    XmlEntityNumber = "&#226;"
                },
                new()
                {
                    UnicodePoint = "e3", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Small Letter A With Tilde", Character = "ã", Windows1252UrlEncoding = "%E3",
                    Utf8UrlEncoding = "%C3%A3", HtmlEntityName = "&atilde;", XmlEntityNumber = "&#227;"
                },
                new()
                {
                    UnicodePoint = "e4", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Small Letter A With Diaeresis", Character = "ä",
                    Windows1252UrlEncoding = "%E4", Utf8UrlEncoding = "%C3%A4", HtmlEntityName = "&auml;",
                    XmlEntityNumber = "&#228;"
                },
                new()
                {
                    UnicodePoint = "e5", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Small Letter A With Ring Above", Character = "å",
                    Windows1252UrlEncoding = "%E5", Utf8UrlEncoding = "%C3%A5", HtmlEntityName = "&aring;",
                    XmlEntityNumber = "&#229;"
                },
                new()
                {
                    UnicodePoint = "e6", UnicodeGroup = "Latin-1 Supplement", UnicodeName = "Latin Small Letter Ae",
                    Character = "æ", Windows1252UrlEncoding = "%E6", Utf8UrlEncoding = "%C3%A6",
                    HtmlEntityName = "&aelig;", XmlEntityNumber = "&#230;"
                },
                new()
                {
                    UnicodePoint = "e7", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Small Letter C With Cedilla", Character = "ç", Windows1252UrlEncoding = "%E7",
                    Utf8UrlEncoding = "%C3%A7", HtmlEntityName = "&ccedil;", XmlEntityNumber = "&#231;"
                },
                new()
                {
                    UnicodePoint = "e8", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Small Letter E With Grave", Character = "è", Windows1252UrlEncoding = "%E8",
                    Utf8UrlEncoding = "%C3%A8", HtmlEntityName = "&egrave;", XmlEntityNumber = "&#232;"
                },
                new()
                {
                    UnicodePoint = "e9", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Small Letter E With Acute", Character = "é", Windows1252UrlEncoding = "%E9",
                    Utf8UrlEncoding = "%C3%A9", HtmlEntityName = "&eacute;", XmlEntityNumber = "&#233;"
                },
                new()
                {
                    UnicodePoint = "ea", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Small Letter E With Circumflex", Character = "ê",
                    Windows1252UrlEncoding = "%EA", Utf8UrlEncoding = "%C3%AA", HtmlEntityName = "&ecirc;",
                    XmlEntityNumber = "&#234;"
                },
                new()
                {
                    UnicodePoint = "eb", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Small Letter E With Diaeresis", Character = "ë",
                    Windows1252UrlEncoding = "%EB", Utf8UrlEncoding = "%C3%AB", HtmlEntityName = "&euml;",
                    XmlEntityNumber = "&#235;"
                },
                new()
                {
                    UnicodePoint = "ec", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Small Letter I With Grave", Character = "ì", Windows1252UrlEncoding = "%EC",
                    Utf8UrlEncoding = "%C3%AC", HtmlEntityName = "&igrave;", XmlEntityNumber = "&#236;"
                },
                new()
                {
                    UnicodePoint = "ed", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Small Letter I With Acute", Character = "í", Windows1252UrlEncoding = "%ED",
                    Utf8UrlEncoding = "%C3%AD", HtmlEntityName = "&iacute;", XmlEntityNumber = "&#237;"
                },
                new()
                {
                    UnicodePoint = "ee", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Small Letter I With Circumflex", Character = "î",
                    Windows1252UrlEncoding = "%EE", Utf8UrlEncoding = "%C3%AE", HtmlEntityName = "&icirc;",
                    XmlEntityNumber = "&#238;"
                },
                new()
                {
                    UnicodePoint = "ef", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Small Letter I With Diaeresis", Character = "ï",
                    Windows1252UrlEncoding = "%EF", Utf8UrlEncoding = "%C3%AF", HtmlEntityName = "&iuml;",
                    XmlEntityNumber = "&#239;"
                },
                new()
                {
                    UnicodePoint = "f0", UnicodeGroup = "Latin-1 Supplement", UnicodeName = "Latin Small Letter Eth",
                    Character = "ð", Windows1252UrlEncoding = "%F0", Utf8UrlEncoding = "%C3%B0",
                    HtmlEntityName = "&eth;", XmlEntityNumber = "&#240;"
                },
                new()
                {
                    UnicodePoint = "f1", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Small Letter N With Tilde", Character = "ñ", Windows1252UrlEncoding = "%F1",
                    Utf8UrlEncoding = "%C3%B1", HtmlEntityName = "&ntilde;", XmlEntityNumber = "&#241;"
                },
                new()
                {
                    UnicodePoint = "f2", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Small Letter O With Grave", Character = "ò", Windows1252UrlEncoding = "%F2",
                    Utf8UrlEncoding = "%C3%B2", HtmlEntityName = "&ograve;", XmlEntityNumber = "&#242;"
                },
                new()
                {
                    UnicodePoint = "f3", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Small Letter O With Acute", Character = "ó", Windows1252UrlEncoding = "%F3",
                    Utf8UrlEncoding = "%C3%B3", HtmlEntityName = "&oacute;", XmlEntityNumber = "&#243;"
                },
                new()
                {
                    UnicodePoint = "f4", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Small Letter O With Circumflex", Character = "ô",
                    Windows1252UrlEncoding = "%F4", Utf8UrlEncoding = "%C3%B4", HtmlEntityName = "&ocirc;",
                    XmlEntityNumber = "&#244;"
                },
                new()
                {
                    UnicodePoint = "f5", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Small Letter O With Tilde", Character = "õ", Windows1252UrlEncoding = "%F5",
                    Utf8UrlEncoding = "%C3%B5", HtmlEntityName = "&otilde;", XmlEntityNumber = "&#245;"
                },
                new()
                {
                    UnicodePoint = "f6", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Small Letter O With Diaeresis", Character = "ö",
                    Windows1252UrlEncoding = "%F6", Utf8UrlEncoding = "%C3%B6", HtmlEntityName = "&ouml;",
                    XmlEntityNumber = "&#246;"
                },
                new()
                {
                    UnicodePoint = "f7", UnicodeGroup = "Latin-1 Supplement", UnicodeName = "Division Sign",
                    Character = "÷", Windows1252UrlEncoding = "%F7", Utf8UrlEncoding = "%C3%B7",
                    HtmlEntityName = "&divide;", XmlEntityNumber = "&#247;"
                },
                new()
                {
                    UnicodePoint = "f8", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Small Letter O With Stroke", Character = "ø", Windows1252UrlEncoding = "%F8",
                    Utf8UrlEncoding = "%C3%B8", HtmlEntityName = "&oslash;", XmlEntityNumber = "&#248;"
                },
                new()
                {
                    UnicodePoint = "f9", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Small Letter U With Grave", Character = "ù", Windows1252UrlEncoding = "%F9",
                    Utf8UrlEncoding = "%C3%B9", HtmlEntityName = "&ugrave;", XmlEntityNumber = "&#249;"
                },
                new()
                {
                    UnicodePoint = "fa", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Small Letter U With Acute", Character = "ú", Windows1252UrlEncoding = "%FA",
                    Utf8UrlEncoding = "%C3%BA", HtmlEntityName = "&uacute;", XmlEntityNumber = "&#250;"
                },
                new()
                {
                    UnicodePoint = "fb", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Small Letter U With Circumflex", Character = "û",
                    Windows1252UrlEncoding = "%FB", Utf8UrlEncoding = "%C3%BB", HtmlEntityName = "&ucirc;",
                    XmlEntityNumber = "&#251;"
                },
                new()
                {
                    UnicodePoint = "fc", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Small Letter U With Diaeresis", Character = "ü",
                    Windows1252UrlEncoding = "%FC", Utf8UrlEncoding = "%C3%BC", HtmlEntityName = "&uuml;",
                    XmlEntityNumber = "&#252;"
                },
                new()
                {
                    UnicodePoint = "fd", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Small Letter Y With Acute", Character = "ý", Windows1252UrlEncoding = "%FD",
                    Utf8UrlEncoding = "%C3%BD", HtmlEntityName = "&yacute;", XmlEntityNumber = "&#253;"
                },
                new()
                {
                    UnicodePoint = "fe", UnicodeGroup = "Latin-1 Supplement", UnicodeName = "Latin Small Letter Thorn",
                    Character = "þ", Windows1252UrlEncoding = "%FE", Utf8UrlEncoding = "%C3%BE",
                    HtmlEntityName = "&thorn;", XmlEntityNumber = "&#254;"
                },
                new()
                {
                    UnicodePoint = "ff", UnicodeGroup = "Latin-1 Supplement",
                    UnicodeName = "Latin Small Letter Y With Diaeresis", Character = "ÿ",
                    Windows1252UrlEncoding = "%FF", Utf8UrlEncoding = "%C3%BF", HtmlEntityName = "&yuml;",
                    XmlEntityNumber = "&#255;"
                },
                new()
                {
                    UnicodePoint = "20ac", UnicodeGroup = "Currency Symbols", UnicodeName = "Euro Sign",
                    Character = "€", Windows1252UrlEncoding = "", Utf8UrlEncoding = "%E2%82%AC",
                    HtmlEntityName = "&euro;", XmlEntityNumber = "&#8364;"
                },
                new()
                {
                    UnicodePoint = "20a3", UnicodeGroup = "Currency Symbols", UnicodeName = "French Franc Sign",
                    Character = "₣", Windows1252UrlEncoding = "", Utf8UrlEncoding = "%E2%82%A3", HtmlEntityName = "",
                    XmlEntityNumber = "&#8355;"
                },
                new()
                {
                    UnicodePoint = "20a4", UnicodeGroup = "Currency Symbols", UnicodeName = "Lira Sign",
                    Character = "₤", Windows1252UrlEncoding = "", Utf8UrlEncoding = "%E2%82%A4", HtmlEntityName = "",
                    XmlEntityNumber = "&#8356;"
                },
                new()
                {
                    UnicodePoint = "20b9", UnicodeGroup = "Currency Symbols", UnicodeName = "Indian Rupee Sign",
                    Character = "₹", Windows1252UrlEncoding = "", Utf8UrlEncoding = "%E2%82%B9", HtmlEntityName = "",
                    XmlEntityNumber = "&#8377;"
                },
            }
        );
}
