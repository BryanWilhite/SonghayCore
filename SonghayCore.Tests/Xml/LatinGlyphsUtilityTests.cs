using System.Globalization;
using ExcelDataReader;
using HtmlAgilityPack;
using Songhay.Models;
using Songhay.Xml;
using System.Text;

namespace Songhay.Tests.Xml;

public class LatinGlyphsUtilityTests
{
    public LatinGlyphsUtilityTests(ITestOutputHelper helper)
    {
        _testOutputHelper = helper;
    }

    [Theory]
    [InlineData("1 &amp; 2; 3 &#38; 4; it&apos;s done", "1 & 2; 3 & 4; it's done")]
    public void Condense_Test(string input, string expectedOutput)
    {
        _testOutputHelper.WriteLine($"{nameof(input)}: {input}");

        var actual = LatinGlyphsUtility.Condense(input, basicLatinOnly: true);

        _testOutputHelper.WriteLine($"{nameof(actual)}: {actual}");

        Assert.Equal(expectedOutput, actual);
    }

    [Theory]
    [InlineData(" ", " ")]
    [InlineData("1 & 2; 3 & 4; it's done", "1 &#38; 2; 3 &#38; 4; it&#39;s done")]
    public void Expand_Test(string input, string expectedOutput)
    {
        _testOutputHelper.WriteLine($"{nameof(input)}: {input}");

        var actual = LatinGlyphsUtility.Expand(input, basicLatinOnly: true);

        _testOutputHelper.WriteLine($"{nameof(actual)}: {actual}");

        Assert.Equal(expectedOutput, actual);
    }

    [Theory]
    [InlineData("%20", "", true)]
    [InlineData("%e2%80%a6,%e2%80%98,%e2%80%99,%e2%80%9c,%e2%80%9d,%e2%80%a2,%c2%a9,%c2%ae", ",,,,,,,", false)]
    public void RemoveUrlEncodings_Test(string input, string expectedOutput, bool basicLatinOnly)
    {
        _testOutputHelper.WriteLine($"{nameof(input)}: {input}");

        var actual = LatinGlyphsUtility.RemoveUrlEncodings(input, basicLatinOnly);

        _testOutputHelper.WriteLine($"{nameof(actual)}: {actual}");

        Assert.Equal(expectedOutput, actual);
    }

    [DebuggerAttachedTheory]
    [InlineData("https://www.codetable.net/decimal/190")]
    public void ShouldDownloadDecimalPage(string location)
    {
        var web = new HtmlWeb();

        var htmlDoc = web.Load(location);
        Assert.NotNull(htmlDoc);

        var table = htmlDoc.DocumentNode.SelectSingleNode("//table[@class='symbolInfo']");
        Assert.NotNull(table);

        var tr = table.SelectSingleNode("tr[1]");
        Assert.NotNull(tr);

        var th = tr.SelectSingleNode("th");
        Assert.Contains("Symbol Name:", th?.InnerText!);

        var td = tr.SelectSingleNode("td");
        Assert.NotNull(td);

        var unicodeName = td.InnerText.Trim();
        Assert.False(string.IsNullOrWhiteSpace(unicodeName));
    }

    [Fact]
    public void ShouldVerifyPoints()
    {
        var glyphs = LatinGlyphsUtility.GetGlyphs(basicLatinOnly: false);
        foreach (var glyph in glyphs)
        {
            Assert.Equal(glyph.XmlEntityNumber, $"&#{glyph.UnicodeInteger};");
        }
    }

    [DebuggerAttachedTheory]
    [InlineData("./xlsx/latin-glyphs.xlsx", "./txt/latin-glyphs.txt")]
    public void ShouldWriteProgramGlyphData(string input, string output)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        var sb = new StringBuilder();
        var projectRoot = ProgramAssemblyUtility.GetPathFromAssembly(GetType().Assembly, "../../../");
        var projectInfo = new DirectoryInfo(projectRoot);
        Assert.True(projectInfo.Exists);

        input = projectInfo.ToCombinedPath(input);
        Assert.True(File.Exists(input));

        output = projectInfo.ToCombinedPath(output);
        Assert.True(File.Exists(output));

        string? GetCharacter(IExcelDataReader reader)
        {
            var unicodePoint = reader.GetValue(0) as string;

            switch (unicodePoint)
            {
                case "22":
                    return "\\\"";
                case "5c":
                    return "\\\\";
                default:
                    return reader.GetValue(3) as string;
            }
        }

