
namespace Songhay.Publications.Models
{
    /// <summary>
    /// Associates <see cref="IDocument"/> metadata
    /// with <see cref="IFragment"/> content.
    /// </summary>
    public class DocumentWithContent
    {
        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the document.
        /// </summary>
        /// <value>
        /// The document.
        /// </value>
        public IDocument Document { get; set; }
    }
}
