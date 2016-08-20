using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Extensions;
using Songhay.Models;
using Songhay.Xml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Songhay.Tests
{
    /// <summary>
    /// Summary description for XmlTest
    /// </summary>
    [TestClass]
    public class XmlTest
    {
        /// <summary>
        /// Initializes the test.
        /// </summary>
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

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        public class OneTwoThree
        {
            public string One { get; set; }
            public string Two { get; set; }
            public string Three { get; set; }
        }

        [TestMethod]
        public void ShouldCombineRssFeeds()
        {
            //reference: http://stackoverflow.com/questions/79197/combining-two-syndicationfeeds

            SyndicationFeed feed;
            SyndicationFeed feed2;

            var feedUri = new Uri("http://stackoverflow.com/feeds/tag/silverlight");
            using (var reader = XmlReader.Create(feedUri.AbsoluteUri))
            {
                feed = SyndicationFeed.Load(reader);
            }

            Assert.IsTrue(feed.Items.Count() > 0, "The expected feed items are not here.");

            var feed2Uri = new Uri("http://stackoverflow.com/feeds/tag/wpf");
            using (var reader2 = XmlReader.Create(feed2Uri.AbsoluteUri))
            {
                feed2 = SyndicationFeed.Load(reader2);
            }

            Assert.IsTrue(feed2.Items.Count() > 0, "The expected feed items are not here.");

            var feedsCombined = new SyndicationFeed(feed.Items.Union(feed2.Items));

            Assert.IsTrue(
                feedsCombined.Items.Count() == feed.Items.Count() + feed2.Items.Count(),
                "The expected number of combined feed items are not here.");

            var builder = new StringBuilder();
            using (var writer = XmlWriter.Create(builder))
            {
                feedsCombined.SaveAsRss20(writer);
                writer.Flush();
                writer.Close();
            }

            var xmlString = builder.ToString();

            Assert.IsTrue(new Func<bool>(
                () =>
                {
                    var test = false;

                    var xDoc = XDocument.Parse(xmlString);
                    var count = xDoc.Root.Element("channel").Elements("item").Count();
                    test = (count == feedsCombined.Items.Count());

                    return test;
                }
            ).Invoke(), "The expected number of RSS items are not here.");
        }

        [TestMethod]
        public void ShouldGenerateRssFeed()
        {
            //reference: [http://dotnetslackers.com/articles/aspnet/How-to-create-a-syndication-feed-for-your-website.aspx]

            var items = new List<SyndicationItem>
            {
                new SyndicationItem
                {
                    Content = TextSyndicationContent.CreatePlaintextContent("This is plain test content for first item."),
                    PublishDate = DateTime.Now,
                    Summary = TextSyndicationContent.CreatePlaintextContent("Item summary for first item…"),
                    Title = TextSyndicationContent.CreatePlaintextContent("First Item Title")
                },
                new SyndicationItem
                {
                    Content = TextSyndicationContent.CreatePlaintextContent("This is plain test content for second item."),
                    PublishDate = DateTime.Now,
                    Summary = TextSyndicationContent.CreatePlaintextContent("Item summary for second item…"),
                    Title = TextSyndicationContent.CreatePlaintextContent("Second Item Title")
                },
                new SyndicationItem
                {
                    Content = TextSyndicationContent.CreateXhtmlContent("This is <strong>XHTML</strong> test content for <em>third</em> item."),
                    PublishDate = DateTime.Now,
                    Summary = TextSyndicationContent.CreatePlaintextContent("Item summary for third item…"),
                    Title = TextSyndicationContent.CreatePlaintextContent("Third Item Title")
                }
            };

            var feed = new SyndicationFeed(items)
            {
                Description = TextSyndicationContent.CreatePlaintextContent("My feed description."),
                Title = TextSyndicationContent.CreatePlaintextContent("My Feed")
            };

            feed.Authors.Add(new SyndicationPerson
            {
                Email = "rasx@songhaysystem.com",
                Name = "Bryan Wilhite",
                Uri = "http://SonghaySystem.com"
            });

            Assert.IsTrue((new List<SyndicationItem>(feed.Items)).Count == 3, "The expected number of Syndication items is not here.");

            feed.Items.ForEachInEnumerable(i =>
            {
                i.Authors.Add(feed.Authors.First());
            });

            var formatter = new Rss20FeedFormatter(feed);

            var settings = new XmlWriterSettings
            {
                CheckCharacters = true,
                CloseOutput = true,
                ConformanceLevel = ConformanceLevel.Document,
                Encoding = Encoding.UTF8,
                Indent = true,
                IndentChars = "    ",
                NamespaceHandling = NamespaceHandling.OmitDuplicates,
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace,
                NewLineOnAttributes = true,
                OmitXmlDeclaration = false
            };

            var buffer = new StringBuilder();
            var output = string.Empty;
            using (var writer = XmlWriter.Create(buffer, settings))
            {
                formatter.WriteTo(writer); // or feed.SaveAsRss20(writer);
                writer.Flush();
                writer.Close();
                output = buffer.ToString();
            }
            TestContext.WriteLine(output);

            Assert.IsTrue(!output.Equals(string.Empty), "The expected output is not here.");

        }

        [TestMethod]
        public void ShouldGetPastebinList()
        {
            var uri = new Uri("https://pastebin.com/api/api_login.php", UriKind.Absolute);
            var postData = new Hashtable
            {
                {"api_dev_key", "8d70f5c9a8df4689262caa2a8d3fbee4"},
                {"api_user_name", "rasx"},
                {"api_user_password", "thr33ess"},
            };
            var sessionKey = WebRequest.CreateHttp(uri).PostForm(uri, postData);
            Assert.IsFalse(string.IsNullOrEmpty(sessionKey));

            uri = new Uri("http://pastebin.com/api/api_post.php", UriKind.Absolute);
            postData = new Hashtable
            {
                {"api_dev_key", "8d70f5c9a8df4689262caa2a8d3fbee4"},
                {"api_user_key", sessionKey},
                {"api_results_limit", "10"},
                {"api_option", "list"},
            };
            var response = WebRequest.CreateHttp(uri).PostForm(uri, postData);
            Assert.IsFalse(string.IsNullOrEmpty(response));
        }

        /// <summary>
        /// Should read folder of XHTML files and write index.
        /// </summary>
        [TestMethod]

        [TestProperty("indexFileName", "CssIndex.xml")]
        [TestProperty("indexTitle", "CSS Samples")]
        [TestProperty("pathToFolder", @"AzureBlobStorage-songhay\samples-css\")]
        [TestProperty("publicRoot", "http://songhay.blob.core.windows.net/samples-css/")]

        //[TestProperty("indexFileName", "JQueryIndex.xml")]
        //[TestProperty("indexTitle", "JQuery Samples")]
        //[TestProperty("pathToFolder", @"AzureBlobStorage-songhay\samples-jquery\")]
        //[TestProperty("publicRoot", "http://songhay.blob.core.windows.net/samples-jquery/")]

        [TestProperty("pathToOutput", @"Songhay.Web.ServerIndex\App_Data\Samples\")]
        public void ShouldReadFolderOfXhtmlFilesAndWriteIndex()
        {
            var projectsFolder = this.TestContext.ShouldGetProjectsFolder(this.GetType());

            var indexFileName = this.TestContext.Properties["indexFileName"].ToString();
            var indexTitle = this.TestContext.Properties["indexTitle"].ToString();
            var pathToFolder = this.TestContext.Properties["pathToFolder"].ToString();
            pathToFolder = Path.Combine(projectsFolder, pathToFolder);
            var pathToOutput = this.TestContext.Properties["pathToOutput"].ToString();
            pathToOutput = Path.Combine(projectsFolder, pathToOutput);
            var publicRoot = this.TestContext.Properties["publicRoot"].ToString();

            XhtmlDocumentUtility.WriteDocumentIndex(indexFileName, indexTitle,
                publicRoot, pathToFolder, pathToOutput);
        }

        [TestMethod]
        [TestProperty("pathToInput", @"Songhay.Web.Mvc.ServerIndex\App_Data\Samples\CssIndex.xml")]
        public void ShouldReadIndex()
        {
            var projectsFolder = this.TestContext.ShouldGetProjectsFolder(this.GetType());
            var pathToInput = this.TestContext.Properties["pathToInput"].ToString();
            pathToInput = Path.Combine(projectsFolder, pathToInput);
            this.TestContext.ShouldFindFile(pathToInput);

            var instance = XmlUtility.GetInstance<XhtmlDocuments>(pathToInput) as XhtmlDocuments;
            Assert.IsNotNull(instance);
        }

        /// <summary>
        /// Should serialize the object.
        /// </summary>
        /// <remarks>
        ///     “System.Xml.XmlSerializer”
        ///     [http://www.eggheadcafe.com/articles/system.xml.xmlserialization.asp]
        /// </remarks>
        [TestMethod]
        [Description("Should serialize object.")]
        public void ShouldSerializeObject()
        {
            var data = new OneTwoThree
            {
                One = "uno",
                Two = "dos",
                Three = "tres"
            };

            var elementTest = new Action<XDocument, string>((xd, elementName) =>
            {
                Assert.AreEqual(elementName,
                    xd.Root.Element(elementName).Name,
                    "The expected element name is not here.");
            });

            var serializer = new XmlSerializer(typeof(OneTwoThree));
            MemoryStream stream = new MemoryStream();
            try
            {
                using (var writer = new XmlTextWriter(stream, Encoding.UTF8))
                {
                    serializer.Serialize(stream, data);
                    var xml = Encoding.UTF8.GetString(stream.ToArray());

                    var xd = XDocument.Parse(xml);
                    Assert.AreEqual("OneTwoThree", xd.Root.Name);
                    elementTest.Invoke(xd, "One");
                    elementTest.Invoke(xd, "Two");
                    elementTest.Invoke(xd, "Three");
                }
            }
            finally
            {
                if (stream != null) stream.Dispose();
            }
        }

        [TestMethod]
        [Description("Should write to memory Stream.")]
        public void ShouldWriteToMemoryStream()
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
