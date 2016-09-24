using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Xml;
using System.Linq;
using System.Xml.Linq;

namespace Songhay.Tests
{
    /// <summary>
    /// Summary description for LinqToXmlTest
    /// </summary>
    [TestClass]
    public class LinqToXmlTest
    {
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
    }
}
