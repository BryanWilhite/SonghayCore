namespace Songhay.Tests;

public class ProgramAssemblyUtilityTests
{
    public ProgramAssemblyUtilityTests(ITestOutputHelper helper)
    {
        _testOutputHelper = helper;
    }

    [Theory]
    [InlineData(@"..\..\..\content\FrameworkAssemblyUtilityTest-ShouldGetPathFromAssembly.json")]
    [InlineData("../../../content/FrameworkAssemblyUtilityTest-ShouldGetPathFromAssembly.json")]
    [InlineData(@"..\..\..\..\SonghayCore\SonghayCore.nuspec")]
    [InlineData("../../../../SonghayCore/SonghayCore.nuspec")]
    public void GetPathFromAssembly_Test(string fileSegment)
    {
        var actualPath = ProgramAssemblyUtility.GetPathFromAssembly(GetType().Assembly, fileSegment);
        _testOutputHelper.WriteLine(actualPath);
        Assert.True(File.Exists(actualPath));
    }

    [Fact]
    public void ProposedLocationTest()
    {
        var assembly = GetType().Assembly;

// ⚠ https://docs.microsoft.com/en-us/dotnet/core/compatibility/syslib-warnings/syslib0012
#pragma warning disable SYSLIB0012

        var proposedLocation = (!string.IsNullOrWhiteSpace(assembly.CodeBase) &&
                                !ProgramFileUtility.IsForwardSlashSystem()) ?
            assembly.CodeBase.Replace("file:///", string.Empty) :
            assembly.Location;

        _testOutputHelper.WriteLine($"{nameof(assembly.Location)}: {assembly.Location}");
        _testOutputHelper.WriteLine($"{nameof(assembly.CodeBase)}: {assembly.CodeBase}");
        _testOutputHelper.WriteLine($"{nameof(proposedLocation)}: {proposedLocation}");

#pragma warning restore SYSLIB0012

    }

    readonly ITestOutputHelper _testOutputHelper;
}