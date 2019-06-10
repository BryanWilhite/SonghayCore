using Xunit;

namespace Songhay.Tests
{
    public class OpcUtilityTest
    {
        [Theory]
        [InlineData("pack://application:,,,/ReferencedAssembly;component/Subfolder/ResourceFile.xaml")]
        public void ShouldAssertPackUriIsValid(string packUri)
        {
            var result = OpcUtility.IsPackUriValid(packUri);
            Assert.True(result);
        }

        [Theory]
        [InlineData("pack://application:,,,/SonghayCore.xUnit.Tests;component/Subfolder/OpcUtilityTest.xaml")]
        public void ShouldGetPackUriFromAssemblyFileName(string packUri)
        {
            var actualPackUri = OpcUtility.GetPackUriFromType(this.GetType(), OpcReferencedTypeStrategy.FromAssemblyFileName);
            Assert.Equal(packUri, actualPackUri);
        }

        [Theory]
        [InlineData("pack://application:,,,/Songhay.Tests;component/Subfolder/OpcUtilityTest.xaml")]
        public void ShouldGetPackUriFromTypeFullName(string packUri)
        {
            var actualPackUri = OpcUtility.GetPackUriFromType(this.GetType(), OpcReferencedTypeStrategy.FromTypeFullName);
            Assert.Equal(packUri, actualPackUri);
        }

        [Theory]
        [InlineData("pack://application:,,,/ReferencedAssembly;component/Subfolder/ResourceFile.xaml")]
        public void ShouldGetRelativePackUri(string packUri)
        {
            var uri = OpcUtility.GetRelativePackUri(packUri);
            Assert.True(uri.IsWellFormedOriginalString(), "The relative URI is not valid.");
        }
    }
}
