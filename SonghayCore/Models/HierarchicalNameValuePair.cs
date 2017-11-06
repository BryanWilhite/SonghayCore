using System.Diagnostics.CodeAnalysis;

namespace Songhay.Models
{
    /// <summary>
    /// Defines hierarchical name-value pairs.
    /// </summary>
    public class HierarchicalNameValuePair : NameValuePair
    {
        /// <summary>
        /// Gets or sets the name value pairs.
        /// </summary>
        /// <value>The name value pairs.</value>
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays",
            Justification = "Used for XML serialization.")]
        public NameValuePair[] NameValuePairs { get; set; }
    }
}
