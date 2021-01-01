using Songhay.Tests.Orderers;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
[assembly: TestCaseOrderer(TestCaseOrderer.TypeName, ordererAssemblyName: "SonghayCore.Tests")]
/*
    FUNKYKB: 🚧 Note that when the real `SonghayCore.xUnit` package is in use
    the `orderAssemblyName` value should be `TestCaseOrderer.AssemblyName`
    instead of this project, linking to `SonghayCore.xUnit` external files.
*/
