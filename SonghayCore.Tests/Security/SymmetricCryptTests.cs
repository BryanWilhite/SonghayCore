using Songhay.Security;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.Tests.Security
{
    public class SymmetricCryptTests
    {
        public SymmetricCryptTests(ITestOutputHelper helper)
        {
            this._testOutputHelper = helper;
        }

        [Theory]
        [InlineData("My!#pwd", "", "")]
        public void ShouldEncryptAndDecrypt(string password, string passwordKey, string passwordInitialVector)
        {
            this._testOutputHelper.WriteLine($"original password: {password}");

            var crypt = new SymmetricCrypt();

            var key = string.IsNullOrEmpty(passwordKey) ? SymmetricCrypt.GetKey() : passwordKey;
            this._testOutputHelper.WriteLine($"key: {key}");

            var iv = string.IsNullOrEmpty(passwordInitialVector) ? SymmetricCrypt.GetInitialVector() : passwordInitialVector;
            this._testOutputHelper.WriteLine("initial vector: {0}", iv);

            var encrypted = crypt.Encrypt(password, key, iv);
            this._testOutputHelper.WriteLine($"encrypted: {encrypted}");

            var actual = crypt.Decrypt(encrypted, key, iv);
            this._testOutputHelper.WriteLine($"decrypted password: {actual}");

            Assert.Equal(password, actual);
        }

        readonly ITestOutputHelper _testOutputHelper;
    }
}