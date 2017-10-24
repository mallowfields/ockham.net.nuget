using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ockham.Nuget.Commands
{
    /// <summary>
    /// Base class for <see cref="InstallCommand"/> and <see cref="UpdateCommand"/>
    /// </summary>
    public abstract class InstallCommandBase : NugetCommand
    {
        #region Source
        private readonly List<string> _sources = new List<string>();

        /// <summary>
        /// Specifies a list of package sources to use.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)]
        public string Source
        {
            get { return string.Join(" ", _sources.Select(s => "\"" + s + "\"")); }
        }

        /// <summary>
        /// Specifies a list of package sources to use.
        /// </summary>
        public List<string> Sources { get { return _sources; } }
        #endregion

        /// <summary>
        /// Forces nuget.exe to run using an invariant, English-based culture.
        /// </summary>
        public bool ForceEnglishOutput { get; set; } = false;
         
        /// <summary>
        /// Suppresses prompts for user input or confirmations.
        /// </summary>
        public bool NonInteractive { get { return true; } }

        /// <inheritdoc />
        protected override List<string> GetArguments()
        {
            var args = new List<string>();
            if (ForceEnglishOutput) args.Add("-ForceEnglishOutput");
            if (NonInteractive) args.Add("-NonInteractive");

            if (Sources.Any())
            {
                args.Add("-Source");
                args.Add(Source);
            } 

            return args;
        }
    }
}
