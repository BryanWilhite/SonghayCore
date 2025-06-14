namespace Songhay.Tests.Extensions;

public class JsonElementExtensionsTests
{
    [Theory]
    [InlineData("""
                {
                    "my-property": {
                        "one": 1,
                        "sure": true
                    }
                }
                """, 32, "my-property: {\"one\": 1, \"sure\":…")]
    [InlineData("""
                {
                    "my-property": {
                        "one": 1.0,
                        "sure": true
                    }
                }
                """, 32, "my-property: {\"one\": 1.0, \"sure\"…")]
    [InlineData("""
                {
                    "my-property": {
                        "one": null,
                        "others": ["y","n","u"]
                    }
                }
                """, 48, "my-property: {\"one\": null, \"others\": [\"y\",\"n\",\"u…")]
    public void DisplayTopProperties_Test(string input, int truncationLength, string expectedOutput)
    {
        using var jDoc = JsonDocument.Parse(input);
        string actual = jDoc.RootElement.DisplayTopProperties(truncationLength);

        Assert.Equal(expectedOutput, actual);
    }

    [Theory]
    [InlineData("{ \"my-property\": {} }", "my-property", "{}")]
    [InlineData("{ \"my-property\": [] }", "my-property", "[]")]
    [InlineData("{ \"my-property\": [] }", "my-not-property", null)]
    [InlineData("{ \"my-property\": \"two\" }", "my-property", "two")]
    [InlineData("{ \"my-property\": null }", "my-property", "")]
    [InlineData("{ \"my-property\": false }", "my-property", "False")]
    public void GetJsonPropertyOrNull_Test(string input, string propertyName, string? expectedOutput)
    {
        using var jDoc = JsonDocument.Parse(input);
        JsonElement? actual = jDoc.RootElement.GetJsonPropertyOrNull(propertyName);
        Assert.Equal(expectedOutput, actual?.ToString());
    }

    [Theory]
    [InlineData("{ \"my-property\": { \"my-nested-property\": true} }", "my-property", "my-nested-property", "True")]
    public void GetJsonPropertyOrNull_Nested_Test(string input, string propertyName, string nestedPropertyName, string? expectedOutput)
    {
        using var jDoc = JsonDocument.Parse(input);
        JsonElement? actual = jDoc.RootElement
            .GetJsonPropertyOrNull(propertyName)
            .GetJsonPropertyOrNull(nestedPropertyName);
        Assert.Equal(expectedOutput, actual?.ToString());
    }

    [Theory]
    [InlineData("{ \"my-property\": [] }", "my-property", true)]
    [InlineData("{ \"my-property\": [] }", "my-not-property", false)]
    [InlineData("{}", "my-not-property", false)]
    public void HasProperty_Test(string input, string propertyName, bool expectedOutput)
    {
        using var jDoc = JsonDocument.Parse(input);
        bool actual = jDoc.RootElement.HasProperty(propertyName);
        Assert.Equal(expectedOutput, actual);
    }

    [Theory]
    [InlineData("{ \"my-property\": [1,2,3] }", "my-property", true)]
    [InlineData("{ \"my-property\": [1,2,3] }", "my-not-property", false)]
    [InlineData("{ \"my-property\": [] }", "my-property", false)]
    [InlineData("{ \"my-property\": null }", "my-property", false)]
    [InlineData("{ \"my-property\": 42 }", "my-property", false)]
    public void ToJsonElementArray_Test(string input, string propertyName, bool isNotEmpty)
    {
        using var jDoc = JsonDocument.Parse(input);
        JsonElement[] actual = jDoc
            .RootElement.GetJsonPropertyOrNull(propertyName)
            .ToJsonElementArray();

        if(isNotEmpty) Assert.NotEmpty(actual);
        else Assert.Empty(actual);
    }

    [Theory]
    [InlineData("{ \"one\": \"uno\", \"two\": \"two\", \"three\": \"tres\" }", 3)]
    public void ToObject_Dictionary_Test(string input, int expectedNumberOfKeys)
    {
        using var jDoc = JsonDocument.Parse(input);
        Dictionary<string, string>? actual = jDoc.RootElement.ToObject<Dictionary<string, string>>();
        Assert.NotNull(actual);
        Assert.Equal(expectedNumberOfKeys, actual.Keys.Count);
    }

