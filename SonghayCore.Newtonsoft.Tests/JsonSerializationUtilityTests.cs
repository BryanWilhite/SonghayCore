using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Songhay.Extensions;
using Xunit;

namespace Songhay.Tests
{
    class AllTheProperties: INotAllTheProperties
    {
        public string? PropertyOne { get; set; }

        public string? PropertyTwo { get; set; }

        public string? PropertyThree { get; set; }
    }

    interface INotAllTheProperties
    {
        string? PropertyOne { get; set; }

        string? PropertyTwo { get; set; }
    }

    public class JsonSerializationUtilityTests
    {
        [Fact]
        public void GetConventionalResolver_Test()
        {
            var documentSettings = JsonSerializationUtility
                .GetConventionalResolver<INotAllTheProperties>(useJavaScriptCase: false)
                .ToJsonSerializerSettings();

            var everything = new AllTheProperties
            {
                PropertyOne = "one",
                PropertyTwo = "two",
                PropertyThree = "three"
            };

            var documentJson = JsonConvert.SerializeObject(everything, documentSettings);

            var jO = JObject.Parse(documentJson);

            Assert.NotNull(jO);
            Assert.False(jO.HasProperty(nameof(AllTheProperties.PropertyThree)));
        }
    }
}
