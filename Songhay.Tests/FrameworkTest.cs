using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Songhay.Tests
{
    using Extensions;

    /// <summary>
    /// Summary description for FrameworkTest
    /// </summary>
    [TestClass]
    public class FrameworkTest
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
        public void ShouldNotConvertDecimalStringToInt()
        {
            var test = false;

            var x = "1.5";
            int y;
            try
            {
                y = Convert.ToInt32(x);
            }
            catch(Exception ex)
            {
                test = true;
                TestContext.WriteLine("Expected exception: {0}", ex.Message);
            }

            Assert.IsTrue(test, "The conversion did not throw an exception.");
        }

        [TestMethod]
        public void ShouldInvokeSingletonMethodWithReflection()
        {
            var type = Type.GetType("Songhay.Tests.Singleton");
            MethodInfo method = type.GetMethod("AddNumbers");
            PropertyInfo prop = type.GetProperty("Instance",
                BindingFlags.Static | BindingFlags.Public);

            var instance = prop.GetValue(prop, Type.EmptyTypes);
            var result = (int)method.Invoke(instance, Type.EmptyTypes);

            Assert.AreEqual(11, result);
        }

        [TestMethod]
        public void ShouldParseUriFragments()
        {
            var location = "./";
            Assert.IsTrue(Uri.IsWellFormedUriString(location, UriKind.Relative), "The expected location is not well formed.");

            location = "./test";
            Assert.IsTrue(Uri.IsWellFormedUriString(location, UriKind.Relative), "The expected location is not well formed.");

            location = "./test/";
            Assert.IsTrue(Uri.IsWellFormedUriString(location, UriKind.Relative), "The expected location is not well formed.");

            location = "./test#";
            Assert.IsFalse(Uri.IsWellFormedUriString(location, UriKind.Relative), "The expected location is actually well formed.");

            location = "http://wordwalkingstick.com/test/#/segment/99";
            Assert.IsTrue(Uri.IsWellFormedUriString(location, UriKind.Absolute), "The expected location is not well formed.");

            var uri = new Uri(location, UriKind.Absolute);

            var fragment = uri.Fragment;
            var fragmentArray = fragment.Split('/');

            Assert.AreEqual<int>(3, fragmentArray.Length);

            location = "/segment/99"; //form of a Silverlight bookmark
            Assert.IsTrue(Uri.IsWellFormedUriString(location, UriKind.Relative), "The expected location is not well formed.");

            uri = new Uri(location, UriKind.Relative);
            var relativeUriSegments = uri.OriginalString.Split('/'); //FUNKYKB: Uri.Segments does not support Relative URIs.
            Assert.AreEqual<int>(3, relativeUriSegments.Length, "The expected number of segments were not found.");

            Assert.IsTrue(Uri.IsWellFormedUriString(location, UriKind.RelativeOrAbsolute), "The expected location is not well formed.");
            uri = new Uri(location, UriKind.RelativeOrAbsolute);
            relativeUriSegments = uri.OriginalString.Split('/');
            Assert.AreEqual<int>(3, relativeUriSegments.Length, "The expected number of segments were not found.");

        }

        [TestMethod]
        public void ShouldRoundCorrectly()
        {
            var input = 603.625M;

            var output = Math.Round(input, 2, MidpointRounding.AwayFromZero);

            Assert.AreEqual(603.63M, output);

            input = 129.195M;

            output = Math.Round(input, 2, MidpointRounding.AwayFromZero);

            Assert.AreEqual(129.20M, output);
        }
    }

    public sealed class Singleton
    {
        private static readonly Singleton instance = new Singleton();

        private Singleton() { }

        public static Singleton Instance
        {
            get
            {
                return instance;
            }
        }

        public int AddNumbers()
        {
            return 10 + 1;
        }
    }
}
