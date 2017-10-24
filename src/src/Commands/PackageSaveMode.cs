using System;
using System.Collections.Generic;
using System.Text;

namespace Ockham.Nuget.Commands
{
    /// <summary>
    /// Valid values for PackageSaveMode of install, update command
    /// </summary>
    [Flags]
    public enum PackageSaveMode
    {
        /// <summary>
        /// -PackageSaveMode nusepc
        /// </summary>
        nuspec = 0x1,

        /// <summary>
        /// -PackageSaveMode nupkg
        /// </summary>
        nupkg = 0x2,

        /// <summary>
        /// -PackageSaveMode nuspec;nupkg
        /// </summary>
        nuspec_and_nupkg = 0x3
    }


}
