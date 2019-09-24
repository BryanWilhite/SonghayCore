using Songhay.Publications.Extensions;
using Songhay.Publications.Models;
using Songhay.Tests;
using System;
using System.IO;
using Xunit;

namespace Songhay.Publications.Tests.Extensions
{
    public class MarkdownEntryExtensionsTests
    {
        [Fact]
        public void DoNullCheck_Test()
        {
            // arrange
            MarkdownEntry data = null;

            // assert
            Assert.Throws<NullReferenceException>(() => data.DoNullCheck());
        }

        [Theory]
        [ProjectFileData(typeof(MarkdownEntryExtensionsTests), "../../../markdown/to-markdown-entry-test.md")]
        public void ToMarkdownEntry_Test(FileInfo entryInfo)
        {
            // arrange
            var entry = entryInfo.ToMarkdownEntry();

            // act
            var jFrontMatter = entry.FrontMatter;

            // assert
            Assert.NotNull(jFrontMatter);
        }
    }
}
