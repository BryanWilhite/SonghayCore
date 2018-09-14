using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Extensions;
using Songhay.Models;
using System;
using System.Collections.Generic;

namespace Songhay.Tests.Extensions
{

    /// <summary>
    /// Tests for <see cref="StringExtensions"/>
    /// </summary>
    [TestClass]
    public class RestApiMetadataExtensionsTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void ShouldConvertToUri()
        {
            var key = "weather-template";
            var meta = new RestApiMetadata
            {
                ApiBase = new Uri("https://my.api.me", UriKind.Absolute),
                UriTemplates = new Dictionary<string, string>
                {
                    { key, "weather/{state}/{city}?forecast={day}" },
                }
            };

            this.TestContext.WriteLine(meta.ToUri(key, "Washington", "Redmond", "Today").OriginalString);
        }
    }
}
