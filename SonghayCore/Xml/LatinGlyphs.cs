using System;
using System.Collections.Generic;

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
        /// The <see cref="System.String"/>
        /// containing the decimal entities.
        /// </param>
        /// <returns>
        /// Returns a <see cref="System.String"/>
        /// with Latin glyphs.
        /// </returns>
        public static string Condense(string entityText)
        {
            if (_glyphs.Count == 0) LatinGlyphs.AddGlyphs();

            foreach (string s in _glyphs.Keys)
            {
                entityText = entityText.Replace(_glyphs[s], s);
            }

            //Search for selected named entities:
            entityText = entityText.Replace("&copy;", "©");
            entityText = entityText.Replace("&eacute;", "é");
            entityText = entityText.Replace("&nbsp;", " ");
            entityText = entityText.Replace("&reg;", "®");
            entityText = entityText.Replace("&trade;", "™");

            return entityText;
        }

        /// <summary>
        /// Expands selected Latin glyphs
        /// into their decimal entity equivalent.
        /// </summary>
        /// <param name="glyphText">
        /// The <see cref="System.String"/>
        /// containing the glyphs.
        /// </param>
        /// <returns>
        /// Returns a <see cref="System.String"/>
        /// with decimal entities.
        /// </returns>
        public static string Expand(string glyphText)
        {
            if (_glyphs.Count == 0) LatinGlyphs.AddGlyphs();

            foreach (string s in _glyphs.Keys)
            {
                glyphText = glyphText.Replace(s, _glyphs[s]);
            }
            return glyphText;
        }

        private static void AddGlyphs()
        {
            //Characters 128–159:
            _glyphs.Add("€", "&#8364;");
            _glyphs.Add("ƒ", "&#402;");
            _glyphs.Add("„", "&#8222;");
            _glyphs.Add("…", "&#8230;");
            _glyphs.Add("†", "&#8224;");
            _glyphs.Add("‡", "&#8225;");
            _glyphs.Add("ˆ", "&#710;");
            _glyphs.Add("‰", "&#8240;");
            _glyphs.Add("Š", "&#352;");
            _glyphs.Add("‹", "&#8249;");
            _glyphs.Add("Œ", "&#338;");
            _glyphs.Add("‘", "&#8216;");
            _glyphs.Add("’", "&#8217;");
            _glyphs.Add("“", "&#8220;");
            _glyphs.Add("”", "&#8221;");
            _glyphs.Add("•", "&#8226;");
            _glyphs.Add("–", "&#8211;");
            _glyphs.Add("—", "&#8212;");
            _glyphs.Add("˜", "&#732;");
            _glyphs.Add("™", "&#8482;");
            _glyphs.Add("š", "&#353;");
            _glyphs.Add("›", "&#8250;");
            _glyphs.Add("œ", "&#339;");
            _glyphs.Add("Ÿ", "&#376;");

            //Characters 160–191:
            _glyphs.Add("¡", "&#161;");
            _glyphs.Add("¢", "&#162;");
            _glyphs.Add("£", "&#163;");
            _glyphs.Add("¤", "&#164;");
            _glyphs.Add("¥", "&#165;");
            _glyphs.Add("¦", "&#166;");
            _glyphs.Add("§", "&#167;");
            _glyphs.Add("¨", "&#168;");
            _glyphs.Add("©", "&#169;");
            _glyphs.Add("ª", "&#170;");
            _glyphs.Add("«", "&#171;");
            _glyphs.Add("¬", "&#172;");
            _glyphs.Add("­", "&#173;"); //This looks like a hyphen but is a soft-hyphen.
            _glyphs.Add("®", "&#174;");
            _glyphs.Add("¯", "&#175;");
            _glyphs.Add("°", "&#176;");
            _glyphs.Add("±", "&#177;");
            _glyphs.Add("²", "&#178;");
            _glyphs.Add("³", "&#179;");
            _glyphs.Add("´", "&#180;");
            _glyphs.Add("µ", "&#181;");
            _glyphs.Add("¶", "&#182;");
            _glyphs.Add("·", "&#183;");
            _glyphs.Add("¸", "&#184;");
            _glyphs.Add("¹", "&#185;");
            _glyphs.Add("º", "&#186;");
            _glyphs.Add("»", "&#187;");
            _glyphs.Add("¼", "&#188;");
            _glyphs.Add("½", "&#189;");
            _glyphs.Add("¾", "&#190;");
            _glyphs.Add("¿", "&#191;");

            //Characters 192–223:
            _glyphs.Add("À", "&#192;");
            _glyphs.Add("Á", "&#193;");
            _glyphs.Add("Â", "&#194;");
            _glyphs.Add("Ã", "&#195;");
            _glyphs.Add("Ä", "&#196;");
            _glyphs.Add("Å", "&#197;");
            _glyphs.Add("Æ", "&#198;");
            _glyphs.Add("Ç", "&#199;");
            _glyphs.Add("È", "&#200;");
            _glyphs.Add("É", "&#201;");
            _glyphs.Add("Ê", "&#202;");
            _glyphs.Add("Ë", "&#203;");
            _glyphs.Add("Ì", "&#204;");
            _glyphs.Add("Í", "&#205;");
            _glyphs.Add("Î", "&#206;");
            _glyphs.Add("Ï", "&#207;");
            _glyphs.Add("Ð", "&#208;");
            _glyphs.Add("Ñ", "&#209;");
            _glyphs.Add("Ò", "&#210;");
            _glyphs.Add("Ó", "&#211;");
            _glyphs.Add("Ô", "&#212;");
            _glyphs.Add("Õ", "&#213;");
            _glyphs.Add("Ö", "&#214;");
            _glyphs.Add("×", "&#215;");
            _glyphs.Add("Ø", "&#216;");
            _glyphs.Add("Ù", "&#217;");
            _glyphs.Add("Ú", "&#218;");
            _glyphs.Add("Û", "&#219;");
            _glyphs.Add("Ü", "&#220;");
            _glyphs.Add("Ý", "&#221;");
            _glyphs.Add("Þ", "&#222;");
            _glyphs.Add("ß", "&#223;");

            //Characters 224–255:
            _glyphs.Add("à", "&#224;");
            _glyphs.Add("á", "&#225;");
            _glyphs.Add("â", "&#226;");
            _glyphs.Add("ã", "&#227;");
            _glyphs.Add("ä", "&#228;");
            _glyphs.Add("å", "&#229;");
            _glyphs.Add("æ", "&#230;");
            _glyphs.Add("ç", "&#231;");
            _glyphs.Add("è", "&#232;");
            _glyphs.Add("é", "&#233;");
            _glyphs.Add("ê", "&#234;");
            _glyphs.Add("ë", "&#235;");
            _glyphs.Add("ì", "&#236;");
            _glyphs.Add("í", "&#237;");
            _glyphs.Add("î", "&#238;");
            _glyphs.Add("ï", "&#239;");
            _glyphs.Add("ð", "&#240;");
            _glyphs.Add("ñ", "&#241;");
            _glyphs.Add("ò", "&#242;");
            _glyphs.Add("ó", "&#243;");
            _glyphs.Add("ô", "&#244;");
            _glyphs.Add("õ", "&#245;");
            _glyphs.Add("ö", "&#246;");
            _glyphs.Add("÷", "&#247;");
            _glyphs.Add("ø", "&#248;");
            _glyphs.Add("ù", "&#249;");
            _glyphs.Add("ú", "&#250;");
            _glyphs.Add("û", "&#251;");
            _glyphs.Add("ü", "&#252;");
            _glyphs.Add("ý", "&#253;");
            _glyphs.Add("þ", "&#254;");
            _glyphs.Add("ÿ", "&#255;");
        }

        /// <summary>
        /// The glyphs{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        private static readonly Lazy<
                    Dictionary<
                        string,
                        (
                            string Character,
                            string Windows1252UrlEncoding,
                            string Utf8Encoding,
                            string HtmlEntityName,
                            string XmlEntityNumber)
                        >
                    > _glyphs = new Lazy<
                        Dictionary<
                            string,
                            (
                                string Character,
                                string Windows1252UrlEncoding,
                                string Utf8Encoding,
                                string HtmlEntityName,
                                string XmlEntityNumber)
                            >
                        >(
                            () => new Dictionary<
                                string,
                            (
                                string Character,
                                string Windows1252UrlEncoding,
                                string Utf8Encoding,
                                string HtmlEntityName,
                                string XmlEntityNumber)
                            >
                            {
                        { "20", ( Character: " ", Windows1252UrlEncoding: "%20", Utf8Encoding: "%20", HtmlEntityName: "", XmlEntityNumber: "&#32;" ) },
                        { "21", ( Character: "!", Windows1252UrlEncoding: "%21", Utf8Encoding: "%21", HtmlEntityName: "", XmlEntityNumber: "&#33;" ) },
                        { "22", ( Character: "\"", Windows1252UrlEncoding: "%22", Utf8Encoding: "%22", HtmlEntityName: "&quot;", XmlEntityNumber: "&#34;" ) },
                        { "23", ( Character: "#", Windows1252UrlEncoding: "%23", Utf8Encoding: "%23", HtmlEntityName: "", XmlEntityNumber: "&#35;" ) },
                        { "24", ( Character: "$", Windows1252UrlEncoding: "%24", Utf8Encoding: "%24", HtmlEntityName: "", XmlEntityNumber: "&#36;" ) },
                        { "25", ( Character: "%", Windows1252UrlEncoding: "%25", Utf8Encoding: "%25", HtmlEntityName: "", XmlEntityNumber: "&#37;" ) },
                        { "26", ( Character: "&", Windows1252UrlEncoding: "%26", Utf8Encoding: "%26", HtmlEntityName: "&amp;", XmlEntityNumber: "&#38;" ) },
                        { "27", ( Character: "'", Windows1252UrlEncoding: "%27", Utf8Encoding: "%27", HtmlEntityName: "&apos;", XmlEntityNumber: "&#39;" ) },
                        { "28", ( Character: "(", Windows1252UrlEncoding: "%28", Utf8Encoding: "%28", HtmlEntityName: "", XmlEntityNumber: "&#40;" ) },
{ "29", ( Character: ")", Windows1252UrlEncoding: "%29", Utf8Encoding: "%29", HtmlEntityName: "", XmlEntityNumber: "&#41;" ) },
{ "2a", ( Character: "*", Windows1252UrlEncoding: "%2A", Utf8Encoding: "%2A", HtmlEntityName: "", XmlEntityNumber: "&#42;" ) },
{ "2b", ( Character: "+", Windows1252UrlEncoding: "%2B", Utf8Encoding: "%2B", HtmlEntityName: "", XmlEntityNumber: "&#43;" ) },
{ "2c", ( Character: ",", Windows1252UrlEncoding: "%2C", Utf8Encoding: "%2C", HtmlEntityName: "", XmlEntityNumber: "&#44;" ) },
{ "2d", ( Character: "-", Windows1252UrlEncoding: "%2D", Utf8Encoding: "%2D", HtmlEntityName: "", XmlEntityNumber: "&#45;" ) },
{ "2e", ( Character: ".", Windows1252UrlEncoding: "%2E", Utf8Encoding: "%2E", HtmlEntityName: "", XmlEntityNumber: "&#46;" ) },
{ "2f", ( Character: "/", Windows1252UrlEncoding: "%2F", Utf8Encoding: "%2F", HtmlEntityName: "", XmlEntityNumber: "&#47;" ) },
{ "30", ( Character: "0", Windows1252UrlEncoding: "%30", Utf8Encoding: "%30", HtmlEntityName: "", XmlEntityNumber: "&#48;" ) },
{ "31", ( Character: "1", Windows1252UrlEncoding: "%31", Utf8Encoding: "%31", HtmlEntityName: "", XmlEntityNumber: "&#49;" ) },
{ "32", ( Character: "2", Windows1252UrlEncoding: "%32", Utf8Encoding: "%32", HtmlEntityName: "", XmlEntityNumber: "&#50;" ) },
{ "33", ( Character: "3", Windows1252UrlEncoding: "%33", Utf8Encoding: "%33", HtmlEntityName: "", XmlEntityNumber: "&#51;" ) },
{ "34", ( Character: "4", Windows1252UrlEncoding: "%34", Utf8Encoding: "%34", HtmlEntityName: "", XmlEntityNumber: "&#52;" ) },
{ "35", ( Character: "5", Windows1252UrlEncoding: "%35", Utf8Encoding: "%35", HtmlEntityName: "", XmlEntityNumber: "&#53;" ) },
{ "36", ( Character: "6", Windows1252UrlEncoding: "%36", Utf8Encoding: "%36", HtmlEntityName: "", XmlEntityNumber: "&#54;" ) },
{ "37", ( Character: "7", Windows1252UrlEncoding: "%37", Utf8Encoding: "%37", HtmlEntityName: "", XmlEntityNumber: "&#55;" ) },
{ "38", ( Character: "8", Windows1252UrlEncoding: "%38", Utf8Encoding: "%38", HtmlEntityName: "", XmlEntityNumber: "&#56;" ) },
{ "39", ( Character: "9", Windows1252UrlEncoding: "%39", Utf8Encoding: "%39", HtmlEntityName: "", XmlEntityNumber: "&#57;" ) },
{ "3a", ( Character: ":", Windows1252UrlEncoding: "%3A", Utf8Encoding: "%3A", HtmlEntityName: "", XmlEntityNumber: "&#58;" ) },
{ "3b", ( Character: ";", Windows1252UrlEncoding: "%3B", Utf8Encoding: "%3B", HtmlEntityName: "", XmlEntityNumber: "&#59;" ) },
{ "3c", ( Character: "<", Windows1252UrlEncoding: "%3C", Utf8Encoding: "%3C", HtmlEntityName: "&lt;", XmlEntityNumber: "&#60;" ) },
{ "3d", ( Character: "=", Windows1252UrlEncoding: "%3D", Utf8Encoding: "%3D", HtmlEntityName: "", XmlEntityNumber: "&#61;" ) },
{ "3e", ( Character: ">", Windows1252UrlEncoding: "%3E", Utf8Encoding: "%3E", HtmlEntityName: "&gt;", XmlEntityNumber: "&#62;" ) },
{ "3f", ( Character: "?", Windows1252UrlEncoding: "%3F", Utf8Encoding: "%3F", HtmlEntityName: "", XmlEntityNumber: "&#63;" ) },
{ "40", ( Character: "@", Windows1252UrlEncoding: "%40", Utf8Encoding: "%40", HtmlEntityName: "", XmlEntityNumber: "&#64;" ) },
{ "41", ( Character: "A", Windows1252UrlEncoding: "%41", Utf8Encoding: "%41", HtmlEntityName: "", XmlEntityNumber: "&#65;" ) },
{ "42", ( Character: "B", Windows1252UrlEncoding: "%42", Utf8Encoding: "%42", HtmlEntityName: "", XmlEntityNumber: "&#66;" ) },
{ "43", ( Character: "C", Windows1252UrlEncoding: "%43", Utf8Encoding: "%43", HtmlEntityName: "", XmlEntityNumber: "&#67;" ) },
{ "44", ( Character: "D", Windows1252UrlEncoding: "%44", Utf8Encoding: "%44", HtmlEntityName: "", XmlEntityNumber: "&#68;" ) },
{ "45", ( Character: "E", Windows1252UrlEncoding: "%45", Utf8Encoding: "%45", HtmlEntityName: "", XmlEntityNumber: "&#69;" ) },
{ "46", ( Character: "F", Windows1252UrlEncoding: "%46", Utf8Encoding: "%46", HtmlEntityName: "", XmlEntityNumber: "&#70;" ) },
{ "47", ( Character: "G", Windows1252UrlEncoding: "%47", Utf8Encoding: "%47", HtmlEntityName: "", XmlEntityNumber: "&#71;" ) },
{ "48", ( Character: "H", Windows1252UrlEncoding: "%48", Utf8Encoding: "%48", HtmlEntityName: "", XmlEntityNumber: "&#72;" ) },
{ "49", ( Character: "I", Windows1252UrlEncoding: "%49", Utf8Encoding: "%49", HtmlEntityName: "", XmlEntityNumber: "&#73;" ) },
{ "4a", ( Character: "J", Windows1252UrlEncoding: "%4A", Utf8Encoding: "%4A", HtmlEntityName: "", XmlEntityNumber: "&#74;" ) },
{ "4b", ( Character: "K", Windows1252UrlEncoding: "%4B", Utf8Encoding: "%4B", HtmlEntityName: "", XmlEntityNumber: "&#75;" ) },
{ "4c", ( Character: "L", Windows1252UrlEncoding: "%4C", Utf8Encoding: "%4C", HtmlEntityName: "", XmlEntityNumber: "&#76;" ) },
{ "4d", ( Character: "M", Windows1252UrlEncoding: "%4D", Utf8Encoding: "%4D", HtmlEntityName: "", XmlEntityNumber: "&#77;" ) },
{ "4e", ( Character: "N", Windows1252UrlEncoding: "%4E", Utf8Encoding: "%4E", HtmlEntityName: "", XmlEntityNumber: "&#78;" ) },
{ "4f", ( Character: "O", Windows1252UrlEncoding: "%4F", Utf8Encoding: "%4F", HtmlEntityName: "", XmlEntityNumber: "&#79;" ) },
{ "50", ( Character: "P", Windows1252UrlEncoding: "%50", Utf8Encoding: "%50", HtmlEntityName: "", XmlEntityNumber: "&#80;" ) },
{ "51", ( Character: "Q", Windows1252UrlEncoding: "%51", Utf8Encoding: "%51", HtmlEntityName: "", XmlEntityNumber: "&#81;" ) },
{ "52", ( Character: "R", Windows1252UrlEncoding: "%52", Utf8Encoding: "%52", HtmlEntityName: "", XmlEntityNumber: "&#82;" ) },
{ "53", ( Character: "S", Windows1252UrlEncoding: "%53", Utf8Encoding: "%53", HtmlEntityName: "", XmlEntityNumber: "&#83;" ) },
{ "54", ( Character: "T", Windows1252UrlEncoding: "%54", Utf8Encoding: "%54", HtmlEntityName: "", XmlEntityNumber: "&#84;" ) },
{ "55", ( Character: "U", Windows1252UrlEncoding: "%55", Utf8Encoding: "%55", HtmlEntityName: "", XmlEntityNumber: "&#85;" ) },
{ "56", ( Character: "V", Windows1252UrlEncoding: "%56", Utf8Encoding: "%56", HtmlEntityName: "", XmlEntityNumber: "&#86;" ) },
{ "57", ( Character: "W", Windows1252UrlEncoding: "%57", Utf8Encoding: "%57", HtmlEntityName: "", XmlEntityNumber: "&#87;" ) },
{ "58", ( Character: "X", Windows1252UrlEncoding: "%58", Utf8Encoding: "%58", HtmlEntityName: "", XmlEntityNumber: "&#88;" ) },
{ "59", ( Character: "Y", Windows1252UrlEncoding: "%59", Utf8Encoding: "%59", HtmlEntityName: "", XmlEntityNumber: "&#89;" ) },
{ "5a", ( Character: "Z", Windows1252UrlEncoding: "%5A", Utf8Encoding: "%5A", HtmlEntityName: "", XmlEntityNumber: "&#90;" ) },
{ "5b", ( Character: "[", Windows1252UrlEncoding: "%5B", Utf8Encoding: "%5B", HtmlEntityName: "", XmlEntityNumber: "&#91;" ) },
{ "5c", ( Character: "\\", Windows1252UrlEncoding: "%5C", Utf8Encoding: "%5C", HtmlEntityName: "", XmlEntityNumber: "&#92;" ) },
{ "5d", ( Character: "]", Windows1252UrlEncoding: "%5D", Utf8Encoding: "%5D", HtmlEntityName: "", XmlEntityNumber: "&#93;" ) },
{ "5e", ( Character: "^", Windows1252UrlEncoding: "%5E", Utf8Encoding: "%5E", HtmlEntityName: "", XmlEntityNumber: "&#94;" ) },
{ "5f", ( Character: "_", Windows1252UrlEncoding: "%5F", Utf8Encoding: "%5F", HtmlEntityName: "", XmlEntityNumber: "&#95;" ) },
{ "60", ( Character: "`", Windows1252UrlEncoding: "%60", Utf8Encoding: "%60", HtmlEntityName: "", XmlEntityNumber: "&#96;" ) },
{ "61", ( Character: "a", Windows1252UrlEncoding: "%61", Utf8Encoding: "%61", HtmlEntityName: "", XmlEntityNumber: "&#97;" ) },
{ "62", ( Character: "b", Windows1252UrlEncoding: "%62", Utf8Encoding: "%62", HtmlEntityName: "", XmlEntityNumber: "&#98;" ) },
{ "63", ( Character: "c", Windows1252UrlEncoding: "%63", Utf8Encoding: "%63", HtmlEntityName: "", XmlEntityNumber: "&#99;" ) },
{ "64", ( Character: "d", Windows1252UrlEncoding: "%64", Utf8Encoding: "%64", HtmlEntityName: "", XmlEntityNumber: "&#100;" ) },
{ "65", ( Character: "e", Windows1252UrlEncoding: "%65", Utf8Encoding: "%65", HtmlEntityName: "", XmlEntityNumber: "&#101;" ) },
{ "66", ( Character: "f", Windows1252UrlEncoding: "%66", Utf8Encoding: "%66", HtmlEntityName: "", XmlEntityNumber: "&#102;" ) },
{ "67", ( Character: "g", Windows1252UrlEncoding: "%67", Utf8Encoding: "%67", HtmlEntityName: "", XmlEntityNumber: "&#103;" ) },
{ "68", ( Character: "h", Windows1252UrlEncoding: "%68", Utf8Encoding: "%68", HtmlEntityName: "", XmlEntityNumber: "&#104;" ) },
{ "69", ( Character: "i", Windows1252UrlEncoding: "%69", Utf8Encoding: "%69", HtmlEntityName: "", XmlEntityNumber: "&#105;" ) },
{ "6a", ( Character: "j", Windows1252UrlEncoding: "%6A", Utf8Encoding: "%6A", HtmlEntityName: "", XmlEntityNumber: "&#106;" ) },
{ "6b", ( Character: "k", Windows1252UrlEncoding: "%6B", Utf8Encoding: "%6B", HtmlEntityName: "", XmlEntityNumber: "&#107;" ) },
{ "6c", ( Character: "l", Windows1252UrlEncoding: "%6C", Utf8Encoding: "%6C", HtmlEntityName: "", XmlEntityNumber: "&#108;" ) },
{ "6d", ( Character: "m", Windows1252UrlEncoding: "%6D", Utf8Encoding: "%6D", HtmlEntityName: "", XmlEntityNumber: "&#109;" ) },
{ "6e", ( Character: "n", Windows1252UrlEncoding: "%6E", Utf8Encoding: "%6E", HtmlEntityName: "", XmlEntityNumber: "&#110;" ) },
{ "6f", ( Character: "o", Windows1252UrlEncoding: "%6F", Utf8Encoding: "%6F", HtmlEntityName: "", XmlEntityNumber: "&#111;" ) },
{ "70", ( Character: "p", Windows1252UrlEncoding: "%70", Utf8Encoding: "%70", HtmlEntityName: "", XmlEntityNumber: "&#112;" ) },
{ "71", ( Character: "q", Windows1252UrlEncoding: "%71", Utf8Encoding: "%71", HtmlEntityName: "", XmlEntityNumber: "&#113;" ) },
{ "72", ( Character: "r", Windows1252UrlEncoding: "%72", Utf8Encoding: "%72", HtmlEntityName: "", XmlEntityNumber: "&#114;" ) },
{ "73", ( Character: "s", Windows1252UrlEncoding: "%73", Utf8Encoding: "%73", HtmlEntityName: "", XmlEntityNumber: "&#115;" ) },
{ "74", ( Character: "t", Windows1252UrlEncoding: "%74", Utf8Encoding: "%74", HtmlEntityName: "", XmlEntityNumber: "&#116;" ) },
{ "75", ( Character: "u", Windows1252UrlEncoding: "%75", Utf8Encoding: "%75", HtmlEntityName: "", XmlEntityNumber: "&#117;" ) },
{ "76", ( Character: "v", Windows1252UrlEncoding: "%76", Utf8Encoding: "%76", HtmlEntityName: "", XmlEntityNumber: "&#118;" ) },
{ "77", ( Character: "w", Windows1252UrlEncoding: "%77", Utf8Encoding: "%77", HtmlEntityName: "", XmlEntityNumber: "&#119;" ) },
{ "78", ( Character: "x", Windows1252UrlEncoding: "%78", Utf8Encoding: "%78", HtmlEntityName: "", XmlEntityNumber: "&#120;" ) },
{ "79", ( Character: "y", Windows1252UrlEncoding: "%79", Utf8Encoding: "%79", HtmlEntityName: "", XmlEntityNumber: "&#121;" ) },
{ "7a", ( Character: "z", Windows1252UrlEncoding: "%7A", Utf8Encoding: "%7A", HtmlEntityName: "", XmlEntityNumber: "&#122;" ) },
{ "7b", ( Character: "{", Windows1252UrlEncoding: "%7B", Utf8Encoding: "%7B", HtmlEntityName: "", XmlEntityNumber: "&#123;" ) },
{ "7c", ( Character: "|", Windows1252UrlEncoding: "%7C", Utf8Encoding: "%7C", HtmlEntityName: "", XmlEntityNumber: "&#124;" ) },
{ "7d", ( Character: "}", Windows1252UrlEncoding: "%7D", Utf8Encoding: "%7D", HtmlEntityName: "", XmlEntityNumber: "&#125;" ) },
{ "7e", ( Character: "~", Windows1252UrlEncoding: "%7E", Utf8Encoding: "%7E", HtmlEntityName: "", XmlEntityNumber: "&#126;" ) },
{ "7f", ( Character: "", Windows1252UrlEncoding: "%7F", Utf8Encoding: "%7F", HtmlEntityName: "", XmlEntityNumber: "&#127;" ) },
{ "80", ( Character: "`", Windows1252UrlEncoding: "%80", Utf8Encoding: "%E2%82%AC", HtmlEntityName: "", XmlEntityNumber: "&#128;" ) },
{ "81", ( Character: "", Windows1252UrlEncoding: "%81", Utf8Encoding: "%81", HtmlEntityName: "", XmlEntityNumber: "&#129;" ) },
{ "201a", ( Character: "‚", Windows1252UrlEncoding: "%82", Utf8Encoding: "%E2%80%9A", HtmlEntityName: "&sbquo;", XmlEntityNumber: "&#8218;" ) },
{ "192", ( Character: "ƒ", Windows1252UrlEncoding: "%83", Utf8Encoding: "%C6%92", HtmlEntityName: "&fnof;", XmlEntityNumber: "&#402;" ) },
{ "201e", ( Character: "„", Windows1252UrlEncoding: "%84", Utf8Encoding: "%E2%80%9E", HtmlEntityName: "&bdquo;", XmlEntityNumber: "&#8222;" ) },
{ "2026", ( Character: "…", Windows1252UrlEncoding: "%85", Utf8Encoding: "%E2%80%A6", HtmlEntityName: "&hellip;", XmlEntityNumber: "&#8230;" ) },
{ "2020", ( Character: "†", Windows1252UrlEncoding: "%86", Utf8Encoding: "%E2%80%A0", HtmlEntityName: "&dagger;", XmlEntityNumber: "&#8224;" ) },
{ "2021", ( Character: "‡", Windows1252UrlEncoding: "%87", Utf8Encoding: "%E2%80%A1", HtmlEntityName: "&Dagger;", XmlEntityNumber: "&#8225;" ) },
{ "2c6", ( Character: "ˆ", Windows1252UrlEncoding: "%88", Utf8Encoding: "%CB%86", HtmlEntityName: "&circ;", XmlEntityNumber: "&#710;" ) },
{ "2030", ( Character: "‰", Windows1252UrlEncoding: "%89", Utf8Encoding: "%E2%80%B0", HtmlEntityName: "&permil;", XmlEntityNumber: "&#8240;" ) },
{ "160", ( Character: "Š", Windows1252UrlEncoding: "%8A", Utf8Encoding: "%C5%A0", HtmlEntityName: "&Scaron;", XmlEntityNumber: "&#352;" ) },
{ "2039", ( Character: "‹", Windows1252UrlEncoding: "%8B", Utf8Encoding: "%E2%80%B9", HtmlEntityName: "&lsaquo;", XmlEntityNumber: "&#8249;" ) },
{ "152", ( Character: "Œ", Windows1252UrlEncoding: "%8C", Utf8Encoding: "%C5%92", HtmlEntityName: "&OElig;", XmlEntityNumber: "&#338;" ) },
{ "8d", ( Character: "", Windows1252UrlEncoding: "%8D", Utf8Encoding: "%C5%8D", HtmlEntityName: "", XmlEntityNumber: "&#141;" ) },
{ "17d", ( Character: "Ž", Windows1252UrlEncoding: "%8E", Utf8Encoding: "%C5%BD", HtmlEntityName: "", XmlEntityNumber: "&#381;" ) },
{ "8f", ( Character: "", Windows1252UrlEncoding: "%8F", Utf8Encoding: "%8F", HtmlEntityName: "", XmlEntityNumber: "&#143;" ) },
{ "90", ( Character: "", Windows1252UrlEncoding: "%90", Utf8Encoding: "%C2%90", HtmlEntityName: "", XmlEntityNumber: "&#144;" ) },
{ "2018", ( Character: "‘", Windows1252UrlEncoding: "%91", Utf8Encoding: "%E2%80%98", HtmlEntityName: "&lsquo;", XmlEntityNumber: "&#8216;" ) },
{ "2019", ( Character: "’", Windows1252UrlEncoding: "%92", Utf8Encoding: "%E2%80%99", HtmlEntityName: "&rsquo;", XmlEntityNumber: "&#8217;" ) },
{ "201c", ( Character: "“", Windows1252UrlEncoding: "%93", Utf8Encoding: "%E2%80%9C", HtmlEntityName: "&ldquo;", XmlEntityNumber: "&#8220;" ) },
{ "201d", ( Character: "”", Windows1252UrlEncoding: "%94", Utf8Encoding: "%E2%80%9D", HtmlEntityName: "&rdquo;", XmlEntityNumber: "&#8221;" ) },
{ "2022", ( Character: "•", Windows1252UrlEncoding: "%95", Utf8Encoding: "%E2%80%A2", HtmlEntityName: "&bull;", XmlEntityNumber: "&#8226;" ) },
{ "2013", ( Character: "–", Windows1252UrlEncoding: "%96", Utf8Encoding: "%E2%80%93", HtmlEntityName: "&ndash;", XmlEntityNumber: "&#8211;" ) },
{ "2014", ( Character: "—", Windows1252UrlEncoding: "%97", Utf8Encoding: "%E2%80%94", HtmlEntityName: "&mdash;", XmlEntityNumber: "&#8212;" ) },
{ "2dc", ( Character: "˜", Windows1252UrlEncoding: "%98", Utf8Encoding: "%CB%9C", HtmlEntityName: "&tilde;", XmlEntityNumber: "&#732;" ) },
{ "2122", ( Character: "™", Windows1252UrlEncoding: "%99", Utf8Encoding: "%E2%84", HtmlEntityName: "	&trade;", XmlEntityNumber: "&#8482;" ) },
{ "161", ( Character: "š", Windows1252UrlEncoding: "%9A", Utf8Encoding: "%C5%A1", HtmlEntityName: "	&scaron;", XmlEntityNumber: "&#353;" ) },
{ "203a", ( Character: "›", Windows1252UrlEncoding: "%9B", Utf8Encoding: "%E2%80", HtmlEntityName: "&rsaquo;", XmlEntityNumber: "&#8250;" ) },
{ "153", ( Character: "œ", Windows1252UrlEncoding: "%9C", Utf8Encoding: "%C5%93", HtmlEntityName: "	&oelig;", XmlEntityNumber: "&#339;" ) },
{ "9d", ( Character: "", Windows1252UrlEncoding: "%9D", Utf8Encoding: "%9D", HtmlEntityName: "", XmlEntityNumber: "&#157;" ) },
{ "17e", ( Character: "ž", Windows1252UrlEncoding: "%9E", Utf8Encoding: "%C5%BE", HtmlEntityName: "", XmlEntityNumber: "&#382;" ) },
{ "178", ( Character: "Ÿ", Windows1252UrlEncoding: "%9F", Utf8Encoding: "%C5%B8", HtmlEntityName: "	&Yuml;", XmlEntityNumber: "&#376;" ) },
{ "a0", ( Character: "", Windows1252UrlEncoding: "%A0", Utf8Encoding: "%C2%A0", HtmlEntityName: "&nbsp;", XmlEntityNumber: "&#160;" ) },
{ "a1", ( Character: "¡", Windows1252UrlEncoding: "%A1", Utf8Encoding: "%C2%A1", HtmlEntityName: "&iexcl;", XmlEntityNumber: "&#161;" ) },
{ "a2", ( Character: "¢", Windows1252UrlEncoding: "%A2", Utf8Encoding: "%C2%A2", HtmlEntityName: "&cent;", XmlEntityNumber: "&#162;" ) },
{ "a3", ( Character: "£", Windows1252UrlEncoding: "%A3", Utf8Encoding: "%C2%A3", HtmlEntityName: "&pound;", XmlEntityNumber: "&#163;" ) },
{ "a4", ( Character: "¤", Windows1252UrlEncoding: "%A4", Utf8Encoding: "%C2%A4", HtmlEntityName: "&curren;", XmlEntityNumber: "&#164;" ) },
{ "a5", ( Character: "¥", Windows1252UrlEncoding: "%A5", Utf8Encoding: "%C2%A5", HtmlEntityName: "&yen;", XmlEntityNumber: "&#165;" ) },
{ "a6", ( Character: "¦", Windows1252UrlEncoding: "%A6", Utf8Encoding: "%C2%A6", HtmlEntityName: "&brvbar;", XmlEntityNumber: "&#166;" ) },
{ "a7", ( Character: "§", Windows1252UrlEncoding: "%A7", Utf8Encoding: "%C2%A7", HtmlEntityName: "&sect;", XmlEntityNumber: "&#167;" ) },
{ "a8", ( Character: "¨", Windows1252UrlEncoding: "%A8", Utf8Encoding: "%C2%A8", HtmlEntityName: "&uml;", XmlEntityNumber: "&#168;" ) },
{ "a9", ( Character: "©", Windows1252UrlEncoding: "%A9", Utf8Encoding: "%C2%A9", HtmlEntityName: "&copy;", XmlEntityNumber: "&#169;" ) },
{ "aa", ( Character: "ª", Windows1252UrlEncoding: "%AA", Utf8Encoding: "%C2%AA", HtmlEntityName: "&ordf;", XmlEntityNumber: "&#170;" ) },
{ "ab", ( Character: "«", Windows1252UrlEncoding: "%AB", Utf8Encoding: "%C2%AB", HtmlEntityName: "&laquo;", XmlEntityNumber: "&#171;" ) },
{ "ac", ( Character: "¬", Windows1252UrlEncoding: "%AC", Utf8Encoding: "%C2%AC", HtmlEntityName: "&not;", XmlEntityNumber: "&#172;" ) },
{ "ad", ( Character: "­", Windows1252UrlEncoding: "%AD", Utf8Encoding: "%C2%AD", HtmlEntityName: "&shy;", XmlEntityNumber: "&#173;" ) },
{ "ae", ( Character: "®", Windows1252UrlEncoding: "%AE", Utf8Encoding: "%C2%AE", HtmlEntityName: "&reg;", XmlEntityNumber: "&#174;" ) },
{ "af", ( Character: "¯", Windows1252UrlEncoding: "%AF", Utf8Encoding: "%C2%AF", HtmlEntityName: "&macr;", XmlEntityNumber: "&#175;" ) },
{ "b0", ( Character: "°", Windows1252UrlEncoding: "%B0", Utf8Encoding: "%C2%B0", HtmlEntityName: "&deg;", XmlEntityNumber: "&#176;" ) },
{ "b1", ( Character: "±", Windows1252UrlEncoding: "%B1", Utf8Encoding: "%C2%B1", HtmlEntityName: "&plusmn;", XmlEntityNumber: "&#177;" ) },
{ "b2", ( Character: "²", Windows1252UrlEncoding: "%B2", Utf8Encoding: "%C2%B2", HtmlEntityName: "&sup2;", XmlEntityNumber: "&#178;" ) },
{ "b3", ( Character: "³", Windows1252UrlEncoding: "%B3", Utf8Encoding: "%C2%B3", HtmlEntityName: "&sup3;", XmlEntityNumber: "&#179;" ) },
{ "b4", ( Character: "´", Windows1252UrlEncoding: "%B4", Utf8Encoding: "%C2%B4", HtmlEntityName: "&acute;", XmlEntityNumber: "&#180;" ) },
{ "b5", ( Character: "µ", Windows1252UrlEncoding: "%B5", Utf8Encoding: "%C2%B5", HtmlEntityName: "&micro;", XmlEntityNumber: "&#181;" ) },
{ "b6", ( Character: "¶", Windows1252UrlEncoding: "%B6", Utf8Encoding: "%C2%B6", HtmlEntityName: "&para;", XmlEntityNumber: "&#182;" ) },
{ "b7", ( Character: "·", Windows1252UrlEncoding: "%B7", Utf8Encoding: "%C2%B7", HtmlEntityName: "&middot;", XmlEntityNumber: "&#183;" ) },
{ "b8", ( Character: "¸", Windows1252UrlEncoding: "%B8", Utf8Encoding: "%C2%B8", HtmlEntityName: "&cedil;", XmlEntityNumber: "&#184;" ) },
{ "b9", ( Character: "¹", Windows1252UrlEncoding: "%B9", Utf8Encoding: "%C2%B9", HtmlEntityName: "&sup1;", XmlEntityNumber: "&#185;" ) },
{ "ba", ( Character: "º", Windows1252UrlEncoding: "%BA", Utf8Encoding: "%C2%BA", HtmlEntityName: "&ordm;", XmlEntityNumber: "&#186;" ) },
{ "bb", ( Character: "»", Windows1252UrlEncoding: "%BB", Utf8Encoding: "%C2%BB", HtmlEntityName: "&raquo;", XmlEntityNumber: "&#187;" ) },
{ "bc", ( Character: "¼", Windows1252UrlEncoding: "%BC", Utf8Encoding: "%C2%BC", HtmlEntityName: "&frac14;", XmlEntityNumber: "&#188;" ) },
{ "bd", ( Character: "½", Windows1252UrlEncoding: "%BD", Utf8Encoding: "%C2%BD", HtmlEntityName: "&frac12;", XmlEntityNumber: "&#189;" ) },
{ "be", ( Character: "¾", Windows1252UrlEncoding: "%BE", Utf8Encoding: "%C2%BE", HtmlEntityName: "&frac34;", XmlEntityNumber: "&#190;" ) },
{ "bf", ( Character: "¿", Windows1252UrlEncoding: "%BF", Utf8Encoding: "%C2%BF", HtmlEntityName: "&iquest;", XmlEntityNumber: "&#191;" ) },
{ "c0", ( Character: "À", Windows1252UrlEncoding: "%C0", Utf8Encoding: "%C3%80", HtmlEntityName: "&Agrave;", XmlEntityNumber: "&#192;" ) },
{ "c1", ( Character: "Á", Windows1252UrlEncoding: "%C1", Utf8Encoding: "%C3%81", HtmlEntityName: "&Aacute;", XmlEntityNumber: "&#193;" ) },
{ "c2", ( Character: "Â", Windows1252UrlEncoding: "%C2", Utf8Encoding: "%C3%82", HtmlEntityName: "&Acirc;", XmlEntityNumber: "&#194;" ) },
{ "c3", ( Character: "Ã", Windows1252UrlEncoding: "%C3", Utf8Encoding: "%C3%83", HtmlEntityName: "&Atilde;", XmlEntityNumber: "&#195;" ) },
{ "c4", ( Character: "Ä", Windows1252UrlEncoding: "%C4", Utf8Encoding: "%C3%84", HtmlEntityName: "&Auml;", XmlEntityNumber: "&#196;" ) },
{ "c5", ( Character: "Å", Windows1252UrlEncoding: "%C5", Utf8Encoding: "%C3%85", HtmlEntityName: "&Aring;", XmlEntityNumber: "&#197;" ) },
{ "c6", ( Character: "Æ", Windows1252UrlEncoding: "%C6", Utf8Encoding: "%C3%86", HtmlEntityName: "&AElig;", XmlEntityNumber: "&#198;" ) },
{ "c7", ( Character: "Ç", Windows1252UrlEncoding: "%C7", Utf8Encoding: "%C3%87", HtmlEntityName: "&Ccedil;", XmlEntityNumber: "&#199;" ) },
{ "c8", ( Character: "È", Windows1252UrlEncoding: "%C8", Utf8Encoding: "%C3%88", HtmlEntityName: "&Egrave;", XmlEntityNumber: "&#200;" ) },
{ "c9", ( Character: "É", Windows1252UrlEncoding: "%C9", Utf8Encoding: "%C3%89", HtmlEntityName: "&Eacute;", XmlEntityNumber: "&#201;" ) },
{ "ca", ( Character: "Ê", Windows1252UrlEncoding: "%CA", Utf8Encoding: "%C3%8A", HtmlEntityName: "&Ecirc;", XmlEntityNumber: "&#202;" ) },
{ "cb", ( Character: "Ë", Windows1252UrlEncoding: "%CB", Utf8Encoding: "%C3%8B", HtmlEntityName: "&Euml;", XmlEntityNumber: "&#203;" ) },
{ "cc", ( Character: "Ì", Windows1252UrlEncoding: "%CC", Utf8Encoding: "%C3%8C", HtmlEntityName: "&Igrave;", XmlEntityNumber: "&#204;" ) },
{ "cd", ( Character: "Í", Windows1252UrlEncoding: "%CD", Utf8Encoding: "%C3%8D", HtmlEntityName: "&Iacute;", XmlEntityNumber: "&#205;" ) },
{ "ce", ( Character: "Î", Windows1252UrlEncoding: "%CE", Utf8Encoding: "%C3%8E", HtmlEntityName: "&Icirc;", XmlEntityNumber: "&#206;" ) },
{ "cf", ( Character: "Ï", Windows1252UrlEncoding: "%CF", Utf8Encoding: "%C3%8F", HtmlEntityName: "&Iuml;", XmlEntityNumber: "&#207;" ) },
{ "d0", ( Character: "Ð", Windows1252UrlEncoding: "%D0", Utf8Encoding: "%C3%90", HtmlEntityName: "&ETH;", XmlEntityNumber: "&#208;" ) },
{ "d1", ( Character: "Ñ", Windows1252UrlEncoding: "%D1", Utf8Encoding: "%C3%91", HtmlEntityName: "&Ntilde;", XmlEntityNumber: "&#209;" ) },
{ "d2", ( Character: "Ò", Windows1252UrlEncoding: "%D2", Utf8Encoding: "%C3%92", HtmlEntityName: "&Ograve;", XmlEntityNumber: "&#210;" ) },
{ "d3", ( Character: "Ó", Windows1252UrlEncoding: "%D3", Utf8Encoding: "%C3%93", HtmlEntityName: "&Oacute;", XmlEntityNumber: "&#211;" ) },
{ "d4", ( Character: "Ô", Windows1252UrlEncoding: "%D4", Utf8Encoding: "%C3%94", HtmlEntityName: "&Ocirc;", XmlEntityNumber: "&#212;" ) },
{ "d5", ( Character: "Õ", Windows1252UrlEncoding: "%D5", Utf8Encoding: "%C3%95", HtmlEntityName: "&Otilde;", XmlEntityNumber: "&#213;" ) },
{ "d6", ( Character: "Ö", Windows1252UrlEncoding: "%D6", Utf8Encoding: "%C3%96", HtmlEntityName: "&Ouml;", XmlEntityNumber: "&#214;" ) },
{ "d7", ( Character: "×", Windows1252UrlEncoding: "%D7", Utf8Encoding: "%C3%97", HtmlEntityName: "&times;", XmlEntityNumber: "&#215;" ) },
{ "d8", ( Character: "Ø", Windows1252UrlEncoding: "%D8", Utf8Encoding: "%C3%98", HtmlEntityName: "&Oslash;", XmlEntityNumber: "&#216;" ) },
{ "d9", ( Character: "Ù", Windows1252UrlEncoding: "%D9", Utf8Encoding: "%C3%99", HtmlEntityName: "&Ugrave;", XmlEntityNumber: "&#217;" ) },
{ "da", ( Character: "Ú",  Windows1252UrlEncoding: "%DA", Utf8Encoding: "%C3%9A", HtmlEntityName: "&Uacute;", XmlEntityNumber: "&#218;" ) },
{ "db", ( Character: "Û", Windows1252UrlEncoding: "%DB", Utf8Encoding: "%C3%9B", HtmlEntityName: "&Ucirc;", XmlEntityNumber: "&#219;" ) },
{ "dc", ( Character: "Ü", Windows1252UrlEncoding: "%DC", Utf8Encoding: "%C3%9C", HtmlEntityName: "&Uuml;", XmlEntityNumber: "&#220;" ) },
{ "dd", ( Character: "Ý", Windows1252UrlEncoding: "%DD", Utf8Encoding: "%C3%9D", HtmlEntityName: "&Yacute;", XmlEntityNumber: "&#221;" ) },
{ "de", ( Character: "Þ", Windows1252UrlEncoding: "%DE", Utf8Encoding: "%C3%9E", HtmlEntityName: "&THORN;", XmlEntityNumber: "&#222;" ) },
{ "df", ( Character: "ß", Windows1252UrlEncoding: "%DF", Utf8Encoding: "%C3%9F", HtmlEntityName: "&szlig;", XmlEntityNumber: "&#223;" ) },
{ "e0", ( Character: "à", Windows1252UrlEncoding: "%E0", Utf8Encoding: "%C3%A0", HtmlEntityName: "&agrave;", XmlEntityNumber: "&#224;") },

                    }
                );
    }
}
