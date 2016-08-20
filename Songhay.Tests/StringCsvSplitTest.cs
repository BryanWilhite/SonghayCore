using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Songhay.Tests
{
    using Extensions;

    /// <summary>
    /// Unit tests for CSVSplit Extension method.
    /// </summary>
    [TestClass]
    public class StringCsvSplitTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext {get; set;}

        [TestMethod]
        [Description("Should split.")]
        public void ShouldSplit()
        {
            var split = "\"123\",\"456\",\"789\"".CsvSplit();
            var test = Validate(split, new[] { "123", "456", "789" });
            Assert.IsTrue(test, "The Validate method failed.");
        }

        [TestMethod]
        [Description("Should split with backslash.")]
        public void ShouldSplitWithBackSlash()
        {
            var split = "\"12\\\"3\",\"456\",\"789\"".CsvSplit();
            var test = Validate(split, new[] { "12\"3", "456", "789" });
            Assert.IsTrue(test, "The Validate method failed.");
        }

        [TestMethod]
        [Description("Should split with comma.")]
        public void ShouldSplitWithComma()
        {
            var split = "\"aaa,bbb\",\"ccc,ddd\",ghi".CsvSplit();
            var test = Validate(split, new[] { "aaa,bbb", "ccc,ddd", "ghi" });
            Assert.IsTrue(test, "The Validate method failed.");
        }

        [TestMethod]
        [Description("Should split with double backslash.")]
        public void ShouldSplitWithDoubleBackslash()
        {
            var split = "\"a\\\\aa\",,bbb,".CsvSplit();
            var test = Validate(split, new[] { "a\\aa", "", "bbb", "" });
            Assert.IsTrue(test, "The Validate method failed.");
        }

        [TestMethod]
        [Description("Should split with empty.")]
        public void ShouldSplitWithEmpty()
        {
            var split = "aaa,,bbb".CsvSplit();
            var test = Validate(split, new[] { "aaa", "", "bbb" });
            Assert.IsTrue(test, "The Validate method failed.");
        }

        [TestMethod]
        [Description("Should split with empty.")]
        public void ShouldSplitWithEmpty2()
        {
            var split = "\"a\\aa\",,bbb,".CsvSplit();
            var test = Validate(split, new[] { "aaa", "", "bbb", "" });
            Assert.IsTrue(test, "The Validate method failed.");
        }

        [TestMethod]
        [Description("Should split with empty trailing.")]
        public void ShouldSplitWithEmptyTrailing()
        {
            var split = "aaa,,bbb,".CsvSplit();
            var test = Validate(split, new[] { "aaa", "", "bbb", "" });
            Assert.IsTrue(test, "The Validate method failed.");
        }

        [TestMethod]
        [Description("Should throw CSV parse exception.")]
        public void ShouldThrowCsvParseException()
        {
            var test = false;
            try
            {
                var split = "\"aaa\\bbb\",ccc,ddd".CsvSplit();
            }
            catch (CsvParseException ex)
            {
                test = true;
                TestContext.WriteLine("Exception Message: {0}", ex.Message);
            }

            Assert.IsTrue(test, "The expected exception was not thrown.");
        }

        [TestMethod]
        [Description("Should throw CSV parse exception.")]
        public void ShouldThrowCsvParseException2()
        {
            var test = false;
            try
            {
                var split = "\"aaabbb\"bbb,ccc,ddd".CsvSplit();
            }
            catch(CsvParseException ex)
            {
                test = true;
                TestContext.WriteLine("Exception Message: {0}", ex.Message);
            }

            Assert.IsTrue(test, "The expected exception was not thrown.");
        }

        [TestMethod]
        [Description("Should throw CSV parse exception.")]
        public void ShouldThrowCsvParseException3()
        {
            var test = false;
            try
            {
                var split = "\"aaabbb\",ccc,\"ddd".CsvSplit();
            }
            catch(CsvParseException ex)
            {
                test = true;
                TestContext.WriteLine("Exception Message: {0}", ex.Message);
            }

            Assert.IsTrue(test, "The expected exception was not thrown.");
        }

        [TestMethod]
        [Description("Should throw CSV parse exception.")]
        public void ShouldThrowCsvParseException4()
        {
            var test = false;
            try
            {
                var split = "aaa,ccc,\"ddd\\".CsvSplit();
            }
            catch(CsvParseException ex)
            {
                test = true;
                TestContext.WriteLine("Exception Message: {0}", ex.Message);
            }

            Assert.IsTrue(test, "The expected exception was not thrown.");
        }

        static bool Validate(string[] results, string[] expectedResults)
        {
            if(results.Length != expectedResults.Length)
            {
                return false;
            }
            for(int i = 0; i < results.Length; i++)
            {
                if(results[i] != expectedResults[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
