using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;
using Songhay.Extensions;
using System.Linq;

namespace Songhay.Tests.Extensions
{
    [TestClass]
    public class ValidationContextExtensionsTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void ShouldGetDisplayText()
        {
            var mine = new MyModelOne
            {
                PropertyOne = string.Empty,
                PropertyTwo = "two",
                PropertyThree = "three"
            };

            var validation = mine.ToValidationResults();
            Assert.IsNotNull(validation, "The expected validation results instance is not here.");
            Assert.IsNotNull(validation.Any(), "The expected validation results are not here.");
            this.TestContext.WriteLine(validation.ToDisplayText());
        }
    }

    class MyModelOne
    {
        [Required]
        public string PropertyOne { get; set; }
        [Required]
        public string PropertyTwo { get; set; }
        [Required]
        public string PropertyThree { get; set; }
    }
}
