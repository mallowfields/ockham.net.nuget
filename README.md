# Ockham.NuGet
An easy-to-use .NET wrapper around the official [NuGet.Client](https://github.com/NuGet/NuGet.Client) packages. Part of the [Ockham.Net project](https://github.com/mallowfields/ockham.net).

## The Problem

> Every Ockham module should solve a clear problem that is not solved in the .Net BCL, or in the particular libraries it is meant to augment.

The problem this library solves for is making it really easy to program or script against the NuGet API.

**Example: Get the full metadata info for a single package ID**

Straight from [one of the unit tests](https://github.com/joshua-honig/ockham.net.nuget/blob/master/src/test/PackageLookupTests.cs):

```C# 
// no usings required
var repo = new Ockham.NuGet.Repository("https://api.nuget.org/v3/index.json");
var packageInfo = repo.GetPackage("jQuery", "3.1.1"); 
```

That `packageInfo` object is a `PackageSearchMetadata` object defined by and returned from the underlying libraries currently included by the [NuGet.Protocol](https://www.nuget.org/packages/NuGet.Protocol/) package. Ockham.NuGet just makes it a lot easier to get to this package info.

For comparison, here's the same thing, directly using the NuGet.Client APIs:

```C#
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.Threading;

var providers = new List<Lazy<INuGetResourceProvider>>();
providers.AddRange(Repository.Provider.GetCoreV3());
var packageSource = new PackageSource("https://api.nuget.org/v3/index.json");
var sourceRepository = new SourceRepository(packageSource, providers);
var metadataResource = sourceRepository.GetResource<PackageMetadataResource>();
var nugetVersion = NuGetVersion.Parse("3.1.1");
var packageIdentity = new PackageIdentity("jQuery", nugetVersion);
var logger = new NullLogger();
var packageInfo = metadataResource.GetMetadataAsync(packageIdentity, logger, CancellationToken.None).Result;
```

Or yet again, but minimizing the `using`s

```C#
// REQUIRED, because GetCoreV3() method is an extension method in the NuGet.Protocol namespace
using NuGet.Protocol; 

var providers = new System.Collections.Generic.List<System.Lazy<NuGet.Protocol.Core.Types.INuGetResourceProvider>>();
providers.AddRange(NuGet.Protocol.Core.Types.Repository.Provider.GetCoreV3());
var packageSource = new NuGet.Configuration.PackageSource("https://api.nuget.org/v3/index.json");
var sourceRepository = new NuGet.Protocol.Core.Types.SourceRepository(packageSource, providers);
var metadataResource = sourceRepository.GetResource<NuGet.Protocol.Core.Types.PackageMetadataResource>();
var nugetVersion = NuGet.Versioning.NuGetVersion.Parse("3.1.1");
var packageIdentity = new NuGet.Packaging.Core.PackageIdentity("jQuery", nugetVersion);
var logger = new NuGet.Common.NullLogger();
var packageInfo = metadataResource.GetMetadataAsync(packageIdentity, logger, System.Threading.CancellationToken.None).Result;
```

### The Problem, in detail
NuGet is great, but it is not easy to interact with programmatically. You can easily push, pack and install with the [nuget cli](https://docs.microsoft.com/en-us/nuget/tools/nuget-exe-cli-reference), but you cannot easily read package metadata details back into a script with it. The [Package Manager Console](https://docs.microsoft.com/en-us/nuget/tools/package-manager-console) cmdlets do let you do this, but they are only available from the special Package Manager Console host inside Visual Studio. Likewise the [Package Manager UI](https://docs.microsoft.com/en-us/nuget/tools/package-manager-ui) in Visual Studio is very powerful, but it's a GUI inside Visual Studio, not a simple object model accessible from .NET code or scripts.

All of the official clients do use the libraries maintained by the [NuGet.Client](https://github.com/NuGet/NuGet.Client) project, but those APIs are complex and completely undocumented. The ["official" documentation on docs.microsoft.com](https://docs.microsoft.com/en-us/nuget/api/nuget-api-v3) is literally a page of links to [three](https://daveaglick.com/posts/exploring-the-nuget-v3-libraries-part-1) [blog](https://daveaglick.com/posts/exploring-the-nuget-v3-libraries-part-2) [posts](https://daveaglick.com/posts/exploring-the-nuget-v3-libraries-part-3) by a private citizen ([Dave Glick](https://daveaglick.com/)), in which he laments the complete lack of documentation. The code examples in his blog don't actually work with the latest releases of the NuGet.Client packages because APIs have moved to different namespaces and different NuGet packages.

This library aims to make the power of NuGet very easy to program or script against by wrapping the official NuGet.Client packages with a thin API wrapper that will remain stable and really easy to use. Most of the work in building this library is reverse engineering the NuGet.Client libraries to figure out all the internal steps required to do seemingly simple things, like get the metadata for a known package ID in an known repository (see example above).

