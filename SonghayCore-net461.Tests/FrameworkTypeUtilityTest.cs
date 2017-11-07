using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Songhay.Tests
{
    /// <summary>
    /// <see cref="FrameworkTypeUtility"/> tests.
    /// </summary>
    [TestClass]
    public class FrameworkTypeUtilityTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        [TestProperty("enumInput", "Warning")]
        [TestProperty("enumInputs", "Warning,Critical,Error")]
        public void ShouldParseEnum()
        {
            var enumInput = this.TestContext.Properties["enumInput"].ToString();
            var enumInputs = this.TestContext.Properties["enumInputs"].ToString();

            var enumValue = FrameworkTypeUtility.ParseEnum(enumInput, SourceLevels.All);
            Assert.AreEqual(SourceLevels.Warning, enumValue, "The expected enum value is not here.");

            var enumValues = FrameworkTypeUtility.ParseEnum(enumInputs, SourceLevels.All);
            Assert.AreEqual(SourceLevels.Warning | SourceLevels.Critical | SourceLevels.Error, enumValue, "The expected enum value is not here.");
        }
    }
}
