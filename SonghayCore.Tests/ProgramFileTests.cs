using System.Text;
using System.Xml;
using Songhay.Tests.Extensions;

namespace Songhay.Tests;

public class ProgramFileTests(ITestOutputHelper helper)
{
    [Theory, InlineData(200)]
    public void ShouldFindPathOfGivenLength(int length)
    {
        DirectoryInfo projectDirectoryInfo = GetType().Assembly.GetNetCoreProjectDirectoryInfo();

        var dict = new Dictionary<string, int>();
        AddPathsToList(projectDirectoryInfo, dict);
        dict.ForEachInEnumerable(d =>
        {
            if (d.Value > length)
                helper
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
        switch (Environment.OSVersion.Platform)
        {
            case PlatformID.Win32NT:
                Assert.Equal("\r\n", Environment.NewLine);
                break;
            case PlatformID.Unix:
                Assert.Equal("\n", Environment.NewLine);
                break;
            default:
                throw new NotSupportedException($"{Environment.OSVersion} is not supported.");
        }
    }

    [Theory, InlineData(@"content\FrameworkFileTest-ShouldSortTextFileData.txt")]
    public void ShouldSortTextFileData(string outFile)
    {
        DirectoryInfo projectDirectoryInfo = GetType().Assembly.GetNetCoreProjectDirectoryInfo();
        outFile = projectDirectoryInfo.ToCombinedPath(outFile);
        Assert.True(File.Exists(outFile), "The expected output file is not here");

        var stream = new FileStream(outFile, FileMode.Open);
        try
        {
            using var sr = new StreamReader(stream);

            string[] dataArray = sr.ReadToEnd().Replace(Environment.NewLine, ",").Split(',');
            dataArray.Select(s => new { Car = s, Count = dataArray.Count(i => i == s) })
                .OrderByDescending(o => o.Count).ThenBy(o => o.Car)
                .GroupBy(o => o.Car)
                .ForEachInEnumerable(o =>
                {
                    helper.WriteLine($"{o.Key} = {o.Count()}");
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
        DirectoryInfo projectDirectoryInfo = GetType().Assembly.GetNetCoreProjectDirectoryInfo();
        outFile = projectDirectoryInfo.ToCombinedPath(outFile);
        Assert.True(File.Exists(outFile), "The expected output file is not here");

        const string fileText = """

                                This is the text to write to the test file.
                                This sentence should be on a line all by itself.
                                According to Notepad++ this file should be in ANSI format by default.
                                According to Notepad++, when Encoding.UTF8 is specified the encoding is UTF8.
                                This is the end of the file.
                                            
                                """;

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
        DirectoryInfo projectDirectoryInfo = GetType().Assembly.GetNetCoreProjectDirectoryInfo();
        outFile = projectDirectoryInfo.ToCombinedPath(outFile);
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
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), folder);
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);

        Assert.True(Directory.Exists(path));
    }
}
