﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Songhay.Tests.Orderers
{
    /// <summary>
    /// Implementation of <see cref="ITestCaseOrderer"/>
    /// for the use of <see cref="TestOrderAttribute"/>.
    /// </summary>
    /// <seealso cref="ITestCaseOrderer" />
    /// <remarks>
    /// For more detail, see “How to Order xUnit Tests and Collections” by Tom DuPont
    /// [http://www.tomdupont.net/2016/04/how-to-order-xunit-tests-and-collections.html]
    /// </remarks>
    public class TestCaseOrderer : ITestCaseOrderer
    {
        /// <summary>The type name</summary>
        public const string AssemblyName = "SonghayCore.xUnit";

        /// <summary>The type name</summary>
        public const string TypeName = "Songhay.Tests.Orderers.TestCaseOrderer";

        /// <summary>The queued tests</summary>
        public static readonly ConcurrentDictionary<string, ConcurrentQueue<string>>
            QueuedTests = new ConcurrentDictionary<string, ConcurrentQueue<string>>();

        /// <summary>Orders test cases for execution.</summary>
        /// <typeparam name="TTestCase"></typeparam>
        /// <param name="testCases">The test cases to be ordered.</param>
        /// <returns>The test cases in the order to be run.</returns>
        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases)
            where TTestCase : ITestCase
        {
            return testCases.OrderBy(GetOrder);
        }

        private static int GetOrder<TTestCase>(TTestCase testCase) where TTestCase : ITestCase
        {
            // Enqueue the test name.
            QueuedTests
                .GetOrAdd(testCase.TestMethod.TestClass.Class.Name, key => new ConcurrentQueue<string>())
                .Enqueue(testCase.TestMethod.Method.Name);

            // Order the test based on the attribute.
            var attr = testCase.TestMethod.Method
                .ToRuntimeMethod()
                .GetCustomAttribute<TestOrderAttribute>();
            return attr?.Ordinal ?? 0;
        }
    }
}
