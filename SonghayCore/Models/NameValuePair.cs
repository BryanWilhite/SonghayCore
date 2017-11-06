
namespace Songhay.Models
{
    /// <summary>
    /// Defines name-value pair.
    /// </summary>
    /// <remarks>
    /// Consider using the <see cref="System.Collections.Generic.KeyValuePair&lt;TKey, TValue&gt;"/> structure
    /// (from .NET 2.0 onwards) before using this type.
    /// </remarks>
    public class NameValuePair
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Name: {0}, Value: {1}", this.Name ?? "[null]", this.Value ?? "[null]");
        }
    }
}
