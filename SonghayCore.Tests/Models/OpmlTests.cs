using Songhay.Tests.Extensions;
using Songhay.Xml;

namespace Songhay.Tests.Models;

public class OpmlTests
{
    public OpmlTests(ITestOutputHelper helper)
    {
        _testOutputHelper = helper;
    }

    [Theory]
    [InlineData(@"Models\OpmlFromInfoPath.xml")]
    public void ShouldFilterCategory(string opmlFile)
    {
        DirectoryInfo projectDirectoryInfo = GetType().Assembly.GetNetCoreProjectDirectoryInfo();
        string path = projectDirectoryInfo.ToCombinedPath(opmlFile);
        Assert.True(File.Exists(path));

        var data = OpmlUtility.GetDocument(path).ToReferenceTypeValueOrThrow();

        Assert.True(data.OpmlBody.ToReferenceTypeValueOrThrow().Outlines.Length == 4);

        data.OpmlBody.Outlines = data.OpmlBody.Outlines
            .Where(o => o.Category != "private").ToArray();

        Assert.True(data.OpmlBody.Outlines.Length == 3);
    }

    [Theory]
    [InlineData(@"Models\OpmlTests.opml")]
    public void ShouldLoadCategoriesAndResources(string opmlFile)
    {
        DirectoryInfo projectDirectoryInfo = GetType().Assembly.GetNetCoreProjectDirectoryInfo();
        string path = projectDirectoryInfo.ToCombinedPath(opmlFile);
        Assert.True(File.Exists(path));

        var data = OpmlUtility.GetDocument(path).ToReferenceTypeValueOrThrow();

        //XPATH: ./outline[not(@url)]
        var categories =
            data.OpmlBody
                .ToReferenceTypeValueOrThrow()
                .Outlines
                .Where(outline => outline.Url == null)
                .ToArray();

        categories
            .ForEachInEnumerable(category =>
            _testOutputHelper.WriteLine($"Category: {category.Text}"));

        //XPATH./outline[@type="link"]
        var resources = categories.First().Outlines
            .Where(outline => outline.OutlineType == "link");

        resources.ForEachInEnumerable(resource =>
            _testOutputHelper.WriteLine($"Resource: {resource.Text}"));
    }

    [Theory]
    [InlineData(@"Models\OpmlFromInfoPath.xml")]
    public void ShouldLoadDocument(string opmlFile)
    {
        DirectoryInfo projectDirectoryInfo = GetType().Assembly.GetNetCoreProjectDirectoryInfo();
        string path = projectDirectoryInfo.ToCombinedPath(opmlFile);
        Assert.True(File.Exists(path));

        var data = OpmlUtility.GetDocument(path).ToReferenceTypeValueOrThrow();

        var expected = "Development Server";
        var actual = data.OpmlHead.ToReferenceTypeValueOrThrow().Title;
        Assert.Equal(expected, actual);

        expected = "LINQ to Entities Paging";
        actual =
            data
                .OpmlBody
                .ToReferenceTypeValueOrThrow()
                .Outlines
                .First(o => o.Text == "Samples")
                .Outlines.First().Text;

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(@"Models\OpmlTests.opml")]
    public void ShouldWriteDateModified(string opmlFile)
    {
        DirectoryInfo projectDirectoryInfo = GetType().Assembly.GetNetCoreProjectDirectoryInfo();
        string path = projectDirectoryInfo.ToCombinedPath(opmlFile);
        Assert.True(File.Exists(path));

        var data = OpmlUtility.GetDocument(path).ToReferenceTypeValueOrThrow();
        DateTime? date = DateTime.Now;
        data.OpmlHead.ToReferenceTypeValueOrThrow().DateModified = date;
        XmlUtility.Write(data, path);

        data = OpmlUtility.GetDocument(path).ToReferenceTypeValueOrThrow();
        var actualDate =
            data
                .OpmlHead
                .ToReferenceTypeValueOrThrow()
                .DateModified
                .ToValueOrThrow();

        Assert.Equal(date.Value.Day, actualDate.Day);
        Assert.Equal(date.Value.Hour, actualDate.Hour);
        Assert.Equal(date.Value.Minute, actualDate.Minute);
    }

    readonly ITestOutputHelper _testOutputHelper;
}
