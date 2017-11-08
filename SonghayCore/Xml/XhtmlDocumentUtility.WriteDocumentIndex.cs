using Songhay.Extensions;
using Songhay.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Songhay.Xml
{
    /// <summary>
    /// Static members for XHTML Documents.
    /// </summary>
    public static partial class XhtmlDocumentUtility
    {
        /// <summary>
        /// Writes the index of XHTML documents.
        /// </summary>
        /// <param name="indexFileName">Name of the index file.</param>
        /// <param name="indexTitle">The index title.</param>
        /// <param name="publicRoot">The public root.</param>
        /// <param name="pathToDirectory">The path to the specified directory.</param>
        /// <param name="pathToOutput">The path to output.</param>
        public static void WriteDocumentIndex(string indexFileName,
            string indexTitle, string publicRoot,
            string pathToDirectory, string pathToOutput)
        {
            var directory = new DirectoryInfo(pathToDirectory);
            var list = new List<XhtmlDocument>();
            directory.GetFiles()
                .ForEachInEnumerable(f =>
                {
                    var uri = string.Concat(publicRoot, f.Name);
                    list.Add(XhtmlDocumentUtility.LoadDocument(f.FullName, uri));
                });

            var serializer = new XmlSerializer(typeof(XhtmlDocuments));
            using(var writer = new XmlTextWriter(string.Concat(pathToOutput, indexFileName), Encoding.UTF8))
            {
                var documents = new XhtmlDocuments
                {
                    Documents = list.OrderBy(d => d.Title).ToArray(),
                    Title = indexTitle
                };
                serializer.Serialize(writer, documents);
            }
        }
    }
}
