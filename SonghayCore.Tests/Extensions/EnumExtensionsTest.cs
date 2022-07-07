using Songhay.Extensions;
using System;
using System.Linq;
using Xunit;

namespace Songhay.Tests.Extensions;

public class EnumExtensionsTest
{
    [Fact]
    public void ShouldGetEnumValues()
    {
        Enum myTestEnumOne = MyTestEnum.One;
        var values = myTestEnumOne.GetEnumValues().OfType<MyTestEnum>();
        Assert.True(values.Any(), "The expected values are not here.");
    }

    [Fact]
    public void ShouldGetEnumDescription()
    {
        Enum myTestEnumOne = MyTestEnum.One;

        var expected = "this is one";
        var actual = myTestEnumOne.GetEnumDescription();
        Assert.Equal(expected, actual);
    }
}

enum MyTestEnum
{
    [System.ComponentModel.Description("this is one")]
    One = 1,
    [System.ComponentModel.Description("this is two")]
    Two = 2,
    [System.ComponentModel.Description("this is three")]
    Three = 3,
}