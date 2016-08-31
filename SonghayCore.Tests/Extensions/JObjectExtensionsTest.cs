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
    }
}
