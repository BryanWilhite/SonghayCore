namespace Songhay.Tests;

public class ProgramAssemblyUtilityTests(ITestOutputHelper helper)
{
    [Theory]
    [InlineData(@"..\..\..\content\FrameworkAssemblyUtilityTest-ShouldGetPathFromAssembly.json")]
    [InlineData("../../../content/FrameworkAssemblyUtilityTest-ShouldGetPathFromAssembly.json")]
    [InlineData(@"..\..\..\..\SonghayCore\SonghayCore.nuspec")]
    [InlineData("../../../../SonghayCore/SonghayCore.nuspec")]
    public void GetPathFromAssembly_Test(string fileSegment)
    {
        string actualPath = ProgramAssemblyUtility.GetPathFromAssembly(GetType().Assembly, fileSegment);
        helper.WriteLine(actualPath);
        Assert.True(File.Exists(actualPath));
    }

    [Fact]
    public void ProposedLocationTest()
    {
        var assembly = GetType().Assembly;

// ⚠ https://docs.microsoft.com/en-us/dotnet/core/compatibility/syslib-warnings/syslib0012
#pragma warning disable SYSLIB0012

        var proposedLocation = !string.IsNullOrWhiteSpace(assembly.CodeBase) &&
                               !ProgramFileUtility.IsForwardSlashSystem() ?
            assembly.CodeBase.Replace("file:///", string.Empty) :
            assembly.Location;

        helper.WriteLine($"{nameof(assembly.Location)}: {assembly.Location}");
        helper.WriteLine($"{nameof(assembly.CodeBase)}: {assembly.CodeBase}");
        helper.WriteLine($"{nameof(proposedLocation)}: {proposedLocation}");

#pragma warning restore SYSLIB0012

    }
}