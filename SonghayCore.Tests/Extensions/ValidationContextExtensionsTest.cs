using System.ComponentModel.DataAnnotations;
using Songhay.Extensions;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.Tests.Extensions
{
    public class ValidationContextExtensionsTest
    {
        public ValidationContextExtensionsTest(ITestOutputHelper helper)
        {
            this._testOutputHelper = helper;
        }

        public void ShouldGetDisplayText()
        {
            var mine = new MyModelOne
            {
                PropertyOne = string.Empty,
                PropertyTwo = "two",
                PropertyThree = "three"
            };

            var validation = mine.ToValidationResults();
            Assert.NotNull(validation);
            Assert.True(validation.Any(), "The expected validation results are not here.");
            this._testOutputHelper.WriteLine(validation.ToDisplayText());
        }

        ITestOutputHelper _testOutputHelper;
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
