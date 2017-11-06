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
            if(_glyphs.Count == 0) LatinGlyphs.AddGlyphs();

            foreach(string s in _glyphs.Keys)
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
            if(_glyphs.Count == 0) LatinGlyphs.AddGlyphs();

            foreach(string s in _glyphs.Keys)
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

        private static Dictionary<string, string> _glyphs = new Dictionary<string, string>();
    }
}
