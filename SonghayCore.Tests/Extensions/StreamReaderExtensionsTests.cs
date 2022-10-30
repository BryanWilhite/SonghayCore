namespace Songhay.Tests.Extensions;

public class StreamReaderExtensionsTests
{
    /// <remarks>
    /// Source of the CSV file:
    /// https://github.com/danielesergio/md-big-data-computing/blob/main/sample_10000.csv
    /// </remarks>
    [Theory]
    [ProjectFileData(typeof(StreamReaderExtensionsTests), "../../../csv/sample_10000.csv")]
    public async Task ShouldReadLines(FileInfo csvFileInfo)
    {
        var lineCount = 0;
        using StreamReader reader = new (csvFileInfo.FullName);

        await reader.ReadLines(line => {
            var actual = line?.Split(',').Count() ?? 0;
            Assert.Equal(8, actual);

            lineCount++;
        });

        Assert.Equal(10000, lineCount);
    }
}
