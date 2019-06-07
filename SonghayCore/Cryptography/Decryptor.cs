using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Songhay.Security.Cryptography
{
    /// <summary>Decryption class.</summary>
    /// <remarks>
    /// This class is based
    /// on Microsoft Patterns and Practices
    /// "How To: Create an Encryption Library"
    /// by J.D. Meier, Alex Mackman, Michael Dunner, and Srinath Vasireddy
    /// at http://msdn.microsoft.com/library/en-us/dnnetsec/html/SecNetHT10.asp.
    /// </remarks>
    public class Decryptor
	{
        /// <summary>Default constructor.</summary>
        /// <param name="AlgorithmEnum">
        /// <see cref="EncryptionAlgorithm"/>
        /// enumerated constant.
        /// </param>
        public Decryptor(EncryptionAlgorithm AlgorithmEnum)
        {
            this._transformer = new DecryptTransformer(AlgorithmEnum);
        }

        /// <summary>
        /// Decrypts scalar values
        /// into a <see cref="System.Byte"/> array.
        /// </summary>
        /// <param name="bytesData">The data to decrypt.</param>
        /// <param name="bytesKey">The encryption key.</param>
        public byte[] Decrypt(byte[] bytesData, byte[] bytesKey)
        {
            //Set up the memory stream for the decrypted data.
            MemoryStream memStreamDecryptedData = new MemoryStream();

            //Pass in the initialization vector.
            this._transformer.IV = this._initVec;
            ICryptoTransform transform = 
                this._transformer.GetCryptoServiceProvider(bytesKey);
            CryptoStream decStream = new CryptoStream(memStreamDecryptedData,
                transform,
                CryptoStreamMode.Write);
            try
            {
                decStream.Write(bytesData, 0, bytesData.Length);
            }
            catch(Exception ex)
            {
                throw new Exception("Error while writing encrypted data to the stream: \n" + ex.Message);
            }
            decStream.FlushFinalBlock();
            decStream.Close();
            // Send the data back.
            return memStreamDecryptedData.ToArray();
        }

        /// <summary>
        /// Decrypts scalar values
        /// into a <see cref="System.String"/>.
        /// </summary>
        /// <param name="EncryptedString">
        /// Encrypted data.
        /// </param>
        /// <param name="EncryptedKey">
        /// Encrypted key.
        /// </param>
        /// <param name="EncryptedIniVector">
        /// Encrypted initialization vector.
        /// </param>
        /// <returns>
        /// A decrypted <see cref="System.Text.UnicodeEncoding"/>
        /// <see cref="System.String"/>.
        /// </returns>
        public string Decrypt(string EncryptedString,string EncryptedKey,string EncryptedIniVector)
        {
            string ret = String.Empty;

            //Data:
            byte[] dBa = Convert.FromBase64String(EncryptedString);

            //Initialization Vector:
            byte[] ivBa = Convert.FromBase64String(EncryptedIniVector);
            this.IV = ivBa;

            //Key:
            byte[] keyBa = Convert.FromBase64String(EncryptedKey);

            byte[] retBa = this.Decrypt(dBa,keyBa);
            UnicodeEncoding ucode = new UnicodeEncoding();
            ret = ucode.GetString(retBa);

            return ret;
        }
        
        /// <summary>Initial Vector</summary>
        public byte[] IV
        {
            set{this._initVec = value;}
        }

        private DecryptTransformer _transformer;
        private byte[] _initVec;
    }
}
