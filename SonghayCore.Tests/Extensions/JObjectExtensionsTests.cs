using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using Songhay.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.Tests.Extensions
{

    public partial class JObjectExtensionsTests
    {
        public JObjectExtensionsTests(ITestOutputHelper helper)
        {
            this._testOutputHelper = helper;
        }

        [Theory, InlineData(@"{ ""data"": { ""one"":""uno"", ""two"":""dos"" } }")]
        public void ShouldGetDictionaryOfStrings(string json)
        {
            this._testOutputHelper.WriteLine("json: {0}", json);

            var jO = JObject.Parse(json);

            var data = jO.GetDictionary("data", throwException : true);
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

            var jA = jO.GetJArray("items", throwException : true);
            var jA_clone = jA.DeepClone();

            jA.Add("four");
            this._testOutputHelper.WriteLine("original JArray: {0}", jA.ToString());
            var e = jA.ElementAtOrDefault(3);
            Assert.NotNull(e);

            this._testOutputHelper.WriteLine("cloned JArray: {0}", jA_clone.ToString());
            var e_clone = jA_clone.ElementAtOrDefault(3);
            Assert.Null(e_clone);
        }

        [Theory, InlineData(3, @"{ ""x"": { ""y"": { ""z"": 3 } } }")]
        public void ShouldGetJObject(int expectedValue, string json)
        {
            var jZ = JObject.Parse(json)
                .GetJObject("x")
                .GetJObject("y");
            Assert.Equal(expectedValue, jZ.GetValue<int>("z"));
        }

        [Theory, InlineData(1, 2, @"{ ""items"": [ { ""x"": 1 }, { ""x"": 2 }, { ""x"": 3 } ] }")]
        public void ShouldGetJTokenFromJArray(int arrayIndex, int expectedValue, string json)
        {
            this._testOutputHelper.WriteLine("arrayIndex: {0}", arrayIndex);
            this._testOutputHelper.WriteLine("expectedValue: {0}", expectedValue);
            this._testOutputHelper.WriteLine("json: {0}", json);

            var jO = JObject.Parse(json);

            var token = jO.GetJTokenFromJArray("items", "x", arrayIndex, throwException : true);
            Assert.NotNull((token as JValue));

            var actualValue = token.GetValue<int>();
            this._testOutputHelper.WriteLine("actualValue: {0}", actualValue);
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory, InlineData(2, @"{ ""x"": 2 }")]
        public void ShouldGetValueFromJObject(int expectedValue, string json)
        {
            var jO = JObject.Parse(json);
            Assert.Equal(expectedValue, jO.GetValue<int>("x"));
        }

        [Theory, InlineData("X", @"{ ""x"": 2 }")]
        public void ShouldHaveProperty(string expectedPropertyValue, string json)
        {
            var jO = JObject.Parse(json);
            Assert.True(jO.HasProperty(expectedPropertyValue));
        }

        [Theory, InlineData(@"{ ""items"": [], ""otherItems"": null }")]
        public void ShouldNotGetJArray(string json)
        {
            this._testOutputHelper.WriteLine("json: {0}", json);

            var jO = JObject.Parse(json);

            var jA = jO.GetJArray("items", throwException : false);
            Assert.Null(jA);

            var jA_other = jO.GetJArray("otherItems", throwException : false);
            Assert.Null(jA_other);

            Assert.Throws<NullReferenceException>(() => jO.GetJArray("otherOtherItems"));
        }

        [Theory, InlineData(@"{ ""x"": { ""y"": { ""z"": 3 } } }")]
        public void ShouldNotGetJObject(string json)
        {
            Assert.Throws<NullReferenceException>(() => JObject.Parse(json).GetValue<int>("z"));
        }

        [Theory]
        [InlineData("{ \"childObject\": \"{}\" }")]
        [InlineData("{ \"childObject\": \"{ \\\"a\\\": true }\" }")]
        [InlineData("{ \"childObject\": \"{ \\\"a\\\": \\\"one\\\", \\\"b\\\": 23.3 }\" }")]
        public void ShouldParseJObject(string json)
        {
            var objectPropertyName = "childObject";
            var jO = JObject.Parse(json);
            var jO_child = jO.ParseJObject(objectPropertyName);

            this._testOutputHelper.WriteLine($"{nameof(json)}: {json}");
            this._testOutputHelper.WriteLine($"{nameof(jO_child)}:{Environment.NewLine}{jO_child.ToString()}");
        }

        readonly ITestOutputHelper _testOutputHelper;
    }
}