using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Songhay.Tests
{
    using Songhay.Extensions;

    /// <summary>
    /// Tests for LINQ-to-Objects
    /// </summary>
    [TestClass]
    public class LinqToObjectsTest
    {
        /// <summary>
        /// Initializes the test.
        /// </summary>
        [TestInitialize]
        public void InitializeTest()
        {
            #region remove previous test results:

            var directory = Directory.GetParent(TestContext.TestDir);

            directory.GetFiles()
                .OrderByDescending(f => f.LastAccessTime).Skip(1)
                .ForEachInEnumerable(f => f.Delete());

            directory.GetDirectories()
                .OrderByDescending(d => d.LastAccessTime).Skip(1)
                .ForEachInEnumerable(d => d.Delete(true));

            #endregion
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void ShouldNotReturnAnyForOfType()
        {
            var set = new[]
            { 
                new KeyValuePair<int, string>(1, "one"),
                new KeyValuePair<int, string>(2, "two"),
                new KeyValuePair<int, string>(3, "three"),
                new KeyValuePair<int, string>(4, "four"),
                new KeyValuePair<int, string>(5, "five"),
                new KeyValuePair<int, string>(6, "six"),
                new KeyValuePair<int, string>(7, "seven"),
            };

            var iterator = set.OfType<double>();

            Assert.IsTrue(iterator is IEnumerable<double>, "The expected interface type is not here.");
            Assert.IsFalse(iterator.Any(), "No results were expected.");
        }
    }
}
