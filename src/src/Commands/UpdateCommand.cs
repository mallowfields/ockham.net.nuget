using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ockham.Nuget.Commands
{
    /// <summary>
    /// Executes the nuget install command
    /// </summary>
    public class UpdateCommand : InstallCommandBase
    {
        /// <inheritdoc />
        public override string Verb => "update";

        /// <inheritdoc />
        protected override List<string> GetArguments()
        {
            var args = base.GetArguments();

            return args;
        }
    }
}
