using Songhay.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.Tests;

public class ProgramFileTests
{
    public ProgramFileTests(ITestOutputHelper helper)
    {
        _testOutputHelper = helper;
    }

    [Theory, InlineData(200)]
    public void ShouldFindPathOfGivenLength(int length)
    {
        var assemblyRoot = ProgramAssemblyUtility.GetPathFromAssembly(GetType().Assembly);
        var projectsFolder = ProgramFileUtility.GetParentDirectory(assemblyRoot, 3);

        var directory = new DirectoryInfo(projectsFolder.ToReferenceTypeValueOrThrow());
        var dict = new Dictionary<string, int>();
        AddPathsToList(directory, dict);
        dict.ForEachInEnumerable(d =>
        {
            if (d.Value > length)
                _testOutputHelper
                    .WriteLine($"{d.Key}:{d.Value.ToString()}");
        });
    }

    void AddPathsToList(DirectoryInfo info, Dictionary<string, int> dict)
    {
        info.GetDirectories()
            .ForEachInEnumerable(d =>
            {
                dict.Add(d.FullName, d.FullName.Length);
                d.GetDirectories()
                    .ForEachInEnumerable(d2 =>
                    {
                        AddPathsToList(d2, dict);
                    });
            });
    }

    [Fact]
    public void ShouldHaveEnvironmentNewline()
    {
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            Assert.Equal("\r\n", Environment.NewLine);

        else if (Environment.OSVersion.Platform == PlatformID.Unix)
            Assert.Equal("\n", Environment.NewLine);

        else
            throw new NotSupportedException($"{Environment.OSVersion} is not supported.");
    }

    [Theory, InlineData(@"content\FrameworkFileTest-ShouldSortTextFileData.txt")]
    public void ShouldSortTextFileData(string outFile)
    {
        var projectRoot = ProgramAssemblyUtility.GetPathFromAssembly(GetType().Assembly, "../../../");
        outFile = ProgramFileUtility.GetCombinedPath(projectRoot, outFile);
        Assert.True(File.Exists(outFile), "The expected output file is not here");

        var stream = new FileStream(outFile, FileMode.Open);
        try
        {
            using var sr = new StreamReader(stream);

            var dataArray = sr.ReadToEnd().Replace(Environment.NewLine, ",").Split(',');
            dataArray.Select(s => new { Car = s, Count = dataArray.Count(i => i == s) })
                .OrderByDescending(o => o.Count).ThenBy(o => o.Car)
                .GroupBy(o => o.Car)
                .ForEachInEnumerable(o =>
                {
                    _testOutputHelper.WriteLine($"{o.Key} = {o.Count()}");
                });
        }
        finally
        {
            stream.Dispose();
        }
    }

    [Theory, InlineData(@"content\FrameworkFileTest-ShouldWriteTextFileWithStreamWriter.txt")]
    public void ShouldWriteTextFileWithStreamWriter(string outFile)
    {
        var projectRoot = ProgramAssemblyUtility.GetPathFromAssembly(GetType().Assembly, "../../../");
        outFile = ProgramFileUtility.GetCombinedPath(projectRoot, outFile);
        Assert.True(File.Exists(outFile), "The expected output file is not here");

        var fileText = @"
This is the text to write to the test file.
This sentence should be on a line all by itself.
According to Notepad++ this file should be in ANSI format by default.
According to Notepad++, when Encoding.UTF8 is specified the encoding is UTF8.
This is the end of the file.
            ";

        var stream = new FileStream(outFile, FileMode.Create);
        try
        {
            using var sw = new StreamWriter(stream, Encoding.UTF8);
            sw.Write(fileText);
        }
        finally
        {
            stream.Dispose();
        }
    }

    [Theory, InlineData(@"content\FrameworkFileTest-ShouldWriteTextFileWithXmlTextWriter.xml")]
    public void ShouldWriteTextFileWithXmlTextWriter(string outFile)
    {
        var projectRoot = ProgramAssemblyUtility.GetPathFromAssembly(GetType().Assembly, "../../../");
        outFile = ProgramFileUtility.GetCombinedPath(projectRoot, outFile);
        Assert.True(File.Exists(outFile), "The expected output file is not here");

        var stream = new FileStream(outFile, FileMode.Create);
        try
        {
            using var writer = new XmlTextWriter(stream, Encoding.UTF8);

            writer.WriteStartElement("root");
            writer.WriteAttributeString("xmlns", "x", null, "urn:1");
            writer.WriteStartElement("item", "urn:1");
            writer.WriteString("This is the text to write to the test file.");
            writer.WriteEndElement();
            writer.WriteStartElement("item", "urn:1");
            writer.WriteString("According to Notepad++, when Encoding.UTF8 is specified the encoding is UTF8.");
            writer.WriteEndElement();
            writer.WriteEndElement();
        }
        finally
        {
            stream.Dispose();
        }
    }

    [Theory, InlineData("TestMyDocumentsFolder")]
    public void ShouldWriteToMyDocumentsFolder(string folder)
    {
        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), folder);
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);

        Assert.True(Directory.Exists(path));
    }

    readonly ITestOutputHelper _testOutputHelper;
}
