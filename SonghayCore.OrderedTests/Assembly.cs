global using System.Reflection;

global using Xunit;
global using Xunit.Abstractions;
global using Xunit.Sdk;

global using Songhay.Extensions;
global using Songhay.Tests.Orderers;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
[assembly: TestCaseOrderer(TestCaseOrderer.TypeName, ordererAssemblyName: TestCaseOrderer.AssemblyName)]
