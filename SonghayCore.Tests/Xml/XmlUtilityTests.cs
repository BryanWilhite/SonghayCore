using System.Text;
using System.Xml;
using Songhay.Xml;

namespace Songhay.Tests.Xml;

public class XmlUtilityTests
{
    [Fact]
    public void ShouldGetNavigableDocument()
    {
        var ms = new MemoryStream();
        try
        {
            using (var writer = XmlWriter.Create(ms, new XmlWriterSettings
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

            var doc = XmlUtility.GetNavigableDocument(ms);
            Assert.NotNull(doc);

            var nav = doc!.CreateNavigator();
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
        var actual = XmlUtility.XmlDecode(input);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData(null, null)]
    [InlineData("<one/>", "&lt;one/&gt;")]
    public void XmlEncode_Test(string? input, string? expected)
    {
        var actual = XmlUtility.XmlEncode(input);

        Assert.Equal(expected, actual);
    }
}
