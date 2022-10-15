using System.ComponentModel.DataAnnotations;

namespace Songhay.Tests.Extensions;

public class ValidationContextExtensionsTest
{
    public ValidationContextExtensionsTest(ITestOutputHelper helper)
    {
        _testOutputHelper = helper;
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

        var results = mine.Validate(mine.ToValidationContext()).ToArray();
        Assert.NotEmpty(results);
        _testOutputHelper.WriteLine(results.ToDisplayText());
    }

    readonly ITestOutputHelper _testOutputHelper;
}

class MyModelOne : IValidatableObject
{
    [Required]
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string? PropertyOne { get; set; }
    [Required]
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string? PropertyTwo { get; set; }
    [Required]
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string? PropertyThree { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        return this.ToValidationResults(validationContext);
    }
}
