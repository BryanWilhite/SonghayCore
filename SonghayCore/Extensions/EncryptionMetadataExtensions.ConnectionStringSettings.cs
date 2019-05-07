#if NET452 || NET462

using Songhay.Models;
using System;
using System.Configuration;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="EncryptionMetadata"/>
    /// </summary>
    public static partial class EncryptionMetadataExtensions
    {
        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <param name="encryptionMeta">The encryption meta.</param>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// encryptionMeta;The expected metadata is not here.
        /// or
        /// settings;The expected configuration settings are not here.
        /// </exception>
        public static string GetConnectionString(this EncryptionMetadata encryptionMeta, ConnectionStringSettings settings)
        {
            if (settings == null) throw new ArgumentNullException("settings", "The expected configuration settings are not here.");

            var connectionString = encryptionMeta.Decrypt(settings.ConnectionString);
            return connectionString;
        }
    }
}

#endif
