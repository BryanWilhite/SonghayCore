using Songhay.Extensions;
using System;
using System.Linq;
using System.Management;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Songhay.Tests
{

    /// <summary>
    /// Summary description for FrameworkTest
    /// </summary>
    [TestClass]
    public class WmiTest
    {
        [TestInitialize]
        public void InitializeTest()
        {
            this.TestContext.RemovePreviousTestResults();
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        [Description("Should list the physical drives.")]
        public void ShouldListPhysicalDrives()
        {
            var mos = new ManagementObjectSearcher(
                @"SELECT *
                    FROM Win32_LogicalDisk"
            );

            mos.Get().OfType<ManagementObject>()
                .OrderBy(p => p.Properties["DeviceID"].Value)
                .ForEachInEnumerable(mo =>
                {
                    mo.Properties
                        .OfType<PropertyData>().ForEachInEnumerable(data =>
                        {
                            TestContext.WriteLine("{0}: {1}", data.Name,
                            (data.Value ?? "N/A").ToString()
                                .Replace("{", "{{").Replace("}", "}}"));
                        });
                });

        }

        /// <summary>
        /// Should the list physical network adapters.
        /// </summary>
        /// <remarks>
        /// See: “Find only physical network adapters with WMI Win32_NetworkAdapter class”
        /// by Mladen Prajdić
        /// [http://sqlserverpedia.com/blog/sql-server-bloggers/find-only-physical-network-adapters-with-wmi-win32_networkadapter-class/]
        /// </remarks>
        [TestMethod]
        [Description("Should list the physical network adapters.")]
        public void ShouldListPhysicalNetworkAdapters()
        {
            var mos = new ManagementObjectSearcher(
                @"SELECT *
                    FROM Win32_NetworkAdapter
                    WHERE Manufacturer != 'Microsoft'
                    AND NOT PNPDeviceID LIKE 'ROOT\\%'"
            );

            // Get the physical adapters and sort them by their index.
            // This is needed because they're not sorted by default
            mos.Get().OfType<ManagementObject>()
                .OrderBy(p => Convert.ToUInt32(p.Properties["Index"].Value))
                .ForEachInEnumerable(mo =>
                {
                    mo.Properties
                        .OfType<PropertyData>().ForEachInEnumerable(data =>
                        {
                            TestContext.WriteLine("{0}: {1}", data.Name,
                            (data.Value ?? "N/A").ToString()
                                .Replace("{", "{{").Replace("}", "}}"));
                        });
                });
        }
    }
}
