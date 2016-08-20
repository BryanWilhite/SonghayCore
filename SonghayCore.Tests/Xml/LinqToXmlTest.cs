using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Extensions;
using Songhay.Xml;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Songhay.Tests
{
    /// <summary>
    /// Summary description for LinqToXmlTest
    /// </summary>
    [TestClass]
    public class LinqToXmlTest
    {
        [TestInitialize]
        public void ClearPreviousTestResults()
        {
            var directory = Directory.GetParent(TestContext.TestDir);

            directory.GetFiles()
                .OrderByDescending(f => f.LastAccessTime).Skip(1)
                .ForEachInEnumerable(f => f.Delete());

            directory.GetDirectories()
                .OrderByDescending(d => d.LastAccessTime).Skip(1)
                .ForEachInEnumerable(d => d.Delete(true));
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        [Description("Should match XPath for descending elements.")]
        public void ShouldMatchXPathForDescendingElements()
        {
            var xml = @"
<root>
    <a>
        <b>
            <c>a text node</c>
        </b>
    </a>
</root>
";
            var xd = XDocument.Parse(xml);

            var actual = XObjectUtility.GetXNode(xd.Root, "/root/a/b/c") as XElement;
            Assert.IsNotNull(actual, "The XPath did not return an XElement.");

            var expected = xd.Elements("root")
                .Elements("a")
                .Elements("b")
                .Elements("c")
                .FirstOrDefault();
            Assert.IsNotNull(expected, "The LINQ assertion did not return an XElement.");

            Assert.AreEqual(expected, actual, "The XElement values are not equal.");

            var c = xd.Elements("root");
            Assert.IsNotNull(c);

            var attr = c.FirstOrDefault().Attribute("foo");
            Assert.IsNull(attr);
        }

        [TestMethod]
        [TestProperty("uri", "http://www.weather.gov/xml/current_obs/KLAX.xml")]
        public async Task ShouldGetRemoteXmlAsync()
        {
            var uri = this.TestContext.Properties["uri"].ToString();

            using (var client = new HttpClient())
            {
                var result = await client.GetStringAsync(uri);
                var remoteDocument = XDocument.Parse(result);
                var isCorrectAssertion = (remoteDocument.Root.Name == "current_observation");
                Assert.IsTrue(isCorrectAssertion, "The expected Root Name is not here.");
            }
        }

        [TestMethod]
        [Description("Should use empty namespaces.")]
        public void ShouldUseEmptyNamespaces()
        {
            var xml = @"
<root>
    <a>
        <b>
            <c>a text node</c>
        </b>
    </a>
</root>
";
            var xd = XDocument.Parse(xml);

            XNamespace empty = string.Empty;

            var c = xd
                .Elements(empty + "root")
                .Elements(empty + "a")
                .Elements(empty + "b")
                .Elements(empty + "c");
            Assert.IsNotNull(c);

            Assert.AreEqual(c.FirstOrDefault(),
                xd.Descendants(empty + "c").FirstOrDefault());
        }
    }
}
