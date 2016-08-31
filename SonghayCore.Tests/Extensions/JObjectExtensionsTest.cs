using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Songhay.Extensions.Tests
{
    using Newtonsoft.Json.Linq;
    using Songhay.Extensions;

    /// <summary>
    /// Tests for extensions of <see cref="System.DateTime"/>.
    /// </summary>
    [TestClass]
    public class JObjectExtensionsTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        [TestProperty("json", @"{ ""data"": { ""one"":""uno"", ""two"":""dos"" } }")]
        public void ShouldGetDictionaryOfStrings()
        {
            var json = this.TestContext.Properties["json"].ToString();
            this.TestContext.WriteLine("json: {0}", json);

            var jO = JObject.Parse(json);

            var data = jO.GetDictionary("data", throwException: true);
            this.TestContext.WriteLine("Dictionary keys: {0}", string.Join(",", data.Keys.ToArray()));
        }

        [TestMethod]
        [TestProperty("json", @"{ ""one"":[""uno"", ""un""], ""two"":[""dos"", ""deux""] }")]
        public void ShouldGetDictionaryOfArrayOfStrings()
        {
            var json = this.TestContext.Properties["json"].ToString();
            this.TestContext.WriteLine("json: {0}", json);

            var jO = JObject.Parse(json);

            var data = jO.GetDictionary(throwException: true);
            this.TestContext.WriteLine("Dictionary keys: {0}", string.Join(",", data.Keys.ToArray()));
        }

        [TestMethod]
        [TestProperty("json", @"{ ""items"": [ ""one"", ""two"", ""three"" ] }")]
        public void ShouldGetJArray()
        {
            var json = this.TestContext.Properties["json"].ToString();
            this.TestContext.WriteLine("json: {0}", json);

            var jO = JObject.Parse(json);

            var jA = jO.GetJArray("items", throwException: true);
            var jA_clone = jA.DeepClone();

            jA.Add("four");
            this.TestContext.WriteLine("original JArray: {0}", jA.ToString());
            var e = jA.ElementAtOrDefault(3);
            Assert.IsNotNull(e, "The expected 4th element is not here.");

            this.TestContext.WriteLine("cloned JArray: {0}", jA_clone.ToString());
            var e_clone = jA_clone.ElementAtOrDefault(3);
            Assert.IsNull(e_clone, "The 4th element is not expected.");
        }

        [TestMethod]
        [TestProperty("arrayIndex", "1")]
        [TestProperty("expectedValue", "2")]
        [TestProperty("json", @"{ ""items"": [ { ""x"": 1 }, { ""x"": 2 }, { ""x"": 3 } ] }")]
        public void ShouldGetJTokenFromJArray()
        {
            #region test properties:

            var arrayIndex = Convert.ToInt32(this.TestContext.Properties["arrayIndex"]);
            var expectedValue = Convert.ToInt32(this.TestContext.Properties["expectedValue"]);
            var json = this.TestContext.Properties["json"].ToString();

            #endregion

            this.TestContext.WriteLine("arrayIndex: {0}", arrayIndex);
            this.TestContext.WriteLine("expectedValue: {0}", expectedValue);
            this.TestContext.WriteLine("json: {0}", json);

            var jO = JObject.Parse(json);

            var token = jO.GetJTokenFromJArray("items", "x", arrayIndex, throwException: true);
            Assert.IsNotNull((token as JValue), "The expected JValue is not here.");

            var actualValue = (token as JValue).Value<int>();
            this.TestContext.WriteLine("actualValue: {0}", actualValue);
            Assert.AreEqual(expectedValue, actualValue, "The expected JObject value is not here.");
        }

        [TestMethod]
        [TestProperty("json", @"{ ""items"": [], ""otherItems"": null }")]
        public void ShouldNotGetJArray()
        {
            var json = this.TestContext.Properties["json"].ToString();
            this.TestContext.WriteLine("json: {0}", json);

            var jO = JObject.Parse(json);

            var jA = jO.GetJArray("items", throwException: false);
            Assert.IsNull(jA, "JArray is not expected.");

            var jA_other = jO.GetJArray("otherItems", throwException: false);
            Assert.IsNull(jA_other, "JArray is not expected.");
        }
    }
}
