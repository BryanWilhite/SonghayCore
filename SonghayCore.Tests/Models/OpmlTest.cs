using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Extensions;
using Songhay.Xml;
using System;
using System.IO;
using System.Linq;

namespace Songhay.Tests
{
    /// <summary>
    /// Summary description for OpmlTest
    /// </summary>
    [TestClass]
    public class OpmlTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        [TestProperty("opmlFile", @"Models\OpmlFromInfoPath.xml")]
        public void ShouldFilterCategory()
        {
            var projectFolder = this.TestContext
                .ShouldGetAssemblyDirectoryInfo(this.GetType()) // netcoreapp2.0
                .Parent // Debug or Release
                .Parent // bin
                .Parent.FullName;

            var opmlFile = this.TestContext.Properties["opmlFile"].ToString();

            var path = Path.Combine(projectFolder, opmlFile);
            this.TestContext.ShouldFindFile(path);

            var data = OpmlUtility.GetDocument(path);

            Assert.IsTrue(data.OpmlBody.Outlines.Length == 4);

            data.OpmlBody.Outlines = data.OpmlBody.Outlines
                .Where(o => o.Category != "private").ToArray();

            Assert.IsTrue(data.OpmlBody.Outlines.Length == 3);
        }

        [TestMethod]
        [TestProperty("opmlFile", @"Models\OpmlTest.opml")]
        public void ShouldLoadCategoriesAndResources()
        {
            var projectFolder = this.TestContext.ShouldGetAssemblyDirectoryParent(this.GetType(), expectedLevels: 3);

            var opmlFile = this.TestContext.Properties["opmlFile"].ToString();

            var path = Path.Combine(projectFolder, opmlFile);
            this.TestContext.ShouldFindFile(path);

            var data = OpmlUtility.GetDocument(path);
            Assert.IsNotNull(data, "The expected OPML data is not here.");

            //XPATH: ./outline[not(@url)]
            var categories = data.OpmlBody.Outlines
                .Where(outline => outline.Url == null);

            categories.ForEachInEnumerable(category =>
                TestContext.WriteLine("Category: " + category.Text));

            //XPATH./outline[@type="link"]
            var resources = categories.First().Outlines
                .Where(outline => outline.OutlineType == "link");

            resources.ForEachInEnumerable(resource =>
                TestContext.WriteLine("Resource: " + resource.Text));
        }

        [TestMethod]
        [TestProperty("opmlFile", @"Models\OpmlFromInfoPath.xml")]
        public void ShouldLoadDocument()
        {
            var projectFolder = this.TestContext
                .ShouldGetAssemblyDirectoryInfo(this.GetType()) // netcoreapp2.0
                .Parent // Debug or Release
                .Parent // bin
                .Parent.FullName;

            var opmlFile = this.TestContext.Properties["opmlFile"].ToString();

            var path = Path.Combine(projectFolder, opmlFile);
            this.TestContext.ShouldFindFile(path);

            var data = OpmlUtility.GetDocument(path);

            var expected = "Development Server";
            var actual = data.OpmlHead.Title;
            Assert.AreEqual(expected, actual);

            expected = "LINQ to Entities Paging";
            actual = data.OpmlBody.Outlines
                .Where(o => o.Text == "Samples")
                .First().Outlines.First().Text;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestProperty("opmlFile", @"Models\OpmlTest.opml")]
        public void ShouldWriteDateModified()
        {
            var projectFolder = this.TestContext
                .ShouldGetAssemblyDirectoryInfo(this.GetType()) // netcoreapp2.0
                .Parent // Debug or Release
                .Parent // bin
                .Parent.FullName;

            var opmlFile = this.TestContext.Properties["opmlFile"].ToString();

            var path = Path.Combine(projectFolder, opmlFile);
            this.TestContext.ShouldFindFile(path);

            var data = OpmlUtility.GetDocument(path);
            DateTime? date = DateTime.Now;
            data.OpmlHead.DateModified = date;
            XmlUtility.Write(data, path);

            data = OpmlUtility.GetDocument(path);
            var actualDate = data.OpmlHead.DateModified;
            Assert.AreEqual(date.Value.Day, actualDate.Value.Day);
            Assert.AreEqual(date.Value.Hour, actualDate.Value.Hour);
            Assert.AreEqual(date.Value.Minute, actualDate.Value.Minute);
        }
    }
}
