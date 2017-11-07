using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Xml;
using System.Linq;
using System.Xml.Linq;

namespace Songhay.Tests
{
    /// <summary>
    /// Defines tests for <see cref="Xml.XObjectUtility"/>.
    /// </summary>
    [TestClass]
    public class XObjectUtilityTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        [TestProperty("expectedValue", "one-text-node-child-at-the-end")]
        [TestProperty("sampleOne", "<root><one>one-text-node<one-child>-child</one-child>-at-the-end</one></root>")]
        public void ShouldJoinFlattenedXTextNodes()
        {
            #region test properties:

            var expectedValue = this.TestContext.Properties["expectedValue"].ToString();
            var sampleOne = this.TestContext.Properties["sampleOne"].ToString();

            #endregion

            var rootElement = XElement.Parse(sampleOne);
            this.TestContext.WriteLine(rootElement.ToString());

            var actual = XObjectUtility.JoinFlattenedXTextNodes(rootElement);
            this.TestContext.WriteLine("actual: {0}", actual);
            Assert.AreEqual(expectedValue, actual, "The expected value is not here.");
        }

        [TestMethod]
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
