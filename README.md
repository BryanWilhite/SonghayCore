# SonghayCore
Songhay System Core

The _Core_ code to install as a NuGet package for all of my solutions. Currently, this package supports `net35`, `net40` and `net45`.

There are actually two _Core_ projects, `Songhay` (a .NET 2.0 project) and `SonghayCore` (a .NET latest-ish project). Two projects exist largely for historical reasons.

## _Core_ Reusable, Opinionated Concerns

### `Songhay.Diagnostics`
[`TraceSources`](https://github.com/BryanWilhite/SonghayCore/blob/master/SonghayCore/Diagnostics/TraceSources.cs) defines how tracing should be implemented with a bias toward using ‘native’ .NET tracing.

### `Songhay.Extensions`
The Songhay System uses imperative C# code with a view to make it more functional in an effort to control complexity and enhance maintainability.

The preference for [extension methods](https://github.com/BryanWilhite/SonghayCore/tree/master/SonghayCore/Extensions) encourages stateless, reusable routines (many of them are “pure” functions).

### `Songhay.Models`
The _Core_ models of the Songhay System define types for MIME, XHTML, OPML, REST, the Repository, the Display Item (for WPF and other MVVM solutions), etc.

The _Core_ models are “anemic” by design (there are very few abstract classes)—any logic would be found _first_ in an Extension Method.

### `Songhay.Security`
[Songhay System security](https://github.com/BryanWilhite/SonghayCore/tree/master/Songhay.Security) is to date concerned with string encryption.

### `Songhay.Xml`
The ‘core’ of the _Core_ is concern for XML. The Songhay System started out as utilities around [`XPathDocument`](https://msdn.microsoft.com/en-us/library/system.xml.xpath.xpathdocument(v=vs.110).aspx) and grew into LINQ for XML—over [`XDocument`](https://msdn.microsoft.com/en-us/library/system.xml.linq.xdocument(v=vs.110).aspx).