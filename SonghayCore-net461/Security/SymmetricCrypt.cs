using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Songhay.Security
{
    /// <summary>
    /// Implements symmetric encryption/decryption
    /// for <see cref="System.Security.Cryptography.AesCryptoServiceProvider"/>.
    /// </summary>
    /// <remarks>
    /// For more information, see “AesCryptoServiceProvider Class”
    /// [http://msdn.microsoft.com/en-us/library/system.security.cryptography.aescryptoserviceprovider.aspx]
    /// </remarks>
    public class SymmetricCrypt
    {
        /// <summary>
        /// Gets the key.
        /// </summary>
        public static string GetKey()
        {
            string output = null;
            using (var provider = new AesCryptoServiceProvider())
            {
                provider.GenerateKey();
                output = Convert.ToBase64String(provider.Key);
            }
            return output;
        }

        /// <summary>
        /// Gets the initial vector.
        /// </summary>
        public static string GetInitialVector()
        {
            string output = null;
            using (var provider = new AesCryptoServiceProvider())
            {
                provider.GenerateIV();
                output = Convert.ToBase64String(provider.IV);
            }
            return output;
        }

        /// <summary>
        /// Decrypts the specified text.
        /// </summary>
        /// <param name="input">The text.</param>
        /// <param name="key">The key.</param>
        /// <param name="vector">The vector.</param>
        public string Decrypt(string input, byte[] key, byte[] vector)
        {
            var inputSet = Encoding.Unicode.GetBytes(input);
            return this.DecryptWithProvider(inputSet, key, vector);
        }

        /// <summary>
        /// Decrypts the specified text.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="key">The key.</param>
        /// <param name="vector">The vector.</param>
        public string Decrypt(string input, string key, string vector)
        {
            var inputSet = Convert.FromBase64String(input);
            var keySet = Convert.FromBase64String(key);
            var ivSet = Convert.FromBase64String(vector);
            return this.DecryptWithProvider(inputSet, keySet, ivSet);
        }

        /// <summary>
        /// Encrypts the specified text.
        /// </summary>
        /// <param name="input">The text.</param>
        /// <param name="key">The key.</param>
        /// <param name="vector">The vector.</param>
        public string Encrypt(string input, byte[] key, byte[] vector)
        {
            var output = this.EncryptWithProvider(input, key, vector);
            return Convert.ToBase64String(output);
        }

        /// <summary>
        /// Encrypts the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="key">The key.</param>
        /// <param name="vector">The vector.</param>
        public string Encrypt(string input, string key, string vector)
        {
            var keySet = Convert.FromBase64String(key);
            var ivSet = Convert.FromBase64String(vector);
            return this.Encrypt(input, keySet, ivSet);
        }

        string DecryptWithProvider(byte[] cipherText, byte[] Key, byte[] IV)
        {
            string plaintext = null;

            using (var provider = new AesCryptoServiceProvider())
            {
                provider.Key = Key;
                provider.IV = IV;

                ICryptoTransform decryptor = provider.CreateDecryptor(provider.Key, provider.IV);
                using (var ms = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (var reader = new StreamReader(csDecrypt))
                        {
                            plaintext = reader.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }

        byte[] EncryptWithProvider(string plainText, byte[] Key, byte[] IV)
        {
            byte[] encrypted;
            using (var provider = new AesCryptoServiceProvider())
            {
                provider.Key = Key;
                provider.IV = IV;
                ICryptoTransform encryptor = provider.CreateEncryptor(provider.Key, provider.IV);
                using (var ms = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (var writer = new StreamWriter(cryptoStream))
                        {
                            writer.Write(plainText);
                        }
                        encrypted = ms.ToArray();
                    }
                }
            }

            return encrypted;
        }
    }
}
