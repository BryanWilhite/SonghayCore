using System.Text;

namespace Songhay.Models
{
    /// <summary>
    /// Defines DBMS metadata
    /// </summary>
    public class DbmsMetadata
    {
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the connection string key.
        /// </summary>
        /// <value>
        /// The connection string key.
        /// </value>
        public string ConnectionStringKey { get; set; }

        /// <summary>
        /// Gets or sets the encryption metadata.
        /// </summary>
        /// <value>
        /// The encryption metadata.
        /// </value>
        public EncryptionMetadata EncryptionMetadata { get; set; }

        /// <summary>
        /// Gets or sets the name of the provider.
        /// </summary>
        /// <value>
        /// The name of the provider.
        /// </value>
        public string ProviderName { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(this.ProviderName)) sb.Append($"{nameof(this.ProviderName)}: {this.ProviderName} | ");
            if (!string.IsNullOrWhiteSpace(this.ConnectionString)) sb.Append($"{nameof(this.ConnectionString)}: {this.ConnectionString.Substring(0, 72)}... ");
            if (this.EncryptionMetadata != null) sb.Append("| has encryption metadata");

            return (sb.Length > 0) ? sb.ToString() : base.ToString();
        }
    }
}
