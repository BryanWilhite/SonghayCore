﻿using System.Xml.Linq;
using Songhay.Xml;

namespace Songhay.Tests.Xml;

public class XObjectUtilityTests(ITestOutputHelper helper)
{
    [Theory]
    [InlineData(@"<category domain=""category"" nicename=""root""><![CDATA[root]]></category>", "root")]
    public void GetCDataValue_Test(string xmlString, string expectedValue)
    {
        //arrange
        var xElement = XElement.Parse(xmlString);

        //assert
        Assert.Equal(expectedValue, XObjectUtility.GetCDataValue(xElement));
    }

    [Theory]
    [InlineData("one-text-node-child-at-the-end", "<root><one>one-text-node<one-child>-child</one-child>-at-the-end</one></root>")]
    public void ShouldJoinFlattenedXTextNodes(string expectedValue, string sampleOne)
    {
        var rootElement = XElement.Parse(sampleOne);
        helper.WriteLine(rootElement.ToString());

        var actual = XObjectUtility.JoinFlattenedXTextNodes(rootElement);
        helper.WriteLine($"actual: {actual}");
        Assert.Equal(expectedValue, actual);
    }

    [Fact]
    public void ShouldMatchXPathForDescendingElements()
    {
        const string xml = """

                           <root>
                               <a>
                                   <b>
                                       <c>a text node</c>
                                   </b>
                               </a>
                           </root>

                           """;
        var xd = XDocument.Parse(xml);

        var actual = XObjectUtility.GetXNode(xd.Root, "/root/a/b/c") as XElement;
        Assert.NotNull(actual);

        var expected = xd.Elements("root")
            .Elements("a")
            .Elements("b")
            .Elements("c")
            .FirstOrDefault();
        Assert.NotNull(expected);

        Assert.Equal(expected, actual);

        var c = xd.Elements("root").ToArray();
        Assert.NotNull(c);

        var attr = c.FirstOrDefault()?.Attribute("foo");
        Assert.Null(attr);
    }
}
