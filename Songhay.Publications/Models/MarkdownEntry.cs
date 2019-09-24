using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Songhay.Publications.Models
{
    /// <summary>
    /// Defines a conventional, Publication entry
    /// </summary>
    public class MarkdownEntry
    {
        /// <summary>
        /// Defines the new-line convention for entries.
        /// </summary>
        public static string NewLine = Environment.NewLine;

        /// <summary>
        /// Markdown <see cref="FileInfo" />
        /// </summary>
        /// <value></value>
        public FileInfo EntryFileInfo { get; set; }

        /// <summary>
        /// JSON front matter
        /// </summary>
        /// <value></value>
        public JObject FrontMatter { get; set; }

        /// <summary>
        /// Text content
        /// </summary>
        /// <value></value>
        public string Content { get; set; }
    }
}