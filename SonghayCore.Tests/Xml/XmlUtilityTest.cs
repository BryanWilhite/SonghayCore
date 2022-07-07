using Songhay.Xml;
using System.IO;
using System.Text;
using System.Xml;
using Xunit;

namespace Songhay.Tests;

public class XmlUtilityTest
{
    [Fact]
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
            Assert.NotNull(doc);

            var nav = doc.CreateNavigator();
            nav.MoveToFirstChild();
            Assert.Equal("DocumentData", nav.LocalName);
        }
        finally
        {
            ms?.Dispose();
        }
    }
}