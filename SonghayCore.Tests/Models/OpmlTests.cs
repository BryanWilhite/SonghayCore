using Songhay.Models;
using Songhay.Xml;

namespace Songhay.Tests.Models;

public class OpmlTests(ITestOutputHelper helper)
{
    [Theory]
    [ProjectDirectoryData("content/xml/OpmlFromInfoPath.xml")]
    public void ShouldFilterCategory(DirectoryInfo projectDirectoryInfo, string opmlFile)
    {
        //arrange:
        string path = projectDirectoryInfo.ToCombinedPath(opmlFile);
        OpmlDocument data = OpmlUtility.GetDocument(path).ToReferenceTypeValueOrThrow();

        //act:
        Assert.Equal(4, data.OpmlBody.ToReferenceTypeValueOrThrow().Outlines.Length);

        data.OpmlBody.Outlines =
        [
            .. data.OpmlBody.Outlines
                .Where(o => o.Category != "private")
        ];

        //assert:
        Assert.Equal(3, data.OpmlBody.Outlines.Length);
    }

    [Theory]
    [ProjectDirectoryData("content/xml/OpmlTests.opml")]
    public void ShouldLoadCategoriesAndResources(DirectoryInfo projectDirectoryInfo, string opmlFile)
    {
        //arrange:
        string path = projectDirectoryInfo.ToCombinedPath(opmlFile);
        OpmlDocument data = OpmlUtility.GetDocument(path).ToReferenceTypeValueOrThrow();

        //act:
        //XPATH: ./outline[not(@url)]
        var categories =
            data.OpmlBody
                .ToReferenceTypeValueOrThrow()
                .Outlines
                .Where(outline => outline.Url == null)
                .ToArray();

        categories
            .ForEachInEnumerable(category =>
            helper.WriteLine($"Category: {category.Text}"));

        //XPATH./outline[@type="link"]
        var resources = categories.First().Outlines
            .Where(outline => outline.OutlineType == "link");

        resources.ForEachInEnumerable(resource =>
            helper.WriteLine($"Resource: {resource.Text}"));
    }

    [Theory]
    [ProjectDirectoryData("content/xml/OpmlFromInfoPath.xml", "Development Server")]
    public void ShouldLoadDocumentWithExpectedOpmlHeadTitle(DirectoryInfo projectDirectoryInfo, string opmlFile, string expected)
    {
        //arrange:
        string path = projectDirectoryInfo.ToCombinedPath(opmlFile);
        OpmlDocument data = OpmlUtility.GetDocument(path).ToReferenceTypeValueOrThrow();

        //act:
        string? actual = data.OpmlHead.ToReferenceTypeValueOrThrow().Title;

        //assert:
        Assert.Equal(expected, actual);

        expected = "LINQ to Entities Paging";

        //act:
        actual =
            data
                .OpmlBody
                .ToReferenceTypeValueOrThrow()
                .Outlines
                .First(o => o.Text == "Samples")
                .Outlines.First().Text;

        //assert:
        Assert.Equal(expected, actual);
    }

    [Theory]
    [ProjectDirectoryData("content/xml/OpmlFromInfoPath.xml", "LINQ to Entities Paging")]
    public void ShouldLoadDocumentWithExpectedOpmlOutlineText(DirectoryInfo projectDirectoryInfo, string opmlFile, string expected)
    {
        //arrange:
        string path = projectDirectoryInfo.ToCombinedPath(opmlFile);
        OpmlDocument data = OpmlUtility.GetDocument(path).ToReferenceTypeValueOrThrow();

        //act:
        string? actual =
            data
                .OpmlBody
                .ToReferenceTypeValueOrThrow()
                .Outlines
                .First(o => o.Text == "Samples")
                .Outlines.First().Text;

        //assert:
        Assert.Equal(expected, actual);
    }

    [Theory]
    [ProjectDirectoryData("content/xml/OpmlTests.opml")]
    public void ShouldWriteDateModified(DirectoryInfo projectDirectoryInfo, string opmlFile)
    {
        //arrange:
        string path = projectDirectoryInfo.ToCombinedPath(opmlFile);
        var data = OpmlUtility.GetDocument(path).ToReferenceTypeValueOrThrow();
        DateTime? date = DateTime.Now;

        data.OpmlHead.ToReferenceTypeValueOrThrow().DateModified = date;
        XmlUtility.Write(data, path);
        data = OpmlUtility.GetDocument(path).ToReferenceTypeValueOrThrow();

        //act:
        DateTime actualDate =
            data
                .OpmlHead
                .ToReferenceTypeValueOrThrow()
                .DateModified
                .ToValueOrThrow();

        //assert:
        Assert.Equal(date.Value.Day, actualDate.Day);
        Assert.Equal(date.Value.Hour, actualDate.Hour);
        Assert.Equal(date.Value.Minute, actualDate.Minute);
    }
}
