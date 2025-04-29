using System.Text.Json.Nodes;

namespace Songhay.Tests;

public class JsonNodeUtilityTests
{
    [Fact]
    public void ToJsonNode_Anon_Test()
    {
        JsonNode? actual = JsonNodeUtility.ConvertToJsonNode(new { one = "uno" });
        Assert.True(actual.HasProperty("one"));
    }
}