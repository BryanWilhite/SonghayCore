using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Extensions;
using System;
using System.Linq;

namespace Songhay.Tests.Extensions
{
    /// <summary>
    /// Tests for extensions of <see cref="Enum"/>.
    /// </summary>
    [TestClass]
    public class EnumExtensionsTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        /// Should get enumeration values.
        /// </summary>
        [TestMethod]
        public void ShouldGetEnumValues()
        {
            Enum myTestEnumOne = MyTestEnum.One;
            var values = myTestEnumOne.GetEnumValues().OfType<MyTestEnum>();
            Assert.IsTrue(values.Any(), "The expected values are not here.");
        }

        /// <summary>
        /// Should get enumeration description.
        /// </summary>
        [TestMethod]
        public void ShouldGetEnumDescription()
        {
            Enum myTestEnumOne = MyTestEnum.One;

            var expected = "this is one";
            var actual = myTestEnumOne.GetEnumDescription();
            Assert.AreEqual(expected, actual, "The expected Enum Description is not here.");
        }
    }

    enum MyTestEnum
    {
        [System.ComponentModel.Description("this is one")]
        One = 1,
        [System.ComponentModel.Description("this is two")]
        Two = 2,
        [System.ComponentModel.Description("this is three")]
        Three = 3,
    }
}
