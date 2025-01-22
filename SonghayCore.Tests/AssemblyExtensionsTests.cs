namespace Songhay.Tests;

public class AssemblyExtensionsTests
{
    [Fact]
    public void GetPathFromAssembly_Test()
    {
        Assembly assembly = GetType().Assembly;

        string? path = assembly.GetPathFromAssembly();

        Assert.True(Directory.Exists(path));
    }
}