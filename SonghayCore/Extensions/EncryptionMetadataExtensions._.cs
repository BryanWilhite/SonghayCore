using Songhay.Models;
using Songhay.Security;
using System;
using System.Data.Common;
using System.Linq;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="EncryptionMetadata"/>
    /// </summary>
    public static partial class EncryptionMetadataExtensions
    {
        /// <summary>
        /// Decrypts the specified encrypted string.
        /// </summary>
        /// <param name="encryptionMeta">The encryption meta.</param>
        /// <param name="encryptedString">The encrypted string.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// encryptionMeta;The expected metadata is not here.
        /// or
        /// encryptedString;The expected encrypted string is not here.
        /// </exception>
        public static string Decrypt(this EncryptionMetadata encryptionMeta, string encryptedString)
        {
            if (encryptionMeta == null) throw new ArgumentNullException("encryptionMeta", "The expected metadata is not here.");
            if (string.IsNullOrWhiteSpace(encryptedString)) throw new ArgumentNullException("encryptedString", "The expected encrypted string is not here.");

            var crypt = new SymmetricCrypt();
            return crypt.Decrypt(encryptedString, encryptionMeta.Key, encryptionMeta.InitialVector);
        }

        /// <summary>
        /// Gets the connection string with decrypted value.
        /// </summary>
        /// <param name="encryptionMeta">The encryption meta.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="connectionStringKey">The connection string key.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// encryptionMeta;The expected metadata is not here.
        /// or
        /// connectionString;The expected configuration settings are not here.
        /// </exception>
        /// <exception cref="NullReferenceException"></exception>
        public static string GetConnectionStringWithDecryptedValue(this EncryptionMetadata encryptionMeta, string connectionString, string connectionStringKey)
        {
            if (encryptionMeta == null) throw new ArgumentNullException("encryptionMeta", "The expected metadata is not here.");
            if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException("connectionString", "The expected configuration settings are not here.");

            var builder = new DbConnectionStringBuilder
            {
                ConnectionString = connectionString
            };

            var key = builder.Keys.OfType<string>().FirstOrDefault(i => i.ToLowerInvariant() == connectionStringKey);
            if (key == null) throw new NullReferenceException(string.Format("The expected key “{0}” in connection string is not here.", connectionStringKey));

            var decryptedValue = encryptionMeta.Decrypt(builder[connectionStringKey].ToString());
            builder[connectionStringKey] = decryptedValue;

            return builder.ConnectionString;
        }

    }
}
