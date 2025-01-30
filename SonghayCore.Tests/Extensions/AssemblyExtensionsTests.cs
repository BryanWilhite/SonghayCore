namespace Songhay.Tests.Extensions;

public class AssemblyExtensionsTests(ITestOutputHelper helper)
{
    [Fact]
    public void FindNetCoreProjectDirectorySiblingInfo_Test()
    {
        DirectoryInfo? actual = GetType().Assembly.FindNetCoreProjectDirectorySiblingInfo("*.OrderedTests");

        Assert.NotNull(actual);

        helper.WriteLine(actual.FullName);
    }
}
