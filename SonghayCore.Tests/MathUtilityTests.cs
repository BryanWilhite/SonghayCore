namespace Songhay.Tests;

public class MathUtilityTests
{
    [Fact]
    public void GetMantissa_Test()
    {
        double x = -1034.068;
        double mantissaValue = MathUtility.GetMantissa(x, 2);

        Assert.Equal(-0.07, mantissaValue);
    }

    [Fact]
    public void TruncateNumber_Test()
    {
        double x = -1034.068;
        double truncated = MathUtility.TruncateNumber(x);

        Assert.Equal(Math.Truncate(x), truncated);
    }

    [Fact]
    public void GetDigitInNumber_Test()
    {
        double x = 1324.068;
        byte? digit = MathUtility.GetDigitInNumber((int)(x * 1000), 3);

        Assert.Equal((byte)0, digit);
    }
}