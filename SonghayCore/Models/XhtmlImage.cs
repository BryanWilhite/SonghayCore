using System;

namespace Songhay.Models
{
    /// <summary>
    /// Defines an image used by XHTML(5) clients
    /// </summary>
    public class XhtmlImage
    {
        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public Uri Location { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public string Height { get; set; }

        /// <summary>
        /// Gets or sets the height in pixels.
        /// </summary>
        /// <value>
        /// The height in pixels.
        /// </value>
        public int? HeightInPixels { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public string Width { get; set; }

        /// <summary>
        /// Gets or sets the width in pixels.
        /// </summary>
        /// <value>
        /// The width in pixels.
        /// </value>
        public int? WidthInPixels { get; set; }
    }
}
