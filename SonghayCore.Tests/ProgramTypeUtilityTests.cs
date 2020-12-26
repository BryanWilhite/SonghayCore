using System.Diagnostics;
using Xunit;

namespace Songhay.Tests
{
    public class ProgramTypeUtilityTests
    {
        [Theory]
        [InlineData("Warning", SourceLevels.All, SourceLevels.Warning)]
        [InlineData("Warning,Critical,Error", SourceLevels.All, SourceLevels.All)]
        public void ShouldParseEnum(string input, SourceLevels defaultEnum, SourceLevels expectedEnum)
        {
            var enumValue = ProgramTypeUtility.ParseEnum(input, defaultEnum);
            Assert.Equal(expectedEnum, enumValue);
        }
    }
}
