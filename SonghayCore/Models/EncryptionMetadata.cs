namespace Songhay.Models
{
    /// <summary>
    /// Defines encryption metadata for persistent storage.
    /// </summary>
    public class EncryptionMetadata
    {
        /// <summary>
        /// Gets or sets the initial vector.
        /// </summary>
        /// <value>
        /// The initial vector.
        /// </value>
        public string InitialVector { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public string Key { get; set; }
    }
}
