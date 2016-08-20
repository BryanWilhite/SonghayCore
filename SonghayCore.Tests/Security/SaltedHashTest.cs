using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Songhay.Tests.Security
{
    using Songhay.Security;
    [TestClass]
    public class SaltedHashTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void ShouldSaveAndValidatePassword()
        {
            // We use the default SHA-256 & 4 byte length
            var demo = new SaltedHash();

            // We have a password, which will generate a Hash and Salt
            var password = "MyGlook234";
            string hash;
            string salt;

            demo.GetHashAndSalt(password, out hash, out salt);
            TestContext.WriteLine("Password = {0} , Hash = {1} , Salt = {2}", password, hash, salt);

            // Password validation
            //
            // We need to pass both the earlier calculated Hash and Salt (we need to store this somewhere safe between sessions)

            // First check if a wrong password passes
            var wrongPassword = "OopsOops";
            var actual = demo.VerifyHash(wrongPassword, hash, salt);
            TestContext.WriteLine("Verifying {0} = {1}", wrongPassword, actual);
            Assert.IsFalse(actual, "The expected password is not here.");

            // Check if the correct password passes
            actual = demo.VerifyHash(password, hash, salt);
            TestContext.WriteLine("Verifying {0} = {1}", password, actual);
            Assert.IsTrue(actual, "The expected password is not here.");
        }
    }
}