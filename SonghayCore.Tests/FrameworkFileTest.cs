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
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        /// Framework file test: should find path of given length.
        /// </summary>
        [TestMethod]
        [TestProperty("length", "200")]
        public void ShouldFindPathOfGivenLength()
        {
            var projectsFolder = this.TestContext.ShouldGetAssemblyDirectoryParent(this.GetType(), expectedLevels: 3);
            var length = Convert.ToInt32(this.TestContext.Properties["length"]);

            var directory = new DirectoryInfo(projectsFolder);
            var dict = new Dictionary<string, int>();
            AddPathsToList(directory, dict);
            dict.ForEachInEnumerable(d =>
                {
                    if (d.Value > length)
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
        [TestMethod]
        [TestProperty("outFile", @"content\FrameworkFileTest-ShouldSortTextFileData.txt")]
        public void ShouldSortTextFileData()
        {
            var projectsFolder = this.TestContext.ShouldGetAssemblyDirectoryParent(this.GetType(), expectedLevels: 3);

            #region test properties:

            var outFile = this.TestContext.Properties["outFile"].ToString();
            outFile = Path.Combine(projectsFolder, this.GetType().Namespace, outFile);
            this.TestContext.ShouldFindFile(outFile);

            #endregion

            var output = new List<string>();
            var stream = new FileStream(outFile, FileMode.Open);
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
        [TestProperty("outFile", @"content\FrameworkFileTest-ShouldWriteTextFileWithStreamWriter.txt")]
        public void ShouldWriteTextFileWithStreamWriter()
        {
            var projectsFolder = this.TestContext.ShouldGetAssemblyDirectoryParent(this.GetType(), expectedLevels: 3);

            #region test properties:

            var outFile = this.TestContext.Properties["outFile"].ToString();
            outFile = Path.Combine(projectsFolder, this.GetType().Namespace, outFile);
            this.TestContext.ShouldFindFile(outFile);

            #endregion

            var fileText = @"
This is the text to write to the test file.
This sentence should be on a line all by itself.
According to Notepad++ this file should be in ANSI format by default.
According to Notepad++, when Encoding.UTF8 is specified the encoding is UTF8.
This is the end of the file.
            ";

            var stream = new FileStream(outFile, FileMode.Create);
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
        [TestProperty("outFile", @"content\FrameworkFileTest-ShouldWriteTextFileWithXmlTextWriter.xml")]
        public void ShouldWriteTextFileWithXmlTextWriter()
        {
            var projectsFolder = this.TestContext.ShouldGetAssemblyDirectoryParent(this.GetType(), expectedLevels: 3);

            #region test properties:

            var outFile = this.TestContext.Properties["outFile"].ToString();
            outFile = Path.Combine(projectsFolder, this.GetType().Namespace, outFile);
            this.TestContext.ShouldFindFile(outFile);

            #endregion

            var stream = new FileStream(outFile, FileMode.Create);
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
        [TestProperty("folder", "TestMyDocumentsFolder")]
        public void ShouldWriteToMyDocumentsFolder()
        {
            var folder = this.TestContext.Properties["folder"].ToString();

            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), folder);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            this.TestContext.ShouldFindFolder(path);
        }
    }
}
