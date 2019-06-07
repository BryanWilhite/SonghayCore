using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Songhay.Security.Cryptography
{
	/// <summary>Encryption class.</summary>
	/// <remarks>
	/// This class is based
	/// on Microsoft Patterns and Practices
	/// "How To: Create an Encryption Library"
	/// by J.D. Meier, Alex Mackman, Michael Dunner, and Srinath Vasireddy
	/// at http://msdn.microsoft.com/library/en-us/dnnetsec/html/SecNetHT10.asp.
	/// </remarks>
	public class Encryptor
	{
        /// <summary>Default constructor.</summary>
        /// <param name="AlgorithmEnum">
        /// <see cref="EncryptionAlgorithm"/>
        /// enumerated constant.
        /// </param>
        public Encryptor(EncryptionAlgorithm AlgorithmEnum)
        {
            transformer = new EncryptTransformer(AlgorithmEnum);
        }

        /// <summary>
        /// Encrypts <see cref="System.Byte"/> data.
        /// </summary>
        /// <param name="bytesData">
        /// The data to encrypt.
        /// </param>
        /// <param name="bytesKey">
        /// The encryption key.
        /// </param>
        public byte[] Encrypt(byte[] bytesData, byte[] bytesKey)
        {
            //Set up the stream that will hold the encrypted data.
            MemoryStream memStreamEncryptedData = new MemoryStream();

            transformer.IV = this._initVec;
            ICryptoTransform transform = 
                transformer.GetCryptoServiceProvider(bytesKey);
            CryptoStream encStream
                = new CryptoStream(memStreamEncryptedData,transform,
                    CryptoStreamMode.Write);
            try
            {
                //Encrypt the data, write it to the memory stream.
                encStream.Write(bytesData, 0, bytesData.Length);
            }
            catch(Exception ex)
            {
                throw new Exception(
                    String.Concat("Error while writing encrypted data to the stream: \n",
                    ex.Message)
                 );
            }
            //Set the IV and key for the client to retrieve
            this._encKey = transformer.Key;
            this._encKeyString = Convert.ToBase64String(this._encKey);
            this._initVec = transformer.IV;
            this._initVecString = Convert.ToBase64String(this._initVec);
            encStream.FlushFinalBlock();
            encStream.Close();

            //Send the data back.
            return memStreamEncryptedData.ToArray();
        }

        /// <summary>
        /// Encrypts <see cref="System.String"/> data.
        /// </summary>
        /// <param name="StringLiteral">
        /// The data to encrypt.
        /// </param>
        /// <param name="Key">
        /// The encryption key.
        /// </param>
        /// <returns>
        /// A string representation
        /// of the encrypted data.
        /// </returns>
        public string Encrypt(string StringLiteral,string Key)
        {
            byte[] ret;
            UnicodeEncoding encoder = new UnicodeEncoding();

            ret = Encrypt(encoder.GetBytes(StringLiteral),
                encoder.GetBytes(Key));

            return Convert.ToBase64String(ret);
        }

        /// <summary>
        /// Encrypts <see cref="System.String"/> data.
        /// </summary>
        /// <param name="StringLiteral">
        /// The data to encrypt.
        /// </param>
        /// <returns>
        /// A string representation
        /// of the encrypted data.
        /// </returns>
        /// <remarks>
        /// Calling this method generates random
        /// <see cref="EncryptTransformer.IV"/>
        /// and <see cref="EncryptTransformer.Key"/> values.
        /// </remarks>
        public string Encrypt(string StringLiteral)
        {
            byte[] ret;
            UnicodeEncoding encoder = new UnicodeEncoding();

            ret = Encrypt(encoder.GetBytes(StringLiteral),null);

            return Convert.ToBase64String(ret);
        }

        /// <summary>Initial Vector</summary>
        public byte[] IV
        {
            get{return this._initVec;}
            set{this._initVec = value;}
        }

        /// <summary>Initial Vector</summary>
        public string IvString
        {
            get{return this._initVecString;}
        }

        /// <summary>Encryption Key</summary>
        public byte[] Key
        {
            get{return this._encKey;}
        }

        /// <summary>Encryption Key</summary>
        public string KeyString
        {
            get{return this._encKeyString;}
        }

        private EncryptTransformer transformer;
        private byte[] _encKey;
        private byte[] _initVec;
        private string _encKeyString;
        private string _initVecString;
    }
}
