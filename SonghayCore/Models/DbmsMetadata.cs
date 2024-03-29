﻿namespace Songhay.Models;

/// <summary>
/// Defines DBMS metadata
/// </summary>
public class DbmsMetadata
{
    /// <summary>
    /// Gets or sets the connection string.
    /// </summary>
    public string? ConnectionString { get; set; }

    /// <summary>
    /// Gets or sets the connection string key.
    /// </summary>
    public string? ConnectionStringKey { get; set; }

    /// <summary>
    /// Gets or sets the encryption metadata.
    /// </summary>
    public EncryptionMetadata? EncryptionMetadata { get; set; }

    /// <summary>
    /// Gets or sets the name of the provider.
    /// </summary>
    public string? ProviderName { get; set; }

    /// <summary>
    /// Returns a <see cref="String" /> that represents this instance.
    /// </summary>
    /// <returns>
    /// A <see cref="String" /> that represents this instance.
    /// </returns>
    public override string ToString()
    {
        var sb = new StringBuilder();

        if (!string.IsNullOrWhiteSpace(ProviderName)) sb.Append($"{nameof(ProviderName)}: {ProviderName} | ");
        if (!string.IsNullOrWhiteSpace(ConnectionString))
            sb.Append($"{nameof(ConnectionString)}: {ConnectionString[..72]}... ");
        if (EncryptionMetadata != null) sb.Append("| has encryption metadata");

        return (sb.Length > 0) ? sb.ToString() : base.ToString() ?? GetType().Name;
    }
}
