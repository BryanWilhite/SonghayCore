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
    public void GetJsonChildElementOrNull_Test(string input, string propertyName, string? expectedOutput)
    {
        using var jDoc = JsonDocument.Parse(input);
        JsonElement? actual = jDoc.RootElement.GetJsonChildElementOrNull(propertyName);
        Assert.Equal(expectedOutput, actual?.ToString());
    }

    [Theory]
    [InlineData("{ \"my-property\": { \"my-nested-property\": true} }", "my-property", "my-nested-property", "True")]
    public void GetJsonChildElementOrNull_Nested_Test(string input, string propertyName, string nestedPropertyName, string? expectedOutput)
    {
        using var jDoc = JsonDocument.Parse(input);
        JsonElement? actual = jDoc.RootElement
            .GetJsonChildElementOrNull(propertyName)
            .GetJsonChildElementOrNull(nestedPropertyName);
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
            .RootElement.GetJsonChildElementOrNull(propertyName)
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
            .GetJsonChildElementOrNull(propertyName)
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
            .GetJsonChildElementOrNull(propertyName)
            .ToReadOnlyCollection(el => el.ToInt());
        Assert.Equal(expectedOutput, actual);
    }

    public record struct MyRecordStruct(string One, string Two);

    public static readonly TheoryData<string, object?> ShouldConvertJsonTextData =
        new()
        {
            { "\"hello world!\"", "hello world!" },
            { "\"2022-07-23T18:59:41.183Z\"", DateTime.Parse("2022-07-23T18:59:41.183Z").ToUniversalTime() },
            { "1", 1 },
            { "\"0\"", false },
            { "\"y\"", true },
            { "123444000.01", 123444000.01d }, // double
            { "123444000.01", 123444000.01m }, // decimal
            { "2.99792458e8", 2.99792458e8 },
            { "15000000000", 15000000000L },
            { "255", (byte)255 },
            { "-32768", (short)-32768 },
            { "\"5\"", '5' },
            { "true", true },
            { "false", false },
            { "\"00000000-0000-0000-0000-000000000000\"", new Guid("00000000-0000-0000-0000-000000000000") },
            { """{ "One": "uno", "Two": "dos" }""", new MyRecordStruct("uno", "dos")},
            { "null", null }
        };

    [Theory]
    [MemberData(nameof(ShouldConvertJsonTextData))]
    public void ToScalarValue_Test(string jsonText, object? expectedBox)
    {
        // arrange:
        JsonElement jE = JsonElement.Parse(jsonText);

        // act:
        object? actual = expectedBox switch
        {
            DateTime => jE.ToScalarValue<DateTime>(),
            Guid => jE.ToScalarValue<Guid>(),
            double => jE.ToScalarValue<double>(),
            decimal => jE.ToScalarValue<decimal>(),
            short => jE.ToScalarValue<short>(),
            long => jE.ToScalarValue<long>(),
            int => jE.ToScalarValue<int>(),
            byte => jE.ToScalarValue<byte>(),
            char => jE.ToScalarValue<char>(),
            bool => jE.ToScalarValue<bool>(),
            string => jE.ToStringValue(), // string is not a struct 🧐
            MyRecordStruct => jE.ToScalarValue<MyRecordStruct>(),

            _ => null
        };

        // assert:
        Assert.Equal($"{expectedBox}", $"{actual}");
    }

    [Theory]
    [InlineData("{ \"my-property\": \"2022-07-23T18:59:41.183Z\" }", "my-property", "2022-07-23T18:59:41.183Z")]
    [InlineData("{ \"my-property\": null }", "my-not-property", null)]
    public void ToScalarValue_DateTime_Test(string input, string propertyName, string? expectedOutput)
    {
        using var jDoc = JsonDocument.Parse(input);
        DateTime? actual = jDoc
            .RootElement.GetJsonChildElementOrNull(propertyName)
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
            .RootElement.GetJsonChildElementOrNull(propertyName)
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
            .RootElement.GetJsonChildElementOrNull(propertyName)
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
            .RootElement.GetJsonChildElementOrNull(propertyName)
            .ToStringValue();

        Assert.Equal(expectedOutput, actual);
    }
}