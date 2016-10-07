using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Xml;
using System.IO;
using System.Text;
using System.Xml;

namespace Songhay.Tests
{
    /// <summary>
    /// Summary description for LinqToXmlTest
    /// </summary>
    [TestClass]
    public class XmlUtilityTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void ShouldGetNavigableDocument()
        {
            var ms = new MemoryStream();
            try
            {
                using (var writer = XmlWriter.Create(ms, new XmlWriterSettings
                {
                    Encoding = UTF8Encoding.UTF8
                }))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("DocumentData");
                    writer.WriteFullEndElement();
                    writer.WriteEndDocument();
                    writer.Flush();
                }

                var doc = XmlUtility.GetNavigableDocument(ms);
                Assert.IsNotNull(doc, "The expected navigable document is not here.");

                var nav = doc.CreateNavigator();
                nav.MoveToFirstChild();
                Assert.AreEqual<string>("DocumentData", nav.LocalName, "The expected root node is not here.");
            }
            finally
            {
                if (ms != null) ms.Dispose();
            }
        }
    }
}
