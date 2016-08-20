using Songhay.Models;
using Songhay.Security;
using System;
using System.Configuration;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="EncryptionMetadata"/>
    /// </summary>
    public static class EncryptionMetadataExtensions
    {
        /// <summary>
        /// Decrypts the specified encrypted string.
        /// </summary>
        /// <param name="encryptionMeta">The encryption meta.</param>
        /// <param name="encryptedString">The encrypted string.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// encryptionMeta;The expected metadata is not here.
        /// or
        /// encryptedString;The expected encrypted string is not here.
        /// </exception>
        public static string Decrypt(this EncryptionMetadata encryptionMeta, string encryptedString)
        {
            if (encryptionMeta == null) throw new ArgumentNullException("encryptionMeta", "The expected metadata is not here.");
            if (string.IsNullOrEmpty(encryptedString)) throw new ArgumentNullException("encryptedString", "The expected encrypted string is not here.");

            var crypt = new SymmetricCrypt();
            return crypt.Decrypt(encryptedString, encryptionMeta.Key, encryptionMeta.InitialVector);
        }

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