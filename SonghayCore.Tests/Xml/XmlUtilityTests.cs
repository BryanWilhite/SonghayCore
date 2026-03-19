using System.Text;
using System.Xml;
using System.Xml.XPath;
using Songhay.Xml;

namespace Songhay.Tests.Xml;

public class XmlUtilityTests(ITestOutputHelper helper)
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

    private const string XmlWithNamespaces =
        """
        <PurchaseOrder xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"  xmlns:xsd="http://www.w3.org/2001/XMLSchema">
            <ItemsOrders>
                <Item>
                    <ItemID>aaa111</ItemID>
                    <ItemPrice>34.22</ItemPrice>
                </Item>
                <Item>
                    <ItemID>bbb222</ItemID>
                    <ItemPrice>2.89</ItemPrice>
                </Item>
            </ItemsOrders>
        </PurchaseOrder>
        """;

    ///<remarks>
    /// [Ask Bing, Does XmlSerializer support XPathDocument?]
    ///
    /// The XmlSerializer in .NET is designed to work with objects (POCOs) and streams/readers that contain XML
    /// in a serializable form. It expects either:
    ///
    /// - A Stream, TextReader, or XmlReader containing XML data
    /// - Or an object instance to serialize
    ///
    /// XPathDocument is a read-only, XPath-optimized in-memory representation of XML,
    /// not a serializable object model.
    /// It does not expose public settable properties for XmlSerializer to populate,
    /// and it is not intended for serialization/deserialization.
    ///</remarks>
    [Fact]
    public void GetInstanceRaw_XPathDocument_Test()
    {
        // act:
        XPathDocument? actual = XmlUtility.GetInstanceRaw<XPathDocument>(Xml);

        // assert:
        Assert.Null(actual);
    }

    [Fact]
    public void GetNamespaceManager_Test()
    {
        // arrange:
        XPathDocument document = XmlUtility.GetNavigableDocument(XmlWithNamespaces).ToReferenceTypeValueOrThrow();

        // act:
        XmlNamespaceManager? nsm = XmlUtility.GetNamespaceManager(document);
        IDictionary<string, string> actual = nsm?.GetNamespacesInScope(XmlNamespaceScope.Local) ?? new Dictionary<string, string>();

        foreach (KeyValuePair<string, string> pair in actual)
        {
            helper.WriteLine($"{pair.Key}: {pair.Value}");
        }

        // assert:
        Assert.Equal(2, actual.Count);
    }

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

    [Theory]
    [InlineData(
        "/Wikimedia/projects/project[@name='Wikipedia']/editions/edition/text()",
        null,
        "en.wikipedia.org"
    )]
    [InlineData(
        "/Wikimedia/projects/project[@name='Wikipedia']/editions/wrong/text()",
        "hello world!",
        "hello world!"
    )]
    public void GetNodeValue_Test(string? nodeQuery, object? defaultValue, string? expected)
    {
        // arrange:
        XPathDocument document = XmlUtility.GetNavigableDocument(Xml).ToReferenceTypeValueOrThrow();

        // act:
        object? actual = XmlUtility.GetNodeValue(document, nodeQuery, false, defaultValue);

        // assert:
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("/Wikimedia/projects/project[@name='Wikipedia']/editions/wrong/text()")]
    public void GetNodeValue_Exception_Test(string? nodeQuery)
    {
        // arrange:
        XPathDocument document = XmlUtility.GetNavigableDocument(Xml).ToReferenceTypeValueOrThrow();

        // assert:
        Assert.Throws<XmlException>(() => XmlUtility.GetNodeValue(document, nodeQuery, true));
    }

    [Theory]
    [InlineData("<e stamp=\"2001-02-14\" />", ".//@stamp", "2001-02-14")]
    public void GetNodeValueAndParse_DateTime_Test(string input, string? nodeQuery, string dateString)
    {
        // arrange:
        XPathDocument document = XmlUtility.GetNavigableDocument(input).ToReferenceTypeValueOrThrow();

        // act:
        object? actual = XmlUtility.GetNodeValueAndParse(document, nodeQuery, false, DateTime.Now);

        // assert:
        Assert.Equal(DateTime.Parse(dateString), (DateTime)actual!);
    }

    [Fact]
    public void InputAs_string_Test()
    {
        // act:
        XPathDocument? actual = XmlUtility.InputAs(Xml);
        helper.WriteLine(actual?.CreateNavigator().OuterXml);

        // assert:
        Assert.NotNull(actual);
    }

    [Fact]
    public void InputAs_XmlDocument_Test()
    {
        // arrange:
        XmlDocument? document = XmlUtility.GetInstanceRaw<XmlDocument>(Xml);

        // act:
        XPathDocument? actual = XmlUtility.InputAs(document);
        helper.WriteLine(actual?.CreateNavigator().OuterXml);

        // assert:
        Assert.NotNull(actual);
    }

    [Fact]
    public void InputAs_XPathDocument_Test()
    {
        // arrange:
        XPathDocument document = XmlUtility.GetNavigableDocument(Xml).ToReferenceTypeValueOrThrow();

        // act:
        XPathDocument? actual = XmlUtility.InputAs(document);
        helper.WriteLine(actual?.CreateNavigator().OuterXml);

        // assert:
        Assert.NotNull(actual);
    }

    [Theory]
    [InlineData("<e stamp=\"2001-02-14\" />", ".//@stamp", "2001-02-14", true)]
    [InlineData("<e stamp=\"2001-02-14\" />", ".//@stamp", "frankie", false)]
    public void IsNodeValue_Test(string input, string? nodeQuery, string? testValue, bool expected)
    {
        // arrange:
        XPathDocument document = XmlUtility.GetNavigableDocument(input).ToReferenceTypeValueOrThrow();

        // act:
        bool actual = XmlUtility.IsNodeValue(document, nodeQuery, false, testValue);

        // assert:
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void OutputAs_XmlDocument_Test()
    {
        // act:
        XmlDocument? actual = XmlUtility.OutputAs<XmlDocument>(Xml);
        helper.WriteLine(actual?.OuterXml);

        // assert:
        Assert.NotNull(actual);
    }

    [Fact]
    public void OutputAs_XPathDocument_Test()
    {
        // act:
        XPathDocument? actual = XmlUtility.OutputAs<XPathDocument>(Xml);
        helper.WriteLine(actual?.CreateNavigator().OuterXml);

        // assert:
        Assert.NotNull(actual);
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

    [Fact]
    public void StripNamespaces_Test()
    {
        // arrange:
        XPathDocument document = XmlUtility.GetNavigableDocument(XmlWithNamespaces).ToReferenceTypeValueOrThrow();

        // act:
        document = XmlUtility.StripNamespaces(document).ToReferenceTypeValueOrThrow();
        string actual = document.CreateNavigator().OuterXml;

        helper.WriteLine(actual);

        // assert:
        Assert.DoesNotContain("xmlns", actual);
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
