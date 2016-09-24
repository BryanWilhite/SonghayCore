using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Extensions;
using Songhay.Xml;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Songhay.Tests
{
    [TestClass]
    public class XmlTest
    {
        [TestInitialize]
        public void InitializeTest()
        {
            #region remove previous test results:

            var directory = Directory.GetParent(TestContext.TestDir);

            directory.GetFiles()
                .Where(f => f.Extension != ".ldf")
                .Where(f => f.Extension != ".mdf")
                .OrderByDescending(f => f.LastAccessTime).Skip(1)
                .ForEachInEnumerable(f => f.Delete());

            directory.GetDirectories()
                .OrderByDescending(d => d.LastAccessTime).Skip(1)
                .ForEachInEnumerable(d => d.Delete(true));

            #endregion

        }

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
    }
}
