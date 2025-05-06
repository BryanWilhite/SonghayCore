using System.Text.Json.Nodes;

namespace Songhay.Tests;

public class JsonNodeUtilityTests(ITestOutputHelper helper)
{
    [Fact]
    public void ToJsonNode_Anon_Test()
    {
        JsonNode? actual = JsonNodeUtility.ConvertToJsonNode(new { one = "uno" });
        Assert.True(actual.HasProperty("one"));
    }

    [Theory]
    [InlineData("""
                {
                    "RestApiMetadataSet": {
                        "SocialTwitter": {
                            "ClaimsSet": {
                                "TwitterConsumerKey": "[key]",
                                "TwitterConsumerSecret": "[secret]",
                                "TwitterToken": "[token]",
                                "TwitterTokenSecret": "[secret]",
                                "TwitterTokenBearer": "[token]"
                            }
                        }
                    }
                }
                """, false)]
    [InlineData("[]", true)]
    [InlineData("42", true)]
    public void ParseJsonObject_Test(string input, bool exceptionExpected)
    {
        ILogger logger = _loggerProvider.CreateLogger(nameof(ParseJsonObject_Test));

        if (exceptionExpected) Assert.Throws<InvalidOperationException>(() => JsonNodeUtility.ParseJsonObject(input, logger));
        else
        {
            JsonObject? actual = JsonNodeUtility.ParseJsonObject(input, logger);
            Assert.NotNull(actual);
        }
    }

    private readonly XUnitLoggerProvider _loggerProvider = new(helper);
}