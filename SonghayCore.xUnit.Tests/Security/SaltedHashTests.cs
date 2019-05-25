using Songhay.Security;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.Tests.Security
{
    public class SaltedHashTests
    {
        public SaltedHashTests(ITestOutputHelper helper)
        {
            this._testOutputHelper = helper;
        }

        [Fact]
        public void GetHashAndSalt_Test()
        {
            // We use the default SHA-256 & 4 byte length
            var demo = new SaltedHash();

            // We have a password, which will generate a Hash and Salt
            var password = "MyGlook234";
            string hash;
            string salt;

            demo.GetHashAndSalt(password, out hash, out salt);
            this._testOutputHelper.WriteLine("Password = {0} , Hash = {1} , Salt = {2}", password, hash, salt);

            // Password validation
            //
            // We need to pass both the earlier calculated Hash and Salt (we need to store this somewhere safe between sessions)

            // First check if a wrong password passes
            var wrongPassword = "OopsOops";
            var actual = demo.VerifyHash(wrongPassword, hash, salt);
            this._testOutputHelper.WriteLine("Verifying {0} = {1}", wrongPassword, actual);
            Assert.False(actual, "The expected password is not here.");

            // Check if the correct password passes
            actual = demo.VerifyHash(password, hash, salt);
            this._testOutputHelper.WriteLine("Verifying {0} = {1}", password, actual);
            Assert.True(actual, "The expected password is not here.");
        }

        readonly ITestOutputHelper _testOutputHelper;
    }
}
