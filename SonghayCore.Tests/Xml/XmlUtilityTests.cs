using System.Text;
using System.Xml;
using System.Xml.XPath;
using Songhay.Xml;

namespace Songhay.Tests.Xml;

public class XmlUtilityTests
{
    [Fact]
    public void ShouldGetNavigableDocument()
    {
        MemoryStream ms = new();

        try
        {
            using (XmlWriter writer = XmlWriter.Create(ms, new XmlWriterSettings
                   {
                       Encoding = Encoding.UTF8
                   }))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("DocumentData");
                writer.WriteFullEndElement();
                writer.WriteEndDocument();
                writer.Flush();
            }

            XPathDocument? doc = XmlUtility.GetNavigableDocument(ms);
            Assert.NotNull(doc);

            XPathNavigator nav = doc!.CreateNavigator();
            nav.MoveToFirstChild();
            Assert.Equal("DocumentData", nav.LocalName);
        }
        finally
        {
            ms.Dispose();
        }
    }

    [Theory]
    [InlineData("", "")]
    [InlineData(null, null)]
    [InlineData("&lt;one/&gt;", "<one/>")]
    public void XmlDecode_Test(string? input, string? expected)
    {
        string? actual = XmlUtility.XmlDecode(input);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData(null, null)]
    [InlineData("<one/>", "&lt;one/&gt;")]
    public void XmlEncode_Test(string? input, string? expected)
    {
        string? actual = XmlUtility.XmlEncode(input);

        Assert.Equal(expected, actual);
    }
}
