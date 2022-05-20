using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Songhay.Extensions;
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

        [Fact]
        public void ShouldGetDisplayText()
        {
            var mine = new MyModelOne
            {
                PropertyOne = string.Empty,
                PropertyTwo = "two",
                PropertyThree = "three"
            };

            var results = mine.Validate(mine.ToValidationContext());
            Assert.NotEmpty(results);
            this._testOutputHelper.WriteLine(results.ToDisplayText());
        }

        readonly ITestOutputHelper _testOutputHelper;
    }

    class MyModelOne : IValidatableObject
    {
        [Required]
        public string PropertyOne { get; set; }
        [Required]
        public string PropertyTwo { get; set; }
        [Required]
        public string PropertyThree { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return this.ToValidationResults(validationContext);
        }
    }
}
