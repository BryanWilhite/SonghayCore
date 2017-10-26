# SonghayCore

The _Core_ code to install as a NuGet package for all of my solutions. Currently, this package supports `net35`, `net40`, `net451` (for WPF), `net452` and `net461` (for WPF). This package also supports .NET Standard 1.2, 1.4 and 2.0.

There are actually two _Core_ projects, `Songhay` (a .NET 2.0 project) and `SonghayCore` (a .NET latest-ish project). Two projects exist largely for historical reasons (yes, in the 21<sup>st</sup> century there are companies that still use .NET 2.0).

**NuGet package:** [`SonghayCore`](https://www.nuget.org/packages/SonghayCore/)

**NuGet package:** [`Songhay.Portable.Core`](https://www.nuget.org/packages/Songhay.Portable.Core/)

## _Core_ Reusable, Opinionated Concerns

### `SonghayCore.MSTest`

[Extension methods](https://github.com/BryanWilhite/SonghayCore/blob/master/SonghayCore.MSTest/Extensions/TestContextExtensions.cs) of `Microsoft.VisualStudio.TestTools.UnitTesting.TestContext` define reusable routines for MSTEST/VSTEST projects, based on [the open source framework](https://github.com/Microsoft/vstest) from Microsoft.

**NuGet package:** [`SonghayCore.MSTest`](http://www.nuget.org/packages/SonghayCore.MSTest/)

### `Songhay.Diagnostics`

[`TraceSources`](https://github.com/BryanWilhite/SonghayCore/blob/master/SonghayCore/Diagnostics/TraceSources.cs) defines how tracing should be implemented with a bias toward using “native” .NET tracing.

**NuGet package:** [`SonghayCore`](https://www.nuget.org/packages/SonghayCore/)

### `Songhay.Extensions`

The Songhay System uses imperative C# code with a view to make it more functional in an effort to control complexity and enhance maintainability.

The preference for [extension methods](https://github.com/BryanWilhite/SonghayCore/tree/master/SonghayCore/Extensions) encourages stateless, reusable routines (many of them are “pure” functions).

**NuGet package:** [`SonghayCore`](https://www.nuget.org/packages/SonghayCore/)

### `Songhay.Models`

The _Core_ models of the Songhay System define types for MIME, XHTML, OPML, REST, the Repository, the Display Item (for WPF and other MVVM solutions), etc.

The _Core_ models are “anemic” by design (there are very few abstract classes)—any logic would be found _first_ in an Extension Method.

**NuGet package:** [`SonghayCore`](https://www.nuget.org/packages/SonghayCore/)

### `Songhay.Net.HttpWebRequest`

Extensions of `System.Net.HttpWebRequest` for legacy (`net35`) applications.

**NuGet package:** [`System.Net.HttpWebRequest`](https://www.nuget.org/packages/Songhay.Net.HttpWebRequest/)

### `Songhay.Security`

[Songhay System security](https://github.com/BryanWilhite/SonghayCore/tree/master/Songhay.Security) is to date concerned with string encryption.

**NuGet package:** [`Songhay.Security`](https://www.nuget.org/packages/Songhay.Security/)

### `Songhay.Xml`

The “core” of the _Core_ is concern for XML. The Songhay System started out as utilities around [`XPathDocument`](https://msdn.microsoft.com/en-us/library/system.xml.xpath.xpathdocument(v=vs.110).aspx) and grew into LINQ for XML—over [`XDocument`](https://msdn.microsoft.com/en-us/library/system.xml.linq.xdocument(v=vs.110).aspx).

**NuGet package:** [`SonghayCore`](https://www.nuget.org/packages/SonghayCore/)
