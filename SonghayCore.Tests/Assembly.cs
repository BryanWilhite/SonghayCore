global using System.Diagnostics;
global using Songhay.Extensions;
global using Songhay.Tests.Orderers;
global using System.Reflection;
global using System.Text.Json;
global using Xunit;
global using Xunit.Abstractions;
global using Xunit.Sdk;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
[assembly: TestCaseOrderer(TestCaseOrderer.TypeName, ordererAssemblyName: "SonghayCore.Tests")]
/*
    FUNKYKB: 🚧 Note that when the real `SonghayCore.xUnit` package is in use
    the `orderAssemblyName` value should be `TestCaseOrderer.AssemblyName`
    instead of this project, linking to `SonghayCore.xUnit` external files.
*/
