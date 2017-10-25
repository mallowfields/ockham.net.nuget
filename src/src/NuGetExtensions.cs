using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using System.Collections.Generic;
using System.Linq;

namespace Ockham.NuGet
{
    public static class NuGetExtensions
    {
        public static IEnumerable<IPackageSearchMetadata> FilterVersions(this IEnumerable<IPackageSearchMetadata> source, VersionRange range)
        {
            return source.Where(x => range.Satisfies(x.Identity.Version));
        }

        public static IEnumerable<IPackageSearchMetadata> FilterVersions(this IEnumerable<IPackageSearchMetadata> source, string range)
        {
            return FilterVersions(source, VersionRange.Parse(range));
        }

        public static IEnumerable<IPackageSearchMetadata> FilterVersions(this IEnumerable<IPackageSearchMetadata> source, NuGetVersion minVersion, NuGetVersion maxVersion, bool minInclusive = true, bool maxInclusive = true)
        {
            return FilterVersions(source, new VersionRange(minVersion, minInclusive, maxVersion, maxInclusive));
        }

        public static IEnumerable<IPackageSearchMetadata> FilterVersions(this IEnumerable<IPackageSearchMetadata> source, string minVersion, string maxVersion, bool minInclusive = true, bool maxInclusive = true)
        {
            return FilterVersions(source, new VersionRange(NuGetVersion.Parse(minVersion), minInclusive, NuGetVersion.Parse(maxVersion), maxInclusive));
        }
    }
}
