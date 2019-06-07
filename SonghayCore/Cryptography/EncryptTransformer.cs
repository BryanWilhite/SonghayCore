using System;
using System.Security.Cryptography;

namespace Songhay.Security.Cryptography
{
    /// <summary>
    /// Enumerates the encryption/decryption algorithms
    /// available in this class.
    /// </summary>
    public enum EncryptionAlgorithm
    {
        /// <summary>
        /// Specifies the Data Encryption Standard (DES) algorithm.
        /// </summary>
        Des = 1,
        /// <summary>
        /// Specifies the RC2 algorithm.
        /// </summary>
        Rc2,
        /// <summary>
        /// Specifies the Rijndael symmetric encryption algorithm.
        /// </summary>
        Rijndael,
        /// <summary>
        /// Specifies the "triple" Data Encryption Standard (DES) algorithm.
        /// </summary>
        TripleDes
    }

    /// <summary>
    /// Generates an encryption service provider
    /// for cryptographic transformations defined by the
    /// <see cref="System.Security.Cryptography.ICryptoTransform"/>
    /// interface.
    /// </summary>
	internal class EncryptTransformer
	{
        /// <summary>Default constructor.</summary>
        /// <param name="AlgorithmEnum">
        /// <see cref="EncryptionAlgorithm"/>
        /// enumerated constant.
        /// </param>
        internal EncryptTransformer(EncryptionAlgorithm AlgorithmEnum)
        {
            //Save the algorithm being used.
            this._algorithmID = AlgorithmEnum;
        }

        /// <summary>
        /// Generates an encryption service provider
        /// from one of the known providers enumerated
        /// in <see cref="EncryptionAlgorithm"/>.
        /// </summary>
        /// <param name="bytesKey">Encryption key.</param>
        internal ICryptoTransform GetCryptoServiceProvider(byte[] bytesKey)
        {
            switch (this._algorithmID)
            {
                    #region known cryptographic transforms:

                case EncryptionAlgorithm.Des:
                {
                    DES des = new DESCryptoServiceProvider();
                    des.Mode = CipherMode.CBC;
                    if (null == bytesKey)
                    {
                        des.GenerateKey();
                        this._encKey = des.Key;
                    }
                    else
                    {
                        des.Key = bytesKey;
                        this._encKey = des.Key;
                    }
                    if (null == this._initVec)
                    {
                        des.GenerateIV();
                        this._initVec = des.IV;
                    }
                    else
                    {
                        des.IV = this._initVec;
                    }
                    return des.CreateEncryptor();
                }
                case EncryptionAlgorithm.TripleDes:
                {
                    TripleDES des3 = new TripleDESCryptoServiceProvider();
                    des3.Mode = CipherMode.CBC;
                    if (null == bytesKey)
                    {
                        des3.GenerateKey();
                        this._encKey = des3.Key;
                    }
                    else
                    {
                        des3.Key = bytesKey;
                        this._encKey = des3.Key;
                    }
                    if (null == this._initVec)
                    {
                        des3.GenerateIV();
                        this._initVec = des3.IV;
                    }
                    else
                    {
                        des3.IV = this._initVec;
                    }
                    return des3.CreateEncryptor();
                }
                case EncryptionAlgorithm.Rc2:
                {
                    RC2 rc2 = new RC2CryptoServiceProvider();
                    rc2.Mode = CipherMode.CBC;
                    if (null == bytesKey)
                    {
                        rc2.GenerateKey();
                        this._encKey = rc2.Key;
                    }
                    else
                    {
                        rc2.Key = bytesKey;
                        this._encKey = rc2.Key;
                    }
                    if (null == this._initVec)
                    {
                        rc2.GenerateIV();
                        this._initVec = rc2.IV;
                    }
                    else
                    {
                        rc2.IV = this._initVec;
                    }
                    return rc2.CreateEncryptor();
                }
                case EncryptionAlgorithm.Rijndael:
                {
                    Rijndael rijndael = new RijndaelManaged();
                    rijndael.Mode = CipherMode.CBC;
                    if(null == bytesKey)
                    {
                        rijndael.GenerateKey();
                        this._encKey = rijndael.Key;
                    }
                    else
                    {
                        rijndael.Key = bytesKey;
                        this._encKey = rijndael.Key;
                    }
                    if(null == this._initVec)
                    {
                        rijndael.GenerateIV();
                        this._initVec = rijndael.IV;
                    }
                    else
                    {
                        rijndael.IV = this._initVec;
                    }
                    return rijndael.CreateEncryptor();
                }

                    #endregion:

                default:
                {
                    throw new CryptographicException(string.Concat("Algorithm ID '", 
                        this._algorithmID,"' not supported."));
                }
            }
        }

        /// <summary>
        /// Initialization Vector
        /// </summary>
        internal byte[] IV
        {
            get{return this._initVec;}
            set{this._initVec = value;}
        }

        /// <summary>
        /// Encryption Key
        /// </summary>
        internal byte[] Key
        {
            get{return this._encKey;}
        }

        private EncryptionAlgorithm _algorithmID;
        private byte[] _initVec;
        private byte[] _encKey;
    }
}
