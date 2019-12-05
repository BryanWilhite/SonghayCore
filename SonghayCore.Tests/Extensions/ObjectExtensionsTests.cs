using Songhay.Extensions;
using Songhay.Models;
using System.IO;
using Xunit;

namespace Songhay.Tests.Extensions
{
    public class ObjectExtensionsTests
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
            var actual = new ProgramArgs(new [] { "" })
                .IsAssignableToISerializable();
            Assert.False(actual);
            Assert.False(typeof(ProgramArgs).IsSerializable);

            actual = new FileInfo("thing.js")
                .IsAssignableToISerializable();
            Assert.True(actual);
            Assert.False(typeof(FileInfo).IsSerializable);
        }
    }
}
