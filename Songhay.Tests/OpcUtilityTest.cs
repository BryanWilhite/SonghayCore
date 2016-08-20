using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Songhay.Tests
{
    /// <summary>
    /// Tests of <see cref="OpcUtility"/>
    /// </summary>
    [TestClass]
    public class OpcUtilityTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        [TestProperty("packUri", "pack://application:,,,/ReferencedAssembly;component/Subfolder/ResourceFile.xaml")]
        public void ShouldAssertPackUriIsValid()
        {
            var packUri = this.TestContext.Properties["packUri"].ToString();
            var result = OpcUtility.IsPackUriValid(packUri);
            Assert.IsTrue(result, "The valid Pack URI is not here.");
        }

        [TestMethod]
        [TestProperty("packUri", "pack://application:,,,/Songhay.Tests;component/Subfolder/OpcUtilityTest.xaml")]
        public void ShouldGetPackUriFromType()
        {
            var packUri = this.TestContext.Properties["packUri"].ToString();

            var actualPackUri = OpcUtility.GetPackUriFromType(this.GetType(), OpcReferencedTypeStrategy.FromAssemblyFileName);
            Assert.AreEqual(packUri, actualPackUri, "The expected pack URI is not here.");

            actualPackUri = OpcUtility.GetPackUriFromType(this.GetType(), OpcReferencedTypeStrategy.FromTypeFullName);
            Assert.AreEqual(packUri, actualPackUri, "The expected pack URI is not here.");
        }

        [TestMethod]
        [TestProperty("packUri", "pack://application:,,,/ReferencedAssembly;component/Subfolder/ResourceFile.xaml")]
        public void ShouldGetRelativePackUri()
        {
            var packUri = this.TestContext.Properties["packUri"].ToString();
            var uri = OpcUtility.GetRelativePackUri(packUri);
            Assert.IsTrue(uri.IsWellFormedOriginalString(), "The relative URI is not valid.");
        }
    }
}
;