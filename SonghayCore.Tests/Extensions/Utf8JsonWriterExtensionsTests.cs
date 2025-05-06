using System.Text;

namespace Songhay.Tests.Extensions;

public class Utf8JsonWriterExtensionsTests(ITestOutputHelper helper)
{
    [Fact]
    public void ShouldWriteEmptyJsonObject()
    {
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);

        writer.WriteObject(writerAction: null);

        writer.Flush();

        string json = Encoding.UTF8.GetString(stream.ToArray());

        helper.WriteLine(json);
        Assert.Equal("{}", json);
    }

    [Fact]
    public void ShouldWriteJsonObject()
    {
        const string match = "match";
        const string path = "path";
        const string nested = "nested";
        const string query = "query";

        var options = new JsonWriterOptions
        {
            Indented = true,
        };

        var pathPropertyExpectedValue = "myIndexNestedObject";

        var matchPropertyName = "myIndexNestedObject.myProperty";
        var matchPropertyExpectedValue = 99;

        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream, options);

        writer.WriteObject(() =>
        {
            writer.WriteObject(query, () =>
            {
                writer.WriteObject(nested, () =>
                {
                    writer.WriteString(path, pathPropertyExpectedValue);

                    writer.WriteObject(query, () =>
                    {
                        writer.WriteObject(match, () =>
                        {
                            writer.WriteNumber(matchPropertyName, matchPropertyExpectedValue);
                        });
                    });
                });
            });
        });

        writer.Flush();

        string json = Encoding.UTF8.GetString(stream.ToArray());
        using var jDocument = JsonDocument.Parse(json);

        helper.WriteLine(json);

        var pathProperty = jDocument.RootElement
            .GetProperty(query)
            .GetProperty(nested)
            .GetProperty(path);

        Assert.Equal(pathProperty.GetString(), pathPropertyExpectedValue);

        var matchProperty = jDocument.RootElement
            .GetProperty(query)
            .GetProperty(nested)
            .GetProperty(query)
            .GetProperty(match)
            .GetProperty(matchPropertyName);

        Assert.Equal(matchProperty.GetInt32(), matchPropertyExpectedValue);
    }
}