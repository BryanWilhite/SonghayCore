using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Songhay.Tests
{
    /// <summary>
    /// Defines tests for <see cref="Songhay.Xml.XObjectUtility"/>.
    /// </summary>
    [TestClass]
    public class XObjectUtilityTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        /// Should clone the element.
        /// </summary>
        /// <remarks>
        /// See “Empty Elements and Self-Closing Tags”
        /// (http://blogs.msdn.com/b/ericwhite/archive/2009/07/08/empty-elements-and-self-closing-tags.aspx)
        /// </remarks>
        [TestMethod]
        [Description("Should clone element.")]
        public void ShouldCloneElement()
        {
            XElement root = XElement.Parse("<Root></Root>");
            TestContext.WriteLine("Original tree: {0}", root);
            XElement rootClone = CloneElement(root);
            TestContext.WriteLine("Cloned tree: {0}", rootClone);
            Assert.AreEqual("<Root />", rootClone.ToString(), "Default clone result not equal.");
            XElement rootCloneWithEmptyTags = CloneElement(root, renderEmptyTags: true);
            TestContext.WriteLine("Cloned tree with empty tags: {0}", rootCloneWithEmptyTags);
            Assert.AreEqual(root.ToString(), rootCloneWithEmptyTags.ToString(), "Clone result with empty tags not equal.");
        }

        [TestMethod]
        [Description("Should write outer XML.")]
        public void ShouldWriteOuterXml()
        {
            var e = new XElement("parent", new XElement("child"));
            TestContext.WriteLine(e.ToString());
        }

        static XElement CloneElement(XElement element, bool renderEmptyTags = false)
        {
            var isEmptyElement = new Func<XElement, bool>((xe) =>
            {
                return !xe.IsEmpty && !xe.Nodes().OfType<XText>().Any();
            });

            return new XElement(element.Name,
                element.Attributes(),
                element.Nodes().Select(n =>
                {
                    XElement e = n as XElement;
                    if(e != null) return CloneElement(e, renderEmptyTags);
                    return n;
                }),
                (isEmptyElement.Invoke(element) && renderEmptyTags) ? string.Empty : null
            );
        }
    }
}