    [Theory]
    [InlineData("{ \"my-property\": [1,2,3] }", "my-property", "1", "2", "3")]
    [InlineData("{ \"my-property\": [1,null,3] }", "my-property", "1", null, "3")]
    [InlineData("{ \"my-property\": [] }", "my-property")]
    public void ToReadOnlyCollection_Test(string input, string propertyName, params string?[] expectedOutput)
    {
        using var jDoc = JsonDocument.Parse(input);
        IReadOnlyCollection<string?> actual = jDoc.RootElement
            .GetJsonPropertyOrNull(propertyName)
            .ToReadOnlyCollection();
        Assert.Equal(expectedOutput, actual);
    }

    [Theory]
    [InlineData("{ \"my-property\": [1,2,3] }", "my-property", 1, 2, 3)]
    [InlineData("{ \"my-property\": [1,null,3] }", "my-property", 1, null, 3)]
    [InlineData("{ \"my-property\": [] }", "my-property")]
    public void ToReadOnlyCollection_Int_Test(string input, string propertyName, params int?[] expectedOutput)
    {
        using var jDoc = JsonDocument.Parse(input);
        IReadOnlyCollection<int?> actual = jDoc.RootElement
            .GetJsonPropertyOrNull(propertyName)
            .ToReadOnlyCollection(el => el.ToInt());
        Assert.Equal(expectedOutput, actual);
    }

    [Theory]
    [InlineData("{ \"my-property\": \"2022-07-23T18:59:41.183Z\" }", "my-property", "2022-07-23T18:59:41.183Z")]
    [InlineData("{ \"my-property\": null }", "my-not-property", null)]
    public void ToScalarValue_DateTime_Test(string input, string propertyName, string? expectedOutput)
    {
        using var jDoc = JsonDocument.Parse(input);
        DateTime? actual = jDoc
            .RootElement.GetJsonPropertyOrNull(propertyName)
            .ToScalarValue<DateTime>();

        Assert.Equal(
            string.IsNullOrWhiteSpace(expectedOutput) ?
                ProgramTypeUtility.ParseDateTime(expectedOutput)
                :
                ProgramTypeUtility.ParseDateTime(expectedOutput).ToValueOrThrow().ToUniversalTime(),
            actual);
    }

    [Theory]
    [InlineData("{ \"my-property\": true }", "my-property", true)]
    [InlineData("{ \"my-property\": \"1\" }", "my-property", true)]
    [InlineData("{ \"my-property\": 0 }", "my-property", null)]
    [InlineData("{ \"my-property\": 42 }", "my-not-property", null)]
    public void ToScalarValue_Boolean_Test(string input, string propertyName, bool? expectedOutput)
    {
        using var jDoc = JsonDocument.Parse(input);
        bool? actual = jDoc
            .RootElement.GetJsonPropertyOrNull(propertyName)
            .ToScalarValue<bool>();

        Assert.Equal(expectedOutput, actual);
    }

    [Theory]
    [InlineData("{ \"my-property\": 42 }", "my-property", 42)]
    [InlineData("{ \"my-property\": 42 }", "my-not-property", null)]
    public void ToScalarValue_Int32_Test(string input, string propertyName, int? expectedOutput)
    {
        using var jDoc = JsonDocument.Parse(input);
        int? actual = jDoc
            .RootElement.GetJsonPropertyOrNull(propertyName)
            .ToScalarValue<int>();

        Assert.Equal(expectedOutput, actual);
    }

    [Theory]
    [InlineData("{ \"my-property\": \"two\" }", "my-property", "two")]
    [InlineData("{ \"my-property\": 42 }", "my-property", "42")]
    [InlineData("{ \"my-property\": true }", "my-property", "True")]
    [InlineData("{ \"my-property\": null }", "my-property", null)]
    [InlineData("{ \"my-property\": [] }", "my-property", null)]
    [InlineData("{ \"my-property\": \"two\" }", "my-other-property", null)]
    public void ToStringValue_Test(string input, string propertyName, string? expectedOutput)
    {
        using var jDoc = JsonDocument.Parse(input);
        string? actual = jDoc
            .RootElement.GetJsonPropertyOrNull(propertyName)
            .ToStringValue();

        Assert.Equal(expectedOutput, actual);
    }
}