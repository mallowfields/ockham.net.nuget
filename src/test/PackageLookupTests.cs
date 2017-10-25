using NuGet.Protocol;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Ockham.NuGet.Tests
{
    public class PackageLookupTests
    {
        [Fact(DisplayName = "Get jQuery")]
        public void GetJQuery_AllVersions()
        {
            var repo = new Repository("https://api.nuget.org/v3/index.json");
            var packages = repo.GetPackages("jQuery");

            // Let's expect at least 5 versions of jQuery on nuget.org
            Assert.True(packages.Skip(4).Any());
        }

        [Fact(DisplayName = "Get specific jQuery version")]
        public void GetJQuery_SpecificVersion()
        {
            var repo = new Repository("https://api.nuget.org/v3/index.json");
            var packageInfo = repo.GetPackage("jQuery", "3.1.1");
            Assert.NotNull(packageInfo);

            var v3packageInfo = packageInfo as PackageSearchMetadata;
            Assert.NotNull(v3packageInfo);
            Assert.Equal(NuGetVersion.Parse("3.1.1"), v3packageInfo.Version);
        }

        [Fact(DisplayName = "Filter jQuery versions (VersionRange)")]
        public void GetJQuery_FilterVersions()
        {
            var repo = new Repository("https://api.nuget.org/v3/index.json");
            var packages = repo.GetPackages("jQuery");

            var range = VersionRange.Parse("[1.11,3.1)");
            var filtered = packages.FilterVersions(range).ToList();
            Assert.True(filtered.Any());
        }

        [Fact(DisplayName = "Filter jQuery versions (string min, max)")]
        public void GetJQuery_FilterVersions2()
        {
            var repo = new Repository("https://api.nuget.org/v3/index.json");
            var packages = repo.GetPackages("jQuery");
             
            var filtered = packages.FilterVersions("1.11", "3.1", maxInclusive: false).ToList();
            Assert.True(filtered.Any());
        }

        [Fact(DisplayName = "Filter jQuery versions (range string)")]
        public void GetJQuery_FilterVersions3()
        {
            var repo = new Repository("https://api.nuget.org/v3/index.json");
            var packages = repo.GetPackages("jQuery");
             
            var filtered = packages.FilterVersions("[1.11,3.1)").ToList();
            Assert.True(filtered.Any());
        }

        [Fact(DisplayName = "Find jQuery versions (VersionRange)")]
        public void GetJQuery_GetVersions_Range()
        {
            var repo = new Repository("https://api.nuget.org/v3/index.json");

            var range = VersionRange.Parse("[1.11,3.1)");
            var packages = repo.GetPackages("jQuery", range);
            Assert.True(packages.Any());
        }

        [Fact(DisplayName = "Find jQuery versions (range string)")]
        public void GetJQuery_GetVersions_Range2()
        {
            var repo = new Repository("https://api.nuget.org/v3/index.json"); 
            var packages = repo.GetPackages("jQuery", "[1.11,3.1)");
            Assert.True(packages.Any());
        }
    }
}
