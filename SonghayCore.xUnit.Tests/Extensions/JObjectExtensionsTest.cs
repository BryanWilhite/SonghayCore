using Newtonsoft.Json.Linq;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.Extensions.Tests
{

    public class JObjectExtensionsTest
    {
        public JObjectExtensionsTest(ITestOutputHelper helper)
        {
            this._testOutputHelper = helper;
        }

        [Theory, InlineData(@"{ ""data"": { ""one"":""uno"", ""two"":""dos"" } }")]
        public void ShouldGetDictionaryOfStrings(string json)
        {
            this._testOutputHelper.WriteLine("json: {0}", json);

            var jO = JObject.Parse(json);

            var data = jO.GetDictionary("data", throwException: true);
            this._testOutputHelper.WriteLine("Dictionary keys: {0}", string.Join(",", data.Keys.ToArray()));
        }

        [Theory, InlineData(@"{ ""one"":[""uno"", ""un""], ""two"":[""dos"", ""deux""] }")]
        public void ShouldGetDictionaryOfArrayOfStrings(string json)
        {
            this._testOutputHelper.WriteLine("json: {0}", json);

            var jO = JObject.Parse(json);

            var data = jO.GetDictionary(throwException: true);
            this._testOutputHelper.WriteLine("Dictionary keys: {0}", string.Join(",", data.Keys.ToArray()));
        }

        [Theory, InlineData(@"{ ""items"": [ ""one"", ""two"", ""three"" ] }")]
        public void ShouldGetJArray(string json)
        {
            this._testOutputHelper.WriteLine("json: {0}", json);

            var jO = JObject.Parse(json);

            var jA = jO.GetJArray("items", throwException: true);
            var jA_clone = jA.DeepClone();

            jA.Add("four");
            this._testOutputHelper.WriteLine("original JArray: {0}", jA.ToString());
            var e = jA.ElementAtOrDefault(3);
            Assert.NotNull(e);

            this._testOutputHelper.WriteLine("cloned JArray: {0}", jA_clone.ToString());
            var e_clone = jA_clone.ElementAtOrDefault(3);
            Assert.Null(e_clone);
        }

        [Theory, InlineData(1, 2, @"{ ""items"": [ { ""x"": 1 }, { ""x"": 2 }, { ""x"": 3 } ] }")]
        public void ShouldGetJTokenFromJArray(int arrayIndex, int expectedValue, string json)
        {
            this._testOutputHelper.WriteLine("arrayIndex: {0}", arrayIndex);
            this._testOutputHelper.WriteLine("expectedValue: {0}", expectedValue);
            this._testOutputHelper.WriteLine("json: {0}", json);

            var jO = JObject.Parse(json);

            var token = jO.GetJTokenFromJArray("items", "x", arrayIndex, throwException: true);
            Assert.NotNull((token as JValue));

            var actualValue = (token as JValue).Value<int>();
            this._testOutputHelper.WriteLine("actualValue: {0}", actualValue);
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory, InlineData(@"{ ""items"": [], ""otherItems"": null }")]
        public void ShouldNotGetJArray(string json)
        {
            this._testOutputHelper.WriteLine("json: {0}", json);

            var jO = JObject.Parse(json);

            var jA = jO.GetJArray("items", throwException: false);
            Assert.Null(jA);

            var jA_other = jO.GetJArray("otherItems", throwException: false);
            Assert.Null(jA_other);

            var jA_otherOther = jO.GetJArray("otherOtherItems", throwException: false);
            Assert.Null(jA_other);
        }

        readonly ITestOutputHelper _testOutputHelper;
    }
}
