using System;

namespace Songhay.Models
{
    /// <summary>
    /// Defines a Unicode glyphic character
    /// </summary>
    public class FrameworkGlyph
    {
        /// <summary>
        /// Gets or sets the unicode point.
        /// </summary>
        /// <value>
        /// The unicode point.
        /// </value>
        public string UnicodePoint { get; set; }

        /// <summary>
        /// Gets or sets the unicode integer.
        /// </summary>
        /// <value>
        /// The unicode integer.
        /// </value>
        public int UnicodeInteger { get { return string.IsNullOrWhiteSpace(this.UnicodePoint) ? 0 : Convert.ToInt32(this.UnicodePoint, 16); } }

        /// <summary>
        /// Gets or sets the unicode group.
        /// </summary>
        /// <value>
        /// The unicode group.
        /// </value>
        public string UnicodeGroup { get; set; }

        /// <summary>
        /// Gets or sets the character, usually the Unicode Point.
        /// </summary>
        /// <value>
        /// The character.
        /// </value>
        public string Character { get; set; }

        /// <summary>
        /// Gets or sets the windows1252 URL encoding.
        /// </summary>
        /// <value>
        /// The windows1252 URL encoding.
        /// </value>
        public string Windows1252UrlEncoding { get; set; }

        /// <summary>
        /// Gets or sets the UTF8 URL encoding.
        /// </summary>
        /// <value>
        /// The UTF8 URL encoding.
        /// </value>
        public string Utf8UrlEncoding { get; set; }

        /// <summary>
        /// Gets or sets the name of the HTML entity.
        /// </summary>
        /// <value>
        /// The name of the HTML entity.
        /// </value>
        public string HtmlEntityName { get; set; }

        /// <summary>
        /// Gets or sets the XML entity number.
        /// </summary>
        /// <value>
        /// The XML entity number.
        /// </value>
        public string XmlEntityNumber { get; set; }
    }
}
