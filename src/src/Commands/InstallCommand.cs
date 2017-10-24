using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ockham.Nuget.Commands
{
    /// <summary>
    /// Executes the nuget install command
    /// </summary>
    public class InstallCommand : InstallCommandBase
    {
        #region FallbackSource
        private readonly List<string> _fallbackSources = new List<string>();

        /// <summary>
        /// A list of package sources to use as fallbacks in case the package isn't found in the primary or default source.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)]
        public string FallbackSource
        {
            get { return string.Join(" ", _fallbackSources.Select(s => "\"" + s + "\"")); }
        }

        /// <summary>
        /// A list of package sources to use as fallbacks in case the package isn't found in the primary or default source.
        /// </summary>
        public List<string> FallbackSources { get { return _fallbackSources; } }
        #endregion

        /// <summary>
        /// Disables installing multiple packages in parallel.
        /// </summary>
        public bool DisableParallelProcessing { get; set; } = false;

        /// <summary>
        /// Installs the package to a folder named with only the package name and not the version number.
        /// </summary>
        public bool ExcludeVersion { get; set; } = false;
         
        /// <summary>
        /// Prevents NuGet from using packages from local machine caches.
        /// </summary>
        public bool NoCache { get; set; } = false;
         
        /// <summary>
        /// Specifies the folder in which packages are installed. If no folder is specified, the current folder is used.
        /// </summary>
        public string OutputDirectory { get; set; }

        /// <inheritdoc />
        public override string Verb => "install";

        /// <inheritdoc />
        protected override List<string> GetArguments()
        {
            var args = base.GetArguments();

            if (DisableParallelProcessing) args.Add("-DisableParallelProcessing");
            if (ExcludeVersion) args.Add("-ExcludeVersion");
            if (NoCache) args.Add("-NoCache");
             
            if (FallbackSources.Any())
            {
                args.Add("-FallbackSource");
                args.Add(FallbackSource);
            }

            if (!string.IsNullOrWhiteSpace(OutputDirectory))
            {
                args.Add("-OutputDirectory");
                args.Add("\"" + OutputDirectory + "\"");
            }



            return args;
        }
    }
}
