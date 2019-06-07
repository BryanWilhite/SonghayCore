using System;
using System.Security.Cryptography;

namespace Songhay.Security.Cryptography
{
    /// <summary>
    /// Generates a decryption service provider
    /// for cryptographic transformations defined by the
    /// <see cref="System.Security.Cryptography.ICryptoTransform"/>
    /// interface.
    /// </summary>
    internal class DecryptTransformer
	{
        /// <summary>Default constructor.</summary>
        /// <param name="AlgorithmEnum">
        /// <see cref="EncryptionAlgorithm"/>
        /// enumerated constant.
        /// </param>
        internal DecryptTransformer(EncryptionAlgorithm AlgorithmEnum)
        {
            this._algorithmEnum = AlgorithmEnum;
        }

        /// <summary>
        /// Generates a decryption service provider
        /// from one of the known providers enumerated
        /// in <see cref="EncryptionAlgorithm"/>.
        /// </summary>
        /// <param name="bytesKey">Encryption key.</param>
        internal ICryptoTransform GetCryptoServiceProvider(byte[] bytesKey)
        {
            switch (this._algorithmEnum)
            {
                case EncryptionAlgorithm.Des:
                {
                    DES des = new DESCryptoServiceProvider();
                    des.Mode = CipherMode.CBC;
                    des.Key = bytesKey;
                    des.IV = this._initVec;
                    return des.CreateDecryptor();
                }
                case EncryptionAlgorithm.TripleDes:
                {
                    TripleDES des3 = new TripleDESCryptoServiceProvider();
                    des3.Mode = CipherMode.CBC;
                    return des3.CreateDecryptor(bytesKey, this._initVec);
                }
                case EncryptionAlgorithm.Rc2:
                {
                    RC2 rc2 = new RC2CryptoServiceProvider();
                    rc2.Mode = CipherMode.CBC;
                    return rc2.CreateDecryptor(bytesKey, this._initVec);
                }
                case EncryptionAlgorithm.Rijndael:
                {
                    Rijndael rijndael = new RijndaelManaged();
                    rijndael.Mode = CipherMode.CBC;
                    return rijndael.CreateDecryptor(bytesKey, this._initVec);
                } 
                default:
                {
                    throw new CryptographicException(String.Concat("Algorithm ID '", 
                        this._algorithmEnum,"' not supported."));
                }
            }
        }

        /// <summary>
        /// Initialization Vector
        /// </summary>
        internal byte[] IV
        {
            set{this._initVec = value;}
        }

        private EncryptionAlgorithm _algorithmEnum;
        private byte[] _initVec;
    }
}
