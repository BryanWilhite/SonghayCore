using Songhay.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Songhay.Tests.Security
{
    [TestClass]
    public class SymmetricCryptTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        [TestProperty("password", "My!#pwd")]
        [TestProperty("passwordKey", "")]
        [TestProperty("passwordInitialVector", "")]
        public void ShouldEncryptAndDecrypt()
        {
            var password = this.TestContext.Properties["password"].ToString();
            this.TestContext.WriteLine("original password: {0}", password);

            var crypt = new SymmetricCrypt();

            var key = this.TestContext.Properties["passwordKey"].ToString();
            key = string.IsNullOrEmpty(key) ? SymmetricCrypt.GetKey() : key;
            this.TestContext.WriteLine("key: {0}", key);

            var iv = this.TestContext.Properties["passwordInitialVector"].ToString();
            iv = SymmetricCrypt.GetInitialVector();
            this.TestContext.WriteLine("initial vector: {0}", iv);

            var encrypted = crypt.Encrypt(password, key, iv);
            this.TestContext.WriteLine("encrypted: {0}", encrypted);

            var actual = crypt.Decrypt(encrypted, key, iv);
            this.TestContext.WriteLine("decrypted password: {0}", actual);

            Assert.AreEqual<string>(password, actual, "The expected decryption is not here.");
        }
    }
}