using Songhay.Models;
using System;
using System.Collections.Generic;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="ProgramMetadata"/>.
    /// </summary>
    public static class ProgramMetadataExtensions
    {
        /// <summary>
        /// Gets the connection string with decrypted value.
        /// </summary>
        /// <param name="meta">The <see cref="ProgramMetadata"/>.</param>
        /// <param name="connectionStringName">Name of the connection string.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">meta - The expected Songhay System metadata is not here.</exception>
        /// <exception cref="NotSupportedException">connectionStringName</exception>
        public static string GetConnectionStringWithDecryptedValue(this ProgramMetadata meta, string connectionStringName)
        {
            if (meta == null) throw new ArgumentNullException(nameof(meta), "The expected Songhay System metadata is not here.");

            if (!meta.DbmsSet.TryGetValue(connectionStringName, out var dbmsMeta))
                throw new NotSupportedException($"{nameof(connectionStringName)}: {connectionStringName} is not here.");

            var connectionString = dbmsMeta
                .EncryptionMetadata
                .GetConnectionStringWithDecryptedValue(dbmsMeta.ConnectionString, dbmsMeta.ConnectionStringKey);

            return connectionString;
        }

        /// <summary>
        /// Converts <see cref="ProgramMetadata" />
        /// to the conventional <see cref="System.Net.Http.Headers.HttpRequestHeaders"/>.
        /// </summary>
        /// <param name="meta">The <see cref="ProgramMetadata"/>.</param>
        /// <param name="restApiMetadataSetKey">The key for <see cref="ProgramMetadata.RestApiMetadataSet"/>.</param>
        /// <returns></returns>
        public static Dictionary<string, string> ToConventionalHeaders(this ProgramMetadata meta, string restApiMetadataSetKey)
        {
            if (meta == null) throw new ArgumentNullException(nameof(meta), "The expected conventional metadata is not here.");

            var genWebApiMeta = meta.RestApiMetadataSet.TryGetValueWithKey(restApiMetadataSetKey);
            var headers = new Dictionary<string, string>
            {
                {
                    genWebApiMeta.ClaimsSet.TryGetValueWithKey(RestApiMetadata.ClaimsSetHeaderApiKey),
                    genWebApiMeta.ApiKey
                }
            };

            return headers;
        }
    }
}
