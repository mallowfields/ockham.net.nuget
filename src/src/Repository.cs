using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NugetRepository = NuGet.Protocol.Core.Types.Repository;

namespace Ockham.NuGet
{
    /// <summary>
    /// A simple wrapper for interacting with a remote NuGet repository
    /// </summary>
    public class Repository
    {
        private readonly string _uri;
        /// <summary>
        /// The 
        /// </summary>
        public string Uri { get { return _uri; } }

        private ILogger _logger = new NullLogger();
        /// <summary>
        /// The <see cref="ILogger"/> instance to be used by the current <see cref="Repository"/>
        /// </summary>
        public ILogger Logger
        {
            get { return _logger; }
            set { _logger = value ?? new NullLogger(); }
        }

        /// <summary>
        /// Connect to a NuGet repository at the specified endpoint URI
        /// </summary>
        /// <param name="sourceUri"></param>
        public Repository(string sourceUri)
        {
            _uri = sourceUri;

            List<Lazy<INuGetResourceProvider>> providers = new List<Lazy<INuGetResourceProvider>>();
            providers.AddRange(NugetRepository.Provider.GetCoreV3());
            PackageSource packageSource = new PackageSource(_uri);
            _sourceRepository = new SourceRepository(packageSource, providers);

            _metadataResource = SourceRepository.GetResource<PackageMetadataResource>();
        }

        private readonly SourceRepository _sourceRepository;
        /// <summary>
        /// The underlying <see cref="global::NuGet.Protocol.Core.Types.SourceRepository"/> for the current <see cref="Repository"/>
        /// </summary>
        public SourceRepository SourceRepository { get { return _sourceRepository; } }

        private PackageMetadataResource _metadataResource;


        /// <summary>
        /// Get the metadata for a specific package ID and version
        /// </summary> 
        public IPackageSearchMetadata GetPackage(string packageID, string version)
        {
            if (string.IsNullOrWhiteSpace(version)) throw new ArgumentNullException("version");
            NuGetVersion nugetVersion = null;
            if (!NuGetVersion.TryParse(version, out nugetVersion))
            {
                throw new ArgumentException("Invalid NuGet version string '" + version + "'", "version");
            }

            PackageIdentity packageIdentity = new PackageIdentity(packageID, nugetVersion);
            return GetPackage(packageIdentity);
        }

        /// <summary>
        /// Get the metadata for a specific PackageIdentity
        /// </summary> 
        public IPackageSearchMetadata GetPackage(PackageIdentity packageIdentity)
        {
            return _metadataResource.GetMetadataAsync(packageIdentity, _logger, CancellationToken.None).Result;
        }

        /// <summary>
        /// Get the metadata for all versions of a package
        /// </summary> 
        public IEnumerable<IPackageSearchMetadata> GetPackages(string packageID, bool includePrerelease = false, bool includeUnlisted = false)
        {
            return GetPackagesAsync(packageID, includePrerelease, includeUnlisted).Result;
        }

        /// <summary>
        /// Get the metadata for a range of package versions
        /// </summary> 
        public IEnumerable<IPackageSearchMetadata> GetPackages(string packageID, string versionRange, bool includePrerelease = false, bool includeUnlisted = false)
        {
            return GetPackagesAsync(packageID, includePrerelease, includeUnlisted).Result.FilterVersions(versionRange);
        }

        /// <summary>
        /// Get the metadata for a range of package versions
        /// </summary> 
        public IEnumerable<IPackageSearchMetadata> GetPackages(string packageID, VersionRange versionRange, bool includePrerelease = false, bool includeUnlisted = false)
        {
            return GetPackagesAsync(packageID, includePrerelease, includeUnlisted).Result.FilterVersions(versionRange);
        }

        /// <summary>
        /// Get the metadata for all versions of a package
        /// </summary> 
        public async Task<IEnumerable<IPackageSearchMetadata>> GetPackagesAsync(string packageID, bool includePrerelease = false, bool includeUnlisted = false)
        {
            return await _metadataResource.GetMetadataAsync(packageID, includePrerelease, includeUnlisted, _logger, CancellationToken.None);
        }
    }
}
