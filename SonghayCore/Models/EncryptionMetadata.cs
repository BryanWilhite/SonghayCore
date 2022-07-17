namespace Songhay.Models;

/// <summary>
/// Defines encryption metadata for persistent storage.
/// </summary>
public class EncryptionMetadata
{
    /// <summary>
    /// Gets or sets the initial vector.
    /// </summary>
    public string? InitialVector { get; set; }

    /// <summary>
    /// Gets or sets the key.
    /// </summary>
    public string? Key { get; set; }
}
