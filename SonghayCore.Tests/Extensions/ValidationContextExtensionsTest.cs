using System.ComponentModel.DataAnnotations;

namespace Songhay.Tests.Extensions;

public class ValidationContextExtensionsTest(ITestOutputHelper helper)
{
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
        helper.WriteLine(results.ToDisplayText());
    }
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
