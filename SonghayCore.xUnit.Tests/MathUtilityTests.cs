using System;
using Xunit;

namespace Songhay.Tests
{

    public class MathUtilityTests
    {
        public void GetMantissa_Test()
        {
            double x = -1034.068;
            double mantissaValue = MathUtility.GetMantissa(x, 2);

            Assert.Equal(-0.07, mantissaValue);
        }

        public void TruncateNumber_Test()
        {
            double x = -1034.068;
            double truncated = MathUtility.TruncateNumber(x);

            Assert.Equal(Math.Truncate(x), truncated);
        }

        public void GetDigitInNumber_Test()
        {
            var x = 1324.068;
            var digit = MathUtility.GetDigitInNumber((int)(x * 1000), 3);

            Assert.Equal((byte)0, digit);
        }
    }
}
