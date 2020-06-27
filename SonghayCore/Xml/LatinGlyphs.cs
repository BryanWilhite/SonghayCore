using Songhay.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Songhay.Xml
{
    /// <summary>
    /// Condenses and expands Latin glyphs.
    /// </summary>
    public static class LatinGlyphs
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
        public static string Condense(string entityText)
        {
            if (string.IsNullOrEmpty(entityText)) return entityText;

            foreach (var datum in GetGlyphsForCondenseOrExpand())
            {
                if (datum.HtmlEntityName?.Length > 0 && entityText.Contains(datum.HtmlEntityName))
                    entityText = Regex.Replace(entityText, Regex.Escape(datum.HtmlEntityName), datum.Character, RegexOptions.IgnoreCase);

                if (entityText.Contains(datum.XmlEntityNumber))
                    entityText = Regex.Replace(entityText, Regex.Escape(datum.XmlEntityNumber), datum.Character, RegexOptions.IgnoreCase);
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
        public static string Expand(string glyphText)
        {
            if (string.IsNullOrEmpty(glyphText)) return glyphText;

            foreach (var datum in GetGlyphsForCondenseOrExpand())
            {
                if (!glyphText.Contains(datum.Character)) continue;
                glyphText = glyphText.Replace(datum.Character, datum.XmlEntityNumber);
            }

            return glyphText;
        }

        /// <summary>
        /// Gets the glyphs.
        /// </summary>
        /// <returns></returns>
        public static FrameworkGlyph[] GetGlyphs() => glyphs.Value;

        /// <summary>
        /// Gets the glyphs for <see cref="Condense(string)"/> or <see cref="Expand(string)"/>.
        /// </summary>
        /// <returns></returns>
        public static FrameworkGlyph[] GetGlyphsForCondenseOrExpand()
        {

            var basicLatin = glyphs.Value
                .Where(g => g.Character?.Length > 0)
                .Where(g => !string.IsNullOrWhiteSpace(g.HtmlEntityName))
                .Where(g => g.UnicodeInteger < 128);

            var beyondBasicLatin = glyphs.Value
                .Where(g => g.Character?.Length > 0)
                .Where(g => g.UnicodeInteger > 127);

            return basicLatin.Union(beyondBasicLatin).ToArray();
        }

        /// <summary>
        /// Removes the URL encodings.
        /// </summary>
        /// <param name="input">The input.</param>
        public static string RemoveUrlEncodings(string input) => RemoveUrlEncodings(input, includeWindows1252UrlEncoding: false);

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
                    input = Regex.Replace(input, Regex.Escape(glyph.Utf8UrlEncoding), string.Empty, RegexOptions.IgnoreCase);

                input = Regex.Replace(input, Regex.Escape(glyph.Utf8UrlEncoding), string.Empty, RegexOptions.IgnoreCase);
            }

            return input;
        }

        private static readonly Lazy<FrameworkGlyph[]> glyphs =
            new Lazy<FrameworkGlyph[]>(() => new FrameworkGlyph[]
                    {
new FrameworkGlyph { UnicodePoint = "20", UnicodeGroup = "Basic Latin", Character = " ", Windows1252UrlEncoding = "%20", Utf8UrlEncoding = "%20", HtmlEntityName = "", XmlEntityNumber = "&#32;" },
new FrameworkGlyph { UnicodePoint = "21", UnicodeGroup = "Basic Latin", Character = "!", Windows1252UrlEncoding = "%21", Utf8UrlEncoding = "%21", HtmlEntityName = "", XmlEntityNumber = "&#33;" },
new FrameworkGlyph { UnicodePoint = "22", UnicodeGroup = "Basic Latin", Character = "\"", Windows1252UrlEncoding = "%22", Utf8UrlEncoding = "%22", HtmlEntityName = "&quot;", XmlEntityNumber = "&#34;" },
new FrameworkGlyph { UnicodePoint = "23", UnicodeGroup = "Basic Latin", Character = "#", Windows1252UrlEncoding = "%23", Utf8UrlEncoding = "%23", HtmlEntityName = "", XmlEntityNumber = "&#35;" },
new FrameworkGlyph { UnicodePoint = "24", UnicodeGroup = "Basic Latin", Character = "$", Windows1252UrlEncoding = "%24", Utf8UrlEncoding = "%24", HtmlEntityName = "", XmlEntityNumber = "&#36;" },
new FrameworkGlyph { UnicodePoint = "25", UnicodeGroup = "Basic Latin", Character = "%", Windows1252UrlEncoding = "%25", Utf8UrlEncoding = "%25", HtmlEntityName = "", XmlEntityNumber = "&#37;" },
new FrameworkGlyph { UnicodePoint = "26", UnicodeGroup = "Basic Latin", Character = "&", Windows1252UrlEncoding = "%26", Utf8UrlEncoding = "%26", HtmlEntityName = "&amp;", XmlEntityNumber = "&#38;" },
new FrameworkGlyph { UnicodePoint = "27", UnicodeGroup = "Basic Latin", Character = "'", Windows1252UrlEncoding = "%27", Utf8UrlEncoding = "%27", HtmlEntityName = "&apos;", XmlEntityNumber = "&#39;" },
new FrameworkGlyph { UnicodePoint = "28", UnicodeGroup = "Basic Latin", Character = "(", Windows1252UrlEncoding = "%28", Utf8UrlEncoding = "%28", HtmlEntityName = "", XmlEntityNumber = "&#40;" },
new FrameworkGlyph { UnicodePoint = "29", UnicodeGroup = "Basic Latin", Character = ")", Windows1252UrlEncoding = "%29", Utf8UrlEncoding = "%29", HtmlEntityName = "", XmlEntityNumber = "&#41;" },
new FrameworkGlyph { UnicodePoint = "2a", UnicodeGroup = "Basic Latin", Character = "*", Windows1252UrlEncoding = "%2A", Utf8UrlEncoding = "%2A", HtmlEntityName = "", XmlEntityNumber = "&#42;" },
new FrameworkGlyph { UnicodePoint = "2b", UnicodeGroup = "Basic Latin", Character = "+", Windows1252UrlEncoding = "%2B", Utf8UrlEncoding = "%2B", HtmlEntityName = "", XmlEntityNumber = "&#43;" },
new FrameworkGlyph { UnicodePoint = "2c", UnicodeGroup = "Basic Latin", Character = ",", Windows1252UrlEncoding = "%2C", Utf8UrlEncoding = "%2C", HtmlEntityName = "", XmlEntityNumber = "&#44;" },
new FrameworkGlyph { UnicodePoint = "2d", UnicodeGroup = "Basic Latin", Character = "-", Windows1252UrlEncoding = "%2D", Utf8UrlEncoding = "%2D", HtmlEntityName = "", XmlEntityNumber = "&#45;" },
new FrameworkGlyph { UnicodePoint = "2e", UnicodeGroup = "Basic Latin", Character = ".", Windows1252UrlEncoding = "%2E", Utf8UrlEncoding = "%2E", HtmlEntityName = "", XmlEntityNumber = "&#46;" },
new FrameworkGlyph { UnicodePoint = "2f", UnicodeGroup = "Basic Latin", Character = "/", Windows1252UrlEncoding = "%2F", Utf8UrlEncoding = "%2F", HtmlEntityName = "", XmlEntityNumber = "&#47;" },
new FrameworkGlyph { UnicodePoint = "30", UnicodeGroup = "Basic Latin", Character = "", Windows1252UrlEncoding = "%30", Utf8UrlEncoding = "%30", HtmlEntityName = "", XmlEntityNumber = "&#48;" },
new FrameworkGlyph { UnicodePoint = "31", UnicodeGroup = "Basic Latin", Character = "", Windows1252UrlEncoding = "%31", Utf8UrlEncoding = "%31", HtmlEntityName = "", XmlEntityNumber = "&#49;" },
new FrameworkGlyph { UnicodePoint = "32", UnicodeGroup = "Basic Latin", Character = "", Windows1252UrlEncoding = "%32", Utf8UrlEncoding = "%32", HtmlEntityName = "", XmlEntityNumber = "&#50;" },
new FrameworkGlyph { UnicodePoint = "33", UnicodeGroup = "Basic Latin", Character = "", Windows1252UrlEncoding = "%33", Utf8UrlEncoding = "%33", HtmlEntityName = "", XmlEntityNumber = "&#51;" },
new FrameworkGlyph { UnicodePoint = "34", UnicodeGroup = "Basic Latin", Character = "", Windows1252UrlEncoding = "%34", Utf8UrlEncoding = "%34", HtmlEntityName = "", XmlEntityNumber = "&#52;" },
new FrameworkGlyph { UnicodePoint = "35", UnicodeGroup = "Basic Latin", Character = "", Windows1252UrlEncoding = "%35", Utf8UrlEncoding = "%35", HtmlEntityName = "", XmlEntityNumber = "&#53;" },
new FrameworkGlyph { UnicodePoint = "36", UnicodeGroup = "Basic Latin", Character = "", Windows1252UrlEncoding = "%36", Utf8UrlEncoding = "%36", HtmlEntityName = "", XmlEntityNumber = "&#54;" },
new FrameworkGlyph { UnicodePoint = "37", UnicodeGroup = "Basic Latin", Character = "", Windows1252UrlEncoding = "%37", Utf8UrlEncoding = "%37", HtmlEntityName = "", XmlEntityNumber = "&#55;" },
new FrameworkGlyph { UnicodePoint = "38", UnicodeGroup = "Basic Latin", Character = "", Windows1252UrlEncoding = "%38", Utf8UrlEncoding = "%38", HtmlEntityName = "", XmlEntityNumber = "&#56;" },
new FrameworkGlyph { UnicodePoint = "39", UnicodeGroup = "Basic Latin", Character = "", Windows1252UrlEncoding = "%39", Utf8UrlEncoding = "%39", HtmlEntityName = "", XmlEntityNumber = "&#57;" },
new FrameworkGlyph { UnicodePoint = "3a", UnicodeGroup = "Basic Latin", Character = ":", Windows1252UrlEncoding = "%3A", Utf8UrlEncoding = "%3A", HtmlEntityName = "", XmlEntityNumber = "&#58;" },
new FrameworkGlyph { UnicodePoint = "3b", UnicodeGroup = "Basic Latin", Character = ";", Windows1252UrlEncoding = "%3B", Utf8UrlEncoding = "%3B", HtmlEntityName = "", XmlEntityNumber = "&#59;" },
new FrameworkGlyph { UnicodePoint = "3c", UnicodeGroup = "Basic Latin", Character = "<", Windows1252UrlEncoding = "%3C", Utf8UrlEncoding = "%3C", HtmlEntityName = "&lt;", XmlEntityNumber = "&#60;" },
new FrameworkGlyph { UnicodePoint = "3d", UnicodeGroup = "Basic Latin", Character = "=", Windows1252UrlEncoding = "%3D", Utf8UrlEncoding = "%3D", HtmlEntityName = "", XmlEntityNumber = "&#61;" },
new FrameworkGlyph { UnicodePoint = "3e", UnicodeGroup = "Basic Latin", Character = ">", Windows1252UrlEncoding = "%3E", Utf8UrlEncoding = "%3E", HtmlEntityName = "&gt;", XmlEntityNumber = "&#62;" },
new FrameworkGlyph { UnicodePoint = "3f", UnicodeGroup = "Basic Latin", Character = "?", Windows1252UrlEncoding = "%3F", Utf8UrlEncoding = "%3F", HtmlEntityName = "", XmlEntityNumber = "&#63;" },
new FrameworkGlyph { UnicodePoint = "40", UnicodeGroup = "Basic Latin", Character = "@", Windows1252UrlEncoding = "%40", Utf8UrlEncoding = "%40", HtmlEntityName = "", XmlEntityNumber = "&#64;" },
new FrameworkGlyph { UnicodePoint = "41", UnicodeGroup = "Basic Latin", Character = "A", Windows1252UrlEncoding = "%41", Utf8UrlEncoding = "%41", HtmlEntityName = "", XmlEntityNumber = "&#65;" },
new FrameworkGlyph { UnicodePoint = "42", UnicodeGroup = "Basic Latin", Character = "B", Windows1252UrlEncoding = "%42", Utf8UrlEncoding = "%42", HtmlEntityName = "", XmlEntityNumber = "&#66;" },
new FrameworkGlyph { UnicodePoint = "43", UnicodeGroup = "Basic Latin", Character = "C", Windows1252UrlEncoding = "%43", Utf8UrlEncoding = "%43", HtmlEntityName = "", XmlEntityNumber = "&#67;" },
new FrameworkGlyph { UnicodePoint = "44", UnicodeGroup = "Basic Latin", Character = "D", Windows1252UrlEncoding = "%44", Utf8UrlEncoding = "%44", HtmlEntityName = "", XmlEntityNumber = "&#68;" },
new FrameworkGlyph { UnicodePoint = "45", UnicodeGroup = "Basic Latin", Character = "E", Windows1252UrlEncoding = "%45", Utf8UrlEncoding = "%45", HtmlEntityName = "", XmlEntityNumber = "&#69;" },
new FrameworkGlyph { UnicodePoint = "46", UnicodeGroup = "Basic Latin", Character = "F", Windows1252UrlEncoding = "%46", Utf8UrlEncoding = "%46", HtmlEntityName = "", XmlEntityNumber = "&#70;" },
new FrameworkGlyph { UnicodePoint = "47", UnicodeGroup = "Basic Latin", Character = "G", Windows1252UrlEncoding = "%47", Utf8UrlEncoding = "%47", HtmlEntityName = "", XmlEntityNumber = "&#71;" },
new FrameworkGlyph { UnicodePoint = "48", UnicodeGroup = "Basic Latin", Character = "H", Windows1252UrlEncoding = "%48", Utf8UrlEncoding = "%48", HtmlEntityName = "", XmlEntityNumber = "&#72;" },
new FrameworkGlyph { UnicodePoint = "49", UnicodeGroup = "Basic Latin", Character = "I", Windows1252UrlEncoding = "%49", Utf8UrlEncoding = "%49", HtmlEntityName = "", XmlEntityNumber = "&#73;" },
new FrameworkGlyph { UnicodePoint = "4a", UnicodeGroup = "Basic Latin", Character = "J", Windows1252UrlEncoding = "%4A", Utf8UrlEncoding = "%4A", HtmlEntityName = "", XmlEntityNumber = "&#74;" },
new FrameworkGlyph { UnicodePoint = "4b", UnicodeGroup = "Basic Latin", Character = "K", Windows1252UrlEncoding = "%4B", Utf8UrlEncoding = "%4B", HtmlEntityName = "", XmlEntityNumber = "&#75;" },
new FrameworkGlyph { UnicodePoint = "4c", UnicodeGroup = "Basic Latin", Character = "L", Windows1252UrlEncoding = "%4C", Utf8UrlEncoding = "%4C", HtmlEntityName = "", XmlEntityNumber = "&#76;" },
new FrameworkGlyph { UnicodePoint = "4d", UnicodeGroup = "Basic Latin", Character = "M", Windows1252UrlEncoding = "%4D", Utf8UrlEncoding = "%4D", HtmlEntityName = "", XmlEntityNumber = "&#77;" },
new FrameworkGlyph { UnicodePoint = "4e", UnicodeGroup = "Basic Latin", Character = "N", Windows1252UrlEncoding = "%4E", Utf8UrlEncoding = "%4E", HtmlEntityName = "", XmlEntityNumber = "&#78;" },
new FrameworkGlyph { UnicodePoint = "4f", UnicodeGroup = "Basic Latin", Character = "O", Windows1252UrlEncoding = "%4F", Utf8UrlEncoding = "%4F", HtmlEntityName = "", XmlEntityNumber = "&#79;" },
new FrameworkGlyph { UnicodePoint = "50", UnicodeGroup = "Basic Latin", Character = "P", Windows1252UrlEncoding = "%50", Utf8UrlEncoding = "%50", HtmlEntityName = "", XmlEntityNumber = "&#80;" },
new FrameworkGlyph { UnicodePoint = "51", UnicodeGroup = "Basic Latin", Character = "Q", Windows1252UrlEncoding = "%51", Utf8UrlEncoding = "%51", HtmlEntityName = "", XmlEntityNumber = "&#81;" },
new FrameworkGlyph { UnicodePoint = "52", UnicodeGroup = "Basic Latin", Character = "R", Windows1252UrlEncoding = "%52", Utf8UrlEncoding = "%52", HtmlEntityName = "", XmlEntityNumber = "&#82;" },
new FrameworkGlyph { UnicodePoint = "53", UnicodeGroup = "Basic Latin", Character = "S", Windows1252UrlEncoding = "%53", Utf8UrlEncoding = "%53", HtmlEntityName = "", XmlEntityNumber = "&#83;" },
new FrameworkGlyph { UnicodePoint = "54", UnicodeGroup = "Basic Latin", Character = "T", Windows1252UrlEncoding = "%54", Utf8UrlEncoding = "%54", HtmlEntityName = "", XmlEntityNumber = "&#84;" },
new FrameworkGlyph { UnicodePoint = "55", UnicodeGroup = "Basic Latin", Character = "U", Windows1252UrlEncoding = "%55", Utf8UrlEncoding = "%55", HtmlEntityName = "", XmlEntityNumber = "&#85;" },
new FrameworkGlyph { UnicodePoint = "56", UnicodeGroup = "Basic Latin", Character = "V", Windows1252UrlEncoding = "%56", Utf8UrlEncoding = "%56", HtmlEntityName = "", XmlEntityNumber = "&#86;" },
new FrameworkGlyph { UnicodePoint = "57", UnicodeGroup = "Basic Latin", Character = "W", Windows1252UrlEncoding = "%57", Utf8UrlEncoding = "%57", HtmlEntityName = "", XmlEntityNumber = "&#87;" },
new FrameworkGlyph { UnicodePoint = "58", UnicodeGroup = "Basic Latin", Character = "X", Windows1252UrlEncoding = "%58", Utf8UrlEncoding = "%58", HtmlEntityName = "", XmlEntityNumber = "&#88;" },
new FrameworkGlyph { UnicodePoint = "59", UnicodeGroup = "Basic Latin", Character = "Y", Windows1252UrlEncoding = "%59", Utf8UrlEncoding = "%59", HtmlEntityName = "", XmlEntityNumber = "&#89;" },
new FrameworkGlyph { UnicodePoint = "5a", UnicodeGroup = "Basic Latin", Character = "Z", Windows1252UrlEncoding = "%5A", Utf8UrlEncoding = "%5A", HtmlEntityName = "", XmlEntityNumber = "&#90;" },
new FrameworkGlyph { UnicodePoint = "5b", UnicodeGroup = "Basic Latin", Character = "[", Windows1252UrlEncoding = "%5B", Utf8UrlEncoding = "%5B", HtmlEntityName = "", XmlEntityNumber = "&#91;" },
new FrameworkGlyph { UnicodePoint = "5c", UnicodeGroup = "Basic Latin", Character = "\\", Windows1252UrlEncoding = "%5C", Utf8UrlEncoding = "%5C", HtmlEntityName = "", XmlEntityNumber = "&#92;" },
new FrameworkGlyph { UnicodePoint = "5d", UnicodeGroup = "Basic Latin", Character = "]", Windows1252UrlEncoding = "%5D", Utf8UrlEncoding = "%5D", HtmlEntityName = "", XmlEntityNumber = "&#93;" },
new FrameworkGlyph { UnicodePoint = "5e", UnicodeGroup = "Basic Latin", Character = "^", Windows1252UrlEncoding = "%5E", Utf8UrlEncoding = "%5E", HtmlEntityName = "", XmlEntityNumber = "&#94;" },
new FrameworkGlyph { UnicodePoint = "5f", UnicodeGroup = "Basic Latin", Character = "_", Windows1252UrlEncoding = "%5F", Utf8UrlEncoding = "%5F", HtmlEntityName = "", XmlEntityNumber = "&#95;" },
new FrameworkGlyph { UnicodePoint = "60", UnicodeGroup = "Basic Latin", Character = "`", Windows1252UrlEncoding = "%60", Utf8UrlEncoding = "%60", HtmlEntityName = "", XmlEntityNumber = "&#96;" },
new FrameworkGlyph { UnicodePoint = "61", UnicodeGroup = "Basic Latin", Character = "a", Windows1252UrlEncoding = "%61", Utf8UrlEncoding = "%61", HtmlEntityName = "", XmlEntityNumber = "&#97;" },
new FrameworkGlyph { UnicodePoint = "62", UnicodeGroup = "Basic Latin", Character = "b", Windows1252UrlEncoding = "%62", Utf8UrlEncoding = "%62", HtmlEntityName = "", XmlEntityNumber = "&#98;" },
new FrameworkGlyph { UnicodePoint = "63", UnicodeGroup = "Basic Latin", Character = "c", Windows1252UrlEncoding = "%63", Utf8UrlEncoding = "%63", HtmlEntityName = "", XmlEntityNumber = "&#99;" },
new FrameworkGlyph { UnicodePoint = "64", UnicodeGroup = "Basic Latin", Character = "d", Windows1252UrlEncoding = "%64", Utf8UrlEncoding = "%64", HtmlEntityName = "", XmlEntityNumber = "&#100;" },
new FrameworkGlyph { UnicodePoint = "65", UnicodeGroup = "Basic Latin", Character = "e", Windows1252UrlEncoding = "%65", Utf8UrlEncoding = "%65", HtmlEntityName = "", XmlEntityNumber = "&#101;" },
new FrameworkGlyph { UnicodePoint = "66", UnicodeGroup = "Basic Latin", Character = "f", Windows1252UrlEncoding = "%66", Utf8UrlEncoding = "%66", HtmlEntityName = "", XmlEntityNumber = "&#102;" },
new FrameworkGlyph { UnicodePoint = "67", UnicodeGroup = "Basic Latin", Character = "g", Windows1252UrlEncoding = "%67", Utf8UrlEncoding = "%67", HtmlEntityName = "", XmlEntityNumber = "&#103;" },
new FrameworkGlyph { UnicodePoint = "68", UnicodeGroup = "Basic Latin", Character = "h", Windows1252UrlEncoding = "%68", Utf8UrlEncoding = "%68", HtmlEntityName = "", XmlEntityNumber = "&#104;" },
new FrameworkGlyph { UnicodePoint = "69", UnicodeGroup = "Basic Latin", Character = "i", Windows1252UrlEncoding = "%69", Utf8UrlEncoding = "%69", HtmlEntityName = "", XmlEntityNumber = "&#105;" },
new FrameworkGlyph { UnicodePoint = "6a", UnicodeGroup = "Basic Latin", Character = "j", Windows1252UrlEncoding = "%6A", Utf8UrlEncoding = "%6A", HtmlEntityName = "", XmlEntityNumber = "&#106;" },
new FrameworkGlyph { UnicodePoint = "6b", UnicodeGroup = "Basic Latin", Character = "k", Windows1252UrlEncoding = "%6B", Utf8UrlEncoding = "%6B", HtmlEntityName = "", XmlEntityNumber = "&#107;" },
new FrameworkGlyph { UnicodePoint = "6c", UnicodeGroup = "Basic Latin", Character = "l", Windows1252UrlEncoding = "%6C", Utf8UrlEncoding = "%6C", HtmlEntityName = "", XmlEntityNumber = "&#108;" },
new FrameworkGlyph { UnicodePoint = "6d", UnicodeGroup = "Basic Latin", Character = "m", Windows1252UrlEncoding = "%6D", Utf8UrlEncoding = "%6D", HtmlEntityName = "", XmlEntityNumber = "&#109;" },
new FrameworkGlyph { UnicodePoint = "6e", UnicodeGroup = "Basic Latin", Character = "n", Windows1252UrlEncoding = "%6E", Utf8UrlEncoding = "%6E", HtmlEntityName = "", XmlEntityNumber = "&#110;" },
new FrameworkGlyph { UnicodePoint = "6f", UnicodeGroup = "Basic Latin", Character = "o", Windows1252UrlEncoding = "%6F", Utf8UrlEncoding = "%6F", HtmlEntityName = "", XmlEntityNumber = "&#111;" },
new FrameworkGlyph { UnicodePoint = "70", UnicodeGroup = "Basic Latin", Character = "p", Windows1252UrlEncoding = "%70", Utf8UrlEncoding = "%70", HtmlEntityName = "", XmlEntityNumber = "&#112;" },
new FrameworkGlyph { UnicodePoint = "71", UnicodeGroup = "Basic Latin", Character = "q", Windows1252UrlEncoding = "%71", Utf8UrlEncoding = "%71", HtmlEntityName = "", XmlEntityNumber = "&#113;" },
new FrameworkGlyph { UnicodePoint = "72", UnicodeGroup = "Basic Latin", Character = "r", Windows1252UrlEncoding = "%72", Utf8UrlEncoding = "%72", HtmlEntityName = "", XmlEntityNumber = "&#114;" },
new FrameworkGlyph { UnicodePoint = "73", UnicodeGroup = "Basic Latin", Character = "s", Windows1252UrlEncoding = "%73", Utf8UrlEncoding = "%73", HtmlEntityName = "", XmlEntityNumber = "&#115;" },
new FrameworkGlyph { UnicodePoint = "74", UnicodeGroup = "Basic Latin", Character = "t", Windows1252UrlEncoding = "%74", Utf8UrlEncoding = "%74", HtmlEntityName = "", XmlEntityNumber = "&#116;" },
new FrameworkGlyph { UnicodePoint = "75", UnicodeGroup = "Basic Latin", Character = "u", Windows1252UrlEncoding = "%75", Utf8UrlEncoding = "%75", HtmlEntityName = "", XmlEntityNumber = "&#117;" },
new FrameworkGlyph { UnicodePoint = "76", UnicodeGroup = "Basic Latin", Character = "v", Windows1252UrlEncoding = "%76", Utf8UrlEncoding = "%76", HtmlEntityName = "", XmlEntityNumber = "&#118;" },
new FrameworkGlyph { UnicodePoint = "77", UnicodeGroup = "Basic Latin", Character = "w", Windows1252UrlEncoding = "%77", Utf8UrlEncoding = "%77", HtmlEntityName = "", XmlEntityNumber = "&#119;" },
new FrameworkGlyph { UnicodePoint = "78", UnicodeGroup = "Basic Latin", Character = "x", Windows1252UrlEncoding = "%78", Utf8UrlEncoding = "%78", HtmlEntityName = "", XmlEntityNumber = "&#120;" },
new FrameworkGlyph { UnicodePoint = "79", UnicodeGroup = "Basic Latin", Character = "y", Windows1252UrlEncoding = "%79", Utf8UrlEncoding = "%79", HtmlEntityName = "", XmlEntityNumber = "&#121;" },
new FrameworkGlyph { UnicodePoint = "7a", UnicodeGroup = "Basic Latin", Character = "z", Windows1252UrlEncoding = "%7A", Utf8UrlEncoding = "%7A", HtmlEntityName = "", XmlEntityNumber = "&#122;" },
new FrameworkGlyph { UnicodePoint = "7b", UnicodeGroup = "Basic Latin", Character = "{", Windows1252UrlEncoding = "%7B", Utf8UrlEncoding = "%7B", HtmlEntityName = "", XmlEntityNumber = "&#123;" },
new FrameworkGlyph { UnicodePoint = "7c", UnicodeGroup = "Basic Latin", Character = "|", Windows1252UrlEncoding = "%7C", Utf8UrlEncoding = "%7C", HtmlEntityName = "", XmlEntityNumber = "&#124;" },
new FrameworkGlyph { UnicodePoint = "7d", UnicodeGroup = "Basic Latin", Character = "}", Windows1252UrlEncoding = "%7D", Utf8UrlEncoding = "%7D", HtmlEntityName = "", XmlEntityNumber = "&#125;" },
new FrameworkGlyph { UnicodePoint = "7e", UnicodeGroup = "Basic Latin", Character = "~", Windows1252UrlEncoding = "%7E", Utf8UrlEncoding = "%7E", HtmlEntityName = "", XmlEntityNumber = "&#126;" },
new FrameworkGlyph { UnicodePoint = "7f", UnicodeGroup = "Basic Latin", Character = "", Windows1252UrlEncoding = "%7F", Utf8UrlEncoding = "%7F", HtmlEntityName = "", XmlEntityNumber = "&#127;" },
new FrameworkGlyph { UnicodePoint = "80", UnicodeGroup = "Latin-1 Supplement", Character = "`", Windows1252UrlEncoding = "%80", Utf8UrlEncoding = "%E2%82%AC", HtmlEntityName = "", XmlEntityNumber = "&#128;" },
new FrameworkGlyph { UnicodePoint = "81", UnicodeGroup = "Latin-1 Supplement", Character = "", Windows1252UrlEncoding = "%81", Utf8UrlEncoding = "%81", HtmlEntityName = "", XmlEntityNumber = "&#129;" },
new FrameworkGlyph { UnicodePoint = "201a", UnicodeGroup = "General Punctuation", Character = "‚", Windows1252UrlEncoding = "%82", Utf8UrlEncoding = "%E2%80%9A", HtmlEntityName = "&sbquo;", XmlEntityNumber = "&#8218;" },
new FrameworkGlyph { UnicodePoint = "192", UnicodeGroup = "Latin Extended-B", Character = "ƒ", Windows1252UrlEncoding = "%83", Utf8UrlEncoding = "%C6%92", HtmlEntityName = "&fnof;", XmlEntityNumber = "&#402;" },
new FrameworkGlyph { UnicodePoint = "201e", UnicodeGroup = "General Punctuation", Character = "„", Windows1252UrlEncoding = "%84", Utf8UrlEncoding = "%E2%80%9E", HtmlEntityName = "&bdquo;", XmlEntityNumber = "&#8222;" },
new FrameworkGlyph { UnicodePoint = "2026", UnicodeGroup = "General Punctuation", Character = "…", Windows1252UrlEncoding = "%85", Utf8UrlEncoding = "%E2%80%A6", HtmlEntityName = "&hellip;", XmlEntityNumber = "&#8230;" },
new FrameworkGlyph { UnicodePoint = "2020", UnicodeGroup = "General Punctuation", Character = "†", Windows1252UrlEncoding = "%86", Utf8UrlEncoding = "%E2%80%A0", HtmlEntityName = "&dagger;", XmlEntityNumber = "&#8224;" },
new FrameworkGlyph { UnicodePoint = "2021", UnicodeGroup = "General Punctuation", Character = "‡", Windows1252UrlEncoding = "%87", Utf8UrlEncoding = "%E2%80%A1", HtmlEntityName = "&Dagger;", XmlEntityNumber = "&#8225;" },
new FrameworkGlyph { UnicodePoint = "2c6", UnicodeGroup = "Spacing Modifier Letters", Character = "ˆ", Windows1252UrlEncoding = "%88", Utf8UrlEncoding = "%CB%86", HtmlEntityName = "&circ;", XmlEntityNumber = "&#710;" },
new FrameworkGlyph { UnicodePoint = "2030", UnicodeGroup = "General Punctuation", Character = "‰", Windows1252UrlEncoding = "%89", Utf8UrlEncoding = "%E2%80%B0", HtmlEntityName = "&permil;", XmlEntityNumber = "&#8240;" },
new FrameworkGlyph { UnicodePoint = "160", UnicodeGroup = "Latin Extended-A", Character = "Š", Windows1252UrlEncoding = "%8A", Utf8UrlEncoding = "%C5%A0", HtmlEntityName = "&Scaron;", XmlEntityNumber = "&#352;" },
new FrameworkGlyph { UnicodePoint = "2039", UnicodeGroup = "General Punctuation", Character = "‹", Windows1252UrlEncoding = "%8B", Utf8UrlEncoding = "%E2%80%B9", HtmlEntityName = "&lsaquo;", XmlEntityNumber = "&#8249;" },
new FrameworkGlyph { UnicodePoint = "152", UnicodeGroup = "Latin Extended-A", Character = "Œ", Windows1252UrlEncoding = "%8C", Utf8UrlEncoding = "%C5%92", HtmlEntityName = "&OElig;", XmlEntityNumber = "&#338;" },
new FrameworkGlyph { UnicodePoint = "8d", UnicodeGroup = "Latin-1 Supplement", Character = "", Windows1252UrlEncoding = "%8D", Utf8UrlEncoding = "%C5%8D", HtmlEntityName = "", XmlEntityNumber = "&#141;" },
new FrameworkGlyph { UnicodePoint = "17d", UnicodeGroup = "Latin Extended-A", Character = "Ž", Windows1252UrlEncoding = "%8E", Utf8UrlEncoding = "%C5%BD", HtmlEntityName = "", XmlEntityNumber = "&#381;" },
new FrameworkGlyph { UnicodePoint = "8f", UnicodeGroup = "Latin-1 Supplement", Character = "", Windows1252UrlEncoding = "%8F", Utf8UrlEncoding = "%8F", HtmlEntityName = "", XmlEntityNumber = "&#143;" },
new FrameworkGlyph { UnicodePoint = "90", UnicodeGroup = "Latin-1 Supplement", Character = "", Windows1252UrlEncoding = "%90", Utf8UrlEncoding = "%C2%90", HtmlEntityName = "", XmlEntityNumber = "&#144;" },
new FrameworkGlyph { UnicodePoint = "2018", UnicodeGroup = "General Punctuation", Character = "‘", Windows1252UrlEncoding = "%91", Utf8UrlEncoding = "%E2%80%98", HtmlEntityName = "&lsquo;", XmlEntityNumber = "&#8216;" },
new FrameworkGlyph { UnicodePoint = "2019", UnicodeGroup = "General Punctuation", Character = "’", Windows1252UrlEncoding = "%92", Utf8UrlEncoding = "%E2%80%99", HtmlEntityName = "&rsquo;", XmlEntityNumber = "&#8217;" },
new FrameworkGlyph { UnicodePoint = "201c", UnicodeGroup = "General Punctuation", Character = "“", Windows1252UrlEncoding = "%93", Utf8UrlEncoding = "%E2%80%9C", HtmlEntityName = "&ldquo;", XmlEntityNumber = "&#8220;" },
new FrameworkGlyph { UnicodePoint = "201d", UnicodeGroup = "General Punctuation", Character = "”", Windows1252UrlEncoding = "%94", Utf8UrlEncoding = "%E2%80%9D", HtmlEntityName = "&rdquo;", XmlEntityNumber = "&#8221;" },
new FrameworkGlyph { UnicodePoint = "2022", UnicodeGroup = "General Punctuation", Character = "•", Windows1252UrlEncoding = "%95", Utf8UrlEncoding = "%E2%80%A2", HtmlEntityName = "&bull;", XmlEntityNumber = "&#8226;" },
new FrameworkGlyph { UnicodePoint = "2013", UnicodeGroup = "General Punctuation", Character = "–", Windows1252UrlEncoding = "%96", Utf8UrlEncoding = "%E2%80%93", HtmlEntityName = "&ndash;", XmlEntityNumber = "&#8211;" },
new FrameworkGlyph { UnicodePoint = "2014", UnicodeGroup = "General Punctuation", Character = "—", Windows1252UrlEncoding = "%97", Utf8UrlEncoding = "%E2%80%94", HtmlEntityName = "&mdash;", XmlEntityNumber = "&#8212;" },
new FrameworkGlyph { UnicodePoint = "2dc", UnicodeGroup = "Spacing Modifier Letters", Character = "˜", Windows1252UrlEncoding = "%98", Utf8UrlEncoding = "%CB%9C", HtmlEntityName = "&tilde;", XmlEntityNumber = "&#732;" },
new FrameworkGlyph { UnicodePoint = "2122", UnicodeGroup = "Letterlike Symbols", Character = "™", Windows1252UrlEncoding = "%99", Utf8UrlEncoding = "%E2%84", HtmlEntityName = "&trade;", XmlEntityNumber = "&#8482;" },
new FrameworkGlyph { UnicodePoint = "161", UnicodeGroup = "Latin Extended-A", Character = "š", Windows1252UrlEncoding = "%9A", Utf8UrlEncoding = "%C5%A1", HtmlEntityName = "&scaron;", XmlEntityNumber = "&#353;" },
new FrameworkGlyph { UnicodePoint = "203a", UnicodeGroup = "General Punctuation", Character = "›", Windows1252UrlEncoding = "%9B", Utf8UrlEncoding = "%E2%80", HtmlEntityName = "&rsaquo;", XmlEntityNumber = "&#8250;" },
new FrameworkGlyph { UnicodePoint = "153", UnicodeGroup = "Latin Extended-A", Character = "œ", Windows1252UrlEncoding = "%9C", Utf8UrlEncoding = "%C5%93", HtmlEntityName = "&oelig;", XmlEntityNumber = "&#339;" },
new FrameworkGlyph { UnicodePoint = "9d", UnicodeGroup = "Latin-1 Supplement", Character = "", Windows1252UrlEncoding = "%9D", Utf8UrlEncoding = "%9D", HtmlEntityName = "", XmlEntityNumber = "&#157;" },
new FrameworkGlyph { UnicodePoint = "17e", UnicodeGroup = "Latin Extended-A", Character = "ž", Windows1252UrlEncoding = "%9E", Utf8UrlEncoding = "%C5%BE", HtmlEntityName = "", XmlEntityNumber = "&#382;" },
new FrameworkGlyph { UnicodePoint = "178", UnicodeGroup = "Latin Extended-A", Character = "Ÿ", Windows1252UrlEncoding = "%9F", Utf8UrlEncoding = "%C5%B8", HtmlEntityName = "&Yuml;", XmlEntityNumber = "&#376;" },
new FrameworkGlyph { UnicodePoint = "a0", UnicodeGroup = "Latin-1 Supplement", Character = "", Windows1252UrlEncoding = "%A0", Utf8UrlEncoding = "%C2%A0", HtmlEntityName = "&nbsp;", XmlEntityNumber = "&#160;" },
new FrameworkGlyph { UnicodePoint = "a1", UnicodeGroup = "Latin-1 Supplement", Character = "¡", Windows1252UrlEncoding = "%A1", Utf8UrlEncoding = "%C2%A1", HtmlEntityName = "&iexcl;", XmlEntityNumber = "&#161;" },
new FrameworkGlyph { UnicodePoint = "a2", UnicodeGroup = "Latin-1 Supplement", Character = "¢", Windows1252UrlEncoding = "%A2", Utf8UrlEncoding = "%C2%A2", HtmlEntityName = "&cent;", XmlEntityNumber = "&#162;" },
new FrameworkGlyph { UnicodePoint = "a3", UnicodeGroup = "Latin-1 Supplement", Character = "£", Windows1252UrlEncoding = "%A3", Utf8UrlEncoding = "%C2%A3", HtmlEntityName = "&pound;", XmlEntityNumber = "&#163;" },
new FrameworkGlyph { UnicodePoint = "a4", UnicodeGroup = "Latin-1 Supplement", Character = "¤", Windows1252UrlEncoding = "%A4", Utf8UrlEncoding = "%C2%A4", HtmlEntityName = "&curren;", XmlEntityNumber = "&#164;" },
new FrameworkGlyph { UnicodePoint = "a5", UnicodeGroup = "Latin-1 Supplement", Character = "¥", Windows1252UrlEncoding = "%A5", Utf8UrlEncoding = "%C2%A5", HtmlEntityName = "&yen;", XmlEntityNumber = "&#165;" },
new FrameworkGlyph { UnicodePoint = "a6", UnicodeGroup = "Latin-1 Supplement", Character = "¦", Windows1252UrlEncoding = "%A6", Utf8UrlEncoding = "%C2%A6", HtmlEntityName = "&brvbar;", XmlEntityNumber = "&#166;" },
new FrameworkGlyph { UnicodePoint = "a7", UnicodeGroup = "Latin-1 Supplement", Character = "§", Windows1252UrlEncoding = "%A7", Utf8UrlEncoding = "%C2%A7", HtmlEntityName = "&sect;", XmlEntityNumber = "&#167;" },
new FrameworkGlyph { UnicodePoint = "a8", UnicodeGroup = "Latin-1 Supplement", Character = "¨", Windows1252UrlEncoding = "%A8", Utf8UrlEncoding = "%C2%A8", HtmlEntityName = "&uml;", XmlEntityNumber = "&#168;" },
new FrameworkGlyph { UnicodePoint = "a9", UnicodeGroup = "Latin-1 Supplement", Character = "©", Windows1252UrlEncoding = "%A9", Utf8UrlEncoding = "%C2%A9", HtmlEntityName = "&copy;", XmlEntityNumber = "&#169;" },
new FrameworkGlyph { UnicodePoint = "aa", UnicodeGroup = "Latin-1 Supplement", Character = "ª", Windows1252UrlEncoding = "%AA", Utf8UrlEncoding = "%C2%AA", HtmlEntityName = "&ordf;", XmlEntityNumber = "&#170;" },
new FrameworkGlyph { UnicodePoint = "ab", UnicodeGroup = "Latin-1 Supplement", Character = "«", Windows1252UrlEncoding = "%AB", Utf8UrlEncoding = "%C2%AB", HtmlEntityName = "&laquo;", XmlEntityNumber = "&#171;" },
new FrameworkGlyph { UnicodePoint = "ac", UnicodeGroup = "Latin-1 Supplement", Character = "¬", Windows1252UrlEncoding = "%AC", Utf8UrlEncoding = "%C2%AC", HtmlEntityName = "&not;", XmlEntityNumber = "&#172;" },
new FrameworkGlyph { UnicodePoint = "ad", UnicodeGroup = "Latin-1 Supplement", Character = "­", Windows1252UrlEncoding = "%AD", Utf8UrlEncoding = "%C2%AD", HtmlEntityName = "&shy;", XmlEntityNumber = "&#173;" },
new FrameworkGlyph { UnicodePoint = "ae", UnicodeGroup = "Latin-1 Supplement", Character = "®", Windows1252UrlEncoding = "%AE", Utf8UrlEncoding = "%C2%AE", HtmlEntityName = "&reg;", XmlEntityNumber = "&#174;" },
new FrameworkGlyph { UnicodePoint = "af", UnicodeGroup = "Latin-1 Supplement", Character = "¯", Windows1252UrlEncoding = "%AF", Utf8UrlEncoding = "%C2%AF", HtmlEntityName = "&macr;", XmlEntityNumber = "&#175;" },
new FrameworkGlyph { UnicodePoint = "b0", UnicodeGroup = "Latin-1 Supplement", Character = "°", Windows1252UrlEncoding = "%B0", Utf8UrlEncoding = "%C2%B0", HtmlEntityName = "&deg;", XmlEntityNumber = "&#176;" },
new FrameworkGlyph { UnicodePoint = "b1", UnicodeGroup = "Latin-1 Supplement", Character = "±", Windows1252UrlEncoding = "%B1", Utf8UrlEncoding = "%C2%B1", HtmlEntityName = "&plusmn;", XmlEntityNumber = "&#177;" },
new FrameworkGlyph { UnicodePoint = "b2", UnicodeGroup = "Latin-1 Supplement", Character = "²", Windows1252UrlEncoding = "%B2", Utf8UrlEncoding = "%C2%B2", HtmlEntityName = "&sup2;", XmlEntityNumber = "&#178;" },
new FrameworkGlyph { UnicodePoint = "b3", UnicodeGroup = "Latin-1 Supplement", Character = "³", Windows1252UrlEncoding = "%B3", Utf8UrlEncoding = "%C2%B3", HtmlEntityName = "&sup3;", XmlEntityNumber = "&#179;" },
new FrameworkGlyph { UnicodePoint = "b4", UnicodeGroup = "Latin-1 Supplement", Character = "´", Windows1252UrlEncoding = "%B4", Utf8UrlEncoding = "%C2%B4", HtmlEntityName = "&acute;", XmlEntityNumber = "&#180;" },
new FrameworkGlyph { UnicodePoint = "b5", UnicodeGroup = "Latin-1 Supplement", Character = "µ", Windows1252UrlEncoding = "%B5", Utf8UrlEncoding = "%C2%B5", HtmlEntityName = "&micro;", XmlEntityNumber = "&#181;" },
new FrameworkGlyph { UnicodePoint = "b6", UnicodeGroup = "Latin-1 Supplement", Character = "¶", Windows1252UrlEncoding = "%B6", Utf8UrlEncoding = "%C2%B6", HtmlEntityName = "&para;", XmlEntityNumber = "&#182;" },
new FrameworkGlyph { UnicodePoint = "b7", UnicodeGroup = "Latin-1 Supplement", Character = "·", Windows1252UrlEncoding = "%B7", Utf8UrlEncoding = "%C2%B7", HtmlEntityName = "&middot;", XmlEntityNumber = "&#183;" },
new FrameworkGlyph { UnicodePoint = "b8", UnicodeGroup = "Latin-1 Supplement", Character = "¸", Windows1252UrlEncoding = "%B8", Utf8UrlEncoding = "%C2%B8", HtmlEntityName = "&cedil;", XmlEntityNumber = "&#184;" },
new FrameworkGlyph { UnicodePoint = "b9", UnicodeGroup = "Latin-1 Supplement", Character = "¹", Windows1252UrlEncoding = "%B9", Utf8UrlEncoding = "%C2%B9", HtmlEntityName = "&sup1;", XmlEntityNumber = "&#185;" },
new FrameworkGlyph { UnicodePoint = "ba", UnicodeGroup = "Latin-1 Supplement", Character = "º", Windows1252UrlEncoding = "%BA", Utf8UrlEncoding = "%C2%BA", HtmlEntityName = "&ordm;", XmlEntityNumber = "&#186;" },
new FrameworkGlyph { UnicodePoint = "bb", UnicodeGroup = "Latin-1 Supplement", Character = "»", Windows1252UrlEncoding = "%BB", Utf8UrlEncoding = "%C2%BB", HtmlEntityName = "&raquo;", XmlEntityNumber = "&#187;" },
new FrameworkGlyph { UnicodePoint = "bc", UnicodeGroup = "Latin-1 Supplement", Character = "¼", Windows1252UrlEncoding = "%BC", Utf8UrlEncoding = "%C2%BC", HtmlEntityName = "&frac14;", XmlEntityNumber = "&#188;" },
new FrameworkGlyph { UnicodePoint = "bd", UnicodeGroup = "Latin-1 Supplement", Character = "½", Windows1252UrlEncoding = "%BD", Utf8UrlEncoding = "%C2%BD", HtmlEntityName = "&frac12;", XmlEntityNumber = "&#189;" },
new FrameworkGlyph { UnicodePoint = "be", UnicodeGroup = "Latin-1 Supplement", Character = "¾", Windows1252UrlEncoding = "%BE", Utf8UrlEncoding = "%C2%BE", HtmlEntityName = "&frac34;", XmlEntityNumber = "&#190;" },
new FrameworkGlyph { UnicodePoint = "bf", UnicodeGroup = "Latin-1 Supplement", Character = "¿", Windows1252UrlEncoding = "%BF", Utf8UrlEncoding = "%C2%BF", HtmlEntityName = "&iquest;", XmlEntityNumber = "&#191;" },
new FrameworkGlyph { UnicodePoint = "c0", UnicodeGroup = "Latin-1 Supplement", Character = "À", Windows1252UrlEncoding = "%C0", Utf8UrlEncoding = "%C3%80", HtmlEntityName = "&Agrave;", XmlEntityNumber = "&#192;" },
new FrameworkGlyph { UnicodePoint = "c1", UnicodeGroup = "Latin-1 Supplement", Character = "Á", Windows1252UrlEncoding = "%C1", Utf8UrlEncoding = "%C3%81", HtmlEntityName = "&Aacute;", XmlEntityNumber = "&#193;" },
new FrameworkGlyph { UnicodePoint = "c2", UnicodeGroup = "Latin-1 Supplement", Character = "Â", Windows1252UrlEncoding = "%C2", Utf8UrlEncoding = "%C3%82", HtmlEntityName = "&Acirc;", XmlEntityNumber = "&#194;" },
new FrameworkGlyph { UnicodePoint = "c3", UnicodeGroup = "Latin-1 Supplement", Character = "Ã", Windows1252UrlEncoding = "%C3", Utf8UrlEncoding = "%C3%83", HtmlEntityName = "&Atilde;", XmlEntityNumber = "&#195;" },
new FrameworkGlyph { UnicodePoint = "c4", UnicodeGroup = "Latin-1 Supplement", Character = "Ä", Windows1252UrlEncoding = "%C4", Utf8UrlEncoding = "%C3%84", HtmlEntityName = "&Auml;", XmlEntityNumber = "&#196;" },
new FrameworkGlyph { UnicodePoint = "c5", UnicodeGroup = "Latin-1 Supplement", Character = "Å", Windows1252UrlEncoding = "%C5", Utf8UrlEncoding = "%C3%85", HtmlEntityName = "&Aring;", XmlEntityNumber = "&#197;" },
new FrameworkGlyph { UnicodePoint = "c6", UnicodeGroup = "Latin-1 Supplement", Character = "Æ", Windows1252UrlEncoding = "%C6", Utf8UrlEncoding = "%C3%86", HtmlEntityName = "&AElig;", XmlEntityNumber = "&#198;" },
new FrameworkGlyph { UnicodePoint = "c7", UnicodeGroup = "Latin-1 Supplement", Character = "Ç", Windows1252UrlEncoding = "%C7", Utf8UrlEncoding = "%C3%87", HtmlEntityName = "&Ccedil;", XmlEntityNumber = "&#199;" },
new FrameworkGlyph { UnicodePoint = "c8", UnicodeGroup = "Latin-1 Supplement", Character = "È", Windows1252UrlEncoding = "%C8", Utf8UrlEncoding = "%C3%88", HtmlEntityName = "&Egrave;", XmlEntityNumber = "&#200;" },
new FrameworkGlyph { UnicodePoint = "c9", UnicodeGroup = "Latin-1 Supplement", Character = "É", Windows1252UrlEncoding = "%C9", Utf8UrlEncoding = "%C3%89", HtmlEntityName = "&Eacute;", XmlEntityNumber = "&#201;" },
new FrameworkGlyph { UnicodePoint = "ca", UnicodeGroup = "Latin-1 Supplement", Character = "Ê", Windows1252UrlEncoding = "%CA", Utf8UrlEncoding = "%C3%8A", HtmlEntityName = "&Ecirc;", XmlEntityNumber = "&#202;" },
new FrameworkGlyph { UnicodePoint = "cb", UnicodeGroup = "Latin-1 Supplement", Character = "Ë", Windows1252UrlEncoding = "%CB", Utf8UrlEncoding = "%C3%8B", HtmlEntityName = "&Euml;", XmlEntityNumber = "&#203;" },
new FrameworkGlyph { UnicodePoint = "cc", UnicodeGroup = "Latin-1 Supplement", Character = "Ì", Windows1252UrlEncoding = "%CC", Utf8UrlEncoding = "%C3%8C", HtmlEntityName = "&Igrave;", XmlEntityNumber = "&#204;" },
new FrameworkGlyph { UnicodePoint = "cd", UnicodeGroup = "Latin-1 Supplement", Character = "Í", Windows1252UrlEncoding = "%CD", Utf8UrlEncoding = "%C3%8D", HtmlEntityName = "&Iacute;", XmlEntityNumber = "&#205;" },
new FrameworkGlyph { UnicodePoint = "ce", UnicodeGroup = "Latin-1 Supplement", Character = "Î", Windows1252UrlEncoding = "%CE", Utf8UrlEncoding = "%C3%8E", HtmlEntityName = "&Icirc;", XmlEntityNumber = "&#206;" },
new FrameworkGlyph { UnicodePoint = "cf", UnicodeGroup = "Latin-1 Supplement", Character = "Ï", Windows1252UrlEncoding = "%CF", Utf8UrlEncoding = "%C3%8F", HtmlEntityName = "&Iuml;", XmlEntityNumber = "&#207;" },
new FrameworkGlyph { UnicodePoint = "d0", UnicodeGroup = "Latin-1 Supplement", Character = "Ð", Windows1252UrlEncoding = "%D0", Utf8UrlEncoding = "%C3%90", HtmlEntityName = "&ETH;", XmlEntityNumber = "&#208;" },
new FrameworkGlyph { UnicodePoint = "d1", UnicodeGroup = "Latin-1 Supplement", Character = "Ñ", Windows1252UrlEncoding = "%D1", Utf8UrlEncoding = "%C3%91", HtmlEntityName = "&Ntilde;", XmlEntityNumber = "&#209;" },
new FrameworkGlyph { UnicodePoint = "d2", UnicodeGroup = "Latin-1 Supplement", Character = "Ò", Windows1252UrlEncoding = "%D2", Utf8UrlEncoding = "%C3%92", HtmlEntityName = "&Ograve;", XmlEntityNumber = "&#210;" },
new FrameworkGlyph { UnicodePoint = "d3", UnicodeGroup = "Latin-1 Supplement", Character = "Ó", Windows1252UrlEncoding = "%D3", Utf8UrlEncoding = "%C3%93", HtmlEntityName = "&Oacute;", XmlEntityNumber = "&#211;" },
new FrameworkGlyph { UnicodePoint = "d4", UnicodeGroup = "Latin-1 Supplement", Character = "Ô", Windows1252UrlEncoding = "%D4", Utf8UrlEncoding = "%C3%94", HtmlEntityName = "&Ocirc;", XmlEntityNumber = "&#212;" },
new FrameworkGlyph { UnicodePoint = "d5", UnicodeGroup = "Latin-1 Supplement", Character = "Õ", Windows1252UrlEncoding = "%D5", Utf8UrlEncoding = "%C3%95", HtmlEntityName = "&Otilde;", XmlEntityNumber = "&#213;" },
new FrameworkGlyph { UnicodePoint = "d6", UnicodeGroup = "Latin-1 Supplement", Character = "Ö", Windows1252UrlEncoding = "%D6", Utf8UrlEncoding = "%C3%96", HtmlEntityName = "&Ouml;", XmlEntityNumber = "&#214;" },
new FrameworkGlyph { UnicodePoint = "d7", UnicodeGroup = "Latin-1 Supplement", Character = "×", Windows1252UrlEncoding = "%D7", Utf8UrlEncoding = "%C3%97", HtmlEntityName = "&times;", XmlEntityNumber = "&#215;" },
new FrameworkGlyph { UnicodePoint = "d8", UnicodeGroup = "Latin-1 Supplement", Character = "Ø", Windows1252UrlEncoding = "%D8", Utf8UrlEncoding = "%C3%98", HtmlEntityName = "&Oslash;", XmlEntityNumber = "&#216;" },
new FrameworkGlyph { UnicodePoint = "d9", UnicodeGroup = "Latin-1 Supplement", Character = "Ù", Windows1252UrlEncoding = "%D9", Utf8UrlEncoding = "%C3%99", HtmlEntityName = "&Ugrave;", XmlEntityNumber = "&#217;" },
new FrameworkGlyph { UnicodePoint = "da", UnicodeGroup = "Latin-1 Supplement", Character = "Ú", Windows1252UrlEncoding = "%DA", Utf8UrlEncoding = "%C3%9A", HtmlEntityName = "&Uacute;", XmlEntityNumber = "&#218;" },
new FrameworkGlyph { UnicodePoint = "db", UnicodeGroup = "Latin-1 Supplement", Character = "Û", Windows1252UrlEncoding = "%DB", Utf8UrlEncoding = "%C3%9B", HtmlEntityName = "&Ucirc;", XmlEntityNumber = "&#219;" },
new FrameworkGlyph { UnicodePoint = "dc", UnicodeGroup = "Latin-1 Supplement", Character = "Ü", Windows1252UrlEncoding = "%DC", Utf8UrlEncoding = "%C3%9C", HtmlEntityName = "&Uuml;", XmlEntityNumber = "&#220;" },
new FrameworkGlyph { UnicodePoint = "dd", UnicodeGroup = "Latin-1 Supplement", Character = "Ý", Windows1252UrlEncoding = "%DD", Utf8UrlEncoding = "%C3%9D", HtmlEntityName = "&Yacute;", XmlEntityNumber = "&#221;" },
new FrameworkGlyph { UnicodePoint = "de", UnicodeGroup = "Latin-1 Supplement", Character = "Þ", Windows1252UrlEncoding = "%DE", Utf8UrlEncoding = "%C3%9E", HtmlEntityName = "&THORN;", XmlEntityNumber = "&#222;" },
new FrameworkGlyph { UnicodePoint = "df", UnicodeGroup = "Latin-1 Supplement", Character = "ß", Windows1252UrlEncoding = "%DF", Utf8UrlEncoding = "%C3%9F", HtmlEntityName = "&szlig;", XmlEntityNumber = "&#223;" },
new FrameworkGlyph { UnicodePoint = "e0", UnicodeGroup = "Latin-1 Supplement", Character = "à", Windows1252UrlEncoding = "%E0", Utf8UrlEncoding = "%C3%A0", HtmlEntityName = "&agrave;", XmlEntityNumber = "&#224;" },
new FrameworkGlyph { UnicodePoint = "e1", UnicodeGroup = "Latin-1 Supplement", Character = "á", Windows1252UrlEncoding = "%E1", Utf8UrlEncoding = "%C3%A1", HtmlEntityName = "&aacute;", XmlEntityNumber = "&#225;" },
new FrameworkGlyph { UnicodePoint = "e2", UnicodeGroup = "Latin-1 Supplement", Character = "â", Windows1252UrlEncoding = "%E2", Utf8UrlEncoding = "%C3%A2", HtmlEntityName = "&acirc;", XmlEntityNumber = "&#226;" },
new FrameworkGlyph { UnicodePoint = "e3", UnicodeGroup = "Latin-1 Supplement", Character = "ã", Windows1252UrlEncoding = "%E3", Utf8UrlEncoding = "%C3%A3", HtmlEntityName = "&atilde;", XmlEntityNumber = "&#227;" },
new FrameworkGlyph { UnicodePoint = "e4", UnicodeGroup = "Latin-1 Supplement", Character = "ä", Windows1252UrlEncoding = "%E4", Utf8UrlEncoding = "%C3%A4", HtmlEntityName = "&auml;", XmlEntityNumber = "&#228;" },
new FrameworkGlyph { UnicodePoint = "e5", UnicodeGroup = "Latin-1 Supplement", Character = "å", Windows1252UrlEncoding = "%E5", Utf8UrlEncoding = "%C3%A5", HtmlEntityName = "&aring;", XmlEntityNumber = "&#229;" },
new FrameworkGlyph { UnicodePoint = "e6", UnicodeGroup = "Latin-1 Supplement", Character = "æ", Windows1252UrlEncoding = "%E6", Utf8UrlEncoding = "%C3%A6", HtmlEntityName = "&aelig;", XmlEntityNumber = "&#230;" },
new FrameworkGlyph { UnicodePoint = "e7", UnicodeGroup = "Latin-1 Supplement", Character = "ç", Windows1252UrlEncoding = "%E7", Utf8UrlEncoding = "%C3%A7", HtmlEntityName = "&ccedil;", XmlEntityNumber = "&#231;" },
new FrameworkGlyph { UnicodePoint = "e8", UnicodeGroup = "Latin-1 Supplement", Character = "è", Windows1252UrlEncoding = "%E8", Utf8UrlEncoding = "%C3%A8", HtmlEntityName = "&egrave;", XmlEntityNumber = "&#232;" },
new FrameworkGlyph { UnicodePoint = "e9", UnicodeGroup = "Latin-1 Supplement", Character = "é", Windows1252UrlEncoding = "%E9", Utf8UrlEncoding = "%C3%A9", HtmlEntityName = "&eacute;", XmlEntityNumber = "&#233;" },
new FrameworkGlyph { UnicodePoint = "ea", UnicodeGroup = "Latin-1 Supplement", Character = "ê", Windows1252UrlEncoding = "%EA", Utf8UrlEncoding = "%C3%AA", HtmlEntityName = "&ecirc;", XmlEntityNumber = "&#234;" },
new FrameworkGlyph { UnicodePoint = "eb", UnicodeGroup = "Latin-1 Supplement", Character = "ë", Windows1252UrlEncoding = "%EB", Utf8UrlEncoding = "%C3%AB", HtmlEntityName = "&euml;", XmlEntityNumber = "&#235;" },
new FrameworkGlyph { UnicodePoint = "ec", UnicodeGroup = "Latin-1 Supplement", Character = "ì", Windows1252UrlEncoding = "%EC", Utf8UrlEncoding = "%C3%AC", HtmlEntityName = "&igrave;", XmlEntityNumber = "&#236;" },
new FrameworkGlyph { UnicodePoint = "ed", UnicodeGroup = "Latin-1 Supplement", Character = "í", Windows1252UrlEncoding = "%ED", Utf8UrlEncoding = "%C3%AD", HtmlEntityName = "&iacute;", XmlEntityNumber = "&#237;" },
new FrameworkGlyph { UnicodePoint = "ee", UnicodeGroup = "Latin-1 Supplement", Character = "î", Windows1252UrlEncoding = "%EE", Utf8UrlEncoding = "%C3%AE", HtmlEntityName = "&icirc;", XmlEntityNumber = "&#238;" },
new FrameworkGlyph { UnicodePoint = "ef", UnicodeGroup = "Latin-1 Supplement", Character = "ï", Windows1252UrlEncoding = "%EF", Utf8UrlEncoding = "%C3%AF", HtmlEntityName = "&iuml;", XmlEntityNumber = "&#239;" },
new FrameworkGlyph { UnicodePoint = "f0", UnicodeGroup = "Latin-1 Supplement", Character = "ð", Windows1252UrlEncoding = "%F0", Utf8UrlEncoding = "%C3%B0", HtmlEntityName = "&eth;", XmlEntityNumber = "&#240;" },
new FrameworkGlyph { UnicodePoint = "f1", UnicodeGroup = "Latin-1 Supplement", Character = "ñ", Windows1252UrlEncoding = "%F1", Utf8UrlEncoding = "%C3%B1", HtmlEntityName = "&ntilde;", XmlEntityNumber = "&#241;" },
new FrameworkGlyph { UnicodePoint = "f2", UnicodeGroup = "Latin-1 Supplement", Character = "ò", Windows1252UrlEncoding = "%F2", Utf8UrlEncoding = "%C3%B2", HtmlEntityName = "&ograve;", XmlEntityNumber = "&#242;" },
new FrameworkGlyph { UnicodePoint = "f3", UnicodeGroup = "Latin-1 Supplement", Character = "ó", Windows1252UrlEncoding = "%F3", Utf8UrlEncoding = "%C3%B3", HtmlEntityName = "&oacute;", XmlEntityNumber = "&#243;" },
new FrameworkGlyph { UnicodePoint = "f4", UnicodeGroup = "Latin-1 Supplement", Character = "ô", Windows1252UrlEncoding = "%F4", Utf8UrlEncoding = "%C3%B4", HtmlEntityName = "&ocirc;", XmlEntityNumber = "&#244;" },
new FrameworkGlyph { UnicodePoint = "f5", UnicodeGroup = "Latin-1 Supplement", Character = "õ", Windows1252UrlEncoding = "%F5", Utf8UrlEncoding = "%C3%B5", HtmlEntityName = "&otilde;", XmlEntityNumber = "&#245;" },
new FrameworkGlyph { UnicodePoint = "f6", UnicodeGroup = "Latin-1 Supplement", Character = "ö", Windows1252UrlEncoding = "%F6", Utf8UrlEncoding = "%C3%B6", HtmlEntityName = "&ouml;", XmlEntityNumber = "&#246;" },
new FrameworkGlyph { UnicodePoint = "f7", UnicodeGroup = "Latin-1 Supplement", Character = "÷", Windows1252UrlEncoding = "%F7", Utf8UrlEncoding = "%C3%B7", HtmlEntityName = "&divide;", XmlEntityNumber = "&#247;" },
new FrameworkGlyph { UnicodePoint = "f8", UnicodeGroup = "Latin-1 Supplement", Character = "ø", Windows1252UrlEncoding = "%F8", Utf8UrlEncoding = "%C3%B8", HtmlEntityName = "&oslash;", XmlEntityNumber = "&#248;" },
new FrameworkGlyph { UnicodePoint = "f9", UnicodeGroup = "Latin-1 Supplement", Character = "ù", Windows1252UrlEncoding = "%F9", Utf8UrlEncoding = "%C3%B9", HtmlEntityName = "&ugrave;", XmlEntityNumber = "&#249;" },
new FrameworkGlyph { UnicodePoint = "fa", UnicodeGroup = "Latin-1 Supplement", Character = "ú", Windows1252UrlEncoding = "%FA", Utf8UrlEncoding = "%C3%BA", HtmlEntityName = "&uacute;", XmlEntityNumber = "&#250;" },
new FrameworkGlyph { UnicodePoint = "fb", UnicodeGroup = "Latin-1 Supplement", Character = "û", Windows1252UrlEncoding = "%FB", Utf8UrlEncoding = "%C3%BB", HtmlEntityName = "&ucirc;", XmlEntityNumber = "&#251;" },
new FrameworkGlyph { UnicodePoint = "fc", UnicodeGroup = "Latin-1 Supplement", Character = "ü", Windows1252UrlEncoding = "%FC", Utf8UrlEncoding = "%C3%BC", HtmlEntityName = "&uuml;", XmlEntityNumber = "&#252;" },
new FrameworkGlyph { UnicodePoint = "fd", UnicodeGroup = "Latin-1 Supplement", Character = "ý", Windows1252UrlEncoding = "%FD", Utf8UrlEncoding = "%C3%BD", HtmlEntityName = "&yacute;", XmlEntityNumber = "&#253;" },
new FrameworkGlyph { UnicodePoint = "fe", UnicodeGroup = "Latin-1 Supplement", Character = "þ", Windows1252UrlEncoding = "%FE", Utf8UrlEncoding = "%C3%BE", HtmlEntityName = "&thorn;", XmlEntityNumber = "&#254;" },
new FrameworkGlyph { UnicodePoint = "ff", UnicodeGroup = "Latin-1 Supplement", Character = "ÿ", Windows1252UrlEncoding = "%FF", Utf8UrlEncoding = "%C3%BF", HtmlEntityName = "&yuml;", XmlEntityNumber = "&#255;" },
                    }
            );
    }
}