        string? GetUnicodePoint(IExcelDataReader reader)
        {
            var type = reader.GetFieldType(0);

            return type == typeof(double) ?
                reader.GetDouble(0).ToString(CultureInfo.InvariantCulture)
                :
                reader.GetValue(0) as string;
        }

        string ReadLine(IExcelDataReader reader)
        {
            var glyph = new ProgramGlyph
            {
                UnicodePoint = GetUnicodePoint(reader) ?? string.Empty,
                UnicodeGroup = reader.GetString(1),
                UnicodeName = reader.GetString(2),
                Character = GetCharacter(reader) ?? string.Empty,
                Windows1252UrlEncoding = reader.GetValue(4) as string ?? string.Empty,
                Utf8UrlEncoding = reader.GetValue(5) as string ?? string.Empty,
                HtmlEntityName = reader.GetValue(6) as string ?? string.Empty,
                XmlEntityNumber = reader.GetValue(7) as string ?? string.Empty,
            };

            var properties = new[]
            {
                $"{nameof(glyph.UnicodePoint)} = \"{glyph.UnicodePoint}\",",
                $"{nameof(glyph.UnicodeGroup)} = \"{glyph.UnicodeGroup}\",",
                $"{nameof(glyph.UnicodeName)} = \"{glyph.UnicodeName}\",",
                $"{nameof(glyph.Character)} = \"{glyph.Character}\",",
                $"{nameof(glyph.Windows1252UrlEncoding)} = \"{glyph.Windows1252UrlEncoding}\",",
                $"{nameof(glyph.Utf8UrlEncoding)} = \"{glyph.Utf8UrlEncoding}\",",
                $"{nameof(glyph.HtmlEntityName)} = \"{glyph.HtmlEntityName}\",",
                $"{nameof(glyph.XmlEntityNumber)} = \"{glyph.XmlEntityNumber}\"",
            };

            return $"new {nameof(ProgramGlyph)} {{ {properties.Aggregate((a, i) => $"{a} {i}")} }},";
        }

        using (var stream = File.Open(input, FileMode.Open, FileAccess.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                reader.Read(); // skip header
                while (reader.Read()) sb.AppendLine(ReadLine(reader));
            }
        }

        File.WriteAllText(output, sb.ToString().Replace("\t", string.Empty));
    }

    [DebuggerAttachedTheory]
    [InlineData("./xlsx/latin-glyphs.xlsx", "./txt/latin-glyph-names.txt")]
    public void ShouldWriteUnicodeNames(string input, string output)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        var sb = new StringBuilder();
        var projectRoot = ProgramAssemblyUtility.GetPathFromAssembly(GetType().Assembly, "../../../");
        var projectInfo = new DirectoryInfo(projectRoot);
        Assert.True(projectInfo.Exists);

        input = projectInfo.ToCombinedPath(input);
        Assert.True(File.Exists(input));

        output = projectInfo.ToCombinedPath(output);
        Assert.True(File.Exists(output));

        string ReadLine(IExcelDataReader reader)
        {
            var xmlEntityNumber = reader.GetValue(7) as string;

            xmlEntityNumber = xmlEntityNumber
                .ToReferenceTypeValueOrThrow()
                .Replace("&#", string.Empty)
                .Replace(";", string.Empty);

            _testOutputHelper.WriteLine($"{nameof(xmlEntityNumber)}: {xmlEntityNumber}");

            var location = $"https://www.codetable.net/decimal/{xmlEntityNumber}";

            var web = new HtmlWeb();

            var htmlDoc = web.Load(location);
            Assert.NotNull(htmlDoc);

            var table = htmlDoc.DocumentNode.SelectSingleNode("//table[@class='symbolInfo']");
            Assert.NotNull(table);

            var tr = table.SelectSingleNode("tr[1]");
            Assert.NotNull(tr);

            var th = tr.SelectSingleNode("th");
            Assert.Contains("Symbol Name:", th?.InnerText!);

            var td = tr.SelectSingleNode("td");
            Assert.NotNull(td);

            var unicodeName = td.InnerText.Trim();
            Assert.False(string.IsNullOrWhiteSpace(unicodeName));

            return unicodeName;
        }

        using (var stream = File.Open(input, FileMode.Open, FileAccess.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                reader.Read(); // skip header
                while (reader.Read()) sb.AppendLine(ReadLine(reader));
            }
        }

        File.WriteAllText(output, sb.ToString().Replace("\t", string.Empty));
    }

    readonly ITestOutputHelper _testOutputHelper;
}
