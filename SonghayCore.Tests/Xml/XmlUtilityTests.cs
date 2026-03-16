using System.Text;
using System.Xml;
using System.Xml.XPath;
using Songhay.Xml;

namespace Songhay.Tests.Xml;

public class XmlUtilityTests
{
    private const string Xml =
        """
        <?xml version="1.0" encoding="utf-8"?>
        <Wikimedia>
          <projects>
            <project name="Wikipedia" launch="2001-01-05">
              <editions>
                <edition language="English">en.wikipedia.org</edition>
                <edition language="German">de.wikipedia.org</edition>
                <edition language="French">fr.wikipedia.org</edition>
                <edition language="Polish">pl.wikipedia.org</edition>
                <edition language="Spanish">es.wikipedia.org</edition>
              </editions>
            </project>
            <project name="Wiktionary" launch="2002-12-12">
              <editions>
                <edition language="English">en.wiktionary.org</edition>
                <edition language="French">fr.wiktionary.org</edition>
                <edition language="Vietnamese">vi.wiktionary.org</edition>
                <edition language="Turkish">tr.wiktionary.org</edition>
                <edition language="Spanish">es.wiktionary.org</edition>
              </editions>
            </project>
          </projects>
        </Wikimedia>
        """;

    [Theory]
    [InlineData(
        "/Wikimedia/projects/project[@name='Wikipedia']/editions/edition/text()",
        "en.wikipedia.org"
        )]
    [InlineData(
        "/Wikimedia/projects/project[@name='Wiktionary']/editions/edition[3]/text()",
        "vi.wiktionary.org"
        )]
    [InlineData(
        "/Wikimedia/projects/project[@name='Wiktionary']/editions/edition[3]/@language",
        "language=\"Vietnamese\""
        )]
    [InlineData(
        "/Wikimedia/projects/project[@name='Wikipedia']/editions/edition[2]",
        "<edition language=\"German\">de.wikipedia.org</edition>"
        )]
    public void GetNavigableNode_Test(string? nodeQuery, string? expected)
    {
        // arrange:
        XPathDocument document = XmlUtility.GetNavigableDocument(Xml).ToReferenceTypeValueOrThrow();

        // act:
        XPathNavigator? actual = XmlUtility.GetNavigableNode(document, nodeQuery);

        // assert:
        Assert.Equal(expected, actual?.OuterXml);
    }

    [Theory]
    [InlineData(
        "/Wikimedia/projects/project[@name='Wikipedia']/editions/edition/text()",
        "en.wikipedia.org|de.wikipedia.org|fr.wikipedia.org|pl.wikipedia.org|es.wikipedia.org"
    )]
    [InlineData(
        "/Wikimedia/projects/project[@name='Wiktionary']/editions/edition[position() > 2]/@language",
        "Vietnamese|Turkish|Spanish"
    )]
    public void GetNavigableNodes_Test(string? nodeQuery, string? expected)
    {
        // arrange:
        XPathDocument document = XmlUtility.GetNavigableDocument(Xml).ToReferenceTypeValueOrThrow();

        // act:
        XPathNodeIterator? iterator = XmlUtility.GetNavigableNodes(document, nodeQuery);
        XPathNavigator?[] docs = iterator?.OfType<XPathNavigator?>().ToArray() ?? [];
        string actual = string.Join('|', docs);

        // assert:
        Assert.Equal(expected, actual);
    }

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

            XPathNavigator nav = doc.CreateNavigator();
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
