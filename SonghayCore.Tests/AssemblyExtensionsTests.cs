namespace Songhay.Tests;

public class AssemblyExtensionsTests
{
    [Fact]
    public void GetPathFromAssembly_Test()
    {
        var assembly = GetType().Assembly;

        var path = assembly.GetPathFromAssembly();

        Assert.True(Directory.Exists(path));
    }
}