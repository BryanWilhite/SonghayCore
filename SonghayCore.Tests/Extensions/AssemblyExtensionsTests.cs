namespace Songhay.Tests.Extensions;

public class AssemblyExtensionsTests
{
    public AssemblyExtensionsTests(ITestOutputHelper helper)
    {
        _testOutputHelper = helper;
    }

    [Fact]
    public void FindNetCoreProjectDirectorySiblingInfo_Test()
    {
        DirectoryInfo? actual = GetType().Assembly.FindNetCoreProjectDirectorySiblingInfo("*.OrderedTests");

        Assert.NotNull(actual);

        _testOutputHelper.WriteLine(actual.FullName);
    }

    readonly ITestOutputHelper _testOutputHelper;
}
