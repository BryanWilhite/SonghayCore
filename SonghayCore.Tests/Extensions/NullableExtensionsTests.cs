namespace Songhay.Tests.Extensions;

public class NullableExtensionsTests
{
    [Fact]
    public void IsAssignableToISerializable_Test()
    {
        /*
            If you've developed a library that targets the .NET Standard,
            your library can be consumed by any .NET implementation
            that supports the .NET Standard. This means that you cannot know
            in advance whether a particular type is serializable;
            you can only determine whether it is serializable at run time.

            [https://docs.microsoft.com/en-us/dotnet/standard/serialization/how-to-determine-if-netstandard-object-is-serializable?view=netframework-4.8]
        */

        var actual = new FileInfo("thing.js").IsAssignableToISerializable();
        Assert.True(actual);
    }

    [Fact]
    public void ThrowWhenNullOrEmpty_Null_Failure_Test()
    {
        IEnumerable<string>? enumerable = null;

        // ReSharper disable once ExpressionIsAlwaysNull
        Assert.Throws<ArgumentNullException>(() => enumerable.ThrowWhenNullOrEmpty());
    }

    [Fact]
    public void ThrowWhenNullOrEmpty_Empty_Failure_Test()
    {
        IEnumerable<string> enumerable = Enumerable.Empty<string>();

        // ReSharper disable once ExpressionIsAlwaysNull
        Assert.Throws<ArgumentNullException>(() => enumerable.ThrowWhenNullOrEmpty());
    }

    [Fact]
    public void ThrowWhenNullOrEmpty_Test()
    {
        IEnumerable<string> enumerable = new []{ "one" };

        enumerable.ThrowWhenNullOrEmpty();
    }

    [Fact]
    public void ThrowWhenNullOrWhiteSpace_Null_Failure_Test()
    {
        string? s = null;

        // ReSharper disable once ExpressionIsAlwaysNull
        Assert.Throws<ArgumentNullException>(() => s.ThrowWhenNullOrWhiteSpace());
    }

    [Fact]
    public void ThrowWhenNullOrWhiteSpace_WhiteSpace_Failure_Test() =>
        Assert.Throws<ArgumentNullException>(() => " ".ThrowWhenNullOrWhiteSpace());

    [Fact]
    public void ThrowWhenNullOrWhiteSpace_Test() => "s".ThrowWhenNullOrWhiteSpace();

    [Fact]
    public void ToReferenceTypeValueOrThrow_Failure_Test()
    {
        string? s = null;

        Assert.Throws<ArgumentNullException>(() => s.ToReferenceTypeValueOrThrow());
    }

    [Fact]
    public void ToReferenceTypeValueOrThrow_Test()
    {
        // ReSharper disable once VariableCanBeNotNullable
        string? s = "x";

        var actual = s.ToReferenceTypeValueOrThrow();

        Assert.Equal(s, actual);
    }

    [Fact]
    public void ToValueOrThrow_Failure_Test()
    {
        byte? b = null;

        Assert.Throws<ArgumentNullException>(() => b.ToValueOrThrow());
    }

    [Fact]
    public void ToValueOrThrow_Test()
    {
        byte? b = 255;

        byte actual = b.ToValueOrThrow();

        Assert.Equal(b, actual);
    }
}
