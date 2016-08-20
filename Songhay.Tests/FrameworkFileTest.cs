using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Songhay.Tests
{
    /// <summary>
    /// General research tests.
    /// </summary>
    [TestClass]
    public class FrameworkFileTest
    {
        /// <summary>
        /// Initializes the test.
        /// </summary>
        [TestInitialize]
        public void InitializeTest()
        {
            this.TestContext.RemovePreviousTestResults();
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        /// Framework file test: should find path of given length.
        /// </summary>
        [TestMethod]
        public void ShouldFindPathOfGivenLength()
        {
            var projectsFolder = this.TestContext.ShouldGetProjectsFolder(this.GetType());

            var directory = new DirectoryInfo(projectsFolder);
            var dict = new Dictionary<string, int>();
            AddPathsToList(directory, dict);
            dict.ForEachInEnumerable(d =>
                {
                    if (d.Value > 200)
                        TestContext
                            .WriteLine(string.Format("{0}:{1}", d.Key, d.Value.ToString()));
                });
        }

        void AddPathsToList(DirectoryInfo info, Dictionary<string, int> dict)
        {
            info.GetDirectories()
                .ForEachInEnumerable(d =>
                {
                    dict.Add(d.FullName, d.FullName.Length);
                    d.GetDirectories()
                        .ForEachInEnumerable(d2 =>
                        {
                            AddPathsToList(d2, dict);
                        });
                });
        }

        /// <summary>
        /// Framework file test: should sort text file data.
        /// </summary>
        [DeploymentItem("FrameworkFileTest-ShouldSortTextFileData.txt")]
        [TestMethod]
        public void ShouldSortTextFileData()
        {
            var output = new List<string>();

            var path = string.Format(@"{0}\FrameworkFileTest-ShouldSortTextFileData.txt", TestContext.DeploymentDirectory);

            var stream = new FileStream(path, FileMode.Open);
            try
            {
                using (var sr = new StreamReader(stream))
                {
                    var dataArray = sr.ReadToEnd().Replace(Environment.NewLine, ",").Split(',');
                    dataArray.Select(s => new { Car = s, Count = dataArray.Where(i => i == s).Count() })
                        .OrderByDescending(o => o.Count).ThenBy(o => o.Car)
                        .GroupBy(o => o.Car)
                        .ForEachInEnumerable(o =>
                    {
                        TestContext.WriteLine(o.Key + " = " + o.Count().ToString());
                    });
                }
            }
            finally
            {
                if (stream != null) stream.Dispose();
            }
        }

        /// <summary>
        /// Framework file test: should write text file with stream writer.
        /// </summary>
        [TestMethod]
        public void ShouldWriteTextFileWithStreamWriter()
        {
            var projectsFolder = this.TestContext.ShouldGetProjectsFolder(this.GetType());
            var fileText = @"
This is the text to write to the test file.
This sentence should be on a line all by iteself.
According to Notepad++ this file should be in ANSI format by default.
According to Notepad++, when Encoding.UTF8 is specified the encoding is UTF8.
This is the end of the file.
            ";
            var path = Path.Combine(projectsFolder, "FrameworkFileTest-ShouldWriteTextFileWithStreamWriter.txt");

            var stream = new FileStream(path, FileMode.Create);
            try
            {
                using (var sw = new StreamWriter(stream, Encoding.UTF8))
                {
                    sw.Write(fileText);
                }
            }
            finally
            {
                if (stream != null) stream.Dispose();
            }
        }

        /// <summary>
        /// Framework file test: should write text file with XML text writer.
        /// </summary>
        [TestMethod]
        public void ShouldWriteTextFileWithXmlTextWriter()
        {
            var projectsFolder = this.TestContext.ShouldGetProjectsFolder(this.GetType());
            var path = Path.Combine(projectsFolder, "FrameworkFileTest-ShouldWriteTextFileWithXmlTextWriter.xml");
            var stream = new FileStream(path, FileMode.Create);
            try
            {
                using (var writer = new XmlTextWriter(stream, Encoding.UTF8))
                {
                    writer.WriteStartElement("root");
                    writer.WriteAttributeString("xmlns", "x", null, "urn:1");
                    writer.WriteStartElement("item", "urn:1");
                    writer.WriteString("This is the text to write to the test file.");
                    writer.WriteEndElement();
                    writer.WriteStartElement("item", "urn:1");
                    writer.WriteString("According to Notepad++, when Encoding.UTF8 is specified the encoding is UTF8.");
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
            }
            finally
            {
                if (stream != null) stream.Dispose();
            }
        }

        /// <summary>
        /// Framework file test: should write to my documents folder.
        /// </summary>
        [TestMethod]
        public void ShouldWriteToMyDocumentsFolder()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                @"\WordWalkingStick\";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }
    }
}
