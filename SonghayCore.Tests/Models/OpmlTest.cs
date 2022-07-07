using Songhay.Extensions;
using Songhay.Xml;
using System;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.Tests;

public class OpmlTest
{
    public OpmlTest(ITestOutputHelper helper)
    {
        this._testOutputHelper = helper;
    }

    [Theory]
    [InlineData(@"Models\OpmlFromInfoPath.xml")]
    public void ShouldFilterCategory(string opmlFile)
    {
        var projectFolder = ProgramAssemblyUtility.GetPathFromAssembly(this.GetType().Assembly, "../../../");

        var path = ProgramFileUtility.GetCombinedPath(projectFolder, opmlFile);
        Assert.True(File.Exists(path));

        var data = OpmlUtility.GetDocument(path);

        Assert.True(data.OpmlBody.Outlines.Length == 4);

        data.OpmlBody.Outlines = data.OpmlBody.Outlines
            .Where(o => o.Category != "private").ToArray();

        Assert.True(data.OpmlBody.Outlines.Length == 3);
    }

    [Theory]
    [InlineData(@"Models\OpmlTest.opml")]
    public void ShouldLoadCategoriesAndResources(string opmlFile)
    {
        var projectFolder = ProgramAssemblyUtility.GetPathFromAssembly(this.GetType().Assembly, "../../../");

        var path = ProgramFileUtility.GetCombinedPath(projectFolder, opmlFile);
        Assert.True(File.Exists(path));

        var data = OpmlUtility.GetDocument(path);
        Assert.NotNull(data);

        //XPATH: ./outline[not(@url)]
        var categories = data.OpmlBody.Outlines
            .Where(outline => outline.Url == null);

        categories.ForEachInEnumerable(category =>
            this._testOutputHelper.WriteLine($"Category: {category.Text}"));

        //XPATH./outline[@type="link"]
        var resources = categories.First().Outlines
            .Where(outline => outline.OutlineType == "link");

        resources.ForEachInEnumerable(resource =>
            this._testOutputHelper.WriteLine($"Resource: {resource.Text}"));
    }

    [Theory]
    [InlineData(@"Models\OpmlFromInfoPath.xml")]
    public void ShouldLoadDocument(string opmlFile)
    {
        var projectFolder = ProgramAssemblyUtility.GetPathFromAssembly(this.GetType().Assembly, "../../../");

        var path = ProgramFileUtility.GetCombinedPath(projectFolder, opmlFile);
        Assert.True(File.Exists(path));

        var data = OpmlUtility.GetDocument(path);

        var expected = "Development Server";
        var actual = data.OpmlHead.Title;
        Assert.Equal(expected, actual);

        expected = "LINQ to Entities Paging";
        actual = data.OpmlBody.Outlines
            .Where(o => o.Text == "Samples")
            .First().Outlines.First().Text;
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(@"Models\OpmlTest.opml")]
    public void ShouldWriteDateModified(string opmlFile)
    {
        var projectFolder = ProgramAssemblyUtility.GetPathFromAssembly(this.GetType().Assembly, "../../../");

        var path = ProgramFileUtility.GetCombinedPath(projectFolder, opmlFile);
        Assert.True(File.Exists(path));

        var data = OpmlUtility.GetDocument(path);
        DateTime? date = DateTime.Now;
        data.OpmlHead.DateModified = date;
        XmlUtility.Write(data, path);

        data = OpmlUtility.GetDocument(path);
        var actualDate = data.OpmlHead.DateModified;
        Assert.Equal(date.Value.Day, actualDate.Value.Day);
        Assert.Equal(date.Value.Hour, actualDate.Value.Hour);
        Assert.Equal(date.Value.Minute, actualDate.Value.Minute);
    }

    readonly ITestOutputHelper _testOutputHelper;
}